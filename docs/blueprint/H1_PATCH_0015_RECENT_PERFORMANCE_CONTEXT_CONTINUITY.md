# H1 Patch 0015 — E0 Accepted Performance History + Context Continuity

Status: Blueprint Proposal 0.3 — EXPLORATORY; recursive adversarial audit in progress; implementation forbidden
Parent promoted `main` checkpoint: `7475a9397cff9063673908c666a729f0f3cd4525`
Blueprint branch: `h1-patch-0015-blueprint`

## 1. Purpose

Close the accepted-history boundary that Patch 0005 deliberately reserved and Patches 0011–0014 deliberately deferred:

```text
Accepted Take
    -> atomic causal commit
        -> accepted Character-legible Performance history projection
            -> effective next Opportunity
                -> current Production-backed Access
                    -> bounded Context containing accepted Scene history
                        -> next Performer
```

Patch 0014 proves exact current `ProductionState -> Access -> Context` continuity, but its v2 packet still requires:

```text
recentPerformances = []
RecentPerformanceText = ""
```

That is no longer sufficient for Full Ensemble E0. Blueprint 0.1 requires social causality, coherence, accepted historical texture, and Context that can distinguish “what just happened.” Patch 0005 explicitly reserved the plural `recentPerformances` array and stated that a later accepted-history/commit slice must own its item schema, causal ordering, and safe population path.

Patch 0015 proposes that missing slice.

It does **not** call a provider/model and does **not** implement the full Scene loop.

## 2. Source-grounded correction history

The Patch 0015 investigation produced two rejected directions before this proposal.

### 2.1 Generic Character Context Consumption / PerformerInput — rejected

Blueprint 0.1 already freezes:

```text
Production State
    -> Access Control
        -> Context Composer
            -> bounded Character Context
                -> Performer
```

Patch 0006 already implements the semantic `ContextPacket -> CandidatePerformance` contract.

A second generic input/consumption layer would duplicate an existing boundary without new authority.

### 2.2 Immediate-one-Performance-only Context — rejected

Proposal 0.2 attempted to populate Context from only the immediately prior committed Performance.

That is too weak for the frozen behavioral goal. Blueprint 0.1 explicitly permits historical texture to remain true in accepted history without being promoted into durable projected state. If Context retained only one previous Performance, dialogue/action older than one handoff could disappear even while it remained causally important. A stateless or recast Performer would then lose Scene coherence unless the system improperly promoted every utterance/action into durable state or relied on provider memory.

Patch 0005 intentionally reserved a **plural** array and deferred its non-empty causal ordering to accepted-history authority.

Proposal 0.3 therefore uses the complete accepted Performance sequence of the current bounded E0 Scene. Later product Context optimization may introduce relevance/windowing/summarization under a new composition contract; E0 explicitly excludes that optimization.

### 2.3 Provider-attempt provenance — deferred

Provider request framing, Run/Attempt attribution, retry/spend/cancellation, and external disclosure provenance remain necessary later. They should attach only after the semantic Context supplied to a Performer can represent accepted Scene history correctly.

## 3. Frozen authority basis

This proposal relies on existing canonical authority:

### Blueprint 0.1

- Character != Performer.
- A generated attempt becomes history only if accepted as a Take.
- Accepted Performance + approved consequences form one atomic causal commit.
- The conceptual source of truth is append-only causal event history.
- Historical texture is true because it occurred in an accepted Take even when it is not durable projected state.
- Context packets conceptually include recent events / “what just happened.”
- Imported text and fictional dialogue are untrusted creative content, distinct from trusted structured state/system authority.
- Performance may be speech, action, silence, refusal, redirection, or another Character-legible response.
- Partial/rejected/cancelled output must not enter Production history.
- all three Missing Raft E0 Characters are co-present in one bounded Scene.
- full Observation/location/hearing/attention semantics remain a later open design.

### Patch 0005

Patch 0005 freezes:

- `recentPerformances` as a root semantic array;
- v1/v2 historical emptiness;
- `RecentPerformanceText` as a separate rendered authority layer;
- future non-empty item schema and causal ordering as responsibility of a later accepted-history/commit slice;
- no fabricated transcript/history.

### Patch 0006

`CandidatePerformance.VisibleText` is the Character-legible Performance surface. Empty VisibleText is valid silence. Typed address/nomination control is separate control data and is not generic observation eligibility.

### Patch 0011

Rejected/Alternate Takes never enter Production history or recent-performance Context.

### Patch 0012

Only an Accepted Take bound to exact source authority may commit. The resulting `E0CausalCommit` owns the exact Accepted `E0Take`; the Take owns the exact `CandidatePerformance`. The commit StateHash cryptographically binds the Take’s semantic candidate-content identity and the resulting Production projection.

### Patch 0013

The postcommit state receives one canonical effective Opportunity transition. Opportunity history is routing history, not transcript/Performance history.

### Patch 0014

Production-backed Context v2 is exact-state-bound. CharacterClaim and recent-Performance disclosure remain deferred. `SourceStateHash` is non-diegetic. Exact source Context must be freshly recomposable; metadata alone is insufficient.

## 4. Exact Patch 0015 question

