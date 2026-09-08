# Kymaean Architecture & Ship Plan

Status: **ACTIVE PROGRAM MAP — Proposal 0.7 dependency laws, maintained after H1 Phase A; NOT PATCH IMPLEMENTATION AUTHORITY**

Maintained: 2026-09-08

Current engineering checkpoint and exact next action are owned exclusively by `CURRENT_STATE.md` at the active exact ref. This program map preserves dependency order and durable laws; it does not restate volatile checkpoint truth.

Purpose: preserve the dependency-ordered engineering path from Ensemble E0 through a native ARM64 Kymaean Microsoft Store release without allowing exploratory code, UI work, provider APIs, Windows capabilities, release tooling, or legacy donors to become accidental fictional/domain authority.

This maintenance revision changes no frozen Blueprint 0.1 law and authorizes no implementation by itself. Approved patch blueprints, `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, machine evidence, Director decisions, WACK, and Partner Center retain their respective authority.

---

## 1. Product identity

Kymaean is the shipping identity of Ensemble:

> A local-first generative theater and creative simulation for persistent characters. The creator establishes people, circumstances, knowledge, relationships, possibilities and pressures. AI performers portray those characters. A Director manages attention and opportunities to act. Accepted performances become causal history, and authoritative consequences alter what future scenes can mean.

The current product is not the shelved DeskShifter/Kymaean Workspace/Routines/wallpaper domain. Legacy repositories and Drive material are donor/reference sources only.

## 2. Laws that survive every phase

1. **Creator authority:** probabilistic systems propose; deterministic authority commits.
2. **Character != Performer:** recasting cannot replace Character identity/history.
3. **Access before relevance:** prohibited information is removed before semantic selection or model input.
4. **Claim != fact:** truth, possibility, observation, claim, knowledge, belief, suspicion, memory, and provenance remain distinct.
5. **Accepted Performance is immutable causal history.**
6. **Accepted Performance + authoritative consequences adopt atomically.**
7. **Infrastructure stays at the edges:** Core does not depend on WinUI, provider SDKs, Windows AI/NPU, filesystem, package, or Store machinery.
8. **Local sovereignty:** Production authority and credentials remain locally controlled; remote providers receive temporary bounded packets only.
9. **Graceful degradation preserves correctness:** provider/model/NPU failure may reduce capability, never corrupt canon or invent fictional action.
10. **ARM64-native retail path:** no x86-native dependency or emulation requirement.
11. **Low idle cost:** event-driven work, bounded resources, no hidden high-frequency polling.
12. **Validation levels never inflate:** static < compiler < runtime < hardware/NPU < package/WACK < Partner Center.
13. **UI is projection, not authority.**
14. **One canonical implementation:** obsolete internal paths are removed when no approved proof/compatibility role remains.
15. **Research is not product authority:** platform/model experiments cannot silently become launch dependencies.

## 3. Target logical architecture

Exact project names and assembly count remain later implementation decisions. Dependency direction remains:

```text
KYMAEAN WINUI 3
 Studio / Stage / Archive
        |
Presentation / strict MVVM
        |
Application use cases + creator commands
        |
+-------+-------------------+
|       |                   |
Core    Persistence ports   AI/provider ports
|       |                   |
+-------+-------------------+
        |
Infrastructure adapters
 storage / secrets / network / Windows capabilities
        |
Windows host edge
 lifecycle / package identity / MSIX / optional extensibility
```

### Core

Owns fictional and causal semantics only. Deterministic logic remains pure where practical.

### Application

Coordinates approved Core authorities into use cases. It may own Scene/run state machines, provider-attempt sequencing, cancellation, creator commands, budgets, and persistence transactions, but cannot redefine truth semantics.

### Persistence

Production causal persistence is separate from settings/preferences. Rebuildable projections/snapshots are acceleration, not source of truth.

### AI/provider

Providers produce attempts/proposals and provenance. Provider/model identity stays beneath model-neutral Application/UI states. Local-model roles remain evidence-selected under ODR-24.

### Windows infrastructure

WinUI, Windows AI/ML/NPU, package APIs, secrets, ordinary settings, power/memory signals, App Actions and MCP remain outside Core and outside Production causal authority.

---

## 4. H1 Phase A closure baseline

The deterministic spine closed through Patch 0018:

```text
Fixture/domain
 -> Access
 -> Context
 -> Performer Candidate
 -> Director Opportunity
 -> Integrity
 -> State Interpreter
 -> State Authority
 -> Take
 -> atomic causal commit
 -> effective Opportunity
 -> Production-bound Context
 -> accepted Performance history
 -> synchronized causal cycle
 -> provider-neutral Performer attempt
 -> deterministic Turn orchestration
