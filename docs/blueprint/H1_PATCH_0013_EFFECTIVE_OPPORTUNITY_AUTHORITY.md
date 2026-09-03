# H1 Patch 0013 — E0 Effective Opportunity Authority

Status: blueprint proposal 0.1 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
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
- Statement != fact; routing authority changes no World/Character/Knowledge/Relationship/Pressure record.
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

- immutable authoritative E0 OpportunityHistory projection;
- exact genesis anchoring of the initial opportunity history;
- exact binding of a no-opportunity postcommit state to its immediately effective source `E0CausalCommit`;
- exact binding of authoritative OpportunityHistory to the state in which the source opportunity was established;
- mandatory postcommit `DirectorOpportunityInput.Bind(...)` using the accepted source Context/Candidate;
- mandatory postcommit recomputation using the existing `LeastInterventionDirector` reference strategy;
- atomic establishment of the selected Current Opportunity plus corresponding immutable opportunity-transition history event;
- history-sensitive StateHash advancement for that opportunity transition;
- one-step deterministic replay of an opportunity transition;
- immutable returned OpportunityHistory projection for the next turn;
- fail-closed stale/foreign/mismatched source rejection.

Patch 0013 does not define or implement:

- evolved `ProductionState -> CharacterBoundedAccessControl` integration;
- common `StateHash` identity on `CharacterAccessProjection` or `ContextPacket`;
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
            -> prove source history chain
            -> DirectorOpportunityInput.Bind(sourceContext, sourceCommit.Take.Performance, history)
            -> LeastInterventionDirector.Propose(input)
            -> compute selected Character
            -> create result Production projection with only CurrentOpportunity changed
            -> compute history-sensitive result StateHash
            -> create immutable E0OpportunityTransition event
            -> derive immutable result OpportunityHistory
            -> return one E0OpportunityTransitionResult
```

No external state is mutated during this operation. Inputs remain immutable. Either the complete result object is returned or no effective transition exists.

## 6. State-management law

Patch 0013 introduces no second mutable Production authority.

`ProductionState` remains the authoritative current projection.

`E0OpportunityTransition` is the append-only causal/history event for the routing transition.

`E0OpportunityHistory` is an immutable derived history projection used as deterministic Director input. It is not independently writable and cannot establish Current Opportunity by itself.

Conceptually:

```text
append-only accepted causal commit events
append-only effective opportunity-transition events
        -> immutable current ProductionState projection
        -> immutable OpportunityHistory projection for Director input
```

The returned ProductionState and OpportunityHistory must both be derivable from the same successful `E0OpportunityTransition` event.

## 7. OpportunityHistory public shape

Proposed public namespace:

```text
Ensemble.E0.Core.Opportunity
```

Proposed type:

```text
public sealed class E0OpportunityHistory
- SceneId
- GenesisStateHash
- LastOpportunityStateHash
- CharacterIds : ImmutableArray<CharacterId>

public static E0OpportunityHistory Initialize(ProductionState genesisState)
```

No public constructor.

No public append/add/update method.

Only Core may derive a later history projection from a successful opportunity transition.

`CharacterIds` is the exact chronological sequence of effective Current Opportunity establishments for the current E0 Scene, beginning with the initial genesis opportunity.

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
GenesisStateHash = genesisState.StateHash
LastOpportunityStateHash = genesisState.StateHash
CharacterIds = [ genesisState.CurrentOpportunityCharacterId ]
```

No separate synthetic genesis opportunity event is invented. The initial opportunity is already part of the frozen genesis Production projection and genesis StateHash.

## 9. Why history remains separate from Production projection in Patch 0013

Do not add `OpportunityHistory` into `ProductionStateProjection` in Patch 0013.

Doing so would change the frozen Patch 0012 canonical Production projection and invalidate the machine-validated genesis/postcommit StateHash oracle.

Instead:

- existing Production projection bytes remain byte-for-byte unchanged;
- existing Patch 0012 genesis and causal-commit hash envelopes remain byte-for-byte unchanged;
- opportunity history is represented by the initial genesis anchor plus append-only opportunity-transition events;
- the current derived history projection is closed-construction convenience for deterministic Director input.

This preserves Patch 0012 authority while allowing future full-session reconstruction to combine event kinds later.

