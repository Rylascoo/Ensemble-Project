# Ensemble Current State

Updated: 2026-09-06

## Authority

Frozen Blueprint 0.1 + approved phase/patch blueprints govern architecture. `Rylascoo/Ensemble-Project` is engineering authority. Resolve `main` for promoted project truth and any named active work branch for its bounded in-progress state; never promote or overstate validation. Workflow: `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`. Open questions: `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md`. Plan: `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`. Product/policy authority and lane boundaries: `docs/PROJECT_AUTHORITY.md`.

The Director owns product constitution, Open Design Register resolution, provider admissibility, and decisions about what the product may do. Engineering/design surfaces supply inputs within their lanes. ODR proposals/resolutions live in `Ensemble-Project/docs` following the ODR-33 precedent.

## Checkpoint

Phase: **E0-A Experimental Harness — Gemini normative-reference amendment implementation complete; recursive static audit CLOSED; exact source/test checkpoint frozen; Director-machine native Windows ARM64 validation pending; real Gemini network execution still gated**.

Active work branch:

`e0a-gemini-normative-reference-amendment`

Promoted `main` before this amendment:

`f9cb1a79ad7923b640ff1a97c46d8fd6ab9dac25`

Frozen Gemini amendment executable/source/test checkpoint:

`39e7984b9ff59228d7d0c424ef7d113582d38bc7`

The recursive static audit closed at that checkpoint after one complete pass found no material correction, authority inconsistency, scope violation, unsupported validation claim, or remaining worthwhile in-scope improvement. This is a **static-audit closure only**; it is not compiler, test, native-runtime, or provider validation.

Post-freeze continuity/evidence descendants on the active branch are documentation-only and do not replace `39e7984b...` as the executable/source/test authority for the pending native run. Pending-native record: `docs/evidence/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_PENDING_NATIVE_VALIDATION.md`.

H1 Phase A remains **CLOSED** by `docs/evidence/H1_CONVERGENCE_AUDIT.md`; promotion `28371ea4bcd9709b771413100af4dcec5a05dc6e`.

Historical E0-A Phase-B architecture remains `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`, Proposal 0.15, with approval at `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_APPROVAL.md`.

The current bounded provider amendment is:

- blueprint: `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`;
- approval/evidence: `docs/evidence/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT_APPROVAL.md`;
- pending-native evidence: `docs/evidence/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_PENDING_NATIVE_VALIDATION.md`.

The amendment changes only the next normative E0-A provider route. It does not reopen Core/H1 causal authority, E0-B+, product UI/persistence, NPU, packaging, Store, or later scope.

## Native validation authority

All completed native evidence below is **Director-machine-sourced** from the Director's native Windows ARM64 machine, not the engineering assistant environment.

### Historical original Phase-B implementation checkpoint

Exact validated executable/test checkout:

`3749210393282f6aa2ac4ceb0176b6adb5df189e`

Observed: Windows `10.0.26200`, ARM64, `win-arm64`, SDK `9.0.317`; clean tracked/staged tree; **622/622 Core tests PASS**; **39/39 Harness tests PASS**; Harness ARM64 build PASS; Missing Raft PASS; generic smoke PASS.

### Historical OpenAI live-host completion checkpoint

Exact validated executable/test checkout:

`1cfdb3aa22abce62a5bd48e80706407670d1c6a9`

Observed: Windows `10.0.26200`, `PROCESSOR_ARCHITECTURE=ARM64`, `dotnet --info` RID `win-arm64`, Host Architecture `arm64`, SDK `9.0.317`; clean tracked/staged tree; no material untracked source/test/fixture files; **622/622 Core tests PASS**; **54/54 Harness tests PASS**; Harness native ARM64 build PASS; Missing Raft PASS; generic smoke PASS; credentialless explicit `e0a-run` PASS with native exit `1`, expected `OPENAI_API_KEY` refusal, no evidence root, and credential remaining absent.

That checkpoint remains the exact machine-tested authority for the historical OpenAI live-host implementation. It does **not** validate the current Gemini amendment.

### Current Gemini amendment validation state

Exact frozen executable/source/test checkout pending native validation:

`39e7984b9ff59228d7d0c424ef7d113582d38bc7`

The Gemini executable/test surface has **not yet received Director-machine native Windows ARM64 validation**. Do not infer compiler, test, native runtime, credentialless Gemini-host, or provider behavior from static review.

