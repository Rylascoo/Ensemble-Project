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

### Q-PROD-01 lifecycle reconstruction checkpoint

| Field | Fact |
|---|---|
| Exact source checkout | `7694f75479445c1f5ad030e37b1800a53d34dae9` |
| Base | `70744dfea843439e057738c9965c676961cf1bfb` |
| Integrated Project main | `fb37a7b6f09f4166d733cbc533e152d4d7d31287` via PR #178; push-triggered Validation #959 PASS |
| Candidate hosted gates | `b4139c76073bf8607765f6b8b9f01f053085e814`: Validation #958 PASS; E0-E preparation #122 PASS |
| Validation tag | `validation/q-prod-01-lifecycle-reconstruction-native-arm64` |
| Evidence | `docs/evidence/Q_PROD_01_LIFECYCLE_RECONSTRUCTION_NATIVE_ARM64_VALIDATION_2026_09_17.md` |
| Host | SurfSeven, native Windows ARM64, repository .NET 9 baseline |
| Application tests | 13/13 PASS on `win-arm64` Release |
| Persistence tests | 22/22 PASS on `win-arm64` Release |
| Product regression | WinUI ARM64 Release PASS, 0 warnings / 0 errors |
| Earned scope | model-neutral create/open/recover coordinator; fresh filesystem-backed store/coordinator reconstruction; semantic recovery after initial-head loss |
| Authority scope | object-lifetime reconstruction only; OS/process restart, device reboot, broader ontology, snapshots/export, final storage/concurrency, power-loss certification, complete P1 and release authority remain unearned |

Native lifecycle-reconstruction authority is exact source `7694f75...`; later evidence/integration commits do not inherit machine-test authority.

### Q-PROD-01 target-device process reopen/recovery checkpoint

| Field | Fact |
|---|---|
| Exact checkout under process evidence | `1ab6b5d0ea0e49afd565485e22a4e17e6d758bf3` |
| Exact-main hosted validation | Validation #962 PASS |
| Integrated Project main | `46a0b6874710deb68917e6801ca3e5e5dadabe04` via PR #180; push-triggered Validation #965 PASS |
| Candidate hosted gates | `4cdef429835a906bb5735447333926b295e57c9c`: Validation #964 PASS; E0-E preparation #124 PASS |
| Process-evidence tag | `validation/q-prod-01-process-reopen-native-arm64` |
| Product lifecycle native source | `7694f75479445c1f5ad030e37b1800a53d34dae9` |
| Evidence | `docs/evidence/Q_PROD_01_PROCESS_REOPEN_NATIVE_ARM64_VALIDATION_2026_09_17.md` |
| Host | SurfSeven, native Windows ARM64 |
| Ordinary process reopen | PASS: ARM64 PID 5748 create/exit -> distinct PID 9768 open/exit |
| Recovery process reopen | PASS: ARM64 PID 22564 create/exit -> head loss -> distinct PID 6584 recover/exit -> distinct PID 22108 open/exit |
| External driver | evidence-only; preserved local apparatus; EXE SHA-256 `fe821698083c5784c7cced8dc060df52cb29ae47f159f3dd279bb50fb165d0ec`; no shipping project added |
| Earned scope | current Production-name persistence reconstructs and recovers across real native ARM64 OS process boundaries |
| Authority scope | not packaged-app relaunch, device reboot/suspend-resume, arbitrary power-loss certification, snapshots/export, broader ontology, final storage/concurrency, complete P1 or release authority |

### Q-PROD-01 persistence schema/version policy checkpoint

