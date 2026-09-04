# H1 Patch 0015 — E0 Recent Performance Context Continuity

Status: Blueprint Proposal 0.2 — EXPLORATORY; recursive adversarial audit in progress; implementation forbidden
Parent promoted `main` checkpoint: `7475a9397cff9063673908c666a729f0f3cd4525`
Blueprint branch: `h1-patch-0015-blueprint`

## 1. Purpose

Close the next missing deterministic E0-A continuity boundary after promoted Patch 0014:

```text
accepted Character Performance
    -> atomic causal commit
        -> effective next Opportunity
            -> current ProductionState
                -> bounded Context for the next Character
                    -> exactly what just happened
```

Patch 0014 proves current Production -> Access -> exact source-bound Context, but deliberately keeps:

```text
recentPerformances = []
RecentPerformanceText = ""
```

and explicitly does not prove that the next Character received what the prior Character just did.

Frozen Blueprint 0.1 says a bounded Character context conceptually includes `what just happened`, while also requiring recent performance to remain a separate authority layer from trusted state and imported/user content.

Patch 0015 therefore proposes the smallest E0-only deterministic law that carries the **immediately prior successfully committed Character-legible Performance** into the next current-opportunity Character's Context without turning it into Observation, Knowledge, Belief, Memory, Claim, or Production state.

No provider/model is called in this patch.

## 2. Superseded architecture discovery

Two earlier exploratory directions are rejected and must not be resurrected during implementation.

### 2.1 Generic Character Context Consumption / PerformerInput — rejected

Frozen Blueprint 0.1 already defines:

```text
Production State
    -> deterministic Access Control
        -> permitted information set
            -> Context Composer
                -> bounded relevance packet
                    -> Performer
```

Patch 0006 already implements the semantic `ContextPacket -> CandidatePerformance` boundary.

A new generic `PerformerInput`, Character interpretation layer, or context-consumption projection would duplicate an existing authority boundary without owning new authority.

### 2.2 Provider-attempt provenance — deferred

Provider request framing, external disclosure provenance, Run/Attempt attribution, retry/spend/cancellation, and provider execution remain necessary later.

They are not the immediate predecessor because a multi-turn E0 provider loop would still be behaviorally incomplete while every evolved Context omits the immediately preceding accepted Performance.

Patch 0015 closes scene-turn continuity first; provider-attempt provenance remains a later separately approved patch.

## 3. Frozen source authority

This proposal is grounded in existing canonical authority, not reconstructed chat intent.

### Blueprint 0.1

Constitutional facts:

- Character != Performer.
- Access Control precedes Context composition.
- A Context packet conceptually includes `what just happened`.
- imported text and fictional dialogue are untrusted creative content and must remain separate from system/trusted-state authority.
- a Performance may contain speech, action, silence, refusal, redirection, or another Character-legible response.
- only an accepted Take enters Production history.
- accepted Performance plus approved consequence is one atomic causal commit.
- all three E0 Characters are co-present in one Scene.
- the ontology must not globally assume everyone sees/hears everything.
- a future full observation boundary remains separate.

### Fixture Dialect v1

Every valid current E0 v1 fixture has the exact observation-contract token:

```text
ensemble.e0.copresent-trio.v1
```

The canonical Missing Raft fixture uses that contract and freezes exactly three co-present Characters for the bounded E0-A observation window.

### Patch 0005

Context v1 reserves `recentPerformances` but freezes it to `[]` and `RecentPerformanceText` to empty because no recent-performance history source existed yet.

### Patch 0006

CandidatePerformance preserves:

```text
SubjectCharacterId
ContextPacketId
VisibleText
Typed Control
```

`VisibleText` is the Character-legible Performance surface. Empty VisibleText means silence. Typed address/nomination control is not observation eligibility and is not copied into recent-performance Context.

### Patches 0011–0013

An Accepted Take is packaged immutably, atomically committed into `E0CausalCommit`, and followed by a canonical opportunity transition whose StateHash selects the effective next Character.

### Patch 0014

Production-backed Context v2 is exact-state-bound, but recent Performance and Observation eligibility are separately deferred. Exact Take binding performs fresh Access + Context recomposition; `StateHash` metadata alone is insufficient.

## 4. Exact Patch 0015 question

> Can Ensemble deterministically prove the immediately preceding committed Performance, apply the explicit E0 co-present-trio disclosure contract, and compose that exact Performance into the next Character's bounded Context while preserving Patch 0014 exact-source recomposition and every epistemic/authority distinction?

## 5. Scope boundary

Patch 0015 defines only:

