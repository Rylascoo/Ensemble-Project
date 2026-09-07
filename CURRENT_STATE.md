# Ensemble Current State

Updated: 2026-09-07

## Authority

Frozen Blueprint 0.1 plus approved phase/patch blueprints govern architecture. `Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` is the only phase/checkpoint/validation/next-action authority. Evidence and handoff documents supply detail but do not independently advance project state.

Product/policy authority and lane boundaries: `docs/PROJECT_AUTHORITY.md`. The Director owns product constitution, Open Design Register resolution, provider admissibility, and decisions about what the product may do. Engineering/design surfaces supply inputs only within their lanes. ODR proposals/resolutions live in `Ensemble-Project/docs` following the ODR-33 precedent.

Engineering workflow discipline: `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`. Fresh engineering chats must resolve repository state before acting, use the strongest available project-relevant reasoning/capabilities, maximize useful task completion per reply, avoid unnecessary conversational overhead, and recursively audit material work for errors, inconsistencies, ambiguity, regressions, scope drift, and worthwhile improvements until one complete pass finds none.

Program plan: `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`. Open design questions: `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md`.

## Current checkpoint

Phase: **E0-A Experimental Harness — post-audit hardening and the approved Gemini normative-reference amendment are now jointly promoted on `main`. The combined Gemini-on-hardened executable/test checkpoint passed recursive static closure, ARM64-target compiler validation, and fresh Director-machine native Windows ARM64 fake-only/credentialless validation, and has its required annotated validation tag. Real Gemini network execution remains separately gated.**

Promoted Gemini-on-hardened PR:

`#43`

Promotion merge:

`c68d4f80e6aaaa1971eefcc6cf8b3d632e02aeea`

Final promotion branch head:

`f4320c2d774c2fee70ced70421ba23d6e05a5763`

Promoted hardening merge:

`6f211ffe1238c065ad093eaff7e3cedaa3067f66`

Exact combined machine-tested executable/test checkpoint:

`5f286e8cfa896d38d85d4611f69a224fae5b55fd`

Annotated validation tag:

`validation/e0a-phase-b-gemini-on-hardened-native-arm64`

Combined static closure:

`docs/evidence/E0A_GEMINI_ON_HARDENED_MAIN_INTEGRATION_STATIC_CLOSURE.md`

Combined native validation evidence:

`docs/evidence/E0A_GEMINI_ON_HARDENED_MAIN_NATIVE_ARM64_VALIDATION.md`

Director-host apparatus contract:

`docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`

The combined checkpoint is the current promoted machine-tested runtime authority for its exact fake-only/credentialless scope. Promotion does not authorize credentials, `countTokens`, provider-network inference, provider spend, or any later phase.

## Phase-B architecture and boundary

H1 Phase A is **CLOSED** by `docs/evidence/H1_CONVERGENCE_AUDIT.md`; promotion `28371ea4bcd9709b771413100af4dcec5a05dc6e`.

