# Ensemble Validation Ledger

Status: validation fact register only. This file cannot advance phase/checkpoint authority; `CURRENT_STATE.md` alone does that.

## Current promoted machine-tested checkpoint

| Field | Fact |
|---|---|
| Exact checkout | `3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99` |
| Annotated tag | `validation/e0a-gemini-rpd-model-selection-native-arm64` |
| Tag object | `ca036b8b44775f77535d04b82ba4ebbbb3d295f0`; dereferences to exact checkout above |
| Amendment | `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` |
| Implementation audit | `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_IMPLEMENTATION_AUDIT.md` |
| Native evidence | `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md` |
| Director-host contract | `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md` |
| Host | Director Windows ARM64 (`win-arm64`) |
| Core tests | 622/622 PASS |
| Harness tests | 122/122 PASS |
| Fresh Harness build | `win-arm64` PASS |
| Fixture smokes | missing-Raft PASS; generic PASS |
| Current credentialless live-profile gate | 3.5 Lite, 3.1 Lite, 2.5 Lite expected missing-key refusal PASS; no evidence roots |
| Retired 2.5 Flash live selection | expected pre-credential rejection PASS; no evidence root |
| Failed native attempt 01 | `31436ee52238c2de96e9bcbe9d61ece173f81dd3`: Core 622/622; Harness 121/122; stopped; no partial promotion |
| Provider credentials/network/`countTokens`/inference/spend | NOT PERFORMED |

Native runtime authority applies only to exact checkout `3d6d8a7f...`. Documentation-only commits after it do not inherit machine-test authority. Cloud ARM64-target builds remain compiler authority only; Linux x64 Core tests remain required semantic regressions, not native Windows ARM64 runtime authority.

## Durable historical native validation tags

These tags preserve exact historical machine-tested checkouts. Historical validity does not make a checkout current.

- `validation/e0a-phase-b-reference-run-native-arm64` -> `3749210393282f6aa2ac4ceb0176b6adb5df189e`
- `validation/e0a-phase-b-live-host-native-arm64` -> `1cfdb3aa22abce62a5bd48e80706407670d1c6a9`
- `validation/e0a-phase-b-post-audit-hardening-native-arm64` -> `5c70f619d6e951d89bb527a5945b014998573dab`
- `validation/e0a-phase-b-gemini-native-arm64` -> `9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`
- `validation/e0a-phase-b-gemini-on-hardened-native-arm64` -> `5f286e8cfa896d38d85d4611f69a224fae5b55fd`
- `validation/e0a-pre-restructure-closure-native-arm64` -> `cc395a25162a0a682796bffb44060c799df0db32`
- `validation/e0a-gemini-comparison-native-arm64` -> `c9f706b42350c8b6cfc462e09cf71db6bc3a2355`; evidence: `docs/evidence/E0A_GEMINI_COMPARISON_NATIVE_ARM64_VALIDATION_2026_09_07.md`
- `validation/e0a-gemini-rpd-model-selection-native-arm64` -> `3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99`

Historical supporting evidence may be archived as repository-surface cleanup proceeds. The annotated tag, exact checkout, and Git history remain durable locators.

## Recording rule

Add a validation fact here only after corresponding evidence exists. Never convert static review into compiler authority, compiler authority into native runtime authority, or native runtime authority into provider/network, hardware/NPU, package, WACK, or Store authority.