> Can Ensemble maintain a closed, deterministic projection of successfully committed Character-legible Performances for the current E0 Scene, keep that projection synchronized with the exact Production transition state, and use it to compose non-empty recent-Performance Context without turning historical dialogue/action into projected truth, Observation, Memory, Belief, Claim, or provider state?

## 5. New authority: accepted Performance history projection

Patch 0015 introduces a **derived immutable history projection**, not a second source of truth.

Working public types:

```text
E0AcceptedPerformanceHistory
E0AcceptedPerformanceHistoryEntry
E0AcceptedPerformanceHistoryContinuity
E0AcceptedPerformanceHistoryException
```

Preferred namespace placement:

```text
Ensemble.E0.Core.CausalCommit
    E0AcceptedPerformanceHistory
    E0AcceptedPerformanceHistoryEntry

Ensemble.E0.Core.Continuity
    E0AcceptedPerformanceHistoryContinuity
```

Rationale:

- the data projection represents accepted commit history and can be consumed by `E0TakeStateBinding` without `CausalCommit -> Continuity` dependency;
- transition/advancement logic must observe both CausalCommit and Opportunity results, so it belongs in the higher Continuity layer;
- Context does not depend on the history type directly; Continuity projects safe semantic recent-Performance items into the internal Context composer.

Exact namespace/public-surface shape remains subject to final reflection audit, but dependency direction is frozen by this proposal unless a later audit finds a cycle.

## 6. History is not Production truth

`E0AcceptedPerformanceHistory` answers only:

> Which Character-legible Performances have been accepted into this bounded Scene’s causal history, in what causal order, and to which Production transition checkpoint is this projection synchronized?

It does not answer:

- what objective claims in those Performances are true;
- what each Character observed, remembered, believed, or knew;
- which consequences became durable state;
- which Performance is “important”;
- what a future provider should summarize;
- what a creator/audience presentation perspective should reveal.

Current authoritative state remains `ProductionState`. The atomic causal event remains `E0CausalCommit`. The new history is a deterministic projection of accepted Character-legible content for bounded Scene continuity.

## 7. Proposed history shape

```text
E0AcceptedPerformanceHistory
- SceneId
- CurrentStateHash
- Entries : ImmutableArray<E0AcceptedPerformanceHistoryEntry>
```

```text
E0AcceptedPerformanceHistoryEntry
- CommitId
- TakeId
- CommitParentStateHash
- CommitResultStateHash
- SubjectCharacterId
- SourceContextPacketId
- CandidateContentIdentityContract
- CandidateContentHash
- VisibleText
```

No public constructors/setters.

No:

- full `E0Take` reference;
- State Authority proposal/decision package;
- record materialization list;
- typed address/nomination control;
- provider/model data;
- raw/partial attempts;
- credentials;
- Observation/Knowledge/Belief/Memory/Claim classification.

The authoritative causal commit continues to retain the richer Take/control/consequence provenance. The history projection retains only the provenance needed to prove and render accepted Character-legible Performance continuity.

## 8. Why `CurrentStateHash` belongs on history

A list of accepted text is not enough. The projection must be synchronized to the exact deterministic spine state.

`CurrentStateHash` advances through both successful commit and successful opportunity transitions:

```text
genesis opportunity-bearing state
    history.CurrentStateHash = genesis StateHash

successful atomic commit
    history.CurrentStateHash = commit.ResultStateHash
    append exactly one Performance entry

successful opportunity transition
    history.CurrentStateHash = opportunity.ResultStateHash
    append nothing
```

Therefore any later Context or Take binding can require:

```text
history.CurrentStateHash == current Production StateHash
```

Skipping a commit projection update, skipping an opportunity projection update, replaying a stale history, or pairing history from another current state fails closed.

`CurrentStateHash` is synchronization metadata. It is never Character-facing text and does not create a second hash authority.

## 9. Initialization

`E0AcceptedPerformanceHistoryContinuity.Initialize(ProductionState genesisState)` succeeds only for exact genesis Production authority:

- supported Production v1;
- initialized StateHash/SceneId;
- valid exact E0 roster/current opportunity;
- recomputed genesis StateHash equals supplied StateHash;
- no effective committed Take/Commit cache entries inconsistent with genesis;
- current Scene valid.

Result:

```text
SceneId = genesis.SceneId
CurrentStateHash = genesis.StateHash
Entries = []
```

No synthetic opening transcript is invented.

## 10. Recording a successful atomic commit

Conceptual API:

```text
RecordCommit(
    E0AcceptedPerformanceHistory sourceHistory,
    ProductionState parentState,
    E0CausalCommitResult commitResult)
```

Required proof:

- all arguments non-null/initialized;
- sourceHistory.SceneId == parentState.SceneId == commitResult.ResultState.SceneId;
- sourceHistory.CurrentStateHash == parentState.StateHash;
- commit event ParentStateHash == parentState.StateHash;
- commit event ResultStateHash == commitResult.ResultState.StateHash;
- commit contract exact;
- Take disposition exactly Accepted;
- Take/Candidate identities initialized;
- Candidate subject belongs to Scene roster;
- Candidate visible text structurally valid under the already-frozen Candidate contract;
- Candidate-content identity/hash agrees with the accepted Take’s Integrity/Interpretation provenance;
- commit result is consistent with the closed deterministic CausalCommit result supplied by Core.