| Field | Fact |
|---|---|
| Lease | `ENG1-QPROD01-PERSIST-01` |
| Base | `09f172d8fe50964f1d5c39f1ddf0f0bddb94d8ab`; push-triggered Validation #974 PASS |
| Exact source | `f6a9f0f6b3abc6d8a34debfcfdc9dac0474e0147` |
| Validation tag | `validation/q-prod-01-persistence-version-policy-native-arm64` |
| Evidence | `docs/evidence/Q_PROD_01_PERSISTENCE_VERSION_POLICY_NATIVE_ARM64_VALIDATION_2026_09_17.md` |
| Host | SurfSeven, native Windows ARM64 |
| Native tests | Persistence 30/30 PASS; Application 13/13 PASS |
| WinUI compiler | ARM64 Release PASS, 0 warnings/errors |
| Repository guards | law PASS; census 338/217/30/91/0; oracle 288/17/271; diff PASS |
| Earned policy | stable file-family magic; physical journal/head schema uint v1; Product event contract version independent; integrity-valid unsupported versions typed as compatibility; corrupted version bytes remain corruption; no implicit migration/rewrite |
| Source scope | five Persistence-owned source/test files; no Application/Windows/provider/E0/shared-authority source mutation |
| Integrated Project main | `0cbf19a997665fa24cd0120b209e2ecd39925cec` via PR #188; push-triggered Validation #980 PASS |
| Candidate hosted gates | `b8733ffef15ba14819dfe7332c53431adaa44a89`: Validation #979 PASS; E0-E preparation #127 PASS |
| Authority scope | integrated compatibility policy only; no migration engine, catalog/ProductionId layout, snapshots/export, final storage/concurrency, broader ontology, complete P1 or release authority |

Exact native authority is source `f6a9f0f...`; later evidence/integration commits do not inherit machine-test authority.

### Q-PROD-01 Application architecture checkpoint

| Field | Fact |
|---|---|
| Lease | `ENG2-QPROD01-APPARCH-01` |
| Base | `331d38538767c948871bf5f0d69e377e3ce43fc9`; push-triggered Validation #987 PASS |
| Exact source | `d58ab0ec048eb913bb67eab8056a098c478b492e` |
| Validation tag | `validation/q-prod-01-application-architecture-native-arm64` |
| Evidence | `docs/evidence/Q_PROD_01_APPLICATION_ARCHITECTURE_NATIVE_ARM64_VALIDATION_2026_09_18.md` |
| Host | SurfSeven, native Windows ARM64 |
| Native tests | Application 24/24 PASS; Persistence 30/30 PASS |
| WinUI compiler | ARM64 Release PASS, 0 warnings/errors |
| Repository guards | law PASS; census 339/218/30/91/0; oracle 288/17/271; diff PASS |
| Hosted exact-head | Validation #989 PASS |
| Integrated Project main | `c09a3858e5fa18b67cc743551eab9fa318f24e61` via PR #187; push-triggered Validation #990 PASS |
| Earned contract | opaque ProductionId; ProductionSummary; list/open catalog port; open returns earned ProductionReplayProjection; ApplicationScope separate from ProductSpace; fail-closed list/open consistency |
| Source scope | exactly five Engineer #2-owned Application/test files; no Persistence/Windows/provider/E0/shared-authority mutation |
| Milestone | `DESIGN_ARCHITECTURE_READY = NOT READY` |
| Authority scope | no persisted Studio/Stage/Archive content beyond ProductionName; no typed open/recovery/compatibility/corruption presentation result; no catalog storage/ID serialization/create/rename/delete; no final navigation labels, Windows lifecycle, provider behavior or visual authority |

Exact native authority is source `d58ab0e...`; later closeout commits do not inherit machine-test authority.

### Q-PROD-01 Application typed-results checkpoint

| Field | Fact |
|---|---|
| Lease | `ENG2-QPROD01-APPRESULT-02` |
| Base | `ca807a96ccc94ef9f7a185b58d99f496da799f23`; push-triggered Validation #996 PASS |
| Exact source | `2c379fd21bac5c4de11df0d458f0194189950c7f` |
| Validation tag | `validation/q-prod-01-application-typed-results-native-arm64` |
| Evidence | `docs/evidence/Q_PROD_01_APPLICATION_TYPED_RESULTS_NATIVE_ARM64_VALIDATION_2026_09_18.md` |
| Host | SurfSeven, native Windows ARM64 |
| Native tests | Application 38/38 PASS; Persistence 30/30 PASS |
| WinUI compiler | ARM64 Release PASS, 0 warnings/errors |
| Repository guards | law PASS; census PASS; oracle 288/17/271; diff PASS |
| Hosted exact-head | Validation #998 PASS |
| Integrated Project main | `7837de20e2e19ffc7f0c31ccc7f60ff5f7200fa2` via PR #194; push-triggered Validation #999 PASS |
| Earned contract | typed full-list/bootstrap; typed selected-open; separate explicit selected-recover; exactly Incompatible/Invalid failure meaning; failures preserve prior Application state |
| Source scope | exactly four Engineer #2-owned Application/test files; no Persistence/Windows/provider/E0/shared-authority mutation |
| Branch lifecycle | lease #192 CLOSED; validation/archive tags peel to exact source; former worker branch/worktree retired after durable integration |
| Milestone | `DESIGN_ARCHITECTURE_READY = NOT READY` |
| Authority scope | no catalog storage/ID encoding, recovery prediction/auto-recovery/migration, partial catalog, richer persisted Studio/Stage/Archive content, Windows composition, provider behavior or visual authority |

