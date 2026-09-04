# Ensemble — E0-A H1 Patch 0015 Implementation Handoff

Prepared: 2026-09-04
Status: READY FOR IMPLEMENTATION

## Mission

Implement only the explicitly approved `H1 Patch 0015 — E0 Accepted Performance History + Context Continuity`, Proposal 0.15, on top of promoted Patch 0014.

Do not redesign the approved architecture during routine implementation. If implementation requires a material semantic change to the frozen authority model, stop and reopen architecture rather than widening the patch silently.

## Source-of-truth order

1. `CURRENT_STATE.md` — promoted Patch 0014 checkpoint and current validation authority.
2. `docs/blueprint/H1_PATCH_0015_RECENT_PERFORMANCE_CONTEXT_CONTINUITY.md` — exact approved Proposal 0.15.
3. `docs/evidence/H1_PATCH_0015_BLUEPRINT_APPROVAL.md` — explicit approval evidence.
4. `docs/evidence/H1_PATCH_0015_BLUEPRINT_AUDIT.md` — complete zero-material-change architecture audit.
5. Current promoted Patch 0014 source/tests.
6. `docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md` — frozen Context v1 semantics and historical recent-performance reservation.
7. `docs/blueprint/H1_PATCH_0006_PERFORMER_CANDIDATE_CONTRACT.md` — exact Candidate VisibleText grammar and Performer boundary.
8. `docs/blueprint/H1_PATCH_0011_TAKE_SEMANTICS.md` — Take authority.
9. `docs/blueprint/H1_PATCH_0012_ATOMIC_CAUSAL_COMMIT.md` — accepted causal-event authority and replay.
10. `docs/blueprint/H1_PATCH_0013_EFFECTIVE_OPPORTUNITY_AUTHORITY.md` — routing-history and Opportunity replay authority.
11. `docs/blueprint/H1_PATCH_0014_PRODUCTION_CONTEXT_CONTINUITY.md` — exact Production-bound Context source proof and historical v1/v2 compatibility.
12. `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` — implementation hygiene.

## Authority checkpoints

Approved Proposal 0.15 audited head:

`cea5820b9614cf9028340a23cf931696feb98680`

Blueprint audit evidence commit:

`521bcc197c5fd348936deb540d12f2012cf8241e`

Blueprint approval evidence commit:

`81b27ade5fb5fd7224ce1709ddfcf01615f525ff`

Promoted parent `main` checkpoint:

`7475a9397cff9063673908c666a729f0f3cd4525`

Inherited Patch 0014 full Core-test authority:

`4ac0250005c8d88c3b815c5d53cfca0a982e454c` — `538/538` PASS

Inherited Patch 0014 native Core/Harness + fixture authority:

`84b3e23db55910f746670cd2e06a67b8a5dea2b3`

Do not claim Patch 0015 compiler/runtime validation until the user runs the native Windows ARM64 gates against the exact implementation head.

## Frozen data flow

Approved Full Ensemble history-bearing sequence:

```text
current opportunity-bearing ProductionState
+ synchronized opaque accepted Performance history
+ synchronized E0OpportunityHistory
    -> ProductionStateCheckpoint.Capture
    -> E0ProductionContextContinuity.ComposeWithAcceptedHistory
         exact empty genesis => existing v2
         accepted history    => v3
    -> Performer Candidate
    -> Integrity
    -> State Interpretation
    -> State Authority
    -> E0Take.Bind(... Accepted ...)
    -> E0TakeStateBinding.BindWithAcceptedHistory
         sole full structured+rendered precommit source proof
    -> DeterministicCausalCommit.Commit
         stage result
    -> E0AcceptedPerformanceHistoryContinuity.RecordCommit
         fresh structured semantic source identity proof
         + canonical causal replay
         + append one accepted Performance semantic item
    -> adopt postcommit Production + history pair only after success
    -> DeterministicOpportunityAuthority.Establish
         stage result
    -> E0AcceptedPerformanceHistoryContinuity.RecordOpportunity
         canonical Opportunity replay
         + routing/history coupling
    -> adopt opportunity-bearing Production + history + OpportunityHistory only after success
```

Patch 0015 defines the deterministic boundaries above. It does not implement a generic Scene-loop orchestrator.

