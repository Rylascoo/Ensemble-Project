# E0-A Post-Audit Hardening — Native Windows ARM64 Validation

Status: **DIRECTOR-MACHINE-SOURCED — PASS — FAKE-ONLY / CREDENTIALLESS SCOPE**

Date: **2026-09-07**

Repository: `Rylascoo/Ensemble-Project`

Hardening branch: `e0a-phase-b-post-audit-hardening`

Exact machine-tested executable/test checkout:

`5c70f619d6e951d89bb527a5945b014998573dab`

This record classifies Director-machine Native Rerun 02 after Attempt 01 exposed a stale test oracle. It is native Windows ARM64 runtime evidence for the exact checkout above. It is not provider-network, provider-spend, NPU, package, WACK, Store, or later-phase evidence.

## Pre-validation authority

The command set observed promoted `origin/main`:

`67de40f4b76075dbf7feb3ad00aee8ed7f0bbbed`

The validation checkout was exact detached HEAD `5c70f619...` with:

- tracked tree clean;
- staged index clean;
- no material untracked files under `src/`, `tests/`, or `fixtures/`;
- one nonmaterial root-level untracked file: `patch0012-local-edit.txt`.

## Native host

Observed on the Director machine:

- Windows `10.0.26200`;
- `PROCESSOR_ARCHITECTURE=ARM64`;
- repository-selected .NET SDK `9.0.317`;
- `dotnet --info` RID `win-arm64`;
- Host Architecture `arm64`.

The command set removed both `OPENAI_API_KEY` and `GEMINI_API_KEY` and asserted both absent before validation.

## Freshness discipline

Before native Harness test execution, the command set removed Harness and Harness-test `bin/` and `obj/` output roots. Before the explicit Harness build used by the fixture and credentialless smokes, it removed the target `win-arm64` Harness output again and asserted that the prior target output was absent.

Therefore the Harness test and smoke stages did not rely on a stale binary from an earlier checkout.

## Core tests

PASS:

- total: `622`;
- succeeded: `622`;
- failed: `0`;
- skipped: `0`;
- command exit: `0`.

## Harness tests

PASS:

- total: `88`;
- succeeded: `88`;
- failed: `0`;
- skipped: `0`;
- command exit: `0`.

This closes the single stale-oracle failure from Attempt 01. The repaired test oracle at `5c70f619...` changes only the expected diagnostic from `provider-incomplete` to `malformed-provider-response`; no runtime source changed.

## Explicit native Harness build

PASS after target-output removal:

- target RID: `win-arm64`;
- build configuration: Debug;
- build exit: `0`;
- fresh output observed at `src/Ensemble.E0.Harness/bin/Debug/net9.0/win-arm64/Ensemble.E0.Harness.dll`.

## Fixture smokes

PASS against that fresh build:

- Missing Raft fixture: exit `0`, `ensemble.e0.missing-raft@0.1.0` validated;
- generic smoke fixture: exit `0`, `ensemble.e0.smoke@0.1.0` validated.

## Credentialless provider-edge gate

PASS:

- explicit `e0a-run CREATIVE-NONE` was invoked with the exact checkout provenance `5c70f619...`;
- native exit: `1` as expected for the missing credential gate;
- combined captured output contained `OPENAI_API_KEY is required at the E0-A provider edge.`;
- no evidence root was created;
- `OPENAI_API_KEY` remained absent;
- `GEMINI_API_KEY` remained absent.

### PowerShell transcript artifact

The interactive transcript contains two `else : The term 'else' is not recognized` parser errors. They occurred only because the response's multi-line `if`/`else` assignment was entered interactively as separate commands: the `if` assignment had already completed before the standalone `else` token was submitted.

These parser messages do **not** invalidate the credentialless probe. After them, the command set successfully asserted all credentialless conditions above. In particular, the combined captured output contained the exact expected missing-OpenAI-key message; otherwise the subsequent `Require(...Contains(...))` statement would have thrown and the later PASS markers could not have been reached.

The canonical filed rerun handoff uses single-line `if ... else ...` assignments and does not contain this interactive formatting hazard.

## Post-validation authority

PASS:

- post-validation HEAD remained exactly `5c70f619d6e951d89bb527a5945b014998573dab`;
- tracked diff exit: `0`;
- staged diff exit: `0`;
- no material untracked source/test/fixture files were admitted;
- terminal marker `HARDENING_NATIVE_RERUN_02_COMPLETE` was reached.

## Classification

```text
Checkout / working-tree authority        PASS
Native Windows ARM64 host probes         PASS
Core tests                               PASS 622/622
Harness tests                            PASS 88/88
Fresh explicit Harness build             PASS
Missing Raft smoke                       PASS
Generic fixture smoke                    PASS
Credentialless OpenAI provider-edge      PASS
Post-validation checkout / cleanliness   PASS
Provider-network execution               NOT AUTHORIZED / NOT PERFORMED
Overall hardening native validation      PASS — FAKE-ONLY / CREDENTIALLESS SCOPE
```

## Validation boundary

This PASS establishes native Windows ARM64 runtime authority only for exact executable/test checkout `5c70f619d6e951d89bb527a5945b014998573dab` and the operations actually exercised above.

It does not establish live provider pricing freshness, cache billing correctness, provider spend correctness, real provider behavior, NPU/hardware acceleration, package/WACK behavior, Store certification, E0-B+, application UI/persistence, or later product behavior.

Real provider credentials, provider-network inference, and provider spend remain separately Director-gated.

Under Engineering Hygiene Law 5 Repository Surface, this machine-tested checkout requires an annotated validation tag before `CURRENT_STATE.md` promotes it as current machine-tested authority.
