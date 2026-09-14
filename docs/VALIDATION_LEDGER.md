# Ensemble Validation Ledger

Status: validation fact register only. This file cannot advance phase/checkpoint authority; `CURRENT_STATE.md` alone does that.

## Current promoted machine-tested checkpoint

| Field | Fact |
|---|---|
| Exact checkout | `0dacdbf6bd5453c192568cd4718207e145dfcf40` |
| Annotated tag | `validation/e0d-snapshot-refresh-native-arm64` |
| Tag object | `f2df6e8be66e7f1a7106d82c89ae13d284442090`; verified to peel to the exact checkout above |
| Architecture | `docs/blueprint/E0D_ABLATION_CONTROLS_METHOD_AND_ISOLATION_CONTRACT_01.md` |
| Implementation | Frozen E0-D Full, relationship-omission, omniscient-context, and deterministic round-robin variants unchanged; this checkpoint changes only the dated Gemini snapshot freshness policy through UTC 2026-09-18. |
| Native evidence | `docs/evidence/E0D_SOURCE_SNAPSHOT_REFRESH_NATIVE_ARM64_VALIDATION_2026_09_14.md` |
| Supporting implementation audit | `docs/evidence/E0D_ABLATION_CONTROLS_IMPLEMENTATION_AUDIT_2026_09_14.md` |
| Director authorization | `docs/evidence/E0D_SOURCE_SNAPSHOT_REFRESH_AND_NATIVE_REVALIDATION_DIRECTOR_AUTHORIZATION_2026_09_14.md` |
| Host | Director Windows ARM64 (`win-arm64`), repository-selected SDK `9.0.317` |
| Core tests | 626/626 PASS |
| Harness tests | 154/154 PASS |
| Fresh Harness build | `net9.0/win-arm64` Release PASS; zero warnings/errors |
| Fixture smokes | Missing Raft PASS with canonical Fixture validation; generic PASS |
| Credentialless provider-edge gate | fixed `e0d-run` missing-key refusal PASS; exit 1 before network and no evidence root |
| Repository checks | repository-law PASS; census PASS 300 / 179 / 30 / 91 / 0 unexplained; oracle PASS 215 documented / 17 asserted / 198 document-only hashes; commit-diff/clean/frozen-identity gates PASS |
| Executable SHA-256 | `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625` |
| Snapshot boundary | verified 2026-09-14; valid inclusively through UTC 2026-09-18; fail closed from UTC 2026-09-19 |
| Provider scope | network / `countTokens` / generation / inference / scoring / spend NOT PERFORMED during validation |

Native runtime authority applies only to exact checkout `0dacdbf6bd5453c192568cd4718207e145dfcf40`. Later documentation/integration commits do not inherit machine-test authority. Cloud ARM64-target builds remain compiler authority only; non-native tests remain semantic regressions, not native Windows ARM64 runtime authority.

### Supporting provider-evidence continuity

The promoted checkpoint does not erase the still-current evidence chain that established the provider/runtime boundary. These records remain supporting evidence only and do not override the promoted checkpoint:

- `docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md`
- `docs/evidence/E0A_GEMINI35_STRUCTURED_OUTPUT_COMPATIBILITY_DIAGNOSTIC_2026_09_10.md`
- `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN05_PREEXECUTION_ACTIVATION_2026_09_10.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN05_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_PREAUTHORIZATION_PUBLIC_FACT_AUDIT_2026_09_09.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_DIRECTOR_AUTHORIZATION_2026_09_09.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_HISTORICAL_ROOT_FORENSIC_RESOLUTION_2026_09_09.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_LOCAL_PREFLIGHT_ATTEMPT_01_2026_09_09.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_LOCAL_PREFLIGHT_ATTEMPT_02_2026_09_09.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_LOCAL_PREFLIGHT_ATTEMPT_03_2026_09_09.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_LOCAL_PREFLIGHT_ATTEMPT_04_2026_09_09.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_TERMINAL_CONTINUITY_INTAKE_2026_09_09.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN02_DIRECTOR_AUTHORIZATION_2026_09_10.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN02_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`
- `docs/evidence/E0A_Q_E0A_03_REFERENCE_EVIDENCE_PLANNING_RECURSIVE_AUDIT_2026_09_09.md`
## Current validation ancestry

- prior E0-D ablation-controls native validation: `docs/evidence/E0D_ABLATION_CONTROLS_NATIVE_ARM64_VALIDATION_2026_09_14.md` at `5ce587b6c9ec83b91f3b5d63a2545cce9b1fe891`, tag `validation/e0d-ablation-controls-native-arm64`;
The promoted E0-D ablation-controls checkpoint depends on and supersedes, but does not erase, the accepted E0-B/E0-A executable validation ancestry below:

- E0-B mixed-cast native validation: `docs/evidence/E0B_MIXED_CAST_NATIVE_ARM64_VALIDATION_2026_09_12.md` at `d1073fe2c76e2e05f2daac47465f86b48b456a9a`;
- E0-B mixed-cast implementation audit: `docs/evidence/E0B_MIXED_CAST_IMPLEMENTATION_AUDIT_2026_09_12.md`;
- Gemini technical-failure diagnostic classification native validation: `docs/evidence/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_NATIVE_ARM64_VALIDATION_2026_09_10.md` at `bb869fb1c505603612bc718f739b3f1b358e5539`;
- Gemini technical-failure diagnostic classification implementation audit: `docs/evidence/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_IMPLEMENTATION_AUDIT_2026_09_10.md`;
- Role control-identity alignment native validation: `docs/evidence/E0A_ROLE_CONTROL_IDENTITY_ALIGNMENT_NATIVE_ARM64_VALIDATION_2026_09_10.md` at `29b62a2e778d93c6727b555f58f8d22aa18665a1`;
- Role control-identity alignment implementation audit: `docs/evidence/E0A_ROLE_CONTROL_IDENTITY_ALIGNMENT_IMPLEMENTATION_AUDIT_2026_09_10.md`;

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

- `validation/e0b-mixed-cast-implementation-native-arm64` -> `d1073fe2c76e2e05f2daac47465f86b48b456a9a`; evidence: `docs/evidence/E0B_MIXED_CAST_NATIVE_ARM64_VALIDATION_2026_09_12.md`
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
- `validation/e0a-gemini-technical-failure-diagnostic-classification-native-arm64` -> `bb869fb1c505603612bc718f739b3f1b358e5539`

## Recording rule

Add a validation fact here only after corresponding evidence exists. Never convert static review into compiler authority, compiler authority into native runtime authority, or native runtime authority into provider/network, hardware/NPU, package, WACK, or Store authority.