No real Gemini request, `GEMINI_API_KEY` inference use, provider-network execution, or provider spend has been performed or authorized by the current implementation work.

## Current E0-A live-provider truth on this branch

The Harness continues to own the reference-run envelope outside Core while reusing the closed H1 Cycle/Turn authority unchanged. Core is a hard no-change boundary for this amendment.

Proposal 0.15's OpenAI / `gpt-5.6-sol` four-arm configuration remains historical architecture and regression evidence. It is **not the active live provider route on this branch**.

The approved current live route is:

```text
Provider              Google Gemini API
Model                 gemini-2.5-flash
Transport             native generateContent / streamGenerateContent REST
Credential             GEMINI_API_KEY
Live variant           CREATIVE-NONE only
Performer thinking     thinkingBudget=0
Interpreter thinking   thinkingBudget=0
Integrity control      thinkingBudget=3584
```

`CREATIVE-LOW`, `CREATIVE-MEDIUM`, and `CREATIVE-HIGH` remain deferred for Gemini until a later provider-specific resource-normalization amendment. Their historical OpenAI definitions are not silently mapped to Gemini.

The amendment does not claim that Gemini-native controls are semantically equivalent to OpenAI `none/low/medium/high` labels.

## Gemini operational envelope

Preserved deterministic laws:

- 12 accepted-Turn cap;
- one attempt per probabilistic invocation;
- zero automatic retries;
- 300-second attempt timeout;
- 4,096 requested `maxOutputTokens` per role;
- 4,096 contributing generated-token ceiling per role after Gemini candidate + thought accounting;
- estimated shadow model-token ceiling USD 5.00/run;
- `store=false`;
- standard service tier through the provider's documented default;
- no tools;
- no provider conversation/history identifier;
- Access before Context;
- exact configured-path request/receipt provenance;
- streamed deltas diagnostic-only;
- closed-response semantic authority;
- spend reconciliation before semantic use;
- fail-closed cancellation/timeout/usage handling;
- deterministic Integrity/State Authority rejection;
- atomic accepted causal commit followed immediately by next Opportunity;
- immutable runtime/evaluation sealing and blind-output isolation.

Performer and Interpreter must report zero thought tokens for a contributing normative call. Integrity uses a fixed elevated `thinkingBudget=3584`, but the provider budget is advisory; observed `candidatesTokenCount + thoughtsTokenCount > 4096` terminates technical before semantic consumption.

Integrity pre-spend risk reservation uses Gemini 2.5 Flash's published 65,536 model output-token limit so the USD ceiling remains fail-closed even if provider thinking exceeds its requested budget.

Gemini `models.countTokens` is invoked against the exact prepared `GenerateContentRequest` before inference. The current model input ceiling frozen for this amendment is 1,048,576 tokens.

## Gemini evidence and provider behavior

Gemini-specific manifests bind the approved amendment rather than the historical OpenAI Proposal 0.15 pricing/cache tuple.

Runtime provenance records the configured route, exact request bytes/hash, returned `responseId`, returned `modelVersion`, usage, candidate/thought/cached token counts, shadow cost, and deterministic terminal path without recording the credential.

Thought summaries/signatures are outside E0-A evidence authority. Requests explicitly set `includeThoughts=false`; buffered thought material is rejected before semantic receipt; streamed thought material is rejected **before raw diagnostic persistence**.

The hashed Gemini request explicitly pins one candidate and the current structured-output format rather than relying on mutable provider defaults. Observable `modelVersion` must remain stable across a contributing run. Prompt blocking, refusal/safety termination, missing/invalid candidate, non-STOP finish, malformed structured output, transport failure, timeout, cancellation, usage-policy overrun, implicit-cache hit, or model identity change cannot become fiction.

Gemini usage accounting fails closed on malformed or inconsistent provider totals and on nonzero tool-use prompt tokens. Streamed semantic bytes after an observed `STOP` are rejected; a later metadata-only completion chunk may supply the final cumulative usage tuple.

Gemini 2.5 implicit caching is provider-managed and cannot be disabled through the current API. The normative reference condition therefore treats any nonzero `cachedContentTokenCount` as technical/noncontributing after usage/spend evidence is recorded but before semantic consumption. No explicit Gemini cache object is created.

## Gemini pricing/data-use snapshot

