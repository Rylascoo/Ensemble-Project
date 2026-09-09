# E0-A Gemini `countTokens` Input Projection — Native Windows ARM64 Validation Handoff

Status: **NATIVE VALIDATION READY — PROVIDER AUTHORIZATION NONE**

Recorded: **2026-09-09**

Authority: subordinate to `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, the frozen correction amendment, and `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_IMPLEMENTATION_AUDIT_2026_09_09.md`.

Still-current continuity inputs:

- `docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md` — bootstrap/branch-topology classification;
- `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md` — quota snapshot/change-trigger boundary.

## Purpose

Validate the exact documentation-inclusive Q-E0A-04 correction checkout on the Director's native Windows ARM64 machine. This is non-network validation only. It does not authorize Gemini/OpenAI credentials, `countTokens`, inference, generation, spend, fallback, probes, or E0-A rerun.

## Candidate identity

The Director packet must provide one exact candidate SHA from branch `e0a-gemini-counttokens-input-projection-correction`. Do not substitute a later head, `main`, source-only predecessor, or older validation tag. Every validation command must use the same clean detached checkout.

Current promoted native authority before this validation remains `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`.

## Worktree law

Use a separate linked worktree. Never `checkout`, `switch`, `reset`, or repurpose the historical root:

`C:\Users\Wiryl\Sol Dev\Ensemble-Project`

Recommended validation worktree:

`C:\Users\Wiryl\Sol Dev\Ensemble-Project-Worktrees\e0a-counttokens-input-projection-native-arm64`

If that path already exists, inspect rather than deleting or repurposing it.

## Preflight

Verify before execution:

- `PROCESSOR_ARCHITECTURE=ARM64`;
- `dotnet --info` succeeds and reports Host architecture `arm64` plus RID `win-arm64`;
- candidate exists after `git fetch --prune origin`;
- `origin/e0a-gemini-counttokens-input-projection-correction` resolves exactly to the packet candidate;
- validation checkout is detached and clean;
- historical-root HEAD/status are recorded and remain unchanged;
- process `GEMINI_API_KEY` and `OPENAI_API_KEY` are absent.

Follow `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`; do not use blank-prone PowerShell `RuntimeInformation` properties as architecture gates.

## Required native package

Use the established native rung:

1. clear stale `bin`/`obj` beneath current Core/Harness/test surfaces;
2. run complete `Ensemble.E0.Core.Tests`;
3. run complete `Ensemble.E0.Harness.Tests`;
4. fresh Debug Harness build for `net9.0/win-arm64`;
5. frozen Missing Raft fixture smoke;
6. generic E0 smoke fixture;
7. with both provider keys absent, exercise current live selection boundaries for:
   - `GEMINI-3.5-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL`;
   - `GEMINI-3.1-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL`;
   - `GEMINI-2.5-FLASH-LITE-NONE` / `CREATIVE-NONE`;
8. require exact missing-key refusal and absent evidence root for each;
9. require retired `GEMINI-2.5-FLASH-NONE` pre-credential live-selection rejection;
10. re-check candidate HEAD, detached/clean status, keys absent, probe evidence roots absent, and historical-root preservation.

The prior promoted checkpoint had Core 622 and Harness 130 tests. Q-E0A-04 replaces one Harness wire test and adds one, so **131 Harness tests** are expected if no later Harness-test change exists. Actual native discovery is authoritative.

## Failure law

Any compiler/test/smoke/credentialless mismatch, evidence-root leak, dirty checkout, candidate mismatch, or root-preservation defect makes the attempt **FAIL**. Do not patch inside the validation worktree and do not create a validation tag for a failed/partial package.

For expected-failure native probes, follow the host-behavior law: temporarily use `$ErrorActionPreference = 'Continue'`, capture stdout/stderr to files, capture `$LASTEXITCODE` immediately, restore the setting, then assert exit/message/filesystem independently.

## Success law

Success must establish exact candidate identity before/after, native ARM64 host identity, complete Core/Harness pass counts, fresh `net9.0/win-arm64` build with warning/error counts, both fixture smokes, all credentialless route checks, no provider network, spend 0, detached/clean candidate checkout, unchanged historical root, and keys absent at closeout.

Only then may engineering create a new annotated validation tag at the exact candidate and update native evidence, `docs/VALIDATION_LEDGER.md`, `CURRENT_STATE.md`, and Q-E0A-04 closeout.

Provider authorization is **NONE** before, during, and after validation. Native success does not authorize a Gemini request.
