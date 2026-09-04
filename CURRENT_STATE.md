# Ensemble Current State

Updated: 2026-09-04

## Source of truth

1. GitHub `Rylascoo/Ensemble-Project` is authoritative current engineering state.
2. Read this file first in every fresh Kymaean engineering chat.
3. Resolve current `main` before modifying source.
4. Canonical approved blueprints and evidence under `docs/` govern patch-specific architecture and validation.
5. Google Drive `Ensemble Project` is supporting design/research material, not executable validation authority.
6. Archived DeskShifter V7 material is immutable regression/reference material only.

## Program architecture authority

Approved program map:

`docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`

Approved proposal:

`0.7`

Exact recursively audited plan commit:

`c2687b4905f6f5eddcd7744c14643421f7ff79e4`

Exact audited plan content SHA:

`ed33ef60ebd7becb09857b8b7e172f2b1c51d711`

Audit evidence:

`docs/evidence/KYMAEAN_ARCHITECTURE_SHIP_PLAN_AUDIT.md`

Director approval evidence:

`docs/evidence/KYMAEAN_ARCHITECTURE_SHIP_PLAN_APPROVAL.md`

The approved plan freezes major dependency ordering from H1/E0 through productization, Alpha/Beta, ARM64/Windows AI/NPU integration, packaging/WACK, and Partner Center while preserving patch-level architecture/implementation approval gates.

## Mandatory Sol High task-scope protocol

After reading this file, every fresh engineering chat must read:

`docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`

For every substantive Director turn, deliberately optimize the work package for GPT-5.6 Sol High: choose the largest logically coupled, falsifiable objective that can be completed with current authority/tools; perform all directly dependent source synthesis, contradiction/falsification analysis, authorized implementation, targeted verification, and recursive audit; then stop at the next consequential Director, machine, security, WACK, Store, or other external-validation gate.

This protocol changes collaboration granularity only. It does not override product architecture, validation authority, Director approval, or external gates.

## Validation authority

Do not promote a lower validation level into a higher one.

- static/adversarial analysis: advisory;
- native Windows ARM64 `dotnet build`: compiler authority for the exercised build;
- native Windows ARM64 `dotnet test`: test execution authority for the exercised suite;
- actual device/runtime exercise: runtime authority for the exercised path only;
- WACK: package-validation authority;
- Partner Center: Microsoft Store certification authority.

No current evidence establishes WinUI runtime behavior, Windows AI Foundry/NPU execution, measured NPU performance/TOPS, MSIX/WACK success, or Store certification.

## Current phase

`E0-A Harness Implementation — H1 Deterministic Spine`

Latest completed patch:

`H1 Patch 0015 — E0 Accepted Performance History + Context Continuity`

Architecture:

`FROZEN — Proposal 0.15`

Implementation:

`COMPLETE FOR EXERCISED PATCH 0015 SCOPE`

Native validation:

`COMPLETE FOR EXERCISED CORE/TEST/HARNESS/FIXTURE GATES`

Promotion:

`PROMOTED TO MAIN`

## Current Git authority

Patch 0015 implementation/promotion PR:

`#29 — H1: implement and validate Recent Performance Context Continuity`

PR #29 squash-merge commit on `main`:

`6ac5936cbf4f344211dece8f95650aa06dd33b0f`

Historical implementation branch:

`h1-patch-0015-accepted-performance-history-implementation`

Parent promoted `main` checkpoint before Patch 0015:

`7475a9397cff9063673908c666a729f0f3cd4525`

Later merge/checkpoint/documentation SHAs are promotion/history authority only. They do **not** replace exact native machine-observed validation SHAs below.

## Patch 0015 machine validation

Canonical evidence:

`docs/evidence/H1_PATCH_0015_NATIVE_ARM64_VALIDATION.md`

Initial partial-attempt evidence:

`docs/evidence/H1_PATCH_0015_NATIVE_ARM64_VALIDATION_ATTEMPT_01.md`

The user's native machine reported:

```text
PROCESSOR_ARCHITECTURE = ARM64
OS = Windows 10.0.26200
RID = win-arm64
.NET SDK = 9.0.317
.NET host = 10.0.11
.NET host architecture = arm64
global.json = repository root
```

