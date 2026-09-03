# Ensemble — E0-A H1 Patch 0012 Implementation Handoff

Prepared: 2026-09-03
Status: READY FOR FRESH-CHAT IMPLEMENTATION AFTER BLUEPRINT PROMOTION

## Mission

Resume Ensemble at H1 Patch 0012 only: implement the explicitly approved E0 Atomic Causal Commit Proposal 0.10 on top of the machine-validated and promoted Patch 0011 Take Semantics baseline.

Do not redesign Proposal 0.10 during routine implementation. Do not enter evolved Production->Access/Context integration, next-Director opportunity transition, full multi-turn replay, durable persistence, branch/canon, provider execution, Scene loop, WinUI, Windows AI/NPU, packaging, WACK, or Store scope during Patch 0012.

## Source-of-truth order

1. `CURRENT_STATE.md` — authoritative live checkpoint/validation level.
2. `docs/blueprint/H1_PATCH_0012_ATOMIC_CAUSAL_COMMIT.md` — exact approved Proposal 0.10 implementation specification.
3. `docs/evidence/H1_PATCH_0012_BLUEPRINT_APPROVAL.md` — explicit approval evidence and frozen-law summary.
4. `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` — implementation cleanliness law.
5. Current promoted `main` source/tests and Patch 0011 machine evidence.
6. Patch 0010/0011 contracts/source only where a concrete StateAuthority/Take boundary requires it.
7. Patch 0003/0009 source only where canonical JSON/provenance/proposal-token reuse requires a smallest refactor.
8. Frozen Blueprint 0.1 only for a genuine constitutional ambiguity.
9. Google Drive only for supporting product context when materially relevant; GitHub remains engineering authority.

## Approved architecture state

Architecture: FROZEN FOR PATCH 0012 IMPLEMENTATION

Blueprint: APPROVED — Proposal 0.10

Exact recursively audited proposal head:
`ad1e84121653457454f6bf90e831e2064a79f224`

Approval record:
`docs/evidence/H1_PATCH_0012_BLUEPRINT_APPROVAL.md`

Recursive architecture audit: COMPLETE — final full pass found zero material corrections and zero worthwhile architectural improvements.

Implementation: NOT STARTED

## Current machine-validated executable baseline

H1 Patch 0011 E0 Take Semantics.

Exact machine-tested executable/test head:
`4250011c167cd9850ad891aaea4ee053216cf135`

Validated on the user's native Windows ARM64 machine:

- Core/Harness Debug build: PASS;
- Harness target: `net9.0\win-arm64`;
- full Core tests: `430/430` PASS;
- Missing Raft Harness: PASS;
- generic smoke Harness: PASS.

Detailed authority:
`docs/evidence/H1_PATCH_0011_ARM64_VALIDATION.md`

Do not promote this evidence to Patch 0012. Patch 0012 receives machine authority only after the user runs the implementation gate on the exact implementation head.

## Canonical boundary

Approved ordering:

```text
immutable ProductionState
    -> ProductionStateCheckpoint captured before Access/Context
        -> existing Access / Context / Performance / Integrity / Interpretation / State Authority
            -> immutable Accepted E0Take
                -> Accepted-only exact Take/source-state binding
                    -> exact StateHash freshness
                        -> deterministic atomic causal commit
                            -> E0CausalCommit
                            -> result ProductionState
```

The atomic law is indivisible:

```text
exact Accepted Performance
+ every retained Approved consequence
    -> effective together

or neither becomes effective
```

Retained Rejected consequences never become effective.

## New public namespaces

Implement approved public Patch 0012 surfaces under:

```text
Ensemble.E0.Core.Production
Ensemble.E0.Core.CausalCommit
```

Shared provenance graph refactor is internal only:

```text
Ensemble.E0.Core.Provenance
```

Do not expose the provenance helper publicly.

## Contract versions

Implement exact strings:

```csharp
ProductionStateContracts.StateContractVersion
    = "ensemble.e0.production-state.v1";

ProductionStateContracts.StateHashContractVersion
    = "ensemble.e0.production-state-hash.sha256.v1";

E0CausalCommitContracts.ContractVersion
    = "ensemble.e0.causal-commit.v1";
```

## Canonical strong IDs

Reuse existing:

- `CommitId`;
- `RecordId`;
- `TakeId`.

Do not add another Commit/Record/Take ID type or Core allocator.

