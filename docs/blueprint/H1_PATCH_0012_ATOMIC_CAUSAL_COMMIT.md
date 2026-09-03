# H1 Patch 0012 — E0 Atomic Causal Commit Contract

Status: blueprint proposal 0.2 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
Parent repository checkpoint: `main` at `20b26711ceffa17e0543ca3fc180c9acf69f5e45`
Parent machine-tested executable/test authority: H1 Patch 0011 at `4250011c167cd9850ad891aaea4ee053216cf135`
Branch: `h1-patch-0012-atomic-causal-commit-blueprint`

## 1. Purpose

Define the next E0 deterministic-spine boundary after machine-validated Patch 0011 Take Semantics:

```text
immutable ProductionState source
    -> source-state checkpoint captured before Access / Context / Performance
        -> existing deterministic E0 pipeline
            -> immutable Accepted E0Take
                -> exact Take/source-state binding
                    -> strict unchanged-state freshness
                        -> deterministic all-or-nothing causal commit
                            -> E0CausalCommit event
                            -> new immutable ProductionState projection
```

Patch 0012 establishes only the minimum E0 in-memory state/event semantics needed to prove:

- the exact Accepted Performance becomes history only with every retained Approved consequence;
- no retained Rejected consequence becomes effective;
- stale/failed/invalid commit attempts alter nothing;
- causal event history remains the conceptual source of truth;
- current state is a deterministic reconstructible projection, not canonical mutable history;
- successful source commit consumes the current opportunity but does not choose the next opportunity.

Patch 0012 does not implement durable storage/recovery, branch DAGs, retcon/rehearsal/canon-promotion UX, provider execution, Scene-loop orchestration, next-Director selection, Observation, World Resolver, WinUI, Windows AI/NPU, packaging, WACK, or Store behavior.

## 2. Recovered frozen authority

Blueprint 0.1 freezes:

- `Production State -> deterministic Access Control -> Context Composer -> Performer`;
- Performer, Director, State Interpreter, and models do not own persistence authority;
- append-only causal event history is the conceptual source of truth, not mutable snapshots;
- Accepted Take plus committed consequences enters causal event history and projects current World/Character/Knowledge/Relationship/Pressure state;
- Accepted Performance and all authoritative Approved consequences form one atomic causal commit;
- either both the Accepted Take and all Approved mutations commit coherently, or neither does;
- historical texture remains recoverable even when no durable projected-state mutation is created;
- Accepted Takes are immutable;
- corrections/retcons/branches/alternate takes are later explicit causal authority, never silent rewrites;
- Production history and diagnostics are separate;
- a committed consequence must remain traceable to accepted Performance or another authorized cause;
- Performance may not commit without Approved consequences and consequences may not commit without Accepted Performance.

Patch 0011 additionally freezes:

- only `E0TakeDisposition.Accepted` is commit-eligible;
- Rejected/Alternate Takes remain non-effective;
- Accepted means commit-eligible, not historical/effective;
- successful commit applies every retained Approved consequence and no retained Rejected consequence;
- freshness handling may reject an immutable Take but may not rewrite its retained consequence package;
- failed commit does not advance Current Opportunity;
- after successful source commit, later orchestration may separately recompute Director and establish the next opportunity;
- successful causal commit associates CommitId with TakeId;
- Rejected/Alternate/failed-before-Take/Accepted-but-failed-commit material has no effective causal CommitId.

## 3. Ordering decision — source checkpoint precedes the generative pipeline

Patch 0012 introduces no new authority inside Access/Context/Performer/Integrity/Interpreter/State Authority/Take.

It introduces one deterministic orchestration precondition before those boundaries:

```text
current immutable ProductionState
    -> ProductionStateCheckpoint.Capture
        -> Access / Context / Performer / Integrity / Interpretation / State Authority / Take
```

The checkpoint captures the exact immutable source-state reference and its StateHash **before** the Character context/performance pipeline begins.

Reason:

- creating a state binding only after a Take exists would permit a stale Take to be rebound to a later structurally similar state;
- an immutable pre-generation checkpoint prevents silent rebasing;
- retaining one state reference is cheaper and clearer than copying state into Context/Take objects;
- commit can later compare the current StateHash against the original checkpoint-derived binding and fail closed if anything changed.