## Dependency law

Preserve:

```text
Domain / Fixture / Production
    -> Access
        -> Context
            -> Performer / Integrity / Interpreter / StateAuthority / Take
                -> CausalCommit
                    -> Opportunity
```

Patch 0015 adds only:

```text
Domain.CharacterLegibleTextInvariants
    -> Context + Performer + CausalCommit internal validation

ContextRecentPerformance
    -> opaque accepted-history DATA in CausalCommit

opaque accepted-history DATA
    -> history-aware exact Take proof in CausalCommit

CausalCommit + Opportunity + Context continuity
    -> accepted-history ADVANCEMENT in Continuity

opaque accepted history
    -> history-aware Production Context continuity
```

Hard rules:

- Domain text invariant depends only on BCL text/Unicode primitives.
- Context must not depend on Performer, CausalCommit, Opportunity, or Continuity.
- Performer delegates its existing VisibleText grammar to the neutral Domain invariant.
- CausalCommit may depend on lower Context/Domain types but must not depend on Opportunity or Continuity.
- Opportunity remains unchanged and still depends on CausalCommit, never reverse.
- Production projection and transition semantics remain unchanged.

## Neutral Character-legible text invariant

Extract the exact Patch 0006 private Candidate VisibleText grammar into one internal Domain-layer owner, conceptually:

```text
Ensemble.E0.Core.Domain.CharacterLegibleTextInvariants
```

No public API.

Preserve exact grammar:

- null invalid;
- empty string valid semantic silence;
- valid UTF-16 surrogate structure;
- nonempty text already Unicode NFC;
- Control characters forbidden except TAB U+0009 and LF U+000A;
- nonempty text contains at least one display-bearing Unicode scalar under the exact Patch 0006 category rule.

The neutral invariant must provide enough internal failure classification for each caller to map failures into its own sanitized exception domain without embedding input text.

`PerformerCandidateContract` must delegate to this helper while preserving current public Candidate behavior/messages. Do not create a second handwritten VisibleText validator.

## Exact public surface

### `Ensemble.E0.Core.CausalCommit`

Add exactly:

```csharp
public sealed class E0AcceptedPerformanceHistory
```

Public surface on the token:

```text
public constructors = 0
public setters      = 0
public properties   = 0
public methods declared on type = 0
```

Internal state exactly:

```text
SceneId : SceneId
CurrentStateHash : StateHash
Entries : ImmutableArray<ContextRecentPerformance>
```

Extend `E0TakeStateBinding` with exactly:

```csharp
E0TakeStateBinding BindWithAcceptedHistory(
    ProductionStateCheckpoint checkpoint,
    ContextPacket context,
    E0Take take,
    E0AcceptedPerformanceHistory history)
```

Historical `Bind(checkpoint, context, take)` remains unchanged.

### `Ensemble.E0.Core.Context`

Add exactly:

```csharp
public sealed class ContextRecentPerformance
```

Public read-only properties exactly:

```text
SourceCharacterId : CharacterId
VisibleText       : string
```

No public constructor/setter.

`ContextPacket` adds exactly:

```text
RecentPerformances : ImmutableArray<ContextRecentPerformance>
```

### `Ensemble.E0.Core.Continuity`

Add exactly:

```csharp
public static class E0AcceptedPerformanceHistoryContinuity
public sealed class E0AcceptedPerformanceHistoryException : Exception
```

Public transition methods exactly:

```csharp
E0AcceptedPerformanceHistory Initialize(
    ProductionState genesisState)

E0AcceptedPerformanceHistory RecordCommit(
    E0AcceptedPerformanceHistory sourceHistory,
    ProductionState parentState,
    E0CausalCommit committedEvent)

E0AcceptedPerformanceHistory RecordOpportunity(
    E0AcceptedPerformanceHistory sourceHistory,
    ProductionState postCommitState,
    E0CausalCommit sourceCommit,
    E0OpportunityHistory sourceOpportunityHistory,
    E0OpportunityTransition establishedEvent)
```

Extend `E0ProductionContextContinuity` with exactly:

```csharp
E0ProductionContextContinuityResult ComposeWithAcceptedHistory(
    ProductionStateCheckpoint checkpoint,
    E0AcceptedPerformanceHistory history)
```

