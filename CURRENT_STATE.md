# Ensemble Current State

Updated: 2026-09-07

## Authority

Frozen Blueprint 0.1 plus approved phase/patch blueprints govern architecture. `Rylascoo/Ensemble-Project` is engineering authority. Resolve repository state fresh; never promote or overstate validation. Workflow discipline: `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`. Open questions: `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md`. Program plan: `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`. Product/policy authority and lane boundaries: `docs/PROJECT_AUTHORITY.md`.

The Director owns product constitution, Open Design Register resolution, provider admissibility, and decisions about what the product may do. Engineering/design surfaces supply inputs within their own lanes. ODR proposals/resolutions live in `Ensemble-Project/docs` following the ODR-33 precedent.

`CURRENT_STATE.md` is the only phase/checkpoint/validation/next-action authority. Evidence and handoff documents supply detail but do not independently advance project state.

## Current checkpoint

Phase: **E0-A Experimental Harness — post-audit hardening is promoted and natively validated; the approved Gemini normative-reference amendment has now been semantically composed onto hardened `main`, recursively statically closed, and ARM64 cross-compile/compiler-gated; the exact combined executable/test checkpoint is pending fresh Director-machine native Windows ARM64 validation. Real Gemini network execution remains gated.**

Current promoted `main` continuity head:

`aa9a3d0dfbba9e882b67a7d0d9d1d510374103fd`

Promoted hardening merge:

`6f211ffe1238c065ad093eaff7e3cedaa3067f66`

Active integration branch:

`e0a-gemini-on-hardened-main`

Exact combined executable/test checkpoint frozen for native validation:

`5f286e8cfa896d38d85d4611f69a224fae5b55fd`

Static closure:

`docs/evidence/E0A_GEMINI_ON_HARDENED_MAIN_INTEGRATION_STATIC_CLOSURE.md`

Native validation handoff:

`docs/handoff/E0A_GEMINI_ON_HARDENED_MAIN_NATIVE_ARM64_VALIDATION_HANDOFF.md`

Director-host apparatus contract:

`docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`

The combined checkpoint is **not yet machine-tested runtime authority**. Do not promote or merge it on the strength of the older independent hardening/Gemini native passes.

## Phase-B architecture and product boundary

H1 Phase A is **CLOSED** by `docs/evidence/H1_CONVERGENCE_AUDIT.md`; promotion `28371ea4bcd9709b771413100af4dcec5a05dc6e`.

