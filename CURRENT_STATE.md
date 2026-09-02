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
- Patch 0006 Proposal 1.7 and Patch 0007 Proposal 1.5 each completed recursive adversarial audit with a full zero-material-change pass before approval.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless stronger frozen authority explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Open design guard
`docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md` is a STRONG DIRECTION / OPEN DESIGN GUARD, not a frozen final schema.

> Keep authority semantics precise; keep creative semantics open.

Current E0 categories such as Knowledge, Beliefs, Suspicions, Memories, Goals, Relationships, Pressures, and Circumstance remain authoritative for frozen E0 contracts. They must not be assumed to prove that the final creator-facing dramatic ontology is a closed set of engine enums. Revisit before freezing the broad State Interpreter mutation ontology or durable post-E0 Production/Studio ontology.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

Machine-validated deterministic patches:
- H1 Patch 0003 — COMPLETE;
- H1 Patch 0004 — COMPLETE;
- H1 Patch 0005 — COMPLETE;
- H1 Patch 0006 — COMPLETE;
- H1 Patch 0007 — COMPLETE for its exercised deterministic Director gates.

## Patch 0007 canonical specification and approval
Canonical blueprint:
`docs/blueprint/H1_PATCH_0007_DIRECTOR_OPPORTUNITY_CONTRACT.md`

Exact recursively audited and user-approved proposal head:
`3562db371f67f07f5896ff2a17f067e0ef02e6e9`

Approval PR #15 is recorded by GitHub as merged at that exact approved head. Its cumulative approved delta was one blueprint document and zero executable files.

Approval record:
`docs/evidence/H1_PATCH_0007_BLUEPRINT_APPROVAL.md`

Creator-ontology guard:
`docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md`

Implementation approval checkpoint:
`5836bee7ec231ad94dccb6b26118046f1cfdabcc`

## Patch 0007 machine validation
Machine-tested executable/test head:
`aa9cd908194f414801fa0ed1bebea62298f798e2`

On the user's native Windows ARM64 development machine at that exact head:

### ARM64 Harness/Core build
Command:
`dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug`

Observed:
- `Ensemble.E0.Core` PASS;
- `Ensemble.E0.Harness` PASS;
- Harness target `net9.0\win-arm64`;
- build PASS.

### Full Core tests
Command:
`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`

Observed:
- total `211`;
- succeeded `211`;
- failed `0`;
- skipped `0`.

Patch 0007 contributes 38 Director test executions over the validated 173-test Patch 0006 baseline.

### Harness regressions
Missing Raft:
- `Fixture validated: ensemble.e0.missing-raft@0.1.0`;
- exit `0`.

Generic smoke:
- `Fixture validated: ensemble.e0.smoke@0.1.0`;
- exit `0`.

Detailed evidence:
`docs/evidence/H1_PATCH_0007_ARM64_VALIDATION.md`

Documentation-only closure commits after `aa9cd908...` do not increase executable validation authority.

## Patch 0007 implementation result
Machine-tested implementation delta from approval checkpoint `5836bee7...` to `aa9cd908...` contains exactly three added files:

1. `src/Ensemble.E0.Core/Director/DirectorOpportunityModels.cs`;
2. `src/Ensemble.E0.Core/Director/LeastInterventionDirector.cs`;
3. `tests/Ensemble.E0.Core.Tests/Director/LeastInterventionDirectorTests.cs`.

No existing Access Control, Context Composer, Performer Candidate, fixture, Harness, project configuration, persistence, UI, Windows AI/NPU, packaging, or Store source changed in the machine-tested implementation delta.

