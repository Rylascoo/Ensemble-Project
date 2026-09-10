#!/usr/bin/env python3
"""Deterministic C3 fixtures for the initial Ensemble Administrator Skills."""

from __future__ import annotations

import hashlib
import importlib.util
import json
import shutil
import subprocess
import sys
import tempfile
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
MEASURE = ROOT / "tools" / "codex-admin-measure.py"
RECONCILE_SELFTEST = ROOT / "tools" / "codex-admin-reconcile-selftest.py"
REQUIRED_SKILLS = (
    "authority-recovery",
    "repo-reconciliation",
    "branch-worktree-census",
    "state-distance",
    "protected-diff-check",
    "ci-status",
    "evidence-integrity",
    "commissioning-closeout",
)


def run(cwd: Path, *args: str, check: bool = True) -> subprocess.CompletedProcess[str]:
    completed = subprocess.run(list(args), cwd=cwd, text=True, encoding="utf-8",
                           stdout=subprocess.PIPE, stderr=subprocess.PIPE, check=False)
    if check and completed.returncode != 0:
        detail = completed.stderr.strip() or completed.stdout.strip()
        raise RuntimeError(f"command failed: {args!r}: {detail}")
    return completed


def git(cwd: Path, *args: str) -> str:
    return run(cwd, "git", *args).stdout.strip()


def load_measure():
    spec = importlib.util.spec_from_file_location("codex_admin_measure", MEASURE)
    if spec is None or spec.loader is None:
        raise RuntimeError("cannot load measurement utility")
    module = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(module)
    return module


def configure_identity(repo: Path) -> None:
    git(repo, "config", "user.name", "C3 Fixture")
    git(repo, "config", "user.email", "c3-fixture@example.invalid")


def commit(repo: Path, message: str) -> str:
    git(repo, "add", "-A")
    git(repo, "commit", "-m", message)
    return git(repo, "rev-parse", "HEAD")


def make_fixture_remote(parent: Path) -> tuple[Path, str]:
    remote = parent / "fixture-remote.git"
    source_head = git(ROOT, "rev-parse", "HEAD")
    run(parent, "git", "clone", "--bare", "--shared", str(ROOT), str(remote))
    run(parent, "git", f"--git-dir={remote}", "update-ref", "refs/heads/main", source_head)
    run(parent, "git", f"--git-dir={remote}", "symbolic-ref", "HEAD", "refs/heads/main")
    return remote, source_head


def make_full_fixture(parent: Path, name: str, remote: Path, source_head: str) -> tuple[Path, Path, str]:
    work = parent / f"{name}-work"
    run(parent, "git", "clone", "--shared", str(remote), str(work))
    configure_identity(work)
    return work, remote, source_head


def skill_contracts() -> None:
    actual = sorted(path.name for path in (ROOT / ".agents" / "skills").iterdir() if path.is_dir())
    if actual != sorted(REQUIRED_SKILLS):
        raise AssertionError(f"unexpected Skill set: {actual}")
    forbidden = ("`git fetch", "`git push", "`git merge", "`git reset", "`git clean", "`git checkout", "`git switch")
    for name in REQUIRED_SKILLS:
        text = (ROOT / ".agents" / "skills" / name / "SKILL.md").read_text(encoding="utf-8")
        if not text.startswith("---\n") or f"name: {name}\n" not in text or "description:" not in text:
            raise AssertionError(f"invalid frontmatter: {name}")
        if "authority" not in text.lower():
            raise AssertionError(f"authority disclaimer missing: {name}")
        if "$env:ENSEMBLE_PYTHON -I -B" not in text:
            raise AssertionError(f"isolated pinned Python runtime contract missing: {name}")
        if "python -B" in text or "$env:ENSEMBLE_PYTHON -B" in text:
            raise AssertionError(f"non-isolated/ambient Python invocation remains: {name}")
        if any(token in text.lower() for token in forbidden):
            raise AssertionError(f"mutating Git command embedded in Skill: {name}")


def reconciler_fixture() -> None:
    result = run(ROOT, sys.executable, "-B", str(RECONCILE_SELFTEST))
    required = (
        "C1_REMOTE_REF_TOPOLOGY=PASS",
        "C1_MUTATING_GIT_GUARD=PASS",
        "C1_SYNTHETIC_CLASSIFICATIONS=PASS",
        "C1_CLASSIFIER_MUTATION_CHECK=PASS",
        "C1_READ_ONLY_GIT_ALLOWLIST=PASS",
    )
    if any(marker not in result.stdout for marker in required):
        raise AssertionError("C1 reconciler fixture did not prove all required invariants")


