# H1 Patch 0016 — Synchronized Causal Cycle

Status: **BLUEPRINT PROPOSAL 0.5 — RECURSIVELY AUDITED; DIRECTOR APPROVAL REQUIRED; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-05

Authoritative parent `main`: `99e0b9fc7fc346a7276928df8f92a6e133f58c2f`

Latest executable authority: H1 Patch 0015 — Proposal 0.15

Supersedes only the unapproved Patch 0016 blueprint branches based on the earlier `5186b0ab...` checkpoint. No approved implementation is superseded.

## 1. Falsification and purpose

**This patch is unnecessary if current production source already exposes one canonical authority that validates and owns the synchronized Production/accepted-history/Opportunity state and advances the accepted causal cycle without test-local manual stitching.**

It does not. Patch 0015 proves every lower transition, while `Patch0015TestSupport.RunTurn(...)` manually sequences checkpoint, Context, Accepted Take binding, causal commit, accepted-history advancement, Opportunity establishment, and accepted-history/Opportunity coupling.

Patch 0016 closes only that seam.

It creates a pure immutable two-phase cycle:

```text
Opportunity-bearing synchronized state
  -> exact bounded Context
  -> externally prepared Accepted Take
  -> commit + accepted-history proof
  -> validated postcommit state
  -> Opportunity establishment + history coupling
  -> next Opportunity-bearing synchronized state
```

The split after causal commit is intentional. Accepted Performance + approved consequences are already authoritative after the first phase. A later deterministic Opportunity failure must stop the run; it must not erase or fictionalize the accepted commit.

## 2. Architectural fit

Patch 0016 directly protects the five engineering properties:

1. **Character continuity** — accepted Character-legible history advances only through existing causal authority.
2. **Bounded perspective** — Context still flows through deterministic Access Control before Context composition.
3. **Agency without hidden authorship** — the Director still chooses only Opportunity; this layer cannot write Performance or world truth.
4. **Causal persistence** — Accepted Take and approved consequences remain atomic, attributable, immutable causal history.
5. **Creator sovereignty** — this layer never chooses Take disposition, review policy, mutations, spend, retry, or provider behavior.

New namespace:

```text
Ensemble.E0.Core.Cycle
```

This is a narrow deterministic causal aggregate, not the future Application run/Scene state machine. No lower namespace depends upward on `Cycle`.

## 3. Exact public surface

Exactly five new public types:

```csharp
public sealed class E0OpportunityBearingCycleState
public sealed class E0PostCommitCycleState
public sealed class E0OpportunityBearingCycleResult
public static class DeterministicE0CausalCycle
public sealed class E0CausalCycleException : Exception
```

### `E0OpportunityBearingCycleState`

Public read-only:

```text
ProductionState : ProductionState
```

Internal only:

```text
AcceptedPerformanceHistory : E0AcceptedPerformanceHistory
OpportunityHistory         : E0OpportunityHistory
```

No public constructor, setters, or declared instance methods.

### `E0PostCommitCycleState`

Public read-only:

```text
ProductionState : ProductionState
Commit          : E0CausalCommit
```

Internal only:

```text
AcceptedPerformanceHistory : E0AcceptedPerformanceHistory
SourceContext               : ContextPacket
SourceOpportunityHistory    : E0OpportunityHistory
```

No public constructor, setters, or declared instance methods.

### `E0OpportunityBearingCycleResult`

Public read-only:

```text
State              : E0OpportunityBearingCycleState
OpportunityEvent   : E0OpportunityTransition
DirectorEvaluation : LeastInterventionDirectorEvaluation
```

No public constructor, setters, or declared instance methods.

### `DeterministicE0CausalCycle`

Exactly four public static methods:

```csharp
E0OpportunityBearingCycleState Initialize(ProductionState genesisState);

E0ProductionContextContinuityResult ComposeContext(
    E0OpportunityBearingCycleState source);

E0PostCommitCycleState CommitAcceptedTake(
    CommitId commitId,
    E0OpportunityBearingCycleState source,
    ContextPacket sourceContext,
    E0Take acceptedTake,
    E0RecordMaterializationSet materializations);

E0OpportunityBearingCycleResult EstablishOpportunity(
    E0PostCommitCycleState source);
```

`E0CausalCycleException` is public/catchable with no public constructor. One private/internal invariant helper or exception may exist if implementation needs context-neutral factory validation; it must not create another public failure hierarchy.

## 4. Opportunity-bearing invariant

