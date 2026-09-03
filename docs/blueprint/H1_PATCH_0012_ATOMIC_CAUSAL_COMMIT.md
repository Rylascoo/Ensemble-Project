# H1 Patch 0012 — E0 Atomic Causal Commit Contract

Status: blueprint proposal 0.4 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
Parent repository checkpoint: `main` at `20b26711ceffa17e0543ca3fc180c9acf69f5e45`
Parent machine-tested executable/test authority: H1 Patch 0011 at `4250011c167cd9850ad891aaea4ee053216cf135`
Branch: `h1-patch-0012-atomic-causal-commit-blueprint`

## 1. Purpose and frozen law

Patch 0012 is the E0 atomic causal-commit boundary after immutable Take semantics.

Blueprint 0.1 requires append-only causal history as conceptual source of truth and requires one indivisible law:

> The exact Accepted Performance and every Approved consequence become effective together, or neither does.

Patch 0011 further requires that the retained Approved/Rejected consequence package is immutable: freshness may reject the Take but may not rewrite it.

Intended flow:

```text
immutable ProductionState
    -> pre-pipeline ProductionStateCheckpoint
        -> existing Access / Context / Performance / Integrity / Interpretation / State Authority
            -> immutable E0Take
                -> exact Take/source-state binding
                    -> strict unchanged-StateHash freshness
                        -> deterministic atomic commit
                            -> immutable E0CausalCommit
                            -> immutable result ProductionState projection
```

Only Accepted Takes are commit-eligible. Rejected/Alternate/failed material remains non-effective and has no effective CommitId.

## 2. Scope boundary

Patch 0012 defines:

- neutral immutable current Production projection;
- history-sensitive StateHash;
- source-state checkpoint captured before the generative pipeline;
- exact Take/source-state binding;
- deterministic new RecordId materialization input;
- Approved Add/Supersede/Deactivate application;
- immutable causal commit event;
- deterministic replay;
- current-opportunity consumption on success;
- in-memory semantic atomicity.

Patch 0012 does not implement durable persistence/recovery, branch/canon UX, retcon/rehearsal, full evolved-state Access/Context migration, next-Director selection, Scene loop, Observation, World Resolver, provider execution, WinUI, Windows AI/NPU, packaging, WACK, or Store behavior.

## 3. Source checkpoint must precede Access/Context

A source-state binding created only after a Take exists could silently rebase a stale Take onto a later structurally similar state.

Therefore orchestration captures:

```text
ProductionStateCheckpoint.Capture(currentState)
```

before Access Control/Context composition begin.

The checkpoint retains the exact immutable source-state reference internally and its StateHash. It does not clone state and is not a history event.

A checkpoint whose source state has no Current Opportunity cannot start a Performer opportunity pipeline.

## 4. Causal history versus projection

```text
ValidatedFixture
    -> immutable genesis

ordered E0CausalCommit events
    -> append-only causal Production history / conceptual source of truth

ProductionState
    -> immutable reconstructible current projection
```

ProductionState is current deterministic authority while current, but not a substitute for causal history.

ValidatedFixture is never mutated or treated as evolved state.

## 5. Dependency direction

Namespaces:

```text
Ensemble.E0.Core.Production
Ensemble.E0.Core.CausalCommit
```

Required direction:

```text
Domain / Fixture
    -> Production
        -> StateAuthority
            -> Take
                -> CausalCommit
```

Production must not depend on StateAuthority, Take, or CausalCommit.

StateAuthority may project neutral Production state.

CausalCommit may consume Production plus existing Context/Interpreter/StateAuthority/Take semantics.

## 6. Contract versions

Exact strings:

```text
ProductionStateContracts.StateContractVersion
= "ensemble.e0.production-state.v1"

ProductionStateContracts.StateHashContractVersion
= "ensemble.e0.production-state-hash.sha256.v1"

E0CausalCommitContracts.ContractVersion
= "ensemble.e0.causal-commit.v1"
```

Patch 0012 reuses the existing internal `CanonicalJson` UTF-8 escaping/canonical-string implementation used by ECJ-1. It must not introduce a competing general canonical serializer.

## 7. Existing CommitId and RecordId remain canonical

Reuse existing `CommitId` and `RecordId` strong types.