Approved Phase-B parent architecture: `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`, Proposal 0.15; approval: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_APPROVAL.md`.

Approved Gemini amendment: `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`; approval evidence: `docs/evidence/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT_APPROVAL.md`.

The Harness owns the approved reference-run envelope outside Core while reusing closed H1 Cycle/Turn authority. The Gemini amendment changes only the next normative provider route and explicitly preserves deterministic Core authority, evidence laws, causal authority, retry/cancellation rules, RunId law, blind-review isolation, and later-phase exclusions except where the amendment expressly says otherwise.

Remain inside **E0-A Phase B**. Do not enter E0-B..G, Application/persistence/UI, Scene endings, Context optimization, Windows AI/NPU, MSIX/WACK/Store, or later scope.

## Repository renovation and hardening authority

Repository-renovation PR #40 was promoted at `175aa1f360aa8458fb571ac21295df13f59fdd8b`; post-renovation continuity PR #41 at `62769b3af3e4309204b89bffa3e9e5e24afc5bf7`. These integrated branch archival records, CI authority separation, shared build surface, oracle-documentation drift protection, bootstrap reconciliation, Repository Surface hygiene, and reasoning-task optimization protocol v0.3.

Post-audit hardening was then integrated through PR #42. Mechanical integration commit: `502e8c10fbaf8873566aab850fbf88a4449211aa`; final PR head: `3c0cf625077d7f57734d229486fa5de404834e74`; merge commit: `6f211ffe1238c065ad093eaff7e3cedaa3067f66`.

Hardening static authority: `docs/evidence/E0A_POST_AUDIT_HARDENING_FINAL_STATIC_CLOSURE.md` and `docs/evidence/E0A_POST_AUDIT_HARDENING_CLOSED_FINDINGS.md`.

Hardening native authority: `docs/evidence/E0A_POST_AUDIT_HARDENING_NATIVE_ARM64_VALIDATION.md`.

Hardening strengthened fail-closed response handling, receipt/usage provenance, spend robustness, evidence ownership/sealing, checkout behavior, evaluation authority, cancellation, provider parsing, timeout authority, and regression coverage without redesigning Proposal 0.15. The one Core-source hardening change remains the narrow E-04 parser normalization in `src/Ensemble.E0.Core/Fixture/StrictJsonPreflight.cs`; the current Gemini integration adds **no Core source change** relative to hardened `main`.

## Durable native validation authorities

All native evidence below is Director-machine-sourced from the Director's Windows ARM64 machine.

Annotated validation tags:

- `validation/e0a-phase-b-reference-run-native-arm64` -> `3749210393282f6aa2ac4ceb0176b6adb5df189e`.
- `validation/e0a-phase-b-live-host-native-arm64` -> `1cfdb3aa22abce62a5bd48e80706407670d1c6a9`.
- `validation/e0a-phase-b-post-audit-hardening-native-arm64` -> `5c70f619d6e951d89bb527a5945b014998573dab`.
- `validation/e0a-phase-b-gemini-native-arm64` -> `9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`.

### Current promoted hardening runtime authority

Exact machine-tested hardening checkout:

`5c70f619d6e951d89bb527a5945b014998573dab`

Observed on the Director host: exact/clean checkout; trusted ARM64 host probes; **622/622 Core tests PASS**; **88/88 Harness tests PASS**; fresh native ARM64 Harness build PASS; Missing Raft PASS; generic smoke PASS; credentialless explicit OpenAI-edge run PASS with expected missing-key refusal and no evidence-root creation; post-validation checkout/cleanliness PASS.

This remains the native runtime authority behind current promoted `main` until the combined Gemini checkpoint independently passes its own native gate.

### Historical original-Gemini runtime authority

Exact original Gemini machine-tested checkout:

`9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`

Evidence: `docs/evidence/E0A_PHASE_B_GEMINI_NATIVE_ARM64_VALIDATION.md`.

Observed on the Director host: **622/622 Core tests PASS**; **81/81 Harness tests PASS**; fresh native ARM64 Harness build PASS; Missing Raft PASS; generic smoke PASS; credentialless Gemini provider-edge PASS with expected missing-`GEMINI_API_KEY` refusal and no evidence-root creation.

That PASS remains valid only for exact original Gemini checkout `9bb65a846...`. It does not validate the combined hardening+Gemini executable and does not authorize token-count network traffic, Gemini inference, credentials, provider-network execution, or spend.

Historical earlier Phase-B checkpoints `374921039...` and `1cfdb3aa...` remain valid only for their recorded scopes.

## Gemini-on-hardened integration — static closure

Composition order is frozen by preserved authority: **hardened promoted baseline first, Gemini amendment second**. The original Gemini branch was therefore not merged wholesale over hardened `main`. Overlapping Harness/test surfaces were semantically reconciled so both the approved Gemini route and the closed hardening laws survive.

The combined executable/test checkpoint is:

`5f286e8cfa896d38d85d4611f69a224fae5b55fd`

It is based on hardened continuity head `aa9a3d0...`, with no Core source change from that baseline.

The recursive composition audit found and corrected material cross-branch conflicts before closure:

- **E-02 streaming cancellation:** removed Gemini's synchronous `EndOfStream` probe; cancellable `ReadLineAsync(cancellationToken)` is the stream input authority and `null` is EOF.
- **E-03 provider parsing:** strict UTF-8 streaming decode, explicit consumed-field JSON type checks, and well-formed provider-string validation prevent malformed provider data from becoming semantics.
- **E-06 spend/provenance:** closed buffered technical outcomes may retain independently validated nonsemantic response/model/usage metadata; streaming usage becomes spend authority only after full stream consumption. Early streaming failure omits usage and therefore commits the hardened full-reservation fallback rather than risk using a provisional/understated tuple. Fully consumed non-STOP streams may retain validated usage while remaining nonsemantic.
- **E-07 timeout authority:** provider `HttpClient.Timeout = Timeout.InfiniteTimeSpan`; the run driver's linked 300-second attempt token remains the sole attempt deadline.
- Hardened pricing representability, reservation identity, unknown-usage state, fallback accounting, evidence namespace ownership, write-once publication, runtime/evaluation seals, checkout guard, deterministic Core authority, and blind evidence laws remain preserved.

Dedicated regression coverage was added for cancellation-first streaming, malformed UTF-8, malformed optional fields and Unicode identity, technical provenance retention, later-invalid-stream-usage fallback, and fully consumed non-STOP known usage.

After the last correction, one complete recursive pass found no further material correction, inconsistency, ambiguity, or worthwhile implementation improvement inside the authorized boundary. See the static-closure record for the complete disposition.

## Compiler / CI state of combined checkpoint

GitHub Actions Validation gate run `34152340592` ran at exact checkout `5f286e8cfa896d38d85d4611f69a224fae5b55fd`.

PASS:

- Core Release build — 0 warnings / 0 errors;
- Harness Release `win-arm64` build — 0 warnings / 0 errors;
- Core test-project Release build — 0 warnings / 0 errors;
- Harness test-project Release build — 0 warnings / 0 errors;
- Oracle documentation drift gate;
- advisory x64 Core tests.

This establishes **ARM64-target cross-compile/compiler authority only**. The runner was Ubuntu x64. It is not native Windows ARM64 execution/runtime authority and establishes nothing about NPU, packaging/WACK, or Store certification.

## Current Gemini route truth

Only `CREATIVE-NONE` is authorized on the Gemini live path by the approved amendment. LOW/MEDIUM/HIGH remain deferred pending a provider-specific resource-normalization amendment.

Current frozen route:

```text
Provider              Google Gemini API
Model                 gemini-2.5-flash
Transport             native generateContent / streamGenerateContent REST
Credential            GEMINI_API_KEY
Performer thinking    0
Interpreter thinking  0
Integrity thinking    3584
Request output cap    4096 candidate tokens
Observed generated ceiling 4096 candidate + thought tokens
Model input limit     1,048,576
Model output limit    65,536
```

Integrity pre-spend risk reserves against the 65,536 model output limit because thinking budget is not treated as a hard provider cap. Successful usage maps candidate + thought tokens into generated/output usage. Nonzero implicit-cache usage is recorded and terminates contribution before semantic use. All input is shadow-estimated at the uncached reference rate.

Provider/account tier, model availability, key type, quota, pricing, and data-use terms require fresh verification immediately before any real network run. Date-validity guards are not live provider verification.

On 2026-09-07, current official Google API documentation was statically rechecked and remained consistent with the integrated countTokens wrapper, GenerateContent/streamGenerateContent request fields, response identity/usage fields, structured-output surface, model limits, and dated shadow pricing assumptions. This static verification does not establish the Director's exact project/account availability or live API compatibility.

No real Gemini request has been made by the integration work.

## Preserved E0-A laws

Preserved laws include:

- Access before Context;
- exact configured-path request/receipt provenance;
- single-turn/stateless provider requests;
- no provider tools or conversation/history identity;
- diagnostic-only streaming until closed semantic authority exists;
- technical/refusal/timeout/malformed/non-STOP/usage-policy failures cannot become fiction;
- spend reconciliation or conservative fallback before semantic consumption;
- deterministic Integrity and State Authority;
- atomic accepted causal commit followed immediately by next Opportunity establishment;
- immutable/write-once runtime evidence and separate blind hard-gate evaluation;
- one attempt per probabilistic invocation, zero automatic retries, 300-second attempt deadline;
- 12 accepted-Turn cap and USD 5.00 shadow-estimate run ceiling.

## Validation-host continuity

All future Director Windows ARM64 command sets must follow `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

