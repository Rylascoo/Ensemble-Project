# H1 Patch 0012 — E0 Atomic Causal Commit Contract

Status: blueprint proposal 0.3 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
Parent repository checkpoint: `main` at `20b26711ceffa17e0543ca3fc180c9acf69f5e45`
Parent machine-tested executable/test authority: H1 Patch 0011 at `4250011c167cd9850ad891aaea4ee053216cf135`
Branch: `h1-patch-0012-atomic-causal-commit-blueprint`

## 1. Purpose

Define the E0 atomic causal-commit boundary after machine-validated Patch 0011:

```text
immutable ProductionState
    -> pre-pipeline ProductionStateCheckpoint
        -> Access / Context / Performance / Integrity / Interpretation / State Authority
            -> immutable Accepted E0Take
                -> exact Take/source-state binding
                    -> strict unchanged-StateHash freshness
                        -> deterministic all-or-nothing commit
                            -> immutable E0CausalCommit
                            -> immutable result ProductionState projection
```

Patch 0012 exists to make an Accepted Performance and every retained Approved consequence effective together, never separately, while preserving append-only causal reconstruction.

## 2. Frozen authority recovered

Blueprint 0.1 requires:

- `Production State -> deterministic Access Control -> Context Composer -> Performer`;
- append-only causal event history as conceptual source of truth, not mutable snapshots;
- Accepted Take plus committed consequences projects current authoritative state;
- Accepted Performance + every Approved consequence is one atomic causal commit;
- either all commit coherently or neither does;
- accepted historical texture survives even with no durable state mutation;
- Accepted Takes are immutable;
- corrections/retcons/branches/alternates are later explicit history, never silent rewrite;
- committed consequence remains traceable to accepted Performance/authorized cause;
- Production history and diagnostics remain separate.

Patch 0011 further requires:

- only Accepted Take is commit-eligible;
- Rejected/Alternate Takes apply nothing;
- retained Approved set is exact and immutable;
- retained Rejected consequence may never become effective under that Take;
- retained Approved consequence may never be silently omitted from a successful commit;
- freshness may fail a Take but cannot rewrite it;
- failed commit leaves Current Opportunity effective;
- successful source commit may be followed by a separate Director opportunity transition;
- successful CommitId is associated with TakeId;
- failed/non-effective material has no effective causal CommitId.

## 3. Source checkpoint precedes generation

Patch 0012 freezes one orchestration precondition before Access/Context:

```text
ProductionStateCheckpoint.Capture(currentState)
```

The checkpoint retains the exact immutable source-state reference internally and exposes its history-sensitive StateHash/Scene/current opportunity.

It is captured before Access Control and Context composition begin.

This prevents a Take from being rebound after generation to a later state that happens to have similar projected values.

No state copy, clock, randomness, mutable singleton, provider operation, or persistence write occurs.

## 4. Causal event versus projection

The architectural distinction is:

```text
ValidatedFixture
    -> immutable genesis input

ordered E0CausalCommit events
    -> append-only Production causal history / conceptual source of truth

ProductionState
    -> immutable reconstructible current projection
```

A `ProductionState` is authoritative as the current deterministic projection, but it is not the canonical historical record and may be reconstructed from genesis plus causal events.

The original ValidatedFixture is never mutated or mislabeled as evolved Production state.

Patch 0012 performs only in-memory semantic state/event construction. Durable persistence/recovery remains later.

## 5. Dependency direction

Namespaces:

```text
Ensemble.E0.Core.Production
Ensemble.E0.Core.CausalCommit
```

Required dependency direction:

```text
Domain / Fixture
    -> Production
        -> StateAuthority
            -> Take
                -> CausalCommit
```

Production must not depend on StateAuthority, Take, or CausalCommit.

StateAuthority may consume neutral Production projections.

CausalCommit may consume Production + existing Context/Interpreter/StateAuthority/Take semantics.

No circular subsystem authority is permitted.

## 6. Contracts

Exact versions:

```text
ProductionStateContracts.StateContractVersion
= "ensemble.e0.production-state.v1"

ProductionStateContracts.StateHashContractVersion
= "ensemble.e0.production-state-hash.sha256.v1"

E0CausalCommitContracts.ContractVersion
= "ensemble.e0.causal-commit.v1"
```

## 7. Existing CommitId remains canonical

Reuse `Ensemble.E0.Core.Domain.CommitId` exactly.

No second Commit ID type and no Core allocator.

