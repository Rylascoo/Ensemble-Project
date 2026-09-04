# H1 Patch 0014 — E0 Production Context Continuity

Status: blueprint proposal 0.2 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
Parent repository checkpoint: `main` at `e06668a2307433bf99b0501dc38a701db392c633`
Parent promoted implementation: H1 Patch 0013 squash merge `15b85a25fa7969d6db69030fa712eea329471e6b`
Parent full-Core-test authority: H1 Patch 0013 at `a3fae23dc4df302e834b031ecfc848a3bb2d37fc`
Parent native Core/Harness build + fixture authority: H1 Patch 0013 at `382e11f9fbe6774806152fad75b6a23cc8733187`
Branch: `h1-patch-0014-production-context-continuity-blueprint`

## 1. Purpose

Patch 0014 defines the smallest deterministic continuity boundary needed after H1 Patch 0013 so a newly established Current Opportunity can receive a Character-safe ContextPacket derived from the exact current ProductionState rather than from the immutable genesis fixture.

The completed H1 deterministic spine now reaches:

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

Patch 0014 closes only that boundary.

It does not invoke a provider, generate a second Performance, orchestrate a complete Scene loop, persist causal history, or claim full multi-turn replay from genesis.

## 2. Recovered frozen authority

Patch 0014 preserves the already-approved laws that:

- ProductionState is the authoritative current projection after genesis;
- ValidatedFixture remains immutable genesis input and is never mutated into evolved state;
- deterministic Access Control always precedes Context composition;
- prohibited information must be removed before any relevance/composition stage can receive it;
- Character-facing projections remain stripped of hidden provenance and creator-only authority metadata;
- objective truth, observation, claim, belief, memory, knowledge, and recent Performance remain distinct authority categories;
- accepted Performance history and semantic Production state are distinct authorities;
- recent fictional Performance is untrusted creative content and remains separate from trusted structured state/system authority;
- Patch 0005 reserved `recentPerformances` for later accepted-history authority;
- ProductionStateCheckpoint is captured before a Performer-source Access/Context pipeline begins;
- StateHash is the history-sensitive identity of the exact Production source state;
- Patch 0013 establishes Current Opportunity before any next Access/Context/Performer work begins;
- a next ContextPacket must not silently rebase onto a different Production state;
- deterministic authority, not a model, owns disclosure and association.

## 3. Why Patch 0014 is next

Patch 0013 explicitly deferred this boundary because current Access/Context still cannot:

1. consume evolved ProductionState rather than only ValidatedFixture;
2. carry exact source StateHash through Access/Context;
3. preserve CharacterClaim as a distinct Character-facing category;
4. populate the accepted-history `recentPerformances` layer.

The frozen order is therefore:

```text
Patch 0012 causal commit
    -> no-opportunity state
Patch 0013 opportunity transition
    -> effective next-opportunity state
Patch 0014 Production -> Access -> Context continuity
    -> legal next-turn source ContextPacket
```

A full Scene-loop orchestrator before this bridge would have to invent disclosure semantics, state association, or recent-history policy inside orchestration code. That would put constitutional authority in the wrong layer.

## 4. Scope boundary

Patch 0014 defines only:

- Production-backed Character Access evaluation;
- lifecycle-aware projection of active Production records;
- explicit CharacterClaim disclosure semantics;
- exact source StateHash identity on Production-backed Access/Context;
- a new Production-bound Context schema/contract while preserving Patch 0005 v1 bytes;
- one exact immutable accepted recent-Performance item type;
- deterministic E0 immediate-recent-Performance disclosure from the immediately preceding Accepted causal commit;
- a higher-level Continuity bridge proving Patch 0012 -> Patch 0013 -> current checkpoint adjacency;
- exact next-turn source-StateHash association in E0TakeStateBinding;
- exact genesis compatibility rules for the historical fixture/v1 path;
- strict v1/v2 anti-downgrade and hybrid-shape rejection;
- fixed canonical byte/hash tests for the new v2 format;
- fail-closed stale/foreign/malformed/downgraded source tests.

Patch 0014 does not implement:

- complete Scene-loop orchestration;
- provider/model invocation, retry, cancellation, streaming, budget, or spend policy;
- full multi-turn replay from genesis through an event sequence;
- durable event store/database/recovery;
- arbitrary history retrieval or semantic memory search;
- more than the immediate Accepted recent Performance;
- Observation / World Resolver;
- broader spatial/hearing/channel visibility semantics;
- audience/creator/Character perspective UX;
- branch/canon/retcon/rehearsal/Alternate promotion;
- final Production/Studio ontology;
- WinUI, Windows AI Foundry, NPU/QNN, MSIX, WACK, or Store certification.

