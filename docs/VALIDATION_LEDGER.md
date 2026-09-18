# Ensemble Validation Ledger

Status: validation fact register only. This file cannot advance phase/checkpoint authority; `CURRENT_STATE.md` alone does that.

## Current promoted machine-tested checkpoint

| Field | Fact |
|---|---|
| Exact checkout | `8770a6361233e8a877a966c45eb6f62c5b3ca182` |
| Annotated tag | `validation/e0d-context-ablation-terminal-fix-native-arm64` |
| Tag object | `0b3582aef6acf7e5ad2cf26585e0ccdc6a9a4dee`; verified to peel to the exact checkout above |
| Architecture | `docs/blueprint/E0D_ABLATION_CONTROLS_METHOD_AND_ISOLATION_CONTRACT_01.md` |
| Implementation | Narrow repair for context-ablation Performer technical/cancelled terminal routing; frozen E0-D variants/method/fixture/provider/profile/scoring/claim/retry contracts otherwise unchanged. |
| Native evidence | `docs/evidence/E0D_CONTEXT_ABLATION_TERMINAL_FINALIZATION_REPAIR_NATIVE_ARM64_VALIDATION_2026_09_16.md` |
| Defect evidence | `docs/evidence/E0D_Q_E0D_01_P01_SLOT2_UNSEALED_TECHNICAL_TERMINAL_ANALYSIS_2026_09_15.md` |
| Host | Director Windows ARM64 (`win-arm64`), repository-selected SDK `9.0.317` |
| Core tests | 628/628 PASS |
| Harness tests | 155/155 PASS, including Relationships-Omitted + Omniscient technical/cancelled runtime-sealing regressions |
| Fresh Harness build | `net9.0/win-arm64` Release PASS; zero warnings/errors |
| Fixture smokes | Missing Raft PASS; generic PASS |
| Credentialless provider-edge gate | exact `e0d-run` missing-key refusal PASS; exit 1 before network and no evidence root |
| Hosted gate | branch-push Validation #885 PASS on exact checkout |
| Repository checks | repository-law PASS; census 316 / 195 / 30 / 91 / 0 unexplained; oracle 266 / 17 / 249; exact delta four files; clean detached validator PASS |
| Executable SHA-256 | `651aedf9e05800ea46ecc8a7c6565f9f91eeaf07794747dcb0a3c7697b946fac` |
| Provider scope | network / `countTokens` / generation / inference / scoring / spend NOT PERFORMED during repair validation |

Native runtime authority for the repaired E0-D executable applies only to exact checkout `8770a6361233e8a877a966c45eb6f62c5b3ca182`. This validation does not authorize P02/P03 live execution. The prior `0dacdbf6...` checkpoint remains historical truth for consumed P01 runs but is superseded for future E0-D executable use. Later documentation/integration commits do not inherit machine-test authority.

## Q-PROD-01 provisional product native checkpoint

| Field | Fact |
|---|---|
| Exact source checkout | `34a3745e12c42247fe4e1663697bf55452f9d505` |
| Base | `dfc641e22f26a81ad1a40cad37626aa41923eb2d` |
| Integrated Project main | `1c6600e8f2d487515cb0029a88752a9d3f9428de` via PR #167; push-triggered Validation #926 PASS |
| Candidate hosted gates | `8dcb64e3bc12732ca5e8e3f7108174b0270bdb16`: Validation #925 PASS; E0-E preparation #111 PASS |
| Evidence | `docs/evidence/Q_PROD_01_FIRST_NATIVE_VERTICAL_SLICE_ARM64_VALIDATION_2026_09_17.md` |
| Host | SurfSeven, native Windows ARM64, repository .NET 9 baseline |
| Product tests | `Kymaean.Application.Tests` 6/6 PASS on `win-arm64` Release |
| Native build | `Kymaean.Windows` ARM64 Release/MSIX PASS, 0 warnings / 0 errors |
| Validation package | Arm64 MSIX SHA-256 `ab1b012e7dc27ad3e2f3960123bc1d18f80d092f0bdbceb87b6d43e24210b0ea`, 28,059,348 bytes |
| Runtime | package activation PASS; native machine `0xAA64`; responsive `Kymaean` window |
| Interaction | final-package keyboard traversal/focus-return PASS; Back restores `Open live stage` focus |
| Windowing | `WM_GETMINMAXINFO` minimum-track contract 720x520 PASS |
| Theme evidence | same-state Light/Dark captures exist; invariant dark Stage preserved |
| High Contrast | observed on immediately preceding UI-identical checkpoint; exact-final recapture still pending because remote automation blocked the OS toggle; no bypass attempted |
| Provider scope | no Gemini/provider traffic during Q-PROD-01 implementation or validation |
| Authority scope | provisional product validation only; not final architecture, Alpha/Beta/release, WACK, Store, or deferred-E0 authority |

