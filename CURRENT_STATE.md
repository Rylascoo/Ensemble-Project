# Ensemble Current State

Updated: 2026-09-06

## Authority

Frozen Blueprint 0.1 + approved phase/patch blueprints govern architecture. `Rylascoo/Ensemble-Project` is engineering authority. Resolve `main`; never promote or overstate validation. Workflow: `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`. Open questions: `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md`. Plan: `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`. Product/policy authority and lane boundaries: `docs/PROJECT_AUTHORITY.md`.

The Director owns product constitution, Open Design Register resolution, provider admissibility, and decisions about what the product may do. Engineering/design surfaces supply inputs within their lanes. ODR proposals/resolutions live in `Ensemble-Project/docs` following the ODR-33 precedent.

## Checkpoint

Phase: **E0-A Experimental Harness — Phase B post-audit hardening in progress on `e0a-phase-b-post-audit-hardening`; Patch Group 1 implemented and recursively static-audited; remaining closed-audit hardening scope recorded and authorized; grouped native validation pending; real provider run still gated**.

H1 Phase A: **CLOSED** by `docs/evidence/H1_CONVERGENCE_AUDIT.md`; promotion `28371ea4bcd9709b771413100af4dcec5a05dc6e`.

Approved Phase-B architecture: `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`, Proposal 0.15; approval: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_APPROVAL.md`.

Original Phase-B implementation evidence: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_IMPLEMENTATION_EVIDENCE.md`; native evidence: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_NATIVE_ARM64_VALIDATION.md`; oracle: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_ORACLE.json`.

Live-host completion native evidence: `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_ARM64_VALIDATION.md`. Attempt 01 partial evidence: `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_VALIDATION_ATTEMPT_01.md`. Director-host apparatus contract: `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

Post-audit hardening Patch Group 1 implementation evidence: `docs/evidence/E0A_POST_AUDIT_HARDENING_PATCH_GROUP_1_IMPLEMENTATION.md`.

Closed-audit remaining hardening authority/continuity: `docs/evidence/E0A_POST_AUDIT_HARDENING_CLOSED_FINDINGS.md`.

Program map: `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`.

This state reconciles former `main` `a3b5126dad440d2f75bef655fdc6e4ac7098b839` with former live-host branch head `9228c77a5b789c4c6a1e0417f889851c6b92730e` as a union. Their merge base was `13ffde90f78a8bf40a04381b136e846d1cf64d1c`. Parallel-main ODR-26 continuity and the live-host validation/authority continuity both survive.

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

Coverage caveat: the pricing-policy tuple test `PricingPolicy_FreezesSourcePromotionAndConservativeRates` is a **constant-freeze tripwire**, not behavioral pricing validation. Its pass contributes to the suite count but only proves that the compiled frozen constants match the independently maintained expected tuple in the test. It does not establish live provider pricing freshness, provider cache behavior, or runtime spend correctness. Those claims require separate runtime/provider evidence and remain subject to the known pricing-snapshot limitation.

The comparison from `1cfdb3aa22abce62a5bd48e80706407670d1c6a9` to former branch head `9228c77a5b789c4c6a1e0417f889851c6b92730e` contains documentation only: `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, four live-host evidence/host records, and this handoff. The four parallel-main commits after merge base are also documentation-only. The reconciliation therefore changes no executable, test, fixture, or Core surface and does not replace `1cfdb3aa...` as the machine-tested live-host executable/test authority.

No real provider request, provider credential use, provider-network execution, or provider spend has been executed or authorized by this validation. No WinUI, Windows AI/NPU, MSIX/WACK, or Store authority.

## Post-audit hardening branch

Authoritative hardening branch:

`e0a-phase-b-post-audit-hardening`

Audited `main` baseline / merge base:

`f9cb1a79ad7923b640ff1a97c46d8fd6ab9dac25`

Patch Group 1 exact executable/test checkpoint:

`171881c1247e1c466fec6abd3e92335a055eb4f2`

Patch Group 1 contains only E-01 Integrity failure terminal sealing and E-02 streaming cancellation boundary. It changes exactly four Harness/Harness-test files and zero Core files. The implementation and recursive static audit are complete. No compiler, test-runtime, Windows ARM64, fixture-smoke, or live-host validation claim applies to this hardening executable/test checkpoint yet.

The E-01 correction routes known `E0TurnOrchestrationException` failures from the configured Integrity evaluation call through the existing `InvalidOutput -> Finish(...)` terminal/runtime-seal path. It adds no broad exception handling and no parallel Harness Integrity semantic validator. The regression proves duplicate concerns fail before Interpreter, no accepted Performance or commit occurs, provider terminal receipts remain evidenced, and runtime evidence seals normally.

The E-02 correction removes synchronous `StreamReader.EndOfStream` probing before cancellable reads. Streaming now reads through `ReadLineAsync(cancellationToken)` and treats `null` as EOF. Existing diagnostic streaming and `response.completed` semantic authority remain unchanged. The regression uses a stalling stream that signals asynchronous read start, cancels deterministically, and proves zero synchronous reads.

