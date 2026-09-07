#!/usr/bin/env python3
"""Deterministic repository-law checks for objective Ensemble hygiene invariants."""

from __future__ import annotations

import xml.etree.ElementTree as ET
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]

EXPECTED_PROJECT_REFERENCES: dict[str, set[str]] = {
    "src/Ensemble.E0.Core/Ensemble.E0.Core.csproj": set(),
    "src/Ensemble.E0.Harness/Ensemble.E0.Harness.csproj": {
        "src/Ensemble.E0.Core/Ensemble.E0.Core.csproj",
    },
    "tests/Ensemble.E0.Core.Tests/Ensemble.E0.Core.Tests.csproj": {
        "src/Ensemble.E0.Core/Ensemble.E0.Core.csproj",
    },
    "tests/Ensemble.E0.Harness.Tests/Ensemble.E0.Harness.Tests.csproj": {
        "src/Ensemble.E0.Core/Ensemble.E0.Core.csproj",
        "src/Ensemble.E0.Harness/Ensemble.E0.Harness.csproj",
    },
}

FORBIDDEN_PROVIDER_TOKENS = (
    "openai_api_key",
    "api.openai.com",
    "openairesponsesport",
)

TEXT_SUFFIXES = {
    ".cs",
    ".csproj",
    ".json",
    ".props",
    ".targets",
    ".config",
}


def rel(path: Path) -> str:
    return path.resolve().relative_to(ROOT).as_posix()


def read_xml(path: Path) -> ET.Element:
    return ET.parse(path).getroot()


def property_value(root: ET.Element, name: str) -> str | None:
    for element in root.iter():
        if element.tag.rsplit("}", 1)[-1] == name:
            return element.text.strip() if element.text else ""
    return None


def normalized_project_reference(project_path: Path, include: str) -> str:
    include_path = Path(include.replace("\\", "/"))
    return rel(project_path.parent / include_path)


def check_project_graph(errors: list[str]) -> None:
    discovered = {
        rel(path)
        for base in (ROOT / "src", ROOT / "tests")
        for path in base.rglob("*.csproj")
    }
    expected = set(EXPECTED_PROJECT_REFERENCES)

    for path in sorted(discovered - expected):
        errors.append(
            f"unclassified project {path}; update repository-law-check.py intentionally "
            "when adding a project"
        )
    for path in sorted(expected - discovered):
        errors.append(f"required project missing: {path}")

    for project, expected_refs in EXPECTED_PROJECT_REFERENCES.items():
        project_path = ROOT / project
        if not project_path.exists():
            continue
        root = read_xml(project_path)
        actual_refs = {
            normalized_project_reference(project_path, element.attrib["Include"])
            for element in root.iter()
            if element.tag.rsplit("}", 1)[-1] == "ProjectReference"
            and "Include" in element.attrib
        }
        if actual_refs != expected_refs:
            errors.append(
                f"project dependency mismatch for {project}: "
                f"expected={sorted(expected_refs)} actual={sorted(actual_refs)}"
            )

    harness = ROOT / "src/Ensemble.E0.Harness/Ensemble.E0.Harness.csproj"
    if harness.exists():
        root = read_xml(harness)
        if property_value(root, "RuntimeIdentifier") != "win-arm64":
            errors.append("Harness RuntimeIdentifier must remain win-arm64")
        if property_value(root, "PlatformTarget") != "ARM64":
            errors.append("Harness PlatformTarget must remain ARM64")


def check_global_build_hygiene(errors: list[str]) -> None:
    props = ROOT / "Directory.Build.props"
    if not props.exists():
        errors.append("Directory.Build.props is missing")
        return
    root = read_xml(props)
    warnings_as_errors = (property_value(root, "TreatWarningsAsErrors") or "").lower()
    deterministic = (property_value(root, "Deterministic") or "").lower()
    if warnings_as_errors != "true":
        errors.append("TreatWarningsAsErrors must remain true in Directory.Build.props")
    if deterministic != "true":
        errors.append("Deterministic must remain true in Directory.Build.props")


def check_current_state_cap(errors: list[str]) -> None:
    path = ROOT / "CURRENT_STATE.md"
    raw = path.read_bytes()
    text = raw.decode("utf-8")
    if len(raw) > 3 * 1024:
        errors.append(f"CURRENT_STATE.md exceeds 3 KiB: {len(raw)} bytes")
    lines = len(text.splitlines())
    if lines > 80:
        errors.append(f"CURRENT_STATE.md exceeds 80 lines: {lines}")


def check_retired_provider_surface(errors: list[str]) -> None:
    for base_name in ("src", "tests"):
        base = ROOT / base_name
        for path in sorted(base.rglob("*")):
            if not path.is_file():
                continue
            relative = path.relative_to(ROOT)
            if any("openai" in part.lower() for part in relative.parts):
                errors.append(f"retired OpenAI executable/test path reintroduced: {relative.as_posix()}")
            if path.suffix.lower() not in TEXT_SUFFIXES:
                continue
            try:
                text = path.read_text(encoding="utf-8").lower()
            except UnicodeDecodeError:
                continue
            for token in FORBIDDEN_PROVIDER_TOKENS:
                if token in text:
                    errors.append(
                        f"retired OpenAI executable/test token {token!r} found in "
                        f"{relative.as_posix()}"
                    )


def check_dead_scaffolding(errors: list[str]) -> None:
    for path in sorted((ROOT / "src").rglob("*.cs")):
        text = path.read_text(encoding="utf-8")
        if "NotImplementedException" in text:
            errors.append(f"dead implementation scaffolding found: {rel(path)}")


def check_handoff_authority(errors: list[str]) -> None:
    current_state = (ROOT / "CURRENT_STATE.md").read_text(encoding="utf-8")
    handoff_dir = ROOT / "docs/handoff"
    if not handoff_dir.exists():
        return
    for path in sorted(handoff_dir.rglob("*")):
        if not path.is_file() or path.name == "README.md":
            continue
        path_text = rel(path)
        if path_text not in current_state:
            errors.append(
                f"active handoff lacks CURRENT_STATE.md authority: {path_text}"
            )


def main() -> int:
    errors: list[str] = []
    check_project_graph(errors)
    check_global_build_hygiene(errors)
    check_current_state_cap(errors)
    check_retired_provider_surface(errors)
    check_dead_scaffolding(errors)
    check_handoff_authority(errors)

    if errors:
        print("REPOSITORY_LAW_CHECK=FAIL")
        for error in errors:
            print(f"ERROR\t{error}")
        return 1

    print("REPOSITORY_LAW_CHECK=PASS")
    print(f"PROJECTS_CLASSIFIED={len(EXPECTED_PROJECT_REFERENCES)}")
    print("CURRENT_STATE_CAP=PASS")
    print("PROJECT_DEPENDENCY_DIRECTION=PASS")
    print("ARM64_HARNESS_IDENTITY=PASS")
    print("WARNINGS_AND_DETERMINISM=PASS")
    print("RETIRED_OPENAI_EXECUTABLE_SURFACE=PASS")
    print("DEAD_IMPLEMENTATION_SCAFFOLDING=PASS")
    print("LIVE_HANDOFF_AUTHORITY=PASS")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
