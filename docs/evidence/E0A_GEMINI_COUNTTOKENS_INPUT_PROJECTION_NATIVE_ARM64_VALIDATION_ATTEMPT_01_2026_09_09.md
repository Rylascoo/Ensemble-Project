# E0-A Gemini `countTokens` Input Projection — Native Windows ARM64 Validation Attempt 01

Status: **FAIL — HARNESS 130/131; DIAGNOSED STALE TEST ORACLE; DOWNSTREAM NATIVE PACKAGE NOT EXECUTED**

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

The validation wrapper correctly stopped at this point.

## Read-only failure-log extraction

The Director subsequently read the existing MSTest log without rerunning the suite.

Log path:

`tests\Ensemble.E0.Harness.Tests\bin\Debug\net9.0\TestResults\Ensemble.E0.Harness.Tests_net9.0_arm64.log`

Log SHA-256:

`1A79B95C4683FD0B212721E03ADCD8335730292415604D845793BA50392A18BF`

Exact failing test:

`GeminiModelComparisonTests.GenerateContentPort_AcceptsEveryApprovedComparisonModelForSchemaCompleteTokenPreflight`

Exact assertion:

```text
Assert.AreEqual failed. Expected:<5>. Actual:<3>.
expected: generationBody.RootElement.EnumerateObject().Count() + 1
actual: nested.EnumerateObject().Count()
```

Stack trace localizes the failure to `GeminiModelComparisonTests.cs:109`.

The diagnostic extraction also established:

- validation HEAD still exact candidate `de38d5d52279c22a1786e11200239c445e04377b`;
- tracked diff exit `0`;
- staged diff exit `0`;
- material untracked files under `src` / `tests` / `fixtures`: none;
- historical root still `689655eed677b789ab3ee395f1c65b4f2cb72cc8`;
- process Gemini/OpenAI keys absent;
- provider network not requested.

## Failure classification

**STALE TEST ORACLE — NOT A PRODUCTION DEFECT.**

Q-E0A-04 intentionally changed the Gemini `countTokens` nested request from the prior schema-complete copy to the frozen input-semantic projection:

- `model`;
- `systemInstruction`;
- `contents`.

The failed comparison test retained the superseded expectation that every prepared generation field plus nested `model` must be copied into `countTokens`, hence expected five fields. The corrected production implementation and dedicated Q-E0A-04 wire oracle both require exactly three nested fields and exclude `generationConfig` / `store` from token preflight.

No source/runtime change is justified by this failure.

Test-only correction commit:

`48a6e67a5f5834b40bcca1b530b87f537140984e`

The correction changes the comparison oracle across every approved Gemini comparison model to require the same three-field input-semantic projection, verify exact hashes for `systemInstruction` and `contents`, and verify that the original generation body still retains `generationConfig` and `store=false`.

Hosted Validation gate `34402647899` completed **SUCCESS** for that test-only correction.

## Hard-stop consequence

Attempt 01 did **not** establish results for:

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

No result from those unexecuted stages may be inferred.

## Provider/network scope

The packet removed both provider-key process variables before the test suite and failed during local Harness tests before any live-run probe. The execution and later log extraction contain no provider request, inference, generation, or spend. Provider authorization remains **NONE**.

## Authority consequence

- candidate `de38d5d52279c22a1786e11200239c445e04377b` is **NOT native-machine-validated**;
- do **not** create a validation tag for it;
- promoted native authority remains `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`;
- Q-E0A-04 remains active for a complete Native Attempt 02 on a new exact candidate containing the test correction;
- preserve the Attempt-01 worktree/log as historical evidence; do not repurpose it for Attempt 02;
- provider authorization remains **NONE**;
- Q-E0A-03, E0-B+, and E0-E execution remain blocked.
