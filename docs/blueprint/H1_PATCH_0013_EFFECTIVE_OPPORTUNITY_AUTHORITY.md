# H1 Patch 0013 — E0 Effective Opportunity Authority

Status: blueprint proposal 0.5 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
Parent repository checkpoint: `main` at `8c89f998fe6f42e04a75b9090fbcc10f0574f5a2`
Parent machine-tested executable/test authority: H1 Patch 0012 at `39bc078c130ab1165c6a81c1673dd5cd25da3724`
Branch: `h1-patch-0013-effective-opportunity-authority-blueprint`

## 1. Purpose

Patch 0013 defines the smallest deterministic authority boundary that must exist after H1 Patch 0012 before a second Character can enter the Access/Context/Performer pipeline.

Patch 0012 intentionally consumes the source Current Opportunity when an Accepted Take commits successfully:

```text
ProductionState with Current Opportunity = source Character
    -> Accepted Take + Approved consequences
        -> atomic causal commit
            -> postcommit ProductionState with Current Opportunity = null
```

Patch 0007 already freezes the next lifecycle boundary:

```text
successful atomic source commit
    -> causal authority supplies authoritative opportunity history ending at source
        -> re-Bind accepted source Context + Candidate after commit
            -> recompute Director proposal/evaluation after commit
                -> later effective-opportunity authority
                    -> Current Opportunity + corresponding opportunity-history event become effective atomically
                        -> only then may next Access/Context/Performer work begin
```

Patch 0013 implements only that previously reserved effective-opportunity authority.

It does not evolve ProductionState into Access/Context yet. That remains a subsequent architecture boundary because current Access/Context still cannot consume evolved Production state, disclose CharacterClaim, carry a common source StateHash, or inject accepted recent Performance.

## 2. Recovered frozen authority

This proposal preserves all previously approved laws:

- Director manages attention/opportunity; it does not write required outcomes or mutate truth.
- Director output is proposal/evaluation only.
- speculative precommit Director evaluation is discardable zero-authority work.
- postcommit Director evaluation must be recomputed from the accepted source after successful causal commit.
- Current Opportunity establishment and its opportunity-history event must be one coherent atomic authority transition.
- no next Performer may run before effective opportunity establishment succeeds.
- accepted Performance + Approved consequences remain the Patch 0012 atomic causal commit and are not reopened.
- Current Opportunity is a Production projection field but is not a State Interpreter mutation domain.
- deterministic authority, not an LLM, establishes routing state.
- routing authority changes no World/Character/Knowledge/Relationship/Pressure record.
- no clock, randomness, network, provider, model, GPU/NPU, background polling, or hidden mutable global state may affect the transition.

## 3. Why Patch 0013 precedes evolved Access/Context

The immediate post-Patch-0012 state has `CurrentOpportunityCharacterId = null`.

`ProductionStateCheckpoint.Capture(...)` correctly refuses a no-opportunity state because there is no Character legally eligible to enter a new performance source pipeline yet.

Therefore an evolved Production-based Access/Context bridge cannot by itself unlock a second turn. The system first needs deterministic authority to establish the next Current Opportunity.

Correct ordering is:

```text
Patch 0012
atomic source Performance+consequence commit
    -> no-opportunity postcommit state

Patch 0013
postcommit Director recomputation + atomic effective-opportunity transition
    -> next-opportunity Production state

later patch
ProductionState -> Access Control -> Context Composer bridge
    -> second-turn source checkpoint/context
```

This ordering prevents Access/Context from inventing or assuming a next actor.

## 4. Scope boundary

Patch 0013 defines only:

- immutable Core-created E0 OpportunityHistory projection;
- exact genesis anchoring of initial opportunity history;
- exact binding of a no-opportunity postcommit state to its immediately effective source `E0CausalCommit`;
- exact binding of OpportunityHistory to the state in which the source opportunity was established;
- mandatory postcommit `DirectorOpportunityInput.Bind(...)` using the accepted source Context/Candidate;
- mandatory postcommit recomputation using the existing `LeastInterventionDirector` reference strategy;
- atomic establishment of selected Current Opportunity plus its immutable opportunity-transition event;
- history-sensitive Production StateHash advancement for that opportunity transition;
- one-step deterministic replay of the opportunity transition;
- immutable returned OpportunityHistory projection for the next turn;
- fail-closed stale/foreign/mismatched source rejection.

Patch 0013 does not define or implement:

- evolved `ProductionState -> CharacterBoundedAccessControl` integration;
- common `StateHash` identity on `CharacterAccessProjection` or `ContextPacket`;
- proof that the accepted source ContextPacket's disclosed records came from the exact source ProductionState;
- CharacterClaim disclosure into Character context;
- accepted recent Performance injection into Context;
- complete Scene loop orchestration;
- full multi-turn replay from genesis through all event kinds;
- durable persistence/event store/database/recovery transaction;
- branch/canon/rehearsal/retcon/Alternate promotion;
- new Director strategy, learned Director, semantic Director, or round-robin E0-D control execution;
- Observation or World Resolver;
- provider/model execution, retry, spend, cancellation, or authenticated provider-attempt provenance;
- final creator-facing Production/Studio ontology;
- WinUI, Windows AI Foundry, NPU/QNN, MSIX, WACK, or Store behavior.

