# E0-A Phase B — Gemini on Hardened Main Integration Static Closure

Status: **STATIC / COMPILER CLOSURE — ZERO-NEW-MATERIAL-CORRECTION PASS ACHIEVED — NATIVE WINDOWS ARM64 VALIDATION PENDING**

Date: **2026-09-07**

## Purpose

This record closes the engineering/static composition of the approved Gemini normative-reference amendment onto the promoted E0-A post-audit-hardening baseline. It does not establish native Windows ARM64 runtime authority and does not authorize a Gemini credential, token-count request, inference, provider-network execution, or spend.

Repository: `Rylascoo/Ensemble-Project`

Integration branch: `e0a-gemini-on-hardened-main`

Hardened `main` composition base:

`aa9a3d0dfbba9e882b67a7d0d9d1d510374103fd`

Exact combined executable/test checkpoint frozen for native validation:

`5f286e8cfa896d38d85d4611f69a224fae5b55fd`

The integration checkpoint is 0 commits behind the hardened composition base. Core source is unchanged by the Gemini integration.

## Historical native authorities do not compose automatically

Post-audit-hardening native checkpoint:

`5c70f619d6e951d89bb527a5945b014998573dab`

Tag: `validation/e0a-phase-b-post-audit-hardening-native-arm64`.

Original Gemini amendment native checkpoint:

`9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`

Tag: `validation/e0a-phase-b-gemini-native-arm64`.

Both remain valid historical evidence for their exact checkouts. Neither is native authority for `5f286e8c...`, because the combined executable differs from both. Fresh native Windows ARM64 validation is therefore mandatory before integration promotion.

## Composition method

The original Gemini branch was not merged wholesale over hardened `main`. Overlapping Harness/test surfaces were reconciled semantically with hardened `main` as the preserved base. Gemini-only source/tests and approved amendment evidence were imported, then recursively audited against the closed hardening findings.

The composition preserves, among other laws:

- hardened pricing representability and checked spend arithmetic;
- reservation identity, known/unknown usage distinction, and conservative fallback accounting;
- write-once evidence namespace ownership and runtime/evaluation seal authority;
- repository-root checkout validation;
- the frozen 300-second attempt cancellation authority with provider `HttpClient.Timeout = Timeout.InfiniteTimeSpan`;
- deterministic Core Integrity, State Authority, Take/commit, and next-Opportunity authority;
- no semantic adoption from technical, malformed, partial, blocked, non-STOP, or usage-policy-invalid provider outcomes.

## Recursive composition corrections

The recursive audit found and corrected Gemini behaviors that were acceptable only in the older standalone branch or predated post-audit hardening.

### E-02 — streaming cancellation preserved

The original Gemini stream loop used `StreamReader.EndOfStream`, reintroducing the synchronous EOF-probe hazard closed by hardening. The combined port now uses cancellable `ReadLineAsync(cancellationToken)` as its only stream input read and treats `null` as EOF.

Regression coverage uses a stream that forbids synchronous reads and proves cancellation reaches the asynchronous read boundary.

### E-03 — malformed-provider handling preserved

The combined Gemini port now uses strict UTF-8 streaming decode, explicit JSON type validation for consumed provider fields, and well-formed UTF-16 validation for provider strings. Malformed input remains technical and cannot become semantic output. `OperationCanceledException` remains caller/deadline authority rather than being normalized into malformed-provider failure.

Thought material remains outside evidence/semantic authority. Streaming thought material is rejected before diagnostic persistence.

### E-06 — spend/provenance preserved across closed and streaming responses

Closed buffered responses may retain independently validated nonsemantic response identity, model identity, and usage on technical outcomes.

Streaming usage is not treated as spend authority until the stream is fully consumed. An early stream parse, identity, thought, semantic, or usage failure therefore omits usage so the hardened driver commits the full reserved fallback amount. This prevents an earlier provisional tuple from understating spend when a later terminal tuple is malformed or contradictory.

After full EOF, a non-STOP technical stream may retain a validated terminal usage tuple while still carrying no structured semantic output.

Dedicated regression coverage proves both the later-invalid-usage fallback case and the fully consumed non-STOP known-usage case.

### E-07 — timeout authority preserved

The combined live host creates the provider `HttpClient` with `Timeout.InfiniteTimeSpan`. The run driver's linked 300-second token remains the sole attempt deadline across input-token preflight and provider execution.