## 5. Dependency direction

Patch 0012 explicitly permits a later Access integration patch to depend on Production without reversing Production's dependency direction.

Lower deterministic dependencies remain conceptually:

```text
Domain / Fixture / internal Provenance
    -> Production
        -> Access
            -> Context
                -> Performer / Integrity / Interpreter / State Authority / Take
                    -> CausalCommit
                        -> Opportunity
```

Patch 0014 must not make Access depend on CausalCommit, Opportunity, or Continuity.

An Access -> CausalCommit dependency is rejected because CausalCommit already consumes Context and would create:

```text
Access -> CausalCommit -> Context -> Access
```

Instead one higher integration namespace owns continuity composition:

```text
Access + Context + CausalCommit + Opportunity
    -> Continuity
```

No lower layer depends on Continuity.

## 6. Public Continuity surface

Proposed namespace:

```text
Ensemble.E0.Core.Continuity
```

Approved public surface is limited to:

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

There is no public mutable continuity/session object, generic arbitrary-history input, or overload accepting caller-created recent Performance data.

## 7. Checkpoint-first law

Patch 0012 freezes checkpoint capture before Access/Context/Performance. Patch 0014 preserves that ordering directly:

```text
var checkpoint = ProductionStateCheckpoint.Capture(currentState);
var context = E0ProductionContextContinuity.Compose...(
    checkpoint,
    ...causal inputs...);
```

The checkpoint remains O(1), retains the exact immutable source-state reference internally, exposes exact StateHash/SceneId/CurrentOpportunity, cannot be rebound, and performs no full-state rehash.

Patch 0014 reuses the already-computed StateHash rather than rehashing Production merely to bind Context.

## 8. Production-backed Access overload

Existing historical API remains:

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

The Production overload is deterministic, synchronous, side-effect free, and must not call Context, CausalCommit, Opportunity, Continuity, provider/model code, clock, randomness, network, GPU, or NPU work.

It fails closed unless `sourceState.ContractVersion` is exactly the inherited supported Production contract:

```text
ensemble.e0.production-state.v1
```

The Production Access policy is therefore explicitly bound to the supported Production representation rather than inferred from arbitrary in-memory objects.

## 9. CharacterAccessProjection evolution

The existing Character-safe projection remains the only trusted state-information input to Context Composer.

Patch 0014 adds:

```text
SourceStateHash : StateHash?
Claims : ImmutableArray<PermittedRecord>
```

Exact shape law:

```text
fixture/v1 projection:
    SourceStateHash = null
    Claims = []

Production-backed projection:
    SourceStateHash = exact sourceState.StateHash
    Claims = permitted active subject-owned CharacterClaim records
```

No ProductionRecord lifecycle/protection/provenance object or raw ProductionRecord instance is exposed.

## 10. Production record projection law

Production-backed Access evaluates every retained Production record for audit disposition, but only Active permitted records enter Character-facing content.

### Active global domains

```text
HistoricalTruth          -> Deny / ProductionAuthorityExcluded
UnresolvedProposition    -> Deny / ProductionAuthorityExcluded
WorldState               -> Deny / ProductionAuthorityExcluded
SceneState               -> Permit / SharedSceneState
Pressure                 -> Permit / PublicPressure
```

### Active Character-owned domains

For the Access subject:

```text
CharacterConstitution    -> Permit / OwnedBySubject
CharacterDisposition     -> Permit / OwnedBySubject
CharacterCircumstance    -> Permit / OwnedBySubject
CharacterObservation     -> Permit / OwnedBySubject
CharacterKnowledge       -> Permit / OwnedBySubject
CharacterBelief          -> Permit / OwnedBySubject
CharacterSuspicion       -> Permit / OwnedBySubject
CharacterMemory          -> Permit / OwnedBySubject
CharacterGoal            -> Permit / OwnedBySubject
CharacterClaim           -> Permit / OwnedBySubject
```

The same domains owned by another Character are:

```text
Deny / OwnedByOtherCharacterExcluded
```

### Relationships

```text
active Relationship with subject == Access subject
    -> Permit / OwnedBySubject

active Relationship with another subject
    -> Deny / OwnedByOtherCharacterExcluded
```

### Inactive records

Any inactive retained record is:

```text
Deny / InactiveRecordExcluded
```

Lifecycle exclusion takes precedence because an inactive semantic record is no longer current effective state.

Patch 0014 adds:

```text
AccessReason.InactiveRecordExcluded
```

## 11. Production Access structural invariants

The Production overload validates rather than trusting malformed in-memory state.

It fails closed unless:

- source state is non-null and exact supported Production contract;
- StateHash and SceneId are initialized;
- roster is the frozen E0 three unique initialized Characters in canonical ordinal order;
- subject resolves exactly once in Production Characters and exactly once in roster;
- retained RecordIds are unique;
- every record has initialized identity and canonical text;
- runtime record subtype agrees with ProductionRecordDomain;
- Character-owned record subjects belong to roster;
- Relationship subject/target are distinct valid roster Characters;
- lifecycle and protection enum values are defined;
- no unsupported domain is accepted.

Validation applies to inactive records too; lifecycle does not excuse malformed retained history.

## 12. CharacterClaim disclosure law

CharacterClaim remains a distinct epistemic category.

A Production CharacterClaim means that the Production has accepted an interpreted proposition attributable to that source Character. It does not make the proposition objective truth, knowledge, belief, memory, or another Character's observation.

Patch 0014 therefore permits only the Access subject's own active CharacterClaim records into that subject's `Claims` category.

Other Characters' durable claims are not automatically inserted into trusted state merely because Production remembers that they made them.

Longer-lived knowledge/memory of another Character's claim requires separately committed semantic state representing that knowledge/memory. Patch 0014 does not synthesize it.

The immediately preceding Accepted visible Performance is handled separately as recent creative history under the narrow E0 co-present rule below.

## 13. Least-authority rationale for claims

Automatically giving every Character every durable claim would collapse:

```text
Production remembers that Character A claimed P
```

into:

```text
Character B currently receives P as trusted state
```

without observation, knowledge, or memory authority.

The subject-owned claim rule preserves self-continuity while avoiding cross-Character omniscience.

## 14. Context v1 remains byte-frozen

Patch 0005 v1 remains historical executable authority.

Exact existing constants remain unchanged:

```text
E0ContextContracts.SchemaVersion
= "ensemble.e0.context.v1"

E0ContextContracts.CompositionContract
= "ensemble.e0.context.full-authorized.v1"

E0ContextContracts.RenderingContract
= "ensemble.e0.context.render.v1"
```

Existing v1 structured bytes, rendered bytes, ContextPacketId, StructuredContextHash, RenderedContextHash, and fixed reference oracles remain unchanged.

Existing v1 canonical root still contains exactly:

```text
"recentPerformances":[]
```

Patch 0014 does not retrofit sourceStateHash or claims into v1 canonical bytes.

## 15. Production-bound Context v2

Patch 0014 adds:

```text
E0ContextContracts.ProductionBoundSchemaVersion
= "ensemble.e0.context.v2"

E0ContextContracts.ProductionBoundCompositionContract
= "ensemble.e0.context.production-bound.v1"

E0ContextContracts.ProductionBoundRenderingContract
= "ensemble.e0.context.render.v2"
```

A new schema is required because sourceStateHash and claims add semantic fields not present in frozen v1 bytes.

## 16. Shared ContextPacket public model

Patch 0014 extends the existing closed `ContextPacket` model rather than introducing a parallel packet hierarchy.

New properties:

```text
SourceStateHash : StateHash?
Claims : ImmutableArray<ContextRecord>
RecentPerformances : ImmutableArray<ContextRecentPerformance>
```

New closed immutable item:

```text
public sealed class ContextRecentPerformance
- TakeId
- SubjectCharacterId
- VisibleText
```

`ContextRecentPerformance` has no public constructor or setter.

It contains no provider/model identity, raw response, Candidate control, Director trace/rule, Integrity/Interpreter/Authority trace, Production provenance, credential, or secret.

The shared model is safe only under the exact version/shape invariants in the next section.

## 17. Closed version/shape invariants

Patch 0014 forbids hybrid Context shapes.

### v1 packet/projection

A v1 composition/canonicalization path is valid only when:

```text
SchemaVersion == ensemble.e0.context.v1
CompositionContract == ensemble.e0.context.full-authorized.v1
Rendered.RenderingContract == ensemble.e0.context.render.v1
SourceStateHash == null
Claims.Length == 0
RecentPerformances.Length == 0
```

The public historical v1 composer also requires its `CharacterAccessProjection` to have:

```text
SourceStateHash == null
Claims.Length == 0
```

A projection with a state hash or claims may not be silently downgraded to v1.

### v2 packet/projection

A v2 production-bound path is valid only when:

```text
SchemaVersion == ensemble.e0.context.v2
CompositionContract == ensemble.e0.context.production-bound.v1
Rendered.RenderingContract == ensemble.e0.context.render.v2
SourceStateHash.HasValue
SourceStateHash.Value is initialized
projection.SourceStateHash == packet.SourceStateHash
Claims are initialized
RecentPerformances.Length is 0 for ComposeGenesis or exactly 1 for ComposeNextTurn
```

No unsupported combination of schema/composition/rendering contracts is serialized.

## 18. Public canonicalizer dispatch law

`ContextPacketCanonicalizer.SerializeStructured(ContextPacket)` remains public for historical callers.

It must validate the complete version/shape invariant before serializing and dispatch only to the exact supported canonical shape:

```text
valid v1 -> exact historical v1 bytes
valid v2 -> exact production-bound v2 bytes
hybrid/unsupported -> fail closed
```

It must never:

- infer v1 merely because SourceStateHash is null;
- ignore non-empty Claims;
- ignore populated RecentPerformances;
- serialize v2 fields under the v1 schema token;
- serialize v1 bytes under a v2 schema token.

`SerializeRendered(RenderedContext)` similarly accepts only the exact supported rendering contract shapes and preserves v1 bytes unchanged.

This closes a downgrade path where authority-bearing content could otherwise be silently omitted from canonical identity.

## 19. Recent Performance item law

Candidate addressed/nominated control remains deterministic routing input already causally bound through the Accepted Take and Patch 0013 opportunity transition.

It is not automatically diegetic Character knowledge.

Therefore a recent Performance item contains only:

```text
TakeId
SubjectCharacterId
VisibleText
```

The next Performer receives the exact accepted visible fictional action, not hidden routing machinery.

## 20. Exact v2 structured canonical order

Root property order is exactly:

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

Exact conceptual shape:

```json
{
  "schemaVersion":"ensemble.e0.context.v2",
  "compositionContract":"ensemble.e0.context.production-bound.v1",
  "sourceStateHash":"<64-lower-hex>",
  "sceneId":"...",
  "subjectCharacterId":"...",
  "opportunityCharacterId":"...",
  "roster":[{"characterId":"...","displayName":"..."}],
  "sceneState":[{"recordId":"...","text":"..."}],
  "pressures":[{"recordId":"...","text":"..."}],
  "constitution":[{"recordId":"...","text":"..."}],
  "disposition":[{"recordId":"...","text":"..."}],
  "circumstance":[{"recordId":"...","text":"..."}],
  "observations":[{"recordId":"...","text":"..."}],
  "knowledge":[{"recordId":"...","text":"..."}],
  "beliefs":[{"recordId":"...","text":"..."}],
  "suspicions":[{"recordId":"...","text":"..."}],
  "memories":[{"recordId":"...","text":"..."}],
  "goals":[{"recordId":"...","text":"..."}],
  "claims":[{"recordId":"...","text":"..."}],
  "relationships":[{"recordId":"...","targetCharacterId":"...","text":"..."}],
  "recentPerformances":[{"takeId":"...","subjectCharacterId":"...","visibleText":"..."}]
}
```

Set/projection arrays remain ordinally canonical. `recentPerformances` is chronological and is never sorted. Patch 0014 permits only zero or one item.

## 21. v2 identity

Production-bound packet identity remains:

```text
StructuredContextHash = SHA256(canonical structured v2 bytes)
ContextPacketId = "CTX:" + StructuredContextHash
```

Because sourceStateHash is inside canonical structured bytes, otherwise identical Character context from different causal Production states cannot silently share a v2 ContextPacketId.

No Production rehash is needed.

## 22. Production-bound rendering

The v1 renderer remains byte-frozen.

The v2 trusted-state renderer preserves existing sections and adds one semantically explicit claim section:

```text
[WHAT YOU HAVE CLAIMED]
```

Claim text must never render under Knowledge, World State, Observation, Belief, or another category that upgrades its authority.

The existing other trusted-state sections and order remain unchanged except for insertion of this distinct claim section after `[WHAT YOU REMEMBER]` and before `[WHO IS PRESENT]`.

## 23. RecentPerformanceText rendering

For v2 genesis:

```text
RecentPerformanceText = ""
```

For v2 next turn, exactly:

```text
[WHAT JUST HAPPENED]
<source display name>:
<exact Accepted VisibleText>
```

Recent Performance is never concatenated into TrustedStateText.

Accepted fictional Performance does not become system instruction authority merely because it is causally effective creative history.

OpportunityText remains exactly:

```text
You have the current opportunity to act.
```

## 24. Trace evolution

Patch 0014 extends ContextCompositionTrace with:

```text
SourceStateHash : StateHash?
IncludedRecentTakeIds : ImmutableArray<TakeId>
```

Exact law:

```text
fixture/v1:
    SourceStateHash = null
    IncludedRecentTakeIds = []

Production/v2 genesis:
    SourceStateHash = checkpoint.StateHash
    IncludedRecentTakeIds = []

Production/v2 next turn:
    SourceStateHash = checkpoint.StateHash
    IncludedRecentTakeIds = [ sourceCommit.Take.TakeId ]
```

Claims are included in existing IncludedRecordIds.

## 25. Composer API and anti-forgery law

Existing public historical API remains the sole public composer:

```text
DeterministicContextComposer.Compose(
    CharacterAccessProjection projection,
    CharacterId currentOpportunityCharacterId)
```

It remains v1-only and enforces the exact v1 projection shape.

Patch 0014 adds an internal production-bound path, conceptually:

```text
internal ComposeProductionBound(
    CharacterAccessProjection projection,
    CharacterId currentOpportunityCharacterId,
    ImmutableArray<ContextRecentPerformance> recentPerformances)
```

Only Continuity may supply causally derived recent items through normal Core flow.

No public API accepts arbitrary recent Performance input.

## 26. Genesis continuity path

`ComposeGenesis(sourceCheckpoint)` succeeds only for an exact genesis Production source.

It verifies:

1. source checkpoint exists and identities are initialized;
2. checkpoint source state contract is exact supported Production v1;
3. checkpoint Current Opportunity exists exactly once in roster;
4. recomputing the inherited exact genesis StateHash envelope over the checkpoint source Production projection equals checkpoint.StateHash;
5. Production Access for checkpoint Current Opportunity succeeds;
6. Access SourceStateHash equals checkpoint.StateHash;
7. v2 production-bound composition succeeds with no recent Performance.

This provides a state-bound opening-turn context without deleting or rewriting the historical fixture/v1 path.

## 27. Next-turn continuity path

`ComposeNextTurn(...)` accepts the exact current checkpoint, immediately preceding source causal commit, and immediately following Patch 0013 opportunity result.

It must prove:

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

The history must contain at least two entries before the penultimate check.

The penultimate-Character law makes immediate accepted-history disclosure explicit: the Performance being disclosed must be the Character whose effective opportunity immediately preceded the newly established one.

The bridge also proves:

- supported commit/Take/opportunity contracts;
- source commit contains an Accepted Take;
- source CommitId and TakeId remain effective in current Production caches;
- source Take Scene equals current Scene;
- source Performance Character belongs to current roster;
- opportunity strategy is exact least-intervention v1;
- opportunity result State Scene/roster/current opportunity agree with checkpoint state;
- all identities used by these comparisons are initialized.

Any mismatch fails before Access/Context output exists.

## 28. Immediate recent-Performance disclosure law

Patch 0014 adopts one narrow E0 rule:

```text
The exact visible Performance from the immediately preceding Accepted source Take
is permitted as recent Performance to the Character holding the newly established Current Opportunity.
```

This is not a general product-wide observation law.

It is justified only by the frozen E0 exactly-one co-present trio Scene. Future Observation/World Resolver contracts may replace this E0 rule for broader scenarios.

## 29. Recent Performance source authority

The one recent item is derived only from:

```text
sourceCommit.Take
```

after the complete adjacency proof succeeds.

Required source disposition is exactly Accepted.

Rejected, Alternate, failed, cancelled, malformed, uncommitted, merely generated, or caller-supplied Candidate output cannot enter recent Performance context.

## 30. State and creative history remain separate

Patch 0014 preserves:

```text
ProductionState
    -> trusted current semantic projection

E0CausalCommit
    -> accepted creative Performance/consequence history

Continuity
    -> proves immediate adjacency
    -> strips/permits one recent creative item

Context v2
    -> keeps trusted state and recent Performance in separate fields/layers
```

Raw accepted Performance text is not duplicated into Production records merely so Access can find it.

No ProductionState field or StateHash transition is added.

## 31. E0TakeStateBinding v2 source-hash law

