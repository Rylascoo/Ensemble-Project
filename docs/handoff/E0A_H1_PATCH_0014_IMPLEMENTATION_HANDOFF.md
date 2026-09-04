# Ensemble — E0-A H1 Patch 0014 Implementation Handoff

Prepared: 2026-09-03
Status: READY FOR IMPLEMENTATION

## Mission

Implement only the approved H1 Patch 0014 — E0 Production Context Continuity Proposal 0.10 on top of promoted Patch 0013.

Do not redesign the approved architecture during routine implementation. Do not enter CharacterClaim disclosure, recent-Performance disclosure, Observation authority, a new rendering contract, full Scene-loop orchestration, full session replay, persistence/recovery, relevance/token budgeting/summarization/compaction, provider execution, WinUI, Windows AI/NPU, packaging, WACK, or Store scope.

## Source-of-truth order

1. `CURRENT_STATE.md` — promoted Patch 0013 checkpoint and exact validation authority.
2. `docs/blueprint/H1_PATCH_0014_PRODUCTION_CONTEXT_CONTINUITY.md` — exact approved Proposal 0.10.
3. `docs/evidence/H1_PATCH_0014_BLUEPRINT_APPROVAL.md` — explicit user approval evidence.
4. `docs/evidence/H1_PATCH_0014_BLUEPRINT_AUDIT.md` — zero-material-change architecture audit evidence.
5. Current promoted Patch 0013 source/tests.
6. `docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md` for inherited Character-bounded Access law.
7. `docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md` for frozen v1 Context bytes/rendering.
8. `docs/blueprint/H1_PATCH_0012_ATOMIC_CAUSAL_COMMIT.md` for checkpoint/binding/Production association law.
9. `docs/blueprint/H1_PATCH_0013_EFFECTIVE_OPPORTUNITY_AUTHORITY.md` for the exact deferred common-state source-context proof and current-opportunity sequencing.
10. `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` — implementation hygiene.

## Authority checkpoints

Approved Blueprint Proposal 0.10 audited head:
`0c937054940a335d6a6f08f68d7effd104f944d2`

Blueprint audit evidence commit:
`bf761301cbfca5a2ae2311f87e8618cbcdf353c8`

Blueprint approval evidence commit:
`9da8d19fd3e6ca46c63a4d0657a64a34cbecb436`

Promoted parent `main` checkpoint:
`e06668a2307433bf99b0501dc38a701db392c633`

Inherited Patch 0013 full Core-test authority:
`a3fae23dc4df302e834b031ecfc848a3bb2d37fc`

Inherited Patch 0013 native Core/Harness build + fixture authority:
`382e11f9fbe6774806152fad75b6a23cc8733187`

Do not claim Patch 0014 compiler/runtime validation until the user runs the native ARM64 gate against the exact implementation head.

## Approved data flow

```text
current ProductionState with Current Opportunity
    -> ProductionStateCheckpoint.Capture(...)
        -> CharacterBoundedAccessControl.Evaluate(ProductionState, CharacterId)
            -> CharacterAccessEvaluation
                -> safe CharacterAccessProjection(SourceStateHash = checkpoint.StateHash)
                    -> internal Production-bound Context v2 composition
                        -> ContextCompositionEvaluation(SourceStateHash = checkpoint.StateHash)
                            -> E0ProductionContextContinuityResult
```

The continuity result retains both:

```text
AccessEvaluation
ContextEvaluation
```

from the same checkpoint so trusted orchestration/provenance can retain the Access decision audit without rescanning Production. Context Composer still receives only the safe projection, never denied AccessDecision data.

## Public surface

### Access

Add exactly:

```text
CharacterBoundedAccessControl.Evaluate(
    ProductionState sourceState,
    CharacterId subjectCharacterId)
```

`CharacterAccessProjection` adds exactly:

```text
SourceStateHash : StateHash?
```

`AccessReason` appends exactly:

```text
InactiveRecordExcluded
CharacterClaimDisclosureDeferred
```

Preserve all existing enum values/positions.

