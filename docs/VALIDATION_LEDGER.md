# Ensemble Validation Ledger

Status: validation fact register only. This file cannot advance phase/checkpoint authority; `CURRENT_STATE.md` alone does that.

## Current promoted machine-tested checkpoint

| Field | Fact |
|---|---|
| Exact checkout | `c9f706b42350c8b6cfc462e09cf71db6bc3a2355` |
| Annotated tag | `validation/e0a-gemini-comparison-native-arm64` |
| Evidence | `docs/evidence/E0A_GEMINI_COMPARISON_NATIVE_ARM64_VALIDATION_2026_09_07.md` |
| Director-host contract | `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md` |
| Host | Director Windows ARM64 |
| Core tests | 622/622 PASS |
| Harness tests | 117/117 PASS |
| Fresh Harness build | `win-arm64` PASS |
| Fixture smokes | missing-Raft PASS; generic PASS |
| Credentialless Gemini gate | predecessor three-profile set: expected refusal PASS; no evidence roots |
| Provider credentials/network/`countTokens`/inference/spend | NOT PERFORMED |

## Current amended compiler/static checkpoint pending native validation

| Field | Fact |
|---|---|
| Source/test checkout | `b0286c3427eb3e2b9f0e8401098596db055f9c7e` |
| Amendment | `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` |
| Implementation audit | `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_IMPLEMENTATION_AUDIT.md` |
| Cloud Validation | run `34185024153` PASS |
| ARM64-target Core/Harness/test builds | PASS compiler authority only |
| x64 Core regression | PASS non-authoritative for native runtime |
| Repository/oracle/document gates | PASS |
| Core/Core-test/fixture delta | none |
| Native Windows ARM64 execution | PENDING |
| Provider credentials/network/`countTokens`/inference/spend | NOT PERFORMED |

Cloud ARM64-target builds are compiler authority only. Linux x64 Core tests are required semantic regressions, not native Windows ARM64 runtime authority. Documentation-only commits after a machine-tested checkout do not inherit native runtime authority.

## Durable historical native validation tags

These tags preserve exact historical machine-tested checkouts. Historical validity does not make a checkout current.

- `validation/e0a-phase-b-reference-run-native-arm64` -> `3749210393282f6aa2ac4ceb0176b6adb5df189e`
- `validation/e0a-phase-b-live-host-native-arm64` -> `1cfdb3aa22abce62a5bd48e80706407670d1c6a9`
- `validation/e0a-phase-b-post-audit-hardening-native-arm64` -> `5c70f619d6e951d89bb527a5945b014998573dab`
- `validation/e0a-phase-b-gemini-native-arm64` -> `9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`
- `validation/e0a-phase-b-gemini-on-hardened-native-arm64` -> `5f286e8cfa896d38d85d4611f69a224fae5b55fd`
- `validation/e0a-pre-restructure-closure-native-arm64` -> `cc395a25162a0a682796bffb44060c799df0db32`
- `validation/e0a-gemini-comparison-native-arm64` -> `c9f706b42350c8b6cfc462e09cf71db6bc3a2355`

Historical supporting evidence may be archived as repository-surface cleanup proceeds. The annotated tag, exact checkout, and Git history remain durable locators.

## Recording rule

Add a validation fact here only after the corresponding evidence exists. Never convert static review into compiler authority, compiler authority into native runtime authority, or native runtime authority into provider/network, hardware/NPU, package, WACK, or Store authority.
