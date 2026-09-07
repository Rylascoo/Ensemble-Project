#!/usr/bin/env python3
"""Deterministic documentation authority/reference census for repository hygiene."""

from __future__ import annotations

import argparse
import collections
import json
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
GENERATED_OUTPUTS = {
    "docs/DOCUMENT_CENSUS.md",
    "docs/DOCUMENT_CENSUS.json",
    "docs/DOCUMENT_INDEX.md",
    "docs/DOCUMENT_INDEX.json",
}
REFERENCE_EXCLUDED = GENERATED_OUTPUTS
AUTHORITY_ROOTS = ("CURRENT_STATE.md", "docs/PROJECT_AUTHORITY.md")
ARCHIVE_PREFIXES = ("docs/evidence/archive/",)


def git(*args: str) -> str:
    return subprocess.check_output(
        ["git", *args], cwd=ROOT, text=True, encoding="utf-8", errors="strict"
    ).strip()


def tracked_paths() -> list[str]:
    raw = subprocess.check_output(["git", "ls-files", "-z"], cwd=ROOT)
    return [item.decode("utf-8") for item in raw.split(b"\0") if item]


def inventory(paths: list[str]) -> list[str]:
    return sorted(
        path for path in paths
        if (path in {"README.md", "CURRENT_STATE.md"} or path.startswith("docs/"))
        and path not in GENERATED_OUTPUTS
    )


def is_archive(path: str) -> bool:
    return path.startswith(ARCHIVE_PREFIXES)


def is_active_evidence(path: str) -> bool:
    return path.startswith("docs/evidence/") and not is_archive(path)


def read_text(path: str) -> str | None:
    try:
        return (ROOT / path).read_text(encoding="utf-8")
    except (UnicodeDecodeError, OSError):
        return None


def last_commit(path: str) -> str:
    return git("log", "-1", "--format=%H", "--", path)


def references_for(
    source: str,
    text: str,
    active_docs: set[str],
    basenames: dict[str, int],
) -> set[str]:
    if source in REFERENCE_EXCLUDED:
        return set()
    found: set[str] = set()
    for target in active_docs:
        if source == target:
            continue
        if target in text:
            found.add(target)
            continue
        name = Path(target).name
        if basenames.get(name) == 1 and name in text:
            found.add(target)
    return found


def authority_graph(
    docs: list[str], text_sources: dict[str, str], basenames: dict[str, int]
) -> tuple[dict[str, set[str]], dict[str, str | None]]:
    active_docs = {path for path in docs if not is_archive(path)}
    edges: dict[str, set[str]] = {path: set() for path in active_docs}
    for source in active_docs:
        text = text_sources.get(source)
        if text is not None:
            edges[source] = references_for(source, text, active_docs, basenames)

    parent: dict[str, str | None] = {}
    queue: collections.deque[str] = collections.deque()
    for root in AUTHORITY_ROOTS:
        if root not in active_docs:
            raise ValueError(f"authority root is missing: {root}")
        parent[root] = None
        queue.append(root)
    while queue:
        source = queue.popleft()
        for target in sorted(edges[source]):
            if target not in parent:
                parent[target] = source
                queue.append(target)
    return edges, parent


def authority_path(path: str, parent: dict[str, str | None]) -> list[str]:
    if path not in parent:
        return []
    result = [path]
    cursor = path
    while parent[cursor] is not None:
        cursor = parent[cursor]  # type: ignore[assignment]
        result.append(cursor)
    result.reverse()
    return result


