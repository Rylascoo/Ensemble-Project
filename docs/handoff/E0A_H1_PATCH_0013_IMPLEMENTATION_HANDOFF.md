# Ensemble — E0-A H1 Patch 0013 Implementation Handoff

Prepared: 2026-09-03
Status: READY FOR IMPLEMENTATION

## Mission

Implement only the approved H1 Patch 0013 Effective Opportunity Authority Proposal 0.6 on top of promoted Patch 0012.

Do not redesign the approved architecture during routine implementation. Do not enter evolved Production->Access/Context, CharacterClaim disclosure, recent-Performance Context, complete Scene loop, full session replay, persistence/recovery, branches/canon/retcon/rehearsal, provider execution, WinUI, Windows AI/NPU, packaging, WACK, or Store scope.

## Source-of-truth order

1. `CURRENT_STATE.md` — live repository checkpoint/validation authority.
2. `docs/blueprint/H1_PATCH_0013_EFFECTIVE_OPPORTUNITY_AUTHORITY.md` — exact approved Proposal 0.6.
3. `docs/evidence/H1_PATCH_0013_BLUEPRINT_APPROVAL.md` — approval evidence.
4. Current promoted Patch 0012 source/tests.
5. `docs/blueprint/H1_PATCH_0007_DIRECTOR_OPPORTUNITY_CONTRACT.md` only for frozen Director lifecycle/semantics.
6. `docs/blueprint/H1_PATCH_0012_ATOMIC_CAUSAL_COMMIT.md` only for Production/StateHash/causal-chain compatibility.
7. `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` — implementation hygiene.

## Authority checkpoints

Approved blueprint Proposal 0.6 audited head:
`a060ce71c9a1dfa5ae9d9faec6b03b7a07f7d781`

Approval evidence commit:
`f144b505a1bad680674e95a814e16a9ff9209ef7`

Promoted parent `main`:
`8c89f998fe6f42e04a75b9090fbcc10f0574f5a2`

Exact Patch 0012 native ARM64-tested executable/test authority:
`39bc078c130ab1165c6a81c1673dd5cd25da3724`

Do not claim Patch 0013 compiler/runtime validation until the user runs the required native ARM64 gate against the exact implementation head.

## Smallest implementation surface

Expected production surface:

- `src/Ensemble.E0.Core/Opportunity/*` — new Patch 0013 authority/history/event/canonicalization implementation;
- `src/Ensemble.E0.Core/Production/ProductionStateModels.cs` — one narrow internal established-opportunity state helper only;
- `src/Ensemble.E0.Core/Production/ProductionStateCanonicalizer.cs` — reuse/expose only the smallest internal canonical projection/genesis helper needed by Patch 0013; do not change existing Patch 0012 preimages;
- no upstream Access/Context/Performer/Director semantic redesign.

Expected tests:

- new `tests/Ensemble.E0.Core.Tests/Opportunity/*` focused Patch 0013 tests/contract audits;
- smallest regression additions only where necessary to prove unchanged Patch 0012 hashes/public surface.

## Core data flow

```text
postcommit ProductionState(CurrentOpportunity = null)
+ exact source E0CausalCommit
+ retained/reconstructed accepted source ContextPacket
+ closed E0OpportunityHistory ending at source
    -> prove source causal/history chain
    -> DirectorOpportunityInput.Bind(...)
    -> LeastInterventionDirector.Propose(...)
    -> selected Character
    -> one-field Production projection transition
    -> kind=opportunityTransition StateHash
    -> E0OpportunityTransition
    -> result ProductionState
    -> result E0OpportunityHistory
```

## Frozen API direction

Public namespace:
`Ensemble.E0.Core.Opportunity`

Implement the exact approved public shapes from Proposal 0.6, including:

- `E0OpportunityHistory`;
- `E0OpportunityTransition`;
- `E0OpportunityTransitionResult`;
- `E0OpportunityException`;
- `DeterministicOpportunityAuthority` live establishment and replay APIs;
- approved Patch 0013 contract version string(s).

No public arbitrary history constructor/append method, Current Opportunity setter, StateHash parser, event builder, or alternate routing mutation path.

## State/hash compatibility hard gate

Existing Patch 0012 canonical bytes must remain unchanged for:

```text
kind = genesis
kind = causalCommit
```

Patch 0013 adds only:

```text
kind = opportunityTransition
```

under the existing `ProductionStateContracts.StateHashContractVersion`.

Do not change the validated Patch 0012 genesis/postcommit oracle hashes.

## Live authority invariants

Fail closed unless at minimum:

- parent postcommit state has no Current Opportunity;
- source commit exactly produced that parent state;
- source commit/Take are effective and Accepted;
- source history Scene/roster/last Character are valid;
- `sourceHistory.LastOpportunityStateHash == sourceCommit.ParentStateHash`;
- source Context matches accepted Candidate Context identity and frozen Patch 0007 structure;
- current Production roster equals source Context roster;
- fresh postcommit Director bind/recompute succeeds;
- selected Character is a roster member.

No precomputed Director evaluation parameter is accepted.

## Opportunity history law

`E0OpportunityHistory.Initialize(...)` succeeds only for an exact genesis Production state by recomputing the existing genesis envelope.

It begins with the fixture-authored opening opportunity exactly once.

Later history is Core-derived only from successful opportunity transitions/replay.

No second history hash chain.

## Event law

Keep the event minimal exactly as approved. Do not duplicate source commit/take IDs, Candidate control, full history, diagnostics, prose, or hidden reasoning where the parent StateHash/source causal event already owns those semantics.

## Production helper law

Do not add a generic internal state/projection replacement API.

Only the narrow null->selected-roster-Character established-opportunity helper approved by Proposal 0.6 is permitted; preserve exact CommitId/TakeId caches.

## Replay law

Replay must independently fail closed on mismatched parent/source commit/source history/event identity and recompute deterministic Director semantics rather than trusting event-selected Character alone.

Replay is one opportunity transition only. Do not claim full session replay.

## Required static/regression gate before returning to user

- inspect every implementation diff against Proposal 0.6;
- prove only approved public surface was added;
- prove existing Patch 0012 hash oracles/preimages are untouched;
- cover genesis history initialization, live transition, nomination/direct-address/recency paths, same-Character selection if valid, stale/spliced history, wrong source commit, wrong Context, malformed/default IDs, replay mismatch/tamper, one-field Production delta, cache preservation, deterministic repeatability, public API containment, and forbidden dependency checks;
- run every available repository/static check that does not pretend to be native Windows ARM64 compiler/runtime authority;
- recursively audit until one complete pass finds no material correction or worthwhile improvement.

## Native ARM64 validation boundary

After static implementation closure, return exact commands for the user to run on the native Windows ARM64 machine. Expected gate should include at least:

```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
```

Do not promote static analysis to compiler/runtime validation.