No clock, lock-free global generation counter, random token, or mutable singleton is introduced.

## 4. Causal events are source; ProductionState is projection

The constitutional distinction is:

```text
ValidatedFixture
    -> immutable genesis input

ordered E0CausalCommit events
    -> append-only causal Production history / conceptual source of truth

ProductionState
    -> immutable current projection reconstructed from genesis + causal events
```

Patch 0012 may return both a new causal event and its new projection, but the projection is not a replacement for history.

Durable persistence may later store causal events and cache projections. Patch 0012 does not choose a persistence product or transaction mechanism.

The original `ValidatedFixture` remains immutable genesis material and is never mutated into an evolved Production.

## 5. Dependency direction

Proposed neutral state namespace:

```text
Ensemble.E0.Core.Production
```

Proposed causal commit namespace:

```text
Ensemble.E0.Core.CausalCommit
```

Dependency direction is:

```text
Domain / Fixture
    -> Production current-state model
        -> Access/Context migration later
        -> State Authority projection
            -> Take
                -> CausalCommit
```

`Production` must not depend on `StateAuthority`, `Take`, or `CausalCommit`.

`StateAuthority` may project from the neutral Production model.

`CausalCommit` may depend on Production, Context, State Interpreter, State Authority, and Take.

This prevents a circular authority graph in which Production state depends on the decision subsystem that is supposed to evaluate it.

## 6. Contract versions

Proposed exact contract holders:

```text
ProductionStateContracts.StateContractVersion
    = "ensemble.e0.production-state.v1"

ProductionStateContracts.StateHashContractVersion
    = "ensemble.e0.production-state-hash.sha256.v1"

E0CausalCommitContracts.ContractVersion
    = "ensemble.e0.causal-commit.v1"
```

Canonical hashing uses SHA-256 only because Patch 0012 needs deterministic content identity, not because SHA-256 authenticates the source or caller.

Physical `.cs` grouping remains implementation hygiene.

## 7. Existing CommitId is canonical

`CommitId` already exists in `Ensemble.E0.Core.Domain.StrongIds` and remains the only Commit identity type.

Patch 0012 introduces no CommitId allocator and freezes no new external format.

Core receives an initialized supplied CommitId. It never derives CommitId from clock, random/GUID generation, TakeId, StateHash, content hashes, provider/model data, or process state.

A supplied CommitId becomes **effective Production history only if `DeterministicCausalCommit.Commit` succeeds and returns an `E0CausalCommit`**.

Failure returns no effective commit object and does not insert the supplied ID into Production causal indexes.

## 8. StateHash exact semantic type

Patch 0012 introduces:

```text
Ensemble.E0.Core.Production.StateHash
```

Conceptual shape:

```text
public readonly record struct StateHash
- Value : lowercase 64-character SHA-256 hex
```

Rules:

- `default(StateHash)` is uninitialized and fails when read;
- normal external callers cannot create arbitrary StateHash values through a public `From(string)` factory in Patch 0012;
- canonical genesis/commit hashing inside the Production/CausalCommit boundary creates valid values;
- equality is exact ordinal value equality;
- `ToString()` returns `Value`;
- future persistence parsing is not frozen by Patch 0012.

StateHash remains distinct from Fixture hash, ContextPacketId, CandidateContentHash, ProposalContentHash, TakeId, CommitId, and RecordId.

## 9. StateHash is history-sensitive current-state identity

A StateHash must bind both current projection and causal head.

A later causal history that happens to return to the same projected values must not resurrect an older StateHash and thereby make an old Take fresh again.

Conceptual computation:

```text
genesis StateHash
    = SHA256(
        StateHashContractVersion
        + canonical genesis projection)

result StateHash
    = SHA256(
        StateHashContractVersion
        + ParentStateHash
        + canonical commit payload identity
        + canonical result projection identity)
```

The canonical commit-payload identity excludes `ResultStateHash` to avoid circular hashing.

Exact byte encoding must be frozen before implementation using invariant UTF-8, unambiguous length framing (or an equally explicit canonical encoding), ordinal ordering, explicit null markers, stable numeric enum values, and stable array ordering.

