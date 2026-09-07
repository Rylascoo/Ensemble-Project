# Ensemble Current State

Updated: 2026-09-07

## Authority

Frozen Blueprint 0.1 + approved phase/patch blueprints govern architecture. `Rylascoo/Ensemble-Project` is engineering authority. Resolve `main`; never promote or overstate validation. Workflow: `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`. Open questions: `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md`. Plan: `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`. Product/policy authority and lane boundaries: `docs/PROJECT_AUTHORITY.md`.

The Director owns product constitution, Open Design Register resolution, provider admissibility, and decisions about what the product may do. Engineering/design surfaces supply inputs within their lanes. ODR proposals/resolutions live in `Ensemble-Project/docs` following the ODR-33 precedent.

## Checkpoint

Phase: **E0-A Experimental Harness — post-audit hardening has completed credentialless native Windows ARM64 validation and is integrated onto renovated `main` in PR #42 pending promotion; the separately approved Gemini normative-reference amendment has its own credentialless native PASS and must be reconciled onto the hardened baseline next; real provider execution remains gated**.

H1 Phase A: **CLOSED** by `docs/evidence/H1_CONVERGENCE_AUDIT.md`; promotion `28371ea4bcd9709b771413100af4dcec5a05dc6e`.

Approved Phase-B architecture: `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`, Proposal 0.15; approval: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_APPROVAL.md`.

Original Phase-B implementation evidence: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_IMPLEMENTATION_EVIDENCE.md`; native evidence: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_NATIVE_ARM64_VALIDATION.md`; oracle: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_ORACLE.json`.