- narrow semantics for `ensemble.e0.copresent-trio.v1` as applied to **immediate recent Performance disclosure**;
- an exact immediate-predecessor source proof from prior accepted `E0CausalCommit` to the current opportunity-bearing Production state;
- one history-aware Production Context continuity path;
- Context v3 structured semantics containing exactly one recent Performance on the evolved path;
- render-v2 semantics for non-empty recent Performance;
- exact v3 fresh recomposition proof in `E0TakeStateBinding`;
- preservation of v1/v2 canonical compatibility;
- deterministic/fail-closed regression and independent-oracle coverage.

Patch 0015 does **not** define:

- a full Observation engine;
- `CharacterObservation` generation;
- location, visibility, hearing, channels, concealment, or attention sensing;
- CharacterClaim disclosure;
- Memory/Belief/Knowledge mutation from recent Performance;
- arbitrary transcript/history retrieval;
- more than the immediate predecessor Performance;
- relevance ranking or summarization of history;
- provider/model invocation;
- provider request framing/provenance;
- retry/cancellation/streaming/spend;
- full Scene-loop orchestration;
- full multi-turn replay from genesis;
- durable persistence/recovery;
- World Resolver;
- WinUI, Windows AI/NPU, MSIX, WACK, or Store work.

## 6. E0 co-present-trio recent-Performance contract

Proposal 0.2 gives the existing fixture token a narrow executable meaning required for E0 turn continuity:

```text
ensemble.e0.copresent-trio.v1
```

For an accepted Character Performance that has successfully entered the current Scene's causal history and is the **immediate predecessor Performance** of the current opportunity-bearing state:

- that Performance is eligible for the current roster Character as `recent Performance` context;
- eligibility applies to the exact Character-legible `CandidatePerformance.VisibleText` only;
- eligibility does not create a `CharacterObservation` record;
- eligibility does not assert that every physical detail in the prose was literally seen/heard through a future spatial/sensory model;
- eligibility does not create Knowledge, Belief, Suspicion, Memory, Claim, or objective truth;
- typed address/nomination control is not disclosed as recent Performance content;
- non-Character provider diagnostics/raw output are not disclosed;
- unaccepted/failed/cancelled/alternate attempts are never disclosed as occurred Performance.

This is an **explicit E0 fixture observation contract**, not a global ontology law. Future fixture/product observation contracts may be narrower and may require the reserved observation subsystem.

## 7. Why this does not silently implement full Observation

Blueprint 0.1 distinguishes:

```text
Event happened
    -> observation eligibility
        -> Character-specific Observation
            -> possible Memory/Belief/Claim changes
```

Patch 0015 stops after the first bounded disclosure decision for one already Character-legible E0 Performance.

It does not materialize an Observation record and therefore cannot be used as proof of later memory, belief, or knowledge.

The E0 reference contract is intentionally simple because all three Characters are co-present and the Fixture Dialect explicitly selects `ensemble.e0.copresent-trio.v1`.

No code may infer the same rule for a future fixture carrying another observation contract.

## 8. Immediate-predecessor law

Recent Performance is not arbitrary history.

A supplied prior `E0CausalCommit` is the immediate recent source only if all of the following can be proved from the current checkpoint and canonical transition rules:

```text
priorCommit.ResultStateHash
    == immediate postcommit state hash

current Production projection with CurrentOpportunity = null
    == the result projection cryptographically bound by priorCommit

current Production StateHash
    == canonical opportunity-transition hash(
           parent = priorCommit.ResultStateHash,
           strategy = frozen least-intervention strategy,
           selected = current opportunity,
           result projection = current Production projection)
```

and:

- current Production still marks prior CommitId as effective;
- current Production still marks prior TakeId as committed;
- prior commit contains an Accepted E0 Take;
- prior Take contains a valid CandidatePerformance;
- prior Candidate subject belongs to the current Scene roster;
- current checkpoint/state/Scene/roster are structurally valid;
- current opportunity belongs to the roster;
- all required contract/version identities are exact.

If any check fails, recent Performance composition fails closed.

## 9. Why StateHash alone remains insufficient

Patch 0014 correctly rejected trusting `SourceStateHash` metadata as proof of exact Context derivation.

Patch 0015 preserves that law.

For Context v3, exact source proof requires both:

```text
current Production authority
+
exact immediate predecessor accepted Performance authority
```

The implementation must freshly reconstruct both sides and compare exact canonical Context bytes/hashes.

A caller cannot obtain v3 acceptance merely by placing a current StateHash beside arbitrary recent prose.

## 10. Dependency problem discovered by audit

Current lower dependency direction is:

```text
Domain / Fixture / internal Provenance
    -> Production
        -> Access
            -> Context
                -> Performer / Integrity / Interpreter / State Authority / Take
                    -> CausalCommit
                        -> Opportunity
```

Patch 0014 explicitly forbids `CausalCommit -> Continuity` and current source makes `Opportunity -> CausalCommit`.

