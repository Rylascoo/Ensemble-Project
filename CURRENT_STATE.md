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
- H1 Patch 0010 E0 Deterministic State Authority Review/Decision Contract Proposal 0.6 is APPROVED, canonical, implemented, and machine-validated for exercised deterministic gates.
- Patches 0006/0007/0008/0009/0010 preserve probabilistic proposal versus deterministic authority separation and completed recursive architecture/static review before closure.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless stronger frozen authority explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is authoritative current engineering state.
- Google Drive `Ensemble Project` is design/research/supporting material, not executable validation authority.

## Open design guard

`docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md` remains a STRONG DIRECTION / OPEN DESIGN GUARD, not a frozen final schema.

> Keep authority semantics precise; keep creative semantics open.

Patch 0009/0010 E0 authority domains do not freeze the final creator-facing Production/Studio ontology.

## Current phase

E0-A Harness Implementation — H1 Deterministic Spine.

Current validated checkpoint:

`H1 Patch 0010 — deterministic State Authority review/decision`

Status:

`COMPLETE FOR EXERCISED DETERMINISTIC STATE AUTHORITY GATES`

## Patch 0010 canonical authority

Canonical blueprint:
`docs/blueprint/H1_PATCH_0010_DETERMINISTIC_STATE_AUTHORITY.md`

Exact recursively audited Proposal 0.6 head:
`2188dfe7fe693c7644c0a7a74eb647974bba876d`

Canonical blueprint promotion commit on `main`:
`fbd3c7004c3f047ebb2b3244e4488f993390231b`

Blueprint approval record:
`docs/evidence/H1_PATCH_0010_BLUEPRINT_APPROVAL.md`

Chat/bootstrap recovery audit:
`docs/evidence/H1_PATCH_0010_CHAT_RECOVERY_AUDIT.md`

Important status rule:

The canonical blueprint preserves its historical Proposal 0.6 audit-stage header at the exact approved content. Current status is established by this file plus the approval/validation evidence records, not by that historical header.

## Patch 0010 implementation and machine authority

Implementation PR:
`#22 — H1: implement deterministic State Authority`

Exact native Windows ARM64 machine-tested executable/test head:
`b07c8e161d221bc9bf2e57802de0aae7b542ce5a`

Squash-merge commit on `main`:
`e2bc3a4130d768dca29698a32fcd6a40e63d5f5c`

Detailed machine evidence:
`docs/evidence/H1_PATCH_0010_ARM64_VALIDATION.md`

The exact machine-tested implementation delta contains only four State Authority source/test files:

1. `src/Ensemble.E0.Core/StateAuthority/StateAuthorityModels.cs`;
2. `src/Ensemble.E0.Core/StateAuthority/DeterministicStateAuthority.cs`;
3. `tests/Ensemble.E0.Core.Tests/StateAuthority/DeterministicStateAuthorityTests.cs`;
4. `tests/Ensemble.E0.Core.Tests/StateAuthority/StateAuthorityContractAuditTests.cs`.

No pre-existing Access, Context, Performer, Director, Integrity, State Interpreter, fixture, Harness, project configuration, persistence, UI, Windows AI/NPU, packaging, WACK, or Store source changed in PR #22.

## Patch 0010 compiler correction history

First target-machine attempt head:
`401f6bf93aadd8879c1645876b63b729fc6ac962`

Observed:
- five C# `CS1503` errors in `DeterministicStateAuthority.cs` caused by `RecordId` versus string lookup-key typing;
- Core build failed;
- test build failed;
- Harness commands run afterward with `--no-build` used older binaries and are not Patch 0010 evidence.

Smallest-surface correction:
`b07c8e161d221bc9bf2e57802de0aae7b542ce5a`

Only `DeterministicStateAuthority.cs` changed: internal existing-record lookup normalized to the validated Record ID string used by string-keyed evaluator dictionaries/sets. Contract behavior did not change.

All required machine gates were then rerun from the corrected exact head.

## Patch 0010 native Windows ARM64 validation

At exact head `b07c8e161d221bc9bf2e57802de0aae7b542ce5a` on the user's native Windows ARM64 development machine:

### ARM64 Core/Harness build

Command:
`dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug`

