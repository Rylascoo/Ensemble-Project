# E0-B Q-E0B-01 Run 01 — Public Provider Fact Audit

Date: 2026-09-12

Status: **PASS FOR PUBLIC FACTS — AUTHENTICATED PROJECT/TIER/QUOTA/CAPACITY FACTS REMAIN SEPARATE — ZERO PROVIDER CALLS**

## Exact scope

This audit supports reserved E0-B run `E0B-Q01-MIX-20260912-01` under approved condition `E0B-MIXED-CAST-01`. It does not authorize execution, credential access, an API probe, retry, fallback, or model substitution.

Native executable authority is `d1073fe2c76e2e05f2daac47465f86b48b456a9a` under annotated tag `validation/e0b-mixed-cast-implementation-native-arm64` (tag object `f7eadb838c4e01d9b4a01d17ff20b8d938046b03`).

The two exact live routes are:

- `gemini-3.5-flash-lite` — MARLOWE/WREN Performer, Integrity, Interpreter;
- `gemini-3.1-flash-lite` — VOSS Performer only.

## Current public lifecycle and capability facts

Official Gemini model/lifecycle surfaces were rechecked on 2026-09-12. Both exact model IDs remain listed. `gemini-3.5-flash-lite` has no announced shutdown date. `gemini-3.1-flash-lite` has an earliest shutdown date of 2027-05-07, with 3.5 Flash-Lite named as its recommended replacement.

Both routes remain compatible with the approved reasoning controls: Gemini 3.5/3.1 Flash-Lite support provider-native `minimal` and `high` thinking levels. The E0-B cast therefore retains `minimal` for Performer/Interpreter and `high` for Integrity without a semantic-control change.

The exact non-preview 3.1 endpoint remains the intended route; the former 3.1 preview endpoint is retired and is not used by E0-B.
## Pricing, data-use, and executable freshness

Official pricing remains aligned with the machine-validated catalog assumptions:

- 3.5 Flash-Lite paid-shadow rates: USD 0.30 / 1M input, USD 0.03 / 1M cached input, USD 2.50 / 1M output including thinking;
- 3.1 Flash-Lite paid-shadow rates: USD 0.25 / 1M text/image/video input, USD 0.025 / 1M cached text/image/video input, USD 1.50 / 1M output including thinking;
- Free-tier input/output remain free, while Free-tier submitted content remains eligible for use to improve Google products.

The route therefore remains synthetic-only. The canonical Missing Raft fixture contains no user-derived, private, production, or credential-bearing content.

`E0AGeminiPricingPolicy.SnapshotValidThrough = 2026-09-14`; the executable therefore remains inside its own fail-closed pricing/data-use freshness window on 2026-09-12. This audit does not extend that code guard.

## Structured-output and API surface

Current Gemini API documentation continues to support `generateContent`; the endpoint is described as legacy but remains available. E0-B uses the already machine-tested GenerateContent transport rather than changing API families.

The current structured-output support table explicitly names Gemini 3.1 Flash-Lite but does not separately enumerate Gemini 3.5 Flash-Lite. That omission is not treated as evidence of incompatibility: the Gemini 3 family documentation retains structured-output support and sealed Run 08 produced 36/36 successful exact-model 3.5 structured generations through the same request family. No compatibility probe is justified before E0-B.
## Rate-limit and authenticated account boundary

Google's current rate-limit documentation states that active limits are project/model/tier specific, are applied per project rather than per API key, are evaluated across RPM, input TPM, and RPD, and are viewed in Google AI Studio. RPD resets at midnight Pacific. Published documentation does not supply one universal Free-tier row that can replace the intended project's current AI Studio view.

The executable still contains historical 15 RPM / 250,000 input TPM / 500 RPD profiles for both selected routes. Those values are not promoted to current account facts by this audit. Before execution, authenticated AI Studio evidence for the intended protected-key project must establish the current exact 3.5 Flash-Lite and 3.1 Flash-Lite rows and current-day capacity. Any material mismatch stops activation for executable/profile audit and, if needed, renewed native validation.

The mixed run has a 12-turn ceiling, one attempt per role invocation, zero automatic retries, and at most 72 provider operations under the existing countTokens + generation discipline. Route-specific pacing remains enforced by the validated mixed-rate router and aggregate Gemini requests remain conservatively paced. No quota, availability, compatibility, or key-health probe is authorized to substitute for the authenticated UI gate.

## API-key/project facts

Google's current key documentation states that AI Studio keys are associated with Google Cloud projects and that newly created AI Studio keys are authorization keys. Historical Run 07 account evidence established the intended Kymaean project/key association at that time, but E0-B requires a fresh same-project/tier/current-quota confirmation because the active run uses two model rows and account state can change.

The protected credential blob and ready marker are present outside Git on the Director machine. This audit does not read or decrypt the credential. Plaintext key material must never enter Git, chat, command arguments, logs, or evidence and may be injected only into the intended child process after durable activation.

## Official sources rechecked 2026-09-12

- `https://ai.google.dev/gemini-api/docs/models`
- `https://ai.google.dev/gemini-api/docs/deprecations`
- `https://ai.google.dev/gemini-api/docs/pricing`
- `https://ai.google.dev/gemini-api/docs/thinking`
- `https://ai.google.dev/gemini-api/docs/generate-content/structured-output`
- `https://ai.google.dev/gemini-api/docs/rate-limits`
- `https://ai.google.dev/gemini-api/docs/api-key`

## Disposition

Public provider facts are **PASS** for reserved Run 01 and remain consistent with executable `d1073fe2c76e2e05f2daac47465f86b48b456a9a`. No source refresh or renewed native validation is required by the public recheck alone.

Execution remains **PROHIBITED** until fresh authenticated AI Studio evidence establishes the intended project/key association, Free tier, exact 3.5 and 3.1 RPM/TPM/RPD rows, and sufficient current-day capacity for the reserved run. This audit consumed zero provider calls and does not access the credential.