Snapshot verified 2026-09-06 against Google's Gemini API documentation:

```text
Gemini 2.5 Flash standard paid text input       $0.30 / 1M tokens
Gemini 2.5 Flash standard paid output           $2.50 / 1M tokens
output pricing includes thinking tokens
```

The current AI Studio free tier is expected to bill USD 0 for the permitted test route, but free-tier content may be used by Google to improve products. Therefore the free-tier E0 route is **synthetic-fixture-only** and is not Production-provider admission.

The deterministic USD 5 ceiling remains active using paid standard-tier pricing as a conservative shadow estimate. All input, including any reported cached input, is shadow-priced at the full uncached input rate.

`E0AGeminiPricingPolicy.RequireNonStaleSnapshot` is a **date-validity guard, not live pricing verification**. The current short experimental snapshot fails closed after 2026-09-13 UTC, but any material provider change discovered earlier reopens verification immediately.

Before a real Gemini network run, separately reverify the exact AI Studio project/model quota, account tier, key type, model availability, current API contract, pricing, and data-use terms.

## Validation-host continuity

Future Director Windows ARM64 command sets must be generated from `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

Known host facts include:

- Windows PowerShell `RuntimeInformation.OSArchitecture`, `.ProcessArchitecture`, and `.RuntimeIdentifier` have returned blank and are not trusted architecture gates on this host.
- Trusted architecture probes are `PROCESSOR_ARCHITECTURE=ARM64` plus parsed `dotnet --info` showing `RID: win-arm64` and Host `Architecture: arm64`.
- Expected native stderr can surface as `NativeCommandError` under `$ErrorActionPreference='Stop'`; expected-failure probes must isolate stderr, capture `$LASTEXITCODE` immediately, and assert exit/message/filesystem effects independently.

The existing host-behavior document's `OPENAI_API_KEY` credentialless example is historical apparatus evidence. The Gemini native validation packet must apply the same wrapper discipline while expecting `GEMINI_API_KEY` on the active branch.

## Standing decisions

- .NET 10 requires later Director approval.
- `IPackageValidator` is not a mandatory first-release gate.
- E5c exception-runtime provenance: open, no verified defect/fix.
- Stage remains design-lane authority.
- ODR-12/13/26/30/32 remain open; none blocks E0-A.
- ODR-26 working structure: `docs/ODR_26_COST_GOVERNANCE_AND_PROVIDER_ADMISSION_WORKING_CONTRACT.md`. **Not frozen/resolved/project law.** It does not silently select or reopen an E0-A/E0-B route.
- The ODR-26 working contract predates the later outside-lane disclosure rule in the same session; Director disposition controls whether historical disclosure is added or exempted. Engineering takes no action on it absent that disposition.
- ODR proposal/resolution authority belongs to the Director and is filed in `Ensemble-Project/docs` regardless of originating lane.
- Real provider credentials/network execution/spend require a separate explicit Director gate.
- The previously stale `e0a-phase-b-live-host-completion-2` remote branch was deleted after explicit Director authorization; no documentation-only cleanup commit was made solely for that action.
- The promoted `e0a-phase-b-live-host-completion`, `e0a-reference-run-envelope-blueprint`, and `e0a-reference-run-envelope-implementation` remote refs were also deleted after explicit Director authorization because they had zero unique commits and durable evidence already pins their historical checkpoints.

## Next

Remain inside **E0-A Phase B**. Do not enter E0-B..G, product Application/persistence/UI, Scene endings, Context optimization, Windows AI/NPU, MSIX/WACK/Store, or later scope.

Current engineering sequence:

1. native-validate exact frozen checkpoint `39e7984b9ff59228d7d0c424ef7d113582d38bc7` on the Director's Windows ARM64 host;
2. require a clean exact checkout, trusted ARM64 probes, Core/Harness tests, Harness native ARM64 build, Missing Raft and generic fixture smokes, and the credentialless `CREATIVE-NONE` host gate with `GEMINI_API_KEY` absent;
3. record the exact native validation evidence without promoting static claims or replacing `39e7984b...` with a documentation-only descendant;
4. only after validation, return to the Director for a **separate explicit authorization** before any Gemini `countTokens`, inference, credential use, or provider-network execution.

The prior explicit OpenAI first-run authorization was superseded by the Director's later Gemini-provider correction and does not authorize Gemini network execution.