Core receives an initialized supplied CommitId. It is not derived from clock, GUID/randomness, TakeId, StateHash, Candidate/Proposal hash, provider, model, or process state.

A supplied CommitId becomes effective only when deterministic commit succeeds and returns an E0CausalCommit.

A failed attempt may be diagnostic material outside Production history but creates no effective CommitId entry.

## 8. StateHash semantic type

Patch 0012 introduces:

```text
Ensemble.E0.Core.Production.StateHash
```

Conceptual public form:

```text
public readonly record struct StateHash
- Value : lowercase 64-character SHA-256 hex
```

Rules:

- default value is uninitialized and fails when read;
- Patch 0012 exposes no public arbitrary-string `From` factory;
- canonical Production hashing creates StateHash internally;
- equality is exact ordinal semantic value equality;
- `ToString()` returns Value;
- future persistence parse/restore API is not frozen here.

StateHash is distinct from FixtureHash, ContextPacketId, CandidateContentHash, ProposalContentHash, TakeId, CommitId, and RecordId.

## 9. History-sensitive hash law

StateHash must change when causal history advances even if durable projected record values do not.

Therefore an earlier Take cannot become fresh again merely because later history returns the projection to similar values.

Conceptual identity:

```text
genesisHash = SHA256(
    hashContract + canonicalGenesisProjection)

resultHash = SHA256(
    hashContract
    + parentStateHash
    + canonicalCommitPayload
    + canonicalResultProjection)
```

`canonicalCommitPayload` excludes ResultStateHash to avoid circularity.

Canonical bytes use invariant UTF-8, explicit length framing, explicit null markers, ordinal collection order, stable numeric enum values, and no platform/culture/reflection/serializer-default dependence.

Exact field framing remains an audit item and must be frozen before approval.

## 10. Neutral Production enums

Exact proposed values:

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

Default/undefined values are invalid.

StateAuthority projection must map these semantics exactly to existing Patch 0010 domain/lifecycle/protection semantics without changing Patch 0010 public enum meaning.

## 11. Production Character identity

Immutable:

```text
ProductionCharacter
- CharacterId
- DisplayName
```

Character IDs are unique/canonical. Display names remain exact validated fixture text.

Patch 0012 does not mutate Character identity or display name.

## 12. Production record model

Immutable base:

```text
ProductionRecord
- RecordId
- Domain
- Lifecycle
- Protection
- Text
- Provenance : canonical ImmutableArray<RecordId>
```

Scoped forms:

```text
GlobalProductionRecord

CharacterProductionRecord
- SubjectCharacterId

RelationshipProductionRecord
- SubjectCharacterId
- TargetCharacterId
```

Construction is closed to canonical genesis/commit/replay paths.

Every RecordId remains globally unique across active and inactive ledger records.

Inactive records remain available for provenance/reconstruction and can never be reused as new IDs.

`Provenance` preserves record-support provenance. Patch 0012 does not silently reinterpret a Supersede/Deactivate `ExistingRecordId` as a `SupportingRecordId`; transition target and supporting evidence remain distinct Patch 0009 concepts.

The causal link from an old record to its Supersede/Deactivate transition is represented explicitly by the causal event's applied effect.

## 13. ProductionState surface

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

Derived effective CommitId/TakeId indexes are internal projection state used only for deterministic duplicate rejection.

ProductionState exposes no mutation method, provider/model/diagnostic data, prompt text, credential, rationale, or hidden reasoning.

## 14. Canonical genesis mapping

Public genesis boundary:

```text
ProductionState.Initialize(
    ValidatedFixture fixture,
    ImmutableArray<RecordId> creatorLockedRecordIds)
```

Exact genesis semantics:

- origin FixtureId/family/version and `FixtureHash.Compute(fixture)` retained;
- SceneId exact;
- Character ID/display-name set exact and canonical;
- roster exact/canonical;
- Current Opportunity = fixture.InitialOpportunity;
- all fixture records Active;
- HistoricalTruth, CharacterConstitution, CharacterObservation are SystemImmutable;
- supplied valid creator locks become CreatorLocked unless already SystemImmutable;
- other records None-protected;
- exact text and existing fixture provenance retained.

Patch 0012 must create one canonical genesis-to-neutral-state mapping, not two independent domain/protection implementations.

A small internal neutral genesis-projection helper is justified because both `ProductionState.Initialize` and the existing fixture-based `StateAuthoritySnapshot.Bind` need the exact same mapping while preserving their own public exception domains.