## Gemini amendment preserved

The integration retains the Director-approved normative route:

```text
provider              Google Gemini API
model                 gemini-2.5-flash
variant               CREATIVE-NONE only
Performer thinking    0
Interpreter thinking  0
Integrity thinking    3584
candidate request cap 4096
combined observed generated-token ceiling 4096
input model limit     1,048,576
output model limit    65,536
credential            GEMINI_API_KEY
```

Integrity spend risk reserves against the published 65,536 output-token model limit. Post-response generated usage maps candidate + thought tokens. Nonzero implicit-cache usage remains technical/noncontributing while all input is conservatively shadow-priced at the uncached paid-tier rate.

## Current provider-contract verification

On 2026-09-07, current official Google Gemini API documentation was rechecked statically; no provider request was made.

Current official documentation still supports the integrated contract used here:

- `models.countTokens` accepts `generateContentRequest` as a `GenerateContentRequest` containing the prompt plus model-steering information such as system instructions;
- `generateContent` / `streamGenerateContent` use the model resource in the endpoint path and support `systemInstruction`, `generationConfig`, optional `serviceTier`, and `store`;
- the current response contract exposes `responseId`, `modelVersion`, `promptFeedback`, `candidates`, and `usageMetadata`;
- current usage metadata exposes `promptTokenCount`, `cachedContentTokenCount`, `candidatesTokenCount`, `toolUsePromptTokenCount`, `thoughtsTokenCount`, and `totalTokenCount`;
- current structured-output configuration exposes `generationConfig.responseFormat.text` with JSON MIME type and schema;
- the current Gemini 2.5 Flash model page continues to list the 1,048,576 input and 65,536 output limits;
- the current Gemini 2.5 Flash pricing page remains consistent with the amendment's dated $0.30/M paid text-input and $2.50/M output-including-thinking shadow rates.

Primary official references remain those filed in `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`, including `https://ai.google.dev/api/generate-content`, `https://ai.google.dev/api/tokens`, the Gemini 2.5 Flash model page, and the Gemini pricing page.

This verification does not prove the Director's exact AI Studio project/account can currently use the model. Exact account tier, key type, quota, project/model availability, pricing/data-use terms, and provider route must still be reverified immediately before any first real network execution, as the amendment already requires.

## Compiler / CI evidence

GitHub Actions Validation gate run `34152340592` executed at exact checkout:

`5f286e8cfa896d38d85d4611f69a224fae5b55fd`

PASS:

- Core Release build: 0 warnings, 0 errors;
- Harness Release `win-arm64` build: 0 warnings, 0 errors;
- Core test-project Release build: 0 warnings, 0 errors;
- Harness test-project Release build: 0 warnings, 0 errors;
- Oracle documentation drift gate;
- advisory x64 Core tests.

This is **ARM64-target cross-compile/compiler authority only**. The build ran on Ubuntu x64 and is not native Windows ARM64 runtime authority.

## Final recursive audit

After the closed-stream usage correction and regression additions, the combined source/test/evidence surface was recursively checked for:

- regression against closed hardening findings E-02, E-03, E-06, E-07 and related spend/evidence laws;
- Gemini amendment scope and route fidelity;
- malformed-provider and cancellation behavior;
- streaming versus closed-response semantic/provenance authority;
- request/usage/evidence consistency;
- historical OpenAI regression preservation;
- Core scope drift;
- stale provider assumptions material to the frozen implementation;
- test-oracle drift;
- validation-level inflation;
- repository/lane hygiene.

One complete pass found no further material correction, inconsistency, ambiguity, or worthwhile implementation improvement inside the authorized boundary.

## Remaining gate

The exact combined executable/test checkpoint `5f286e8c...` now requires fresh Director-machine native Windows ARM64 validation generated from:

`docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`

The validation must include exact checkout/cleanliness authority, native host probes, Core tests, Harness tests, a fresh clean-output native ARM64 Harness build, Missing Raft and generic fixture smokes, and the credentialless Gemini provider-edge expected-refusal gate with both provider credentials absent and no evidence-root creation.

Until that succeeds, `5f286e8c...` is **compiler-gated / statically closed, not machine-tested runtime authority**. After a native PASS, Repository Surface law requires an annotated validation tag at that exact checkout before `CURRENT_STATE.md` may promote it as a machine-validated checkpoint.
