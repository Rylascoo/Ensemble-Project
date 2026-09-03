# H1 Patch 0012 — E0 Atomic Causal Commit Contract

Status: blueprint proposal 0.5 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
Parent repository checkpoint: `main` at `20b26711ceffa17e0543ca3fc180c9acf69f5e45`
Parent machine-tested executable/test authority: H1 Patch 0011 at `4250011c167cd9850ad891aaea4ee053216cf135`
Branch: `h1-patch-0012-atomic-causal-commit-blueprint`

## 1. Purpose and frozen law

Patch 0012 defines the E0 atomic causal-commit boundary after immutable Take semantics.

Blueprint 0.1 freezes:

> Accepted Performance and its authoritative Approved consequences form one atomic causal commit. Either the exact Accepted Take and every Approved consequence become effective together, or neither does.

Patch 0011 additionally freezes that the Take's retained Approved/Rejected consequence package is immutable. Freshness may reject the Take but may not rewrite that package.

The intended boundary is:

```text
immutable ProductionState
    -> pre-pipeline ProductionStateCheckpoint
        -> existing Access / Context / Performance / Integrity / Interpretation / State Authority
            -> immutable E0Take
                -> exact Take/source-state binding
                    -> strict unchanged-StateHash freshness
                        -> deterministic atomic causal commit
                            -> immutable E0CausalCommit
                            -> immutable result ProductionState projection
```

Only `E0TakeDisposition.Accepted` is commit-eligible. Rejected, Alternate, failed-before-Take, and Accepted-but-failed-commit material remains non-effective and has no effective CommitId.

## 2. Scope boundary

Patch 0012 defines only:

- neutral immutable current Production projection;
- history-sensitive StateHash;
- source-state checkpoint captured before the generative pipeline;
- exact Take/source-state binding;
- deterministic new RecordId materialization input;
- Approved Add/Supersede/Deactivate application;
- immutable causal commit event;
- deterministic replay;
- Current Opportunity consumption on successful commit;
- in-memory semantic atomicity.

Patch 0012 does not implement:

- durable persistence/database/event-store/recovery transaction;
- branch/canon lineage, retcon, rehearsal, Alternate promotion, or related UX;
- evolved ProductionState -> Access Control integration;
- CharacterClaim disclosure/history projection into current Context;
- full evolved-state Context composition;
- next-Director opportunity selection/application;
- Scene loop;
- Observation or World Resolver;
- provider/model execution or provenance authentication;
- WinUI, Windows AI Foundry/NPU, packaging, WACK, or Store behavior.

The evolved Access/Context bridge is deliberately deferred because the current Access/Context contract has no CharacterClaim category and no StateHash. Patch 0012 must not silently resolve that ontology/disclosure question merely to claim a multi-turn loop.

## 3. Source checkpoint precedes Access/Context

Creating a source-state binding only after a Take exists could silently rebase a stale Take onto a later structurally similar state.

Therefore orchestration captures:

```text
ProductionStateCheckpoint.Capture(currentState)
```

before Access Control/Context composition begins.

`Capture` is valid only when `currentState.CurrentOpportunityCharacterId` exists. A no-opportunity ProductionState cannot be represented as a Performer source checkpoint.

The checkpoint:

- retains the exact immutable source-state reference internally;
- exposes its StateHash, SceneId, and non-null CurrentOpportunityCharacterId;
- performs no deep copy;
- is not a causal history event;
- cannot be rebound to a different state.

For Patch 0012 executable scope, the current fixture-based Access/Context path may be used only where the fixture and initialized genesis ProductionState represent the same frozen source. Patch 0012 does not claim the current Access path can consume an evolved post-commit ProductionState.

## 4. Causal history versus current projection

```text
ValidatedFixture
    -> immutable genesis input

ordered E0CausalCommit events
    -> append-only causal Production history / conceptual source of truth

ProductionState
    -> immutable reconstructible current projection
```

`ProductionState` is authoritative for deterministic current-state checks while current, but it is not a substitute for the event sequence.

`ValidatedFixture` is never mutated or treated as evolved state.

Blueprint 0.1's origin chronology remains part of immutable fixture identity and is bound by `FixtureHash.Compute(fixture)`. Patch 0012 does not duplicate that chronology into the current Production projection. Post-genesis causal ordering is the ordered E0CausalCommit sequence.

