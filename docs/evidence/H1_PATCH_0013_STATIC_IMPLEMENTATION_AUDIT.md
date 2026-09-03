# H1 Patch 0013 — Static Implementation Audit

Status: STATIC IMPLEMENTATION AUDIT COMPLETE — NATIVE ARM64 COMPILER/TEST VALIDATION REQUIRED
Date: 2026-09-03

## Authority boundary

This document records advisory static implementation review only.

It does **not** establish:

- C# compiler success;
- test execution success;
- Harness build/runtime success;
- native ARM64 runtime behavior;
- Windows AI/NPU behavior;
- WACK/package/Store behavior.

Those claims remain reserved for the user's native Windows ARM64 machine and later external gates.

## Approved architecture

Canonical blueprint:

`docs/blueprint/H1_PATCH_0013_EFFECTIVE_OPPORTUNITY_AUTHORITY.md`

Approved proposal:

`0.6`

Exact recursively audited proposal head:

`a060ce71c9a1dfa5ae9d9faec6b03b7a07f7d781`

Approval evidence:

`docs/evidence/H1_PATCH_0013_BLUEPRINT_APPROVAL.md`

Implementation handoff checkpoint:

`58cac0fb9942cae14d7af246f266f0154c911606`

Implementation branch:

`h1-patch-0013-effective-opportunity-authority-implementation`

Parent promoted `main`:

`8c89f998fe6f42e04a75b9090fbcc10f0574f5a2`

Exact parent Patch 0012 native machine-tested executable/test authority:

`39bc078c130ab1165c6a81c1673dd5cd25da3724`

## Production implementation surface

The complete Patch 0013 production delta from implementation baseline is limited to:

1. `src/Ensemble.E0.Core/Opportunity/OpportunityModels.cs` — added;
2. `src/Ensemble.E0.Core/Opportunity/OpportunityCanonicalizer.cs` — added;
3. `src/Ensemble.E0.Core/Opportunity/DeterministicOpportunityAuthority.cs` — added;
4. `src/Ensemble.E0.Core/Production/ProductionStateModels.cs` — one narrow internal `WithEstablishedOpportunity(CharacterId, StateHash)` helper added.

No existing Access, Context, Performer, Director, Integrity, State Interpreter, State Authority, CausalCommit, Fixture, Harness, project configuration, persistence, provider, Windows, NPU, packaging, or Store production source was changed.

`ProductionStateCanonicalizer.cs` is unchanged. Existing Patch 0012 `kind = genesis` and `kind = causalCommit` implementations therefore remain on the exact inherited source path.

## Source stability

The first complete Patch 0013 production-source head is:

`bd3c1ee7139a4405e6a39df7d46997f75d9e874e`

A repository comparison from that source head through audited pre-static-evidence head:

`bf8a42d099d291d4f06acb2337a214a278b76a25`

contains **zero production-source changes**.

All later corrections before this audit were confined to:

- tests;
- the Patch 0013 reference-oracle evidence;
- one implementation-handoff API-name correction / unreachable-test clarification.

Thus the production implementation survived the later recursive correctness/consistency/test/hygiene passes unchanged.

## Implemented boundary

Static inspection confirms the implementation follows the approved flow:

```text
postcommit ProductionState(CurrentOpportunity = null)
+ exact source E0CausalCommit
+ live accepted source ContextPacket
+ closed E0OpportunityHistory
    -> exact source state/event/history validation
    -> fresh postcommit DirectorOpportunityInput.Bind(...)
    -> existing LeastInterventionDirector.Propose(...)
    -> selected roster Character
    -> one-field Production opportunity projection
    -> canonical kind=opportunityTransition StateHash
    -> minimal E0OpportunityTransition
    -> ProductionState.WithEstablishedOpportunity(...)
    -> E0OpportunityHistory append
    -> coherent Event + State + History + fresh DirectorEvaluation
```

Replay follows:

```text
parent ProductionState
+ exact source E0CausalCommit
+ source E0OpportunityHistory
+ established E0OpportunityTransition
    -> shared source-chain validation
    -> reconstruct exact structural Director input from causal inputs
    -> existing LeastInterventionDirector.Propose(...)
    -> verify selected Character
    -> recompute exact opportunity hash
    -> narrow Production transition
    -> history append
    -> coherent replay result
```

Replay does not duplicate the least-intervention selection algorithm.

## State/history authority audit

Static review confirms:

- `ProductionState` remains authoritative current projection;
- existing `StateHash` remains the single history-sensitive state identity;
- no `HistoryHash` / `OpportunityHistoryHash` type exists;
- `E0OpportunityHistory` has no public constructor or append/update API;
- history initializes only by recomputing the existing exact genesis envelope;
- opening history contains the fixture-authored Current Opportunity once;
- live/replay require `sourceHistory.LastOpportunityStateHash == sourceCommit.ParentStateHash`;
- source history must end at the Accepted source Character;
- successful history advancement sets `LastOpportunityStateHash` to the exact opportunity-result StateHash;
- the opportunity event never embeds full prior history;
- no second mutable routing authority or generic Current Opportunity setter exists.

