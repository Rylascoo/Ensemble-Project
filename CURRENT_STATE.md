# Ensemble Current State

Updated: 2026-09-03

## Authority

- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- H1 Patch 0002.2 Missing Raft Fixture Contract Blueprint 0.4 is canonical.
- H1 Patch 0003 ECJ-1 Canonical Fixture Identity Blueprint 0.2 is APPROVED and machine-validated for exercised gates.
- H1 Patch 0004 Deterministic Character-Bounded Access Control Blueprint 0.2 is APPROVED and machine-validated for exercised gates.
- H1 Patch 0005 Deterministic Context Composer + Dual Context Identity Blueprint 0.3 is APPROVED and machine-validated for exercised gates.
- H1 Patch 0006 Performer Candidate Output Contract Proposal 1.7 is APPROVED and machine-validated for exercised gates.
- H1 Patch 0007 E0 Director Opportunity Contract Proposal 1.5 is APPROVED and machine-validated for exercised gates.
- H1 Patch 0008 E0 Integrity Validator Contract Proposal 1.1 is APPROVED and machine-validated for exercised gates.
- H1 Patch 0009 E0 State Interpreter Mutation-Proposal Contract Proposal 0.7 is APPROVED, canonical, implemented, and machine-validated for exercised gates.
- H1 Patch 0010 E0 Deterministic State Authority Review/Decision Contract Proposal 0.6 is APPROVED and canonical; implementation exists on its implementation branch but is NOT yet machine-validated.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless stronger frozen authority explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is authoritative current engineering state.
- Google Drive `Ensemble Project` is design/research/supporting material, not executable validation authority.

## Open design guard

`docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md` remains a STRONG DIRECTION / OPEN DESIGN GUARD, not a frozen final schema.

> Keep authority semantics precise; keep creative semantics open.

Patch 0009/0010 E0 authority domains do not freeze the final creator-facing Production/Studio ontology.

## Current phase

E0-A Harness Implementation — H1 Deterministic Spine.

Current patch:

`H1 Patch 0010 — deterministic State Authority review/decision implementation`

Current implementation status:

`STATICALLY AUDITED; TARGET-MACHINE VALIDATION PENDING`

## Patch 0010 canonical authority

Canonical blueprint:
`docs/blueprint/H1_PATCH_0010_DETERMINISTIC_STATE_AUTHORITY.md`

Exact recursively audited Proposal 0.6 head:
`2188dfe7fe693c7644c0a7a74eb647974bba876d`

Canonical blueprint promotion commit on `main`:
`fbd3c7004c3f047ebb2b3244e4488f993390231b`

Original approval evidence checkpoint:
`b179d4a1a324349d66dce6158fc89dba5ddf0e2f`

Approval record:
`docs/evidence/H1_PATCH_0010_BLUEPRINT_APPROVAL.md`

Chat/bootstrap recovery audit:
`docs/evidence/H1_PATCH_0010_CHAT_RECOVERY_AUDIT.md`

Important status rule:

The canonical blueprint preserves the historical Proposal 0.6 audit-stage header at its exact approved content. Do not infer current project status from that historical header. Current status is established by this file plus the approval/evidence records.

## Patch 0010 implementation

Implementation branch:
`h1-patch-0010-state-authority-implementation`

Current recursively static-audited implementation head:
`401f6bf93aadd8879c1645876b63b729fc6ac962`

Delta from approval checkpoint `b179d4a1...`:
- six commits ahead;
- zero commits behind;
- exactly four added executable/test files:
  1. `src/Ensemble.E0.Core/StateAuthority/StateAuthorityModels.cs`;
  2. `src/Ensemble.E0.Core/StateAuthority/DeterministicStateAuthority.cs`;
  3. `tests/Ensemble.E0.Core.Tests/StateAuthority/DeterministicStateAuthorityTests.cs`;
  4. `tests/Ensemble.E0.Core.Tests/StateAuthority/StateAuthorityContractAuditTests.cs`.

No pre-existing Access, Context, Performer, Director, Integrity, State Interpreter, fixture, Harness, project configuration, persistence, UI, Windows AI/NPU, packaging, WACK, or Store source is changed in that branch delta.

## Patch 0010 static/adversarial audit result

At exact head `401f6bf...`, recursive static review against Proposal 0.6 found no material architecture or contract defect in the four-file delta.

Reviewed dimensions include:

1. Interpreter proposal vs deterministic State Authority separation;
2. proposal-content semantic identity and cryptographic domain separation;
3. prose-free least-privilege evaluator input/trace;
4. fixture-derived structural snapshot and canonical ordering;
5. SystemImmutable and exact-record CreatorLocked protection;
6. typed mutation domain/transition invariants;
7. ReviewSet exact proposal binding;
8. hard deterministic rule > explicit review choice > policy default;
9. mandatory-review floor and operation-sensitive UnresolvedProposition review;
10. policy-eligible domains;
11. support/reference/domain/ownership/protection/conflict hard rules;
12. fixed reason ordering;
13. Approved/Rejected/RequiresReview and Complete/ReviewRequired semantics;
14. empty proposal behavior;
15. truth/claim/belief/knowledge/objective-state separation;
16. deterministic ordering/replay;
17. immutable/non-public authority-produced construction surfaces;
18. absence of ProductionState/StateHash/new RecordId allocation/mutation application/Take/Commit/provider/UI/NPU/Store authority;
19. frozen upstream regression identities represented in tests;
20. initial-fixture-only validation limit.

Static/adversarial review is advisory only.

## Patch 0010 validated architecture boundary

Proposal 0.6 freezes only deterministic mutation review/decision logic.