Core validates supplied IDs for initialization/required association/uniqueness/collision only.

Do not derive CommitId/RecordId from clock, randomness, GUID generation, provider/model data, StateHash, TakeId, Candidate hash, or proposal hash.

## ProductionState and StateHash

Implement immutable `ProductionState` and `StateHash` exactly as Proposal 0.10 defines.

Important laws:

- `StateHash` is a distinct strong value with exact lowercase 64-character SHA-256 hex;
- `default(StateHash)` is uninitialized and reading Value fails;
- no public arbitrary-string StateHash factory/parser exists in Patch 0012;
- genesis and causal-commit canonicalizers create StateHash internally;
- `ProductionState` has closed construction and no public mutation API;
- current state retains immutable Characters, roster, records, Current Opportunity, origin fixture identity/hash, and internal effective CommitId/TakeId duplicate indexes;
- derived effective ID indexes are not serialized into canonical Production projection hashing;
- `ValidatedFixture` is genesis input only and is never mutated into evolved state.

## Production enum contract

Use the exact Proposal 0.10 numeric values including `Unspecified = 0`.

Do not numeric-cast Production enums to existing Patch 0010 StateAuthority enums. Their numeric layouts intentionally differ.

Implement one exhaustive explicit semantic mapping for:

- Production domain -> StateAuthority domain;
- Production lifecycle -> StateAuthority lifecycle;
- Production protection -> StateAuthority protection;
- Global/Character/Relationship record shape -> exact StateAuthority descriptor shape.

`Unspecified`/undefined values fail before mapping.

## Genesis and neutral mapping

Implement:

```text
ProductionState.Initialize(
    ValidatedFixture fixture,
    ImmutableArray<RecordId> creatorLockedRecordIds)
```

Genesis must preserve exact fixture identity/hash, Scene, Character display identity, canonical roster, initial opportunity, exact record text/provenance, and Patch 0010 protection semantics.

SystemImmutable remains stronger than CreatorLocked.

Duplicate/unknown/uninitialized creator-lock inputs fail closed.

Do not create separate handwritten Fixture->Production and Fixture->StateAuthority protection/domain law tables.

Refactor toward one narrow neutral genesis mapping that can serve both Production initialization and existing fixture-derived StateAuthority snapshot semantics without allocating a full ProductionState merely to build the legacy snapshot.

Existing `StateAuthoritySnapshot.Bind(ValidatedFixture, creatorLocks)` public behavior and exception domain must remain compatible.

## Shared provenance graph

The existing Fixture provenance-DAG algorithm becomes the second-use shared primitive.

Refactor one internal neutral validator such as:

```text
Ensemble.E0.Core.Provenance.RecordProvenanceGraphValidator
```

It may depend only on RecordId and BCL collection/validation primitives.

It validates:

- unique node RecordIds;
- every support RecordId resolves;
- acyclicity.

Fixture adapts to it while preserving `FixtureValidationException` behavior.

Production/CausalCommit adapt to the same primitive and translate expected structural failure into their owning typed exception domain.

Do not keep two cycle-detection algorithms.

## O(1) source checkpoint

Implement:

```text
ProductionStateCheckpoint.Capture(ProductionState sourceState)
```

Checkpoint validates only the closed-state prerequisites needed to start an opportunity pipeline:

- non-null state;
- readable initialized StateHash/SceneId/current opportunity;
- current opportunity exists and belongs to roster.

Checkpoint must not re-hash state, rebuild StateAuthority snapshot, rerun provenance validation, or perform another full record-ledger audit.

It retains the exact immutable source-state reference internally and exposes StateHash/Scene/current non-null opportunity only.

## Evolved StateAuthority snapshot

Add the approved overload:

```text
StateAuthoritySnapshot.Bind(ProductionState state)
```

Map exact Scene/roster/record identity/domain/lifecycle/protection/scope.

Do not expose record Text or provenance through StateAuthority descriptors.

Add/reuse one internal exact snapshot semantic comparator over Scene, canonical roster, descriptor shape/count/order/RecordId/domain/lifecycle/protection/subject/target.

Do not introduce SnapshotHash.

CausalCommit must not create a second StateAuthority evaluator/policy.

## Accepted-only Take/source-state binding

Implement immutable:

```text
E0TakeStateBinding
- SourceStateHash
- Take : exact immutable Accepted E0Take
```

Sole rich construction:

```text
E0TakeStateBinding.Bind(
    ProductionStateCheckpoint sourceCheckpoint,
    ContextPacket sourceContext,
    E0Take take)
```

Binding rejects Rejected/Alternate Takes.

Binding validates the exact association defined by Proposal 0.10, including:

- Take/Candidate/Context IDs;
- source Context subject/opportunity;
- checkpoint Current Opportunity;
- Scene and roster association;
- one fresh Production-derived StateAuthority snapshot semantic comparison to the Take retained snapshot;
- terminal Complete/no RequiresReview authority.

On success it retains the exact Take reference and exact checkpoint StateHash.

Do not add another Take hash or coarse-ID rebinding mechanism.

## Context identity limit

Current Access is fixture-based and current Context carries no StateHash.

Patch 0012 may prove a genesis-equivalent source association when Context was produced from the same immutable fixture used to initialize the genesis ProductionState.

Do not claim evolved Production->Access/Context support.

Do not add a Production Access overload in this patch merely to close the loop: CharacterClaim/recent-Performance disclosure semantics are intentionally unresolved and belong to a later approved integration contract.

## Strict freshness

Commit-time freshness is exactly:

```text
currentState.StateHash == binding.SourceStateHash
```

Binding already performed the one exact source snapshot association proof.

After exact StateHash equality, Commit must not rebuild/recompare StateAuthority snapshot again. That would add redundant O(record-ledger) work without new authority.

Any changed causal/state-transition head fails closed.

Do not rerun StateAuthority on changed state and do not substitute a new consequence set under the same Take.

## Record materialization

Implement:

```text
E0RecordMaterialization
- MutationIndex
- RecordId

E0RecordMaterializationSet
- Items
```

`Bind` rejects default/null/negative/duplicate/uninitialized/duplicate-RecordId forms and canonicalizes ascending mutation index.

Commit-specific exactness:

- one item for every Approved Add;
- one item for every Approved Supersede;
- none for Approved Deactivate;
- none for Rejected;
- no collision with any active/inactive Production record;
- no missing/extra materialization;
- no RecordId reuse.

No materialization is generated inside Core.

## Shared deterministic transition engine

Commit and Replay must share one internal transition implementation.

Input semantics:

```text
parent ProductionState
+ exact Accepted E0Take
+ exact materialization set
```

Process exact proposal/decision pairs in retained mutation-index order.

Rules:

```text
Rejected -> no state effect
Approved -> exact typed mutation effect
RequiresReview -> fail closed
```

Add/Supersede/Deactivate behavior must match Proposal 0.10 exactly.

Structural transition checks are defense in depth, not a second approval policy.

## Minimal causal event

Implement exactly the approved minimal event surface:

```text
E0CausalCommit
- ContractVersion
- CommitId
- ParentStateHash
- ResultStateHash
- Take : exact Accepted E0Take
- RecordMaterializations : exact canonical set
```

Do not add:

- AppliedEffects hierarchy;
- duplicate committed opportunity field;
- duplicate mutation domain/kind/existing target representation;
- raw Candidate/proposal text copies.

The exact Take already owns Candidate/proposal/authority semantics. Materializations are the only successful-application data not recoverable from Take.

## Sole new-event authority

Implement:

```text
DeterministicCausalCommit.Commit(
    CommitId commitId,
    ProductionState currentState,
    E0TakeStateBinding binding,
    E0RecordMaterializationSet materializations)
    -> E0CausalCommitResult
```

There is intentionally no separate `E0Take` parameter. Use `binding.Take`.

Commit validates exact StateHash freshness, Accepted/terminal authority invariants, current opportunity, materialization exactness, duplicate effective IDs, transition/provenance validity, and complete canonical result construction.

No state-only commit API and no second Apply/Promote/Accept event factory.

On success:

```text
ResultState.CurrentOpportunityCharacterId = null
```

Do not choose a next opportunity.

Failure returns no event/result state; immutable input remains unchanged.

## One-step Replay

Implement:

```text
DeterministicCausalCommit.Replay(
    ProductionState parentState,
    E0CausalCommit committedEvent)
    -> ProductionState
```

Replay verifies/reconstructs exactly one already-effective commit transition.

Replay has no source binding, so it independently projects one Production-derived StateAuthority snapshot and compares it to event.Take's snapshot.