Live-host completion native evidence: `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_ARM64_VALIDATION.md`. Attempt 01 partial evidence: `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_VALIDATION_ATTEMPT_01.md`. Director-host apparatus contract: `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

Program map: `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`.

This state reconciles former `main` `a3b5126dad440d2f75bef655fdc6e4ac7098b839` with former live-host branch head `9228c77a5b789c4c6a1e0417f889851c6b92730e` as a union. Their merge base was `13ffde90f78a8bf40a04381b136e846d1cf64d1c`. Parallel-main ODR-26 continuity and the live-host validation/authority continuity both survive.

## Repository renovation checkpoint

Repository-renovation integration PR #40 was promoted to `main` at `175aa1f360aa8458fb571ac21295df13f59fdd8b`; post-renovation continuity PR #41 was promoted at `62769b3af3e4309204b89bffa3e9e5e24afc5bf7`. Together they integrate C1 branch archival records, C2 CI authority separation, C3 shared build surface, C4 oracle-documentation drift guard, bootstrap-surface reconciliation, the Repository Surface extension of Engineering Hygiene Law 5, and `PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` version 0.3.

The exact renovation integration head `45022b945b52143843bb7c565a14267a653b36d8` passed the GitHub validation workflow: **`win-arm64` cross-compile/build PASS** for all four projects, Oracle documentation drift PASS, and advisory x64 Core tests PASS. The cross-compile result is **compiler authority only**; it is not native Windows ARM64 execution/runtime authority and does not replace Director-machine native checkpoints.

Repository-surface retirement is complete for the eight maintenance/continuity branches created by the renovation cycle. Each former branch was first verified as a strict ancestor of `main` with zero unique commits, then replaced by an annotated `archive/...` tag at its exact final head recording former branch name, disposition, and archive date; the corresponding branch ref was deleted.

## Native validation

All native evidence below is **Director-machine-sourced** from the Director's native Windows ARM64 machine, not the engineering assistant environment.

Durable annotated validation tags include:

- `validation/e0a-phase-b-reference-run-native-arm64` -> `3749210393282f6aa2ac4ceb0176b6adb5df189e`.
- `validation/e0a-phase-b-live-host-native-arm64` -> `1cfdb3aa22abce62a5bd48e80706407670d1c6a9`.
- `validation/e0a-phase-b-post-audit-hardening-native-arm64` -> `5c70f619d6e951d89bb527a5945b014998573dab`.
- `validation/e0a-phase-b-gemini-native-arm64` -> `9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`.

### Original Phase-B implementation checkpoint

Exact validated executable/test checkout:

`3749210393282f6aa2ac4ceb0176b6adb5df189e`

Observed: Windows `10.0.26200`, ARM64, `win-arm64`, SDK `9.0.317`; clean tracked/staged tree; **622/622 Core tests PASS**; **39/39 Harness tests PASS**; Harness ARM64 build PASS; Missing Raft PASS; generic smoke PASS.

### Live-host completion checkpoint

Exact validated executable/test checkout:

`1cfdb3aa22abce62a5bd48e80706407670d1c6a9`

Observed: Windows `10.0.26200`, `PROCESSOR_ARCHITECTURE=ARM64`, `dotnet --info` RID `win-arm64`, Host Architecture `arm64`, SDK `9.0.317`; clean tracked/staged tree; no material untracked source/test/fixture files; **622/622 Core tests PASS**; **54/54 Harness tests PASS**; Harness native ARM64 build PASS; Missing Raft PASS; generic smoke PASS; credentialless explicit `e0a-run` PASS with native exit `1`, expected `OPENAI_API_KEY` refusal, no evidence root, and credential remaining absent.

### Post-audit hardening checkpoint

Exact machine-tested executable/test checkout:

`5c70f619d6e951d89bb527a5945b014998573dab`

Annotated validation tag: `validation/e0a-phase-b-post-audit-hardening-native-arm64`.

Evidence: `docs/evidence/E0A_POST_AUDIT_HARDENING_NATIVE_ARM64_VALIDATION.md`.

Observed on the Director's Windows ARM64 host: exact detached checkout; clean tracked/staged tree; no material untracked source/test/fixture files; `PROCESSOR_ARCHITECTURE=ARM64`; Windows `10.0.26200`; SDK `9.0.317`; RID `win-arm64`; Host Architecture `arm64`; **622/622 Core tests PASS**; **88/88 Harness tests PASS** after clearing Harness/test build outputs; fresh explicit Harness native ARM64 build PASS after clearing target output again; Missing Raft PASS; generic smoke PASS; credentialless explicit OpenAI-edge `e0a-run CREATIVE-NONE` PASS with native exit `1`, exact expected missing-`OPENAI_API_KEY` refusal, no evidence root, and both provider credentials absent; post-validation exact checkout and cleanliness PASS.

Native Attempt 01 had first closed the earlier MSTEST0032 compiler blocker, then failed only `BufferedWrongJsonType_FailsClosedAsTechnicalReceipt` at 87/88 because the test retained the older `provider-incomplete` diagnostic after E-06 intentionally refined malformed response handling to `malformed-provider-response`. Repair `5c70f619...` changed that one expected diagnostic string only; no runtime source changed. Rerun 02 then passed the complete sequence.

The interactive Rerun 02 transcript contained two standalone PowerShell `else` parser messages caused by entering multi-line `if`/`else` assignments as separate interactive commands. Those messages occurred after each `if` assignment had completed and do not invalidate the credentialless gate: the subsequent assertions for exit, exact refusal message, absent evidence root, absent credentials, and post-cleanliness all passed. The filed canonical handoff uses single-line `if ... else ...` assignments.

### Hardening-on-renovated-main integration checkpoint

PR #42 integration commit:

`502e8c10fbaf8873566aab850fbf88a4449211aa`

Parents:

- renovated/main continuity: `67de40f4b76075dbf7feb3ad00aee8ed7f0bbbed`;
- hardening evidence head: `b3718fb14104df4f8f542fe293f7328a7d3ed430`.

The integration tree was mechanically composed so current `main` supplies repository-renovation files, README, and promoted continuity while the hardening parent supplies its `src/`, `tests/`, evidence, and handoff surface. Recursive two-parent comparison confirms no hardening executable/test byte changed during integration and no renovation artifact was dropped. From the common hardening baseline to renovated `main`, no `.csproj`, source, test, or fixture file changed; `.editorconfig` adds advisory/suggestion formatting/style guidance only.

The exact integration commit `502e8c10...` passed the GitHub validation workflow: ARM64 cross-compile compiler gate PASS for Core, Harness, Core tests, and Harness tests; Oracle documentation drift PASS; advisory x64 Core tests PASS. That is compiler/CI authority for the integration commit only. Native Windows ARM64 runtime authority remains the tagged exact hardening checkout `5c70f619...` because that is the executable/test checkout actually run on the Director machine.

### Gemini normative-reference amendment checkpoint — live branch, not yet composed with hardening

Exact validated executable/test checkout:

`9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`

Annotated validation tag: `validation/e0a-phase-b-gemini-native-arm64`.

Branch evidence: `docs/evidence/E0A_PHASE_B_GEMINI_NATIVE_ARM64_VALIDATION.md` on `e0a-gemini-normative-reference-amendment`.

Observed on the Director's Windows ARM64 host: exact checkout and clean tracked/staged tree with only the known nonmaterial root file; `PROCESSOR_ARCHITECTURE=ARM64`; Windows `10.0.26200`; SDK `9.0.317`; RID `win-arm64`; Host Architecture `arm64`; **622/622 Core tests PASS**; **81/81 Harness tests PASS**; fresh clean-output Harness native ARM64 build PASS; Missing Raft PASS; generic smoke PASS; credentialless explicit Gemini `e0a-run CREATIVE-NONE` PASS with native exit `1`, expected `GEMINI_API_KEY` refusal, no evidence root, and both provider credentials absent.

This is native credentialless/fake-only authority for exact Gemini branch checkpoint `9bb65a846...`. It is not native authority for the future combined hardening+Gemini integration checkout and does not authorize Gemini `countTokens`, inference, credential use, provider-network execution, or spend.

Coverage caveat: pricing-policy constant tests are constant-freeze tripwires, not live provider-pricing validation. Native fake-only/credentialless validation does not establish live pricing freshness, provider cache billing, provider spend correctness, or real provider behavior.

No real provider request, provider credential use, provider-network execution, or provider spend has been executed or authorized by the native validations above. No WinUI, Windows AI/NPU, MSIX/WACK, or Store authority.

## Phase-B truth

The Harness owns the approved reference-run envelope outside Core while reusing the closed H1 Cycle/Turn authority unchanged.

The historical OpenAI implementation remains the origin of the Phase-B reference envelope. Post-audit hardening strengthens its fail-closed response handling, receipt/usage provenance, spend robustness, evidence ownership/sealing, deterministic authority, checkout behavior, and regression coverage without redesigning Proposal 0.15.

The separately approved Gemini normative-reference amendment changes only the next normative E0-A provider route. Its blueprint explicitly preserves existing deterministic H1/Core authority, evidence laws, causal authority, retry/cancellation rules, RunId law, blind-review isolation, and later-phase exclusions unless specifically amended. Because post-audit hardening strengthens those preserved laws, the correct composition order is **hardening baseline first, Gemini amendment second**. The original Gemini branch must not be merged directly over the hardened baseline if doing so would discard overlapping hardening changes.

Historical OpenAI approved variants were:

- `CREATIVE-NONE` — Performer/Interpreter `none`, Integrity `high`; normative reference run.
- `CREATIVE-LOW` — Performer/Interpreter `low`, Integrity `high`; matched characterization.
- `CREATIVE-MEDIUM` — Performer/Interpreter `medium`, Integrity `high`; matched characterization.
- `CREATIVE-HIGH` — Performer/Interpreter `high`, Integrity `high`; matched characterization.

The Gemini amendment authorizes only `CREATIVE-NONE` on the Gemini live path and defers provider-specific mappings for LOW/MEDIUM/HIGH.

Frozen cross-provider envelope laws remain: 12 accepted-Turn cap; one attempt per probabilistic invocation; zero automatic retries; 300-second attempt timeout; 4096 intended generated-token ceiling per role; estimated model-token spend ceiling USD 5.00/run; no tools/provider conversation/hidden reasoning. Provider-specific request, pricing, caching, token-count, reasoning-budget, and credential controls are governed by the exact approved route rather than generalized across providers.

The live host validates exact clean checkout, evidence-root absence, fixture/Missing-Raft contract, and applicable pricing/provider snapshot validity before credential access. It exposes explicit `e0a-run` and independent post-run `e0a-evaluate` paths and preserves write-once/runtime-root evidence sealing.

Date-validity guards are not live provider verification. Provider/account tier, model availability, key type, pricing, quota, and data-use terms require re-verification before any real network run.

Preserved laws include Access before Context, exact configured-path request/receipt provenance, diagnostic-only streamed deltas, closed-response semantic authority, spend reconciliation before semantic use, fail-closed cancellation/timeout/usage handling, deterministic Integrity/State Authority rejection, atomic accepted causal commit followed immediately by next Opportunity establishment, immutable evidence sealing, and blind-output isolation.

## Validation-host continuity

Future Director Windows ARM64 command sets must be generated from `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

