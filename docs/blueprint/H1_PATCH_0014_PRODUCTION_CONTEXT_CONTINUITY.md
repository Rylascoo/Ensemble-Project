# H1 Patch 0014 — E0 Production Context Continuity

Status: blueprint proposal 0.3 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
Parent repository checkpoint: `main` at `e06668a2307433bf99b0501dc38a701db392c633`
Parent promoted implementation: H1 Patch 0013 squash merge `15b85a25fa7969d6db69030fa712eea329471e6b`
Parent full-Core-test authority: H1 Patch 0013 at `a3fae23dc4df302e834b031ecfc848a3bb2d37fc`
Parent native Core/Harness build + fixture authority: H1 Patch 0013 at `382e11f9fbe6774806152fad75b6a23cc8733187`
Branch: `h1-patch-0014-production-context-continuity-blueprint`

## 1. Purpose

Patch 0014 defines the smallest deterministic continuity boundary needed after H1 Patch 0013 so a newly established Current Opportunity can receive a Character-safe ContextPacket derived from the exact current ProductionState rather than from the immutable genesis fixture.

The completed H1 deterministic spine reaches:

```text
source ProductionState with Current Opportunity
    -> Access / Context / Performer / Integrity / Interpretation / State Authority
        -> immutable Accepted Take
            -> Patch 0012 atomic causal commit
                -> no-opportunity postcommit ProductionState
                    -> Patch 0013 postcommit Director recomputation
                        -> effective-opportunity ProductionState
```

The next missing boundary is:

```text
Patch 0013 effective-opportunity ProductionState
    -> exact pre-pipeline ProductionStateCheckpoint
        -> Production-backed deterministic Access Control
            -> causally proven immediate accepted-history disclosure
                -> Production-bound deterministic Context Composer
                    -> exact next-turn ContextPacket
```

Patch 0014 closes only that boundary. It does not invoke a provider, generate a second Performance, orchestrate a complete Scene loop, persist causal history, or claim full multi-turn replay from genesis.

## 2. Recovered frozen authority

Patch 0014 preserves these approved laws:

- ProductionState is authoritative current projection after genesis;
- ValidatedFixture remains immutable genesis input and is never evolved in place;
- deterministic Access Control precedes Context composition;
- prohibited information is removed before any relevance/composition stage can receive it;
- Character-facing projections carry no hidden provenance or creator-only authority metadata;
- objective truth, observation, claim, belief, memory, knowledge, and recent Performance remain distinct authority categories;
- accepted Performance history and semantic Production state are distinct authorities;
- recent fictional Performance is untrusted creative content separate from trusted structured state/system authority;
- Patch 0005 reserved `recentPerformances` for later accepted-history authority;
- ProductionStateCheckpoint is captured before a Performer-source Access/Context pipeline;
- StateHash is the history-sensitive identity of the exact Production source state;
- Patch 0013 establishes Current Opportunity before next Access/Context/Performer work;
- next Context must not silently rebase onto another Production state;
- deterministic authority, not a model, owns disclosure and source association;
- exact empty Candidate VisibleText represents silence and Ensemble invents no narration for it.

## 3. Why Patch 0014 is next

Current Access/Context still cannot:

1. consume evolved ProductionState rather than only ValidatedFixture;
2. carry exact source StateHash through Access/Context;
3. preserve CharacterClaim as a distinct Character-facing category;
4. populate accepted-history `recentPerformances`.

Frozen ordering is:

```text
Patch 0012 causal commit
    -> no-opportunity state
Patch 0013 opportunity transition
    -> effective next-opportunity state
Patch 0014 Production -> Access -> Context continuity
    -> legal next-turn source ContextPacket
```

A full Scene-loop orchestrator before this bridge would place disclosure/state-association/history authority in orchestration code, which is the wrong layer.

## 4. Scope boundary

Patch 0014 defines only:

- Production-backed Character Access evaluation;
- lifecycle-aware projection of active Production records;
- CharacterClaim disclosure semantics;
- source StateHash identity on Production-backed Access/Context;
- Production-bound Context v2 while preserving Context v1 bytes;
- one closed accepted recent-Performance item type;
- E0 immediate-recent-Performance disclosure from the immediately preceding Accepted causal commit;
- a higher Continuity bridge proving Patch0012 -> Patch0013 -> current checkpoint adjacency;
- v2 source-StateHash association in E0TakeStateBinding;
- safe historical v1 binding compatibility only after exact genesis and semantic source-equivalence proof;
- strict v1/v2 anti-downgrade/hybrid-shape rejection;
- fixed independent v2 canonical/hash oracles and fail-closed regression tests.

Patch 0014 does not implement complete Scene-loop orchestration, provider/model invocation, retry/cancellation/streaming/spend policy, full replay, persistence, arbitrary history retrieval, semantic search, more than one immediate recent Performance, Observation/World Resolver, broad spatial/hearing/channel rules, perspective UX, branch/canon/retcon/rehearsal, final Production/Studio ontology, WinUI, Windows AI/NPU, MSIX, WACK, or Store certification.

## 5. Dependency direction

Lower deterministic dependencies remain:

```text
Domain / Fixture / internal Provenance
    -> Production
        -> Access
            -> Context
                -> Performer / Integrity / Interpreter / State Authority / Take
                    -> CausalCommit
                        -> Opportunity
```

Access must not depend on CausalCommit, Opportunity, or Continuity.

`Access -> CausalCommit` is rejected because CausalCommit already consumes Context and would create `Access -> CausalCommit -> Context -> Access`.

Patch 0014 adds one higher integration layer:

```text
Access + Context + CausalCommit + Opportunity
    -> Continuity
```

No lower layer depends on Continuity.

CausalCommit may additionally depend directly on Access for the narrow legacy-v1 source-equivalence proof described below. That direction is legal because Access is lower than CausalCommit and creates no cycle.

## 6. Public Continuity surface

Namespace:

```text
Ensemble.E0.Core.Continuity
```

Public types:

```text
public static class E0ProductionContextContinuity
public sealed class E0ContextContinuityException : Exception
```

Methods:

```text
public static ContextCompositionEvaluation ComposeGenesis(
    ProductionStateCheckpoint sourceCheckpoint)

public static ContextCompositionEvaluation ComposeNextTurn(
    ProductionStateCheckpoint sourceCheckpoint,
    E0CausalCommit sourceCommit,
    E0OpportunityTransitionResult opportunityResult)
```

No mutable session object, generic history input, or public recent-Performance injection overload exists.

## 7. Checkpoint-first law

Correct flow remains:

```text
var checkpoint = ProductionStateCheckpoint.Capture(currentState);
var context = E0ProductionContextContinuity.Compose...(
    checkpoint,
    ...causal inputs...);
```

Checkpoint capture stays O(1), retains the exact immutable source-state reference internally, exposes exact StateHash/SceneId/CurrentOpportunity, cannot rebind, and performs no full-state rehash.

Normal v2 composition reuses the existing StateHash.

## 8. Production-backed Access overload

Historical API remains:

```text
CharacterBoundedAccessControl.Evaluate(
    ValidatedFixture fixture,
    CharacterId subjectCharacterId)
```

Patch 0014 adds:

```text
CharacterBoundedAccessControl.Evaluate(
    ProductionState sourceState,
    CharacterId subjectCharacterId)
```

Production Access is deterministic, synchronous, side-effect free, and performs no Context/CausalCommit/Opportunity/Continuity/provider/model/clock/random/network/GPU/NPU work.

It requires exact supported Production contract:

```text
ensemble.e0.production-state.v1
```

## 9. CharacterAccessProjection evolution

Patch 0014 adds:

```text
SourceStateHash : StateHash?
Claims : ImmutableArray<PermittedRecord>
```

Exact shape:

```text
fixture-backed historical projection:
    SourceStateHash = null
    Claims = []

Production-backed projection:
    SourceStateHash = sourceState.StateHash
    Claims = active permitted subject-owned CharacterClaim records
```

No lifecycle/protection/provenance object or raw ProductionRecord is exposed.

## 10. Production record projection law

Production Access evaluates every retained record for an audit decision. Only Active permitted records enter Character-facing content.

Active global domains:

```text
HistoricalTruth          -> Deny / ProductionAuthorityExcluded
UnresolvedProposition    -> Deny / ProductionAuthorityExcluded
WorldState               -> Deny / ProductionAuthorityExcluded
SceneState               -> Permit / SharedSceneState
Pressure                 -> Permit / PublicPressure
```

Active subject-owned domains:

```text
CharacterConstitution
CharacterDisposition
CharacterCircumstance
CharacterObservation
CharacterKnowledge
CharacterBelief
CharacterSuspicion
CharacterMemory
CharacterGoal
CharacterClaim
    -> Permit / OwnedBySubject
```

Those domains owned by another Character are `Deny / OwnedByOtherCharacterExcluded`.

Relationships:

```text
active Relationship subject == Access subject
    -> Permit / OwnedBySubject
otherwise
    -> Deny / OwnedByOtherCharacterExcluded
```

Any inactive record is `Deny / InactiveRecordExcluded`, and Patch 0014 adds that AccessReason. Lifecycle exclusion takes precedence because inactive semantic state is not current effective state.

## 11. Production Access structural invariants

Production Access fails closed unless:

- state is non-null and exact supported Production contract;
- StateHash/SceneId are initialized;
- roster is exactly three unique initialized E0 Characters in ordinal canonical order;
- subject resolves exactly once in Production Characters and roster;
- RecordIds are unique;
- every record identity/text is valid and canonical;
- runtime record subtype agrees with domain;
- Character record subjects are roster members;
- Relationship subject/target are distinct valid roster Characters;
- lifecycle/protection enums are defined;
- no unsupported domain exists.

Inactive retained records are structurally validated too.

## 12. CharacterClaim disclosure law

A Production CharacterClaim is an accepted interpreted proposition attributable to that source Character. It is not objective truth, knowledge, belief, memory, or another Character's observation.

Only the Access subject's own active CharacterClaim records enter that subject's `Claims` category.

Other Characters' durable claims are not automatically inserted into trusted state. Longer-lived knowledge/memory of another Character's claim requires separately committed semantic state representing that knowledge/memory.

The immediate prior visible Performance is handled separately as recent creative history.

## 13. Least-authority claim rationale

The rule prevents:

```text
Production remembers Character A claimed P
```

from silently becoming:

```text
Character B receives P as trusted state
```

without observation/knowledge/memory authority.

## 14. Context v1 remains byte-frozen

Existing constants remain exactly:

```text
ensemble.e0.context.v1
ensemble.e0.context.full-authorized.v1
ensemble.e0.context.render.v1
```

Existing structured/rendered bytes, byte lengths, hashes, ContextPacketId, and fixed Patch0005 oracles remain unchanged.

Existing v1 structured bytes still contain exactly `"recentPerformances":[]` and no sourceStateHash/claims fields.

## 15. Production-bound Context v2

New constants:

```text
E0ContextContracts.ProductionBoundSchemaVersion
= "ensemble.e0.context.v2"

E0ContextContracts.ProductionBoundCompositionContract
= "ensemble.e0.context.production-bound.v1"

E0ContextContracts.ProductionBoundRenderingContract
= "ensemble.e0.context.render.v2"
```

A new schema is required because sourceStateHash and claims change semantic bytes.

## 16. Shared ContextPacket model

The existing closed ContextPacket gains:

```text
SourceStateHash : StateHash?
Claims : ImmutableArray<ContextRecord>
RecentPerformances : ImmutableArray<ContextRecentPerformance>
```

New closed item:

```text
public sealed class ContextRecentPerformance
- TakeId
- SubjectCharacterId
- VisibleText
```

