#!/usr/bin/env python3
"""Synthetic C1 fixtures for tools/codex-admin-reconcile.py."""

from __future__ import annotations

import importlib.util
import json
import subprocess
import sys
import tempfile
from dataclasses import dataclass
from pathlib import Path
from typing import Callable

TOOL = Path(__file__).with_name("codex-admin-reconcile.py")
READ_ONLY = {"remote", "rev-parse", "status", "symbolic-ref", "worktree", "rev-list", "for-each-ref", "merge-base"}


def observed_read_only_command(args: list[str]) -> bool:
    if not args or args[0] not in READ_ONLY:
        return False
    if args[0] == "remote":
        return args == ["remote", "get-url", "origin"]
    if args[0] == "symbolic-ref":
        return args == ["symbolic-ref", "--short", "-q", "HEAD"]
    if args[0] == "worktree":
        return len(args) >= 2 and args[1] == "list"
    return args[0] in {"rev-parse", "status", "rev-list", "for-each-ref", "merge-base"}
REQUIRED = {
    "CURRENT_CLEAN",
    "BEHIND_FAST_FORWARD_CANDIDATE",
    "AHEAD",
    "DIVERGED",
    "DETACHED",
    "DIRTY_OR_UNTRACKED",
    "UNEXPLAINED_WORKTREE",
    "REMOTE_IDENTITY_MISMATCH",
}


def git(cwd: Path, *args: str) -> str:
    completed = subprocess.run(
        ["git", "-C", str(cwd), *args],
        text=True,
        encoding="utf-8",
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        check=False,
    )
    if completed.returncode != 0:
        detail = completed.stderr.strip() or completed.stdout.strip()
        raise RuntimeError(f"git {' '.join(args)} failed: {detail}")
    return completed.stdout.strip()


def configure_identity(repo: Path) -> None:
    git(repo, "config", "user.name", "C1 Fixture")
    git(repo, "config", "user.email", "c1-fixture@example.invalid")


def commit_file(repo: Path, name: str, text: str) -> None:
    (repo / name).write_text(text, encoding="utf-8")
    git(repo, "add", name)
    git(repo, "commit", "-m", f"fixture {name}")


@dataclass
class Fixture:
    root: Path
    remote: Path
    primary: Path
    peer: Path


def make_fixture(root: Path) -> Fixture:
    remote = root / "remote.git"
    primary = root / "primary"
    peer = root / "peer"
    git(root, "init", "--bare", "--initial-branch=main", str(remote))
    git(root, "clone", str(remote), str(primary))
    configure_identity(primary)
    commit_file(primary, "base.txt", "base\n")
    git(primary, "push", "-u", "origin", "main")
    git(root, "clone", str(remote), str(peer))
    configure_identity(peer)
    return Fixture(root=root, remote=remote, primary=primary, peer=peer)


def snapshot(repo: Path) -> dict[str, str]:
    branch = subprocess.run(
        ["git", "-C", str(repo), "symbolic-ref", "--short", "-q", "HEAD"],
        text=True,
        encoding="utf-8",
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        check=False,
    ).stdout.strip()
    return {
        "head": git(repo, "rev-parse", "HEAD"),
        "branch": branch,
        "status": git(repo, "status", "--porcelain=v2", "--branch"),
        "worktrees": git(repo, "worktree", "list", "--porcelain"),
        "heads": git(repo, "for-each-ref", "--format=%(refname)|%(objectname)", "refs/heads", "refs/remotes"),
    }


def classify(fixture: Fixture, expected_origin: str | None = None) -> dict[str, object]:
    command = [
        sys.executable,
        str(TOOL),
        "--repo",
        str(fixture.primary),
        "--expected-origin",
        expected_origin or str(fixture.remote),
    ]
    completed = subprocess.run(
        command,
        text=True,
        encoding="utf-8",
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        check=False,
    )
    if completed.returncode != 0:
        raise RuntimeError(f"classifier failed: {completed.stdout.strip()} {completed.stderr.strip()}")
    result = json.loads(completed.stdout)
    if result.get("would_modify_repository") is not False:
        raise AssertionError("classifier did not report read-only posture")
    for command_args in result.get("git_commands", []):
        if not observed_read_only_command(command_args):
            raise AssertionError(f"non-read-only command observed: {command_args!r}")
    return result


def run_case(
    name: str,
    expected: str,
    mutate: Callable[[Fixture], None] | None = None,
    origin_override: Callable[[Fixture], str] | None = None,
) -> str:
    with tempfile.TemporaryDirectory(prefix=f"ensemble-c1-{name.lower()}-") as temp:
        fixture = make_fixture(Path(temp))
        if mutate:
            mutate(fixture)
        before = snapshot(fixture.primary)
        result = classify(fixture, origin_override(fixture) if origin_override else None)
        after = snapshot(fixture.primary)
        actual = str(result["classification"])
        if actual != expected:
            raise AssertionError(f"{name}: expected {expected}, got {actual}; reason={result.get('reason')}")
        if before != after:
            raise AssertionError(f"{name}: classifier changed repository state")
        print(f"CASE={name}|EXPECTED={expected}|ACTUAL={actual}|PASS")
        return actual