Therefore Patch 0015 must **not** make `CausalCommit` depend on `Opportunity` merely to verify a v3 source Context. That would form a cycle and violate the authority direction.

## 11. Dependency-safe canonical refactor

Proposal 0.2 uses one narrow lower refactor.

The byte-for-byte opportunity-transition StateHash algorithm is moved/delegated into an internal Production-level pure canonical primitive that accepts primitive values:

```text
parent StateHash
transition contract token
strategy contract token
selected CharacterId
result ProductionStateProjection
    -> StateHash
```

Rules:

- exact existing JSON envelope/property order is preserved;
- exact existing `kind = opportunityTransition` is preserved;
- exact existing Patch 0013 StateHash oracle must remain unchanged;
- `OpportunityCanonicalizer` becomes a thin delegating wrapper or uses the shared lower primitive;
- Production does not select the Character and does not know Director policy;
- Production does not depend on Opportunity or Director types;
- the lower primitive merely canonicalizes supplied transition facts.

This is a canonicalization refactor, not a new state-transition authority.

## 12. Shared exact recent source validator

Add one internal deterministic validator in a dependency-safe layer at or below CausalCommit, working title:

```text
E0RecentPerformanceSource
```

It binds:

```text
current ProductionStateCheckpoint
+ prior E0CausalCommit
```

and proves the immediate-predecessor law using:

- existing CausalCommit canonicalization;
- the shared lower Production opportunity-transition hash primitive;
- the frozen E0 least-intervention strategy contract;
- current Production effective CommitId/TakeId caches;
- exact current Scene/roster/opportunity facts.

Its successful result exposes internally only what higher Context continuity requires, for example:

```text
SourceStateHash
SourceCommitId
SourceTakeId
SourceCharacterId
CandidatePerformance
```

No public caller-authored recent text field exists.

The exact namespace/public visibility remains an implementation-surface audit item; preferred default is internal closed construction with only higher public continuity/binding APIs exposed.

## 13. No new authoritative history store

`E0RecentPerformanceSource` is a one-boundary proof object, not:

- a transcript store;
- a second causal history;
- an event store;
- an Opportunity history replacement;
- Production projection state.

It proves one exact predecessor and can be discarded after Context/binding work.

Full durable history remains deferred.

## 14. Context version preservation

Historical contracts remain immutable:

```text
v1 schema:
ensemble.e0.context.v1

v1 composition:
ensemble.e0.context.full-authorized.v1

v2 schema:
ensemble.e0.context.v2

v2 composition:
ensemble.e0.context.production-bound.v1

render v1:
ensemble.e0.context.render.v1
```

Patch 0015 must not populate `recentPerformances` under v1 or v2.

All existing v1/v2 canonical bytes, hashes, ContextPacketIds, and tests remain exact.

## 15. Context v3 proposal

New exact tokens proposed:

```text
RecentPerformanceSchemaVersion = "ensemble.e0.context.v3"
RecentPerformanceCompositionContract = "ensemble.e0.context.production-bound.recent-performance.v1"
RecentPerformanceRenderingContract = "ensemble.e0.context.render.v2"
```

Names of constants may improve during audit; token values should not be frozen until the final blueprint pass.

Context v3 requires:

- initialized `SourceStateHash`;
- exact current Production-backed Access projection;
- exactly one valid immediate recent Performance;
- render-v2;
- no CharacterClaim disclosure.

Hybrid v1/v2/v3 schema/composition/render shapes fail closed.

## 16. Genesis behavior

Genesis has no prior committed Performance.

Therefore the canonical genesis live Context remains Patch 0014 production-bound **v2**, with:

```text
recentPerformances = []
RecentPerformanceText = ""
render-v1
```

Patch 0015 does not invent a v3-empty genesis packet.

This keeps the new version semantically meaningful: v3 means an exact immediate recent Performance is present.

## 17. Historical v2 evolved contexts

Patch 0014 already established valid evolved v2 Contexts with empty recent Performance.

They remain valid immutable historical/regression artifacts and may remain accepted where existing compatibility law already permits them.

Patch 0015 must not rewrite or invalidate their bytes.

However, a new **history-aware live continuation path** after an accepted commit uses v3. Future provider execution intended to satisfy Full Ensemble E0 turn continuity must use the history-aware path rather than silently composing history-omitting v2.

Whether existing `E0ProductionContextContinuity.Compose(checkpoint)` remains available as an explicit compatibility/current-state-only API or is constrained for evolved live orchestration is an API-compatibility audit item. Proposal 0.2 prefers preserving the method unchanged and adding an explicit history-aware overload/path.

## 18. ContextRecentPerformance semantic DTO

Add one minimal semantic type, working name:

```text
ContextRecentPerformance
```

Fields:

```text
SourceCharacterId : CharacterId
VisibleText       : string
```

