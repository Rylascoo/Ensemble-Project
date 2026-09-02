# H1 Patch 0003 — ARM64 Validation Evidence

Date: 2026-09-02

## Authority boundary

This record captures validation performed by the user on the native Windows ARM64 development machine for H1 Patch 0003 at the exact executable implementation head:

`c55eb022983a3954a78c8386a8c83ce8d5f4f2a7`

Any documentation-only commits after that SHA do not increase or alter the executable validation level.

## Source / branch preparation

Commands exercised:

```powershell
git fetch origin
git switch h1-patch-0003-implementation
git pull --ff-only origin h1-patch-0003-implementation
git rev-parse HEAD
```

Observed implementation head:

`c55eb022983a3954a78c8386a8c83ce8d5f4f2a7`

Result: PASS. The tested working tree was the intended Patch 0003 implementation head.

## Native Windows ARM64 compiler gate

Command:

```powershell
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
```

Observed:
- restore completed successfully;
- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Harness` succeeded;
- Harness output targeted `net9.0\win-arm64`;
- build succeeded.

Result: PASS.

This is compiler authority for the exercised Debug ARM64 Harness/Core build at the tested head.

## Core test-execution gate

Command:

```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

Observed summary:
- total: `73`;
- failed: `0`;
- succeeded: `73`;
- skipped: `0`;
- test duration: approximately `1.0s`;
- command/build duration: approximately `2.7s`.

Result: PASS — 73/73.

The exercised suite includes the 62 previously validated regression tests plus the 11 Patch 0003 ECJ-1/hash tests covering:
- frozen Missing Raft canonical byte length and digest;
- repeat serialization determinism;
- source formatting/root-property-order independence;
- ordinal canonicalization of semantically unordered Character/roster/record/relationship/provenance collections;
- semantic text hash mutation rejection;
- relationship-text hash mutation rejection;
- provenance-source-order set semantics;
- carriage-return rejection;
- unpaired-surrogate rejection;
- exact ECJ-1 escaping behavior;
- deterministic generic-smoke hashing without Missing Raft authority.

Existing chronology, generic-dialect, duplicate-property, NFC, provenance-DAG, Missing Raft structural/authority/provenance, and generic-smoke regressions remained green through the complete passing suite.

## Canonical Missing Raft runtime gate

Command:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json
$LASTEXITCODE
```

Observed Harness result:

`Fixture validated: ensemble.e0.missing-raft@0.1.0`

Observed process exit code:

`0`

Result: PASS.

Because the known Missing Raft Harness path invokes `MissingRaftContract.Validate(ValidatedFixture)`, this exercised the Patch 0003 structural contract plus ECJ-1 canonicalization, SHA-256 computation, and frozen expected-digest enforcement for the canonical fixture.

## Generic smoke runtime regression

Command:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
$LASTEXITCODE
```

Observed Harness result:

`Fixture validated: ensemble.e0.smoke@0.1.0`

Observed process exit code:

`0`

Result: PASS.

The generic unknown-family path remains functional and is not required to possess a frozen Missing Raft digest.

## Independent ECJ-1 digest review

Before executable validation, two independent reference serialization paths over the canonical Missing Raft source agreed on:
- canonical ECJ-1 UTF-8 byte length: `9112` bytes;
- SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

The C# test suite freezes both values, so accidental byte-contract drift fails closed.

This independent reference is advisory/static evidence; the user's passing target-machine test/runtime output is the execution authority for the code actually exercised.

## Static / hygiene / scope result

Final comparison against the approved Patch 0003 parent baseline found exactly five executable/test source paths changed:
1. `src/Ensemble.E0.Core/Fixture/CanonicalText.cs`;
2. `src/Ensemble.E0.Core/Fixture/Ecj1FixtureCanonicalizer.cs`;
3. `src/Ensemble.E0.Core/Fixture/FixtureHash.cs`;
4. `src/Ensemble.E0.Core/Fixture/MissingRaftContract.cs`;
5. `tests/Ensemble.E0.Core.Tests/Fixture/Ecj1FixtureIdentityTests.cs`.

Confirmed:
- canonical Missing Raft source JSON is unchanged from the Patch 0002.2 validated baseline;
- generic E0 fixture schema/transport representation is unchanged;
- `ValidatedFixture` remains the single successful fixture domain representation;
- ECJ-1 uses explicit property emission rather than reflection/serializer/dictionary-order authority;
- chronology remains semantically ordered;
- semantically unordered fixture collections canonicalize ordinally by stable ID without mutating the validated representation;
- semantic text is rejected rather than silently repaired for carriage-return or invalid-surrogate cases;
- SHA-256 is fixture identity/change detection only and grants no Character access;
- provenance remains evidence/derivation only and grants no access;
- the expected Missing Raft digest is frozen in `MissingRaftContract`, not a mutable sidecar;
- the Harness interface and generic family dispatch remain unchanged;
- no second fixture representation, canonical fixture copy, registry/plugin framework, compatibility mode, or hash/access conflation was added.

Final advisory static/hygiene/scope verdict: PASS.

## Explicitly unvalidated / excluded

This evidence does not establish or implement:
- Patch 0004 deterministic Access Control;
- Context Composer or ContextPacketHash;
- StateHash;
- ProductionState construction or persistence;
- accepted-history / causal commits;
- Performer, Director, Integrity Validator, or State Interpreter runtime orchestration;
- provider or AI behavior;
- Windows AI / NPU execution;
- WinUI;
- packaging;
- WACK;
- Microsoft Store certification.

SHA-256 fixture identity is not a signature, authorization mechanism, encryption mechanism, or publisher-authenticity claim.

No NPU, WACK, Store, or broader product-runtime claim may be inferred from this Patch 0003 evidence.

## Verdict

H1 Patch 0003 passes its exercised exit gates at executable head `c55eb022983a3954a78c8386a8c83ce8d5f4f2a7`:
- native Windows ARM64 compiler: PASS;
- Core tests: PASS — 73/73;
- canonical Missing Raft runtime with hash enforcement: PASS, exit `0`;
- generic smoke runtime regression: PASS, exit `0`;
- independent ECJ-1 byte/digest review: PASS advisory;
- final static/hygiene/scope comparison: PASS advisory.
