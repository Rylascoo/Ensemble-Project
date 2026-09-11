# Ensemble Administrator Runtime Specification

Status: **DIRECTOR-APPROVED RUNTIME/COMMISSIONING CONTRACT - IMPLEMENTATION PROCEEDS ONLY THROUGH THE EARNED C0-C13 GATES PLUS THE NARROW C9A REVIEW GATE**

Approved by Director: 2026-09-09
Approval baseline: `Rylascoo/Ensemble-Project` `main@46eb164323542cdf2c405964eb914a3ad02d945f`
Director amendment: 2026-09-11 - C9A read-only Codex-to-Claude orchestration may be commissioned after C9 independently of C10. C10 remains a separate protective-Hook gate; C11 and all general/mutating Automations remain downstream of a valid C10 pass. Durable decision: `docs/evidence/CODEX_ADMINISTRATOR_C9A_READ_ONLY_CLAUDE_ORCHESTRATION_DIRECTOR_AMENDMENT_2026_09_11.md`.

This specification is subordinate to frozen product/phase law, `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, `docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md`, `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`, owning-lane authority, exact source/evidence, and explicit Director decisions. It does not authorize provider traffic, validation promotion, Design adjudication, product/ODR resolution, Hooks, Automations, Claude use, or an external side effect merely by existing.

## 1. Runtime purpose and authority boundary

The Ensemble Project Administrator is an **operational dispatcher**, not a project manager, product authority, engineering authority, design authority, provider authority, validation authority, or independent backlog owner.

Its job is to recover current authority, perform deterministic project mechanics, dispatch bounded work, coordinate shared Git state, collect evidence, and return unresolved decisions to the owning Sol or Director.

```text
DIRECTOR
├── Engineering Sol — Engineering authority
├── Design Sol      — Design authority
└── Ensemble Project Administrator — operational dispatcher
    ├── Scout       — read-only reconnaissance
    ├── Worker      — isolated bounded executor
    └── Reviewer    — read-only falsification
```

Astra is a scarce model escalation, not an agent identity. Claude is a later independent-review plane, not part of the Administrator's authority.

Durable rule: **Parallelize mechanics, not authority.**

No Administrator session is authoritative merely because it is long-lived. Fresh-session recovery from repository truth is normal operation.

## 2. Canonical state versus executable state

Repository-native law remains canonical. This approved Runtime Specification lives in `Rylascoo/Ensemble-Project`.

Machine-local Administrator configuration is a **generated executable realization**, never project authority.

Preferred dedicated Codex home:

```text
C:\Users\Wiryl\.codex-ensemble
```

It must not reuse `C:\Users\Wiryl\.codex` as its runtime home and must not inherit personal Codex memories, personal project trust, broad browser/plugin configuration, or personal model defaults by accident.

The local runtime may cache executable provenance and configuration hashes, but it must never become a second queue, second `CURRENT_STATE`, second validation ledger, or second provider-authority register.

## 3. Exact local layout

Commissioning target:

```text
C:\Users\Wiryl\.codex-ensemble\
├── AGENTS.md
├── config.toml
├── administrator.config.toml
├── agents\
│   ├── scout.toml
│   ├── worker.toml
│   └── reviewer.toml
├── bin\
│   └── ensemble-admin.ps1
├── state\
│   └── runtime-manifest.json
├── locks\
├── scratch\
└── backups\
```

`AGENTS.md` contains only the stable Administrator-role bootstrap: recover repository authority first, obey owning-repository `AGENTS.md`, never create authority, never infer provider/spend permission, and use the dispatch/stop rules defined here.

Authoritative deterministic workflows do **not** live as an unversioned local `skills` collection. Their canonical implementation lives in `Ensemble-Project`, using repository-scoped `.agents/skills/...` wrappers and versioned deterministic scripts under an appropriate repository tooling path. The local runtime may call them but does not own them.

`runtime-manifest.json` contains executable provenance only:

```text
runtime-spec source ref
selected Codex executable path
Codex version
Codex executable hash if practical
config/profile/agent hashes
last successful runtime-census timestamp
```

It carries no queue status, phase status, provider authority, project decision, or validation claim.

## 4. Configuration layering contract

The runtime must verify current Codex precedence before configuration is materialized. The approved design assumption at approval time is:

```text
CLI override
  > trusted project .codex/config.toml
    > selected profile
      > CODEX_HOME/config.toml
        > managed/system/default layers
