# Kymaean Architecture & Ship Plan — Recursive Audit Evidence

Status: **AUDIT COMPLETE — DIRECTOR APPROVAL REQUIRED; NO IMPLEMENTATION AUTHORITY**

Date: 2026-09-04

Canonical plan:

`docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`

Audited proposal:

`0.7`

Exact audited proposal commit:

`c2687b4905f6f5eddcd7744c14643421f7ff79e4`

Exact audited plan content SHA:

`ed33ef60ebd7becb09857b8b7e172f2b1c51d711`

Authoritative starting `main`:

`1238b568565bf704a6e6fff70828730982db4b3e`

Architecture branch:

`kymaean-architecture-ship-plan`

---

## 1. Audit purpose

This audit determines whether Proposal 0.7 is coherent enough to become the major program map from the current Patch 0015/E0 checkpoint to a native ARM64 Microsoft Store release.

It does not validate implementation, runtime behavior, Windows AI execution, NPU routing, package behavior, WACK, or Store certification.

---

## 2. Authority reconciled

The audit checked Proposal 0.7 against:

- frozen Ensemble Blueprint 0.1;
- current `CURRENT_STATE.md` and Patch 0015 promoted authority;
- `docs/PROJECT_AUTHORITY.md`;
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md`;
- `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`;
- completed H1 Patch 0001-0015 architecture/evidence as relevant to the current dependency spine;
- the connected Drive legacy `Kymaean_Feature_Roadmap_and_Locked_Decisions_2026-08-20` document only as donor/platform inspiration, not current product authority;
- `Rylascoo/Kymaean-Project` as an untrusted donor repository;
- archived `Rylascoo/DeskShifter` V7 as immutable provenance/reference baseline;
- current official Microsoft and Qualcomm platform documentation where time-sensitive platform facts materially affect the ship plan.

The plan explicitly avoids restating a competing source-of-truth hierarchy and defers to `docs/PROJECT_AUTHORITY.md`.

---

## 3. Current external platform facts reverified for this audit

These are research inputs, not Ensemble validation claims.

### Windows App SDK

Microsoft's current Windows App SDK release/download documentation lists `2.4.0` as the latest stable Windows App SDK 2.x release at the plan date, released 2026-08-13, with ARM64 runtime distribution.

The plan therefore treats 2.4 as the current productization baseline while requiring the exact stable servicing version/API surface to be reverified when P2/packaging work actually begins.

### Package validation

Microsoft's Windows App SDK 2.0 release notes document the stable `Microsoft.Windows.Management.Deployment` package-validation framework, including:

- `IPackageValidator`;
- `PackageCertificateEkuValidator`;
- `PackageFamilyNameValidator`;
- `PackageMinimumVersionValidator`;
- `IsPackageValidationSupported`;
- `GetValidationEventSourceForUri`;
- package-validator attachment to add/stage options.

Proposal 0.7 therefore makes a supported `IPackageValidator` validation stage a required pre-WACK release gate rather than an optional donor idea.

### .NET support horizon

Microsoft's current .NET support policy lists:

- .NET 9: STS / Maintenance / end of support 2026-11-10;
- .NET 10: LTS / Active / end of support 2028-11-14.

The existing project mandate remains .NET 9. Proposal 0.7 does not change it. Instead, the plan makes Phase D the latest normal Director decision point for re-evaluating that support horizon before broad WinUI productization, with earlier escalation if schedule slippage makes the risk material and a Beta recheck before RC.

### Phi Silica / Aion Instruct

Microsoft currently documents Phi Silica as transitioning to Aion Instruct:

- early October 2026 testing/LoRA-training package;
- October 2026 Insider rollout;
- November 2026 retail rollout/removal of Phi Silica;
- LoRAs require transition/retraining guidance.

Proposal 0.7 therefore treats model identity and model-specific LoRA as adapter concerns, not Character/Core/Application authority.

### Windows AI / Windows ML / QNN

Microsoft documentation currently exposes `Microsoft.Windows.AI.MachineLearning` execution-provider APIs and lists Qualcomm `QNNExecutionProvider` for Windows ML 2.x on supported Snapdragon X hardware. Portions of the API documentation retain prerelease warnings, so every actual API used must be reverified at implementation time.

Qualcomm currently documents AI Engine Direct as a lower-level route to Kryo CPU, Adreno GPU and Hexagon NPU, including ONNX/TensorFlow delegation to Hexagon NPU, and documents Windows ML/ONNX interoperability for Windows on Snapdragon.

Proposal 0.7 therefore:

- prefers supported ready QNN/Hexagon NPU execution for heavy eligible local workloads;
- permits Qualcomm AI Engine Direct / Neural Processing SDK tooling where it materially improves optimization/profiling and remains release-safe;
- keeps CPU/GPU fallback capability-specific;
- makes no TOPS or actual-NPU-execution claim without device evidence.

---

## 4. Donor-repository conclusion retained

The legacy donor audit remains stable:

- infrastructure patterns may be selectively reused only after the current capability is designed;
- donor code is never current product authority;
- Production persistence is designed fresh;
- old Workspace/Routines/wallpaper domain is unrelated and excluded;
- `AppServices` monolith/private virtual-desktop ABI/Shell STA implementation are not direct retail donors;
- circuit breaker, bounded ephemeral cache, memory/power lessons, QNN probe patterns, package-validation patterns, PE architecture inspection and static verification lessons are eligible only in the phases named by Proposal 0.7.

Proposal 0.7 formalizes this as the Legacy Donor Quarantine Rule.

---

## 5. Proposal correction history

### Proposal 0.1

Initial program map established the broad path from E0 through persistence, Windows product architecture, AI/NPU, Alpha, Beta, WACK and Store.

Correction discovered:

- productization was too serialized; WinUI/UI convergence was unnecessarily blocked behind completed AI/NPU productionization.

### Proposal 0.2

Introduced parallel Windows/UI and AI/NPU productization after persistence.

Correction discovered:

- persistence implementation itself did not need to complete before the Windows shell/provider lanes could begin once post-E0 contracts were stable.

### Proposal 0.3

Moved persistence, WinUI, AI/NPU and UI toward parallel productization after the product-runtime architecture gate.

Corrections discovered:

- Windows-managed local model language implied too much about local portrayal before ODR-24 evidence;
- .NET 9 support decision at Beta alone was too late;
- release supply-chain review needed explicit coverage.

### Proposal 0.4

Made local workloads evidence-selected, moved the first framework decision before broad productization, and added release dependency/license/security provenance.

Corrections discovered:

- source-of-truth wording slightly diverged from canonical `PROJECT_AUTHORITY`;
- P4 code's soft dependency on the initial P2 shell needed explicit treatment;
- secret/settings donor patterns needed to stay outside causal-persistence ownership.

### Proposal 0.5

Aligned authority wording and lane dependencies.

Corrections discovered:

- NPU preference, exact Windows-AI degradation, LoRA/vision-text capability treatment and `IPackageValidator` release use were under-specified relative to project constraints.

### Proposal 0.6

Made heavy eligible local inference preferentially target supported ready QNN/Hexagon NPU; explicitly required readiness-state degradation including `CapabilityMissing` and `NotCompatibleWithSystemHardware`; clarified evidence-selected Windows text/vision/LoRA work; made `IPackageValidator` pre-WACK validation mandatory.

Final wording corrections:

- removed an unnecessary `certified` qualifier from the QNN execution-provider route and used the externally supported term;
- clarified Phase D is the latest normal .NET support-horizon gate, with earlier escalation if schedule risk becomes material;
- made the no-Kymaean-cloud-account assumption explicit from frozen local-sovereignty law.

### Proposal 0.7

Current recursively audited candidate.

---

## 6. Final clean-pass result

After Proposal 0.7 incorporated every correction above, the audit restarted from frozen Blueprint authority and completed one full pass with:

```text
0 material correctness corrections
0 authority/source-of-truth corrections
0 E0-scope/exclusion corrections
0 causal/event-persistence corrections
0 Character/Performer/Director/State-Authority corrections
0 Access/privacy/Presentation-Perspective corrections
0 provider/provenance/cost corrections
0 local-AI task-authority corrections
0 graceful-degradation corrections
0 ARM64/native-path corrections
0 NPU/QNN/Qualcomm-policy corrections
0 WinUI/MVVM dependency corrections
0 UI/design-stream convergence corrections
0 donor-quarantine corrections
0 security-milestone corrections
0 package/IPackageValidator/WACK/Store-authority corrections
0 framework-support-horizon corrections
0 release-supply-chain corrections
0 Sol-High task-scope workflow corrections
0 planning-range corrections that can be justified at current evidence level
0 worthwhile in-scope simplifications
```

The plan intentionally remains falsifiable. E0 evidence may still reopen frozen provisions under Blueprint 0.1's own rules, and later platform evidence may change productization details without invalidating the dependency laws.

---

## 7. Approved shape if the Director accepts Proposal 0.7

The major path becomes:

```text
Patch 0015 checkpoint
 -> close H1 deterministic spine
 -> complete E0-A harness
 -> E0-B..G
 -> E0 convergence / deletion audit
 -> post-E0 product-runtime architecture gate
 -> parallel productization
      P1 causal persistence/recovery
      P2 native WinUI/ARM64 shell
      P3 provider + evidence-selected Windows AI/NPU
      P4 UI implementation after minimum P2 shell
 -> runtime-complete Alpha
 -> Alpha hardening / launch-scope closure
 -> Beta/release architecture gate
 -> optional App Actions / non-blocking MCP as justified
 -> exact ARM64 MSIX + IPackageValidator + WACK + final security
 -> Store RC
 -> Partner Center certification