Exact native authority is source `2c379fd...`; later closeout commits do not inherit machine-test authority.

### Q-PROD-01 Persistence catalog/recovery adapter checkpoint

| Field | Fact |
|---|---|
| Lease | `ENG1-QPROD01-PERSISTCAT-02` |
| Base | `257fbc52946dad32b0376e825c8137262ae3c849`; push-triggered Validation #1005 PASS |
| Exact source | `1b20d937892c3248a0f84d536870b789d865e851` |
| Validation tag | `validation/q-prod-01-persistence-catalog-recovery-adapter-native-arm64` |
| Evidence | `docs/evidence/Q_PROD_01_PERSISTENCE_CATALOG_RECOVERY_ADAPTER_NATIVE_ARM64_VALIDATION_2026_09_18.md` |
| Host | SurfSeven, native Windows ARM64 |
| Native tests | Persistence 52/52 PASS; Application 38/38 PASS |
| WinUI compiler | ARM64 Release PASS, 0 warnings/errors |
| Repository guards | law PASS; census 341/220/30/91/0; oracle 288/17/271; diff/race/worktree PASS |
| Hosted exact-head | Validation #1007 PASS |
| Integrated Project main | `b14b579408bec23865efede9a92a66d4f3bd1cd8` via PR #199; push-triggered Validation #1008 PASS |
| Earned contract | opaque app-private root; Persistence-owned catalog namespace; bounded opaque locators; versioned/checksummed exact UTF-16 ProductionId metadata; complete-or-fail list; committed open; explicit recover; typed compatibility/corruption mapping; unknown IDs create no storage |
| Consumer acceptance | Engineer #3 exact-source native validation and semantic acceptance PASS; ENG3-REQ-PERSIST-01 #195 CLOSED |
| Branch lifecycle | lease #197 CLOSED; validation/archive tags peel to exact source; former Engineer #1 branch/worktree retired after durable integration |
| Milestone | `DESIGN_ARCHITECTURE_READY = NOT READY` |
| Authority scope | no create/rename/delete, migration, snapshots/export, final concurrency/power-loss certification, richer persisted Studio/Stage/Archive state, Windows composition proof, provider behavior or visual authority |

Exact native authority is source `1b20d937...`; later closeout commits do not inherit machine-test authority.

### Q-PROD-01 Windows runtime/composition checkpoint

| Field | Fact |
|---|---|
| Lease | `ENG3-QPROD01-WINCOMP-01` / Issue #186 |
| Base | `aa1adecbac58b87ec97d9ce6cf977deb9b7224ee`; exact-main Validation #1020 PASS |
| Exact source | `ac8122c24d81731671802b57220d3c631e8c9975` |
| Validation tag | `validation/q-prod-01-windows-runtime-composition-native-arm64` |
| Evidence | `docs/evidence/Q_PROD_01_WINDOWS_RUNTIME_COMPOSITION_NATIVE_ARM64_VALIDATION_2026_09_18.md` |
| Host | SurfSeven, native Windows ARM64 |
| Native tests | Application 38/38 PASS; Persistence 52/52 PASS |
| WinUI compiler | Release win-arm64 PASS, 0 warnings/errors |
| Package/runtime | unsigned ARM64 MSIX PASS; launch/relaunch PASS; LocalState catalog reuse PASS; Demo/missing-raft absent |
| Startup probe | typed Product failure preserved; IOException/UnauthorizedAccessException -> generic infrastructure state; InvalidOperationException escapes; Open/Recover route once |
| Repository guards | law PASS; census PASS; oracle 288/17/271; diff/race/worktree PASS |
| Hosted exact-head | branch Validation #1021 PASS; PR #208 Validation #1022 PASS |
| Integrated Project main | `032f6781c2687a07644a33532967c8e49c85437a` via PR #208; push-triggered Validation #1023 PASS |
| Earned contract | production Windows composes Application + Persistence from app-private LocalState; explicit open/recover; bounded Windows infrastructure failure state; no Persistence internals or fabricated Product state |
| Milestone | `DESIGN_ARCHITECTURE_READY = NOT READY` |
| Authority scope | no richer persisted Studio/Stage/Archive content, Product lifecycle expansion, provider behavior, final architecture, WACK/Store or deferred-E0 conclusion |

