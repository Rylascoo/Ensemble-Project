# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority; `AGENTS.md` defines bootstrap procedure and `docs/PROJECT_AUTHORITY.md` owns product/policy/lane authority.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 **CLOSED / PROMOTED / ARCHIVED**. `main` baseline: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Repository-convergence branch: `repo-convergence-integrity-2026-09-08`, forked from E0-A head `a6e12b033fd3136c4b6e167482d4c3c2d0c64029`.

Current architecture remains `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` plus its predecessors named in `docs/DOCUMENT_INDEX.md`.

## Validation
**Promoted executable checkout:** `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`. Director Windows ARM64: Core **622/622**, Harness **125/125**, build/smokes/credentialless gates **PASS**. Later convergence work is documentation/tooling only and does not inherit or replace that native-runtime authority.

## Provider evidence / decision
Attempt 03 sealed archive: first Performer `countTokens` HTTP **400** after `480.4031 ms`; 0 turns, $0, no generation. The preceding same-key/header/model/method simple `countTokens` succeeded; authentication transport is closed as that failure's explanation. Exact rejected field remains unproven. Classification: **3.5 Flash-Lite frozen-request live compatibility unproven**.

Director decision: **3.5 Flash-Lite live execution PAUSED; bounded provider-error/request-compatibility engineering amendment AUTHORIZED; 3.1 Flash-Lite provider execution DEFERRED. Provider authorization: NONE.** No credential use, `countTokens`, generation/probe, inference, spend, fallback, or other provider traffic.

## Next
Complete repository convergence: enforce state currency and residency, reconcile the evidence-lane branch, audit document/branch authority, restore a trustworthy `main`, and recursively verify no executable drift. The deferred technical handoff remains `docs/handoff/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_ENGINEERING_HANDOFF_2026_09_08.md`; do not execute it until convergence closes. After convergence, resume that amendment on a fresh engineering branch. Any source change requires cloud validation plus new native Windows ARM64 validation and annotated validation tag before any future provider request.
