# Kymaean Architecture & Ship Plan

Status: **PROGRAM ARCHITECTURE PROPOSAL 0.2 — RECURSIVE AUDIT IN PROGRESS; NOT IMPLEMENTATION AUTHORITY**

Date: 2026-09-04

Authoritative starting `main`: `1238b568565bf704a6e6fff70828730982db4b3e`

Purpose: define the dependency-ordered engineering path from the current Ensemble E0 deterministic spine to a native ARM64 Kymaean Microsoft Store release without allowing legacy inspiration, exploratory branches, UI work, provider APIs, or release tooling to become accidental domain authority.

This is a program map. It does **not** override Blueprint 0.1, approved patch blueprints, `CURRENT_STATE.md`, the Engineering Hygiene Constitution, machine validation, Director approval, WACK, or Partner Center.

---

## 1. Product identity and source reconciliation

Kymaean is the shipping identity of the current Ensemble application.

The product remains the frozen Ensemble thesis:

> A local-first generative theater and creative simulation for persistent characters. The creator establishes people, circumstances, knowledge, relationships, possibilities, and pressures. AI performers portray those characters. A Director manages attention and opportunities to act. Accepted performances become causal history, and authoritative consequences alter what future scenes can mean.

The current project is **not** the shelved DeskShifter/Kymaean Workspace/Routines/wallpaper product.

The Drive document `Kymaean_Feature_Roadmap_and_Locked_Decisions_2026-08-20`, `Rylascoo/Kymaean-Project`, and archived DeskShifter V7 are legacy donor/reference material. Their ARM64, Windows, AI, privacy, packaging, project-system, and validation lessons may be reused selectively. Their old product domain must not enter this application without an explicit new product decision.

Authority order remains:

1. frozen Ensemble Blueprint and approved phase specifications;
2. `CURRENT_STATE.md`;
3. current GitHub source/tests/evidence;
4. project authority/hygiene/reasoning protocols;
5. Drive design/research material;
6. donor repositories as immutable reference only.

---

## 2. Architectural laws that survive every later phase

1. **Creator authority:** probabilistic systems propose; deterministic authority commits.
2. **Character != Performer:** recasting never replaces Character identity/history.
3. **Access before relevance:** prohibited information is removed before semantic selection or model input.
4. **Claims are not facts:** truth, possibility, observation, claim, belief, suspicion, memory, and provenance remain distinct.
5. **Accepted Performance is immutable causal history.**
6. **Accepted Performance + approved consequence commit atomically.**
7. **Infrastructure stays at the edges:** Core does not depend on WinUI, provider SDKs, Windows AI, QNN, filesystem, package, or Store machinery.
8. **Local sovereignty:** Production authority and credentials remain locally controlled; remote providers receive temporary bounded packets only.
9. **Graceful degradation preserves correctness:** provider/model/NPU failure may reduce quality or automation, never corrupt canon or invent fictional action.
10. **ARM64-native retail path:** no x86 native dependency or emulation requirement.
11. **Low idle cost:** event-driven work, bounded resources, no hidden high-frequency polling.
12. **Validation levels never inflate:** static < compiler < runtime < hardware/NPU < package/WACK < Partner Center.
13. **UI is projection, not authority:** Studio/Stage/Archive and Presentation Perspective cannot alter truth by displaying it.
14. **One canonical implementation:** before release, obsolete internal paths are removed rather than preserved as imaginary compatibility debt.

---

## 3. Target logical architecture

Exact project names and assembly count remain post-E0 implementation decisions. The intended dependency direction is:

```text
                         KYMAEAN WINUI 3
                    Studio / Stage / Archive
                              |
                              v
                    PRESENTATION / VIEW STATE
                       strict MVVM layer
                              |
                              v
                       APPLICATION LAYER
          Production/Scene use cases + creator commands
             capability-neutral orchestration
                              |
          +-------------------+-------------------+
          |                   |                   |
          v                   v                   v
 DETERMINISTIC CORE      PERSISTENCE PORTS     AI/PROVIDER PORTS
 Character / World       causal event store    Performer backend
 Knowledge / Pressure    replay/projections    local semantic work
 Access / Context        recovery / branches   provenance/budgets
 Director / Integrity    portable Production   readiness/health
 Interpreter / State
 Take / Causal Commit
          ^                   ^                   ^
          |                   |                   |
          +-------------------+-------------------+
                              |
                              v
                    INFRASTRUCTURE ADAPTERS
        Windows storage/secrets/provider/network adapters
      Windows AI Foundry / Windows ML / Qualcomm QNN lane
          power/memory/diagnostics/package integrations
                              |
                              v
                       WINDOWS HOST EDGE
         lifecycle / activation / package identity / MSIX
            App Actions / optional MCP / notifications
```

### Core

Owns fictional and causal authority only. Deterministic logic remains pure wherever practical.

### Application layer

Coordinates approved Core authorities into use cases. It may own Scene/run state machines, creator commands, provider-attempt sequencing, cancellation boundaries, and persistence transactions, but cannot redefine truth semantics.

### Persistence

Production causal persistence is separate from settings/preferences. Rebuildable projections/snapshots are acceleration, not source of truth.

### AI/provider layer

Providers produce attempts/proposals and provenance. Provider/model identity stays beneath model-neutral Application/UI states.

### Windows infrastructure

WinUI, Windows AI, Windows ML/QNN, package APIs, secrets, power/memory signals, App Actions and MCP remain outside Core.

---

## 4. Current position

At `main` `1238b568...`, the project already has the native-validated deterministic spine through Patch 0015:

- fixture/domain foundation;
- Access Control;
- Context composition;
- Performer Candidate contract;
- Director Opportunity;
- Integrity;
- State Interpreter proposals;
- deterministic State Authority;
- immutable Take semantics;
- atomic causal commit;
- effective Opportunity authority;
- Production-bound Context continuity;
- accepted Character-legible Performance-history continuity.

Patch 0015 native authority includes `571/571` Core tests PASS, plus prior successful Harness build and current fixture validations for unchanged executable source.

Still unproven/unimplemented:

- complete E0 run/Scene orchestration;
- real provider execution and attempt/retry/spend/streaming provenance;
- E0-A through E0-G completion;
- durable cross-Scene Production persistence/recovery;
- launch-level observation/Scene lifecycle decisions;
- WinUI runtime;
- Windows AI/NPU execution;
- MSIX/WACK/Store certification.

---

## 5. Program phases

### A — Close the H1 deterministic spine

Goal: implement only the remaining deterministic seams needed for a genuine E0 run driver, then perform the required end-of-H1 convergence/deletion audit.

Likely blueprint boundaries, each separately approved:

- deterministic run/turn orchestration;
- ownership of synchronized Production/history/Opportunity across one full live cycle;
- technical attempt/result identity needed to distinguish provider activity from fictional history;
- cancellation/failure semantics that cannot become fictional action;
- any remaining deterministic E0 reference-run invariants.

Exit:

- one canonical deterministic path from opportunity-bearing Production through accepted/rejected attempt handling to the next synchronized opportunity-bearing state;
- no provider SDK needed to prove that state machine;
- native ARM64 Core validation passes;
- convergence audit finds no material correction or worthwhile in-scope simplification.

No persistence, WinUI, Windows AI, general observation, World Resolver, or Store scope enters this phase.

---

### B — Complete the E0-A experimental harness

Goal: make the smallest real experimental system capable of Same-Model Isolated Cast.

Add only what Blueprint 0.1 requires:

- official frontier-provider adapter for the reference model;
- request/attempt/result provenance;
- streaming as provisional output only;
- cancellation/refusal/error behavior;
- deterministic retry/spend policy;
- run driver;
- immutable run evidence;
- exact Character Context capture;
- accepted/rejected/partial diagnostic separation;
- blind-review transcript package.

Exit:

- repeated E0-A runs need no hand-edited state between Turns;
- technical failure never becomes fiction;
- provenance explains every accepted Performance/commit;
- native ARM64 harness/runtime path is exercised;
- end-of-E0-A convergence audit passes.