## Source causal binding audit

Static review confirms live/replay reject or fail closed on the approved boundaries including:

- postcommit state with non-null Current Opportunity;
- unsupported source commit contract;
- source commit ResultStateHash not equal to supplied parent state;
- non-Accepted/unsupported source Take;
- source CommitId/TakeId absent from parent effective caches;
- invalid/cross-roster Candidate control;
- source Take Scene mismatch;
- foreign/mismatched OpportunityHistory Scene/hash/final Character;
- live Context Scene/ContextPacket identity mismatch;
- live Context roster mismatch through fresh Director binding/comparison;
- replay event parent/strategy/selected/result-hash tampering.

No automatic rebase, fallback target, random repair, or alternate strategy path exists.

## Director audit

Patch 0013 reuses the existing frozen Patch 0007 Director authority exactly:

- live path calls `DirectorOpportunityInput.Bind(...)` after the source commit is proven effective;
- live path then calls `LeastInterventionDirector.Propose(...)`;
- replay reconstructs only the frozen structural input fields and calls the same `LeastInterventionDirector.Propose(...)`;
- no selection algorithm is copied into Patch 0013;
- exact strategy contract remains `ensemble.e0.director.least-intervention.v1`;
- unsupported replay strategy fails closed;
- event stores only the strategy token and selected Character, not rule/diagnostics/control/history/prose.

### Same-Character matrix clarification

The approved blueprint asked to test that repeated same-Character establishment remains legal **when valid explicit selection semantics produce it**.

Under the current frozen E0 v1 upstream contracts, immediate source-Character reselection is structurally unreachable:

- Candidate v1 forbids self-nomination;
- Candidate v1 forbids self-address;
- authoritative Director history ends at the source Character;
- with three E0 roster Characters, recency fallback therefore selects another less-recent/never-seen Character.

The implementation does not add a new prohibition against same-Character selection after Director evaluation; the case is simply unreachable through valid current v1 inputs.

`Patch0013EdgeContractTests.ImmediateSameCharacterReselection_IsUnreachableUnderFrozenE0V1Inputs` freezes that fact instead of weakening upstream contracts or inventing a public bypass for artificial coverage.

## Production mutation audit

The only new Production mutation helper is conceptually and actually:

```text
internal ProductionState WithEstablishedOpportunity(
    CharacterId selectedCharacterId,
    StateHash resultStateHash)
```

It:

- rejects non-null current opportunity;
- rejects uninitialized selected Character/result StateHash;
- requires selected Character to resolve exactly once in the current roster;
- internally derives the one-field projection change;
- preserves exact existing effective CommitId/TakeId immutable caches.

No helper accepting arbitrary `ProductionStateProjection` was added for Patch 0013.

## Canonicalization and oracle audit

Patch 0013 adds exactly one new StateHash envelope kind:

```text
opportunityTransition
```

under inherited:

```text
ensemble.e0.production-state-hash.sha256.v1
```

Exact payload:

```json
{"schemaVersion":"ensemble.e0.opportunity-transition.v1","strategyContract":"ensemble.e0.director.least-intervention.v1","selectedCharacterId":"MARLOWE"}
```

Reference evidence:

`docs/evidence/H1_PATCH_0013_REFERENCE_ORACLE.md`

Independent static reconstruction reproduced the two existing machine-established Patch 0012 hashes first:

```text
Genesis:
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104

Patch 0012 oracle postcommit:
057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30
```

Using that exact Patch 0012 postcommit state as parent, the independently derived Patch 0013 fallback reference is:

```text
SelectedCharacterId:
MARLOWE

ResultStateHash:
dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
```

`Patch0013ReferenceOracleTests` pins all three digests.

This remains static evidence until native test execution confirms it.

## Test surface

Patch 0013 adds 23 focused MSTest methods across:

- `DeterministicOpportunityAuthorityTests.cs`;
- `Patch0013ContractAuditTests.cs`;
- `Patch0013EdgeContractTests.cs`;
- `Patch0013InvariantTests.cs`;
- `Patch0013ReferenceOracleTests.cs`.

`Patch0013TestSupport.cs` supplies deterministic Missing Raft source setup without adding production authority.

The expected full Core suite count is therefore:

```text
473 inherited + 23 Patch 0013 = 496 tests
```

This is an expected count only. No passing result is claimed before native execution.

Coverage statically includes:

- exact genesis/history initialization/reset resistance;
- fallback/nomination/direct-address and direct-address ordinal tie-break;
- self-reselection unreachability under frozen E0 v1;
- exact source commit/history/Context/cache binding;
- malformed roster/foreign Scene/default selected ID;
- Candidate control hash causality;
- Performance-prose non-influence on explicit nomination selection;
- narrow Production mutation and cache preservation;
- atomic one-field result/history advancement and failure immutability;
- event minimality/privacy;
- exact payload bytes/property order;
- independent opportunity StateHash envelope computation;
- fixed Patch 0013 reference StateHash oracle;
- culture independence / repeat determinism;
- live/replay structural Director-input equivalence;
- replay foreign source/history/parent/strategy/selection/hash rejection;
- exact strategy contracts;
- no history hash, public setter, Production Access/Context overload, or hardware/provider dependency;
- exact Patch 0012 hash regressions.

