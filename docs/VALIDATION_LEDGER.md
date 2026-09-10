# Ensemble Validation Ledger

Status: validation fact register only. This file cannot advance phase/checkpoint authority; `CURRENT_STATE.md` alone does that.

## Current promoted machine-tested checkpoint

| Field | Fact |
|---|---|
| Exact checkout | `7868e5cb12a27260e288d95c248d6f846cf37701` |
| Annotated tag | `validation/e0a-gemini-generation-error-diagnostic-native-arm64` |
| Tag object | `595fef66a79ac2939f439748fed096094b445cf1`; annotated tag locally verified to dereference to the exact checkout above |
| Architecture | `docs/blueprint/E0A_GEMINI_GENERATION_ERROR_DIAGNOSTIC_CORRECTION_AMENDMENT.md` |
| Correction | Gemini generation HTTP failures now reuse the bounded Google RPC status/validated-field diagnostic parser already approved for `countTokens`; generation request bytes and all Core/Fixture/provider-policy semantics remain unchanged |
| Native evidence | `docs/evidence/E0A_GEMINI_GENERATION_ERROR_DIAGNOSTIC_NATIVE_ARM64_VALIDATION_2026_09_10.md` |
| Supporting implementation audit | `docs/evidence/E0A_GEMINI_GENERATION_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_10.md` |
| Triggering terminal analysis | `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md` |
| Director-host contract | `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md` |
| Host | Director Windows ARM64 (`win-arm64`), SDK `9.0.317` |
| Core tests | 622/622 PASS |
| Harness tests | 134/134 PASS |
| Fresh Harness build | `net9.0/win-arm64` Debug PASS; zero warnings/errors |
| Fixture smokes | missing-Raft PASS; generic PASS |
| Current credentialless live-profile gate | 3.5 Lite, 3.1 Lite, 2.5 Lite expected missing-key refusal PASS; no evidence roots |
| Retired 2.5 Flash live selection | expected pre-credential rejection PASS; no evidence root |
| Repository checks | repository-law PASS; document census PASS; oracle guard PASS |
| Provider scope | network / `countTokens` / generation / inference / spend NOT PERFORMED during validation; provider authorization NONE |

Native runtime authority applies only to exact checkout `7868e5cb12a27260e288d95c248d6f846cf37701`. Documentation-only commits after it do not inherit machine-test authority. Cloud ARM64-target builds remain compiler authority only; Linux x64 Core tests remain required semantic regressions, not native Windows ARM64 runtime authority.

## Current validation ancestry

The promoted generation-diagnostic checkpoint depends on and supersedes, but does not erase, the accepted validation ancestry below:

- Gemini `countTokens` input-projection native validation: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_2026_09_09.md` at `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`;
- Gemini `countTokens` input-projection implementation audit: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_IMPLEMENTATION_AUDIT_2026_09_09.md`;
- bounded provider-error diagnostic native validation: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_NATIVE_ARM64_VALIDATION_2026_09_08.md`;
- bounded provider-error diagnostic implementation audit: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_08.md`;
- prior countTokens correction native validation: `docs/evidence/E0A_GEMINI_COUNTTOKENS_CORRECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`;
- free-tier RPD/model-selection implementation audit: `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_IMPLEMENTATION_AUDIT.md`;
- free-tier RPD/model-selection native validation: `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`.

These documents remain current evidence because the promoted correction was built on that accepted lineage. They do not override the promoted checkpoint above.

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
- `validation/e0a-gemini-counttokens-correction-native-arm64` -> `689655eed677b789ab3ee395f1c65b4f2cb72cc8`
- `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64` -> `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`
- `validation/e0a-gemini-counttokens-input-projection-native-arm64` -> `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`

## Recording rule

Add a validation fact here only after corresponding evidence exists. Never convert static review into compiler authority, compiler authority into native runtime authority, or native runtime authority into provider/network, hardware/NPU, package, WACK, or Store authority.