No causal CommitId/TakeId is effective at genesis.

## 15. ProductionStateException

Production genesis/state validation uses:

```text
public sealed class ProductionStateException : Exception
```

No public constructor/factory. Canonical Production boundaries emit sanitized structural failures.

The existing `StateAuthoritySnapshot.Bind(ValidatedFixture, locks)` public failure contract remains `StateAuthorityException`; any internal sharing of neutral genesis mapping must preserve that compatibility rather than leaking ProductionStateException through the Patch 0010 public boundary.

## 16. ProductionStateCheckpoint

Immutable conceptual public surface:

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

Internally it retains one exact immutable source-state reference; no deep copy.

A null Current Opportunity cannot begin a Performer opportunity pipeline.

Checkpoint is source orchestration provenance, not a causal history event.

## 17. Evolved State Authority projection

Patch 0012 extends State Authority with a canonical Production overload:

```text
StateAuthoritySnapshot.Bind(ProductionState state)
```

Existing fixture Bind remains public and behavior-compatible.

The new overload maps exact Scene/roster and every Production RecordId/domain/lifecycle/protection/scope.

It carries no record text into StateAuthority descriptors and performs no mutation/policy decision.

There remains one DeterministicStateAuthority evaluator; CausalCommit must not duplicate its decision logic.

## 18. E0TakeStateBinding

Patch 0011 E0Take remains byte/public-surface compatible; no StateHash is retrofitted into Take.

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

It validates against the checkpoint's exact retained source state:

- TakeId initialized;
- Take Performance ContextPacketId == source ContextPacketId;
- Take Performance SubjectCharacterId == source Context subject;
- source Context subject == source Context opportunity;
- source Character == checkpoint/source state Current Opportunity;
- source Context Scene == ProductionState Scene == Take proposal SourceSceneId;
- source Context roster == ProductionState roster canonically;
- fresh StateAuthoritySnapshot from checkpoint state is semantically identical to Take authority snapshot;
- Take Authority remains Complete, decision order/count exact, no RequiresReview.

The binding stores the checkpoint StateHash and cannot accept a replacement later ProductionState.

## 19. Context source-state proof limit

Patch 0004/0005 Context contracts do not carry StateHash.

Patch 0012 cannot retroactively prove every existing ContextPacket field was derived from the checkpoint state.

What is proven:

- source state captured before pipeline;
- Context Scene/roster/opportunity/subject structurally match source state;
- Performance binds exact ContextPacketId;
- Take StateAuthority snapshot matches source state;
- commit requires source StateHash unchanged.

Effective E0 orchestration must actually feed the checkpoint state into Access/Context and retain that association in run provenance.

A future Access/Context migration may carry StateHash directly. Patch 0012 does not rewrite existing Context hashes or create a fake alias.

## 20. Strict freshness

Successful commit requires:

```text
currentState.StateHash == binding.SourceStateHash
```

Any causal-head difference fails closed even if durable record values are otherwise equal.

No "close enough" comparison and no silent re-evaluation/rewrite of Take decisions.

A different current state requires a later new evaluation/new Take path.

## 21. Record materialization types

No Core RecordId allocator.

Caller supplies:

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

Rules:

- exactly one materialization for each Approved Add;
- exactly one for each Approved Supersede;
- none for Approved Deactivate;
- none for Rejected decisions;
- mutation indexes canonical ascending/unique;
- RecordIds initialized/unique;
- no collision with any active/inactive Production RecordId;
- no missing/extra entry;
- RecordIds never reused.

## 22. Deterministic application

For exact proposal/decision order:

```text
Rejected -> no applied effect
Approved -> exact typed effect
RequiresReview -> fail closed
```

No semantic re-approval occurs.

### Add

Create one Active record with exact supplied new RecordId, proposal domain/text/subject/target, None protection, and **exact proposal SupportingRecordIds as provenance**.

### Supersede

Require exact referenced record Active and domain/subject/target compatible; mark it Inactive; create one Active replacement with supplied new RecordId, exact replacement semantics, None protection, and **exact proposal SupportingRecordIds as provenance**.

ExistingRecordId remains explicit transition lineage in the applied Supersede effect and is not silently inserted into SupportingRecordIds/provenance.

### Deactivate

Require exact referenced record Active/domain/subject/target compatible; mark it Inactive; create no new record.

Structural checks are defense in depth against malformed internal objects, not a second StateAuthority evaluator.

## 23. Applied effects

