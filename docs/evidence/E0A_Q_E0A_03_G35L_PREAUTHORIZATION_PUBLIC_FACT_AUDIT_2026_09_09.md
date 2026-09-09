# E0-A Q-E0A-03 — Gemini 3.5 Flash-Lite Pre-Authorization Public-Fact Audit

Status: **PASS — PUBLIC/VOLATILE FACT GATE ONLY — PROVIDER TRAFFIC NOT AUTHORIZED**

Date: 2026-09-09

Authority: `CURRENT_STATE.md` remains the only phase/checkpoint/next-action authority. This evidence record implements the public-provider-fact portion of `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md` for the prospective first Q-E0A-03 arm. It does not authorize credentials, provider traffic, inference, spend, retry, fallback, or a phase transition.

## Prospective run boundary checked

```text
arm/profile CREATIVE-MINIMAL / GEMINI-3.5-FLASH-LITE-MINIMAL
model       gemini-3.5-flash-lite
candidate   12 accepted turns
executable  3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2
tag         validation/e0a-gemini-counttokens-input-projection-native-arm64
fixture     ensemble.e0.missing-raft@0.1.0
fixture SHA 5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703
```

No provider request was sent during this audit.

## Official public evidence reverified

### Model availability and lifecycle

Google's current Gemini model catalog identifies `gemini-3.5-flash-lite` as a **Stable** Gemini 3 model. Its dedicated model page reports a 1,048,576-token input limit, 65,536-token output limit, text output, structured-output support, thinking support, and stable model id `gemini-3.5-flash-lite`. Google's current deprecation schedule lists its July 21, 2026 release and **no shutdown date announced**.

Sources:

- <https://ai.google.dev/gemini-api/docs/models/gemini-3.5-flash-lite>
- <https://ai.google.dev/gemini-api/docs/deprecations>
- <https://ai.google.dev/gemini-api/docs/changelog>

Disposition: **PASS**. No public model-removal/lifecycle contrary signal was found.

### GenerateContent / structured-output compatibility

The current `models.generateContent` API reference explicitly includes `generationConfig.responseFormat`, with text output configured by `responseFormat.text.mimeType` and `responseFormat.text.schema`. This is the same generation-output family emitted by the machine-validated Harness. The dedicated 3.5 Flash-Lite model page explicitly marks **Structured outputs: Supported**.

A separate legacy structured-output guide currently omits 3.5 Flash-Lite from one model-support table. That omission was treated as a contrary signal and resolved against the more specific current model page plus the current GenerateContent schema. The omission is therefore recorded as documentation inconsistency, not evidence that the route is unsupported.

Sources:

- <https://ai.google.dev/api/generate-content>
- <https://ai.google.dev/gemini-api/docs/models/gemini-3.5-flash-lite>
- <https://ai.google.dev/gemini-api/docs/generate-content/structured-output>

Disposition: **PASS WITH DOCUMENTATION-INCONSISTENCY NOTE**. No source/executable change is justified by the current public evidence.

### `countTokens` surface

The current `models.countTokens` reference continues to accept either contents or a `generateContentRequest`; the latter includes prompt input and steering information such as system instructions. Q-E0A-04's machine-validated correction intentionally projects only token-relevant `model + systemInstruction + contents` into the preflight and leaves the prepared generation request unchanged.

Source: <https://ai.google.dev/api/tokens>

Disposition: **PASS**. The current public API surface does not contradict the corrected input-semantic preflight.

### Thinking control

Google currently documents `thinkingLevel=minimal` as supported for Gemini 3.5/3.1 Flash-Lite and explicitly states that minimal does **not** guarantee thinking is fully disabled. This matches the frozen `CREATIVE-MINIMAL` law and the Harness accounting policy.

Source: <https://ai.google.dev/gemini-api/docs/generate-content/thinking>

Disposition: **PASS**.

### Pricing and Free-tier data use

Current Standard pricing for Gemini 3.5 Flash-Lite remains:

```text
Free tier input/output: free of charge
Paid reference input:   USD 0.30 / 1M tokens
Paid reference output:  USD 2.50 / 1M tokens including thinking
Paid cache input:       USD 0.03 / 1M tokens
```

Those Standard paid reference rates match the machine-validated executable's conservative shadow-pricing snapshot. Google's current pricing page states Free-tier content is used to improve products. The Gemini API Additional Terms likewise state that Unpaid Services may use submitted content and generated responses to improve/develop Google products and ML technologies and that human reviewers may process API input/output; users are instructed not to submit sensitive, confidential, or personal information.

Sources:

- <https://ai.google.dev/gemini-api/docs/pricing>
- <https://ai.google.dev/gemini-api/terms>

Disposition: **PASS** only under the already-frozen canonical synthetic Missing-Raft-only boundary. This audit does not broaden permitted data.

### Authentication and quota discipline

Google's current API-key documentation continues to support authorization keys and recommends environment-variable handling. The platform is transitioning away from unrestricted Standard keys; the project already moved to the Auth-key path during the prior diagnostic sequence.

Google's public rate-limit documentation states that limits are project-scoped, vary by model/tier, are visible in AI Studio, and are not guaranteed. It supplies no new public fact that contradicts the Director's 2026-09-09 route-specific reuse decision for `gemini-3.5-flash-lite`: **15 RPM / 250,000 input TPM / 500 RPD** until a recorded material contrary signal occurs. Therefore the standing Director decision remains operative and this audit does not silently replace it with a generic public table.

Sources:

- <https://ai.google.dev/gemini-api/docs/api-key>
- <https://ai.google.dev/gemini-api/docs/rate-limits>
- `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md`

Disposition: **PASS / NO CONTRARY SIGNAL FOUND**. Actual credential presence is never recorded here. The local execution packet must still fail closed before provider invocation if exact checkout, fixture, evidence-root, credential, or other local preflight requirements are not satisfied.

## Freshness boundary

The machine-validated executable still contains `E0AGeminiPricingPolicy.SnapshotValidThrough = 2026-09-14`. This audit does not extend that code guard. A provider run after the guard becomes stale requires the already-frozen audited source-refresh and renewed native-validation/tag path.

## Recursive audit conclusion

One apparent issue was found and resolved: the legacy structured-output support table omitted 3.5 Flash-Lite. The dedicated current model page explicitly says Structured outputs are supported, and the current GenerateContent schema supports the exact `responseFormat.text` structure used by the Harness. No other material contradiction was found across model availability, lifecycle, thinking semantics, pricing, Free-tier data-use terms, authentication direction, or the current public API schema.

Therefore the **public/volatile provider-fact portion of the first-run activation gate passes on 2026-09-09**. This does not prove live generation compatibility and does not authorize network execution.

## Remaining gate

Provider authorization remains **NONE**. The next project decision is an explicit Director authorization for exactly one named Q-E0A-03 3.5 Flash-Lite full-reference run. After that authorization, a native Windows ARM64 execution packet must re-establish exact checkout/tag/fixture/evidence-root/key/preflight invariants before the first provider-bound invocation. The first real `countTokens` request is part of the experiment, not a disposable compatibility probe; once provider invocation begins, the authorization is consumed by any terminal result and there is no automatic retry or fallback authority.
