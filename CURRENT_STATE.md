# Ensemble Current State

Updated: 2026-09-02

## Authority
- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- H1 Patch 0002.2 Missing Raft Fixture Contract Blueprint 0.4 is canonical for the Missing Raft fixture subset.
- H1 Patch 0003 ECJ-1 Canonical Fixture Identity Blueprint 0.2 is APPROVED and canonical.
- H1 Patch 0004 Deterministic Character-Bounded Access Control Blueprint 0.2 is APPROVED and canonical.
- H1 Patch 0005 Deterministic Context Composer + Dual Context Identity Blueprint 0.3 is APPROVED and canonical.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

H1 Patch 0003 is COMPLETE, machine-validated for its exercised gates, and promoted through PR #8.

H1 Patch 0004 is COMPLETE, machine-validated for its exercised gates, and promoted through PR #10.

H1 Patch 0005 Blueprint 0.3 is explicitly approved and promoted through PR #11 at `4ca5e88210209885e836b566f42602fb5bf94607`. Patch 0005 executable implementation has not yet been machine-validated.

## Validated executable baseline
Latest machine-tested executable implementation head:
`b228cd134f8e3258bb54fcb8e8f1fb01c21b96f8`

Validated on the user's native Windows ARM64 machine:
- Native Windows ARM64 compiler gate: PASS — Core and Harness built successfully; Harness targeted `net9.0\win-arm64`.
- Core tests: PASS — 90 total, 90 succeeded, 0 failed, 0 skipped.
- Canonical Missing Raft runtime with structural + ECJ-1/SHA-256 enforcement: PASS; exit `0`.
- Generic smoke runtime regression: PASS; exit `0`.
- Final Patch 0004 static/adversarial/hygiene/scope review: PASS advisory.

Detailed evidence:
`docs/evidence/H1_PATCH_0004_ARM64_VALIDATION.md`

Documentation-only Patch 0005 blueprint/checkpoint commits do not increase executable validation authority.

## Patch 0003 frozen identity
Canonical specification:
`docs/blueprint/H1_PATCH_0003_ECJ1_FIXTURE_HASH.md`

Missing Raft 0.1.0 frozen ECJ-1 reference remains:
- canonical UTF-8 byte length: `9112`;
- SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

The canonical Missing Raft source JSON remains unchanged.

## Patch 0004 validated Access Control boundary
Canonical specification:
`docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`

Validated law preserved for Patch 0005:
1. `CharacterAccessProjection` is the only approved Character-safe information input to Context Composer;
2. Production HistoricalTruth, UnresolvedProposition, WorldState, chronology, denied Access decisions, and fixture provenance are not Composer inputs;
3. projection records contain safe disclosure fields only and no authoritative `Validated*` references;
4. Access Control emits the maximal permitted set and remains upstream of all relevance/composition logic;
5. E0-D omniscient context remains outside the safe reference Access Control operation.

## Current Patch 0005 specification
Canonical specification:
`docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md`