No reflection order, runtime object identity, culture-sensitive formatting, unspecified serializer defaults, clock, or platform-specific newline behavior may enter the hash.

## 10. Production enum contracts

Production State uses neutral domain enums rather than depending on State Authority enums.

Proposed exact values:

```text
ProductionRecordDomain
- Unspecified = 0
- HistoricalTruth = 1
- UnresolvedProposition = 2
- WorldState = 3
- SceneState = 4
- CharacterConstitution = 5
- CharacterDisposition = 6
- CharacterCircumstance = 7
- CharacterObservation = 8
- CharacterKnowledge = 9
- CharacterBelief = 10
- CharacterSuspicion = 11
- CharacterMemory = 12
- CharacterGoal = 13
- CharacterClaim = 14
- Relationship = 15
- Pressure = 16

ProductionRecordLifecycle
- Unspecified = 0
- Active = 1
- Inactive = 2

ProductionRecordProtection
- Unspecified = 0
- None = 1
- SystemImmutable = 2
- CreatorLocked = 3
```

`Unspecified`/undefined values never represent valid initialized Production records.

The State Authority projection must have an exact one-to-one tested mapping from these Production semantics to existing Patch 0010 record-domain/lifecycle/protection semantics. Patch 0012 does not change Patch 0010 enum values or public meaning.

## 11. Production character identity

Current state must retain stable Character identity/display metadata separately from mutable records so evolved state does not depend on stale `ValidatedFixture.Characters` objects.

Conceptual immutable type:

```text
ProductionCharacter
- CharacterId
- DisplayName
```

Characters are unique and canonical by CharacterId.

Patch 0012 does not mutate Character identity/display name.

## 12. Production record surface

Conceptual abstract immutable base:

```text
ProductionRecord
- RecordId
- Domain
- Lifecycle
- Protection
- Text
- Provenance : canonical ImmutableArray<RecordId>
```

Sealed scoped forms:

```text
GlobalProductionRecord

CharacterProductionRecord
- SubjectCharacterId

RelationshipProductionRecord
- SubjectCharacterId
- TargetCharacterId
```

Construction is not publicly open. Genesis and deterministic causal application own record creation.

Every RecordId is globally unique across the Production record ledger, including inactive records.

The record ledger preserves inactive superseded/deactivated records for provenance and ID non-reuse. Access/Context projections may later select Active material only under their own contracts.

## 13. ProductionState exact semantic surface

Conceptual public immutable surface:

```text
public sealed class ProductionState
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

Derived duplicate-detection indexes for committed CommitIds and committed TakeIds may exist internally but are not creative-history substitutes and need not be public.

ProductionState exposes no mutation methods.

Genesis has a public canonical construction boundary; evolved states are created only through deterministic causal commit/replay internals.

ProductionState retains no provider/model/prompt/credential/token/diagnostic/chain-of-thought fields.

## 14. ProductionState genesis

Conceptual public genesis boundary:

```text
ProductionState.Initialize(
    ValidatedFixture fixture,
    ImmutableArray<RecordId> creatorLockedRecordIds)
    -> ProductionState