### Context

`E0ContextContracts` adds exactly:

```text
ProductionBoundSchemaVersion
= "ensemble.e0.context.v2"

ProductionBoundCompositionContract
= "ensemble.e0.context.production-bound.v1"
```

Do not add a render-v2 contract.

`ContextPacket` adds exactly:

```text
SourceStateHash : StateHash?
```

`ContextCompositionTrace` adds exactly:

```text
SourceStateHash : StateHash?
```

Existing public v1 `DeterministicContextComposer.Compose(CharacterAccessProjection, CharacterId)` remains the sole public composer and must reject Production-backed projections.

Production-bound v2 composition remains internal.

### Continuity

New public namespace:

`Ensemble.E0.Core.Continuity`

Approved public types:

```text
E0ProductionContextContinuity
E0ProductionContextContinuityResult
E0ContextContinuityException
```

Approved public entry point:

```text
E0ProductionContextContinuity.Compose(
    ProductionStateCheckpoint sourceCheckpoint)
    -> E0ProductionContextContinuityResult
```

No history, CausalCommit, Opportunity, Director, Observation, provider, or recent-Performance parameters.

## Production Access law

Evaluate every retained Production record exactly once for deterministic AccessDecision coverage.

Only Active currently authorized records enter the Character-facing projection.

Inherited active rules remain:

```text
HistoricalTruth          -> Deny / ProductionAuthorityExcluded
UnresolvedProposition    -> Deny / ProductionAuthorityExcluded
WorldState               -> Deny / ProductionAuthorityExcluded
SceneState               -> Permit / SharedSceneState
Pressure                 -> Permit / PublicPressure
subject-owned inherited Character domains -> Permit / OwnedBySubject
other-owned inherited Character domains   -> Deny / OwnedByOtherCharacterExcluded
subject-owned Relationship                 -> Permit / OwnedBySubject
other-owned Relationship                   -> Deny / OwnedByOtherCharacterExcluded
```

Patch 0014-specific rules:

```text
any Active CharacterClaim -> Deny / CharacterClaimDisclosureDeferred
any Inactive record        -> Deny / InactiveRecordExcluded
```

Inactive precedence comes first.

CharacterClaim denial is deliberate: committed Claim is not Memory/current recall and does not become Character context automatically.

## Production Access structural gate

Fail closed unless at minimum:

- exact supported `ensemble.e0.production-state.v1` contract;
- initialized StateHash/SceneId;
- exactly three unique canonical Production Characters;
- Production Character identities exactly equal canonical roster identities;
- canonical display names;
- subject resolves exactly once in both Characters and roster;
- unique ordinal retained RecordIds;
- valid canonical record identity/text;
- ProductionRecord runtime subtype agrees with domain;
- Character record subjects are roster members;
- Relationship subject/target are valid, distinct roster Characters;
- lifecycle/protection/domain enum values are defined;
- denied/inactive records are structurally validated too.

Do not duplicate full provenance DAG validation per Access evaluation. Provenance does not grant Access and Production mutation/genesis authorities already own DAG validation.

## Context v1 hard preservation

Patch 0005 v1 bytes are immutable.

Preserve exact:

```text
schemaVersion = ensemble.e0.context.v1
compositionContract = ensemble.e0.context.full-authorized.v1
renderingContract = ensemble.e0.context.render.v1
recentPerformances = []
RecentPerformanceText = ""
```

Preserve existing fixed structured/rendered byte lengths, hashes, and ContextPacketId oracles.

No historical v1 digest is updated merely because v2 exists.

## Production-bound Context v2

v2 changes structured Context bytes only by:

1. v2 schema/composition tokens;
2. inserting `sourceStateHash` immediately after `compositionContract`.

Exact root order:

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

`recentPerformances` remains exactly `[]`.

Rendering remains exact existing render-v1 semantics.

Strong genesis compatibility law:

```text
genesis fixture/v1 rendered canonical bytes
== genesis Production/v2 rendered canonical bytes
```

Therefore genesis v1/v2 RenderedContextHash must match, while structured Context identities must differ.