No:

- CommitId;
- TakeId;
- prior ContextPacketId;
- provider/model identity;
- CandidateContentHash;
- addressed IDs;
- nominated Character;
- mutation/consequence data;
- observation/knowledge/memory labels.

Those are provenance/control/authority facts, not Character-facing recent Performance semantic content.

The source Character must resolve exactly once in the current Context roster.

VisibleText preserves the exact accepted CandidatePerformance text subject to the already-frozen canonical text discipline. It is never summarized, rewritten, interpreted, or sentiment-classified.

## 19. Silence

Patch 0006 defines empty CandidatePerformance `VisibleText` as valid silence.

A recent Performance item therefore still exists when `VisibleText == ""`.

Structured v3 must distinguish:

```text
recentPerformances = []
```

from:

```text
recentPerformances = [ { sourceCharacterId: X, visibleText: "" } ]
```

The latter means a committed silent Performance occurred.

Render-v2 must communicate that empty visible Performance deterministically without fabricating dramatic prose. Proposal 0.2 reserves an explicit protocol marker such as `<silence>` for rendering metadata; exact spelling/format is a rendering-oracle audit item before freeze.

The marker is a representation of the already-defined empty-VisibleText semantic, not a newly authored fictional action.

## 20. Structured v3 canonical order

V3 preserves the v2 root order and replaces only the terminal reserved empty array with its exact semantic item:

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

`recentPerformances` in v3 is exactly one-item for Patch 0015.

Proposed item property order:

```text
sourceCharacterId
visibleText
```

No additional item metadata.

## 21. Structured identity

V3 identity remains:

```text
StructuredContextHash = SHA256(canonical v3 structured bytes)
ContextPacketId = "CTX:" + StructuredContextHash
```

Because recent Performance bytes enter structured canonicalization:

- two otherwise identical current Production states with different immediate accepted Performance text cannot share a v3 ContextPacketId;
- silent Performance differs from no recent Performance;
- different source Character differs even with identical visible text.

`SourceStateHash` still binds the current Production state. Recent Performance does not replace it.

## 22. Render-v2

A non-empty recent-Performance semantic layer is a rendering-semantic change, so Proposal 0.2 does **not** reuse render-v1.

New rendering contract:

```text
ensemble.e0.context.render.v2
```

Render-v2 preserves the exact v1 `TrustedStateText` and `OpportunityText` generation rules.

Only `RecentPerformanceText` gains deterministic non-empty semantics.

Proposed conceptual format:

```text
[WHAT JUST HAPPENED]
<source display name>:
<exact visible Performance text>
```

For multiline text, exact text is preserved under a deterministic indentation/framing rule; no paraphrase.

For empty visible text, render-v2 uses the exact frozen silence marker established before approval.

`SourceStateHash`, CommitId, TakeId, ContextPacketId, provider identity, candidate hashes, and typed control never enter Character-facing rendering.

## 23. Recent Performance is untrusted creative content

Frozen Blueprint 0.1 explicitly separates recent Performance from system/trusted-state authority.

Therefore later provider request framing must place `RecentPerformanceText` in its own untrusted creative-content layer.

Patch 0015 itself does not build the provider request.

No content inside prior Character dialogue/action may alter:

- system instructions;
- access policy;
- output contract;
- State Authority;
- cost/retry policy.

This patch preserves the semantic separation so later prompt construction can enforce it.

## 24. Public history-aware Continuity path

Preferred Proposal 0.2 API direction:

Preserve existing Patch 0014 API unchanged:

```csharp
E0ProductionContextContinuity.Compose(ProductionStateCheckpoint)
```

Add one history-aware overload or sibling method that requires the prior accepted commit:

```csharp
E0ProductionContextContinuity.Compose(
    ProductionStateCheckpoint sourceCheckpoint,
    E0CausalCommit immediatePriorCommit)
```

Behavior:

```text
validate exact current checkpoint
    -> bind exact E0RecentPerformanceSource
        -> fresh Production Access once
            -> internal v3 Context composition
                -> exact source/current/recent associations
                    -> return same Access + Context evaluation result shape
```

No raw recent text parameter exists.

The overload rejects genesis because there is no prior commit; callers use the existing one-argument v2 path at genesis.

Exact method/class naming remains open until public-surface audit.

## 25. Fresh Access remains mandatory

History-aware Continuity still performs Production-backed Character Access from the **current** source state.

Recent Performance bypasses neither Access nor current-state filtering.

The two source channels remain conceptually distinct:

```text
current retained Production records
    -> current Access Control

immediate accepted Character Performance
    -> explicit E0 recent-Performance disclosure contract
```

They meet only at Context composition.

The recent Performance does not masquerade as a Production record and therefore is not injected into `CharacterAccessProjection` merely to reuse record machinery.