Known host rules include:

- trust `PROCESSOR_ARCHITECTURE=ARM64` plus parsed `dotnet --info` (`RID: win-arm64`, Host `Architecture: arm64`), not the known blank PowerShell `RuntimeInformation` property probes;
- capture `$LASTEXITCODE` immediately after every native command whose status is an oracle;
- expected-failure stderr must be isolated from `$ErrorActionPreference='Stop'` with temporary stdout/stderr files and a temporary `Continue` setting;
- delete stale Harness target output before the authoritative build;
- hard-stop executable smokes after any failed current-checkout Harness build;
- in interactive PowerShell, submit complete `if ... else ...` assignment expressions together.

## Live branches

- `e0a-gemini-on-hardened-main`
  - **ACTIVE — static/compiler closure complete; exact executable/test checkpoint `5f286e8c...` pending fresh native Windows ARM64 validation.**
  - static closure: `docs/evidence/E0A_GEMINI_ON_HARDENED_MAIN_INTEGRATION_STATIC_CLOSURE.md`.
  - validation handoff: `docs/handoff/E0A_GEMINI_ON_HARDENED_MAIN_NATIVE_ARM64_VALIDATION_HANDOFF.md`.

- `e0a-gemini-normative-reference-amendment`
  - historical original-Gemini line; exact native checkpoint `9bb65a846...`; tag `validation/e0a-phase-b-gemini-native-arm64`.
  - no longer the implementation target; do not merge it wholesale over hardened `main`.

