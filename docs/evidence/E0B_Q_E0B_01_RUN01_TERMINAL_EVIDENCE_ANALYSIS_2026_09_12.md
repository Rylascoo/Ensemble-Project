# E0-B Q-E0B-01 Run 01 - Terminal Evidence Analysis

Date: 2026-09-12

Status: **TERMINAL - NONCONTRIBUTING - TURN 3 WREN PERFORMER HTTP 503 / UNAVAILABLE**

## Exact run

- RunId: `E0B-Q01-MIX-20260912-01`
- condition: `E0B-MIXED-CAST-01`
- executable: `d1073fe2c76e2e05f2daac47465f86b48b456a9a`
- native tag: `validation/e0b-mixed-cast-implementation-native-arm64`
- fixture: `ensemble.e0.missing-raft@0.1.0`
- canonical fixture SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`
- terminal: `TechnicalFailure`
- accepted turns: `2/12`
- final state hash: `69e85371060b965c6fb6d5ad48ee446a6195a4d2efb4b533638e396c9bd8968c`
- final Opportunity: `WREN`
- conservative shadow estimate: USD `0.17175080`
- unknown provider usage: `true`

Run 01 executed exactly once after its activation and continuity closeout were integrated and exact-main Validation was green. The evidence root and deterministic run/root claims now exist, so this RunId/root are permanently consumed and may never be replayed.

Current predecessor chain retained by this terminal successor:

- `docs/evidence/E0B_MIXED_CAST_IMPLEMENTATION_INTEGRATION_CLOSEOUT_2026_09_12.md`
- `docs/evidence/E0B_Q_E0B_01_RUN01_PUBLIC_FACT_AUDIT_2026_09_12.md`
- `docs/evidence/E0B_Q_E0B_01_RUN01_BLIND_SCORING_INSTRUMENT_2026_09_12.json`
- `docs/evidence/E0B_Q_E0B_01_RUN01_PREACTIVATION_2026_09_12.md`
- `docs/evidence/E0B_Q_E0B_01_RUN01_PREEXECUTION_ACTIVATION_2026_09_12.md`
- `docs/evidence/E0B_Q_E0B_01_RUN01_ACTIVATION_INTEGRATION_CLOSEOUT_2026_09_12.md`
## Provider consumption and runtime result

Run 01 performed exactly **14 Gemini API operations** under the frozen pacing law: seven successful `countTokens` preflights and seven generation attempts. Six generations succeeded; the seventh, Turn 3 WREN Performer on `gemini-3.5-flash-lite`, returned HTTP `503` with sanitized status `UNAVAILABLE`.

Turns 1 and 2 completed Performer -> Integrity -> Interpreter -> deterministic authority -> Take/commit and advanced Opportunity `VOSS -> MARLOWE -> WREN`. The alternate VOSS Performer route `gemini-3.1-flash-lite` succeeded on Turn 1 with exact returned model identity; MARLOWE Performer plus both Integrity/Interpreter cycles succeeded on the 3.5 reference route.

The seven `countTokens` preflights projected `10,790` input tokens. Six successful generation receipts total `9,925` input tokens, `1,942` output tokens including `1,458` reasoning tokens, and `0` cached input tokens. The terminal WREN generation has no response ID, returned model, usage receipt, stream, or structured output. Its exact diagnostic is `gemini-http-503;status=UNAVAILABLE`.

The Harness retained the failed-call reservation in the conservative terminal estimate, yielding USD `0.17175080` with `hasUnknownProviderUsage=true`. There was one attempt per role invocation, zero automatic retries, no fallback, no model/profile/Fixture substitution, and no second launch.

## Evidence integrity

The sealed runtime contains `24` rooted artifacts plus `run.final.json`. Independent SHA-256 recomputation found `0` artifact mismatches across the seal. Bounded scans for `GEMINI_API_KEY`, `x-goog-api-key`, Authorization/Bearer markers, `AIza`, and `AQ.` found `0` matches across all `25` files.

- runtime root: `744cb09383de4be17cadd35b4fd71d2242affde8262b247c394ba8caf8790345`
- runtime seal identity: `31615c838d2e216783e6014f21f4341ab95b25cc41b1c236349fa177a57a3609`
- `run.summary.json` SHA-256: `3c9d92c61dc41c25785915510438298664748853a939a5cbb78654b718f282d2`

The two accepted performances and their deterministic commits remain immutable evidence. No 12-turn contribution, hard-gate PASS, experiential score, or unblinding is asserted.
## Classification

Run 01 proves the mixed-cast apparatus exercised both intended model routes before the terminal. The 3.1 VOSS Performer path succeeded with exact returned `gemini-3.1-flash-lite`; the 3.5 MARLOWE Performer, Integrity, and Interpreter paths also succeeded repeatedly. No source, schema, parser, control-ID, deterministic-authority, provenance, pricing, rate-discipline, or evidence-sealing defect is established by this run.

The terminal event is bounded provider **HTTP availability-class evidence** for one Turn 3 WREN Performer generation on `gemini-3.5-flash-lite`. It does not establish credential rejection, quota exhaustion, structured-output incompatibility, a persistent outage, or a mixed-cast-specific model incompatibility. The evidence is insufficient to infer Google's root cause or future availability.

This result is materially consistent with the prior E0-A availability-class failures while remaining a separate E0-B condition result. No executable/source correction is earned.

## Contribution and next boundary

Run 01 is permanently consumed and **noncontributing** because only `2/12` required accepted turns completed. Under the frozen E0-B method, only a contributing transcript with complete runtime seal and hard-gate PASS may enter blind experiential scoring; therefore the frozen scoring instrument remains unused and no unblinding occurs.

The activation authorized exactly one execution and explicitly authorized no retry, replay, fallback, replacement RunId, alternate route/credential, paid/Priority switch, or model substitution. This terminal record creates none of those authorities.

Q-E0B-01 therefore remains open but provider traffic returns to **zero** pending a Director disposition: either close E0-B on this preserved noncontributing condition evidence and advance in frozen experiment order, or separately authorize a materially justified replacement under a new audited decision/activation. Q-E0C-01 remains blocked until that disposition closes E0-B.