This exact source checkpoint retains native machine-test authority. Q-PROD-01 first-slice integration is durable on Project `main@1c6600e8f2d487515cb0029a88752a9d3f9428de` after PR #167 and push-triggered exact-main Validation #926 PASS; the documentation/merge commit does not inherit or inflate native-runtime authority beyond the validated `34a3745...` source.

### Q-PROD-01 provisional persistence journal checkpoint

| Field | Fact |
|---|---|
| Exact source checkout | `fff8298a066531cdfbd2bcb7bdacdbc7d024bcca` |
| Base | `6d2a32e174e2f00d16f91d04d6ad82adab2326fa` |
| Integrated Project main | `2c165539fcfb4541d90e177ac09a9800fc0169e8` via PR #169; push-triggered Validation #932 PASS |
| Candidate hosted gates | `d16ef3e8b64a7ce03f6875207fd1e136c2177e36`: Validation #931 PASS; E0-E preparation #113 PASS |
| Validation tag | `validation/q-prod-01-persistence-journal-native-arm64` |
| Evidence | `docs/evidence/Q_PROD_01_PERSISTENCE_JOURNAL_NATIVE_ARM64_VALIDATION_2026_09_17.md` |
| Host | SurfSeven, native Windows ARM64, repository .NET 9 baseline |
| Persistence tests | `Kymaean.Infrastructure.Persistence.Tests` 9/9 PASS on `win-arm64` Release |
| Product regression | `Kymaean.Application.Tests` 6/6 PASS; `Kymaean.Windows` ARM64 Release build PASS, 0 warnings / 0 errors |
| Earned scope | append-only framed journal, chained/payload hashes, checksummed committed-head witness, corruption/tail-loss detection, explicit validated-suffix recovery |
| Provider scope | no Gemini/provider traffic |
| Authority scope | provisional persistence substrate only; not complete P1, final event schema, replay/snapshot/export policy, Alpha/Beta/release, WACK, Store, or deferred-E0 authority |

The native validation identity is the exact `fff8298...` source. Later evidence/continuity commits may describe or integrate it but do not inherit native machine-test authority.

### Q-PROD-01 provisional Product event / replay checkpoint

| Field | Fact |
|---|---|
| Exact source checkout | `789028a52b133dfc29c1bdb79ac5533b40713f78` |
| Base | `832bc347c8bc9decf903094038ca890f89075c95` |
| Integrated Project main | `dbd49bc4cf97fca4515d9df87a436301f355bb4b` via PR #172; push-triggered Validation #941 PASS |
| Candidate hosted gates | `3fe8197b0062d1efa52370f6fcc137147fd169af`: Validation #940 PASS; E0-E preparation #116 PASS |
| Validation tag | `validation/q-prod-01-production-event-replay-native-arm64` |
| Evidence | `docs/evidence/Q_PROD_01_PRODUCTION_EVENT_REPLAY_NATIVE_ARM64_VALIDATION_2026_09_17.md` |
| Host | SurfSeven, native Windows ARM64, repository .NET 9 baseline |
| Application tests | `Kymaean.Application.Tests` 10/10 PASS on `win-arm64` Release |
| Persistence tests | `Kymaean.Infrastructure.Persistence.Tests` 14/14 PASS on `win-arm64` Release |
| Product regression | `Kymaean.Windows` ARM64 Release build PASS, 0 warnings / 0 errors |
| Earned scope | model-neutral Application event-store port; versioned `ProductionCreatedEvent`; deterministic replay projection; fail-closed typed decoding |
| Authority scope | provisional P1 slice only; typed semantic recovery, broader causal ontology, snapshots/export, complete P1, final architecture/release and deferred-E0 authority remain unearned |

