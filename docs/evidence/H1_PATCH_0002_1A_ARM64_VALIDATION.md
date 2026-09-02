# H1 Patch 0002.1a — ARM64 Validation Evidence

Date: 2026-09-01
Tested executable implementation head: `365537456f5890f3e2766abeb833f974c8fd7d8e`
Target: user's native Windows ARM64 development machine

## Scope
Patch 0002.1a adds one foundational invariant: the complete validated-fixture provenance graph must be acyclic. The implementation uses iterative topological validation and adds a regression test for a two-record provenance cycle.

## Compiler gate
Command:
`dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug`

Result: PASS.
- `Ensemble.E0.Core` succeeded.
- `Ensemble.E0.Harness` succeeded.
- Harness output: `net9.0\win-arm64`.
- Build completed in 2.5 s.

## Test gate
Command:
`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`

Result: PASS.
- total: 31
- succeeded: 31
- failed: 0
- skipped: 0
- test duration: 0.7 s
- command/build duration: 2.8 s

This includes the new provenance-cycle regression test.

## Runtime regression gate
Command:
`dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json`

Observed output:
`Fixture validated: ensemble.e0.smoke@0.1.0`

Observed process exit code: `0`.

Result: PASS. The previously validated generic fixture path remains functional with the new DAG invariant.

## Validation boundary
This evidence establishes compiler, test-execution, and runtime-regression authority only for the behavior exercised above. It does not validate Missing Raft-specific semantics, ECJ-1/hash, Access Control, Context Composer, persistence, provider/AI behavior, Windows AI/NPU execution, WinUI, packaging, WACK, or Store behavior.

## Hygiene result
The correction remains limited to the provenance-DAG invariant, its construction-boundary hook, one regression test, checkpoint documentation, and this evidence record. No schema or product-scope expansion occurred.