A defensive implementation may replay the commit from the parent state and compare exact result authority if doing so does not duplicate mutation policy; preferred default is to reuse `DeterministicCausalCommit.Replay(...)` rather than implement a second commit algorithm.

Success appends exactly one entry derived from the accepted commit and advances `CurrentStateHash` to the postcommit StateHash.

Failure returns no new history.

## 11. Recording a successful Opportunity transition

Conceptual API:

```text
RecordOpportunity(
    E0AcceptedPerformanceHistory sourceHistory,
    ProductionState postCommitState,
    E0OpportunityTransitionResult opportunityResult)
```

Required proof:

- sourceHistory.CurrentStateHash == postCommitState.StateHash;
- sourceHistory.SceneId matches both states;
- postcommit state has no Current Opportunity;
- event ParentStateHash == postCommitState.StateHash;
- event ResultStateHash == result StateHash;
- exact transition/strategy contracts;
- result CurrentOpportunityCharacterId == event.SelectedCharacterId;
- result roster/Scene remain coherent;
- opportunity result/history/evaluation are structurally coherent closed outputs of Patch 0013 authority.

Success advances only `CurrentStateHash` to the opportunity-bearing result state. It appends no Performance.

This avoids making the accepted Performance history a second Opportunity-history data structure.

## 12. Failure/alternate paths never advance accepted history

No history entry is created for:

- Rejected Take;
- Alternate Take;
- Integrity Reject;
- Integrity RequestAnotherTake;
- unresolved State Authority review;
- malformed Candidate;
- provider refusal/error/timeout/cancellation;
- partial stream;
- failed atomic commit;
- technical diagnostic text.

A successfully Accepted zero-mutation Take **does** append its Performance because historical texture occurred even when no durable consequence changed projected records.

A successfully Accepted Take whose proposed consequences are all authoritatively rejected likewise appends its accepted Performance because the Performance itself entered history atomically with the terminal consequence decision package.

## 13. Causal order and window for E0

For the bounded current E0 Scene:

```text
recentPerformances = every accepted Performance entry in E0AcceptedPerformanceHistory.Entries
```

in exact append order.

No sorting by Character, ID, time, score, semantic relevance, or provider output order.

No truncation, summarization, paraphrase, token budget, relevance selection, recency count, or model-based compression.

Why all current-Scene accepted Performances:

- Patch 0005 intentionally deferred causal ordering to accepted-history authority;
- Blueprint 0.1 requires historical texture to remain recoverable without state explosion;
- E0 explicitly excludes context optimization;
- Full Ensemble E0 must test multi-turn social causality and coherence without relying on hidden provider conversation memory;
- a one-Performance window loses earlier accepted Scene texture too aggressively.

This is an E0 reference rule, not a final product long-context strategy. Any later bounded window/retrieval/summarization changes composition semantics and requires a new approved contract.

## 14. Scene boundary

Patch 0015 history is scoped to one Scene.

Current Fixture Dialect v1 has exactly one Scene and exactly three co-present Characters. Patch 0015 therefore does not define:

- carrying transcript history between Scenes;
- archive retrieval into a new Scene;
- scene-to-scene memory promotion;
- branch/canon merging;
- cross-Production Character continuity.

When a future Scene transition exists, its architecture must explicitly decide which prior history becomes Context and under what Character access/observation/memory law.

## 15. E0 co-present-trio disclosure contract

Fixture Dialect v1 freezes exactly:

```text
ensemble.e0.copresent-trio.v1
```

and Missing Raft freezes all three Characters as co-present for the bounded E0-A observation window.

Patch 0015 now gives that previously non-executable token the **minimal E0 recent-Performance disclosure meaning** needed for the experiment:

> Every successfully committed `CandidatePerformance.VisibleText` in the current E0 Scene is Character-legible recent Performance content for each current roster Character.

This is newly specified Patch 0015 behavior; earlier sources froze the token/co-presence but did not silently define this executable rule.

The rule applies only to the Character-legible Performance layer, not to hidden typed control, provider diagnostics, creator-only information, or inferred consequences.

It is not a global law that every Character always sees/hears everything. Future observation contracts may be narrower and must not inherit this behavior without explicit authority.

## 16. This is not CharacterObservation generation

Recent Performance disclosure remains separate from the future epistemic causal path:

```text
Event happened
    -> observation eligibility
        -> CharacterObservation
            -> possible Memory/Belief/Claim changes
```

Patch 0015 does not materialize `CharacterObservation` records.

Receiving/rendering accepted recent Performance therefore does not itself mean:

- the Character now “knows” every proposition in the prose;
- quoted claims became facts;
- internal implications became beliefs;
- content became durable memory;
- future visibility/hearing/location rules are solved.

This preserves the truth/claim/belief/memory distinctions in Blueprint 0.1.

## 17. Typed control is not recent Performance content

Patch 0006 typed control (`AddressedCharacterIds`, `NominatedCharacterId`) already served deterministic routing/Director semantics.

Patch 0015 history provenance may retain source Context/candidate identity, but `recentPerformances` contains only:

```text
SourceCharacterId
VisibleText
```

No typed control is disclosed as Character semantic history.

Reason: Patch 0006 explicitly states typed control is not generic causal impact or observation eligibility. Exposing it to future Performers would create a new hidden knowledge channel.

