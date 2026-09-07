# H1 Patch 0002.2 — ARM64 Validation Evidence

Date: 2026-09-01
Tested executable implementation head: `33a8a9f96d2320260619ac28309f0f40bee8e19e`
Target: user's native Windows ARM64 development machine

## Scope
Patch 0002.2 adds the canonical Missing Raft fixture, fixture-specific structural validation over the existing immutable `ValidatedFixture`, minimal Harness family dispatch, and mutation-style contract tests while preserving E0 Fixture Dialect v1 and the generic validation path.

## Compiler gate
Command:
`dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug`

Result: PASS.
- `Ensemble.E0.Core` succeeded.
- `Ensemble.E0.Harness` succeeded.
- Harness output targeted `net9.0\win-arm64`.
- Build completed in 2.7 s.

## Test gate
Command:
`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`

Result: PASS.
- total: 62
- succeeded: 62
- failed: 0
- skipped: 0
- test duration: 1.0 s
- command/build duration: 2.8 s

The passing suite includes the Missing Raft mutation tests, the direct requirement that the generic smoke fixture remains generically valid but fails explicit `MissingRaftContract.Validate`, and the existing generic provenance-DAG regression coverage.

## Canonical Missing Raft runtime gate
Command:
`dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json`

Observed result:
`Fixture validated: ensemble.e0.missing-raft@0.1.0`

Observed process exit code: `0`.

Result: PASS.

## Generic smoke regression gate
Command:
`dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json`

Observed result:
`Fixture validated: ensemble.e0.smoke@0.1.0`

Observed process exit code: `0`.

Result: PASS. Unknown/non-Missing-Raft generic fixture families remain on the generic validation path.

## Source-content review
PASS against the approved `H1_PATCH_0002_2_MISSING_RAFT_CONTRACT.md` semantic gate.

Confirmed before machine validation:
- no approved fact/state was intentionally omitted;
- no extra dramatic premise, moral winner, or forced confession/accusation/revelation/reconciliation/solution was introduced;
- raft failure remains unresolved;
- the strengthened current did not release the correctly secured mooring;
- Wren observed Marlowe's later shoreline return, not the release;
- Voss's accidental-loss explanation remains a plausible belief rather than Production truth;
- Marlowe's Character-owned knowledge does not gain Wren-private observation or an unsupported claim that he knew Wren correctly secured the mooring.

## Final hygiene/scope comparison
PASS against the Patch 0002.1a validated baseline.

The final implementation changes only five executable/test/fixture surfaces:
1. one canonical Missing Raft JSON source;
2. one fixture-specific `MissingRaftContract` over `ValidatedFixture`;
3. minimal Harness family dispatch;
4. one canonical-fixture link in the existing Core test project;
5. mutation-style Missing Raft contract tests.

No generic fixture schema fork, second fixture/domain representation, registry/plugin/factory framework, prose parser, compatibility layer, Access Control implementation, Context Composer, hashing, persistence, causal commits, provider/AI code, WinUI, Windows AI/NPU, packaging, WACK, or Store work was added.

## Validation boundary
This evidence establishes compiler authority, Core test-execution authority, and target-machine runtime authority only for the behavior exercised above at executable head `33a8a9f96d2320260619ac28309f0f40bee8e19e`.

It does not establish ECJ-1/SHA-256 fixture hashing, deterministic Access Control, Context Composer, Production state construction, persistence/causal commits, provider or AI behavior, Windows AI/NPU execution, WinUI behavior, packaging, WACK, or Store certification.

Documentation-only commits after the tested executable head do not increase the executable validation level.
