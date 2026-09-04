# Kymaean Architecture & Ship Plan

Status: **PROGRAM ARCHITECTURE PROPOSAL 0.1 — AUDIT REQUIRED; NOT IMPLEMENTATION AUTHORITY**

Date: 2026-09-04

Authoritative starting `main`: `1238b568565bf704a6e6fff70828730982db4b3e`

Purpose: define the dependency-ordered engineering path from the current Ensemble E0 deterministic spine to a native ARM64 Kymaean Microsoft Store release without turning legacy product inspiration, exploratory branches, UI work, provider APIs, or release tooling into accidental domain authority.

This document is a program map. It does **not** override Blueprint 0.1, approved patch blueprints, `CURRENT_STATE.md`, the Engineering Hygiene Constitution, machine validation, Director approval gates, WACK, or Partner Center certification.

---

## 1. Product identity and authority

Kymaean is the shipping product identity of the current Ensemble application.

The current product thesis remains the frozen Ensemble thesis:

> A local-first generative theater and creative simulation for persistent characters. The creator establishes people, circumstances, knowledge, relationships, possibilities, and pressures. AI performers portray those characters. A Director manages attention and opportunities to act. Accepted performances become causal history, and authoritative consequences alter what future scenes can mean.

The current project is **not** the shelved DeskShifter/Kymaean Workspace/Routines/wallpaper application. The Drive file `Kymaean_Feature_Roadmap_and_Locked_Decisions_2026-08-20` and `Rylascoo/Kymaean-Project` are legacy donor/reference material only. Their Windows, ARM64, AI, privacy, packaging, and validation lessons may be reused selectively; their Workspace/Routines/wallpaper product model is not part of this product unless separately reintroduced by explicit Director decision.

Authority order remains:

1. frozen Ensemble Blueprint and approved phase specifications;
2. `CURRENT_STATE.md`;
3. current GitHub source/tests/evidence;
4. project authority/hygiene/reasoning protocols;
5. Google Drive design/research material;
6. archived DeskShifter/Kymaean donor code as immutable reference only.

---

## 2. Non-negotiable architecture laws

The ship plan preserves these laws through every later layer:

1. **Creator authority:** models propose; deterministic authority commits.
2. **Character != Performer:** model/provider replacement never replaces Character identity/history.
3. **Access before relevance:** prohibited information is removed before any semantic/context-selection system can see it.
4. **Claims are not facts:** truth, possibility, observation, claim, belief, suspicion, memory, and provenance remain distinct.
5. **Accepted Performance is immutable causal history.**
6. **Performance + approved consequence commit atomically.**
7. **Infrastructure stays at the edges:** Core does not depend on WinUI, provider SDKs, Windows AI, QNN, filesystem, package, or Store machinery.
8. **Local sovereignty:** complete Production authority and credentials remain locally controlled; remote providers receive temporary bounded packets only.
9. **Graceful degradation preserves correctness:** provider/model/NPU loss can reduce quality or automation, never corrupt canon or invent fictional behavior.
10. **ARM64 native retail path:** no x86 native dependency or emulation requirement.
11. **Low idle cost:** event-driven work, explicit resource budgets, no hidden high-frequency polling.
12. **Validation levels never inflate:** static < compiler < runtime < hardware/NPU < package/WACK < Partner Center.
13. **UI is a projection, not authority:** Studio/Stage/Archive and Presentation Perspective cannot mutate truth merely by displaying it.
14. **One canonical implementation:** pre-release compatibility debt is not a reason to retain superseded internal paths.

---

## 3. Target logical architecture

Exact project names remain post-E0 implementation details. The intended dependency shape is:

```text
                         KYMAEAN WINUI 3
                    Studio / Stage / Archive
                              |
                              v
                    PRESENTATION / VIEW STATE
                    strict MVVM + commands
                              |
                              v
                       APPLICATION LAYER
              Production/Scene use-case orchestration
            creator commands / run commands / queries
               capability-neutral state machines
                              |
         +--------------------+--------------------+
         |                    |                    |
         v                    v                    v
  DETERMINISTIC CORE     PERSISTENCE PORTS      AI/PROVIDER PORTS
  Character / World      causal event store     performer backend
  Knowledge / Pressure   projections/snapshot   local semantic work
  Access / Context       recovery / branches    provenance / budgets
  Director / Integrity   portable Production    capability/readiness
  Interpreter / State
  Take / Causal Commit
         ^                    ^                    ^
         |                    |                    |
         +--------------------+--------------------+
                              |
                              v
                    INFRASTRUCTURE ADAPTERS
          Windows storage / secrets / provider APIs
         Windows AI Foundry / Windows ML / QNN lane
         network providers / diagnostics / package APIs
                              |
                              v
                       WINDOWS HOST EDGE
      lifecycle / power / memory / activation / package identity
          App Actions / optional MCP / notifications / MSIX
```

### 3.1 Core

Core owns fictional and causal authority only. It should remain platform-neutral and deterministic wherever practical.

### 3.2 Application layer

The Application layer coordinates approved Core authorities into user-visible use cases. It may own Scene/run state machines, creator commands, provider-attempt sequencing, cancellation boundaries, and persistence transactions, but it does not redefine Core truth semantics.

### 3.3 Persistence ports

Persistence stores causal authority and reconstructable projections. Settings/configuration storage is a separate concern from Production causal persistence.

### 3.4 AI/provider ports

Provider interfaces produce proposals/attempts and provenance. They never mutate Production directly. Backend identity remains below model-neutral application/UI states.

### 3.5 Windows infrastructure

Windows App SDK, WinUI, Windows AI, Windows ML/QNN, package APIs, credentials, power/memory signals, App Actions and MCP live outside Core.

---

## 4. Current implemented position

As of authoritative `main` `1238b568...`:

- E0 fixture/domain foundations exist;
- deterministic Access Control exists;
- deterministic Context composition exists;
- Performer Candidate contract exists;
- deterministic Director Opportunity exists;
- Integrity validation exists;
- State Interpreter proposal contract exists;
- deterministic State Authority exists;
- immutable Take semantics exist;
- atomic causal commit exists;
- effective Opportunity authority exists;
- Production-bound Context continuity exists;
- accepted Character-legible Performance history continuity exists;
- native ARM64 Core test authority for Patch 0015 is `571/571` PASS;
- native Harness build and both current fixture validations have passed for the unchanged executable source lineage.

Not yet established:

- complete E0 run/Scene orchestration;
- real provider/model execution;
- provider attempt/retry/spend/streaming provenance;
- E0-A through E0-G experimental completion;
- durable cross-Scene Production persistence/recovery;
- final observation/world-resolution semantics;
- WinUI product runtime;
- Windows AI Foundry or NPU execution;
- MSIX/WACK/Store authority.

---

## 5. Delivery phases and hard gates

### PHASE A — H1 deterministic-spine closure

Goal: finish only the deterministic seams required for a genuine E0 run driver, then perform the first broad convergence/deletion audit required by the Engineering Hygiene Constitution.

Likely work boundaries, subject to individual approved blueprints:

- deterministic run/turn orchestration state machine;
- exact ownership of current Production/history/Opportunity synchronization across one full live cycle;
- provider-attempt/result boundary as data/authority contract where needed, without provider SDK coupling;
- attempt/provenance identifiers needed to distinguish technical attempts from fictional history;
- cancellation/failure semantics that cannot become fictional action;
- any remaining E0 reference-run invariants.

Exit gate:

- one canonical deterministic path from current opportunity-bearing Production state through accepted/rejected attempt handling to the next synchronized opportunity-bearing state;
- no provider SDK required to validate the deterministic state machine;
- native ARM64 Core suite passes;
- end-of-H1 convergence audit finds no material correction or worthwhile in-scope simplification.

**Do not** expand this phase into persistence, WinUI, Windows AI, general observation, World Resolver, or Store work.

---

### PHASE B — E0-A harness completion

Goal: turn the deterministic engine into the smallest real experimental system capable of the Same-Model Isolated Cast test.