## 10. Source causal-chain binding

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
3. `sourceCommit.ContractVersion` is the current Patch 0012 contract;
4. `sourceCommit.ResultStateHash == postCommitState.StateHash`;
5. `postCommitState` contains the source CommitId as effective;
6. `postCommitState` contains the source TakeId as committed;
7. `sourceCommit.Take.Disposition == Accepted`;
8. source Take Scene equals `postCommitState.SceneId`;
9. source Take Character is in the current roster;
10. `sourceHistory.SceneId == postCommitState.SceneId`;
11. source history is non-empty;
12. every source-history Character is a current roster Character;
13. `sourceHistory.LastOpportunityStateHash == sourceCommit.ParentStateHash`;
14. `sourceHistory.CharacterIds[^1] == sourceCommit.Take.Performance.SubjectCharacterId`;
15. source ContextPacket identity equals the Accepted Performance ContextPacket identity;
16. source Context Scene/subject/opportunity/roster remain structurally valid for Patch 0007 binding;
17. current Production roster equals source Context roster canonically.

The `LastOpportunityStateHash == sourceCommit.ParentStateHash` rule is the crucial anti-splice proof:

```text
state where source opportunity became effective
    == sourceHistory.LastOpportunityStateHash
    == sourceCommit.ParentStateHash
        -> source causal commit
            -> sourceCommit.ResultStateHash
            == postCommitState.StateHash
```

Thus arbitrary structurally plausible opportunity history cannot be paired with an unrelated postcommit state.

## 11. Mandatory postcommit Director re-Bind/recompute

`Establish(...)` accepts no precomputed Director proposal/evaluation parameter.

It must perform the frozen Patch 0007 sequence internally after the source commit is already proven effective:

```text
DirectorOpportunityInput.Bind(
    sourceContext,
    sourceCommit.Take.Performance,
    sourceHistory.CharacterIds)

LeastInterventionDirector.Propose(input)
```

The resulting `LeastInterventionDirectorEvaluation` is the only Director evaluation eligible for this transition.

This prevents speculative precommit evaluation from being promoted into authority.

No random fallback, alternate target, hidden repair, or second strategy is attempted if binding/recomputation fails.

## 12. E0 reference strategy only

Patch 0013 establishes only the existing `LeastInterventionDirector` reference strategy.

It does not create a strategy interface merely for future flexibility and does not implement the E0-D round-robin control yet.

Reason:

- Patch 0007 already gives the reference E0 strategy exact deterministic semantics;
- effective-routing authority is the missing boundary under test;
- introducing a strategy abstraction now would add surface without solving a current requirement;
- a later E0-D control patch may reuse the same authority/event pattern with a separately approved deterministic strategy contract.

## 13. Effective transition semantics

The effective state transition changes exactly one Production projection field:

```text
CurrentOpportunityCharacterId:
null -> DirectorEvaluation.Proposal.SelectedCharacterId
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

## 14. Atomic authority result

Proposed result:

```text
public sealed class E0OpportunityTransitionResult
- Event : E0OpportunityTransition
- State : ProductionState
- History : E0OpportunityHistory
```

The operation constructs all three from one deterministic transition before returning.

There is no public API that returns only the state mutation or only appends history.

In-memory atomicity means:

- success returns coherent Event + State + History;
- failure returns none of them and mutates no input;
- adopting `State` while discarding `Event`/`History` is later orchestration misuse, not a second Core mutation path.

Durable transactional persistence remains out of scope.

## 15. Opportunity transition event

Proposed immutable event:

```text
public sealed class E0OpportunityTransition
- ContractVersion
- ParentStateHash
- ResultStateHash
- SourceCommitId
- SourceTakeId
- DirectorEvaluation : LeastInterventionDirectorEvaluation
```

No public constructor.

No event ID is added in Patch 0013.

The transition is already uniquely history-bound by:

- exact ParentStateHash;
- exact source CommitId/TakeId;
- exact canonical Director evaluation;
- exact ResultStateHash.

Calling Establish twice against the same immutable valid parent inputs deterministically yields the same result. Once the returned state becomes current, it has a non-null Current Opportunity and cannot accept the same transition again.

## 16. Event contents and privacy

The opportunity event retains only structural routing provenance already represented by the Director evaluation:

- strategy contract;
- Scene/source/context IDs;
- roster IDs;
- addressed Character IDs;
- nominated Character ID when present;
- chronological OpportunityHistory Character IDs;
- selected Character;
- deterministic rule;
- structural attention diagnostics.

It does not duplicate:

- Context prose;
- private Character state text;
- accepted Performance text;
- State Interpreter proposal text;
- State Authority record contents;
- provider/model/request/response data;
- hidden Director reasoning.

The source `E0CausalCommit` already owns exact Accepted Take provenance; the opportunity event references its CommitId/TakeId rather than duplicating it.

## 17. StateHash compatibility law

Patch 0013 must preserve both already-validated Patch 0012 hash envelopes byte-for-byte:

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

This is an additive domain extension, not a rewrite of existing preimages. The explicit `kind` discriminator prevents collision of semantic envelopes.

The existing `StateHash` strong type remains canonical. No second state-hash type is introduced.

## 18. Exact opportunity payload

Proposed canonical payload property order:

```json
{
  "schemaVersion":"ensemble.e0.opportunity-transition.v1",
  "sourceCommitId":"...",
  "sourceTakeId":"...",
  "director":{
    "strategyContract":"ensemble.e0.director.least-intervention.v1",
    "opportunityContractVersion":"ensemble.e0.director.opportunity.v1",
    "sceneId":"...",
    "sourceCharacterId":"...",
    "sourceContextPacketId":"...",
    "rosterCharacterIds":["..."],
    "addressedCharacterIds":["..."],
    "nominatedCharacterId":null,
    "opportunityHistory":["..."],
    "selectedCharacterId":"...",
    "rule":"recencyFallback",
    "neverOpportunitiedCharacterIds":["..."],
    "recentAttentionPattern":"none"
  }
}
```

Exact rule tokens:

```text
nomination
directAddress
recencyFallback
```

Exact recent-attention tokens:

```text
none
repeatedSameCharacter
twoCharacterAlternation
```

Canonical array semantics:

- roster IDs: existing Director canonical ordinal order;
- addressed IDs: existing Director canonical ordinal order;
- nominated ID: exact ID or JSON null;
- OpportunityHistory: exact chronological order; never sorted;
- never-opportunitied IDs: existing Director canonical ordinal order.

The payload contains no timestamps, sequence numbers, random IDs, floats, scores, token counts, or culture-sensitive values.

## 19. Why the existing StateHash contract is extended rather than replaced

`ProductionStateContracts.StateHashContractVersion` is a generic Production-state history hash contract, and Patch 0012 already uses an explicit `kind` discriminator for genesis vs causal commit.

Patch 0013 therefore adds a third disjoint transition kind without changing either existing kind's canonical bytes.

Introducing a second StateHash contract string for one new transition would make one opaque `StateHash` type silently represent unrelated contract families and complicate later causal chaining without protecting any existing oracle.

The safer compatibility rule is:

> same generic StateHash contract; new disjoint kind; old envelopes immutable.

Implementation tests must prove all Patch 0012 oracle hashes remain unchanged.

## 20. Result history derivation

On successful transition:

```text
resultHistory.SceneId = sourceHistory.SceneId
resultHistory.GenesisStateHash = sourceHistory.GenesisStateHash
resultHistory.CharacterIds = sourceHistory.CharacterIds + selected Character
resultHistory.LastOpportunityStateHash = resultState.StateHash
```

The append operation is Core-internal and can occur only while constructing a successful `E0OpportunityTransitionResult` or replay result.

No history element can be deleted, reordered, replaced, or edited.

## 21. One-step deterministic replay

Proposed API:

```text
DeterministicOpportunityAuthority.Replay(
    ProductionState parentState,
    E0CausalCommit sourceCommit,
    E0OpportunityHistory sourceHistory,
    E0OpportunityTransition establishedEvent)
    -> E0OpportunityTransitionResult