No new Commit/Record ID type or Core allocator/format.

CommitId and new RecordIds are supplied by higher orchestration and validated for initialization/uniqueness/collision only.

Core does not derive them from clock, randomness, GUID, hash, provider/model identity, process state, TakeId, or StateHash.

A supplied CommitId is effective only if commit succeeds and returns an event.

## 8. StateHash exact type

Patch 0012 introduces:

```text
Ensemble.E0.Core.Production.StateHash
```

Conceptual shape:

```text
public readonly record struct StateHash
- Value : exact lowercase 64-character SHA-256 hex
```

Rules:

- `default(StateHash)` is uninitialized and reading Value fails;
- no public arbitrary-string factory in Patch 0012;
- canonical genesis/commit hashing creates valid StateHash internally;
- equality is exact value equality;
- ToString returns Value;
- persistence parsing is later authority.

StateHash is distinct from FixtureHash, ContextPacketId, CandidateContentHash, ProposalContentHash, TakeId, CommitId, and RecordId.

## 9. Production enum contracts

Exact values:

```text
ProductionRecordDomain
0 Unspecified
1 HistoricalTruth
2 UnresolvedProposition
3 WorldState
4 SceneState
5 CharacterConstitution
6 CharacterDisposition
7 CharacterCircumstance
8 CharacterObservation
9 CharacterKnowledge
10 CharacterBelief
11 CharacterSuspicion
12 CharacterMemory
13 CharacterGoal
14 CharacterClaim
15 Relationship
16 Pressure

ProductionRecordLifecycle
0 Unspecified
1 Active
2 Inactive

ProductionRecordProtection
0 Unspecified
1 None
2 SystemImmutable
3 CreatorLocked
```

Unspecified/undefined is invalid.

StateAuthority has an exact tested one-to-one mapping to its existing Patch 0010 enums; Patch 0010 enum values/public meaning remain unchanged.

## 10. Production Character and record model

Immutable Character identity:

```text
ProductionCharacter
- CharacterId
- DisplayName
```

Immutable record base:

```text
ProductionRecord
- RecordId
- Domain
- Lifecycle
- Protection
- Text
- Provenance : canonical ImmutableArray<RecordId>
```

Scoped sealed forms:

```text
GlobalProductionRecord
CharacterProductionRecord - SubjectCharacterId
RelationshipProductionRecord - SubjectCharacterId - TargetCharacterId
```

No public arbitrary record constructors/factories. Genesis/commit/replay own construction.

RecordIds remain globally unique across active and inactive records.

Inactive records remain present for reconstruction and ID non-reuse.

`Provenance` remains the exact support-reference concept inherited from fixture/proposal semantics. Supersede/Deactivate ExistingRecordId remains transition lineage and is not silently inserted into `SupportingRecordIds` or record Provenance.

## 11. ProductionState surface

Immutable sealed conceptual public surface:

```text
ProductionState
- ContractVersion
- StateHash
- OriginFixtureId
- OriginFixtureFamilyId
- OriginFixtureVersion
- OriginFixtureHash
- SceneId
- Characters
- RosterCharacterIds
- CurrentOpportunityCharacterId : CharacterId?
- Records
```

Internal derived committed-CommitId/committed-TakeId indexes support deterministic duplicate rejection but are not creative-history substitutes.

No public state mutation methods.

No provider/model/prompt/credential/diagnostic/rationale/chain-of-thought fields.

## 12. Canonical genesis

Public genesis boundary:

```text
ProductionState.Initialize(
    ValidatedFixture fixture,
    ImmutableArray<RecordId> creatorLockedRecordIds)
```

Genesis preserves:

- exact FixtureId/family/version and `FixtureHash.Compute(fixture)`;
- exact SceneId;
- exact canonical Character ID/display names;
- exact canonical roster;
- Current Opportunity = fixture.InitialOpportunity;
- every genesis record Active;
- HistoricalTruth, CharacterConstitution, CharacterObservation SystemImmutable;
- valid supplied creator locks CreatorLocked unless already stronger SystemImmutable;
- all other records None-protected;
- exact text and existing provenance.

One internal neutral genesis mapping must serve Production initialization and existing fixture-derived StateAuthority snapshot semantics; do not maintain two independent domain/protection rule tables.

