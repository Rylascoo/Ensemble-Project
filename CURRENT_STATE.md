# Ensemble Current State

Updated: 2026-09-05

## Authority

Frozen Blueprint 0.1 + approved patch blueprints govern architecture. `Rylascoo/Ensemble-Project` is engineering authority. Resolve `main`; never overstate validation.

## Current checkpoint

Phase: `E0-A Harness Implementation — H1 Deterministic Spine`

Latest completed: **H1 Patch 0018 — Deterministic Turn Orchestration — Proposal 0.6**.

PR `#33`; promotion `d85904617c361f34cdb31a10d2e0200e3becc83c`.

Artifacts:
- `docs/blueprint/H1_PATCH_0018_DETERMINISTIC_TURN_ORCHESTRATION.md`
- `docs/evidence/PATCH_0018_EVIDENCE.md`
- `docs/evidence/PATCH_0018_ORACLE.json`

## Native validation

Patch 0018 evidence is **Director-machine-sourced** from the Director's native Windows ARM64 machine, not the engineering assistant environment.

Validated checkout `023b469b801239c2b0d597aec1e7712aa4a8faa7`; executable/test checkpoint `7e94614dc484aff8cb9b8e39c1a74a7b04ea3238`.

Observed: Windows `10.0.26200`, ARM64, `win-arm64`, SDK `9.0.317`; clean tree; Core + tests compile PASS; **622/622 PASS**; Harness ARM64 build PASS; Missing Raft PASS; generic smoke PASS. Later pre-validation changes were docs-only.

No WinUI, Windows AI/NPU, MSIX/WACK, or Store authority.

## Implemented H1 spine

Fixture/domain -> Access -> Context -> Performer Candidate -> Director Opportunity -> Integrity -> State Interpreter -> State Authority -> Take -> atomic causal commit -> effective Opportunity -> Production-bound Context -> accepted Performance history -> synchronized causal cycle -> provider-neutral Performer attempt -> deterministic Turn orchestration.

Patch 0018 closes deterministic Turn progression from opportunity-bearing state through fresh Context, attempt gate, Integrity, Interpretation/Authority, Accepted Take and causal commit to valid postcommit state. Later Opportunity establishment remains explicit Patch 0016 authority. Technical failure/cancellation never becomes fiction; stale Context/attempt identity fails closed.

E0 Context reconstructs permitted state each turn and includes all accepted current-Scene Performances in append order; no windowing/retrieval/summarization. ODR-12/13/30/32 remain open and do not block E0-A.

## Standing decisions

- .NET 10 migration requires later Director approval.
- `IPackageValidator` is not a first-release gate; ship-plan wording is stale.
- E5c exception-runtime provenance: open, no verified defect/fix.
- Stage representation remains design-lane authority.

## Next action

Run the **end-of-H1 convergence/deletion audit** against roadmap Phase-A exit criteria. Falsify whether any deterministic seam still blocks the canonical path from opportunity-bearing Production through attempt/Turn/commit to the next synchronized opportunity-bearing state; remove unnecessary H1 surface where safe. If Phase A closes, blueprint the smallest E0-A Harness boundary for real provider execution/provenance outside Core.