## 26. No CharacterClaim reopening

Patch 0014 `CharacterClaimDisclosureDeferred` remains exact.

A prior Performance may contain a claim in its visible prose. That does not make the claim a `CharacterClaim` record, knowledge, memory, or truth for the next Character.

Recent Performance says only:

> this Character-legible Performance just occurred under the active E0 recent-performance disclosure contract.

Any extracted Claim semantics still belong to State Interpreter/authorized state evolution and later disclosure law.

## 27. No Observation record generation

Patch 0015 must not materialize:

```text
CharacterObservation
```

from recent Performance.

This avoids the invalid collapse:

```text
received recent Performance
    != observed fact record
    != remembered fact
    != believed fact
    != known fact
```

Future observation semantics may later derive Character-specific state under an approved causal path; Patch 0015 leaves that open.

## 28. No typed-control leakage

Prior CandidatePerformance typed control already fulfilled its immediate Director routing role.

Recent v3 Context includes only Character-legible `VisibleText` plus source Character identity.

It does not expose:

- `AddressedCharacterIds`;
- `NominatedCharacterId`.

Reason:

- Patch 0006 says these are Performer control, not generic causal impact or observation eligibility;
- Director already consumed them to establish current opportunity;
- exposing them as Character semantic knowledge would create a new channel not frozen by Blueprint 0.1.

## 29. Exact v3 Take binding

Patch 0014 `E0TakeStateBinding` cannot safely accept v3 with its existing three-argument proof because current Production alone cannot reconstruct recent Performance content.

Proposal 0.2 therefore adds a history-aware binding path, likely:

```csharp
E0TakeStateBinding.Bind(
    ProductionStateCheckpoint sourceCheckpoint,
    ContextPacket sourceContext,
    E0Take take,
    E0CausalCommit immediatePriorCommit)
```

Exact shape remains subject to API audit.

V3 proof order:

1. validate checkpoint/current source identities;
2. validate supplied Context schema/composition/render as exact v3 combination;
3. bind/recompute exact immediate recent source from `immediatePriorCommit`;
4. fresh Production Access from current state;
5. fresh internal v3 Context composition using that exact recent source;
6. compare exact canonical structured bytes;
7. compare exact canonical rendered bytes;
8. compare ContextPacketId;
9. compare StructuredContextHash;
10. compare RenderedContextHash;
11. compare SourceStateHash;
12. then run inherited Take/StateAuthority snapshot checks.

No v3 path may fall back to metadata-only equality.

## 30. Existing v1/v2 Take binding remains exact

Existing three-argument `E0TakeStateBinding.Bind` semantics remain:

- historical v1 only under its exact-genesis compatibility law;
- production-bound v2 under Patch 0014 fresh current-state recomposition;
- unsupported/hybrid contracts reject.

Proposal 0.2 does not silently reinterpret old v2 Context as containing recent history.

Whether evolved v2 remains allowed for new post-Patch0015 Take creation or is restricted to historical compatibility is an explicit audit item. Default compatibility posture: do not break already-frozen v2 acceptance in Patch 0015; instead require v3 in the new live history-aware orchestration path.

## 31. Prior commit structural proof

The shared recent-source validator must not trust a supplied `E0CausalCommit` object merely because its hashes are populated.

At minimum it recomputes the prior commit result against the only postcommit projection compatible with the current state:

```text
postCommitProjection = currentState.Projection with
{
    CurrentOpportunityCharacterId = null
}
```

Then:

```text
CausalCommitCanonicalizer.ComputeResultHash(
    priorCommit.ParentStateHash,
    priorCommit.CommitId,
    priorCommit.Take,
    priorCommit.RecordMaterializations,
    postCommitProjection)
    == priorCommit.ResultStateHash
```

This binds the retained current projection (except the later opportunity selection) to the supplied accepted Take/commit payload.

It then proves the opportunity transition from `priorCommit.ResultStateHash` to the exact current StateHash using the shared canonical opportunity hash primitive.

## 32. Current state cache proof

The prior CommitId and prior TakeId must both remain effective in current Production internal caches.

This is necessary but not sufficient; the canonical immediate-predecessor recomputation remains mandatory.

Caches never replace StateHash proof.

## 33. Prior Candidate validation

Recent source validation requires a structurally valid prior CandidatePerformance:

- exact Patch 0006 contract;
- initialized source CharacterId;
- initialized source ContextPacketId;
- source Character in current roster;
- initialized/non-null control object;
- canonical addressed IDs and valid optional nomination under inherited Candidate rules;
- VisibleText satisfies the existing canonical text discipline including valid empty silence.

Do not re-run Integrity/Interpreter/Authority policy for the prior commit; the exact accepted atomic commit is the historical authority being proven.

## 34. Opportunity transition strategy

