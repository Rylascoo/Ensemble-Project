# H1 Deterministic Spine — End-of-Phase Convergence / Deletion Audit

Status: **PHASE A CLOSED — STATIC CONVERGENCE CLEAN; DIRECTOR-MACHINE ARM64 EVIDENCE SATISFIED**

Date: 2026-09-05

Audited `main` baseline: `28d1f66579a98cabfda7e679dc18b0fd93e3d5ba`

Latest executable promotion: H1 Patch 0018 — Deterministic Turn Orchestration Proposal 0.6, squash commit `d85904617c361f34cdb31a10d2e0200e3becc83c`.

## 1. Question

Roadmap Phase A closes only if Ensemble now has one canonical deterministic path from opportunity-bearing Production through attempt handling and accepted causal commit to the next synchronized opportunity-bearing state, without provider SDK execution, with native ARM64 evidence, and after an active deletion/simplification audit.

The audited canonical live path is:

```text
DeterministicE0CausalCycle.Initialize
 -> DeterministicE0CausalCycle.ComposeContext
 -> external Performer attempt semantics
 -> DeterministicE0TurnOrchestrator.GateAttempt
 -> EvaluateIntegrity
 -> external semantic State Interpretation proposal
 -> EvaluateAuthority / ResolveAuthorityReview
 -> BindAcceptedTake
 -> CommitAccepted
 -> valid E0PostCommitCycleState
 -> DeterministicE0CausalCycle.EstablishOpportunity
 -> next E0OpportunityBearingCycleState
```

Technical failure, cancellation, `RequestAnotherTake`, and unresolved authority review stop before causal adoption. They do not create Character behavior or mutate Production.

## 2. Phase-A exit proof

### Canonical deterministic route

Patch 0016 owns synchronized Production/accepted-history/Opportunity progression and deliberately preserves the postcommit adoption boundary. Patch 0017 binds one exact semantic Context to `CandidateReady`, payload-free `TechnicalFailure`, or `Cancelled`. Patch 0018 composes those authorities into the closed Turn state machine while retaining explicit later Opportunity establishment.

Patch 0018 tests prove:

- stale Candidate and stale technical attempt rejection;
- technical/cancelled outcomes mutate nothing and carry no fictional payload;
- Integrity Accept and `RequestAnotherTake` paths;
- exact proposal/policy review continuity;
- terminal State Authority before TakeId/Take construction;
- reference Accepted Take replay;
- rejected consequence mutation does not reject the reference Take;
- accepted commit equivalence with the inherited Cycle authority;
- commit returns valid postcommit state before Opportunity;
- explicit next Opportunity yields the synchronized successor;
- every pre-AcceptedTakeReady Turn state is rejected by commit;
- identical explicit inputs reproduce equivalent commit/successor semantics;
- three accepted turns preserve `VOSS -> MARLOWE -> WREN -> VOSS` Opportunity rotation;
- forged same-state Context identity cannot reenter progression.

### No provider dependency in Core

`Ensemble.E0.Core.csproj` has no provider/package dependency beyond the .NET SDK target. Provider/model/network/request/streaming/retry/spend/runtime execution remains outside Core. The existing Harness still depends only on Core.

### Native ARM64 authority

Patch 0018 was validated on the **Director's native Windows ARM64 machine**, not the engineering assistant environment.

Exact validated checkout: `023b469b801239c2b0d597aec1e7712aa4a8faa7`.

Observed:

```text
Windows 10.0.26200
ARM64
RID win-arm64
SDK 9.0.317
tracked/staged clean
Core compile PASS
Core.Tests compile PASS
622/622 tests PASS
Harness ARM64 build PASS
Missing Raft PASS
generic smoke PASS
```

The last executable/test checkpoint was `7e94614dc484aff8cb9b8e39c1a74a7b04ea3238`; changes through the validated checkout after it were documentation-only.

## 3. Active deletion / simplification audit

The audit searched executable source and Harness for TODO/FIXME/HACK/`NotImplementedException`/obsolete scaffolding and found none.

Candidate surfaces were then tested against actual authority rather than deleted by age alone.

