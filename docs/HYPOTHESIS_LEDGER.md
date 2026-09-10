# Ensemble Hypothesis Ledger

Status: unverified-assumption register. Hypotheses are not decisions, phase authority, provider admission, or validation evidence.

A hypothesis belongs here when engineering work depends on a fact that has not yet been established at the required evidence level. Resolved or narrowed hypotheses should record the disposition and supporting evidence rather than silently disappear.

## Open / narrowed hypotheses

### HYP-001 — Live Gemini request compatibility — NARROWED / 3.5 GENERATION REJECTION ESTABLISHED

Hypothesis: the approved REST request/streaming shapes for the current E0-A comparison profiles remain accepted by the real Google Gemini API:

- `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`;
- `gemini-3.1-flash-lite` / `GEMINI-3.1-FLASH-LITE-MINIMAL`;
- `gemini-2.5-flash-lite` / `GEMINI-2.5-FLASH-LITE-NONE`.

Current evidence: historical Attempt 04 falsified the former full-request `countTokens` projection; Q-E0A-04 corrected that boundary and native-validated the input-semantic projection. The first separately authorized Q-E0A-03 Run 01 then proved the corrected 3.5 Flash-Lite `countTokens` path succeeds (`649` input tokens) and reached the first Performer generation request. That generation request returned HTTP 400; no provider model identity, usage, or structured output was available and zero turns were accepted. Terminal analysis: `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`.

The sealed Run 01 diagnostic retained only `gemini-http-400`, so the evidence does not identify the rejected generation field/provider condition. Current provider documentation reviewed for the preauthorization gate did not establish a generation-request field defect. Engineering therefore must not mutate generation bytes by hypothesis.

A bounded generation-error diagnostic correction is machine-validated at `7868e5cb12a27260e288d95c248d6f846cf37701`, annotated tag `validation/e0a-gemini-generation-error-diagnostic-native-arm64`, so a future separately authorized failure can preserve allowlisted Google RPC status/field evidence without provider prose. This validation does not prove live generation compatibility and does not authorize provider traffic.

Disposition: 3.5 Flash-Lite generation compatibility is **not established**; one real request was rejected HTTP 400 and the exact cause remains unresolved. 3.1 Flash-Lite and 2.5 Flash-Lite live generation compatibility also remain unverified. Q-E0A-03 stays open under its frozen activation/closure contract and provider authority is **NONE**.
Verification trigger: only a newly Director-authorized exact provider run may produce further live evidence. No disposable compatibility probe, automatic retry/rerun, fallback, or alternate-model call is implied.

Falsifier/narrowing evidence: a future authorized run returns bounded structured rejection detail, succeeds through a usable generation/stream response, or otherwise establishes a provider behavior that narrows the current HTTP-400 ambiguity.

### HYP-002 — Account/key/model/quota availability — PARTIALLY ESTABLISHED / VOLATILE

Hypothesis: the Director-selected Free-tier project and a valid key can access the selected model and required `countTokens` / generation routes within the approved E0-A envelope at execution time.

Current evidence: Director-supplied AI Studio UI on 2026-09-07 established Free tier and the comparison quota matrix. For `gemini-3.5-flash-lite`, the Director reconfirmed on 2026-09-09 that the working snapshot remains 15 RPM / 250,000 input TPM / 500 RPD and authorized reuse until a material contrary signal appears; durable decision: `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md`. The static full-run conservative bound remains 72 provider operations <= 500 RPD. Attempt 04 and Q-E0A-03 Run 01 prove that the Director-machine key/project path reached the real 3.5 `countTokens` and generation endpoints; Run 01's generation HTTP 400 is not, by itself, evidence of quota exhaustion.

For 3.1 Flash-Lite and 2.5 Flash-Lite, the recorded 2026-09-07 limits remain historical input to the executable/profile design but have no equivalent Director quota-reuse decision. Their current account/quota state must therefore be reverified before any separately authorized run.

Verification trigger: before each provider authorization, apply the exact route's current decision law. For 3.5, re-read quota values only if a recorded contrary-signal trigger occurs; still reverify material model/project/key/provider changes. For 3.1/2.5, reverify the applicable account/project/model/quota facts immediately before authorization.

Falsifier: key/project association, model availability, remaining/current quota, tier, region/project configuration, provider `429`/`RESOURCE_EXHAUSTED`, or another account constraint violates the selected route's admitted envelope.

### HYP-003 — Current pricing, lifecycle, and data-use state — VOLATILE

Hypothesis: the intended account/tier pricing and data-use terms remain compatible with the approved synthetic-only E0-A run boundary at execution time.

Current evidence: provider-reference snapshots and Director-supplied Free-tier account status were established during the 2026-09-07 to 2026-09-09 provider work. The approved Free-tier route remains synthetic-fixture-only because Free-tier submissions may be used for provider product/model improvement. Shadow rates are encoded per current comparison profile. The machine-validated executable's pricing/data-use freshness guard is valid only through **2026-09-14** and fails closed afterward.

Verification trigger: immediately before each authorized provider-network execution, and whenever the executable snapshot expires or a material provider pricing, data-use, lifecycle, tier, or model-status change is discovered. Execution after 2026-09-14 requires an audited source snapshot refresh followed by renewed native validation/tagging before provider traffic.

Falsifier: current provider terms, billing, retention/data-use, pricing, lifecycle, model availability, or route behavior differs materially from the assumptions required by the approved synthetic run.

### HYP-004 — Live provider generation/usage accounting mapping — OPEN / GENERATION INVOCATION REACHED

Hypothesis: real Gemini response usage fields, reasoning/thinking-token reporting, cache reporting, thought-signature behavior, finish/terminal semantics, and streaming behavior map to the Harness accounting and fail-closed rules as expected for the Gemini 3 thinking-level family (3.5/3.1 Flash-Lite) and the 2.5 Flash-Lite budget-controlled family.

Current evidence: deterministic/fake regressions, machine-validated token-preflight correction, and Q-E0A-03 Run 01. Run 01 reached the real 3.5 Flash-Lite generation boundary after successful `countTokens`, but Google returned HTTP 400 before a usable generation response; provider identity, usage, reasoning/cache accounting, thought-signature behavior, stream framing, and finish semantics therefore remain unverified. The Harness correctly treated provider usage as unknown and preserved zero accepted turns.

The generation-error diagnostic correction improves future bounded failure evidence but does not establish any generation/usage mapping fact by itself.

Verification trigger: first separately authorized successful generation path for each materially distinct provider behavior family during Q-E0A-03.

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
- The former 3.5 `countTokens` compatibility gate is corrected and native-validated; Q-E0A-03 Run 01 instead establishes a later generation-boundary HTTP 400 whose exact cause remains unresolved.
- Q-E0A-03 remains blocked for provider execution under `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md` until a new exact Director authorization exists.
- Provider authorization is **NONE**.
- Free-tier live experiments remain canonical synthetic Missing Raft-only.
- One attempt per probabilistic role invocation, zero automatic retries, no automatic fallback, the USD 5 shadow ceiling, evidence immutability, and experiment order remain fixed.
- .NET 10 requires later Director approval; GitHub plan-dependent branch protection is deferred; Stage remains design-lane authority.
