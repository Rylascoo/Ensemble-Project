# E0-A Gemini Auth-Key External Diagnostic — 2026-09-08

Status: **DIRECTOR EXTERNAL DIAGNOSTIC COMPLETE — CURRENT AUTH KEY CAN GENERATE ON GEMINI 3.6 FLASH VIA QUERY-PARAMETER AUTH — TARGET `countTokens` DIFFERENTIAL STILL OPEN**

## Scope

This record captures a Director-performed diagnostic outside the controlled E0-A Harness run surface after real attempt 02 had already consumed its authorization. It is evidence about provider authentication behavior only; it is not an E0-A reference run, validation checkpoint, comparison result, or authorization for any further provider traffic.

The credential value is intentionally omitted. The Director pasted the temporary credential into chat and also placed it in a local PowerShell command, so that credential is treated as compromised and must be revoked. It must not be reused for further testing.

## Submitted result

The Director reported a successful raw REST `generateContent` request with:

- Gemini Developer API host: `generativelanguage.googleapis.com`;
- API version: `v1beta`;
- model: `gemini-3.6-flash`;
- method: `generateContent`;
- credential family: new `AQ.` authorization-key format;
- credential transport: `?key=` query parameter;
- client: Windows PowerShell `Invoke-RestMethod`;
- result: HTTP success with a normal text response.

The response text itself is not adopted as project evidence. Only the transport/authentication outcome is relevant.

## What this establishes

The successful request establishes that this newly created temporary `AQ.` key was complete enough, active enough, and bound to a usable Gemini project/account path for a current-model native `generateContent` request when transmitted through the query parameter. It also proves that Gemini 3.6 Flash native generation was reachable from the Director machine at the time of the test.

It does **not** establish:

- that the separate key used in attempt 02 was copied correctly;
- that `x-goog-api-key` succeeds for this auth-key/account path;
- that `models.countTokens` succeeds;
- that `gemini-3.5-flash-lite` succeeds;
- that the E0-A Harness request path is live-compatible;
- that any further E0-A/provider run is authorized.

## Reconciliation with current Google documentation

Official Gemini API-key documentation rechecked 2026-09-08 states:

- new AI Studio keys are authorization (`AQ.`) keys by default;
- auth keys are bound to a Google Cloud service account and restricted to the Gemini API by default;
- unrestricted **Standard** keys are rejected, and Standard keys are being retired for Gemini API use in September 2026;
- REST examples use `x-goog-api-key` for Gemini API authentication;
- the `models.countTokens` REST reference currently shows a shell example using `?key=`.

Therefore the September unrestricted-Standard-key policy is real, but it does not explain this successful `AQ.` auth-key request or by itself explain attempt 02's 401. Advice to fix a new auth key by converting it into a Standard key restricted in Cloud Console is not adopted. Google's current key documentation distinguishes auth keys from Standard-key restriction workflows.

Official references:

- `https://ai.google.dev/gemini-api/docs/api-key`
- `https://ai.google.dev/api/tokens`
- `https://ai.google.dev/gemini-api/docs/models/gemini-3.5-flash-lite`
- `https://ai.google.dev/gemini-api/docs/deprecations`

Gemini 3.5 Flash-Lite remains a current stable model with no shutdown date announced as of the same verification date, so model deprecation is not an evidence-supported explanation for attempt 02.

## Current fault tree

Attempt 02 at promoted checkout `689655eed677b789ab3ee395f1c65b4f2cb72cc8` reached first-Performer `countTokens` through `x-goog-api-key` and received HTTP 401. The new external success narrows the remaining live fault tree to at least these separable axes:

1. auth transport (`?key=` versus `x-goog-api-key`);
2. API method (`generateContent` versus `countTokens`);
3. target model (`gemini-3.6-flash` versus `gemini-3.5-flash-lite`);
4. the specific credential used in attempt 02 versus a newly created auth key.

The existing evidence does not justify changing source code yet because more than one axis changed between the failed Harness request and the successful external request.

## Next falsifiable provider gate

After revoking the exposed credential, the smallest useful live diagnostic is a fresh, unshared auth key used for a same-key/same-model/same-body **two-request `countTokens` transport differential** against `gemini-3.5-flash-lite`:

1. one `countTokens` request using `?key=`;
2. one otherwise identical `countTokens` request using `x-goog-api-key`.

No generation request is required for this differential. The pair must use a new non-exposed key and bounded output that records only HTTP status plus safe provider reason/status fields, never the credential or raw request URI.

This two-request probe is **not authorized by this document**. It requires a new explicit Director authorization before either request is sent.

Provider authorization after this external diagnostic: **NONE**.
