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
H1 Patch 0001 is merged to `main` and validated for native Windows ARM64 compilation plus process-start/runtime-guard behavior.

Patch 0001a on `h1-hygiene-baseline` has now also passed its required machine gate:
1. Ensemble Engineering Hygiene Constitution and baseline hygiene audit.
2. Strong-ID `ToString()` fail-closed correction for default/uninitialized structs.
3. Stable .NET 9 SDK policy (`9.0.100` + `latestFeature`, prerelease disabled).
4. One canonical `tests/Ensemble.E0.Core.Tests` project using MSTest SDK / Microsoft.Testing.Platform.
5. Regression tests proving valid strong IDs stringify correctly and default/uninitialized strong IDs fail closed.

## Baseline hygiene audit decisions
- Preserve `CanonicalId`, `StrictJsonPreflight`, `FixtureValidationException`, Core/Harness dependency direction, warnings-as-errors, deterministic build settings, native ARM64 Harness targeting, and one canonical Core test project.
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
- Native machine evidence confirms Windows `win-arm64`, .NET host architecture `arm64`, .NET SDK 9.0.317 installed, and .NET runtime 9.0.19 installed.
- Patch 0001a machine validation on 2026-09-01 used SDK 9.0.317 under the stable .NET 9 roll-forward policy.
- `dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug` succeeded; Core and Harness compiled successfully, with Harness output targeting `net9.0\win-arm64`.
- `dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug` succeeded: 2 total, 2 succeeded, 0 failed, 0 skipped.
- `dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build` started successfully on the same target machine, passed the Windows/ARM64 architecture guard, reached the expected missing-fixture usage path, and returned exit code `2`.
- Detailed Patch 0001a evidence is recorded in `docs/evidence/H1_PATCH_0001A_ARM64_VALIDATION.md`.
- No fixture semantic validation, Access Control, Context Composer, persistence, provider behavior, Windows AI/NPU execution, packaging, WACK, or Store validation exists yet.

## Immediate next action
Complete the final PR #2 hygiene/scope review and merge Patch 0001a if the diff remains clean. Then begin Patch 0002.1 design/implementation: typed E0 Fixture Dialect v1 plus generic semantic validation using the existing Core test project. Do not add hashing or Access Control until Patch 0002.1 is separately reviewed and machine-validated.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