Exact native authority is source `ac8122c...`; later closeout commits do not inherit machine-test authority.

### Q-PROD-01 rebuildable projection snapshot checkpoint

| Field | Fact |
|---|---|
| Lease | `ENG1-QPROD01-SNAPSHOT-03` / Issue #203 |
| Base | `7918b92dbfca983ffc92967eb3bd6a24016c9493`; exact-main Validation #1026 PASS |
| Exact source | `aaca35069ca68a1a28d0bd189e6a0eb2e7ad8724` |
| Validation tag | `validation/q-prod-01-persistence-snapshot-native-arm64-r2` |
| Evidence | `docs/evidence/Q_PROD_01_REBUILDABLE_PROJECTION_SNAPSHOT_NATIVE_ARM64_VALIDATION_2026_09_18.md` |
| Host | SurfSeven, native Windows ARM64 |
| Native tests | Persistence 65/65 PASS; Application 38/38 PASS |
| WinUI compiler | Release win-arm64 PASS, 0 warnings/errors |
| Repository guards | law PASS; census 343/222/30/91/0; oracle 288/17/271; diff/race/worktree PASS |
| Hosted exact-head | branch Validation #1029 PASS; PR #210 Validation #1030 PASS |
| Integrated Project main | `6df408fe43cb714ad00d63c97f916174beffb0ab` via PR #210; push-triggered Validation #1031 PASS |
| Earned contract | Persistence-owned versioned/checksummed projection snapshot bound to exact final journal sequence + record hash; missing/stale/malformed/checksum-invalid/version-incompatible/wrong cache state rebuilds from journal truth; cache cannot mask corrupt/incompatible/replay-invalid journal; explicit recovery refreshes only after authoritative validation; expected cache I/O/access failure is best-effort |
| Performance non-authority | no read-path acceleration earned; catalog still validates, decodes and replays authoritative journal history before snapshot reconciliation |
| Milestone | `DESIGN_ARCHITECTURE_READY = NOT READY` |
| Authority scope | no portable export, final concurrency/power-loss certification, richer persisted Studio/Stage/Archive content, provider behavior, WACK/Store or deferred-E0 conclusion |

Exact native authority is source `aaca350...`; this closeout records integration and does not inflate machine-test or performance authority.

### Q-PROD-01 portable credential-independent export checkpoint

| Field | Fact |
|---|---|
| Lease | `ENG1-QPROD01-EXPORT-04` / Issue #213; duplicate transport #212 closed as duplicate |
| Base | `040f6b66410fd1d5076fd99188b624564682a09a`; exact-main Validation #1034 PASS |
| Exact source | `ae6a871a9338ae02f63193267a6e596a4ebfefc9` |
| Validation tag | `validation/q-prod-01-portable-export-native-arm64` |
| Evidence | `docs/evidence/Q_PROD_01_PORTABLE_EXPORT_NATIVE_ARM64_VALIDATION_2026_09_18.md` |
| Host | SurfSeven, native Windows ARM64 |
| Native tests | Persistence 80/80 PASS; Application 38/38 PASS |
| WinUI compiler | Release win-arm64 PASS, 0 warnings/errors |
| Repository guards | law PASS; pre-freeze census 344/223/30/91/0 with unchanged document tree; exact-source/integrated hosted census PASS; oracle 288/17/271; diff/race/worktree PASS |
| Hosted exact source | Validation #1038 PASS; #1037 is diagnostic only because force-push BASE_SHA `5f5cce...` was unavailable, not a source failure |
| Hosted exact-head | PR #214 Validation #1039 PASS |
| Integrated Project main | `adaeb47c366e60e3b00b92ab517a5068f8b1a426` via PR #214; push-triggered Validation #1040 PASS |
| Earned contract | Persistence-owned versioned/checksummed portable package; exact Product identity + raw validated event payload order preserved; live locator/layout/snapshot/provider-local artifacts excluded; no implicit recovery/repair; durable pending+flush+finalize file path; read-only inspection |
| Failure boundary | unknown/corrupt/replay-invalid source -> Invalid; unsupported identity/event contract -> Incompatible; destination I/O/access remains environmental |
| Milestone | `DESIGN_ARCHITECTURE_READY = NOT READY` |
| Authority scope | no Product import/restore/create/rename/delete lifecycle, final corruption/power-loss/concurrency certification, richer persisted Studio/Stage/Archive content, provider behavior, WACK/Store or deferred-E0 conclusion |