A cycle state is valid only when existing lower authorities plus cross-object checks prove:

- Production StateHash/Scene/current Opportunity are initialized;
- current Opportunity belongs to the E0 Scene roster;
- accepted Performance history is synchronized to exact Scene, StateHash, and roster;
- Opportunity history Scene and `LastOpportunityStateHash` equal Production;
- Opportunity history is nonempty and ends with current Opportunity;
- if `H` is accepted-history count and `O` is Opportunity-history count:

```text
O == H + 1
```

Genesis is therefore `H=0, O=1`.

No new StateHash algorithm, persistent cycle identity, or mutable singleton is introduced.

## 5. `Initialize`

`Initialize(genesisState)`:

1. requires a non-null exact genesis Production;
2. initializes accepted Performance history with existing Continuity authority;
3. initializes Opportunity history with existing Opportunity authority;
4. validates the aggregate invariant above;
5. returns the closed immutable Opportunity-bearing token.

The bounded duplicate genesis validation already performed by the two existing initializers is accepted for E0 rather than modifying prior authorities for micro-optimization.

## 6. `ComposeContext`

`ComposeContext(source)`:

1. captures a fresh `ProductionStateCheckpoint`;
2. calls `E0ProductionContextContinuity.ComposeWithAcceptedHistory(...)`;
3. returns the existing continuity result unchanged.

This preserves the constitutional order:

```text
Production -> Access Control -> permitted projection -> Context Composer
```

Genesis remains exact Context v2. Synchronized evolved state remains exact Context v3. No Context schema/render/hash changes.

## 7. `CommitAcceptedTake` — first adoption boundary

The method:

1. validates required inputs;
2. captures the source checkpoint;
3. calls `E0TakeStateBinding.BindWithAcceptedHistory(...)`;
4. calls `DeterministicCausalCommit.Commit(...)`;
5. keeps the commit result local/staged;
6. calls `E0AcceptedPerformanceHistoryContinuity.RecordCommit(...)`;
7. creates a closed postcommit token only after all preceding proof succeeds;
8. returns that token.

The postcommit token must prove:

- Production has no current Opportunity;
- commit result StateHash equals Production StateHash;
- exact CommitId and TakeId are effective in Production;
- Take is Accepted;
- accepted history is synchronized to postcommit Production and ends with the committed Performance subject + exact VisibleText;
- retained source Opportunity history belongs to the same Scene, ends at the committed subject, and its last StateHash is the commit parent;
- accepted-history count equals retained source Opportunity-history count;
- retained source Context belongs to the commit parent StateHash/Scene and exact committed Performance Context/subject/opportunity/roster.

The retained Context is not re-rendered again here: `BindWithAcceptedHistory` already performed exact fresh structured + rendered source proof before commit.

A Rejected or Alternate Take cannot enter this method successfully and cannot mutate Production or either history.

## 8. `EstablishOpportunity` — second adoption boundary

The method:

1. calls `DeterministicOpportunityAuthority.Establish(...)` using the validated postcommit token;
2. keeps that result local/staged;
3. calls `E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(...)`;
4. constructs the next closed Opportunity-bearing token;
5. re-proves `O == H + 1` and exact state/history synchronization;
6. returns the successor plus exact Opportunity event and Director evaluation.

If this phase fails, the already valid postcommit token remains authoritative. The failure is a hard deterministic system/integrity failure: future run orchestration must stop/fail closed and record diagnostics. It may not invent Character behavior, silently request another fictional Take, or fabricate a next Opportunity.

## 9. Purity and authority

All operations are synchronous pure transitions over explicit immutable inputs. Source tokens are never mutated.

The caller may calculate more than one pure successor from a source, but Patch 0016 creates no branch/canon authority. The future E0 runner owns one current token and explicitly adopts one successful successor at each boundary.

No TakeId, CommitId, RecordId, RunId, or AttemptId allocation is added. Existing explicit IDs/materializations remain inputs.

No model/provider name enters Core.

## 10. Failure and privacy

Public stage failures use fixed structural messages only:

```text
E0 causal cycle initialization failed.
E0 causal cycle Context composition failed.
E0 causal cycle accepted Take commit failed.
E0 causal cycle Opportunity establishment failed.
```

Catch only expected lower domain failures reachable from the called authority. Do not blanket-wrap `Exception`.

No public error text may include VisibleText, Character-private Context prose, mutation prose, imported/user/provider payload, credentials, or secrets. Any retained inner-exception chain must be audited for the same rule.

## 11. Expected implementation surface