---

### C — Run E0-B through E0-G and perform E0 convergence

Frozen sequence:

1. E0-A same-model isolated cast;
2. E0-B mixed-model cast;
3. E0-C repeated identical-condition runs;
4. E0-D ablations;
5. E0-E single-model playwright control;
6. E0-F integrity/failure injection;
7. E0-G substantially different generalization fixture.

Convergence must determine which mechanisms earn their existence and which should be removed. A strong playwright-control result may force simplification; hard-gate failures must be repaired before artistic evidence counts.

Exit:

- contributing runs pass hard integrity gates;
- generalization passes or explicitly falsifies/revises the design;
- broad cleanup/deletion audit completes;
- Director approves the post-E0 architecture.

**This is the principal architecture pivot gate before productization.**

---

### D — Freeze the post-E0 product-runtime architecture

Resolve only launch-critical Open Design Register questions, including:

- minimum Scene lifecycle/ending semantics;
- minimum observation eligibility beyond the E0 co-present rule;
- consequence review/creator-authority mode for V1;
- branch/Another Take/Rehearsal minimum, if any;
- portable Production format/schema policy;
- causal persistence/recovery contract;
- provider casting/understudy/cost policy;
- which Watch/Direct/Perform/Write capabilities are required at launch;
- minimum Studio/Stage/Archive functional contracts;
- Presentation Perspective requirements for V1;
- content/governance requirements before Alpha.

Do not force every ODR item into V1.

Exit:

- approved product-runtime architecture;
- dependency graph stable enough for implementation;
- UI layout remains owned by the design stream except for required semantic view-state contracts.

---

### E — Implement durable Production persistence/recovery

Production must become a real persistent creative object rather than an in-memory E0 state machine.

Required properties:

- append-only causal authority;
- atomic accepted-Take + approved-consequence persistence;
- deterministic replay/projection rebuild;
- crash-consistent transaction boundary;
- corruption detection and fail-closed recovery;
- Production identity/schema version;
- migration policy without pre-release legacy clutter;
- snapshots/projection caches rebuildable from authority;
- branch identity only if launch scope earns it;
- portable export separated from credentials/secrets;
- diagnostics independently deletable;
- interrupted-write, corruption, stale-projection, replay, migration and recovery tests.

Donor boundary:

- legacy atomic settings-write ideas may inform settings/preferences;
- legacy settings storage is **never** a Production event-store design.

Exit:

- multi-Turn Production closes/reopens and reconstructs exact authority;
- corruption/failure paths fail safely;
- target ARM64 runtime exercises recovery.

---

## 6. Parallel productization lanes after persistence

After D/E establish stable Application and persistence contracts, two major lanes should proceed **in parallel** and converge at Alpha.

### F1 — First Windows application architecture baseline

Goal: give the UI/design stream a real semantic integration target early, rather than making UI wait for AI/NPU productionization.

Plan-date baseline:

- Windows 11 Copilot+ PC;
- ARM64-native retail path;
- current project-approved .NET 9 baseline;
- C#;
- WinUI 3;
- Windows App SDK 2.4 stable line unless separately validated servicing changes it;
- packaged/MSIX identity from the product-shell baseline where practical.

Logical separation should preserve Core, Application, Persistence, AI/provider contracts, Windows infrastructure, Presentation, package/release verification, and tests without creating one assembly per noun.

Strict MVVM:

```text
View
 -> ViewModel
   -> Application use case
     -> Core/capability port
       -> infrastructure adapter
         -> typed state/result
           -> dispatcher/view state
```

ViewModels do not own P/Invoke, COM, model objects, provider clients, secrets, package APIs, persistence, or native pickers.

Exit:

- packaged native ARM64 app launches on the target device;
- shell can open/query a real Production through Application contracts;
- semantic view states are provider/model-neutral;
- first Windows-baseline convergence audit passes.

Legacy lessons eligible here: ARM64 project configuration, shallow build outputs, WinRT/AOT-safe MVVM patterns, power namespace/compiler regressions, static XAML/binding verification patterns.

