# Q-E0A-03 Gemini 3.1 Flash-Lite Run 07 — Preactivation

Date: 2026-09-11

Status: **PARTIAL PASS — AUTHENTICATED AI STUDIO PROJECT GATE OPEN — PROVIDER TRAFFIC PROHIBITED**

## Exact reserved run boundary

- RunId: `E0A-Q03-G31L-20260911-07`
- executable: `bb869fb1c505603612bc718f739b3f1b358e5539`
- native tag: `validation/e0a-gemini-technical-failure-diagnostic-classification-native-arm64`
- tag object: `c4ff8dc4b9ebd31674208f4ba9c958d50fc975d5`
- profile: `CREATIVE-MINIMAL / GEMINI-3.1-FLASH-LITE-MINIMAL`
- model: `gemini-3.1-flash-lite`
- fixture: `ensemble.e0.missing-raft@0.1.0`
- canonical fixture SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`
- evidence root: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0A-Q03-G31L-20260911-07`

The RunId has no repository occurrence before this record, no active Harness process uses it, and the evidence root does not exist. This record reserves the identity only; it does not authorize execution while the authenticated project gate below remains open.

## Predecessor and authority

Run 06 is immutable/noncontributing after its first Performer generation returned HTTP 503 / `UNAVAILABLE` following a successful `countTokens`. Sealed predecessor: `docs/evidence/E0A_Q_E0A_03_G35L_RUN06_TERMINAL_EVIDENCE_ANALYSIS_2026_09_11.md`. PR #76 integrated the terminal closeout to `main` as `e63d942b168e97b9acf2a52a3667bafbb2c0e5bb`; post-merge Validation #659 (`34636200663`) passed. Archive tag `archive/q-e0a-03-run06-terminal-2026-09-11` peels to final branch head `f8a6dd33752e3ecbea105b561c18e896f5334b9f`.

The promoted executable remains governed by `docs/evidence/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_INTEGRATION_CLOSEOUT_2026_09_10.md`; Run 06 earned no source correction. The frozen comparison order therefore advances to the 3.1 full-reference candidate. `docs/evidence/GEMINI_API_PROJECT_RELEVANCE_STANDING_DIRECTOR_AUTHORIZATION_2026_09_10.md` permits one exact fresh project-relevant synthetic Free-tier run after all activation gates are durably satisfied; it does not permit bypassing account/quota verification, retries, fallback, paid routing, or experiment-order law.
## Public provider recheck

Official Google Gemini documentation was re-read on 2026-09-11:

- the active model list contains exact endpoint `gemini-3.1-flash-lite`;
- the deprecation schedule records release date 2026-05-07, earliest shutdown date 2027-05-07, and `gemini-3.5-flash-lite` as the recommended replacement;
- the former `gemini-3.1-flash-lite-preview` endpoint is shut down and explicitly redirects users to the exact non-preview model ID used here;
- Gemini API Free tier is available for `gemini-3.1-flash-lite`;
- Free-tier input/output are free of charge and Free-tier content may be used to improve Google products;
- current paid shadow rates are USD 0.25/M text-image-video input, USD 0.025/M cached input, and USD 1.50/M output including thinking, matching the executable profile;
- the current documented context/output limits are 1,048,576 / 65,536 tokens, matching the executable profile;
- `minimal` thinking is supported/default for 3.1 Flash-Lite and `high` is supported, matching Performer/Interpreter minimal and Integrity high;
- structured outputs remain supported for Gemini 3.1 Flash-Lite.

Official sources:

- `https://ai.google.dev/gemini-api/docs/models`
- `https://ai.google.dev/gemini-api/docs/deprecations`
- `https://ai.google.dev/gemini-api/docs/pricing`
- `https://ai.google.dev/gemini-api/docs/generate-content/gemini-3`
- `https://ai.google.dev/gemini-api/docs/generate-content/structured-output`
- `https://ai.google.dev/gemini-api/docs/rate-limits`
- `https://ai.google.dev/gemini-api/docs/api-key`

