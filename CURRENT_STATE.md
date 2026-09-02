# Ensemble Current State

Updated: 2026-09-01

## Authority
- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law for implementation quality and cleanup unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

## Validated baseline
H1 Patch 0002.1 + Patch 0002.1a are merged to `main` through PRs #3 and #4.

The baseline includes typed E0 Fixture Dialect v1, immutable generic fixture validation, strict JSON/text boundaries, structural Character ownership, fixture-global record/reference validation, and an acyclic provenance DAG invariant enforced at `ValidatedFixture` construction.

## Latest machine validation
Patch 0002.1a tested executable implementation head: `365537456f5890f3e2766abeb833f974c8fd7d8e`.

- Native Windows ARM64 compiler gate: PASS — Core and Harness built successfully; Harness output targeted `net9.0\win-arm64`.
- Core tests: PASS — 31 total, 31 succeeded, 0 failed, 0 skipped.
- Generic fixture runtime regression: PASS — `ensemble.e0.smoke@0.1.0` validated; exit code `0`.
- Final hygiene/scope comparison: PASS.
- Detailed evidence: `docs/evidence/H1_PATCH_0002_1A_ARM64_VALIDATION.md`.
- Evidence/checkpoint commits after the tested head do not alter executable source.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine is test-execution authority for the tests actually exercised.
- Actual target-device execution: runtime authority for the behavior actually exercised.

## Explicitly unvalidated / excluded
No Missing Raft-specific semantic validation, ECJ-1, SHA-256 fixture hashing, deterministic Access Control, Context Composer, Production state construction, persistence, causal commits, provider/AI behavior, Windows AI/NPU execution, WinUI, packaging, WACK, or Store validation exists yet.

## Immediate next action
Design H1 Patch 0002.2: canonical Missing Raft fixture plus experiment-specific structural validation. Compare every addition against the validated generic fixture baseline. Keep ECJ-1/hash and Access Control deferred until Patch 0002.2 is separately reviewed and machine-validated.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