## 5. Data flow

The live E0 reference path after a successful Patch 0012 commit is:

```text
source OpportunityHistory
    + accepted source ContextPacket
    + exact source E0CausalCommit
    + exact postcommit ProductionState
        -> DeterministicOpportunityAuthority.Establish(...)
            -> prove source causal chain
            -> prove source history anchor
            -> DirectorOpportunityInput.Bind(sourceContext, sourceCommit.Take.Performance, history)
            -> LeastInterventionDirector.Propose(input)
            -> selected Character
            -> result Production projection with only CurrentOpportunity changed
            -> history-sensitive result StateHash
            -> immutable E0OpportunityTransition event
            -> immutable result OpportunityHistory
            -> E0OpportunityTransitionResult
```

No external state is mutated. Either the complete result is returned or no effective opportunity transition exists.

## 6. State-management law

Patch 0013 introduces no second mutable Production authority and no second causal hash chain.

`ProductionState` remains the authoritative current projection.

`StateHash` remains the single history-sensitive causal state identity.

`E0OpportunityTransition` is the append-only causal/history event for the routing transition.

`E0OpportunityHistory` is an immutable derived projection used as deterministic Director input. It is closed-construction and cannot establish Current Opportunity by itself.

Conceptually:

```text
append-only accepted causal commits
append-only effective opportunity transitions
        -> one history-sensitive StateHash chain
        -> immutable current ProductionState
        -> immutable OpportunityHistory projection
```

The returned ProductionState and OpportunityHistory must both be derivable from the same successful opportunity transition.

## 7. OpportunityHistory public shape

Proposed public namespace:

```text
Ensemble.E0.Core.Opportunity
```

Proposed type:

```text
public sealed class E0OpportunityHistory
- SceneId
- LastOpportunityStateHash : StateHash
- CharacterIds : ImmutableArray<CharacterId>

public static E0OpportunityHistory Initialize(ProductionState genesisState)
```

No public constructor.

No public append/add/update method.

Only Core may derive a later history projection from a successful opportunity transition/replay.

`CharacterIds` is the exact chronological sequence of effective Current Opportunity establishments for the current E0 Scene, beginning with the fixture-authored opening opportunity exactly once.

`LastOpportunityStateHash` identifies the exact Production state in which `CharacterIds[^1]` became effective Current Opportunity.

## 8. Exact genesis anchoring

`E0OpportunityHistory.Initialize(genesisState)` succeeds only for an exact genesis Production state.

It must fail closed unless:

1. `genesisState` is non-null;
2. StateHash, SceneId, roster IDs, and Current Opportunity are initialized;
3. roster contains exactly the frozen E0 three unique Characters;
4. Current Opportunity is a roster Character;
5. recomputing the existing Patch 0012 genesis hash envelope over the state projection yields exactly `genesisState.StateHash`.

The final check prevents history reset from an evolved postcommit or post-opportunity state. A later state may resemble genesis structurally, but its history-sensitive StateHash was not created by the genesis envelope.

Initialization returns:

```text
SceneId = genesisState.SceneId
LastOpportunityStateHash = genesisState.StateHash
CharacterIds = [ genesisState.CurrentOpportunityCharacterId ]
```

No synthetic genesis Director event is invented. The fixture-authored opening opportunity is the first authoritative history entry, matching Patch 0007.

## 9. Why OpportunityHistory needs no separate hash

Proposal 0.2 introduced a separate OpportunityHistory hash chain. Recursive audit removed it as redundant.

The authoritative identity already exists:

- OpportunityHistory is closed-construction;
- it can initialize only from an exact genesis StateHash;
- it can advance only from a successful effective opportunity transition;
- `LastOpportunityStateHash` is updated to that transition's exact result StateHash;
- each future causal commit must use that result state as its exact parent;
- each future effective-opportunity authority requires `sourceHistory.LastOpportunityStateHash == sourceCommit.ParentStateHash`.

Therefore the Production `StateHash` chain already commits to the exact event path that produced the current opportunity history.

A second HistoryHash would duplicate causal identity, add another contract/parser/testing surface, and not strengthen the closed-construction trust boundary.

Correct law:

> one causal StateHash chain; OpportunityHistory is a closed projection anchored to its last effective opportunity StateHash.

## 10. Why history remains separate from Production projection

Do not add `OpportunityHistory` into `ProductionStateProjection` in Patch 0013.

Doing so would change the frozen Patch 0012 canonical Production projection and invalidate the machine-validated genesis/postcommit StateHash oracle.

Instead:

- existing Production projection bytes remain byte-for-byte unchanged;
- existing Patch 0012 genesis and causal-commit hash envelopes remain byte-for-byte unchanged;
- initial history is anchored to exact genesis StateHash;
- each later history projection advances only with a successful opportunity transition result;
- later full-session reconstruction may rebuild OpportunityHistory from genesis plus opportunity events.