Add only experimental/infrastructure capability required by Blueprint 0.1:

- official provider adapter boundary for the reference frontier model;
- provider request/attempt/result provenance;
- streaming as provisional output only;
- cancellation/refusal/error handling;
- deterministic retry/spend limits;
- E0 run driver;
- immutable run evidence bundle;
- Character Context capture;
- accepted/rejected/partial diagnostic separation;
- blind-review transcript package generation.

The harness is still a developer experiment, not the product UI.

Exit gate:

- E0-A can run repeatedly from the frozen fixture without hand-editing state between turns;
- technical failure never enters fiction;
- complete experimental provenance can explain every accepted Performance and committed consequence;
- native ARM64 harness/runtime path is exercised;
- end-of-E0-A harness convergence audit passes.

---

### PHASE C — E0-B through E0-G and convergence

Goal: determine whether Ensemble's behavioral architecture earns its complexity before productizing it.

Required sequence remains frozen:

1. E0-A — same-model isolated cast;
2. E0-B — mixed-model cast;
3. E0-C — repeated identical-condition runs;
4. E0-D — ablations;
5. E0-E — single-model playwright control;
6. E0-F — integrity/failure injection;
7. E0-G — substantially different generalization fixture.

E0 convergence must answer:

- Which mechanisms materially improve Character distinction/social causality/agency?
- Which mechanisms are unnecessary and should be deleted?
- Which hard integrity failures exist?
- Does the playwright control make Ensemble's extra architecture unjustified?
- What E0 evidence reopens any frozen Blueprint provision?

Exit gate:

- hard integrity gates pass for contributing runs;
- generalization fixture passes the required behavioral test or explicitly falsifies/revises the architecture;
- one broad cleanup/deletion audit completes;
- Director approves the post-E0 architecture before product-runtime work begins.

**This is the main architectural pivot gate of the entire project.**

---

### PHASE D — Post-E0 product architecture freeze

Goal: convert successful behavioral architecture into a durable application architecture without prematurely solving every Open Design Register item.

Resolve only launch-critical post-E0 questions, including:

- minimum Scene lifecycle and ending semantics;
- minimum observation eligibility needed beyond the E0 co-present rule;
- consequence review/creator-authority mode required for first release;
- minimum branch/Another Take/Rehearsal semantics, if any, for launch;
- Production portable format and schema/version policy;
- causal persistence and crash-recovery contract;
- provider casting/understudy/cost policy needed for launch;
- required creator postures for first release (Watch/Direct/Perform/Write);
- minimum Studio, Stage, and Archive functional contracts;
- content/governance requirements before Alpha;
- presentation-perspective requirements needed for launch UX.

Do not force every ODR item into V1. Unresolved non-blockers stay deferred by name.

Exit gate:

- approved product-runtime blueprint;
- dependency graph and project boundaries frozen enough for implementation;
- no UI layout frozen by backend architecture beyond necessary view-state contracts.

---

### PHASE E — Durable Production persistence and recovery

Goal: make Production a real persistent creative object rather than an in-memory E0 state machine.

Required properties:

- append-only causal event authority;
- atomic persistence of accepted Take + approved consequences;
- deterministic projection/rebuild;
- crash-consistent transaction boundary;
- corruption detection and fail-closed recovery;
- explicit Production identity and schema version;
- migration strategy without imaginary pre-release compatibility debt;
- snapshot/projection caches treated as rebuildable acceleration, not truth;
- branch/history identity if launch scope requires it;
- portable export/import separated from credentials/secrets;
- diagnostics deletable independently from Production truth;
- tests for interrupted write, truncated/corrupt data, stale projection, replay, migration, and recovery.

Legacy donor rule:

- old `SettingsStore` atomic-write ideas may inform ordinary settings/preferences;
- they are **not** a Production event-store design and must not be transplanted as one.

Exit gate:

- create -> commit multiple Turns -> close process -> reopen -> reconstruct exact authority;
- corruption/failure cases fail safely;
- native ARM64 runtime exercises recovery.

