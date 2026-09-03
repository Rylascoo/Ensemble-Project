# Ensemble Current State

Updated: 2026-09-03

## Source of truth

1. GitHub `Rylascoo/Ensemble-Project` is authoritative current engineering state.
2. Read this file first in every fresh Kymaean engineering chat.
3. Resolve current `main` before modifying source.
4. Canonical approved blueprints and evidence under `docs/` govern patch-specific architecture and validation.
5. Google Drive `Ensemble Project` is supporting design/research material, not executable validation authority.
6. Archived DeskShifter V7 material is immutable regression/reference material only.

## Validation authority

Do not promote a lower validation level into a higher one.

- static/adversarial analysis: advisory;
- Visual Studio / `dotnet build` on native Windows ARM64: compiler authority for the exercised build;
- `dotnet test` on native Windows ARM64: test execution authority for the exercised suite;
- actual device/runtime exercise: runtime authority for the exercised path only;
- WACK: package-validation authority;
- Partner Center: Microsoft Store certification authority.

No current evidence establishes Windows AI/NPU execution, WACK success, packaging success, or Store certification.

## Current phase

`E0-A Harness Implementation — H1 Deterministic Spine`

Latest completed patch:

`H1 Patch 0013 — E0 Effective Opportunity Authority`

Architecture:

`FROZEN — Proposal 0.6`

Implementation:

`COMPLETE FOR EXERCISED PATCH 0013 SCOPE`

Native validation:

`COMPLETE FOR EXERCISED CORE/TEST/HARNESS GATES`

Promotion:

`PROMOTED TO MAIN`

## Current Git authority

Patch 0013 implementation/promotion PR:

`#27 — H1: implement and validate E0 Effective Opportunity Authority`

PR #27 squash-merge commit on `main`:

`15b85a25fa7969d6db69030fa712eea329471e6b`

Historical implementation branch:

`h1-patch-0013-effective-opportunity-authority-implementation`

Parent promoted `main` checkpoint before Patch 0013:

`8c89f998fe6f42e04a75b9090fbcc10f0574f5a2`

Later squash-merge and documentation/checkpoint commits are promotion/history authority only. They do **not** replace the exact native machine-observed validation SHAs below.

## Patch 0013 machine validation

Machine evidence:

`docs/evidence/H1_PATCH_0013_ARM64_VALIDATION.md`

Historical first native attempt:

`docs/evidence/H1_PATCH_0013_NATIVE_VALIDATION_ATTEMPT_01.md`

Patch 0013 reached complete exercised validation across two adjacent implementation heads because the first native run exposed a test-only MSTest analyzer defect.

### Full Core-test authority

Exact successful test head:

`a3fae23dc4df302e834b031ecfc848a3bb2d37fc`

Observed on the user's native Windows ARM64 development machine:

- full Core tests: `496/496` PASS;
- failed: `0`;
- skipped: `0`;
- `Ensemble.E0.Core` compiled successfully;
- `Ensemble.E0.Core.Tests` compiled successfully;
- test execution succeeded.

### Native Core/Harness build and fixture authority

Exact exercised Harness/Core head:

`382e11f9fbe6774806152fad75b6a23cc8733187`

Observed on the user's native Windows ARM64 development machine:

- native Core/Harness Debug build: PASS;
- Harness target: `net9.0\win-arm64`;
- Missing Raft Harness output: `Fixture validated: ensemble.e0.missing-raft@0.1.0`;
- generic smoke Harness output: `Fixture validated: ensemble.e0.smoke@0.1.0`.

The user did not separately print `$LASTEXITCODE` for those two runs, so no explicit numeric exit-code claim is made.

The only repository change between `382e11f9fbe6774806152fad75b6a23cc8733187` and `a3fae23dc4df302e834b031ecfc848a3bb2d37fc` is the analyzer correction in:

`tests/Ensemble.E0.Core.Tests/Opportunity/Patch0013ContractAuditTests.cs`

There are zero production-source changes between those heads. The production Core/Harness source exercised at the first head is therefore repository-identical to the production Core/Harness source present at the successful full-test head.

## Patch 0013 canonical hash oracles

Genesis `StateHash`:

`30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104`

Patch 0012 causal-commit post-state `StateHash`:

`057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30`

Patch 0013 effective-opportunity result `StateHash`:

`dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310`