Existing `StateAuthoritySnapshot.Bind(fixture, locks)` must preserve its public StateAuthorityException behavior even if it shares neutral mapping internally.

No effective CommitId/TakeId exists at genesis.

## 13. ProductionStateException

Expected Production initialization/state-contract failures use:

```text
public sealed class ProductionStateException : Exception
```

No public constructor/factory. Messages/inner data remain structural/sanitized.

## 14. ProductionStateCheckpoint

Conceptual public surface:

```text
ProductionStateCheckpoint
- StateHash
- SceneId
- CurrentOpportunityCharacterId
```

Construction:

```text
ProductionStateCheckpoint.Capture(ProductionState sourceState)
```

Internally retains the exact immutable ProductionState reference; no deep copy.

The retained reference is not publicly exposed as a bypass state-access API.

## 15. Evolved StateAuthority snapshot

Add canonical overload:

```text
StateAuthoritySnapshot.Bind(ProductionState state)
```

Existing fixture Bind remains public/compatible.

The Production overload maps exact Scene/roster and every record ID/domain/lifecycle/protection/scope, but no record Text/provenance content enters the descriptor.

CausalCommit never implements a second StateAuthority evaluator.

Snapshot semantic equality used by Take binding/commit is exact field equality over SceneId, canonical roster, ordered descriptor count/type/RecordId/domain/lifecycle/protection and subject/target where applicable. The comparison is one shared internal StateAuthority helper; Patch 0012 introduces no redundant public SnapshotHash.

## 16. E0TakeStateBinding

Patch 0011 E0Take is not modified.

Immutable binding:

```text
E0TakeStateBinding
- SourceStateHash
- TakeId
- SourceContextPacketId
- SourceCharacterId
```

Sole rich construction:

```text
E0TakeStateBinding.Bind(
    ProductionStateCheckpoint sourceCheckpoint,
    ContextPacket sourceContext,
    E0Take take)
```

It validates against checkpoint source state:

- TakeId initialized;
- Performance ContextPacketId == source ContextPacketId;
- Performance SubjectCharacterId == source Context subject;
- source Context subject == source Context opportunity;
- source Character == source-state Current Opportunity;
- source Context Scene == state Scene == proposal SourceSceneId;
- source Context roster == state roster canonically;
- fresh Production-derived StateAuthority snapshot semantically equals Take retained snapshot;
- Take Authority Complete, decision order/count exact, no RequiresReview.

Binding stores the checkpoint StateHash and cannot rebind to a later state.

## 17. Context derivation proof limit

Current Access/Context contracts do not carry StateHash.

Patch 0012 proves structural source-state/Context/Take association and unchanged StateHash, but cannot cryptographically prove every existing ContextPacket field was composed from that state.

Effective E0 orchestration must actually feed the checkpoint state into Access/Context and retain source provenance.

A later Access/Context migration may carry StateHash directly. Patch 0012 does not rewrite existing Context hashes or fabricate a cross-object proof.

## 18. Strict freshness

Successful commit requires:

```text
currentState.StateHash == binding.SourceStateHash
```

Any causal-head change fails closed, even if durable record values later look equal.

No silent re-evaluation or mutation-package substitution.

## 19. Record materialization

Typed caller input:

```text
E0RecordMaterialization
- MutationIndex
- RecordId
- private constructor
- Create(index, recordId)

E0RecordMaterializationSet
- Items : canonical ImmutableArray<E0RecordMaterialization>
- private constructor
- Bind(items)
```

Exact rules:

- one item for each Approved Add;
- one item for each Approved Supersede;
- none for Approved Deactivate;
- none for Rejected;
- indexes unique/ascending;
- RecordIds initialized/unique;
- no collision with active or inactive record ledger;
- no missing/extra item;
- no RecordId reuse.

## 20. Deterministic application

Apply proposal/decision pairs in exact mutation-index order.

```text
Rejected -> no effect
Approved -> exact typed effect
RequiresReview -> fail closed
```

### Add

Create one Active None-protected record with supplied new RecordId and exact proposal domain/text/subject/target. Record Provenance = exact proposal SupportingRecordIds.

### Supersede

Require exact target Active/domain/subject/target; mark it Inactive without rewriting it; create one Active None-protected replacement with supplied new RecordId and exact proposal semantics. Replacement Provenance = exact proposal SupportingRecordIds.

