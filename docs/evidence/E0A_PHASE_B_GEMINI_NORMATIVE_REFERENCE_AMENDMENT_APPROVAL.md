# E0-A Phase B — Gemini Normative Reference Amendment Approval

Status: **APPROVED**

Director approval date: **2026-09-06**

Approved architecture:

`docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`

## Decision

The Director approved the corrected E0-A Gemini amendment after recursive review for errors, inconsistencies, and possible improvements.

Approval authorizes the bounded Harness/test/evidence implementation needed to replace the next normative E0-A live provider route with Google Gemini API / stable `gemini-2.5-flash` while preserving Core and the deterministic H1 causal spine unchanged.

The approval includes these corrections from the review:

- use native `generateContent` / `streamGenerateContent` for the normative route rather than the Interactions API because true thinking-off is required;
- authorize only `CREATIVE-NONE` on the Gemini live path for now;
- Performer/Interpreter use `thinkingBudget=0`;
- Integrity uses a fixed elevated `thinkingBudget=3584` with a 512-token visible candidate ceiling, without claiming equivalence to OpenAI `high`;
- preserve an intended 4,096 generated-token ceiling and fail closed on observed overrun before semantic consumption;
- pre-spend Integrity risk accounting uses the provider's published 65,536 output-token model limit because `thinkingBudget` is advisory;
- use exact-request Gemini token counting;
- free-tier testing is synthetic-fixture-only and is not Production-provider admission;
- preserve the USD 5 deterministic run ceiling with conservative paid-tier shadow pricing even when actual free-tier billing is zero;
- Gemini implicit cache hits are recorded and cannot contribute to the normative no-implicit-cache reference condition;
- defer Gemini LOW/MEDIUM/HIGH characterization until a later provider-specific resource-normalization decision.

## Authority boundary

This approval does **not** authorize:

- a real Gemini network request;
- use of `GEMINI_API_KEY` for inference;
- paid Gemini spend;
- private, personal, confidential, or Production-derived user material on the free-tier test route;
- E0-B or later phases;
- LOW/MEDIUM/HIGH Gemini characterization;
- Core redesign;
- product Application/persistence/UI, Windows AI/NPU, packaging, WACK, Store, or later scope.

The prior explicit OpenAI-run authorization was superseded by the Director's later correction that Ensemble testing will use Google Gemini API keys for the foreseeable future. It must not be interpreted as Gemini network authorization.

## Validation boundary

Implementation must return to Director-machine native Windows ARM64 validation before the first real Gemini call. Validation claims remain subject to `docs/PROJECT_AUTHORITY.md` and the project validation hierarchy.
