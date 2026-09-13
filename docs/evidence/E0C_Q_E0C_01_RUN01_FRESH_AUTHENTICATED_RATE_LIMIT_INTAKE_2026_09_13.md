# E0-C Q-E0C-01 - Run 01 Fresh Authenticated Rate-Limit Intake

Date: 2026-09-13

Status: **FRESH AUTHENTICATED RATE-LIMIT/TIER/CAPACITY EVIDENCE ACCEPTED - EXACT PROJECT-ID/TESTING-KEY ASSOCIATION VIEW STILL PENDING - NO EXECUTION AUTHORITY**

## Source evidence

The Director supplied two fresh authenticated Google AI Studio rate-limit screenshots in the active Engineering conversation. The screenshots themselves are not committed because authenticated account UI context is not required for executable reproduction and must not become repository account material.

Exact received-file SHA-256 values:

- portrait/cropped rate-limit capture: `a18df146af2006027ed45131e3406565fb264b927bb1b1ae9fbecdbae8f0ce46`;
- wide rate-limit capture: `8da73795e40e91d26c9a3c94bb448ef230270bee315c5a3052d57e4d6cbca064`.

The wide capture is the controlling rate-limit evidence because it exposes RPM, TPM, and RPD simultaneously. The portrait capture independently corroborates the selected-project / Free-tier / RPM / TPM surfaces but does not expose the RPD column.

## Fresh UI facts established

The authenticated page is `Gemini API Rate Limit`, displays **Free tier**, and has a selected project label visibly truncated as `Gemini Project - Kymae...`, consistent with the previously authenticated Kymaean project but not sufficient by itself to prove the exact project ID.

The page explicitly describes the table as **peak usage per model compared to its limit over the last 28 days**. For `Gemini 3.5 Flash Lite`, the wide capture shows:

- RPM peak / limit: `9 / 15`;
- TPM peak / limit: `22.07K / 250K`;
- RPD peak / limit: `36 / 500`.

## Capacity interpretation

Because the displayed values are 28-day peak usage, current usage within that window cannot exceed the displayed peak. The capture therefore establishes conservative minimum headroom of at least `6 RPM`, `227.93K TPM`, and `464 RPD` for the exact 3.5 Flash-Lite row at capture time.

The frozen Slot 1 envelope is at most 72 provider operations and the validated Harness retains route pacing. The fresh rate-limit/tier/capacity evidence therefore presents no quota contradiction to the exact Run 08 profile. It does not guarantee provider availability and creates no probe, retry, concurrency, fallback, paid-tier, or route-change authority.

## Remaining authenticated-account boundary

The current E0-C preactivation requires fresh authenticated evidence of the intended exact project/account association as well as tier/quota/capacity. The rate-limit captures prove the fresh Free-tier 3.5 row and capacity, but the project selector is truncated and the separate account-association view was not supplied with this intake.

Historical E0-B evidence remains valid precedent for the intended Kymaean project association. Under the corrected E0-C activation law, that historical view is not silently promoted into the fresh exact-project gate.

Therefore the only authenticated UI blocker remaining is a fresh view that identifies the intended Kymaean project/project ID and confirms the existing testing credential belongs to it, without revealing credential contents. The commissioned Administrator browser remains Guest-only, so this UI fact must be supplied manually.

Protected local credential-container readiness has independently passed without exposing plaintext. Both canonical `IDENT` namespaces remain unconsumed. The exact UTC Slot-1-then-Slot-2 batch window remains intentionally **unfrozen** until the final activation record can bind it to the complete current execution facts.

Provider traffic remains **ZERO** under this intake.