---

### PHASE F — Provider runtime, cost governance, and local-AI capability layer

Goal: productionize model execution behind stable model-neutral contracts.

#### Frontier Performer lane

- official provider APIs only;
- locally protected credentials;
- bounded Character Context payloads;
- streaming provisional until acceptance;
- request/attempt/retry/cancel provenance;
- deterministic spend/retry authority;
- provider-specific reasoning/performance settings mapped honestly;
- understudy substitution at explicit Performance boundaries only.

#### Windows managed local-model lane

The application must depend on a capability abstraction, not on a model name.

Current platform note as of 2026-09-04: Microsoft's documentation says Phi Silica is being replaced by Aion Instruct, with testing/rollout beginning in October 2026 and retail replacement targeted for November 2026. Therefore Phi-era work is a transition lane, not domain architecture.

Use model-neutral readiness such as:

```text
Ready
SetupRequired
Unavailable
PausedByPolicy
Backoff
Faulted
```

Map real `AIFeatureReadyState` values and successor-model states at the adapter edge.

#### Windows ML / QNN custom-model lane

- `Microsoft.Windows.AI.MachineLearning` execution-provider discovery;
- Qualcomm QNN/Hexagon NPU path where supported and validated;
- user consent for any required model/provider acquisition;
- event-driven heavy work only;
- CPU/GPU fallback only when explicitly supported by capability policy;
- no NPU/TOPS claim without device evidence.

#### Resource governance

- single-flight where resource contention warrants it;
- circuit breaker/backoff;
- bounded non-authoritative caches;
- host-level Energy Saver policy;
- memory-pressure release after in-flight work reaches a safe boundary;
- no routine forced full-GC policy.

Legacy donor candidates become eligible here only after new architecture approval:

- circuit-breaker logic;
- bounded HMAC-keyed ephemeral cache;
- memory-pressure policy lessons;
- QNN provider-probe patterns;
- secret-store design lessons;
- LoRA quarantine/fallback policy.

Exit gate:

- at least one production Performer backend works end-to-end;
- local Windows-AI capability degrades correctly on every supported readiness state;
- target Snapdragon device evidence distinguishes actual NPU execution from mere API availability;
- provider loss cannot corrupt Production.

---

### PHASE G — First Windows application architecture baseline

Goal: introduce the actual native Windows product shell only after E0 convergence and product-runtime authority are coherent.

Target platform baseline at plan date:

- Windows 11 Copilot+ PC;
- ARM64 native retail path;
- C# / current project-approved .NET 9 baseline;
- WinUI 3;
- Windows App SDK 2.4 stable line unless separately validated servicing changes it;
- packaged/MSIX identity from the beginning of the product shell where practical, because later Windows extensibility depends on package identity.

Logical project separation should preserve:

```text
Core
Application
Persistence contracts/implementation
AI/provider contracts/adapters
Windows infrastructure
WinUI presentation
Package/release verification
Tests
```

Exact assembly count must be justified; do not create one project per concept without a dependency reason.

Strict MVVM law:

```text
View
 -> ViewModel
   -> Application use case / async orchestrator
     -> Core or capability port
       -> infrastructure adapter
         -> typed result/state
           -> dispatcher/view state
```

ViewModels must not own P/Invoke, COM, model objects, provider clients, secrets, package APIs, filesystem persistence, or native pickers.

First Windows-baseline convergence audit is mandatory.

Exit gate:

- packaged native ARM64 app launches on the target machine;
- application shell can load/query a real Production through Application contracts;
- UI can be developed against stable view-state contracts without depending on provider/model identity.

---

### PHASE H — UI/design stream convergence

Google Drive remains canonical for visual/design exploration. GitHub receives only approved implementation requirements and shipping assets.

The engineering/UI integration contract should expose semantic state, not backend mechanics.

Required product spaces remain:

- **Studio** — what could happen;
- **Stage** — what happens;
- **Archive** — what happened, changed, and remains in motion.

Creator postures remain:

- Watch;
- Direct;
- Perform / Take a Seat;
- Write.

