# E0-A Phase B — Gemini Normative Reference Amendment Approval

Status: **APPROVED — WITH RECORDED IMPLEMENTATION-AUDIT CORRECTION**

Director approval date: **2026-09-06**

Approved architecture:

`docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`

## Decision

The Director approved the corrected E0-A Gemini amendment after recursive review for errors, inconsistencies, and possible improvements.

Approval authorizes the bounded Harness/test/evidence implementation needed to replace the next normative E0-A live provider route with Google Gemini API / stable `gemini-2.5-flash` while preserving Core and the deterministic H1 causal spine unchanged.

The approval includes these corrections from the Director-reviewed proposal:

- use native `generateContent` / `streamGenerateContent` for the normative route rather than the Interactions API because true thinking-off is required;
- authorize only `CREATIVE-NONE` on the Gemini live path for now;
- Performer/Interpreter use `thinkingBudget=0`;
- Integrity uses a fixed elevated `thinkingBudget=3584`, without claiming equivalence to OpenAI `high`;
- preserve an intended 4,096 generated-token ceiling and fail closed on observed overrun before semantic consumption;
- use conservative pre-spend Integrity risk accounting because `thinkingBudget` is advisory;
- use exact-request Gemini token counting;
- free-tier testing is synthetic-fixture-only and is not Production-provider admission;
- preserve the USD 5 deterministic run ceiling with conservative paid-tier shadow pricing even when actual free-tier billing is zero;
- Gemini implicit cache hits are recorded and cannot contribute to the normative no-implicit-cache reference condition;
- defer Gemini LOW/MEDIUM/HIGH characterization until a later provider-specific resource-normalization decision.

## Post-approval recursive implementation-audit correction

The Director-reviewed draft initially expressed Integrity as `thinkingBudget=3584` plus a 512-token `maxOutputTokens` request cap. During the already-authorized implementation audit, that partition was found to depend on an unsafe assumption about how Gemini applies `maxOutputTokens` relative to thinking.

The implementation and blueprint were corrected to request `maxOutputTokens=4096` for Integrity, matching the original E0-A role limit, while enforcing the approved 4,096 **combined observed** `candidatesTokenCount + thoughtsTokenCount` ceiling after receipt and before semantic use. Pre-spend Integrity risk accounting continues to reserve against the provider's larger published model-output limit.

This correction does not add a reasoning arm, raise the contributing-run generated-token ceiling, change Core, or broaden provider/network authority. It removes an implementation assumption so the already-approved 4,096 resource law remains fail-closed across provider accounting semantics.

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