---

### F2 — Production provider/runtime + Windows AI/NPU lane

Goal: productionize model execution behind stable ports while F1 and UI work continue.

#### Frontier Performer backend

- official provider APIs only;
- protected local credentials;
- bounded Character Context;
- provisional streaming until acceptance;
- attempt/retry/cancel/spend provenance;
- deterministic retry/spend authority;
- honest provider-specific reasoning/performance mapping;
- understudy substitution only at explicit Performance boundaries.

#### Windows managed language-model backend

Depend on a **capability**, never a model name.

Microsoft currently documents Phi Silica as transitioning to Aion Instruct, with testing/rollout beginning October 2026 and retail replacement targeted for November 2026. Therefore Phi-specific code is a transition adapter, not Core/Application architecture.

UI/Application readiness should remain model-neutral, e.g.:

```text
Ready
SetupRequired
Unavailable
PausedByPolicy
Backoff
Faulted
```

Map real readiness states at the infrastructure edge, including `AIFeatureReadyState` where applicable.

#### Windows ML / Qualcomm NPU lane

For custom/derived local workloads:

- `Microsoft.Windows.AI.MachineLearning` execution-provider discovery/acquisition/registration;
- QNN execution provider on compatible Snapdragon hardware;
- Qualcomm AI Engine Direct / Neural Processing SDK tooling where it materially improves model conversion, quantization, profiling or direct Hexagon-NPU optimization and remains compatible with Store/release constraints;
- prefer Windows ML/ONNX Runtime + certified QNN EP as the retail interoperability path unless evidence justifies a lower-level direct runtime dependency;
- event-driven heavy work only;
- explicit acquisition consent where needed;
- no NPU/TOPS claim without device evidence.

Microsoft's current Windows ML documentation marks portions of `Microsoft.Windows.AI.MachineLearning` as prerelease-sensitive even while QNN is listed as an available Windows ML 2.x execution provider. Every actual API used must therefore be reverified at implementation time.

#### Resource policy

- single-flight where contention warrants it;
- circuit breaker/backoff;
- bounded non-authoritative caches;
- host-level Energy Saver policy;
- memory-pressure pause/release at safe boundaries;
- no routine forced full-GC policy.

Eligible donor concepts here: circuit breaker, bounded HMAC-keyed cache, memory-pressure lessons, QNN probe patterns, secret-store hardening lessons, LoRA quarantine/fallback policy.

Exit:

- at least one production Performer backend works end-to-end;
- Windows-managed local capability degrades correctly across supported readiness states;
- provider loss cannot corrupt Production;
- target Snapdragon tests distinguish API availability from actual NPU execution.

---

### F3 — UI/design convergence lane

Drive remains canonical for visual/design exploration; GitHub receives approved semantic implementation requirements and shipping assets.

Frozen product spaces:

- **Studio** — what could happen;
- **Stage** — what happens;
- **Archive** — what happened, changed, and remains in motion.

Creator postures remain Watch, Direct, Perform/Take a Seat, and Write, with launch emphasis determined post-E0.

The UI should bind to semantic application state, not hashes, provider internals, model names, or NPU marketing states.

Exit before Alpha:

- principal Studio -> Stage -> Archive journey runs against real persisted Production data;
- Presentation Perspective cannot leak inaccessible information;
- keyboard, screen-reader, scaling, high-contrast, reduced-motion and focus acceptance criteria exist.

---

### Parallel-lane convergence

```text
                 E0 CONVERGENCE
                       |
                       v
           Product-runtime architecture
                       |
                       v
              Persistence / Application
                       |
           +-----------+-----------+
           |           |           |
           v           v           v
      WinUI shell   AI/NPU      UI/design
        F1            F2           F3
           \           |           /
            \          |          /
             +---------+---------+
                       |
                       v
              RUNTIME-COMPLETE ALPHA
```

This is intentionally different from a fully serialized plan. It lets your UI/visual stream join as early as technically responsible without letting presentation drive domain authority.

---

