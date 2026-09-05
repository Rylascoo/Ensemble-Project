# Ensemble Current State

Updated: 2026-09-05

## Authority

Frozen Blueprint 0.1 and approved patch blueprints govern architecture. `Rylascoo/Ensemble-Project` is engineering authority. Read `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`, resolve `main`, and never overstate validation.

## Current checkpoint

Phase: `E0-A Harness Implementation — H1 Deterministic Spine`

Latest completed patch: **H1 Patch 0017 — Provider-Neutral Performer Attempt Boundary — Proposal 0.2**

PR `#32`; promotion commit `659430dfbf3289cb5eb9632968db2ab3b03ace23`.

Canonical artifacts:
- `docs/blueprint/H1_PATCH_0017_PERFORMER_ATTEMPT_BOUNDARY.md`
- `docs/evidence/PATCH_0017_EVIDENCE.md`
- `docs/evidence/PATCH_0017_ORACLE.json`

## Native validation

Patch 0017 ARM64 evidence is **Director-machine-sourced** from the Director's native Windows ARM64 machine, not the engineering assistant environment.

Exact validated checkout: `2c64502a05388206c698d3f04f0eeb8389545afe`.

Observed: Windows `10.0.26200`, ARM64, `win-arm64`, .NET SDK `9.0.317`; tracked/staged clean; Core + tests compile PASS; **598/598 tests PASS**, 0 failed/skipped; Harness ARM64 build PASS; Missing Raft PASS; generic smoke PASS. Later pre-promotion changes were evidence/oracle only.

No WinUI, Windows AI/NPU, MSIX/WACK, or Store authority exists.

## Implemented H1 spine

Fixture/domain -> Access -> Context -> Performer Candidate -> Director Opportunity -> Integrity -> State Interpreter -> State Authority -> Take -> atomic causal commit -> effective Opportunity -> Production-bound Context -> accepted Performance history -> synchronized causal cycle -> provider-neutral Performer attempt result.

Patch 0017 binds exact semantic Context to `CandidateReady`, payload-free `TechnicalFailure`, or `Cancelled`. It adds no provider execution, authenticated provenance, retry policy, persistence, or fictional authority.

E0 Context reconstructs permitted state each turn and includes all accepted current-Scene Performances in append order; no windowing/retrieval/summarization. ODR-12/13/30/32 remain open in `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md` and do not block E0-A.

## Standing decisions

- .NET 10 migration requires later Director approval.
- `IPackageValidator` is not a first-release gate; stale ship-plan wording needs maintenance.
- E5c exception-runtime-type provenance remains open; no verified defect/fix.
- Stage representation remains design-lane authority.

## Next action

Falsify and blueprint the **minimal deterministic run/turn orchestrator** consuming Patch 0017 attempt result plus existing Integrity, Interpreter, State Authority, Take, and Patch 0016 Cycle authority.

It must reject stale attempt/context identity and keep provider execution/provenance, retry/spend policy, persistence, Scene lifecycle, and post-E0 Context optimization outside scope unless separately approved.

No implementation before recursive architecture audit and explicit Director approval.
