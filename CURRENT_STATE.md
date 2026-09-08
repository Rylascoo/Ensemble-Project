# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: RPD/model-selection amendment `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Promoted machine-tested checkout:** `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`. Director Windows ARM64: Core **622/622**, Harness **125/125**, fresh `win-arm64` build, fixture smokes and credentialless route gates: **PASS**. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_CORRECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`.

`countTokens` nested-model/diagnostic correction is promoted; later docs do not inherit native authority.

Real attempt 01 at `3d6d8a7f...` is consumed/failed before successful token count: `docs/evidence/E0A_FIRST_REAL_GEMINI_35_LITE_ATTEMPT_01_2026_09_08.md`. Attempt 02 at promoted `689655...` is consumed/failed at first Performer `countTokens`: HTTP **401**, 0 accepted turns, $0 shadow spend, no generation/inference. Archive/root-cause evidence: `docs/evidence/E0A_SECOND_REAL_GEMINI_35_LITE_ATTEMPT_02_ARCHIVE_AUDIT_2026_09_08.md`.

External Director diagnostic: a new `AQ.` auth key successfully reached native `gemini-3.6-flash:generateContent` via `?key=`. This narrows but does not resolve auth transport/method/model/key axes and does not validate the Harness. Evidence: `docs/evidence/E0A_GEMINI_AUTH_KEY_EXTERNAL_DIAGNOSTIC_2026_09_08.md`. The exposed diagnostic key is compromised and must be revoked/never reused.

Supporting audits: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

No retry, third reference run, 3.1/2.5 run, fallback, or other provider traffic is authorized. API keys must never enter repository/evidence/chat.

## Next
**Auth-transport differential decision.** After revoking the exposed key, the smallest useful live probe is a new unshared auth key with exactly two otherwise-identical `gemini-3.5-flash-lite:countTokens` requests: one via `?key=`, one via `x-goog-api-key`. No generation. Not authorized yet; both requests require new explicit Director authorization. Do not patch source before this differential.
