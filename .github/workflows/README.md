# CI authority

The validation ladder keeps distinct evidence levels: static review < compiler < native Windows ARM64 runtime < hardware/NPU < WACK < Partner Center.

Job 1, **Compiler gate (ARM64 cross-compile)**, IS compiler authority and may be cited as `win-arm64 cross-compile/build PASS` when successful. It builds all four projects in Release on Linux x64 while preserving the Harness project's pinned `RuntimeIdentifier=win-arm64` and `PlatformTarget=ARM64`. A successful run establishes that the checkout restores and compiles successfully for that target. It does **not** establish process start, target-device behavior, Windows runtime behavior, hardware/NPU execution, packaging/WACK, or Store certification.

Job 2, **Core tests (required x64 regression, non-authoritative)**, is a required semantic regression gate executed on Linux x64. A failure blocks the workflow; a success means only `Core did not regress on x64`. It may never be promoted to native Windows ARM64 runtime evidence and does not substitute for, or inflate, another validation rung.

Job 3, **Oracle assertion coverage**, scans documented 64-hex oracle values and the existing lexical MSTest equality-assertion pattern, then rejects loss of previously scanned assertion coverage against the first parent and push/PR baseline. It does not maintain a committed document-path inventory, so evidence archive moves do not create meaningless oracle-path drift. It is repository-integrity evidence only; it supplies no behavioral, compiler, runtime, hardware, packaging, or Store authority.

Job 4, **Document reference census**, deterministically reports repository-document inventory, last-modifying commits, active/archive inbound references, and conservative evidence-archive candidates using `tools/document-census.py`. It supports evidence/archive hygiene and document-system audits. It is repository-integrity information only and supplies no behavioral or validation authority.

Target execution of the pinned Harness remains a native Windows ARM64 validation rung. None of these CI jobs substitutes for that execution evidence.
