# Ensemble Current State

Updated: 2026-09-07

## Authority

Frozen Blueprint 0.1 + approved phase/patch blueprints govern architecture. `Rylascoo/Ensemble-Project` is engineering authority. Resolve `main`; never promote or overstate validation. Workflow: `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`. Open questions: `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md`. Plan: `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`. Product/policy authority and lane boundaries: `docs/PROJECT_AUTHORITY.md`.

The Director owns product constitution, Open Design Register resolution, provider admissibility, and decisions about what the product may do. Engineering/design surfaces supply inputs within their lanes. ODR proposals/resolutions live in `Ensemble-Project/docs` following the ODR-33 precedent.

## Checkpoint

Phase: **E0-A Experimental Harness — Phase B reference-run implementation plus live-host completion native-validated for fake-only/credentialless scope; real provider run still gated**.

H1 Phase A: **CLOSED** by `docs/evidence/H1_CONVERGENCE_AUDIT.md`; promotion `28371ea4bcd9709b771413100af4dcec5a05dc6e`.

Approved Phase-B architecture: `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`, Proposal 0.15; approval: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_APPROVAL.md`.

Original Phase-B implementation evidence: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_IMPLEMENTATION_EVIDENCE.md`; native evidence: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_NATIVE_ARM64_VALIDATION.md`; oracle: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_ORACLE.json`.

Live-host completion native evidence: `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_ARM64_VALIDATION.md`. Attempt 01 partial evidence: `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_VALIDATION_ATTEMPT_01.md`. Director-host apparatus contract: `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

Program map: `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`.

This state reconciles former `main` `a3b5126dad440d2f75bef655fdc6e4ac7098b839` with former live-host branch head `9228c77a5b789c4c6a1e0417f889851c6b92730e` as a union. Their merge base was `13ffde90f78a8bf40a04381b136e846d1cf64d1c`. Parallel-main ODR-26 continuity and the live-host validation/authority continuity both survive.

## Repository renovation checkpoint

Repository-renovation integration PR #40 was promoted to `main` at `175aa1f360aa8458fb571ac21295df13f59fdd8b`. It integrates C1 branch archival records, C2 CI authority separation, C3 shared build surface, C4 oracle-documentation drift guard, bootstrap-surface reconciliation, the Repository Surface extension of Engineering Hygiene Law 5, and `PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` version 0.3.

The exact integration head `45022b945b52143843bb7c565a14267a653b36d8` passed the GitHub validation workflow: **`win-arm64` cross-compile/build PASS** for all four projects, Oracle documentation drift PASS, and advisory x64 Core tests PASS. The cross-compile result is **compiler authority only**; it is not native Windows ARM64 execution/runtime authority and does not replace either Director-machine native checkpoint below.

Existing machine-validated checkpoints now have durable annotated validation tags:

- `validation/e0a-phase-b-reference-run-native-arm64` -> `3749210393282f6aa2ac4ceb0176b6adb5df189e`.
- `validation/e0a-phase-b-live-host-native-arm64` -> `1cfdb3aa22abce62a5bd48e80706407670d1c6a9`.

The seven merged renovation/documentation branches `docs-bootstrap-surface-reconciliation`, `docs/repository-surface-constitution`, `renovation/c1-branch-archive`, `renovation/c2-core-ci`, `renovation/c3-build-surface`, `renovation/c4-oracle-guard`, and `renovation/integrate-c1-c4-docs` are now archivable under Repository Surface law. They remain present until annotated `archive/...` tags are created at their final heads and the branch refs are deleted; their continued presence does not make them active engineering work.

## Native validation

All native evidence below is **Director-machine-sourced** from the Director's native Windows ARM64 machine, not the engineering assistant environment.

### Original Phase-B implementation checkpoint

Exact validated executable/test checkout:

`3749210393282f6aa2ac4ceb0176b6adb5df189e`

Annotated validation tag: `validation/e0a-phase-b-reference-run-native-arm64`.

Observed: Windows `10.0.26200`, ARM64, `win-arm64`, SDK `9.0.317`; clean tracked/staged tree; **622/622 Core tests PASS**; **39/39 Harness tests PASS**; Harness ARM64 build PASS; Missing Raft PASS; generic smoke PASS.

### Live-host completion checkpoint

Exact validated executable/test checkout:

`1cfdb3aa22abce62a5bd48e80706407670d1c6a9`

Annotated validation tag: `validation/e0a-phase-b-live-host-native-arm64`.

Observed: Windows `10.0.26200`, `PROCESSOR_ARCHITECTURE=ARM64`, `dotnet --info` RID `win-arm64`, Host Architecture `arm64`, SDK `9.0.317`; clean tracked/staged tree; no material untracked source/test/fixture files; **622/622 Core tests PASS**; **54/54 Harness tests PASS**; Harness native ARM64 build PASS; Missing Raft PASS; generic smoke PASS; credentialless explicit `e0a-run` PASS with native exit `1`, expected `OPENAI_API_KEY` refusal, no evidence root, and credential remaining absent.

Coverage caveat: the pricing-policy tuple test `PricingPolicy_FreezesSourcePromotionAndConservativeRates` is a **constant-freeze tripwire**, not behavioral pricing validation. Its pass contributes to the suite count but only proves that the compiled frozen constants match the independently maintained expected tuple in the test. It does not establish live provider pricing freshness, provider cache behavior, or runtime spend correctness. Those claims require separate runtime/provider evidence and remain subject to the known pricing-snapshot limitation.

The comparison from `1cfdb3aa22abce62a5bd48e80706407670d1c6a9` to former branch head `9228c77a5b789c4c6a1e0417f889851c6b92730e` contains documentation only: `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, four live-host evidence/host records, and this handoff. The four parallel-main commits after merge base are also documentation-only. The reconciliation therefore changes no executable, test, fixture, or Core surface and does not replace `1cfdb3aa...` as the machine-tested live-host executable/test authority.

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