The design stream may continue in parallel before this phase. Integration begins when the Windows application baseline exposes the required semantic view states.

Engineering must not reinterpret visual experimentation as product authority. When a Drive design is approved for implementation, its semantic requirements are recorded in GitHub.

Exit gate:

- principal Studio -> Stage -> Archive journey works against real persisted Production data;
- Presentation Perspective cannot leak inaccessible information;
- keyboard, screen-reader, scaling, high-contrast, reduced-motion and focus behavior have explicit acceptance tests.

---

### PHASE I — Runtime-complete Alpha

Definition: the first build that is recognizably the product rather than a harness or shell.

Minimum Alpha flow:

```text
Create/open Production
 -> establish Cast / Scene conditions
 -> select or auto-cast Performer backend
 -> run multi-turn Scene
 -> accept/reject/alternate according to approved authority
 -> commit causal consequences
 -> advance opportunity
 -> persist
 -> close/reopen
 -> reconstruct exact history/state
 -> inspect meaningful Archive state
```

Alpha must also exercise:

- provider failure/cancellation;
- local capability unavailable/not-ready states;
- credential absence/revocation;
- corrupted/interrupted persistence recovery;
- accessibility basics;
- suspend/resume/lifecycle behavior;
- memory pressure;
- Energy Saver behavior;
- no high-frequency idle polling.

Security milestone:

- run the first standard Codex Security repository scan here;
- perform targeted/diff security review earlier for any security-sensitive provider, secret, import, persistence, App Action, or MCP change.

Alpha convergence audit must actively delete obsolete E0/harness/product scaffolding that no longer earns its existence.

---

### PHASE J — Alpha hardening and launch-scope closure

Goal: turn a functioning Alpha into a bounded first-release product.

Close launch-blocking decisions only:

- exact first-release Studio capabilities;
- Stage interaction/posture emphasis informed by E0 and UX evidence;
- minimum Archive navigation;
- provider lineup and onboarding;
- local-AI role;
- cost/budget UX;
- privacy disclosure and controls;
- import/export scope;
- content/governance policy;
- crash/recovery UX;
- diagnostics policy;
- first-run/offline/no-provider experience.

Performance/power evidence on Snapdragon target:

- idle CPU/wake behavior;
- memory stability;
- model acquisition/release;
- NPU/CPU/GPU routing evidence where applicable;
- cancellation latency;
- long Scene stress;
- Energy Saver behavior;
- suspend/resume.

No marketing statement may infer NPU use from model API availability alone.

---

### PHASE K — Beta / release architecture gate

Goal: feature freeze the first Store candidate architecture.

Required conditions:

- no unresolved P0/P1 data-corruption, privacy, authority, security, or crash defects;
- causal persistence/recovery stable;
- creator-access boundaries stable;
- core launch UX complete enough for accessibility review;
- provider/local-AI fallback deterministic;
- no unsupported/private Windows API required for core launch;
- package identity and servicing model fixed;
- telemetry/diagnostics policy fixed;
- all launch feature claims correspond to implemented behavior.

#### .NET support-horizon gate

Current Microsoft support policy lists .NET 9 end of support as **2026-11-10** and .NET 10 as active LTS.

The current project mandate remains .NET 9 until the Director approves otherwise. At Beta, explicitly decide:

- if the release and supported servicing window safely precede .NET 9 EOL, retain the validated .NET 9 release line for the first submission; or
- if release/servicing overlaps that horizon, approve a .NET 10 migration and rerun the full compiler/runtime/package/security matrix before RC.

Do not silently retarget the framework during ordinary patch work.

Beta/release architecture convergence audit is mandatory.

---

### PHASE L — Windows extensibility: App Actions and MCP

#### App Actions

App Actions are a good candidate for first release **only after** packaged identity and stable Application commands exist.

Rules:

- expose narrow user-meaningful commands, never internal authority primitives;
- validate invocation through the supported Windows action runtime;
- bounded inputs/outputs;
- explicit creator authority preserved;
- actions call Application use cases, not Core internals or providers directly;
- package identity is a prerequisite.

