#!/usr/bin/env python3
"""Read-only repository-state classifier for Administrator commissioning C1."""

from __future__ import annotations

import argparse
import json
import os
import subprocess
import sys
from pathlib import Path
from urllib.parse import urlsplit

READ_ONLY_GIT_SUBCOMMANDS = frozenset({
    "remote",
    "rev-parse",
    "status",
    "symbolic-ref",
    "worktree",
    "rev-list",
    "for-each-ref",
    "merge-base",
})

CLASSIFICATIONS = frozenset({
    "CURRENT_CLEAN",
    "BEHIND_FAST_FORWARD_CANDIDATE",
    "AHEAD",
    "DIVERGED",
    "DETACHED",
    "DIRTY_OR_UNTRACKED",
    "UNEXPLAINED_WORKTREE",
    "REMOTE_IDENTITY_MISMATCH",
})


class ReconcileError(RuntimeError):
    pass


def is_read_only_git_command(args: tuple[str, ...]) -> bool:
    if not args or args[0] not in READ_ONLY_GIT_SUBCOMMANDS:
        return False
    if args[0] == "remote":
        return len(args) == 3 and args[1:] == ("get-url", "origin")
    if args[0] == "symbolic-ref":
        return args[1:] == ("--short", "-q", "HEAD")
    if args[0] == "worktree":
        return len(args) >= 2 and args[1] == "list"
    return args[0] in {"rev-parse", "status", "rev-list", "for-each-ref", "merge-base"}


class GitRunner:
    def __init__(self, repo: Path) -> None:
        self.repo = repo
        self.commands: list[list[str]] = []

    def run(self, *args: str, check: bool = True) -> subprocess.CompletedProcess[str]:
        if not is_read_only_git_command(args):
            raise ReconcileError(f"non-read-only Git command rejected: {args!r}")
        self.commands.append(list(args))
        completed = subprocess.run(
            ["git", "-C", str(self.repo), *args],
            text=True,
            encoding="utf-8",
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            check=False,
        )
        if check and completed.returncode != 0:
            detail = completed.stderr.strip() or completed.stdout.strip()
            raise ReconcileError(f"git {' '.join(args)} failed: {detail}")
        return completed


def normalized_path(value: str | Path) -> str:
    return os.path.normcase(os.path.abspath(os.fspath(value)))


def normalize_origin(value: str) -> str:
    raw = value.strip().rstrip("/")
    lower = raw.lower()
    if lower.startswith("git@github.com:"):
        path = raw.split(":", 1)[1]
        key = f"github.com/{path}"
    elif "://" in raw:
        parsed = urlsplit(raw)
        if parsed.scheme.lower() == "file":
            return normalized_path(parsed.path)
        key = f"{parsed.hostname or ''}{parsed.path}"
    else:
        return normalized_path(raw)
    key = key.rstrip("/")
    if key.lower().endswith(".git"):
        key = key[:-4]
    return key.lower()


def parse_status(text: str) -> tuple[list[str], str | None]:
    dirty: list[str] = []
    upstream: str | None = None
    for line in text.splitlines():
        if line.startswith("# branch.upstream "):
            upstream = line.removeprefix("# branch.upstream ").strip() or None
        elif line and not line.startswith("# "):
            dirty.append(line)
    return dirty, upstream


def parse_worktrees(text: str) -> list[dict[str, object]]:
    records: list[dict[str, object]] = []
    current: dict[str, object] = {}
    for raw in [*text.splitlines(), ""]:
        line = raw.strip()
        if not line:
            if current:
                records.append(current)
                current = {}
            continue
        key, _, value = line.partition(" ")
        if key in {"detached", "bare", "prunable", "locked"} and not value:
            current[key] = True
        else:
            current[key] = value
    return records


def resolve_git_path(repo_root: Path, value: str) -> str:
    path = Path(value)
    if not path.is_absolute():
        path = repo_root / path
    return normalized_path(path)


def relation_from_counts(ahead: int, behind: int) -> tuple[str, str]:
    if ahead == 0 and behind == 0:
        return "CURRENT_CLEAN", "tracked branch equals fetched upstream"
    if ahead == 0 and behind > 0:
        return "BEHIND_FAST_FORWARD_CANDIDATE", f"clean tracked branch is behind upstream by {behind} commit(s)"
    if ahead > 0 and behind == 0:
        return "AHEAD", f"local branch is ahead of upstream by {ahead} commit(s)"
    return "DIVERGED", f"local/upstream diverged: ahead={ahead}, behind={behind}"


def census_remote_refs(runner: GitRunner) -> dict[str, object]:
    main_ref = "refs/remotes/origin/main"
    main_check = runner.run("rev-parse", "--verify", f"{main_ref}^{{commit}}", check=False)
    if main_check.returncode != 0:
        raise ReconcileError("fetched origin/main is unavailable; reconcile only after an approved fetch")
    main_sha = main_check.stdout.strip()
    text = runner.run(
        "for-each-ref",
        "--format=%(refname)|%(objectname)|%(symref)",
        "refs/remotes/origin",
    ).stdout
    records: list[dict[str, str]] = []
    counts = {"MAIN": 0, "ANCESTRAL_TO_MAIN": 0, "AHEAD_OF_MAIN": 0, "DIVERGED_SIDE_HISTORY": 0}
    for line in sorted(item for item in text.splitlines() if item.strip()):
        ref, sha, symref = line.split("|", 2)
        if symref:
            continue
        if ref == main_ref:
            relation = "MAIN"
        else:
            left = runner.run("merge-base", "--is-ancestor", sha, main_sha, check=False)
            if left.returncode == 0:
                relation = "ANCESTRAL_TO_MAIN"
            elif left.returncode != 1:
                raise ReconcileError(f"merge-base failed for {ref} against origin/main")
            else:
                right = runner.run("merge-base", "--is-ancestor", main_sha, sha, check=False)
                if right.returncode == 0:
                    relation = "AHEAD_OF_MAIN"
                elif right.returncode == 1:
                    relation = "DIVERGED_SIDE_HISTORY"
                else:
                    raise ReconcileError(f"merge-base failed for origin/main against {ref}")
        counts[relation] += 1
        records.append({"ref": ref, "sha": sha, "relation": relation})
    return {"main_sha": main_sha, "counts": counts, "refs": records}

