# CI authority

The validation ladder is static < compiler < native ARM64 runtime < WACK < Partner Center.

Job 1, **Compiler gate (ARM64 cross-compile)**, IS compiler authority and may be cited as such. It builds all four projects in Release on Linux x64, preserving the Harness project's win-arm64 RuntimeIdentifier and ARM64 PlatformTarget. The Harness cannot be executed off an ARM64 Windows host; this does not diminish compiler authority.

Job 2, **Core tests (advisory, x64, non-authoritative)**, is x64 execution and sits BELOW the first rung. It means only "Core did not regress on x64" and may never be cited in CURRENT_STATE.md, an evidence file, or any promotion decision. Its advisory result does not block the compiler gate.

Neither job substitutes for native ARM64 validation.
