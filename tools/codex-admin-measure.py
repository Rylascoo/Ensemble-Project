#!/usr/bin/env python3
"""Deterministic read-only measurements for Ensemble Administrator Skills."""

from __future__ import annotations

import argparse
import fnmatch
import hashlib
import importlib.util
import json
import subprocess
import sys
from pathlib import Path
from typing import Any

MAX_STATE_DISTANCE = 3
AUTHORITY_PATHS = (
    "AGENTS.md",
    "CURRENT_STATE.md",
    "docs/PROJECT_AUTHORITY.md",
    "docs/ENGINEERING_HYGIENE_CONSTITUTION.md",
    "docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md",
    "docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md",
    "docs/AGENT_TOOLING_CAPABILITY_SNAPSHOT.md",
    "docs/PROJECT_EXECUTION_QUEUE.md",
    "docs/VALIDATION_LEDGER.md",
    "docs/blueprint/CODEX_ADMINISTRATOR_RUNTIME_SPECIFICATION.md",
)
PROTECTED_EXACT = {
    "CURRENT_STATE.md",
    "docs/PROJECT_EXECUTION_QUEUE.md",
    "docs/PROJECT_AUTHORITY.md",
    "docs/ENGINEERING_HYGIENE_CONSTITUTION.md",
    "docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md",
    "docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md",
    "docs/VALIDATION_LEDGER.md",
    "docs/REPOSITORY_RESIDENCY.md",
    "docs/EVIDENCE_LANE_CHARTER.md",
    "docs/OPEN_DESIGN_REGISTER_CONTINUATION.md",
    "docs/evidence/DESIGN_LEDGER.md",
}
PROTECTED_GLOBS = (
    "docs/evidence/*DIRECTOR_AUTHORIZATION*",
    "docs/evidence/*DIRECTOR_DECISION*",
    "docs/evidence/*PROVIDER*AUTH*",
    "docs/evidence/*PROVIDER*DECISION*",
)


class MeasureError(RuntimeError):
    pass


def run_git(repo: Path, *args: str, check: bool = True) -> subprocess.CompletedProcess[str]:
    allowed = {"rev-parse", "log", "rev-list", "diff", "cat-file", "status"}
    if not args or args[0] not in allowed:
        raise MeasureError(f"non-read-only Git command rejected: {args!r}")
    completed = subprocess.run(
        ["git", "-C", str(repo), *args],
        text=True,
        encoding="utf-8",
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        check=False,
    )
    if check and completed.returncode != 0:
        detail = completed.stderr.strip() or completed.stdout.strip()
        raise MeasureError(f"git {' '.join(args)} failed: {detail}")
    return completed


def resolve_commit(repo: Path, ref: str) -> str:
    return run_git(repo, "rev-parse", "--verify", f"{ref}^{{commit}}").stdout.strip()


def emit(payload: dict[str, Any]) -> int:
    print(json.dumps(payload, indent=2, sort_keys=True))
    return 0 if payload.get("pass", True) else 1

def state_distance(repo: Path, ref: str, max_distance: int) -> dict[str, Any]:
    commit = resolve_commit(repo, ref)
    state_commit = run_git(repo, "log", "-1", "--format=%H", commit, "--", "CURRENT_STATE.md").stdout.strip()
    if not state_commit:
        raise MeasureError(f"CURRENT_STATE.md has no history at {commit}")
    distance = int(run_git(repo, "rev-list", "--count", f"{state_commit}..{commit}").stdout.strip())
    size = int(run_git(repo, "cat-file", "-s", f"{commit}:CURRENT_STATE.md").stdout.strip())
    return {
        "schema_version": 1,
        "measurement": "state-distance",
        "ref": ref,
        "commit": commit,
        "state_commit": state_commit,
        "distance": distance,
        "max_distance": max_distance,
        "current_state_bytes_utf8": size,
        "pass": distance <= max_distance,
        "authority_created": False,
        "would_modify_repository": False,
    }


def protected_path(path: str) -> bool:
    normalized = path.replace("\\", "/")
    return normalized in PROTECTED_EXACT or any(fnmatch.fnmatch(normalized, pattern) for pattern in PROTECTED_GLOBS)


