# Ensemble / Kymaean — Codex Administrator Integration Working Continuity

Status: TEMPORARY NON-AUTHORITATIVE WORKING CONTINUITY EVIDENCE

Updated: 2026-09-09

Purpose: preserve the still-unfrozen Codex Administrator / Claude integration-design context across fresh ChatGPT chats without making chat history, this file, or any Administrator session a project-authority surface.

## Authority warning

This file is not project law, phase authority, validation authority, provider authority, Design authority, Engineering authority, or a second backlog.

A fresh chat must resolve live repository authority first. `Rylascoo/Ensemble-Project` and its exact live refs govern Engineering/product/cross-project truth; `Rylascoo/Ensemble-Website` and canonical Drive state govern Design-owned truth when relevant. If this file conflicts with live repository authority, repository authority wins. If it contains a newer real-world local-machine/setup observation, verify and reconcile its provenance instead of pretending the repository already knew it.

This file is working continuity only. It must sunset once the Codex Integration Blueprint / Administrator Runtime Specification is approved and durably represented in repository-native specification/authority surfaces.

## Program position

The repository-native manager migrations are complete:

- Engineering Sol is the accountable Engineering manager for `Rylascoo/Ensemble-Project`.
- Design Sol is the accountable Design/brand/UI/website manager for `Rylascoo/Ensemble-Website` plus canonical Drive visual masters.
- long personality prompts and handoffs are no longer required bootstrap memory.
- GitHub exact refs, `CURRENT_STATE.md`, repository law, evidence, and the execution queue remain durable project memory.

Active meta-objective:

> Design, recursively audit, then commission the Ensemble Project Administrator in Codex. Claude / Claude Code integration follows once Codex is sufficiently stable.

The project is still in blueprint/runtime-specification design. Do not treat this file as implementation authorization.

## Director collaboration / commissioning mode

The Director wants this complex integration designed completely before implementation begins, then commissioned step by step rather than as one opaque batch.

During blueprint and commissioning work:

- resolve repository, local-machine, tool, and connector facts directly when available instead of asking the Director to reconstruct them;
- ask the Director only for a genuine unresolved choice, authorization, credential/account action, or other fact the available evidence cannot resolve;
- present the complete plan/specification before configuration changes begin;
- once implementation is authorized, advance one commissioning gate at a time, explain what the step changes and what evidence will prove success, and inspect the result before proceeding;
- when the Director must run something manually, provide the smallest exact PowerShell/terminal command or UI action required and state the expected evidence to return;
- when another AI surface must be invoked manually, provide the exact `SURFACE`, current `MODEL`, `REASONING/EFFORT`, `MODE`, `USAGE CLASS`, and one paste-ready bounded prompt;
- keep progress/status updates compact while preserving exact refs, failures, and decisions;
- recursively audit major decisions and any correction; when an audit finds a material defect, correct it and restart the relevant audit before advancing;
- do not jump from blueprint discussion into Codex/Claude configuration merely because machine access exists.

For volatile Codex, ChatGPT, Claude, plugin/MCP, model, usage-limit, or configuration semantics, verify current official documentation and actual installed capability before freezing a runtime setting. The observed versions and subscription facts in this file are continuity evidence, not future product truth.

## Target authority and agent topology

```text
DIRECTOR
│
├── Engineering Sol — accountable Engineering authority
├── Design Sol      — accountable Design authority
└── Ensemble Project Administrator — Codex operational dispatcher only
      ├── Scout
      ├── Worker
      └── Reviewer
```

The Administrator is not a third project authority. Durable principle: **parallelize mechanics, not authority**.

- Scout: spawned read-only reconnaissance/evidence mapping.
- Worker: spawned bounded executor in an isolated worktree.
- Reviewer: spawned read-only falsification when risk warrants.
- Astra: scarce model escalation inside Codex, not a permanent agent identity.
- Claude: independent review/intelligence plane, never authority.

The persistent object is the Administrator role/configuration, not one immortal chat/session.

Normal concurrency: one spawned agent. Two when genuine parallelism adds value. Three is an exceptional ceiling. Normally no more than one expensive Sol/Astra-class spawned agent at a time. Authority-sensitive integration remains serialized.