```

End-of-H1 convergence result is recorded in `docs/evidence/H1_CONVERGENCE_AUDIT.md`.

Director-machine native Windows ARM64 authority for Patch 0018 observed:

- ARM64 / `win-arm64` / SDK 9.0.317;
- clean tracked/staged tree;
- Core and Core.Tests compile PASS;
- **622/622 Core tests PASS**;
- Harness ARM64 build PASS;
- Missing Raft PASS;
- generic smoke PASS.

Phase A exit was satisfied: one canonical deterministic path reaches a valid postcommit state and then an explicit synchronized next Opportunity-bearing state; technical/cancel/retry/review paths cannot become fiction; Core requires no provider SDK; convergence found no justified executable deletion.

At H1 closure, the following later-program proofs remained outside that closure boundary:

- real provider execution and request/attempt/result provenance;
- deterministic run-level retry/spend/cancellation orchestration;
- immutable E0 run evidence and blind-review packaging;
- E0-A through E0-G behavioral evidence;
- durable cross-Scene Production persistence/recovery;
- launch observation/Scene lifecycle decisions;
- WinUI runtime;
- Windows AI/NPU execution;
- MSIX/WACK/Store certification.

---

## 5. Sequential proof path to product-runtime architecture

### A — Close H1 deterministic spine — **CLOSED**

Authority: Patch 0018 + `H1_CONVERGENCE_AUDIT.md`.

No further H1 deterministic patch is justified unless E0-A falsifies an existing seam.

### B — Complete the E0-A experimental Harness

Goal: the smallest real system capable of frozen Same-Model Isolated Cast.

Add only what E0-A evidence requires:

- official reference provider/model adapter at the infrastructure edge;
- provider-neutral request/attempt/result orchestration contracts where needed;
- exact provider/model/settings provenance;
- exact Character Context capture and disclosure attribution;
- provisional streaming/partial output separated from fictional history;
- refusal/error/cancellation handling;
- deterministic retry/spend authority;
- run driver and RunId-attributable immutable evidence;
- accepted/rejected/partial diagnostic separation;
- blind-review transcript/evaluator package.

Exit:

- repeated E0-A runs need no hand-edited state between Turns;
- technical failure never becomes fiction;
- every accepted Performance/commit is attributable to exact run/attempt/context/provider evidence;
- native ARM64 Harness/runtime path is exercised;
- end-of-E0-A convergence/deletion audit passes.

Provider/model choice, credentials, actual spend, and network execution remain explicit experimental/runtime authority decisions; they do not enter Core.

### C — Run E0-B through E0-G and converge

Frozen sequence:

1. E0-A same-model isolated cast;
2. E0-B mixed-model cast;
3. E0-C repeated identical-condition runs;
4. E0-D ablations;
5. E0-E single-model playwright control;
6. E0-F integrity/failure injection;
7. E0-G substantially different generalization fixture.

Convergence decides which mechanisms earn productization. Hard-gate failures are repaired before artistic evidence counts. A strong simpler-control result may force simplification.

Exit: contributing runs pass integrity gates; generalization succeeds or falsifies/revises architecture; broad deletion audit completes; Director approves post-E0 architecture.

### D — Freeze post-E0 product-runtime architecture

Resolve only launch-critical questions, including minimum Scene lifecycle/observation semantics, creator consequence review, branching/Another Take/Rehearsal minimum if any, portable Production format, persistence/recovery ports, provider casting/cost policy, Application commands/queries, minimum Studio/Stage/Archive semantics, Presentation Perspective, launch-required creator postures, content/governance requirements, and evidence-selected local semantic tasks.

Do not force every ODR item into V1.

Framework support remains a separate Director decision. Current validated baseline stays .NET 9 until explicitly changed; no ordinary patch may silently retarget it.

Exit: approved product-runtime architecture and stable-enough Application/persistence/provider/presentation contracts for parallel productization.

---

## 6. Parallel productization after Phase D

### P1 — Durable Production persistence and recovery

Required: append-only causal authority, atomic persistence, deterministic replay/projection rebuild, crash consistency, corruption detection, recovery, schema/version policy, rebuildable snapshots, portable export separate from credentials, and target-device close/reopen/recovery evidence.

Legacy settings storage never substitutes for Production persistence.

### P2 — Native Windows application baseline

Required: native ARM64 WinUI shell, strict MVVM, packaged identity when practical, semantic model-neutral view state, real Application contracts, and no provider/native/package/persistence machinery in ViewModels.

Test adapters may scaffold unfinished ports but cannot satisfy final lane validation.

### P3 — Production provider runtime + evidence-selected local capabilities

Reference provider requirements: official supported APIs, protected local credentials, bounded Character Context, provisional streaming, attempt/retry/cancel/spend provenance, deterministic budgets, and explicit understudy boundaries.

Local Windows/NPU tasks remain capability-driven and evidence-selected. No model name or specific API becomes Core/Application authority. Actual APIs, readiness states, execution providers, and hardware paths must be verified when implemented. NPU/TOPS claims require target-device evidence.

### P4 — UI/design implementation convergence

Drive remains canonical for visual exploration. GitHub receives approved semantic implementation requirements and shipping assets.

Frozen product spaces remain:

- **Studio** — what could happen;
- **Stage** — what happens;
- **Archive** — what happened, changed, and remains in motion.

UI binds to semantic Application state, not hashes, provider internals, model names, or NPU marketing states. Accessibility criteria are exercised on the real shell before Alpha exit.

---

## 7. Runtime-complete Alpha

Minimum integrated flow:

```text
Create/open Production
 -> establish Cast/Scene
 -> cast Performer backend
 -> run multi-turn Scene
 -> creator/authority Take handling
 -> commit causal consequences
 -> advance Opportunity
 -> persist
 -> close/reopen
 -> reconstruct exact history/state
 -> inspect Archive