```

Replay does not require the private source ContextPacket prose.

The established event already retains the exact structural `DirectorOpportunityInput` that live authority created by calling `DirectorOpportunityInput.Bind(...)` after commit.

Replay must:

1. validate event contract/identities;
2. require `parentState.StateHash == event.ParentStateHash`;
3. require parent Current Opportunity is null;
4. require source commit ResultStateHash equals parent StateHash;
5. require event source CommitId/TakeId equal the supplied source commit;
6. require parent effective CommitId/TakeId caches contain them;
7. require sourceHistory last state hash equals source commit ParentStateHash;
8. require sourceHistory last Character equals Accepted Performance subject;
9. require event Director input Scene/source/context IDs equal the source commit Take structural identity;
10. require event roster equals parent roster;
11. require event addressed/nominated controls equal the Accepted Candidate control;
12. require event OpportunityHistory equals supplied sourceHistory exactly;
13. recompute `LeastInterventionDirector.Propose(event.DirectorEvaluation.Trace.Input)`;
14. require recomputed Proposal/Rule/diagnostics exactly equal the event evaluation;
15. recreate the result projection with only Current Opportunity changed;
16. recompute the opportunity-transition StateHash;
17. require it equals `event.ResultStateHash`;
18. derive the result OpportunityHistory;
19. return coherent Event + State + History.

This is exact one-step opportunity-transition replay only. It does not claim full session replay from genesis.

## 22. Live source Context vs replay provenance

Live establishment requires the exact validated source `ContextPacket` so the authority obeys Patch 0007's mandatory postcommit `DirectorOpportunityInput.Bind(...)` law.

Replay does not need to retain or redisclose that private Context prose because the effective event preserves the exact structural Director input produced by the successful live binding.

This deliberately separates:

- live authority association proof;
- durable structural routing provenance;
- private Character context content.

## 23. No speculative Director promotion path

The public Establish API accepts no `DirectorOpportunityProposal` or `LeastInterventionDirectorEvaluation` from callers.

The public event constructor is unavailable.

Therefore callers cannot provide a speculative precommit proposal and ask Core to make it effective.

The only live path is postcommit re-Bind + recompute inside deterministic authority.

## 24. Stale/foreign source rejection

Fail closed for at least:

- postcommit state already has Current Opportunity;
- source causal event ResultStateHash does not match current state;
- source CommitId/TakeId are not effective in the current state;
- source history belongs to another Scene;
- source history last StateHash does not equal causal commit ParentStateHash;
- source history does not end at accepted source Character;
- source ContextPacketId does not match accepted Candidate ContextPacketId;
- source Context Scene/roster does not match Production Scene/roster;
- event is replayed against wrong parent;
- event source commit/take identity is changed;
- event Director input/control/history is changed;
- selected Character/rule/diagnostics are changed;
- result hash is changed.

No stale transition is rebased automatically.

## 25. Failure semantics

If Establish fails after the source causal commit already succeeded:

- the source causal commit remains valid/effective;
- the supplied postcommit state remains unchanged with no Current Opportunity;
- no opportunity event becomes effective;
- source OpportunityHistory remains unchanged;
- no alternate target is invented;
- no next Context is composed;
- no next Performer is triggered.

This exactly preserves Patch 0007's frozen failure law.

## 26. No record/truth authority

Patch 0013 cannot create, supersede, deactivate, or reinterpret any ProductionRecord.

It cannot alter:

- WorldState;
- SceneState;
- CharacterKnowledge/Belief/Suspicion/Memory/Goal/Disposition/Circumstance/Claim;
- Relationship;
- Pressure;
- HistoricalTruth;
- UnresolvedProposition;
- Constitution or Observation.

Current Opportunity is routing state only.

## 27. Dependency direction

Conceptual dependency direction:

```text
Domain
  -> Fixture / Access / Context / Performer / Director
  -> Production
  -> StateAuthority / Take / CausalCommit
  -> Opportunity
```

More precisely, the new Opportunity integration layer may depend on:

- Domain strong IDs;
- Context packet structural identity;
- Performer Candidate control through the Accepted Take;
- existing Director input/evaluation/strategy;
- Production current projection/StateHash;
- Patch 0012 causal event/Accepted Take provenance.

Production must not depend on Opportunity.

A small internal `ProductionState.WithOpportunityTransition(...)` helper may preserve existing commit/take caches while accepting a new projection/hash, but its signature must use only Production-layer types and must not reference Opportunity-layer event types.

No dependency cycle is introduced.

## 28. Public surface discipline

Patch 0013 should add only the public surface required to express the authority boundary, likely:

```text
E0OpportunityTransitionContracts
E0OpportunityHistory
E0OpportunityTransition
E0OpportunityTransitionResult
DeterministicOpportunityAuthority
E0OpportunityTransitionException
```

No public:

- CurrentOpportunity setter;
- mutable history collection;
- event constructor;
- arbitrary StateHash constructor/parser;
- Director evaluation constructor;
- generic Production mutation API;
- persistence interface;
- strategy registry;
- provider/model abstraction;
- async/background API.

Exact final type count remains subject to implementation-minimality review, but functionality must not be hidden behind additional speculative abstractions.

## 29. Existing source changes expected if later approved

Smallest likely production-source surface:

```text
src/Ensemble.E0.Core/Opportunity/
    OpportunityModels.cs
    DeterministicOpportunityAuthority.cs
    OpportunityTransitionCanonicalizer.cs

