# Ensemble Current State

Updated: 2026-09-02

## Authority
- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- H1 Patch 0002.2 Missing Raft Fixture Contract Blueprint 0.4 is canonical for the Missing Raft fixture subset.
- H1 Patch 0003 ECJ-1 Canonical Fixture Identity Blueprint 0.2 is APPROVED and canonical.
- H1 Patch 0004 Deterministic Character-Bounded Access Control Blueprint 0.2 is APPROVED and canonical.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

H1 Patch 0003 is COMPLETE, machine-validated for its exercised gates, and promoted to `main` through PR #8.

H1 Patch 0004 Blueprint 0.2 is explicitly approved and promoted to `main` through PR #9. Patch 0004 executable implementation has not yet been machine-validated.

## Validated executable baseline
Latest machine-tested executable implementation head:
`c55eb022983a3954a78c8386a8c83ce8d5f4f2a7`

Validated on the user's native Windows ARM64 machine:
- Native Windows ARM64 compiler gate: PASS — Core and Harness built successfully; Harness targeted `net9.0\win-arm64`.
- Core tests: PASS — 73/73, 0 failed, 0 skipped.
- Canonical Missing Raft runtime with ECJ-1/SHA-256 enforcement: PASS; exit `0`.
- Generic smoke runtime regression: PASS; exit `0`.
- Independent ECJ-1 digest review: PASS advisory.
- Final static/hygiene/scope review: PASS advisory.

Detailed evidence:
`docs/evidence/H1_PATCH_0003_ARM64_VALIDATION.md`

Documentation-only blueprint/checkpoint commits after `c55eb…` do not increase executable validation authority.

## Patch 0003 frozen identity
Canonical specification:
`docs/blueprint/H1_PATCH_0003_ECJ1_FIXTURE_HASH.md`

Missing Raft 0.1.0 frozen ECJ-1 reference:
- canonical UTF-8 byte length: `9112`;
- SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

The canonical Missing Raft source JSON remains unchanged.

## Current Patch 0004 specification
Canonical specification:
`docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`

Approved law includes:
1. `Production State -> deterministic Access Control -> permitted Character information -> Context Composer -> bounded context -> Performer` remains the required authority order;
2. `ensemble.e0.character-bounded.v1` is implemented as one concrete deterministic contract, not an ACL/policy framework;
3. Character projections deny Production `HistoricalTruth`, `UnresolvedProposition`, `WorldState`, and chronology;
4. Scene identity/roster identities, all `SceneState`, and root `Pressure` are shared with the Scene roster;
5. each Character receives only their own Constitution, Disposition, Circumstance, Observation, Knowledge, Belief, Suspicion, Memory, Goal, and outbound Relationships;
6. every other Character's private state and inbound Relationships remain denied;
7. Character-facing projection records strip provenance entirely so denied source IDs/content cannot leak;
8. Access Control returns the maximal permitted set; relevance, token budgeting, summarization, and prompt construction belong to later Context Composer work;
9. a deterministic permit/deny audit is retained locally but must remain separate from the Context Composer/Performer-facing surface;
10. projection/audit collections sort ordinally by stable ID;
11. initial opportunity/Director state is outside the Access Control projection;
12. E0-D omniscient context remains an explicitly labeled experimental ablation outside the safe reference Access Control operation;
13. Access Control must not depend on Missing-Raft-specific policy logic;
14. no Context Composer, persistence, provider/AI, Windows AI/NPU, WinUI, packaging, WACK, or Store work belongs in Patch 0004.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine: test-execution authority for tests actually exercised.
- Actual target-device execution: runtime authority for behavior actually exercised.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Explicitly unvalidated / excluded
No Patch 0004 executable validation exists yet.

No Context Composer or ContextPacketHash, StateHash, ProductionState construction/persistence, accepted-history/causal commits, Performer/Director/Integrity Validator/State Interpreter runtime orchestration, provider/AI behavior, Windows AI/NPU execution, WinUI, packaging, WACK, or Store validation exists yet.

Access Control will be deterministic information authority only. It does not itself prove model behavior, context quality, runtime orchestration, or product UX.

## Immediate next action
Implement H1 Patch 0004 only from current `main` on a dedicated implementation branch, following:
`docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`

Implementation order:
1. add the smallest immutable Character-safe projection types;
2. add one concrete `CharacterBoundedAccessControl` operation for `ensemble.e0.character-bounded.v1`;
3. derive access only from category, structural ownership, roster, and contract — never provenance traversal;
4. produce a separate deterministic local permit/deny audit;
5. add exact generic + Missing Raft access-set, provenance-leak, ordering, and fail-closed tests;
6. verify Access Control does not mutate the fixture or change the frozen Missing Raft hash;
7. run static/adversarial/hygiene review;
8. request native Windows ARM64 build, full tests, Missing Raft runtime regression, and generic smoke runtime regression;
9. promote only after all exercised gates pass.

Do not enter Context Composer or create an omniscient/debug bypass during Patch 0004 implementation.

## Continuity
Fresh chats read this file first.

For Patch 0004 implementation, then read:
- `docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`;
- `src/Ensemble.E0.Core/Fixture/ValidatedFixture.cs`;
- only directly affected Core/test files.

For fixture/hash authority, read only as needed:
- `docs/blueprint/H1_PATCH_0003_ECJ1_FIXTURE_HASH.md`;
- `docs/blueprint/H1_PATCH_0002_2_MISSING_RAFT_CONTRACT.md`.

Do not reconstruct already-approved project state from chat history when GitHub contains it.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
