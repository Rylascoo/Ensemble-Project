# Ensemble Current State

Updated: 2026-09-01

## Authority
- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law for implementation quality and cleanup unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository for UI/UX architecture, mockups/prototypes, visual identity/artwork, motion/animation, Store/marketing assets, and research/reference material.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

## Validated baseline
H1 Patch 0001 is merged to `main` and has:
1. .NET 9 Core + console Harness skeleton.
2. Strong typed IDs.
3. Strict fixture JSON ingestion/preflight.
4. Fixture envelope bootstrap DTOs.
5. Validation entrypoints.
6. Native `win-arm64` Harness target and runtime architecture guard.
7. Successful native Windows ARM64 compiler validation.
8. Successful process-start/runtime-guard sanity validation.

## Active correction boundary
Branch: `h1-hygiene-baseline`
Draft PR: #2 `H1: establish engineering hygiene baseline`

Patch 0001a contains only:
1. Ensemble Engineering Hygiene Constitution and baseline hygiene audit.
2. Strong-ID `ToString()` fail-closed correction for default/uninitialized structs.
3. `global.json` policy changed from exact feature-band pinning to supported stable .NET 9 (`9.0.100` + `latestFeature`, prerelease disabled).
4. `tests/Ensemble.E0.Core.Tests` regression-test foundation using MSTest SDK / Microsoft.Testing.Platform.
5. Tests proving valid strong IDs round-trip and default/uninitialized strong IDs reject stringification.
6. Documentation/checkpoint updates.

Do not add fixture-domain expansion, hashing, Access Control, persistence, providers, WinUI, Windows AI Foundry, NPU/QNN, Store packaging, or post-E0 product implementation to this correction.

## Baseline hygiene audit decisions
- Preserve `CanonicalId`, `StrictJsonPreflight`, `FixtureValidationException`, Core/Harness dependency direction, warnings-as-errors, deterministic build settings, and native ARM64 Harness targeting.
- Preserve one Core test project and extend it in Patch 0002.1 rather than introducing another test stack.
- Patch 0001 fixture-envelope types are bootstrap placeholders, not compatibility contracts.
- In Patch 0002.1, replace `FixtureEnvelopeDocument` and `FixtureEnvelopeValidator` directly with the typed E0 Fixture Dialect v1 model/validators; do not retain a legacy adapter.
- Remove fixture-authored `accessPolicies`; access will be governed by a known deterministic access contract plus authority category/ownership law.
- Correct fixture identity semantics in Patch 0002.1 so family identity and fully versioned authoritative fixture identity are distinct; do not preserve the current unversioned `FixtureId` interpretation as a compatibility alias.
- Update `FixtureLoader` directly to the new typed transport model when Patch 0002.1 lands.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine is test-execution authority for the tests actually exercised.
- Actual target-device execution: runtime authority for the behavior actually exercised.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Current validation state
- H1 Patch 0001 has been statically reviewed in GitHub.
- Native machine evidence confirms Windows `win-arm64`, .NET host architecture `arm64`, .NET SDK 9.0.317 installed, and .NET runtime 9.0.19 installed.
- On 2026-09-01, `dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug` succeeded on the target Windows ARM64 machine using SDK 9.0.317.
- `Ensemble.E0.Core` compiled successfully to `bin\Debug\net9.0\Ensemble.E0.Core.dll`.
- `Ensemble.E0.Harness` compiled successfully to `bin\Debug\net9.0\win-arm64\Ensemble.E0.Harness.dll`.
- `dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build` started successfully on the same target machine, passed the Windows/ARM64 architecture guard, reached the expected missing-fixture usage path, and returned exit code `2`.
- Patch 0001a has static review only until build, tests, and runtime sanity are executed on the target ARM64 machine.
- No fixture semantic validation, Access Control, Context Composer, persistence, provider behavior, Windows AI/NPU, packaging, WACK, or Store validation exists yet.

## Immediate next action
From `h1-hygiene-baseline`:
1. confirm `dotnet --version` selects a stable .NET 9 SDK;
2. build the Harness project;
3. run `dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`;
4. rerun the existing Harness runtime sanity check and verify exit code `2`.

If all gates are clean, merge Patch 0001a and begin Patch 0002.1: typed E0 Fixture Dialect v1 plus generic semantic validation using the existing Core test project.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