Do not create permanent Git, Browser, Security, Documentation, or Repository agents unless later evidence proves a separate identity is useful; those are Skills/tool capabilities for now.

## Control, execution, and review planes

Control plane: Director + Engineering Sol + Design Sol + GitHub authority + central queue/law. It decides what should happen, under whose authority, and what evidence is required.

Execution plane: Codex Administrator + Scout/Worker/Reviewer + local Git/PowerShell + worktrees + tests/CI + deterministic Skills + later browser/CDP/Hooks/selected Automations. It carries already-authorized work through safely.

Independent-review plane: initially Claude Chat / Claude Code. It asks what the owning manager/executor may have missed and returns advisory evidence.

## Local workspace and observed environment

Actual umbrella workspace:

```text
C:\Users\Wiryl\Sol Dev\
├── Ensemble-Project\
├── Ensemble-Project-Worktrees\
├── Ensemble-Project-Evidence\
├── Ensemble-Website\
├── Ensemble-Website-Worktrees\
├── Kymaean Build History Assets\
└── historical / donor material
```

`Sol Dev` is not itself a Git repository. It is only the operational umbrella. Repository-bound work must anchor to the exact owning Git root. Do not recursively adopt sibling donor/historical projects unless current authority explicitly requires them.

Observed machine facts during the integration-design chat:

- Windows 11 ARM64.
- Git `2.55.0.windows.3`.
- .NET SDK `10.0.400`.
- Python `3.12.10` ARM64.
- Node `26.7.0`.
- PowerShell available/default shell.
- `gh` CLI was not installed at discovery time.
- Codex CLI was later installed; screenshot showed Codex CLI `v0.154.0` launched from `C:\Users\Wiryl\Sol Dev`.
- user Codex config exists at `C:\Users\Wiryl\.codex\config.toml` and appeared essentially minimal when inspected.
- Codex screenshot showed one MCP startup issue; C0 must identify/repair/remove it before plugin-dependent automation.
- Remote Desktop Commander is connected. Its current empty allowed-directory list exposes a broader filesystem scope than Ensemble needs; commissioning should narrow it.
- the Remote Desktop Commander process had not yet inherited the newly installed Codex PATH at inspection time; do not misdiagnose that as a Codex install failure.
- Claude Code was given access to `C:\Users\Wiryl\Sol Dev`; dedicated Claude installation/authentication/model/usage verification remains later commissioning work.

A crucial observed continuity case: local `Ensemble-Project` was clean but detached at an older validated checkpoint and did not contain the newer repository-native `AGENTS.md`. Therefore:

> **Clean local checkout != current authoritative checkout.**

Remote GitHub authority should be recovered first. Local-vs-origin reconciliation must precede treating any local file/checkpoint as current or authoritative.

## Repository reconciliation design

GitHub Desktop was considered and explicitly rejected from the permanent blueprint. System Git + PowerShell provides cheaper, clearer, more controllable repository reconciliation.

Task-start state machine:

```text
verify expected repository + remote URL
        ↓
git fetch --prune origin
        ↓
resolve HEAD / branch-or-detached / dirty+untracked / worktrees / origin refs
        ↓
classify
  CURRENT_CLEAN
  BEHIND_FAST_FORWARD_CANDIDATE
  AHEAD
  DIVERGED
  DETACHED
  DIRTY_OR_UNTRACKED
        ↓
safe action or stop/classify
```

Only a clean intended tracked branch that is strictly behind may become a deliberate `--ff-only` update candidate. Never automatically update detached validation/evidence checkouts, dirty trees, ahead/diverged branches, unique work branches, or checkouts with unresolved worktree ownership.

The Administrator is the default shared-Git ref coordinator. Serialize shared Git metadata/ref operations where concurrency could conflict, including branch/tag lifecycle, push, worktree add/remove, and integration/rebase/merge. Workers normally edit/build/test/commit in isolated worktrees; the Administrator race-checks origin and handles promotion/push/PR mechanics unless a later R0 rule explicitly delegates more.

## Administrator runtime direction

Preferred machine-local profile location:

```text
C:\Users\Wiryl\.codex-ensemble
```

not under `Sol Dev`.