```

The Administrator launcher therefore performs a preflight before every session:

1. verify `CODEX_HOME`;
2. verify selected executable/version against the runtime manifest;
3. inspect whether the target repository introduced or changed `.codex/config.toml`;
4. fail closed if that layer changes Administrator security/authority assumptions without an approved Runtime Specification update;
5. use `--strict-config`;
6. start from the exact expected Git root, never the `Sol Dev` umbrella.

The launcher does not use `--yolo`, `--dangerously-bypass-approvals-and-sandbox`, `--dangerously-bypass-hook-trust`, or another broad permission override.

Live permission relaxation during an Administrator session is prohibited because runtime permission overrides may propagate into child agents and invalidate role separation.

Volatile Codex behavior must be reverified at C0 and whenever a re-verification trigger in the tooling snapshot applies.

## 5. Base Codex security contract

The dedicated base configuration must establish these semantics:

```text
authentication: ChatGPT subscription only
API-key authentication: prohibited by default
approval policy: on-request
approval reviewer: user
login shell: disabled
shell environment: core inheritance only
automatic KEY / SECRET / TOKEN exclusions: enabled
memories generation: disabled
memories injection: disabled
command network access: disabled by default
live browser/CDP/computer-use: disabled until C8
Hooks: absent/disabled until C10
Automations: absent/disabled until C11
danger-full-access: prohibited
```

`forced_login_method = "chatgpt"` is the commissioning target unless current Codex semantics change before C0.

Provider/runtime credentials are never copied into the Administrator environment. If future Engineering work legitimately requires a provider credential, that credential belongs to the separately authorized runtime process, not to the Administrator.

Native session history may be retained for local troubleshooting, but it is non-authoritative and cannot substitute for repository evidence.

## 6. Administrator permission posture

The Administrator control session uses:

```text
sandbox: read-only
approval: on-request
command network: off
```

The control session may read repositories, logs and evidence within approved filesystem scope. It may not silently edit source or project authority.

Shared Git operations that necessarily cross protected `.git` or network boundaries — fetch, worktree creation/removal, commit, tag, branch lifecycle, push, PR mechanics — are intentionally approval-gated Administrator operations.

This converts a potentially dangerous ambient capability into a visible serialized event.

A single approval request should contain the smallest complete deterministic Git operation rather than forcing the Director through many individual commands.

## 7. Scout, Worker and Reviewer profiles

### Scout

Scout is reconnaissance only.

```text
sandbox = read-only
network command access = off
model class = cheapest qualified read-heavy class
reasoning = medium by default
Git mutation = prohibited
authority-file mutation = prohibited
provider execution = prohibited
```

Scout returns evidence, refs, paths, conflicts and unknowns. It does not create project-law changes.

### Worker

Worker is the only normal source-mutating child role.

```text
sandbox = workspace-write
CWD = one exact isolated worktree
network command access = off by default
reasoning = matched to work-package risk
shared Git metadata = Administrator-owned
cross-worktree writes = prohibited
authority closure = prohibited
provider execution = prohibited unless separately authorized
```

A Worker edits/builds/tests inside its assigned worktree. It does not independently create/remove worktrees, push, merge, retag, dispose of branches, reinterpret `CURRENT_STATE`, or close queue/validation/provider authority.

### Reviewer

Reviewer is falsification only.

```text
sandbox = read-only
reasoning = high for consequential review
source mutation = prohibited
authority mutation = prohibited
provider execution = prohibited
```

Reviewer returns findings to the owning Sol/Administrator. It does not fix while reviewing.

### Permission-inheritance test

C4 must prove on the actual commissioned Codex version that these asymmetric profiles behave as specified.

If native subagent inheritance prevents a read-only Administrator from safely producing a workspace-write Worker while preserving Scout/Reviewer read-only isolation, the runtime must use separate top-level Worker execution contexts. It may not solve the problem by broadening the Administrator control session.

## 8. Concurrency contract

Normal concurrency is **one spawned task**.

Two concurrent tasks are allowed only when the work is genuinely independent and their shared Git/authority surfaces do not intersect.

Three is the hard exceptional ceiling.

Only one scarce/high-cost Sol/Astra-class spawned task may normally run at once.

Shared Git metadata, authority integration, provider authorization, cross-lane adjudication and final promotion remain serialized regardless of available agent slots.

## 9. Repository reconciliation algorithm

Every repository-bound operation begins with deterministic identity recovery.

```text
expected repository identity + expected origin URL
        ↓
