# H1 Patch 0016 — Synchronized Causal Advancement — Blueprint Audit

Status: **RECURSIVE ADVERSARIAL AUDIT COMPLETE — DIRECTOR APPROVAL REQUIRED; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-04

Canonical blueprint:

`docs/blueprint/H1_PATCH_0016_SYNCHRONIZED_CAUSAL_ADVANCEMENT.md`

Audited proposal:

`0.5`

Exact audited proposal commit:

`dd42e48d8c4b3450466c236ebc9f289279f3aad0`

Exact audited blueprint content SHA:

`3f6fecfcb9da76b7a8a4279d93038c234b57ebd6`

Authoritative parent `main` at audit closure:

`99e0b9fc7fc346a7276928df8f92a6e133f58c2f`

Architecture branch:

`h1-patch-0016-accepted-take-transition-blueprint`

---

## 1. Audit purpose

Determine whether Proposal 0.5 is the smallest coherent deterministic boundary immediately above Patch 0015 and whether it can be implemented by composing existing authorities without changing fictional semantics, canonical identities, provider policy, persistence, run orchestration, platform code, or Store scope.

This evidence is architecture/static authority only. It does not claim compiler, runtime, ARM64 execution, Harness execution, NPU execution, package validation, WACK, or Store certification.

---

## 2. Authority reconciled

The audit checked Proposal 0.5 against:

- current `main` / `CURRENT_STATE.md` checkpoint;
- Director-approved `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md` Proposal 0.7;
- `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` v0.2;
- frozen Ensemble Blueprint 0.1 laws carried through the H1 patch series;
- Patch 0011 Take semantics;
- Patch 0012 atomic causal commit/state binding;
- Patch 0013 effective Opportunity authority;
- Patch 0014 Production-bound Context continuity;
- Patch 0015 accepted Performance history + Context continuity;
- Patch 0015 reference-oracle and native-validation evidence;
- current `ProductionState`, `E0Take`, `E0TakeStateBinding`, `DeterministicCausalCommit`, `E0OpportunityHistory`, `DeterministicOpportunityAuthority`, `E0AcceptedPerformanceHistory`, accepted-history Continuity, and Production Context Continuity source;
- current Patch 0015 live test support and structural/reflection test patterns;
- Engineering Hygiene Constitution checkpoint requirements.

No legacy donor code or Windows/platform API is needed by this patch.

---

## 3. Next-boundary decision

Three candidate directions were compared:

1. provider-attempt/provenance first;
2. one-shot Accepted-Take-to-next-Opportunity wrapper;
3. synchronized deterministic causal advancement with two explicit phase states.

The third is selected.

Provider-attempt architecture first would define technical execution before there is one canonical post-Accepted-Take causal target.

A one-shot wrapper is invalid because Patch 0015 already freezes two adoption boundaries:

```text
Commit + RecordCommit
 -> valid postcommit Production/accepted-history successor

Opportunity Establish + RecordOpportunity
 -> valid next Opportunity-bearing synchronized successor
```

A later Opportunity failure cannot erase a valid accepted causal commit.

The selected Patch 0016 state machine therefore is:

```text
E0OpportunityBearingCycleState
    -- CommitAcceptedTake -->
E0PostCommitCycleState
    -- EstablishOpportunity -->
E0OpportunityBearingCycleState
```

---

## 4. Parallel exploratory-branch reconciliation

The audit inspected the non-authoritative Patch 0016 explorations:

- `h1-patch-0016-accepted-take-advancement-blueprint`;
- `h1-patch-0016-synchronized-causal-cycle-blueprint`;
- earlier one-shot state on the current architecture branch.

Useful work was synthesized rather than discarded:

- two-boundary type-state model retained;
- `Advancement` chosen over Core `Orchestration` so future Application-layer Scene/run orchestration remains distinct;
- exact supplied source Context retained as evidence;
- no new event/hash/version/replay authority;
- caller-supplied CommitId/materializations retained;
- structural delegation tests retained.

One-shot all-or-nothing advancement and raw final lower Opportunity-state exposure are superseded.

At audit closure the canonical branch is based directly on current `main`; unrelated workflow/promotion changes are not part of the Patch 0016 diff.

---

## 5. Existing source proves implementability

The current source already provides every lower operation required.

`Patch0015TestSupport.RunTurn(...)` manually composes the exact sequence Patch 0016 will own, demonstrating the seam without serving as production authority.

Existing lower owners include:

- `ProductionStateCheckpoint.Capture`;
- `E0ProductionContextContinuity.ComposeWithAcceptedHistory`;
- `E0TakeStateBinding.BindWithAcceptedHistory`;
- `DeterministicCausalCommit.Commit`;
- `E0AcceptedPerformanceHistoryContinuity.RecordCommit`;
- `DeterministicOpportunityAuthority.Establish`;
- `E0AcceptedPerformanceHistoryContinuity.RecordOpportunity`.

