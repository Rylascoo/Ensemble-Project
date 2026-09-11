#!/usr/bin/env python3
"""Offline falsification tests for the C9A Claude review dispatcher."""
from __future__ import annotations

import importlib.util
import json
import os
import subprocess
import sys
import tempfile
from pathlib import Path

TOOL = Path(__file__).with_name("codex-admin-claude-review.py")
spec = importlib.util.spec_from_file_location("c9a_dispatch", TOOL)
if spec is None or spec.loader is None:
    raise RuntimeError("unable to import dispatcher")
mod = importlib.util.module_from_spec(spec)
sys.modules[spec.name] = mod
spec.loader.exec_module(mod)


def git(repo: Path, *args: str) -> str:
    cp = subprocess.run(
        ["git", "-C", str(repo), *args], text=True, encoding="utf-8",
        stdout=subprocess.PIPE, stderr=subprocess.PIPE, check=False,
    )
    if cp.returncode != 0:
        raise RuntimeError(cp.stderr.strip() or cp.stdout.strip())
    return cp.stdout.strip()


def init_repo(root: Path) -> Path:
    repo = root / "repo"
    repo.mkdir()
    git(repo, "init", "--initial-branch=main")
    git(repo, "config", "user.name", "C9A Fixture")
    git(repo, "config", "user.email", "c9a@example.invalid")
    (repo / "review.txt").write_text("bounded review specimen\n", encoding="utf-8")
    git(repo, "add", "review.txt")
    git(repo, "commit", "-m", "fixture")
    return repo


class FakeRunner:
    def __init__(self, repo: Path, *, mutate: bool = False, bad_auth: bool = False):
        self.repo = repo
        self.mutate = mutate
        self.bad_auth = bad_auth
        self.review_calls = 0

    def __call__(self, args, *, cwd, env, input_text=None):
        if args[1:] == ["auth", "status", "--json"]:
            auth = {
                "loggedIn": True,
                "authMethod": "claude.ai",
                "apiProvider": "apiKey" if self.bad_auth else "firstParty",
                "subscriptionType": "pro",
            }
            return subprocess.CompletedProcess(args, 0, json.dumps(auth), "")
        if args[1:] == ["--version"]:
            return subprocess.CompletedProcess(args, 0, "fake-claude 1.0\n", "")
        self.review_calls += 1
        required = [
            "-p", "--safe-mode", "--restricted", "--tools", "", "--strict-mcp-config", "--setting-sources", "user",
            "--permission-mode", "plan", "--permission-prompts", "none",
            "--no-session-persistence", "--output-format", "json", "--effort", "high",
            "--no-chrome", "--disable-slash-commands", "--json-schema",
        ]
        expected = mod.review_command(Path(args[0]))
        if list(args) != expected or any(token not in args for token in required if token):
            return subprocess.CompletedProcess(args, 9, "", "unexpected review command")
        if "ANTHROPIC_API_KEY" in env or "CLAUDE_CODE_USE_BEDROCK" in env:
            return subprocess.CompletedProcess(args, 10, "", "sensitive route leaked")
        if not input_text or "EXACT_COMMIT:" not in input_text or "review.txt" not in input_text:
            return subprocess.CompletedProcess(args, 11, "", "packet missing exact provenance")
        if self.mutate:
            (self.repo / "UNAUTHORIZED_MUTATION.txt").write_text("mutation\n", encoding="utf-8")
        structured = {"verdict": "PASS", "summary": "bounded fake review", "findings": []}
        envelope = {
            "provider": "firstParty",
            "num_turns": 1,
            "permission_denials": [],
            "is_error": False,
            "structured_output": structured,
        }
        return subprocess.CompletedProcess(args, 0, json.dumps(envelope), "")


def request(repo: Path, scratch: Path, config: Path) -> mod.DispatchRequest:
    return mod.DispatchRequest(
        repo=repo,
        ref="HEAD",
        paths=("review.txt",),
        question="Falsify the bounded C9A review path.",
        scratch_root=scratch,
        claude_exe=Path(sys.executable),
        claude_config_dir=config,
    )


def expect_failure(fn, label: str) -> None:
    try:
        fn()
    except mod.DispatchError:
        return
    raise AssertionError(f"expected failure: {label}")


def main() -> int:
    assert 0 < mod.PROCESS_TIMEOUT_SECONDS <= 180
    with tempfile.TemporaryDirectory(prefix="ensemble-c9a-selftest-") as tmp:
        root = Path(tmp)
        repo = init_repo(root)
        config = root / "claude-config"
        config.mkdir()
        scratch = root / "scratch"

        old_key = os.environ.get("ANTHROPIC_API_KEY")
        os.environ["ANTHROPIC_API_KEY"] = "must-be-scrubbed"
        try:
            runner1 = FakeRunner(repo)
            first = mod.dispatch(request(repo, scratch, config), runner=runner1)
            runner2 = FakeRunner(repo)
            second = mod.dispatch(request(repo, scratch, config), runner=runner2)
        finally:
            if old_key is None:
                os.environ.pop("ANTHROPIC_API_KEY", None)
            else:
                os.environ["ANTHROPIC_API_KEY"] = old_key

        assert runner1.review_calls == 1 and runner2.review_calls == 1
        assert first["pass"] is True and second["pass"] is True
        assert first["packet"]["sha256"] == second["packet"]["sha256"]
        assert first["repository_snapshot"]["unchanged"] is True

        bad_auth = FakeRunner(repo, bad_auth=True)
        expect_failure(
            lambda: mod.dispatch(request(repo, scratch, config), runner=bad_auth),
            "non-first-party auth",
        )
        assert bad_auth.review_calls == 0

        mutating = FakeRunner(repo, mutate=True)
        expect_failure(
            lambda: mod.dispatch(request(repo, scratch, config), runner=mutating),
            "repository mutation",
        )
        assert mutating.review_calls == 1
        mutation = repo / "UNAUTHORIZED_MUTATION.txt"
        assert mutation.exists()
        mutation.unlink()
        assert git(repo, "status", "--porcelain") == ""

        inside = repo / "forbidden-scratch"
        expect_failure(
            lambda: mod.dispatch(request(repo, inside, config), runner=FakeRunner(repo)),
            "scratch inside worktree",
        )
        assert not inside.exists()

    print("C9A_CLAUDE_REVIEW_SELFTEST=PASS")
    print("DETERMINISTIC_PACKET=PASS")
    print("AUTH_FAIL_CLOSED=PASS")
    print("REPOSITORY_MUTATION_DETECTION=PASS")
    print("SCRATCH_BOUNDARY=PASS")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
