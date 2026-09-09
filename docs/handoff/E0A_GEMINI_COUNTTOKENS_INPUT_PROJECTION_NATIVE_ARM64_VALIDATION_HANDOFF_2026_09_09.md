# E0-A Gemini `countTokens` Input Projection — Native Windows ARM64 Validation Handoff

Status: **NATIVE ATTEMPT 01 FAILED — DIAGNOSIS REQUIRED — PROVIDER AUTHORIZATION NONE**

Recorded: **2026-09-09**

Authority: subordinate to `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, the frozen correction amendment, and `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_IMPLEMENTATION_AUDIT_2026_09_09.md`.

Still-current continuity inputs:

- `docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md` — bootstrap/branch-topology classification;
- `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md` — quota snapshot/change-trigger boundary.

## Purpose

Validate the exact documentation-inclusive Q-E0A-04 correction checkout on the Director's native Windows ARM64 machine. This is non-network validation only. It does not authorize Gemini/OpenAI credentials, `countTokens`, inference, generation, spend, fallback, probes, or E0-A rerun.

## Candidate identity

Attempt 01 candidate:

`de38d5d52279c22a1786e11200239c445e04377b`

Branch: `e0a-gemini-counttokens-input-projection-correction`.

Current promoted native authority remains `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`.

## Attempt 01 result

Attempt 01 is frozen as **FAIL** in:

`docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_ATTEMPT_01_2026_09_09.md`

Established:

- exact detached candidate `de38d5d5...`;
- native Windows ARM64 host identity passed;
- provider keys absent before testing;
- stale outputs cleared;
- Core **622/622 PASS**;
- Harness **130/131 FAIL**.

The packet hard-stopped on `HARNESS_TEST_EXIT=1`. Fresh Harness build, fixture smokes, credentialless route gates, and final closeout checks were not executed and must not be inferred.

The submitted console output did not contain the failing test identity, expected/actual assertion, or stack trace. The next action is to read the existing native result log:

`tests\Ensemble.E0.Harness.Tests\bin\Debug\net9.0\TestResults\Ensemble.E0.Harness.Tests_net9.0_arm64.log`

Do not patch source/tests and do not rerun the full native package until that failure is classified.

## Worktree law

Use the existing separate detached worktree only for read-only failure diagnosis until a correction is frozen:

`C:\Users\Wiryl\Sol Dev\Ensemble-Project-Worktrees\e0a-counttokens-input-projection-native-arm64`

Never `checkout`, `switch`, `reset`, patch, or repurpose the historical root:

`C:\Users\Wiryl\Sol Dev\Ensemble-Project`

Follow `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`; do not use blank-prone PowerShell `RuntimeInformation` properties as architecture gates.

## Native package after correction

If diagnosis proves a source/test correction is required, freeze a new exact candidate and repeat the complete established native rung from the beginning:

1. clear stale `bin`/`obj` beneath current Core/Harness/test surfaces;
2. complete `Ensemble.E0.Core.Tests`;
3. complete `Ensemble.E0.Harness.Tests`;
4. fresh Debug Harness build for `net9.0/win-arm64`;
5. frozen Missing Raft fixture smoke;
6. generic E0 smoke fixture;
7. current live-selection credentialless boundaries for Gemini 3.5 Flash-Lite, 3.1 Flash-Lite, and 2.5 Flash-Lite with both provider keys absent;
8. exact missing-key refusal and absent evidence root for each;
9. retired `GEMINI-2.5-FLASH-NONE` pre-credential rejection;
10. final candidate HEAD/detached/clean, key absence, evidence-root absence, and historical-root preservation.

## Failure law

Any compiler/test/smoke/credentialless mismatch, evidence-root leak, dirty checkout, candidate mismatch, or root-preservation defect makes an attempt **FAIL**. Do not patch inside the validation worktree and do not create a validation tag for a failed/partial package.

For expected-failure native probes, follow the host-behavior law: temporarily use `$ErrorActionPreference = 'Continue'`, capture stdout/stderr to files, capture `$LASTEXITCODE` immediately, restore the setting, then assert exit/message/filesystem independently.

## Success law

Success must establish exact candidate identity before/after, native ARM64 host identity, complete Core/Harness pass counts, fresh `net9.0/win-arm64` build with warning/error counts, both fixture smokes, all credentialless route checks, no provider network, spend 0, detached/clean candidate checkout, unchanged historical root, and keys absent at closeout.

Only then may engineering create a new annotated validation tag at the exact candidate and update native evidence, `docs/VALIDATION_LEDGER.md`, `CURRENT_STATE.md`, and Q-E0A-04 closeout.

Provider authorization is **NONE** before, during, and after validation. Native success does not authorize a Gemini request.
