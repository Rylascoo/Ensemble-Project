# E0-C Q-E0C-01 - Public Provider Fact Audit

Date: 2026-09-12

Status: **CURRENT PUBLIC FACT SNAPSHOT - SUPPORTS PREACTIVATION ONLY - AUTHENTICATED CURRENT PROJECT/QUOTA GATE REMAINS SEPARATE**

## Scope

This audit rechecks only public Google Gemini facts required by the approved `E0C-IDENTICAL-REFERENCE-01` route. It sends no provider request and does not establish authenticated project/key/tier/capacity state.

## Current public facts

Official Google AI for Developers pages observed on 2026-09-12 establish:

- `gemini-3.5-flash-lite` remains listed as a **Stable** Gemini 3 endpoint with exact model string `gemini-3.5-flash-lite` (`https://ai.google.dev/gemini-api/docs/models`, page last updated 2026-09-04 UTC);
- the deprecations table lists `gemini-3.5-flash-lite` release date 2026-07-21 with **No shutdown date announced** (`https://ai.google.dev/gemini-api/docs/deprecations`, page last updated 2026-09-05 UTC);
- Gemini thinking documentation lists `gemini-3.5-flash-lite` default thinking `minimal` and supported levels `minimal`, `low`, `medium`, `high`, preserving the selected Run 08 Performer/Interpreter minimal and Integrity high controls (`https://ai.google.dev/gemini-api/docs/thinking`);
- Standard pricing for Gemini 3.5 Flash-Lite remains Free tier free of charge; paid shadow rates are USD `0.30` / 1M input tokens and USD `2.50` / 1M output tokens including thinking tokens; Free tier data is marked as used to improve Google products (`https://ai.google.dev/gemini-api/docs/pricing`);
- rate limits remain measured by RPM, input TPM, and RPD; limits are applied per project rather than per API key; RPD resets at midnight Pacific (`https://ai.google.dev/gemini-api/docs/rate-limits`).

## Interpretation

No public fact contradicts exact Run 08 route reuse. The public snapshot therefore supports continued preregistration/preactivation preparation for the stable 3.5 Flash-Lite Standard/Free route.

The existence of newer Gemini models does not authorize substitution. E0-C is an identical-condition experiment and remains pinned to the selected Run 08 route.

## Freshness / stop rule

Project freshness guard for this public snapshot is through **2026-09-14**, unless Google changes any material model/lifecycle/pricing/data-use/thinking/rate-limit fact sooner. Recheck before execution if that date is exceeded or any material source changes.

Authenticated AI Studio project/key/tier/quota/capacity evidence remains an execution-time gate and is not inferred from these public pages.
