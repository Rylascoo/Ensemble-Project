# Ensemble Current State

Updated: 2026-09-06

## Authority

Frozen Blueprint 0.1 + approved patch/phase blueprints govern architecture. `Rylascoo/Ensemble-Project` is engineering authority. Resolve `main`; never overstate validation. Workflow: `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`. Product/policy authority and lane boundaries are governed by `docs/PROJECT_AUTHORITY.md`.

The Director owns product constitution, Open Design Register resolution, provider admissibility, and decisions about what the product may do. Engineering/design surfaces supply inputs within their lanes. ODR proposals/resolutions live in `Ensemble-Project/docs` following the ODR-33 precedent.

## Checkpoint

Phase: **E0-A Experimental Harness — Phase B reference-run implementation plus live-host completion native-validated for fake-only/credentialless scope; real provider run still gated**.

H1 Phase A: **CLOSED** by `docs/evidence/H1_CONVERGENCE_AUDIT.md`; promotion `28371ea4bcd9709b771413100af4dcec5a05dc6e`.

Approved Phase-B architecture: `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`, Proposal 0.15; approval: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_APPROVAL.md`.

Original Phase-B implementation evidence: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_IMPLEMENTATION_EVIDENCE.md`; native evidence: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_NATIVE_ARM64_VALIDATION.md`; oracle: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_ORACLE.json`.

Live-host completion native evidence: `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_ARM64_VALIDATION.md`. Attempt 01 partial evidence: `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_VALIDATION_ATTEMPT_01.md`. Director-host apparatus contract: `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

Program map: `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`.

## Native validation

All native evidence below is **Director-machine-sourced** from the Director's native Windows ARM64 machine, not the engineering assistant environment.

### Original Phase-B implementation checkpoint

Exact validated executable/test checkout:

`3749210393282f6aa2ac4ceb0176b6adb5df189e`

Observed: Windows `10.0.26200`, ARM64, `win-arm64`, SDK `9.0.317`; clean tracked/staged tree; **622/622 Core tests PASS**; **39/39 Harness tests PASS**; Harness ARM64 build PASS; Missing Raft PASS; generic smoke PASS.

### Live-host completion checkpoint

Exact validated executable/test checkout:

`1cfdb3aa22abce62a5bd48e80706407670d1c6a9`

Observed: Windows `10.0.26200`, `PROCESSOR_ARCHITECTURE=ARM64`, `dotnet --info` RID `win-arm64`, Host Architecture `arm64`, SDK `9.0.317`; clean tracked/staged tree; no material untracked source/test/fixture files; **622/622 Core tests PASS**; **54/54 Harness tests PASS**; Harness native ARM64 build PASS; Missing Raft PASS; generic smoke PASS; credentialless explicit `e0a-run` PASS with native exit `1`, expected `OPENAI_API_KEY` refusal, no evidence root, and credential remaining absent.

The Harness pricing-policy tuple test is a **constant-freeze tripwire**, not behavioral pricing validation. Its contribution to the 54/54 count must not be cited as proof of live provider pricing freshness or runtime billing behavior.

Commits after `1cfdb3aa22abce62a5bd48e80706407670d1c6a9` that record host quirks, evidence, authority, current state, or handoff are documentation-only unless a later comparison proves otherwise. They do not replace `1cfdb3aa...` as the exact machine-tested executable/test authority.

No real provider request, provider credential use, provider-network execution, or provider spend has been executed or authorized by this validation. No WinUI, Windows AI/NPU, MSIX/WACK, or Store authority.

## Phase-B live-host truth

The Harness owns the approved reference-run envelope outside Core while reusing the closed H1 Cycle/Turn authority unchanged. Core remains unchanged by the live-host completion.

Frozen provider/model control: **OpenAI / `gpt-5.6-sol` only** for E0-A under Blueprint law #35. This is an E0-A reference-envelope configuration constraint; provider/token-counter interfaces are not themselves pinned to those literals, though `E0ARequestBuilder` intentionally emits the OpenAI Responses request shape.

Approved live variants are exactly:

- `CREATIVE-NONE` — Performer/Interpreter `none`, Integrity `high`; normative reference run.
- `CREATIVE-LOW` — Performer/Interpreter `low`, Integrity `high`; matched characterization.
- `CREATIVE-MEDIUM` — Performer/Interpreter `medium`, Integrity `high`; matched characterization.
- `CREATIVE-HIGH` — Performer/Interpreter `high`, Integrity `high`; matched characterization.

Frozen operational limits remain: 12 accepted-Turn cap; one attempt per probabilistic invocation; zero automatic retries; 300-second attempt timeout; 4096 max output tokens per role; estimated model-token spend ceiling USD 5.00/run; `store=false`; `service_tier=default`; truncation disabled; no tools/provider conversation/hidden reasoning.

The live host validates exact clean checkout, evidence-root absence, fixture/Missing-Raft contract, and pricing date validity before credential access. It exposes explicit `e0a-run` and independent post-run `e0a-evaluate` paths and preserves write-once/runtime-root evidence sealing.

Prompt caching is explicit-mode to disable the provider's implicit breakpoint. Cache-write usage is retained for provenance; spend reconciliation prices all reported input tokens conservatively and a nonzero cache-write count terminates technical after usage/spend recording but before semantic consumption.

Pricing snapshot verified 2026-09-06 against OpenAI GPT-5.6 Sol promotional pricing. The host's `RequireNonStaleSnapshot` is a **date-validity guard, not live pricing verification**: it fails closed after 2026-11-21 UTC but cannot detect provider pricing-rule changes inside that window. Closing that residual requires a trusted short-lived pricing attestation or equivalent stable machine-readable provider source. Director authorization for any real run must treat live pricing re-verification as an external gate consideration.

Preserved laws include Access before Context, exact configured-path request/receipt provenance, diagnostic-only streamed deltas, closed-response semantic authority, spend reconciliation before semantic use, fail-closed cancellation/timeout/usage handling, deterministic Integrity/State Authority rejection, atomic accepted causal commit followed immediately by next Opportunity establishment, immutable evidence sealing, and blind-output isolation.

## Validation-host continuity

Future Director Windows ARM64 command sets must be generated from `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

