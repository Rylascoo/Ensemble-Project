# H1 Patch 0015 Native ARM64 Validation

Status: PASS

Patch: H1 Patch 0015 — Recent Performance Context Continuity

Implementation branch:

`h1-patch-0015-accepted-performance-history-implementation`

## Machine authority

Validation was performed by the user on a native Windows ARM64 machine.

Environment evidence:

- `PROCESSOR_ARCHITECTURE=ARM64`
- OS: Windows `10.0.26200`
- .NET SDK: `9.0.317`
- .NET host: `10.0.11`
- host architecture: `arm64`
- RID: `win-arm64`
- active `global.json`: repository root

No x86/x64 emulation claim is made.

## Working-tree authority

At successful Core-test head:

`b890b7eca66c391fae3ec30af0442dcc0e9f6aec`

Observed before the successful rerun:

- `git diff --quiet` -> `TRACKED_DIFF_EXIT=0`
- `git diff --cached --quiet` -> `STAGED_DIFF_EXIT=0`

The unrelated untracked `patch0012-local-edit.txt` remained outside tracked/staged cleanliness checks and was not modified.

## Native validation attempt 01

Initial exact branch head:

`5cb055e6dddea721aee98fee7f633191543e6490`

Results:

- Core production project compiled successfully.
- Core test project compiled successfully.
- Core tests: `570/571` PASS, `1` FAIL, `0` skipped.
- Harness build: PASS.
- Missing Raft fixture execution: PASS.
- Generic smoke fixture execution: PASS.

The only failure was:

`BindingOwnsTheSingleSourceSnapshotProofAndCommitDoesNotRepeatIt`

The failure was an inherited IL-structure assertion that expected `ProductionStateAuthoritySnapshot.Bind` to remain directly in the public historical `E0TakeStateBinding.Bind` wrapper. Patch 0015 intentionally centralized both public binding entry points through shared private `BindCore`, where the source snapshot proof still occurs exactly once.

No production implementation correction was required.

## Patch-first correction

Correction commit:

`b890b7eca66c391fae3ec30af0442dcc0e9f6aec`

Changed executable/test surface relative to `5cb055e6...`:

- one inherited test file only:
  - `tests/Ensemble.E0.Core.Tests/CausalCommit/Patch0012StructuralImplementationTests.cs`

The corrected structural assertion now proves:

1. historical `Bind(...)` calls shared `BindCore(...)` exactly once;
2. `BindWithAcceptedHistory(...)` calls the same `BindCore(...)` exactly once;
3. neither public wrapper directly repeats the source snapshot proof;
4. `BindCore(...)` performs exactly one `ProductionStateAuthoritySnapshot.Bind(...)`;
5. `BindCore(...)` performs exactly one `StateAuthoritySnapshotSemanticComparer.Equals(...)`;
6. `DeterministicCausalCommit.Commit(...)` still performs neither source snapshot proof operation.

No Core production source, Harness source, fixture, canonical oracle, or Patch 0015 production test changed in this correction.

## Targeted native correction validation

At exact head:

`b890b7eca66c391fae3ec30af0442dcc0e9f6aec`

Command:

`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug --filter "FullyQualifiedName~BindingOwnsTheSingleSourceSnapshotProofAndCommitDoesNotRepeatIt"`

Observed result:

- Core project compiled successfully.
- Core test project compiled successfully.
- targeted test invocation succeeded.
- test platform summary reported `571` total, `571` succeeded, `0` failed, `0` skipped under the filtered invocation behavior.
- `TARGETED_TEST_EXIT=0`.

## Full Core native validation

At exact head:

`b890b7eca66c391fae3ec30af0442dcc0e9f6aec`

Command:

`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`

Observed result:

- Core project compiled successfully.
- Core test project compiled successfully.
- `571/571` tests succeeded.
- `0` failed.
- `0` skipped.
- build succeeded.
- `CORE_TEST_EXIT=0`.

This is the exact successful full Core-test machine authority for Patch 0015.

## Harness and fixture machine authority

Harness and fixture validation was performed at exact head:

`5cb055e6dddea721aee98fee7f633191543e6490`

Results:

- Harness build: PASS, `HARNESS_BUILD_EXIT=0`.
- Missing Raft fixture: `Fixture validated: ensemble.e0.missing-raft@0.1.0`, `MISSING_RAFT_EXIT=0`.
- Generic smoke fixture: `Fixture validated: ensemble.e0.smoke@0.1.0`, `GENERIC_SMOKE_EXIT=0`.

The only subsequent repository change before successful Core validation was the documentation record of validation attempt 01 plus the inherited test-only structural assertion correction. Comparison `5cb055e6... -> b890b7e...` contains no `src/` or fixture changes. Therefore the exact Harness/fixture machine authority remains the `5cb055e6...` execution, while the exact full Core-test authority is `b890b7e...`.

## Validation authority boundary

Established:

- native Windows ARM64 Core compilation: PASS;
- native Windows ARM64 Core test execution: PASS (`571/571`);
- native Windows ARM64 Harness build: PASS;
- Missing Raft fixture execution: PASS;
- generic smoke fixture execution: PASS.

Not established by this evidence:

- WinUI runtime behavior;
- Windows AI Foundry execution;
- NPU execution;
- WACK/package validation;
- Microsoft Store certification.

Those remain separate future gates.