The immediate current StateHash proof must use the exact frozen E0 least-intervention strategy contract already used by Patch 0013 live/replay authority.

The lower Production hash primitive receives the strategy token as data; it does not select or approve policy.

The recent-source validator supplies/requires the exact E0 strategy token.

A current state whose hash chain was produced under another strategy does not satisfy this Patch 0015 E0 source proof unless a later approved contract explicitly supports that strategy.

## 35. Opportunity history remains separate

`E0OpportunityHistory` is not a Performance transcript.

Patch 0015 does not add Candidate text, TakeId, or CommitId to it.

Opportunity history continues to answer routing recency only.

Recent Performance comes from the accepted causal commit chain.

## 36. Production projection remains unchanged

Do not add recent Performance or transcript content to `ProductionStateProjection` merely to simplify Context recomposition.

Reasons:

- historical texture already lives in accepted causal history;
- duplicating it into current projection would create state explosion and second authority representation;
- Patch 0012/0013 StateHash oracles would change unnecessarily;
- Blueprint 0.1 explicitly distinguishes historical texture from durable projected state.

Patch 0015 preserves current Production projection semantics.

## 37. ObservationContract representation

Current Production projection does not separately carry the Fixture Dialect `ObservationContract` string.

Proposal 0.2 does not add it to Production state merely for Patch 0015.

Current E0 Fixture Dialect v1 accepts exactly one known observation contract:

```text
ensemble.e0.copresent-trio.v1
```

and Production genesis is created only from validated E0 v1 fixtures whose origin fixture hash/version are already state-bound.

Therefore the v1 E0 recent-performance continuity path may rely on the frozen dialect contract as an invariant of the source fixture family.

If multiple observation contracts are introduced later, Production/context authority must be deliberately versioned or supplied an independently authenticated fixture-contract association rather than silently assuming v1 behavior.

## 38. Context trace provenance

Context v3 trace should gain only the minimum non-diegetic recent-source association needed for diagnostics/proof, if required.

Candidate fields for audit:

```text
RecentPerformanceSourceCharacterId
RecentPerformanceSourceCommitId
RecentPerformanceSourceTakeId
```

However, putting CommitId/TakeId into the public Context trace may unnecessarily widen the surface.

Preferred Proposal 0.2 default:

- v3 `ContextPacket` carries only semantic recent Performance;
- `ContextCompositionTrace` may carry source Character identity but not causal IDs unless implementation proves they are needed;
- exact commit/take association remains in the internal recent-source proof and higher evidence/tests.

No causal identifier enters Character-facing rendering.

## 39. Failure semantics

History-aware Context composition fails closed for:

- null/uninitialized checkpoint;
- genesis passed to recent path;
- null/uninitialized prior commit;
- non-Accepted prior Take;
- invalid prior Candidate;
- prior commit not effective in current state;
- prior Take not committed in current state;
- recomputed prior commit result mismatch;
- current state not exact canonical immediate opportunity successor of prior commit;
- wrong strategy token;
- Scene mismatch;
- roster mismatch;
- current opportunity invalid;
- recent source Character absent from roster;
- unsupported Context contract/hybrid shape;
- malformed recent Performance semantic content;
- canonicalization/hash mismatch.

Failure never repairs, fabricates, drops, or substitutes recent Performance silently.

## 40. Atomic/history semantics

Only a successfully committed Accepted Take becomes recent Performance source.

Therefore the following never enter v3 recent Context:

- Rejected Take;
- Alternate Take that was not committed on this history;
- Integrity rejection;
- request-another-take outcome;
- malformed candidate;
- provider refusal/error;
- partial stream;
- cancellation;
- failed State Authority package;
- failed atomic causal commit;
- technical diagnostic text.

This directly preserves Blueprint hard gates 6, 7, 8, and 10.

## 41. Performance versus consequence

The recent Performance layer carries the exact accepted Character Performance, not a prose summary of its authoritative consequences.

Consequences already affect current Production records through the atomic commit and therefore flow through current Access/Context state where permitted.

This preserves two distinct causal channels:

```text
what the Character just did
    -> recent Performance

what became authoritatively true/changed because of it
    -> current Production -> Access -> trusted Character state
```

They must not be collapsed.

## 42. Privacy and disclosure

Recent Performance contains only content already accepted as Character-legible Performance under the explicit current E0 observation contract.

It must not disclose:

- private Production truth not spoken/acted in the accepted Performance;
- another Character's hidden records;
- prior ContextPacket structured private state;
- Interpretation proposal reasoning;
- State Authority decisions;
- creator-only metadata;
- provider diagnostics;
- secrets/credentials.

A prior Performance that itself illegally leaked inaccessible information should have failed Integrity before acceptance; Patch 0015 does not add a second hidden rewriting/filtering pass over accepted history.