## Architecture / hygiene audit

Audit order:

```text
correctness
-> consistency
-> authority
-> scope
-> tests
-> simplicity
-> hygiene
-> ARM64 suitability
-> vision
-> evidence
```

Results:

### Correctness

No material source correction remains from static review. Live/replay source-chain and hash/history relations are internally coherent.

### Consistency

Names, contract strings, event shape, result shape, and helper signature match approved Proposal 0.6.

One handoff typo (`E0OpportunityException`) was corrected to the approved `E0OpportunityTransitionException` before static closure.

### Authority

No Director proposal becomes authority without Patch 0013 transition. No generic state setter/history append/event constructor is public. Parent StateHash remains the sole causal parent pointer.

### Scope

No evolved Production->Access/Context, CharacterClaim disclosure, accepted recent Performance context, complete Scene loop, durable persistence, branch/canon/retcon/rehearsal, provider execution, UI, Windows AI/NPU, packaging, WACK, or Store implementation appears.

### Tests

Required Proposal 0.6 behaviors are represented by focused tests or, for immediate same-Character reselection, by an explicit structural-unreachability regression under frozen current E0 contracts.

A false initial "foreign genesis history" test assumption was caught during recursive review: two independently initialized Missing Raft genesis histories are semantically identical. The test was corrected to use a genuinely advanced foreign history anchor rather than preserving a false-positive assertion.

### Simplicity

No strategy interface, history hash, event ID, SourceCommitId/SourceTakeId duplicate, generic state-transition framework, or persistence abstraction was introduced.

### Hygiene

One canonical live authority and one canonical replay authority share one source-chain validation path. Existing Director and Production canonicalizers remain canonical owners of their semantics.

### ARM64 suitability

Production work is deterministic synchronous Core/BCL computation only: bounded validation/canonical hashing plus one immutable history-array append. No idle task, thread, timer, filesystem, network, GPU/NPU, provider, or Windows dependency is introduced.

No battery/runtime measurement is claimed.

### Vision

The patch makes routing itself causally remembered without turning Director attention into truth/state authorship, preserving Ensemble/Kymaean's human-agency/causal-consequence architecture.

### Evidence

Static evidence is explicit about its authority level and retains the established Patch 0012 machine authority unchanged.

## Recursive implementation audit history

### Pass 1

- source mapped to approved minimal Opportunity/Production surfaces;
- public/event/history/StateHash shape checked;
- broad Production mutation avoided;
- initial focused test suite added.

### Pass 2

- same-Character test requirement reconciled with frozen Candidate/Director v1 semantics without weakening them;
- replay foreign-history test false assumption found and corrected;
- direct-address ordinal tie-break added;
- implementation handoff exception-type name corrected.

### Pass 3

- malformed Production roster/foreign history Scene/default selected-ID coverage added;
- failure immutability and Performance-prose non-selection influence covered;
- source-scope scan found no CharacterClaim/Windows/network/random/provider/background leakage;
- independent canonical reconstruction reproduced both Patch 0012 hashes;
- exact Patch 0013 reference digest derived and pinned;
- source stability comparison confirmed zero production changes after `bd3c1ee7139a4405e6a39df7d46997f75d9e874e`.

### Final zero-material-change pass

The complete implementation was rechecked against Proposal 0.6 for:

- data flow;
- state authority;
- source/event/history anti-splice semantics;
- postcommit Director recomputation;
- Candidate-control causal binding;
- event privacy/minimality;
- narrow Production mutation;
- canonical bytes/oracles;
- replay;
- strategy compatibility;
- public API containment;
- dependency direction;
- failure/immutability behavior;
- memory/linear-history behavior;
- ARM64/no-idle suitability;
- deferred-scope preservation;
- validation claim discipline.

Result:

```text
ZERO MATERIAL SOURCE CORRECTIONS FOUND
ZERO WORTHWHILE IMPLEMENTATION SIMPLIFICATIONS FOUND
ZERO SCOPE EXPANSIONS REQUIRED
```

## Static checkpoint

Audited code/test/reference-oracle head before this evidence-only commit:

`bf8a42d099d291d4f06acb2337a214a278b76a25`

Production source head remains:

`bd3c1ee7139a4405e6a39df7d46997f75d9e874e`

## Required next authority gate

Run on the user's native Windows ARM64 machine from the exact implementation branch/head supplied after this audit:

```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug

dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug

dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json

dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
```

Expected Core test count if compilation succeeds and no discovery issue exists:

```text
496
```

Expected new fixed Patch 0013 reference hash:

```text
dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
```

Do not promote this static checkpoint to compiler/test/runtime authority until the machine evidence exists.