```

Genesis must validate the fixture/creator-lock input and project exact initial authority semantics:

- exact fixture origin identity and `FixtureHash.Compute(fixture)`;
- exact SceneId;
- exact stable Character IDs/display names;
- exact canonical Scene roster;
- Current Opportunity = `fixture.InitialOpportunity`;
- all genesis records Active;
- HistoricalTruth, CharacterConstitution, and CharacterObservation are SystemImmutable;
- supplied creator locks become CreatorLocked only when protection is not already stronger SystemImmutable;
- all other unlocked records use None protection;
- exact text and existing provenance are preserved.

Implementation must not create two independently maintained protection/domain mappings. The existing fixture-derived State Authority path and the new Production genesis/state path must share one canonical mapping implementation where practical, or be exhaustively cross-audited by tests if a small refactor is required to achieve one source of truth.

No CommitId or committed TakeId exists at genesis.

## 15. ProductionStateCheckpoint

Patch 0012 introduces a source-state checkpoint captured before Access/Context/Performance:

```text
public sealed class ProductionStateCheckpoint
- StateHash
- SceneId
- CurrentOpportunityCharacterId
```

Conceptual construction:

```text
ProductionStateCheckpoint.Capture(ProductionState sourceState)
```

Internally the checkpoint retains the exact immutable `ProductionState` reference needed for later canonical snapshot comparison; it does not deep-copy the state graph.

The public surface does not expose the retained state reference merely to bypass ordinary state flow.

A checkpoint with null Current Opportunity cannot begin a Performer opportunity pipeline.

This object is orchestration provenance, not a new creative history event.

## 16. Evolved State Authority projection

Patch 0010 currently binds StateAuthoritySnapshot from `ValidatedFixture`.

Patch 0012 requires a canonical evolved-state projection owned by State Authority:

```text
StateAuthoritySnapshot.Bind(ProductionState state)
```

The existing fixture-based Bind remains valid for frozen regression tests.

The ProductionState overload must map exact Scene, roster, every record ID/domain/lifecycle/protection, and character/relationship scope without copying text into the authority descriptor surface.

It performs no mutation and no policy decision.

State Authority decision logic remains single and canonical; CausalCommit must not implement a second authority evaluator.

## 17. E0TakeStateBinding

Patch 0011 intentionally did not add StateHash to the exact Take surface. Patch 0012 preserves that contract.

Proposed immutable binding:

```text
public sealed class E0TakeStateBinding
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
    -> E0TakeStateBinding
```

Binding validates against the checkpoint's retained exact source state:

- initialized TakeId;
- Take Performance ContextPacketId == source ContextPacketId;
- Take Performance SubjectCharacterId == source Context subject;
- source Context subject == source Context opportunity;
- source Character == checkpoint/source-state Current Opportunity;
- source Context Scene == ProductionState Scene == Take proposal SourceSceneId;
- source Context roster equals ProductionState roster canonically;
- a fresh `StateAuthoritySnapshot.Bind(checkpoint sourceState)` is semantically identical to the snapshot retained in the Take authority trace;
- Take Authority evaluation remains structurally terminal Complete and contains no RequiresReview.

The resulting binding stores the original checkpoint StateHash. It cannot be rebound to a later ProductionState.

## 18. Explicit Context derivation limit

Current Patch 0004/0005 Access/Context contracts do not carry StateHash.

Patch 0012 therefore cannot cryptographically prove from an existing ContextPacket alone that every bounded field was derived from the checkpoint's ProductionState.

Patch 0012 proves only what current contracts support:

- checkpoint captures exact immutable source state before the pipeline;
- Context structural Scene/roster/opportunity/subject association matches that source state;
- Performance is bound to exact ContextPacketId;
- Take State Authority snapshot matches the checkpoint state;
- commit requires unchanged checkpoint StateHash.

Effective E0 orchestration must use the checkpoint state as the source for Access/Context and retain that provenance. A later Access/Context migration may propagate StateHash directly.

Patch 0012 must not alter frozen ContextPacket hashes or invent a fake cross-object hash to claim stronger proof.

## 19. Strict freshness

Successful commit requires:

```text
currentState.StateHash == takeStateBinding.SourceStateHash
```

No current-state change, including a causal-history-only change that happens to restore equal durable record values, may be treated as fresh.

If hashes differ, commit fails closed.

Patch 0012 does not re-evaluate against a new state and replace the Take's consequence set. A different current authority outcome requires a later explicit new evaluated Take/causal path.

## 20. RecordId materialization types

Patch 0012 does not introduce a RecordId allocator.

Proposed typed caller-supplied materialization:

```text
public sealed class E0RecordMaterialization
- MutationIndex
- RecordId
- private constructor
- Create(int mutationIndex, RecordId recordId)

public sealed class E0RecordMaterializationSet
- Items : canonical ImmutableArray<E0RecordMaterialization>
- private constructor
- Bind(ImmutableArray<E0RecordMaterialization>)
```

Materialization set rules:

- exactly one entry for each Approved Add mutation;
- exactly one entry for each Approved Supersede mutation;
- no entry for Approved Deactivate;
- no entry for Rejected mutations;
- mutation indexes unique and canonical ascending;
- RecordIds initialized and unique within the set;
- no supplied RecordId collides with any current active or inactive Production RecordId;
- no extra/missing entry is accepted.

Record IDs are never reused after supersession/deactivation.

## 21. Deterministic application semantics

Patch 0012 consumes the exact mutation/decision pairs retained by the Take.

For every canonical mutation index:

```text
Rejected
    -> no effect