Recursive audit removed two unnecessary intermediate choices: a new `integrity.rejected` evidence event and a duplicate-specific Harness catch filter. The final implementation uses the existing typed Core orchestration failure boundary and existing run terminal/evidence lifecycle.

The comprehensive post-implementation audit is closed and achieved a zero-new-finding pass. The complete remaining correction set, dependency order, regression-gap obligations, and grouped-validation boundary are preserved without reinterpretation in `docs/evidence/E0A_POST_AUDIT_HARDENING_CLOSED_FINDINGS.md`. Authorized remaining groups are: Group 2 E-03/E-07; Group 3 E-06/P-02/P-05; Group 4 I-02/I-03/I-04/P-04; Group 5 E-04/E-05/I-05; Group 6 P-01/P-03 plus final regression-gap review.

Draft review surface: PR #39. `main` remains untouched by the hardening implementation.

Per the hardening branch strategy, remaining hardening groups stay isolated on this branch and grouped native Windows ARM64 validation occurs before merge to `main`.

## Phase-B live-host truth

The Harness owns the approved reference-run envelope outside Core while reusing the closed H1 Cycle/Turn authority unchanged. Core remains unchanged by the live-host completion and by Patch Group 1 hardening.

Frozen provider/model control: **OpenAI / `gpt-5.6-sol` only** for E0-A under Blueprint law #35. This is an E0-A reference-envelope configuration constraint; provider/token-counter interfaces are not themselves pinned to those literals, though `E0ARequestBuilder` intentionally emits the OpenAI Responses request shape.

Approved live variants are exactly:

- `CREATIVE-NONE` — Performer/Interpreter `none`, Integrity `high`; normative reference run.
- `CREATIVE-LOW` — Performer/Interpreter `low`, Integrity `high`; matched characterization.
- `CREATIVE-MEDIUM` — Performer/Interpreter `medium`, Integrity `high`; matched characterization.
- `CREATIVE-HIGH` — Performer/Interpreter `high`, Integrity `high`; matched characterization.

Frozen operational limits remain: 12 accepted-Turn cap; one attempt per probabilistic invocation; zero automatic retries; 300-second attempt timeout; 4096 max output tokens per role; estimated model-token spend ceiling USD 5.00/run; `store=false`; `service_tier=default`; truncation disabled; no tools/provider conversation/hidden reasoning.

The live host validates exact clean checkout, evidence-root absence, fixture/Missing-Raft contract, and pricing date validity before credential access. It exposes explicit `e0a-run` and independent post-run `e0a-evaluate` paths and preserves write-once/runtime-root evidence sealing.

Prompt caching is explicit-mode to disable the provider's implicit breakpoint. Cache-write usage is retained for provenance; spend reconciliation prices all reported input tokens conservatively and a nonzero cache-write count terminates technical after usage/spend recording but before semantic consumption.

The live host's `RequireNonStaleSnapshot` remains a date-validity guard, not live pricing verification. It fails closed after the published promotional-guarantee date, but it cannot detect provider changes to pricing terms, cache-write multiplier, long-context threshold, or related billing rules that occur earlier within that date window.

Closing that residual still requires the previously identified short-lived trusted pricing attestation or an equivalent stable machine-readable provider pricing source. The prior native PASS does not erase or narrow that known limit.

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
- ODR-12/13/26/30/32 remain open; none blocks E0-A.
- ODR-26 working structure: `docs/ODR_26_COST_GOVERNANCE_AND_PROVIDER_ADMISSION_WORKING_CONTRACT.md`. **Not frozen/resolved/project law; no E0-A/E0-B provider-selection effect.**
- The ODR-26 working contract is preserved unchanged from parallel `main`. It was drafted outside its authoring surface's lane and is reported for Director disposition rather than adjudicated or corrected by engineering.
- ODR proposal/resolution authority belongs to the Director and is filed in `Ensemble-Project/docs` regardless of originating lane.
- Real provider credentials/network execution/spend require a separate explicit Director gate.
- `e0a-phase-b-live-host-completion-2` is a strict stale ancestor with no unique commits and remains untouched for Director disposition.

## Next

Remain inside **E0-A Phase B**. Do not redesign Proposal 0.15 or enter E0-B..G, product Application/persistence/UI, Scene endings, Context optimization, Windows AI/NPU, MSIX/WACK/Store, or later scope.

Continue post-audit hardening only on `e0a-phase-b-post-audit-hardening` in the dependency order recorded by `docs/evidence/E0A_POST_AUDIT_HARDENING_CLOSED_FINDINGS.md`. Patch Group 1 is complete; Patch Group 2 is E-03 provider decoding plus E-07 timeout composition.

After all authorized hardening groups and recursive audits are complete, run the grouped native Windows ARM64 validation gate on the exact final executable/test checkout, using `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md` as the validation-host contract. Only that later native evidence may replace `1cfdb3aa...` as executable/test authority.

The first real reference-provider run is not an authorized action while hardening is in progress. It remains separately gated and requires explicit Director authorization for `OPENAI_API_KEY`, provider credentials, network inference, and spend after the hardening branch is validated and promoted. Nothing in prior fake-only native validation, Patch Group 1 implementation, static audit, or draft PR #39 opens that gate.
