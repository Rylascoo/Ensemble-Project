# CI authority

The validation ladder keeps distinct evidence levels: static review < compiler < native Windows ARM64 runtime < hardware/NPU < WACK < Partner Center.

Job 1, **Compiler gate (ARM64 cross-compile)**, IS compiler authority and may be cited as `win-arm64 cross-compile/build PASS` when successful. It builds all four projects in Release on Linux x64 while preserving the Harness project's pinned `RuntimeIdentifier=win-arm64` and `PlatformTarget=ARM64`. A successful run establishes restore/compile success for that target only; it does not establish target-device execution, Windows runtime behavior, hardware/NPU execution, packaging/WACK, or Store certification.

Job 2, **Core tests (required x64 regression, non-authoritative)**, is a required semantic regression gate on Linux x64. A failure blocks the workflow; success means only `Core did not regress on x64`. It is not native Windows ARM64 runtime evidence.

Job 3, **Repository law enforcement**, runs `tools/repository-law-check.py`. It deterministically enforces the classified four-project dependency graph, Harness ARM64 project identity, warnings/determinism build settings, the `CURRENT_STATE.md` compact cap, retirement of obsolete OpenAI executable/test artifacts, absence of `NotImplementedException` source scaffolding, and CURRENT_STATE authority for any live handoff. It is repository-integrity evidence only.

Job 4, **Oracle assertion coverage**, rejects loss of previously documented+asserted 64-hex oracle coverage against the first parent and push/PR baseline. It does not maintain a committed path-sensitive oracle index and is repository-integrity evidence only.

Job 5, **Document authority census**, runs `tools/document-census.py --summary --check`; active evidence must be reachable from `CURRENT_STATE.md` or `docs/PROJECT_AUTHORITY.md`. Navigation indexes and archived evidence cannot confer reachability. It is repository-integrity evidence only.

## Reviewed action revisions

Reviewed 2026-09-07 from official GitHub release/tag metadata and pinned by exact commit:

- `actions/checkout` v7.0.1 -> `3d3c42e5aac5ba805825da76410c181273ba90b1` (`node24`)
- `actions/setup-dotnet` v6.0.0 -> `a98b56852c35b8e3190ac28c8c2271da59106c68` (`node24`)
- `actions/setup-python` v7.0.0 -> `5fda3b95a4ea91299a34e894583c3862153e4b97` (`node24`)

The .NET SDK remains explicitly `9.0.317`; action-runtime modernization does not authorize a framework/toolchain generation change. Re-review upstream release/runtime metadata before changing these action pins.

Target execution of the pinned Harness remains a native Windows ARM64 validation rung. None of the repository-integrity or Linux x64 jobs substitutes for native execution evidence.
