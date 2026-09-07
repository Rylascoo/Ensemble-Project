# Ensemble Validation Ledger

Status: validation fact register only. This file cannot advance phase/checkpoint authority; `CURRENT_STATE.md` alone does that.

## Current promoted machine-tested checkpoint

| Field | Fact |
|---|---|
| Exact checkout | `cc395a25162a0a682796bffb44060c799df0db32` |
| Annotated tag | `validation/e0a-pre-restructure-closure-native-arm64` |
| Evidence | `docs/evidence/E0A_PRE_RESTRUCTURE_CLOSURE_NATIVE_ARM64_VALIDATION.md` |
| Director-host contract | `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md` |
| Host | Director Windows ARM64 |
| Core tests | 622/622 PASS |
| Harness tests | 103/103 PASS |
| Fresh Harness build | `win-arm64` PASS |
| Fixture smokes | missing-Raft PASS; generic PASS |
| Credentialless Gemini gate | expected refusal PASS |
| Provider credentials/network/inference/spend | NOT PERFORMED |
| Cloud validation at exact checkpoint | run `34164258601` PASS |
| Evidence-only continuity validation | run `34166676374` PASS |

Cloud ARM64-target builds are compiler authority only. Linux x64 Core tests are required semantic regressions, not native Windows ARM64 runtime authority.

## Durable historical native validation tags

These tags preserve exact historical machine-tested checkouts. Historical validity does not make a checkout current.

- `validation/e0a-phase-b-reference-run-native-arm64` -> `3749210393282f6aa2ac4ceb0176b6adb5df189e`
- `validation/e0a-phase-b-live-host-native-arm64` -> `1cfdb3aa22abce62a5bd48e80706407670d1c6a9`
- `validation/e0a-phase-b-post-audit-hardening-native-arm64` -> `5c70f619d6e951d89bb527a5945b014998573dab`
- `validation/e0a-phase-b-gemini-native-arm64` -> `9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`
- `validation/e0a-phase-b-gemini-on-hardened-native-arm64` -> `5f286e8cfa896d38d85d4611f69a224fae5b55fd`
- `validation/e0a-pre-restructure-closure-native-arm64` -> `cc395a25162a0a682796bffb44060c799df0db32`

Historical supporting evidence may be archived as repository-surface cleanup proceeds. The annotated tag, exact checkout, and Git history remain durable locators.

## Recording rule

Add a validation fact here only after the corresponding evidence exists. Never convert static review into compiler authority, compiler authority into native runtime authority, or native runtime authority into provider/network, hardware/NPU, package, WACK, or Store authority.
