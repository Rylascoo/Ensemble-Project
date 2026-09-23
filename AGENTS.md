# Ensemble Project Bootstrap Contract

Status: durable engineering continuity contract. It does not carry product, phase, validation, or provider authority.

## Purpose

This file is the durable bootstrap for application work in `Rylascoo/Ensemble-Project`, regardless of which ChatGPT/Codex surface performs the task.

A pasted personality prompt, special agent identity, or long fresh-chat handoff is not required. Durable role, workflow, authority, hygiene, orchestration, and continuity rules live in the repository. `CURRENT_STATE.md` alone carries the volatile project checkpoint/validation/next action.

Historical handoffs and personality prompts are continuity evidence only.

## Task roles and accountability

Operational personas are retired. Historical evidence may still say **Engineering Sol**, **Design Sol**, **Engineer #1/#2/#3**, Relay, or Administrator; those names are provenance, not separate current project identities.

Every current task declares one or more roles:

- **Implementation** — source/tests/build/persistence/Windows mutation inside an already-authorized scope;
- **Design** — application UX/UI, interaction, accessibility, visual/state presentation, and Design acceptance;
- **Product / Architecture** — Product semantics, contracts, ontology boundaries, ODR input, and architecture decisions subject to Director authority;
- **Independent Review** — behaviorally no-write exact-ref/diff falsification; it does not repair the candidate it reviews;
- **Governance / Recovery** — authority reconciliation, repository continuity, evidence provenance, branch/worktree disposition, and exceptional Ryladmin recovery.

The **Director** retains product constitution, consequential policy, provider/spend authority, irreducible Design taste, and explicit merge authorization where repository law requires it.

A single model may perform different roles in different bounded tasks. The role and mutation authority matter; the persona name does not.

Product orientation:

> Ensemble/Kymaean is a local-first generative theater and creative simulation for persistent characters. The creator establishes people, circumstances, knowledge, relationships, possibilities and pressures; AI performers portray those characters; a Director manages attention/opportunity; accepted performances and authoritative consequences become causal history that changes what future scenes can mean.

This summary is durable orientation, not a substitute for current source/Blueprint authority.

`docs/design/app/` is the active application-design authority root. Website owns website design/implementation and historical app-design provenance; Drive owns shared creative masters. Co-location never lets Implementation redefine Design or Design invent Product/runtime semantics.

Director + ChatGPT reconcile work manually. Codex is reserved for bounded implementation/debugging when it materially helps; Git remains durable authority and GitHub remains PR/integration authority. Ryladmin is exceptional governance/recovery/evidence infrastructure, not a normal application-work identity or autonomous runtime.

### Native Codex execution roles

For an Implementation task, one primary writer is accountable for the bounded change. Historical **Engineering Sol** / **Engineer #1** labels identify prior responsibility only.

Default implementation uses an isolated worktree and one primary writer. Delegate concrete read-heavy exploration, source/architecture mapping, log analysis and independent review to bounded native supporting subagents when useful. Use the built-in explorer/worker; do not recreate persistent Engineer #2/#3 managers. Supporting agents cannot independently change authority, queue state, validation, provider permissions or Design law.

Consequential application changes receive read-only exact-ref/diff review. The reviewer returns findings and must not repair the source it reviews. Verify its effective read-only policy; a custom-agent configuration is not proof that parent runtime overrides are absent. The primary writer reconciles findings and performs all repairs.

One active mutation package per primary writer; no overlapping writers in a worktree. Separate Design/Engineering ownership remains load-bearing. Historical returned producers and leases stay preserved until explicitly dispositioned; native tooling does not adopt them.

Native mechanics belong in project `.codex` configuration and the Desktop environment. No API key or Agents API runtime is required. Built-in subagents provide supporting evidence, never authority. Repository refs/PRs/CI and bounded return artifacts preserve progress; task chats are not a second backlog.

The successor scope and historical supersession boundary are in `docs/NATIVE_CODEX_APPLICATION_WORKFLOW.md`; `docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md` retains authority separation and review discipline.

### Product drift guard

Ensemble is not the shelved DeskShifter productivity/workspace/wallpaper product. If a proposed architecture begins optimizing for task management, desktop organization, or that product's old mission rather than Ensemble's current Blueprint/product law, stop and resolve the drift.

`Rylascoo/Kymaean-Project` and archived DeskShifter material are quarantined donors, not sources of truth. Consult donor material only when current project law explicitly authorizes a narrow donor disposition and preserve provenance.

## Fresh-chat bootstrap

Before making a load-bearing project-state claim or modifying the repository:

1. Resolve **all** `Rylascoo/Ensemble-Project` live branch refs from GitHub. Record exact `main` plus every non-main branch HEAD, then identify the intended active branch from current authority; do not infer it from chat history, default-branch search, a handoff name, or a stale local checkout.
2. Read `CURRENT_STATE.md` at that exact active ref first. It is the sole volatile engineering phase/checkpoint/validation/next-action authority. If `main` and the active ref differ, compare their state surfaces before relying on a volatile fact.
3. Verify `CURRENT_STATE.md` currency. CI permits at most three commits after its last change; a passing distance check does not make its prose correct, so reconcile it against exact source/evidence before relying on a load-bearing claim.
4. Read `docs/PROJECT_AUTHORITY.md`, `docs/ENGINEERING_HYGIENE_CONSTITUTION.md`, `docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md`, and `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`.
5. Read `docs/AGENT_TOOLING_CAPABILITY_SNAPSHOT.md` only for current model/tool allocation when an external/Codex/Claude/browser dispatch is relevant. It is volatile tooling guidance, not architecture or phase authority.
6. Read `docs/PROJECT_EXECUTION_QUEUE.md` and reconcile its statuses/prerequisites against exact current state. The queue sequences work but cannot override stronger authority; a mismatch is a continuity defect to repair before acting on the disputed item.
7. Resolve the current executable-validation boundary from `docs/VALIDATION_LEDGER.md`: exact checkout, annotated tag, validation rung, and whether any later commit changed `src/`, `tests/`, fixtures, project/build files, or other executable behavior.
8. Read only the current architecture/evidence surfaces named by `CURRENT_STATE.md` and the authority chain. Read a historical handoff only when current authority names it or a narrow provenance ambiguity requires it.
9. Resolve current Director decisions, provider authorization, dated external-fact validity, and any explicit deferred/reopened assumptions before work that could cross those boundaries.
10. Inspect every live branch for an explained implementation/review/validation/Director-disposition role. Historical branch truth belongs in annotated archive tags, not indefinite live refs.
11. Inspect local repository state before treating it as clean or disposable: include untracked paths and `git worktree list --porcelain`. For any nested or suspicious Git worktree, resolve `git rev-parse --absolute-git-dir` and `git rev-parse --git-common-dir`; filesystem location alone does not establish repository ownership.
12. When the task touches UI, website, visual assets, design evidence, or a suspicious cross-lane artifact, verify canonical residency against `docs/REPOSITORY_RESIDENCY.md` and, when necessary, live `Rylascoo/Ensemble-Website` / canonical Drive state.
13. Check the latest CI/compiler result at the exact active ref. Never promote compiler or off-target test evidence into native Windows ARM64 runtime authority.

Do not reconstruct current authority from an old handoff, personality prompt, branch name, chat summary, or default-branch search.

## Handoff/personality retirement

Long handoffs are convenience packets, not project memory. The prior `01_PERSONALITY_Ensemble_Engineering.md` prompt is migration input/history after its still-valid durable rules are absorbed into repository law.

A fresh application chat should normally need only a minimal pointer such as:

```text
Resume Ensemble from live Rylascoo/Ensemble-Project authority.
ROLE: Implementation
```

Use `ROLE: Design`, `ROLE: Product / Architecture`, or `ROLE: Independent Review` when that is the actual task.

The agent then executes this bootstrap and exact-ref reconciliation.

If a historical handoff conflicts with current repository authority, repository authority wins. If the handoff contains a newer real-world event not yet recorded in GitHub, classify that as a continuity defect and durably reconcile the evidence boundary before proceeding; do not silently pretend the repository already knew it.

## Source-of-truth / external-workspace boundary

Canonical sources:

1. frozen Blueprint / approved phase law;
2. `CURRENT_STATE.md` for volatile engineering checkpoint/validation/next action;
3. source/tests/fixtures/commits/current evidence;
4. Director decisions and durable policy records;
5. Project `docs/design/app/` for current application Design, `Rylascoo/Ensemble-Website` for website-only Design/history, and Drive for shared creative masters when relevant.

Current application-design truth is rooted at `docs/design/app/AUTHORITY.md`. The exact Website sources and `docs/design/app/TRANSFER_RECEIPT.json` remain immutable transfer provenance rather than a second live authority.

Google Drive `Ensemble Project` is the active project visual/research/master-asset workspace, never engineering validation authority.

Drive disambiguation is load-bearing:

- active root: `Ensemble Project` (`1VKomZE6PSaEM9c7q22p4r_N6UY0HM2D8`), containing active `03 Visual Identity & Artwork / Kymaean` (`1MJrfMi1EZ_Wk3-IqC_BMMz1GTYpsrMnz`);
- quarantined legacy root: `Kymaean Project` (`1b825q4ou6kWFRlAz75sjQ5LL5HgrpF81`).