Replay validates exact parent hash, event contract, Accepted Take invariants, current opportunity, materializations, duplicate IDs, transition result, and recomputed ResultStateHash.

Do not claim full session replay from genesis; next-opportunity/non-commit transition authority is not defined yet.

## Canonical JSON and StateHash

Reuse existing internal `CanonicalJson` string/UTF-8 rules.

Do not introduce a second general canonical JSON serializer.

Freeze exact Proposal 0.10 property order/token/null behavior for:

- Production projection;
- Take causal payload;
- policy/review/decisions;
- record materializations;
- genesis StateHash envelope;
- causalCommit StateHash envelope.

Mutation-domain canonical strings must reuse one internal mapping with the existing Patch 0009 proposal canonicalizer rather than a duplicate table.

New canonical integer fields such as MutationIndex must use culture-independent invariant minimal base-10 ASCII JSON number encoding.

StateHash envelopes:

```text
genesis:
SHA256(hashContract + kind=genesis + canonical projection envelope)

causal commit:
SHA256(hashContract + kind=causalCommit + parentStateHash + canonical commit payload + result projection envelope)
```

Implement the exact JSON objects/property ordering from the blueprint rather than textual concatenation shorthand.

Provide fixed genesis and post-commit byte/hash test oracles.

## Derived duplicate indexes

ProductionState internally carries immutable effective CommitId and committed TakeId indexes for duplicate rejection.

They are closed-state derived caches, not creative-history authority.

Do not include the full growing indexes in canonical Production projection hashing; parent StateHash + current commit payload already binds causal identity and avoids needless growing hash work.

Do not claim Patch 0012 can rebuild all such indexes from a full multi-turn event stream; later transition/persistence contracts own that.

## Exception boundaries

Implement exactly:

```text
public sealed class ProductionStateException : Exception
public sealed class E0CausalCommitException : Exception
```

No public constructors/factories.

Expected boundary normalization:

- Production initialization may normalize expected `FixtureValidationException` into sanitized `ProductionStateException`;
- Production/CausalCommit canonicalization must not leak internal `CanonicalJsonException`;
- CausalCommit public boundaries normalize expected `ProductionStateException` and `StateAuthorityException` into sanitized `E0CausalCommitException`;
- commit-owned uninitialized strong-ID/StateHash runtime failure is normalized without retaining arbitrary runtime exception evidence;
- shared provenance validation failure is translated to the owning domain.

Do not concatenate upstream messages into public wrapper messages.

Do not copy arbitrary exception Data.

Do not catch-all unexpected runtime/programming failures and relabel them as ordinary commit failure.

No Candidate/Context/mutation/provider/user content leakage in exception Message/inner representation/ToString.

## Required implementation tests

Implement the complete matrix in Proposal 0.10 Section 37; do not silently narrow it.

Critical families include:

- public namespaces/types/version strings;
- exact enum numeric contracts;
- no enum numeric casts;
- canonical strong-ID reuse/no allocator;
- closed immutable state/record/checkpoint/binding/event/result construction;
- neutral genesis mapping reuse;
- shared provenance graph refactor preserving Fixture behavior;
- genesis exact semantics and fixed canonical hash oracle;
- checkpoint O(1) behavior/no hidden re-hash/snapshot/provenance work;
- fixture and Production snapshot equivalence at genesis;
- evolved snapshot mapping and exact comparator;
- Accepted-only binding and exact Take reference ownership;
- no separate Take parameter at Commit;
- no duplicate Commit snapshot projection after binding;
- fixture-only current Access boundary preserved;
- zero-mutation/all-Rejected Accepted commits;
- exact Add/Supersede/Deactivate/mixed consequence application;
- materialization exactness/collision/non-reuse;
- provenance resolution/acyclicity;
- stale StateHash failure;
- duplicate effective CommitId/TakeId failure;
- opportunity consumption/no next opportunity;
- minimal event surface/no AppliedEffects;
- existing Candidate/proposal hashes cover semantics relied upon by causal identity;
- shared mutation-domain token mapping;
- invariant canonical integer formatting;
- exact canonical Production/Take/materialization bytes;
- fixed post-commit StateHash oracle;
- deterministic identical inputs/IDs;
- different valid CommitId/materialization changes StateHash;
- event ResultStateHash equals result state StateHash;
- exact one-step Replay and tamper/wrong-parent rejection;
- Commit/Replay shared engine;
- no state-only commit API/no event rewrite/delete API;
- typed exception normalization/sanitization/no catch-all;
- no full-session replay/persistence/evolved-Access/next-Director/UI/AI/NPU authority leakage;
- frozen Patch 0003–0011 regressions unchanged except explicitly approved Patch 0012 extensions/refactors;
- full Core regression;
- Missing Raft Harness;
- generic smoke Harness.

