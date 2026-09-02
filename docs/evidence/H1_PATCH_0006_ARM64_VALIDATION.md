# H1 Patch 0006 — Native Windows ARM64 Validation

Status: COMPLETE for exercised gates

## Scope

Patch 0006 implements the approved Performer Candidate Output Contract only.

## Native Windows ARM64 validation

### Harness build/runtime authority head

`fa37653d5c3e492e53d0f927a9c0844e139f12eb`

User-supplied native Windows ARM64 evidence on 2026-09-02:

#### Harness build

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

#### Missing Raft Harness regression

Result: PASS; exit `0`.

#### Generic smoke Harness regression

Result: PASS; exit `0`.

### Corrected full Core test authority head

Machine-tested branch head:

`93d549b2ca02db81a87596c0936f29f7c89058db`

Command:

```text
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

Result:

- restore PASS;
- `Ensemble.E0.Core` rebuilt successfully;
- `Ensemble.E0.Core.Tests` build PASS;
- full Core test execution PASS;
- total: `173`;
- failed: `0`;
- succeeded: `173`;
- skipped: `0`.

## Analyzer correction lineage

The initial Core test attempt at `fa37653...` was blocked before execution by two `MSTEST0032` warnings-as-errors in `PerformerCandidateContractTests.cs`.

Those assertions were tautological compile-time literal-vs-constant checks. They were removed while retaining the meaningful runtime semantic-vs-transport version assertion.

Corrected executable/test head:

`175f25ff8055943cdd900b08c1b89bea8b887692`

GitHub comparison from `fa37653...` to `175f25...` changes only:

- `tests/Ensemble.E0.Core.Tests/Performer/PerformerCandidateContractTests.cs`.

No production source changed.

The later machine-tested head `93d549b...` adds this evidence document only beyond that corrected test state. Therefore:

- Harness build/runtime evidence at `fa37653...` applies to the same production source content;
- the corrected full 173-test gate at `93d549b...` validates the same production implementation with the analyzer-only test correction;
- documentation-only commits do not increase executable validation authority.

## Exercised Patch 0006 guarantees

The passing Core suite exercises the approved candidate boundary, including:

- semantic `CandidatePerformance` contract version separation from AI JSON transport version;
- strict `ContextPacket + UTF-8 candidate JSON` construction path;
- grammar-open visible Performance with exact text preservation;
- silence invariants;
- display-bearing Unicode requirement;
- NFC/control-character enforcement;
- optional address and nomination control;
- exact roster/self/duplicate/case-sensitive control validation;
- roster-derived address bound during streaming preflight;
- 1 MiB inclusive parser ceiling and JSON depth 8;
- strict duplicate/unknown/missing/property/token/BOM/comment/trailing-content handling;
- sanitized candidate-domain failures without untrusted-value echo;
- candidate/control non-public constructors and narrow public authority surface;
- no PerformanceKind taxonomy or provider/rendering/mutation/Take/Director authority fields;
- frozen Missing Raft StructuredContextHash and RenderedContextHash regression;
- frozen Missing Raft ECJ-1 `9112` bytes/hash regression.

## Validation authority

- Static/adversarial review: PASS advisory only.
- Native Windows ARM64 Harness build: PASS at `fa37653...`.
- Native Windows ARM64 Core tests: PASS — 173/173 at `93d549b...`.
- Missing Raft Harness runtime: PASS/0 at `fa37653...`.
- generic smoke Harness runtime: PASS/0 at `fa37653...`.

The tested evidence is intentionally split by exact commit because the only post-Harness correction was test-only and the later evidence closure is documentation-only.

## Nonclaims

This evidence does not establish:

- provider/model execution or quality;
- provider request/system-contract behavior;
- provider-attempt provenance persistence;
- Director behavior;
- Integrity Validator acceptance/rejection;
- State Interpreter or State Authority;
- Take semantics;
- atomic causal commit/persistence;
- E0-D bindings;
- Windows AI/NPU execution/performance;
- WinUI;
- WACK;
- Microsoft Store certification.

Patch 0006 validation proves only the exercised deterministic Performer candidate contract boundary and its regressions.