### Full Core-test authority

Exact successful full-test head:

`b890b7eca66c391fae3ec30af0442dcc0e9f6aec`

Observed:

- `Ensemble.E0.Core` compiled successfully;
- `Ensemble.E0.Core.Tests` compiled successfully;
- full Core tests: `571/571` PASS;
- failed: `0`;
- skipped: `0`;
- `CORE_TEST_EXIT=0`;
- tracked working-tree diff check: clean;
- staged diff check: clean.

The user's unrelated untracked `patch0012-local-edit.txt` remained untouched and outside tracked/staged cleanliness authority.

### Native Harness build and fixture authority

Exact exercised Harness/Core head:

`5cb055e6dddea721aee98fee7f633191543e6490`

Observed:

- Harness Debug build: PASS;
- `HARNESS_BUILD_EXIT=0`;
- Missing Raft output: `Fixture validated: ensemble.e0.missing-raft@0.1.0`;
- `MISSING_RAFT_EXIT=0`;
- generic smoke output: `Fixture validated: ensemble.e0.smoke@0.1.0`;
- `GENERIC_SMOKE_EXIT=0`.

The only executable/test change after this Harness/fixture execution and before the successful full Core-test head was one inherited test assertion correction in:

`tests/Ensemble.E0.Core.Tests/CausalCommit/Patch0012StructuralImplementationTests.cs`

Comparison `5cb055e6... -> b890b7e...` contains no `src/` or fixture changes. Therefore Harness/fixture authority remains the exact `5cb055e6...` execution while full Core-test authority is `b890b7e...`.

### Native feedback correction

Initial full Core test run at `5cb055e6...` compiled Core and tests but produced:

```text
570/571 PASS
1 FAIL
```

Failing inherited structural test:

`BindingOwnsTheSingleSourceSnapshotProofAndCommitDoesNotRepeatIt`

Root cause:

- Patch 0015 intentionally centralized both public `E0TakeStateBinding` entry points through shared private `BindCore(...)`;
- the inherited Patch 0012 IL test still expected `ProductionStateAuthoritySnapshot.Bind(...)` directly inside the historical public `Bind(...)` wrapper;
- authority behavior remained correct, but the inherited structural assertion no longer matched implementation structure.

Patch-first correction commit:

`b890b7eca66c391fae3ec30af0442dcc0e9f6aec`

The corrected test now proves:

1. historical `Bind(...)` calls shared `BindCore(...)` exactly once;
2. `BindWithAcceptedHistory(...)` calls the same `BindCore(...)` exactly once;
3. neither wrapper duplicates source snapshot proof;
4. `BindCore(...)` performs exactly one `ProductionStateAuthoritySnapshot.Bind(...)`;
5. `BindCore(...)` performs exactly one `StateAuthoritySnapshotSemanticComparer.Equals(...)`;
6. `DeterministicCausalCommit.Commit(...)` performs neither proof operation.

Targeted rerun: PASS.

Full rerun: `571/571` PASS.

No production implementation correction was required.

## Patch 0015 canonical authority

Canonical blueprint:

`docs/blueprint/H1_PATCH_0015_RECENT_PERFORMANCE_CONTEXT_CONTINUITY.md`

Approved Proposal:

`0.15`

Exact recursively audited proposal head:

`cea5820b9614cf9028340a23cf931696feb98680`

Blueprint audit:

`docs/evidence/H1_PATCH_0015_BLUEPRINT_AUDIT.md`

Blueprint approval:

`docs/evidence/H1_PATCH_0015_BLUEPRINT_APPROVAL.md`

Implementation handoff:

`docs/handoff/E0A_H1_PATCH_0015_IMPLEMENTATION_HANDOFF.md`

Static implementation audit:

`docs/evidence/H1_PATCH_0015_STATIC_IMPLEMENTATION_AUDIT.md`

Independent reference oracle:

`docs/evidence/H1_PATCH_0015_REFERENCE_ORACLE.md`

Native validation:

`docs/evidence/H1_PATCH_0015_NATIVE_ARM64_VALIDATION.md`

Final recursive implementation audit:

`docs/evidence/H1_PATCH_0015_FINAL_IMPLEMENTATION_AUDIT.md`

PR review audit:

`docs/evidence/H1_PATCH_0015_PR_REVIEW_AUDIT.md`

Documentation clarification discovered during PR review:

`docs/evidence/H1_PATCH_0015_DOCUMENTATION_ERRATA.md`

The errata preserves historical checkpoint artifacts and clarifies only:

1. an older handoff live-oracle ID example superseded by exact Proposal 0.15/reference-oracle inputs;
2. ambiguous dependency-arrow shorthand in historical blueprint-audit prose.

Neither clarification changes implementation, canonical values, public APIs, or machine authority.

## Patch 0015 live reference oracle

Reference evidence:

`docs/evidence/H1_PATCH_0015_REFERENCE_ORACLE.md`

Inherited exact genesis Production state remains:

```text
StateHash
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
```

Inherited Production-bound VOSS Context v2 remains:

```text
Structured bytes = 2655
StructuredContextHash = 27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
ContextPacketId = CTX:27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
Rendered bytes = 1905
RenderedContextHash = ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88
```

Exact Patch 0015 v2-source semantic identities:

```text
CandidateContentHash
6ce2a98dc6208400126feba1a356910376d40c6155843764ecf673b8b9441ec1

ProposalContentHash
ac0f7a91c855b99fdb6f1b9415ff150352e8e3f79268e666d38531323b7d16c3
```

Exact Patch 0015 live postcommit StateHash:

```text
a7e6e1d5e386b2f6b459f8432b85ee7b14c3b25dbfec6e228240f98e523fe96c
```

Exact Patch 0015 live Opportunity StateHash:

```text
e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151
```

Expected selected Character remains:

`MARLOWE`

First nonempty MARLOWE Context v3:

```text
SchemaVersion = ensemble.e0.context.v3
CompositionContract = ensemble.e0.context.production-bound.accepted-history.v1
RenderingContract = ensemble.e0.context.render.v2
SourceStateHash = e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151
Structured bytes = 3521
StructuredContextHash = ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f
ContextPacketId = CTX:ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f
Rendered bytes = 2443
RenderedContextHash = 668c632ebdb4e4a2de838cbc5ae49b17005984880345ed28ec0c6eaa2bfcef16
```

First accepted recent Performance semantics:

```text
SourceCharacterId = VOSS
VisibleText = No.
```

Exact rendered recent layer:

```text
[RECENT PERFORMANCES]
Dr. Voss:
[PERFORMANCE]
- No.
```

These values are asserted inside the native `571/571` suite.

## Patch 0015 implemented boundary

Patch 0015 closes the deterministic accepted Character-legible Performance-history continuity seam reserved by Patch 0005 and deliberately left empty through Patch 0014.

Approved live sequence:

```text
current opportunity-bearing ProductionState
+ synchronized opaque E0AcceptedPerformanceHistory
+ synchronized E0OpportunityHistory
    -> ProductionStateCheckpoint.Capture
    -> E0ProductionContextContinuity.ComposeWithAcceptedHistory
         exact empty genesis => historical v2
         synchronized accepted history => v3
    -> Performer Candidate
    -> Integrity
    -> State Interpretation
    -> State Authority
    -> E0Take.Bind(... Accepted ...)
    -> E0TakeStateBinding.BindWithAcceptedHistory
         full structured+rendered precommit source proof
    -> DeterministicCausalCommit.Commit
         staged result
    -> E0AcceptedPerformanceHistoryContinuity.RecordCommit
         fresh structured semantic source proof
         + canonical causal replay
         + append exactly one accepted Character-legible Performance
    -> adopt postcommit Production/history pair only after success
    -> DeterministicOpportunityAuthority.Establish
         staged result
    -> E0AcceptedPerformanceHistoryContinuity.RecordOpportunity
         canonical Opportunity replay
         + routing/history coupling
    -> adopt opportunity-bearing Production/history/OpportunityHistory triple only after success
```

Core frozen laws now implemented and exercised include:

1. `E0AcceptedPerformanceHistory` is a closed opaque in-memory synchronization/projection token, not an event store.
2. Its public surface has no public constructor, setter, property, or declared method.
3. Internal history state is only SceneId, CurrentStateHash, and ordered `ImmutableArray<ContextRecentPerformance>`.
4. `ContextRecentPerformance` exposes only `SourceCharacterId` + exact `VisibleText`.
5. No per-entry CommitId, TakeId, StateHash, ContextPacketId, Candidate hash, typed control, State Authority data, materialization, provider data, or epistemic classification is projected into Character Context history.
6. One internal Domain-layer Character-legible-text invariant owns the exact former Performer VisibleText grammar.
7. Performer delegates to that neutral invariant without changing historical Candidate semantics/messages.
8. Context does not depend upward on Performer/CausalCommit/Opportunity/Continuity.
9. CausalCommit does not depend upward on Opportunity or Continuity.
10. Historical Context v1 remains exact.
11. Production-bound Context v2 remains exact.
12. Context v3 is additive and requires synchronized nonempty accepted Performance history.
13. V1/v2 require initialized empty recent history.
14. V3 structured identity includes exact accepted Performance source Character, text, and causal append order.
15. V3 render-v2 changes only the recent Performance layer; trusted-state and opportunity rendering remain inherited.
16. Semantic silence is preserved distinctly from empty history and literal silence-marker prose.
17. `ComposeWithAcceptedHistory` emits exact v2 only at exact empty genesis; synchronized nonempty evolved history emits v3.
18. Historical `Compose(checkpoint)` remains a compatibility API.
19. `BindWithAcceptedHistory` is the approved Full Ensemble live precommit proof path.
20. Precommit history-aware binding freshly recomputes Access and exact expected Context and compares canonical structured/rendered bytes plus identities/hashes.
21. Historical three-argument `Bind(...)` remains available and rejects v3.
22. `RecordCommit` revalidates synchronized history/state, recomposes exact structured semantic source Context identity, and canonical-replays the causal event before append.
23. `RecordCommit` does not overclaim independent historical rendered-byte proof; that belongs to precommit binding.
24. Accepted zero-mutation or all-durable-consequence-Rejected Takes still append the accepted Character-legible Performance because historical occurrence and durable consequence are separate layers.
25. Rejected/Alternate/failed/non-effective paths do not append.
26. `RecordOpportunity` reuses canonical Opportunity replay, advances no Performance item, and couples Opportunity-history count to Performance-history count.
27. Current E0 `ensemble.e0.copresent-trio.v1` gains only the bounded common recent-Performance eligibility rule defined by Proposal 0.15.
28. Patch 0015 does not generalize co-presence into complete perception or create CharacterObservation.
29. Recent Performance remains historical fictional occurrence, not objective truth, Knowledge, Belief, Suspicion, Memory, or CharacterClaim.
30. Self history remains included when a Character later receives Opportunity again.
31. Accepted history preserves exact current-Scene append order, including identical repeated items and Character recurrence.
32. No provider-session memory is required for this continuity.
33. No provider/model/network/filesystem/clock/random/background/GPU/NPU/Windows-platform dependency is introduced.

## Final recursive audit

Final implementation evidence:

`docs/evidence/H1_PATCH_0015_FINAL_IMPLEMENTATION_AUDIT.md`

PR-review evidence:

`docs/evidence/H1_PATCH_0015_PR_REVIEW_AUDIT.md`

Audit order:

```text
correctness
-> consistency
-> authority
-> disclosure/privacy
-> dependency direction
-> canonical/version compatibility
-> scope
-> tests/reference oracle
-> simplicity
-> hygiene
-> ARM64 suitability
-> project vision
-> evidence
```

The final post-native pass reached zero material corrections and zero worthwhile in-scope improvements.

PR review then found two documentation-only historical ambiguities, recorded in `H1_PATCH_0015_DOCUMENTATION_ERRATA.md`. After that clarification, the restarted full PR review again reached:

`ZERO MATERIAL CORRECTIONS / ZERO WORTHWHILE IN-SCOPE IMPROVEMENTS`

## Explicit Patch 0015 non-scope

Do not extend Patch 0015 into any of the following without a separately approved next architecture:

- general CharacterObservation generation;
- location/hearing/attention/concealment/private-Performance observation semantics;
- CharacterClaim Context disclosure;
- Knowledge/Belief/Suspicion/Memory promotion from recent Performance;
- provider/model invocation;
- provider request/attempt/retry/spend/streaming provenance;
- relevance/windowing/summarization/token-budget/index layers;
- cross-Scene history retrieval;
- full Scene-loop orchestration;
- durable causal-event persistence/recovery;
- full replay from genesis;
- branch/canon/retcon/rehearsal;
- World Resolver;
- WinUI;
- Windows AI Foundry/NPU integration;
- MSIX packaging;
- WACK;
- Store certification.

## Open design guard

`docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md` remains a STRONG DIRECTION / OPEN DESIGN GUARD, not a frozen creator-facing schema.

> Keep authority semantics precise; keep creative semantics open.

Completed deterministic-spine authority semantics do not freeze final creator-facing Production/Studio ontology, future CharacterClaim/Performance disclosure policy, final Observation model, branch/rehearsal UX, or broader creative semantics.

## Prior completed machine authority

Patch 0014 remains historical machine authority:

- Proposal `0.10`;
- full Core-test authority `4ac0250005c8d88c3b815c5d53cfca0a982e454c` — `538/538` PASS;
- Harness/fixture authority `84b3e23db55910f746670cd2e06a67b8a5dea2b3`;
- PR #28 squash merge `16eb353305b7674a5730f8090549c06a0af03a3d`;
- Patch 0014 promoted checkpoint before Patch 0015 `7475a9397cff9063673908c666a729f0f3cd4525`.

Patch 0013 remains historical machine authority:

- Proposal `0.6`;
- full Core-test head `a3fae23dc4df302e834b031ecfc848a3bb2d37fc` — `496/496` PASS;
- Harness/fixture head `382e11f9fbe6774806152fad75b6a23cc8733187`;
- PR #27 squash merge `15b85a25fa7969d6db69030fa712eea329471e6b`.

Patch 0012 remains historical machine authority at `39bc078c130ab1165c6a81c1673dd5cd25da3724` with `473/473` Core tests passed plus native Harness/fixture gates.

Patch 0011 remains historical machine authority at `4250011c167cd9850ad891aaea4ee053216cf135` with `430/430` Core tests passed.

H1 Patches 0003–0010 remain approved/canonical and machine-validated for their exercised gates. Dedicated blueprint/evidence files remain their detailed historical authority.

Do not reload or summarize all historical patches in fresh chats unless required by the current task.

## Fresh-chat bootstrap

For the next Kymaean engineering chat:

1. read this `CURRENT_STATE.md` first;
2. read `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` and optimize every substantive task scope for GPT-5.6 Sol High;
3. read `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md` only as needed for the current program boundary;
4. resolve current `main` before changing source;
5. treat PR #29 as merged and Patch 0015 as complete;
6. preserve exact full-Core-test machine authority `b890b7eca66c391fae3ec30af0442dcc0e9f6aec`;
7. preserve exact native Harness/fixture authority `5cb055e6dddea721aee98fee7f633191543e6490`;
8. do not replace those machine-observed SHAs with later squash-merge/checkpoint SHAs;
9. preserve all frozen Context v1/v2 identities and Patch 0015 v3 reference-oracle identities;
10. read `docs/evidence/H1_PATCH_0015_DOCUMENTATION_ERRATA.md` if consulting the historical Patch 0015 handoff or blueprint-audit arrow shorthand;
11. do not reopen approved Proposal 0.15 or redesign completed Patch 0015;
12. do not silently enter Patch 0015 deferred non-scope;
13. read only the canonical roadmap and source files needed to identify the next patch boundary;
14. before substantial new implementation, recursively audit the next blueprint and obtain explicit approval;
15. use the same patch-first, machine-authority, recursive-audit, PR-review, and promotion discipline for the next approved implementation.

## Next action

The Kymaean Architecture & Ship Plan Proposal 0.7 is Director-approved as the program map. Patch 0015 remains the latest completed executable patch.

Next engineering work is to identify and blueprint the smallest remaining H1/E0-A deterministic boundary required to reach a complete run driver from current Patch 0015 authority. No implementation begins until that next blueprint is recursively audited and explicitly approved.
