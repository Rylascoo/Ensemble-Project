# E0-C Q-E0C-01 - Public Provider Fact Audit

Date: 2026-09-12

Status: **PASS - EXACT RUN-08 ROUTE REMAINS PUBLICLY COMPATIBLE - AUTHENTICATED PROJECT/QUOTA FACTS REMAIN SEPARATE**

## Exact route

E0-C returns to the selected Run 08 route: Google Gemini Developer API `gemini-3.5-flash-lite`, provider profile `GEMINI-3.5-FLASH-LITE-MINIMAL`, Standard service behavior, creative roles at native `minimal`, Integrity at native `high`.

## Current official Google facts

Fresh official documentation review on 2026-09-12 found no public contradiction to the frozen executable assumptions:

- Models: `https://ai.google.dev/gemini-api/docs/models` lists Gemini 3.5 Flash-Lite as a stable model and exact endpoint `gemini-3.5-flash-lite`.
- Deprecations: `https://ai.google.dev/gemini-api/docs/deprecations` lists Gemini 3.5 Flash-Lite with release date 2026-07-21 and **no shutdown date announced**.
- Pricing: `https://ai.google.dev/gemini-api/docs/pricing` lists Standard Free Tier input/output as free of charge; paid shadow rates are USD 0.30/M input and USD 2.50/M output including thinking; Free-tier submitted content is marked as used to improve Google products.
- Thinking: `https://ai.google.dev/gemini-api/docs/thinking` lists `gemini-3.5-flash-lite` default `minimal` and supports `minimal`, `low`, `medium`, and `high`, preserving both Run 08 role controls.
- Rate limits: `https://ai.google.dev/gemini-api/docs/rate-limits` states limits are project-level and commonly enforced as RPM, input TPM, and RPD; model/project-specific current limits are exposed through AI Studio.

Google's 2026-07-21 release notes also identify Gemini 3.5 Flash-Lite as a generally available stable model.

## Exact executable freshness

The Run 08 executable's short-lived pricing/data-use guard remains valid through 2026-09-14. This audit found no material lifecycle, pricing, data-use, reasoning-control, or API-family change that invalidates that guard before the E0-C activation boundary.

## What this audit does not prove

Public documentation does not establish the intended account/project/key association or current per-project capacity. Those remain governed by authenticated AI Studio evidence and the approved exact-route quota-reuse decision. No provider request, `countTokens`, key-health probe, or availability probe was used to obtain this audit.