All three are asserted by `Patch0013ReferenceOracleTests` and passed as part of the native `496/496` suite at `a3fae23dc4df302e834b031ecfc848a3bb2d37fc`.

Reference derivation evidence:

`docs/evidence/H1_PATCH_0013_REFERENCE_ORACLE.md`

## Patch 0013 final recursive audit

Final evidence:

`docs/evidence/H1_PATCH_0013_FINAL_IMPLEMENTATION_AUDIT.md`

Historical pre-native static audit:

`docs/evidence/H1_PATCH_0013_STATIC_IMPLEMENTATION_AUDIT.md`

Audit order:

`correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence`

Final post-native result:

- zero material correctness corrections;
- zero consistency corrections;
- zero authority corrections;
- zero scope corrections;
- zero worthwhile test improvements;
- zero worthwhile simplifications;
- zero material hygiene corrections;
- zero material ARM64-suitability corrections;
- zero project-vision inconsistencies;
- zero evidence corrections.

The first complete Patch 0013 production-source head was:

`bd3c1ee7139a4405e6a39df7d46997f75d9e874e`

Repository comparison from that head through the successful full-test head contains zero production-source changes.

The only native correction after the first attempted gate was test-only: two compile-time always-true contract assertions were replaced with reflection-based runtime inspection of the same exact public Director contract constants. No analyzer suppression or production workaround was added.

## Patch 0013 canonical authority

Canonical blueprint:

`docs/blueprint/H1_PATCH_0013_EFFECTIVE_OPPORTUNITY_AUTHORITY.md`

Approved Proposal:

`0.6`

Exact recursively audited proposal head:

`a060ce71c9a1dfa5ae9d9faec6b03b7a07f7d781`

Blueprint approval evidence:

`docs/evidence/H1_PATCH_0013_BLUEPRINT_APPROVAL.md`

Implementation handoff:

`docs/handoff/E0A_H1_PATCH_0013_IMPLEMENTATION_HANDOFF.md`

## Patch 0013 implemented boundary

Patch 0013 closes the deterministic Patch 0012 causal-commit -> effective-next-opportunity boundary only.

Canonical live flow:

```text
postcommit ProductionState(CurrentOpportunity = null)
+ exact source E0CausalCommit
+ accepted source ContextPacket
+ closed E0OpportunityHistory
    -> validate exact source causal chain + history anchor
    -> fresh postcommit DirectorOpportunityInput.Bind(...)
    -> existing LeastInterventionDirector.Propose(...)
    -> selected roster Character
    -> narrow Production opportunity projection
    -> canonical kind=opportunityTransition StateHash
    -> minimal E0OpportunityTransition
    -> append closed E0OpportunityHistory
    -> coherent Event + State + History + fresh DirectorEvaluation
```

Canonical one-step Replay flow:

```text
parent ProductionState
+ exact source E0CausalCommit
+ source E0OpportunityHistory
+ established E0OpportunityTransition
    -> shared source-chain validation
    -> reconstruct frozen structural Director input
    -> existing LeastInterventionDirector.Propose(...)
    -> verify selected Character
    -> recompute exact opportunity-transition hash
    -> narrow Production transition
    -> history append
    -> coherent replay result
```

Core frozen laws now implemented and exercised include:

1. Patch 0012 causal commit consumes the source Current Opportunity; Patch 0013 establishes the effective next opportunity before the next Performer boundary.
2. `ProductionState` remains the authoritative current projection.
3. Existing `StateHash` remains the sole history-sensitive state identity; no second history hash exists.
4. `E0OpportunityHistory` has closed construction, genesis-only initialization, and internal append only.
5. Genesis history initialization is accepted only when the existing exact genesis StateHash recomputes successfully.
6. `LastOpportunityStateHash` binds the history to the Production state in which the last history Character became effective opportunity.
7. Source anti-splice requires `sourceHistory.LastOpportunityStateHash == sourceCommit.ParentStateHash` and history final Character == Accepted source Character.
8. Opportunity history is not added to the Production projection, preserving inherited Patch 0012 projection/hash semantics.
9. Live authority recomputes a fresh Director input after proving the source causal chain; caller-supplied precomputed Director evaluation/proposal is not accepted.
10. The existing `ensemble.e0.director.least-intervention.v1` strategy remains the sole reference selection law.
11. Replay reuses that same Director strategy and does not duplicate the selection algorithm.
12. Candidate addressed/nominated control remains causally bound through the source Candidate/Take/CausalCommit; the opportunity event does not duplicate it.
13. The opportunity event surface remains minimal: ContractVersion, ParentStateHash, ResultStateHash, StrategyContract, SelectedCharacterId.
14. No SourceCommitId, SourceTakeId, event id, full history, control payload, prose, Director trace, or Director rule is added to the event.
15. Production mutation is limited to internal `WithEstablishedOpportunity(CharacterId, StateHash)` and only permits null -> selected roster Character while preserving effective CommitId/TakeId caches.
16. Patch 0013 adds one disjoint `kind = opportunityTransition` envelope under existing `ensemble.e0.production-state-hash.sha256.v1`.
17. Existing Patch 0012 `kind = genesis` and `kind = causalCommit` canonical paths/oracles remain unchanged.
18. Live success is atomic in memory: Event + ProductionState + OpportunityHistory + fresh DirectorEvaluation or failure.
19. Replay is one-step only and does not claim full session replay from genesis.
20. Patch 0013 adds no idle/background/network/provider/clock/random/GPU/NPU work.

