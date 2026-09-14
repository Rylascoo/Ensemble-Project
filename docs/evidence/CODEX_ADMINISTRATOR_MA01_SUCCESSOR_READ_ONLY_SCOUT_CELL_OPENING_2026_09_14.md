# Codex Administrator MA-01 Successor Read-Only Scout Cell Opening

Date: 2026-09-14

Status: **DIRECTOR OPENING — SINGLE-USE MA-01 SUCCESSOR RUN AUTHORIZED — EXECUTION NOT YET CONSUMED — NATIVE SUBAGENTS STILL DISABLED BY DEFAULT**

## Authority and predecessor

This document opens one materially changed successor attempt for **MA-01 — read-only Scout cell** under `docs/PROJECT_PARALLEL_AGENT_OPERATING_MODEL_DIRECTOR_AMENDMENT_2026_09_13.md`. It does not retry or erase the consumed predecessor.

Fixture-source Project baseline: `main@243dd7314cda1770878d555f04d4373199e29492`.
Ryladmin Runtime + Continuity baseline: `main@8a11841fbf060fcfbe60ef84ad6a3083848d43ff`.
Website baseline: `main@4bb91292171e0d503d3a4e058ee421436221a793`.
Exact Codex realization: `codex-cli 0.154.0-alpha.6.2`, executable SHA-256 `21AE7DF1EF034C6522DB6EFA2B127C0073FD33EDEA6FD6EC3063EF9AF1BB94EA`.
Exact Scout profile SHA-256: `7E072EA63A29F7F09226660A7E110F428060DBB88ACC58030E85953D823892A4`.

The predecessor opening is permanently consumed and terminally failed before Scout spawn. Its runner SHA-256 was `A3D67FB0FF2995EAEB00F2180377EACDD37770C22153646BF9EA030F35B95076`; durable result: `docs/evidence/CODEX_ADMINISTRATOR_MA01_READ_ONLY_SCOUT_CELL_TERMINAL_FAILURE_2026_09_14.md`. This successor has a new single-use authorization and a materially changed sealed runner.

The commissioned installation remains unchanged with `multi_agent=false` and `agents.enabled=false`. This opening authorizes only one CLI-override admission run; it creates no runtime installation change and no MA-02+ authority.

## Materially changed successor realization

Successor scratch root: `C:\Users\Wiryl\.codex-ensemble\scratch\MA01-20260914-02`.
Fixture manifest SHA-256: `950CD230BEC928D1D07D6D0EEAD8C615BA4B0A3E746BCE158E318794D5E152B5`.
Successor preparation manifest SHA-256: `0BEEE6CC97897383E966F146C116C3BE10E83921B8B74668E08E10BF6E11ED44`.
Successor delta audit SHA-256: `00AEB1D18593962C3961FA429453FA1CA15943D32FDEDA03F89E515F3274CAFF`.

Sealed execution artifacts:

- `ma01-prompt.txt` SHA-256 `B4BB58EECBC6E04452E4D3F2118E2FE0265519FE574BC4FB99A967E9DCE118DD` — byte-identical to the admitted predecessor prompt;
- `ma01-output-schema.json` SHA-256 `F6B0167FC7214CF5FBD216BDCFBB7019A1C8601581452299DA3CF7F2C9E0B0E2` — byte-identical to the admitted predecessor schema;
- `ma01-run.ps1` SHA-256 `CD2BD829B8DD8E96C685342F4AF98EC22FCC849E5DD2BD07E216428ED35668E3`;
- `ma01-supervisor.ps1` SHA-256 `1D1A4F106B0466CA3B285FAFB1B9E1B516E8D3028F2F65D208AC27706F4BF7CC`.

The deterministic delta audit proves the prompt and schema are byte-identical, the supervisor changes only the successor scratch-root identity, and the runner changes only that root plus relocation of `--ask-for-approval never` from the invalid `codex exec` option vector to the valid global pre-`exec` option surface. Unified runner-diff SHA-256: `600D169C67E19D6AD22730AF5DE39F8B22B4D0679A594C7BB395BBA53E40714C`.

