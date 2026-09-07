# Ensemble Current State

Updated: 2026-09-06 hardening cycle

## Authority

Frozen Blueprint 0.1 + approved phase/patch blueprints govern architecture. `Rylascoo/Ensemble-Project` is engineering authority. Resolve `main`; never promote or overstate validation.

Workflow: `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`.

Product/policy authority and lane boundaries: `docs/PROJECT_AUTHORITY.md`.

Open questions: `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md`.

Program plan: `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`.

The Director owns product constitution, Open Design Register resolution, provider admissibility, and decisions about what the product may do. Engineering/design surfaces supply inputs within their lanes. ODR proposals/resolutions live in `Ensemble-Project/docs` following the ODR-33 precedent.

## Current checkpoint

Phase: **E0-A Experimental Harness — Phase B post-audit hardening implementation and final recursive static audit COMPLETE; grouped native Windows ARM64 validation PENDING; real provider execution NOT AUTHORIZED**.

Authoritative hardening branch:

`e0a-phase-b-post-audit-hardening`

Live `main` / audited hardening baseline / merge base:

`f9cb1a79ad7923b640ff1a97c46d8fd6ab9dac25`

Exact final hardening executable/test checkpoint:

`d18ec637bb8881a38db9cfeb1420f093a994ac17`

Documentation/evidence commits follow that executable/test checkpoint on the hardening branch. They do not replace `d18ec637...` as the checkout requiring grouped native validation.

Draft review surface: **PR #39**. Keep draft; do not merge before native validation and final review.

## Post-audit hardening disposition

The comprehensive E0-A post-implementation audit was closed with a zero-new-finding pass, then its complete authorized correction set was implemented in dependency order.

Closed-finding authority and final disposition:

`docs/evidence/E0A_POST_AUDIT_HARDENING_CLOSED_FINDINGS.md`

Final all-group static closure / finding-to-test matrix / falsification record:

`docs/evidence/E0A_POST_AUDIT_HARDENING_FINAL_STATIC_CLOSURE.md`

Grouped native validation handoff:

`docs/handoff/E0A_POST_AUDIT_HARDENING_GROUPED_NATIVE_ARM64_VALIDATION_HANDOFF.md`

Implementation checkpoints:

- Group 1 — E-01 Integrity terminal sealing + E-02 streaming cancellation: `171881c1247e1c466fec6abd3e92335a055eb4f2`.
- Group 2 — E-03 provider decoding + E-07 timeout composition: `f37cb8ec41e50d50aba027286326a10677db1040`.
- Group 3 — E-06 receipt provenance + P-02/P-05 spend robustness: `c3a6846d267ce3f93a3e80a297057d4a7a14d99d`.
- Group 4 — I-02/I-03/I-04/P-04 evidence/evaluation authority: `2a55319820587b63da13377a775150b90c24b107`.
- Group 5 — E-04/E-05/I-05 parser/checkout hardening: `f79bc36bf72d3db49b703bf98919534fef9673b5`.
- Group 6 — P-01/P-03 + final regression-gap review: final executable/test checkpoint `d18ec637bb8881a38db9cfeb1420f093a994ac17`.

All six per-group recursive static audits are complete. The final cross-component falsification pass and a subsequent zero-new-material-correction pass are complete.

No compiler/test-runtime/native Windows ARM64 claim applies to `d18ec637...` yet. No GitHub Actions workflow result exists for that checkpoint.

### Cumulative hardening source boundary

The cumulative hardening branch changes exactly one Core source file:

`src/Ensemble.E0.Core/Fixture/StrictJsonPreflight.cs`

That E-04 correction narrowly normalizes invalid property-name decoding into the existing fixture-validation domain. No other `src/Ensemble.E0.Core/**` source file changed.

All other executable hardening corrections are Harness-side. Group 6 intentionally changes one Core structural test file to remove incidental private implementation coupling while preserving frozen Patch 0012 behavioral/public/canonical laws.

### Final regression-gap state

All closed-audit final regression obligations are mapped. Existing exact coverage was retained for duplicate Integrity concerns, refusal/incomplete responses, malformed Unicode, cancellation/stalled streams, timeout composition, failed-response usage, spend extremes/out-of-tier usage, run ownership, runtime/evaluation tampering, evaluation failures, checkout-from-subdirectory behavior, and malformed evaluation inputs.