## 11. Source causal/history binding

`DeterministicOpportunityAuthority.Establish(...)` consumes:

```text
ProductionState postCommitState
E0CausalCommit sourceCommit
ContextPacket sourceContext
E0OpportunityHistory sourceHistory
```

It must fail closed unless all of the following are true:

1. all inputs are non-null and structurally initialized;
2. `postCommitState.CurrentOpportunityCharacterId == null`;
3. postcommit roster is exactly three unique initialized E0 Characters;
4. `sourceCommit.ContractVersion` is the current Patch 0012 contract;
5. `sourceCommit.ResultStateHash == postCommitState.StateHash`;
6. `postCommitState` contains source CommitId as effective;
7. `postCommitState` contains source TakeId as committed;
8. `sourceCommit.Take.Disposition == Accepted`;
9. source Take Scene equals `postCommitState.SceneId`;
10. source Take Character is in current roster;
11. `sourceHistory.SceneId == postCommitState.SceneId`;
12. source history is non-empty;
13. every source-history Character is a current roster Character;
14. `sourceHistory.LastOpportunityStateHash == sourceCommit.ParentStateHash`;
15. `sourceHistory.CharacterIds[^1] == sourceCommit.Take.Performance.SubjectCharacterId`;
16. source ContextPacket identity equals Accepted Performance ContextPacket identity;
17. source Context Scene/subject/opportunity/roster remain structurally valid for Patch 0007 binding;
18. current Production roster equals source Context roster canonically.

The anti-splice chain is:

```text
state where source opportunity became effective
    == sourceHistory.LastOpportunityStateHash
    == sourceCommit.ParentStateHash
        -> exact source causal commit
            -> sourceCommit.ResultStateHash
            == postCommitState.StateHash
```

Because `E0OpportunityHistory` cannot be publicly created/edited, this ties its chronological Character sequence to the same causal state chain rather than accepting arbitrary structurally plausible history as authority.

## 12. Known Context common-state boundary remains open

Patch 0013 does not claim that a source `ContextPacketId` proves every disclosed Context record came from the exact source ProductionState.

That common-state proof was deliberately deferred by Patch 0012 because current Access/Context remains fixture-rooted and has no StateHash/CharacterClaim/recent-Performance evolution contract.

Patch 0013 needs retained/reconstructed source Context only for the already-frozen Patch 0007 structural Director binding:

- Scene;
- source Character;
- source opportunity;
- roster;
- exact Candidate ContextPacket association.

The later Production->Access/Context patch must close the stronger state/context freshness boundary before multi-turn Character context is claimed.

## 13. Mandatory postcommit Director re-Bind/recompute

`Establish(...)` accepts no precomputed Director proposal/evaluation parameter.

It must perform the frozen Patch 0007 sequence internally after the source commit is already proven effective:

```text
var input = DirectorOpportunityInput.Bind(
    sourceContext,
    sourceCommit.Take.Performance,
    sourceHistory.CharacterIds);

var evaluation = LeastInterventionDirector.Propose(input);
```

The resulting evaluation is the only live Director result eligible for this transition.

No speculative precommit evaluation can be promoted directly.

No random fallback, alternate target, hidden repair, or second strategy is attempted if binding/recomputation fails.

## 14. Source Candidate control is already causally bound

The minimal opportunity event does not need to duplicate addressed/nominated Candidate control.

The source `E0CausalCommit` owns the exact Accepted `E0Take`, which owns the exact `CandidatePerformance` and control.

Patch 0008's canonical `CandidateContentHash` includes:

- Candidate contract;
- source Character;
- ContextPacketId;
- visible Performance text;
- exact ordinal addressed Character IDs;
- exact nominated Character ID/null.

Patch 0012's canonical causal Take payload includes that Candidate content identity/hash, and the causal commit result StateHash includes the canonical causal payload.

Therefore the opportunity transition's parent StateHash already causally commits to the accepted Candidate/control that the postcommit Director must use.

Replay receives the exact source `E0CausalCommit` and verifies it is the event that produced the supplied parent state before reconstructing Director input.

## 15. E0 reference strategy only

Patch 0013 establishes only the existing `LeastInterventionDirector` reference strategy.

It does not create a strategy interface merely for future flexibility and does not implement E0-D round-robin execution yet.

The opportunity event is minimally strategy-labeled rather than embedding a least-intervention-specific Trace. This avoids making deterministic diagnostics into duplicated event authority and leaves a later approved control strategy free to reuse or extend the event pattern.

## 16. Strategy contract is replay law

`ensemble.e0.director.least-intervention.v1` becomes a causal replay dependency once Patch 0013 events can establish effective Production state.

Therefore future changes must obey:

- semantic changes to Director v1 selection/canonical structural interpretation may not silently reuse the v1 StrategyContract;
- a materially different strategy must receive a distinct contract identifier;
- replay of an existing v1 opportunity event must continue to execute v1 semantics or fail explicitly as unsupported rather than reinterpret history.

Patch 0013 does not implement strategy-version registries or migration; it freezes this compatibility obligation.