def state_distance_fixture(measure, parent: Path, remote: Path, source_head: str) -> None:
    repo, _, _ = make_full_fixture(parent, "state", remote, source_head)
    initial = measure.state_distance(repo, "HEAD", 3)
    if not initial["pass"] or initial["distance"] > 3:
        raise AssertionError(f"initial state-distance failed: {initial}")
    for index in range(4):
        with (repo / "README.md").open("a", encoding="utf-8") as handle:
            handle.write(f"\nC3 state fixture {index}\n")
        commit(repo, f"C3 state fixture {index}")
    stale = measure.state_distance(repo, "HEAD", 3)
    if stale["pass"] or stale["distance"] <= 3:
        raise AssertionError(f"stale state-distance was not rejected: {stale}")


def protected_diff_fixture(measure, parent: Path, remote: Path, source_head: str) -> None:
    repo, _, baseline = make_full_fixture(parent, "protected", remote, source_head)
    state = repo / "CURRENT_STATE.md"
    state.write_text(state.read_text(encoding="utf-8") + "\n<!-- C3 protected fixture -->\n", encoding="utf-8")
    head = commit(repo, "C3 protected fixture")
    blocked = measure.protected_diff(repo, baseline, head, set())
    allowed = measure.protected_diff(repo, baseline, head, {"CURRENT_STATE.md"})
    if blocked["pass"] or blocked["unexplained_protected_paths"] != ["CURRENT_STATE.md"]:
        raise AssertionError(f"protected diff did not fail closed: {blocked}")
    if not allowed["pass"] or allowed["unexplained_protected_paths"]:
        raise AssertionError(f"explicit protected allowance failed: {allowed}")


def ci_fixture(measure) -> None:
    sha = "a" * 40
    source = {"workflow_runs": [
        {"id": 10, "run_number": 2, "name": "Validation gate", "head_sha": sha,
         "status": "completed", "conclusion": "success"},
        {"id": 9, "run_number": 1, "name": "Validation gate", "head_sha": sha,
         "status": "completed", "conclusion": "failure"},
        {"id": 11, "run_number": 1, "name": "E0-E preparation gate", "head_sha": sha,
         "status": "completed", "conclusion": "success"},
        {"id": 12, "run_number": 99, "name": "Validation gate", "head_sha": "b" * 40,
         "status": "completed", "conclusion": "success"},
    ]}
    passed = measure.ci_status(source, sha, ["Validation gate", "E0-E preparation gate"])
    missing = measure.ci_status(source, sha, ["Missing gate"])
    complete = measure.ci_status({"workflow_runs": source["workflow_runs"][:3]}, sha, ["Validation gate"], 3)
    omitted = measure.ci_status({"workflow_runs": source["workflow_runs"][:1]}, sha, ["Validation gate"], 3)
    if not passed["pass"] or passed["validation_rung_promoted"]:
        raise AssertionError(f"exact-SHA CI success not recognized: {passed}")
    if missing["pass"] or missing["missing_workflows"] != ["Missing gate"]:
        raise AssertionError(f"missing CI gate did not fail closed: {missing}")
    if not complete["pass"] or complete["runs"][0]["run_number"] != 2 or not complete["complete_input"]:
        raise AssertionError(f"complete latest-run selection failed: {complete}")
    if omitted["pass"] or omitted["complete_input"] or not omitted["completeness_errors"]:
        raise AssertionError(f"omitted newer CI run did not fail closed: {omitted}")
    records = ["|".join(str(run[key]) for key in ("id", "run_number", "name", "head_sha", "status", "conclusion")) for run in source["workflow_runs"][:3]]
    cli_args = [sys.executable, "-I", "-B", str(MEASURE), "ci-status"]
    for record in records:
        cli_args.extend(["--run-record", record])
    cli_args.extend(["--expected-run-count", "3", "--expected-sha", sha, "--require-workflow", "Validation gate"])
    cli = run(ROOT, *cli_args)
    parsed = json.loads(cli.stdout)
    if not parsed.get("pass") or parsed.get("validation_rung_promoted"):
        raise AssertionError(f"--run-record CLI contract failed: {parsed}")


def evidence_fixture(measure, parent: Path) -> None:
    root = parent / "evidence"
    root.mkdir()
    target = root / "sample.bin"
    target.write_bytes(b"ensemble-c3\n")
    digest = hashlib.sha256(target.read_bytes()).hexdigest().upper()
    passed = measure.evidence_integrity(root, [f"sample.bin={digest}"])
    failed = measure.evidence_integrity(root, [f"sample.bin={'0' * 64}"])
    if not passed["pass"] or not passed["files"][0]["matched"]:
        raise AssertionError(f"evidence hash match failed: {passed}")
    if failed["pass"] or not failed["mismatches"]:
        raise AssertionError(f"evidence hash mismatch did not fail closed: {failed}")
    try:
        measure.evidence_integrity(root, ["sample.bin=not-a-sha"])
    except measure.MeasureError:
        pass
    else:
        raise AssertionError("malformed evidence SHA was accepted")