Accepted Performance history is retained by the causal event's exact E0Take. It is not converted into a new `HistoricalTruth` Production record merely to duplicate event history.

## 5. Dependency direction

Namespaces:

```text
Ensemble.E0.Core.Production
Ensemble.E0.Core.CausalCommit
```

Required dependency direction for new Patch 0012 types:

```text
Domain / Fixture
    -> Production
        -> StateAuthority
            -> Take
                -> CausalCommit
```

Production must not depend on Access, Context, StateAuthority, Take, or CausalCommit.

StateAuthority may project neutral Production state.

CausalCommit may consume Production plus existing Context/Interpreter/StateAuthority/Take semantics.

Current Access remains fixture-based in Patch 0012. A later integration patch may depend on Production without reversing Production's dependency direction.

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

Patch 0012 reuses the existing internal `CanonicalJson` UTF-8/canonical-string implementation used by ECJ-1 and existing content hashes. It introduces no competing general canonical serializer.

## 7. Existing CommitId and RecordId remain canonical

Reuse existing `CommitId` and `RecordId` strong types.

Patch 0012 introduces no second Commit/Record ID type and no Core allocator/format.

CommitId and new RecordIds are supplied by higher deterministic orchestration and validated only for:

- initialization;
- required mutation-index association;
- uniqueness;
- collision/non-reuse against effective state.

Core does not derive these IDs from clock, randomness, GUID generation, process state, provider/model identity, TakeId, StateHash, Candidate hash, or proposal hash.

A supplied CommitId becomes an effective causal CommitId only when commit succeeds and returns an `E0CausalCommit`.

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

- `default(StateHash)` is uninitialized and reading `Value` fails;
- no public arbitrary-string factory exists in Patch 0012;
- canonical genesis/commit hashing creates valid StateHash internally;
- equality is exact value equality;
- `ToString()` returns Value;
- persistence parsing/deserialization is later authority.

StateHash is distinct from FixtureHash, ContextPacketId, CandidateContentHash, ProposalContentHash, TakeId, CommitId, and RecordId.

## 9. Production enum contracts

Exact numeric values:

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

Unspecified and undefined values are invalid.

StateAuthority keeps its approved Patch 0010 public enums. There is one exact tested mapping between Production and StateAuthority enums; their public meanings are not rewritten.

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
CharacterProductionRecord
- SubjectCharacterId

RelationshipProductionRecord
- SubjectCharacterId
- TargetCharacterId
```

There are no public arbitrary record constructors/factories. Genesis and the shared deterministic transition engine own record construction.

RecordIds are globally unique across active and inactive records. Inactive records remain present for causal reconstruction and ID non-reuse.

`Provenance` remains the exact support-reference concept inherited from fixture/proposal semantics. Supersede/Deactivate `ExistingRecordId` is transition lineage, not `SupportingRecordIds`, and must not be silently inserted into record Provenance.

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

Internal replay indexes retain effective CommitIds and committed TakeIds for deterministic duplicate rejection. They are derived caches of the causal event sequence, not creative-state semantics and not a replacement for event history.

The indexes are intentionally excluded from canonical Production projection hashing. The causal parent hash + new commit payload already binds ordered history; repeatedly hashing all prior CommitId/TakeId values would duplicate that authority and create growing per-commit work.

No public state mutation methods.

No provider/model/prompt/credential/diagnostic/rationale/chain-of-thought fields.

## 12. Canonical genesis

Public genesis boundary:

```text
ProductionState.Initialize(
    ValidatedFixture fixture,
    ImmutableArray<RecordId> creatorLockedRecordIds)
    -> ProductionState