| Candidate | Decision | Reason |
|---|---|---|
| `E0ProductionContextContinuity.Compose(checkpoint)` | RETAIN | Patch 0015 explicitly freezes it as historical regression/compatibility API. It is not the approved Full Ensemble live path. |
| `E0TakeStateBinding.Bind(checkpoint, context, take)` | RETAIN | Same Patch 0015 historical/reference role; nonempty live history requires the history-aware binder. |
| `ComposeWithAcceptedHistory` / `BindWithAcceptedHistory` | RETAIN | Canonical live history-aware proof used by Cycle. |
| `E0AcceptedPerformanceHistoryContinuity` | RETAIN | Independently replays canonical commit/Opportunity authorities and proves Character-legible history synchronization; not duplicated by Turn. |
| `DeterministicE0CausalCycle` | RETAIN | Sole synchronization/adoption owner over Production, accepted history, and Opportunity. |
| `DeterministicE0TurnOrchestrator` | RETAIN | Sole closed Turn progression owner; delegates rather than duplicates lower authorities. |
| `RunId` strong ID | RETAIN | Frozen E0 experimental provenance requires run-attributable evidence; H1 correctly does not inject it into Core Turn/Cycle semantics. |
| `RecordProvenanceGraphValidator` | RETAIN | Shared neutral validator used by fixture, Production, and causal-commit paths. |
| current ARM64 fixture Harness bootstrap | RETAIN | Small Phase-B host edge and current native/runtime validation vehicle; not a competing run driver. |
| old patch implementation handoff documents | DELETE FROM ACTIVE TREE | Superseded transition artifacts; approved blueprints/evidence and Git history preserve authority. Fresh chats should not be routed through stale implementation handoffs. |

No executable type, operation, abstraction, or dependency is currently safe to delete without either violating an approved patch contract or reducing an independently useful proof boundary.

This is a deletion audit, not a deletion quota applied blindly to live authority. The justified deletion is stale handoff documentation; executable deletion would currently make the tree less coherent.

## 4. Authority / dependency audit

The final H1 layering is not duplicate orchestration:

```text
lower deterministic semantic authorities
 -> history / continuity proofs
 -> synchronized Cycle adoption authority
 -> Turn progression authority
 -> future E0-A run/provider host
```

Key boundaries remain intact:

- Character != Performer;
- Access precedes Context and provider use;
- technical failure cannot become fiction;
- State Interpreter proposes; State Authority decides;
- Take construction follows terminal authority;
- accepted Take + consequences adopt atomically at commit;
- postcommit remains authoritative if later Opportunity establishment fails;
- provider/model/provenance/retry/spend remain outside Core;
- accepted Performance history stays separate from durable projected truth;
- deterministic state is reconstructable without provider conversation memory.

## 5. Roadmap maintenance required by this audit

The active roadmap entered this audit with historical Patch 0015/571-test current-position text and a later-superseded mandatory first-release `IPackageValidator` gate.

Maintenance in this same docs-only convergence package must:

1. advance the current-position section through Patch 0018 / Phase-A closure / 622-test Director-machine evidence;
2. mark Phase A closed and Phase B current;
3. remove `IPackageValidator` as a mandatory first-release gate, consistent with the later Director decision that it validates externally staged packages rather than our own exact candidate;
4. preserve package/signing/architecture/clean-install/WACK/Partner Center authority;
5. leave historical Proposal-0.7 audit evidence historical rather than rewriting past research claims.

## 6. Convergence result

After restarting the audit from correctness after each material finding:

```text
0 material executable correctness corrections outstanding
0 duplicate canonical live paths
0 Character/Performer conflations
0 Access-before-Context violations
0 technical-failure fictionalization paths
0 Take/commit/Opportunity adoption collapses
0 stale-context gaps in the H1 live path
0 provider/platform dependencies in Core
0 unjustified executable abstractions identified
0 executable deletions justified without violating approved authority
1 justified active-tree deletion class: superseded implementation handoffs
1 stale roadmap release-gate correction required: IPackageValidator
```

**Roadmap Phase A — Close the H1 deterministic spine: CLOSED.**

The next engineering phase is **Phase B — Complete the E0-A experimental Harness**. Its first architecture must remain outside Core for provider execution/provenance and must preserve frozen E0 run evidence, deterministic retry/spend/cancellation authority, and the now-closed H1 semantic state machine.
