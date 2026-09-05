# Ensemble Current State

Updated: 2026-09-05

## Authority

Frozen Blueprint 0.1 and approved patch blueprints govern architecture. `Rylascoo/Ensemble-Project` is engineering authority. Read `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`, resolve `main`, and never overstate validation.

## Current checkpoint

Phase: `E0-A Harness Implementation — H1 Deterministic Spine`

Latest completed patch: **H1 Patch 0018 — Deterministic Turn Orchestration — Proposal 0.6**.

PR `#33`; squash promotion commit `d85904617c361f34cdb31a10d2e0200e3becc83c`.

Canonical artifacts:
- `docs/blueprint/H1_PATCH_0018_DETERMINISTIC_TURN_ORCHESTRATION.md`
- `docs/evidence/PATCH_0018_EVIDENCE.md`
- `docs/evidence/PATCH_0018_ORACLE.json`

## Native validation

Patch 0018 ARM64 evidence is **Director-machine-sourced** from the Director's native Windows ARM64 machine, not the engineering assistant environment.

Validated checkout: `023b469b801239c2b0d597aec1e7712aa4a8faa7`; last executable/test checkpoint `7e94614dc484aff8cb9b8e39c1a74a7b04ea3238`.

Observed: Windows `10.0.26200`, ARM64, `win-arm64`, SDK `9.0.317`; tracked/staged clean; Core + tests compile PASS; **622/622 tests PASS**, 0 failed/skipped; Harness ARM64 build PASS; Missing Raft PASS; generic smoke PASS. Commits after the executable checkpoint through validation were docs-only.

No WinUI, Windows AI/NPU, MSIX/WACK, or Store authority exists.

## Implemented H1 spine

Fixture/domain -> Access -> Context -> Performer Candidate -> Director Opportunity -> Integrity -> State Interpreter -> State Authority -> Take -> atomic causal commit -> effective Opportunity -> Production-bound Context -> accepted Performance history -> synchronized causal cycle -> provider-neutral Performer attempt -> deterministic Turn orchestration.

Patch 0018 closes the deterministic Turn path from an opportunity-bearing synchronized state through fresh Context, attempt gating, Integrity, Interpretation/Authority, Accepted Take and causal commit to a valid postcommit state. Later Opportunity establishment remains the explicit Patch 0016 operation. Technical failure/cancellation never becomes fiction; stale Context/attempt identities fail closed.

E0 Context reconstructs permitted state each turn and includes all accepted current-Scene Performances in append order; no windowing/retrieval/summarization. ODR-12/13/30/32 remain open and do not block E0-A.

## Standing decisions

- .NET 10 migration requires later Director approval.
- `IPackageValidator` is not a first-release gate; stale ship-plan wording needs maintenance.
- E5c exception-runtime-type provenance remains open; no verified defect/fix.
- Stage representation remains design-lane authority.

## Next action

Perform the **end-of-H1 convergence/deletion audit** against the Phase-A exit criteria in `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`.

Falsify whether any deterministic seam still blocks one canonical path from opportunity-bearing Production through attempt/Turn/accepted commit to the next synchronized opportunity-bearing state. Remove unnecessary H1 surface where safe. If the audit closes Phase A, blueprint the smallest E0-A experimental Harness boundary for real provider execution/provenance outside Core.
