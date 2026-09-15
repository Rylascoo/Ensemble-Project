# E0-D Q-E0D-01 — P01 Slot 1 Preexecution Activation

Date: 2026-09-15

Status: **ACTIVATION PACKAGE PREPARED — EXACT P01 SLOT 1 ONLY — PROVIDER TRAFFIC ZERO — LIVE LAUNCH REQUIRES INTEGRATION + EXACT-MAIN VALIDATION + FROZEN WINDOW + SLOT-FRESH AUTHENTICATED CAPACITY PASS**

## Authority chain

Current checkpoint: `CURRENT_STATE.md`; queue: `docs/PROJECT_EXECUTION_QUEUE.md`.

Frozen method: `docs/blueprint/E0D_ABLATION_CONTROLS_METHOD_AND_ISOLATION_CONTRACT_01.md`.

Frozen preregistration / blind instrument: `docs/evidence/E0D_Q_E0D_01_PREREGISTRATION_2026_09_14.json`, SHA-256 `fdc40125cb34a76245040631d25a48934f5664897dda504964d83144231f1d37`.

Frozen allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`, SHA-256 `d9fd8f5384de8cf78c137ddfa62adde04a92b89096abb42434e6b738ef206194`.

Renewed native authority: `docs/evidence/E0D_SOURCE_SNAPSHOT_REFRESH_NATIVE_ARM64_VALIDATION_2026_09_14.md`, SHA-256 `beea96dcbecfd9451d57257794c02aa1f728026007806f7d0c813f0f21b6ede2`.

Director live-execution authorization: `docs/evidence/E0D_P01_SLOT1_LIVE_EXECUTION_DIRECTOR_AUTHORIZATION_2026_09_15.md`, SHA-256 `ef5ba2e8d2932be433f5ee6cdd74bd381c028564a6270eb7ca9da0e7b30888fa`.

PR #145 integrated that authorization as Project `main@b3b1486ae108e4b6dc09db5d6c6e17999d32ce7d`; push-triggered exact-main Validation #849 passed. The authorization source is archive-tagged at `archive/e0d-p01-slot1-live-authorization-2026-09-15` and its temporary branch is retired.

## Exact Slot 1 identity

- pair: `E0D-P01-RELATIONSHIP-OMISSION`;
- ordinal / slot: `1` / `P01-FULL`;
- variant: `E0D-FULL-REFERENCE-01`;
- RunId: `E0D-Q01-P01-FULL-20260914-01`;
- evidence root: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0D-Q01-P01-FULL-20260914-01`;
- run claim: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\.ensemble-e0a-claims\run-944cb219c94ba067690ddd2512bdc300dcc9517653407e3aa0ec86858336568c.claim`;
- root claim: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\.ensemble-e0a-claims\root-42a75e08f7d968665af1f70a80abfa6f7d1f2331228148993dc9f9ba70adad29.claim`;
- immutable P01 window: `2026-09-16T14:30:00Z..23:30:00Z`.

## Frozen runtime condition