### G — Runtime-complete Alpha

Definition: first build recognizably functioning as Kymaean rather than harness/shell.

Minimum Alpha flow:

```text
Create/open Production
 -> establish Cast/Scene conditions
 -> cast Performer backend
 -> run multi-turn Scene
 -> accept/reject/alternate under approved authority
 -> commit causal consequences
 -> advance Opportunity
 -> persist
 -> close/reopen
 -> reconstruct exact history/state
 -> inspect meaningful Archive state
```

Alpha also exercises:

- provider refusal/error/cancellation;
- local capability unavailable/not-ready states;
- credential absence/revocation;
- interrupted/corrupt persistence recovery;
- suspend/resume and lifecycle;
- memory pressure and Energy Saver;
- accessibility basics;
- no high-frequency idle polling.

Security milestone:

- standard Codex Security repository scan after this runtime-complete Alpha baseline;
- targeted/diff security review earlier for security-sensitive persistence, secrets, imports, providers, App Actions or MCP changes.

Alpha convergence audit must actively delete obsolete E0/product scaffolding that no longer earns itself.

---

### H — Alpha hardening and launch-scope closure

Close first-release decisions only:

- exact Studio capabilities;
- Stage posture/interaction emphasis informed by E0 + UX evidence;
- minimum Archive navigation;
- provider lineup/onboarding;
- local-AI role;
- cost/budget UX;
- privacy controls/disclosure;
- import/export scope;
- content/governance policy;
- crash/recovery UX;
- diagnostics policy;
- first-run/offline/no-provider experience.

Target Snapdragon evidence should include idle wake/CPU behavior, memory stability, model acquisition/release, actual inference-device evidence where available, cancellation latency, long-Scene stress, Energy Saver, and suspend/resume.

---

### I — Beta / release architecture gate

Conditions:

- no unresolved P0/P1 data-corruption, privacy, authority, security, or crash defects;
- persistence/recovery stable;
- creator-access boundaries stable;
- launch UX ready for accessibility review;
- provider/local-AI fallback deterministic;
- no unsupported/private Windows API required by core launch;
- package/servicing model fixed;
- diagnostics/telemetry policy fixed;
- launch claims match implemented behavior.

#### .NET support-horizon gate

Microsoft currently lists .NET 9 end of support as **2026-11-10** and .NET 10 as active LTS.

The current project mandate remains .NET 9 until the Director explicitly changes it. At Beta:

- retain .NET 9 only if release/servicing timing remains acceptable under the approved product policy; or
- explicitly approve a .NET 10 migration and rerun compiler/runtime/security/package validation before RC.

No silent framework retarget during ordinary patch work.

Beta/release architecture convergence audit is mandatory.

---

### J — Windows extensibility

#### App Actions

Candidate for first release only after stable Application commands and package identity exist.

- expose narrow creator-meaningful commands, never internal authority primitives;
- supported invocation/runtime validation;
- bounded inputs/outputs;
- calls Application use cases, not Core internals/providers directly;
- explicit creator authority preserved.

Old Workspace action names are not reusable product semantics.

#### MCP

Non-blocking/optional for first Store release unless later platform maturity and product value justify promotion.

- separate adapter/process boundary where appropriate;
- minimal disclosure;
- no credential/event-store direct access;
- no private chain-of-thought exposure;
- command/context facade only;
- read-only first preferred unless mutation clearly earns itself.

MCP must not become a release dependency merely because the architecture can support it.

---

### K — Packaging, WACK, and Store RC

Release engineering:

- Release ARM64 build;
- MSIX identity/versioning/signing;
- Windows App SDK deployment choice;
- manifest/capability review;
- ARM64/native payload purity inspection;
- `IPackageValidator`-based validation where applicable;
- clean install/upgrade/uninstall/reinstall;
- Production/settings retention-policy checks;
- App Actions testing if included;
- MCP registration testing only if included;
- WACK;
- privacy policy/listing consistency;
- approved screenshots/assets;
- accessibility evidence;
- final security scan;
- final runtime smoke on exact candidate.