Key laws:

- `StateAuthorityInput.Bind(snapshot, source, proposal)` is the sole rich Source/Proposal binding boundary;
- exact semantic Interpreter ProposalContentHash binds review to proposal content;
- evaluator input/trace is prose-free;
- snapshot is fixture-derived structural authority state plus exact-record creator-lock overlay, not ProductionState;
- HistoricalTruth, CharacterConstitution, and CharacterObservation are SystemImmutable;
- hard structural rejection is unwaivable;
- explicit review choice can override policy default but not a hard rejection;
- WorldState, SceneState, CharacterKnowledge, CharacterMemory, CharacterDisposition, and Relationship are always MandatoryReview in this E0 contract;
- UnresolvedProposition Supersede/Deactivate is MandatoryReview;
- UnresolvedProposition Add, CharacterBelief, CharacterSuspicion, CharacterGoal, CharacterCircumstance, CharacterClaim Add, and Pressure are policy-eligible after hard rules;
- evaluation returns ordered per-mutation Approved / Rejected / RequiresReview decisions;
- Approved means later commit-eligible only, never already true/effective;
- empty mutation proposal is valid and evaluates Complete with zero decisions;
- same resolved ExistingRecord targeted by multiple Supersede/Deactivate mutations is a hard conflict for all contenders;
- no confidence scalar or semantic keyword judge exists;
- no StateHash/stale-state commit protocol exists yet;
- provisional Take ordering remains OPEN for the dedicated Take contract;
- no ProductionState, new RecordId allocation, mutation application, TakeId, CommitId, atomic commit, persistence, provider execution, UI, Windows AI/NPU, packaging, WACK, or Store machinery enters Patch 0010.

## Patch 0010 validation boundary

Patch 0010 has NOT yet established:

- Windows ARM64 compiler authority;
- Patch 0010 test-execution authority;
- target-device runtime authority;
- multi-turn evolved-state review;
- NPU behavior;
- WACK or Store authority.

Do not call Patch 0010 COMPLETE until the target-machine gates below are observed against an exact commit.

## Immediate next action

Validate exact implementation head `401f6bf93aadd8879c1645876b63b729fc6ac962` on the user's native Windows ARM64 development machine unless the branch changes before validation.

Required gates:

### 1. ARM64 Core/Harness build

```powershell
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
```

Required observations:
- `Ensemble.E0.Core` PASS;
- `Ensemble.E0.Harness` PASS;
- Harness target remains `net9.0\win-arm64`;
- zero build errors.

### 2. Full Core tests

```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

Record exact total/succeeded/failed/skipped counts.

Do not predict the new total before observing the machine result.

### 3. Missing Raft Harness regression

Run the existing canonical Missing Raft Harness command used by the prior patch evidence and require PASS / exit `0`.

### 4. Generic smoke Harness regression

Run the existing generic smoke Harness command used by the prior patch evidence and require PASS / exit `0`.

If compiler/test/runtime feedback reveals a defect, patch only the smallest affected Patch 0010 surface, rerun recursive static review, and return to the exact failed machine gate.

If all four gates pass, record exact machine-tested head and create Patch 0010 ARM64 validation evidence before merge/closure.

## Retained machine-validated baseline

Latest currently machine-validated full Core regression authority remains Patch 0009.

Patch 0009 exact machine-tested executable/test head:
`a257cf7553398a323d8ce790aa600950eb88c1b9`

Observed on the user's native Windows ARM64 development machine:
- Core/Harness build PASS;
- Harness target `net9.0\win-arm64`;
- full Core tests `330/330` PASS;
- Missing Raft Harness PASS / exit `0`;
- generic smoke Harness PASS / exit `0`.

Patch 0009 detailed evidence:
`docs/evidence/H1_PATCH_0009_ARM64_VALIDATION.md`

Patch 0008 detailed evidence:
`docs/evidence/H1_PATCH_0008_ARM64_VALIDATION.md`

Patch 0007 detailed evidence:
`docs/evidence/H1_PATCH_0007_ARM64_VALIDATION.md`

## Frozen regression identities

Missing Raft / Voss Context:
- StructuredContextHash `bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- ContextPacketId `CTX:bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- RenderedContextHash `ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88`.

Patch 0008 Candidate-content oracle:
`18eb8c9ba9d35f34f84e1fdd983eeb2a19ce015a2e44457c4514576f984af3b2`

Missing Raft ECJ-1:
- `9112` UTF-8 bytes;
- SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

## Validation authority

- Static/adversarial review: advisory only.
- Visual Studio / `dotnet` ARM64 build output on target machine: compiler authority.
- `dotnet test` on target machine: test-execution authority for exercised tests.
- Actual target-device execution: runtime authority for exercised behavior.
- NPU execution requires explicit hardware evidence.
- WACK is package-validation authority.
- Partner Center is Store-certification authority.

Never promote a lower validation level into a higher one.

## Fresh-chat bootstrap law

Fresh engineering chats must:

1. read `CURRENT_STATE.md` first;
2. resolve current `main` branch/commit;
3. inspect relevant active branches/PRs/recent commits before concluding a next contract does not exist;
4. read only the exact blueprint/evidence/source files needed for the current task;
5. treat machine evidence supplied by the user as authoritative for that machine;
6. patch the smallest affected surface;
7. update this checkpoint only when a meaningful project state changes.

When this file says to recover a contract, `not on main` does not mean `does not exist`.

Do not reconstruct approved project state from chat history when GitHub contains it.

## Project rule

Every patch is reviewed against the previous validated baseline:

`correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence`

Git preserves history; the active source tree preserves only the best current architecture.
