#!/usr/bin/env python3
"""C9A on-demand, repository-read-only Claude review dispatcher."""
from __future__ import annotations

import argparse
import hashlib
import json
import os
import subprocess
import sys
import uuid
from dataclasses import dataclass
from pathlib import Path
from typing import Callable

SENSITIVE_ENV = (
    "ANTHROPIC_API_KEY", "ANTHROPIC_AUTH_TOKEN", "ANTHROPIC_BASE_URL",
    "CLAUDE_CODE_USE_BEDROCK", "CLAUDE_CODE_USE_VERTEX",
    "CLAUDE_CODE_USE_FOUNDRY", "AWS_ACCESS_KEY_ID", "AWS_SECRET_ACCESS_KEY",
    "AWS_SESSION_TOKEN", "GOOGLE_APPLICATION_CREDENTIALS", "AZURE_API_KEY",
)

REVIEW_SCHEMA = {
    "type": "object",
    "properties": {
        "verdict": {"type": "string", "enum": ["PASS", "PASS_WITH_FINDINGS", "FAIL"]},
        "summary": {"type": "string"},
        "findings": {"type": "array", "items": {"type": "object"}},
    },
    "required": ["verdict", "summary", "findings"],
    "additionalProperties": True,
}


class DispatchError(RuntimeError):
    pass


@dataclass(frozen=True)
class DispatchRequest:
    repo: Path
    ref: str
    paths: tuple[str, ...]
    question: str
    scratch_root: Path
    claude_exe: Path
    claude_config_dir: Path


@dataclass(frozen=True)
class CommandResult:
    returncode: int
    stdout: str
    stderr: str
    timed_out: bool = False
    timeout_seconds: int | None = None


Runner = Callable[..., CommandResult]
PROCESS_TIMEOUT_SECONDS = 180


