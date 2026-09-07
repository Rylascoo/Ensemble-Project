# H1 Patch 0015 — Static Implementation Audit

Status: STATIC IMPLEMENTATION CLOSED — NATIVE WINDOWS ARM64 VALIDATION NOT YET RUN
Date: 2026-09-04

Patch: `H1 Patch 0015 — E0 Accepted Performance History + Context Continuity`
Approved architecture: Proposal `0.15`

## Authority checkpoints

Promoted parent `main` checkpoint:

```text
7475a9397cff9063673908c666a729f0f3cd4525
```

Approved Proposal 0.15 audited blueprint head:

```text
cea5820b9614cf9028340a23cf931696feb98680
```

Blueprint audit evidence:

```text
521bcc197c5fd348936deb540d12f2012cf8241e
```

Blueprint approval evidence:

```text
81b27ade5fb5fd7224ce1709ddfcf01615f525ff
```

Implementation handoff checkpoint:

```text
146f5a9bc483651c2e8bf7fe7106422e17a6679c
```

Exact source/test implementation head audited here:

```text
69dac983d8bb0663eda24e3b988626517f7d6218
```

This SHA is static source/test authority only. It has not yet been compiled or tested on the user's native Windows ARM64 machine.

## Implementation surface

### Added production files

```text
src/Ensemble.E0.Core/Domain/CharacterLegibleTextInvariants.cs
src/Ensemble.E0.Core/CausalCommit/AcceptedPerformanceHistory.cs
src/Ensemble.E0.Core/CausalCommit/E0TakeStateBinding.AcceptedHistory.cs
src/Ensemble.E0.Core/Continuity/E0AcceptedPerformanceHistoryContinuity.cs
```

### Modified production files

```text
src/Ensemble.E0.Core/Performer/PerformerCandidateContract.cs
src/Ensemble.E0.Core/Context/ContextModels.cs
src/Ensemble.E0.Core/Context/ContextPacketCanonicalizer.cs
src/Ensemble.E0.Core/Context/DeterministicContextComposer.cs
src/Ensemble.E0.Core/CausalCommit/CausalCommitModels.cs
src/Ensemble.E0.Core/Continuity/E0ProductionContextContinuity.cs
```

No Production transition/canonicalizer, Opportunity authority/canonicalizer, Integrity, State Interpreter, State Authority, Take semantics, fixture, Access, Harness, WinUI, provider, AI, packaging, or Store source was modified.

### Added focused tests

```text
tests/Ensemble.E0.Core.Tests/Continuity/Patch0015TestSupport.cs
tests/Ensemble.E0.Core.Tests/Continuity/AcceptedPerformanceHistoryTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/AcceptedHistoryContextTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/AcceptedHistoryBindingTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/Patch0015ContractAuditTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/Patch0015DeterminismTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/Patch0015MultiTurnRegressionTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/Patch0015ReferenceOracleTests.cs
```

### Narrow inherited-test evolution

Exactly the identified later-surface assumptions were evolved:

```text
tests/Ensemble.E0.Core.Tests/CausalCommit/Patch0012ContractAuditTests.cs
tests/Ensemble.E0.Core.Tests/Context/DeterministicContextComposerTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/Patch0014ContractAuditTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/ProductionContextBindingTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/ProductionContextContinuityTests.cs
```

The changes are limited to accepting the approved opaque history type, recent semantic DTO/property, new version constants/methods, and the extra internal ContextPacket constructor field. Historical semantic/hash expectations were not loosened.

## Architecture conformance

### Dependency direction

Implemented direction remains:

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
    -> opaque accepted-history data in CausalCommit

opaque accepted-history data
    -> history-aware Take proof in CausalCommit

CausalCommit + Opportunity + Context continuity
    -> accepted-history advancement in Continuity

opaque accepted history
    -> history-aware Production Context continuity