Canonical Administrator runtime/configuration specification should eventually live in `Ensemble-Project`; the local profile is a generated executable realization and never project authority.

Conceptual local layout:

```text
C:\Users\Wiryl\.codex-ensemble\
├── config.toml
├── bootstrap/runtime instructions
├── agents\
│   ├── scout.toml
│   ├── worker.toml
│   └── reviewer.toml
└── skills\
```

Codex may later materialize/verify its approved settings but must not invent its own authority or security posture.

## Agent-profile and authority-write discipline

Agent profiles contain only stable capability/restriction rules. Current task context belongs in the dispatch packet.

Workers should normally not independently close or reinterpret:

- `CURRENT_STATE.md`;
- `PROJECT_EXECUTION_QUEUE.md`;
- `PROJECT_AUTHORITY.md`;
- cross-project orchestration/reasoning law;
- Design `CURRENT_STATE.md` / Design Ledger;
- provider authorization records;
- validation authority records.

A Worker may propose a necessary continuity diff, but the owning Sol reconciles the meaning before adoption. External reviewers normally return findings instead of editing authority surfaces.

## Dispatch / review contract

Consequential routed work should identify source role, target surface, queue item when applicable, exact source/target repository refs, bounded question/transformation, authority already granted, allowed/prohibited scope, required evidence, stop/failure conditions, return target, surface/model/reasoning/mode/usage class.

Review classes remain:

- R0 mechanical: executor self-check + machine validation/CI.
- R1 ordinary: executor + owning Sol recursive audit.
- R2 consequential: owning Sol + one independent reviewer when separation materially reduces risk.
- R3 critical: owning Sol + independent reviewer + optional scarce specialist falsification.
- R4 Director: evidence reconciled first, then Director decision.

Reviewer invocation is conditional, not ritual.

## Cross-lane transport

`docs/PROJECT_EXECUTION_QUEUE.md` remains the single cross-project backlog/sequencing register.

GitHub Issues are only a candidate dispatch transport, not yet activated project law. A future pilot must prove that the transport carries exact bounded packets without becoming a second backlog, sprint system, priority authority, or phase authority. If Issues add friction or duplicate planning state, reject them and use another transport. The architecture depends on a transport contract, not specifically on Issues.

## Resource / subscription governance

Director-supplied Work/Codex usage state during the design chat:

- 5-hour allowance: 97% left at observation time.
- weekly allowance: 90% left at observation time.
- then-current weekly reset: Sep 15, 2026 11:32 AM.
- banked full-reset expirations: Sep 21 2026 3:07 AM CDT; Oct 3 2026 7:44 PM CDT; Oct 4 2026 5:05 PM CDT.

These are volatile commissioning inputs, not durable law. The final Administrator must be sustainable after the banked resets are gone.

Usage states to design: `NORMAL`, `CONSERVE`, `RESERVE`, `CRITICAL`. If live usage can be read reliably, route economically from measured state. If usage is unavailable, do not invent it; default to conservative routing.

Durable compute principle:

> Use the least expensive currently available model/effort expected to satisfy the task's reliability class. Escalate because risk, uncertainty, or a new falsifiable attempt justifies it — not because the task is merely large or tedious.

A cheaper-model failure does not automatically authorize Sol; Sol failure does not automatically justify Astra. Before an expensive retry, classify the observed failure and state what materially different hypothesis or capability the next attempt tests.

Commissioning reset epochs are temporary capacity planning only: E-CODEX-0 blueprint/preflight; E-CODEX-1 Administrator commissioning; E-CODEX-2 real worker/cross-lane/evidence pilots; E-CODEX-3 recovery/falsification/browser/Claude stress; then E-CODEX-N under normal Plus limits. Record before/after usage and useful outputs to build an empirical project compute-efficiency dataset.

## Tool decisions

Keep/use:

- GitHub connector for authoritative repository/PR/Issue/CI access.
- Google Drive for Design visual-master/provenance access.
- Remote Desktop Commander for bounded manager-plane local inspection; narrow permissions during commissioning.
- OpenAI Developers for current OpenAI/Codex tooling guidance when actually in scope.
- Codex Security at justified security/milestone gates, not every patch.
- system Git + PowerShell for local Git control.
- local .NET/Python/Node toolchain.

