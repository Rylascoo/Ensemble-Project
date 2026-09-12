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
    def __init__(
        self, repo: Path, *, mutate: bool = False, bad_auth: bool = False,
        timed_out: bool = False, invalid_payload: bool = False, num_turns: int = 1,
        bad_result_provider: bool = False,
    ):
        self.repo = repo
        self.mutate = mutate
        self.bad_auth = bad_auth
        self.timed_out = timed_out
        self.invalid_payload = invalid_payload
        self.num_turns = num_turns
        self.bad_result_provider = bad_result_provider
        self.review_calls = 0

    def __call__(self, args, *, cwd, env, input_text=None):
        if args[1:] == ["auth", "status", "--json"]:
            auth = {
                "loggedIn": True,
                "authMethod": "claude.ai",
                "apiProvider": "apiKey" if self.bad_auth else "firstParty",
                "subscriptionType": "pro",
            }
            return mod.CommandResult(0, json.dumps(auth), "")
        if args[1:] == ["--version"]:
            return mod.CommandResult(0, "fake-claude 1.0\n", "")
        self.review_calls += 1
        expected = mod.review_command(Path(args[0]))
        if list(args) != expected:
            return mod.CommandResult(9, "", "unexpected review command")
        assert "--json-schema" not in args
        assert args[args.index("--max-turns") + 1] == "1"
        if "ANTHROPIC_API_KEY" in env or "CLAUDE_CODE_USE_BEDROCK" in env:
            return mod.CommandResult(10, "", "sensitive route leaked")
        if not input_text or "EXACT_COMMIT:" not in input_text or "review.txt" not in input_text:
            return mod.CommandResult(11, "", "packet missing exact provenance")
        if "Return exactly one JSON object" not in input_text:
            return mod.CommandResult(12, "", "local-JSON contract missing")
        if self.mutate:
            (self.repo / "UNAUTHORIZED_MUTATION.txt").write_text("mutation\n", encoding="utf-8")
        if self.timed_out:
            return mod.CommandResult(
                124, "partial-output", "timeout-stderr", True, mod.PROCESS_TIMEOUT_SECONDS
            )
        payload = {"verdict": "PASS", "summary": "bounded fake review", "findings": []}
        if self.invalid_payload:
            payload = {"verdict": "PASS", "summary": "missing findings"}
        envelope = {
            "provider": "apiKey" if self.bad_result_provider else "firstParty",
            "num_turns": self.num_turns,
            "permission_denials": [],
            "is_error": False,
            "result": json.dumps(payload),
        }
        return mod.CommandResult(0, json.dumps(envelope), "")

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
        assert first["claude"]["native_structured_output_requested"] is False
        assert "--json-schema" not in first["claude"]["command"]
        assert first["execution"]["max_turns_requested"] == 1
        assert first["execution"]["local_schema_validation"] is True

        bad_auth = FakeRunner(repo, bad_auth=True)
        expect_failure(
            lambda: mod.dispatch(request(repo, scratch, config), runner=bad_auth),
            "non-first-party auth",
        )
        assert bad_auth.review_calls == 0

        bad_provider = FakeRunner(repo, bad_result_provider=True)
        expect_failure(
            lambda: mod.dispatch(request(repo, scratch, config), runner=bad_provider),
            "non-first-party result provider",
        )
        assert bad_provider.review_calls == 1
        provider_telemetry = max(
            scratch.glob("c9a-*/telemetry.json"), key=lambda x: x.stat().st_mtime_ns
        )
        provider_data = json.loads(provider_telemetry.read_text(encoding="utf-8"))
        assert provider_data["pass"] is False
        assert "unexpected Claude result provider" in provider_data["failure_reason"]

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

        invalid = FakeRunner(repo, invalid_payload=True)
        expect_failure(
            lambda: mod.dispatch(request(repo, scratch, config), runner=invalid),
            "locally invalid review payload",
        )
        assert invalid.review_calls == 1
        invalid_telemetry = max(
            scratch.glob("c9a-*/telemetry.json"), key=lambda x: x.stat().st_mtime_ns
        )
        invalid_data = json.loads(invalid_telemetry.read_text(encoding="utf-8"))
        assert invalid_data["pass"] is False
        assert "local review schema validation failed" in invalid_data["failure_reason"]
        assert invalid_data["repository_snapshot"]["unchanged"] is True

        timed_out = FakeRunner(repo, timed_out=True)
        expect_failure(
            lambda: mod.dispatch(request(repo, scratch, config), runner=timed_out),
            "bounded timeout",
        )
        assert timed_out.review_calls == 1
        timeout_telemetry = max(
            scratch.glob("c9a-*/telemetry.json"), key=lambda x: x.stat().st_mtime_ns
        )
        timeout_data = json.loads(timeout_telemetry.read_text(encoding="utf-8"))
        assert timeout_data["pass"] is False
        assert timeout_data["execution"]["timed_out"] is True
        assert timeout_data["execution"]["exit_code"] == 124
        assert timeout_data["repository_snapshot"]["unchanged"] is True
        assert timeout_data["result"]["bytes"] > 0
        assert timeout_data["stderr"]["bytes"] > 0

        timeout_mutation = FakeRunner(repo, timed_out=True, mutate=True)
        expect_failure(
            lambda: mod.dispatch(request(repo, scratch, config), runner=timeout_mutation),
            "timeout plus repository mutation",
        )
        mutation_telemetry = max(
            scratch.glob("c9a-*/telemetry.json"), key=lambda x: x.stat().st_mtime_ns
        )
        mutation_data = json.loads(mutation_telemetry.read_text(encoding="utf-8"))
        assert mutation_data["repository_snapshot"]["unchanged"] is False
        assert "repository snapshot changed" in mutation_data["failure_reason"]
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
    print("LOCAL_SCHEMA_VALIDATION=PASS")
    print("ONE_TURN_COMMAND_BOUND=PASS")
    print("TIMEOUT_TELEMETRY=PASS")
    print("POST_TIMEOUT_MUTATION_DETECTION=PASS")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