No public constructor/setter. No provider/model identity, raw response, Candidate control, Director trace, Integrity/Interpreter/Authority trace, Production provenance, credentials, or secrets.

## 17. Closed version/shape invariants

Hybrid shapes are invalid.

v1 requires exactly:

```text
SchemaVersion == ensemble.e0.context.v1
CompositionContract == ensemble.e0.context.full-authorized.v1
Rendered.RenderingContract == ensemble.e0.context.render.v1
SourceStateHash == null
Claims.Length == 0
RecentPerformances.Length == 0
```

Historical public v1 Compose also requires projection `SourceStateHash == null` and `Claims.Length == 0`.

v2 requires exactly:

```text
SchemaVersion == ensemble.e0.context.v2
CompositionContract == ensemble.e0.context.production-bound.v1
Rendered.RenderingContract == ensemble.e0.context.render.v2
SourceStateHash.HasValue with initialized value
projection.SourceStateHash == packet.SourceStateHash
Claims initialized
RecentPerformances.Length == 0 for ComposeGenesis
RecentPerformances.Length == 1 for ComposeNextTurn
```

Unsupported contract combinations fail closed.

## 18. Public canonicalizer dispatch law

`ContextPacketCanonicalizer.SerializeStructured(ContextPacket)` validates full version/shape before dispatch:

```text
valid v1 -> exact historical bytes
valid v2 -> exact production-bound bytes
anything else -> fail closed
```

It never ignores populated Claims/RecentPerformances, infers a version from nullable fields, serializes v2 fields under v1 tokens, or serializes v1 bytes under v2 tokens.

`SerializeRendered` likewise accepts only supported rendering contracts and preserves historical v1 bytes exactly.

## 19. Recent Performance item law

Recent Character-facing history includes only:

```text
TakeId
SubjectCharacterId
VisibleText
```

Addressed/nominated Candidate control remains routing authority and is not automatically diegetic knowledge.

## 20. Exact v2 structured canonical order

Root order:

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
claims
relationships
recentPerformances
```

Exact recent item order:

```json
{"takeId":"...","subjectCharacterId":"...","visibleText":"..."}
```

Set/projection arrays remain ordinal. `recentPerformances` is chronological, unsorted, and Patch0014 allows zero or one item only.

## 21. v2 packet identity

```text
StructuredContextHash = SHA256(canonical structured v2 bytes)
ContextPacketId = "CTX:" + StructuredContextHash
```

sourceStateHash is part of canonical bytes, so otherwise identical context from different causal Production states has different v2 identity.

## 22. Production-bound trusted-state rendering

The v2 trusted renderer preserves existing category order and inserts exactly one distinct section after `[WHAT YOU REMEMBER]`:

```text
[WHAT YOU HAVE CLAIMED]
```

Claim text never renders as Knowledge, World State, Observation, Belief, or another upgraded category.

## 23. RecentPerformanceText and silence

For v2 genesis:

```text
RecentPerformanceText = ""
```

For a non-empty immediately preceding Accepted VisibleText:

```text
[WHAT JUST HAPPENED]
<source display name>:
<exact Accepted VisibleText>
```

For an Accepted silent Performance where `VisibleText == ""`:

```text
RecentPerformanceText = ""
```

The structured `RecentPerformances` array still contains the exact one Accepted item, including TakeId and empty VisibleText, so causal identity/history remains distinct. Rendering invents no narration, marker, placeholder, or dangling speaker label for silence.

No recent Performance text enters TrustedStateText. OpportunityText remains exactly `You have the current opportunity to act.`

## 24. Trace evolution

ContextCompositionTrace adds:

```text
SourceStateHash : StateHash?
IncludedRecentTakeIds : ImmutableArray<TakeId>
```

Exact law:

```text
v1:
    SourceStateHash = null
    IncludedRecentTakeIds = []

v2 genesis:
    SourceStateHash = checkpoint.StateHash
    IncludedRecentTakeIds = []

