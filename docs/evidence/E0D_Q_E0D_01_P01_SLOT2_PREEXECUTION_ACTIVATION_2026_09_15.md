# E0-D Q-E0D-01 — P01 Slot 2 Preexecution Activation

Date: 2026-09-15

Status: **ACTIVE ACTIVATION PACKAGE — EXACT P01 SLOT 2 ONLY — NAMESPACE UNCONSUMED — PROVIDER TRAFFIC ZERO — LIVE LAUNCH GATED BY EXACT-MAIN VALIDATION + FRESH AUTHENTICATED CAPACITY + FROZEN INTERLOCKS**

## Authority

Director live-execution authorization: `docs/evidence/E0D_P01_SLOT2_LIVE_EXECUTION_DIRECTOR_AUTHORIZATION_2026_09_15.md`.

Terminal predecessor: `docs/evidence/E0D_Q_E0D_01_P01_SLOT1_TERMINAL_EVIDENCE_ANALYSIS_2026_09_15.md`.

Timing authority: `docs/evidence/E0D_P01_PAIR_WINDOW_DIRECTOR_AMENDMENT_2026_09_15.md`.

Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`.

This package creates no claim, evidence root, credential injection, Gemini API call, inference, scoring or spend. It packages the exact interlocks for one Slot-2 launch only after integration and fresh launch-boundary gates pass.

## Exact Slot 2 identity

- pair: `E0D-P01-RELATIONSHIP-OMISSION`;
- ordinal / slot: `2` / `P01-ABLATION`;
- variant: `E0D-RELATIONSHIPS-OMITTED-01`;
- RunId: `E0D-Q01-P01-REL-20260914-02`;
- evidence root: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0D-Q01-P01-REL-20260914-02`;
- run claim: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\.ensemble-e0a-claims\run-d410e9ccd14404dc8e1134ddf9e2ad9213e9c1eb4b2473c92dae40ea95da7fad.claim`;
- root claim: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\.ensemble-e0a-claims\root-540ffa3347fee74bcff9d781431da64b20ed854f15cd37c04b60312b9ede5ca5.claim`;
- active pair window: `2026-09-15T21:00:00Z..2026-09-16T06:00:00Z`.

Frozen runtime/provider identity:

- native checkout: `0dacdbf6bd5453c192568cd4718207e145dfcf40`;
- validation tag: `validation/e0d-snapshot-refresh-native-arm64`;
- executable SHA-256: `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`;
- fixture: `ensemble.e0.missing-raft@0.1.0`, canonical semantic/provenance SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`;
- provider/model/profile: Google Gemini API / `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`;
- association: `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` / Free tier;
- eligible local protected credential: `gemini-ensemble-testing-key` CurrentUser protection only; superseded old protected credential ineligible;
- source snapshot validity: inclusive through UTC `2026-09-18`.

## Terminal predecessor effect

Slot 1 is permanently consumed/noncontributing at `InvalidOutput` 2/12 with sealed hard-gate PASS / 0 findings. P01 is already experientially ineligible because Full did not reach 12/12. Slot 2 cannot restore pair scoring and must not reinterpret, replay, repair or replace Slot 1.

## Integration barrier

Before any launch-boundary authenticated inspection or irreversible operation:

1. this package and the Slot-2 Director authorization must be integrated to current Project `main`;
2. branch-push/PR hosted gates must pass on the exact candidate;
3. push-triggered exact-main Validation must pass on the resulting merge.

Until all three are true, stop before Slot-2 credential injection, claim creation, evidence-root creation, `countTokens`, generation or any other Gemini provider traffic.

This activation candidate is audited against Project main@399024b54b8c2c66525d081e816740f055f26f6a; if main moves before integration, rebase/reconcile and rerun the exact-head gates rather than inheriting authority from this snapshot.

This activation candidate is audited against Project main@399024b54b8c2c66525d081e816740f055f26f6a; if main moves before integration, rebase/reconcile and rerun the exact-head gates rather than inheriting authority from this snapshot.

## Immediate preclaim gates

After the integration barrier passes, and only while the amended P01 pair window is open, perform a fresh launch-boundary inspection. All of the following must pass simultaneously:

- authenticated AI Studio confirms project `Ensemble Testing`, project ID `gen-lang-client-0793779417`, credential label `Gemini API Key`, Free tier, exact `Gemini 3.5 Flash Lite`, and sufficient current RPM/TPM/RPD capacity;
- exact checkout/tag/executable/fixture/profile identities remain unchanged and clean;
- exact Slot-2 evidence root, run claim and root claim are absent;
- no matching Harness process is active before launch;
- ambient `GEMINI_API_KEY` is absent;
- the eligible protected credential decrypts only in memory and is selected for the intended Harness child;
- no contrary provider/account/model/lifecycle/pricing/data-use/reasoning-control/quota/route signal exists;
- current UTC remains inside `2026-09-15T21:00:00Z..2026-09-16T06:00:00Z`.

A stale observation plus arithmetic is not sufficient. No standalone compatibility/key-health/quota probe is authorized.

## Exact execution shape

Only after every integration and immediate-preclaim gate passes:

`Ensemble.E0.Harness.exe e0d-run E0D-RELATIONSHIPS-OMITTED-01 <canonical-missing-raft-fixture.json> E0D-Q01-P01-REL-20260914-02 <exact-evidence-root> 0dacdbf6bd5453c192568cd4718207e145dfcf40`

The child process must run from the exact validated checkout required by the Harness live-run authority guard. Plaintext credential material must never be printed, hashed, persisted, exported, placed in command arguments, copied to chat or written to repository/evidence logs.

## Consumption / terminal law

The first actual deterministic claim/evidence-root creation consumes Slot 2 regardless of terminal outcome. There is no retry, replay, replacement, fallback, paid/Priority route, model/profile/fixture substitution, retune or automatic window extension. Any terminal result must be preserved, runtime-sealed, independently hard-gate reviewed, evaluated/sealed, analyzed and reconciled before any later-slot decision.

This package authorizes no P02/P03/E0-E execution and creates no Administrator runtime authority.