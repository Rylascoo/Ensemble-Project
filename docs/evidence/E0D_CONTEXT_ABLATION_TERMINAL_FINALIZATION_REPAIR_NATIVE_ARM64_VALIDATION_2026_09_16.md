# E0-D Context-Ablation Terminal Finalization Repair — Native Windows ARM64 Validation

Date: 2026-09-16

Status: **REPAIR IMPLEMENTED + NATIVE MACHINE VALIDATION PASS — EXACT CHECKOUT `8770a6361233e8a877a966c45eb6f62c5b3ca182` — LATER E0-D LIVE EXECUTION STILL REQUIRES DIRECTOR DISPOSITION**

## Evidence-earned repair boundary

Authority predecessor: `docs/evidence/E0D_Q_E0D_01_P01_SLOT2_UNSEALED_TECHNICAL_TERMINAL_ANALYSIS_2026_09_15.md`.

P01 Slot 2 proved that an E0-D context-ablation Performer technical result was routed through baseline `DeterministicE0TurnOrchestrator.GateAttempt`, which recomposed the ordinary context, rejected the relationship-omitted packet as stale, and threw before `Finish(...)`. The consumed Slot-2 root remains immutable and unsealed; this repair does not modify, retry, replay, replace, or synthesize evidence for that run.

The smallest executable correction is four files relative to the prior admitted E0-D native checkout `0dacdbf6bd5453c192568cd4718207e145dfcf40`:

- `src/Ensemble.E0.Core/Experiments/E0D/E0DExperimentalTurnGate.cs` adds a context-ablation-aware technical/cancelled gate using the existing variant-aware context validation and `E0TurnProgress.CreateTechnical` path;
- `src/Ensemble.E0.Harness/Run/E0AReferenceRunDriver.cs` routes only Relationships-Omitted / Omniscient Performer technical terminals through that gate;
- Core regression coverage proves technical and cancelled outcomes for both context-ablation variants and rejects a mismatched variant context;
- Harness regression coverage proves all four variant/outcome combinations reach `run.terminal`, `run.summary.json`, and `run.final.json` without network traffic.

Full-reference, round-robin, provider/model/profile, fixture, scoring, claim, retry, and experiment-method contracts are unchanged.

## Exact validation identity

```text
checkout   8770a6361233e8a877a966c45eb6f62c5b3ca182
base       bd32532133b4dbc66d2ffd1a7f77c3d0a2fd38dd
branch     e0d-context-ablation-terminal-fix-2026-09-16
tag        validation/e0d-context-ablation-terminal-fix-native-arm64
tag object 0b3582aef6acf7e5ad2cf26585e0ccdc6a9a4dee
target     8770a6361233e8a877a966c45eb6f62c5b3ca182
```
## Native Windows ARM64 results

Validation ran on SurfSeven using repository-selected .NET SDK `9.0.317` and target `win-arm64` from clean detached checkout `C:\Users\Wiryl\Sol Dev\E0V-8770a63`.

- `Ensemble.E0.Core.Tests`: **628/628 PASS**.
- `Ensemble.E0.Harness.Tests`: **155/155 PASS**.
- Release Harness build: **PASS**, 0 warnings / 0 errors.
- Missing Raft fixture smoke: **PASS**.
- generic fixture smoke: **PASS**.
- runtime-sealing regressions for Relationships-Omitted + Omniscient × TechnicalFailure + Cancelled: **PASS**.
- existing successful context-ablation 12-turn paths remain PASS in the full Harness suite.
- exact executable SHA-256: `651aedf9e05800ea46ecc8a7c6565f9f91eeaf07794747dcb0a3c7697b946fac`.

With `GEMINI_API_KEY` removed only from the validation child environment, exact `e0d-run` exited **1** with `GEMINI_API_KEY is required at the E0-A provider edge.` The disposable validation evidence root remained absent and the validator checkout remained clean. No provider request, `countTokens`, generation, inference, scoring, or spend occurred.

Hosted branch-push Validation #885 passed on exact head `8770a6361233e8a877a966c45eb6f62c5b3ca182`.

## Repository gates

At exact checkout `8770a6361233e8a877a966c45eb6f62c5b3ca182`:

- repository law: **PASS**;
- document census: **316 inventory / 195 current / 30 historical / 91 archive / 0 unexplained current**;
- oracle guard: **266 documented / 17 asserted / 249 document-only hashes**;
- exact executable delta from prior native authority: **4 files only**;
- detached validator cleanliness before and after validation: **PASS**.
## Authority consequence

`8770a6361233e8a877a966c45eb6f62c5b3ca182` is the successor native Windows ARM64 validation identity for the repaired E0-D executable. The previous `0dacdbf6...` identity remains historical truth for the consumed P01 runs but is superseded for any future E0-D executable use.

This validation does **not** authorize P02/P03 activation, namespace claims, execution credentials, provider traffic, inference, scoring, or spend. P01 remains permanently consumed/noncontributing and experiential-ineligible. E0-D remains **ACTIVE / HOLD** until the Director decides whether the still-unconsumed P02/P03 slots remain admissible under this changed executable. Any later authorized slot must still pass its own fresh execution-sensitive capacity/interlock gate immediately before claim/provider traffic.