def protected_diff(repo: Path, base: str, head: str, allowed: set[str]) -> dict[str, Any]:
    base_sha = resolve_commit(repo, base)
    head_sha = resolve_commit(repo, head)
    names = run_git(repo, "diff", "--name-only", "--diff-filter=ACDMRTUXB", f"{base_sha}..{head_sha}").stdout.splitlines()
    protected = sorted(path for path in names if protected_path(path))
    unexplained = sorted(path for path in protected if path not in allowed)
    return {
        "schema_version": 1,
        "measurement": "protected-diff-check",
        "base": base_sha,
        "head": head_sha,
        "changed_paths": sorted(names),
        "protected_paths": protected,
        "allowed_protected_paths": sorted(allowed),
        "unexplained_protected_paths": unexplained,
        "pass": not unexplained,
        "authority_created": False,
        "would_modify_repository": False,
    }

def load_json_source(path: str | None) -> dict[str, Any]:
    text = Path(path).read_text(encoding="utf-8") if path else sys.stdin.read()
    try:
        value = json.loads(text)
    except json.JSONDecodeError as exc:
        raise MeasureError(f"invalid CI JSON: {exc}") from exc
    if not isinstance(value, dict):
        raise MeasureError("CI JSON root must be an object")
    for candidate in (
        value.get("content"),
        value.get("structuredContent", {}).get("content") if isinstance(value.get("structuredContent"), dict) else None,
    ):
        if isinstance(candidate, str):
            try:
                nested = json.loads(candidate)
            except json.JSONDecodeError:
                nested = None
            if isinstance(nested, dict):
                value = nested
                break
    if isinstance(value.get("result"), dict) and isinstance(value["result"].get("workflow_runs"), list):
        value = value["result"]
    return value


def ci_source_from_run_records(records: list[str]) -> dict[str, Any]:
    runs: list[dict[str, Any]] = []
    for record in records:
        parts = record.split("|")
        if len(parts) != 6 or any(not part for part in parts):
            raise MeasureError("each --run-record must be id|run_number|name|head_sha|status|conclusion")
        run_id, run_number, name, head_sha, status, conclusion = parts
        try:
            parsed_id = int(run_id)
            parsed_number = int(run_number)
        except ValueError as exc:
            raise MeasureError("CI run id and run_number must be integers") from exc
        runs.append({
            "id": parsed_id, "run_number": parsed_number, "name": name,
            "head_sha": head_sha, "status": status, "conclusion": conclusion,
        })
    if not runs:
        raise MeasureError("at least one --run-record is required")
    return {"workflow_runs": runs}


def ci_status(source: dict[str, Any], expected_sha: str, required: list[str], expected_run_count: int | None = None) -> dict[str, Any]:
    expected_sha = expected_sha.strip().lower()
    if len(expected_sha) != 40 or any(ch not in "0123456789abcdef" for ch in expected_sha):
        raise MeasureError("expected-sha must be an exact 40-hex commit")
    runs = source.get("workflow_runs")
    if not isinstance(runs, list):
        raise MeasureError("CI JSON must contain workflow_runs[]")
    exact = [run for run in runs if isinstance(run, dict) and str(run.get("head_sha") or "").lower() == expected_sha]
    ids = [run.get("id") for run in runs if isinstance(run, dict)]
    complete_input = True
    completeness_errors: list[str] = []
    if expected_run_count is not None:
        if expected_run_count < 0:
            raise MeasureError("expected-run-count must be non-negative")
        if len(runs) != expected_run_count:
            complete_input = False
            completeness_errors.append(f"received {len(runs)} run record(s), expected {expected_run_count}")
        if len(exact) != len(runs):
            complete_input = False
            completeness_errors.append("one or more supplied run records do not match expected-sha")
        if len(set(ids)) != len(ids):
            complete_input = False
            completeness_errors.append("duplicate workflow run id supplied")
    latest: dict[str, dict[str, Any]] = {}
    for run in exact:
        name = str(run.get("name") or "")
        rank = (int(run.get("run_number") or 0), int(run.get("id") or 0))
        prior = latest.get(name)
        prior_rank = (int(prior.get("run_number") or 0), int(prior.get("id") or 0)) if prior else (-1, -1)
        if rank > prior_rank:
            latest[name] = run
    names = required or sorted(latest)
    normalized = []
    missing = []
    for name in names:
        run = latest.get(name)
        if run is None:
            missing.append(name)
            continue
        normalized.append({
            "name": name,
            "id": run.get("id"),
            "run_number": run.get("run_number"),
            "status": run.get("status"),
            "conclusion": run.get("conclusion"),
            "head_sha": run.get("head_sha"),
        })
    passing = complete_input and not missing and bool(normalized) and all(item["status"] == "completed" and item["conclusion"] == "success" for item in normalized)
    return {
        "schema_version": 1,
        "measurement": "ci-status",
        "expected_sha": expected_sha,
        "required_workflows": names,
        "input_run_count": len(runs),
        "expected_run_count": expected_run_count,
        "complete_input": complete_input,
        "completeness_errors": completeness_errors,
        "runs": normalized,
        "missing_workflows": missing,
        "pass": passing,
        "validation_rung_promoted": False,
        "authority_created": False,
    }