v2 next turn:
    SourceStateHash = checkpoint.StateHash
    IncludedRecentTakeIds = [sourceCommit.Take.TakeId]
```

Claims are included in IncludedRecordIds.

## 25. Composer API / anti-forgery law

Historical public composer remains the sole public composer:

```text
DeterministicContextComposer.Compose(
    CharacterAccessProjection projection,
    CharacterId currentOpportunityCharacterId)
```

It is v1-only and rejects v2-bearing projections.

Patch0014 adds only an internal production-bound composition path accepting the Production projection, current opportunity, and closed recent items. No public API accepts arbitrary recent Performance input.

## 26. Genesis continuity

`ComposeGenesis(checkpoint)` succeeds only when:

1. checkpoint/state identities are initialized;
2. source Production contract is supported;
3. Current Opportunity resolves exactly once in roster;
4. inherited genesis-hash recomputation over checkpoint source projection equals checkpoint.StateHash;
5. Production Access succeeds for checkpoint opportunity;
6. Access SourceStateHash equals checkpoint.StateHash;
7. v2 composition succeeds with zero recent items.

This provides a state-bound opening Context without deleting the historical fixture/v1 surface.

## 27. Next-turn continuity adjacency

`ComposeNextTurn(...)` proves:

```text
sourceCommit.ResultStateHash
    == opportunityResult.Event.ParentStateHash

opportunityResult.Event.ResultStateHash
    == sourceCheckpoint.StateHash

opportunityResult.State.StateHash
    == sourceCheckpoint.StateHash

opportunityResult.Event.SelectedCharacterId
    == sourceCheckpoint.CurrentOpportunityCharacterId

opportunityResult.History.LastOpportunityStateHash
    == sourceCheckpoint.StateHash

opportunityResult.History.SceneId
    == sourceCheckpoint.SceneId

opportunityResult.History.CharacterIds[^1]
    == sourceCheckpoint.CurrentOpportunityCharacterId

opportunityResult.History.CharacterIds[^2]
    == sourceCommit.Take.Performance.SubjectCharacterId