Event retains index-addressed effects rather than duplicating proposal prose:

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

Exactly one effect for every retained Approved decision and none for retained Rejected decisions.

Exact E0Take + effects are sufficient for deterministic state reconstruction.

## 24. Historical texture

Successful event retains exact Accepted E0Take.

Accepted zero-mutation and all-Rejected-consequence Takes may validly commit historical Performance with zero applied effects.

StateHash still changes because causal head changed.

No durable record is invented merely to make transcript history exist.

## 25. Current Opportunity consumption

On success:

```text
ResultState.CurrentOpportunityCharacterId = null
```

This means source opportunity is consumed and next opportunity is not yet established.

Patch 0012 does not call Director or infer next opportunity from Candidate control.

Failed/stale/non-Accepted attempts return no result state; caller's immutable source/current state remains unchanged.

## 26. E0CausalCommit event

Immutable exact event surface:

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

No public constructor/factory except through deterministic commit authority.

No provider response, prompt, credentials, diagnostics, token data, confidence/rationale, or chain-of-thought.

Rejected decisions/reasons remain in exact Take and never appear as applied effects.

## 27. E0CausalCommitResult

Immutable:

```text
E0CausalCommitResult
- Commit
- ResultState
```

Construction internal to CausalCommit.

A single result object makes the semantic all-or-nothing outcome explicit.

## 28. Sole new-commit authority

```text
DeterministicCausalCommit.Commit(
    CommitId commitId,
    ProductionState currentState,
    E0TakeStateBinding binding,
    E0Take take,
    E0RecordMaterializationSet materializations)
    -> E0CausalCommitResult
```

`DeterministicCausalCommit` is the only public path that creates a new effective E0CausalCommit.

It validates all inputs, builds the complete new ledger/effective indexes/effects/projection hash/result hash/event/state, and only then returns.

No input object mutates.

## 29. Commit eligibility

Require:

- current ProductionState valid;
- CommitId initialized;
- binding matches TakeId/Context/source Character;
- Take disposition Accepted;
- Take Authority Complete with exact decision order/count and no RequiresReview;
- current StateHash == binding SourceStateHash;
- current opportunity == Take Performance subject;
- fresh current Production StateAuthority snapshot == Take snapshot;
- exact materialization set;
- CommitId not already effective;
- TakeId not already committed.

Rejected/Alternate fail before result construction.

## 30. Canonical replay authority

Reconstructibility requires an explicit deterministic replay path; it cannot remain an assertion implemented only in tests.

Patch 0012 therefore adds a distinct non-authorizing operation:

```text
DeterministicCausalCommit.Replay(
    ProductionState parentState,
    E0CausalCommit committedEvent)
    -> ProductionState
```

Replay does **not** create a new commit or grant acceptance authority. It verifies/reconstructs an already-effective immutable causal event.

Replay requires:

- event ParentStateHash == parent StateHash;
- event Take Accepted and structurally valid;
- CommitId/TakeId not already effective in parent;
- event applied-effect set exactly equals Approved decisions and contains no Rejected effect;
- effect RecordIds/domain/transitions valid against parent;
- deterministic result projection reproduces event ResultStateHash.

Commit and Replay must share one internal deterministic application/canonicalization engine rather than duplicate transition semantics.

## 31. In-memory atomicity and event-retention gate

Patch 0012 performs no durable writes. Core objects are immutable.

Failure before successful Commit result exposes no partial state or effective event.

This is in-memory semantic atomicity only.

For E0 Harness validity, an effective result state may not be retained/continued while discarding its corresponding E0CausalCommit event. That would destroy reconstructibility and violate the frozen source-of-truth law.

The Harness/orchestration causal-history gate must append/retain the successful event whenever it adopts ResultState.

Durable crash-safe transaction/recovery remains later persistence authority.

## 32. Duplicate effective identities

ProductionState carries internal derived indexes sufficient to reject repeated effective:

- CommitId;
- committed TakeId.

Indexes are replay projections, not history substitutes.

They do not claim global uniqueness for Rejected/Alternate/failed diagnostic IDs.

## 33. Canonical hash payload

Semantic fields required in canonical encoding:

### Genesis projection

- State + StateHash contract versions;
- origin FixtureId/family/version/canonical FixtureHash;
- SceneId;
- canonical Character ID/display-name set;
- canonical roster;
- Current Opportunity explicit null/non-null;
- every Production record ordered by RecordId with domain/lifecycle/protection/scope/text/provenance;
- empty effective CommitId/TakeId indexes.