Evaluate later only when a demonstrated gap exists: `gh` CLI, Figma, Vercel, Context7, PostHog/Datadog.

Rejected/deferred for now: GitHub Desktop integration, Linear/Jira/ClickUp/Slack, duplicate AI build platforms, extra permanent Git/Browser/Security/Documentation agents.

Tool-admission principle:

> A tool must eliminate a demonstrated project problem before entering the permanent toolchain.

## Credentials and external-event firewall

Distinguish repository authentication, tool/account authentication, and runtime/provider credentials. Provider keys remain task-specific/process-local where practical, are not copied into unrelated worktrees, and never imply provider-call authority.

Require a fresh authority check immediately before external events such as provider-bound inference, paid API action, deployment/publishing, Store submission, external messaging, irreversible external deletion, credential creation/rotation, or security-sensitive account changes. Preparing an action is distinct from executing it.

## Skills / Hooks / Automations

Skills should be script-first. Deterministic scripts should perform repository reconciliation, branch/worktree census, state-distance checks, CI/document-authority/secret scans, and closeout checks; models interpret anomalies instead of manually enumerating deterministic state.

Hooks come later and should enforce deterministic invariants, not make project judgments or rewrite authority files.

Automations come later only after repeated manual behavior proves stable. Each proposed Automation must pass an economic break-even test. No automatic merge, provider traffic, or unique-branch deletion.

## Browser / CDP

Browser work is initially a Skill/capability, not a permanent Browser agent. Later use a dedicated Kymaean development browser profile with no unrelated authenticated sessions. Browser/CDP evidence remains browser evidence only and never becomes WinUI/native/WACK/Store validation. CDP stays disabled until its commissioning gate.

## Claude direction

Claude begins more restricted than Codex to preserve independence. Initial Claude Code posture: exact repository access; pinned exact-SHA read-only review checkout/worktree; no routine GitHub mutation; no provider runtime access; no automatic implementation; findings return to the owning Sol.

Claude review packets should contain only the exact baseline, bounded question, relevant contract, candidate diff, tests/evidence, and falsification objective. Do not send full project history by default.

Dedicated Claude commissioning must verify executable/installation, Claude Pro subscription authentication, accidental API-key billing paths, model menu, usage status, filesystem permissions, Git behavior, and only then MCP/subagents/hooks if justified. Default target is subscription use, not API billing, unless separately authorized.

## Failure / retry taxonomy

Administrator should distinguish at least:

- `TASK_FAILURE`
- `STALE_BASELINE`
- `AUTHORITY_FAILURE`
- `TOOL_FAILURE`
- `RESOURCE_FAILURE`
- `ENVIRONMENT_FAILURE`
- `EVIDENCE_FAILURE`
- `CROSS_LANE_BLOCK`
- `EXTERNAL_GATE`

Failure category controls recovery. Resource failure is not technical failure; provider rejection is not retry authority; stale local state is not overwrite permission. Retry loops must be bounded and falsifiable.

## Commissioning gates

Current proposed earned-capability sequence:

| Gate | Capability |
|---|---|
| C0 | environment / tool / MCP / authentication census understood |
| C1 | deterministic local-vs-origin reconciliation works |
| C2 | fresh Administrator authority recovery works |
| C3 | deterministic Skills work |
| C4 | safe isolated worktree + bounded R0 mutation |
| C5 | branch / tag / push / PR lifecycle works |
| C6 | cross-lane transport experiment works |
| C7 | Q-E0A-03 local evidence pilot works without provider traffic |
| C8 | browser/CDP pilot works safely |
| C9 | Claude read-only independent review commissioned |
| C10 | Hooks proven |
| C11 | selected Automations proven |
| C12 | disaster recovery / fresh Administrator succeeds |
| C13 | sustainable under ordinary Plus limits |

Do not skip directly to broad automation.

## Known next-specification questions

The next blueprint refinement must define:

1. exact `C:\Users\Wiryl\.codex-ensemble` layout and config precedence;
2. Scout / Worker / Reviewer TOML profiles and permission matrix;
3. deterministic repository reconciliation algorithm and shared-Git locking;
4. worktree lifecycle and authority-file protections;
5. model/reasoning/usage scheduler and reliable usage-counter source, if available;
6. exact C0-C13 pass/fail tests;
7. one current Codex MCP startup issue;
8. replacement of generic umbrella setup (`pip install -r requirements.txt`, `pnpm install`) and generic Docker/Unix cleanup with minimal Administrator defaults and repo-specific Skills;
9. whether `gh` CLI solves a real gap;
10. exact Remote Desktop Commander allowlist/security profile;
11. browser/CDP security profile;
12. Claude Code installation/auth/model/usage configuration;
13. empirical Plus-cost calibration;
14. rollback/recovery for Administrator configuration;
15. which existing deterministic repository scripts should be wrapped rather than rewritten;
16. later Design-owned retirement/reclassification of the Website compatibility copy of the cross-project reasoning protocol.

## Explicitly rejected / deferred ideas

Do not silently revive without new evidence:

- GitHub Desktop as synchronization infrastructure;
- automatic local pull merely because remote changed;
- treating `Sol Dev` as one Git project;
- trusting a clean local checkout as current;
- a second project backlog in Issues/Linear/Jira/etc.;
- permanent Git/Browser/Security/Documentation agents;
- default three-agent parallelism;
- multiple expensive Sol/Astra agents by default;
- Reviewer on every patch;
- Hooks before manual Skills are proven;
- Automations before stable manual workflow;
- automatic Claude integration before Claude commissioning;
- raw provider credentials propagated across worktrees;
- Codex self-defining its own authority/security posture;
- new `PROJECT_ADMINISTRATOR_CONSTITUTION.md` or `AGENT_OPERATING_MODEL.md` unless later evidence shows existing orchestration law is insufficient.

## Exact next objective

Remain in blueprint design. Do not configure Codex, mutate project runtime/source, enable Hooks/Automations, or commission Claude merely because the tools exist.

Next objective:

> **Design the exact Ensemble Administrator Runtime Specification.**

It must cover directory/profile layout, Codex config layers, permission matrix, Scout/Worker/Reviewer profiles, deterministic Git reconciliation and locking, worktree lifecycle, authority-file protections, model/reasoning/usage scheduler, failure/retry rules, C0-C13 commissioning tests, and rollback/recovery.

Recursively audit that Runtime Specification until one complete pass finds no material error, authority leak, unsupported capability assumption, duplicate state surface, validation/provider/billing ambiguity, token-inefficient behavior, or worthwhile in-scope improvement. Only after the Director sees and approves the complete Runtime Specification should configuration implementation begin.

## Fresh-chat bootstrap

```text
Resume the Ensemble/Kymaean Codex Administrator integration-design program from `Rylascoo/Ensemble-Project`.

Resolve live GitHub refs and repository authority first according to root `AGENTS.md`; do not trust chat history or a stale local checkout. Read `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, `docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md`, `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`, `docs/AGENT_TOOLING_CAPABILITY_SNAPSHOT.md`, and `docs/PROJECT_EXECUTION_QUEUE.md` as required by live authority. Read `Rylascoo/Ensemble-Website` only where current Design/cross-lane truth materially matters.

Then read `docs/evidence/CODEX_ADMINISTRATOR_INTEGRATION_WORKING_CONTINUITY.md` strictly as temporary non-authoritative working continuity evidence. Read it through to EOF; if a connector or tool truncates a long read, continue with line-ranged reads until the whole file has been consumed. If it conflicts with live authority, live authority wins; verify any newer local/tooling observation before adopting it.

We are still designing, not implementing. Continue with the exact Ensemble Administrator Runtime Specification and recursively audit all conclusions. Walk the Director through the process using the collaboration/commissioning mode in this file: resolve what tools can resolve, ask only genuine unresolved questions, and do not make the Director reconstruct prior chat context. Do not configure Codex, enable Hooks/Automations, or commission Claude until the complete Runtime Specification has been presented and approved.
```

## Sunset condition

Once the Codex Integration Blueprint / Administrator Runtime Specification is approved and durably represented in repository-native specification/authority surfaces, retire this working continuity file from active use and preserve only whatever historical provenance repository law requires. Future fresh chats should then resume from GitHub and verified local state without depending on this file or the conversation that created it.
