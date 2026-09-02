# H1 Baseline Hygiene Audit

Date: 2026-09-01
Baseline: `main` after H1 Patch 0001 merge
Purpose: establish a clean inherited baseline before Patch 0002 fixture-domain expansion

## Overall result
Patch 0001 is appropriately small and clean for its validated scope. No legacy subsystem, provider abstraction, UI dependency, persistence layer, database, or speculative AI framework exists.

Three immediate hygiene corrections are justified before Patch 0002, and several placeholder surfaces are explicitly marked for replacement rather than compatibility preservation.

## Immediate corrections

### 1. Strong ID fail-closed stringification
Current strong IDs throw when `.Value` is accessed on an uninitialized/default struct but `ToString()` returns an empty string. That asymmetry can conceal invalid state in logging, interpolation, diagnostics, or future serialization helpers.

Decision: change every strong ID `ToString()` implementation to return `Value`, causing default/uninitialized IDs to fail closed consistently.

### 2. .NET 9 SDK policy
Patch 0001 evidence correctly records that SDK 9.0.317 produced the first successful ARM64 build. The project policy, however, is supported stable .NET 9 rather than permanent dependence on that exact feature band.

Decision: change `global.json` to request `9.0.100` with `rollForward: latestFeature` and `allowPrerelease: false`. The exact SDK used by each validation remains recorded in evidence.

### 3. Regression-test foundation
The strong-ID behavior change needs an automated regression test, and Patch 0002.1 immediately requires deterministic semantic tests. A Core test project is therefore an earned boundary rather than speculative infrastructure.

Decision: add `tests/Ensemble.E0.Core.Tests` using the current supported MSTest SDK / Microsoft.Testing.Platform path, with tests proving valid strong IDs round-trip and every default/uninitialized strong ID fails closed during stringification. Patch 0002.1 will extend this same project rather than introduce a second test stack.

## Surfaces intentionally preserved
- `CanonicalId`: simple ASCII canonical-ID validation remains appropriate.
- `StrictJsonPreflight`: keep as the raw JSON security/syntax boundary; Patch 0002.1 may extend it with size/dialect restrictions.
- `FixtureValidationException`: keep as the fixture-validation boundary.
- Core/Harness dependency direction: preserve Harness -> Core only.
- `Directory.Build.props`: nullable enabled, implicit usings, warnings-as-errors, deterministic build remain appropriate.
- native `win-arm64` Harness target and runtime architecture guard remain appropriate.

## Surfaces explicitly scheduled for clean replacement in Patch 0002.1
These are Patch 0001 bootstrap placeholders, not compatibility contracts:

### `FixtureEnvelopeDocument`
The `JsonElement` envelope was intentionally minimal. Replace it with the typed E0 Fixture Dialect v1 transport schema. Do not preserve an adapter or legacy DTO.

### `accessPolicies`
Remove the fixture-authored ACL section. Patch 0002.1 will use a known deterministic access-contract identifier plus authority-category/ownership law. The fixture must not be able to invent permissions that contradict authority semantics.

### `FixtureEnvelopeValidator`
Replace with schema-version validation plus generic E0 semantic validation. Do not create a compatibility wrapper around the old envelope validator.

### Fixture identity semantics
Patch 0001 currently constructs `FixtureId` from the unversioned metadata `id` only. Patch 0002.1 must distinguish fixture family identity from the fully versioned authoritative fixture identity (for example, family + version -> `family@version`). Do not retain the old interpretation as an alias.

### `FixtureLoader`
Retain the strict transport-boundary responsibility, but update its return type directly to the new typed document rather than supporting both old and new fixture representations.

## Deferred, not defects
- Full typed Missing Raft schema
- semantic invariant validation
- canonical JSON / ECJ-1
- SHA-256 fixture identity
- deterministic Access Control
- Context Composer
- persistence / causal commit chain
- provider/model abstractions
- WinUI / Windows AI / NPU / Store surfaces

These remain outside the current correction boundary and must not be pulled into this patch.

## Hygiene verdict
- Duplicate implementations: none
- Dead code: none identified
- Speculative abstraction: none identified
- Naming drift: none material in validated Patch 0001 scope
- Dependency-direction violations: none
- Hidden compatibility burden: none yet; placeholder fixture types must be replaced rather than preserved
- Validation inflation: none
- Immediate cleanup required: strong-ID fail-closed behavior + .NET 9 SDK policy + regression-test foundation

After the corrections build, `dotnet test` passes, and the existing ARM64 runtime sanity check passes, the repository is a clean baseline for Patch 0002.1.