Exact native authority is source `ae6a871...`; this closeout records integration and does not convert export into import/lifecycle or richer Product authority.

### Q-PROD-01 broader corruption/interruption evidence checkpoint

| Field | Fact |
|---|---|
| Lease | `ENG1-QPROD01-CORRUPT-05` / Issue #217 |
| Base | `7d050a4a8330514a8b126718dd3fe8bb9332b506`; exact-main Validation #1043 PASS |
| Exact evidence source | `a77170e99daa614c89c7949baab1ad959ffe64bc` |
| Validation tag | `validation/q-prod-01-corruption-interruption-evidence-native-arm64` |
| Evidence | `docs/evidence/Q_PROD_01_CORRUPTION_INTERRUPTION_EVIDENCE_NATIVE_ARM64_VALIDATION_2026_09_18.md` |
| Source scope | one Persistence test file only; no production source changed |
| Host | SurfSeven, native Windows ARM64 |
| Native tests | Persistence 85/85 PASS; Application 38/38 PASS |
| WinUI compiler | Release win-arm64 PASS, 0 warnings/errors after clearing a transient XAML-compiler file lock; exact source unchanged |
| Repository guards | law PASS; census 345/224/30/91/0; oracle 288/17/271; diff/race/worktree PASS |
| Hosted branch | Validation #1044 PASS |
| Hosted exact-head | PR #218 Validation #1045 PASS |
| Integrated Project main | `70b832ff61195a4818282b838716c6e92689fff7` via PR #218; push-triggered Validation #1046 PASS |
| Newly exercised evidence | torn committed entry/head fail closed; impossible integrity-valid head fails closed; mixed valid+corrupt crash suffix recovery is all-or-nothing; pending artifacts remain non-causal on ordinary read |
| Runtime result | no production defect exposed; no runtime/storage-format change required |
| Milestone | `DESIGN_ARCHITECTURE_READY = NOT READY` |
| Authority scope | no final storage/concurrency policy, multi-process guarantee, power-loss certification, Product lifecycle/import, richer persisted Studio/Stage/Archive content, provider behavior, WACK/Store or deferred-E0 conclusion |

Exact native authority is evidence source `a77170e...`; this checkpoint broadens tested failure-state evidence without promoting it into general power-loss or concurrency certification.

### Q-PROD-01 final provisional storage/concurrency policy checkpoint

| Field | Fact |
|---|---|
| Lease | `ENG1-QPROD01-POLICY-06` / Issue #220 |
| Base | `8d47959ddadaa84db82ed5d3ceaeef9c42189f19`; exact-main Validation #1049 PASS |
| Exact policy source | `c352d96fc3a10ea5e0a517c4057e4173000288d3` |
| Validation tag | `validation/q-prod-01-final-provisional-storage-concurrency-policy-native-arm64` |
| Policy evidence | `docs/evidence/Q_PROD_01_FINAL_PROVISIONAL_STORAGE_CONCURRENCY_POLICY_NATIVE_ARM64_2026_09_18.md` |
| Integration closeout | `docs/evidence/Q_PROD_01_FINAL_PROVISIONAL_STORAGE_CONCURRENCY_POLICY_INTEGRATION_CLOSEOUT_2026_09_18.md` |
| Source scope | documentation/authority only; no runtime or test source changed |
| Host | SurfSeven, native Windows ARM64 |
| Native tests | Persistence 85/85 PASS; Application 38/38 PASS |
| WinUI compiler | Release win-arm64 PASS, 0 warnings/errors |
| Repository guards | law PASS; census 347/226/30/91/0; oracle 288/17/271; diff/race/worktree PASS |
| Hosted branch | Validation #1050 PASS |
| Hosted exact-head | PR #221 Validation #1051 PASS |
| Integrated Project main | `c3e7171b61daead881ed639729496cd050868d12` via PR #221; push-triggered Validation #1052 PASS |
| Native contention evidence | same-Production `ReadAll`/`Append`/`Recover` fail fast with environmental Windows sharing-violation I/O while another process owns the per-root journal gate; a different Production root remains independently readable |
| Earned policy | one exclusive journal gate per Production root; no transparent same-root multi-process queue/wait/retry; no catalog-global transaction or cross-Production atomicity; snapshot non-causal; export point-in-history; no general power-loss certification |
| Runtime result | no production defect exposed; no runtime/storage-format change required |
| Engineer #1 charter | five-rung Q-PROD-01 Persistence sequence COMPLETE |
| Milestone | `DESIGN_ARCHITECTURE_READY = NOT READY` |
| Authority scope | no Product lifecycle/import/restore expansion, richer persisted Studio/Stage/Archive content, provider/deferred-E0 behavior, transparent multi-process support, universal power-loss guarantee, WACK/Store or release authority |