```

Verified statically:

- Context does not depend on Performer, CausalCommit, Opportunity, or Continuity;
- CausalCommit history data/proof does not depend on Opportunity or Continuity;
- Opportunity source remains unchanged and depends on CausalCommit, never reverse;
- Production source remains unchanged;
- the neutral Domain text invariant depends only on BCL Unicode/text primitives.

## Neutral Character-legible text law

`CharacterLegibleTextInvariants` is internal-only and preserves the exact Patch 0006 grammar:

- null invalid;
- empty string valid semantic silence;
- UTF-16 surrogate structure validated;
- nonempty text must already be Unicode NFC;
- Control characters forbidden except TAB U+0009 and LF U+000A;
- nonempty text must contain at least one display-bearing Unicode scalar under the inherited category rule.

`PerformerCandidateContract` delegates to this helper and preserves the existing externally tested Performer exception messages exactly for every failure class.

Context/history paths reuse the same invariant and normalize failures into their own sanitized exception domains. No second handwritten visible-text grammar exists.

## Opaque accepted Performance history

`E0AcceptedPerformanceHistory` has:

```text
public constructors = 0
public setters = 0
public properties = 0
public methods declared on type = 0
```

Internal state is exactly:

```text
SceneId
CurrentStateHash
Entries : ImmutableArray<ContextRecentPerformance>
```

The token remains an immutable in-memory synchronization/projection token, not an event store, transcript repository, durable history, or authority source.

The internal invariant/projector validates initialized Scene/State identity, initialized/non-null entries, exact shared visible-text grammar, and exact current-roster membership where a Context projection is requested.

## Character-safe history semantics

`ContextRecentPerformance` exposes exactly:

```text
SourceCharacterId
VisibleText
```

It exposes no:

- CommitId;
- TakeId;
- parent/result StateHash;
- ContextPacketId;
- Candidate hash/identity contract;
- typed address or nomination control;
- State Authority data;
- materialization/provenance data;
- provider/model data;
- Observation/Knowledge/Belief/Suspicion/Memory/Claim classification.

Accepted Performance remains historical occurrence, not truth or epistemic state.

CharacterClaim disclosure remains unchanged and denied by inherited Production Access law.

## Context v1/v2/v3 compatibility

Historical constants remain exact.

Added exactly:

```text
AcceptedHistorySchemaVersion
ensemble.e0.context.v3

AcceptedHistoryCompositionContract
ensemble.e0.context.production-bound.accepted-history.v1

AcceptedHistoryRenderingContract
ensemble.e0.context.render.v2
```

Closed packet matrix is implemented:

```text
v1 + full-authorized.v1 + render.v1
    SourceStateHash = null
    RecentPerformances initialized empty
    RecentPerformanceText = ""

v2 + production-bound.v1 + render.v1
    SourceStateHash initialized
    RecentPerformances initialized empty
    RecentPerformanceText = ""

v3 + production-bound.accepted-history.v1 + render.v2
    SourceStateHash initialized
    RecentPerformances initialized nonempty
    RecentPerformanceText nonempty
```

Default/uninitialized recent arrays and schema/composition/render/history hybrids fail closed.

Each v3 recent source Character must resolve exactly once in the packet roster.

Historical public composer remains the sole public `DeterministicContextComposer` method. Production-bound v2 and accepted-history v3 composers remain internal.

## Context v3 canonicalization/rendering

Structured v3 preserves the exact v2 root order and retains `recentPerformances` as the final root property.

Recent items preserve accepted append order and serialize exactly:

```text
sourceCharacterId
visibleText
```

No sorting or deduplication occurs.

Rendering preserves inherited trusted-state and Opportunity algorithms. Only the recent Performance layer changes under render-v2.

Non-silent form:

```text
[RECENT PERFORMANCES]
<display name>:
[PERFORMANCE]
- <first line>
  <continuation line>