Native machine-test authority is exact source `789028a...`; later evidence/integration commits do not inherit it.

### Q-PROD-01 semantic-aware recovery checkpoint

| Field | Fact |
|---|---|
| Exact source checkout | `97720ffc937853b59110c184392b1f6f97f39420` |
| Base | `77d7ab7a36a27f5c4aa8d86aaf1df9878440f496` |
| Integrated Project main | `0b0c60f719cce406047894aecad1597fc5775faf` via PR #176; push-triggered Validation #953 PASS |
| Candidate hosted gates | `f78d884fcc7424d467f3aea119137a755b1f06c3`: Validation #952 PASS; E0-E preparation #120 PASS |
| Validation tag | `validation/q-prod-01-semantic-recovery-native-arm64` |
| Evidence | `docs/evidence/Q_PROD_01_SEMANTIC_RECOVERY_NATIVE_ARM64_VALIDATION_2026_09_17.md` |
| Host | SurfSeven, native Windows ARM64, repository .NET 9 baseline |
| Application tests | 10/10 PASS on `win-arm64` Release |
| Persistence tests | 20/20 PASS on `win-arm64` Release |
| Product regression | WinUI ARM64 Release PASS, 0 warnings / 0 errors |
| Earned scope | typed append/recovery decode + deterministic-replay validation before journal mutation/head promotion; model-neutral `Recover()` port |
| Authority scope | provisional P1 slice only; broader ontology, snapshots/export, full lifecycle reconstruction, final storage/concurrency, power-loss certification, complete P1 and release authority remain unearned |

Native semantic-recovery authority is exact source `97720ff...`; later evidence/integration commits do not inherit machine-test authority.

### Q-DESIGN-20 bounded native-corrections checkpoint

| Field | Fact |
|---|---|
| Exact source checkout | `248435944815a34e4caac761ed308ed3b07e325b` |
| Base | `83d735b9e2800aa2c209641356bb8e0fbf31de93` |
| Integrated Project main | `2fea5c084eb3b77bfc8f0dcc1241055627adc63c` via PR #174; push-triggered Validation #947 PASS |
| Candidate hosted gates | `5980a33acf5940c67e0af59593d55884b5627336`: Validation #946 PASS; E0-E preparation #118 PASS |
| Validation tag | `validation/q-design-20-native-corrections-arm64` |
| Evidence | `docs/evidence/Q_DESIGN_20_NATIVE_CORRECTIONS_ARM64_VALIDATION_2026_09_17.md` |
| Host | SurfSeven, native Windows ARM64, repository .NET 9 baseline |
| Native tests | Application 10/10 PASS; persistence 14/14 PASS |
| Package | ARM64 MSIX PASS, 0 warnings/errors; SHA-256 `5948a58ee49e4de34839ecd0755b308378d4fcd05ab9297477706a7aab4e8852` |
| UI return | NC-01..NC-05 applied; same-state Light/Dark captured; Back focus-return PASS; 720×520 observation PASS |
| Placeholders | Source Sans 3 packaging/localization/metrics and S1 native/shipping Stage asset packaging remain unresolved/accepted placeholders |
| High Contrast | exact-source recapture not attempted; prior remote-safety limitation remains; no bypass |
| Authority scope | bounded native correction checkpoint durably integrated; short Design acceptance pending |

Native correction authority is exact source `2484359...`; later evidence/integration commits do not inherit machine-test authority.

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