Observed:
- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Harness` succeeded;
- Harness output target `net9.0\win-arm64`;
- build succeeded.

### Full Core tests

Command:
`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`

Observed:
- total `392`;
- succeeded `392`;
- failed `0`;
- skipped `0`;
- test execution and build succeeded.

Patch 0010 raises the latest validated full Core regression count from Patch 0009's 330 to 392.

### Harness regressions

Missing Raft:
- `Fixture validated: ensemble.e0.missing-raft@0.1.0`.

Generic smoke:
- `Fixture validated: ensemble.e0.smoke@0.1.0`.

These Harness runs occurred after the successful Patch 0010 build and therefore exercised the rebuilt branch output.

## Validated Patch 0010 architecture boundary

Proposal 0.6 establishes deterministic mutation review/decision only.

Key laws validated by the implementation/test boundary include:

1. `StateAuthorityInput.Bind(snapshot, source, proposal)` is the sole rich Source/Proposal binding boundary;
2. exact semantic Interpreter `ProposalContentHash` uses cryptographic domain separation and binds review choices to exact proposal identity;
3. evaluator input and trace remain prose-free;
4. `StateAuthoritySnapshot` is a fixture-derived structural authority projection plus exact-record creator-lock overlay, not ProductionState;
5. HistoricalTruth, CharacterConstitution, and CharacterObservation records are SystemImmutable;
6. hard structural rejection is unwaivable;
7. explicit review choice can override policy default but not hard rejection;
8. WorldState, SceneState, CharacterKnowledge, CharacterMemory, CharacterDisposition, and Relationship are mandatory-review domains in this E0 contract;
9. UnresolvedProposition Supersede/Deactivate is mandatory review;
10. UnresolvedProposition Add, CharacterBelief, CharacterSuspicion, CharacterGoal, CharacterCircumstance, CharacterClaim Add, and Pressure are policy-eligible after hard rules;
11. evaluation returns ordered per-mutation Approved / Rejected / RequiresReview decisions;
12. Approved means later commit-eligible under evaluated authority inputs only; it is not already effective state;
13. empty mutation proposal is valid and evaluates Complete with zero decisions;
14. multiple Supersede/Deactivate mutations targeting the same resolved existing record hard-reject all contenders;
15. fixed deterministic reason ordering is preserved;
16. no model confidence, prose keyword judgment, random choice, provider call, or hidden semantic inference enters State Authority;
17. no StateHash/stale-state commit protocol exists yet;
18. no ProductionState, new RecordId allocation, mutation application, TakeId, CommitId, atomic commit, persistence, Scene loop, UI, Windows AI/NPU, packaging, WACK, or Store machinery enters Patch 0010.

## Explicit validation limits

Patch 0010 does not establish:

- evolved multi-turn ProductionState review;
- ProductionState / StateHash;
- authoritative new RecordId allocation;
- mutation application;
- accepted/rejected/alternate Take semantics or TakeId allocation;
- provisional-Take ordering relative to State Interpreter / State Authority;
- atomic causal Commit / CommitId;
- persistence/recovery;
- authenticated policy/review provenance;
- provider/model State Interpreter execution;
- Scene-loop execution;
- World Resolver / Observation engine;
- Windows AI / NPU execution or performance;
- WinUI behavior;
- packaging / WACK;
- Microsoft Store certification.

## Next-contract recovery result

After Patch 0010 closure, repository continuity was checked before inventing a next patch.

Searches found:

- no active branch named for Patch `0011`;
- no active branch named for `Take` or `Commit` work;
- no existing Patch 0011 PR;
- no dedicated Take-semantics PR or commit establishing the next implementation-complete contract.

Existing frozen H1 authority consistently orders the remaining spine as:

1. accepted/rejected/alternate Take semantics needed by E0;
2. atomic causal commit.

However, the immediate provisional-Take-versus-State-Interpreter/State-Authority ordering remains explicitly OPEN and must be resolved by the dedicated Take contract rather than inferred from earlier chat diagrams.

## Immediate next action

Prepare a standalone H1 Patch 0011 Take Semantics blueprint from canonical repository/project authority.

Before drafting:

1. read this checkpoint first;
2. resolve current `main` head;
3. confirm again that no newer relevant branch/PR exists;
4. read frozen Blueprint 0.1, approved H1 Deterministic Spine authority, and only the relevant Patch 0006/0007/0008/0009/0010 contracts/evidence needed to recover Take requirements;
5. explicitly reconcile the open provisional-Take ordering rather than assuming `Interpreter -> Authority -> Take` or `Take -> Interpreter`;
6. preserve Candidate/Integrity/Interpreter/State Authority identity and non-authority distinctions;
7. keep Take identity distinct from CandidateContentHash, ProposalContentHash, CommitId, and StateHash;
8. do not enter atomic commit, ProductionState application, persistence, provider execution, Scene loop, UI, Windows AI/NPU, packaging, WACK, or Store implementation merely as Take-semantics cleanup;
9. recursively adversarial-audit the standalone blueprint to a full zero-material-change pass before requesting approval;
10. do not write executable Patch 0011 code before blueprint approval.

## Latest machine-validated regression authority

Patch 0010 exact machine-tested executable/test head:
`b07c8e161d221bc9bf2e57802de0aae7b542ce5a`

Observed:
- ARM64 Core/Harness build PASS;
- Harness target `net9.0\win-arm64`;
- full Core tests `392/392` PASS;
- Missing Raft Harness PASS;
- generic smoke Harness PASS.

Detailed evidence:
`docs/evidence/H1_PATCH_0010_ARM64_VALIDATION.md`

Prior validation evidence:
- `docs/evidence/H1_PATCH_0009_ARM64_VALIDATION.md`;
- `docs/evidence/H1_PATCH_0008_ARM64_VALIDATION.md`;
- `docs/evidence/H1_PATCH_0007_ARM64_VALIDATION.md`.

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
3. inspect relevant active branches, PRs, and recent commits before concluding a next contract does not exist;
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