One current Google guide still says all Gemini 3 models are “currently in preview,” while the exact-model lifecycle surfaces distinguish `gemini-3.1-flash-lite` from the shut-down preview endpoint. This package does not infer a GA label from that inconsistent wording; only the independently documented exact endpoint, lifecycle, capabilities, Free-tier route, pricing and data-use facts are load-bearing.
## Rate-limit and key boundary

Google’s current rate-limit documentation no longer publishes one universal Free-tier RPM/TPM/RPD table. It states that active limits are model/project/tier specific, are applied per project rather than per API key, and must be viewed in Google AI Studio; RPD resets at midnight Pacific. The 2026-09-07 3.1 values encoded in the executable — 15 RPM / 250,000 input TPM / 500 RPD — are therefore historical until the intended project’s current AI Studio rate-limit view is rechecked.

Google’s current key documentation also states that every Gemini API key is associated with a Google Cloud project and that new AI Studio keys are authorization keys. Historical project evidence already established the protected Director-machine key as an `AQ.` authorization-key shape and Run 06 proved that protected path could reach real Gemini endpoints. That does not establish which current project/tier/rate-limit row governs 3.1 today.

The commissioned Administrator browser/CDP capability is deliberately a clean Guest-only profile with zero authenticated account residue. Attaching to the Director’s ordinary authenticated browser would violate that isolation contract. Therefore no automated authenticated AI Studio inspection was performed and no API request was substituted for the missing UI fact.

## Native/local preactivation

Preserved validator `C:\Users\Wiryl\Sol Dev\E0V-bb869fb` remains detached and clean at exact executable `bb869fb1c505603612bc718f739b3f1b358e5539`; the annotated validation tag dereferences exactly to it.

With that checkout as working directory, repository `global.json` selected SDK `9.0.317`. A fresh Release `win-arm64` Harness build succeeded with zero warnings/errors. The canonical Missing-Raft smoke passed. The exact 3.1 profile is live-selectable in the validated model catalog with `CREATIVE-MINIMAL`, 12 accepted turns, 15 RPM, 250,000 input TPM, 500 RPD, paid shadow rates 0.25 / 0.025 / 1.50, and 1,048,576 / 65,536 token limits.

A credentialless invocation of the exact 3.1 run path failed closed with `GEMINI_API_KEY is required at the E0-A provider edge.`, exit code 1, before evidence-root creation. Ambient `GEMINI_API_KEY` was absent before and after. This verifies checkout/fixture/profile/freshness routing without provider traffic or credential access.
## Sole remaining activation blocker

Before any provider traffic, an authenticated Google AI Studio view for the intended protected-key project must establish all of the following without exposing the credential:

1. the intended project is still on the approved Free tier;
2. the protected authorization key is associated with that intended project and remains the key intended for the run;
3. the project’s active `gemini-3.1-flash-lite` RPM, input-TPM and RPD limits are visible and compatible with the frozen run envelope;
4. if any current quota value differs from the encoded 15 RPM / 250,000 input TPM / 500 RPD snapshot, stop and audit whether the executable/profile requires correction and renewed native validation before traffic.

No compatibility ping, quota probe, key-health call, generation request, `countTokens` call, fallback model, or 3.5 replacement is authorized to substitute for this authenticated UI gate.

## Disposition

Run 07 is **RESERVED BUT NOT EXECUTABLE**. Public provider facts, exact native executable/tag, fixture/profile compatibility, freshness guard, fresh RunId/evidence root, standing authority and zero-provider local preflight are satisfied. The authenticated AI Studio project/key/quota fact remains unresolved.

Provider consumption after Run 06 remains **0 calls**. After the authenticated gate is durably recorded and this activation boundary is integrated with green post-merge Validation, standing Director authority may cover exactly one execution of this reserved RunId under the frozen no-retry/no-fallback/synthetic-only law. Until then, provider traffic remains prohibited.