Approved
    -> exact typed transition below

RequiresReview
    -> invalid Take/commit input; fail closed
```

No prose reinterpretation or new approval occurs.

### Add

- create one Active Production record;
- use exactly the supplied materialized RecordId for that mutation index;
- preserve exact proposal domain/text/subject/target semantics;
- provenance = exact canonical proposal SupportingRecordIds;
- protection = None for Patch 0012-created mutable records.

### Supersede

- require referenced existing record Active and exact domain/subject/target match;
- mark existing record Inactive without rewriting it;
- create one Active replacement record with supplied materialized RecordId;
- preserve exact replacement text/domain/subject/target;
- replacement provenance = canonical unique union of superseded RecordId plus proposal SupportingRecordIds;
- replacement protection = None unless a stronger future contract explicitly authorizes inherited protection; Patch 0012 may not silently supersede protected records because State Authority would not have Approved that transition.

### Deactivate

- require referenced existing record Active and exact domain/subject/target match;
- mark existing record Inactive without rewriting it;
- create no replacement RecordId.

Application performs structural invariant checking but does not re-decide State Authority.

## 22. Applied effect types

The causal event should not duplicate exact mutation prose already retained by the exact E0Take.

Instead expose index-addressed materialization/effect identity:

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

Effects are immutable and constructed only by the deterministic commit operation.

There is exactly one effect for every retained Approved decision and no effect for any retained Rejected decision.

Together with the exact retained E0Take, these effects are sufficient to reconstruct the applied result without copying Candidate/proposal text.

## 23. Historical texture

Successful commit always retains the exact Accepted E0Take in causal history.

Therefore:

```text
Accepted Take + zero proposals
Accepted Take + all Rejected proposals
```

are both valid commits when every other precondition passes.

They make the exact Performance historical even when no durable record changes.

Result StateHash still changes because causal history changed.

## 24. Current Opportunity consumption

Successful commit consumes the source opportunity:

```text
ResultState.CurrentOpportunityCharacterId = null
```

This is a transitional authoritative state meaning the committed source opportunity has ended and no next opportunity has yet been established.

Patch 0012 does not use Candidate control nomination to select the next Character, does not call Director, and does not create a hidden next-opportunity policy.

Failed/stale/non-Accepted attempts return no state and leave the caller's immutable current state unchanged.

Later deterministic Director/orchestration authority must separately establish the next Current Opportunity before another Performer context pipeline begins.

## 25. E0CausalCommit exact event surface

Conceptual immutable event:

```text
public sealed class E0CausalCommit
- ContractVersion
- CommitId
- ParentStateHash
- ResultStateHash
- Take : exact Accepted E0Take
- CommittedOpportunityCharacterId
- AppliedEffects : canonical ImmutableArray<E0AppliedMutationEffect>
```

Constructor is not public. Only `DeterministicCausalCommit` may produce a valid event under the approved production path.

The event retains no provider response, prompt, credential, token accounting, diagnostics, confidence/rationale, or chain-of-thought.

Rejected proposal decisions/reasons remain recoverable from the exact retained Take but never appear as applied effects.

## 26. E0CausalCommitResult

Conceptual immutable result:

```text
public sealed class E0CausalCommitResult
- Commit
- ResultState
```

Construction is internal to the commit subsystem.

Returning one result object prevents ordinary callers from treating an event and its derived state as independently successful operations.

## 27. Sole commit authority

Patch 0012 owns one deterministic public commit operation:

```text
public static class DeterministicCausalCommit

Commit(
    CommitId commitId,
    ProductionState currentState,
    E0TakeStateBinding takeStateBinding,
    E0Take take,
    E0RecordMaterializationSet materializations)
    -> E0CausalCommitResult
