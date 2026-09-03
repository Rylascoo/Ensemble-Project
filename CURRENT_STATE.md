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
- H1 Patch 0006 Performer Candidate Output Contract Proposal 1.7 is APPROVED and canonical.
- H1 Patch 0007 E0 Director Opportunity Contract Proposal 1.5 is APPROVED and canonical.
- H1 Patch 0008 E0 Integrity Validator Contract Proposal 1.1 is APPROVED and canonical.
- H1 Patch 0009 E0 State Interpreter Mutation-Proposal Contract Proposal 0.7 is APPROVED and canonical for implementation.
- Patches 0006/0007/0008/0009 each completed recursive adversarial audit with a full zero-material-change pass before approval.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless stronger frozen authority explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Open design guard
`docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md` is a STRONG DIRECTION / OPEN DESIGN GUARD, not a frozen final schema.

> Keep authority semantics precise; keep creative semantics open.

Patch 0009 performed the required State Interpreter revisit. Its E0 mutation domains are explicitly E0 authority vocabulary for the experiment, not authority to assume that the final creator-facing dramatic ontology is a closed set of engine enums. The post-E0 durable Production/Studio ontology remains open.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

Machine-validated deterministic patches:
- H1 Patch 0003 — COMPLETE;
- H1 Patch 0004 — COMPLETE;
- H1 Patch 0005 — COMPLETE;
- H1 Patch 0006 — COMPLETE;
- H1 Patch 0007 — COMPLETE for its exercised deterministic Director gates;
- H1 Patch 0008 — COMPLETE for its exercised deterministic Integrity gates.

Patch 0009 is APPROVED FOR IMPLEMENTATION but not yet machine-validated.

## Patch 0009 canonical specification and approval
Canonical blueprint:
`docs/blueprint/H1_PATCH_0009_STATE_INTERPRETER_PROPOSAL_CONTRACT.md`

Exact recursively audited and user-approved proposal head:
`30a8d454a0081713839e70b3777c2c45a9fc51af`

Approval PR #19 was promoted from draft after explicit user approval and squash-merged from that exact audited head without blueprint-content changes.

Canonical blueprint promotion commit on `main`:
`4e546ba0f89879e38f31107f2fc3df9d0b911fe8`

Approval record:
`docs/evidence/H1_PATCH_0009_BLUEPRINT_APPROVAL.md`

Patch 0009 approved architecture:
1. schema/parser only; no State Interpreter provider execution;
2. `StateInterpretationSource.Bind(ContextPacket, CandidatePerformance, IntegrityValidationEvaluation)` is the sole rich structural binding boundary;
3. Source binding structurally requires the exact Patch 0008 Accept association but does not authenticate assessor/provider provenance, accepted-Take status, or State authority;
4. Patch 0009 does not reimplement Patch 0008 concern-evidence validation;
5. Source public identity is least-privilege: Candidate content identity, Source Scene, source Character/Context identities, canonical three-Character roster;
6. semantic Proposal copies Candidate/Scene identity from trusted Source; untrusted AI JSON contains only `schemaVersion` + `mutations`;
7. provisional-Take-versus-State-Interpreter immediate ordering remains open;
8. E0 mutation domains are explicitly E0 authority vocabulary rather than final creator-facing ontology;
9. typed immutable semantic variants make invalid authority combinations difficult to express;
10. E0 domains: WorldState, SceneState, UnresolvedProposition, CharacterKnowledge, CharacterBelief, CharacterSuspicion, CharacterMemory, CharacterGoal, CharacterDisposition, CharacterCircumstance, CharacterClaim, Relationship, Pressure;
11. Constitution, HistoricalTruth / accepted Performance history, Observation, PresentationPerspective, and Director opportunity cannot be Interpreter mutation targets;
12. Knowledge and Memory are Add-only in E0, preserving open forgetting/unlearning/memory-rewrite semantics;
13. CharacterClaim is source-Character Add-only interpreted proposition, not verbatim quotation and not objective truth;
14. Add / Supersede / Deactivate only; no destructive Delete or silent Replace;
15. existing/supporting Record IDs remain syntactic proposal references only and do not prove existence, disclosure, causal support, or authority;
16. empty mutation proposal is valid;
17. exact duplicate semantic mutations fail; non-identical conflicts remain for deterministic State Authority;
18. strict parser ceiling 1 MiB inclusive, maximum depth 8, strict unknown/missing/decoded-duplicate/type/trailing/Unicode/NFC/display-bearing rules and sanitized exceptions;
19. no mutation becomes authoritative merely because it parsed;
20. no State Authority, Take/Commit, provider/retry/spend, persistence, Scene loop, World Resolver, UI, Windows AI/NPU, packaging, WACK, or Store scope enters Patch 0009.

Patch 0009 blueprint approval does not establish compiler/test/runtime validation.

## Patch 0008 machine validation
Exact machine-tested executable/test head:
`30a07d0aeee63db927eecd49391fb6271268dc4d`

On the user's native Windows ARM64 development machine at that exact head:

### ARM64 Harness/Core build
`dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug`

Observed:
- `Ensemble.E0.Core` PASS;
- `Ensemble.E0.Harness` PASS;
- Harness target `net9.0\win-arm64`;
- build PASS.

