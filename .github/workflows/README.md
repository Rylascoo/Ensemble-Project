# CI authority

The validation ladder keeps distinct evidence levels: static review < compiler < native Windows ARM64 runtime < hardware/NPU < WACK < Partner Center.

Job 1, **Compiler gate (ARM64 cross-compile)**, IS compiler authority and may be cited as `win-arm64 cross-compile/build PASS` when successful. It builds all four projects in Release on Linux x64 while preserving the Harness project's pinned `RuntimeIdentifier=win-arm64` and `PlatformTarget=ARM64`. A successful run establishes that the checkout restores and compiles successfully for that target. It does **not** establish process start, target-device behavior, Windows runtime behavior, hardware/NPU execution, packaging/WACK, or Store certification.

Job 2, **Core tests (required x64 regression, non-authoritative)**, is a required semantic regression gate executed on Linux x64. A failure blocks the workflow; a success means only `Core did not regress on x64`. It may never be promoted to native Windows ARM64 runtime evidence and does not substitute for, or inflate, another validation rung.

Job 3, **Oracle documentation drift**, guards the generated oracle inventory and rejects lost scanned assertion coverage. It is repository-integrity evidence only; it supplies no behavioral, compiler, runtime, hardware, packaging, or Store authority.

Target execution of the pinned Harness remains a native Windows ARM64 validation rung. None of these CI jobs substitutes for that execution evidence.