```

Genesis preserves:

- exact FixtureId/family/version;
- exact `FixtureHash.Compute(fixture)` including frozen origin chronology;
- exact SceneId;
- exact canonical Character IDs/display names;
- exact canonical roster;
- Current Opportunity = `fixture.InitialOpportunity`;
- every genesis record Active;
- HistoricalTruth, CharacterConstitution, and CharacterObservation as SystemImmutable;
- valid creator locks as CreatorLocked unless stronger SystemImmutable already applies;
- all other records None-protected;
- exact record text and exact existing provenance.

One internal neutral genesis projection/mapping must serve both Production initialization and fixture-derived StateAuthority snapshot semantics. Patch 0012 must not maintain separate competing domain/protection tables.

Existing `StateAuthoritySnapshot.Bind(fixture, locks)` retains its public behavior/exception domain even if its internal projection is refactored to share neutral mapping.

No effective CommitId/TakeId exists at genesis. Internal duplicate indexes begin empty.

## 13. ProductionStateException

Expected Production initialization/state-contract failures use:

```text
public sealed class ProductionStateException : Exception
```

It is publicly catchable and sealed with no public constructor/factory. Approved production emitters are Production-owned construction/validation boundaries only.

Messages and retained inner data remain structural/sanitized and must not expose arbitrary creative/provider content.

## 14. ProductionStateCheckpoint

Conceptual public surface:

```text
ProductionStateCheckpoint
- StateHash
- SceneId
- CurrentOpportunityCharacterId : CharacterId
```

Construction:

```text
ProductionStateCheckpoint.Capture(ProductionState sourceState)
```

Capture fails if source state is null, invalid, or has no Current Opportunity.

Internally it retains the exact immutable ProductionState reference without exposing that reference publicly.

Checkpoint creation neither mutates state nor creates causal history.

## 15. Evolved StateAuthority snapshot

Patch 0012 adds the narrow canonical evolved-state overload:

```text
StateAuthoritySnapshot.Bind(ProductionState state)
```

The existing fixture-based Bind remains public/compatible for frozen regressions.

The Production overload maps exact:

- SceneId;
- canonical roster;
- every record RecordId;
- Production domain -> exact StateAuthority domain;
- lifecycle;
- protection;
- Character subject/Relationship subject+target scope.

Record Text/provenance does not enter the StateAuthority descriptor, preserving Patch 0010's authority surface.

Snapshot semantic equality required by Take binding/commit is exact field equality over Scene, canonical roster, ordered descriptor count/type/RecordId/domain/lifecycle/protection, and subject/target where applicable.

One shared internal StateAuthority semantic-comparison helper owns this equality. Patch 0012 introduces no redundant public SnapshotHash.

CausalCommit never implements a second StateAuthority evaluator or policy.

## 16. E0TakeStateBinding

Patch 0011 E0Take is not modified.

Immutable binding surface:

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

It validates against the checkpoint's exact retained source state:

- non-null inputs;
- initialized TakeId/source IDs;
- Performance ContextPacketId == source ContextPacketId;
- Performance SubjectCharacterId == source Context subject;
- source Context subject == source Context opportunity;
- source Character == checkpoint Current Opportunity;
- source Context Scene == ProductionState Scene == proposal SourceSceneId;
- source Context roster == ProductionState roster canonically;
- fresh Production-derived StateAuthority snapshot semantically equals Take retained snapshot;
- Take authority status Complete;
- decision count/order exact;
- no RequiresReview decision.

The binding stores the checkpoint StateHash and cannot rebind the same Take to a later state.

## 17. Current Context derivation proof limit

Current Access/Context contracts do not carry StateHash and Access currently reads `ValidatedFixture` rather than `ProductionState`.

Therefore Patch 0012 can prove:

- structural source Context/Take association;
- source opportunity/scene/roster association;
- Take StateAuthority snapshot equality to the bound ProductionState;
- exact SourceStateHash freshness at commit.

It cannot prove that every ContextPacket field was derived from that ProductionState.

For the currently implemented fixture path, a valid Patch 0012 reference test may bind a Context produced from the same immutable fixture used to initialize the genesis ProductionState. That is a genesis-equivalent orchestration proof, not evolved-state Context support.

Before an evolved second-turn E0 run can be authoritative, a later approved integration patch must define ProductionState -> Access projection and resolve how new E0 domains such as CharacterClaim and accepted recent Performance history participate in bounded Context. Patch 0012 must not pre-decide those disclosure semantics.

## 18. Strict freshness

Successful commit requires:

```text
currentState.StateHash == binding.SourceStateHash
```

Any causal-head change fails closed, even if current durable record values later appear equal.

Patch 0012 does not silently rerun StateAuthority against changed state, declare states "close enough," or substitute a different Approved/Rejected package under the same immutable Take.

A changed state requires failure under the existing Take or a later explicitly approved new-evaluation/new-Take path.

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

`Bind` validates item structural/canonical form. Exact Take-specific requiredness is validated by Commit/Replay.

Exact commit rules:

- one item for each Approved Add;
- one item for each Approved Supersede;
- none for Approved Deactivate;
- none for Rejected;
- mutation indexes unique and ascending;
- RecordIds initialized and unique;
- no collision with any active or inactive Production record;
- no missing/extra item;
- no RecordId reuse.

No materialization is generated inside Core.

## 20. Shared deterministic transition engine

Commit and Replay share one internal deterministic transition engine.

It consumes:

```text
parent ProductionState
+ exact Accepted E0Take
+ exact E0RecordMaterializationSet
```

and constructs the candidate result projection without mutating inputs.

For each mutation index in exact retained proposal/decision order:

```text
Rejected
    -> no state effect