## 18. Context semantic DTO

Add:

```text
ContextRecentPerformance
- SourceCharacterId : CharacterId
- VisibleText : string
```

Read-only; Core-internal constructor.

`ContextPacket` adds:

```text
RecentPerformances : ImmutableArray<ContextRecentPerformance>
```

Rules:

- v1: exactly empty;
- v2: exactly empty;
- v3: one or more entries, exact E0 accepted-history order;
- item source Character resolves exactly once in packet roster;
- VisibleText satisfies existing canonical text discipline and may be exactly empty for silence.

No CommitId/TakeId/StateHash/ContextPacketId/candidate hash/control fields enter this Character semantic DTO.

## 19. Context contract preservation and v3

Historical contracts remain byte-immutable:

```text
v1 schema              ensemble.e0.context.v1
v1 composition         ensemble.e0.context.full-authorized.v1
v2 schema              ensemble.e0.context.v2
v2 composition         ensemble.e0.context.production-bound.v1
render v1              ensemble.e0.context.render.v1
```

Proposal 0.3 proposes:

```text
v3 schema              ensemble.e0.context.v3
v3 composition         ensemble.e0.context.production-bound.accepted-history.v1
render v2              ensemble.e0.context.render.v2
```

V1/v2 continue to require `RecentPerformances.Length == 0` and empty `RecentPerformanceText`.

V3 requires:

- initialized `SourceStateHash`;
- Production-backed current Access;
- one or more exact accepted Scene Performances;
- exact render-v2;
- no CharacterClaim disclosure.

Every hybrid schema/composition/render combination fails closed.

Token values remain provisional until the final clean blueprint pass; if they change during audit, no compatibility promise attaches before approval.

## 20. Genesis semantics

Genesis has no accepted Performance history.

Therefore the history-aware live Context path at exact genesis emits the existing Patch 0014 **v2** packet unchanged:

```text
recentPerformances = []
RecentPerformanceText = ""
render-v1
```

No empty-history v3 packet is created.

This preserves the meaning:

```text
v2 = exact Production-bound current-state Context with no accepted recent Performance layer
v3 = exact Production-bound current-state Context plus non-empty accepted Performance history
```

## 21. Historical evolved v2 remains valid

Patch 0014 already machine-validated evolved v2 Context with empty recent Performance.

Patch 0015 does not rewrite or invalidate those bytes or the existing three-argument Take binding compatibility law.

However, the **new live E0 continuation path** must use accepted-history-aware Context. After at least one committed Performance exists, that path emits v3. Provider execution added later for Full Ensemble E0 must consume this history-aware path rather than silently using history-omitting v2.

The old one-argument `E0ProductionContextContinuity.Compose(checkpoint)` remains an explicit current-state-only compatibility API. It does not claim Full Ensemble turn continuity.

## 22. History-aware Continuity API

Preserve:

```csharp
E0ProductionContextContinuity.Compose(ProductionStateCheckpoint checkpoint)
```

Add:

```csharp
E0ProductionContextContinuity.Compose(
    ProductionStateCheckpoint checkpoint,
    E0AcceptedPerformanceHistory history)
```

Behavior:

```text
validate exact checkpoint/current Production
    -> validate history.CurrentStateHash == checkpoint.StateHash
    -> validate Scene/roster/history structural invariants
    -> fresh Production Access once
    -> if history empty and checkpoint exact genesis:
           existing internal v2 composition
       else:
           project history entries to ContextRecentPerformance[]
           internal v3 composition
    -> verify Packet/Trace SourceStateHash == checkpoint.StateHash
    -> return same Access + Context evaluation result shape
```

Non-genesis history-aware composition with an empty history fails closed.

A non-empty history paired with a genesis checkpoint fails closed.

## 23. Why history-aware empty is genesis-only

A caller must not erase accepted history simply by presenting an empty closed history object beside an evolved state.

Therefore the history-aware path recomputes the exact genesis StateHash when `history.Entries` is empty.

If the checkpoint is not exact genesis, empty history is invalid.

This closes the accidental omission path while preserving the one-argument Patch 0014 compatibility API for historical/regression use.

## 24. Structured v3 canonical order

V3 preserves the v2 root order:

```text
schemaVersion
compositionContract
sourceStateHash
sceneId
subjectCharacterId
opportunityCharacterId
roster
sceneState
pressures
constitution
disposition
circumstance
observations
knowledge
beliefs
suspicions
memories
goals
relationships
recentPerformances
```

`recentPerformances` item property order:

```text
sourceCharacterId
visibleText
```

Recent Performance array order is accepted causal append order and is **not sorted**.

All historical record/roster ordering rules remain unchanged.

## 25. V3 structured identity

```text
StructuredContextHash = SHA256(canonical v3 structured bytes)
ContextPacketId = "CTX:" + StructuredContextHash
```

Therefore:

- different accepted Performance text changes Context identity even if durable projected records are identical;
- different Performance source Character changes Context identity;
- different causal Performance order changes Context identity;
- one silent Performance differs from no accepted Performance;
- history content cannot be replaced while preserving exact ContextPacketId except by cryptographic collision.

`SourceStateHash` remains exact current Production identity. It is not replaced by history identity.

## 26. Render-v2 exact shape

