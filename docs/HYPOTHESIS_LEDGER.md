# Ensemble Hypothesis Ledger

Status: unverified-assumption register. Hypotheses are not decisions, phase authority, provider admission, or validation evidence.

A hypothesis belongs here when engineering work depends on a fact that has not yet been established at the required evidence level. Resolved or narrowed hypotheses should record the disposition and supporting evidence rather than silently disappear.

## Open / narrowed hypotheses

### HYP-001 - Live Gemini request compatibility - NARROWED / 3.5 FULL-RUNTIME TRANSPORT ESTABLISHED

Hypothesis: the approved REST request/streaming shapes for the current E0-A comparison profiles remain accepted by the real Google Gemini API:

- `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`;
- `gemini-3.1-flash-lite` / `GEMINI-3.1-FLASH-LITE-MINIMAL`;
- `gemini-2.5-flash-lite` / `GEMINI-2.5-FLASH-LITE-NONE`.

Current evidence: Run 02 plus four bounded diagnostics localized and corrected the 3.5 structured-output encoding. Run 03 then executed 21 real `gemini-3.5-flash-lite` generation requests through the corrected `responseMimeType` + `responseJsonSchema` path; every generation receipt succeeded with stable returned model identity. Six turns committed before a schema-valid Interpreter response was rejected by local deterministic semantic law. Evidence: `docs/evidence/E0A_Q_E0A_03_G35L_RUN03_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`.

Disposition: 3.5 Flash-Lite transport, stable returned-model identity, repeated structured-output generation, and the corrected legacy GenerateContent encoding are established for the Run 03 route. Run 03 does not establish 12-turn reference contribution because it terminated on a local Interpreter semantic mismatch after six accepted turns. 3.1 Flash-Lite and 2.5 Flash-Lite live generation compatibility remain separately unverified.

Verification trigger: a fresh 3.5 reference candidate after the Interpreter semantic-output correction is machine-validated tests contribution/reliability, not basic transport compatibility. Later model families retain their own live-compatibility requirements.

Falsifier/narrowing evidence: a fresh named corrected reference run reveals a different provider incompatibility or cannot satisfy the frozen activation/contribution gates.
### HYP-002 — Account/key/model/quota availability — PARTIALLY ESTABLISHED / VOLATILE

Hypothesis: the Director-selected Free-tier project and a valid key can access the selected model and required `countTokens` / generation routes within the approved E0-A envelope at execution time.

Current evidence: Director-supplied AI Studio UI on 2026-09-07 established Free tier and the comparison quota matrix. For `gemini-3.5-flash-lite`, the Director reconfirmed on 2026-09-09 that the working snapshot remains 15 RPM / 250,000 input TPM / 500 RPD and authorized reuse until a material contrary signal appears; durable decision: `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md`. The static full-run conservative bound remains 72 provider operations <= 500 RPD. Attempt 04 and Q-E0A-03 Run 01 prove that the Director-machine key/project path reached the real 3.5 `countTokens` and generation endpoints; Run 01's generation HTTP 400 is not, by itself, evidence of quota exhaustion.

For 3.1 Flash-Lite and 2.5 Flash-Lite, the recorded 2026-09-07 limits remain historical input to the executable/profile design but have no equivalent Director quota-reuse decision. Their current account/quota state must therefore be reverified before any separately authorized run.

Verification trigger: before each provider execution, apply the exact route's current decision law. For 3.5, re-read quota values only if a recorded contrary-signal trigger occurs; still reverify material model/project/key/provider changes. For 3.1/2.5, reverify the applicable account/project/model/quota facts immediately before execution.

Falsifier: key/project association, model availability, remaining/current quota, tier, region/project configuration, provider `429`/`RESOURCE_EXHAUSTED`, or another account constraint violates the selected route's admitted envelope.

### HYP-003 — Current pricing, lifecycle, and data-use state — VOLATILE

Hypothesis: the intended account/tier pricing and data-use terms remain compatible with the approved synthetic-only E0-A run boundary at execution time.

Current evidence: provider-reference snapshots and Director-supplied Free-tier account status were established during the 2026-09-07 to 2026-09-09 provider work. The approved Free-tier route remains synthetic-fixture-only because Free-tier submissions may be used for provider product/model improvement. Shadow rates are encoded per current comparison profile. The machine-validated executable's pricing/data-use freshness guard is valid only through **2026-09-14** and fails closed afterward.

Verification trigger: immediately before each authorized provider-network execution, and whenever the executable snapshot expires or a material provider pricing, data-use, lifecycle, tier, or model-status change is discovered. Execution after 2026-09-14 requires an audited source snapshot refresh followed by renewed native validation/tagging before provider traffic.

Falsifier: current provider terms, billing, retention/data-use, pricing, lifecycle, model availability, or route behavior differs materially from the assumptions required by the approved synthetic run.

### HYP-004 - Live provider generation/usage accounting mapping - NARROWED / 3.5 FULL-RUNTIME ACCOUNTING OBSERVED

Hypothesis: real Gemini response usage fields, reasoning/thinking-token reporting, cache reporting, thought-signature behavior, finish/terminal semantics, and streaming behavior map to the Harness accounting and fail-closed rules as expected for the Gemini 3 thinking-level family (3.5/3.1 Flash-Lite) and the 2.5 Flash-Lite budget-controlled family.

Current evidence: Run 03 completed 21 generation responses across Performer/Integrity/Interpreter with stable returned model `gemini-3.5-flash-lite`. Generation receipts report 36,222 input tokens, 6,121 output tokens including 4,811 reasoning tokens, and zero cached input; the Harness reconciled spend with `hasUnknownProviderUsage=false`. Six complete accepted-turn causal commits prove the response/accounting/provenance path operates beyond a diagnostic call. The terminal turn was a local semantic-parser rejection, not unknown provider usage.

Verification trigger: later model-family execution, a contrary provider accounting signal, nonzero cache behavior, thought-signature behavior requiring interpretation, or another response shape not exercised by Run 03.

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
- Project-relevant Gemini Free-tier use is covered by the current standing Director authority; every run/batch remains separately bounded, logged, scope-evaluated, and immutable when consumed.
- Standing project-relevant synthetic Free-tier authority is active; no paid-route, private/user-derived material, automatic retry/fallback, or consumed-run replay authority exists.
- Free-tier live experiments remain canonical synthetic Missing Raft-only.
- One attempt per probabilistic role invocation, zero automatic retries, no automatic fallback, the USD 5 shadow ceiling, evidence immutability, and experiment order remain fixed.
- .NET 10 requires later Director approval; GitHub plan-dependent branch protection is deferred; Stage remains design-lane authority.
