#!/usr/bin/env python3
"""Deterministic documentation authority/reference census for repository hygiene."""

from __future__ import annotations

import argparse
import collections
import fnmatch
import json
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
GENERATED_OUTPUTS = {"docs/DOCUMENT_CENSUS.md", "docs/DOCUMENT_CENSUS.json", "docs/DOCUMENT_INDEX.md", "docs/DOCUMENT_INDEX.json"}
REFERENCE_EXCLUDED = GENERATED_OUTPUTS
AUTHORITY_ROOTS = ("CURRENT_STATE.md", "docs/PROJECT_AUTHORITY.md")
ARCHIVE_PREFIXES = ("docs/evidence/archive/",)
HISTORICAL_GLOBS = ("docs/BRANCH_ARCHIVE_*.md", "docs/blueprint/H1_PATCH_*.md")


def git(*args: str) -> str:
    return subprocess.check_output(["git", *args], cwd=ROOT, text=True, encoding="utf-8", errors="strict").strip()


def tracked_paths() -> list[str]:
    raw = subprocess.check_output(["git", "ls-files", "-z"], cwd=ROOT)
    return [item.decode("utf-8") for item in raw.split(b"\0") if item]


def inventory(paths: list[str]) -> list[str]:
    return sorted(
        path for path in paths
        if (path in {"README.md", "CURRENT_STATE.md", "AGENTS.md"} or path.startswith("docs/"))
        and path not in GENERATED_OUTPUTS
    )


def is_archive(path: str) -> bool:
    return path.startswith(ARCHIVE_PREFIXES)


def is_historical(path: str) -> bool:
    return any(fnmatch.fnmatch(path, pattern) for pattern in HISTORICAL_GLOBS)


def surface_for(path: str) -> str:
    if is_archive(path):
        return "archive"
    if is_historical(path):
        return "historical"
    return "current"


def is_current_evidence(path: str) -> bool:
    return path.startswith("docs/evidence/") and surface_for(path) == "current"


def read_text(path: str) -> str | None:
    try:
        return (ROOT / path).read_text(encoding="utf-8")
    except (UnicodeDecodeError, OSError):
        return None


def last_commit(path: str) -> str:
    return git("log", "-1", "--format=%H", "--", path)


def references_for(source: str, text: str, current_docs: set[str], basenames: dict[str, int]) -> set[str]:
    if source in REFERENCE_EXCLUDED:
        return set()
    found: set[str] = set()
    for target in current_docs:
        if source == target:
            continue
        if target in text:
            found.add(target)
            continue
        name = Path(target).name
        if basenames.get(name) == 1 and name in text:
            found.add(target)
    return found


def authority_graph(docs: list[str], text_sources: dict[str, str], basenames: dict[str, int]) -> tuple[dict[str, set[str]], dict[str, str | None]]:
    current_docs = {path for path in docs if surface_for(path) == "current"}
    edges: dict[str, set[str]] = {path: set() for path in current_docs}
    for source in current_docs:
        text = text_sources.get(source)
        if text is not None:
            edges[source] = references_for(source, text, current_docs, basenames)
    parent: dict[str, str | None] = {}
    queue: collections.deque[str] = collections.deque()
    for root in AUTHORITY_ROOTS:
        if root not in current_docs:
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
        full_sources = sorted(source for source, text in text_sources.items() if source != path and path in text)
        name = Path(path).name
        basename_sources: list[str] = []
        if basenames[name] == 1:
            basename_sources = sorted(source for source, text in text_sources.items() if source != path and name in text and source not in full_sources)
        raw = (ROOT / path).read_bytes()
        entries.append({
            "path": path,
            "surface": surface_for(path),
            "bytes": len(raw),
            "lines": len(raw.decode("utf-8").splitlines()),
            "last_commit": last_commit(path),
            "authority_reachable": path in reachable_parent,
            "authority_path": authority_path(path, reachable_parent),
            "outbound_current_document_references": sorted(edges.get(path, set())),
            "inbound_exact_path_sources": full_sources,
            "inbound_unique_basename_sources": basename_sources,
        })
    return {
        "schema": "ensemble.repository-document-census.v8",
        "head": git("rev-parse", "HEAD"),
        "authority_roots": list(AUTHORITY_ROOTS),
        "inventory_count": len(entries),
        "current_inventory_count": sum(1 for e in entries if e["surface"] == "current"),
        "historical_inventory_count": sum(1 for e in entries if e["surface"] == "historical"),
        "archive_inventory_count": sum(1 for e in entries if e["surface"] == "archive"),
        "entries": entries,
    }


def unexplained_current(report: dict[str, object]) -> list[dict[str, object]]:
    entries = report["entries"]
    assert isinstance(entries, list)
    return [entry for entry in entries if entry["surface"] == "current" and not entry["authority_reachable"]]


def print_summary(report: dict[str, object]) -> None:
    entries = report["entries"]
    assert isinstance(entries, list)
    unexplained = unexplained_current(report)
    print(f"DOCUMENT_CENSUS_HEAD\t{report['head']}")
    print(f"DOCUMENT_CENSUS_SCHEMA\t{report['schema']}")
    print(f"DOCUMENT_CENSUS_AUTHORITY_ROOTS\t{','.join(report['authority_roots'])}")
    print(f"DOCUMENT_CENSUS_INVENTORY\t{report['inventory_count']}")
    print(f"DOCUMENT_CENSUS_CURRENT\t{report['current_inventory_count']}")
    print(f"DOCUMENT_CENSUS_HISTORICAL\t{report['historical_inventory_count']}")
    print(f"DOCUMENT_CENSUS_ARCHIVE\t{report['archive_inventory_count']}")
    print(f"DOCUMENT_CENSUS_UNEXPLAINED_CURRENT\t{len(unexplained)}")
    for entry in entries:
        if is_current_evidence(str(entry["path"])):
            chain = " -> ".join(entry["authority_path"])
            print(f"CURRENT_EVIDENCE_AUTHORITY\t{1 if entry['authority_reachable'] else 0}\t{entry['path']}\t{chain}")
    for entry in unexplained:
        print(f"UNEXPLAINED_CURRENT_DOCUMENT\t{entry['path']}")


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--json", action="store_true")
    parser.add_argument("--summary", action="store_true")
    parser.add_argument("--check", action="store_true", help="fail when any current document is unreachable from durable authority roots")
    args = parser.parse_args()
    report = build()
    if args.json:
        print(json.dumps(report, indent=2, sort_keys=True))
    else:
        print_summary(report)
    if args.check:
        unexplained = unexplained_current(report)
        if unexplained:
            print("DOCUMENT_AUTHORITY_CHECK=FAIL")
            for entry in unexplained:
                print(f"ERROR\tunexplained current document: {entry['path']}")
            return 1
        print("DOCUMENT_AUTHORITY_CHECK=PASS")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