`SourceStateHash` is non-diegetic system association metadata. Never render it into Character-facing trusted text, Opportunity text, future creative prompt prose, or recent-performance text.

## Closed v1/v2 canonical shape law

`ContextPacketCanonicalizer.SerializeStructured(ContextPacket)` must validate the complete version/shape combination before serializing.

Valid v1 requires:

```text
SchemaVersion == v1
CompositionContract == historical v1 composition
RenderingContract == render.v1
SourceStateHash == null
RecentPerformanceText == ""
```

Valid v2 requires:

```text
SchemaVersion == v2
CompositionContract == production-bound.v1
RenderingContract == render.v1
SourceStateHash initialized
RecentPerformanceText == ""
```

Hybrid/unsupported combinations fail closed.

Never silently drop SourceStateHash under v1.

## Exact source-context proof at Take binding

Matching `SourceStateHash` is necessary but not sufficient.

For v2, `E0TakeStateBinding.Bind(...)` must:

1. freshly evaluate Production Access from `sourceCheckpoint.SourceState` for checkpoint CurrentOpportunity;
2. internally compose the expected Production-bound v2 Context from that fresh safe projection;
3. compare expected vs supplied v2 packet using exact canonical structured bytes, exact canonical rendered bytes, ContextPacketId, StructuredContextHash, RenderedContextHash, SourceStateHash, subject/opportunity/Scene/roster identity;
4. then continue all existing StateAuthority snapshot and Accepted-Take checks.

This closes the Patch 0013-deferred requirement to prove that disclosed records came from the exact source ProductionState.

Do not treat StateHash metadata as a capability token.

## Historical v1 binding compatibility

A v1 Context may bind only when:

1. source checkpoint is exact genesis by recomputing the inherited genesis StateHash;
2. fresh Production Access is evaluated for checkpoint CurrentOpportunity;
3. a private compatibility projection drops only SourceStateHash;
4. frozen public v1 Context Composer recomposes expected v1 Context;
5. exact structured/rendered bytes and all identities/hashes match supplied v1 packet.

Rules:

```text
exact genesis + exact Production-derived v1 -> permitted
genesis + foreign/different v1             -> reject
evolved state + any v1                     -> reject
evolved state + exact Production-derived v2 -> required
```

Do not manually duplicate Access or rendering policy in CausalCommit.

## Observation / history / claim hard boundary

Patch 0014 must not populate or infer:

- recent Performance;
- Character observation eligibility;
- CharacterClaim recall;
- cross-Character claim knowledge;
- Performance-history narration.

Co-presence, AddressedCharacterIds, NominatedCharacterId, Director selection, relationship state, and causal adjacency do not establish perception.

Keep:

```text
recentPerformances = []
RecentPerformanceText = ""
CharacterClaim -> denied
```

## Inherited-test supersession

Exactly two inherited tests currently encode the temporary historical condition that Production-backed Access did not yet exist:

- `tests/Ensemble.E0.Core.Tests/CausalCommit/Patch0012ContractAuditTests.cs`;
- `tests/Ensemble.E0.Core.Tests/Opportunity/Patch0013ContractAuditTests.cs`.

Patch 0014 is the approved boundary that supersedes only those specific “not yet” assertions.

Narrowly evolve them so the inherited suite accepts the approved Production Access overload while retaining all enduring privacy, dependency, hardware-exclusion, public-surface, and canonical-hash assertions.

Do not modify historical evidence documents.

## Independent v2 oracle requirement

Before freezing v2 expected hashes, independently derive them outside the production Context canonicalizer.

The independent derivation must first reproduce:

- inherited Patch 0005 v1 structured/rendered hashes;
- inherited genesis StateHash;
- inherited Patch 0012 causal-commit StateHash;
- inherited Patch 0013 opportunity-transition StateHash for evolved case.

Then fix independent v2 structured Context hashes/ContextPacketIds for:

1. Missing Raft genesis current opportunity;
2. exact evolved Missing Raft state after Patch0012 + Patch0013.