def classify_repository(repo: Path, expected_origin: str, known_worktrees: list[str]) -> dict[str, object]:
    runner = GitRunner(repo)
    top = runner.run("rev-parse", "--show-toplevel").stdout.strip()
    repo_root = Path(top)
    git_dir_raw = runner.run("rev-parse", "--absolute-git-dir").stdout.strip()
    common_raw = runner.run("rev-parse", "--git-common-dir").stdout.strip()
    origin = runner.run("remote", "get-url", "origin").stdout.strip()
    head = runner.run("rev-parse", "HEAD").stdout.strip()
    branch_run = runner.run("symbolic-ref", "--short", "-q", "HEAD", check=False)
    branch = branch_run.stdout.strip() or None

    status_text = runner.run("status", "--porcelain=v2", "--branch").stdout
    dirty_entries, upstream_name = parse_status(status_text)
    worktrees = parse_worktrees(runner.run("worktree", "list", "--porcelain").stdout)
    remote_ref_census = census_remote_refs(runner)

    actual_paths = {
        normalized_path(str(record["worktree"]))
        for record in worktrees
        if record.get("worktree")
    }
    expected_paths = {normalized_path(repo_root), *(normalized_path(item) for item in known_worktrees)}
    unexpected = sorted(actual_paths - expected_paths)
    missing_known = sorted(expected_paths - actual_paths)

    origin_matches = normalize_origin(origin) == normalize_origin(expected_origin)
    upstream_resolved = False
    ahead: int | None = None
    behind: int | None = None

    if upstream_name:
        upstream_check = runner.run("rev-parse", "--verify", f"{upstream_name}^{{commit}}", check=False)
        upstream_resolved = upstream_check.returncode == 0
        if upstream_resolved:
            counts = runner.run("rev-list", "--left-right", "--count", f"HEAD...{upstream_name}").stdout.strip().split()
            if len(counts) != 2:
                raise ReconcileError(f"unexpected rev-list count output: {counts!r}")
            ahead, behind = (int(counts[0]), int(counts[1]))

    unexplained_reasons: list[str] = []
    if unexpected:
        unexplained_reasons.append("unexpected linked worktree(s) present")
    if missing_known:
        unexplained_reasons.append("expected worktree(s) missing")
    if branch and not upstream_name:
        unexplained_reasons.append("current branch has no configured upstream")
    elif branch and upstream_name and not upstream_resolved:
        unexplained_reasons.append(f"configured upstream is unresolved: {upstream_name}")

    if not origin_matches:
        classification, reason = "REMOTE_IDENTITY_MISMATCH", "configured origin does not match expected repository identity"
    elif dirty_entries:
        classification, reason = "DIRTY_OR_UNTRACKED", f"working tree has {len(dirty_entries)} dirty/untracked entrie(s)"
    elif unexplained_reasons:
        classification, reason = "UNEXPLAINED_WORKTREE", "; ".join(unexplained_reasons)
    elif branch is None:
        classification, reason = "DETACHED", "HEAD is detached"
    elif ahead is None or behind is None:
        raise ReconcileError("tracked-branch relationship could not be resolved")
    else:
        classification, reason = relation_from_counts(ahead, behind)

    if classification not in CLASSIFICATIONS:
        raise ReconcileError(f"unknown classification produced: {classification}")

    return {
        "schema_version": 1,
        "classification": classification,
        "reason": reason,
        "repository_root": normalized_path(repo_root),
        "git_dir": resolve_git_path(repo_root, git_dir_raw),
        "git_common_dir": resolve_git_path(repo_root, common_raw),
        "origin": origin,
        "expected_origin": expected_origin,
        "origin_matches": origin_matches,
        "head": head,
        "branch": branch,
        "upstream": upstream_name,
        "upstream_resolved": upstream_resolved,
        "ahead": ahead,
        "behind": behind,
        "dirty_entries": dirty_entries,
        "worktrees": worktrees,
        "remote_ref_census": remote_ref_census,
        "unexpected_worktrees": unexpected,
        "missing_known_worktrees": missing_known,
        "requires_prior_fetch_for_tracking_truth": True,
        "would_modify_repository": False,
        "git_commands": runner.commands,
    }


def build_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--repo", required=True, type=Path)
    parser.add_argument("--expected-origin", required=True)
    parser.add_argument(
        "--known-worktree",
        action="append",
        default=[],
        help="Known linked worktree path; repeat as needed. Repository root is implicit.",
    )
    parser.add_argument("--pretty", action="store_true")
    return parser


def main(argv: list[str] | None = None) -> int:
    args = build_parser().parse_args(argv)
    try:
        result = classify_repository(args.repo, args.expected_origin, args.known_worktree)
    except (OSError, ValueError, ReconcileError) as exc:
        print(json.dumps({"error": str(exc), "would_modify_repository": False}, sort_keys=True))
        return 2
    print(json.dumps(result, indent=2 if args.pretty else None, sort_keys=True))
    return 0


if __name__ == "__main__":
    sys.exit(main())