live GitHub remote authority
        ↓
local Git root / common Git dir / worktree census
        ↓
approved git fetch --prune when local reconciliation is needed
        ↓
HEAD / branch-or-detached / dirty+untracked / tracking / ahead-behind
        ↓
classification
```

Required classifications:

```text
CURRENT_CLEAN
BEHIND_FAST_FORWARD_CANDIDATE
AHEAD
DIVERGED
DETACHED
DIRTY_OR_UNTRACKED
UNEXPLAINED_WORKTREE
REMOTE_IDENTITY_MISMATCH
```

Only a clean intended tracked branch that is strictly behind may become a deliberate `--ff-only` update candidate.

Never automatically update or repurpose detached validation/evidence checkouts, dirty trees, ahead/diverged branches, unique work branches, unexplained worktrees, or unknown remotes.

The Administrator must recover remote authority before assuming a clean local checkout is current.

## 10. Shared-Git lock

One lock exists per Git common directory for shared metadata operations.

The lock protects:

```text
fetch/prune coordination
branch/tag lifecycle
worktree add/remove/prune
commit/promotion mechanics when shared refs are affected
merge/rebase
push
PR preparation that depends on a stable ref snapshot
```

Worker file editing and compilation inside separate worktrees do not hold the shared-Git lock.

The lock record contains only:

```text
repository/common-git identity
operation
process/session identity
UTC acquisition time
owning dispatch ID
```

Lock acquisition is atomic. Lock contention stops the second operation rather than waiting indefinitely.

A stale lock is never removed merely because it is old. Recovery first proves the owning process is gone, inspects Git's own lock files and worktree state, and classifies whether an interrupted external operation may have occurred. Ambiguity is an `EVIDENCE_FAILURE` or `ENVIRONMENT_FAILURE`, not permission to delete the lock.

## 11. Worktree lifecycle

Standard execution topology:

```text
authoritative baseline
    ↓
Administrator creates isolated worktree
    ↓
Worker edits/builds/tests there
    ↓
owning Sol recursively audits
    ↓
Administrator race-checks origin
    ↓
promotion / PR / merge mechanics
    ↓
evidence closeout
    ↓
archive tag where repository law requires
    ↓
