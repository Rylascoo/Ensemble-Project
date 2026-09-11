# Ensemble Validation Ledger

Status: validation fact register only. This file cannot advance phase/checkpoint authority; `CURRENT_STATE.md` alone does that.

## Current promoted machine-tested checkpoint

| Field | Fact |
|---|---|
| Exact checkout | `29b62a2e778d93c6727b555f58f8d22aa18665a1` |
| Annotated tag | `validation/e0a-role-control-identity-alignment-native-arm64` |
| Tag object | `f6080e37df44737b3053fa8237b95bdc2cc0508c`; verified to peel to the exact checkout above |
| Architecture | `docs/blueprint/E0A_ROLE_CONTROL_IDENTITY_ALIGNMENT_AMENDMENT.md` |
| Correction | Bounded Performer/Interpreter context now exposes canonical `rosterCharacterIds`; typed Character-ID output must reuse exact case-sensitive roster IDs. Deterministic parsers, Core authority, Fixture, schemas, provider controls, and accepted-turn law remain unchanged. |
| Native evidence | `docs/evidence/E0A_ROLE_CONTROL_IDENTITY_ALIGNMENT_NATIVE_ARM64_VALIDATION_2026_09_10.md` |
| Supporting implementation audit | `docs/evidence/E0A_ROLE_CONTROL_IDENTITY_ALIGNMENT_IMPLEMENTATION_AUDIT_2026_09_10.md` |
| Triggering provider evidence | `docs/evidence/E0A_Q_E0A_03_G35L_RUN04_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`; `docs/evidence/E0A_Q_E0A_03_G35L_RUN03_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`; `docs/evidence/E0A_Q_E0A_03_G35L_RUN02_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`; `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`; `docs/evidence/E0A_GEMINI35_STRUCTURED_OUTPUT_COMPATIBILITY_DIAGNOSTIC_2026_09_10.md` |
| Director-host contract | `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md` |
| Host | Director Windows ARM64 (`win-arm64`), repository-selected SDK `9.0.317` |
| Core tests | 622/622 PASS |
| Harness tests | 137/137 PASS |
| Fresh Harness build | `net9.0/win-arm64` Debug PASS; zero warnings/errors |
| Fixture smokes | Missing Raft PASS with canonical Fixture validation; generic PASS |
| Credentialless provider-edge gate | 3.5 Lite, 3.1 Lite, 2.5 Lite exact missing-key refusal PASS; retired 2.5 Flash exact pre-credential rejection PASS; no evidence roots |
| Repository checks | repository-law PASS; document census PASS with 0 unexplained current; oracle guard PASS at exact checkout, 104 documented / 17 asserted / 87 document-only hashes |
| Provider scope | network / `countTokens` / generation / inference / spend NOT PERFORMED during validation |

Native runtime authority applies only to exact checkout `29b62a2e778d93c6727b555f58f8d22aa18665a1`. Documentation-only and integration commits do not inherit machine-test authority. Cloud ARM64-target builds remain compiler authority only; Linux x64 Core tests remain semantic regressions, not native Windows ARM64 runtime authority.
## Current validation ancestry

The promoted role control-identity alignment checkpoint depends on and supersedes, but does not erase, the accepted validation ancestry below:

- State Interpreter semantic-output alignment native validation: `docs/evidence/E0A_STATE_INTERPRETER_SEMANTIC_OUTPUT_ALIGNMENT_NATIVE_ARM64_VALIDATION_2026_09_10.md` at `e052ef4cdaf8ac7291971f8c42d121e1ee6eecf0`;
- State Interpreter semantic-output alignment implementation audit: `docs/evidence/E0A_STATE_INTERPRETER_SEMANTIC_OUTPUT_ALIGNMENT_IMPLEMENTATION_AUDIT_2026_09_10.md`;
- Gemini structured-output compatibility native validation: `docs/evidence/E0A_GEMINI_STRUCTURED_OUTPUT_COMPATIBILITY_NATIVE_ARM64_VALIDATION_2026_09_10.md` at `cef3fc15e31192a48aa3bddd99450b65a58bd8f1`;
- Gemini structured-output compatibility implementation audit: `docs/evidence/E0A_GEMINI_STRUCTURED_OUTPUT_COMPATIBILITY_IMPLEMENTATION_AUDIT_2026_09_10.md`;
- Gemini generation-error diagnostic native validation: `docs/evidence/E0A_GEMINI_GENERATION_ERROR_DIAGNOSTIC_NATIVE_ARM64_VALIDATION_2026_09_10.md` at `7868e5cb12a27260e288d95c248d6f846cf37701`;
- Gemini generation-error diagnostic implementation audit: `docs/evidence/E0A_GEMINI_GENERATION_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_10.md`;
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
- `validation/e0a-gemini-generation-error-diagnostic-native-arm64` -> `7868e5cb12a27260e288d95c248d6f846cf37701`
- `validation/e0a-gemini-structured-output-compatibility-native-arm64` -> `cef3fc15e31192a48aa3bddd99450b65a58bd8f1`

## Recording rule

Add a validation fact here only after corresponding evidence exists. Never convert static review into compiler authority, compiler authority into native runtime authority, or native runtime authority into provider/network, hardware/NPU, package, WACK, or Store authority.