Exact native authority is policy source `c352d96...`; this manager closeout records its durable integration and does not create new machine/runtime authority.

### Q-PROD-01 Persistence manager closeout / retirement checkpoint

| Field | Fact |
|---|---|
| Closeout source | `19c06b74b12e85faab1d8306ebeb20089cd99bd7` |
| Closeout PR | #222 |
| Integrated Project main | `828e643dee05f92825ac0c57b6dc7aa8240b4d2f` |
| Hosted closeout gates | branch Validation #1053 PASS; PR exact-head Validation #1054 PASS; E0-E preparation #141 PASS |
| Exact-main closeout gate | push-triggered Validation #1055 PASS, all eight jobs |
| Lease retirement | Issue #220 CLOSED / completed; source and closeout branches retired |
| Preserved tags | `validation/q-prod-01-final-provisional-storage-concurrency-policy-native-arm64`; `archive/engineer-01/q-prod-01/policy-06`; `archive/q-prod-01-policy-closeout-2026-09-18` |
| Runtime authority | unchanged; this is continuity/retirement validation only |
| Successor boundary | Engineer #2 lease `ENG2-QPROD01-DESIGNARCH-02` / #223 opened from exact validated main; `DESIGN_ARCHITECTURE_READY = NOT READY` |

This terminal closeout adds no Product/Persistence semantics, provider authority, deferred-E0 execution, WACK/Store authority, or higher validation rung.

### Q-PROD-01 World-current-truth successor checkpoint

| Field | Fact |
|---|---|
| Package | `QPROD01-NATIVE-PRODUCER-01` |
| Fresh base | `3a42bcd60047f260eadfce3781afb3bcefcbdd87`; exact-main Validation #1108 PASS |
| Historical producer | `dc1780e46ef999eb01e214671486b83186ca04c4`; PR #226 closed unmerged as superseded; branch, tag and worktree preserved |
| Exact runtime source | `168c5d306afc88202e9d5d30aacaeca8e4119dc6`; seven production/test blobs initially byte-identical to the producer |
| Reviewed corrected candidate | `0a48949d6c1e02be420bb9204b3b85b08fd76150`; six direct null-contract tests added after review, no runtime-source change |
| Successor PR | PR #240 merged normally from exact head `1c14bdcb600d7a0a08d29044fff646bfb2c7e406`; integrated Project main `9516a27ca019d2c6af304f46da89bde215449df4` |
| Validation tag | `validation/q-prod-01-world-current-truth-successor-native-arm64` -> exact runtime source |
| Evidence | `docs/evidence/Q_PROD_01_WORLD_CURRENT_TRUTH_SUCCESSOR_NATIVE_ARM64_VALIDATION_2026_09_20.md` |
| Host | SurfSeven, native Windows ARM64, repository .NET 9 baseline |
| Native tests | focused World-current contract 19/19 PASS; Application 57/57 PASS; Persistence 85/85 PASS; Core 628/628 PASS |
| WinUI compiler | ARM64 Release PASS, 0 warnings/errors |
| Repository guards | law PASS; final staged census 511/390/30/91/0; oracle 674/17/657; diff/protected-diff/commissioning closeout PASS |
| Review | exact-SHA behavioral no-write review found no actionable findings after correction; effective sandbox was workspace-write, so technical containment was not proven |
| Earned contract | immutable canonical `WorldCurrentTruth` / `WorldCurrentState`; creator-specific whole-state replacement; old histories empty; deterministic replay; null/duplicate rejection; typed failure preserves prior usable projection/navigation |
| Authority scope | Application producer only; no Persistence consumer/encoding, Character knowledge, UI, provider, deferred-E0, final architecture, release, WACK or Store authority |

