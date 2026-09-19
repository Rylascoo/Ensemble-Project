# Administrator Quota Reserve and Repository Write Policy

Status: DIRECTOR-APPROVED Q-ADMIN-04 ADDENDUM / IMPLEMENTATION AND LIVE VALIDATION REQUIRED
Date: 2026-09-19
Parent: `docs/PROJECT_CODEX_ADMINISTRATOR_ESTABLISHMENT_DIRECTOR_AUTHORIZATION_2026_09_19.md`

## Decision and scope

The Director requested:

> i think we can also add when codex reaches around 10% usage remaining, putting itself in a light use state until the token limit resets then resume as normal. you also should have write abilities to the repository to formalize all of our changes

Adopt approximately 10% remaining account allowance as the conservation trigger and explicitly authorize scoped repository writes to formalize approved work. This addendum governs quota pacing and write scope within Q-ADMIN-04; it does not create a second backlog, admit an untested capability, resume paused app work, or erase an earlier failed attempt. The numeric defaults below are project policy, not OpenAI limits or measured optimal settings.

## Quota input and accounting

Use supported, authenticated usage interfaces verified on the exact admitted Codex realization. Keep account allowance, per-task token usage, context-window capacity, and model availability separate. Never infer subscription headroom from a context-window percentage or convert raw tokens to allowance percentage without a validated conversion.

For the selected task/model, identify all applicable metering buckets/windows and deduplicate equivalent legacy/multi-bucket reports. For each valid window, remaining percentage is `100 - usedPercent`; the limiting remaining allowance is the minimum across applicable windows, not their average. Do not hard-code primary as five-hour or secondary as weekly. A short-window reset cannot clear another still-low or exhausted allowance. Model changes do not erase shared limits.

Record observation time, source/version, bucket identity, window duration, remaining percentage, reset timestamp when supplied, and active dispatch reservations. Validate ranges and freshness before dispatch. Missing, malformed, stale or unexpectedly disappearing required measurements mean USAGE_UNKNOWN, never unlimited capacity. An explicitly documented non-applicable window need not be invented. External sessions can consume allowance, so estimates cannot guarantee a hard floor.

Account snapshots govern dispatch eligibility. Collect manager/worker/reviewer usage once by unique job/event identity for efficiency analysis; avoid double-counting cumulative events or aggregate/child totals. Keep private account identifiers and raw session/auth data out of Git. Unsupported metrics or enforcement remain explicitly unknown/unavailable.

## Required operating states

| State | Admission and behavior |
|---|---|
| NORMAL | Fresh usable allowance and task-sized headroom: select qualified skill/model/effort and proceed within the current authorized objective. |
| LIGHT | Enter at or below 10% limiting remaining allowance, or earlier when forecast work would consume the closeout reserve. No new large/high-cost task or parallel expansion. At most one small qualified model task at a time; prefer deterministic checks, bounded summaries and checkpoint preparation. |
| CHECKPOINT_ONLY | At or below 5% remaining, or whenever safe closeout would otherwise be jeopardized: no new substantive model task. Preserve the current result/partial work, tests, ownership and exact next operation; then stop model dispatch. |
| WAITING_FOR_RESET | Exhausted allowance or no safe eligible work above the reserve: no model calls merely to wait or ask for quota. Only an admitted deterministic supervisor may observe a reset; otherwise return a durable checkpoint for manual resume. |
| USAGE_UNKNOWN | Stop admitting new model work. Preserve evidence; use only bounded supported read-only refresh attempts or manual recovery, never assume replenishment. |

The initial protected reserve is at least 5 percentage points per applicable window, increased when measured closeout/in-flight needs require it. The 5% checkpoint threshold and 15% normal-resume threshold are conservative implementation defaults chosen under the Director's approximate-10% instruction. They must be versioned and tested, not silently tuned to keep workers busy.

Checkpoint on entry to LIGHT rather than waiting for exhaustion. Before each dispatch account for remaining approved work, in-flight reservations, review/closeout cost, and uncertainty. If no trustworthy allowance-cost estimate is available near the threshold, prefer deterministic work or waiting. Do not lower correctness, review quality, sandbox controls, or required evidence to use the remaining allowance. Model/effort changes occur at task boundaries and must remain qualified and supported.