Patch 0012 binding already receives:

```text
ProductionStateCheckpoint sourceCheckpoint
ContextPacket sourceContext
E0Take take
```

For v2 production-bound Context it additionally requires:

```text
sourceContext.SourceStateHash.HasValue
sourceContext.SourceStateHash.Value == sourceCheckpoint.StateHash
sourceContext.SchemaVersion == ensemble.e0.context.v2
sourceContext.CompositionContract == ensemble.e0.context.production-bound.v1
sourceContext.Rendered.RenderingContract == ensemble.e0.context.render.v2
```

This state-hash association is additive; it does not replace existing Scene/subject/opportunity/roster/StateAuthority-snapshot checks.

## 32. Legacy v1 downgrade guard

A v1 Context has no source StateHash and therefore cannot safely represent an evolved post-genesis Performer source.

E0TakeStateBinding may accept v1 only when the checkpoint source ProductionState is provably exact genesis by inherited genesis-hash recomputation.

Exact rule:

```text
exact genesis checkpoint + exact valid v1 Context
    -> historical compatibility permitted

evolved checkpoint + v1 Context
    -> reject

evolved checkpoint + exact v2 Context bound to checkpoint StateHash
    -> required
```

The v1 compatibility proof is intentionally O(n) only on the legacy genesis path. Normal Production-bound v2 binding uses O(1) StateHash equality and does not rehash Production.

No automatic v1 fallback occurs when v2 validation fails.

## 33. No new Production transition

Patch 0014 is information composition, not Production mutation.

It adds no:

- Production mutation helper;
- Production StateHash envelope kind;
- causal event;
- commit/event ID;
- opportunity transition.

Patch 0012 genesis/causalCommit StateHash oracles and Patch 0013 opportunityTransition oracle remain unchanged.

## 34. Determinism

Identical authoritative inputs must produce byte-identical output across repeats and ordinary supported cultures.

No output may depend on clock/date, randomness, process/machine identity, thread scheduling, dictionary insertion order, filesystem/network/provider state, GPU/NPU state, or locale-sensitive sort/case behavior.

All set-like canonical order remains ordinal.

## 35. Failure atomicity

Patch 0014 mutates no external state.

Any source, Access, disclosure, canonicalization, version-shape, or composition failure returns no ContextPacket and changes no Production/history input.

No retry, alternate source, guessed repair, stale packet, or evolved-state v1 downgrade is permitted.

Public Continuity failures are sanitized as `E0ContextContinuityException`.

Existing direct Access/Context APIs retain their existing exception domains.

## 36. Security and prompt-authority law

Patch 0014 preserves:

```text
system/application authority
!= trusted structured fictional state
!= accepted recent fictional Performance
!= future user/imported creative content
```

Specifically:

- denied Production authority records never enter the packet;
- other Characters' private state never enters the packet;
- inactive records never enter Character-facing content;
- provenance/protection/lifecycle metadata never enters Character-facing content;
- recent Performance never enters TrustedStateText;
- recent Performance is not system instruction authority;
- CharacterClaim remains labeled as claim rather than fact/knowledge;
- no credentials/provider metadata enter the packet;
- hybrid context shapes cannot canonicalize by dropping authority-bearing fields.

## 37. ARM64 / memory / battery implications

Patch 0014 remains bounded deterministic CPU authority work.

NPU offload is inappropriate for filtering, structural validation, ordinal ordering, and cryptographic identity association; accelerator dispatch would add complexity/energy cost without improving authority.

Per explicit context boundary:

- checkpoint capture remains O(1);
- current StateHash reuse is O(1);
- Production Access scans the bounded retained record set once;
- recent accepted history is bounded to one Performance;
- no whole-session history copy occurs;
- Context hashing/rendering is proportional only to permitted content;
- no background service/polling/network/provider/GPU/NPU work is added.

This preserves low idle battery impact on ARM64 Copilot+ PCs.

## 38. Genesis Production-Access equivalence

For exact genesis ProductionState derived from a ValidatedFixture, Production-backed Access is semantically equivalent to historical fixture Access for inherited categories:

- roster;
- SceneState;
- Pressure;
- Constitution;
- Disposition;
- Circumstance;
- Observation;
- Knowledge;
- Belief;
- Suspicion;
- Memory;
- Goal;
- Relationship.

Intentional additions are only SourceStateHash, Claims, and lifecycle-aware audit decisions.

## 39. v1 regression law