The live host's `RequireNonStaleSnapshot` remains a date-validity guard, not live pricing verification. It fails closed after the published promotional-guarantee date, but it cannot detect provider changes to pricing terms, cache-write multiplier, long-context threshold, or related billing rules that occur earlier within that date window.

Closing that residual still requires the previously identified short-lived trusted pricing attestation or an equivalent stable machine-readable provider pricing source. This native PASS does not erase or narrow that known limit.

Preserved laws include Access before Context, exact configured-path request/receipt provenance, diagnostic-only streamed deltas, closed-response semantic authority, spend reconciliation before semantic use, fail-closed cancellation/timeout/usage handling, deterministic Integrity/State Authority rejection, atomic accepted causal commit followed immediately by next Opportunity establishment, immutable evidence sealing, and blind-output isolation.

## Validation-host continuity

Future Director Windows ARM64 command sets must be generated from `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

Known host facts include:

- Windows PowerShell `RuntimeInformation.OSArchitecture`, `.ProcessArchitecture`, and `.RuntimeIdentifier` have returned blank and are not trusted architecture gates on this host.
- Trusted architecture probes are `PROCESSOR_ARCHITECTURE=ARM64` plus parsed `dotnet --info` showing `RID: win-arm64` and Host `Architecture: arm64`.
- Expected native stderr can surface as `NativeCommandError` under `$ErrorActionPreference='Stop'`; expected-failure probes must isolate stderr, capture `$LASTEXITCODE` immediately, and assert exit/message/filesystem effects independently.

## Live implementation branches

- `e0a-phase-b-post-audit-hardening`
  - HEAD: `8f5e9a0983beafe247a11f62922b8d2bd5974faa`
  - Divergence from `main` `175aa1f360aa8458fb571ac21295df13f59fdd8b`: **65 ahead / 9 behind**.
  - **OPEN — AWAITING DIRECTOR DISPOSITION, COMPILER GATE FAILING.**
  - COMPILER GATE: **FAIL** — MSTEST0032 at `tests/Ensemble.E0.Harness.Tests/E0ALiveHostReadinessTests.cs:179`. `Assert.AreEqual(300, E0ARunEnvelope.AttemptTimeoutSeconds)` compares a literal with the compile-time constant `AttemptTimeoutSeconds = 300`; with `TreatWarningsAsErrors=true`, the analyzer warning fails Debug and Release compilation. Introduced by `2ff81d471ce62217ec9380b361ed4ec2af4a3ab1`.

- `e0a-gemini-normative-reference-amendment`
  - HEAD: `00b9052fdd4c65cc200ed01c61f3f2a80c10dc62`
  - Divergence from `main` `175aa1f360aa8458fb571ac21295df13f59fdd8b`: **48 ahead / 9 behind**.
  - **OPEN — AWAITING DIRECTOR DISPOSITION, COMPILER GATE FAILING.**
  - COMPILER GATE: **FAIL** — CS0103 at `tests/Ensemble.E0.Harness.Tests/GeminiUsageContractTests.cs:90`, `GeminiThinkingDisclosureTests.cs:22`, and `GeminiMalformedResponseTests.cs:155`. Each references `IntegrityCandidateInput` without importing its namespace, `Ensemble.E0.Core.Integrity`.

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

## Next

Remain inside **E0-A Phase B**. Do not redesign Proposal 0.15 or enter E0-B..G, product Application/persistence/UI, Scene endings, Context optimization, Windows AI/NPU, MSIX/WACK/Store, or later scope.

Repository renovation C1-C4 and the continuity-law updates are promoted to `main`. Their CI cross-compile evidence is compiler authority only and does not replace native runtime evidence. The seven merged renovation/documentation branches are archivable but remain present until their required annotated archive tags are created and the branch refs are deleted.

The next unresolved engineering decision is Director disposition of the two divergent implementation branches recorded above. Neither may be silently merged, discarded, or used as validation authority while its compiler gate is failing.

The next consequential product action remains the first real reference-provider run under the frozen envelope/evidence package. It requires **separate explicit Director authorization** for `OPENAI_API_KEY`, provider credentials, network inference, and spend. Nothing in fake-only native validation, repository renovation, CI cross-compilation, or promotion opens that gate.