```

Semantic silence form:

```text
<display name>:
[PERFORMANCE: SILENCE]
```

Literal `[PERFORMANCE: SILENCE]` remains ordinary non-silent text and has a distinct structured/rendered identity.

## History-aware Context continuity

`ComposeWithAcceptedHistory(checkpoint, history)`:

- validates checkpoint/source association;
- validates history Scene/State synchronization;
- evaluates Production Access exactly once;
- exact empty history is accepted only at recomputed exact genesis and emits historical v2 unchanged;
- nonempty history is rejected at exact genesis;
- synchronized nonempty non-genesis history emits v3;
- Packet and Trace SourceStateHash must equal the checkpoint StateHash;
- expected history failures normalize to `E0ContextContinuityException`.

Historical `Compose(checkpoint)` remains exact Patch 0014 behavior and may emit evolved v2 as compatibility output.

## History-aware Take proof

`BindWithAcceptedHistory(...)` reuses the inherited shared binding path rather than copying Take/Scene/roster/StateAuthority proof logic.

It adds only the approved history-aware source-Context proof:

```text
empty synchronized exact genesis -> exact fresh v2 required
nonempty synchronized non-genesis -> exact fresh v3 required
v1 -> reject
evolved v2 -> reject
```

For v3 it freshly evaluates Production Access, freshly recomposes expected v3, and requires exact:

- canonical structured bytes;
- canonical rendered bytes;
- ContextPacketId;
- StructuredContextHash;
- RenderedContextHash;
- SourceStateHash;
- Scene/subject/opportunity/roster associations;
- inherited exact State Authority snapshot equivalence.

Regression coverage now directly rejects stale history plus tampered v3 recent semantics, rendered content, structured hash, and rendered hash.

Historical three-argument `Bind(...)` remains exact and rejects v3.

## Commit advancement

`RecordCommit(...)` follows the frozen order:

- closed history/state validation;
- parent checkpoint capture;
- fresh history-aware semantic Context composition;
- exact committed Performance subject/ContextPacketId proof;
- canonical `DeterministicCausalCommit.Replay(...)`;
- require replay current opportunity null;
- append exactly one source Character + exact VisibleText;
- advance only history state anchor;
- return a new token without mutating the source.

The implementation does not overclaim postcommit rendered proof. Full structured+rendered source proof remains owned by precommit `BindWithAcceptedHistory`.

Tests cover stale history/wrong-parent event rejection, historical history-omitting v1-source commit rejection, zero-mutation accepted commits, and Accepted Takes whose durable consequence is entirely hard-rejected while the Character-legible Performance still appends.

## Opportunity advancement

`RecordOpportunity(...)`:

- requires synchronized nonempty history and exact postcommit hash;
- requires last semantic item to match source committed Performance subject/text;
- enforces pre-transition routing count == accepted Performance count;
- calls canonical `DeterministicOpportunityAuthority.Replay(...)`;
- requires result routing count == accepted history count + 1;
- requires last route == selected Character == result current opportunity;
- appends no Performance;
- advances only history CurrentStateHash;
- returns a new token.

Tests cover successful count induction plus wrong routing history and foreign event rejection while preserving the prior postcommit history token unchanged.

## Multi-turn continuity

Focused regressions prove:

- accepted history preserves exact append order across multiple commits/opportunities;
- earlier zero-durable-consequence Performance text survives;
- identical repeated semantic items remain separate entries;
- routing recurrence `VOSS -> MARLOWE -> WREN -> VOSS` is preserved;
- when VOSS receives Opportunity again, its earlier VOSS Performance is still present in source Context;
- a later history can contain VOSS twice without deduplication;
- no provider-session memory is required.

## Independent live reference oracle

Evidence:

```text
docs/evidence/H1_PATCH_0015_REFERENCE_ORACLE.md
```

The independent derivation first reproduced established references exactly:

```text
Genesis StateHash
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104

Historical Patch 0012 postcommit
057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30

Historical Patch 0013 Opportunity
dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310

Historical Patch 0014 evolved v2 Context
structured 3456 bytes
9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00
rendered 2389 bytes
9925327709d4a99697d4b65e5ad870e6e3ff5c5d573e25c61814e48d77163a2d
```

Only then were Patch 0015 values accepted:

```text
Genesis VOSS v2 Context
structured 2655 bytes
27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
rendered 1905 bytes
ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88

v2-source CandidateContentHash
6ce2a98dc6208400126feba1a356910376d40c6155843764ecf673b8b9441ec1

