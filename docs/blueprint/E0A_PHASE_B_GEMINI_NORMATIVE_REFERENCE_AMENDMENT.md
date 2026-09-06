# E0-A Phase B — Gemini Normative Reference Amendment

Status: **APPROVED — DIRECTOR 2026-09-06; IMPLEMENTATION AUTHORIZED; REAL GEMINI NETWORK EXECUTION NOT YET AUTHORIZED**

Parent authority: `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md` Proposal 0.15.

This amendment changes only the next normative E0-A provider route. All deterministic H1/Core authority, evidence laws, causal authority, retry/cancellation rules, RunId law, blind-review isolation, and later-phase exclusions remain unchanged unless explicitly stated here.

## 1. Director decision

For the foreseeable experimental period, Ensemble testing moves to the Google Gemini API family. For the next E0-A normative Same-Model Isolated Cast reference experiment, the route is:

```text
Provider              Google Gemini API
Model                 gemini-2.5-flash (stable)
Transport             native GenerateContent / streamGenerateContent REST
Credential             GEMINI_API_KEY
Performer thinking     disabled: thinkingBudget=0
Interpreter thinking   disabled: thinkingBudget=0
Integrity thinking     fixed elevated control: thinkingBudget=3584
```

`gemini-2.5-flash` is selected for this E0-A condition because Google documents true thinking-off through `thinkingBudget=0`. Newer Gemini 3.x Flash models are not substituted for this condition while they cannot fully disable thinking.

This amendment does **not** claim that Gemini provider-native controls are semantically equivalent to OpenAI `none/low/medium/high` labels.

## 2. Scope: normative reference only

Only `CREATIVE-NONE` is authorized on the Gemini live path by this amendment.

`CREATIVE-LOW`, `CREATIVE-MEDIUM`, and `CREATIVE-HIGH` remain deferred. Their original Proposal 0.15 definitions remain historical architecture, but no Gemini mapping is frozen or authorized here. A later characterization amendment must resolve provider-specific reasoning-resource normalization before those arms can execute.

E0-B remains out of scope.

## 3. Provider transport

Use native Gemini `generateContent` / `streamGenerateContent`, not the Interactions API, because the current Interactions reasoning surface does not expose the true thinking-off control required by the normative condition.

Requests remain single-turn/stateless and contain:

- fixed prompt text in `systemInstruction`;
- dynamic bounded Ensemble data in one user `contents` item as non-instructional data;
- no tools;
- no provider conversation/history identifiers;
- `store=false`;
- standard inference tier;
- structured JSON output through Gemini JSON-schema configuration;
- exact request bytes hashed before disclosure.

Performer and Interpreter stream for diagnostics only. Integrity remains buffered. Only the closed successful structured response becomes semantically eligible.

## 4. Output and thinking budgets

The provider-neutral intended generated-token ceiling remains **4,096 tokens per role invocation**.

Gemini exposes visible candidate tokens and thinking tokens separately, so the configured controls are:

```text
Performer    thinkingBudget=0      candidate maxOutputTokens=4096
Integrity    thinkingBudget=3584   candidate maxOutputTokens=512
Interpreter  thinkingBudget=0      candidate maxOutputTokens=4096
```

The Integrity response schema contains only the bounded `concerns` array over the five existing concern names. A 512-token candidate ceiling is intentionally far above the valid response payload while reserving most of the 4,096 intended generated-token budget for the fixed Integrity control.

Google documents `thinkingBudget` as guidance and notes that actual thinking may overflow or underflow it. Therefore:

- successful Gemini usage records candidate and thought tokens separately;
- Ensemble maps billable/generated output as `candidate + thought` tokens;
- any observed generated total above 4,096 is a technical/noncontributing overrun before semantic consumption;
- Performer/Interpreter must report zero thought tokens for a contributing normative call;
- Integrity may vary around its configured thinking budget only while the observed generated total remains within 4,096.

For pre-spend risk accounting, Integrity reserves against Gemini 2.5 Flash's published 65,536 output-token model limit rather than treating `thinkingBudget=3584` as a hard provider cap. This protects the USD ceiling even if provider thinking exceeds the configured budget.

## 5. Input counting and model limits

Before inference, use Gemini `models.countTokens` with the exact `GenerateContentRequest` as `generateContentRequest` input so system instructions and all model-steering content are counted from the same canonical request.

Published model limits frozen for this amendment snapshot:

```text
input token limit     1,048,576
output token limit       65,536
```

The Harness remains fail-closed if token counting cannot be performed or if the exact input exceeds the model limit.

## 6. Usage and implicit caching

Gemini usage maps as:

```text
promptTokenCount          -> InputTokens
candidatesTokenCount      -> visible candidate output
thoughtsTokenCount        -> ReasoningTokens
OutputTokens              -> candidatesTokenCount + thoughtsTokenCount
cachedContentTokenCount   -> CachedInputTokens
CacheWriteTokens          -> 0
```

Gemini 2.5 implicit caching is provider-managed and automatically enabled; the current API does not expose an off switch. The reference experiment therefore does not pretend to disable it.

For the normative no-implicit-cache reference condition, any nonzero `cachedContentTokenCount` is recorded and terminates the call as technical/noncontributing before semantic consumption. All input is conservatively costed at the uncached reference rate.

No explicit Gemini cache object is created.

## 7. Pricing and route scope

The authorized near-term testing route may use the Gemini AI Studio free tier, but free-tier execution is **synthetic-fixture-only**. Google currently states that free-tier content may be used to improve Google products. This amendment is not Production-provider admission and does not authorize private, personal, confidential, or Production-derived user material on that route.

Actual free-tier billed cost is expected to be USD 0. The deterministic USD 5 run ceiling remains active using a conservative **shadow paid-tier reference estimate** so resource governance does not disappear merely because the current route is free.

Snapshot verified 2026-09-06 against Google's Gemini API pricing documentation:

```text
Gemini 2.5 Flash standard paid text input       $0.30 / 1M tokens
Gemini 2.5 Flash standard paid output           $2.50 / 1M tokens
output pricing includes thinking tokens
```

For conservative reconciliation, cached input is also estimated at the full $0.30/M input rate rather than relying on cache savings.

This is a dated reference-cost model, not a claim about actual free-tier billing. Provider/account tier and current pricing/data-use state must be reverified immediately before a real network run.

## 8. Pricing freshness

The implementation carries `VerifiedOn=2026-09-06` and a short experimental fail-closed freshness window. This is an engineering guard for this amendment only; it is not ODR-26 policy law or a frozen provider-admission cadence.

As before, a date-validity guard is **not live pricing verification**. Any material provider change discovered inside the window reopens the snapshot immediately.

## 9. Response authority

A successful Gemini response must provide:

- nonblank `responseId`;
- nonblank `modelVersion`;
- exactly one usable candidate;
- `finishReason=STOP`;
- structured text bytes matching the configured schema;
- complete usage metadata.

Prompt blocking, safety/refusal termination, missing candidate, non-STOP finish, malformed structured output, transport failure, timeout, cancellation, usage-policy overrun, cache hit, or identity inconsistency remains technical/noncontributing and cannot become fiction.

Observable `modelVersion` change within a contributing run retains the existing model-identity fail-closed rule.

Thought summaries/signatures are not requested, stored, or semantically consumed.

## 10. Evidence and Core

Core remains unchanged.

The existing configured-path receipt, exact Context disclosure, immutable runtime seal, blind transcript/mapping isolation, post-run hard-gate evaluation, deterministic Integrity/State Authority, Take/commit, and immediate next-Opportunity laws remain unchanged.

Gemini-specific request bytes, response/model identity, finish reason, usage, cached-token count, thought-token count, and shadow cost become Harness provenance only.

## 11. Implementation and validation gate

Implementation is authorized only in the Harness/test/evidence boundary needed by this amendment.

Before any real Gemini provider request:

1. fake/unit tests must pass;
2. Core must remain unchanged;
3. native Windows ARM64 Core/Harness tests and Harness build must pass;
4. credentialless `e0a-run CREATIVE-NONE` must fail before evidence creation with the expected `GEMINI_API_KEY` refusal;
5. exact AI Studio project/model quota, account tier, current key type, pricing/data-use terms, and model availability must be reverified;
6. a separate explicit Director authorization is required for the first Gemini network execution.

The prior OpenAI network authorization was superseded by the Director's later Gemini-provider correction and does not authorize Gemini inference.

## 12. External verification snapshot

Official Google sources verified 2026-09-06:

- Gemini thinking / 2.5 Flash thinking budgets: `https://ai.google.dev/gemini-api/docs/generate-content/thinking`
- Gemini GenerateContent API: `https://ai.google.dev/api/generate-content`
- Gemini token counting: `https://ai.google.dev/api/tokens`
- Gemini structured output: `https://ai.google.dev/gemini-api/docs/generate-content/structured-output`
- Gemini context caching: `https://ai.google.dev/gemini-api/docs/caching/`
- Gemini pricing: `https://ai.google.dev/gemini-api/docs/pricing`
- Gemini 2.5 Flash model limits/version status: `https://ai.google.dev/gemini-api/docs/models/gemini-2.5-flash-preview-09-2025`

Any later provider change must be reverified rather than inferred from this snapshot.
