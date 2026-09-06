# E0-A Phase B Live-Host Completion — Native Validation Attempt 01

Status: **DIRECTOR-MACHINE-SOURCED — PARTIAL PASS; HARNESS TEST COMPILATION AND NO-KEY ORACLE REQUIRE CORRECTION**

Date: 2026-09-06

This evidence is sourced from commands executed by the Director on the native Windows ARM64 machine. It does not authorize credentials, provider requests, network inference, or spend.

## Exact checkout

- Branch: `e0a-phase-b-live-host-completion`
- Tested HEAD: `bd04749a20141d1f7d221700ad3f1d3ee9f33eb1`
- `origin/main`: `13ffde90f78a8bf40a04381b136e846d1cf64d1c`
- Initial tracked diff exit: `0`
- Initial staged diff exit: `0`
- Non-material untracked file: `patch0012-local-edit.txt`
- No material untracked file under `src/`, `tests/`, or `fixtures/`.

## Native ARM64 environment

- `PROCESSOR_ARCHITECTURE=ARM64`
- .NET SDK: `9.0.317`
- `dotnet --info` RID: `win-arm64`
- `dotnet --info` Host Architecture: `arm64`
- Harness output path: `net9.0\win-arm64`

The PowerShell expressions using `[System.Runtime.InteropServices.RuntimeInformation]` returned blank values in this Windows PowerShell host and therefore caused two custom probe exceptions. This is a validation-command defect, not contrary architecture evidence: `dotnet --info` and `PROCESSOR_ARCHITECTURE` independently establish the required native ARM64 environment. Future validation must use the authoritative `dotnet --info`/environment checks rather than the broken PowerShell reflection probe.

## Credential state

`OPENAI_API_KEY` was explicitly removed before tests and the live-host smoke. No provider credential was present or introduced by the validation.

No real provider request or spend was authorized or performed.

## Results

### Core tests

PASS.

- total: `622`
- failed: `0`
- succeeded: `622`
- skipped: `0`
- exit: `0`

### Harness tests

FAIL TO COMPILE due to test-oracle analyzer diagnostics.

- Harness production project compiled successfully as a dependency.
- Harness test project produced seven `MSTEST0032` errors in `E0ALiveHostReadinessTests.cs`, lines 99–105.
- The seven diagnostics correspond to direct `Assert.AreEqual` comparisons between literals and C# `const` pricing-policy fields in `PricingPolicy_FreezesSourcePromotionAndConservativeRates`.
- This is the same analyzer class previously encountered when direct assertions compare compile-time constants; it does not establish a production-code failure.
- Harness test exit: `1`.

Required correction: replace the seven direct compile-time tautological assertions with a non-tautological collection/runtime oracle without suppressing MSTest analyzers and without changing production behavior.

### Harness native ARM64 build

PASS.

- exit: `0`
- output: `src\Ensemble.E0.Harness\bin\Debug\net9.0\win-arm64\Ensemble.E0.Harness.dll`

### Missing Raft fixture smoke

PASS.

- output: `Fixture validated: ensemble.e0.missing-raft@0.1.0`
- exit: `0`

### Generic fixture smoke

PASS.

- output: `Fixture validated: ensemble.e0.smoke@0.1.0`
- exit: `0`

### Credentialless live-host smoke

The executable emitted the expected refusal text:

`OPENAI_API_KEY is required at the E0-A provider edge.`

No evidence root was created and the credential remained absent.

However, the scripted oracle is **not counted as PASS** because `$ErrorActionPreference='Stop'` caused PowerShell to convert the native command's stderr into a terminating `NativeCommandError`. As a result:

- `$NoKeyOutput` was empty;
- `$LASTEXITCODE` was observed as `-1` rather than the executable's intended exit code;
- the subsequent message-capture assertion could not evaluate the emitted refusal text.

This is a validation-command/wrapper defect. Future validation must temporarily use non-terminating native-command error handling while capturing `2>&1`, then restore the prior PowerShell error preference and assert the actual native exit code, refusal text, absent credential, and absent evidence root.

The observed console refusal and absent evidence root are supporting evidence that the intended host gate fired, but they do not satisfy the scripted oracle on Attempt 01.

## Post-validation integrity

- post-validation HEAD: `bd04749a20141d1f7d221700ad3f1d3ee9f33eb1`
- post tracked diff exit: `0`
- post staged diff exit: `0`
- command set reached `VALIDATION_COMMAND_SET_COMPLETE` in the interactive shell despite the earlier thrown validation assertions.

## Classification

Attempt 01 is **partial native evidence only**. It does not establish native-validation completion because the Harness test project did not compile and the credentialless live-host assertion wrapper was defective.

The production Harness itself built successfully for native Windows ARM64, both fixture smokes passed, and Core remained 622/622. The correction must remain test/validation-oracle only unless a later native run reveals a production defect.
