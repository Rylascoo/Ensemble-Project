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

H1 Patch 0005 is COMPLETE for its exercised gates and promoted through PR #12.

## Latest machine-validated Patch 0005 implementation state
Corrected full Core test head:
`befb6648c36400546ac4843d575bd64760b23445`

At that head on the user's native Windows ARM64 machine:
- `Ensemble.E0.Core` rebuilt successfully as part of `dotnet test`;
- Core tests: PASS — 115 total, 115 succeeded, 0 failed, 0 skipped.

Native Harness build/runtime authority was established at earlier implementation head:
`44efd6358d8a94e769eee2ff9b2c7bd4715f5587`

At that head on the same native Windows ARM64 machine:
- Core and Harness build: PASS;
- Harness target output: `net9.0\win-arm64`;
- Missing Raft runtime regression: PASS; exit `0`;
- generic smoke runtime regression: PASS; exit `0`.

GitHub comparison from `44efd...` through `befb...` contains only:
- `docs/evidence/H1_PATCH_0005_REFERENCE_ORACLE.md`;
- `tests/Ensemble.E0.Core.Tests/Context/DeterministicContextComposerTests.cs`.

No production source changed between those machine-tested heads. Therefore the Harness build/runtime evidence applies to the same production source content that passed the corrected 115-test gate.

Patch 0005 PR #12 closure head:
`7256d5a59776b5c8198b14de4335ebb92eda89c4`

PR #12 merge commit on `main`:
`b0573a6ad8049ee5a03cd5af822476ad013135e3`

The merge commit has two parents: the durable `CURRENT_STATE.md` checkpoint line and the exact PR #12 closure head. This preserved both histories without a force update after the GitHub draft-to-ready connector failed.

Commits after `befb...` on the PR implementation lineage are documentation-only evidence closure and do not increase executable validation authority.

Detailed evidence:
- `docs/evidence/H1_PATCH_0005_ARM64_VALIDATION.md`
- `docs/evidence/H1_PATCH_0005_REFERENCE_ORACLE.md`

## Patch 0005 corrected frozen Context identity
Canonical Missing Raft / Voss reference:
- structured canonical UTF-8 length: `2569` bytes;
- StructuredContextHash: `bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- ContextPacketId: `CTX:bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- TrustedStateText UTF-8 length: `1696` bytes;
- rendered envelope UTF-8 length: `1905` bytes;
- RenderedContextHash: `ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88`.

The original pre-machine oracle accidentally transcribed canonical `REL-VOSS-MARLOWE` text as `competence` instead of fixture-authoritative `perception`. Because both words are ten ASCII bytes, byte-length expectations remained unchanged while both hashes changed. The canonical fixture controlled; production Composer code did not change.

## Patch 0005 implementation result
Canonical specification:
`docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md`

Implemented architecture:
1. `DeterministicContextComposer` is the single safe E0 reference Composer for `ensemble.e0.context.full-authorized.v1`;
2. public Composer input is limited to `CharacterAccessProjection + CurrentOpportunityCharacterId`;
3. current opportunity must equal the projection subject and grants no additional knowledge;
4. every already-permitted Access record is included exactly once in its original authority category;
5. Knowledge, Belief, Suspicion, Memory, Observation, SceneState, Pressure, Constitution, Disposition, Circumstance, Goal, and directional Relationship remain structurally distinct;
6. no semantic relevance, embeddings, ranking, token optimization, summarization, paraphrase, deduplication, or truncation occurs in the E0 reference contract;
7. `recentPerformances` is canonically exact `[]`; no accepted-history DTO or non-empty population path exists yet;
8. provider-neutral rendering uses `ensemble.e0.context.render.v1` with deterministic LF-only headings/bullets and separate trusted-state, recent-performance, and opportunity layers;
9. Rendered context omits internal Record IDs, Character IDs, SceneId, fixture hash, provenance, access decisions, provider/model data, and diagnostics;
10. `StructuredContextHash` identifies canonical structured semantic context;
11. `RenderedContextHash` identifies the rendering contract plus exact provider-neutral rendered disclosure;
12. `ContextPacketId = CTX:<StructuredContextHash>` is content identity, not authorization, fixture/run provenance, signing, or publisher identity;
13. local composition trace is separate from Performer-facing packet context and contains no denied Access IDs or record text;
14. public safe output types are read-only with Core-internal constructors so external assemblies cannot forge authoritative Context objects through public construction;
15. canonical JSON scalar/string + UTF-8 emission is shared with ECJ-1 through one small internal helper; fixture-specific ECJ-1 ordering remains explicit;
16. frozen Missing Raft ECJ-1 identity remains exactly 9112 bytes and SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`;
17. E0-D omniscient and relationship-omission conditions remain separate experimental paths, not flags/bypasses in the safe reference Composer;
18. no provider/model adapter, Director, persistence/history implementation, ProductionState, StateHash, WinUI, Windows AI/NPU, packaging, WACK, or Store machinery was introduced.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine: test-execution authority for tests actually exercised.
- Actual target-device execution: runtime authority for behavior actually exercised.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Explicitly unvalidated / excluded
Patch 0005 does not establish or implement:
- provider/model behavior;
- Performer system contract or provider-request framing;
- Director/opportunity selection;
- accepted Take/history authority or non-empty recent-performance context;
- semantic/probabilistic relevance optimization;
- E0-D omniscient or relationship-omission execution;
- ProductionState or StateHash;
- Integrity Validator;
- State Interpreter / State Authority;
- causal commit/persistence/recovery replay;
- full observation engine;
- Windows AI or NPU execution/performance;
- WinUI;
- packaging/WACK;
- Microsoft Store certification.

Deterministic Context Composer validation proves the exercised safe composition/render/identity boundary. It does not itself prove Character/model quality, Director behavior, causal state evolution, persistence, product UX, or later security boundaries.

## Immediate next action
Hold at the validated H1 Patch 0005 boundary.

Do not enter provider integration, Director logic, accepted-history plumbing, persistence, or E0-D bypasses as Patch 0005 cleanup.

Before entering the next H1 slice:
1. read this checkpoint first;
2. recover the canonical roadmap/specification for the next deterministic-spine boundary;
3. determine whether the next authority slice is already implementation-complete in GitHub;
4. if not, create and explicitly approve a standalone blueprint before executable work begins;
5. preserve `CharacterAccessProjection` as the only Character information authority input to Context composition;
6. preserve the validated ContextPacket/dual-hash boundary unless a stronger explicitly approved specification deliberately changes it.

No further Patch 0005 implementation work is currently required.

## Continuity
Fresh chats read this file first.

For Patch 0005 history and authority, then read:
- `docs/evidence/H1_PATCH_0005_ARM64_VALIDATION.md`;
- `docs/evidence/H1_PATCH_0005_REFERENCE_ORACLE.md`;
- `docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md`.

For earlier authority, read only as needed:
- `docs/evidence/H1_PATCH_0004_ARM64_VALIDATION.md`;
- `docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`;
- `docs/blueprint/H1_PATCH_0003_ECJ1_FIXTURE_HASH.md`;
- `docs/blueprint/H1_PATCH_0002_2_MISSING_RAFT_CONTRACT.md`.

Do not reconstruct already-approved project state from chat history when GitHub contains it.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
