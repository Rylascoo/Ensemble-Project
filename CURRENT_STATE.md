# Ensemble Current State

Updated: 2026-09-06

## Authority

Frozen Blueprint 0.1 + approved patch/phase blueprints govern architecture. `Rylascoo/Ensemble-Project` is engineering authority. Resolve `main`; never overstate validation. Workflow: `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`.

## Checkpoint

Phase: **E0-A Experimental Harness — Phase B reference-run implementation validated; behavioral provider run still gated**.

H1 Phase A: **CLOSED** by `docs/evidence/H1_CONVERGENCE_AUDIT.md`; promotion `28371ea4bcd9709b771413100af4dcec5a05dc6e`.

Approved Phase-B architecture: `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`, Proposal 0.15; approval: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_APPROVAL.md`.

Validated implementation evidence: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_IMPLEMENTATION_EVIDENCE.md`; native evidence: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_NATIVE_ARM64_VALIDATION.md`; oracle: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_ORACLE.json`.

Program map: `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`.

## Native validation

Phase-B evidence is **Director-machine-sourced** from the Director's native Windows ARM64 machine, not the engineering assistant environment.

Exact validated executable/test checkout:

`3749210393282f6aa2ac4ceb0176b6adb5df189e`

Observed: Windows `10.0.26200`, ARM64, `win-arm64`, SDK `9.0.317`; clean tracked/staged tree; **622/622 Core tests PASS**; **39/39 Harness tests PASS**; Harness ARM64 build PASS; Missing Raft PASS; generic smoke PASS.

Commits after the validated checkout that record/reconcile evidence or current-state documentation are docs-only and do not replace the exact machine-tested executable/test authority.

No real provider request, provider credential use, or provider spend has been executed or authorized by this validation. No WinUI, Windows AI/NPU, MSIX/WACK, or Store authority.

## Phase-B implementation truth

The Harness now owns the approved reference-run envelope outside Core while reusing the closed H1 Cycle/Turn authority unchanged.

Frozen reference configuration: OpenAI official Responses API boundary; `gpt-5.6-sol`; Performer/Interpreter reasoning `none`, Integrity `high`; 12 accepted-Turn cap; one attempt per probabilistic invocation; zero automatic retries; 300-second attempt timeout; 4096 max output tokens per role; estimated model-token spend ceiling USD 5.00/run; `store=false`; no tools/provider conversation/hidden reasoning.

Preserved laws include Access before Context, exact configured-path request/receipt provenance, diagnostic-only streamed deltas, closed-response semantic authority, spend reconciliation before semantic use, fail-closed cancellation/timeout/usage handling, deterministic Integrity/State Authority rejection, atomic accepted causal commit followed immediately by next Opportunity establishment, immutable evidence sealing, and blind-output isolation.

Core remains unchanged by Phase B; provider/network/filesystem/secrets remain Harness-edge concerns.

## Standing decisions

- .NET 10 requires later Director approval.
- `IPackageValidator` is not a mandatory first-release gate.
- E5c exception-runtime provenance: open, no verified defect/fix.
- Stage remains design-lane authority.
- ODR-12/13/30/32 remain open; none blocks E0-A.
- Real provider credentials/network execution/spend require a separate explicit Director gate.

## Next

Remain inside **E0-A Phase B**. Do not redesign Proposal 0.15 and do not enter E0-B..G, product Application/persistence/UI, Scene-ending semantics, Context optimization, Windows AI/NPU, MSIX/WACK/Store, or later scope.

The next consequential step is the first real reference-provider run using the already frozen envelope and evidence package. That step requires explicit Director authorization for credentials/network execution and spend. Until that gate is granted, only non-spend audit/evidence/promotion work is authorized.

The roadmap's older instruction to blueprint the smallest Phase-B Harness boundary is superseded by this checkpoint; the dependency ordering and Phase-B exit criteria remain authoritative.