Known host facts include:

- Windows PowerShell `RuntimeInformation.OSArchitecture`, `.ProcessArchitecture`, and `.RuntimeIdentifier` have returned blank and are not trusted architecture gates on this host.
- Trusted architecture probes are `PROCESSOR_ARCHITECTURE=ARM64` plus parsed `dotnet --info` showing `RID: win-arm64` and Host `Architecture: arm64`.
- Expected native stderr can surface as `NativeCommandError` under `$ErrorActionPreference='Stop'`; expected-failure probes must isolate stderr, capture `$LASTEXITCODE` immediately, and assert exit/message/filesystem effects independently.

## Standing decisions

- .NET 10 requires later Director approval.
- `IPackageValidator` is not a mandatory first-release gate.
- E5c exception-runtime provenance: open, no verified defect/fix.
- Stage remains design-lane authority.
- ODR-12/13/30/32 remain open; none blocks E0-A.
- ODR proposal/resolution authority belongs to the Director and is filed in `Ensemble-Project/docs` regardless of originating lane.
- Real provider credentials/network execution/spend require a separate explicit Director gate.
- `e0a-phase-b-live-host-completion-2` is a strict stale ancestor with no unique commits and remains untouched for Director disposition.

## Next

Remain inside **E0-A Phase B**. Do not redesign Proposal 0.15 and do not enter E0-B..G, product Application/persistence/UI, Scene-ending semantics, Context optimization, Windows AI/NPU, MSIX/WACK/Store, or later scope.

The active work branch is `e0a-phase-b-live-host-completion`. Before promotion, resolve current `main`, recursively audit the final branch diff/evidence/current-state/handoff for correctness, consistency, authority, scope, validation wording, pricing residual, and hygiene, and confirm no executable/test changes occurred after machine-tested checkpoint `1cfdb3aa...`.

If that audit is clean, promote the live-host completion branch to `main` through the normal reviewed GitHub path. Documentation-only closure commits after `1cfdb3aa...` do not require another native run unless the executable/test surface changed.

After promotion, the next consequential product action remains the first real reference-provider run using the frozen envelope and evidence package. That action requires **separate explicit Director authorization** for credentials, network execution, and spend. Nothing in fake-only native validation or promotion opens that gate.