## Explicit Patch 0013 non-scope

Do not extend Patch 0013 into any of the following without the separately approved next architecture:

- evolved `ProductionState -> Access/Context` integration;
- CharacterClaim / recent-Performance disclosure policy;
- full multi-turn replay from genesis;
- durable persistence/recovery;
- branch/canon/retcon/rehearsal;
- provider/model execution;
- full Scene loop;
- Observation / World Resolver;
- WinUI;
- Windows AI Foundry / NPU integration;
- MSIX packaging;
- WACK;
- Store certification.

## Open design guard

`docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md` remains a STRONG DIRECTION / OPEN DESIGN GUARD, not a frozen creator-facing schema.

> Keep authority semantics precise; keep creative semantics open.

Completed deterministic-spine authority semantics do not freeze the final creator-facing Production/Studio ontology, evolved Character Context disclosure model, final branch/rehearsal UX, or broader creative semantics.

## Prior completed machine authority

H1 Patch 0012 — E0 Atomic Causal Commit:

- canonical Proposal `0.10`;
- exact native machine-tested executable/test head `39bc078c130ab1165c6a81c1673dd5cd25da3724`;
- `473/473` Core tests PASS;
- native `win-arm64` Harness build PASS;
- Missing Raft and generic smoke PASS with explicitly observed exit `0` in its final validation evidence;
- promoted by PR #26 squash merge `bc208e6c9b46acf3c98b454df13bfc65efca5a38`;
- post-promotion checkpoint before Patch 0013: `8c89f998fe6f42e04a75b9090fbcc10f0574f5a2`.

Patch 0013 supersedes Patch 0012 as the latest completed and promoted deterministic-spine patch for the exercised scope. Patch 0012 evidence remains authoritative for its exact historical machine-tested head.

H1 Patch 0011 — E0 Take Semantics remains historical machine authority at exact tested head `4250011c167cd9850ad891aaea4ee053216cf135` with `430/430` Core tests passed.

H1 Patches 0003–0010 remain approved/canonical and machine-validated for their exercised gates. Dedicated blueprint/evidence files remain their detailed historical authority.

Do not reload or summarize all historical patches in fresh chats unless required by the current task.

## Fresh-chat bootstrap

For the next Kymaean engineering chat:

1. read this `CURRENT_STATE.md` first;
2. resolve current `main` before changing source;
3. treat PR #27 as merged and Patch 0013 as complete;
4. preserve exact full-Core-test authority `a3fae23dc4df302e834b031ecfc848a3bb2d37fc`;
5. preserve exact native Core/Harness build and fixture authority `382e11f9fbe6774806152fad75b6a23cc8733187`;
6. do not replace those machine-observed SHAs with the later squash-merge or documentation/checkpoint SHA;
7. do not reopen approved Proposal 0.6 or redesign completed Patch 0013;
8. do not silently enter deferred Patch 0013 non-scope;
9. read only the canonical roadmap and source files needed to define the next patch boundary;
10. use the same patch-first and recursive-audit discipline for the next approved implementation.

## Next action

Patch 0013 architecture, implementation, native validation, recursive audit, evidence, PR review, and promotion are complete.

Next engineering work must begin as a new patch from the canonical Kymaean roadmap rather than by extending Patch 0013 opportunistically.
