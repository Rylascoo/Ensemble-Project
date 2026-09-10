# Gemini API Usage Ledger

Status: **ACTIVE - PROJECT-RELEVANT GEMINI KEY**

Current authority: `docs/evidence/GEMINI_API_PROJECT_RELEVANCE_STANDING_DIRECTOR_AUTHORIZATION_2026_09_10.md`. Earlier diagnostic batches remain governed historically by `docs/evidence/GEMINI_API_TEST_KEY_STANDING_DIRECTOR_AUTHORIZATION_2026_09_10.md` and any exact run authorization recorded at the time.

This ledger records provider consumption without recording the credential. Every new batch must state its project relevance and how its result eliminates, defers, narrows, or justifies the next provider consumption.

| Date UTC | Batch / Run | Purpose | Model / endpoint | Calls | Result | Project relevance | Next-use optimization |
|---|---|---|---|---:|---|---|---|
| 2026-09-10 | `E0A-Q03-G35L-20260910-02` | Full-reference Run 02 under its then-exact one-run authorization; validate post-diagnostic generation path | `gemini-3.5-flash-lite`; `countTokens` + `streamGenerateContent` | 2 | `countTokens` PASS at 649 input tokens; generation HTTP 400 `INVALID_ARGUMENT`, field `generation_config.response_format.text.mime_type`; zero accepted turns; authorization consumed. | High: localized the next E0-A blocker after the corrected token-count path. | Never replay Run 02; use bounded diagnostics to isolate the rejected generation field before another full run. |
| 2026-09-10 | `GEMINI35-FORMAT-COMPAT-20260910-01` | Distinguish general 3.5 Flash-Lite generation failure from structured-output encoding incompatibility | `gemini-3.5-flash-lite`; `streamGenerateContent` | 3 | Bare generation HTTP 200; legacy `responseMimeType` + simple `responseSchema` HTTP 200; near-runtime `responseMimeType` + `responseJsonSchema` HTTP 200. Local summary serialization failed after responses; provider results preserved in diagnostic evidence. | High: proved the model/endpoint itself works and narrowed the defect to structured-output encoding. | Eliminate further general-availability probes; perform only one durable near-runtime capture of the corrected shape. |
| 2026-09-10 | `GEMINI35-FORMAT-COMPAT-20260910-02` | Durable capture of the working near-runtime legacy structured-output shape | `gemini-3.5-flash-lite`; `streamGenerateContent` | 1 | HTTP 200; candidate present; prompt 649 / candidate 114 / total 763 tokens; diagnostic 128-token cap ended `MAX_TOKENS`; request/response hashes preserved. | High: supplied the exact provider evidence needed for the narrow request-builder correction now native-validated at `cef3fc15...`. | No further compatibility-only probe is justified; next useful consumption is one fresh full-reference run through the corrected executable after non-approval activation gates pass. |

Launcher/setup failures before these batches consumed 0 provider calls.

## Consumption rule

Before provider traffic, record the exact question/run boundary and choose the lowest-call design that can materially advance the active project gate. After traffic, append actual calls/results, classify project relevance, and state the evidence-driven next-use reduction or justification. Stop when another call is unlikely to change a decision, resolve a blocker, or produce required experiment evidence.

## Retirement

Key retirement status: **ACTIVE FOR PROJECT-RELEVANT GEMINI WORK**.

When Gemini testing/experimental evidence closes, revoke or retire the key in Google AI Studio and append the retirement date and confirmation here. No key material is ever recorded in this ledger.