Approved
    -> apply exact corresponding typed proposal mutation

RequiresReview
    -> invalid for commit/replay; fail closed
```

The transition engine does not make a new approval decision.

### Add

Create one new Active None-protected Production record with:

- supplied materialized RecordId;
- exact proposal domain;
- exact proposal text;
- exact subject/target scope;
- exact proposal SupportingRecordIds as Provenance.

### Supersede

Require the exact ExistingRecordId target to be Active and match the domain/subject/target semantics already approved under the unchanged source snapshot.

Then:

- retain the existing record unchanged except Lifecycle -> Inactive;
- create one new Active None-protected replacement with supplied new RecordId and exact proposal domain/text/scope;
- replacement Provenance = exact proposal SupportingRecordIds.

ExistingRecordId remains transition lineage available from the Take proposal. It is not added to support Provenance unless the Interpreter itself explicitly supplied it as a SupportingRecordId.

### Deactivate

Require the exact ExistingRecordId target to be Active and match the approved domain/subject/target semantics.

Then change only Lifecycle -> Inactive. No replacement record and no new RecordId.

SupportingRecordIds were validated against the source snapshot. New materialized IDs did not exist there and therefore cannot be same-commit support references. Resulting support provenance must still be validated as resolving and acyclic.

These application checks are deterministic defense in depth, not a second StateAuthority policy.

## 21. Minimal causal event: do not duplicate Take authority

The exact E0Take already retains:

- exact CandidatePerformance;
- exact StateInterpretationProposal;
- fresh canonical StateAuthorityEvaluation with policy/review/decisions;
- TakeId;
- Accepted disposition.

Therefore an `AppliedEffects` hierarchy would duplicate domain/kind/ExistingRecordId/approval semantics already fixed by the Take.

The only successful-commit application data not recoverable from the Take is the caller-supplied new RecordId materialization for Approved Add/Supersede mutations.

Patch 0012 therefore uses a minimal event:

```text
E0CausalCommit
- ContractVersion
- CommitId
- ParentStateHash
- ResultStateHash
- Take : exact Accepted E0Take
- RecordMaterializations : exact canonical E0RecordMaterializationSet
```

There is no `AppliedEffects` public hierarchy and no duplicated committed-opportunity field. The committed source Character is exactly `Take.Performance.SubjectCharacterId`; Commit/Replay require the parent Current Opportunity to equal it.

No public event constructor/factory exists outside deterministic commit authority.

## 22. Historical texture and Current Opportunity

A successful event retains the exact Accepted E0Take.

Therefore these are valid commits:

```text
Accepted Take + zero mutations
Accepted Take + all retained decisions Rejected
```

Both create Performance history with zero durable record effects.

Result StateHash still changes because the causal head/commit payload changed.

On every successful source commit:

```text
ResultState.CurrentOpportunityCharacterId = null
```

The source opportunity is consumed.

Patch 0012 does not call Director, infer next opportunity from Candidate control, or expose an arbitrary ProductionState opportunity setter.

Failure returns no event/result state. The caller's immutable source state remains unchanged.

## 23. Sole new-commit authority

Result:

```text
E0CausalCommitResult
- Commit
- ResultState
```

Result construction is internal to CausalCommit.

Sole new-event authority:

```text
DeterministicCausalCommit.Commit(
    CommitId commitId,
    ProductionState currentState,
    E0TakeStateBinding binding,
    E0Take take,
    E0RecordMaterializationSet materializations)
    -> E0CausalCommitResult
