# E0-A Gemini Auth-Transport Differential — Director Authorization

Status: **AUTHORIZED — EXACTLY TWO `countTokens` REQUESTS — NO GENERATION — NO RETRIES**

Date: 2026-09-08

## Director authorization

The Director explicitly authorized exactly two Gemini authentication-differential requests using one fresh, unshared key:

1. one `gemini-3.5-flash-lite:countTokens` request authenticated with the `?key=` query parameter;
2. one otherwise identical `gemini-3.5-flash-lite:countTokens` request authenticated with the `x-goog-api-key` header.

The model, endpoint method, request body, client process, and credential must be identical between the two requests. Authentication transport is the only intentional variable.

## Boundaries

- No `generateContent`, inference, fiction generation, reference run, fallback, or other provider request is authorized.
- No automatic or manual retry is authorized.
- The exposed `AQ.` key from the preceding external diagnostic is compromised and must not be used. The differential requires a fresh key that is never pasted into chat, repository content, command text, files, logs, or evidence.
- The probe may report only bounded safe results: request leg, HTTP status, `totalTokens` on success, and provider error status/reason when safely parseable. It must not print the request URI, credential, raw headers, or raw provider body.
- Once the first network leg is attempted, do not rerun the packet. If local execution aborts after one leg, stop and audit rather than duplicating a request.
- After the two authorized legs terminate, provider authorization returns to **NONE**.

## Purpose

Attempt 02 at promoted checkout `689655eed677b789ab3ee395f1c65b4f2cb72cc8` reached `gemini-3.5-flash-lite:countTokens` via `x-goog-api-key` and received HTTP 401. A later Director external diagnostic successfully used a newly created `AQ.` auth key for `gemini-3.6-flash:generateContent` via `?key=`. This differential isolates authentication transport while holding key/model/method/body constant.

No source change is authorized by this document. The result must be audited before any implementation decision.