Eligible donor material: validated-package staging concept, PE architecture rules, deep/static verification catalogue, shallow ARM64 build-output lessons.

Exit:

- immutable Store RC commit/package;
- final security scan clean or accepted findings documented;
- WACK passes exact candidate;
- clean target-device install/runtime smoke passes;
- Director approves submission.

WACK remains package authority, not Store certification.

---

### L — Partner Center certification

Partner Center is final Store authority.

Certification rejection becomes a patch-first release work package against the exact finding. After acceptance, tag/preserve exact release evidence and update durable project state before entering post-launch servicing.

---

## 7. Legacy Donor Quarantine Rule

Before donor code enters current Kymaean:

1. design the capability from current architecture first;
2. identify whether donor code solves part of that approved problem;
3. audit correctness, concurrency, privacy, battery/resource cost, ARM64, API currency, Store safety, dependency direction;
4. prefer concept extraction/clean reimplementation over blind copy;
5. add Ensemble-native tests;
6. validate at the correct machine/external authority level;
7. record donor repo/commit/blob provenance and changes;
8. never create compatibility obligations to donor behavior.

Current donor disposition:

| Donor area | Disposition | Eligible phase |
|---|---|---|
| inference circuit breaker | port closely + improve | F2 |
| bounded non-authoritative cache | port closely if justified | F2 |
| memory-pressure handling | rewrite behind generic resource policy | F1/F2 |
| Energy Saver behavior | host-level resource policy | F1/F2 |
| local secret store | rewrite/harden | E/F2 |
| atomic settings writes | settings only | E/F1 |
| Production persistence | design fresh | E |
| Windows ML/QNN probe | current-API rewrite/hardware spike | F2 |
| Windows language-model provider | architecture donor only | F2 |
| LoRA manager | policy donor; transition-sensitive | F2 |
| App Actions router | trust/activation pattern only | J |
| MCP server | isolation concept only | J |
| package validators | strong donor candidate | K |
| PE ARM64 inspection | extract/improve | K |
| deep/static verifier | verification-pattern donor | F1/K |
| WinUI compiler lessons | regression checklist | F1 |
| `AppServices` monolith | reject | never |
| Shell STA implementation | reject direct reuse | only if a new justified STA requirement appears |
| private virtual-desktop ABI | reject retail | never in baseline |
| Workspace/Routines/wallpaper domain | unrelated old product | never unless explicitly reintroduced |

---

## 8. Sol High task-scope law for all remaining engineering

`docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` governs the remainder of the project.

For **every substantive Director turn**, choose the largest logically coupled, falsifiable objective that can be completed with current authority/tools.

Default shape:

```text
current authority
 -> one unresolved objective
 -> required source/research synthesis
 -> alternatives/dependency reasoning
 -> contradiction/falsification pass
 -> implementation only if authorized
 -> targeted verification
 -> recursive audit to one clean pass
 -> durable checkpoint if materially warranted
 -> stop at next consequential Director/machine/security/WACK/Store gate
```

This applies to architecture, implementation, compiler/runtime diagnosis, persistence, AI/NPU, WinUI, UI-integration contracts, security, performance, packaging, WACK preparation, Store remediation, handoffs and audits.

Sol High is spent on reasoning density and dependency closure, not maximizing prose, code, file count, tokens, or unrelated scope. Simple tasks stay simple.

---

## 9. Validation ladder

| Milestone | Authority required |
|---|---|
| architecture | recursive static/adversarial audit + Director approval |
| patch implementation | static audit + native ARM64 compile/tests for affected surface |
| E0 behavioral result | real provider/runtime experiment with frozen provenance |
| persistence | target-device close/reopen/recovery exercise |
| WinUI baseline | native target-device launch/use |
| Windows AI | target-device readiness/model execution evidence |
| NPU | actual hardware/NPU evidence; never API availability alone |
| Alpha | integrated target-device runtime matrix |
| security | Codex Security at Alpha, security-sensitive diffs, final RC |
| package | exact package validation + clean install |
| WACK | WACK on exact candidate |
| Store | Partner Center certification |