Group 6 added the four remaining nonredundant cases:

- provider technical failure across Performer / Integrity / Interpreter;
- failure after a previously accepted Turn;
- mismatched configured success receipt without semantic adoption;
- saved-artifact deterministic reconstruction of a mutating three-Turn run with nonempty controls and `Add -> Supersede -> Deactivate`.

## Native validation authority

All native evidence below is **Director-machine-sourced** from the Director's native Windows ARM64 machine, not the engineering assistant environment.

### Historical original Phase-B implementation checkpoint

`3749210393282f6aa2ac4ceb0176b6adb5df189e`

Observed historically: Windows `10.0.26200`, ARM64, `win-arm64`, SDK `9.0.317`; clean tracked/staged tree; **622/622 Core tests PASS**; **39/39 Harness tests PASS**; Harness ARM64 build PASS; Missing Raft PASS; generic smoke PASS.

### Historical live-host completion checkpoint

Current machine-tested executable/test authority until hardening native validation succeeds:

`1cfdb3aa22abce62a5bd48e80706407670d1c6a9`

Observed historically: Windows `10.0.26200`, `PROCESSOR_ARCHITECTURE=ARM64`, `dotnet --info` RID `win-arm64`, Host `Architecture: arm64`, SDK `9.0.317`; clean tracked/staged tree; no material untracked source/test/fixture files; **622/622 Core tests PASS**; **54/54 Harness tests PASS**; Harness native ARM64 build PASS; Missing Raft PASS; generic smoke PASS; credentialless explicit `e0a-run` PASS with native exit `1`, expected `OPENAI_API_KEY` refusal, no evidence root, and credential remaining absent.

This historical PASS does not validate hardening checkpoint `d18ec637...`.

Coverage caveat: the pricing-policy tuple test is a constant-freeze tripwire, not behavioral provider-pricing validation. Native fake-only validation cannot establish live pricing freshness, cache billing behavior, or provider spend correctness.

## Phase-B approved truth preserved by hardening

Approved architecture:

`docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`, Proposal 0.15.

Approval:

`docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_APPROVAL.md`.

The Harness owns the reference-run envelope outside Core while reusing the closed H1 deterministic Cycle/Turn authority.

Frozen provider/model control for E0-A remains **OpenAI / `gpt-5.6-sol` only** under the approved reference envelope. This is an E0-A experiment configuration constraint, not product-wide provider law.

Approved variants remain exactly:

- `CREATIVE-NONE` — Performer/Interpreter `none`, Integrity `high`; normative reference run.
- `CREATIVE-LOW` — Performer/Interpreter `low`, Integrity `high`.
- `CREATIVE-MEDIUM` — Performer/Interpreter `medium`, Integrity `high`.
- `CREATIVE-HIGH` — Performer/Interpreter `high`, Integrity `high`.

Frozen operational limits remain:

- 12 accepted-Turn cap;
- one attempt per probabilistic role invocation;
- zero automatic retries;
- 300-second attempt deadline;
- 4096 max output tokens per role;
- estimated model-token spend ceiling USD 5.00/run;
- `store=false`;
- `service_tier=default`;
- truncation disabled;
- no tools/provider conversation/hidden reasoning.

Preserved laws include Access before Context; exact configured-path request/receipt provenance; diagnostic-only streamed deltas; closed-response semantic authority; spend reconciliation before semantic use; fail-closed cancellation/timeout/usage handling; deterministic Integrity and State Authority; atomic accepted causal commit; immediate deterministic Opportunity establishment; immutable evidence sealing; blind-output isolation; technical failure never becoming fiction.

## Hardening truth now added

The hardened Harness additionally establishes statically:

- expected Integrity orchestration rejection reaches ordinary terminal/runtime sealing;
- no synchronous EOF probe precedes cancellable streaming reads;
- malformed provider JSON/Unicode is rejected before semantic adoption;
- the explicit 300-second attempt token cannot be preempted by the default `HttpClient` timeout;
- failed/refused/incomplete calls preserve validated nonsemantic receipt/usage provenance where available;
- unknown usage is explicit and conservatively fallback-accounted;
- standard-tier pricing is not falsely applied above its verified token tier;
- spend arithmetic/reservations fail closed on unrepresentable state and use reservation identity rather than monetary amount as authority;
- local evidence/run ownership is create-new/write-once;
- terminal spend/pricing/usage/state claims are runtime-rooted and bound by a recomputable runtime-seal identity;
- evaluation re-verifies runtime authority before publication and cannot endorse altered final summary claims;
- Integrity concern names are exactly the five approved names;
- checkout inspection is repository-root-relative independent of launch subdirectory;
- Patch 0012 tests no longer freeze incidental private call graphs/catch/declaration layout.

## Pricing gate residual

`E0APricingPolicy.RequireNonStaleSnapshot` remains a date-validity guard, not live pricing verification. It cannot detect a provider pricing/cache/long-context policy change occurring earlier inside the published date window.

Closing that residual still requires the previously identified short-lived trusted pricing attestation or equivalent stable machine-readable provider pricing source. Nothing in hardening or native fake-only validation erases that limitation.

## Validation-host continuity

Future Director Windows ARM64 command sets must follow:

`docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

Known host facts:

- PowerShell `RuntimeInformation` architecture properties have returned blank and are not trusted host gates.
- Trusted probes are `PROCESSOR_ARCHITECTURE=ARM64` plus parsed `dotnet --info` showing `RID: win-arm64` and Host `Architecture: arm64`.
- Expected native stderr can surface as `NativeCommandError` under `$ErrorActionPreference='Stop'`; expected-failure probes must isolate stdout/stderr, capture `$LASTEXITCODE` immediately, and assert exit/message/filesystem effects independently.

The exact grouped validation command set for `d18ec637...` is already filed in the native-validation handoff above. It intentionally checks out that executable/test checkpoint detached because documentation commits follow it on the hardening branch.

## Standing decisions

- H1 Phase A: **CLOSED** by `docs/evidence/H1_CONVERGENCE_AUDIT.md`; promotion `28371ea4bcd9709b771413100af4dcec5a05dc6e`.
- .NET 10 requires later Director approval.
- `IPackageValidator` is not a mandatory first-release gate.
- E5c exception-runtime provenance: open; no verified defect/fix.
- Stage remains design-lane authority.
- ODR-12/13/26/30/32 remain open; none blocks E0-A.
- ODR-26 working structure: `docs/ODR_26_COST_GOVERNANCE_AND_PROVIDER_ADMISSION_WORKING_CONTRACT.md`; it is not frozen/resolved/project law and has no E0-A/E0-B provider-selection effect.
- ODR proposals/resolutions are Director authority and live in `Ensemble-Project/docs` regardless of originating lane.
- Real provider credentials/network execution/spend require a separate explicit Director gate.
- `e0a-phase-b-live-host-completion-2` remains a stale ancestor for Director disposition.

## Provider / scope gate

**PROVIDER EXECUTION: NOT AUTHORIZED.**

Do not access or use `OPENAI_API_KEY`. Do not perform provider inference, provider-network execution, or provider spend.

Do not enter E0-B..G, product Application/persistence/UI, Stage/Scene-ending semantics, Context optimization, Windows AI/NPU, MSIX/WACK/Store, Partner Center, or later ODR scope.

## Next

Remain inside **E0-A Phase B**.

The only next executable-authority action is grouped native Windows ARM64 validation of exact checkout:

`d18ec637bb8881a38db9cfeb1420f093a994ac17`

Use:

`docs/handoff/E0A_POST_AUDIT_HARDENING_GROUPED_NATIVE_ARM64_VALIDATION_HANDOFF.md`.

After a complete Director-machine PASS:

1. record exact native environment, Core/Harness counts, build, fixture-smoke, credentialless-gate, and post-cleanliness evidence;
2. make that exact checkout the current machine-tested hardening authority;
3. update this current state;
4. review draft PR #39 for promotion to `main`.

A native PASS does **not** open the real-provider gate. The first real reference-provider run remains separately Director-authorized work after hardening promotion/review.
