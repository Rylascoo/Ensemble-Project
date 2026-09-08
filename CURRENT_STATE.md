# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: reference envelope + Gemini normative + rate-discipline comparison + RPD/model-selection amendment `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Promoted machine-tested checkout:** `3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99` / `validation/e0a-gemini-rpd-model-selection-native-arm64` — annotated tag remotely verified. Native Windows ARM64: Core **622/622**, Harness **122/122**, fresh Harness build, both fixture smokes, current credentialless profile gates, retired-2.5-Flash rejection, exact HEAD/cleanliness: **PASS**. Evidence: `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`.

Attempt 01 at `31436ee5...` failed Harness 121/122 from a stale readiness expectation; correction `b4d39cd91d1c23bad1f0354702fb64411e82780d` changed only that test surface. Audit: `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_IMPLEMENTATION_AUDIT.md`. Supporting audits: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

No Core/Core-test/fixture delta. Later documentation commits do not inherit native authority.

## Provider boundary
**AUTHORIZED ONCE:** exactly one real synthetic Missing Raft `GEMINI-3.5-FLASH-LITE-MINIMAL` run at promoted checkout `3d6d8a7f...`. Authorization: `docs/evidence/E0A_FIRST_REAL_GEMINI_35_LITE_DIRECTOR_AUTHORIZATION_2026_09_08.md`.

Scope: `CREATIVE-MINIMAL`; 12-turn cap; 15 RPM / 250K TPM / 500 RPD; one attempt/role; zero retries; existing $5 shadow ceiling/evidence laws. Free-tier input remains synthetic only. Director confirmed no other material Gemini traffic since quota evidence. Any terminal result consumes this authorization. No 3.1/2.5 run, second run, retry, other fixture, or broader provider traffic is authorized.

API key remains environment/process-local only and must never enter repository, evidence, command output, or chat.

## Next
**Execute the single authorized real run on the Director Windows ARM64 host from exact checkout `3d6d8a7f...`, capture the complete terminal output and evidence-root path, then stop.** Audit the resulting evidence before any further provider authorization or comparison run.
