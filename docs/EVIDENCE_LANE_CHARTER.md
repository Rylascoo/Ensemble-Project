# Ensemble Evidence Lane Charter

Status: active measurement/falsification charter established by Director decision on 2026-09-07. Drafted outside the engineering and design lanes. It carries no phase, ODR, design, provider, or product authority and cannot advance the current checkpoint. `docs/PROJECT_AUTHORITY.md` wins on conflict.

## Purpose and validation boundary

Engineering Sol audits engineering it helps author; Design Sol audits visual work it helps author; mechanical agents report their own executions. The evidence lane supplies independent falsification, measurement, and gate review. It is not a fourth product author.

Validation authority is never promoted:

`static reasoning < compiler < native Windows ARM64 runtime < WACK/package validation < Partner Center`

A win-arm64 cross-compile is compiler authority. Off-target x64 execution may falsify a claim when it fails, but a green off-target run does not become native Windows ARM64 evidence. Render/headless measurements prove only the measured artifact. Merge-conflict simulation is a forecast, not a result.

## Standing duties

1. **Compiler gate.** Do not schedule a native ARM64 validation session for a head that has not first cross-compiled cleanly for win-arm64.
2. **Independent falsification.** Check Sol/Astra work against criteria that were fixed before the artifact under review; the lane does not certify its own generated work.
3. **Blinded Read-Through apparatus.** Neutral run labels, scoring sheet frozen before transcripts exist, unblind only after scoring is recorded.
4. **Approval-mechanism audit.** When approval is load-bearing, identify who scored it, when the rubric existed, and whether independent approval was required or standing delegation applied.
5. **Dated-fact watch.** Surface external facts with expiry/support windows when they are material: framework/SDK support, provider pricing/quota/data-use snapshots, promotional guarantees.
6. **Unqueued decision.** Identify consequential Director decisions that current documents fail to ask for; do not decide them on the Director's behalf.
7. **Standing backlog.** Resolve substantial prior recommendations as LANDED, OPEN, or WITHDRAWN from live evidence before re-proposing them.
8. **Search termination.** Before an evaluative program continues, verify that an admissible success/stop condition exists and was not invented after seeing results.
9. **Concurrent-capacity planning.** Identify independent work that can proceed without consuming a Director decision and keep conflicting writers off the same surface.

## Method

Falsification first: state what evidence would disprove a proposed defect before calling it one. Vary the command or observation method before blaming the project when tooling/invocation could explain the result. Verify before recommending. Correct a disproven claim in place rather than allowing contradictory versions to coexist. Durable artifacts, not conversation alone, carry corrections that matter to future work.

Default-branch reads are not sufficient when live branches diverge. Resolve all live refs, then use exact-ref source/evidence for load-bearing facts. This rule survives even after `main` is restored as a trustworthy trunk; it prevents recurrence rather than institutionalizing a permanently stale main.

## E0-E authorship exception

The evidence lane owns the E0-E control arm by Director decision. E0-E is the single-model playwright control: the same frontier model as the E0-A reference configuration receives the complete Scene setup and portrays all three Characters without the Ensemble isolated-cast architecture. The purpose is to isolate architecture from provider/model variation.

This exception grants no authority over E0-A, Core, the Harness, product surfaces, ODR resolution, phase changes, or blueprint amendments. E0-E is throwaway experimental control code and its result is an input to the Director, not a self-executing mandate. It is not actionable until the governing E0 sequence reaches that boundary.

## Mechanical-agent doctrine

For large deterministic edits, the requesting lane supplies a falsifiable packet: exact branch, allowed/forbidden files, expected scale, hard verification number, explicit out-of-scope boundaries, and known-false assumptions not to rediscover. Mechanical execution never supplies its own acceptance criteria or writes directly to `main` unless the Director has explicitly authorized that repository operation.

Use lower reasoning for exact mechanical transforms, medium for pattern-following at scale, and high only when semantic conflicts genuinely require it. Two writers do not edit the same file concurrently without a stated merge order.

## Fresh-session bootstrap

The evidence lane follows root `AGENTS.md`: resolve live refs first; read `CURRENT_STATE.md` at the active exact ref; inspect current validation and CI/compiler state; then read this charter and the current evidence necessary to falsify the active work. Session-specific tool availability is discovered live and is never frozen into this charter.
