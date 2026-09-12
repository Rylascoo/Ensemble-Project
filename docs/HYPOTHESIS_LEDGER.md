# Ensemble Hypothesis Ledger

Status: unverified-assumption register. Hypotheses are not decisions, phase authority, provider admission, or validation evidence.

A hypothesis belongs here when engineering work depends on a fact that has not yet been established at the required evidence level. Resolved or narrowed hypotheses should record the disposition and supporting evidence rather than silently disappear.

## Open / narrowed hypotheses

### HYP-001 - Live Gemini request compatibility - NARROWED / 3.5 + 3.1 LIVE TRANSPORT ESTABLISHED

Hypothesis: the approved REST request/streaming shapes for the current E0-A comparison profiles remain accepted by the real Google Gemini API:

- `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`;
- `gemini-3.1-flash-lite` / `GEMINI-3.1-FLASH-LITE-MINIMAL`;
- `gemini-2.5-flash-lite` / `GEMINI-2.5-FLASH-LITE-NONE`.

Current evidence: Run 02 plus bounded diagnostics localized and corrected the 3.5 structured-output encoding. Run 03 completed 21 real generations with stable model identity and six accepted turns before a local Interpreter semantic rejection. Runs 04/05 localized and corrected later local contract/diagnostic defects; Run 06 then terminated on first Performer generation HTTP 503 / `UNAVAILABLE`. Run 07 exercised 3.1 Flash-Lite: eight `countTokens` preflights succeeded, seven generations succeeded with exact returned model `gemini-3.1-flash-lite`, two turns committed, and Turn 3 Integrity generation then returned HTTP 503 / `UNAVAILABLE`. Evidence: the Run 03/04/05/06/07 terminal analyses.

Disposition: exercised 3.5 and 3.1 Flash-Lite transport/structured-output compatibility are now established. Run 07 demonstrates all three 3.1 roles can return valid structured output and reach deterministic commits before its availability-class terminal. Runs 06 and 07 provide separate HTTP 503 / `UNAVAILABLE` events across both full-candidate models, but neither event supports parser, canonical-ID, credential, quota-429, or local semantic failure claims. Runs 03/04/05/06/07 remain noncontributing and immutable. 2.5 Flash-Lite live generation compatibility remains unverified.

Verification trigger: Run 08 has completed and established full 3.5 live transport/structured-output compatibility across all three roles through 12 accepted turns. No further compatibility probe or Standard/Free replacement is triggered. The 2.5 control remains separately dispositioned and, if executed, must pass its own current activation gates.

Falsifier/narrowing evidence: Run 08 completed 36/36 exact-model generations and 12 causal commits without provider or local semantic failure. This narrows the earlier Run 06/07 503 observations as availability-class failures rather than evidence of persistent route incompatibility; those failures remain immutable reliability evidence.
### HYP-002 — Account/key/model/quota availability — PARTIALLY ESTABLISHED / VOLATILE

Hypothesis: the Director-selected Free-tier project and a valid key can access the selected model and required `countTokens` / generation routes within the approved E0-A envelope at execution time.

Current evidence: Director-supplied AI Studio UI on 2026-09-07 established Free tier and the comparison quota matrix. For `gemini-3.5-flash-lite`, the Director reconfirmed on 2026-09-09 that the working snapshot remains 15 RPM / 250,000 input TPM / 500 RPD and authorized reuse until a material contrary signal appears; durable decision: `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md`. The static full-run conservative bound remains 72 provider operations <= 500 RPD. Attempt 04 and Q-E0A-03 Run 01 prove that the Director-machine key/project path reached the real 3.5 `countTokens` and generation endpoints. Run 06 again reached 3.5 `countTokens` and generation; its HTTP 503 / `UNAVAILABLE` result is not a quota `429` / `RESOURCE_EXHAUSTED` signal.

For 3.1 Flash-Lite, current Google documentation and Director-supplied authenticated AI Studio evidence on 2026-09-11 established the exact model/Free-tier route, Kymaean project/key association, and 15 RPM / 250,000 input TPM / 500 RPD. Run 07 then reached eight real `countTokens` calls and eight generation attempts; seven generations succeeded before one HTTP 503 / `UNAVAILABLE`. The result is not a quota `429` / `RESOURCE_EXHAUSTED` signal and confirms the account/key/model route was operational during the run. For 2.5 Flash-Lite, current account/quota state remains separately unverified.

Current evidence now also includes Run 08: 36 `countTokens` and 36 generation operations completed successfully on the approved 3.5 Free-tier route with no `429` / `RESOURCE_EXHAUSTED` signal. This establishes account/key/model/quota availability for that consumed run; 2.5 current account/quota state remains separately unverified.

Verification trigger: before any separately authorized 2.5 control or later provider-network execution, recheck the selected route's material account/project/key/model/tier/quota facts and stop on any contrary signal.

Falsifier: key/project association, model availability, remaining/current quota, tier, region/project configuration, provider `429`/`RESOURCE_EXHAUSTED`, or another account constraint violates the selected route's admitted envelope.

### HYP-003 — Current pricing, lifecycle, and data-use state — VOLATILE

Hypothesis: the intended account/tier pricing and data-use terms remain compatible with the approved synthetic-only E0-A run boundary at execution time.

Current evidence: official Google surfaces were rechecked 2026-09-11 for Run 08. Exact `gemini-3.5-flash-lite` remains a stable Gemini API model with Free-tier Standard access, 1,048,576 input / 65,536 output limits, Structured Outputs and Thinking support, and no announced shutdown date. Free-tier input/output are free of charge and submitted content may be used to improve Google products. Current paid shadow rates are USD 0.30/M input, USD 0.03/M cached input, and USD 2.50/M output including thinking, matching the executable 3.5 profile. The machine-validated pricing/data-use freshness guard remains valid only through **2026-09-14** and fails closed afterward.