v2-source ProposalContentHash
ac0f7a91c855b99fdb6f1b9415ff150352e8e3f79268e666d38531323b7d16c3

live postcommit StateHash
a7e6e1d5e386b2f6b459f8432b85ee7b14c3b25dbfec6e228240f98e523fe96c

live MARLOWE Opportunity StateHash
e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151

first MARLOWE v3 Context
structured 3521 bytes
ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f
rendered 2443 bytes
668c632ebdb4e4a2de838cbc5ae49b17005984880345ed28ec0c6eaa2bfcef16
```

Executable oracle coverage also proves the new live MARLOWE Opportunity Production projection bytes are exactly equal to the historical Patch 0013 oracle MARLOWE projection bytes while the StateHashes remain distinct due solely to lineage identity.

## Determinism

Focused tests prove:

- repeated history-aware v3 composition is byte-identical;
- v3 composition is invariant under `ar-SA` CurrentCulture/CurrentUICulture;
- accepted recent-history order is intentionally structured-byte significant;
- inherited canonical invariance for unrelated source collections remains covered by historical Context regression tests;
- fixed independent v3 structured/rendered byte/hash oracles are pinned.

## Scope / resource suitability

No provider/model call, network, filesystem persistence, clock, random source, Task/thread/timer, GPU, NPU, QNN, ONNX, Windows API, WinUI, package, or Store dependency was added to the Patch 0015 public surface.

Expected deterministic work remains bounded by the approved E0 model:

```text
history validation        O(H + P)
history-aware Context     O(R + A log A + B + H + P)
history-aware Take proof  fresh history-aware Context + inherited State Authority proof
RecordCommit              fresh history-aware Context + canonical commit replay + immutable append
RecordOpportunity         canonical Opportunity replay + O(H) history validation/coupling
```

`ImmutableArray` append copies O(H) items/references. Repeated complete-Scene history rendering may make total long-Scene work superlinear. This is the explicitly accepted unoptimized E0 reference behavior, not a retail battery/performance claim.

No idle/background work is introduced, so the design remains suitable for later ARM64-native integration without forcing CPU/GPU/NPU activity while idle. Device battery/performance behavior is not claimed without profiling.

## Recursive audit corrections

Every material correction restarted the audit from correctness.

Corrections/improvements applied during implementation review included:

1. Access/Context source hardening already frozen during the implementation sequence so forged v3 recent sources must resolve exactly once in roster and v3 binding fails safely on missing rendered content.
2. Exact Proposal 0.15 oracle harness correction: live reference branch now reuses `TAKE-PATCH-0012-ORACLE`, `COMMIT-PATCH-0012-ORACLE`, and `PRESSURE-PATCH-0012-ORACLE` instead of invented Patch 0015 IDs.
3. Independent live oracle derivation with inherited digest self-checks before freezing any new hash.
4. Direct stale-history and structured/rendered/hash tamper regressions at `BindWithAcceptedHistory`.
5. Direct v3 repeated-byte and culture-invariance regressions.
6. Direct stale/wrong-parent `RecordCommit` and wrong-routing/foreign-event `RecordOpportunity` regressions.
7. Explicit Accepted-Take/all-consequences-rejected history append regression.
8. Executable proof that live and historical MARLOWE Opportunity Production projection bytes are identical while lineage StateHashes differ.
9. Explicit repeated-item, Character recurrence, and self-history multi-turn regression.

## Final recursive pass

The final pass was performed in this order:

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

Result:

```text
material corrections remaining: 0
worthwhile improvements remaining within approved Patch 0015 scope: 0
```

No architecture redesign was required.

## Validation boundary

This static audit is advisory source/test evidence only.

No local .NET compiler was available in the assistant execution environment. An attempted read-only repository checkout was also blocked by container DNS, so no substitute compile claim is made.

The next authority gate is the user's native Windows ARM64 machine at an exact implementation/evidence checkpoint. Until that gate passes, Patch 0014 remains the latest machine-validated Core/Harness authority.