## 43. Determinism

Given identical:

```text
current ProductionStateCheckpoint
+ immediate prior E0CausalCommit
```

history-aware continuity must produce byte-identical:

- recent-source proof result;
- Access evaluation/projection;
- structured Context v3 bytes;
- rendered Context v2 bytes;
- StructuredContextHash;
- ContextPacketId;
- RenderedContextHash;
- trace.

No clock, randomness, network, provider, locale-sensitive sort, or mutable global state.

## 44. Complexity

Let:

```text
R = retained current Production records
A = current permitted records
B = bytes of current permitted Context content
P = bytes of immediate prior Performance VisibleText
```

Expected work per history-aware composition:

- recent-source chain validation: O(R + P) only to the extent current projection/commit canonical bytes are serialized;
- Production Access: O(R);
- Context canonicalization/render: approximately O(A log A + B + P);
- exact v3 Take binding: same order plus inherited State Authority snapshot validation.

No background/idle work.

No token-budget optimization is introduced in E0.

## 45. ARM64/battery suitability

Patch 0015 is synchronous deterministic CPU work only at explicit context/take boundaries.

It adds no:

- background polling;
- network call;
- provider SDK;
- filesystem requirement;
- GPU/NPU invocation;
- Windows AI API;
- timer;
- thread loop.

The immediate one-Performance window bounds memory and serialization growth for E0.

This is architectural suitability reasoning, not measured power/performance evidence.

## 46. Canonical regression authority

Implementation must preserve exact historical identities already machine-validated:

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

The lower opportunity canonicalizer refactor must reproduce the Patch 0013 opportunity hash exactly.

## 47. New independent reference oracle

Before native validation, Patch 0015 should derive at least one v3 oracle independently of production Context canonicalization.

Canonical test chain should use the already-independent Patch 0012 -> Patch 0013 source chain and a fixed accepted prior Performance with non-empty VisibleText chosen by the Patch 0015 oracle test.

The independent oracle must first reproduce:

- inherited prior commit StateHash;
- inherited opportunity result StateHash;
- inherited v1/v2 Context identities where applicable;

then derive:

- exact v3 structured bytes/hash/ContextPacketId;
- exact render-v2 bytes/hash.

Also derive a silence v3 oracle or at minimum a fixed determinism test proving one empty-text recent item differs from no-history v2.

Do not freeze numeric v3 digests in this Proposal until exact canonical render text is approved.

## 48. Expected source surface if approved

Patch-first implementation should likely touch only:

```text
src/Ensemble.E0.Core/Production/ProductionStateCanonicalizer.cs
src/Ensemble.E0.Core/Opportunity/OpportunityCanonicalizer.cs
src/Ensemble.E0.Core/CausalCommit/... recent-source + binding surface
src/Ensemble.E0.Core/Context/ContextModels.cs
src/Ensemble.E0.Core/Context/ContextPacketCanonicalizer.cs
src/Ensemble.E0.Core/Context/DeterministicContextComposer.cs
src/Ensemble.E0.Core/Continuity/E0ProductionContextContinuity.cs
focused Patch 0015 tests
```

No fixture JSON change should be needed because the observation contract is already frozen in Fixture Dialect v1 and both canonical fixtures already carry it.

No Access policy change should be needed.

No Production projection change should be needed.

No Performer/Integrity/Interpreter/State Authority/Take semantic change should be needed beyond the exact v3 source-binding overload in CausalCommit.

## 49. Required tests if approved

At minimum:

### Contract/version compatibility

- v1 canonical oracles unchanged;
- v2 genesis/evolved canonical oracles unchanged;
- v3 exact schema/composition/render combination accepted;
- every hybrid v1/v2/v3 combination rejected;
- v3 requires exactly one recent Performance;
- v1/v2 require zero recent Performances.

### Immediate source proof

- canonical Patch0012->0013 chain binds exact prior Performance;
- wrong prior commit rejects;
- older-but-still-effective commit rejects as non-immediate;
- wrong prior result hash rejects;
- wrong current StateHash rejects;
- wrong current opportunity rejects;
- wrong strategy/hash chain rejects;
- missing effective CommitId rejects;
- missing committed TakeId rejects;
- malformed prior Candidate rejects;
- foreign Scene/roster source rejects.

### Disclosure/epistemic separation

- v3 carries only source CharacterId + exact VisibleText;
- addressed/nominated control absent;
- CommitId/TakeId absent from rendered content;
- SourceStateHash absent from rendered content;
- prior structured Context/private records absent;
- CharacterClaim remains denied;
- no CharacterObservation/Knowledge/Belief/Memory record is generated.

### Silence

- committed empty VisibleText produces one recent item;
- silence v3 differs structurally from history-empty v2;
- render-v2 silence marker exact;
- no fabricated prose beyond the frozen protocol marker.

