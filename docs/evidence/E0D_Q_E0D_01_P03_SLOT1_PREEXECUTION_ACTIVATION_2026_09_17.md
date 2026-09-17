# E0-D Q-E0D-01 — P03 Slot 1 Preexecution Activation

Date: 2026-09-17

Status: **ACTIVE ACTIVATION PACKAGE — EXACT P03 SLOT 1 ONLY — PREWINDOW HOLD — NAMESPACE UNCONSUMED — PROVIDER TRAFFIC ZERO — LIVE LAUNCH GATED BY WINDOW OPEN + EXACT-MAIN VALIDATION + FRESH AUTHENTICATED CAPACITY + FROZEN INTERLOCKS**

## Authority

Director live-execution authorization: `docs/evidence/E0D_P03_SLOT1_LIVE_EXECUTION_DIRECTOR_AUTHORIZATION_2026_09_17.md`.

P02 terminal predecessor: `docs/evidence/E0D_Q_E0D_01_P02_SLOT2_TERMINAL_EVIDENCE_ANALYSIS_2026_09_17.md`.

Successor-executable admissibility: `docs/evidence/E0D_SUCCESSOR_EXECUTABLE_P02_P03_ADMISSIBILITY_DIRECTOR_DISPOSITION_2026_09_16.md`.

Successor native validation: `docs/evidence/E0D_CONTEXT_ABLATION_TERMINAL_FINALIZATION_REPAIR_NATIVE_ARM64_VALIDATION_2026_09_16.md`.

Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`.

This package creates no claim, evidence root, credential injection, Gemini API call, inference, scoring or spend. It packages the exact interlocks for one P03 Slot-1 launch only after integration, the frozen window opens, and fresh launch-boundary gates pass.

## Exact P03 Slot 1 identity

- pair: `E0D-P03-ROUND-ROBIN`;
- ordinal / slot: `5` / `P03-FULL`;
- variant: `E0D-FULL-REFERENCE-01`;
- RunId: `E0D-Q01-P03-FULL-20260914-05`;
- evidence root: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0D-Q01-P03-FULL-20260914-05`;
- run claim: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\.ensemble-e0a-claims\run-5ae4ccb8203695c4b9d4f4f974db4a12a080d126ce5a5efc34659705e0ee569f.claim`;
- root claim: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\.ensemble-e0a-claims\root-a2ea5583a18efd3dec6e11d4e264b6fe92291f9380125347e89d594e84c09c11.claim`;
- frozen pair window: `2026-09-18T14:30:00Z..2026-09-18T23:30:00Z`.

Frozen runtime/provider identity:

- admitted native checkout: `8770a6361233e8a877a966c45eb6f62c5b3ca182`;
- validation tag: `validation/e0d-context-ablation-terminal-fix-native-arm64`;
- executable SHA-256: `651aedf9e05800ea46ecc8a7c6565f9f91eeaf07794747dcb0a3c7697b946fac`;
- fixture: `ensemble.e0.missing-raft@0.1.0`, canonical semantic/provenance SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`;
- provider/model/profile: Google Gemini API / `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`;
- association: `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` / Free tier;
- eligible protected credential: `gemini-ensemble-testing-key` CurrentUser protection only; superseded old protected credential ineligible.

## Integration / prewindow barrier

Before any launch-boundary authenticated inspection or irreversible operation:

1. this package and the P03 Slot-1 Director authorization must be integrated to current Project `main`;
2. branch-push/PR hosted gates must pass on the exact candidate;
3. push-triggered exact-main Validation must pass on the resulting merge;
4. current UTC must then reach the frozen window start `2026-09-18T14:30:00Z`.

Until all four are true, stop before execution-sensitive AI Studio capacity inspection, credential injection, claim creation, evidence-root creation, `countTokens`, generation or any other Gemini provider traffic.

## Immediate preclaim gates

After the integration barrier passes, and only while the frozen P03 window is open, perform one fresh launch-boundary inspection. All of the following must pass simultaneously:

- authenticated AI Studio confirms project `Ensemble Testing`, project ID `gen-lang-client-0793779417`, credential label `Gemini API Key`, Free tier, exact `Gemini 3.5 Flash Lite`, and sufficient current RPM/TPM/RPD capacity;
- exact admitted checkout/tag/executable/fixture/profile identities remain unchanged and clean;
- exact P03 Slot-1 evidence root, run claim and root claim are absent;
- no matching Harness process is active before launch;
- ambient `GEMINI_API_KEY` is absent;
- the eligible protected credential decrypts only in memory and is selected for the intended Harness child;
- no contrary provider/account/model/lifecycle/pricing/data-use/reasoning-control/quota/route signal exists;
- current UTC remains inside `2026-09-18T14:30:00Z..2026-09-18T23:30:00Z`.

A stale observation plus arithmetic is not sufficient. No standalone compatibility/key-health/quota probe is authorized.

## Exact execution shape

Only after every integration and immediate-preclaim gate passes:

`Ensemble.E0.Harness.exe e0d-run E0D-FULL-REFERENCE-01 <canonical-missing-raft-fixture.json> E0D-Q01-P03-FULL-20260914-05 <exact-evidence-root> 8770a6361233e8a877a966c45eb6f62c5b3ca182`

The child process must run from the exact retained validator checkout required by the Harness live-run authority guard. Plaintext credential material must never be printed, hashed, persisted, exported, placed in command arguments, copied to chat or written to repository/evidence logs.

## Consumption / terminal law

The first actual deterministic claim/evidence-root creation consumes P03 Slot 1 regardless of terminal outcome. There is no retry, replay, replacement, fallback, paid/Priority route, model/profile/fixture substitution, retune or automatic window extension. Any terminal result must be preserved, runtime-sealed where supported, independently hard-gate reviewed, evaluated/sealed where prerequisites exist, analyzed and reconciled before any P03 Slot-2 decision.

P03 Slot 2 remains separately unauthorized and requires its own fresh post-predecessor capacity evidence, Director live-execution authorization, frozen interlocks and the same pair window after Slot 1 reaches terminal state. This package authorizes no P03 Slot-2 or E0-E execution and creates no Administrator runtime authority.