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
H1 Patch 0001 and Patch 0001a are merged to `main`.

The current baseline includes:
1. .NET 9 Core + native Windows ARM64 console Harness skeleton.
2. Strong typed IDs with fail-closed behavior for default/uninitialized values.
3. Strict fixture JSON ingestion/preflight bootstrap boundary.
4. Fixture-envelope bootstrap DTOs and validation entrypoints, explicitly scheduled for direct replacement in Patch 0002.1.
5. Stable .NET 9 SDK policy (`9.0.100` + `latestFeature`, prerelease disabled).
6. One canonical `tests/Ensemble.E0.Core.Tests` project using MSTest SDK / Microsoft.Testing.Platform.
7. Ensemble Engineering Hygiene Constitution and baseline hygiene audit.
8. Successful native Windows ARM64 compiler, test-execution, and process-start/runtime-guard validation for the behavior exercised so far.

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
- Patch 0001a validation on 2026-09-01 used SDK 9.0.317 under the stable .NET 9 roll-forward policy.
- Harness/Core build succeeded, with Harness output targeting `net9.0\win-arm64`.
- Core regression tests succeeded: 2 total, 2 succeeded, 0 failed, 0 skipped.
- Harness process started successfully, passed the Windows/ARM64 architecture guard, reached the expected missing-fixture usage path, and returned exit code `2`.
- Detailed evidence is recorded in `docs/evidence/H1_PATCH_0001_ARM64_BUILD.md`, `docs/evidence/H1_PATCH_0001_ARM64_RUNTIME_SANITY.md`, and `docs/evidence/H1_PATCH_0001A_ARM64_VALIDATION.md`.
- No fixture semantic validation, Access Control, Context Composer, persistence, provider behavior, Windows AI/NPU execution, packaging, WACK, or Store validation exists yet.

## Immediate next action
Begin Patch 0002.1: typed E0 Fixture Dialect v1 plus generic semantic validation using the existing Core test project. Replace the bootstrap fixture DTO/validator surfaces directly rather than layering compatibility adapters. Do not add ECJ-1 hashing, Access Control, Context Composer, or later H1 machinery until Patch 0002.1 is separately reviewed and machine-validated.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
