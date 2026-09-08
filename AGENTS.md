# Ensemble Engineering Bootstrap Contract

Status: durable engineering continuity contract. It does not carry product, phase, validation, or provider authority.

## Fresh-chat bootstrap

Before making a load-bearing project-state claim or modifying the repository:

1. Resolve `Rylascoo/Ensemble-Project` live branch refs from GitHub. Record the exact active branch and HEAD; do not infer them from chat history, default-branch search, or a stale local checkout.
2. Read `CURRENT_STATE.md` at that exact active ref first. If `main` and the active ref differ, compare their state surfaces before relying on a volatile fact.
3. Verify `CURRENT_STATE.md` currency. CI permits at most three commits after its last change; a passing distance check does not make its prose correct, so reconcile it against exact source/evidence before relying on a load-bearing claim.
4. Read `docs/PROJECT_AUTHORITY.md`, `docs/ENGINEERING_HYGIENE_CONSTITUTION.md`, and `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`.
5. Read `docs/PROJECT_EXECUTION_QUEUE.md` and reconcile its statuses/prerequisites against exact current state. The queue sequences work but cannot override stronger authority; a mismatch is a continuity defect to repair before acting on the disputed item.
6. Resolve the current executable-validation boundary from `docs/VALIDATION_LEDGER.md`: exact checkout, annotated tag, validation rung, and whether any later commit changed `src/`, `tests/`, fixtures, project/build files, or other executable behavior.
7. Read only the current architecture/evidence/handoff surfaces named by `CURRENT_STATE.md` and the authority chain. A handoff is live only when `CURRENT_STATE.md` names its exact path.
8. Resolve current Director decisions, provider authorization, dated external-fact validity, and any explicit deferred/reopened assumptions before work that could cross those boundaries.
9. Inspect live branches for an unexplained implementation/review/validation/Director-disposition role. Historical branch truth belongs in annotated archive tags, not indefinite live refs.
10. When the task touches UI, website, visual assets, design evidence, or a suspicious cross-lane artifact, verify canonical residency against `docs/REPOSITORY_RESIDENCY.md` and, when necessary, `Rylascoo/Ensemble-Website` and the canonical Drive workspace.
11. Check the latest CI/compiler result at the exact active ref. Never promote compiler or off-target test evidence into native Windows ARM64 runtime authority.

## Exact-ref evidence rule

Default-branch code search is discovery only. A load-bearing fact about active work must be re-read from the exact active branch, exact commit, validation tag, or durable evidence record that owns it. If sources disagree, report and reconcile the disagreement before recommending a change.

## Ordered-execution rule

`docs/PROJECT_EXECUTION_QUEUE.md` is the durable backlog/sequencing register. Before starting a new work package, verify that its queue status permits work and that every prerequisite is satisfied by exact evidence. Later-phase preparation may proceed only when the queue and governing authority explicitly distinguish preparation from execution. Never consume a future experiment, provider request, scorer result, renderer budget, or irreversible gate merely because preparation is allowed.

When closeout changes a queue item's status, prerequisite, owner, or exit condition, update the queue in the same logical closure. If a consequential task is discovered and is not already represented by the queue, roadmap, ODR, hypothesis ledger, or lane ledger, add or classify it before ending the work package.

## Self-healing boundary

A fresh chat should detect and classify drift automatically. It may repair only within already-authorized engineering scope. It must not silently:

- move an ambiguous cross-lane artifact;
- convert design evidence into product/policy authority;
- promote a historical document by linking it;
- change provider authorization, pricing validity, or a Director decision;
- merge or discard unique branch work without establishing its disposition;
- inflate a validation rung.

When a deterministic repository-law defect is inside authorized scope, correct it in the same work package and verify the correction. When authority is genuinely ambiguous, stop at the Director decision instead of guessing.

## Closeout duty

Before ending a substantive engineering work package:

- recursively audit correctness, consistency, authority, scope, tests, simplicity, hygiene, ARM64 suitability, vision, evidence, branch lifecycle, document residency, continuity, and execution-queue state;
- update `CURRENT_STATE.md` whenever the work changes phase/checkpoint/validation/next action, and in all cases before the N=3 currency limit would be exceeded;
- update `docs/PROJECT_EXECUTION_QUEUE.md` whenever the work changes a queued status/prerequisite or discovers an otherwise untracked consequential task;
- remove or archive completed temporary handoffs and stale branch refs according to repository law;
- leave one obvious engineering next action, preserve any separately authorized parallel-lane preparation boundary, and leave no silent change in project authority.