Patch 0014 must explicitly assert inherited Patch 0005 exact v1:

- structured canonical JSON bytes and byte length;
- rendered canonical JSON bytes and byte length;
- StructuredContextHash;
- RenderedContextHash;
- ContextPacketId;
- exact empty `recentPerformances`;
- exact historical public Compose signature.

No inherited digest may be updated merely because v2 exists.

The Patch 0005 test that previously asserted there was no Context recent-Performance type is intentionally superseded only by Patch 0014's approved closed `ContextRecentPerformance` type; all other provider-neutral/no-provenance laws remain intact.

## 40. Independent v2 reference oracles

Implementation must derive fixed v2 Context oracles independently from production canonicalizer code.

Required paths:

```text
Missing Raft fixture
    -> exact genesis ProductionState
        -> checkpoint
            -> ComposeGenesis
```

and:

```text
Missing Raft genesis
    -> exact Patch 0012 accepted causal commit
        -> exact Patch 0013 effective opportunity transition
            -> checkpoint
                -> ComposeNextTurn
```

Independent derivation must first reproduce inherited fixture/v1 Context/Production/Patch0013 hashes before a new v2 digest is accepted.

## 41. Required Production Access tests

At minimum:

1. genesis Production Access equals fixture Access for inherited categories;
2. SourceStateHash is exact;
3. unsupported Production contract fails;
4. active post-commit state changes are visible only according to access policy;
5. inactive records are absent and explicitly denied;
6. subject-owned CharacterClaim appears only in Claims;
7. other Character's claim is denied;
8. claim is absent from Knowledge/Belief/Memory/World/Scene categories;
9. subject-owned Relationship is visible and other-subject Relationship denied;
10. HistoricalTruth/WorldState/Unresolved remain denied;
11. lifecycle/protection/provenance are stripped;
12. malformed roster/domain/subtype/relationship/record identity fails;
13. inactive malformed records still fail structural validation;
14. ordering is ordinal and deterministic;
15. Access has no CausalCommit/Opportunity/Continuity dependency.

## 42. Required Context version-shape tests

At minimum:

1. v1 projection with SourceStateHash fails;
2. v1 projection with non-empty Claims fails;
3. v1 packet with any v2-only field fails canonicalization;
4. v2 packet requires initialized SourceStateHash;
5. v2 packet requires exact v2 schema/composition/rendering triple;
6. hybrid contract triples fail;
7. canonicalizer never drops Claims or RecentPerformances;
8. unsupported schema fails;
9. v1 bytes/hashes remain exact;
10. v2 exact property order is enforced;
11. recent item exact property order is enforced;
12. v2 repeat/culture determinism holds.

## 43. Required Context v2 behavioral tests

At minimum:

1. genesis v2 carries exact SourceStateHash;
2. genesis recentPerformances is empty;
3. claims are structurally separate;
4. `[WHAT YOU HAVE CLAIMED]` contains claims and no upgraded category does;
5. genesis RecentPerformanceText is empty;
6. ContextPacketId equals `CTX:` + StructuredContextHash;
7. duplicate record IDs across categories fail;
8. recent Character must belong to roster;
9. recent TakeId must be initialized;
10. recent text never enters TrustedStateText;
11. no claim appears as world truth/knowledge;
12. Trace SourceStateHash and recent Take IDs match packet inputs.

## 44. Required next-turn Continuity tests

At minimum:

1. valid Patch0012 -> Patch0013 -> checkpoint chain composes exactly one recent Performance;
2. recent TakeId/Character/VisibleText equal exact Accepted source Take;
3. current Context subject/opportunity equals checkpoint Current Opportunity;
4. SourceStateHash equals checkpoint StateHash;
5. stale/foreign source commit fails;
6. stale/foreign opportunity result fails;
7. commit result / opportunity parent mismatch fails;
8. opportunity result / checkpoint hash mismatch fails;
9. selected Character / checkpoint opportunity mismatch fails;
10. history Scene mismatch fails;
11. history last hash mismatch fails;
12. history last Character mismatch fails;
13. history penultimate Character not equal source Performer fails;
14. history shorter than two entries fails;
15. source CommitId missing from effective cache fails;
16. source TakeId missing from committed cache fails;
17. non-Accepted source Take fails;
18. Scene/roster/source Character mismatch fails;
19. unsupported opportunity strategy fails;
20. failure leaves all supplied immutable inputs unchanged.

## 45. Required Take-binding tests

At minimum:

1. exact v2 Context SourceStateHash binds;
2. foreign/uninitialized v2 SourceStateHash fails;
3. v2 hybrid contract triple fails;
4. evolved checkpoint + v1 Context fails downgrade guard;
5. exact genesis checkpoint + exact legacy v1 Context remains compatible;
6. existing Patch0012 Scene/subject/opportunity/roster/StateAuthority checks remain enforced;
7. v2 SourceStateHash check is additive, not replacement authority.

## 46. Required structural/reflection tests

At minimum:

- Continuity namespace exports only approved two public types;
- Continuity exposes only ComposeGenesis and ComposeNextTurn;
- no public Continuity constructor/session/history mutation API;
- ContextRecentPerformance has no public constructor/setter;
- Production Access overload exact signature is frozen;
- CharacterAccessProjection adds only approved fields;
- ContextPacket/Trace v2 additions are immutable;
- v1/v2 constants are exact;
- historical public Compose signature remains exact and sole public composer;
- production-bound composer is internal;
- Access does not reference CausalCommit/Opportunity/Continuity;
- Production does not reference Access/Context/Continuity;
- no Windows/network/random/time/provider/GPU/NPU public dependency is introduced.

## 47. Canonical preservation tests

Patch 0014 explicitly retains inherited fixed oracles including:

- Patch 0003 fixture hash;
- Patch 0005 v1 Context hashes;
- Patch 0012 genesis Production StateHash;
- Patch 0012 causal-commit StateHash;
- Patch 0013 opportunity-transition StateHash.

v2 Context identity is additive and rewrites none of them.

## 48. Simplicity guard

Patch 0014 does not introduce:

- general event-store abstraction;
- session aggregate;
- repository/service locator;
- generic context-source interface hierarchy;
- background caches;
- semantic retrieval/vector search;
- model-assisted access filtering;
- observer framework;
- provider routing;
- async machinery where no asynchronous work exists;
- second Production state representation.

The E0 Continuity bridge remains narrow and typed.

## 49. No full Scene-loop claim

After Patch 0014 Core may deterministically create the next legal ContextPacket after one completed causal-commit/opportunity cycle.

That is not a complete Scene loop.

Repeated orchestration, stop/budget semantics, provider attempts, retry/failure policy, and arbitrary event-sequence replay remain later approved work.

## 50. No full replay claim

Patch 0014 output is deterministic and recomputable from supplied authoritative inputs, but Patch 0014 defines no persistent ordered event stream that automatically reconstructs arbitrary turns from genesis.

Full multi-turn replay remains deferred.

## 51. No Observation claim

Immediate previous Accepted Performance disclosure is frozen only for the E0 co-present trio.

Patch 0014 does not infer line of sight, hearing range, channel membership, concealment, private messages, spatial adjacency, or attention gating.

Future Observation authority owns those broader semantics.

## 52. No provider/model claim

Patch 0014 produces a deterministic provider-neutral ContextPacket only.

It does not choose model/provider/runtime, Windows AI versus cloud, NPU/GPU/CPU inference, generation parameters, prompt template, retries, token budget, or cost authorization.

This keeps deterministic creative authority independent from replaceable inference infrastructure.

## 53. Expected implementation surface

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

Plus focused Patch0014 tests/evidence.

ProductionState projection/canonicalizer and Opportunity implementation should not need modification.

## 54. Proposal 0.2 audit corrections

Proposal 0.2 makes three material authority corrections to 0.1 without expanding scope:

1. next-turn continuity explicitly proves OpportunityHistory Scene and penultimate source Character before disclosing recent Performance;
2. v1/v2 shared models now have closed version/shape invariants, and public canonicalization must reject hybrid/downgraded shapes instead of silently omitting fields;
3. Production-backed Access explicitly validates the supported ProductionState contract and all retained record structure, including inactive records.

These corrections restart the recursive audit from correctness.

## 55. Recursive audit order

Before approval, recursively audit:

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

Any material correction restarts the pass from correctness.

## 56. Approval gate

Implementation must not begin until one complete recursive pass finds zero material corrections or worthwhile improvements and the user explicitly approves the blueprint.

Approval freezes:

- Patch0014 purpose/scope;
- Production-backed Access law;
- CharacterClaim disclosure law;
- immediate recent-Performance disclosure law;
- Continuity dependency placement;
- v1 preservation/v2 versioning and closed shape rules;
- SourceStateHash propagation;
- legacy v1 downgrade guard;
- exact v2 canonical/rendering shape;
- tests and non-goals.