Never quarantine or adopt by name match alone.

## Exact-ref evidence rule

Default-branch code search is discovery only. A load-bearing fact about active work must be re-read from the exact active branch, exact commit, validation tag, or durable evidence record that owns it. If sources disagree, report and reconcile the disagreement before recommending a change.

Branch topology and accepted-content reconciliation are separate questions. A historical or archived checkpoint being non-ancestral to current `main` does not by itself prove lost work; compare accepted source/test/document content or semantic contract before classifying loss. Conversely, ancestry alone does not promote historical validation or make obsolete content current.

## Ordered-execution rule

`docs/PROJECT_EXECUTION_QUEUE.md` is the durable backlog/sequencing register. Before starting a new work package, verify that its queue status permits work and every prerequisite is satisfied by exact evidence.

Later-phase preparation may proceed only when queue/governing authority explicitly distinguishes preparation from execution. Never consume a future experiment, provider request, scorer result, renderer budget, or irreversible gate merely because preparation is allowed.

When closeout changes a queue item's status, prerequisite, owner, or exit condition, update the queue in the same logical closure. If a consequential task is discovered and is not already represented by the queue, roadmap, ODR, hypothesis ledger, or lane ledger, add/classify it before ending the package.

## Bounded work-package interface

Consume and produce routed work according to `docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md`.

A dispatch packet transports work; it does not create authority.

Before acting on an incoming packet, verify:

- source/target role;
- central queue item when applicable;
- exact repository/ref baseline;
- bounded question/transformation;
- authority already granted;
- allowed/prohibited scope;
- expected evidence/return artifact;
- stop/failure conditions;
- whether baseline state moved since dispatch.

When one task role needs another role or external execution surface, generate the complete bounded work packet rather than asking the Director to reconstruct project context.

For a genuinely external manual handoff, include:

```text
SURFACE: ...
MODEL: ...
REASONING/EFFORT: ...
MODE: ...
USAGE CLASS: normal | scarce
```

then one complete paste-ready prompt. Verify current recommendations from the tooling snapshot/current product capability before dispatch.

Ordinary native supporting subtasks need a concise bounded task, exact source/ref, read/write scope and evidence return; the external handoff header is not required for each native subagent. Do not assume automated cross-chat or Claude routing exists.

The old `docs/blueprint/CODEX_ADMINISTRATOR_RUNTIME_SPECIFICATION.md` and its commissioning evidence remain historical/separately gated infrastructure, not application setup prerequisites. Do not commission that runtime, Hooks, recurring automation, Claude routing or provider traffic from native Codex availability.

Within an authorized package, continue through implementation, focused validation, correction, required broader checks, final diff, commit, authorized branch push and draft PR. Do not stop at an intermediate step for routine confirmation. Stop at completion, genuine authority ambiguity, concurrent-work collision, external/manual dependency or a safety/validation boundary. Merge requires Director authorization. Application readiness/actions and exact current tool guidance: `docs/NATIVE_CODEX_APPLICATION_WORKFLOW.md`.

## Self-healing boundary

A fresh chat should detect and classify drift automatically. It may repair only within already-authorized engineering scope. It must not silently:

- move an ambiguous cross-lane artifact;
- convert design evidence into product/policy authority;
- promote a historical document by linking it;
- change provider authorization, pricing validity, or a Director decision;
- merge or discard unique branch work without establishing disposition;
- inflate a validation rung;
- diagnose a runtime/provider failure from hypothesis without preserved evidence;
- invoke provider traffic because an executor technically can.

When a deterministic repository-law defect is inside authorized scope, correct it in the same package and verify the correction. When authority is genuinely ambiguous, stop at the Director decision instead of guessing.

## Closeout duty

Before ending a substantive engineering work package:

- recursively audit correctness, consistency, authority, scope, tests, simplicity, hygiene, ARM64 suitability, vision, evidence, branch lifecycle, document residency, continuity, orchestration/dispatch state, and execution-queue state;
- update `CURRENT_STATE.md` whenever work changes phase/checkpoint/validation/next action, and in all cases before the N=3 currency limit would be exceeded;
- update `docs/PROJECT_EXECUTION_QUEUE.md` whenever work changes a queued status/prerequisite or discovers an otherwise untracked consequential task;
- preserve/retire temporary handoffs and branches according to repository law;
- leave one obvious engineering next action;
- preserve separately authorized parallel-lane boundaries;
- leave every live branch/local worktree explained or dispositioned;
- leave no silent change in project/provider/validation authority.

A clean closeout means repository continuity is sufficient for the next task to resume without the Director reconstructing the previous chat.