Approved Phase-B parent architecture: `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`, Proposal 0.15; approval: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_APPROVAL.md`.

Approved Gemini amendment: `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`; approval: `docs/evidence/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT_APPROVAL.md`.

The Harness owns the approved reference-run envelope outside Core while reusing closed H1 Cycle/Turn authority. The Gemini amendment changes only the normative E0-A provider route and preserves deterministic Core authority, evidence laws, causal authority, retry/cancellation rules, RunId law, blind-review isolation, and later-phase exclusions except where expressly amended.

Remain inside **E0-A Phase B**. Do not enter E0-B..G, Application/persistence/UI, Scene endings, Context optimization, Windows AI/NPU, MSIX/WACK/Store, or later scope.

## Repository renovation and hardening authority

Repository-renovation PR #40 was promoted at `175aa1f360aa8458fb571ac21295df13f59fdd8b`; post-renovation continuity PR #41 at `62769b3af3e4309204b89bffa3e9e5e24afc5bf7`. These integrated branch archival records, CI authority separation, shared build surface, oracle-documentation drift protection, bootstrap reconciliation, Repository Surface hygiene, and reasoning/task optimization protocol continuity.

Post-audit hardening was integrated through PR #42. Mechanical integration commit: `502e8c10fbaf8873566aab850fbf88a4449211aa`; final PR head: `3c0cf625077d7f57734d229486fa5de404834e74`; merge commit: `6f211ffe1238c065ad093eaff7e3cedaa3067f66`.

Hardening static authority: `docs/evidence/E0A_POST_AUDIT_HARDENING_FINAL_STATIC_CLOSURE.md` and `docs/evidence/E0A_POST_AUDIT_HARDENING_CLOSED_FINDINGS.md`.

Hardening native authority: `docs/evidence/E0A_POST_AUDIT_HARDENING_NATIVE_ARM64_VALIDATION.md`.

Hardening strengthened fail-closed response handling, receipt/usage provenance, spend robustness, evidence ownership/sealing, checkout behavior, evaluation authority, cancellation, provider parsing, timeout authority, and regression coverage without redesigning Proposal 0.15. The one Core-source hardening change remains the narrow E-04 parser normalization in `src/Ensemble.E0.Core/Fixture/StrictJsonPreflight.cs`; Gemini-on-hardened composition adds no further Core source change relative to hardened `main`.

## Durable native validation authorities

All native evidence is Director-machine-sourced from the Director's Windows ARM64 machine.

Annotated validation tags:

- `validation/e0a-phase-b-reference-run-native-arm64` -> `3749210393282f6aa2ac4ceb0176b6adb5df189e`.
- `validation/e0a-phase-b-live-host-native-arm64` -> `1cfdb3aa22abce62a5bd48e80706407670d1c6a9`.
- `validation/e0a-phase-b-post-audit-hardening-native-arm64` -> `5c70f619d6e951d89bb527a5945b014998573dab`.
- `validation/e0a-phase-b-gemini-native-arm64` -> `9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`.
- `validation/e0a-phase-b-gemini-on-hardened-native-arm64` -> `5f286e8cfa896d38d85d4611f69a224fae5b55fd`.

Current promoted runtime authority is exact combined checkout `5f286e8cfa896d38d85d4611f69a224fae5b55fd` for its recorded fake-only/credentialless scope.

Historical original-Gemini native authority remains exact checkout `9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`; it is preserved as historical evidence and does not supersede the composed checkpoint.

## Gemini-on-hardened composition authority

Composition order is frozen: **hardened promoted baseline first, Gemini amendment second**. The original Gemini branch was not merged wholesale over hardened `main`; overlapping Harness/test surfaces were semantically reconciled so the approved Gemini route and all closed hardening laws survive together.

Exact executable/test checkpoint:

`5f286e8cfa896d38d85d4611f69a224fae5b55fd`

Hardened composition base:

`aa9a3d0dfbba9e882b67a7d0d9d1d510374103fd`

The recursive composition audit found and corrected these material cross-branch conflicts before closure:

- **E-02 streaming cancellation:** removed Gemini's synchronous `EndOfStream` probe; cancellable `ReadLineAsync(cancellationToken)` owns stream input and `null` is EOF.
- **E-03 provider parsing:** strict UTF-8 streaming decode, consumed-field JSON type checks, and well-formed provider-string validation prevent malformed provider material from becoming semantics.
- **E-06 spend/provenance:** buffered closed technical outcomes may retain independently validated nonsemantic metadata; streaming usage becomes spend authority only after full stream consumption. Early streaming failures omit usage and trigger hardened full-reservation fallback; fully consumed non-STOP streams may retain validated usage while remaining nonsemantic.
- **E-07 timeout authority:** provider `HttpClient.Timeout = Timeout.InfiniteTimeSpan`; the run driver's linked 300-second attempt token remains sole attempt deadline.
- hardened pricing representability, reservation identity, unknown-usage state, fallback accounting, evidence namespace ownership, write-once publication, runtime/evaluation seals, checkout guard, deterministic Core authority, and blind-evidence laws remain preserved.

Dedicated regression coverage includes cancellation-first streaming, malformed UTF-8, malformed optional fields and Unicode identity, technical provenance retention, later-invalid-stream-usage fallback, and fully consumed non-STOP known usage.

The final recursive static pass found no additional material correction, inconsistency, ambiguity, or worthwhile improvement inside the authorized boundary.

## Compiler / CI authority of combined executable checkpoint

GitHub Actions Validation gate run `34152340592` ran at exact checkout `5f286e8cfa896d38d85d4611f69a224fae5b55fd`.

PASS:

- Core Release build — 0 warnings / 0 errors;
- Harness Release `win-arm64` build — 0 warnings / 0 errors;
- Core test-project Release build — 0 warnings / 0 errors;
- Harness test-project Release build — 0 warnings / 0 errors;
- Oracle documentation drift gate;
- advisory x64 Core tests.

This is ARM64-target cross-compile/compiler authority only; the runner was Ubuntu x64.

PR #43 final head `f4320c2d774c2fee70ced70421ba23d6e05a5763` also passed the PR-triggered compiler gate, oracle-drift gate, and advisory x64 Core tests before merge. Commits after exact executable/test checkpoint `5f286e8c...` were documentation-only.

## Combined native Windows ARM64 validation — PASS

Exact Director-machine validated checkout:

`5f286e8cfa896d38d85d4611f69a224fae5b55fd`

Observed baseline `origin/main`:

`aa9a3d0dfbba9e882b67a7d0d9d1d510374103fd`

Host authority:

- Windows `10.0.26200`;
- `PROCESSOR_ARCHITECTURE=ARM64`;
- repository-selected SDK `9.0.317`;
- RID `win-arm64`;
- Host Architecture `arm64`.

Native results:

```text
Exact checkout / tracked+staged cleanliness PASS
Material-untracked source/test/fixture check  PASS
Core tests                                    PASS 622/622
Harness tests                                 PASS 125/125
Fresh native ARM64 Harness build              PASS
Missing Raft smoke                            PASS
Generic fixture smoke                         PASS
Credentialless Gemini provider edge           PASS expected refusal
Evidence-root absence                         PASS
Post-validation checkout/cleanliness          PASS
OPENAI_API_KEY                                absent
GEMINI_API_KEY                                absent
Gemini credential use                         NOT PERFORMED
Gemini countTokens network request             NOT PERFORMED
Gemini inference                               NOT PERFORMED
Gemini provider-network execution              NOT PERFORMED
Gemini spend                                   NOT PERFORMED
```

Only nonmaterial untracked file observed: root-level `patch0012-local-edit.txt`.

The credentialless run exited `1` with the exact required refusal `GEMINI_API_KEY is required at the E0-A provider edge.` and created no evidence root. The host's known `NativeCommandError` stderr presentation does not invalidate the gate because stderr was isolated, native exit captured immediately, and exit/message/filesystem/credential assertions all passed independently.

Evidence: `docs/evidence/E0A_GEMINI_ON_HARDENED_MAIN_NATIVE_ARM64_VALIDATION.md`.

Annotated validation tag: `validation/e0a-phase-b-gemini-on-hardened-native-arm64`.

This establishes fake-only/credentialless native runtime validity for the exact combined checkout. It does **not** establish live Gemini API compatibility, `countTokens` network behavior, model/project availability, account tier/quota/key type, current provider pricing/data-use state, real implicit-cache behavior, provider usage-accounting correctness under real responses, inference correctness, or provider spend.

## Current Gemini route truth

Only `CREATIVE-NONE` is authorized on the Gemini live path. LOW/MEDIUM/HIGH remain deferred pending provider-specific resource-normalization authority.

```text
Provider                    Google Gemini API
Model                       gemini-2.5-flash
Transport                   generateContent / streamGenerateContent REST
Credential                  GEMINI_API_KEY
Performer thinking          0
Interpreter thinking        0
Integrity thinking          3584
Request candidate cap       4096
Observed generated ceiling  4096 candidate + thought
Model input limit           1,048,576
Model output limit          65,536
```

Integrity pre-spend risk reserves against the model's 65,536 output-token limit because thinking budget is not treated as a hard provider cap. Successful usage maps candidate + thought tokens to generated/output usage. Nonzero implicit-cache usage is recorded and terminates contribution before semantic use. All input is shadow-estimated at the uncached reference rate.

Provider/account tier, model availability, key type, quota, pricing, and data-use terms require fresh verification immediately before any real network run. Date-validity guards are not live provider verification.

No real Gemini request has been made by the integration or native validation work.

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

Future Director Windows ARM64 command sets must follow `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

