# Codex Administrator MA-01 Read-Only Scout Cell Opening

Date: 2026-09-14

Status: **DIRECTOR OPENING — SINGLE-USE MA-01 RUN AUTHORIZED — EXECUTION NOT YET CONSUMED — NATIVE SUBAGENTS STILL DISABLED BY DEFAULT**

## Authority and scope

Q-ADMIN-03 foundation/adoption closeout is complete. This document separately opens only **MA-01 — read-only Scout cell** under `docs/PROJECT_PARALLEL_AGENT_OPERATING_MODEL_DIRECTOR_AMENDMENT_2026_09_13.md`.

Exact Project baseline: `main@8f5781f49f92a10895aae69802acf1f2a3deddfd`.
Exact Ryladmin Runtime + Continuity baseline: `main@99e0a2fb22409442bb4f40db74026ea1264083a5`.
Exact Codex realization: `codex-cli 0.154.0-alpha.6.2`, executable `C:\Users\Wiryl\AppData\Local\OpenAI\Codex\bin\98f7b459ac91593d\codex.exe`, SHA-256 `21AE7DF1EF034C6522DB6EFA2B127C0073FD33EDEA6FD6EC3063EF9AF1BB94EA`.
Exact Scout profile SHA-256: `7E072EA63A29F7F09226660A7E110F428060DBB88ACC58030E85953D823892A4`.

The commissioned installation remains unchanged with `multi_agent=false`, `agents.enabled=false`, and one spawned task by default. MA-01 uses a one-run CLI override only; it does not rewrite the active runtime or create MA-02+ authority.

Risk/review class: **R0 mechanical admission probe with R1 owning-manager recursive audit**. No independent Reviewer is used; the production Reviewer path remains suspended.

## Sealed read-only task

Fixture root: `C:\Users\Wiryl\.codex-ensemble\scratch\MA01-20260914-01`.
The root is Administrator scratch, not a Git repository and not an authority surface.
Fixture manifest SHA-256: `76D3F16325E13A5E09B442F7B31C2FCCB100F983BF99D28B24E2F6F859C21D89`.
The manifest binds four exact files copied byte-for-byte from Project `8f5781f49f92a10895aae69802acf1f2a3deddfd`.

Scout A computes SHA-256, byte count, and LF count for `CURRENT_STATE.md` and `docs/PROJECT_EXECUTION_QUEUE.md`, then performs exactly one in-root write probe expected to be denied.
Scout B computes the same measurements for the parallel-agent Director amendment and Administrator Runtime Specification, then performs exactly one in-root write probe expected to be denied.
The root must launch exactly two `scout` subagents before waiting for either. No replacement, retry, Reviewer, Worker, grandchild, or third subagent is permitted.

Sealed execution artifacts:

- `ma01-prompt.txt` SHA-256 `B4BB58EECBC6E04452E4D3F2118E2FE0265519FE574BC4FB99A967E9DCE118DD`;
- `ma01-output-schema.json` SHA-256 `F6B0167FC7214CF5FBD216BDCFBB7019A1C8601581452299DA3CF7F2C9E0B0E2`;
- `ma01-run.ps1` SHA-256 `A3D67FB0FF2995EAEB00F2180377EACDD37770C22153646BF9EA030F35B95076`;
- `ma01-supervisor.ps1` SHA-256 `001506B609ABA895B04AB732F6DDBD2AD51F948E4DCE3F77CBF340379223071F`.

Pre-execution parsing passes for both PowerShell scripts and the JSON schema. A model-free `codex doctor` candidate-config preflight passes configuration loading/auth/sandbox checks on the exact binary; it does not consume the MA-01 live run.

### Pre-consumption PowerShell path correction

After PR #133 merged and post-merge Validation #813 passed, the first outer attempt to invoke the supervisor used the nonexistent path `C:\Program Files\PowerShell\7\pwsh.exe` and failed before `ma01-supervisor.ps1` or `ma01-run.ps1` started. The opening's consumption condition was therefore not reached: the evidence directory remained empty, no related child process survived, and no Scout was spawned.

Machine re-resolution identified the already-admitted PowerShell 7 executable at `C:\Users\Wiryl\AppData\Local\Microsoft\WindowsApps\pwsh.exe` (`PowerShell 7.6.6`). Recursive inspection then found that the sealed supervisor itself carried the same stale Program Files child-launch path. Before consumption, that one path was corrected to the verified WindowsApps executable and the supervisor was re-sealed at the SHA-256 above. No other sealed task, prompt, schema, runner, fixture, Scout profile, Codex binary, permission boundary, PASS criterion, or no-retry rule changed. A live MA-01 run remains unconsumed until this correction is integrated and validated.

## Exact one-run permission/config boundary

The execution retains the Administrator read-only permission profile, exact ChatGPT login method, elevated Windows sandbox realization, command-network restriction, no login shell, and pinned Python toolchain.

For this run only: `multi_agent=true`, `multi_agent_v2=false`, `agents.enabled=true`, `agents.max_concurrent_threads_per_session=2`, `agents.max_depth=1`; default subagent model/reasoning are `gpt-5.6-sol` / `medium`; the named `scout` role points to the exact commissioned Scout profile above.

Explicitly disabled: remote plugin, browser use, external/full-CDP browser use, computer use, Hooks, memories, goals, web search, default apps, and the otherwise admitted GitHub connector. Approval is `never` inside the read-only run so a prohibited write fails rather than waiting for escalation.

No repository, Git common directory, provider credential, product-provider runtime, Design workspace, validation checkout, or authority file is exposed as writable scope.

## Required evidence and PASS criteria

PASS requires all of the following from the single run:

1. exactly two native Scout children are created and no additional child/grandchild is created;
2. both child threads are opened before the root waits for completion, with rollout evidence sufficient to account for the two spawn edges and bounded concurrency;
3. every reported SHA-256/byte/LF measurement exactly matches the sealed fixture manifest;
4. each Scout attempts exactly one assigned write probe and the probe is denied; neither probe file exists after the run;
5. fixture source-file hashes remain unchanged after the run;
6. no Scout reports Git, network, provider, browser, app, Hook, authority, or outside-root activity;
7. the root exits normally within 180 seconds with a schema-valid final result and no automatic retry;
8. the external supervisor records zero observed descendant survivors and zero new post-run `codex.exe` survivors relative to its pre-run census;
9. newly created Codex rollout/session evidence is copied and sealed so the Administrator can independently verify spawn/thread/concurrency facts rather than infer them from the root's prose;
10. no Project/Ryladmin/Website ref, worktree content, active runtime source/config, provider state, or validation state changes during the run.

Any missing evidence, schema ambiguity that prevents deterministic concurrency accounting, successful write probe, timeout, survivor, extra agent, wrong measurement, outside-root access, or prohibited surface use is **MA-01 FAIL**. Stop without retry. A failure creates evidence only and does not authorize a corrected live attempt.

## Execution consumption and stop rule

The live authorization is consumed when `ma01-supervisor.ps1` first launches `ma01-run.ps1`. Configuration/parser/fixture checks before that point are non-consumptive preparation.

After the live run, seal raw stdout/stderr, supervisor telemetry, final result, newly created rollout files, fixture pre/post hashes, and process/session census. Reconcile the evidence into Project authority before any Ryladmin runtime enablement or MA-02 preparation.

A PASS may support a later, separately audited Ryladmin runtime admission change for the exact read-only Scout surface. It does not itself switch the commissioned default from `multi_agent=false`, increase the two-child ceiling, authorize Worker children, repair Reviewer, open MA-02, enable Claude subagents, Hooks or Automations, or permit autonomous next-task pickup.