- executable checkout: `0dacdbf6bd5453c192568cd4718207e145dfcf40`;
- validation tag: `validation/e0d-snapshot-refresh-native-arm64`;
- executable SHA-256: `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`;
- provider/model/profile: Google Gemini API / `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`;
- service tier request: `standard` / authenticated account tier required at launch: Free;
- Performer reasoning: `minimal`; Integrity: `high`; Interpreter: `minimal`;
- Fixture: `ensemble.e0.missing-raft@0.1.0`, canonical SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`;
- accepted-turn cap: `12`; attempts per invocation: `1`; automatic retries: `0`; timeout: `300s`; max visible output tokens per role: `4096`;
- no retry, replay, replacement, fallback, paid/Priority route, model/profile/fixture substitution or retune.

The E0-D source snapshot is valid inclusively through UTC `2026-09-18` and fails closed from UTC `2026-09-19`. P01 lies inside that validated freshness boundary.

## Fresh local activation preflight

On Director host `SurfSeven`, the validation tag freshly dereferenced to exact checkout `0dacdbf6bd5453c192568cd4718207e145dfcf40`.

A fresh Release `win-arm64` Harness build from that detached exact checkout reproduced executable SHA-256 `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`. The checkout remained clean. A fresh Missing Raft fixture smoke exited `0` with `Fixture validated: ensemble.e0.missing-raft@0.1.0`.

The reserved evidence root, run claim and root claim are all absent. No matching live E0-D Slot 1 Harness process exists. Ambient `GEMINI_API_KEY` is absent.

Protected credential artifacts remain present outside Git at the established CurrentUser location: `gemini-test-key.dpapi` and `gemini-test-key.ready`. This activation package did not decrypt, print, hash, persist, export or inject plaintext credential material. The earlier authenticated readiness record established the protected key as the intended `AQ.` Auth-key associated with `Gemini API Key - testing` / `Gemini Project - Kymaean` / project ID `gen-lang-client-0490221700` / Free tier.

## Execution-sensitive launch barrier

This package intentionally does **not** satisfy the slot-fresh authenticated capacity gate. The package was prepared on UTC `2026-09-15`, before the frozen P01 window opens. A stale observation plus arithmetic is prohibited by the frozen method.

Exactly one Slot 1 launch becomes available only after **all** of the following are true:

1. this exact activation package is integrated to Project `main` through required exact-head hosted gates and the resulting push-triggered exact-main Validation is successful;
2. current UTC time is inside `2026-09-16T14:30:00Z..23:30:00Z` and that window has not been shifted, extended, reopened or replaced;
3. immediately before the irreversible claim boundary, authenticated Google AI Studio inspection freshly confirms the intended `Gemini Project - Kymaean` / `gen-lang-client-0490221700`, `Gemini API Key - testing`, Free tier, exact `Gemini 3.5 Flash Lite` row, and sufficient current RPM / TPM / RPD capacity for the frozen run envelope;
4. protected credential artifacts remain present and fresh in-memory readiness verification succeeds without disclosure, persistence or argument leakage;
5. the renewed source snapshot remains valid and no material contrary lifecycle, pricing, data-use, reasoning-control, model, account, project, key, quota or route signal exists;
6. executable/tag, fixture/profile and Slot 1 identities still exactly match this package;
7. evidence root, run claim and root claim remain absent and unconsumed, and no matching run process is active.

Failure of any item is a hard stop before namespace claim, evidence-root creation, credential injection or provider traffic. If the P01 window closes before all gates pass, Slot 1 remains unexecuted and requires Director disposition; the window may not be moved to rescue it.

## Exact single-use execution shape

Only after every barrier above passes, the intended Harness invocation is:

`Ensemble.E0.Harness.exe e0d-run E0D-FULL-REFERENCE-01 <canonical-missing-raft-fixture.json> E0D-Q01-P01-FULL-20260914-01 <exact-evidence-root> 0dacdbf6bd5453c192568cd4718207e145dfcf40`

The credential may be injected only into the intended child-process environment through the existing protected CurrentUser mechanism. It must never appear in command arguments, console output, logs, repository evidence, chat or generated artifacts.

The run's own `countTokens` and generation operations are authorized only as part of that one execution. No separate provider compatibility, availability, key-health or quota probe is authorized. The first namespace claim/evidence-root creation consumes Slot 1 regardless of terminal outcome.

## Terminal and successor law

Any technical failure, provider failure, invalid output, cancellation, refusal, timeout, budget terminal or other noncontributing result consumes this Slot 1 identity and earns no retry or replacement. Terminal evidence must be preserved immutably, sealed, hard-gate evaluated and reconciled before any successor action.

This activation creates no authority for `P01-ABLATION`, P02, P03, E0-E, Administrator MA gates, Reviewer, Hooks, Automations or autonomous chaining. P01 Slot 2 remains frozen/unclaimed and requires a separate exact activation package, a new post-Slot-1 slot-fresh authenticated capacity observation, and separate Director live-execution authority after Slot 1 reaches terminal state.

Provider traffic remains **ZERO** while this package is prepared and integrated. No namespace claim, evidence-root creation, execution credential use, `countTokens`, generation, inference, scoring or spend occurs before the execution-sensitive barrier is satisfied.