def make_behind(fixture: Fixture) -> None:
    commit_file(fixture.peer, "remote.txt", "remote\n")
    git(fixture.peer, "push", "origin", "main")
    git(fixture.primary, "fetch", "origin")


def make_ahead(fixture: Fixture) -> None:
    commit_file(fixture.primary, "local.txt", "local\n")


def make_diverged(fixture: Fixture) -> None:
    commit_file(fixture.primary, "local.txt", "local\n")
    commit_file(fixture.peer, "remote.txt", "remote\n")
    git(fixture.peer, "push", "origin", "main")
    git(fixture.primary, "fetch", "origin")


def make_detached(fixture: Fixture) -> None:
    git(fixture.primary, "checkout", "--detach", "HEAD")


def make_dirty(fixture: Fixture) -> None:
    (fixture.primary / "untracked.txt").write_text("dirty\n", encoding="utf-8")


def make_unexplained_worktree(fixture: Fixture) -> None:
    extra = fixture.root / "extra-worktree"
    git(fixture.primary, "worktree", "add", "-b", "fixture-extra", str(extra), "HEAD")


def make_missing_upstream(fixture: Fixture) -> None:
    git(fixture.primary, "checkout", "-b", "stale-root")
    git(fixture.primary, "push", "-u", "origin", "stale-root")
    git(fixture.primary, "push", "origin", "--delete", "stale-root")
    git(fixture.primary, "fetch", "--prune", "origin")


def run_remote_ref_census_case() -> None:
    with tempfile.TemporaryDirectory(prefix="ensemble-c1-ref-census-") as temp:
        fixture = make_fixture(Path(temp))
        base = git(fixture.primary, "rev-parse", "HEAD")
        git(fixture.primary, "branch", "historical", base)
        git(fixture.primary, "push", "origin", "historical")

        commit_file(fixture.peer, "main2.txt", "main2\n")
        git(fixture.peer, "push", "origin", "main")
        git(fixture.peer, "checkout", "-b", "ahead", "main")
        commit_file(fixture.peer, "ahead.txt", "ahead\n")
        git(fixture.peer, "push", "origin", "ahead")
        git(fixture.peer, "checkout", "main")
        git(fixture.peer, "checkout", "-b", "diverged", base)
        commit_file(fixture.peer, "diverged.txt", "diverged\n")
        git(fixture.peer, "push", "origin", "diverged")
        git(fixture.peer, "checkout", "main")
        git(fixture.primary, "fetch", "--prune", "origin")

        before = snapshot(fixture.primary)
        result = classify(fixture)
        after = snapshot(fixture.primary)
        expected = {"MAIN": 1, "ANCESTRAL_TO_MAIN": 1, "AHEAD_OF_MAIN": 1, "DIVERGED_SIDE_HISTORY": 1}
        actual = result["remote_ref_census"]["counts"]
        if actual != expected:
            raise AssertionError(f"remote ref census mismatch: expected={expected} actual={actual}")
        if before != after:
            raise AssertionError("remote ref census changed repository state")
        print("C1_REMOTE_REF_TOPOLOGY=PASS")

def run_guard_rejection_case() -> None:
    spec = importlib.util.spec_from_file_location("codex_admin_reconcile", TOOL)
    if spec is None or spec.loader is None:
        raise AssertionError("could not load reconciler for guard test")
    module = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(module)
    forbidden = [
        ("worktree", "add", "forbidden"),
        ("remote", "set-url", "origin", "forbidden"),
        ("symbolic-ref", "HEAD", "refs/heads/forbidden"),
        ("checkout", "main"),
        ("reset", "--hard", "HEAD"),
    ]
    for args in forbidden:
        if module.is_read_only_git_command(args):
            raise AssertionError(f"guard admitted mutating Git command: {args!r}")
    print("C1_MUTATING_GIT_GUARD=PASS")


def main() -> int:
    seen = {
        run_case("CURRENT_CLEAN", "CURRENT_CLEAN"),
        run_case("BEHIND", "BEHIND_FAST_FORWARD_CANDIDATE", make_behind),
        run_case("AHEAD", "AHEAD", make_ahead),
        run_case("DIVERGED", "DIVERGED", make_diverged),
        run_case("DETACHED", "DETACHED", make_detached),
        run_case("DIRTY", "DIRTY_OR_UNTRACKED", make_dirty),
        run_case("UNEXPLAINED", "UNEXPLAINED_WORKTREE", make_unexplained_worktree),
        run_case(
            "REMOTE_MISMATCH",
            "REMOTE_IDENTITY_MISMATCH",
            origin_override=lambda fixture: str(fixture.remote) + "-wrong",
        ),
    }
    run_case("MISSING_UPSTREAM", "UNEXPLAINED_WORKTREE", make_missing_upstream)
    run_remote_ref_census_case()
    run_guard_rejection_case()
    if seen != REQUIRED:
        raise AssertionError(f"required classifications mismatch: seen={sorted(seen)} required={sorted(REQUIRED)}")
    print("C1_SYNTHETIC_CLASSIFICATIONS=PASS")
    print("C1_CLASSIFIER_MUTATION_CHECK=PASS")
    print("C1_READ_ONLY_GIT_ALLOWLIST=PASS")
    return 0


if __name__ == "__main__":
    sys.exit(main())