Exact runtime authority remains `168c5d3...`; the corrected review candidate adds tests only. Integration main and the documentation-only closeout do not inherit native runtime authority or inflate compiler evidence.

### Q-PROD-01 lossless Persistence consumer integrated checkpoint

| Field | Fact |
|---|---|
| Base | `fb5c8d630b5aec1d6ab75bb57b7ffc65da79c00f` |
| Exact tested/reviewed candidate | `e07267185cbae7caa62f70bc9f9a39852c0eedd0` |
| Validation tag | `validation/q-prod-01-lossless-persistence-consumer-native-arm64` |
| Evidence | `docs/evidence/Q_PROD_01_LOSSLESS_PERSISTENCE_CONSUMER_NATIVE_ARM64_VALIDATION_2026_09_21.md` |
| Director decision | `docs/Q_PROD_01_LOSSLESS_UTF16_PERSISTENCE_CONTRACT_2026_09_21.md` |
| Native Windows ARM64 | focused 44/44, Persistence 129/129, Application 57/57, Core 628/628 PASS; WinUI Release 0 warnings/errors |
| Earned contract | known-ID writer; one replacement append; exact UTF-16 creation v2/replacement v1; legacy creation v1 decode; full snapshot v2; raw portable export/replay |
| Review | no actionable findings; exact-ref behaviorally no-write reviewer; workspace-write/auto_review, technical containment unproven |
| Integration | PR #242 merged at `4283505a0cd44dae9fa812893ec79d9a052ee6f9`; exact-main push Validation #1119 (35566903678) PASS; runtime/test/build blobs identical to tested candidate |
| Disposition | consumer integrated / CLOSED; Issue #225 fulfilled / CLOSED. Explicit Engineering reassessment records `DESIGN_ARCHITECTURE_READY = READY` in `docs/evidence/Q_PROD_01_DESIGN_ARCHITECTURE_READINESS_REASSESSMENT_2026_09_21.md`; no new native runtime claim |
| Limits | no Application-domain narrowing, new UI/provider/deferred-E0/final-architecture/release/WACK/Store authority; readiness is bounded architecture handoff only |

Machine evidence binds the candidate and identical tested runtime/test/build blobs. Initial setup failures and two corrected snapshot-fixture failures are retained in the evidence record. No validation claim is inflated by a later documentation-only commit.

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

### Q-PROD-02 / Q-DESIGN-20 R2 Windows presentation integrated checkpoint

| Field | Fact |
|---|---|
| Base | `c27689ab690c23265e8dbe6cfc6691e428723ee0` |
| Exact accessibility-corrected executable source | `150f7c57b523f6d95e8f85a5b1c20ad769c3a120` |
| Prior full visual/native source | `95d133dfbb8e6e0c5b9519dd2c0b668416c93218`; tagged `validation/q-prod-02-r2-native-arm64-ui-2026-09-21` |
| Evidence | `docs/evidence/Q_PROD_02_Q_DESIGN_20_R2_NATIVE_ARM64_VALIDATION_2026_09_21.md` |
| Design acceptance | `docs/design/app/evidence/native/APPUI_01_Q_DESIGN_20_R2_NATIVE_ACCEPTANCE_2026_09_21.json`; ACCEPTED / INTEGRATED |
| Native ARM64 | Exact-current Application 57/57 PASS; WinUI Release build 0 warnings/errors. Persistence 129/129 remains bound to `f7678811c715f3c3cfe6daed95a38737fedc0ba2`. |
| Native UI | Full Open/Recover, contextual Current Production, three durable shell items, Back name/tooltip/focus return, Light F2/Dark D3, MAT F1 and 720x520 evidence at visually identical `95d133...`; exact-current UIA reports Production row exactly `Glass Harbor` with zero internal object/ID leakage. |
| Exact-current temporary MSIX | SHA-256 `F0E0FF8C7CE857FC1832F6CBCCD0C681D14366BA570205EFB981F208172BDB9F`; temporary package unregistered. |
| Final PR head | `56450a0b97939ac5c2ca6224c6f426dba6b31c3b`; Validation #1144 PASS; E0-E preparation #172 PASS |
| Integrated main | PR #244 merge `2470ed1068454e3ff5cfd573dcc266328bd1d1cc`; push-triggered exact-main Validation #1145 PASS |
| Limits | High Contrast exact-source recapture, Source Sans 3/S1 packaging, WACK/Store/release remain unearned. World-current UI remains separately gated. |