ExistingRecordId is represented separately in the applied effect as transition lineage.

### Deactivate

Require exact target Active/domain/subject/target; mark it Inactive; create no new record.

SupportingRecordIds were validated by StateAuthority against the source snapshot. Since new materialized RecordIds did not exist in the source snapshot, they cannot be proposal supporting references in the same commit. New record provenance therefore points only to pre-existing records and preserves the acyclic provenance direction established by the source graph; implementation must still test the resulting Production provenance graph remains acyclic.

Structural checks are defense in depth, not a second approval policy.

## 21. Applied effects

Immutable event effects:

```text
abstract E0AppliedMutationEffect
- MutationIndex
- Domain

E0AppliedAddEffect
- NewRecordId

E0AppliedSupersedeEffect
- ExistingRecordId
- NewRecordId

E0AppliedDeactivateEffect
- ExistingRecordId
```

Exactly one effect per retained Approved decision; none per Rejected decision.

Exact E0Take + effects reconstruct application without duplicating proposal text.

## 22. Historical texture and Current Opportunity

Successful event retains exact Accepted E0Take, so zero-mutation and all-Rejected-consequence Takes may still commit exact Performance history with zero effects.

Result StateHash changes because causal head changed.

On success:

```text
ResultState.CurrentOpportunityCharacterId = null
```

The source opportunity is consumed. Patch 0012 does not call Director or infer next opportunity from Candidate control.

Failure returns no result state/event; the caller's immutable source state remains unchanged.

## 23. E0CausalCommit event and result

Exact immutable event surface:

```text
E0CausalCommit
- ContractVersion
- CommitId
- ParentStateHash
- ResultStateHash
- Take : exact Accepted E0Take
- CommittedOpportunityCharacterId
- AppliedEffects : canonical ImmutableArray<E0AppliedMutationEffect>
```

No public event constructor/factory outside deterministic commit authority.

Result:

```text
E0CausalCommitResult
- Commit
- ResultState
```

Result construction is internal to CausalCommit.

## 24. Sole new-commit authority

```text
DeterministicCausalCommit.Commit(
    CommitId commitId,
    ProductionState currentState,
    E0TakeStateBinding binding,
    E0Take take,
    E0RecordMaterializationSet materializations)
    -> E0CausalCommitResult
```

It validates all preconditions and constructs complete effects/ledger/indexes/hash/event/state before returning.

No input mutation; no second Apply/Accept/Promote factory.

Eligibility includes Accepted Take, Complete/no-review authority, exact binding/current StateHash, current opportunity == Performance subject, exact current/take snapshot equality, exact materializations, and no already-effective CommitId/TakeId.

## 25. Canonical Replay

Reconstructibility is explicit:

```text
DeterministicCausalCommit.Replay(
    ProductionState parentState,
    E0CausalCommit committedEvent)
    -> ProductionState
```

Replay does not create/authorize a new causal event. It verifies and reconstructs one already-effective event.

Replay requires exact ParentStateHash, Accepted Take, duplicate-ID absence, exact approved-effect set, valid transitions/RecordIds, and recomputed ResultStateHash equality.

Commit and Replay share one internal deterministic transition/canonicalization engine.

## 26. In-memory semantic atomicity

Patch 0012 writes no filesystem/database/network state. Core objects are immutable.

Failure before successful result exposes no partially mutated ProductionState and no effective E0CausalCommit.

This is in-memory semantic atomicity only.

E0 Harness validity additionally requires that adopting ResultState also retain/append its matching E0CausalCommit. Discarding the event while continuing from its projection is a causal-history hard-gate failure.

Crash-safe durable transaction/recovery remains later.

## 27. Canonical Production projection JSON

StateHash uses the existing internal `CanonicalJson` string escaping and UTF-8 rules.

The canonical Production projection is a JSON object with **exact property order**:

```text
1 schemaVersion
2 origin
3 sceneId
4 characters
5 roster
6 currentOpportunityCharacterId
7 records
8 effectiveCommitIds
9 committedTakeIds
```

`origin` exact order:

```text
1 fixtureId
2 fixtureFamilyId
3 fixtureVersion
4 fixtureHash
```

