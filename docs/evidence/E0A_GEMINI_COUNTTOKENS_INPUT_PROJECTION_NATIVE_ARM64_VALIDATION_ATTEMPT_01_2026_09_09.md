# E0-A Gemini `countTokens` Input Projection — Native Windows ARM64 Validation Attempt 01

Status: **FAIL — HARNESS 130/131; DOWNSTREAM NATIVE PACKAGE NOT EXECUTED**

Date: **2026-09-09**

Candidate: `de38d5d52279c22a1786e11200239c445e04377b`

Branch at validation start: `e0a-gemini-counttokens-input-projection-correction`

Hosted prerequisite: Validation gate `34385049237` completed **SUCCESS** at the exact candidate before native execution.

## Submitted Director-machine evidence

The Director ran the guarded Q-E0A-04 native validation packet from the historical repository root and created a separate detached worktree at:

`C:\Users\Wiryl\Sol Dev\Ensemble-Project-Worktrees\e0a-counttokens-input-projection-native-arm64`

Observed preflight:

- historical root HEAD before validation: `689655eed677b789ab3ee395f1c65b4f2cb72cc8`;
- `origin/e0a-gemini-counttokens-input-projection-correction` resolved exactly to candidate `de38d5d52279c22a1786e11200239c445e04377b`;
- detached validation checkout resolved exactly to the candidate and was reported clean before testing;
- `PROCESSOR_ARCHITECTURE=ARM64`;
- .NET SDK `9.0.317`;
- Windows `10.0.26200`;
- `dotnet --info` RID `win-arm64`;
- Host `10.0.11`, architecture `arm64`;
- process `GEMINI_API_KEY` and `OPENAI_API_KEY` were absent before native test execution;
- stale Core/Harness/test `bin`/`obj` outputs were cleared before testing.

## Native results reached

### Core

```text
Test summary: total: 622, failed: 0, succeeded: 622, skipped: 0
CORE_TEST_EXIT=0
```

Result: **PASS — 622/622**.

### Harness

```text
Test summary: total: 131, failed: 1, succeeded: 130, skipped: 0
HARNESS_TEST_EXIT=1
```

Result: **FAIL — 130/131**.

MSTest reported its detailed result log at:

`tests\Ensemble.E0.Harness.Tests\bin\Debug\net9.0\TestResults\Ensemble.E0.Harness.Tests_net9.0_arm64.log`

The submitted console output did not include the failing test name, assertion expected/actual values, or stack trace. Those facts therefore remain **NOT ESTABLISHED** until that existing log is read.

## Hard-stop consequence

The validation wrapper correctly threw immediately on the Harness nonzero exit. Therefore Attempt 01 did **not** establish results for:

- the fresh post-test Harness build;
- Missing Raft fixture smoke;
- generic fixture smoke;
- Gemini 3.5 Flash-Lite credentialless boundary;
- Gemini 3.1 Flash-Lite credentialless boundary;
- Gemini 2.5 Flash-Lite credentialless boundary;
- retired Gemini 2.5 Flash live-selection rejection;
- final detached/clean checkout check;
- final provider-key absence check;
- final historical-root identity/status preservation check.

No result from those unexecuted stages may be inferred from earlier checkpoints or visible preflight state.

## Provider/network scope

The packet removed both provider-key process variables before the test suite and failed during local Harness tests before any live-run probe. The submitted execution contains no provider request, inference, generation, or spend. Provider authorization remains **NONE**.

## Classification boundary

This failed attempt does not by itself prove an ARM64-specific production defect. The hosted gate compiled but did not execute the Harness test suite. Q-E0A-04 changed only `GeminiGenerateContentPort.cs` plus `GeminiGenerateContentPortWireTests.cs`, replacing one countTokens wire oracle and adding one net Harness test. Exact failure classification must therefore wait for the native TestResults log rather than being inferred from architecture or changed-file proximity.

Do not patch source/tests until the failing test, expected/actual result, and stack trace are established from that log.

## Authority consequence

- candidate `de38d5d52279c22a1786e11200239c445e04377b` is **NOT native-machine-validated**;
- do **not** create a validation tag for it;
- promoted native authority remains `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`;
- Q-E0A-04 remains active for diagnosis/correction;
- provider authorization remains **NONE**;
- Q-E0A-03, E0-B+, and E0-E execution remain blocked.