## 17. Effective transition semantics

The effective state transition changes exactly one Production projection field:

```text
CurrentOpportunityCharacterId:
null -> evaluation.Proposal.SelectedCharacterId
```

It does not change:

- origin fixture identity;
- SceneId;
- Character collection/display identities;
- roster;
- any ProductionRecord;
- record lifecycle/protection/text/provenance;
- effective CommitId cache;
- committed TakeId cache.

The selected Character must be exactly one roster Character.

No generic Current Opportunity setter is introduced.

## 18. Narrow Production mutation helper

Patch 0013 must not add an internal helper that accepts arbitrary replacement `ProductionStateProjection` merely to preserve private commit/take caches.

The smallest allowed Production-layer helper is conceptually:

```text
internal ProductionState WithEstablishedOpportunity(
    CharacterId selectedCharacterId,
    StateHash resultStateHash)
```

It must itself fail closed unless:

- current Production opportunity is null;
- selected Character ID is initialized;
- selected Character is exactly one current roster member;
- result StateHash is initialized.

It internally derives:

```text
_projection with { CurrentOpportunityCharacterId = selectedCharacterId }
```

and preserves exact existing effective CommitId/committed TakeId caches unchanged.

Its signature contains only Production/Domain types and creates no Production -> Opportunity dependency.

The Opportunity authority may separately construct the same one-field result projection for canonical hash computation using existing internal Production projection primitives, but it cannot ask Production to accept arbitrary record/roster/origin changes.

## 19. Atomic authority result

Proposed result:

```text
public sealed class E0OpportunityTransitionResult
- Event : E0OpportunityTransition
- State : ProductionState
- History : E0OpportunityHistory
- DirectorEvaluation : LeastInterventionDirectorEvaluation
```

`DirectorEvaluation` is returned for E0 diagnostics/provenance. It is not separately effective authority and is not embedded wholesale in the canonical opportunity event.

The operation constructs all four from one deterministic live transition before returning.

There is no public API that returns a state-only Current Opportunity mutation or a history-only append.

In-memory atomicity means:

- success returns coherent Event + State + History + recomputed evaluation;
- failure returns none and mutates no input;
- adopting one returned component while discarding required causal companions is later orchestration misuse, not a second Core mutation path.

Durable transactional persistence remains out of scope.

## 20. Minimal opportunity transition event

Proposed immutable event:

```text
public sealed class E0OpportunityTransition
- ContractVersion
- ParentStateHash
- ResultStateHash
- StrategyContract
- SelectedCharacterId
```

No public constructor.

No event ID is added in Patch 0013.

`ParentStateHash` is the sole parent causal pointer. Live/replay authority requires the supplied source `E0CausalCommit.ResultStateHash` to equal it, so the exact source commit is already transitively bound—including its CommitId, Accepted Take, Candidate control, State Authority decisions, and consequences.

A separate `SourceCommitId` would duplicate identity already committed by `ParentStateHash` and would be analogous to adding a redundant ParentCommitId alongside the causal state pointer. It is therefore intentionally absent.

The event does not duplicate `DirectorOpportunityInput`, Proposal, Trace, rule, diagnostics, Context prose, Candidate prose/control, Take payload, source CommitId/TakeId, or full OpportunityHistory.

Calling Establish twice against the same immutable valid parent inputs deterministically yields the same event/result. Once the returned state becomes current, it has a non-null Current Opportunity and cannot accept the same transition again.

## 21. StateHash compatibility law

Patch 0013 must preserve both already-validated Patch 0012 StateHash envelopes byte-for-byte:

```text
kind = genesis
kind = causalCommit
```

No existing property order, string, null rule, canonical Production projection, causal payload, or oracle digest may change.

Patch 0013 adds one disjoint hash-envelope kind under the existing generic Production StateHash contract:

```json
{
  "hashContract":"ensemble.e0.production-state-hash.sha256.v1",
  "kind":"opportunityTransition",
  "parentStateHash":"<64-lower-hex>",
  "opportunityPayload":{...},
  "resultProjection":{...existing canonical Production projection...}
}
```

This is an additive domain extension, not a rewrite of existing preimages. The explicit `kind` discriminator separates semantic envelopes.

The existing `StateHash` strong type remains canonical. No second state-hash type is introduced.

## 22. Exact opportunity payload

Contract:

```text
E0OpportunityTransitionContracts.ContractVersion
= "ensemble.e0.opportunity-transition.v1"
```

Exact canonical payload property order:

```json
{
  "schemaVersion":"ensemble.e0.opportunity-transition.v1",
  "strategyContract":"ensemble.e0.director.least-intervention.v1",
  "selectedCharacterId":"..."
}
```

The payload contains no:

- source CommitId/TakeId duplicate;
- full OpportunityHistory array/hash;
- ContextPacket prose;
- Candidate prose/control duplication;
- Director rule/diagnostics;
- timestamps;
- sequence numbers;
- random IDs;
- floats/scores;
- culture-sensitive values.

`ParentStateHash` binds exact source causal history; `StrategyContract` identifies deterministic selection semantics; `SelectedCharacterId` records the established routing result.

