# Ensemble Current State

Updated: 2026-09-01

## Authority
- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- H1 Patch 0002.2 Missing Raft Fixture Contract Blueprint 0.4 is the canonical GitHub implementation specification for the Missing Raft fixture subset.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law for implementation quality and cleanup unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

H1 Patch 0002.2 is COMPLETE, machine-validated for its exercised gates, and promoted to `main` through PR #6.

## Validated executable baseline
The current validated executable baseline includes H1 Patch 0002.1, Patch 0002.1a, and Patch 0002.2.

Latest machine-tested executable implementation head:
`33a8a9f96d2320260619ac28309f0f40bee8e19e`

Patch 0002.2 promotion head on `main` before this checkpoint update:
`e9f61b79bc4b6bbf7272e9815acd0073e2275c8a`

The commit after the tested executable head and before promotion is documentation-only validation evidence. It does not increase the executable validation level.

Validated on the user's native Windows ARM64 machine:
- Native Windows ARM64 compiler gate: PASS — Core and Harness built successfully; Harness output targeted `net9.0\win-arm64`.
- Core tests: PASS — 62 total, 62 succeeded, 0 failed, 0 skipped.
- Canonical Missing Raft runtime: PASS — `ensemble.e0.missing-raft@0.1.0` validated; exit code `0`.
- Generic smoke runtime regression: PASS — `ensemble.e0.smoke@0.1.0` validated; exit code `0`.
- Direct Missing Raft validation rejects the generic smoke fixture through the passing Core test suite.
- Source-content review: PASS.
- Final hygiene/scope comparison: PASS.
- Detailed evidence: `docs/evidence/H1_PATCH_0002_2_ARM64_VALIDATION.md`.

## Patch 0002.2 implementation result
Canonical specification:
`docs/blueprint/H1_PATCH_0002_2_MISSING_RAFT_CONTRACT.md`

Canonical fixture source:
`fixtures/missing-raft/missing-raft-0.1.0.json`

Implemented architecture:
1. E0 Fixture Dialect v1 and generic validation remain unchanged;
2. `MissingRaftContract.Validate(ValidatedFixture)` adds fixture-specific structural law without a second fixture/domain representation;
3. exact Missing Raft fixture/Scene/Character/record IDs, authority categories, ownership, chronology, relationship targets, and frozen provenance are mechanically protected;
4. provenance remains evidence/derivation only and does not grant access;
5. Harness dispatch is explicit and minimal for the known Missing Raft family while unknown generic families remain generically valid;
6. relationship-context IDs remain a derived union for later E0-D context exclusion rather than fixture mutation;
7. source prose is reviewed in Patch 0002.2 but is not cryptographically bound yet;
8. mutation-style tests use the one canonical Missing Raft source rather than malformed-fixture copy sprawl;
9. no generic registry/plugin/factory/causal/ablation framework was introduced.

## Source-content disposition
The canonical Missing Raft source passed review against the approved semantic contract.

Confirmed:
- raft failure remains unresolved;
- the stronger current did not release the correctly secured mooring;
- Wren observed Marlowe's later shoreline return, not the release;
- Wren's suspicion remains suspicion rather than release knowledge;
- Voss's accidental-loss explanation remains plausible belief rather than Production truth;
- Marlowe's Character-owned knowledge does not gain Wren-private observation or an unsupported claim that he knew Wren correctly secured the mooring;
- no extra dramatic premise, moral winner, forced confession, accusation, revelation, reconciliation, or solution was added.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine is test-execution authority for the tests actually exercised.
- Actual target-device execution is runtime authority for the behavior actually exercised.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Explicitly unvalidated / excluded
Patch 0002.2 does not validate or implement ECJ-1 canonical serialization, SHA-256 fixture hashing, deterministic Access Control, Context Composer, Production state construction, persistence, causal commits, Performer/Director/Integrity Validator/Interpreter runtime orchestration, provider/AI behavior, Windows AI/NPU execution, WinUI, packaging, WACK, or Store behavior.

No NPU, WACK, Store, or broader product-runtime claim may be inferred from the Patch 0002.2 evidence.

## Immediate next action
Hold at the validated H1 Patch 0002.2 boundary.

Do not begin Patch 0003 hashing or Patch 0004 Access Control as part of Patch 0002.2 cleanup or correction. A subsequent chat may resume the next approved H1 slice only after reading this checkpoint and the relevant canonical specification/roadmap source for that slice.

No further Patch 0002.2 implementation work is currently required.

## Continuity
Fresh chats read this file first.

For Patch 0002.2 validation history, then read:
- `docs/evidence/H1_PATCH_0002_2_ARM64_VALIDATION.md`;
- `docs/blueprint/H1_PATCH_0002_2_MISSING_RAFT_CONTRACT.md`.

`docs/handoff/E0A_H1_PATCH_0002_2_IMPLEMENTATION_HANDOFF.md` is now a completed historical handoff, not an instruction to reimplement the patch.

Do not reconstruct already-approved project state from chat history when GitHub contains it.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
