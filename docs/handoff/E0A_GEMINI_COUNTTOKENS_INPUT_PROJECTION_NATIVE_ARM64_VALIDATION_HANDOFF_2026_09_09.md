# E0-A Gemini `countTokens` Input Projection — Native Windows ARM64 Validation Handoff

Status: **NATIVE ATTEMPT 01 DIAGNOSED — TEST-ONLY CORRECTION HOSTED GREEN — ATTEMPT 02 PENDING FINAL HOSTED GREEN — PROVIDER AUTHORIZATION NONE**

Recorded: **2026-09-09**

Authority: subordinate to `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, the frozen correction amendment, and `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_IMPLEMENTATION_AUDIT_2026_09_09.md`.

Still-current continuity inputs:

- `docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md` — bootstrap/branch-topology classification;
- `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md` — quota snapshot/change-trigger boundary;
- `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md` — current Gemini-3 response compatibility boundary;
- `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_ATTEMPT_01_2026_09_09.md` — failed Attempt-01 evidence and diagnosis.

## Purpose

Validate the exact documentation-inclusive Q-E0A-04 correction checkout on the Director's native Windows ARM64 machine. This is non-network validation only. It does not authorize Gemini/OpenAI credentials, `countTokens`, inference, generation, spend, fallback, probes, or an E0-A provider run.

## Attempt 01

Attempt 01 candidate:

`de38d5d52279c22a1786e11200239c445e04377b`

Result: **FAIL — Core 622/622 PASS; Harness 130/131 FAIL**.

Read-only MSTest-log extraction proved the sole failure was:

`GeminiModelComparisonTests.GenerateContentPort_AcceptsEveryApprovedComparisonModelForSchemaCompleteTokenPreflight`

The stale oracle expected the superseded full-copy `countTokens` body (`5` nested fields) while the frozen Q-E0A-04 contract intentionally requires the three-field input-semantic projection (`model`, `systemInstruction`, `contents`). This is a **test-oracle defect, not a production defect**.

Test-log SHA-256:

`1A79B95C4683FD0B212721E03ADCD8335730292415604D845793BA50392A18BF`

Test-only correction checkpoint:

`48a6e67a5f5834b40bcca1b530b87f537140984e`

Hosted Validation gate `34402647899`: **SUCCESS**.

No runtime/source correction was made after the original Q-E0A-04 production implementation.

## Attempt 02 candidate identity

Branch:

`e0a-gemini-counttokens-input-projection-correction`

Attempt 02 must use the exact final branch HEAD after the Attempt-01 diagnosis/continuity package itself passes the complete hosted Validation gate. The execution packet must pin that SHA explicitly and require `origin/e0a-gemini-counttokens-input-projection-correction` to resolve to the same SHA before testing.

Current promoted native authority remains:

`e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`

No later commit inherits that native authority.

## Worktree law

Preserve the failed Attempt-01 worktree and its native log as historical evidence:

`C:\Users\Wiryl\Sol Dev\Ensemble-Project-Worktrees\e0a-counttokens-input-projection-native-arm64`

Do not reset, switch, patch, clean, delete, or repurpose that worktree for Attempt 02.

Use a new linked detached worktree for Attempt 02:

`C:\Users\Wiryl\Sol Dev\Ensemble-Project-Worktrees\e0a-counttokens-input-projection-native-arm64-attempt02`

Never `checkout`, `switch`, `reset`, or repurpose the historical root:

`C:\Users\Wiryl\Sol Dev\Ensemble-Project`

Follow `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`; do not use blank-prone PowerShell `RuntimeInformation` properties as architecture gates.

## Attempt 02 preflight

Verify before execution:

- `PROCESSOR_ARCHITECTURE=ARM64`;
- `dotnet --info` succeeds and reports Host architecture `arm64` plus RID `win-arm64`;
- `git fetch --prune origin` succeeds;
- remote correction branch resolves exactly to the supplied Attempt-02 candidate;
- Attempt-01 worktree still resolves to failed candidate `de38d5d5...` and is not modified by Attempt 02;
- new Attempt-02 checkout is detached and clean;
- historical-root HEAD/status are recorded and remain unchanged;
- process `GEMINI_API_KEY` and `OPENAI_API_KEY` are absent.

## Required native package

Repeat the complete established rung from the beginning; no Attempt-01 partial result carries forward:

1. clear stale `bin`/`obj` beneath current Core/Harness/test surfaces in the Attempt-02 worktree;
2. run complete `Ensemble.E0.Core.Tests`;
3. run complete `Ensemble.E0.Harness.Tests` and require all **131** discovered tests to pass if discovery remains unchanged;
4. fresh Debug Harness build for `net9.0/win-arm64`;
5. frozen Missing Raft fixture smoke;
6. generic E0 smoke fixture;
7. with both provider keys absent, exercise current live-selection boundaries for:
   - `GEMINI-3.5-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL`;
   - `GEMINI-3.1-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL`;
   - `GEMINI-2.5-FLASH-LITE-NONE` / `CREATIVE-NONE`;
8. require exact missing-key refusal and absent evidence root for each;
9. require retired `GEMINI-2.5-FLASH-NONE` pre-credential live-selection rejection;
10. re-check Attempt-02 HEAD, detached/clean status, keys absent, probe evidence roots absent, Attempt-01 worktree preservation, and historical-root preservation.

## Failure law

Any compiler/test/smoke/credentialless mismatch, evidence-root leak, dirty checkout, candidate mismatch, Attempt-01 worktree mutation, or historical-root preservation defect makes Attempt 02 **FAIL**. Do not patch inside either native-validation worktree and do not create a validation tag for a failed/partial package.

For expected-failure native probes, follow the host-behavior law: temporarily use `$ErrorActionPreference = 'Continue'`, capture stdout/stderr to files, capture `$LASTEXITCODE` immediately, restore the setting, then assert exit/message/filesystem independently.

## Success law

Success must establish exact candidate identity before/after, native ARM64 host identity, complete Core/Harness pass counts, fresh `net9.0/win-arm64` build, both fixture smokes, all credentialless route checks, no provider network, spend 0, detached/clean Attempt-02 checkout, unchanged Attempt-01 worktree, unchanged historical root, and keys absent at closeout.

Only then may engineering create a new annotated validation tag at the exact Attempt-02 candidate and update native evidence, `docs/VALIDATION_LEDGER.md`, `CURRENT_STATE.md`, and Q-E0A-04 closeout.

Provider authorization is **NONE** before, during, and after validation. Native success does not authorize a Gemini request.
