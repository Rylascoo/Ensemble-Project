# E0-A Gemini Auth-Transport Differential Result — 2026-09-08

Status: **COMPLETE — AUTHORIZATION CONSUMED — HEADER `countTokens` LIVE PASS — QUERY LEG LOCALLY INCONCLUSIVE — NO GENERATION**

## Scope

This record captures the Director-executed two-leg authentication differential authorized by `E0A_GEMINI_AUTH_TRANSPORT_DIFFERENTIAL_DIRECTOR_AUTHORIZATION_2026_09_08.md`.

Exactly one fresh, unshared `AQ.` Auth key was used for both legs. Model, endpoint method, JSON body, client process, and credential were held constant; credential transport was the intended variable. No generation, inference, reference run, retry, fallback, or other model traffic was authorized or performed by this packet.

## Local integrity preflight

Before network execution the replacement Windows PowerShell packet established:

- static body SHA-256 `A2D989D83CE9E0ECB71801EB1BDA66F530B8AA5ED5B6391C0F81C9A6A2DC818A`;
- HTTP assembly and request construction PASS;
- zero network requests before credential entry;
- two independently pasted copies of the fresh key matched byte-for-byte under ordinal comparison;
- `AQ.` prefix-class PASS;
- ASCII PASS;
- whitespace/wrapper checks PASS;
- observed key length 53 characters, recorded only as an observation and not treated as a format law.

The credential value was not emitted.

## Authorized leg results

### Leg 1 — `?key=` query parameter

The packet emitted `FIRST_NETWORK_LEG_STARTED=YES` and invoked the query-auth request path. It then returned:

- `HTTP_STATUS=NO_HTTP_RESPONSE`;
- `TOTAL_TOKENS=NONE`;
- no provider status/reason;
- `TRANSPORT_FAILURE_TYPE=System.Management.Automation.MethodInvocationException`;
- elapsed approximately 17 ms.

Because the packet intentionally suppressed exception messages that could contain a credential-bearing URI, the underlying local exception is not preserved. This leg therefore does **not** establish that Google received or rejected a query-auth request. Query-parameter behavior remains unresolved.

### Leg 2 — `x-goog-api-key` header

Using the same key, model, method, body, process, and client, the header-auth leg returned:

- HTTP **200**;
- `totalTokens=8`;
- no provider error status/reason;
- no transport failure;
- elapsed approximately 946 ms.

This is direct live evidence that a fresh `AQ.` Auth key succeeds with `x-goog-api-key` against `gemini-3.5-flash-lite:countTokens` from the Director machine.

## Audit conclusions

1. The two-request authorization is consumed. The first leg was attempted and the second leg definitely reached the provider; no rerun is authorized.
2. The current Harness use of `x-goog-api-key` is **not** a generic live incompatibility for Auth keys, `gemini-3.5-flash-lite`, or `countTokens`.
3. Attempt 02's HTTP 401 therefore cannot be used as evidence that header authentication itself is defective. A credential-specific failure on the attempt-02 key is now the leading explanation.
4. The query leg is locally inconclusive and supplies no evidence favoring a source change to query-parameter authentication.
5. Google's current Gemini API reference and token-counting guide also document `x-goog-api-key` for Gemini REST and `countTokens`; the live header PASS aligns with current provider documentation.
6. No source patch is justified by this differential.
7. This result does not yet prove the entire E0-A Harness live run. Attempt 02 used the Harness-composed nested `generateContentRequest` token-count body, while this differential used a minimal `contents` token-count body. The earlier 401 is authentication-class evidence, but body/live-generation behavior remains to be exercised by a separately authorized run.

Official provider references rechecked 2026-09-08:

- `https://ai.google.dev/api`
- `https://ai.google.dev/gemini-api/docs/tokens`
- `https://ai.google.dev/api/tokens`

## Authority after result

Provider authorization returns to **NONE**. No third reference run, generation request, retry, fallback, 3.1/2.5 traffic, or further authentication probe is authorized by this record.