Possible examples are determined only after launch workflows are frozen; do not reuse old Workspace action names.

#### MCP

MCP remains a later/optional integration unless current Windows stability and Store policy make it low-risk.

Rules:

- separate adapter/process boundary where appropriate;
- minimal context disclosure;
- no direct event-store or credential access;
- no private chain-of-thought exposure;
- explicit command/context façade;
- read-only first is preferred unless a mutation use case clearly earns itself;
- MCP must not block first Store release.

---

### PHASE M — Packaging, WACK, and Store RC

Goal: produce the exact package that can be submitted.

Release engineering includes:

- Release ARM64 build;
- MSIX package identity/versioning;
- signing configuration;
- Windows App SDK runtime/deployment choice;
- package manifest capability review;
- ARM64/native payload purity inspection;
- `IPackageValidator`-based validation where applicable;
- clean install;
- upgrade from prior candidate;
- uninstall/reinstall;
- settings/Production retention policy verification;
- App Actions test if included;
- MCP package registration test only if included;
- WACK;
- privacy policy and Store listing consistency;
- screenshots/assets from the approved design stream;
- accessibility evidence;
- final security scan;
- release notes/support information.

Legacy donor candidates eligible here:

- `ValidatedPackageStager` concept;
- PE architecture inspection rules;
- deep/static verifier test catalogue;
- shallow ARM64 build-output lessons.

WACK passing is package authority only. It is not Partner Center certification.

Exit gate:

- one immutable Store RC commit/package identity;
- final security scan clean or accepted findings documented;
- WACK passes on the exact candidate;
- clean target-device install/runtime smoke passes;
- Director approves submission.

---

### PHASE N — Partner Center submission and certification

Goal: external Microsoft certification of the exact Store candidate.

Partner Center is the final authority for Store acceptance. Any certification rejection becomes a release-blocking patch-first work package; do not generalize a rejected capability into a permanent product redesign until the actual finding is understood.

After certification:

- tag exact release commit;
- preserve Store submission/package evidence;
- update `CURRENT_STATE.md` to released status;
- create post-launch servicing roadmap from real feedback and platform changes.

---

## 6. Critical path vs parallel work

### Engineering critical path

```text
CURRENT PATCH 0015
  -> H1 deterministic closure
  -> E0-A harness
  -> E0-B..G
  -> E0 CONVERGENCE
  -> post-E0 product architecture
  -> persistence/recovery
  -> provider/local-AI runtime
  -> Windows application baseline
  -> Alpha
  -> hardening
  -> Beta
  -> Store RC
  -> Partner Center
```

### Parallel UI/visual stream

```text
Drive visual exploration
  -> Stage/Studio/Archive design systems
  -> interaction/motion/accessibility studies
  -> approved semantic UI requirements
                         |
                         +---- joins engineering at Windows app baseline
```

### Parallel platform research stream

Permitted before implementation when it does not contaminate E0:

- Windows App SDK servicing research;
- Aion/Phi transition research;
- Windows ML/QNN hardware spikes on isolated branches;
- Store/package/App Actions/MCP API verification;
- security/threat research.

Research cannot promote itself into product architecture without the appropriate gate.

---

## 7. Legacy Donor Quarantine Rule

Old `Rylascoo/Kymaean-Project` and archived DeskShifter V7 are donor/reference repositories, never implementation authority.

Before donor code enters Ensemble/Kymaean:

1. define the capability from current architecture first;
2. identify whether donor code solves part of that already-approved problem;
3. audit correctness, concurrency, privacy, resource/battery, ARM64, API currency, Store safety and dependency direction;
4. prefer conceptual extraction or clean reimplementation over blind copy;
5. add Ensemble-native tests;
6. validate on the user's ARM64 target at the correct authority level;
7. record donor repository/commit/blob provenance and what changed;
8. never create compatibility obligations to the donor implementation.

Current donor map:

| Donor area | Disposition | Eligible phase |
|---|---|---|
| Inference circuit breaker | port closely + improve | F |
| Bounded non-authoritative cache | port closely where justified | F |
| Memory-pressure handling | rewrite behind generic resource policy | F/G |
| Energy Saver behavior | elevate to host resource policy | F/G |
| Local secret store | rewrite/harden | E/F |
| Atomic settings writes | settings only; never Production authority | E/G |
| Production persistence | design fresh | E |
| Windows ML/QNN probe | current-API rewrite/hardware spike | F |
| Windows language-model provider | architecture donor only | F |
| LoRA manager | policy donor; model-transition-sensitive | F |
| App Actions router | trust/activation pattern only | L |
| MCP server | isolation concept only | L |
| Package validators | strong donor candidate | M |
| PE ARM64 inspection | extract/improve | M |
| deep/static verifier | verification-pattern donor | G/M |
| WinUI compiler lessons | regression checklist | G |
| `AppServices` monolith | reject | never |
| Shell STA implementation | reject direct reuse | only if a new justified STA need appears |
| private virtual-desktop ABI | reject retail | never in baseline |
| Workspace/Routines/wallpaper domain | unrelated old product; reject | never unless separately reintroduced |

---

## 8. GPT-5.6 Sol High work-package protocol for the remainder of the project

`docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` remains active workflow law.

Every substantive Director turn should be scoped before execution as the **largest logically coupled, falsifiable objective** that can be completed with current authority/tools.

Default work-package shape:

```text
current authority
 -> one unresolved objective
 -> all directly required source/research synthesis
 -> alternatives / dependency reasoning
 -> contradiction and falsification pass
 -> implementation only if authorized
 -> targeted verification
 -> recursive audit until one clean pass
 -> durable checkpoint when materially warranted
 -> stop at next consequential Director/machine/security/WACK/Store gate
```

Sol High is spent on reasoning density, cross-source synthesis, architecture, root cause, edge cases, regression strategy, privacy/accessibility/ARM64 implications, falsification and deletion—not on maximizing prose, token count, file count or unrelated scope.

Simple tasks stay simple.

This protocol should govern **all substantive engineering work through Store release**, including architecture, implementation, compiler/runtime diagnosis, persistence, AI/NPU, WinUI integration, security, performance, package work, WACK preparation, Store remediation, handoffs and recursive audits.

---

## 9. Validation ladder by milestone

| Milestone | Required authority |
|---|---|
| architecture proposal | recursive static/adversarial audit + Director approval |
| patch implementation | static audit + native ARM64 compile/tests for affected surface |
| E0 behavioral claim | actual provider/runtime experiment with frozen provenance |
| persistence claim | target-device close/reopen/recovery execution |
| WinUI baseline | native target-device launch/use |
| local Windows AI claim | target-device readiness/model execution evidence |
| NPU claim | actual hardware/NPU evidence; never API inference alone |
| Alpha | integrated target-device runtime matrix |
| security milestone | Codex Security + targeted manual/architecture review as needed |
| package candidate | exact signed/package validation + clean install |
| WACK | WACK on exact candidate |
| Store release | Partner Center certification of exact submission |

---

## 10. Launch-blocking risk register

### R1 — E0 falsifies the behavioral thesis

Impact: very high.

Response: treat falsification as valuable evidence; simplify or revise before productization rather than compensating with UI polish.

### R2 — Persistence semantics are designed too late or too casually

Impact: very high.

Response: make durable causal persistence its own Sol-High architecture phase before broad product features depend on it.

### R3 — Windows AI model transition

Impact: high.

Response: model-neutral capability interface; treat Phi/Aion as adapters; retrain/revalidate model-specific LoRA; no model name in Core/UI authority.

### R4 — .NET 9 support horizon

Impact: high if schedule approaches 2026-11-10.

Response: explicit Beta decision; no silent retarget; full validation if migration approved.

### R5 — UI and engine semantic mismatch

Impact: high.

Response: integrate through semantic view-state/use-case contracts at the first Windows baseline; do not force UI to understand internal hashes/provider mechanics.

### R6 — provider cost/latency or refusal variability