```

Alpha also exercises provider failure/cancellation, capability unavailability, credential absence/revocation, persistence interruption/corruption, lifecycle/suspend-resume, resource pressure/Energy Saver, accessibility basics, and low-idle behavior.

Security-sensitive diffs receive targeted review earlier; a standard repository security scan occurs at runtime-complete Alpha. Alpha convergence must actively remove obsolete E0/product scaffolding.

## 8. Beta / release architecture gate

Conditions include no unresolved P0/P1 corruption/privacy/authority/security/crash defects, stable persistence/recovery, stable creator-access boundaries, accessibility-ready launch UX, deterministic provider/capability fallback, no unsupported/private launch API dependency, fixed package/servicing model, fixed diagnostics policy, and claims matching evidence.

Recheck the framework decision against the actual RC date. Beta/release architecture convergence audit is mandatory.

---

## 9. Windows extensibility

### App Actions

Candidate only after stable Application commands and package identity. Expose creator-meaningful commands, not Core/provider internals. Old Workspace action names are not reusable semantics.

### MCP

Optional/non-blocking unless later platform maturity and product value explicitly promote it. Keep disclosure minimal, isolate credentials/event-store authority, and prefer read-only first.

---

## 10. Packaging, WACK, and Store RC

Release engineering must prove the exact candidate rather than a nearby development build:

- Release ARM64 build;
- MSIX identity/versioning/signing;
- deployment/runtime choice;
- manifest/capability review;
- ARM64/native payload purity inspection;
- supported package/identity/signature/version validation appropriate to **our own exact candidate**;
- clean install/upgrade/uninstall/reinstall;
- Production/settings retention-policy checks;
- included extensibility tests;
- WACK on the exact candidate;
- privacy/listing/accessibility evidence;
- deterministic dependency/version/license inventory;
- dependency vulnerability/security review and no embedded credentials;
- symbol/crash-diagnostic policy;
- final security scan;
- final runtime smoke.

**`IPackageValidator` is not a mandatory first-release gate.** It is an externally staged-package validation mechanism and is not adopted merely to self-validate Kymaean's own candidate. It may be reconsidered only if a concrete future external-package validation use case earns it and the supported API is verified then.

Exit: immutable Store RC commit/package, exact dependency/package provenance, final security result, WACK PASS, clean target-device install/runtime smoke, and Director submission approval.

WACK is package authority, not Store certification. Partner Center remains final Store authority.

---

## 11. Legacy donor quarantine

Before donor code enters Kymaean:

1. design the current capability first;
2. identify whether donor code solves part of that approved problem;
3. audit correctness, concurrency, privacy, battery/resource cost, ARM64, API currency, Store safety, dependency direction;
4. prefer concept extraction/clean reimplementation over blind copy;
5. add Ensemble-native tests;
6. validate at the correct authority level;
7. record donor provenance;
8. never create compatibility obligations to donor behavior.

Eligible donor concepts include circuit breakers, bounded non-authoritative caches, resource-pressure lessons, secret-store hardening, ARM64/package inspection, and verification patterns only in the phase that earns them. Production persistence is designed fresh. The old Workspace/Routines/wallpaper domain and private/unsupported shell mechanisms remain rejected.

---

## 12. Sol High task-scope law

`docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` governs substantive engineering work.

Default shape:

```text
current authority
 -> one unresolved objective
 -> required source/research synthesis
 -> falsification
 -> implementation only if authorized
 -> targeted verification
 -> recursive audit
 -> durable checkpoint when warranted
 -> stop at next consequential Director/machine/security/WACK/Store gate