```

No second Apply/Save/Accept/Promote/Commit factory may bypass this operation.

The method validates all preconditions and computes the complete new record ledger, causal indexes, applied effects, result projection identity, result StateHash, event, and result state before returning.

No input object is mutated.

## 28. Commit eligibility

`DeterministicCausalCommit.Commit` requires:

- current ProductionState non-null and structurally initialized;
- CommitId initialized;
- binding non-null and matching TakeId/Context/source Character;
- Take non-null and `Disposition == Accepted`;
- Take Authority terminal Complete, ordered one-to-one with proposal mutations, no RequiresReview;
- current StateHash exactly equals binding SourceStateHash;
- current opportunity is non-null and exactly the Take Performance subject;
- fresh StateAuthoritySnapshot from current state semantically equals the Take snapshot;
- exact materialization set;
- CommitId not previously effective in current state's derived causal index;
- TakeId not previously effective in current state's derived causal index.

Rejected/Alternate Takes fail before result-state construction.

## 29. In-memory semantic atomicity

Patch 0012 performs no filesystem/database/network writes.

All Core semantic objects are immutable.

Therefore failure before a successful `E0CausalCommitResult` return exposes no partially mutated ProductionState and no effective `E0CausalCommit`.

This proves E0 **in-memory semantic atomicity**.

It does not claim crash-safe durable transaction/recovery. Later persistence must treat the causal event as source of truth and must not persist transcript and projection independently in a way that can violate atomic causal history.

## 30. Canonical hash payload

Exact byte canonicalization remains to be finalized by the recursive audit before approval, but fields are frozen at the semantic level.

Genesis projection identity includes:

- State/StateHash contract versions;
- origin FixtureId, FixtureFamilyId, FixtureVersion, canonical Fixture hash;
- SceneId;
- canonical Character identities/display names;
- canonical roster;
- Current Opportunity with explicit null marker;
- every Production record in RecordId ordinal order with numeric domain/lifecycle/protection, subject/target nullable identity, exact text, canonical provenance IDs;
- empty effective CommitId/TakeId indexes.

Post-commit causal payload identity includes:

- Commit contract version;
- CommitId;
- ParentStateHash;
- TakeId and Accepted disposition;
- Candidate content identity contract/hash from the Take proposal association;
- Proposal content identity contract/hash from the retained State Authority input;
- exact ordered State Authority mutation decisions and numeric reason codes;
- committed source CharacterId;
- exact ordered AppliedEffects and materialized RecordIds.

Result projection identity includes the same canonical Production projection fields plus canonical effective CommitId/TakeId indexes.

The result StateHash is computed from parent hash + causal payload identity + result projection identity.

## 31. Replay law

Given identical:

- genesis ValidatedFixture;
- creator-lock set;
- ordered successful E0CausalCommit events;

replay must reconstruct equivalent ProductionState semantics and exact final StateHash.

Replay must require each event ParentStateHash to equal the current replay StateHash and must reproduce each event ResultStateHash.

Patch 0012 freezes only a linear E0 history. ParentStateHash naturally preserves later branchability without implementing branch DAG/canon-promotion semantics now.

No prior causal commit is rewritten or deleted.

## 32. Duplicate effective identity protection

ProductionState maintains derived, immutable duplicate-detection indexes sufficient to reject:

- an already-effective CommitId;
- an already-committed TakeId.

These indexes are projections of causal history, not replacements for event history.

They do not claim global uniqueness for Rejected/Alternate/failed diagnostic TakeIds or unsuccessful attempted CommitIds.

## 33. State Authority and protection preservation

Commit does not override State Authority.

The combination of:

- original pre-pipeline ProductionStateCheckpoint;
- immutable Take/source-state binding;
- strict current StateHash equality;
- exact current/take StateAuthoritySnapshot equivalence;

ensures application occurs only against the unchanged authority projection under which the retained Take decisions were bound.

Structural application checks remain defense in depth, not a second approval algorithm.

SystemImmutable/CreatorLocked records cannot be made mutable by CausalCommit.

## 34. Core validity versus authenticated E0 provenance

Patch 0012 Core remains structurally trustworthy but synthetic-capable, like earlier deterministic contracts.

Typed objects/hashes are not credentials.

Core does not authenticate:

- RunId provenance;
- provider/attempt identity;
- source Context disclosure provenance;
- Integrity assessor/review identity;
- State Authority policy/review human identity;
- Take-disposition intervention identity;
- CommitId allocator provenance;
- RecordId allocator provenance.

Effective E0 Harness/orchestration must retain enough configured provenance to associate at minimum:

- RunId;
- CommitId;
- TakeId;
- Take disposition source/intervention label;
- SourceStateHash;
- SourceContextPacketId;
- Candidate content identity/hash;
- proposal identity/hash;
- retained authority decisions/reasons and policy/review provenance;
- provider/attempt/context-disclosure provenance required by the configured run;
- supplied RecordId materializations.

A structurally valid synthetic Core commit does not by itself authenticate Production provenance for experimental evidence.

Production creative history must remain reconstructible without provider diagnostics or credentials; experimental provenance may be stored separately but must associate with CommitId/TakeId.

## 35. Exception boundary

Patch 0012 defines:

```text
public sealed class E0CausalCommitException : Exception
```

with no public constructor/factory; expected construction remains assembly-internal under the sole approved commit/binding/materialization boundaries.

Expected failures include:

- null/missing inputs;
- uninitialized CommitId/RecordId/StateHash on trusted semantic objects;
- non-Accepted Take;
- checkpoint/binding/Take mismatch;
- stale StateHash;
- source/current opportunity mismatch;
- State Authority snapshot mismatch;
- duplicate effective CommitId/TakeId;
- malformed decision/mutation alignment;
- missing/extra/duplicate/colliding RecordId materializations;
- invalid Add/Supersede/Deactivate target state;
- malformed Production record/index/hash structure encountered at the public boundary.

Public exception representation remains structural and sanitized. Candidate text, Context prose, mutation text, provider payloads, credentials, arbitrary user content, or diagnostic bodies may not leak through Message/Data/inner chain/ToString.

Unexpected programming/runtime failures must not be relabeled as ordinary causal-commit rejection.

Failure produces no fallback state and no effective commit.

## 36. Determinism, memory, and ARM64 suitability

Patch 0012 Core uses no network, filesystem, clock, randomness, provider API, GPU, NPU, background thread, polling, or global mutable state.

The source checkpoint retains one immutable ProductionState reference rather than cloning the state before inference.

Commit should structurally share unchanged immutable Production records where safe and allocate only changed/new record objects plus bounded event/result metadata.

Hashing/application are linear in the bounded E0 record/mutation surface and occur only at an explicit accepted-Take boundary, so Patch 0012 introduces no idle battery work by design. This is architectural suitability, not measured hardware power evidence.

## 37. Required implementation test families

Future implementation must prove at minimum:

1. exact namespace/type/contract-version surfaces;
2. existing CommitId reused and no allocator/new Commit ID type exists;
3. StateHash default fails closed and canonical creation is internal/deterministic;
4. exact Production enum numeric values and default/undefined fail closed;
5. ProductionState is immutable/sealed with no public mutation/application constructor;
6. genesis preserves fixture origin hash/Scene/Characters/roster/opportunity/records/provenance/protection exactly;
7. RecordIds globally unique across active/inactive ledger;
8. genesis StateHash deterministic and changes for meaningful fixture/lock changes;
9. source checkpoint captures exact immutable state before pipeline and does not deep-copy;
10. StateAuthoritySnapshot from genesis ProductionState equals existing fixture-derived semantics;
11. evolved StateAuthority snapshot maps exact active/inactive/protection/domain/scope semantics;
12. Take/source binding validates checkpoint state, ContextPacketId, subject, scene, roster, opportunity, and Take snapshot;
13. a Take cannot be rebound through the binding API to a later ProductionState;
14. existing Context full-content/source-state proof is explicitly not claimed;
15. Rejected/Alternate Takes cannot commit;
16. Accepted zero-mutation Take commits Performance history and changes StateHash;
17. Accepted all-Rejected consequence set commits Performance history with no applied effects and changes StateHash;
18. Approved Add creates exact active record with exact provenance;
19. Approved Supersede inactivates old record, creates exact replacement, and carries superseded ID in provenance;
20. Approved Deactivate inactivates exact record and creates no new record;
21. mixed Approved/Rejected applies every Approved and no Rejected;
22. exactly one AppliedEffect exists for every Approved decision and none for Rejected;
23. materialization required exactly for Approved Add/Supersede and forbidden otherwise;
24. missing/extra/duplicate/colliding materialization fails;
25. inactive RecordId reuse fails;
26. stale StateHash fails with no result;
27. later history returning to same durable record projection does not resurrect old StateHash;
28. duplicate effective CommitId fails;
29. duplicate committed TakeId fails;
30. successful commit consumes Current Opportunity and establishes no next opportunity;
31. failed commit leaves caller state/opportunity unchanged;
32. exact Take retained by event and CommitId associated with TakeId;
33. ParentStateHash/ResultStateHash chain exact;
34. deterministic repeat over identical inputs/IDs yields equivalent event/result/hash;
35. different valid CommitId or materialized RecordId changes causal/state identity as specified;
36. replay from genesis + ordered commits reproduces exact final StateHash/projection;
37. replay rejects wrong parent hash or tampered event/effect identity;
38. no prior commit rewrite/delete/mutation API exists;
39. no persistence/provider/Director-next-opportunity/Scene-loop/UI/AI/NPU authority leaks into public commit surface;
40. exception surface is sanitized and no catch-all converts unexpected runtime failures;
41. fixed Missing Raft fixture/context/candidate/state-authority/Take regression identities remain unchanged where their contracts are not intentionally extended;
42. full Core regression green;
43. Missing Raft Harness green;
44. generic smoke Harness green.

## 38. Explicit non-goals

Patch 0012 does not implement/freeze:

- global CommitId allocator/format;
- global RecordId allocator/format;
- durable database/event-store technology;
- crash-safe filesystem/database transaction or recovery;
- branch DAG/canon lineage identity;
- Alternate promotion, retcon, rehearsal, canon-promotion UX;
- final Archive queries/search;
- full Access Control migration to evolved ProductionState;
- full Context Composer migration to evolved ProductionState;
- cryptographic proof of existing ContextPacket full-content derivation from StateHash;
- provider/prompt/assessor authentication;
- next Director opportunity selection/application;
- Scene-loop orchestration;
- Observation engine;
- World Resolver;
- final consequence-review UX;
- final Another Take / Take a Seat UX;
- Windows AI Foundry;
- NPU execution;
- WinUI;
- MSIX;
- WACK;
- Store certification.

## 39. Patch boundary summary

Intended deterministic spine after Patch 0012:

```text
ProductionState checkpoint
    -> Access Control
        -> Context Composer
            -> Performer Candidate
                -> Integrity
                    -> State Interpretation
                        -> deterministic State Authority
                            -> immutable E0 Take
                                -> source-state binding
                                    -> strict StateHash freshness
                                        -> DETERMINISTIC ATOMIC CAUSAL COMMIT
                                            -> append-only E0CausalCommit event
                                            -> new immutable ProductionState projection
                                                -> later Director continuation