def authority_recovery_fixture(measure, parent: Path, remote: Path, source_head: str) -> None:
    repo, remote, head = make_full_fixture(parent, "authority", remote, source_head)
    passed = measure.authority_recovery(repo, str(remote), head, [])
    if not passed["pass"] or passed["missing_authority_paths"]:
        raise AssertionError(f"authority recovery failed: {passed}")
    if not passed["authority_blob_shas"].get("AGENTS.md"):
        raise AssertionError("AGENTS.md missing from authority recovery cross-check")
    prior = git(repo, "rev-parse", "HEAD^")
    stale = measure.authority_recovery(repo, str(remote), prior, [])
    if stale["pass"] or stale["remote_main_matches_fetched"]:
        raise AssertionError(f"stale fetched main did not fail closed: {stale}")


def closeout_fixture(measure, parent: Path, remote: Path, source_head: str) -> None:
    repo, _, baseline = make_full_fixture(parent, "closeout", remote, source_head)
    readme = repo / "README.md"
    readme.write_text(readme.read_text(encoding="utf-8") + "\nC3 closeout fixture.\n", encoding="utf-8")
    harmless = commit(repo, "C3 harmless closeout fixture")
    passed = measure.commissioning_closeout(repo, baseline, harmless, set())
    if not passed["pass"] or not passed["exact_clean_head"]:
        raise AssertionError(f"clean harmless closeout failed: {passed}")
    with readme.open("a", encoding="utf-8") as handle:
        handle.write("dirty fixture\n")
    dirty = measure.commissioning_closeout(repo, baseline, harmless, set())
    if dirty["pass"] or dirty["worktree_clean"]:
        raise AssertionError(f"dirty worktree closeout was accepted: {dirty}")
    git(repo, "restore", "README.md")
    state = repo / "CURRENT_STATE.md"
    state.write_text(state.read_text(encoding="utf-8") + "\n<!-- C3 closeout authority fixture -->\n", encoding="utf-8")
    protected_head = commit(repo, "C3 protected closeout fixture")
    blocked = measure.commissioning_closeout(repo, harmless, protected_head, set())
    allowed = measure.commissioning_closeout(repo, harmless, protected_head, {"CURRENT_STATE.md"})
    if blocked["pass"] or not blocked["protected_diff"]["unexplained_protected_paths"]:
        raise AssertionError(f"protected closeout did not fail closed: {blocked}")
    if not allowed["pass"]:
        raise AssertionError(f"reviewed protected closeout did not pass: {allowed}")


def read_only_guard_fixture(measure, parent: Path, remote: Path, source_head: str) -> None:
    repo, _, _ = make_full_fixture(parent, "guard", remote, source_head)
    try:
        measure.run_git(repo, "checkout", "--detach", "HEAD")
    except measure.MeasureError:
        pass
    else:
        raise AssertionError("measurement utility accepted a mutating Git command")


def main() -> int:
    measure = load_measure()
    skill_contracts()
    print("C3_SKILL_CONTRACTS=PASS")
    reconciler_fixture()
    print("C3_REPO_RECONCILIATION_FIXTURE=PASS")
    print("C3_BRANCH_WORKTREE_CENSUS_FIXTURE=PASS")
    with tempfile.TemporaryDirectory(prefix="ensemble-c3-skills-") as raw:
        parent = Path(raw)
        remote, source_head = make_fixture_remote(parent)
        state_distance_fixture(measure, parent, remote, source_head)
        print("C3_STATE_DISTANCE_FIXTURE=PASS")
        protected_diff_fixture(measure, parent, remote, source_head)
        print("C3_PROTECTED_DIFF_FIXTURE=PASS")
        ci_fixture(measure)
        print("C3_CI_STATUS_FIXTURE=PASS")
        evidence_fixture(measure, parent)
        print("C3_EVIDENCE_INTEGRITY_FIXTURE=PASS")
        authority_recovery_fixture(measure, parent, remote, source_head)
        print("C3_AUTHORITY_RECOVERY_FIXTURE=PASS")
        closeout_fixture(measure, parent, remote, source_head)
        print("C3_COMMISSIONING_CLOSEOUT_FIXTURE=PASS")
        read_only_guard_fixture(measure, parent, remote, source_head)
        print("C3_MEASUREMENT_READ_ONLY_GUARD=PASS")
    print("C3_INITIAL_SKILLS_FIXTURES=PASS")
    return 0


if __name__ == "__main__":
    try:
        raise SystemExit(main())
    except (AssertionError, OSError, RuntimeError, subprocess.CalledProcessError) as exc:
        print(f"C3_INITIAL_SKILLS_FIXTURES=FAIL: {exc}", file=sys.stderr)
        raise SystemExit(1)