Model-free preflight passes on the exact realization: corrected global-to-`exec --help` parsing; both PowerShell scripts; JSON schema/manifest parsing; exact fixture rehash; empty successor evidence directory; effective `multi_agent=true` with `multi_agent_v2=false` and prohibited feature toggles false; candidate configuration accepted by Codex Doctor with zero failures. These checks do not consume the live authorization. Codex Doctor's own installation/connectivity diagnostics are not an MA run and create no E0-D/provider execution authority.

## Sealed read-only task and permission boundary

The fixture root is Administrator scratch, not a Git repository or authority surface. Its manifest binds four files copied byte-for-byte from Project `243dd7314cda1770878d555f04d4373199e29492`.

Scout A inspects only `CURRENT_STATE.md` and `docs/PROJECT_EXECUTION_QUEUE.md`; Scout B inspects only the parallel-agent Director amendment and Administrator Runtime Specification. Each computes SHA-256, byte count, and LF count for its two files and performs exactly one in-root write probe expected to be denied.

The root must launch exactly two `scout` subagents before waiting for either. No replacement, retry, Reviewer, Worker, grandchild, or third subagent is permitted.

For this run only: `multi_agent=true`, `multi_agent_v2=false`, `agents.enabled=true`, `agents.max_concurrent_threads_per_session=2`, `agents.max_depth=1`; default subagent model/reasoning are `gpt-5.6-sol` / `medium`; the named Scout role points to the exact profile above.

Explicitly disabled: remote plugin, browser use, external/full-CDP browser use, computer use, Hooks, memories, goals, web search, default apps, and the otherwise admitted GitHub connector. Approval is `never` at the global CLI surface while the execution sandbox is read-only, so prohibited writes fail instead of waiting for escalation.

## Required evidence and PASS criteria

PASS requires all of the following from the single successor run:

1. exactly two native Scout children are created and no additional child/grandchild is created;
2. both child threads are opened before the root waits, with rollout evidence sufficient to account for both spawn edges and bounded overlap;
3. every SHA-256/byte/LF measurement exactly matches the sealed fixture manifest;
4. each Scout attempts exactly one assigned write probe and the probe is denied; neither probe file exists after the run;
5. fixture source-file hashes remain unchanged after the run;
6. no Scout reports Git, network, provider, browser, app, Hook, authority, or outside-root activity;
7. the root exits normally within 180 seconds with a schema-valid final result and no automatic retry;
8. the external supervisor records zero observed descendant survivors and zero new post-run `codex.exe` survivors;
9. newly created Codex rollout/session evidence is copied and sealed for independent spawn/thread/concurrency verification;
10. no Project/Ryladmin/Website ref, worktree content, active runtime source/config, provider state, or validation state changes during the run.

Any missing evidence, extra agent, schema ambiguity preventing deterministic concurrency accounting, successful write probe, timeout, survivor, wrong measurement, outside-root access, or prohibited surface use is **MA-01 SUCCESSOR FAIL**. Stop without retry. A failure creates evidence only and does not authorize another corrected live attempt.

## Pre-consumption race barrier, consumption, and stop rule

Immediately before launch, re-resolve Project/Ryladmin/Website live refs; rehash the Codex binary, Scout profile, fixture, prompt, schema, runner, supervisor, preparation manifest, and delta audit; prove the successor evidence directory and write-probe residue are empty; prove no related MA-01 successor process is active; rehash every fixture source; and prove the commissioned runtime still has native agents/default multi-agent disabled. Any unexplained mismatch or authority movement stops execution until separately reconciled.

This successor authorization is consumed when the sealed `ma01-supervisor.ps1` first launches the sealed `ma01-run.ps1`. Parser/config/fixture/hash checks before that point are non-consumptive preparation.

After the run, seal raw stdout/stderr, supervisor telemetry, final result, newly created rollout/session files, fixture pre/post hashes, and process/session census. Reconcile the result into Project authority before any Ryladmin runtime admission or MA-02 preparation.

A PASS may support a later, separately audited Ryladmin runtime admission change for the exact read-only Scout surface. It does not itself switch the commissioned default, increase the two-child ceiling, authorize Worker children, repair Reviewer, open MA-02, enable Claude subagents, Hooks or Automations, or permit autonomous next-task pickup. It creates no E0-D provider, credential, validation, claim, window, inference, scoring, or spend authority.