## 23. Why the existing Production StateHash contract is extended

`ProductionStateContracts.StateHashContractVersion` is a generic Production-state history hash contract, and Patch 0012 already uses an explicit `kind` discriminator for genesis vs causal commit.

Patch 0013 therefore adds a third disjoint transition kind without changing either existing kind's canonical bytes.

Introducing a second generic StateHash contract string for one transition kind would make one opaque `StateHash` type silently represent separate top-level contract families without protecting any existing oracle.

Compatibility rule:

> same generic Production StateHash contract; new disjoint kind; old envelopes immutable.

Implementation tests must prove both Patch 0012 oracle hashes remain unchanged.

## 24. Result history derivation

On successful transition:

```text
resultHistory.SceneId = sourceHistory.SceneId
resultHistory.CharacterIds = sourceHistory.CharacterIds + selected Character
resultHistory.LastOpportunityStateHash = resultState.StateHash
```

The append operation is Core-internal and can occur only while constructing successful opportunity result/replay.

No history element can be deleted, reordered, replaced, or edited.

A newly established effective opportunity appends exactly one history entry even if the selected Character equals the previous Character, matching Patch 0007.

## 25. One-step deterministic replay

Proposed API:

```text
DeterministicOpportunityAuthority.Replay(
    ProductionState parentState,
    E0CausalCommit sourceCommit,
    E0OpportunityHistory sourceHistory,
    E0OpportunityTransition establishedEvent)
    -> E0OpportunityTransitionResult
```

Replay does not require retained private source Context prose.

It must:

1. validate event contract/identities;
2. require `parentState.StateHash == event.ParentStateHash`;
3. require parent Current Opportunity is null;
4. require parent roster is exactly three unique initialized E0 Characters;
5. require source commit ResultStateHash equals parent StateHash;
6. require parent effective CommitId/TakeId caches contain source commit/take;
7. require sourceHistory LastOpportunityStateHash equals source commit ParentStateHash;
8. require sourceHistory last Character equals Accepted Performance subject;
9. require source History Scene equals parent/source Take Scene;
10. require source history Characters are current roster Characters;
11. reconstruct exact structural `DirectorOpportunityInput` from authoritative parent/source-commit/source-history facts;
12. call `LeastInterventionDirector.Propose(reconstructedInput)`;
13. require event StrategyContract exactly current least-intervention v1 StrategyContract;
14. require recomputed selected Character equals event SelectedCharacterId;
15. recreate result projection with only Current Opportunity changed;
16. recompute opportunity-transition StateHash;
17. require it equals event.ResultStateHash;
18. derive result OpportunityHistory;
19. create result ProductionState only through narrow `WithEstablishedOpportunity` helper;
20. return coherent Event + State + History + fresh DirectorEvaluation.

Replay reconstructs Director input from:

- parent SceneId/roster;
- accepted source Character/ContextPacketId;
- exact Accepted Candidate addressed/nominated control;
- exact source OpportunityHistory.

Those are exactly the structural fields used by frozen Patch 0007 reference Director.

## 26. Live Bind vs replay reconstruction

Live establishment must call `DirectorOpportunityInput.Bind(sourceContext, acceptedCandidate, history)` after successful source commit. This is frozen Patch 0007 law and is not weakened.

Replay is different: it reconstructs an already-effective event from authoritative causal inputs and does not need to redisclose private source Context prose.

Replay may use the existing internal `DirectorOpportunityInput` constructor only after recreating/canonicalizing the same structural fields from authoritative inputs, then must pass that input through existing `LeastInterventionDirector.Propose(...)` validation/strategy.

Replay must preserve Patch 0007 canonical input storage:

- roster ordinal by CharacterId;
- addressed IDs ordinal by CharacterId;
- nominated scalar unchanged;
- OpportunityHistory exact chronological order.

It must not duplicate the least-intervention selection algorithm.

No new public Director input construction bypass is introduced.

## 27. No speculative Director promotion path

The live Establish API accepts no `DirectorOpportunityProposal` or `LeastInterventionDirectorEvaluation` parameter.

The event constructor is unavailable publicly.

Therefore a speculative precommit evaluation cannot be supplied and promoted to effective routing state.

The only live path is postcommit source proof -> `DirectorOpportunityInput.Bind` -> fresh strategy evaluation -> atomic authority transition.

## 28. Stale/foreign source rejection

Fail closed for at least:

- postcommit state already has Current Opportunity;
- source causal event ResultStateHash does not match current state;
- source CommitId/TakeId are not effective in current state;
- source history belongs to another Scene;
- source history LastOpportunityStateHash does not equal causal commit ParentStateHash;
- source history does not end at accepted source Character;
- source ContextPacketId does not match accepted Candidate ContextPacketId;
- source Context Scene/roster does not match Production Scene/roster;
- event is replayed against wrong parent;
- event strategy/selected Character is changed;
- result hash is changed.

No stale transition is rebased automatically.

## 29. Failure semantics

If Establish fails after source causal commit already succeeded:

- source causal commit remains valid/effective;
- supplied postcommit state remains unchanged with Current Opportunity null;
- no opportunity event becomes effective;
- source OpportunityHistory remains unchanged at source Character;
- no alternate target is invented;
- no next Context is composed;
- no next Performer is triggered.

This preserves Patch 0007's frozen failure law.

## 30. No record/truth authority

Patch 0013 cannot create, supersede, deactivate, or reinterpret any ProductionRecord.

It cannot alter:

- WorldState;
- SceneState;
- CharacterKnowledge/Belief/Suspicion/Memory/Goal/Disposition/Circumstance/Claim;
- Relationship;
- Pressure;
- HistoricalTruth;
- UnresolvedProposition;
- Constitution;
- Observation.

Current Opportunity is routing state only.

## 31. Dependency direction

The new Opportunity integration layer may depend on:

- Domain strong IDs;
- Context structural identity for live binding;
- Accepted Candidate control through source causal commit/Take;
- existing Director input/evaluation/strategy;
- Production current projection/StateHash;
- Patch 0012 causal event/Accepted Take provenance.

Production must not depend on Opportunity.

The narrow internal `ProductionState.WithEstablishedOpportunity(CharacterId, StateHash)` helper uses only Production/Domain types and preserves private commit/take caches without accepting arbitrary projection state.

Opportunity may use existing internal Production canonicalization/projection primitives because all Core namespaces share one assembly; no reverse Production dependency is added.

Replay may use existing internal `DirectorOpportunityInput` constructor only after recreating/validating structural fields. No public Director bypass is introduced.

## 32. Public surface discipline

Patch 0013 should add only the public surface required to express the authority boundary, likely:

```text
E0OpportunityTransitionContracts
E0OpportunityHistory
E0OpportunityTransition
E0OpportunityTransitionResult
DeterministicOpportunityAuthority
E0OpportunityTransitionException
```

No extra OpportunityHistory hash type/contract is introduced.

No public:

- CurrentOpportunity setter;
- mutable history collection;
- history append method;
- event constructor;
- arbitrary StateHash constructor/parser;
- Director evaluation constructor;
- Director input bypass;
- generic Production mutation API;
- persistence interface;
- strategy registry;
- provider/model abstraction;
- async/background API.

## 33. Smallest likely implementation surface if approved

Likely production-source additions:

```text
src/Ensemble.E0.Core/Opportunity/
    OpportunityModels.cs
    DeterministicOpportunityAuthority.cs
    OpportunityCanonicalizer.cs

src/Ensemble.E0.Core/Production/ProductionStateModels.cs
    one narrow internal WithEstablishedOpportunity helper
```

No existing Director selection semantic change is expected.

Do not edit Access, Context, Performer, Integrity, State Interpreter, State Authority, Take, or Patch 0012 CausalCommit semantics merely for convenience.

Tests belong in focused Opportunity/Patch0013 files plus Patch 0012 oracle-regression assertions.

## 34. Memory and ARM64/battery implications

Patch 0013 performs synchronous in-memory work only after a Character performance has committed and a next opportunity is requested.

Expected work:

- validate immutable IDs/arrays;
- one deterministic Director selection over E0 three-Character roster and chronological OpportunityHistory;
- one StateHash over compact routing payload plus existing Production projection;
- construct immutable event/state/history objects.

There is no:

- idle work;
- timer/polling;
- thread/service;
- network/provider call;
- GPU/NPU work;
- persistent background allocation.

`E0OpportunityHistory.CharacterIds` grows linearly with effective opportunities. Each event is constant-size with respect to prior opportunity-history length, so event payload growth is linear rather than quadratic.

E0 prioritizes behavioral provenance over premature compression of active history array. This is architecturally compatible with ARM64/low-idle discipline, but no measured power/performance claim is made.

## 35. Determinism

Identical valid:

- postcommit ProductionState;
- source E0CausalCommit;
- live source ContextPacket structural identity;
- source E0OpportunityHistory;

must produce identical:

- DirectorOpportunityInput;
- LeastInterventionDirectorEvaluation;
- selected Character;
- opportunity event payload;
- result StateHash;
- result Production projection;
- result OpportunityHistory.

No environment-dependent input participates.

## 36. Integrity/authority invariants

Patch 0013 hard invariants:

1. effective opportunity requires already-successful Accepted source causal commit;
2. source commit must be exact transition into supplied no-opportunity state;
3. source opportunity history must be closed-construction and anchored to source commit parent StateHash;
4. history must end at source Accepted Performance Character;
5. live source Context must match Accepted Candidate ContextPacket identity;
6. live postcommit Director input must be freshly bound;
7. reference Director result must be freshly recomputed;
8. Director proposal never mutates Production directly;
9. only one roster Character may become Current Opportunity;
10. Current Opportunity establishment and opportunity event/history advancement are atomic in memory;
11. Production helper can change only null Current Opportunity -> selected roster Character;
12. no ProductionRecord changes;
13. routing history advances the single Production StateHash chain even though records do not;
14. existing Patch 0012 hashes remain unchanged;
15. accepted Candidate control used by Director is already causally bound through source Take/Candidate content identity;
16. no next Performer before success;
17. replay rejects altered event semantics or wrong parent/source/history;
18. opportunity event does not duplicate parent CommitId/TakeId, private Context, Candidate control, or full prior history;
19. least-intervention v1 strategy semantics become replay-stable contract law;
20. deferred Production/Context common-state proof is not falsely claimed complete.