Do not abandon a write mid-transaction: finish an already-bounded safe atomic step or request controlled cancellation, seal partial state, and release shared Git locks. Never claim an uncommitted partial result is validated or merged. Existing multi-agent activity must drain or cancel safely; LIGHT permits no additional fan-out. User stop/pause, revoked authority and safety failures override every quota state.

## Reset and resumption

Remain in LIGHT or a stricter state until a fresh supported observation proves relevant allowance replenishment and all applicable windows exceed 15% remaining. Require enough headroom for the next task plus reserve. This hysteresis prevents repeated switching around 10%; elapsed time or an old reset timestamp alone is not proof. A blocked weekly/model/shared bucket keeps normal work held.

Before normal dispatch resumes, recheck auth class, runtime/capability admission, exact repository refs, lease validity, user pause/cancel, and exclusive write ownership. Reconcile moved baselines instead of replaying a saved command blindly. Persist the last quota state and blocking reason across restart so restart cannot reset the policy to NORMAL.

In-session pacing/resumption is approved once implemented and verified. Designing and testing deterministic wait-and-resume supervision is approved, but unattended wake/restart remains default-off until the existing C10 eligibility and C11 admission requirements and an exact bounded supervisor acceptance are satisfied. This addendum is not a pass for either gate. No scheduler or future task is activated by this document. Waiting must not consume model turns; use supported events or bounded timed rechecks with backoff, not busy polling. An expired session/lease or absent admitted supervisor returns a manual-resume checkpoint.

No automatic credit purchase, banked-reset redemption, authentication/provider/account switch, spending fallback, security bypass or retry of consumed experimental openings is allowed to extend operation.

## Explicit repository-write authority

The Administrator is authorized to author and preserve approved runtime, test, workflow, recovery and continuity changes in an isolated Ryladmin branch/worktree; it is not limited to proposing prose. It may formalize Director decisions and prepare their Project documentation in a separately scoped Project worktree, with accountable Engineering adoption of protected Project authority changes. Routine edits within an approved objective do not require repeated blanket permission.

Use an explicit bounded repository-writer context or admitted top-level Worker while retaining the control session's read-only default. If a new write profile is required, test it outside the active installation before use. Never broaden parent permissions merely to make children write. Authoritative capability/grant records cannot be edited by the candidate they admit, and a writer cannot approve its own capability promotion. No parent/child or old/new-chat overlapping writes.

Only the serialized coordinator performs shared Git mechanics after the required owning-role acceptance. Commit/push/PR operations preserve exact scope and expected HEAD; integration uses explicit PR operations plus the applicable checks and exact-main validation. No direct main contents writes, automatic quota-triggered merge, unreviewed active-runtime overwrite, force-push of paused work, or extraction of credentials. Missing connector/OS permissions remain an access blocker; documentation is not a claim those permissions are installed.

Product semantics, Design assets/law, provider/spend, Store submission, and currently paused implementation remain separately owned. Recording approved decisions is not permission to invent new decisions. A/B installation and rollback remain binding.

## Implementation acceptance

Add the deterministic quota policy and task/result fields to ADMIN-EST-FOUNDATION-01 without expanding its first live-agent scope. Test below/equal/above 10%, 5% reserve, 10/15% hysteresis, mixed windows, real reset versus elapsed clock, unknown/stale inputs, cumulative usage deduplication, competing usage/in-flight reservations, controlled cancellation, restart, changed baseline, expired lease and user pause. Prove no underlying dispatch for rejected cases, no model calls while waiting, and no purchase/reset/auth fallback.

Write tests must prove allowed isolated edits and denied foreign/protected/active-installation writes, denial of self-grant, one-writer ownership, read-only Scout/Reviewer behavior, and no promotion by a forged result. Record policy tests separately from live metering/containment and unattended capability admission. Until exact implementation evidence exists, status remains APPROVED / NOT IMPLEMENTED.

## Verified external interface references

Checked 2026-09-19 in official documentation: `https://developers.openai.com/codex/app-server` (redirects to `https://learn.chatgpt.com/docs/app-server`) documents `account/rateLimits/read`, `account/rateLimits/updated`, `thread/tokenUsage/updated` and `model/list`. Rate windows expose usage percentage and reset metadata when supplied. `https://developers.openai.com/codex/subagents` documents explicit model/effort configuration and permission inheritance. These are integration leads, not proof of installed-version support or current account quota; verify before implementation. Thresholds, reserve, state transitions and write delegation above are Director/project policy, not platform guarantees.
