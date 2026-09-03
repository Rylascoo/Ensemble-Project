# H1 Patch 0008 — ARM64 Validation Evidence

Date: 2026-09-02
Status: MACHINE VALIDATED FOR EXERCISED PATCH 0008 DETERMINISTIC GATES

Canonical blueprint:
`docs/blueprint/H1_PATCH_0008_INTEGRITY_VALIDATOR_CONTRACT.md`

Implementation approval checkpoint:
`34b3ffdae98df21fc938370e964a39626e7ead32`

Exact machine-tested executable/test head:
`30a07d0aeee63db927eecd49391fb6271268dc4d`

Implementation PR:
**#18 — H1: implement deterministic Integrity Validator contract**

## Native ARM64 compiler gate

User machine command:

`dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug`

Observed:
- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Harness` succeeded;
- Harness output target `net9.0\win-arm64`;
- build succeeded;
- elapsed 3.1s.

Result: **PASS**.

## Full Core test gate

User machine command:

`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`

Observed:
- total: `253`;
- failed: `0`;
- succeeded: `253`;
- skipped: `0`;
- test duration: 1.7s;
- command/build duration: 3.6s.

Patch 0008 contributes 42 Integrity test executions over the machine-validated Patch 0007 baseline of 211 Core tests.

Result: **PASS**.

## Harness regressions

Missing Raft command:

`dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json`

Observed:
- `Fixture validated: ensemble.e0.missing-raft@0.1.0`;
- exit code `0`.

Generic smoke command:

`dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json`

Observed:
- `Fixture validated: ensemble.e0.smoke@0.1.0`;
- exit code `0`.

Result: **PASS** for both exercised Harness regressions.

## Machine-tested implementation scope

The exact executable/test delta from approval checkpoint `34b3ffdae...` to machine-tested head `30a07d0a...` contains exactly three added files:

1. `src/Ensemble.E0.Core/Integrity/IntegrityModels.cs`;
2. `src/Ensemble.E0.Core/Integrity/DeterministicIntegrityValidator.cs`;
3. `tests/Ensemble.E0.Core.Tests/Integrity/DeterministicIntegrityValidatorTests.cs`.

No existing Access, Context, Performer, Director, fixture, Harness, project configuration, persistence, UI, Windows AI/NPU, packaging, or Store source changed in the machine-tested executable/test delta.

## Exercised Patch 0008 behavior

The passing tests exercise the approved deterministic Integrity boundary, including:

- least-privilege `IntegrityCandidateInput.Bind(ContextPacket, CandidatePerformance)`;
- Candidate content identity contract and deterministic SHA-256 binding;
- deterministic Reject ordering for subject/context mismatches;
- hard-rule short circuit before concern evidence;
- canonical typed `IntegrityConcernEvidence`;
- stale/mismatched evidence rejection;
- deterministic dispositions `Accept`, `Reject`, `RequestAnotherTake`;
- no authority-bearing eligibility/attestation object;
- no truth-policing keyword scanner or hidden rewrite;
- no retry/spend, Take, State, commit, history, opportunity, provider, or persistence authority;
- frozen Missing Raft Context and ECJ-1 regression identities.

Reference Candidate content oracle exercised by the test suite:

`18eb8c9ba9d35f34f84e1fdd983eeb2a19ce015a2e44457c4514576f984af3b2`

## Validation authority boundary

This evidence establishes only the exercised target-machine gates above.

It does **not** establish:
- authenticated semantic-assessor/provider behavior;
- provider-attempt provenance;
- deterministic retry/cost/cancellation behavior;
- accepted Take or TakeId semantics;
- State Interpreter or deterministic State Authority;
- ProductionState / StateHash;
- atomic causal commit or persistence/recovery;
- effective Current Opportunity mutation or Scene-loop behavior;
- World Resolver/observation behavior;
- Windows AI/NPU execution or performance;
- WACK or Microsoft Store certification.

Static/adversarial review remains advisory. The user's native ARM64 build output is compiler authority for the exercised build, `dotnet test` is test-execution authority for the exercised suite, and the Harness runs are runtime authority only for those exercised fixture-validation paths.
