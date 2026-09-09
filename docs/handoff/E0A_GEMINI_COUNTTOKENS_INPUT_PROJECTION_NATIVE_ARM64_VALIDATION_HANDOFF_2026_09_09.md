# E0-A Gemini `countTokens` Input Projection — Native Windows ARM64 Validation Handoff

Status: **NATIVE VALIDATION READY — PROVIDER AUTHORIZATION NONE**

Recorded: **2026-09-09**

Authority: subordinate to `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, the frozen correction amendment, and `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_IMPLEMENTATION_AUDIT_2026_09_09.md`.

## Purpose

Validate the exact documentation-inclusive Q-E0A-04 correction checkout on the Director's native Windows ARM64 machine. This is a non-network validation only. It does not authorize Gemini/OpenAI credentials, `countTokens`, inference, generation, spend, fallback, probe traffic, or E0-A rerun.

## Candidate identity

The Director packet accompanying this handoff must provide one exact candidate SHA from branch:

`e0a-gemini-counttokens-input-projection-correction`

Do not substitute a later branch head, `main`, a source-only predecessor, or an older validation tag. The machine-tested SHA must be the exact clean detached checkout used for every validation command.

The current promoted native authority before this validation remains:

`e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`

Do not repurpose or move the historical validated root.

## Worktree law

Use a separate linked worktree. The canonical/historical root at:

`C:\Users\Wiryl\Sol Dev\Ensemble-Project`

must remain at its pre-validation checkout, detached/clean if it is already detached/clean. Do not `checkout`, `switch`, `reset`, or force-move that worktree.

Recommended validation worktree:

`C:\Users\Wiryl\Sol Dev\Ensemble-Project-Worktrees\e0a-counttokens-input-projection-native-arm64`

If that path already exists, stop rather than deleting or repurposing it without inspection.

## Preflight

Before execution, verify:

- `PROCESSOR_ARCHITECTURE=ARM64`;
- `.NET` runtime/SDK resolves natively for ARM64;
- candidate SHA exists after `git fetch --prune origin`;
- `origin/e0a-gemini-counttokens-input-projection-correction` resolves exactly to the candidate SHA named in the Director packet;
- candidate worktree is detached and clean;
- canonical/historical root HEAD/status are recorded and unchanged;
- process-scoped `GEMINI_API_KEY` and `OPENAI_API_KEY` are absent.

## Required native package

Run the same established validation rung used for the bounded-diagnostic checkpoint, with expected Harness count adjusted only by actual discovery at the exact candidate:

1. delete stale `bin`/`obj` beneath current Core/Harness/test source surfaces in the validation worktree;
2. run complete `Ensemble.E0.Core.Tests`;
3. run complete `Ensemble.E0.Harness.Tests`;
4. perform a fresh Debug Harness build for `net9.0/win-arm64`;
5. run the frozen Missing Raft fixture smoke;
6. run the generic E0 smoke fixture;
7. with provider keys absent, invoke the current live profile selection boundary for:
   - `GEMINI-3.5-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL`;
   - `GEMINI-3.1-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL`;
   - `GEMINI-2.5-FLASH-LITE-NONE` / `CREATIVE-NONE`;
8. verify each current route returns the exact missing-key refusal before any evidence root is created;
9. verify retired `GEMINI-2.5-FLASH-NONE` returns the expected pre-credential live-selection rejection;
10. re-check candidate HEAD, detached/clean status, credential absence, evidence-root absence, and canonical/historical root preservation.

## Expected regression consequence

The prior promoted checkpoint had:

- Core: 622 tests;
- Harness: 130 tests.

Q-E0A-04 replaces one existing wire test and adds one new request-surface drift test. If no additional Harness-test change is present at the exact candidate, **131 Harness tests** are expected. The actual native test discovery/result is authoritative; do not force a count if repository state differs.

## Failure law

Any compiler failure, Core/Harness failure, fixture smoke failure, credentialless-boundary mismatch, evidence-root leak, dirty checkout, candidate mismatch, or worktree/root-preservation defect makes the native attempt **FAIL**.

Do not patch in the validation worktree. Preserve the exact failure output and return to engineering on the branch.

Do not create a validation tag for a failed or partially executed package.

## Success law

A successful package must establish:

- exact candidate SHA before and after all commands;
- native ARM64 host identity;
- complete Core/Harness pass counts;
- fresh `net9.0/win-arm64` build result with warnings/errors;
- both fixture smokes PASS;
- all credentialless route checks PASS;
- no provider network performed;
- spend 0;
- candidate worktree detached/clean;
- canonical/historical root unchanged;
- keys absent at closeout.

Only after those facts are captured may engineering create a new annotated validation tag at the exact candidate and close Q-E0A-04 in repository evidence/state.

## Provider boundary

Provider authorization is **NONE** before, during, and after this validation. Native validation success does not itself authorize any Gemini request.