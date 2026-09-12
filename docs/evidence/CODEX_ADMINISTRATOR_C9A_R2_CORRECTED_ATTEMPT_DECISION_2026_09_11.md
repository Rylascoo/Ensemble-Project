# Codex Administrator C9A R2 Corrected-Attempt Decision — 2026-09-11

Status: **AUTHORIZED FOR ONE FUTURE LIVE R2 PILOT AFTER DURABLE DECISION + SEPARATE R2 CANDIDATE VALIDATION; NO LIVE ATTEMPT CONSUMED**

## Authority and preserved boundaries

- Director-approved C9A architecture remains `docs/evidence/CODEX_ADMINISTRATOR_C9A_READ_ONLY_CLAUDE_ORCHESTRATION_DIRECTOR_AMENDMENT_2026_09_11.md`.
- R1 timeout falsification remains `docs/evidence/CODEX_ADMINISTRATOR_C9A_READ_ONLY_CLAUDE_ORCHESTRATION_TIMEOUT_FALSIFICATION_2026_09_11.md`.
- C9 manual Claude review remains commissioned.
- C9A remains uncommissioned until a live pilot satisfies the Director amendment's pass condition.
- C10 remains independently falsified; Hooks remain disabled; C11+ remain blocked.
- This decision creates no Engineering, provider, validation-rung, Design, product, ODR, merge, deployment, scheduling, or mutation authority.

## Observed R1 facts

R1 used Claude Code `2.1.267` on Windows ARM64, isolated under `C:\Users\Wiryl\.claude-ensemble`, with scrubbed `claude.ai` / `firstParty` / `pro` authentication and no API-key/cloud-provider route.

Two bounded live packets failed closed at the same 180-second execution bound. Attempt 01 was 84,199 bytes; Attempt 02 was 30,565 bytes. The second attempt therefore falsified packet size alone as a sufficient explanation. Neither packet may be retried unchanged.

R1 also exposed an evidence-quality weakness: timeout occurred before the dispatcher wrote result/stderr/telemetry or its own post-execution repository snapshot. The surrounding Administrator independently confirmed zero Project mutation, but a corrected realization must seal those facts itself.

## Materially changed R2 hypothesis

The new hypothesis is that Claude Code's native headless `--json-schema` / StructuredOutput path can itself introduce internal structured-output retries, stalls, or malformed-output recovery in a tool-less print-mode review, and that this mechanism materially contributed to the R1 timeouts.

This is a falsifiable hypothesis, not a finding about Claude service health. Non-live upstream issue inspection supports testing it: `anthropics/claude-code#27926` reported a headless `--json-schema` hang; `#77026` reports headless schema-validation retry exhaustion/malformed structured output; and open `#87234` reports structured-output failures and hidden retries correlated with toolless calls. These reports are supporting external evidence only and do not replace local commissioning evidence.

## R2 corrected realization contract

R2 must preserve every C9A isolation control while changing only the suspected structured-output mechanism and the timeout evidence path:

1. retain Claude Code `2.1.267` and its already-recorded executable identity for this causal test; do not silently substitute `2.1.269` or another version;
2. retain `high` effort and the 180-second hard bound so the experiment does not mix mechanism, version, effort, and timeout changes;
3. retain `--safe-mode`, `--restricted`, empty tools, strict empty MCP inheritance, plan/no-prompt posture, no session persistence, no Chrome, no slash commands, scrubbed alternate credentials, and first-party Pro auth enforcement;
4. remove Claude native `--json-schema` / StructuredOutput entirely;
5. request exactly one JSON object in ordinary print-mode output and add `--max-turns 1`;
6. parse the ordinary Claude result deterministically and validate it locally against the frozen review payload contract (`verdict`, `summary`, `findings`);
7. preserve one dispatcher invocation only, with zero automatic retry;
8. on timeout or any other failure, preserve partial stdout/stderr, write failure telemetry, and take the post-execution Git/ref/worktree snapshot before failing closed;
9. fail if the repository snapshot changes, including when the Claude subprocess times out.

The R2 dispatcher may still write only its bounded Administrator scratch packet/result/stderr/telemetry. Those artifacts remain execution evidence, never Project authority.

## Offline evidence already earned

The non-live R2 realization has passed the dedicated adversarial suite under commissioned Python 3.12.10 ARM64, SHA-256 `7F1EBBBB04164CAF8F8073AAFA7F82321FB44695E10FE6A9EBAF2138B63892C4`:

- deterministic packet construction PASS;
- non-first-party auth fail-closed PASS;
- repository mutation detection PASS;
- scratch-outside-worktree enforcement PASS;
- local schema validation PASS;
- explicit one-turn command bound PASS;
- timeout telemetry sealing PASS;
- post-timeout mutation detection PASS.

The full existing Administrator Skill fixture suite also passed after the R2 changes, including commissioning closeout and the measurement read-only guard. Repository law, document census with zero unexplained current documents, oracle coverage, and diff hygiene passed on the non-live worktree.

## One-attempt authorization and prerequisites

Exactly one R2 live pilot may be consumed only after all of the following are true:

1. this corrected-attempt decision is merged to Project `main` through PR-only integration and exact post-merge Validation is green;
2. the R2 implementation is carried on a separate exact-base candidate, recursively audited, and its own exact-head Validation is green before the live call;
3. immediately before dispatch, the Administrator rechecks exact candidate HEAD/cleanliness, Claude executable/version/hash, scrubbed first-party Pro authentication, scratch boundary, and the absence of alternate credential routes;
4. the pilot packet is freshly generated from the R2 exact candidate and is not byte-identical to either preserved R1 packet;
5. the packet remains bounded to the C9A law/decision, the R1 falsification context needed to assess the correction, and the R2 dispatcher/self-test/Skill surfaces needed to review the realization;
6. no provider/product experiment traffic, Engineering execution, automatic implementation, or mutation authority is coupled to the review.

The live result passes C9A only if the existing Director-amendment commissioning condition is satisfied, including a structured advisory result, sealed hashes/telemetry, first-party subscription boundary, one-turn/single-attempt behavior, and identical repository/Git state before and after.

If R2 times out, returns malformed/local-schema-invalid output, reports unauthorized capability use, changes repository state, or otherwise fails the contract, it fails closed and **no same-realization third retry is authorized**. Any later version upgrade, timeout/effort change, tool-policy change, or other realization requires another explicit materially changed hypothesis and decision. A newer installed/released Claude version is not itself permission to retry.

## Disposition

**Authorize one future C9A R2 live pilot after the durable-decision and separate-candidate gates above. Do not execute it from this decision branch.**

Until that future pilot passes, C9A remains **NOT COMMISSIONED** and must not be described as automated Claude routing. C9 remains the commissioned manual independent-review plane. C10 and C11+ remain unchanged.
