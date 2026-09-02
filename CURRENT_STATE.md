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
H1 Patch 0002.1 — E0 Fixture Dialect v1 — is merged to `main` through PR #3.

## Active correction boundary
H1 Patch 0002.1a — provenance DAG correction.
Branch: `h1-patch-0002-1a-provenance-dag`
PR: #4 `H1: enforce acyclic fixture provenance`

Patch 0002.1a adds one inherited invariant only:
- `ValidatedFixture` provenance must form a DAG;
- cycle detection is iterative/topological rather than recursive;
- a regression test rejects a two-record provenance cycle.

No fixture schema, authority category, Missing Raft content, hashing, Access Control, Context Composer, persistence, provider/AI, UI, NPU, or Store surface changed.

## Patch 0002.1a validation
Tested executable implementation head: `365537456f5890f3e2766abeb833f974c8fd7d8e`.

- Native Windows ARM64 compiler gate: PASS — Core and Harness built successfully; Harness output targeted `net9.0\win-arm64`.
- Core tests: PASS — 31 total, 31 succeeded, 0 failed, 0 skipped.
- Generic fixture runtime regression: PASS — `ensemble.e0.smoke@0.1.0` validated; exit code `0`.
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
Perform the final Patch 0002.1a hygiene/scope comparison and promote it to `main` if unchanged, then design H1 Patch 0002.2: canonical Missing Raft fixture plus experiment-specific structural validation.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