branch/worktree disposal
```

Worktree names include the queue/work-package identity and a short slug.

A worktree receives exactly one bounded work package.

Detached machine-validation/evidence worktrees remain immutable historical checkouts unless the active task explicitly governs them.

A Worker never borrows an existing validation worktree because it happens to contain convenient source.

Branch deletion continues to obey the repository rule requiring preservation by annotated archive tag where applicable.

## 12. Protected project-authority surfaces

The following are manager-owned surfaces by default:

```text
CURRENT_STATE.md
docs/PROJECT_EXECUTION_QUEUE.md
docs/PROJECT_AUTHORITY.md
docs/ENGINEERING_HYGIENE_CONSTITUTION.md
docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md
docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md
docs/VALIDATION_LEDGER.md
provider authorization/closure records
cross-project authority records
Design CURRENT_STATE / Design Ledger
```

Worker profiles may read them.

A Worker may return a **proposed diff** when its work proves one must change, but the owning Sol interprets and adopts that diff.

A deterministic closeout Skill rejects unexplained protected-surface changes before promotion.

The Administrator never marks a validation rung passed because CI passed, never grants provider authority because a credential exists, and never closes an ODR because an agent recommends an answer.

## 13. Dispatch packet

Every consequential routed task carries:

```text
Dispatch ID
queue item / governing authority
source role
target role/surface
repository
exact source ref
target baseline/ref
objective
falsification condition
authority already granted
allowed scope
prohibited scope
required evidence
validation rung allowed to be claimed
external-action authority, if any
stop conditions
return target
model/reasoning class
usage class
review class
```

A missing consequential field is a dispatch defect, not an invitation for the Worker to infer intent.

Task context lives in the packet. Stable behavior lives in the profile. Project truth lives in the repository.

## 14. Review classes

Existing classes remain unchanged:

```text
R0 — mechanical: executor self-check + deterministic validation
R1 — ordinary: executor + owning Sol recursive audit
R2 — consequential: owning Sol + one independent falsification pass
R3 — critical: owning Sol + independent reviewer + optional scarce specialist
R4 — Director: evidence reconciled, then explicit Director decision
```

Reviewer invocation is conditional.

More reviewers are not automatically more reliable; review is added only where independence has positive information value.

## 15. Model and reasoning scheduler

The durable specification uses **capability classes**, not permanent model names.

```text
M0  deterministic script; no model where judgment is unnecessary
M1  economical qualified reconnaissance model
M2  normal manager/worker model
M3  scarce high-capability escalation
```

The C0 runtime manifest records the currently verified model mapping.

Current commissioning intent is:

```text
Routine Administrator interpretation → current qualified Sol-class / Medium
ordinary consequential Worker        → Sol-class / High when complexity warrants
Reviewer                              → Sol-class / High
Astra-class                           → R3 or a specifically falsifiable escalation
```

A lower-cost Scout model may replace Sol only after a bounded qualification test shows that it reliably returns exact refs/evidence without authority errors.

Model failure never automatically selects a more expensive model. Before escalation, the Administrator records what failed and what materially different capability/hypothesis the next attempt tests.

## 16. Usage governance

Usage is not guessed.

The runtime recognizes:

```text
NORMAL
CONSERVE
RESERVE
CRITICAL
UNKNOWN
```

If Codex exposes a reliable machine-readable quota source, C0/C13 may admit it. UI scraping or inference from elapsed time is not an acceptable quota source.

Before enough empirical data exists, `UNKNOWN` routes as `CONSERVE`.

After calibration, capacity is based on observed percentage-point consumption per successful project task, not invented token estimates. The reserve unit is the measured high-percentile cost of one consequential manager/reviewer turn plus two routine Administrator turns.

Operational meaning:

```text
NORMAL   — enough measured reserve for normal routing and justified parallelism
CONSERVE — batch mechanics, prefer M0/M1, normal concurrency 1
RESERVE  — M2 only for consequential work; M3 requires explicit justification
CRITICAL — preserve authority/continuity only; no discretionary expensive work
UNKNOWN  — behave as CONSERVE
```

Banked reset credits are temporary commissioning capacity, never baseline economics.

C13 recalibrates the policy under ordinary subscription limits.

## 17. Failure and retry taxonomy

Every failure is classified before retry:

```text
TASK_FAILURE
STALE_BASELINE
AUTHORITY_FAILURE
TOOL_FAILURE
RESOURCE_FAILURE
ENVIRONMENT_FAILURE
EVIDENCE_FAILURE
CROSS_LANE_BLOCK
EXTERNAL_GATE
```

Rules:

- No blind retry loop.
- One repeat of an idempotent deterministic operation is allowed only when evidence identifies a transient failure.
- A model retry requires a changed prompt hypothesis, evidence set, model capability, or reasoning level.
- `STALE_BASELINE` triggers reconciliation, not patching.
- `AUTHORITY_FAILURE` stops for the owning Sol/Director.
- `RESOURCE_FAILURE` changes scheduling, not project truth.
- `EVIDENCE_FAILURE` preserves partial evidence and stops.
- Provider refusal/error never authorizes another provider invocation.
- Astra escalation is never automatic.
- Retry count is bounded by the work package; absent an explicit value, one corrected attempt is the maximum before returning the failure.

## 18. Credential and external-event firewall

Repository authentication, Codex/ChatGPT authentication, connector authentication and product-provider credentials are separate domains.

The Administrator never copies an auth file from the personal `.codex` home into `.codex-ensemble`.

Dedicated ChatGPT authentication occurs through the supported login flow.

Provider keys are not stored in Administrator config, worktree metadata, dispatch packets, logs, prompts or runtime manifests.

Immediately before an external side effect, the Administrator rechecks applicable authority. External events include:

```text
provider-bound inference
paid API action
push or publication where policy requires a gate
deployment
Store submission
external messaging
irreversible deletion
credential creation/rotation
security-sensitive account changes
```

Preparation of an external action is not authorization to execute it.

## 19. Tool-admission contract

A tool enters the permanent Administrator runtime only when it eliminates a demonstrated project problem.

Expected core surfaces:

```text
GitHub authority/CI connector
system Git + PowerShell
local .NET/Python/Node toolchain where applicable
bounded Remote Desktop manager-plane access
Google Drive only for Design-owned provenance when required
current OpenAI documentation for volatile Codex semantics
```

`gh`, Figma, Vercel, Context7, telemetry platforms and other integrations remain unadmitted until a concrete gap proves their value.

GitHub Desktop remains rejected as synchronization infrastructure.

No permanent Git, Browser, Security or Documentation agent is created; those remain tool/Skill capabilities unless future evidence proves identity separation useful.

## 20. Remote Desktop scope

The stable manager-plane allowlist should be reduced to the smallest Ensemble operational surface:

```text
C:\Users\Wiryl\Sol Dev\Ensemble-Project
C:\Users\Wiryl\Sol Dev\Ensemble-Project-Worktrees
C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence
C:\Users\Wiryl\Sol Dev\Ensemble-Website
C:\Users\Wiryl\Sol Dev\Ensemble-Website-Worktrees
C:\Users\Wiryl\.codex-ensemble
```

The ordinary umbrella `C:\Users\Wiryl\Sol Dev` is not a permanent broad grant.

Temporary access to `C:\Users\Wiryl\.codex` is allowed only during C0 comparison/migration inspection and is removed after the dedicated runtime is established.

Historical/donor and build-history assets are outside the permanent scope unless an authorized task names them.

## 21. Initial deterministic Skills

C3 admits only Skills whose deterministic core is justified.

Required capability set:

```text
authority-recovery
repo-reconciliation
branch-worktree-census
state-distance
protected-diff-check
ci-status
evidence-integrity
commissioning-closeout
```

Before writing a new script, C3 inventories existing repository scripts and wraps them where they already implement the required invariant.

Skills perform deterministic measurement. Models interpret anomalies.

A Skill never modifies `CURRENT_STATE`, the queue, provider authority or validation authority merely because its checks passed.

## 22. Browser/CDP

Browser capability remains off through C7.

C8 creates one dedicated Kymaean development browser profile with no unrelated authenticated sessions.

CDP/browser work is a capability, not a permanent agent.

Browser evidence remains browser evidence and cannot be promoted into WinUI runtime evidence, native ARM64 evidence, WACK evidence, or Store evidence.

Consumer-chat browser automation remains excluded.

## 23. Hooks

Hooks remain disabled/unconfigured until C10.

The first permitted hook is deterministic enforcement, not judgment.

C10's reference pilot is a `PreToolUse` guard that blocks a deliberately forbidden protected-surface or unsafe Git operation in a disposable commissioning fixture.

The hook:

```text
does not rewrite authority files
does not make product decisions
does not trigger provider traffic
fails closed
has a reviewed exact hash
has bounded execution time
produces an explicit blocking reason
```

No hook is run with `--dangerously-bypass-hook-trust`.

Hook changes require renewed trust when current Codex semantics use content-hash trust.

## 24. Automations

General, background, scheduled, polling, self-triggering, and mutation-capable Administrator Automations remain disabled until C11. C11 remains downstream of a valid C10 pass.

C9A is a deliberately narrow exception for **on-demand repository-read-only Claude review orchestration** after C9. It is not a C11 Automation framework and does not authorize scheduling, background execution, mutation, automatic implementation, or automatic action on reviewer findings.

A C9A dispatcher may deterministically build and hash a fixed exact-ref review packet in Administrator scratch, invoke the already-commissioned dedicated Claude review plane once, capture/hash the structured advisory result and runtime telemetry, and return the result to the owning Sol/Administrator. It may not mutate a repository/worktree/Git surface, rewrite authority, invoke product-provider traffic, use browser/CDP, enable Claude tools/MCP/subagents/session persistence, fall back to API-key/cloud-provider billing, or retry automatically.

C11 is an **admission test**, not a mandate to automate something.

A candidate C11 Automation must first exist as a stable manual Skill, show repeated demand, have bounded semantics, and save more human/model effort than it consumes. The initial C11 candidate remains bounded and read-only unless a later explicit Director decision authorizes a stronger class. A C10 pass is necessary but not sufficient for mutation-capable automation.

Passing C11 may legitimately conclude:


```text
NO AUTOMATION ADMITTED
```

Forbidden without separate explicit authority remain automatic merge, automatic provider traffic, automatic unique-branch deletion, automatic authority-file rewriting, automatic cross-lane decisions, and automatic deployment/publication.

## 25. Claude

Claude is not commissioned before C9.

Initial Claude posture:

```text
exact-SHA read-only checkout/worktree
no routine GitHub mutation
no provider runtime credentials
no automatic implementation
no authority writes
findings returned to the owning Sol
```

C9 independently verifies installation, subscription authentication, absence of accidental API-key billing, model availability, usage reporting, filesystem scope and Git behavior before Claude is used as an independent reviewer.

After C9, C9A may commission one deterministic on-demand dispatcher around that already-restricted review plane. The dispatcher must provide only a fixed packet, preserve the dedicated `CLAUDE_CONFIG_DIR`, remove API-key/alternate-cloud credential routes, disable built-in tools, inherited MCP, subagents and session persistence, allow no repository/Git mutation, perform at most one invocation for the packet, and return findings for manager reconciliation without acting on them. C9A writes only bounded scratch packet/result/telemetry evidence outside project worktrees.

MCP, subagents and Claude hooks remain later options, not prerequisites.

## 26. Runtime provenance and rollback

Before each configuration-changing commissioning gate, non-secret Administrator configuration is snapshotted under:

```text
C:\Users\Wiryl\.codex-ensemble\backups\<gate>-<UTC>\
```

Never back up or duplicate authentication secrets into these snapshots.

Rollback restores only the previous non-secret runtime layer.

If runtime integrity becomes uncertain, the preferred recovery is:

```text
preserve current suspect runtime directory
create a clean .codex-ensemble candidate
regenerate from the approved repository specification
authenticate through the supported login flow
re-run C0-required checks
resume only at the highest gate re-proven
```

No project authority depends on recovering Codex session history.

## 27. Commissioning gates

### C0 — Environment / tool / authentication census

Pass requires:

- exact Codex executable provenance resolved;
- version discrepancy resolved or deliberately pinned;
- dedicated `CODEX_HOME` design materialized;
- ChatGPT auth confirmed with no stored API key;
- PATH/launcher behavior deterministic;
- `doctor --json` understood;
- MCP/plugin inventory explained;
- prior MCP warning either reproducible and corrected or proven stale;
- current `rg.exe` warning dispositioned;
- Defender warning observed but **no exclusions added without evidence of actual interference**;
- Remote Desktop scope plan confirmed;
- no provider credentials present.

Approval-time known C0 inputs include a Director-machine Codex executable reporting `0.153.4`, its local update cache reporting `0.154.0` available, Remote Desktop's process environment not resolving `codex` through PATH, and `doctor` reporting missing `rg.exe`. These are volatile observed inputs, not frozen expected values; C0 must reverify them.

### C1 — Repository reconciliation

Run the deterministic reconciler against the real repositories and synthetic state fixtures.

Pass requires exact agreement with manual Git truth for all state classes, preservation of detached validation worktrees, correct remote identity checks and zero automatic overwrite/checkout behavior.

### C2 — Fresh Administrator authority recovery

Start a genuinely fresh Administrator session.

Without chat-history help, it must recover current main/ref, `CURRENT_STATE` currency, queue state, validation authority, provider authority, local detached/root discrepancy, live worktree topology, and latest CI without rung inflation.

It must not ask the Director for facts available in the repository/tools.

### C3 — Deterministic Skills

Each initial Skill must pass deterministic fixtures and manual cross-checks.

No Skill may invent authority from a passing result.

### C4 — Isolated Worker mutation

Commission one harmless R0 work package in a new isolated worktree.

Pass requires Administrator control read-only, Scout read-only, Worker writes only inside its worktree, Reviewer read-only, shared Git metadata Administrator-owned, protected authority surfaces untouched, build/test evidence attributable, and clean disposal possible.

This gate also proves actual subagent permission inheritance. If native spawning cannot preserve the required boundaries, separate top-level Worker execution becomes the commissioned mechanism.

### C5 — Branch / tag / push / PR lifecycle

Use a disposable commissioning branch carrying only commissioning evidence.

Prove fresh origin race-check, serialized push, PR mechanics, no direct `main` mutation, archive-tag discipline, and safe branch/worktree disposal.

### C6 — Cross-lane transport pilot

Pilot GitHub Issues only as an **ephemeral dispatch transport**, never backlog authority.

The packet must reference the single central queue item and exact repository refs.

Pass if the issue transports the packet/result cleanly without creating independent priority/status truth.

If it duplicates planning state or adds friction, reject Issues and select another transport; the architecture is transport-neutral.

### C7 — Real local-evidence pilot

Use the preserved Q-E0A-03 terminal evidence as the preferred read-only specimen **only if doing so does not delay or replace active Engineering work**.

If Engineering has already closed it, use the preserved closed evidence as an oracle or another comparable non-provider evidence task.

Pass requires local hash/evidence analysis with no provider call, no source correction, no validation-rung promotion, no authority mutation, correct failure classification, and useful bounded return to Engineering Sol.

### C8 — Browser/CDP pilot

Create the dedicated Kymaean browser profile and prove origin/session isolation, bounded browser permissions, evidence labeling and safe teardown.

No unrelated authenticated session may be visible.

### C9 - Claude read-only review

Commission Claude under the restricted independent-review posture and prove subscription/billing/auth boundaries plus one exact-SHA bounded review.

### C9A - Read-only Claude orchestration

After C9, commission one deterministic on-demand Codex-to-Claude review dispatcher. Pass requires exact-ref packet provenance and pre-dispatch hash, dedicated first-party Claude subscription authentication with no API-key/alternate-cloud route, tools/MCP/subagents/session persistence disabled, one bounded invocation, structured result/telemetry capture and result hash, before/after proof of zero repository/Git mutation, no product-provider/browser use, no automatic retry, and manager reconciliation with no automatic implementation.

C9A is independent of C10 and does not satisfy it. A C9A pass creates no C11, scheduling, background execution, mutation, authority, provider, validation, or deployment permission.

### C10 - Hook pilot

Admit exactly one deterministic protective hook and prove the expected event fires, a forbidden disposable action is blocked before execution, an allowed action still works, hook change invalidates prior trust where applicable, hook failure fails closed, and no authority rewrite occurs.

### C11 - Automation admission

C11 remains unavailable until C10 passes its fail-closed protective-Hook contract. Measure one stable manual candidate against the economic admission rule.

Either admit one bounded read-only Automation or record `NO AUTOMATION ADMITTED`. A C11 pass does not by itself authorize mutation-capable automation; that requires separate explicit Director authority and applicable protective controls.

### C12 — Disaster recovery / fresh Administrator

Simulate loss of the executable runtime configuration while preserving repository authority.

Rebuild from the approved specification and clean authentication, then reproduce C0/C1/C2 without relying on the old Administrator session.

### C13 — Ordinary-limit sustainability

Evaluate only under ordinary subscription capacity, not temporary banked resets.

Use one full natural usage epoch and real project work; do not manufacture token-burning tasks to satisfy the test.

Pass requires routine mechanics mostly use M0/M1, ordinary work does not require Astra, no required project work is lost to avoidable quota exhaustion, reserve policy remains viable, and usage measurements can calibrate future scheduling.

Until C13 passes, the Administrator is commissioned functionally but its long-term compute-efficiency claim remains provisional.

## 28. Branch-lifecycle condition discovered during design recovery

At the approval baseline, remote recovery found no non-main branch ahead of `main`, but found significant live-ref residue: merged/ancestral refs plus divergent queue/temporary reconciliation histories.

This does not invalidate `main` authority.

It does mean the first repository-hygiene commissioning work must deterministically classify those refs before broad Administrator branch disposal is ever enabled.

No divergent Design/queue ref may be deleted merely because its commit message looks superseded. Existing repository archive-tag law remains binding.

The exact approval-baseline census belongs in the transition evidence record rather than being frozen here as permanent runtime truth.

## 29. Post-approval repository transition

Director approval does **not** mean turn everything on.

The first implementation action is a repository-native continuity package that:

1. materializes this approved Runtime Specification in the authoritative repository;
2. adds/classifies the Administrator commissioning work in the central execution queue;
3. retires the temporary `CODEX_ADMINISTRATOR_INTEGRATION_WORKING_CONTINUITY.md` from active bootstrap use in accordance with its sunset condition;
4. records the exact commissioning baseline;
5. performs the required branch-role reconciliation needed before new Git lifecycle mechanics are exercised.

Only after that repository package is reconciled does C0 machine commissioning begin.

## 30. Frozen prohibitions for the initial runtime

The initial commissioned Administrator may not:

- invent or adjudicate product/ODR authority;
- alter provider/spend authorization;
- make provider calls without a separately explicit authorization;
- promote validation evidence;
- treat CI as native ARM64 evidence;
- write Design authority from Engineering;
- bypass the owning Sol for protected authority changes;
- automatically merge;
- automatically delete unique branches;
- use danger-full-access;
- bypass sandbox/approval/hook trust;
- copy personal Codex auth files;
- use personal Codex memories as project authority;
- run browser/CDP before C8;
- commission Claude before C9;
- enable Hooks before C10;
- enable general/background/scheduled or mutation-capable Automations before C11;
- use C9A for mutation, automatic implementation/action, scheduling, polling, background execution, browser/CDP, product-provider traffic, or any surface beyond the fixed advisory Claude review dispatcher;
- create a second backlog;
- trust a clean local checkout as current;
- treat `Sol Dev` as one repository;
- retain an unexplained retry loop.

## 31. Approval and audit criterion

This Runtime Specification was Director-approved after a recursive audit of repository law and current Codex behavior.

Implementation/commissioning must stop and reopen the relevant specification question if later evidence finds a material authority leak, unsupported Codex capability assumption, duplicate state surface, provider/billing ambiguity, validation-rung ambiguity, credential leak path, shared-Git race, cross-lane authority transfer, unbounded retry/concurrency, unnecessary tool, token-expensive default, rollback hole, or fresh-chat continuity dependency.

After the post-approval transition closes, commissioning proceeds **C0 through C9 sequentially**. Once C9 passes, C9A may be commissioned independently as the narrow read-only Claude orchestration branch. C10 remains the separate protective-Hook branch; C11-C13 remain sequential downstream of a valid C10 pass. Each gate is explained before action, its expected evidence is stated, and its result is inspected before any successor it can authorize.