Known host facts include:

- Windows PowerShell `RuntimeInformation.OSArchitecture`, `.ProcessArchitecture`, and `.RuntimeIdentifier` have returned blank and are not trusted architecture gates on this host.
- Trusted architecture probes are `PROCESSOR_ARCHITECTURE=ARM64` plus parsed `dotnet --info` showing `RID: win-arm64` and Host `Architecture: arm64`.
- Expected native stderr can surface as `NativeCommandError` under `$ErrorActionPreference='Stop'`; expected-failure probes must isolate stderr, capture `$LASTEXITCODE` immediately, and assert exit/message/filesystem effects independently.
- After any failed Harness build, later fixture or credentialless output cannot be counted unless a fresh successful build for the exact checkout precedes it.
- When PowerShell expressions containing `if ... else ...` are intended for interactive entry, keep the complete expression on one submitted command or otherwise ensure the parser receives the `else` as part of the same statement.

## Live implementation / integration branches

- `e0a-hardening-main-integration`
  - Integration checkpoint before this continuity update: `502e8c10fbaf8873566aab850fbf88a4449211aa`.
  - Exact machine-tested hardening executable/test checkpoint: `5c70f619d6e951d89bb527a5945b014998573dab`.
  - Native tag: `validation/e0a-phase-b-post-audit-hardening-native-arm64`.
  - **PR #42 — PROMOTION CANDIDATE. Native hardening PASS; integration compiler/oracle gates PASS; continuity reconciliation in progress.**