Default source additions:

```text
src/Ensemble.E0.Core/Cycle/
  E0CausalCycleModels.cs
  DeterministicE0CausalCycle.cs
```

Default tests:

```text
tests/Ensemble.E0.Core.Tests/Cycle/
  E0CausalCycleContractTests.cs
  E0CausalCycleTests.cs
  E0CausalCycleDeterminismTests.cs
```

Expected edits to Patch 0015 production source: **zero**.

Expected Harness edits: **zero**.

If implementation requires changing semantics in Context, Take, CausalCommit, Opportunity, State Authority, or accepted-history Continuity, stop and reopen this blueprint rather than widening the patch.

## 12. Required tests

The gating suite must prove:

- exact five-type public surface and exact method/property shapes;
- closed constructors/setters and no provider/platform/persistence surface;
- exact Missing Raft genesis initialization;
- genesis Context v2 oracle unchanged;
- first accepted live Take yields the frozen Patch 0015 postcommit StateHash;
- phase one leaves no current Opportunity and advances accepted history exactly once;
- source Opportunity history does not advance in phase one;
- Rejected/Alternate Takes cannot commit;
- stale/tampered Context or mixed otherwise-valid state/history components fail closed;
- first Opportunity remains the frozen MARLOWE route and frozen Opportunity StateHash;
- phase two appends no Performance and advances Opportunity history exactly once;
- next Context remains exact MARLOWE v3 oracle;
- multi-turn recurrence/order and semantic silence remain inherited;
- source tokens remain unchanged;
- repeated identical calls produce equivalent existing canonical identities;
- deterministic phase-two failure cannot mutate/erase the valid postcommit predecessor;
- no lower reference-oracle assertion is removed or weakened.

Public-surface reflection is allowed. Private/IL/call-graph assertions are not required for this patch.

## 13. Explicit non-scope

No provider/model invocation; request/attempt/result DTO; retries/spend; streaming; cancellation/refusal/timeout/backoff; understudy; Candidate generation; model-assisted validation/interpretation; Take review UX; non-Accepted retry policy; full Scene/run loop; run termination/budgets; persistence/recovery; cross-Scene history; Observation/CharacterClaim expansion; World Resolver; branch/retcon/rehearsal; WinUI; Windows AI/NPU; App Actions/MCP; MSIX/WACK/Store.

Those remain downstream until they have an architectural reason to exist.

## 14. Canonical compatibility

Patch 0016 introduces no new canonicalizer, persistent event, Production field, Context version, or hash algorithm.

Frozen Patch 0015 identities must remain byte-for-byte exact, including:

```text
postcommit StateHash
a7e6e1d5e386b2f6b459f8432b85ee7b14c3b25dbfec6e228240f98e523fe96c

Opportunity StateHash
e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151

MARLOWE v3 StructuredContextHash
ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f
```

## 15. Why this is the smallest next boundary

Provider/attempt provenance first would have no single canonical causal target. A full E0 runner would conflate technical execution with fictional authority. A one-shot Accepted-Take-to-next-Opportunity operation would erase the valid postcommit adoption boundary. A mutable coordinator would make illegal phases expressible.

This two-state cycle makes the causal boundary explicit while leaving technical execution outside it.

After implementation and native validation, the next architecture question is whether the remaining H1 seam is the minimal provider-neutral technical attempt/result + cancellation/failure contract needed by the E0-A run driver.

## 16. Implementation handoff after approval

Only after explicit Director approval:

1. create an implementation branch from the then-current approved parent;
2. implement patch-first on the source/test surface above;
3. preserve all existing reference-oracle values;
4. perform static/targeted tests available in the implementation environment;
5. request native Windows ARM64 build/test evidence only at the machine gate;
6. recursively audit correctness -> consistency -> authority -> privacy -> dependency direction -> scope -> tests/oracles -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence;
7. stop before provider execution or the next unapproved architecture boundary.

No separate handoff document is to be created.

## 17. Recursive audit result

Fresh review against current `main`, Blueprint 0.1, Ship Plan 0.7, Patch 0015 source, and the five engineering properties found:

```text
0 material correctness corrections outstanding
0 authority/adoption contradictions
0 dependency-direction violations
0 Character/Performer conflation
0 Access-before-Context violations
0 causal atomicity violations
0 creator-authority violations
0 canonical/version changes
0 provider/platform scope leaks
0 worthwhile in-scope public-surface simplifications
```

Implementation remains forbidden until explicit Director approval.