A non-empty recent Performance layer is a Character-visible semantic change, so render-v1 cannot be reused.

Render-v2 preserves `TrustedStateText` and `OpportunityText` byte-for-byte under their v1 algorithms. Only `RecentPerformanceText` changes.

Exact render-v2 `RecentPerformanceText`:

```text
[RECENT PERFORMANCES]
<source display name>:
[PERFORMANCE]
- <first source line>
  <continuation source line>

<next source display name>:
[PERFORMANCE]
- <first source line>
  <continuation source line>
```

Rules:

- entries appear in accepted causal order;
- exactly one blank line separates entries;
- no trailing LF;
- source display name resolves from current packet roster;
- source text is never trimmed, paraphrased, summarized, repaired, or reordered;
- non-empty VisibleText uses the same LF split / two-space continuation discipline as existing Context text rendering;
- Character IDs are not rendered;
- no causal/provenance IDs are rendered.

## 27. Silence rendering is unambiguous

Empty `CandidatePerformance.VisibleText` is already-defined valid silence.

Structured v3 represents it exactly as:

```json
{"sourceCharacterId":"...","visibleText":""}
```

Render-v2 represents a silent entry exactly as:

```text
<source display name>:
[PERFORMANCE: SILENCE]
```

There is no `[PERFORMANCE]` line or bullet body for silence.

This is deliberately distinguishable from a non-silent Character who literally performs text such as `[PERFORMANCE: SILENCE]`, which renders under the non-silent form:

```text
<source display name>:
[PERFORMANCE]
- [PERFORMANCE: SILENCE]
```

Thus semantic silence and literal marker text cannot collide at the rendered layer.

## 28. Recent Performance remains untrusted creative content

`RecentPerformanceText` is not merged into `TrustedStateText`.

Future provider request framing must preserve separate authority layers for:

```text
system/Performer contract
trusted state
recent accepted fictional Performance
opportunity
imported/user content where applicable
```

Patch 0015 does not build the provider request.

A prior accepted Character line that contains prompt-like language remains fictional/untrusted content and cannot modify Access, State Authority, retry/spend policy, or output contract merely because it appears in history.

## 29. History-aware Take binding

The current `E0TakeStateBinding.Bind(checkpoint, context, take)` remains exact v1/v2 compatibility behavior.

Add an overload:

```csharp
E0TakeStateBinding.Bind(
    ProductionStateCheckpoint checkpoint,
    ContextPacket context,
    E0Take take,
    E0AcceptedPerformanceHistory history)
```

Because the history data type lives at the CausalCommit layer and contains no Opportunity/Continuity behavior, this does not create `CausalCommit -> Continuity` or `CausalCommit -> Opportunity` dependency.

V3 proof:

1. validate checkpoint/current Production;
2. validate history structure and `history.CurrentStateHash == checkpoint.StateHash`;
3. validate exact Scene association;
4. reject empty history unless exact genesis, in which case v2 is expected rather than v3;
5. fresh Production Access from current state;
6. project exact ordered history entries to safe `ContextRecentPerformance` semantics;
7. fresh internal v3 Context composition;
8. compare exact canonical structured bytes;
9. compare exact canonical rendered bytes;
10. compare ContextPacketId;
11. compare StructuredContextHash;
12. compare RenderedContextHash;
13. compare SourceStateHash;
14. then run inherited Take/StateAuthority snapshot checks.

No caller-supplied arbitrary recent prose parameter exists.

## 30. History validation at read boundaries

Although normal public construction is closed, Context/Take boundaries defensively reject malformed history objects.

At minimum:

- Entries not default;
- SceneId initialized;
- CurrentStateHash initialized;
- every entry non-null;
- CommitId/TakeId/parent/result hashes initialized;
- CharacterId/ContextPacketId initialized;
- candidate identity contract exact;
- candidate hash exact lowercase 64-hex format;
- VisibleText non-null and structurally canonical;
- CommitIds unique;
- TakeIds unique;
- each source Character resolves in current roster;
- first/next history entry ordering invariants established by closed transition authority;
- no current history state mismatch.

Do not attempt full event replay from these projected entries; they intentionally do not contain the complete causal commit payload.

## 31. Why no separate history hash is added

Proposal 0.3 does not add `PerformanceHistoryHash`.

Reasons:

- every history update is closed and synchronized to an existing Production `StateHash` transition;
- each committed Performance already affects the causal StateHash chain through the accepted Take/candidate-content identity;
- Context v3 directly hashes the exact disclosed ordered Performance semantics;
- a second history hash would create parallel identity authority without a current consumer.

A future durable serialized history format may require its own authenticated/canonical envelope. That is persistence scope, not Patch 0015.

## 32. Causal commit remains source event authority

The new history projection is derived from successful `E0CausalCommitResult` objects.

It does not replace or weaken:

```text
E0CausalCommit
- exact Accepted Take
- exact parent/result StateHash
- exact record materializations
```

If history is discarded, accepted causal events remain the conceptual reconstructive source once durable event persistence exists.

Patch 0015 does not claim durable reconstruction yet.

## 33. Opportunity history remains routing-only

`E0OpportunityHistory` remains unchanged.

Do not add:

- visible Performance text;
- CommitId;
- TakeId;
- ContextPacketId;
- transcript semantics