Use existing reflection/IL contract-audit patterns where required. Do not weaken production constructors for negative tests.

## Expected patch-first source surface

Patch 0012 is broader than Patch 0011 but must still be minimal.

Expected new/changed areas are approximately:

```text
src/Ensemble.E0.Core/Production/...
src/Ensemble.E0.Core/CausalCommit/...
src/Ensemble.E0.Core/Provenance/...        (internal shared primitive)
src/Ensemble.E0.Core/Fixture/...           (small provenance adapter refactor only)
src/Ensemble.E0.Core/StateAuthority/...    (Production snapshot overload, shared mappings/comparator/token reuse as required)
tests/Ensemble.E0.Core.Tests/Production/...
tests/Ensemble.E0.Core.Tests/CausalCommit/...
```

A minimal canonicalization helper refactor in the existing StateInterpreter/StateAuthority source is allowed only where required to share the already-frozen Patch 0009 mutation-domain token mapping.

Do not broaden into Access/Context/Harness orchestration merely for convenience.

## Recursive implementation audit

After implementation, recursively audit in this exact discipline:

```text
correctness
-> consistency
-> authority
-> scope
-> tests
-> simplicity
-> hygiene
-> ARM64 suitability
-> project vision
-> evidence
```

If any material correction or worthwhile improvement is found, apply the smallest patch and restart from correctness.

Stop only after one complete pass finds no material corrections and no worthwhile improvements.

Static review is advisory. Do not claim Windows compilation/runtime authority until the user runs the exact branch on native Windows ARM64.

## Native Windows ARM64 validation gate

When implementation is statically converged, ask the user for one grouped command block:

```powershell
git status --short
git rev-parse HEAD

dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug

dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug

dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json

dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
```

If build fails, later `--no-build` Harness results may represent old binaries and are not Patch 0012 evidence. Patch the smallest compiler surface, then rerun the complete final gate from the corrected exact head.

Do not claim NPU execution, Windows AI behavior, WACK, packaging, or Store certification from these commands.

## Exit gate

Patch 0012 implementation is not complete until:

- exact approved Proposal 0.10 semantics are implemented;
- no duplicate commit/Take/effect/snapshot/token/provenance authority representation remains;
- complete Patch 0012 contract/behavior/tamper/replay tests exist;
- full Core tests pass on the user's native Windows ARM64 machine;
- Core/Harness Debug build succeeds natively for `win-arm64`;
- Missing Raft and generic smoke Harness regressions pass after the successful build;
- recursive implementation audit converges with zero material corrections/improvements;
- exact machine-tested implementation head is recorded;
- evidence explicitly distinguishes compiler/test/Harness authority from unverified broader runtime/NPU/package/Store claims.

## Explicit exclusions

Do not implement in Patch 0012:

- durable CommitId/RecordId allocator/format;
- persisted StateHash parser/deserializer;
- database/event-store/recovery transaction;
- full multi-turn state-transition replay;
- next-opportunity StateHash transition bytes;
- branch DAG/canon lineage;
- Alternate promotion/retcon/rehearsal;
- final Archive/UI query model;
- evolved `ProductionState -> CharacterBoundedAccessControl` contract;
- CharacterClaim/current-history Context disclosure semantics;
- accepted recent Performance injection into Context;
- direct full ContextPacket derivation proof from StateHash;
- provider/assessor/reviewer authentication;
- next Director opportunity state transition;
- Scene loop;
- Observation/World Resolver;
- final Another Take/Take a Seat/consequence UX;
- Windows AI Foundry/NPU execution;
- WinUI/MSIX/WACK/Store certification.

## Approval discipline

Proposal 0.10 is approved. Do not ask the user to reconfirm implementation choices already fixed by the blueprint.

Request architectural approval only if concrete implementation/compiler evidence proves a material need to change frozen semantics, authority/privacy/security boundaries, public/persistent compatibility contracts, dependency direction, or major scope.

Otherwise make the smallest implementation/refactor/test decision consistent with the approved contract and continue recursively.