No v2 hash claim is valid merely because production code produced it.

## Expected implementation surface

Likely modified production files:

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

Plus focused Patch 0014 tests/evidence and only the two narrowly superseded inherited assertions above.

Prefer keeping `ContextPacketCanonicalizer.SerializeRendered` and trusted-state rendering source unchanged because render semantics do not change.

ProductionState transition/canonicalization, Opportunity, Performer, Integrity, Interpreter, and State Authority should require no semantic redesign.

## Test matrix

At minimum cover:

### Production Access
- genesis equivalence to fixture Access;
- exact SourceStateHash;
- committed active inherited-domain changes;
- inactive exclusion/reason precedence;
- subject/other CharacterClaim deferred denial;
- no Claim text/ID in projection;
- relationship directionality;
- Production truth exclusion;
- provenance/protection/lifecycle stripping;
- malformed Characters/roster/display names/domain/subtype/record/relationship;
- denied/inactive malformed record validation;
- ordinal deterministic output;
- generic smoke + Missing Raft.

### Context v2
- exact SourceStateHash genesis/evolved;
- exact empty recentPerformances/RecentPerformanceText;
- no Claims or recent-history public type/property;
- exact root order;
- render-v1 contract;
- genesis v1/v2 rendered bytes/hash equality;
- structured identity divergence;
- exact ContextPacketId relation;
- repeat/culture determinism;
- denied/private metadata absence;
- independent fixed v2 oracles.

### Version/downgrade
- public v1 Compose rejects Production-backed projection;
- v1 packet with SourceStateHash fails;
- v2 requires initialized SourceStateHash;
- hybrid/unsupported triples fail;
- inherited v1 bytes/oracles exact;
- no render-v2 constant.

### Continuity
- genesis and exact Patch0013 evolved checkpoints;
- result contains same Access + Context evaluations from one source state;
- Access/Packet/Trace hashes equal checkpoint hash;
- no-opportunity state cannot checkpoint;
- malformed source fails;
- repeat byte identity;
- no source mutation;
- public surface contains no history/Observation/Director/provider input.

### Take binding
- exact fresh-derived v2 succeeds;
- packet with same StateHash but mismatched Character-safe content fails;
- packet from different Production state fails;
- v2 byte/render/hash tamper fails;
- evolved + v1 fails;
- exact genesis + exact recomposed v1 succeeds;
- foreign genesis v1 fails;
- existing Scene/subject/opportunity/roster/StateAuthority checks remain.

### Structural/hygiene
- exact approved Continuity public surface;
- SourceStateHash-only Access/Context/Trace evolution;
- exact AccessReason append order;
- no recent Performance type;
- no ProductionBoundRenderingContract;
- lower dependency direction preserved;
- two inherited temporary assertions narrowly evolved;
- new contract constants tested via runtime/reflection/string paths where needed so MSTest does not flag compile-time tautologies.

## Performance / ARM64 accounting

Let:

```text
R = retained Production records
A = active permitted records
B = canonical/rendered permitted byte volume
```

Expected deterministic CPU work:

```text
Production Access                 ~ O(R)
Context composition/canonicalize  ~ O(A log A + B)
Continuity Compose                ~ O(R + A log A + B)
Take binding fresh source proof   ~ O(R + A log A + B) + existing StateAuthority validation
```

No idle/background/network/provider/GPU/NPU work.

Do not claim constant long-session turn cost or retail battery performance without device profiling.

## Recursive implementation audit

After implementation, repeatedly audit:

```text
correctness
-> consistency
-> authority
-> disclosure/privacy
-> dependency direction
-> canonical/version compatibility
-> tests
-> simplicity
-> hygiene
-> ARM64/battery suitability
-> project vision
-> evidence
```

Any material correction restarts from correctness.

Stop only after one complete pass finds no material correction or worthwhile improvement.

## Native ARM64 validation boundary

After static implementation closure, return exact commands for the user to run on the native Windows ARM64 machine. At minimum:

```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
```

Do not promote static analysis to native compiler/runtime authority.