Impact: medium/high.

Response: deterministic budgets, provenance, cancellation, understudies, local capability lane, provider-neutral UX.

### R7 — NPU expectations exceed supported evidence

Impact: medium/high.

Response: measure actual target-device execution; NPU is an optimization/capability lane, not a truth requirement.

### R8 — Store/platform prerelease surfaces destabilize release

Impact: medium/high.

Response: App Actions only if stable and useful; MCP non-blocking; preview/experimental APIs excluded from core release unless explicitly approved and validated.

### R9 — legacy donor contamination

Impact: medium.

Response: Legacy Donor Quarantine Rule and donor provenance tests.

### R10 — overengineering delays behavioral/product proof

Impact: high.

Response: abstractions must earn themselves; convergence audits actively delete; one objective per Sol-High work package.

---

## 11. Planning estimate

These are engineering planning ranges, not delivery promises. UI/visual work is assumed to continue in parallel.

### Solid product-runtime foundation

Meaning: E0 converged, persistence/recovery coherent, provider/runtime architecture coherent, first Windows application baseline exists, and the UI stream has a durable semantic integration target.

Estimated remaining engineering effort from this checkpoint:

- **~15–23 focused engineering days**.

### Runtime-complete Alpha

Meaning: real persisted Production can run multi-turn Scenes through the actual Windows app with provider/local capability degradation and restart/recovery.

Estimated remaining engineering effort:

- **~20–30 focused engineering days**.

### Store-ready engineering RC

Meaning: launch scope frozen, security/performance/power/accessibility hardening complete, exact ARM64 MSIX validated, WACK passed, Store submission candidate ready.

Estimated remaining engineering effort:

- **~30–42 focused engineering days**.

With UI/visual design proceeding in parallel and current working intensity, a reasonable calendar planning center remains roughly **5–8 weeks**, with platform/model transition, E0 findings, persistence design, and Store validation as the largest uncertainty sources.

Task-scope optimization should reduce conversational fragmentation. A provisional forecast is **roughly 20–35 substantial Sol-High engineering work packages/chats** from this checkpoint to Store RC, rather than hundreds of artificially split architecture replies. Compiler/runtime feedback inside a work package remains patch-first and may require short machine-feedback turns.

Token use is intentionally **not** a project KPI. The optimization target is durable dependency closure per Director turn.

---

## 12. Immediate next work after this plan is approved

Do **not** jump to WinUI, persistence, Aion/Phi, QNN, or Store packaging.

The next engineering action remains:

> identify and blueprint the smallest remaining H1/E0-A deterministic boundary required to reach a complete run driver from current Patch 0015 authority.

Before implementation:

1. resolve current `main`;
2. run the end-of-Patch-0015/H1-boundary scope analysis;
3. inspect any unmerged exploratory branches only as non-authoritative prior reasoning;
4. design the smallest next canonical boundary;
5. recursively audit it to a clean pass;
6. obtain explicit Director approval;
7. implement patch-first;
8. return to native ARM64 validation.

---

## 13. Approval boundary

This Proposal 0.1 is a program architecture/ship-plan candidate only.

Approval of this plan would freeze:

- the dependency ordering of major phases;
- E0 convergence before product architecture;
- persistence before broad product feature dependence;
- model/provider neutrality;
- Windows infrastructure at the edges;
- UI/design parallelization and semantic convergence boundary;
- Legacy Donor Quarantine Rule;
- Sol High optimization for all substantive future work;
- validation/release gate hierarchy;
- App Actions as post-package-identity capability;
- MCP as non-blocking/optional unless later evidence changes that;
- explicit .NET support-horizon gate;
- Store/WACK/Partner Center separation.

Approval would **not** freeze:

- exact future patch numbers;
- exact project/assembly names;
- final UI layout or visual system;
- final provider lineup;
- exact local-model implementation after the Phi/Aion transition;
- exact NPU model workload;
- every Open Design Register decision;
- exact launch date;
- features that E0 evidence later proves should change.

No implementation is authorized by this document alone.