```

Visual/interaction exploration continues independently in Drive throughout the early phases and joins implementation through semantic view-state contracts after Phase D/P2 rather than waiting for all infrastructure to finish.

---

## 8. Planning interpretation

Proposal 0.7 retains the current planning ranges as estimates, not promises:

- solid product-runtime foundation: ~15-23 focused engineering days;
- runtime-complete Alpha: ~20-30 focused engineering days;
- Store-ready engineering RC: ~30-42 focused engineering days;
- calendar planning center with parallel UI/design work: roughly 5-8 weeks;
- roughly 20-35 substantial GPT-5.6 Sol High engineering work packages/chats plus short external machine-feedback turns.

Token volume is explicitly not a project KPI. The active workflow law optimizes useful reasoning/dependency closure per Director turn.

---

## 9. Validation boundary

This audit is static architecture/research evidence only.

It does **not** establish:

- compilation of any future product layer;
- WinUI launch/runtime;
- persistence runtime correctness;
- provider/runtime behavior;
- Windows AI readiness or inference;
- QNN/Hexagon NPU execution;
- measured battery/performance;
- MSIX success;
- `IPackageValidator` runtime success;
- WACK success;
- Partner Center certification.

Those remain the explicit future gates defined by the plan.

---

## 10. Approval gate

No source implementation is authorized by this audit.

Director approval is required before this plan is promoted into durable project authority.

If approved, the immediate checkpoint work should:

1. preserve Proposal 0.7 and this audit as immutable approval inputs;
2. add approval evidence;
3. promote/reference the plan from durable project state, including the pending direct `CURRENT_STATE.md` Sol-High bootstrap pointer;
4. leave product source untouched;
5. begin the next Sol-High work package by identifying the smallest remaining H1/E0-A deterministic boundary required for the complete run driver.