### Causal semantics

- rejected/alternate/uncommitted Take cannot source v3;
- zero-mutation Accepted commit still sources recent Performance;
- all-rejected-consequence Accepted commit still sources recent Performance;
- failed commit cannot source v3;
- consequence records appear only through current Production Access, not recent Performance DTO.

### Exact binding

- fresh v3 recomposition accepts exact current packet;
- changed recent VisibleText rejects even with same SourceStateHash;
- changed source Character rejects;
- tampered structured bytes/hash/ContextPacketId rejects;
- tampered render bytes/hash rejects;
- wrong immediate prior commit rejects;
- old three-argument v1/v2 binding regressions remain exact.

### Canonicalization/determinism

- lower opportunity hash refactor reproduces exact Patch 0013 oracle;
- v3 repeat deterministic;
- culture invariant (`ar-SA` or equivalent);
- multiline Unicode NFC Performance exact;
- canonical property order exact;
- fixed independent v3 reference oracle passes.

### Public/dependency surface

- no `CausalCommit -> Opportunity` dependency;
- no recent transcript collection added to Production projection;
- no public arbitrary recent-text constructor;
- no provider/network/Windows/NPU dependency;
- no new generic PerformerInput/Context Consumption API.

## 50. Recursive audit order

Every material correction restarts at correctness:

```text
correctness
-> consistency
-> authority
-> disclosure/privacy
-> epistemic separation
-> dependency direction
-> causal immediacy
-> canonical/version compatibility
-> scope
-> tests
-> simplicity
-> hygiene
-> ARM64 suitability
-> project vision
-> evidence
```

## 51. Proposal 0.2 unresolved audit items

This proposal is not yet approval-ready. The following must be resolved through source-grounded adversarial review:

1. Does `ensemble.e0.copresent-trio.v1` legitimately authorize immediate Character-legible Performance disclosure to every current roster Character, or does the fixture contract require a narrower interpretation?
2. Is one immediate Performance exactly the correct E0 window, or did earlier E0 preparation freeze a larger bounded recent-context window that must be recovered from source before deciding?
3. Should render-v2 use `<silence>`, another protocol marker, or a structurally empty framed body for empty VisibleText?
4. Should `ContextRecentPerformance` expose SourceCharacterId publicly, or is display-name rendering plus internal identity sufficient?
5. Does v3 require a new public `RecentPerformances` property on `ContextPacket`, or should it remain an internal semantic collection exposed only through rendering/trace? Current preference: public immutable semantic collection because Structured Context is already a public inspectable contract, but this needs surface review.
6. Can exact prior Candidate structural validation reuse an existing helper without creating cross-authority coupling or duplicated policy?
7. Does the shared lower opportunity hash primitive belong in `ProductionStateCanonicalizer`, or should a separate internal `ProductionTransitionCanonicalizer` avoid overloading genesis projection responsibilities?
8. Is CausalCommit referencing the frozen Director strategy token dependency-safe, or should the recent-source proof receive a lower immutable transition descriptor created without a reverse dependency?
9. Can `E0TakeStateBinding` safely preserve evolved v2 acceptance indefinitely while new live orchestration requires v3, or does that leave a future accidental history-omission path too easy?
10. Should existing one-argument `E0ProductionContextContinuity.Compose` explicitly reject non-genesis after Patch 0015, or would that be an unnecessary breaking change to Patch 0014's frozen API?
11. Does current Production retain enough exact information to reject an older still-effective prior commit after several future turns using only current projection + commit recomputation + opportunity hash, or will a future multi-turn state allow a false immediate-predecessor proof? This must be proven before freeze.
12. Does the absence of durable event history make Patch 0015 necessarily one-turn-only for now, and if so should the blueprint state that explicitly rather than implying general multi-turn continuity?
13. Should the prior commit source Character equal the immediately previous opportunity-history Character, and can that fact be proved without introducing `CausalCommit -> Opportunity` dependency?
14. Does the Context v3 semantic recent Performance need a stable source order field, or is exactly-one sufficient and future multi-item history necessarily a new schema version?
15. Does enabling non-empty `RecentPerformanceText` require changes to Integrity disclosure checks before provider execution, or is that safely deferred because no provider is called in this patch?

## 52. Current recommendation

The source-supported next patch is **Recent Performance Context Continuity**, not generic Context Consumption and not provider-attempt provenance.

The direction appears architecturally necessary because Full Ensemble E0 cannot test social causality if the next Performer does not receive the immediately preceding accepted Character-legible Performance.

However Proposal 0.2 remains exploratory. No implementation, approval evidence, handoff, `CURRENT_STATE.md` change, or promotion action is permitted until the unresolved items above are resolved and one complete recursive pass finds no material correction or worthwhile improvement.
