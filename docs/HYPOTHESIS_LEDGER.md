# Ensemble Hypothesis Ledger

Status: unverified-assumption register. Hypotheses are not decisions, phase authority, provider admission, or validation evidence.

A hypothesis belongs here when engineering work depends on a fact that has not yet been established at the required evidence level. Resolved or narrowed hypotheses should record the disposition and supporting evidence rather than silently disappear.

## Open / narrowed hypotheses

### HYP-001 — Live Gemini request compatibility — NARROWED / CORRECTED PREFLIGHT MACHINE-VALIDATED

Hypothesis: the approved REST request/streaming shapes for the current E0-A comparison profiles remain accepted by the real Google Gemini API:

- `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`;
- `gemini-3.1-flash-lite` / `GEMINI-3.1-FLASH-LITE-MINIMAL`;
- `gemini-2.5-flash-lite` / `GEMINI-2.5-FLASH-LITE-NONE`.

Current evidence: Attempt 04 conclusively localized the prior 3.5 Flash-Lite failure to the first Performer `countTokens` preflight. Google returned HTTP 400 / `INVALID_ARGUMENT` at `generate_content_request.generation_config.response_format.text.mime_type`; generation was never reached. Archive audit: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`.

Q-E0A-04 corrected that live-falsified boundary by projecting only `model + systemInstruction + contents` into Gemini Developer API `countTokens` while leaving the prepared generation request unchanged. The corrected executable is machine-validated on native Windows ARM64 at exact checkout `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`, annotated tag `validation/e0a-gemini-counttokens-input-projection-native-arm64`, tag object `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`.

This establishes the local correction and validation boundary; it does **not** prove that the full 3.5 generation/streaming path, provider usage mapping, or either other live comparison profile will succeed against the real provider.

Disposition: the historical 2026-09-08 3.5 pause and 3.1 deferral were gates pending diagnosis/correction of the old full-request `countTokens` behavior. That prerequisite is now satisfied by Q-E0A-04. They do not become standing provider authorization. Q-E0A-03 remains blocked for network execution until its exact provider facts and separate Director authorization are satisfied. Planning/closure contract: `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md`.

Verification trigger: the first separately authorized Q-E0A-03 full-reference run for 3.5 Flash-Lite. The run itself is the compatibility test; there is no separate disposable provider probe. Any later profile remains separately authorized.

Falsifier/narrowing evidence: a real provider rejects or semantically contradicts the corrected approved request/stream contract for the selected profile, or returned response/usage behavior cannot satisfy the existing fail-closed envelope.

### HYP-002 — Account/key/model/quota availability — PARTIALLY ESTABLISHED / VOLATILE

Hypothesis: the Director-selected Free-tier project and a valid key can access the selected model and required `countTokens` / generation routes within the approved E0-A envelope at execution time.

Current evidence: Director-supplied AI Studio UI on 2026-09-07 established Free tier and the comparison quota matrix. For `gemini-3.5-flash-lite`, the Director reconfirmed on 2026-09-09 that the working snapshot remains 15 RPM / 250,000 input TPM / 500 RPD and authorized reuse until a material contrary signal appears; durable decision: `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md`. The static full-run conservative bound remains 72 provider operations <= 500 RPD. Attempt 04 also proved that the Director-machine key/project path could reach the real 3.5 `countTokens` API, though the old request shape was rejected for compatibility rather than authentication.

For 3.1 Flash-Lite and 2.5 Flash-Lite, the recorded 2026-09-07 limits remain historical input to the executable/profile design but have no equivalent Director quota-reuse decision. Their current account/quota state must therefore be reverified before any separately authorized run.

Verification trigger: before each provider authorization, apply the exact route's current decision law. For 3.5, re-read quota values only if a recorded contrary-signal trigger occurs; still reverify material model/project/key/provider changes. For 3.1/2.5, reverify the applicable account/project/model/quota facts immediately before authorization.

Falsifier: key/project association, model availability, remaining/current quota, tier, region/project configuration, provider `429`/`RESOURCE_EXHAUSTED`, or another account constraint violates the selected route's admitted envelope.

### HYP-003 — Current pricing, lifecycle, and data-use state — VOLATILE

Hypothesis: the intended account/tier pricing and data-use terms remain compatible with the approved synthetic-only E0-A run boundary at execution time.

Current evidence: provider-reference snapshots and Director-supplied Free-tier account status were established during the 2026-09-07 to 2026-09-09 provider work. The approved Free-tier route remains synthetic-fixture-only because Free-tier submissions may be used for provider product/model improvement. Shadow rates are encoded per current comparison profile. The machine-validated executable's pricing/data-use freshness guard is valid only through **2026-09-14** and fails closed afterward.

Verification trigger: immediately before each authorized provider-network execution, and whenever the executable snapshot expires or a material provider pricing, data-use, lifecycle, tier, or model-status change is discovered. Execution after 2026-09-14 requires an audited source snapshot refresh followed by renewed native validation/tagging before provider traffic.

Falsifier: current provider terms, billing, retention/data-use, pricing, lifecycle, model availability, or route behavior differs materially from the assumptions required by the approved synthetic run.

### HYP-004 — Live provider generation/usage accounting mapping — OPEN

Hypothesis: real Gemini response usage fields, reasoning/thinking-token reporting, cache reporting, thought-signature behavior, finish/terminal semantics, and streaming behavior map to the Harness accounting and fail-closed rules as expected for the Gemini 3 thinking-level family (3.5/3.1 Flash-Lite) and the 2.5 Flash-Lite budget-controlled family.

Current evidence: deterministic/fake regressions, machine-validated Q-E0A-04 token-preflight correction, and real `countTokens` boundary evidence. No E0-A comparison run has yet reached successful generation/inference, so real generation usage/reasoning/cache/stream mapping remains unverified for every current comparison profile.

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
- The former 3.5 pause / 3.1 diagnostic deferral is historical compatibility-gate context; Q-E0A-04 has completed the required correction and native validation. This does not authorize either route.
- Q-E0A-03 remains blocked for provider execution under `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md`.
- Provider authorization is **NONE**.
- Free-tier live experiments remain canonical synthetic Missing Raft-only.
- One attempt per probabilistic role invocation, zero automatic retries, no automatic fallback, the USD 5 shadow ceiling, evidence immutability, and experiment order remain fixed.
- .NET 10 requires later Director approval; GitHub plan-dependent branch protection is deferred; Stage remains design-lane authority.
