# Codex Administrator MA-01 Read-Only Scout Cell Terminal Failure

Date: 2026-09-14

Status: **MA-01 FAIL — SINGLE-USE RUN CONSUMED — FAILED BEFORE SCOUT SPAWN — NO RETRY AUTHORIZED — NATIVE SUBAGENTS DEFAULT OFF**

## Authority and exact realization

Opening authority was Project `main@0c2ef2c0e8e141a03237ed4eaf00ce8b56c75a7a`, after PR #134 corrected the sealed supervisor PowerShell path and post-merge Validation #816 passed all five jobs. Ryladmin Runtime + Continuity authority remained `main@99e0a2fb22409442bb4f40db74026ea1264083a5`; Website remained `main@b9b9946e05af6d39e0555295611837ba546e659c`.

The single authorized realization retained Codex `0.154.0-alpha.6.2`, executable SHA-256 `21AE7DF1EF034C6522DB6EFA2B127C0073FD33EDEA6FD6EC3063EF9AF1BB94EA`, Scout profile SHA-256 `7E072EA63A29F7F09226660A7E110F428060DBB88ACC58030E85953D823892A4`, runner SHA-256 `A3D67FB0FF2995EAEB00F2180377EACDD37770C22153646BF9EA030F35B95076`, and corrected supervisor SHA-256 `001506B609ABA895B04AB732F6DDBD2AD51F948E4DCE3F77CBF340379223071F`.

Immediately before launch, all five sealed artifact hashes, all four fixture hashes, the exact Codex/Scout identities, zero preexisting evidence, zero preexisting related MA-01 processes, PowerShell `7.6.6`, and installed `multi_agent=false` were re-proved. Project and Ryladmin refs matched the opening.

## Consumption and terminal result

The supervisor launched the sealed runner at `2026-09-14T14:24:44.5306265Z`; this satisfied the opening's explicit consumption condition. The authorization is therefore consumed and may not be retried.

The runner exited with code `2` at `2026-09-14T14:24:45.9121186Z`, well inside the 180-second bound. Supervisor containment completed with `timed_out=false`, zero observed survivor PIDs, and zero post-run new `codex.exe` survivors.
## Failure mechanism

`codex-events.jsonl` is zero bytes and no `root-result.json` was produced. `codex-stderr.txt` records the terminal CLI parser error:

> `error: unexpected argument '--ask-for-approval' found`

The same stderr identifies the accepted command surface as `codex exec [OPTIONS] ...`; the sealed runner had supplied `--ask-for-approval never` inside its `exec` argument vector. The parser rejected that invocation before a Codex event stream or session rollout was created.

A model-free post-failure syntax inspection confirms the mismatch: top-level `codex --help` exposes `-a, --ask-for-approval <APPROVAL_POLICY>`, while `codex exec --help` omits that option. A future materially changed runner could therefore test moving `--ask-for-approval never` into the pre-`exec` global argument surface, but this evidence does not authorize such a run.

This is an **invocation-layer falsification of the exact sealed MA-01 runner realization**. It is not evidence that native multi-agent capability itself is unavailable or incorrect. No native Scout thread/session evidence exists, so the required two-Scout cell was never established.

Supervisor telemetry observed two ordinary process descendants below the runner and a maximum of two simultaneous descendants. Those process facts must not be reinterpreted as two Scout agents: `new_session_files=[]`, `copied_rollout_files=[]`, stdout is empty, and no root result exists.

## Sealed raw evidence

- `codex-events.jsonl`: 0 bytes; SHA-256 `E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855`.
- `codex-stderr.txt`: 251 bytes; SHA-256 `C206A405769CB087010D604FAB55FD38B0612D2DFC6435797CFBA6DF5240B7E7`.
- `supervisor-telemetry.json`: 500 bytes; SHA-256 `5BBE3EC81BD9CD0F372B1210B3A527703B8583737104D54433EA417D7867E35F`.
- telemetry root PID `5860`; observed descendant PIDs `19532`, `23828`; root exit code `2`; timeout false; survivor PIDs empty; new Codex PIDs empty.
- no write-probe file exists for Scout A or Scout B.
- all four sealed fixture source hashes still exactly match `fixture-manifest.json` after the run.
## Post-run integrity

Fresh post-run ref resolution found Project still at `0c2ef2c0e8e141a03237ed4eaf00ce8b56c75a7a`, Ryladmin still at `99e0a2fb22409442bb4f40db74026ea1264083a5`, and Website still at `b9b9946e05af6d39e0555295611837ba546e659c`.

All 13 installed non-secret Administrator runtime files exactly match `Ryladmin/runtime/CURRENT_SOURCE_MANIFEST.json` at `99e0a2fb22409442bb4f40db74026ea1264083a5`; mismatch count is zero. Installed `config.toml` still carries `multi_agent=false`. No commissioned runtime admission occurred.

No Project, Ryladmin, or Website authority ref changed during the run. No fixture content changed. No write probe succeeded. No provider credential, product-provider runtime, browser, app/connector, Hook, validation checkout, or Design authority was exercised by the failed invocation.

## Concurrent Engineering reconciliation

Before this terminal evidence was promoted, the independent E0-D Engineering lane completed its own overlapping authority-state transition. PR #135 merged the native-validated E0-D implementation, and PR #136 then closed that integration as Project `main@2468fa5a6116ed3acea915cd5478ce66c05ecab2`; push-triggered exact-main Validation #822 completed SUCCESS. The MA-01 terminal candidate was therefore rebuilt from that exact main rather than merging its earlier staged copies of `CURRENT_STATE.md` or `docs/PROJECT_EXECUTION_QUEUE.md`.

This reconciliation preserves Engineering's E0-D state verbatim: implementation + native validation are integrated, live activation remains separately unauthorized, and provider traffic remains zero. The Administrator closeout changes only MA-01 continuity/queue state and this terminal evidence.
## Later Engineering preauthorization reconciliation

After the prior integration reconciliation, Engineering independently completed the read-only E0-D live-activation preauthorization audit. PR #137 exact head `96bf5b332bd4d81fbe33960b110ddfaf0f51490f` passed Validation #824 and E0-E preparation #81, merged as Project `main@4da6a0c18b99cd2eff06c78d5bd00eaf76181935`, and push-triggered exact-main Validation #825 passed all five jobs. The MA-01 terminal candidate was therefore rebuilt again from that exact main rather than carrying forward stale copies of `CURRENT_STATE.md` or `docs/PROJECT_EXECUTION_QUEUE.md`.

This second reconciliation preserves Engineering's E0-D preauthorization state verbatim: live activation remains unauthorized, provider traffic remains zero, protected key/account/quota/capacity readiness remains pending, and the validated executable snapshot retains its UTC 2026-09-14 validity cutoff. The Administrator closeout changes only MA-01 continuity/queue state and this terminal evidence.

## Disposition

MA-01 is **FAIL and consumed**. Stop without retry. The active Administrator runtime remains `multi_agent=false`; native subagents remain uncommissioned. MA-02, MA-03, MA-04, MA-05, Reviewer repair/revalidation, Hooks, Automations, and autonomous next-task pickup remain blocked or separately gated exactly as before.

Any future Scout admission attempt requires a new explicit Director opening for a materially changed realization, including model-free verification of the exact `codex exec` argument surface and a newly sealed/validated runner. This terminal evidence does not authorize that successor attempt and must not be treated as an automatic retry ticket.

The Administrator failure itself created no E0-D implementation, provider traffic, RunId, namespace, UTC window, credential use, inference, spend, validation promotion, or launch authority. The later E0-D implementation/integration transition was independently owned and validated by Engineering as recorded above.