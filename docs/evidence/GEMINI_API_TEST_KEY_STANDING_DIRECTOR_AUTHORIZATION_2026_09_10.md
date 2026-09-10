# Gemini API Test Key — Standing Director Authorization

Date: 2026-09-10

Status: **ACTIVE TEMPORARY TEST AUTHORITY**

The Director instructed that the verified Gemini API Auth key is a temporary project testing resource intended to be used efficiently to advance Ensemble/Kymaean toward completion. Engineering does not require a new Director approval for each bounded Gemini Free-tier compatibility or diagnostic request that is necessary to resolve an already-authorized Engineering test blocker.

This standing authority does **not** convert an experimental reference run into a retryable run. Frozen experiment identities, contribution law, accepted-turn caps, evidence immutability, and explicit full-reference-run boundaries remain intact. A consumed named run is never replayed under this authority.

## Boundaries

- synthetic/no-secret project fixtures and diagnostic payloads only;
- Gemini API Free tier only; no billing activation, paid fallback, or chargeable route without separate Director authority;
- use only current admitted Gemini models/endpoints relevant to active Engineering validation or compatibility work;
- bounded calls with a stated diagnostic/test purpose; avoid uncontrolled retry loops;
- preserve meaningful provider failures rather than retrying away evidence;
- never place the API key in Git, chat, evidence payloads, console transcripts, command arguments, or generated artifacts;
- local protected persistence on the Director machine is allowed for the duration of the testing program when it improves execution efficiency;
- every provider-use batch is recorded in `docs/evidence/GEMINI_API_USAGE_LEDGER.md` with date, purpose, model/endpoint, bounded call count, and result;
- when Gemini testing is complete, retire/revoke the key and record retirement in the usage ledger.

This record supersedes earlier per-call credential strictness only for bounded Free-tier Engineering compatibility/diagnostic testing. It does not authorize product-policy changes, user/private data, provider admission beyond testing, or paid spend.