```

Complete already-authorized coupled work before returning; do not fragment work for low-value approvals.

## 13. Validation ladder

| Milestone | Authority required |
|---|---|
| architecture | recursive static/adversarial audit + Director approval |
| patch implementation | static audit + native ARM64 compile/tests for affected surface |
| E0 behavioral result | real provider/runtime experiment with frozen provenance |
| persistence | target-device close/reopen/recovery |
| WinUI baseline | native target-device launch/use |
| Windows AI | target-device capability/model execution evidence |
| NPU | actual hardware/NPU evidence |
| Alpha | integrated target-device runtime matrix |
| security | targeted sensitive-diff review + Alpha/final scans |
| package | exact-candidate package validation + clean install |
| WACK | WACK on exact candidate |
| Store | Partner Center certification |

## 14. Main risks

- **Behavioral falsification:** simplify if Ensemble does not outperform or materially differ from simpler controls.
- **Persistence error:** causal persistence requires dedicated authority/recovery design.
- **Platform/model churn:** capability boundaries absorb change; reverify APIs at implementation time.
- **Framework support horizon:** explicit Director decision; no silent retarget.
- **UI/backend mismatch:** semantic Application/view-state contracts precede broad UI integration.
- **Provider variability/cost:** deterministic budgets/provenance/cancellation; provider-neutral UI.
- **NPU overclaim:** hardware execution evidence required.
- **Prerelease/Store risk:** preview APIs do not become launch dependencies without explicit approval.
- **Legacy contamination:** donor quarantine + new tests + provenance.
- **Supply-chain drift:** exact dependency provenance and exact-candidate validation.
- **Overengineering:** abstractions earn themselves; convergence audits actively delete.

---

## 15. Current-action boundary

Do not jump to WinUI, persistence, local AI/NPU, packaging, or Store work while an earlier dependency remains active.

The exact current engineering objective, provider authorization, validation boundary, and next action are read only from `CURRENT_STATE.md` at the active exact ref. This program map intentionally does not duplicate them.

---

## 16. Program-map boundary

This roadmap preserves major dependency ordering:

```text
H1 CLOSED
 -> E0-A Harness
 -> E0-B..G
 -> E0 convergence
 -> post-E0 product-runtime architecture
 -> parallel persistence / Windows shell / provider-local capability / UI lanes
 -> runtime-complete Alpha
 -> hardening + Beta/release architecture
 -> optional extensibility as justified
 -> exact ARM64 MSIX + WACK + security
 -> Store RC
 -> Partner Center
```

It does not freeze future patch numbers, project/assembly names, final UI, provider lineup, local-model roles, NPU workloads, every ODR item, launch date, or any mechanism E0 later proves should change.

No implementation is authorized by this plan alone.