Historical `Compose(checkpoint)` remains unchanged.

No public transcript/history collection, count, history hash, repository, event-store abstraction, provider abstraction, or Scene orchestrator.

## Accepted history semantics

History is an opaque, closed, in-memory derived Context projection.

Internal payload contains only:

```text
ImmutableArray<ContextRecentPerformance>
```

Each item contains only:

```text
SourceCharacterId
VisibleText
```

Never copy into the history item:

- CommitId;
- TakeId;
- parent/result StateHash;
- ContextPacketId;
- CandidateContentHash;
- Candidate identity contract;
- typed address/nomination control;
- State Authority data;
- materializations;
- provider data;
- Observation/Knowledge/Belief/Memory/Claim classification.

Authority remains:

```text
ProductionState               current-state authority
E0CausalCommit                causal-event authority
E0OpportunityHistory          routing-history projection
E0AcceptedPerformanceHistory  opaque live Context projection only
```

StateHash synchronization is not arbitrary transcript authentication.

## Initialization

`E0AcceptedPerformanceHistoryContinuity.Initialize(genesisState)` must reuse:

```csharp
E0OpportunityHistory.Initialize(genesisState)
```

for exact genesis validation.

Success internal state:

```text
SceneId = genesis.SceneId
CurrentStateHash = genesis.StateHash
Entries = initialized empty
```

No opening transcript is synthesized.

## Commit advancement

`RecordCommit(history, parentState, committedEvent)` must:

1. validate closed inputs/history;
2. require Scene and CurrentStateHash match parent;
3. capture parent checkpoint;
4. freshly `ComposeWithAcceptedHistory` from that exact source;
5. require Accepted committed Take and exact Performance subject/ContextPacketId against the fresh expected packet;
6. canonical-replay with `DeterministicCausalCommit.Replay(parentState, committedEvent)`;
7. require replay Scene exact and no current opportunity;
8. append exactly one `ContextRecentPerformance` from committed Performance subject + exact VisibleText;
9. advance internal CurrentStateHash to replayed postcommit StateHash;
10. return a new token without mutating the source.

Postcommit proof limit is explicit:

- `RecordCommit` proves exact structured semantic source Context identity through ContextPacketId and canonical replay.
- It does not independently re-prove historical rendered bytes because the established causal event does not retain the source `RenderedContext` object/bytes.
- Full structured+rendered proof belongs to `BindWithAcceptedHistory` before commit.

## Opportunity advancement

`RecordOpportunity(history, postCommitState, sourceCommit, sourceOpportunityHistory, establishedEvent)` must:

1. validate exact state/history synchronization;
2. require nonempty accepted history;
3. require source commit result hash equals postcommit state hash;
4. require last accepted history semantic item equals source committed Performance on subject + exact VisibleText;
5. require pre-transition OpportunityHistory count equals PerformanceHistory count;
6. canonical-replay using `DeterministicOpportunityAuthority.Replay(...)`;
7. require result routing-history count = accepted-history count + 1;
8. require result routing-history last Character == selected event Character == result-state current opportunity;
9. append no Performance;
10. advance only internal CurrentStateHash to replayed Opportunity StateHash;
11. return a new token.

Count induction:

```text
genesis:           OpportunityHistory = 1, PerformanceHistory = 0
after commit:      OpportunityHistory = H, PerformanceHistory = H
after opportunity: OpportunityHistory = H+1, PerformanceHistory = H
```

Do not duplicate Director selection, Opportunity hashing, or replay logic.

## E0 Performance eligibility / Observation boundary

Current fixture dialect validates exactly:

```text
ensemble.e0.copresent-trio.v1
```

Patch 0015 gives this current bounded E0 contract only the following new meaning:

> Every successfully committed `CandidatePerformance.VisibleText` in the current E0 Scene is common Character-legible recent Performance content for every current roster Character.

This is not a general Observation engine.

Do not expose or infer:

- typed address/nomination control;
- hidden reasoning;
- provider diagnostics;
- creator-only state;
- inferred consequence;
- objective truth;
- CharacterObservation;
- Knowledge;
- Belief;
- Suspicion;
- Memory;
- CharacterClaim.

