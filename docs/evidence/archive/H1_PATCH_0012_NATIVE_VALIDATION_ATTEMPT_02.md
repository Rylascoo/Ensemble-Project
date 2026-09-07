# H1 Patch 0012 — Native Windows ARM64 Validation Attempt 02

Date: 2026-09-03
Status: TEST EXECUTION FAILURE ISOLATED — PROVENANCE CORRECTION APPLIED — RERUN REQUIRED

## Authority boundary

This record preserves evidence supplied from the user's native Windows ARM64 development machine for the second Patch 0012 validation attempt.

It does **not** establish complete Patch 0012 machine validation because the Core test assembly executed with failures.

The latest fully machine-validated executable checkpoint therefore remains H1 Patch 0011 at exact tested head:

`4250011c167cd9850ad891aaea4ee053216cf135`

## Tested branch and head

Branch:

`h1-patch-0012-atomic-causal-commit-implementation`

Exact tested branch head:

`4dabf508ba17d3d40dc73449dbb6ad0b2c122010`

`git status --short` was empty before the test run.

## Test compilation and execution result

Command:

```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

Compilation result:

`PASS`

Observed build outputs:

- `Ensemble.E0.Core` succeeded under `net9.0`;
- `Ensemble.E0.Core.Tests` succeeded under `net9.0`.

Test execution result:

`FAIL`

Observed summary:

- total: `472`;
- succeeded: `371`;
- failed: `101`;
- skipped: `0`.

The supplied ARM64 test log showed the failures cascading through Production, StateAuthority, Take, and CausalCommit paths from one common exception:

```text
ProductionGenesisProjectionException: Record provenance is not canonical.
```

The failing boundary was `ProductionGenesisProjection.ValidateProvenance(...)` during fixture-to-Production/StateAuthority genesis projection.

## Root cause

Patch 0012 incorrectly required `ValidatedFixture` provenance arrays to already be ordinally sorted before neutral genesis projection.

That is incompatible with the established fixture contract:

- `GenericE0FixtureValidator` preserves valid provenance source order while enforcing identity/duplicate validity;
- ECJ-1 canonicalization treats provenance as a semantically unordered edge set and sorts it only for canonical identity;
- the canonical Missing Raft fixture contains a valid non-ordinal source-order case on `PRESSURE-ISOLATION`:
  - source order: `SCENE-RAFT-GONE`, `SCENE-PROVISIONS-LIMITED`;
  - canonical ordinal order: `SCENE-PROVISIONS-LIMITED`, `SCENE-RAFT-GONE`;
- Proposal 0.10 requires existing `StateAuthoritySnapshot.Bind(fixture, locks)` public behavior/exception-domain compatibility and canonical Production provenance.

Therefore valid fixture provenance must be validated and canonicalized during Production projection rather than rejected for source ordering.

## Patch-first correction

Production correction commit:

`f1c272d24cb1d8bc35b090d611549338acbdf5df`

`ProductionGenesisProjection.ValidateProvenance(...)` now:

1. rejects default provenance;
2. rejects uninitialized RecordIds;
3. rejects duplicate RecordIds;
4. returns the exact edge set sorted by ordinal RecordId for canonical Production storage.

No fixture, StateAuthority policy, Take, causal-transition, materialization, or hash-envelope authority was changed.

Focused regression-test commit:

`94f13c06ecd7834b9c1e7abc777b4e3ef6d92c3a`

The new regression test proves that canonical Missing Raft's valid source-order `PRESSURE-ISOLATION` provenance is accepted, preserves the exact edge set, and is stored in canonical Production order.

## Recursive correction audit

Because this was a material correctness correction, the Patch 0012 audit restarted from correctness.

Result before native rerun:

- correctness: correction matches existing fixture/ECJ semantics and Proposal 0.10 compatibility law;
- consistency: Production provenance canonicalization now agrees with ECJ-1's unordered-edge semantics;
- authority: fixture-derived StateAuthority compatibility is restored without creating new authority;
- scope: only one Production genesis helper and one focused test changed;
- tests: supplied failure family is directly covered by the canonical Missing Raft regression;
- simplicity/hygiene: no duplicate canonicalizer or compatibility shim added;
- ARM64 suitability: work occurs only at explicit genesis/snapshot projection and adds no idle/background work;
- vision/evidence: deterministic authority and validation-level separation remain intact.

No complete machine-validation claim is made until the corrected branch head passes the required native gate.
