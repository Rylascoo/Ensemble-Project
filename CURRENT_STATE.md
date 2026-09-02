# Ensemble Current State

Updated: 2026-09-01

## Authority
- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- H1 Patch 0002.2 Missing Raft Fixture Contract Blueprint 0.4 is explicitly approved as the canonical GitHub implementation specification for the Missing Raft fixture subset.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law for implementation quality and cleanup unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

## Validated executable baseline
H1 Patch 0002.1 + Patch 0002.1a are merged to `main` through PRs #3 and #4.

The baseline includes typed E0 Fixture Dialect v1, immutable generic fixture validation, strict JSON/text boundaries, structural Character ownership, fixture-global record/reference validation, and an acyclic provenance DAG invariant enforced at `ValidatedFixture` construction.

Latest machine-validated executable head: `365537456f5890f3e2766abeb833f974c8fd7d8e`.

- Native Windows ARM64 compiler gate: PASS — Core and Harness built successfully; Harness output targeted `net9.0\win-arm64`.
- Core tests: PASS — 31 total, 31 succeeded, 0 failed, 0 skipped.
- Generic fixture runtime regression: PASS — `ensemble.e0.smoke@0.1.0` validated; exit code `0`.
- Final hygiene/scope comparison: PASS.
- Detailed evidence: `docs/evidence/H1_PATCH_0002_1A_ARM64_VALIDATION.md`.

## Approved Patch 0002.2 specification
PR #5 `H1: define Missing Raft fixture contract` was promoted to `main` as a blueprint-only fast-forward. No executable source changed in that promotion.

Canonical specification:
`docs/blueprint/H1_PATCH_0002_2_MISSING_RAFT_CONTRACT.md`

Fresh-chat implementation handoff:
`docs/handoff/E0A_H1_PATCH_0002_2_IMPLEMENTATION_HANDOFF.md`

Key implementation laws:
1. preserve E0 Fixture Dialect v1 and generic validation unless implementation proves an impossibility;
2. use `MissingRaftContract.Validate(ValidatedFixture)` without a second fixture/domain representation;
3. keep exact Missing Raft fixture/Scene/Character/record IDs, authority categories, ownership, chronology, relationship targets, and frozen provenance explicit;
4. provenance explains evidence/derivation and never grants access;
5. one canonical fixture source: `fixtures/missing-raft/missing-raft-0.1.0.json`;
6. relationship omission for E0-D is later context exclusion over the same frozen fixture, not fixture mutation;
7. prose is source-reviewed in Patch 0002.2 and becomes cryptographically bound only in Patch 0003;
8. no generic registry/plugin/causal/ablation framework is earned yet.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine is test-execution authority for the tests actually exercised.
- Actual target-device execution: runtime authority for the behavior actually exercised.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Explicitly unvalidated / excluded
Patch 0002.2 executable implementation does not exist yet. No canonical Missing Raft runtime validation, ECJ-1, SHA-256 fixture hashing, deterministic Access Control, Context Composer, Production state construction, persistence, causal commits, provider/AI behavior, Windows AI/NPU execution, WinUI, packaging, WACK, or Store validation exists yet.

## Immediate next action
Begin H1 Patch 0002.2 implementation from current `main` on a new dedicated branch.

Implementation order:
1. author the one canonical Missing Raft JSON fixture exactly from the approved contract;
2. perform source-content review against the approved semantic meanings before calling it canonical;
3. add `MissingRaftContract` stable IDs/sets and fail-closed structural validation over `ValidatedFixture`;
4. add the smallest Harness family dispatch;
5. extend the existing Core test project with mutation-style contract tests;
6. run static + engineering-hygiene comparison against the validated Patch 0002.1a baseline;
7. request target Windows ARM64 build, full Core test, Missing Raft runtime, and generic smoke-regression evidence from the user;
8. merge only after all gates pass.

Do not enter Patch 0003 hashing or Patch 0004 Access Control during 0002.2 correction/debugging.

## Continuity
Fresh chats read this file first, then `docs/handoff/E0A_H1_PATCH_0002_2_IMPLEMENTATION_HANDOFF.md`, then the approved Patch 0002.2 contract, then only source/test files relevant to the immediate implementation slice. Do not reconstruct already-approved project state from chat history.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