Self history remains included. Selective/private/spatial/inaudible/concealed Performance remains future Observation authority.

## Context v3

Historical constants remain exact:

```text
ensemble.e0.context.v1
ensemble.e0.context.full-authorized.v1
ensemble.e0.context.render.v1
ensemble.e0.context.v2
ensemble.e0.context.production-bound.v1
```

Add exactly:

```text
AcceptedHistorySchemaVersion
    = ensemble.e0.context.v3
AcceptedHistoryCompositionContract
    = ensemble.e0.context.production-bound.accepted-history.v1
AcceptedHistoryRenderingContract
    = ensemble.e0.context.render.v2
```

Version matrix:

```text
v1 + full-authorized.v1 + render.v1
    SourceStateHash = null
    RecentPerformances = initialized empty
    RecentPerformanceText = ""

v2 + production-bound.v1 + render.v1
    SourceStateHash initialized
    RecentPerformances = initialized empty
    RecentPerformanceText = ""

v3 + production-bound.accepted-history.v1 + render.v2
    SourceStateHash initialized
    RecentPerformances = initialized nonempty
    RecentPerformanceText = nonempty
```

Reject all hybrids and default/uninitialized RecentPerformances.

Exact genesis history-aware composition remains v2 unchanged. There is no empty-history v3 packet.

## Structured v3 canonicalization

Preserve v2 root order exactly and make `recentPerformances` the final root field.

Recent item property order exactly:

```text
sourceCharacterId
visibleText
```

Recent array order is accepted causal append order and is never sorted.

Identity:

```text
StructuredContextHash = SHA256(canonical v3 UTF-8 bytes)
ContextPacketId = "CTX:" + StructuredContextHash
```

## Render-v2

Trusted-state and Opportunity rendering remain byte-for-byte inherited. Only RecentPerformanceText gains nonempty behavior.

Non-silent item form:

```text
[RECENT PERFORMANCES]
<source display name>:
[PERFORMANCE]
- <first line>
  <continuation line>

<next source display name>:
[PERFORMANCE]
- ...
```

Rules:

- accepted causal order;
- one blank line between entries;
- no trailing LF;
- display name from current roster;
- exact source text, no trim/repair/paraphrase/summarize/reorder;
- inherited LF split and two-space continuation discipline;
- no Character IDs or causal/provenance IDs rendered.

Semantic silence:

```text
<source display name>:
[PERFORMANCE: SILENCE]
```

Literal text `[PERFORMANCE: SILENCE]` remains ordinary non-silent text and must not collide with semantic silence.

## History-aware Context and Take proof

`ComposeWithAcceptedHistory`:

- require history Scene/StateHash synchronization;
- fresh Production Access exactly once;
- empty exact genesis => existing v2;
- nonempty non-genesis => v3;
- Packet/Trace SourceStateHash must equal checkpoint;
- normalize expected history failures to `E0ContextContinuityException`.

`BindWithAcceptedHistory`:

- exact genesis v2 + exact empty history allowed;
- v3 + exact synchronized nonempty history allowed;
- v1 rejected;
- evolved v2 rejected;
- state/Scene/history mismatch rejected;
- fresh Access + fresh exact expected Context;
- exact canonical structured bytes;
- exact canonical rendered bytes;
- ContextPacketId / structured hash / rendered hash / SourceStateHash;
- inherited Take/source/opportunity/Scene/roster proof;
- inherited StateAuthority snapshot proof;
- normalize expected history failures to `E0CausalCommitException`.

SourceStateHash alone never rescues mismatched content.

## Historical compatibility

Do not alter historical v1/v2 canonical identities, behavior, or public APIs.

Historical APIs remain:

```csharp
DeterministicContextComposer.Compose(...)
E0ProductionContextContinuity.Compose(checkpoint)
E0TakeStateBinding.Bind(checkpoint, context, take)
```

Historical `Compose(checkpoint)` may still emit evolved v2 under Patch 0014 compatibility behavior. Historical three-argument Take binding rejects v3.

## Live oracle branch

Do not reuse Patch 0012/0013 v1-source StateHashes for the new live v2-source lineage.

Frozen independent intermediate values:

```text
Production-bound genesis VOSS ContextPacketId
CTX:27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565

v2-source CandidateContentHash
6ce2a98dc6208400126feba1a356910376d40c6155843764ecf673b8b9441ec1

v2-source StateAuthority ProposalContentHash
ac0f7a91c855b99fdb6f1b9415ff150352e8e3f79268e666d38531323b7d16c3
```

The live oracle must use one exact fixed branch, preserving historical oracle semantics where applicable:

```text
source Production = exact Missing Raft genesis
source Context = exact Production-bound VOSS v2
VisibleText = "No."
Candidate control = empty addressed list + null nomination
Take disposition = Accepted
Pressure mutation/materialization = canonical Patch0012 oracle semantics
TakeId = TAKE-PATCH-0015-LIVE-ORACLE
CommitId = COMMIT-PATCH-0015-LIVE-ORACLE
materialized RecordId = PRESSURE-PATCH-0015-LIVE-ORACLE
```

Before native validation, independent oracle evidence must:

1. reproduce frozen historical v1/v2 and Patch0012/Patch0013 hashes;
2. reproduce the v2-source Candidate/proposal hashes above;
3. derive/freeze new live postcommit StateHash;
4. derive/freeze MARLOWE selection + new live Opportunity StateHash;
5. derive/freeze first MARLOWE v3 structured byte count/hash/ContextPacketId;
6. derive/freeze render-v2 byte count/hash;
7. prove multi-entry append order;
8. prove semantic silence differs from empty history and literal silence-marker text.

Independent oracle derivation is static evidence, not machine validation.

## Narrow inherited test adaptations

Only executable tests whose temporary premise is explicitly superseded may change.

Known required adaptations:

1. `tests/Ensemble.E0.Core.Tests/Context/DeterministicContextComposerTests.cs`
   - replace the old no-recent-history-type assertion with the exact one-type closed surface.
2. `tests/Ensemble.E0.Core.Tests/CausalCommit/Patch0012ContractAuditTests.cs`
   - add only `E0AcceptedPerformanceHistory` to the exact public CausalCommit type list; preserve all unrelated guards.
3. `tests/Ensemble.E0.Core.Tests/Continuity/Patch0014ContractAuditTests.cs`
   - evolve only the temporary no-history/v3/constants/API assertions.
4. `tests/Ensemble.E0.Core.Tests/Continuity/ProductionContextContinuityTests.cs`
   - evolve exact Continuity namespace and exact `Compose` method-count/signature assertions to include the approved history additions.
5. `tests/Ensemble.E0.Core.Tests/Continuity/ProductionContextBindingTests.cs`
   - adapt only the private positional `ContextPacket` reflection clone helper to carry `RecentPerformances`; preserve existing tamper coverage.

If implementation reveals another hidden exact-surface assertion, modify only the smallest superseded premise and document it in static audit evidence.

Do not modify historical evidence files.

## Expected implementation source surface

Patch-first target:

```text
src/Ensemble.E0.Core/Domain/
    internal CharacterLegibleTextInvariants

src/Ensemble.E0.Core/Context/
    ContextRecentPerformance
    ContextPacket.RecentPerformances
    v3/render-v2 constants and canonical shape support
    internal accepted-history composer/rendering support

src/Ensemble.E0.Core/Performer/
    delegate existing VisibleText grammar to neutral Domain invariant only

src/Ensemble.E0.Core/CausalCommit/
    opaque E0AcceptedPerformanceHistory
    internal history validation/projector
    E0TakeStateBinding.BindWithAcceptedHistory

src/Ensemble.E0.Core/Continuity/
    E0AcceptedPerformanceHistoryContinuity
    E0AcceptedPerformanceHistoryException
    E0ProductionContextContinuity.ComposeWithAcceptedHistory

focused Patch 0015 tests
narrow inherited adaptations above
```

Preferred no-semantic-change areas:

- Fixture JSON/dialect;
- Access policy;
- Production projection/canonicalizer/transition;
- Candidate public contract/parser/control semantics;
- Director policy;
- Opportunity public semantics/canonicalizer;
- Integrity semantics;
- State Interpreter semantics;
- State Authority semantics;
- E0Take semantics;
- Harness/provider/runtime execution.