`characters` sorted by CharacterId ordinal; each object exact order:

```text
1 id
2 displayName
```

`roster` sorted CharacterId ordinal.

`currentOpportunityCharacterId` is JSON null or exact ID string.

`records` sorted RecordId ordinal; each object exact order:

```text
1 id
2 domain
3 lifecycle
4 protection
5 subjectCharacterId
6 targetCharacterId
7 text
8 provenance
```

Domain canonical strings:

```text
historicalTruth
unresolvedProposition
worldState
sceneState
characterConstitution
characterDisposition
characterCircumstance
characterObservation
characterKnowledge
characterBelief
characterSuspicion
characterMemory
characterGoal
characterClaim
relationship
pressure
```

Lifecycle strings: `active | inactive`.

Protection strings: `none | systemImmutable | creatorLocked`.

Subject/target are explicit null where not applicable.

Provenance IDs sorted ordinal.

EffectiveCommitIds and committedTakeIds sorted ordinal.

No StateHash field appears inside the projection being hashed.

## 28. Canonical causal commit payload JSON

Canonical payload property order:

```text
1 schemaVersion
2 commitId
3 take
4 committedOpportunityCharacterId
5 appliedEffects
```

`take` exact property order:

```text
1 contractVersion
2 takeId
3 disposition
4 performanceSubjectCharacterId
5 performanceContextPacketId
6 candidateContentIdentityContract
7 candidateContentHash
8 proposalContentIdentityContract
9 proposalContentHash
10 sourceSceneId
11 authorityContractVersion
12 authorityStatus
13 authorityPolicy
14 authorityReviewSet
15 authorityDecisions
```

Take disposition canonical string is exactly `accepted` for a commit payload.

Authority status is exactly `complete`.

`authorityPolicy` order:

```text
1 contractVersion
2 autoApproveDomains
```

AutoApproveDomains use the existing Patch 0009 lower-camel mutation-domain strings and remain canonical in existing policy order.

`authorityReviewSet` order:

```text
1 contractVersion
2 proposalContentIdentityContract
3 proposalContentHash
4 choices
```

Choices preserve canonical mutation-index order; each exact object:

```text
1 mutationIndex
2 choice
```

Choice strings: `approve | reject`.

`authorityDecisions` preserve exact mutation-index order; each exact object:

```text
1 mutationIndex
2 disposition
3 reasons
```

Disposition strings permitted in a committed payload: `approved | rejected`.

Reason strings are exact lower-camel mappings of current Patch 0010 reason codes:

```text
supportingRecordMissing
existingRecordMissing
existingRecordInactive
existingRecordDomainMismatch
existingRecordSubjectMismatch
existingRecordTargetMismatch
existingRecordProtected
conflictingExistingRecordTarget
mandatoryReview
policyReviewRequired
policyAutoApproved
explicitReviewApproved
explicitReviewRejected
```

Reasons preserve the exact canonical retained order.

`appliedEffects` sorted mutation index; each exact object order:

```text
1 mutationIndex
2 domain
3 kind
4 existingRecordId
5 newRecordId
```

Domain uses Patch 0009 lower-camel mutation-domain strings.

Kind: `add | supersede | deactivate`.

Existing/new RecordIds use explicit JSON null when not applicable.

No Candidate/proposal raw text is duplicated into the commit payload; exact content is already retained by E0Take and bound through existing canonical content hashes. Performance subject/context identity is included explicitly because Candidate content identity alone is not attempt/source-context identity.

## 29. Exact StateHash envelopes

Genesis StateHash is SHA-256 over canonical UTF-8 JSON with exact order:

```json
{
  "hashContract":"ensemble.e0.production-state-hash.sha256.v1",
  "kind":"genesis",
  "projection":{...canonical production projection...}
}
```

Post-commit StateHash is SHA-256 over canonical UTF-8 JSON with exact order:

```json
{
  "hashContract":"ensemble.e0.production-state-hash.sha256.v1",
  "kind":"commit",
  "parentStateHash":"<64-lower-hex>",
  "commitPayload":{...canonical causal payload...},
  "resultProjection":{...canonical production projection...}
}
```

These envelopes replace ad-hoc string concatenation/length framing and reuse CanonicalJson escaping rules.