src/Ensemble.E0.Core/Production/ProductionStateModels.cs
    internal state-transition helper only
```

Potentially one internal semantic-comparison helper may be justified if exact Director evaluation replay comparison cannot be expressed without duplication.

Do not edit Access, Context, Performer, Integrity, State Interpreter, State Authority, Take, existing Director selection semantics, or Patch 0012 causal-commit semantics merely for convenience.

Tests belong in a focused Patch 0013/Opportunity surface plus regression assertions for Patch 0012 hashes.

## 30. Memory and ARM64/battery implications

Patch 0013 performs bounded synchronous in-memory work only when a Character performance has already committed and a next opportunity is requested.

Expected work:

- validate immutable IDs/arrays;
- one deterministic Director selection over the E0 three-Character roster and bounded chronological OpportunityHistory;
- one canonical JSON hash over a small structural routing payload plus existing Production projection;
- construct immutable event/state/history objects.

There is:

- no idle work;
- no timer;
- no polling;
- no thread/service;
- no network;
- no provider call;
- no GPU/NPU work;
- no animation;
- no persistent memory resident solely for this patch.

OpportunityHistory grows linearly with effective opportunities. E0 deliberately prioritizes behavioral provenance over premature optimization; a three-Character experiment with bounded runs does not justify a compressed recency index that would weaken auditability.

The design is therefore suitable for the ARM64/battery discipline architecturally, but this proposal makes no measured power/performance claim.

## 31. Determinism

Identical valid:

- postcommit ProductionState;
- source E0CausalCommit;
- source ContextPacket structural semantics;
- authoritative source OpportunityHistory;

must produce identical:

- DirectorOpportunityInput;
- LeastInterventionDirectorEvaluation;
- selected Character;
- E0OpportunityTransition canonical payload;
- result StateHash;
- result Production projection;
- result OpportunityHistory.

No environment-dependent input participates.

## 32. Integrity/authority invariants

Patch 0013 hard invariants:

1. effective opportunity requires an already-successful Accepted source causal commit;
2. source commit must be the exact transition into the supplied no-opportunity state;
3. source opportunity history must chain to that source commit's parent state;
4. history must end at the source Accepted Performance Character;
5. source Context must match the Accepted Candidate's ContextPacket identity;
6. postcommit Director input must be freshly bound;
7. reference Director result must be freshly recomputed;
8. Director proposal never mutates Production directly;
9. only one roster Character may become Current Opportunity;
10. Current Opportunity establishment and opportunity event/history advancement are atomic in memory;
11. no ProductionRecord changes;
12. StateHash changes even though records do not, because routing history changed;
13. existing Patch 0012 hashes remain unchanged;
14. no next Performer before success;
15. replay cannot accept altered event semantics or wrong parent/source history.

## 33. Required implementation tests after approval

At minimum:

### Genesis/history

- exact genesis initializes OpportunityHistory;
- history contains exactly initial Current Opportunity;
- postcommit state cannot initialize/reset history;
- prior opportunity-transition result cannot initialize/reset history;
- invalid/no Current Opportunity genesis fails;

### Source causal binding

- exact source commit/result state binds;
- wrong ResultStateHash fails;
- foreign CommitId/TakeId fails;
- source history last StateHash mismatch fails;
- source history last Character mismatch fails;
- Scene/roster mismatch fails;
- source ContextPacketId mismatch fails;

### Mandatory postcommit Director recomputation

- nomination path selects exact nominated Character;
- direct-address path selects exact least-recent addressed Character;
- recency fallback selects exact least-recent roster Character;
- no API accepts a precomputed evaluation for live establishment;
- source candidate control, not Performance prose, drives nomination/address input;

### Atomic transition

- parent Current Opportunity must be null;
- result Current Opportunity equals selected Character;
- every non-opportunity Production projection field is semantically identical;
- effective CommitId/TakeId caches remain effective and unchanged;
- result StateHash differs from parent;
- result History appends exactly selected Character;
- result History LastOpportunityStateHash equals result StateHash;
- failure leaves inputs unchanged;

### Canonicalization/oracles

- exact opportunity payload bytes/property order oracle;
- exact opportunity result StateHash oracle;
- culture independence;
- event reconstruction from equivalent immutable inputs is byte-identical;
- existing Patch 0012 genesis hash remains exactly `30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104`;
- existing Patch 0012 postcommit hash remains exactly `057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30`;

### Replay

- exact one-step replay returns semantically identical state/history;
- wrong parent StateHash fails;
- wrong source causal event fails;
- altered history fails;
- altered Director input/control fails;
- altered selected Character/rule/diagnostics fails;
- altered result hash fails;

### Architecture/hygiene

- no Access/Context Production overload appears;
- no CharacterClaim/recent-Performance context path appears;
- no generic Current Opportunity setter appears;
- no network/filesystem/clock/random/Windows/provider/GPU/NPU/background dependencies appear;
- public Opportunity namespace contains only approved Patch 0013 types;
- all pre-existing Core tests remain green.

## 34. Harness/runtime validation boundary

If implementation is later approved, static analysis by ChatGPT remains advisory.

Required native authority remains the user's Windows ARM64 machine.

At minimum the later validation gate should include:

```text
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug

dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug

dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json

dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
```

Patch 0013 architecture approval or static implementation review must not be described as compiler/runtime validation.

## 35. Explicit non-goals after Patch 0013

Even after successful Patch 0013 implementation, do not claim:

- second-turn Character context works;
- multi-turn Scene loop works;
- CharacterClaim is disclosed correctly;
- accepted previous Performance reaches the next Character context;
- full session replay works;
- durable event storage/recovery works;
- E0-A behavioral model execution works;
- Windows AI/NPU execution exists;
- WinUI product behavior exists;
- package/WACK/Store readiness exists.

Patch 0013 creates routing authority only.

## 36. Expected next boundary after Patch 0013

If Patch 0013 validates successfully, the next likely architecture boundary is the deliberately deferred evolved Production Access/Context bridge.

That later patch must resolve, rather than assume:

- exact `ProductionState -> CharacterBoundedAccessControl` projection rules;
- current active/inactive ProductionRecord disclosure;
- CharacterClaim disclosure policy;
- accepted recent Performance history source/projection;
- common source StateHash identity/freshness between checkpoint, Access projection, ContextPacket, and later Take/commit binding;
- preservation of Patch 0004/0005 fixture-based reference oracles where required;
- privacy-first exclusion of prohibited authoritative/other-Character information.

Patch 0013 must not pre-solve those questions.

## 37. Recursive adversarial audit checklist

Before approval, recursively review this proposal for:

1. frozen Patch 0007 lifecycle fidelity;
2. Patch 0012 scope preservation;
3. exact postcommit/no-opportunity ordering;
4. source causal-event identity;
5. history anti-splice proof;
6. genesis history reset resistance;
7. Director postcommit re-Bind/recompute;
8. no speculative proposal promotion;
9. state/history atomicity;
10. StateHash history sensitivity;
11. Patch 0012 oracle compatibility;
12. no duplicate state/hash authority;
13. no new truth/record authority;
14. one-step replay correctness;
15. no private Context duplication in opportunity event;
16. event provenance sufficiency;
17. stale/foreign event fail-closed behavior;
18. dependency direction/no cycles;
19. minimal public surface;
20. E0 three-Character scope;
21. deterministic array ordering/canonicalization;
22. no environment/culture dependence;
23. ARM64/no-idle-work suitability;
24. no premature persistence/full replay;
25. no premature Access/Context/CharacterClaim/recent-Performance work;
26. no provider/Windows/UI/NPU scope leak;
27. testability without public authority bypasses;
28. validation claim discipline;
29. naming/terminology consistency;
30. whether any simpler design preserves all frozen laws with less authority surface.

## 38. Current decision

Current status:

```text
Patch 0012:
COMPLETE / NATIVE ARM64 VALIDATED / PROMOTED

Patch 0013:
EFFECTIVE OPPORTUNITY AUTHORITY
BLUEPRINT PROPOSAL 0.1
RECURSIVE ADVERSARIAL AUDIT IN PROGRESS
IMPLEMENTATION NOT AUTHORIZED
```

No implementation branch or production-source edit is authorized until this blueprint completes recursive audit and receives explicit user approval.
