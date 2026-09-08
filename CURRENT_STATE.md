# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: reference envelope + Gemini normative + rate-discipline comparison + RPD/model-selection amendment `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Promoted machine-tested checkout:** `3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99` / `validation/e0a-gemini-rpd-model-selection-native-arm64`. Native Windows ARM64: Core **622/622**, Harness **122/122**, fresh Harness build, both fixture smokes, current credentialless profile gates, retired-2.5-Flash rejection, exact HEAD/cleanliness: **PASS**. Evidence: `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`.

Implementation audit: `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_IMPLEMENTATION_AUDIT.md`. Supporting audits: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`. No Core/Core-test/fixture delta. Later documentation commits do not inherit native authority.

## Provider boundary
The one-run Director authorization is **CONSUMED**. First real synthetic Missing Raft `GEMINI-3.5-FLASH-LITE-MINIMAL` attempt at exact checkout `3d6d8a7f...` started `2026-09-08T04:39:59.7873582Z` and terminated `TechnicalFailure` at `2026-09-08T04:40:01.2732976Z`: exit 3, accepted turns 0, shadow spend `$0.000000`, stderr empty, 8 evidence files, post-run credential cleanup PASS. Evidence: `docs/evidence/E0A_FIRST_REAL_GEMINI_35_LITE_ATTEMPT_01_2026_09_08.md`.

Director observed no AI Studio usage shortly after the attempt. Zero shadow spend strongly indicates generation/inference was not reached, but whether the first `countTokens` HTTP request was reached remains unresolved until the preserved evidence archive is audited. Do not infer the exact provider boundary from dashboard absence alone.

No retry, second run, 3.1/2.5 run, other fixture, or broader provider traffic is authorized. API key remains environment/process-local only and must never enter repository, evidence, command output, or chat.

## Next
**Audit `E0A-REAL-G35L-20260908-01-evidence.zip` without rerunning Gemini.** Resolve the first failing event/transport boundary from `events.ndjson` and attempt evidence, then patch only if evidence identifies an implementation defect. Any later real-provider invocation requires new explicit Director authorization.