Known host rules include:

- trust `PROCESSOR_ARCHITECTURE=ARM64` plus parsed `dotnet --info` (`RID: win-arm64`, Host `Architecture: arm64`), not known blank PowerShell `RuntimeInformation` property probes;
- capture `$LASTEXITCODE` immediately after native commands whose status is an oracle;
- isolate expected-failure stderr from `$ErrorActionPreference='Stop'` using temporary stdout/stderr files and temporary `Continue`;
- clear Harness/test outputs before the native test rung when stale checkout output could exist;
- clear the target Harness output again before the authoritative explicit native build;
- hard-stop executable smokes after any failed current-checkout build;
- submit complete interactive `if ... else ...` assignment expressions together.

## Repository Surface cleanup candidates

The remote currently contains `main` plus four completed E0-A work branches. None has a remaining implementation, review, validation, or Director-disposition role.

- `e0a-gemini-on-hardened-main` -> `f4320c2d774c2fee70ced70421ba23d6e05a5763`
  - merged by PR #43; `0` unique commits relative to promoted `main`; archive as merged integration history.
- `e0a-hardening-main-integration` -> `3c0cf625077d7f57734d229486fa5de404834e74`
  - merged by PR #42; `0` unique commits relative to promoted `main`; archive as merged integration history.
- `e0a-phase-b-post-audit-hardening` -> `b3718fb14104df4f8f542fe293f7328a7d3ed430`
  - promoted through PR #42; `0` unique commits relative to promoted `main`; archive as merged/promoted implementation history.