```

History must contain at least two entries.

It additionally proves supported commit/Take/opportunity contracts, Accepted source Take, effective source CommitId/TakeId in current caches, source Take/current Scene match, source Character in roster, exact least-intervention v1 strategy, opportunity result State Scene/roster/current-opportunity agreement with checkpoint, and initialized identities.

The penultimate-Character proof explicitly binds the disclosed recent Performance to the effective opportunity immediately preceding the newly established one.

## 28. Immediate recent-Performance disclosure

Under the frozen E0 exactly-one co-present trio Scene:

```text
The exact visible Performance from the immediately preceding Accepted source Take
is permitted as recent Performance to the newly selected Current Opportunity Character.
```

This is not a general Observation law. Future Observation/World Resolver authority may replace it outside E0.

## 29. Recent source authority

Recent history is derived only from `sourceCommit.Take` after full adjacency validation. Disposition must be Accepted.

Rejected, Alternate, failed, cancelled, malformed, uncommitted, merely generated, or caller-supplied Candidate output cannot enter recent history.

## 30. State and creative history remain separate

```text
ProductionState -> current trusted semantic projection
E0CausalCommit  -> accepted creative Performance/consequence history
Continuity      -> proves immediate adjacency and strips one recent item
Context v2      -> keeps trusted state and recent creative history separate
```

No raw Performance text is copied into Production and no new Production StateHash kind exists.

## 31. E0TakeStateBinding v2 law

For v2 source Context, binding additionally requires exact:

```text
sourceContext.SourceStateHash.HasValue
sourceContext.SourceStateHash.Value == sourceCheckpoint.StateHash
sourceContext.SchemaVersion == ensemble.e0.context.v2
sourceContext.CompositionContract == ensemble.e0.context.production-bound.v1
sourceContext.Rendered.RenderingContract == ensemble.e0.context.render.v2
```

This is additive to existing Scene/subject/opportunity/roster/StateAuthority-snapshot checks.

Closed construction plus internal-only v2 composition makes the StateHash a trusted derivation association in normal Core flow.

## 32. Legacy v1 compatibility requires source-equivalence proof

A v1 Context carries no StateHash. Exact-genesis proof alone is insufficient because a caller can legally obtain a v1 Context from another ValidatedFixture.

Therefore E0TakeStateBinding accepts v1 only after both:

### A. exact genesis proof

Recompute the inherited genesis StateHash envelope over the checkpoint source Production projection and require exact checkpoint.StateHash equality.

### B. exact Production-backed disclosure equivalence

Freshly evaluate Production Access for the checkpoint Current Opportunity and prove the supplied v1 Context's disclosed semantic content equals that exact Access projection for every historical v1 category:

```text
SceneId
SubjectCharacterId
OpportunityCharacterId
Roster CharacterIds + display names
SceneState
Pressures
Constitution
Disposition
Circumstance
Observations
Knowledge
Beliefs
Suspicions
Memories
Goals
Relationships
```

Additionally require:

```text
fresh Production Access SourceStateHash == checkpoint.StateHash
fresh Production Access Claims == []
source v1 Context Claims == []
source v1 Context RecentPerformances == []
source v1 structured bytes/hash/ContextPacketId self-consistent
source v1 rendered bytes/hash self-consistent
```

Only then is historical v1 binding permitted.

This is an intentionally bounded O(n) compatibility path. Normal v2 binding is O(1) StateHash association and does not repeat Access projection comparison.

CausalCommit may call lower-layer Production Access for this compatibility proof without creating a dependency cycle.

Exact rule:

```text
exact genesis + semantically equivalent exact v1 Context -> permitted
genesis + foreign/different v1 Context -> reject
evolved source + any v1 Context -> reject
evolved source + exact v2 state-bound Context -> required
```

No automatic downgrade fallback occurs.

## 33. No new Production transition

Patch0014 adds no Production mutation helper, StateHash envelope kind, causal event, event ID, or opportunity transition. Patch0012 and Patch0013 fixed StateHash oracles remain unchanged.

## 34. Determinism

Identical authoritative inputs produce byte-identical output across repeats/cultures. No output depends on clock/date, randomness, process/machine identity, thread scheduling, dictionary insertion order, filesystem/network/provider/GPU/NPU state, or locale-sensitive ordering.

## 35. Failure atomicity

Any source, Access, disclosure, canonicalization, version-shape, equivalence, or composition failure returns no ContextPacket and changes no Production/history input. No retry, alternate source, guessed repair, stale packet, or v1 downgrade is permitted.

Public Continuity failures are sanitized `E0ContextContinuityException`. Direct lower APIs retain their existing exception domains.

## 36. Security / prompt-authority law

Patch0014 preserves:

```text
system/application authority
!= trusted structured fictional state
!= accepted recent fictional Performance
!= future user/imported creative content
```

Denied Production authority records, other Characters' private state, inactive records, provenance/protection/lifecycle metadata, credentials, and provider metadata never enter Character-facing content. Recent Performance never enters TrustedStateText. CharacterClaim remains explicitly a claim. Hybrid packets cannot canonicalize by dropping fields. Silent Performance causes no invented narration.

## 37. ARM64 / memory / battery implications

Patch0014 is bounded deterministic CPU authority work. NPU offload is inappropriate for filtering/validation/ordinal ordering/hash association.

- checkpoint capture remains O(1);
- normal v2 source binding is O(1) StateHash equality;
- Production Access scans bounded retained records once;
- recent history is one item maximum;
- no whole-session history copy;
- no idle/background/network/provider/GPU/NPU work;
- legacy v1 equivalence is O(n) only on exact genesis compatibility path.

## 38. Genesis Access equivalence

For exact genesis ProductionState, Production Access is semantically equivalent to fixture Access for roster, SceneState, Pressure, Constitution, Disposition, Circumstance, Observation, Knowledge, Belief, Suspicion, Memory, Goal, and Relationship.

Intentional additions are SourceStateHash, empty Claims, and lifecycle-aware audit decisions.

## 39. v1 regression law

Patch0014 explicitly preserves Patch0005 exact v1 structured/rendered bytes and lengths, hashes, ContextPacketId, empty recentPerformances, and public Compose signature.

The old test asserting no Context Performance type is intentionally superseded only by the approved closed ContextRecentPerformance type; all no-provenance/provider-neutral laws remain.

## 40. Independent v2 reference oracles

Implementation must derive fixed v2 oracles independently from production canonicalizer code for:

```text
Missing Raft genesis -> checkpoint -> ComposeGenesis
```

and:

```text
Missing Raft genesis
 -> exact Patch0012 Accepted causal commit
 -> exact Patch0013 opportunity transition
 -> checkpoint
 -> ComposeNextTurn
