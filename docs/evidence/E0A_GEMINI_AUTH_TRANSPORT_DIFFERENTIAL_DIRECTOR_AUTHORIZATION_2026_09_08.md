# E0-A Gemini Auth-Transport Differential — Director Authorization

Status: **CONSUMED — TWO-LEG PACKET COMPLETE — NO FURTHER PROVIDER TRAFFIC AUTHORIZED**

Date: 2026-09-08

## Director authorization

The Director authorized exactly two Gemini authentication-differential requests using one fresh, unshared key:

1. one `gemini-3.5-flash-lite:countTokens` request authenticated with the `?key=` query parameter;
2. one otherwise identical `gemini-3.5-flash-lite:countTokens` request authenticated with the `x-goog-api-key` header.

The model, endpoint method, request body, client process, and credential were to remain identical between the two requests. Authentication transport was the only intentional variable.

## Mandatory local credential preflight — no network

Before either authorized request, the packet required a fresh AI Studio copy to match a second local paste exactly, with nonblank `AQ.` prefix, ASCII-only content, no whitespace or quote wrappers, and no credential emission. No undocumented exact-length rule was adopted.

## First local packet incident

The first Director execution aborted before network use because Windows PowerShell's loaded .NET surface did not expose `System.Convert.ToHexString`. It failed before `FIRST_NETWORK_LEG_STARTED=YES`; zero provider requests were attempted and authorization therefore remained unconsumed. The replacement packet used Windows-PowerShell-compatible hexadecimal conversion and moved static compatibility checks before credential entry.

## Consuming execution

The replacement packet passed static compatibility and credential-integrity preflights, then emitted `FIRST_NETWORK_LEG_STARTED=YES` and attempted both authorized legs.

Result authority is recorded in:

`docs/evidence/E0A_GEMINI_AUTH_TRANSPORT_DIFFERENTIAL_RESULT_2026_09_08.md`

Summary:

- query-auth leg: no HTTP response; local `System.Management.Automation.MethodInvocationException`; provider receipt/rejection not established;
- header-auth leg: HTTP **200**, `totalTokens=8`, no provider/transport error;
- no generation, inference, reference run, retry, fallback, or other model traffic;
- authorization consumed and provider authorization returned to **NONE**.

## Boundaries after consumption

No source change is authorized by this record. In particular, the inconclusive query leg is not evidence for changing the Harness away from `x-goog-api-key`; the successful header leg directly proves that transport works for a fresh `AQ.` key against `gemini-3.5-flash-lite:countTokens`.

Any further provider request requires a new explicit Director authorization.