- `e0a-gemini-normative-reference-amendment` -> `1810292ff3d14f78d80c4e5064e7a7495f718a6d`
  - original Gemini implementation/evidence line; `50` unique historical commits and `112` behind promoted `main`; superseded by the semantically composed Gemini-on-hardened route. Archive as **superseded unique-history**, not as a merged branch.

Repository Surface law requires an annotated `archive/...` tag at each final branch head before deleting its branch ref. The original Gemini line's existing native validation tag remains valid historical machine-test evidence and is separate from its branch-archive tag.

## Standing decisions

- .NET 10 requires later Director approval.
- `IPackageValidator` is not a mandatory first-release gate.
- E5c exception-runtime provenance remains open with no verified defect/fix.
- Stage remains design-lane authority.
- ODR-12/13/26/30/32 remain open; none blocks E0-A.
- `docs/ODR_26_COST_GOVERNANCE_AND_PROVIDER_ADMISSION_WORKING_CONTRACT.md` remains a working structure only: not frozen/resolved/project law and no E0-A/E0-B provider-selection effect.
- The ODR-26 working contract was drafted outside its authoring surface's lane and is reported for Director disposition rather than adjudicated by engineering.
- ODR proposal/resolution authority belongs to the Director and is filed in `Ensemble-Project/docs` regardless of originating lane.
- Real provider credentials/network execution/spend require a separate explicit Director gate.

## Next

Immediate repository task:

1. create annotated archive tags at the exact four branch heads listed above, recording former branch, disposition, and archive date;
2. push those tags, delete the four completed branch refs, and verify the remote branch surface contains only `main`;
3. reconcile `CURRENT_STATE.md` after archival if any branch-surface fact changes.

After Repository Surface cleanup, the next consequential **product** action is the first real Gemini E0-A `CREATIVE-NONE` reference-provider run. It is **not authorized yet**. Before any provider request, the Director must separately authorize credentials/network/spend and the engineering surface must immediately re-verify the exact project/account/key/model availability, quota/tier, pricing, data-use terms, and other provider assumptions required by the approved Gemini amendment.

No real provider credential, `countTokens` request, provider-network inference, provider spend, E0-B+, product UI/persistence, NPU, packaging, WACK, or Store work is authorized by the current state.