- `e0a-phase-b-post-audit-hardening`
  - implementation/evidence history promoted through PR #42; branch is no longer open implementation work and is Repository-Surface archival material once its required archive tag is filed.

- `e0a-hardening-main-integration`
  - PR #42 integration history; merged and likewise eligible for archival under Repository Surface law after its required archive tag is filed.

## Standing decisions

- .NET 10 requires later Director approval.
- `IPackageValidator` is not a mandatory first-release gate.
- E5c exception-runtime provenance remains open with no verified defect/fix.
- Stage remains design-lane authority.
- ODR-12/13/26/30/32 remain open; none blocks E0-A.
- ODR-26 working structure: `docs/ODR_26_COST_GOVERNANCE_AND_PROVIDER_ADMISSION_WORKING_CONTRACT.md`. It is **not frozen/resolved/project law and has no E0-A/E0-B provider-selection effect**.
- The ODR-26 working contract was drafted outside its authoring surface's lane and is reported for Director disposition rather than adjudicated by engineering.
- ODR proposal/resolution authority belongs to the Director and is filed in `Ensemble-Project/docs` regardless of originating lane.
- Real provider credentials/network execution/spend require a separate explicit Director gate.

## Next

The immediate and only implementation-validation next step is:

1. validate exact combined executable/test checkout `5f286e8cfa896d38d85d4611f69a224fae5b55fd` on the Director's native Windows ARM64 machine using `docs/handoff/E0A_GEMINI_ON_HARDENED_MAIN_NATIVE_ARM64_VALIDATION_HANDOFF.md` and the authoritative host-behavior contract;
2. require exact checkout/cleanliness, trusted host probes, native Core tests, native Harness tests, fresh native ARM64 Harness build, Missing Raft smoke, generic fixture smoke, and credentialless Gemini expected-refusal with both provider credentials absent and no evidence-root creation;
3. if native validation passes, file the exact Director-machine evidence and create/push an annotated validation tag at **exactly `5f286e8c...`** before promoting it as machine-tested authority;
4. recursively audit the post-validation evidence/continuity update and only then consider PR/promotion of the combined Gemini-on-hardened line;
5. a first real Gemini provider run remains a later, separately gated Director decision and additionally requires immediate project/model/account/quota/pricing/data-use re-verification.

No real provider credential, token-count network request, inference, provider spend, E0-B+, product UI/persistence, NPU, packaging, WACK, or Store work is authorized by the current state.