```

Principal law:

> Patch 0012 may make history effective only by committing one immutable Accepted Take and every consequence retained as Approved under that Take, with no retained Rejected consequence, against the exact unchanged causal Production state captured before the Take's Access/Context/Performance pipeline. A source checkpoint prevents rebasing; a history-sensitive StateHash prevents stale application; the successful operation returns one immutable causal event plus one reconstructible current projection; failure returns neither. Causal events remain the conceptual source of truth, the Take is never rewritten, the source Current Opportunity is consumed only on success, and next-opportunity authority remains later.

## 40. Approval / implementation gate

This blueprint is architecture only.

Before implementation:

1. recursively adversarial-audit Proposal 0.2 against frozen Blueprint 0.1, approved Patches 0003–0011, current source/tests, engineering hygiene, event-history source-of-truth law, dependency direction, State Authority ownership, Take immutability, pre-generation checkpoint/freshness, Context source-state proof limitation, materialized RecordId semantics, record provenance, hash canonicalization, replay, duplicate identity handling, atomicity, provenance/authentication, invalid-state construction, memory/ARM64 suitability, E0 experiment isolation, and future persistence/branch separation;
2. restart the audit after every material correction;
3. require one complete final pass with zero material corrections and zero worthwhile architectural improvements;
4. obtain explicit user approval;
5. create an implementation handoff for a fresh project chat;
6. do not write Patch 0012 executable code in the architecture phase.
