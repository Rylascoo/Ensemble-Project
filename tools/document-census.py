#!/usr/bin/env python3
"""Deterministic documentation inbound-reference census for repository hygiene."""

from __future__ import annotations

import argparse
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
REFERENCE_EXCLUDED = GENERATED_OUTPUTS | {
    "docs/evidence/ORACLE_INDEX.md",
}
ARCHIVE_PREFIXES = ("docs/evidence/archive/",)


def git(*args: str) -> str:
    return subprocess.check_output(
        ["git", *args],
        cwd=ROOT,
        text=True,
        encoding="utf-8",
        errors="strict",
    ).strip()


def tracked_paths() -> list[str]:
    output = subprocess.check_output(["git", "ls-files", "-z"], cwd=ROOT)
    return [item.decode("utf-8") for item in output.split(b"\0") if item]


def inventory(paths: list[str]) -> list[str]:
    wanted = []
    for path in paths:
        if path in {"README.md", "CURRENT_STATE.md"} or path.startswith("docs/"):
            if path not in GENERATED_OUTPUTS:
                wanted.append(path)
    return sorted(wanted)


def is_archive(path: str) -> bool:
    return path.startswith(ARCHIVE_PREFIXES)


def read_text(path: str) -> str | None:
    try:
        return (ROOT / path).read_text(encoding="utf-8")
    except (UnicodeDecodeError, OSError):
        return None


def last_commit(path: str) -> str:
    return git("log", "-1", "--format=%H", "--", path)


def split_sources(sources: list[str]) -> tuple[list[str], list[str]]:
    active = [source for source in sources if not is_archive(source)]
    archive = [source for source in sources if is_archive(source)]
    return active, archive


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

    basenames: dict[str, int] = {}
    for path in docs:
        name = Path(path).name
        basenames[name] = basenames.get(name, 0) + 1

    entries = []
    for path in docs:
        full_sources = sorted(
            source
            for source, text in text_sources.items()
            if source != path and path in text
        )
        active_full_sources, archive_full_sources = split_sources(full_sources)

        name = Path(path).name
        basename_sources: list[str] = []
        if basenames[name] == 1:
            basename_sources = sorted(
                source
                for source, text in text_sources.items()
                if source != path and name in text and source not in full_sources
            )
        active_basename_sources, archive_basename_sources = split_sources(basename_sources)

        raw = (ROOT / path).read_bytes()
        text = raw.decode("utf-8")
        entries.append(
            {
                "path": path,
                "surface": "archive" if is_archive(path) else "active",
                "bytes": len(raw),
                "lines": len(text.splitlines()),
                "last_commit": last_commit(path),
                "reference_source_excluded": path in REFERENCE_EXCLUDED,
                "inbound_exact_path_count": len(full_sources),
                "inbound_exact_path_sources": full_sources,
                "inbound_active_exact_path_count": len(active_full_sources),
                "inbound_active_exact_path_sources": active_full_sources,
                "inbound_archive_exact_path_count": len(archive_full_sources),
                "inbound_archive_exact_path_sources": archive_full_sources,
                "inbound_unique_basename_count": len(basename_sources),
                "inbound_unique_basename_sources": basename_sources,
                "inbound_active_unique_basename_count": len(active_basename_sources),
                "inbound_active_unique_basename_sources": active_basename_sources,
                "inbound_archive_unique_basename_count": len(archive_basename_sources),
                "inbound_archive_unique_basename_sources": archive_basename_sources,
            }
        )

    active = [entry for entry in entries if entry["surface"] == "active"]
    archive = [entry for entry in entries if entry["surface"] == "archive"]
    return {
        "schema": "ensemble.repository-document-census.v3",
        "head": git("rev-parse", "HEAD"),
        "inventory_count": len(entries),
        "active_inventory_count": len(active),
        "archive_inventory_count": len(archive),
        "reference_excluded_sources": sorted(REFERENCE_EXCLUDED),
        "entries": entries,
    }


def is_archive_candidate(entry: dict[str, object]) -> bool:
    return (
        entry["surface"] == "active"
        and entry["inbound_active_exact_path_count"] == 0
        and entry["inbound_active_unique_basename_count"] == 0
    )


def print_summary(report: dict[str, object]) -> None:
    entries = report["entries"]
    assert isinstance(entries, list)
    active = [entry for entry in entries if entry["surface"] == "active"]
    archive = [entry for entry in entries if entry["surface"] == "archive"]
    candidates = [entry for entry in active if is_archive_candidate(entry)]
    print(f"DOCUMENT_CENSUS_HEAD\t{report['head']}")
    print(f"DOCUMENT_CENSUS_SCHEMA\t{report['schema']}")
    print(f"DOCUMENT_CENSUS_INVENTORY\t{report['inventory_count']}")
    print(f"DOCUMENT_CENSUS_ACTIVE\t{len(active)}")
    print(f"DOCUMENT_CENSUS_ARCHIVE\t{len(archive)}")
    print(f"DOCUMENT_CENSUS_ARCHIVE_CANDIDATES\t{len(candidates)}")
    for entry in active:
        path = str(entry["path"])
        if path.startswith("docs/evidence/"):
            active_sources = ",".join(entry["inbound_active_exact_path_sources"])
            archive_sources = ",".join(entry["inbound_archive_exact_path_sources"])
            print(
                "ACTIVE_EVIDENCE_CENSUS\t"
                f"{entry['inbound_active_exact_path_count']}\t"
                f"{entry['inbound_active_unique_basename_count']}\t"
                f"{entry['inbound_archive_exact_path_count']}\t"
                f"{path}\t{active_sources}\t{archive_sources}"
            )
    for entry in candidates:
        print(f"ARCHIVE_CANDIDATE\t{entry['path']}")
    for entry in archive:
        print(f"ARCHIVE_DOCUMENT\t{entry['path']}")


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--json", action="store_true", help="emit the complete JSON report")
    parser.add_argument("--summary", action="store_true", help="emit concise census lines")
    args = parser.parse_args()

    report = build()
    if args.json:
        print(json.dumps(report, indent=2, sort_keys=True))
    else:
        print_summary(report)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