Verification trigger: immediately before each authorized provider-network execution, and whenever the executable snapshot expires or a material provider pricing, data-use, lifecycle, tier, or model-status change is discovered. Execution after 2026-09-14 requires an audited source snapshot refresh followed by renewed native validation/tagging before provider traffic.

Falsifier: current provider terms, billing, retention/data-use, pricing, lifecycle, model availability, or route behavior differs materially from the assumptions required by the approved synthetic run.

### HYP-004 - Live provider generation/usage accounting mapping - NARROWED / 3.5 + 3.1 ACCOUNTING OBSERVED

Hypothesis: real Gemini response usage fields, reasoning/thinking-token reporting, cache reporting, thought-signature behavior, finish/terminal semantics, and streaming behavior map to the Harness accounting and fail-closed rules as expected for the Gemini 3 thinking-level family (3.5/3.1 Flash-Lite) and the 2.5 Flash-Lite budget-controlled family.

Current evidence: Run 03 completed 21 generation responses across Performer/Integrity/Interpreter with stable returned model `gemini-3.5-flash-lite`; receipts report 36,222 input tokens, 6,121 output tokens including 4,811 reasoning tokens, and zero cached input. Runs 05/06 demonstrated conservative unknown-usage reservation retention on failed calls. Run 07 extends live accounting to 3.1 Flash-Lite: seven successful generation receipts report 10,886 input tokens, 2,143 output tokens including 1,514 reasoning tokens, and zero cached input; the failed Turn 3 Integrity generation returned no usage receipt, so the Harness retained its full USD 0.09911125 reservation and terminated at conservative shadow USD 0.10504725 with `hasUnknownProviderUsage=true`.

Run 08 extends 3.5 accounting through a complete 12-turn candidate: 36 successful generation receipts report 78,645 input tokens, 11,596 output tokens including 8,411 reasoning tokens, zero cached input/cache-write tokens, and no unknown provider usage; sealed shadow estimate USD 0.05258350.

Verification trigger: a later Director-selected route, the separately dispositioned 2.5 budget-controlled family, contrary accounting signal, nonzero cache behavior, thought-signature behavior requiring interpretation, or another response shape not yet exercised.

Falsifier: observed provider responses cannot be reconciled with the approved accounting/provenance rules without changing runtime semantics.

### HYP-005 — `countTokens` quota accounting — OPEN / CONSERVATIVELY BOUNDED

Hypothesis: Google may or may not charge `models.countTokens(generateContentRequest)` against the same project request-rate and/or daily request buckets used by generation; current authoritative provider evidence does not establish an exemption or shared-bucket rule strongly enough for the E0-A gate.

Current evidence: real `countTokens` requests have occurred, but no controlled live quota-accounting experiment or authoritative provider statement resolves their relationship to generation RPM/RPD accounting.

Current engineering disposition: the Harness conservatively rate-paces every Gemini API-bound operation, including `countTokens`; applies exact rolling input-TPM discipline to generation; and statically admits RPD as though all six possible `countTokens` + generation operations per accepted turn share the daily bucket. This is a fail-closed choice, not evidence that Google actually accounts for `countTokens` that way.

Verification trigger: authoritative provider clarification or separately authorized live evidence that can distinguish quota behavior without weakening other E0-A controls.

Falsifier/narrowing evidence: authoritative or empirical evidence establishes that `countTokens` is governed by a distinct/exempt quota surface. Any pacing or RPD-bound relaxation requires a separate audited amendment; it must not be inferred automatically.

## Closed or deferred matters that are not hypotheses

The following are decisions or scope gates and must not be reclassified as assumptions:

- Gemini remains the sole current E0-A provider method; historical OpenAI executable support is retired.
- The implemented comparison-profile set is 3.5 Flash-Lite Minimal (12 turns), 3.1 Flash-Lite Minimal (12 turns), and 2.5 Flash-Lite None (3-turn exact-no-thinking control), with historical 2.5 Flash non-live.
- The former 3.5 `countTokens` compatibility gate is corrected and native-validated; Run 02 localized the later generation rejection to `generation_config.response_format.text.mime_type`; the resulting legacy structured-output request correction is also native-validated at `cef3fc15e31192a48aa3bddd99450b65a58bd8f1`.
- Run 03 established repeated 3.5 transport/accounting success; Runs 04/05 localized later local contract/diagnostic defects; Run 06 terminated on first 3.5 Performer generation HTTP 503 / `UNAVAILABLE`; Run 07 established repeated 3.1 transport/structured-output/accounting success and two accepted turns before another HTTP 503 / `UNAVAILABLE`. Option A's single replacement Run 08 then completed 12/12 accepted turns with 36/36 exact-model generations and a passing sealed hard-gate evaluation. Run 08 is the sole contributing full candidate; reference selection and the planned 2.5 control's execution-or-omission disposition remain Director decisions.
- Project-relevant Gemini Free-tier use is covered by the current standing Director authority; every run/batch remains separately bounded, logged, scope-evaluated, and immutable when consumed.
- Standing project-relevant synthetic Free-tier authority is active; no paid-route, private/user-derived material, automatic retry/fallback, or consumed-run replay authority exists.
- Free-tier live experiments remain canonical synthetic Missing Raft-only.
- One attempt per probabilistic role invocation, zero automatic retries, no automatic fallback, the USD 5 shadow ceiling, evidence immutability, and experiment order remain fixed.
- .NET 10 requires later Director approval; GitHub plan-dependent branch protection is deferred; Stage remains design-lane authority.