```

Independent derivation must first reproduce inherited fixture/v1 Context/Production/Patch0013 fixed hashes.

## 41. Required Production Access tests

At minimum:

1. genesis Production Access equals fixture Access for inherited categories;
2. SourceStateHash exact;
3. unsupported Production contract fails;
4. active committed state changes follow policy;
5. inactive records absent and denied;
6. subject claim only in Claims;
7. other claim denied;
8. claim absent from upgraded epistemic/global categories;
9. relationship ownership preserved;
10. HistoricalTruth/World/Unresolved denied;
11. lifecycle/protection/provenance stripped;
12. malformed roster/domain/subtype/relationship/record fails;
13. inactive malformed records fail;
14. ordinal deterministic ordering;
15. no Access dependency on CausalCommit/Opportunity/Continuity.

## 42. Required Context version/shape tests

1. v1 projection with StateHash fails;
2. v1 projection with Claims fails;
3. v1 packet with v2-only fields fails serialization;
4. v2 requires initialized SourceStateHash;
5. v2 requires exact schema/composition/rendering triple;
6. hybrid triples fail;
7. canonicalizer never drops Claims/RecentPerformances;
8. unsupported schema fails;
9. v1 bytes/hashes exact;
10. v2 property order exact;
11. recent item property order exact;
12. repeat/culture determinism.

## 43. Required Context v2 behavioral tests

1. genesis v2 SourceStateHash exact;
2. genesis recent array empty;
3. claims structurally separate;
4. claim heading/category exact;
5. genesis RecentPerformanceText empty;
6. non-silent next Performance renders exact header/name/text;
7. silent Accepted Performance keeps one structured recent item but exact empty RecentPerformanceText;
8. ContextPacketId = CTX + structured hash;
9. duplicate IDs fail;
10. recent Character roster-bound;
11. recent TakeId initialized;
12. recent text absent from TrustedStateText;
13. trace state/recent identities exact.

## 44. Required next-turn Continuity tests

1. valid Patch0012 -> Patch0013 -> checkpoint chain yields one recent item;
2. recent TakeId/Character/VisibleText exact;
3. subject/opportunity = checkpoint opportunity;
4. SourceStateHash = checkpoint hash;
5. foreign/stale commit fails;
6. foreign/stale opportunity result fails;
7. commit-result/opportunity-parent mismatch fails;
8. opportunity-result/checkpoint mismatch fails;
9. selected/current-opportunity mismatch fails;
10. history Scene mismatch fails;
11. history last hash mismatch fails;
12. history last Character mismatch fails;
13. history penultimate source Character mismatch fails;
14. history length < 2 fails;
15. effective CommitId/TakeId absence fails;
16. non-Accepted Take fails;
17. Scene/roster/source Character mismatch fails;
18. unsupported strategy fails;
19. malformed identities fail;
20. failure leaves inputs unchanged.

## 45. Required Take-binding tests

1. exact v2 StateHash binds;
2. foreign/uninitialized v2 StateHash fails;
3. hybrid v2 contract fails;
4. evolved checkpoint + v1 fails;
5. exact genesis + exact Production-equivalent v1 succeeds;
6. exact genesis + semantically different v1 fixture Context fails;
7. v1 roster/display-name mismatch fails;
8. v1 record text/category mismatch fails;
9. v1 structured/hash self-inconsistency fails;
10. v1 rendered/hash self-inconsistency fails;
11. existing Scene/subject/opportunity/roster/StateAuthority checks remain;
12. v2 StateHash is additive authority, not replacement.

## 46. Required structural/reflection tests

- Continuity namespace exports only approved two types;
- only ComposeGenesis/ComposeNextTurn public;
- no mutable Continuity/session/history API;
- ContextRecentPerformance no public ctor/setter;
- Production Access overload exact;
- Access projection additions exact;
- ContextPacket/Trace additions immutable;
- v1/v2 constants exact;
- historical public Compose remains sole public composer;
- production composer internal;
- Access has no CausalCommit/Opportunity/Continuity reference;
- Production has no Access/Context/Continuity reference;
- CausalCommit's new Access dependency is limited to legacy v1 source-equivalence validation;
- no Windows/network/random/time/provider/GPU/NPU public dependency.

## 47. Canonical preservation

Retain fixed Patch0003 fixture hash, Patch0005 v1 Context hashes, Patch0012 genesis/causal-commit StateHashes, and Patch0013 opportunity-transition StateHash. v2 identity is additive.

## 48. Simplicity guard

No event-store/session aggregate/repository locator/generic context-source hierarchy/background cache/vector search/model-assisted Access/observer framework/provider routing/unnecessary async/second Production representation.

## 49. Explicit nonclaims

Patch0014 does not establish a complete Scene loop, full multi-turn replay, Observation semantics beyond frozen E0 co-presence, or any provider/model/Windows AI/NPU/package/Store behavior.

## 50. Expected implementation surface

Likely modified:

```text
src/Ensemble.E0.Core/Access/CharacterAccessModels.cs
src/Ensemble.E0.Core/Access/CharacterBoundedAccessControl.cs
src/Ensemble.E0.Core/Context/ContextModels.cs
src/Ensemble.E0.Core/Context/ContextPacketCanonicalizer.cs
src/Ensemble.E0.Core/Context/DeterministicContextComposer.cs
src/Ensemble.E0.Core/CausalCommit/CausalCommitModels.cs
```

Likely added:

```text
src/Ensemble.E0.Core/Continuity/E0ProductionContextContinuity.cs
```

Plus focused tests/evidence. ProductionState projection/canonicalizer and Opportunity implementation should remain unchanged.

## 51. Proposal evolution

### 0.2 corrections

- explicit OpportunityHistory Scene/penultimate source-Character proof;
- closed v1/v2 version-shape invariants and canonicalizer downgrade rejection;
- exact Production contract and retained-record validation.

### 0.3 corrections

- legacy v1 binding now requires full Production-backed semantic disclosure equivalence in addition to exact-genesis proof;
- CausalCommit may depend on lower Access only for that compatibility proof;
- exact silent Accepted Performance remains present in structured recent history but renders as empty RecentPerformanceText with no invented narration.

These are material authority corrections, so the recursive audit restarts from correctness.

## 52. Recursive audit order

```text
correctness
-> consistency
-> authority
-> disclosure/privacy
-> dependency direction
-> canonical/version compatibility
-> scope
-> tests
-> simplicity
-> hygiene
-> ARM64/battery suitability
-> project vision
-> evidence
```

Any material correction restarts from correctness.

## 53. Approval gate

Implementation must not begin until one complete recursive pass finds zero material corrections or worthwhile improvements and the user explicitly approves.

Approval freezes Patch0014 purpose/scope, Production Access law, CharacterClaim disclosure, immediate recent Performance rule, Continuity placement, v1 preservation/v2 versioning and shape rules, SourceStateHash propagation, safe v1 compatibility proof, exact v2 canonical/rendering behavior including silence, and tests/non-goals.
