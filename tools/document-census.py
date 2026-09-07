#!/usr/bin/env python3
"""Deterministic documentation inbound-reference census for repository hygiene."""

from __future__ import annotations

import argparse
import json
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
GENERATED = {
    "docs/DOCUMENT_CENSUS.md",
    "docs/DOCUMENT_CENSUS.json",
}


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
            if path not in GENERATED:
                wanted.append(path)
    return sorted(wanted)


def read_text(path: str) -> str | None:
    try:
        return (ROOT / path).read_text(encoding="utf-8")
    except (UnicodeDecodeError, OSError):
        return None


def last_commit(path: str) -> str:
    return git("log", "-1", "--format=%H", "--", path)


def build() -> dict[str, object]:
    tracked = tracked_paths()
    docs = inventory(tracked)
    text_sources: dict[str, str] = {}
    for source in tracked:
        if source in GENERATED:
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
        name = Path(path).name
        basename_sources: list[str] = []
        if basenames[name] == 1:
            basename_sources = sorted(
                source
                for source, text in text_sources.items()
                if source != path and name in text and source not in full_sources
            )
        raw = (ROOT / path).read_bytes()
        text = raw.decode("utf-8")
        entries.append(
            {
                "path": path,
                "bytes": len(raw),
                "lines": len(text.splitlines()),
                "last_commit": last_commit(path),
                "inbound_exact_path_count": len(full_sources),
                "inbound_exact_path_sources": full_sources,
                "inbound_unique_basename_count": len(basename_sources),
                "inbound_unique_basename_sources": basename_sources,
            }
        )

    return {
        "schema": "ensemble.repository-document-census.v1",
        "head": git("rev-parse", "HEAD"),
        "inventory_count": len(entries),
        "entries": entries,
    }


def print_summary(report: dict[str, object]) -> None:
    entries = report["entries"]
    assert isinstance(entries, list)
    print(f"DOCUMENT_CENSUS_HEAD\t{report['head']}")
    print(f"DOCUMENT_CENSUS_INVENTORY\t{report['inventory_count']}")
    zero_exact = [entry for entry in entries if entry["inbound_exact_path_count"] == 0]
    print(f"DOCUMENT_CENSUS_ZERO_EXACT\t{len(zero_exact)}")
    for entry in entries:
        path = str(entry["path"])
        if path.startswith("docs/evidence/"):
            sources = ",".join(entry["inbound_exact_path_sources"])
            print(
                "EVIDENCE_CENSUS\t"
                f"{entry['inbound_exact_path_count']}\t"
                f"{entry['inbound_unique_basename_count']}\t"
                f"{path}\t{sources}"
            )
    for entry in zero_exact:
        print(f"ZERO_EXACT\t{entry['path']}")


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