Runtime/UI evidence remains bound to the exact tested sources above; the merge and later documentation closeout do not inflate native authority.

### Q-PROD-03 / Q-DESIGN-21 World-current presentation integrated checkpoint

| Field | Fact |
|---|---|
| Base | `d77685f73f6271e4cf86902b5471da90fbe11ad4`; exact-main Validation #1147 PASS |
| Exact tested executable source | `75faeb827162c89b537d2a944473b89eabfc22ea` |
| Evidence | `docs/evidence/Q_PROD_03_WORLD_CURRENT_TRUTH_PRESENTATION_NATIVE_ARM64_VALIDATION_2026_09_21.md` |
| Design acceptance | `docs/design/app/evidence/native/APPUI_01_Q_DESIGN_21_WORLD_CURRENT_TRUTH_NATIVE_ACCEPTANCE_2026_09_21.json`; ACCEPTED / INTEGRATED |
| Native ARM64 | Application 57/57 PASS; Persistence 129/129 PASS; WinUI Release build PASS, 0 warnings/errors |
| Native UI | contextual route, non-empty/empty inspection, blank/duplicate blocking, exact UTF-16 preservation, canonical whole-state review, confirmed replay, valid empty replacement, Incompatible, Invalid, environmental Confirmation unavailable, ordinary-open re-establishment, Light/Dark, 720x520 and focus return PASS |
| Accessibility | zero tested internal WorldCurrentTruth/WorldTruthDraftItem/ProductionSummary/fixture-ID/entry-path names; Back restores World truths focus |
| Package | temporary ARM64 MSIX SHA-256 `BF50ABA83CA9E10B893BFCBA3D7364E3C1528B0EE966972FF195BCE44991D364`; unregistered after evidence |
| Final PR head | `128f207bd68f7a999498ea578cc67a98137ed7e9`; push Validation #1153 PASS; PR Validation #1154 PASS; E0-E preparation #175 PASS |
| Integrated main | PR #245 merge `969dfc790a00d2cdbc61d7c3d28a95ceb96b6f72`; push-triggered exact-main Validation #1155 PASS |
| Limits | High Contrast exact-source recapture, Source Sans 3/S1 packaging, WACK/Store/release remain unearned; no Product/Persistence semantics changed |

Native authority remains bound to the exact tested source; integration and later documentation do not inflate the evidence rung.

### Q-PROD-04 Production creation integrated checkpoint

| Field | Fact |
|---|---|
| Base | `f8a5a6008b2bad93a8a13b1927da302c34113e33`; exact-main Validation #1158 PASS |
| Director contract | `docs/Q_PROD_04_PRODUCTION_CREATION_DIRECTOR_DECISION_2026_09_22.md`; A/A/A |
| Exact tested executable source | `8f7d5f05909dd88f6dd24e20b60b80ae5b36f5cc` |
| Evidence | `docs/evidence/Q_PROD_04_PRODUCTION_CREATION_NATIVE_ARM64_VALIDATION_2026_09_22.md` |
| Earned Application contract | Application-owned `IProductionCreator`; creation returns opaque ID + authoritative creation replay; known list updates; active scope/current Production/ProductSpace remain unchanged; duplicate names allowed |
| Earned Persistence contract | catalog-generated opaque identity; independent technical locator; fully formed pending construction outside discoverable catalog; complete directory publication; exact UTF-16 identity/name preservation; no partial catalog entry on pre-publication failure |
| Native ARM64 | Application 66/66 PASS; Persistence 135/135 PASS; WinUI Release build PASS, 0 warnings/errors |
| Final PR head | `1f9ded8aea55d28afb61c171b366c650201e2d93`; push Validation #1162 PASS; PR Validation #1163 PASS; E0-E preparation #176 PASS |
| Integrated main | PR #246 merge `bb356f0bedd80c3f5b9a0985e81cc63ea5b6fb44`; push-triggered exact-main Validation #1164 PASS |
| Limits | no creation UI/FIRSTUSE/Home, rename/delete/import/restore, Character/Scene/provider semantics, Alpha/final architecture, WACK/Store/release authority |

Native authority remains bound to the exact tested source; integration and this documentation closeout do not inflate the evidence rung.

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