## 37. Required implementation tests after approval

At minimum:

### Genesis/history

- exact genesis initializes history;
- history contains fixture-authored opening opportunity exactly once;
- postcommit state cannot initialize/reset history;
- prior opportunity-transition result cannot initialize/reset history;
- invalid/no Current Opportunity genesis fails;

### Source causal/history binding

- exact source commit/result state binds;
- postcommit roster exact three-Character invariant enforced;
- wrong ResultStateHash fails;
- foreign source causal event with nonmatching ResultStateHash fails;
- source CommitId/TakeId must be effective in parent state;
- source history LastOpportunityStateHash mismatch fails;
- source history last Character mismatch fails;
- Scene/roster mismatch fails;
- source ContextPacketId mismatch fails;

### Candidate-control causality

- source Accepted Candidate addressed/nominated control is recovered from exact source commit Take;
- CandidateContentHash differs when addressed/nominated control differs;
- opportunity transition event does not duplicate control;
- replay recomputation uses exact source commit Candidate control;

### Mandatory postcommit Director recomputation

- nomination selects exact nominated Character;
- direct-address selects exact least-recent addressed Character;
- recency fallback selects exact least-recent roster Character;
- repeated same Character remains legal when explicit selection semantics produce it;
- no live API accepts precomputed Director evaluation;
- source Candidate control, not Performance prose, drives nomination/address input;

### Narrow Production mutation

- helper rejects non-null current opportunity;
- helper rejects uninitialized/out-of-roster selected Character;
- helper preserves every non-opportunity projection field;
- helper preserves effective CommitId/TakeId caches;
- no arbitrary `ProductionStateProjection` replacement helper is added for Patch 0013;

### Atomic transition

- parent Current Opportunity must be null;
- result Current Opportunity equals selected Character;
- every non-opportunity Production projection field is semantically identical;
- result StateHash differs from parent;
- result History appends exactly selected Character;
- result History LastOpportunityStateHash equals result StateHash;
- failure leaves inputs unchanged;

### Event minimality/privacy

- event exposes no ContextPacket/Context prose;
- event exposes no Candidate/Take payload;
- event exposes no source CommitId/TakeId duplicate;
- event exposes no full OpportunityHistory array/hash;
- event exposes no Candidate control duplicate;
- event exposes no least-intervention rule/diagnostic fields;

### Canonicalization/oracles

- exact opportunity payload bytes/property order oracle;
- exact opportunity result StateHash oracle;
- opportunity StateHash changes when causally relevant source history/control changes through ParentStateHash or selected result changes;
- culture independence;
- equivalent valid inputs produce byte-identical event/hash;
- existing Patch 0012 genesis hash remains exactly `30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104`;
- existing Patch 0012 postcommit hash remains exactly `057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30`;

### Replay

- exact one-step replay returns semantically identical state/history/evaluation;
- reconstructed Director input exactly matches live canonical structural input;
- wrong parent StateHash fails;
- wrong source causal event fails;
- mismatched source history anchor fails;
- altered strategy contract fails;
- altered selected Character fails;
- altered result hash fails;
- replay does not duplicate Director selection algorithm;

### Strategy compatibility

- event uses exact `ensemble.e0.director.least-intervention.v1` strategy contract;
- unsupported strategy contract fails closed in v1 replay;
- contract audits make silent v1 strategy token/semantic substitution visible;

### Architecture/hygiene

- no OpportunityHistory hash type/parallel hash authority appears;
- no SourceCommitId/SourceTakeId is added to the opportunity event;
- no Access/Context Production overload appears;
- no CharacterClaim/recent-Performance context path appears;
- no public or generic Current Opportunity setter appears;
- no network/filesystem/clock/random/Windows/provider/GPU/NPU/background dependencies appear;
- public Opportunity namespace contains only approved Patch 0013 types;
- all pre-existing Core tests remain green.

## 38. Harness/runtime validation boundary

If implementation is later approved, static analysis by ChatGPT remains advisory.

Required native authority remains the user's Windows ARM64 machine.

At minimum later validation should include:

```text
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug

dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug

dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json

dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
```

Architecture approval or static implementation review must not be described as compiler/runtime validation.

## 39. Explicit non-goals after Patch 0013

Even after successful Patch 0013 implementation, do not claim:

- second-turn Character context works;
- multi-turn Scene loop works;
- CharacterClaim is disclosed correctly;
- accepted previous Performance reaches next Character context;
- full session replay works;
- durable event storage/recovery works;
- E0-A provider/model behavioral execution works;
- Windows AI/NPU execution exists;
- WinUI product behavior exists;
- package/WACK/Store readiness exists.

Patch 0013 creates routing authority only.

## 40. Expected next boundary after Patch 0013