- `e0a-phase-b-post-audit-hardening`
  - Historical implementation/evidence branch now subsumed by the PR #42 integration candidate if PR #42 promotes successfully.
  - Evidence head: `b3718fb14104df4f8f542fe293f7328a7d3ed430`.
  - Do not use its stale branch `CURRENT_STATE.md`/README as promoted project authority.

- `e0a-gemini-normative-reference-amendment`
  - Documentation/evidence branch head after native validation: `1810292ff3d14f78d80c4e5064e7a7495f718a6d`.
  - Exact machine-tested source/test checkpoint: `9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`.
  - Native tag: `validation/e0a-phase-b-gemini-native-arm64`.
  - **OPEN — original Gemini branch credentialless native PASS; must be reconciled onto the hardened baseline after hardening promotion.**
  - Its existing native PASS validates only `9bb65a846...`, not the future combined hardening+Gemini checkout.

## Standing decisions

- .NET 10 requires later Director approval.
- `IPackageValidator` is not a mandatory first-release gate.
- E5c exception-runtime provenance: open, no verified defect/fix.
- Stage remains design-lane authority.
- ODR-12/13/26/30/32 remain open; none blocks E0-A.
- ODR-26 working structure: `docs/ODR_26_COST_GOVERNANCE_AND_PROVIDER_ADMISSION_WORKING_CONTRACT.md`. **Not frozen/resolved/project law; no E0-A/E0-B provider-selection effect.**
- The ODR-26 working contract was drafted outside its authoring surface's lane and is reported for Director disposition rather than adjudicated or corrected by engineering.
- ODR proposal/resolution authority belongs to the Director and is filed in `Ensemble-Project/docs` regardless of originating lane.
- Real provider credentials/network execution/spend require a separate explicit Director gate.

## Next

Remain inside **E0-A Phase B**. Do not redesign Proposal 0.15 or enter E0-B..G, product Application/persistence/UI, Scene endings, Context optimization, Windows AI/NPU, MSIX/WACK/Store, or later scope.

Immediate sequence:

1. complete PR #42 recursive promotion audit and repository validation on the final continuity head;
2. promote hardening onto `main` if that audit remains clean;
3. construct a new Gemini-on-hardened-main integration candidate that preserves both the hardening laws and the approved Gemini normative-reference amendment;
4. recursively audit that combined source/test/evidence surface for semantic conflicts, stale provider assumptions, test-oracle drift, and repository hygiene;
5. require fresh native Windows ARM64 validation of the combined hardening+Gemini executable/test checkout before it becomes machine-tested authority or can be promoted;
6. only after combined validation and promotion consider the separately gated first real Gemini reference-provider run.

The existing Gemini native PASS does not eliminate step 5 because it predates hardening composition. The existing hardening native PASS does not authorize Gemini network execution. No real provider credential, provider-network inference, provider spend, E0-B+, product UI/persistence, NPU, packaging, WACK, or Store work is authorized by the current state.