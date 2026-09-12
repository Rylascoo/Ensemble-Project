# Codex Administrator C9A R2 Live-Pilot Falsification — 2026-09-11

Status: **C9A R2 FALSIFIED / NOT COMMISSIONED; NO SAME-REALIZATION RETRY AUTHORIZED**

## Authority and scope

This record closes the one-attempt authorization in `CODEX_ADMINISTRATOR_C9A_R2_CORRECTED_ATTEMPT_DECISION_2026_09_11.md`.

Preserved boundaries:

- C9 manual Claude review remains commissioned and advisory only.
- C9A architecture remains Director-approved but no automated Claude routing is commissioned.
- C10 remains independently falsified; Hooks remain disabled.
- C11+ and general/mutating Administrator Automations remain blocked.
- This record creates no Engineering, provider, validation-rung, Design, product, ODR, merge, deployment, scheduling, or mutation authority.

## Exact implementation candidate

- audited base: `339d0f58e9647dc4efb67ffe362e4ab21c7997a3`;
- exact R2 candidate: `58fedc8e54679804813d6780b31f4c034d0e9a2f`;
- PR: `#90`, closed unmerged after the pilot failed;
- exact-head Validation: `#701` **SUCCESS**, including the C9A offline falsification step, repository law, document census, oracle coverage, ARM64 cross-compile, and required x64 regression;
- archive tag: `archive/q-admin-02-c9a-r2-falsified-timeout-2026-09-11`;
- archive tag object: `a79f13c11effc47ac4ad5d9696e051c690d38307`;
- peeled candidate: `58fedc8e54679804813d6780b31f4c034d0e9a2f`.

## Pre-dispatch gate

Immediately before dispatch, the Administrator reverified:

- clean exact candidate `58fedc8e54679804813d6780b31f4c034d0e9a2f`;
- Claude Code `2.1.267` on Windows ARM64;
- executable SHA-256 `0DC306259E3036AF4255F66B77451D3F7297BCD376BFAE20F7B42E2FA1473607`;
- isolated `CLAUDE_CONFIG_DIR` under `C:\Users\Wiryl\.claude-ensemble`;
- `claude.ai` / `firstParty` / `pro` authentication;
- no alternate Anthropic/API/cloud-provider credential variables present;
- scratch root `C:\Users\Wiryl\.codex-ensemble\scratch\c9a-r2-live` outside every Project worktree.

The fresh five-file R2 packet was not byte-identical to either preserved R1 packet.

- packet bytes: `44,452`;
- packet SHA-256: `C722DB533B0D7C06641E70D833828CA6FADC478E56CD888868B4F4427182CDC2`;
- question SHA-256: `56826A69E21BEBD82AF391E01CF11BC2E2272CB5EFA4200D13EBA70931A6B560`.

The invocation retained `--safe-mode`, `--restricted`, empty tools, strict MCP configuration, plan/no-prompt posture, no session persistence, no Chrome, no slash commands, `high` effort, `--max-turns 1`, ordinary JSON output, a 180-second hard bound, and zero dispatcher retry.

Claude native `--json-schema` / StructuredOutput was not requested. Structured advisory output remained subject to deterministic local validation.

## Live result

Exactly one authorized R2 Claude invocation was consumed. It failed closed at the same hard bound observed in R1:

- result: **FAIL CLOSED — Claude subprocess exceeded 180 seconds**;
- dispatcher exit code: `124`;
- result bytes: `0`;
- result SHA-256: `E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855`;
- stderr bytes: `0`;
- stderr SHA-256: `E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855`;
- sealed telemetry bytes: `5,293`;
- telemetry SHA-256: `EFD80C7125B73EE6D76507FF2CD3B5194BD60CA50D545FB57AAC2EC6FF1AB5BC`;
- `dispatcher_retry_count=0`.

The R2 evidence correction worked: timeout telemetry and the post-execution repository snapshot were sealed before the dispatcher failed closed.

Before and after snapshots were identical for HEAD, branch, Git refs hash, status hash, and worktree topology hash. The candidate worktree remained clean at `58fedc8e54679804813d6780b31f4c034d0e9a2f`. Claude caused no Project file, Git ref, branch, tag, provider, authority, or external-project mutation.

## Falsification conclusion

R2 falsifies the hypothesis that removing Claude native StructuredOutput while adding one-turn/local-schema/timeout telemetry is by itself sufficient to make the C9A realization complete within the frozen 180-second/high-effort envelope.

This result does not establish a general Claude service defect. It establishes only that this exact R2 realization did not satisfy C9A commissioning.

## Disposition

**C9A R2 is NOT COMMISSIONED. Do not retry this realization or any preserved R1/R2 packet.**

Any later C9A live attempt requires a new explicit materially changed hypothesis and decision, followed by a separately audited exact candidate and its own pre-live gates. A newer Claude version, longer timeout, different effort level, or other realization change is not self-authorizing.

C9 remains the commissioned manual independent-review plane. C10 remains falsified on its separate Hook failure. C11+ remain blocked.

Mechanical closeout:

- PR #90 is closed unmerged;
- the falsified R2 candidate is preserved by the annotated archive tag above;
- the implementation must not be merged or described as active automation;
- Project continuity must state that the single R2 authorization was consumed and failed closed;
- Ryladmin may mirror this result only as Administrator continuity and must not replace Project authority.