Approved law includes:
1. Patch 0005 implements the deterministic Context Composer immediately after validated Access Control;
2. E0 reference composition is `ensemble.e0.context.full-authorized.v1`: every permitted record is included exactly once in its original authority category;
3. Composer public input is only `CharacterAccessProjection + CurrentOpportunityCharacterId`;
4. opportunity must equal the subject Character, be rostered, and grants no additional knowledge;
5. `ContextSchemaVersion = ensemble.e0.context.v1`;
6. `RenderingContract = ensemble.e0.context.render.v1`;
7. packet structure preserves Scene/subject/opportunity identity, roster, SceneState, Pressures, Constitution, Disposition, Circumstance, Observations, Knowledge, Beliefs, Suspicions, Memories, Goals, and directional Relationships as distinct fields;
8. `recentPerformances` is reserved as exact empty schema-v1 array; Patch 0005 creates no recent-history DTO/non-empty path;
9. provider-neutral rendering separates trusted state, recent performance, and opportunity layers with exact LF/headings/bullet/no-trailing-LF behavior;
10. final system/provider request construction remains outside Context Composer;
11. `StructuredContextHash` is SHA-256 over explicit canonical structured semantic content;
12. `RenderedContextHash` is SHA-256 over RenderingContract plus exact provider-neutral rendered disclosure;
13. `ContextPacketId = CTX:<StructuredContextHash>` and is semantic content identity, not fixture/run provenance or authorization;
14. canonical JSON string/UTF-8 emission is shared with ECJ-1 rather than duplicated, while fixture-specific canonical order remains explicit;
15. Missing Raft ECJ-1 must remain exactly 9112 bytes and the frozen SHA-256 digest;
16. packet/render/trace must not contain denied Access IDs or fixture provenance;
17. packet/render/trace constructors remain non-public authority boundaries;
18. no relevance inference, token optimization, provider/model calls, Director selection logic, persistence, recent-history implementation, or E0-D bypass belongs in Patch 0005.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine: test-execution authority for tests actually exercised.
- Actual target-device execution: runtime authority for behavior actually exercised.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Explicitly unvalidated / excluded
No Patch 0005 executable validation exists yet.

Patch 0005 does not establish or implement:
- semantic/probabilistic relevance ranking;
- embeddings/vector search;
- token budgeting/truncation or context-cost optimization;
- provider system prompt/request construction;
- provider/model adapters or calls;
- provider-request hash;
- Director/opportunity selection;
- accepted Take/history projection or non-empty recent performance;
- E0-D omniscient or relationship-omission execution;
- Integrity Validator;
- State Interpreter / State Authority;
- ProductionState / StateHash;
- causal commit/persistence/recovery replay;
- observation engine;
- Windows AI or NPU execution/performance;
- WinUI;
- packaging/WACK;
- Microsoft Store certification.

## Immediate next action
Implement H1 Patch 0005 only from current `main` on a dedicated implementation branch, following:
`docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md`

Implementation order:
1. independently derive the canonical Missing Raft Voss structured/rendered reference hashes from the approved blueprint contract before freezing expected values in production tests;
2. extract the smallest shared canonical JSON string/UTF-8 primitive from ECJ-1 only if needed, preserving exact existing ECJ-1 bytes/digest;
3. add immutable non-forgeable Context packet/render/trace models;
4. implement deterministic full-authorized composition from `CharacterAccessProjection + opportunity CharacterId` only;
5. implement exact structured canonicalization, dual SHA-256 identity, and `CTX:<StructuredContextHash>` packet identity;
6. implement exact provider-neutral render contract without provider/system prompt machinery;
7. add exact generic + Missing Raft packet-set, category, leakage, render-byte, hash, ordering, determinism, fail-closed, and non-forgeability tests;
8. prove `recentPerformances` remains exact empty array with no speculative history implementation;
9. prove the frozen Missing Raft fixture ECJ-1 output/hash remains unchanged;
10. run static/adversarial/hygiene/scope review;
11. request native Windows ARM64 build, full tests, Missing Raft runtime regression, and generic smoke runtime regression;
12. promote only after all exercised gates pass.

Do not enter provider integration, Director logic, accepted-history/persistence, relevance inference, token optimization, or E0-D bypasses during Patch 0005.

## Continuity
Fresh chats read this file first.

For Patch 0005 implementation, then read:
- `docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md`;
- `src/Ensemble.E0.Core/Access/CharacterAccessModels.cs`;
- `src/Ensemble.E0.Core/Access/CharacterBoundedAccessControl.cs`;
- `src/Ensemble.E0.Core/Fixture/Ecj1FixtureCanonicalizer.cs`;
- only directly affected Core/test files.

For prior authority, read only as needed:
- `docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`;
- `docs/blueprint/H1_PATCH_0003_ECJ1_FIXTURE_HASH.md`;
- `docs/blueprint/H1_PATCH_0002_2_MISSING_RAFT_CONTRACT.md`.

Do not reconstruct already-approved project state from chat history when GitHub contains it.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