```

Commit validates at minimum:

- non-null initialized current state/inputs;
- initialized CommitId;
- binding belongs to exact Take;
- Take disposition == Accepted;
- retained authority is terminal Complete/no RequiresReview;
- currentState.StateHash == binding.SourceStateHash;
- current opportunity exists and equals Performance subject;
- current Production-derived StateAuthority snapshot semantically equals Take snapshot;
- exact materialization set for Approved Add/Supersede only;
- no effective duplicate CommitId;
- no effective duplicate TakeId;
- deterministic transition/provenance invariants.

All candidate result objects/hash/event fields are constructed successfully before return.

There is no second Apply/Accept/Promote/new-event factory.

## 24. Canonical Replay

Reconstructibility is explicit:

```text
DeterministicCausalCommit.Replay(
    ProductionState parentState,
    E0CausalCommit committedEvent)
    -> ProductionState
```

Replay verifies and reconstructs an already-effective in-memory event. It does not create or authorize a new event/CommitId.

Replay requires:

- event contract current;
- exact ParentStateHash equality;
- exact Accepted Take structural invariants;
- parent Current Opportunity == Take Performance subject;
- parent Production-derived StateAuthority snapshot == Take snapshot;
- exact materialization set required by retained Approved Add/Supersede mutations;
- no duplicate effective CommitId/TakeId in parent indexes;
- shared deterministic transition success;
- recomputed ResultStateHash == event.ResultStateHash.

Commit and Replay use the same transition and canonicalization implementation.

Patch 0012 does not define persisted-event parsing. Closed event construction makes arbitrary external event manufacture unavailable through normal public C# APIs; later persistence must define its own strict deserialization/authentication boundary.

## 25. In-memory semantic atomicity

Patch 0012 writes no filesystem/database/network state. Core objects are immutable.

Failure before successful result exposes no partially mutated ProductionState and no effective E0CausalCommit.

This is in-memory semantic atomicity only.

E0 Harness validity additionally requires that adopting `ResultState` also retain/append the matching `E0CausalCommit`. Continuing from a projection while discarding its event is a causal-history hard-gate failure.

Crash-safe durable transaction/recovery remains later authority.

## 26. Canonical Production projection JSON

StateHash uses existing internal `CanonicalJson` string escaping and UTF-8 rules.

Canonical Production projection exact property order:

```text
1 schemaVersion
2 origin
3 sceneId
4 characters
5 roster
6 currentOpportunityCharacterId
7 records
```

`origin` exact order:

```text
1 fixtureId
2 fixtureFamilyId
3 fixtureVersion
4 fixtureHash
```

`characters` sorted by CharacterId ordinal. Each exact object order:

```text
1 id
2 displayName
```

`roster` sorted CharacterId ordinal.

`currentOpportunityCharacterId` is JSON null or exact ID string.

`records` sorted RecordId ordinal. Each exact object order:

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

Canonical domain strings:

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

Lifecycle: `active | inactive`.

Protection: `none | systemImmutable | creatorLocked`.

Subject/target use explicit JSON null when not applicable.

Provenance IDs are canonical ordinal.

No StateHash, effective CommitId index, or committed TakeId index appears inside this projection.

The internal duplicate indexes are reconstructed from genesis + ordered events and validated by closed construction/replay; they are not part of creative current-state semantics.

## 27. Canonical Take payload used by causal identity

The causal payload must bind the exact Take semantics without duplicating raw Candidate/proposal text already covered by frozen content hashes.

Exact `take` property order:

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

Rules:

- disposition is exactly `accepted`;
- authority status exactly `complete`;
- candidate content identity/hash is the Patch 0008 identity retained through the canonical Take association;
- CandidateContentHash already covers Candidate contract version, subject/context IDs, exact VisibleText, addressed IDs, and nomination under its frozen contract;
- ProposalContentHash already covers proposal contract version, Candidate identity/hash, source Scene, exact mutation domain/operation/scope/existing ID/text/supporting IDs under its frozen contract;
- source Scene remains explicit;
- parent StateHash plus mandatory snapshot semantic equality binds the exact source authority snapshot without introducing a redundant SnapshotHash.

`authorityPolicy` exact order:

```text
1 contractVersion
2 autoApproveDomains
```

AutoApproveDomains use existing Patch 0009 lower-camel mutation-domain strings in canonical existing policy order.

`authorityReviewSet` exact order:

```text
1 contractVersion
2 proposalContentIdentityContract
3 proposalContentHash
4 choices
```

Choice exact object order:

```text
1 mutationIndex
2 choice
```

Choice strings: `approve | reject`.

Choices preserve canonical mutation-index order.

`authorityDecisions` exact object order:

```text
1 mutationIndex
2 disposition
3 reasons
```

Committed disposition strings: `approved | rejected` only.

Reason strings exactly map current Patch 0010 values:

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

Reasons preserve retained canonical order.

## 28. Canonical causal commit payload JSON

Exact property order:

```text
1 schemaVersion
2 commitId
3 take
4 recordMaterializations
```

`recordMaterializations` preserves canonical ascending MutationIndex order. Each exact object:

```text
1 mutationIndex
2 recordId
```

Only Approved Add/Supersede mutations appear.

No AppliedEffects, raw Candidate text, raw proposal text, duplicate ExistingRecordId, or duplicate committed-opportunity field appears in the payload.

Every transition effect is deterministically recoverable from exact Take + materializations.

## 29. Exact StateHash envelopes

Genesis StateHash is SHA-256 over canonical UTF-8 JSON exact order:

```json
{
  "hashContract":"ensemble.e0.production-state-hash.sha256.v1",
  "kind":"genesis",
  "projection":{...canonical Production projection...}
}
```

Post-commit StateHash is SHA-256 over canonical UTF-8 JSON exact order:

```json
{
  "hashContract":"ensemble.e0.production-state-hash.sha256.v1",
  "kind":"commit",
  "parentStateHash":"<64-lower-hex>",
  "commitPayload":{...canonical causal commit payload...},
  "resultProjection":{...canonical Production projection...}
}
```

No ad-hoc concatenation or competing serializer.

Unsupported enum/type/null/default collection state fails canonicalization rather than receiving a fallback representation.

Different valid CommitId or materialized RecordId intentionally changes the causal payload and resulting StateHash.

Because ParentStateHash recursively binds prior causal history, a later state that returns to identical durable record values still has a different StateHash.

## 30. Duplicate effective identities are derived replay indexes

ProductionState internally retains effective CommitId and committed TakeId indexes solely to reject accidental effective identity reuse efficiently.

Commit/Replay reject any effective duplicate CommitId or TakeId.

The indexes are updated deterministically after successful transition and reconstructed during ordered replay.

They are not serialized into the canonical Production projection in Patch 0012 because:

1. parent StateHash already binds ordered prior history;
2. commit payload binds the new CommitId/TakeId;
3. hashing the full growing sets each commit duplicates causal identity work;
4. closed ProductionState construction/replay owns index integrity;
5. avoiding repeated full-history-set hashing reduces unnecessary CPU/allocation work on ARM64.

This does not weaken the rule that duplicate effective identities fail closed.

## 31. Replay and event immutability law

Given identical:

- immutable genesis fixture;
- exact creator-lock set;
- ordered E0CausalCommit sequence;

Replay reconstructs equivalent final Production semantics and exact StateHash.

No prior event rewrite/delete API exists.

Patch 0012 freezes linear E0 causal history only. ParentStateHash preserves a future branching hook without defining branch/canon UX.

## 32. Structural validity versus authentication

Core remains synthetic-capable. Hash/type association is not authentication.

Patch 0012 Core does not authenticate:

- RunId;
- provider/model/attempt;
- source-context disclosure path;
- Integrity assessor/reviewer provenance;
- StateAuthority human/policy/review provenance beyond exact supplied semantic objects;
- Take-disposition intervention identity;
- CommitId allocator;
- RecordId allocator.

Effective E0 experimental provenance must separately associate at minimum:

- RunId;
- CommitId;
- TakeId;
- disposition source/intervention;
- SourceStateHash;
- SourceContextPacketId;
- Candidate/proposal identities;
- authority decisions/reasons/policy/review provenance;
- configured provider/attempt/context provenance;
- supplied RecordId materializations.

A synthetic Core commit is valid deterministic test material but does not authenticate experimental Production evidence by itself.

Creative causal history remains independent from deletable provider diagnostics.

## 33. Protection and provenance preservation

Checkpoint + binding + strict StateHash + exact StateAuthority snapshot equality ensure application occurs against the unchanged authority projection under which the Take was evaluated.

CausalCommit cannot override SystemImmutable/CreatorLocked protection or invent an approval.

New record Provenance is exactly proposal SupportingRecordIds.

Supersede/Deactivate ExistingRecordId remains exact transition lineage in the retained Take proposal.

Every support reference in result state must resolve to an existing Production record and the resulting provenance graph must remain acyclic.

Same-commit new RecordIds cannot appear as supporting provenance because proposal support was bound before those IDs existed.

## 34. Exception domains

Expected Production initialization/state failures:

```text
ProductionStateException
```

Expected checkpoint/binding/materialization/commit/replay failures:

```text
E0CausalCommitException
```

Both are public sealed/catchable with no public constructor/factory.

Existing upstream exception contracts remain compatible.

Public exception representation must be structural/sanitized and must not expose Candidate text, Context prose, mutation text, provider content, credentials, arbitrary user content, or diagnostic bodies.

Expected upstream sanitized domain exceptions may be retained only where explicitly normalized. No arbitrary catch-all converts unexpected runtime/programming failures into ordinary commit rejection.

Failure returns no fallback state/event.

## 35. Determinism, memory, and ARM64 suitability

Patch 0012 Core uses no network, filesystem, clock, randomness, provider API, GPU, NPU, polling, background thread, or global mutable state.

Checkpoint retains one immutable source-state reference instead of cloning.

Commit/Replay may structurally share unchanged immutable records and allocate only changed/new records plus bounded event/result metadata.

Canonical JSON/hash/application work occurs only at genesis/commit/replay boundaries.

The canonical current projection is O(current record ledger size). Derived CommitId/TakeId history sets are not repeatedly serialized into StateHash, preventing avoidable growth of hash input beyond the causal parent chain itself.

There is no idle work. These are architectural properties, not measured target-device power evidence.

## 36. Required implementation tests

Future implementation must prove at minimum:

1. exact public namespaces/types/version strings;
2. existing CommitId/RecordId reused; no new allocator/ID format;
3. StateHash default invalid, no public arbitrary factory, deterministic internal creation;
4. exact Production enum numeric values; default/undefined invalid;
5. ProductionState/records/checkpoint/event/result construction closed and immutable;
6. one canonical neutral genesis mapping, not competing Production/StateAuthority tables;
7. genesis exact origin/Scene/Characters/roster/opportunity/records/text/provenance/protection;
8. fixture chronology remains bound by OriginFixtureHash but is not duplicated into mutable/current projection;
9. global RecordId uniqueness across active+inactive ledger;
10. exact genesis canonical JSON bytes and fixed StateHash oracle;
11. checkpoint fails without Current Opportunity, retains exact source reference internally, exposes non-null opportunity;
12. fixture-derived and Production-derived StateAuthority snapshots equivalent at genesis;
13. evolved snapshot exact lifecycle/protection/domain/scope mapping;
14. one shared exact snapshot semantic comparator; no SnapshotHash duplication;
15. binding validates checkpoint/Context/Take/snapshot and cannot rebase to a later state;
16. full Context derivation from StateHash is explicitly not claimed;
17. current fixture-based Access is not falsely presented as evolved Production Access;
18. Rejected/Alternate Takes cannot commit;
19. zero-mutation Accepted commits exact Performance with zero record materializations and changed history-sensitive StateHash;
20. all-Rejected Accepted likewise commits Performance only;
21. Approved Add exact new active record and exact support provenance;
22. Approved Supersede exact old inactive/new active, exact support provenance, transition ExistingRecordId retained only by Take semantics;
23. Approved Deactivate exact inactive/no new record;
24. mixed decisions apply every and only Approved mutation;
25. materialization set contains exactly Approved Add/Supersede items;
26. missing/extra/duplicate/colliding/reused RecordId materialization fails;
27. result support references resolve and provenance graph remains acyclic;
28. stale SourceStateHash fails with no result;
29. later history with same durable record values never resurrects an old StateHash;
30. duplicate effective CommitId/TakeId fails using derived indexes;
31. success consumes Current Opportunity and does not establish next opportunity;
32. failure leaves caller state/opportunity unchanged;
33. event exact surface is CommitId/ParentHash/ResultHash/exact Take/exact materializations only;
34. no AppliedEffects hierarchy or duplicated committed opportunity/domain/kind/existing-target authority;
35. exact Candidate and proposal content identities cover the retained text/control/mutation semantics claimed by causal payload;
36. exact canonical Production JSON property/order/string/null rules and no derived ID indexes;
37. exact canonical Take/policy/review/decision payload bytes;
38. exact canonical materialization payload bytes;
39. fixed post-commit StateHash oracle;
40. identical Commit semantic inputs/IDs produce equivalent event/state/hash;
41. valid different CommitId or materialized RecordId changes StateHash;
42. Replay exactly reproduces Commit result state/hash;
43. Replay rejects wrong parent, duplicate identities, invalid Take/materializations, transition failure, or ResultStateHash mismatch;
44. Commit/Replay share one transition/canonicalization engine;
45. Harness cannot adopt result state without retaining matching event;
46. no prior event rewrite/delete API;
47. no persistence/provider/evolved-Access/next-Director/Scene-loop/UI/AI/NPU authority leaks;
48. exception representation sanitized; no arbitrary catch-all;
49. frozen Patch 0003–0011 identities/regressions remain unchanged except intentional new Patch 0012 state/hash oracles;
50. full Core regression suite green;
51. Missing Raft Harness regression green;
52. generic smoke Harness regression green.

## 37. Explicit non-goals

Patch 0012 does not freeze or implement:

- CommitId/RecordId durable allocator/format;
- persisted StateHash parser;
- database/event-store/recovery transaction;
- branch DAG/canon lineage;
- Alternate promotion/retcon/rehearsal;
- Archive/UI query model;
- evolved ProductionState -> CharacterBoundedAccessControl contract;
- CharacterClaim/current-history Context disclosure semantics;
- accepted recent Performance injection into Context;
- direct proof of existing ContextPacket derivation from StateHash;
- provider/assessor/reviewer authentication;
- next Director opportunity state transition;
- Scene loop;
- Observation/World Resolver;
- final Another Take/Take a Seat/consequence UX;
- Windows AI Foundry/NPU;
- WinUI/MSIX/WACK/Store certification.

## 38. Principal law

> An E0 causal commit can make history effective only from the exact immutable source Production checkpoint captured before the generative pipeline, only for an immutable Accepted Take, and only while that history-sensitive StateHash remains current. It commits the exact Performance plus every retained Approved consequence together, creates no effect for retained Rejected consequences, consumes the source opportunity, and returns one immutable minimal causal event plus one reconstructible current projection—or returns neither. The event retains only the exact Take and the new RecordId materializations that cannot be recovered from that Take; Commit and Replay share one deterministic transition engine; canonical JSON binds the causal payload and result projection into StateHash. Causal events remain the conceptual source of truth. Evolved Production -> Access/Context integration, next-opportunity authority, and durable persistence remain later boundaries and must not be falsely claimed by Patch 0012.

## 39. Approval / implementation gate

This blueprint is architecture only.

Before implementation:

1. recursively adversarial-audit Proposal 0.5 against Blueprint 0.1, approved Patches 0003–0011, current source/tests, engineering hygiene, dependency direction, genesis mapping, StateAuthority ownership, Take immutability, checkpoint/freshness, current fixture-only Access boundary, Context identity limit, RecordId materialization, supporting provenance versus transition lineage, minimal event representation, canonical JSON byte identity, StateHash history sensitivity, derived duplicate indexes, replay, opportunity consumption, provenance/authentication, invalid-state construction, memory/ARM64 suitability, experiment isolation, and future persistence/branch/integration separation;
2. restart the audit after every material correction;
3. require one complete final pass with zero material corrections and zero worthwhile architectural improvements;
4. obtain explicit user approval;
5. create a fresh-chat implementation handoff;
6. write no Patch 0012 executable code during architecture phase.
