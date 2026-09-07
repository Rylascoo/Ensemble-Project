# E0-A Post-Audit Hardening — Native Windows ARM64 Validation Attempt 01

Status: **DIRECTOR-MACHINE-SOURCED — FAIL — 1 OF 88 HARNESS TESTS FAILED; DIAGNOSTIC IDENTIFICATION PENDING**

Date: **2026-09-07**

This record preserves the first Director-machine native Windows ARM64 validation attempt of the compiler-repaired post-audit hardening candidate. It is failure evidence, not a validation pass, and it does not authorize provider credentials, provider-network execution, inference, or spend.

## Exact attempted checkout

`dc7ac428f00041f71f3ae78baa9b7bf83c61ce55`

Promoted `origin/main` observed by the command set:

`c882233f589fb8c809a7d3aaa12893172e026d3c`

The attempted checkout contains the one-line compiler repair that removes the redundant literal timeout assertion which previously triggered MSTEST0032. Comparison from prior branch head `8f5e9a0983beafe247a11f62922b8d2bd5974faa` to this candidate is exactly one deleted test assertion and no product-source change.

## Checkout and host authority

Observed before execution:

- exact detached HEAD: `dc7ac428f00041f71f3ae78baa9b7bf83c61ce55`;
- tracked tree clean;
- staged index clean;
- only observed nonmaterial untracked file: root-level `patch0012-local-edit.txt`;
- `PROCESSOR_ARCHITECTURE=ARM64`;
- Windows `10.0.26200`;
- .NET SDK `9.0.317` selected by repository `global.json`;
- `dotnet --info` RID `win-arm64`;
- Host Architecture `arm64`.

## Core tests

PASS:

- total: `622`;
- succeeded: `622`;
- failed: `0`;
- skipped: `0`;
- native exit: `0`.

## Harness compile and test execution

The Harness project and Harness test project both compiled successfully far enough for the native ARM64 test assembly to execute. The earlier MSTEST0032 compiler blocker is therefore closed at this candidate.

Native Harness test result:

- total: `88`;
- succeeded: `87`;
- failed: `1`;
- skipped: `0`;
- test command exit: `1`.

The command output named the generated test log path but did not include the failing test name or assertion text in the returned transcript. That diagnostic must be retrieved before any repair is designed. No failing-test identity is inferred by this record.

## Later validation stages

The validation function throws immediately when the Harness test exit is nonzero. Therefore the following hardening stages were **NOT EXECUTED** in Attempt 01:

- explicit clean-output native ARM64 Harness build;
- Missing Raft fixture smoke;
- generic fixture smoke;
- credentialless `e0a-run` provider-edge gate;
- post-validation cleanliness block inside the validation function.

No result from those stages may be claimed for `dc7ac428...` from this attempt.

No provider credential was intentionally set by the command set and no provider-network action was authorized.

## Classification

```text
Checkout / working-tree authority        PASS
Native Windows ARM64 host probes         PASS
Core tests                               PASS 622/622
Harness compilation                      PASS to native test execution
Harness tests                            FAIL 87/88
Fresh explicit Harness build             NOT EXECUTED
Missing Raft smoke                       NOT EXECUTED
Generic fixture smoke                    NOT EXECUTED
Credentialless provider-edge probe       NOT EXECUTED
Provider execution                       NOT AUTHORIZED / NOT PERFORMED
Overall hardening native validation      FAIL — DIAGNOSE 1 HARNESS TEST
```

## Next gate

Retrieve the exact failing Harness test name, failure message, and stack/assertion output from the emitted native test log for this exact attempt. Engineering must diagnose that evidence against the repository before changing source or tests.

Any correction creates a new executable/test checkpoint and requires a fresh full native validation run from the beginning. The historical live-host machine-tested authority remains unchanged until such a later candidate passes completely.
