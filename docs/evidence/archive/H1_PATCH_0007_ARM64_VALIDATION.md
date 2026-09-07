# H1 Patch 0007 — ARM64 Validation Evidence

Status: PASS for exercised Patch 0007 gates
Date: 2026-09-02
Machine authority: user's native Windows ARM64 development machine

## Validated implementation identity

Implementation branch:
`h1-patch-0007-director-opportunity-implementation`

Machine-tested executable/test head:
`aa9cd908194f414801fa0ed1bebea62298f798e2`

Approved baseline:
`5836bee7ec231ad94dccb6b26118046f1cfdabcc`

Canonical specification:
`docs/blueprint/H1_PATCH_0007_DIRECTOR_OPPORTUNITY_CONTRACT.md`

Implementation PR:
#16 — `H1: implement E0 Director opportunity contract`

## Native ARM64 build gate

Command:

```powershell
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
```

Observed result at exact head `aa9cd908...`:

- restore complete;
- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Harness` succeeded;
- Harness output target: `net9.0\win-arm64`;
- build succeeded;
- observed duration: 3.1s.

Result: **PASS**.

This is compiler authority for the exercised native ARM64 Core/Harness build at the tested head. It does not establish Store, WACK, NPU, or broader runtime claims.

## Full Core test gate

Command:

```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

Observed result at exact head `aa9cd908...`:

- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Core.Tests` succeeded;
- test execution succeeded;
- total: `211`;
- succeeded: `211`;
- failed: `0`;
- skipped: `0`;
- observed test duration: 1.5s;
- command build succeeded in 3.5s.

Result: **PASS — 211/211**.

Patch 0007 contributes 38 Director test executions over the previously validated 173-test Patch 0006 baseline.

The test surface exercises the approved deterministic Director boundary, including:

- Missing Raft opening VOSS fixture authority;
- least-privilege `DirectorOpportunityInput.Bind(...)`;
- structural validation distinct from causal-history authentication;
- Context/Candidate identity binding;
- exact opportunity-history order, including repeated effective Character IDs;
- nomination > addressed-pool recency > roster recency;
- never-seen and ordinal tie behavior;
- Candidate VisibleText isolation;
- silence fallback;
- diagnostics-only never-opportunitied/repeated-same/A-B-A-B detection;
- no fairness/application/Take/State/provider authority surface;
- deterministic repeated Bind/Propose behavior;
- frozen Missing Raft Context identities;
- frozen ECJ-1 9,112-byte fixture identity.

## Missing Raft Harness regression

Command:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json
$LASTEXITCODE
```

Observed output:

```text
Fixture validated: ensemble.e0.missing-raft@0.1.0
0
```

Result: **PASS / exit 0**.

## Generic smoke Harness regression

Command:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
$LASTEXITCODE
```

Observed output:

```text
Fixture validated: ensemble.e0.smoke@0.1.0
0
```

Result: **PASS / exit 0**.

## Scope verified at machine-tested head

Before the machine gate, GitHub comparison from approval checkpoint `5836bee7...` to tested head `aa9cd908...` contained exactly three implementation files:

1. `src/Ensemble.E0.Core/Director/DirectorOpportunityModels.cs` — added;
2. `src/Ensemble.E0.Core/Director/LeastInterventionDirector.cs` — added;
3. `tests/Ensemble.E0.Core.Tests/Director/LeastInterventionDirectorTests.cs` — added.

No existing Access, Context, Performer, Fixture, Harness, project configuration, persistence, UI, Windows AI/NPU, packaging, or Store source changed in the machine-tested implementation delta.

Documentation-only closure commits may follow this machine-tested head. They do not increase executable validation authority and must not be represented as newly tested executable content.

## Validation boundary

This evidence establishes only the exercised Patch 0007 deterministic Director calculation boundary:

`ContextPacket + CandidatePerformance + supplied opportunity history`
`-> structurally validated DirectorOpportunityInput`
`-> pure LeastInterventionDirector proposal/evaluation`

It does **not** establish or implement:

- authoritative opportunity-history persistence/authentication;
- accepted Take or TakeId semantics;
- Integrity Validator;
- State Interpreter or deterministic State Authority;
- atomic source Performance+consequence commit;
- effective Current Opportunity mutation;
- atomic opportunity establishment/history append;
- next-Performer triggering or Scene loop;
- provider/model integration or model quality;
- World Resolver;
- E0-D round-robin execution;
- Windows AI / NPU execution;
- WinUI;
- WACK or Store certification.

Static/adversarial review remains advisory. The ARM64 build output, target-machine test output, and exercised Harness runtime output above are the applicable machine authorities for this patch.