def sha256_bytes(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest().upper()


def sha256_text(text: str) -> str:
    return sha256_bytes(text.encode("utf-8"))


def sha256_file(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest().upper()


def _timeout_text(value: str | bytes | None) -> str:
    if value is None:
        return ""
    if isinstance(value, bytes):
        return value.decode("utf-8", errors="replace")
    return value


def process_runner(
    args: list[str], *, cwd: Path, env: dict[str, str], input_text: str | None = None
) -> CommandResult:
    try:
        completed = subprocess.run(
            args, cwd=str(cwd), env=env, input=input_text, text=True, encoding="utf-8",
            errors="replace", stdout=subprocess.PIPE, stderr=subprocess.PIPE, check=False,
            timeout=PROCESS_TIMEOUT_SECONDS,
        )
        return CommandResult(completed.returncode, completed.stdout, completed.stderr)
    except subprocess.TimeoutExpired as exc:
        return CommandResult(
            124, _timeout_text(exc.stdout), _timeout_text(exc.stderr),
            timed_out=True, timeout_seconds=PROCESS_TIMEOUT_SECONDS,
        )

def git(repo: Path, *args: str) -> str:
    completed = subprocess.run(
        ["git", "-C", str(repo), *args],
        text=True,
        encoding="utf-8",
        errors="replace",
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        check=False,
    )
    if completed.returncode != 0:
        detail = completed.stderr.strip() or completed.stdout.strip()
        raise DispatchError(f"git {' '.join(args)} failed: {detail}")
    return completed.stdout


def worktree_paths(repo: Path) -> list[Path]:
    lines = git(repo, "worktree", "list", "--porcelain").splitlines()
    result: list[Path] = []
    for line in lines:
        if line.startswith("worktree "):
            result.append(Path(line[9:]).resolve())
    return result


def is_within(child: Path, parent: Path) -> bool:
    try:
        child.resolve().relative_to(parent.resolve())
        return True
    except ValueError:
        return False


def snapshot(repo: Path) -> dict[str, object]:
    branch_cp = subprocess.run(
        ["git", "-C", str(repo), "symbolic-ref", "--short", "-q", "HEAD"],
        text=True, encoding="utf-8", errors="replace",
        stdout=subprocess.PIPE, stderr=subprocess.PIPE, check=False,
    )
    status = git(repo, "status", "--porcelain=v2", "--branch", "--untracked-files=all")
    refs = git(repo, "for-each-ref", "--format=%(refname)|%(objectname)|%(objecttype)")
    worktrees = git(repo, "worktree", "list", "--porcelain")
    return {
        "head": git(repo, "rev-parse", "HEAD").strip(),
        "branch": branch_cp.stdout.strip(),
        "status_sha256": sha256_text(status),
        "refs_sha256": sha256_text(refs),
        "worktrees_sha256": sha256_text(worktrees),
        "status": status,
    }


def build_packet(request: DispatchRequest, commit: str) -> tuple[str, list[dict[str, str]]]:
    sections = [
        "# Ensemble C9A independent review packet",
        f"EXACT_COMMIT: {commit}",
        "AUTHORITY: advisory review only; this packet grants no mutation or project authority.",
        "REVIEW QUESTION:",
        request.question.strip(),
        "",
        "REVIEW RULES:",
        "- Review only the packet content below.",
        "- Do not assume repository, web, tool, credential, or runtime access.",
        "- Treat findings as advisory; do not implement or mutate anything.",
        "- Focus on authority leakage, mutation paths, auth/billing fallback, retry loops, and hidden dependencies.",
        "- Return exactly one JSON object with keys verdict, summary, and findings; no markdown fences or prose outside JSON.",
        "",
    ]
    manifest: list[dict[str, str]] = []
    for rel in request.paths:
        if rel.startswith(("/", "\\")) or ".." in Path(rel).parts:
            raise DispatchError(f"unsafe repository path: {rel}")
        blob = git(request.repo, "rev-parse", f"{commit}:{rel}").strip()
        content = git(request.repo, "show", f"{commit}:{rel}")
        manifest.append({"path": rel, "blob": blob, "sha256": sha256_text(content)})
        sections.extend([f"## FILE: {rel}", f"BLOB: {blob}", content.rstrip("\n"), ""])
    packet = "\n".join(sections).rstrip() + "\n"
    return packet, manifest


def scrubbed_env(config_dir: Path) -> tuple[dict[str, str], list[str]]:
    env = dict(os.environ)
    removed: list[str] = []
    for name in SENSITIVE_ENV:
        if name in env:
            removed.append(name)
            env.pop(name, None)
    env["CLAUDE_CONFIG_DIR"] = str(config_dir)
    return env, removed


def checked_auth(
    request: DispatchRequest, run_dir: Path, env: dict[str, str], runner: Runner
) -> dict[str, object]:
    completed = runner(
        [str(request.claude_exe), "auth", "status", "--json"],
        cwd=run_dir, env=env,
    )
    if completed.returncode != 0:
        raise DispatchError("Claude auth status failed under the scrubbed C9 profile")
    try:
        status = json.loads(completed.stdout)
    except json.JSONDecodeError as exc:
        raise DispatchError("Claude auth status was not JSON") from exc
    required = {
        "loggedIn": True,
        "authMethod": "claude.ai",
        "apiProvider": "firstParty",
        "subscriptionType": "pro",
    }
    mismatches = {k: (status.get(k), v) for k, v in required.items() if status.get(k) != v}
    if mismatches:
        raise DispatchError(f"Claude auth boundary mismatch: {mismatches}")
    return {key: status.get(key) for key in required}


def review_command(claude_exe: Path) -> list[str]:
    return [
        str(claude_exe), "-p",
        "--safe-mode", "--restricted", "--tools", "",
        "--strict-mcp-config", "--setting-sources", "user",
        "--permission-mode", "plan", "--permission-prompts", "none",
        "--no-session-persistence", "--output-format", "json",
        "--effort", "high", "--max-turns", "1",
        "--no-chrome", "--disable-slash-commands",
    ]


def extract_review_payload(envelope: dict[str, object]) -> dict[str, object] | None:
    result = envelope.get("result")
    if not isinstance(result, str):
        return None
    try:
        parsed = json.loads(result)
    except json.JSONDecodeError:
        return None
    return parsed if isinstance(parsed, dict) else None


def validate_review_payload(payload: dict[str, object] | None) -> tuple[bool, str | None]:
    if payload is None:
        return False, "result was not a JSON object string"
    required = REVIEW_SCHEMA.get("required", [])
    if not isinstance(required, list):
        return False, "review schema required-set is invalid"
    missing = [name for name in required if name not in payload]
    if missing:
        return False, f"missing required fields: {missing}"
    properties = REVIEW_SCHEMA.get("properties", {})
    if not isinstance(properties, dict):
        return False, "review schema properties are invalid"
    verdict_rule = properties.get("verdict", {})
    allowed = verdict_rule.get("enum", []) if isinstance(verdict_rule, dict) else []
    if payload.get("verdict") not in allowed:
        return False, "invalid verdict"
    if not isinstance(payload.get("summary"), str):
        return False, "summary was not a string"
    findings = payload.get("findings")
    if not isinstance(findings, list) or not all(isinstance(item, dict) for item in findings):
        return False, "findings was not an array of objects"
    return True, None

def dispatch(request: DispatchRequest, runner: Runner = process_runner) -> dict[str, object]:
    repo = request.repo.resolve()
    top = Path(git(repo, "rev-parse", "--show-toplevel").strip()).resolve()
    if top != repo:
        raise DispatchError(f"--repo must be the exact Git root: {top}")
    if not request.paths:
        raise DispatchError("at least one --path is required")
    if not request.question.strip():
        raise DispatchError("--question must be non-empty")
    if not request.claude_exe.is_file():
        raise DispatchError(f"Claude executable not found: {request.claude_exe}")
    if not request.claude_config_dir.is_dir():
        raise DispatchError(f"Claude config directory not found: {request.claude_config_dir}")

    pre = snapshot(repo)
    scratch_root = request.scratch_root.resolve()
    for worktree in worktree_paths(repo):
        if is_within(scratch_root, worktree):
            raise DispatchError(f"scratch root must be outside every Git worktree: {scratch_root}")
    scratch_root.mkdir(parents=True, exist_ok=True)
    run_dir = scratch_root / f"c9a-{uuid.uuid4().hex}"
    run_dir.mkdir(parents=False, exist_ok=False)

    commit = git(repo, "rev-parse", f"{request.ref}^{{commit}}").strip()
    packet, manifest = build_packet(request, commit)
    packet_path = run_dir / "review-packet.md"
    packet_path.write_text(packet, encoding="utf-8", newline="\n")
    packet_hash = sha256_file(packet_path)

    env, removed_env = scrubbed_env(request.claude_config_dir.resolve())
    auth = checked_auth(request, run_dir, env, runner)
    version_cp = runner([str(request.claude_exe), "--version"], cwd=run_dir, env=env)
    if version_cp.returncode != 0:
        raise DispatchError("Claude version preflight failed")

    command = review_command(request.claude_exe)
    completed = runner(command, cwd=run_dir, env=env, input_text=packet)
    post = snapshot(repo)
    unchanged = pre == post
    timed_out = bool(getattr(completed, "timed_out", False))

    result_path = run_dir / "claude-result.json"
    stderr_path = run_dir / "claude-stderr.txt"
    result_path.write_text(completed.stdout, encoding="utf-8", newline="\n")
    stderr_path.write_text(completed.stderr, encoding="utf-8", newline="\n")
    result_hash = sha256_file(result_path)
    stderr_hash = sha256_file(stderr_path)

    envelope: dict[str, object] | None = None
    payload: dict[str, object] | None = None
    failure_reason: str | None = None
    if timed_out:
        failure_reason = f"Claude subprocess exceeded {getattr(completed, 'timeout_seconds', PROCESS_TIMEOUT_SECONDS)}s hard bound"
    elif completed.returncode != 0:
        failure_reason = f"Claude review exited {completed.returncode}"
    else:
        try:
            parsed = json.loads(completed.stdout)
        except json.JSONDecodeError:
            failure_reason = "Claude review output envelope was not JSON"
        else:
            if isinstance(parsed, dict):
                envelope = parsed
                payload = extract_review_payload(envelope)
            else:
                failure_reason = "Claude review output envelope was not an object"

    valid_payload, validation_error = validate_review_payload(payload)
    if failure_reason is None and not valid_payload:
        failure_reason = f"local review schema validation failed: {validation_error}"

    provider = envelope.get("provider") if envelope else None
    permission_denials = envelope.get("permission_denials", []) if envelope else []
    num_turns = envelope.get("num_turns") if envelope else None
    is_error = bool(envelope.get("is_error", False)) if envelope else False
    if failure_reason is None and provider != "firstParty":
        failure_reason = f"unexpected Claude result provider: {provider!r}"
    if failure_reason is None and num_turns != 1:
        failure_reason = f"unexpected Claude turn count: {num_turns!r}"
    if failure_reason is None and permission_denials not in ([], None):
        failure_reason = "Claude reported permission denials"
    if failure_reason is None and is_error:
        failure_reason = "Claude envelope reported is_error"
    if not unchanged:
        mutation_reason = "repository snapshot changed during review"
        failure_reason = f"{failure_reason}; {mutation_reason}" if failure_reason else mutation_reason

    findings = payload.get("findings") if payload else None
    verdict = payload.get("verdict") if payload else None
    passed = failure_reason is None

    telemetry: dict[str, object] = {
        "schema": "ensemble.c9a-claude-review.v2",
        "pass": passed,
        "failure_reason": failure_reason,
        "repo": str(repo),
        "requested_ref": request.ref,
        "exact_commit": commit,
        "packet": {"path": str(packet_path), "sha256": packet_hash, "bytes": packet_path.stat().st_size},
        "manifest": manifest,
        "question_sha256": sha256_text(request.question.strip()),
        "claude": {
            "path": str(request.claude_exe.resolve()),
            "sha256": sha256_file(request.claude_exe),
            "version": version_cp.stdout.strip(),
            "auth": auth,
            "removed_sensitive_env_names": sorted(removed_env),
            "command": command[1:],
            "native_structured_output_requested": False,
        },
        "result": {"path": str(result_path), "sha256": result_hash, "bytes": result_path.stat().st_size},
        "stderr": {"path": str(stderr_path), "sha256": stderr_hash, "bytes": stderr_path.stat().st_size},
        "execution": {
            "exit_code": completed.returncode,
            "timed_out": timed_out,
            "timeout_seconds": getattr(completed, "timeout_seconds", None),
            "provider": provider,
            "num_turns": num_turns,
            "permission_denials": permission_denials,
            "is_error": is_error,
            "max_turns_requested": 1,
            "session_persistence_requested": False,
            "tools_requested": [],
            "mcp_inheritance_allowed": False,
            "dispatcher_retry_count": 0,
            "local_schema_validation": True,
        },
        "review": {
            "verdict": verdict,
            "summary": payload.get("summary") if payload else None,
            "findings_count": len(findings) if isinstance(findings, list) else None,
            "payload": payload,
            "validation_error": validation_error,
        },
        "repository_snapshot": {"before": pre, "after": post, "unchanged": unchanged},
    }
    telemetry_path = run_dir / "telemetry.json"
    telemetry_path.write_text(
        json.dumps(telemetry, indent=2, sort_keys=True) + "\n",
        encoding="utf-8", newline="\n",
    )
    telemetry["telemetry"] = {
        "path": str(telemetry_path),
        "sha256": sha256_file(telemetry_path),
        "bytes": telemetry_path.stat().st_size,
    }
    if not passed:
        raise DispatchError(
            f"C9A review failed closed: {failure_reason}; telemetry={telemetry_path}"
        )
    return telemetry

def parse_args(argv: list[str]) -> DispatchRequest:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--repo", required=True, type=Path)
    parser.add_argument("--ref", required=True)
    parser.add_argument("--path", action="append", dest="paths", required=True)
    parser.add_argument("--question", required=True)
    parser.add_argument("--scratch-root", required=True, type=Path)
    parser.add_argument("--claude-exe", required=True, type=Path)
    parser.add_argument("--claude-config-dir", required=True, type=Path)
    args = parser.parse_args(argv)
    return DispatchRequest(
        repo=args.repo,
        ref=args.ref,
        paths=tuple(args.paths),
        question=args.question,
        scratch_root=args.scratch_root,
        claude_exe=args.claude_exe,
        claude_config_dir=args.claude_config_dir,
    )


def main(argv: list[str] | None = None) -> int:
    try:
        telemetry = dispatch(parse_args(argv or sys.argv[1:]))
    except DispatchError as exc:
        print(f"C9A_DISPATCH=FAIL\nREASON={exc}", file=sys.stderr)
        return 1
    print("C9A_DISPATCH=PASS")
    print(json.dumps(telemetry, indent=2, sort_keys=True))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
