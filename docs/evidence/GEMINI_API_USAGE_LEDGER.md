# Gemini API Usage Ledger

Status: ACTIVE — TEMPORARY TEST KEY

Authority: `docs/evidence/GEMINI_API_TEST_KEY_STANDING_DIRECTOR_AUTHORIZATION_2026_09_10.md`.

This ledger records Gemini API use without recording the credential itself. Earlier historical Gemini invocations remain documented in their original evidence records; this consolidated ledger begins with the Director's 2026-09-10 standing test-key instruction.

| Date UTC | Batch / Run | Purpose | Model / endpoint | Calls | Result |
|---|---|---|---|---:|---|
| 2026-09-10 | `E0A-Q03-G35L-20260910-02` | Full-reference Run 02 under its exact one-run authorization; validate post-diagnostic generation path | `gemini-3.5-flash-lite`; `countTokens` + `streamGenerateContent` | 2 | `countTokens` PASS at 649 input tokens; generation HTTP 400 `INVALID_ARGUMENT`, field `generation_config.response_format.text.mime_type`; zero accepted turns; authorization consumed. |

## Retirement

Key retirement status: **ACTIVE FOR BOUNDED TESTING**.

When Gemini testing closes, revoke/retire the key in Google AI Studio and append the retirement date and confirmation here. No key material is ever recorded in this ledger.