def build() -> dict[str, object]:
    tracked = tracked_paths()
    docs = inventory(tracked)
    text_sources: dict[str, str] = {}
    for source in tracked:
        if source in REFERENCE_EXCLUDED:
            continue
        text = read_text(source)
        if text is not None:
            text_sources[source] = text

    basenames: dict[str, int] = collections.Counter(Path(path).name for path in docs)
    edges, reachable_parent = authority_graph(docs, text_sources, basenames)
    entries = []
    for path in docs:
        full_sources = sorted(
            source for source, text in text_sources.items()
            if source != path and path in text
        )
        active_full = [source for source in full_sources if not is_archive(source)]
        archive_full = [source for source in full_sources if is_archive(source)]
        name = Path(path).name
        basename_sources: list[str] = []
        if basenames[name] == 1:
            basename_sources = sorted(
                source for source, text in text_sources.items()
                if source != path and name in text and source not in full_sources
            )
        active_basename = [source for source in basename_sources if not is_archive(source)]
        archive_basename = [source for source in basename_sources if is_archive(source)]
        raw = (ROOT / path).read_bytes()
        entries.append({
            "path": path,
            "surface": "archive" if is_archive(path) else "active",
            "bytes": len(raw),
            "lines": len(raw.decode("utf-8").splitlines()),
            "last_commit": last_commit(path),
            "authority_reachable": path in reachable_parent,
            "authority_path": authority_path(path, reachable_parent),
            "outbound_active_document_references": sorted(edges.get(path, set())),
            "inbound_active_exact_path_sources": active_full,
            "inbound_archive_exact_path_sources": archive_full,
            "inbound_active_unique_basename_sources": active_basename,
            "inbound_archive_unique_basename_sources": archive_basename,
        })

    active = [entry for entry in entries if entry["surface"] == "active"]
    archive = [entry for entry in entries if entry["surface"] == "archive"]
    return {
        "schema": "ensemble.repository-document-census.v7",
        "head": git("rev-parse", "HEAD"),
        "authority_roots": list(AUTHORITY_ROOTS),
        "inventory_count": len(entries),
        "active_inventory_count": len(active),
        "archive_inventory_count": len(archive),
        "entries": entries,
    }


def is_archive_candidate(entry: dict[str, object]) -> bool:
    return is_active_evidence(str(entry["path"])) and not bool(entry["authority_reachable"])


def archive_candidates(report: dict[str, object]) -> list[dict[str, object]]:
    entries = report["entries"]
    assert isinstance(entries, list)
    return [entry for entry in entries if is_archive_candidate(entry)]


def print_summary(report: dict[str, object]) -> None:
    entries = report["entries"]
    assert isinstance(entries, list)
    active = [entry for entry in entries if entry["surface"] == "active"]
    archive = [entry for entry in entries if entry["surface"] == "archive"]
    candidates = archive_candidates(report)
    unreachable_other = [
        entry for entry in active
        if not entry["authority_reachable"] and not is_archive_candidate(entry)
    ]
    print(f"DOCUMENT_CENSUS_HEAD\t{report['head']}")
    print(f"DOCUMENT_CENSUS_SCHEMA\t{report['schema']}")
    print(f"DOCUMENT_CENSUS_AUTHORITY_ROOTS\t{','.join(report['authority_roots'])}")
    print(f"DOCUMENT_CENSUS_INVENTORY\t{report['inventory_count']}")
    print(f"DOCUMENT_CENSUS_ACTIVE\t{len(active)}")
    print(f"DOCUMENT_CENSUS_ARCHIVE\t{len(archive)}")
    print(f"DOCUMENT_CENSUS_UNREACHABLE_EVIDENCE\t{len(candidates)}")
    print(f"DOCUMENT_CENSUS_UNREACHABLE_OTHER\t{len(unreachable_other)}")
    for entry in active:
        if str(entry["path"]).startswith("docs/evidence/"):
            chain = " -> ".join(entry["authority_path"])
            print(
                "ACTIVE_EVIDENCE_AUTHORITY\t"
                f"{1 if entry['authority_reachable'] else 0}\t"
                f"{entry['path']}\t{chain}"
            )
    for entry in candidates:
        print(f"EVIDENCE_ARCHIVE_CANDIDATE\t{entry['path']}")
    for entry in unreachable_other:
        print(f"UNREACHABLE_ACTIVE_DOCUMENT\t{entry['path']}")


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--json", action="store_true")
    parser.add_argument("--summary", action="store_true")
    parser.add_argument(
        "--check",
        action="store_true",
        help="fail when active evidence is unreachable from durable authority roots",
    )
    args = parser.parse_args()
    report = build()

    if args.json:
        print(json.dumps(report, indent=2, sort_keys=True))
    else:
        print_summary(report)

    if args.check:
        candidates = archive_candidates(report)
        if candidates:
            print("DOCUMENT_AUTHORITY_CHECK=FAIL")
            for entry in candidates:
                print(f"ERROR\tunreachable active evidence: {entry['path']}")
            return 1
        print("DOCUMENT_AUTHORITY_CHECK=PASS")

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