Implemented architecture:
1. `DirectorOpportunityInput.Bind(ContextPacket, CandidatePerformance, ImmutableArray<CharacterId>)` structurally validates and copies only Scene/source/context identity, canonical roster/control IDs, and supplied opportunity-event history;
2. structural Bind is explicitly not causal-history authentication;
3. E0 roster is exactly three Characters and history must be non-empty, roster-bound, and end at the source Character;
4. Director strategy cannot inspect Candidate VisibleText or Character-private Context by type;
5. semantic `DirectorOpportunityProposal` contract `ensemble.e0.director.opportunity.v1` contains only ContractVersion, SceneId, SourceCharacterId, SelectedCharacterId;
6. strategy `ensemble.e0.director.least-intervention.v1` selects nomination, otherwise least-recent addressed Character, otherwise least-recent roster Character;
7. recency uses never-seen first, then oldest final history index, then ordinal CharacterId tie-break;
8. trace-only diagnostics record never-opportunitied roster members, repeated same-Character attention, and final A-B-A-B alternation;
9. diagnostics never alter selection and create no fairness/equal-turn authority;
10. no scores, weights, probabilities, randomness, exclusion timers, maximum-gap rules, LLM Director routing, or VisibleText semantic parsing;
11. Bind/Propose are pure calculation only and do not mutate Current Opportunity, append history, trigger a Performer, commit a Take, mutate State, or persist anything;
12. precommit evaluation is zero-authority speculation; postcommit recomputation is still only a proposal;
13. later effective-opportunity authority must separately establish Current Opportunity and its corresponding opportunity-history event coherently or establish neither;
14. E0-D round-robin remains a separate future strategy expected to share DirectorOpportunityInput and DirectorOpportunityProposal shape;
15. post-E0 subset attention and final structured/local-semantic/hybrid Director remain open.

## Frozen regression identities
Missing Raft / Voss Context:
- StructuredContextHash `bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- ContextPacketId `CTX:bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- RenderedContextHash `ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88`.

Missing Raft ECJ-1:
- `9112` UTF-8 bytes;
- SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

## Patch 0006 retained machine authority
Patch 0006 detailed evidence remains:
`docs/evidence/H1_PATCH_0006_ARM64_VALIDATION.md`

Its corrected full Core test authority was 173/173 before Patch 0007 added 38 Director executions. Patch 0007's 211/211 full-suite result supersedes it as the latest exercised Core regression test total while preserving Patch 0006's historical validation record.

## Validation authority
- Static/adversarial review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine: test-execution authority for exercised tests.
- Actual target-device execution: runtime authority for exercised behavior.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Explicitly unvalidated / excluded
Patch 0007 does not establish or implement:
- authoritative opportunity-history authentication/persistence;
- accepted/rejected/alternate Take semantics or TakeId allocation;
- Integrity Validator acceptance/rejection;
- State Interpreter candidate mutations;
- deterministic State Authority;
- ProductionState / StateHash;
- atomic source Performance+consequence commit/persistence/recovery;
- effective Current Opportunity mutation;
- atomic opportunity establishment/history append;
- next-Performer triggering or Scene loop;
- live provider/model behavior or provider-attempt provenance;
- observation engine / World Resolver;
- E0-D round-robin execution;
- Windows AI or NPU execution/performance;
- WinUI;
- packaging/WACK;
- Microsoft Store certification.

Patch 0007 validation proves only the exercised deterministic Director input/proposal/evaluation boundary and its regressions.

## Immediate next action
Hold at the validated H1 Patch 0007 boundary until the next canonical H1 deterministic-spine contract is recovered from existing project authority.

Before new executable work:
1. read this checkpoint first;
2. read the frozen Blueprint 0.1 / approved E0-A and H1 authority only as needed to resolve the next boundary;
3. search existing GitHub/project sources for an already-approved next contract before inventing one;
4. if no implementation-complete contract exists, create a standalone blueprint and recursively adversarial-audit it to a zero-material-change pass before requesting approval;
5. preserve the validated separation among Performer Candidate, Director proposal, later Integrity/Take/State authority, causal commit, and effective opportunity establishment;
6. do not infer that the creator-facing ontology is closed merely because E0 uses explicit semantic categories;
7. do not begin provider, persistence, Scene-loop, UI, Windows AI/NPU, or Store work merely as Patch 0007 cleanup.

## Continuity
Fresh chats read this file first.

Patch 0007:
- `docs/evidence/H1_PATCH_0007_ARM64_VALIDATION.md`;
- `docs/evidence/H1_PATCH_0007_BLUEPRINT_APPROVAL.md`;
- `docs/blueprint/H1_PATCH_0007_DIRECTOR_OPPORTUNITY_CONTRACT.md`.

Creator ontology guard:
- `docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md`.

Immediate upstream authority:
- `docs/evidence/H1_PATCH_0006_ARM64_VALIDATION.md`;
- `docs/blueprint/H1_PATCH_0006_PERFORMER_CANDIDATE_CONTRACT.md`;
- `docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md`;
- `docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`.

Do not reconstruct approved project state from chat history when GitHub contains it.

## Project rule
Every patch is reviewed against the previous validated baseline:
`correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence`

Git preserves history; the active source tree preserves only the best current architecture.