---

## 10. Main risks

### Behavioral falsification

If E0 does not outperform/sufficiently differ from simpler controls, simplify before productization.

### Persistence error

Causal persistence is a dedicated architecture phase; ordinary settings storage cannot substitute for it.

### Windows AI churn

Phi/Aion and Windows ML evolution stay behind adapter/capability boundaries; no model name enters Core authority.

### .NET 9 horizon

Explicit Beta decision before 2026-11-10 support end; no silent retarget.

### UI/backend mismatch

Integrate through semantic Application/view-state contracts as soon as F1 exists.

### Provider variability/cost

Deterministic budgets, provenance, cancellation and understudies; provider-neutral UI.

### NPU overclaim

Measure actual device execution. NPU acceleration is a capability/optimization lane, not fictional authority.

### Prerelease/Store risk

Preview APIs do not become launch dependencies without explicit approval and exact package validation. MCP remains non-blocking.

### Legacy contamination

Donor quarantine + new tests + provenance.

### Overengineering

Abstractions earn themselves; convergence audits actively delete.

---

## 11. Planning range

These are planning ranges, not delivery promises. Your UI/visual stream is assumed to continue in parallel.

### Solid product-runtime foundation

E0 converged, persistence/recovery coherent, product Application contracts approved, first Windows baseline available, UI has a durable semantic integration target.

**~15–23 focused engineering days from this checkpoint.**

### Runtime-complete Alpha

Real persisted Production runs multi-turn Scenes through the actual Windows app with provider/local capability degradation and restart/recovery.

**~20–30 focused engineering days.**

### Store-ready engineering RC

Launch scope frozen; security/performance/power/accessibility hardening complete; exact ARM64 MSIX validated; WACK passed; submission candidate ready.

**~30–42 focused engineering days.**

Because F1/F2/F3 run in parallel and UI/visual design is separate, a reasonable calendar planning center remains **roughly 5–8 weeks**, with E0 findings, persistence, platform/model churn and Store validation as the main uncertainty.

The Sol High protocol should materially reduce conversational fragmentation. A provisional forecast is **~20–35 substantial Sol-High engineering work packages/chats** from this checkpoint to Store RC, plus short machine-feedback turns when native validation is required.

Token use is not a project KPI; durable dependency closure per Director turn is.

---

## 12. Immediate next action after plan approval

Do **not** jump to WinUI, persistence, Aion/Phi, QNN, or Store work.

The next engineering objective remains:

> identify and blueprint the smallest remaining H1/E0-A deterministic boundary required to reach a complete run driver from current Patch 0015 authority.

Work package:

1. resolve current `main`;
2. inspect the exact Patch 0015 boundary;
3. inspect unmerged exploratory branches only as non-authoritative prior reasoning;
4. identify the smallest next canonical boundary;
5. design/falsify/recursively audit it;
6. obtain explicit Director approval;
7. implement patch-first;
8. return to native ARM64 validation.

---

## 13. Approval boundary

Approval of this program plan would freeze only the **major dependency ordering and delivery laws**:

- E0 convergence before product architecture;
- persistence before broad product feature dependence;
- Windows/UI and AI/NPU productization in parallel after stable Application/persistence contracts;
- provider/model neutrality;
- Windows infrastructure at the edges;
- Drive/GitHub design-engineering boundary;
- Legacy Donor Quarantine Rule;
- Sol High optimization for every substantive remaining engineering task;
- validation hierarchy;
- App Actions after stable package identity/Application commands;
- MCP non-blocking unless later evidence promotes it;
- explicit .NET support-horizon decision;
- WACK vs Partner Center authority separation.

Approval would **not** freeze:

- exact future patch numbers;
- exact project/assembly names;
- final UI layout/visual identity;
- final provider lineup;
- exact Phi/Aion implementation;
- exact NPU workloads or direct-vs-Windows-ML QNN choice;
- every Open Design Register item;
- exact launch date;
- any mechanism E0 later proves should change.

No implementation is authorized by this plan alone.