If Patch 0013 validates successfully, the next likely architecture boundary is the deliberately deferred evolved Production Access/Context bridge.

That later patch must resolve, rather than assume:

- exact `ProductionState -> CharacterBoundedAccessControl` projection rules;
- active/inactive ProductionRecord disclosure;
- CharacterClaim disclosure policy;
- accepted recent Performance history source/projection;
- common source StateHash identity/freshness between checkpoint, Access projection, ContextPacket, and later Take/commit binding;
- preservation/versioning of Patch 0004/0005 fixture-based reference oracles;
- privacy-first exclusion of prohibited authoritative/other-Character information.

Patch 0013 must not pre-solve those questions.

## 41. Recursive adversarial audit — corrections through pass 4

Proposal 0.1 through 0.4 were not accepted unchanged.

### Pass 1 corrections

1. **Full Director evaluation in every event duplicated derived strategy data.**
   - Corrected: event carries compact transition authority; fresh evaluation is returned diagnostically and recomputed in replay.

2. **Full OpportunityHistory in every event caused avoidable O(N^2) cumulative event payload.**
   - Corrected: event never stores full prior history; current history remains one linear closed projection.

3. **Proposal 0.1 did not explicitly preserve deferred Context/Production common-state proof.**
   - Corrected: Patch 0013 explicitly refuses to claim Context record provenance/freshness beyond frozen Director structural association.

4. **SourceTakeId duplicated identity already owned by source causal state.**
   - Corrected initially by retaining only SourceCommitId; later pass removed that remaining duplicate too.

### Pass 2 corrections

5. **An arbitrary-projection Production helper would be too broad for routing-only patch.**
   - Corrected: only narrow internal null->selected `WithEstablishedOpportunity(CharacterId, StateHash)` helper is permitted.

6. **Source-state contract relied on downstream invariants without restating E0 roster boundary.**
   - Corrected: live/replay explicitly require exactly three unique initialized roster Characters.

7. **Opening history wording could imply synthetic Director event at genesis.**
   - Corrected: history begins with fixture-authored opening opportunity exactly once; no synthetic genesis Director event.

### Pass 3 corrections

8. **Separate OpportunityHistory hash chain duplicated already-history-sensitive Production StateHash chain.**
   - Corrected: removed HistoryHash type/contract/event field entirely. Closed history is anchored by LastOpportunityStateHash to source commit ParentStateHash.

9. **Compact event causality needed confirmation that omitted Candidate control was not transient.**
   - Confirmed/documented: source causal StateHash binds CandidateContentHash, and CandidateContentHash includes exact addressed/nominated control.

10. **Effective events make Director strategy semantics replay-critical.**
    - Corrected: least-intervention v1 StrategyContract is explicitly replay law; semantic change requires a new contract or preserved v1 implementation.

### Pass 4 correction

11. **SourceCommitId duplicated the parent causal pointer.**
    - Corrected: removed SourceCommitId from event/payload. `ParentStateHash` is the sole parent causal pointer and already binds exact source CommitId/Take/control/consequences through Patch 0012's hash preimage.

Recursive audit continues from Proposal 0.5.

## 42. Remaining recursive audit checklist

Before approval, continue until one complete pass finds no material correction or worthwhile simplification across:

1. Patch 0007 lifecycle fidelity;
2. Patch 0012 scope/oracle preservation;
3. exact postcommit/no-opportunity ordering;
4. source causal-event identity through ParentStateHash;
5. history anti-splice proof;
6. genesis history reset resistance;
7. one StateHash authority chain/no redundant history hash;
8. Candidate-control causal binding;
9. Director postcommit re-Bind/recompute;
10. strategy-version replay compatibility;
11. no speculative proposal promotion;
12. state/event/history atomicity;
13. narrow Production mutation authority;
14. StateHash history sensitivity;
15. minimal event/no duplicated parent identities;
16. no new truth/record authority;
17. one-step replay correctness;
18. live Context vs replay structural provenance;
19. stale/foreign event fail-closed behavior;
20. dependency direction/no cycles;
21. minimal public surface;
22. E0 three-Character scope;
23. canonicalization/property ordering;
24. culture/environment independence;
25. ARM64/no-idle-work suitability;
26. linear history/event growth;
27. no premature persistence/full replay;
28. no premature Access/Context/CharacterClaim/recent-Performance work;
29. no provider/Windows/UI/NPU scope leak;
30. testability without public authority bypasses;
31. validation claim discipline;
32. naming/terminology consistency;
33. whether any simpler design preserves all frozen laws with less authority surface.

## 43. Current decision

Current status:

```text
Patch 0012:
COMPLETE / NATIVE ARM64 VALIDATED / PROMOTED

Patch 0013:
EFFECTIVE OPPORTUNITY AUTHORITY
BLUEPRINT PROPOSAL 0.5
RECURSIVE ADVERSARIAL AUDIT IN PROGRESS
IMPLEMENTATION NOT AUTHORIZED
```

No implementation branch or production-source edit is authorized until this blueprint completes recursive audit and receives explicit user approval.