to Opportunity history.

The two projections answer different questions:

```text
E0OpportunityHistory
    who held effective opportunity in routing order?

E0AcceptedPerformanceHistory
    which Character-legible Performances were accepted in Scene causal order?
```

Neither replaces Production StateHash authority.

## 34. Production projection remains unchanged

Recent Performance history must not be added to `ProductionStateProjection`.

That would:

- duplicate causal history into current state;
- make historical texture look like durable projected state;
- cause unnecessary state growth;
- alter Patch 0012/0013 StateHash oracles;
- undermine Blueprint 0.1’s historical-texture/durable-consequence distinction.

Consequences continue to reach current Character state only through committed Production mutations and current Access.

## 35. Performance versus consequence remains distinct

Patch 0015 Context can therefore contain both:

```text
trusted current state
    consequences that became current authority and passed Access

recent accepted Performances
    exact Character-legible historical texture
```

The same prior event may contribute to both layers for different reasons.

They must not be merged or deduplicated merely because prose appears semantically similar.

## 36. CharacterClaim remains deferred

A prior Performance may contain a claim in its visible prose. That does not make the proposition a `CharacterClaim` record, Knowledge, Belief, or truth for the receiving Character.

Patch 0014 `CharacterClaimDisclosureDeferred` remains exact for retained Production records.

Patch 0015 says only that accepted Character-legible Performance occurred in the co-present E0 Scene.

## 37. ObservationContract representation

Current Production projection does not separately carry the fixture `ObservationContract` token.

Proposal 0.3 does not add it to Production solely for this patch.

Fixture Dialect v1 validates exactly one E0 observation token and Production genesis can originate only from a validated E0 v1 fixture with immutable origin identity already state-bound.

Therefore the new E0 accepted-history continuity path is explicitly versioned as an **E0 v1 reference behavior**.

If a later fixture dialect introduces multiple observation contracts, this assumption must be reopened and the active observation contract must become independently authoritative at the appropriate boundary. Patch 0015 must not generalize beyond the current dialect.

## 38. Multiple-turn proof without durable replay

Proposal 0.3 supports multiple live E0 turns without claiming full replay/persistence because history advancement is incremental and closed:

```text
history at current StateHash
    + closed successful commit result
        -> advanced history at postcommit StateHash
            + closed successful opportunity result
                -> advanced history at next current StateHash
```

Each accepted Performance is appended exactly once at the commit boundary.

No older commit has to be rediscovered from current projection, and no reverse reconstruction is required.

This is stronger and simpler than Proposal 0.2’s caller-supplied “immediate prior commit” proof and avoids the need to lower Opportunity hash canonicalization into Production solely for Context.

Full replay from genesis, deserializing/re-authenticating a history projection, branch recovery, and durable persistence remain separately deferred.

## 39. Exact current-state association prevents omission/splice

History-aware Context/Take paths require exact equality:

```text
history.CurrentStateHash == checkpoint.StateHash
history.SceneId == checkpoint.SceneId
```

Because `RecordCommit` and `RecordOpportunity` advance the history state anchor only from corresponding closed successful transition results:

- stale history fails;
- history from another branch/current state fails;
- a history that skipped a successful commit fails;
- a history that skipped opportunity advancement fails;
- a history advanced with a foreign transition result fails;
- an empty history cannot be paired with evolved state.

This is association proof for an in-memory closed projection. It is not a durable-store authentication claim.

## 40. Public surface restraint

Preferred public surface additions are limited to:

```text
CausalCommit namespace
- E0AcceptedPerformanceHistory
- E0AcceptedPerformanceHistoryEntry

Continuity namespace
- E0AcceptedPerformanceHistoryContinuity
```

plus:

- `ContextRecentPerformance` and `ContextPacket.RecentPerformances`;
- one history-aware Continuity overload;
- one history-aware Take-binding overload;
- v3/render-v2 contract constants.

Avoid:

- generic transcript interfaces;
- provider abstractions;
- event bus;
- repository/store interfaces;
- history service DI framework;
- new Scene-loop orchestrator;
- generic observation framework.

## 41. Canonical compatibility authority

Implementation must preserve exact machine-established historical identities.

### Context v1 Missing Raft / VOSS

```text
Structured bytes = 2569
StructuredContextHash = bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b
Rendered bytes = 1905
RenderedContextHash = ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88
```

### Context v2 genesis / VOSS

```text
Structured bytes = 2655
StructuredContextHash = 27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
RenderedContextHash = ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88
```

### Context v2 canonical evolved / MARLOWE

```text
Structured bytes = 3456
StructuredContextHash = 9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00
RenderedContextHash = 9925327709d4a99697d4b65e5ad870e6e3ff5c5d573e25c61814e48d77163a2d
```

### Production chain

```text
Genesis StateHash = 30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
Patch 0012 postcommit = 057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30
Patch 0013 opportunity result = dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
```

No Patch 0015 architecture requires changing any of those bytes/hashes.

## 42. Canonical first v3 reference chain

The existing independent Patch 0012 -> Patch 0013 oracle uses Candidate visible text exactly:

```text
No.
```

because `Patch0012TestSupport.BuildPipeline(...)` defaults `candidateText = "No."`.

That canonical chain is therefore the preferred first Patch 0015 v3 oracle:

```text
VOSS accepted Performance: "No."
Patch 0012 postcommit StateHash: 05703456...
Patch 0013 selected next Character: MARLOWE
current opportunity StateHash: dc7e169f...
accepted Performance history: [ VOSS -> "No." ]
current Character Context: MARLOWE v3
```

Before native validation, an independent oracle must derive exact:

- v3 structured byte count;
- v3 StructuredContextHash;
- v3 ContextPacketId;
- render-v2 byte count;
- v3 RenderedContextHash.

It must first reproduce the inherited v1/v2 and Production hashes above.

A second deterministic test must prove multi-entry append order, and a silence test must prove one committed silent entry is distinct from empty history and literal marker text.

## 43. Determinism

Given identical:

```text
current ProductionStateCheckpoint
+ identical closed accepted Performance history projection
```

history-aware Context composition produces byte-identical:

- Access evaluation/projection;
- structured Context bytes;
- rendered Context bytes;
- StructuredContextHash;
- ContextPacketId;
- RenderedContextHash;
- trace.

History advancement from identical source history + identical closed transition result is byte/semantic deterministic.

No wall clock, randomness, provider state, locale-sensitive sorting, network, or global mutable state.

## 44. Complexity and memory

Let:

```text
R = current retained Production records
A = current permitted records
B = bytes of current permitted state content
H = accepted Performance count in current E0 Scene
P = total bytes of accepted Performance VisibleText in that Scene
```

Expected history-aware Context cost:

```text
Access:       O(R)
Context:      O(A log A + B + H + P)
```

History append with `ImmutableArray` may copy O(H) references/entries per append. That is acceptable for the bounded E0 reference harness; product-scale history storage is not claimed.

Repeatedly rendering full current-Scene accepted history means total run work can grow superlinearly with Scene length. E0 intentionally excludes token/context optimization so behavioral fidelity can be measured first.

A later product composer may use bounded retrieval/summarization under a new contract.

## 45. ARM64/battery suitability

Patch 0015 adds synchronous deterministic CPU/memory work only at explicit commit/opportunity/context boundaries.

No:

- idle polling;
- background loop;
- provider/network call;
- filesystem requirement;
- GPU/NPU work;
- Windows AI API;
- timer/random source;
- emulation path.

The patch is therefore suitable for the current native ARM64 deterministic Core/Harness architecture by design, while making no measured power/performance claim.

## 46. Explicit non-scope

Patch 0015 does not implement:

- CharacterObservation generation;
- general observation eligibility/location/hearing/attention;
- CharacterClaim Context disclosure;
- Memory/Belief/Knowledge promotion from recent Performance;
- history relevance/windowing/summarization;
- cross-Scene history retrieval;
- provider/model invocation;
- provider attempt/request provenance;
- retries/spend/cancellation/streaming;
- Integrity model-assisted provider call;
- State Interpreter provider call;
- full Scene-loop orchestration;
- Run manifest/store;
- durable causal-event persistence/recovery;
- full multi-turn replay from genesis;
- branching/canon/retcon/rehearsal;
- World Resolver;
- WinUI;
- Windows AI Foundry/NPU;
- MSIX/WACK/Store certification.

## 47. Expected implementation surface

If approved, patch-first source surface is expected to remain approximately:

```text
src/Ensemble.E0.Core/CausalCommit/
    accepted Performance history data contracts
    history-aware E0TakeStateBinding overload

src/Ensemble.E0.Core/Continuity/
    accepted Performance history transition authority
    history-aware Production Context continuity overload

src/Ensemble.E0.Core/Context/
    ContextRecentPerformance
    v3 contract constants
    v3 canonical serializer/validation
    internal v3 composer
    render-v2 recent Performance rendering

focused Patch 0015 tests
```

Preferred no-change surfaces:

- Fixture JSON;
- Access policy/source;
- Production projection/state transition source;
- Performer candidate semantics;
- Director selection policy;
- Opportunity canonicalizer/hash;
- Integrity semantics;
- State Interpreter semantics;
- State Authority semantics;
- Take semantics;
- Harness provider/runtime execution.

If implementation requires changing those preferred no-change semantics, stop and reopen architecture rather than expanding scope silently.

## 48. Required tests if approved

### History construction/transition

- exact genesis initialization succeeds with empty entries;
- non-genesis initialization rejects;
- foreign Scene rejects;
- stale CurrentStateHash rejects;
- successful commit appends exactly one entry and advances state anchor;
- failed/rejected/alternate/noncommit paths cannot append;
- zero-mutation accepted commit appends;
- all-consequence-rejected accepted commit appends;
- successful opportunity advances state anchor and appends nothing;
- skipped commit/opportunity update causes later history-aware Context failure;
- duplicate CommitId/TakeId entry impossible/fails defensive validation;
- foreign closed transition result rejects.

### Multi-turn

- two and three accepted commits yield two/three entries in exact causal order;
- same Character may appear multiple times without deduplication;
- current history remains synchronized across commit -> opportunity -> commit -> opportunity;
- old entry text remains present even if it created no durable Production record;
- no provider session memory is needed for the Context packet to retain accepted Scene history.

### V1/v2 compatibility