### Full Core tests
`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`

Observed:
- total `253`;
- succeeded `253`;
- failed `0`;
- skipped `0`.

Patch 0008 contributes 42 Integrity test executions over the machine-validated 211-test Patch 0007 baseline.

### Harness regressions
Missing Raft: PASS / exit `0`.
Generic smoke: PASS / exit `0`.

Detailed evidence:
`docs/evidence/H1_PATCH_0008_ARM64_VALIDATION.md`

Documentation-only closure commits after `30a07d0a...` do not increase executable validation authority.

## Patch 0008 implementation result
Machine-tested executable/test delta from approval checkpoint `34b3ffdae...` to `30a07d0a...` contains exactly:
1. `src/Ensemble.E0.Core/Integrity/IntegrityModels.cs`;
2. `src/Ensemble.E0.Core/Integrity/DeterministicIntegrityValidator.cs`;
3. `tests/Ensemble.E0.Core.Tests/Integrity/DeterministicIntegrityValidatorTests.cs`.

No existing Access, Context, Performer, Director, fixture, Harness, project configuration, persistence, UI, Windows AI/NPU, packaging, or Store source changed in that machine-tested executable/test delta.

Reference Candidate-content oracle:
`18eb8c9ba9d35f34f84e1fdd983eeb2a19ce015a2e44457c4514576f984af3b2`.

## Patch 0007 retained machine authority
Patch 0007 machine-tested executable/test head:
`aa9cd908194f414801fa0ed1bebea62298f798e2`

Detailed evidence:
`docs/evidence/H1_PATCH_0007_ARM64_VALIDATION.md`

Patch 0008's 253/253 result is the latest full Core regression authority while preserving Patch 0007's historical validation record.

## Frozen regression identities
Missing Raft / Voss Context:
- StructuredContextHash `bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- ContextPacketId `CTX:bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- RenderedContextHash `ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88`.

Missing Raft ECJ-1:
- `9112` UTF-8 bytes;
- SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

## Validation authority
- Static/adversarial review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine: test-execution authority for exercised tests.
- Actual target-device execution: runtime authority for exercised behavior.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Explicitly unvalidated / excluded
Patch 0009 approval does not establish or implement:
- State Interpreter provider/model execution or request composition;
- authenticated semantic-assessor/provider-attempt provenance;
- accepted/rejected/alternate Take semantics or TakeId allocation;
- deterministic State Authority decisions;
- ProductionState / StateHash;
- authoritative RecordId allocation for new State records;
- mutation application;
- atomic source Performance+approved-consequence causal commit/persistence/recovery;
- effective Current Opportunity mutation/history append;
- next-Performer triggering or Scene loop;
- observation engine / World Resolver;
- E0-D round-robin execution;
- E0-E playwright-control Interpreter protocol;
- Windows AI or NPU execution/performance;
- WinUI;
- packaging/WACK;
- Microsoft Store certification.

## Immediate next action
Implement approved H1 Patch 0009 Proposal 0.7 from an implementation branch created from this approval checkpoint.

Implementation rules:
1. patch only the State Interpreter source-binding / semantic models / strict parser / targeted tests needed by Proposal 0.7;
2. do not change validated Access, Context, Performer, Director, Integrity, fixture, Harness, project configuration, or packaging source unless a concrete compiler/contract defect proves a minimal change necessary;
3. preserve the exact Patch 0009 authority boundaries above;
4. do not introduce provider execution, State Authority, Take, commit, persistence, Scene loop, World Resolver, UI, Windows AI/NPU, or Store work;
5. run recursive implementation audit before the native gate;
6. return to the user's native Windows ARM64 machine for build, full Core tests, Missing Raft Harness, and smoke Harness before marking Patch 0009 COMPLETE;
7. if machine feedback exposes a compiler/runtime issue, patch the smallest affected surface and preserve exact validation authority heads.

## Continuity
Fresh chats read this file first.

Patch 0009:
- `docs/evidence/H1_PATCH_0009_BLUEPRINT_APPROVAL.md`;
- `docs/blueprint/H1_PATCH_0009_STATE_INTERPRETER_PROPOSAL_CONTRACT.md`.

Patch 0008:
- `docs/evidence/H1_PATCH_0008_ARM64_VALIDATION.md`;
- `docs/evidence/H1_PATCH_0008_BLUEPRINT_APPROVAL.md`;
- `docs/blueprint/H1_PATCH_0008_INTEGRITY_VALIDATOR_CONTRACT.md`.

Patch 0007:
- `docs/evidence/H1_PATCH_0007_ARM64_VALIDATION.md`;
- `docs/evidence/H1_PATCH_0007_BLUEPRINT_APPROVAL.md`;
- `docs/blueprint/H1_PATCH_0007_DIRECTOR_OPPORTUNITY_CONTRACT.md`.

Creator ontology guard:
- `docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md`.

Immediate upstream authority:
- `docs/blueprint/H1_PATCH_0006_PERFORMER_CANDIDATE_CONTRACT.md`;
- `docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md`;
- `docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`.

Do not reconstruct approved project state from chat history when GitHub contains it.

## Project rule
Every patch is reviewed against the previous validated baseline:
`correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence`

Git preserves history; the active source tree preserves only the best current architecture.