Current `ProductionState` also already exposes internal effective-Commit/committed-Take identity checks needed by the postcommit aggregate invariant, while existing accepted-history and Opportunity invariants provide the required synchronization/roster checks.

No lower public API needs widening.

---

## 6. Exact public surface audit

Proposal 0.5 adds exactly five public `Ensemble.E0.Core.Advancement` types:

```text
E0OpportunityBearingCycleState
E0PostCommitCycleState
E0OpportunityBearingCycleResult
DeterministicE0CausalAdvancement
E0CausalAdvancementException
```

The surface is minimal for the approved purpose:

- two closed phase capability values preserve the two adoption boundaries;
- the final result exposes only synchronized state + Opportunity event + Director evaluation;
- public Commit on the postcommit token preserves first-boundary causal evidence;
- histories remain hidden so they are not independently mixable through the supported Advancement surface;
- Director evaluation is needed for later E0 provenance and adds no new prose disclosure;
- no raw `E0OpportunityTransitionResult` is returned;
- no new status/failure enum, replay API, canonicalizer, allocator, event, hash, schema/version or provider interface is added.

No current `Ensemble.E0.Core.Advancement` namespace exists on `main`.

---

## 7. Synchronization/type-state audit

Opportunity-bearing aggregate law:

```text
O == H + 1
```

where H is accepted Performance-history count and O is Opportunity-history count.

The aggregate additionally binds Scene, current StateHash, current Opportunity, roster and final Opportunity-history Character.

Postcommit aggregate law:

```text
OH == H
```

and additionally proves:

- no current Opportunity;
- commit result hash == postcommit StateHash;
- effective CommitId + committed TakeId are present in Production;
- accepted history is synchronized and ends at the committed Performance;
- retained source Opportunity history binds to commit parent and committed subject;
- retained exact source Context binds to commit parent, Scene, Candidate Context identity and subject/opportunity.

No canonical hash recomputation is needed at the aggregate layer.

Closed construction makes invalid phase combinations unavailable to supported public callers without adding a public Bind/Rebind test seam.

---

## 8. Source Context proof audit

Patch 0016 must preserve the exact supplied Context artifact that travelled through Candidate -> Integrity -> Interpreter -> State Authority -> Take.

`CommitAcceptedTake(...)` passes that exact packet to `BindWithAcceptedHistory(...)`.

It may not silently substitute a newly recomposed Context.

The inherited Patch 0015 binder remains the full structured+rendered source proof for live v2/v3 Context. The postcommit aggregate performs only narrow retained-artifact coupling and does not reserialize/recompose it.

This preserves the future provider-provenance seam without entering provider scope now.

---

## 9. Adoption/atomicity audit

Phase one does not expose a Patch 0016 successor until:

```text
BindWithAcceptedHistory
 -> Commit
 -> RecordCommit
 -> validated postcommit aggregate construction
```

Phase two does not expose a next Opportunity-bearing successor until:

```text
Opportunity Establish
 -> RecordOpportunity
 -> validated final aggregate construction
```

No Opportunity is established in phase one.

No causal Commit/Bind is repeated in phase two.

The already-returned postcommit token is immutable and remains a valid accepted causal successor even if later phase-two execution encounters an integrity/system failure.

No rollback semantics are invented.

---

## 10. Failure/testability correction

Proposal 0.4 removed a test requirement that would have required artificial fault injection or private-state corruption solely to force phase-two failure from a supported valid postcommit token.

Current E0 lower invariants are intended to make phase two deterministic/total for a valid closed token. A lower failure from such a token is an integrity/system defect, not Character behavior, an Alternate Take, or fictional retry policy.

Proposal 0.5 then corrected the remaining public null-dereference hazard:

- null required phase/reference inputs are normalized to fixed stage-specific `E0CausalAdvancementException` messages before token dereference;
- nonnull semantic inputs remain lower-authority responsibility;
- no broad `catch (Exception)` is permitted.

The failure/adoption law is tested through closed construction, stage normalization, structural delegation and immutable/pure-transition behavior rather than manufactured impossible runtime states.

---

## 11. Failure/privacy audit

Public messages are fixed:

```text
E0 causal advancement initialization failed.
E0 causal advancement Context composition failed.
E0 causal advancement accepted Take commit failed.
E0 causal advancement Opportunity establishment failed.
```

They contain no Candidate VisibleText, rendered/private Context, mutation/record prose, provider/user/imported payload, credentials or secrets.

Expected lower exceptions are caught narrowly at the stage that invokes them.