- all historical v1 byte/hash oracles unchanged;
- genesis v2 byte/hash oracle unchanged;
- evolved Patch 0014 v2 byte/hash oracle unchanged;
- old one-argument Continuity remains v2;
- old three-argument Take binding remains exact.

### V3 contracts

- history-aware exact genesis emits v2;
- nonempty accepted history emits v3;
- evolved empty history rejects;
- v3 requires render-v2;
- v1/v2 require empty RecentPerformances;
- every hybrid version combination rejects;
- exact root/item property order.

### Disclosure/privacy/epistemic separation

- only source CharacterId + exact VisibleText enter Context recent semantics;
- typed address/nomination absent;
- CommitId/TakeId/StateHash/ContextPacketId/candidate hashes absent from rendered recent Performance;
- prior private Context state absent;
- CharacterClaim remains denied;
- no CharacterObservation/Knowledge/Belief/Memory record generated;
- accepted claims remain historical speech, not truth.

### Rendering

- exact single-entry rendering;
- exact multi-entry blank-line/order behavior;
- multiline LF/two-space continuation exact;
- Unicode NFC exact;
- exact silence rendering;
- literal `[PERFORMANCE: SILENCE]` non-silent text cannot collide with silence rendering;
- no trailing LF;
- render-v2 envelope exact.

### Exact Take binding

- exact history/current state/context/take accepts;
- stale history rejects;
- changed history entry text rejects;
- changed source Character rejects;
- reordered entries reject;
- dropped entry rejects;
- extra entry rejects;
- tampered structured bytes/hash/ContextPacketId rejects;
- tampered rendered bytes/hash rejects;
- SourceStateHash metadata alone cannot rescue mismatched history.

### Determinism/oracle

- repeat determinism;
- culture invariance (`ar-SA` or equivalent);
- canonical `No.` Patch0012->0013 v3 independent oracle;
- inherited Production/Context hashes preserved;
- no new provider/network/Windows/NPU dependency.

### Public/dependency audit

- no public history constructors/setters;
- only approved new public types/methods/properties;
- `CausalCommit` does not reference `Continuity` or `Opportunity` for history behavior;
- Context does not accept raw arbitrary history/prose from public callers;
- Opportunity history remains unchanged;
- Production projection remains unchanged;
- no generic PerformerInput/AI request abstraction appears.

## 49. Recursive audit order

Every material correction restarts from correctness:

```text
correctness
-> consistency
-> authority
-> accepted-history integrity
-> disclosure/privacy
-> epistemic separation
-> dependency direction
-> version/canonical compatibility
-> multi-turn coherence
-> scope
-> tests
-> simplicity
-> hygiene
-> ARM64 suitability
-> project vision
-> evidence
```

## 50. Proposal 0.3 remaining audit questions

Proposal 0.3 is materially stronger than 0.2 but is not approval-ready until these questions are resolved:

1. Should the accepted-history projection be named `E0AcceptedPerformanceHistory` or `E0AcceptedTakeHistory`, given that the causal source is an Accepted Take but the Context consumer needs only Character-legible Performance?
2. Is `CandidateContentHash` necessary on the projected history entry, or do CommitId/TakeId/state hashes + source Context identity + closed transition construction make it redundant?
3. Should history `RecordCommit` defensively call `DeterministicCausalCommit.Replay` on every append, or is accepting a closed `E0CausalCommitResult` plus exact association checks sufficient and materially simpler?
4. What is the smallest exact validation needed for `RecordOpportunity` without duplicating Patch 0013 selection/hash logic?
5. Should the history-aware `E0TakeStateBinding` overload directly consume the history data type, or should a higher Continuity helper perform v3 proof and then issue a lower verified binding token? Dependency audit currently favors the direct data-type overload because the data type itself has no higher-layer behavior.
6. Does adding `CurrentStateHash` to accepted Performance history make the history projection look too much like a competing state authority, or is the synchronization anchor sufficiently clear and necessary?
7. Should `ContextCompositionTrace` add only accepted-history count/source Character IDs, or remain unchanged because `ContextPacket.RecentPerformances` already makes semantic inclusion inspectable and causal provenance remains in the separate history projection? Current preference: keep trace unchanged unless implementation proves a diagnostic gap.
8. Does the E0 co-present-trio rule need to exclude the source Character from receiving its own prior Performance on a later turn? Current reasoning says no: self-produced accepted action is part of Scene history, but this should be explicitly audited against bounded Character perspective.
9. Does full current-Scene Performance history conflict with “minimal necessary disclosure,” or is it justified for E0 because the entire accepted Scene transcript is Character-legible under the explicit co-present reference contract and context optimization is excluded?
10. Should provider-facing request framing later receive all v3 history verbatim, or may a later Context Composer v4 select/summarize it? Proposal 0.3 says later v4 may narrow it; verify this does not undermine E0 comparison validity.

## 51. Current recommendation

Patch 0015 should be **Accepted Performance History + Context Continuity**.

This is the source-supported missing seam between the already-validated atomic causal history and future real Performer execution. It makes accepted historical texture available to subsequent bounded Character contexts without contaminating Production projection state or prematurely implementing general Observation.

No implementation, approval evidence, implementation handoff, `CURRENT_STATE.md` update, or promotion is permitted until Proposal 0.3 survives recursive adversarial review and explicit user approval.