Any unsupported enum/type/null/collection shape causes canonicalization failure rather than fallback serialization.

Different valid CommitId or materialized RecordId intentionally changes causal payload and resulting StateHash.

## 30. Duplicate effective identity and replay indexes

ProductionState internally projects effective CommitIds and committed TakeIds.

Commit/Replay reject duplicate effective CommitId or TakeId.

These indexes are derived from causal history and included in canonical result projection, allowing replay/hash checks to detect index corruption.

They do not claim uniqueness for Rejected/Alternate/failed diagnostic IDs.

## 31. Replay law and event immutability

Given identical genesis fixture, creator-lock set, and ordered E0CausalCommit events, Replay reconstructs exact final Production semantics and StateHash.

Wrong parent, tampered exact Take semantics, tampered policy/review/decisions/effects, altered materialized IDs, or altered ResultStateHash fails closed.

Patch 0012 freezes linear E0 history only. ParentStateHash naturally preserves later branchability without defining branch/canon UX.

No prior event rewrite/delete API.

## 32. Structural validity versus authentication

Core remains synthetic-capable. Hash/type association is not authentication.

Core does not authenticate RunId, provider/attempt, context-disclosure path, Integrity assessor, StateAuthority reviewer/policy human provenance, Take-disposition intervention, CommitId allocator, or RecordId allocator.

Effective E0 provenance must associate at minimum RunId, CommitId, TakeId, disposition source/intervention, SourceStateHash, SourceContextPacketId, Candidate/proposal identities, authority decisions/reasons/policy/review provenance, configured provider/attempt/context provenance, and supplied RecordId materializations.

A synthetic Core commit is valid for deterministic tests but does not authenticate experimental Production evidence by itself.

Creative causal history remains independent from deletable provider diagnostics.

## 33. Protection and provenance preservation

Checkpoint + binding + strict StateHash + exact StateAuthority snapshot equality ensure application occurs against the unchanged authority projection under which the Take was evaluated.

CausalCommit cannot override SystemImmutable/CreatorLocked protection or invent approval.

New record Provenance is exactly the proposal SupportingRecordIds. ExistingRecordId transition lineage is separately recoverable from Supersede/Deactivate effects.

Because supporting IDs must exist in the source snapshot, same-commit new IDs cannot become supporting provenance. Resulting provenance graph must remain acyclic and every support reference must resolve to an existing Production record.

## 34. Exception domains

Production initialization/state expected failures:

```text
ProductionStateException
```

Checkpoint/binding/materialization/commit/replay expected failures:

```text
E0CausalCommitException
```

Both public sealed/catchable, no public constructor/factory.

Existing upstream public exception domains remain compatible.

Exception representation is structural/sanitized and must not leak Candidate text, Context prose, mutation text, provider content, credentials, arbitrary user content, or diagnostic bodies.

Expected upstream sanitized domain exceptions may be retained only where explicitly normalized; no arbitrary catch-all may relabel unexpected programming/runtime failures.

Failure returns no fallback state/event.

## 35. Determinism, memory, ARM64 suitability

No network, filesystem, clock, randomness, provider API, GPU, NPU, polling, background thread, or global mutable state.

Checkpoint retains one immutable state reference rather than cloning.

Commit/Replay structurally share unchanged immutable records and allocate only changed/new records plus bounded event/result metadata.

Canonical JSON/hashing/application are linear in bounded E0 state/mutation/history-index size and execute only at initialization/commit/replay boundaries. No idle work is introduced. This is architecture suitability, not measured power evidence.

## 36. Required implementation tests

Future implementation must prove at minimum:

1. exact public namespaces/types/version strings;
2. existing CommitId/RecordId reused; no allocator/new ID type;
3. StateHash default invalid/no public arbitrary factory/deterministic canonical creation;
4. exact Production enum numeric values/default/undefined invalid;
5. ProductionState/records/checkpoint/events/results immutable/closed construction;
6. single canonical neutral genesis mapping;
7. genesis exact origin/Scene/Characters/roster/opportunity/records/provenance/protection;
8. global RecordId uniqueness active+inactive;
9. exact genesis canonical JSON bytes and fixed hash oracle;
10. checkpoint retains exact source state reference and cannot start from no-opportunity state;
11. fixture-derived and Production-derived StateAuthority snapshots equivalent at genesis;
12. evolved snapshot exact lifecycle/protection/domain/scope mapping;
13. shared exact snapshot semantic comparison; no SnapshotHash duplication;
14. binding validates checkpoint/Context/Take/snapshot and cannot rebase later;
15. current Context full-content StateHash proof is explicitly not claimed;
16. Rejected/Alternate cannot commit;
17. zero-mutation Accepted commits exact Performance, zero effects, changed history-sensitive StateHash;
18. all-Rejected Accepted likewise commits Performance only;
19. Approved Add exact record/provenance;
20. Approved Supersede exact old inactive/new active, exact support provenance, ExistingRecordId only in transition effect;
21. Approved Deactivate exact inactive/no replacement;
22. mixed decisions apply every/only Approved;
23. one effect per Approved and none per Rejected;
24. materialization exactly Approved Add/Supersede only;
25. missing/extra/duplicate/colliding/new-ID reuse fails;
26. result provenance references resolve and graph remains acyclic;
27. stale StateHash fails/no result;
28. later history with same durable record values never resurrects old StateHash;
29. duplicate effective CommitId/TakeId fails;
30. success consumes Current Opportunity/no next opportunity;
31. failure leaves caller state/opportunity unchanged;
32. event retains exact Take and CommitId/TakeId association;
33. exact canonical Production JSON property/order/string/null rules;
34. exact canonical commit payload bytes including subject/context/policy/review/decisions/effects;
35. fixed post-commit StateHash oracle;
36. deterministic identical Commit inputs/IDs equivalent;
37. valid different CommitId/materialized ID changes StateHash;
38. Replay exactly reproduces Commit result state/hash;
39. Replay rejects wrong parent/tampered Take/policy/review/decision/effect/hash;
40. Commit/Replay share one transition engine;
41. Harness cannot adopt result state without retaining matching event;
42. no prior event rewrite/delete API;
43. no persistence/provider/next-Director/Scene-loop/UI/AI/NPU authority leaks;
44. sanitized exception representation/no arbitrary catch-all;
45. frozen Patch 0003–0011 identities/regressions remain unchanged except newly introduced Patch 0012 state/hash oracles;
46. full Core regression green;
47. Missing Raft Harness green;
48. generic smoke Harness green.

## 37. Explicit non-goals

No CommitId/RecordId allocator/format, durable database/event-store/recovery transaction, branch DAG/canon lineage, Alternate promotion/retcon/rehearsal UX, final Archive queries, full evolved-state Access/Context migration, proof of full existing Context derivation from StateHash, provider/assessor authentication, next Director opportunity application, Scene loop, Observation, World Resolver, final consequence/Another Take/Take a Seat UX, Windows AI Foundry/NPU, WinUI, MSIX, WACK, or Store certification.

## 38. Principal law

> An E0 causal commit can make history effective only from the exact immutable source Production checkpoint captured before Access/Context/Performance, only for an immutable Accepted Take, and only while that history-sensitive StateHash remains current. It commits the exact Performance plus every retained Approved consequence together, creates no effect for retained Rejected consequences, consumes the source opportunity, returns one immutable causal event plus one reconstructible projection, and returns neither on failure. Commit and Replay share one deterministic transition engine. Canonical JSON binds causal payload and result projection into StateHash. Causal events remain the conceptual source of truth; the Take is never rewritten; next-opportunity and durable persistence remain later authority.

## 39. Approval / implementation gate

This blueprint is architecture only.

Before implementation:

1. recursively adversarial-audit Proposal 0.4 against Blueprint 0.1, approved Patches 0003–0011, current source/tests, hygiene law, dependency direction, genesis mapping, StateAuthority ownership, Take immutability, checkpoint/freshness, Context identity limit, RecordId materialization, supporting provenance versus transition lineage, canonical JSON byte identity, replay, duplicate identities, opportunity consumption, provenance/authentication, invalid-state construction, memory/ARM64 suitability, experiment isolation, future persistence/branch separation;
2. restart after every material correction;
3. require one full pass with zero material corrections and zero worthwhile architectural improvements;
4. obtain explicit user approval;
5. create fresh-chat implementation handoff;
6. write no Patch 0012 executable code during architecture phase.
