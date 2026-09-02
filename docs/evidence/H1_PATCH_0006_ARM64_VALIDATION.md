# H1 Patch 0006 — Native Windows ARM64 Validation

Status: validation in progress — corrected Core test rerun required

## Scope

Patch 0006 implements the approved Performer Candidate Output Contract only.

## Initial machine-tested implementation head

`fa37653d5c3e492e53d0f927a9c0844e139f12eb`

User-supplied native Windows ARM64 evidence on 2026-09-02:

### Harness build

Command:

```text
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
```

Result:

- restore PASS;
- `Ensemble.E0.Core` build PASS;
- `Ensemble.E0.Harness` build PASS;
- Harness output target: `net9.0\win-arm64`;
- overall build PASS.

### Initial Core test gate

Command:

```text
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

Result:

- Core build PASS;
- test project compilation blocked by two `MSTEST0032` analyzer errors in `PerformerCandidateContractTests.cs`;
- both errors were tautological assertions comparing compile-time contract constants with their own literal values;
- tests did not execute at this head.

This was a test-only analyzer failure, not a production compile/runtime failure.

### Missing Raft Harness regression

Result: PASS; exit `0`.

### Generic smoke Harness regression

Result: PASS; exit `0`.

## Targeted correction

Corrected executable/test head:

`175f25ff8055943cdd900b08c1b89bea8b887692`

GitHub comparison from the initial machine-tested head `fa37653...` to `175f25...` changes only:

- `tests/Ensemble.E0.Core.Tests/Performer/PerformerCandidateContractTests.cs`

Correction:

- removed two tautological literal-vs-constant assertions flagged by `MSTEST0032`;
- retained the meaningful runtime assertion that parsed `CandidatePerformance.ContractVersion` equals the semantic contract and differs from the AI JSON transport schema;
- no production source changed;
- expected total test executions remain 173.

Because production source is byte-identical between `fa37653...` and `175f25...`, the native Harness build/runtime evidence above applies to the same production source that will be exercised by the corrected Core test rerun.

A later evidence-only commit is a documentation descendant of `175f25...`; it does not change executable or test content and does not increase validation authority.

## Remaining machine gate

Run the full Core suite at the current branch head. The executable/test content is exactly the corrected `175f25...` state.

Expected:

- test project compiles with warnings-as-errors;
- total 173;
- failed 0;
- succeeded 173;
- skipped 0.

Patch 0006 is not machine-validation complete until that corrected test gate passes.

## Nonclaims

This evidence does not establish provider/model behavior, Director behavior, Integrity acceptance, State Interpreter/Authority, Take semantics, causal persistence, NPU execution, WinUI, WACK, or Store certification.