An inner chain may be retained only if implementation audit proves its complete reachable messages structural and safe; otherwise Advancement drops the inner chain while preserving the fixed public stage classification.

The returned `LeastInterventionDirectorEvaluation` contains the existing public routing evidence (IDs, control/routing history/rule), not Character prose or provider payload, so Patch 0016 creates no new disclosure channel.

---

## 12. Canonical/replay compatibility

Patch 0016 adds no canonical identity and changes no canonical algorithm.

Existing canonical replay remains owned by CausalCommit and Opportunity.

The exact Patch 0015 oracle remains required unchanged, including:

```text
genesis StateHash
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104

source ContextPacketId
CTX:27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565

CandidateContentHash
6ce2a98dc6208400126feba1a356910376d40c6155843764ecf673b8b9441ec1

ProposalContentHash
ac0f7a91c855b99fdb6f1b9415ff150352e8e3f79268e666d38531323b7d16c3

postcommit StateHash
a7e6e1d5e386b2f6b459f8432b85ee7b14c3b25dbfec6e228240f98e523fe96c

Opportunity StateHash
e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151

selected Character
MARLOWE

next v3 ContextPacketId
CTX:ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f

next rendered Context hash
668c632ebdb4e4a2de838cbc5ae49b17005984880345ed28ec0c6eaa2bfcef16
```

Existing reference-oracle tests already pin these values.

---

## 13. Test strategy audit

Focused Patch 0016 tests are sufficient and do not need to replay every Patch 0012–0015 lower test through the wrapper.

Required coverage includes:

- exact five-type public surface and four method signatures;
- closed token construction/type-state legality;
- fixed null-stage failures;
- exact genesis v2 and evolved v3 Context oracles;
- exact first postcommit and Opportunity hashes;
- accepted-history append/anchor behavior;
- Opportunity-history advancement/count laws;
- rejected/alternate/no-mutation/all-rejected-consequence cases;
- representative source-Context and materialization failures;
- primitive-chain equivalence at both adoption boundaries;
- multi-turn recurrence/repeated Performance/self-history;
- culture/repeated-call determinism;
- exact direct-call structural delegation;
- absence of new replay/hash/version/canonicalizer and duplicate Director/event validation;
- full historical suite regression;
- native ARM64 Core + Harness + fixture gates after implementation.

The repository already contains IL call-inspection logic in `Patch0012StructuralImplementationTests`; a smallest test-only helper extraction is permitted only if it avoids genuine duplication without weakening historical assertions.

No production test seam is required.

---

## 14. Dependency/scope audit

The new `Advancement` layer is above existing deterministic Core authorities and below future Application/provider run orchestration.

It introduces no:

- provider/model/network/stream/retry/spend/cancellation semantics;
- automatic Candidate/Integrity/Interpreter/State Authority/Take creation;
- full Scene/run loop;
- persistence/recovery;
- observation/claim epistemics;
- World Resolver;
- branch/canon/rehearsal;
- WinUI/Windows AI/Windows ML/QNN/NPU;
- App Actions/MCP;
- MSIX/IPackageValidator/WACK/Store.

Patch 0016 remains an intermediate H1 seam. It does not declare H1 complete or trigger the end-of-H1 convergence audit by itself.

---

## 15. ARM64/battery/hygiene audit

The proposed code is synchronous deterministic CPU/memory composition over existing immutable Core objects.

New aggregate construction may validate accepted history in O(H); Opportunity history is not rescanned, only cross-token Scene/hash/count/last facts are checked.

No background work, timer, polling, filesystem, network, provider, Windows API, GPU/NPU, cache or idle wake is introduced.

No measured ARM64/battery claim is made before machine evidence.

Expected implementation is additive to two Core source files under the new Advancement namespace plus focused tests, with zero lower semantic edits by default.

---

## 16. Final recursive pass

After Proposal 0.5, one complete fresh pass reached:

```text
0 material correctness corrections
0 authority/adoption corrections
0 dependency-direction corrections
0 synchronization/type-state corrections
0 source-Context proof corrections
0 failure/privacy corrections
0 canonical/hash/version corrections
0 E0/ship-plan scope corrections
0 worthwhile public-surface simplifications
0 worthwhile test improvements
0 ARM64/battery/hygiene corrections
0 evidence corrections
```

No implementation has started.

No source, test, fixture or Harness file changed during blueprint work.

---

## 17. Approval boundary

Director approval would freeze Proposal 0.5 only for Patch 0016 implementation.

Approval would not authorize provider-attempt/provenance, full run orchestration, E0-A provider execution, persistence, UI, local AI/NPU, or release work.

After approval, create explicit approval evidence and an implementation handoff/branch from the exact approved architecture, then implement patch-first and return to native Windows ARM64 validation at the proper gate.