If a material semantic change to those preferred no-change areas becomes necessary, stop and reopen architecture.

## Minimum test matrix

Implement focused tests for:

- opaque history exact public/internal shape;
- exact genesis initialization/non-genesis rejection;
- neutral Character-legible text invariant equivalence with all existing Performer text cases;
- historical Performer exception behavior preserved;
- v1/v2 historical canonical bytes/hashes exact;
- exact v1/v2/v3 version matrix and hybrid rejection;
- exact v3 root/item order;
- exact single/multi-entry rendering, silence, multiline behavior, no trailing LF;
- genesis empty history => unchanged v2;
- evolved empty history rejection;
- exact synchronized nonempty history => v3;
- history-aware binder full exact byte/hash proof;
- stale/reordered/dropped/extra/tampered history rejection;
- Accepted zero-mutation and all-consequence-Rejected commits append;
- failed/nonaccepted paths do not append;
- RecordCommit exact canonical replay and structured source identity proof;
- RecordOpportunity exact replay/count induction and no Performance append;
- two/three-turn causal append order, repetition, same Character recurrence, self history;
- earlier accepted text survives without durable consequence;
- recent Performance remains separate from trusted consequence and epistemic state;
- no typed control/causal/provider/private metadata in recent DTO/rendering;
- v3 passes Candidate -> Integrity -> Interpreter -> Authority -> Take unchanged;
- independent live oracle hashes/bytes;
- repeat determinism and culture invariance;
- exact dependency/public-surface guards;
- no provider/network/filesystem/clock/random/Windows/GPU/NPU dependency.

## Complexity / ARM64 suitability

Let:

```text
R = retained Production records
A = permitted current records
B = permitted current-state bytes
H = accepted Performance count in current E0 Scene
P = total accepted VisibleText bytes
```

Expected explicit-boundary work:

```text
history validation        O(H + P when text validation required)
history-aware Context     O(R + A log A + B + H + P)
history-aware Take proof  fresh history-aware Context + inherited StateAuthority proof
RecordCommit              fresh history-aware Context + canonical commit replay + immutable append
RecordOpportunity         canonical Opportunity replay + O(H) history validation/coupling
```

No idle polling, background work, filesystem, network, provider, GPU/NPU, clock, random, or emulation path is added.

Do not claim measured battery or long-session performance without device profiling.

## Recursive implementation audit

After implementation, repeatedly audit:

```text
correctness
-> consistency
-> authority
-> accepted-history integrity
-> disclosure/privacy
-> epistemic separation
-> dependency direction
-> canonical/version compatibility
-> multi-turn coherence
-> failure behavior
-> scope
-> tests
-> simplicity
-> hygiene
-> ARM64 suitability
-> project vision
-> evidence
```

Any material correction restarts from correctness.

Stop only after one complete pass finds no material correction or worthwhile improvement.

## Native ARM64 validation boundary

After static implementation closure, provide exact commands for the user to run on the native Windows ARM64 machine.

At minimum the final exact implementation head must exercise:

```powershell
git rev-parse HEAD
git diff --quiet
git diff --cached --quiet

dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug

dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug

dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json

dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
```

Record actual machine-discovered test count; do not predict or promote a count as authority.

Preserve unrelated local/untracked files.

## Explicit non-scope

Do not enter:

- general Observation generation;
- private/spatial/hearing/attention/concealment semantics;
- CharacterClaim disclosure;
- Knowledge/Belief/Memory promotion from history;
- relevance/windowing/summarization/token budgeting;
- cross-Scene retrieval;
- provider/model execution;
- provider request/attempt/retry/spend/streaming provenance;
- full Scene-loop orchestration;
- durable event persistence/recovery or full replay from genesis;
- branch/canon/retcon/rehearsal;
- World Resolver;
- WinUI;
- Windows AI Foundry/NPU;
- MSIX/WACK/Store work.

## Implementation-start rule

Implementation may begin only from the implementation branch created from this handoff checkpoint.

The approved Proposal 0.15 architecture is frozen. Patch-first implementation must modify the smallest canonical source/test surface necessary to realize it, recursively audit until one complete clean pass, and defer all machine-authority claims until native user validation.
