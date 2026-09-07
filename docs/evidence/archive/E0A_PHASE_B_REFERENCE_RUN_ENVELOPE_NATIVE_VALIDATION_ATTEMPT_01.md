# E0-A Phase B — Native Windows ARM64 Validation Attempt 01

Status: **DIRECTOR-MACHINE-SOURCED — PARTIAL PASS; HARNESS TEST COMPILATION FAILED**
Date: 2026-09-06

## Exact checkout

`a456a74ff6995ecfebc25a6134b2ca25d6260432`

Tracked and staged trees were clean before and after validation:

```text
TRACKED_DIFF_EXIT=0
STAGED_DIFF_EXIT=0
POST_TRACKED_DIFF_EXIT=0
POST_STAGED_DIFF_EXIT=0
```

One pre-existing untracked non-material file was present and excluded from validation scope:

`patch0012-local-edit.txt`

No untracked file existed under `src/`, `tests/`, or `fixtures/`.

## Director machine

PowerShell reported `PROCESSOR_ARCHITECTURE=ARM64`.

The attempted PowerShell `RuntimeInformation` static-property probe returned blank values in the shell and therefore is not architecture evidence. The authoritative .NET CLI environment report was:

```text
Windows 10.0.26200
RID win-arm64
SDK 9.0.317
SDK commit 26570c2743
MSBuild 17.14.51+25f168cee
Host 10.0.11
Host Architecture arm64
```

`OPENAI_API_KEY` was explicitly removed before validation. No provider request, credential use, or spend occurred.

## Results

### Existing Core tests — PASS

```text
total: 622
succeeded: 622
failed: 0
skipped: 0
CORE_TEST_EXIT=0
```

### New Harness tests — COMPILATION FAIL

`Ensemble.E0.Harness` compiled successfully as a dependency, but `Ensemble.E0.Harness.Tests` failed with six `MSTEST0032` analyzer errors in:

`tests/Ensemble.E0.Harness.Tests/E0AEnvelopeAndIdentityTests.cs`

All six diagnostics target `ApprovedEnvelopeConstants_AreExact` assertions whose actual values are C# `const` fields. MSTest 4.1 identifies those assertions as conditions known to be always true because the constant references are compiler-inlined. No runtime Harness test executed in this attempt.

```text
HARNESS_TEST_EXIT=1
```

This is a test-oracle construction defect, not evidence of a production Harness failure.

### Native ARM64 Harness build — PASS

```text
Ensemble.E0.Core succeeded
Ensemble.E0.Harness succeeded
HARNESS_BUILD_EXIT=0
```

### Missing Raft fixture smoke — PASS

```text
Fixture validated: ensemble.e0.missing-raft@0.1.0
MISSING_RAFT_EXIT=0
```

### Generic fixture smoke — PASS

```text
Fixture validated: ensemble.e0.smoke@0.1.0
GENERIC_SMOKE_EXIT=0
```

## Required correction

Replace the six tautological constant assertions with a non-constant-folded test oracle while preserving the exact approved constant values. Do not suppress `MSTEST0032`, change production constants, modify Core, or widen Proposal 0.15 scope.

After the test-only correction, rerun native validation against one exact resolved commit. Until then, E0-A Phase B remains pending native validation and is not promotion-ready.