def sha256_file(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest().upper()


def evidence_integrity(root: Path, expectations: list[str]) -> dict[str, Any]:
    records = []
    errors = []
    for item in expectations:
        if "=" not in item:
            raise MeasureError(f"expectation must be path=sha256: {item!r}")
        name, expected = item.rsplit("=", 1)
        expected = expected.strip().upper()
        if len(expected) != 64 or any(ch not in "0123456789ABCDEF" for ch in expected):
            raise MeasureError(f"expected SHA-256 must be exact 64-hex: {item!r}")
        path = Path(name)
        target = path if path.is_absolute() else root / path
        if not target.is_file():
            records.append({"path": str(target), "exists": False, "expected_sha256": expected})
            errors.append(str(target))
            continue
        actual = sha256_file(target)
        matched = actual == expected
        records.append({
            "path": str(target),
            "exists": True,
            "size_bytes": target.stat().st_size,
            "expected_sha256": expected,
            "actual_sha256": actual,
            "matched": matched,
        })
        if not matched:
            errors.append(str(target))
    return {
        "schema_version": 1,
        "measurement": "evidence-integrity",
        "files": records,
        "mismatches": errors,
        "pass": bool(records) and not errors,
        "authority_created": False,
        "would_modify_files": False,
    }


def load_reconciler(repo: Path):
    del repo  # target checkout may predate Administrator tooling
    tool = Path(__file__).resolve().with_name("codex-admin-reconcile.py")
    spec = importlib.util.spec_from_file_location("codex_admin_reconcile", tool)
    if spec is None or spec.loader is None:
        raise MeasureError(f"cannot load reconciler: {tool}")
    module = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(module)
    return module

def authority_recovery(repo: Path, expected_origin: str, remote_main: str, known: list[str]) -> dict[str, Any]:
    if len(remote_main) != 40 or any(ch not in "0123456789abcdefABCDEF" for ch in remote_main):
        raise MeasureError("remote-main must be an exact 40-hex commit")
    reconciler = load_reconciler(repo)
    local = reconciler.classify_repository(repo, expected_origin, known)
    available = run_git(repo, "cat-file", "-e", f"{remote_main}^{{commit}}", check=False).returncode == 0
    blobs: dict[str, str | None] = {}
    distance: dict[str, Any] | None = None
    if available:
        for path in AUTHORITY_PATHS:
            probe = run_git(repo, "rev-parse", "--verify", f"{remote_main}:{path}", check=False)
            blobs[path] = probe.stdout.strip() if probe.returncode == 0 else None
        distance = state_distance(repo, remote_main, MAX_STATE_DISTANCE)
    else:
        blobs = {path: None for path in AUTHORITY_PATHS}
    fetched = local.get("remote_ref_census", {}).get("main_sha")
    missing = sorted(path for path, blob in blobs.items() if blob is None)
    return {
        "schema_version": 1,
        "measurement": "authority-recovery",
        "remote_main": remote_main.lower(),
        "remote_main_available_locally": available,
        "fetched_origin_main": fetched,
        "remote_main_matches_fetched": fetched == remote_main.lower(),
        "authority_blob_shas": blobs,
        "missing_authority_paths": missing,
        "state_distance": distance,
        "local_reconciliation": local,
        "pass": available and not missing and bool(local.get("origin_matches")) and fetched == remote_main.lower(),
        "interpretation_required": True,
        "authority_created": False,
        "would_modify_repository": False,
    }


def run_check(repo: Path, args: list[str]) -> dict[str, Any]:
    completed = subprocess.run(
        [sys.executable, "-B", *args], cwd=repo, text=True, encoding="utf-8",
        stdout=subprocess.PIPE, stderr=subprocess.PIPE, check=False,
    )
    return {
        "command": args,
        "exit_code": completed.returncode,
        "stdout": completed.stdout.strip(),
        "stderr": completed.stderr.strip(),
        "pass": completed.returncode == 0,
    }

def commissioning_closeout(repo: Path, baseline: str, head: str, allowed: set[str]) -> dict[str, Any]:
    base_sha = resolve_commit(repo, baseline)
    head_sha = resolve_commit(repo, head)
    checked_out_head = resolve_commit(repo, "HEAD")
    status = run_git(repo, "status", "--porcelain=v1").stdout.splitlines()
    exact_clean_head = checked_out_head == head_sha and not status
    checks = [
        run_check(repo, ["tools/repository-law-check.py"]),
        run_check(repo, ["tools/document-census.py", "--summary", "--check"]),
        run_check(repo, ["tools/oracle-index.py", "--check", "--baseline", base_sha]),
    ]
    diff_check = run_git(repo, "diff", "--check", f"{base_sha}..{head_sha}", check=False)
    checks.append({
        "command": ["git", "diff", "--check", f"{base_sha}..{head_sha}"],
        "exit_code": diff_check.returncode,
        "stdout": diff_check.stdout.strip(),
        "stderr": diff_check.stderr.strip(),
        "pass": diff_check.returncode == 0,
    })
    protected = protected_diff(repo, base_sha, head_sha, allowed)
    passing = exact_clean_head and all(check["pass"] for check in checks) and protected["pass"]
    return {
        "schema_version": 1,
        "measurement": "commissioning-closeout",
        "baseline": base_sha,
        "head": head_sha,
        "checked_out_head": checked_out_head,
        "worktree_clean": not status,
        "worktree_status": status,
        "exact_clean_head": exact_clean_head,
        "checks": checks,
        "protected_diff": protected,
        "pass": passing,
        "validation_rung_promoted": False,
        "authority_created": False,
        "would_modify_repository": False,
    }


def build_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(description=__doc__)
    sub = parser.add_subparsers(dest="command", required=True)
    state = sub.add_parser("state-distance")
    state.add_argument("--repo", type=Path, required=True)
    state.add_argument("--ref", default="HEAD")
    state.add_argument("--max-distance", type=int, default=MAX_STATE_DISTANCE)
    protected = sub.add_parser("protected-diff-check")
    protected.add_argument("--repo", type=Path, required=True)
    protected.add_argument("--base", required=True)
    protected.add_argument("--head", default="HEAD")
    protected.add_argument("--allow-protected", action="append", default=[])
    ci = sub.add_parser("ci-status")
    ci_input = ci.add_mutually_exclusive_group(required=True)
    ci_input.add_argument("--input")
    ci_input.add_argument("--run-record", action="append", default=[])
    ci.add_argument("--expected-sha", required=True)
    ci.add_argument("--expected-run-count", type=int)
    ci.add_argument("--require-workflow", action="append", default=[])
    evidence = sub.add_parser("evidence-integrity")
    evidence.add_argument("--root", type=Path, default=Path.cwd())
    evidence.add_argument("--expect", action="append", required=True)
    recovery = sub.add_parser("authority-recovery")
    recovery.add_argument("--repo", type=Path, required=True)
    recovery.add_argument("--expected-origin", required=True)
    recovery.add_argument("--remote-main", required=True)
    recovery.add_argument("--known-worktree", action="append", default=[])
    closeout = sub.add_parser("commissioning-closeout")
    closeout.add_argument("--repo", type=Path, required=True)
    closeout.add_argument("--baseline", required=True)
    closeout.add_argument("--head", default="HEAD")
    closeout.add_argument("--allow-protected", action="append", default=[])
    return parser

def main(argv: list[str] | None = None) -> int:
    args = build_parser().parse_args(argv)
    try:
        if args.command == "state-distance":
            result = state_distance(args.repo.resolve(), args.ref, args.max_distance)
        elif args.command == "protected-diff-check":
            result = protected_diff(args.repo.resolve(), args.base, args.head, set(args.allow_protected))
        elif args.command == "ci-status":
            source = ci_source_from_run_records(args.run_record) if args.run_record else load_json_source(args.input)
            expected_count = args.expected_run_count
            if args.run_record and expected_count is None:
                raise MeasureError("--expected-run-count is required with --run-record")
            if expected_count is None and isinstance(source.get("total_count"), int):
                expected_count = source["total_count"]
            result = ci_status(source, args.expected_sha, args.require_workflow, expected_count)
        elif args.command == "evidence-integrity":
            result = evidence_integrity(args.root.resolve(), args.expect)
        elif args.command == "authority-recovery":
            result = authority_recovery(
                args.repo.resolve(), args.expected_origin, args.remote_main, args.known_worktree
            )
        elif args.command == "commissioning-closeout":
            result = commissioning_closeout(
                args.repo.resolve(), args.baseline, args.head, set(args.allow_protected)
            )
        else:
            raise MeasureError(f"unknown command: {args.command}")
    except (OSError, ValueError, MeasureError, subprocess.CalledProcessError) as exc:
        print(json.dumps({
            "error": str(exc),
            "pass": False,
            "authority_created": False,
            "would_modify_repository": False,
        }, sort_keys=True))
        return 2
    return emit(result)


if __name__ == "__main__":
    raise SystemExit(main())