### Commit payload

- Commit contract version;
- CommitId;
- ParentStateHash;
- TakeId + Accepted disposition;
- Candidate content identity/hash from Take proposal association;
- Proposal content identity/hash from Take StateAuthority input;
- exact ordered StateAuthority decisions and numeric reason codes;
- committed source Character;
- exact ordered AppliedEffects and materialized IDs.

### Result projection

Same canonical Production fields plus updated effective CommitId/TakeId indexes.

Result StateHash = hash contract + parent hash + commit payload + result projection.

Exact byte framing remains to be frozen before approval.

## 34. Replay law

Given identical genesis fixture, creator-lock set, and ordered successful events, Replay must reconstruct exact final Production semantics and StateHash.

Wrong parent, tampered effect, altered Take semantics, or changed ResultStateHash fails closed.

Patch 0012 freezes a linear E0 causal sequence only. ParentStateHash preserves future branchability without defining branch/canon UX.

Prior events are immutable; no rewrite/delete API.

## 35. Protection preservation

Source checkpoint + binding + strict StateHash + snapshot equivalence ensure application uses the same authority projection as the retained Take.

CausalCommit never overrides SystemImmutable/CreatorLocked protection or invents a new approval.

Structural application validation only detects impossible/malformed input.

## 36. Structural validity versus authentication

Core remains synthetic-capable. Hash/type association is not authentication.

Core does not authenticate RunId, provider/attempt, context-disclosure path, Integrity assessor, policy/reviewer identity, disposition intervention, CommitId allocator, or RecordId allocator.

Effective E0 experimental provenance must associate at minimum:

- RunId;
- CommitId;
- TakeId;
- disposition source/intervention label;
- SourceStateHash;
- SourceContextPacketId;
- Candidate identity/hash;
- proposal identity/hash;
- authority decisions/reasons/policy/review provenance;
- provider/attempt/context provenance required by configured run;
- supplied RecordId materializations.

A synthetic Core commit can exercise deterministic tests but does not authenticate an experimental Production run by itself.

Production creative history remains independent from deletable provider diagnostics; E0 provenance may be separate but must associate through CommitId/TakeId.

## 37. Exception domains

Production initialization/state validation:

```text
ProductionStateException
```

Causal checkpoint/binding/materialization/commit/replay expected failures:

```text
E0CausalCommitException
```

Both are public sealed catchable exceptions with no public constructor/factory.

Existing upstream public exception domains remain compatible.

Messages/inner/data/ToString remain structural/sanitized and must not leak Candidate text, Context prose, mutation text, provider payloads, credentials, arbitrary user content, or diagnostic bodies.

Expected upstream structural exceptions may be normalized with exact sanitized domain inner exceptions where approved; arbitrary unexpected runtime/programming failures must not be relabeled as ordinary commit failure.

Failure returns no fallback state/event.

## 38. Determinism, memory, ARM64 suitability

No network, filesystem, clock, randomness, provider API, GPU, NPU, polling, background thread, or global mutable state.

Checkpoint retains one immutable state reference rather than cloning.

Commit/Replay structurally share unchanged immutable records where safe; only changed/new records and bounded event/result metadata allocate.

Hashing/application are linear in bounded E0 state/mutation size and run only at explicit initialization/commit/replay boundaries, not idle loops. This is architectural suitability, not device-power evidence.

## 39. Required implementation test families

Implementation must prove at minimum:

1. exact namespaces/types/version strings;
2. existing CommitId only, no allocator/new type;
3. StateHash default invalid, canonical creation internal/deterministic;
4. exact Production enum values/default/undefined invalid;
5. ProductionState/records/checkpoint/events/results immutable with closed constructors;
6. one canonical genesis domain/protection mapping;
7. genesis exact origin hash/Scene/Characters/roster/opportunity/records/provenance/protection;
8. global RecordId uniqueness active+inactive;
9. deterministic genesis StateHash; meaningful fixture/lock changes alter it;
10. checkpoint captured before pipeline retains exact state reference/no deep clone;
11. fixture-derived and Production-derived StateAuthority snapshots semantically equal at genesis;
12. evolved StateAuthority snapshot maps lifecycle/protection/domain/scope exactly;
13. binding validates checkpoint Context/Take/snapshot relations;
14. binding cannot rebase Take to later ProductionState;
15. existing full Context derivation from StateHash is not falsely claimed;
16. Rejected/Alternate cannot commit;
17. zero-mutation Accepted commits Performance history, zero effects, changed StateHash;
18. all-Rejected Accepted commits Performance history, zero effects, changed StateHash;
19. Approved Add exact record/provenance;
20. Approved Supersede inactivates target, creates replacement, preserves exact SupportingRecordIds separately from ExistingRecordId transition lineage;
21. Approved Deactivate inactivates target/no new record;
22. mixed decisions apply all/only Approved;
23. one AppliedEffect per Approved and none per Rejected;
24. exact materialization only for Approved Add/Supersede;
25. missing/extra/duplicate/colliding IDs fail;
26. inactive ID reuse fails;
27. stale StateHash fails/no result;
28. later history with equivalent durable projection does not resurrect old StateHash;
29. duplicate effective CommitId fails;
30. duplicate committed TakeId fails;
31. successful commit consumes opportunity/no next opportunity;
32. failed commit leaves caller state/opportunity unchanged;
33. event retains exact Take and associates CommitId/TakeId;
34. exact Parent/Result hash chain;
35. deterministic repeat identical inputs/IDs equivalent;
36. changed valid CommitId/materialized ID changes identity as specified;
37. Replay reproduces Commit result state/hash exactly;
38. Replay rejects wrong parent/tampered Take/effects/hash;
39. Commit and Replay share one internal transition implementation;
40. Harness cannot adopt ResultState without retaining successful causal event in its E0 history package;
41. no rewrite/delete API for prior events;
42. no persistence/provider/next-Director/Scene-loop/UI/AI/NPU authority leaks;
43. sanitized exceptions/no arbitrary catch-all;
44. frozen Patch 0003–0011 regressions remain green;
45. full Core regression green;
46. Missing Raft Harness green;
47. generic smoke Harness green.

## 40. Explicit non-goals

Patch 0012 does not implement/freeze:

- CommitId/RecordId global allocators or formats;
- durable database/event-store technology;
- crash-safe persistence/recovery transaction;
- branch DAG/canon lineage identity;
- Alternate promotion/retcon/rehearsal/canon UX;
- final Archive query/search;
- full Access Control migration to evolved ProductionState;
- full Context Composer migration to evolved ProductionState;
- cryptographic proof of current Context full-content derivation from StateHash;
- provider/prompt/assessor authentication;
- next Director opportunity selection/application;
- Scene-loop orchestration;
- Observation;
- World Resolver;
- final consequence-review/Another Take/Take a Seat UX;
- Windows AI Foundry;
- NPU execution;
- WinUI;
- MSIX;
- WACK;
- Store certification.

## 41. Patch boundary summary

```text
ProductionState checkpoint
    -> Access Control
        -> Context Composer
            -> Performer Candidate
                -> Integrity
                    -> State Interpretation
                        -> deterministic State Authority
                            -> immutable E0 Take
                                -> Take/source-state binding
                                    -> strict StateHash freshness
                                        -> DETERMINISTIC ATOMIC CAUSAL COMMIT
                                            -> append-only event
                                            -> reconstructible ProductionState projection
                                                -> later Director continuation
```

Principal law:

> An E0 causal commit can make history effective only from the exact immutable source Production state checkpoint captured before the Take pipeline, only for an immutable Accepted Take, and only while that history-sensitive StateHash remains current. It makes the exact Performance plus every retained Approved consequence effective together, creates no effect for retained Rejected consequences, consumes the source opportunity, returns one immutable causal event plus one reconstructible current projection, and returns neither on failure. Commit and Replay share one deterministic transition engine; causal events remain the conceptual source of truth; the Take is never rewritten; next-opportunity and durable persistence remain later authority.

## 42. Approval / implementation gate

This blueprint is architecture only.

Before implementation:

1. recursively adversarial-audit Proposal 0.3 against Blueprint 0.1, approved Patches 0003–0011, source/tests, hygiene law, dependency direction, genesis mapping, StateAuthority ownership, Take immutability, pre-generation checkpoint/freshness, Context identity limit, materialization semantics, supporting provenance versus transition lineage, replay, hash canonicalization, duplicate IDs, opportunity consumption, structural/authenticated provenance separation, invalid-state construction, memory/ARM64 suitability, experiment isolation, future persistence/branch separation;
2. restart audit after every material correction;
3. require a full pass with zero material corrections and zero worthwhile architectural improvements;
4. obtain explicit user approval;
5. create fresh-chat implementation handoff;
6. write no Patch 0012 executable code during architecture phase.
