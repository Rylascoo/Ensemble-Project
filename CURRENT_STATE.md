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
- native Windows ARM64 `dotnet build`: compiler authority for the exercised build;
- native Windows ARM64 `dotnet test`: test execution authority for the exercised suite;
- actual device/runtime exercise: runtime authority for the exercised path only;
- WACK: package-validation authority;
- Partner Center: Microsoft Store certification authority.

No current evidence establishes Windows AI/NPU execution, performance/TOPS, MSIX/WACK success, or Store certification.

## Current phase

`E0-A Harness Implementation — H1 Deterministic Spine`

Latest completed patch:

`H1 Patch 0014 — E0 Production Context Continuity`

Architecture:

`FROZEN — Proposal 0.10`

Implementation:

`COMPLETE FOR EXERCISED PATCH 0014 SCOPE`

Native validation:

`COMPLETE FOR EXERCISED CORE/TEST/HARNESS GATES`

Promotion:

`PROMOTED TO MAIN`

## Current Git authority

Patch 0014 implementation/promotion PR:

`#28 — H1: implement and validate E0 Production Context Continuity`

PR #28 squash-merge commit on `main`:

`16eb353305b7674a5730f8090549c06a0af03a3d`

Historical implementation branch:

`h1-patch-0014-production-context-continuity-implementation`

Parent promoted `main` checkpoint before Patch 0014:

`e06668a2307433bf99b0501dc38a701db392c633`

Later merge/checkpoint/documentation SHAs are promotion/history authority only. They do **not** replace the exact native machine-observed validation SHAs below.

## Patch 0014 machine validation

Machine evidence:

`docs/evidence/H1_PATCH_0014_NATIVE_ARM64_VALIDATION.md`

The user's native machine reported:

```text
PROCESSOR_ARCHITECTURE = ARM64
RID = win-arm64
.NET host architecture = arm64
.NET SDK = 9.0.317
```

### Full Core-test authority

Exact successful test head:

`4ac0250005c8d88c3b815c5d53cfca0a982e454c`

Observed:

- `Ensemble.E0.Core` compiled successfully;
- `Ensemble.E0.Core.Tests` compiled successfully;
- full Core tests: `538/538` PASS;
- failed: `0`;
- skipped: `0`;
- `CORE_TEST_EXIT=0`;
- tracked working-tree diff check: clean;
- staged diff check: clean.

### Native Core/Harness build and fixture authority

Exact exercised Harness/Core head:

`84b3e23db55910f746670cd2e06a67b8a5dea2b3`

Observed:

- native Core/Harness Debug build: PASS;
- `HARNESS_BUILD_EXIT=0`;
- Missing Raft output: `Fixture validated: ensemble.e0.missing-raft@0.1.0`;
- `MISSING_RAFT_EXIT=0`;
- generic smoke output: `Fixture validated: ensemble.e0.smoke@0.1.0`;
- `GENERIC_SMOKE_EXIT=0`.

The first full Core-test attempt at this head exposed exactly one test-source compiler defect:

```text
ProductionContextContinuityTests.cs(44,25): CS0103
The name 'MissingRaftContract' does not exist in the current context
```

Patch-first correction at `4ac0250...` added only:

```csharp
using Ensemble.E0.Core.Fixture;
```

GitHub comparison proves exactly one changed test file and one added line between the Harness head and the successful test head. There are zero production, Harness, or fixture changes between them.

The user's unrelated untracked `patch0012-local-edit.txt` was preserved and never modified, staged, compiled as source, or used as Patch 0014 authority.

## Patch 0014 canonical Context oracles

Reference derivation:

`docs/evidence/H1_PATCH_0014_REFERENCE_ORACLE.md`

Frozen historical Context v1 / Missing Raft VOSS remains:

```text
Structured bytes = 2569
StructuredContextHash = bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b
Rendered bytes = 1905
RenderedContextHash = ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88
```

Patch 0014 Context v2 genesis / VOSS:

```text
Source StateHash = 30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
Structured bytes = 2655
StructuredContextHash = 27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
ContextPacketId = CTX:27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
Rendered bytes = 1905
RenderedContextHash = ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88
```

Patch 0014 Context v2 canonical Patch0012→Patch0013 evolved / MARLOWE:

```text
Source StateHash = dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
Structured bytes = 3456
StructuredContextHash = 9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00
ContextPacketId = CTX:9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00
Rendered bytes = 2389
RenderedContextHash = 9925327709d4a99697d4b65e5ad870e6e3ff5c5d573e25c61814e48d77163a2d
```

These are asserted by `Patch0014ReferenceOracleTests` and passed inside the native `538/538` suite at `4ac0250005c8d88c3b815c5d53cfca0a982e454c`.

## Patch 0014 canonical authority

Canonical blueprint:

`docs/blueprint/H1_PATCH_0014_PRODUCTION_CONTEXT_CONTINUITY.md`

Approved Proposal:

`0.10`

Exact recursively audited proposal head:

`0c937054940a335d6a6f08f68d7effd104f944d2`

Blueprint audit:

`docs/evidence/H1_PATCH_0014_BLUEPRINT_AUDIT.md`

Blueprint approval:

`docs/evidence/H1_PATCH_0014_BLUEPRINT_APPROVAL.md`

Implementation handoff:

`docs/handoff/E0A_H1_PATCH_0014_IMPLEMENTATION_HANDOFF.md`

Static implementation audit:

`docs/evidence/H1_PATCH_0014_STATIC_IMPLEMENTATION_AUDIT.md`

Native validation:

`docs/evidence/H1_PATCH_0014_NATIVE_ARM64_VALIDATION.md`

Final recursive audit:

`docs/evidence/H1_PATCH_0014_FINAL_IMPLEMENTATION_AUDIT.md`

Final production-source change before test/evidence-only hardening:

`4d4455ad7167aaf864230a350d5151d14f9c7fa3`

Repository comparison from that head through the successful full-test head contains zero `src/` changes.

## Patch 0014 implemented boundary

Patch 0014 closes the deterministic current-Production -> Character Access -> Character Context source-proof boundary.

Canonical flow:

```text
current ProductionState with Current Opportunity
    -> ProductionStateCheckpoint.Capture(...)
        -> CharacterBoundedAccessControl.Evaluate(ProductionState, CharacterId)
            -> CharacterAccessEvaluation
                -> internal Production-bound DeterministicContextComposer
                    -> ContextPacket v2 + ContextCompositionTrace
                        -> E0ProductionContextContinuityResult
```

Exact accepted-source proof in `E0TakeStateBinding`:

```text
source ProductionStateCheckpoint
+ supplied source ContextPacket
+ Accepted E0Take
    -> cheap state/schema/subject/Scene checks
    -> fresh Production Access recomposition
    -> fresh Context recomposition
    -> exact canonical structured bytes comparison
    -> exact canonical rendered bytes comparison
    -> ContextPacketId / StructuredContextHash / RenderedContextHash comparison
    -> inherited StateAuthority snapshot proof
    -> bind or fail closed
```

Core frozen laws now implemented and exercised include:

1. Access Control remains before Context/relevance.
2. Production-backed Access is deterministic, synchronous, side-effect free, and evaluates every retained record exactly once for an AccessDecision.
3. Inactive lifecycle exclusion has precedence over ownership/domain disclosure reasons.
4. Active `HistoricalTruth`, `UnresolvedProposition`, and `WorldState` remain denied.
5. Active `SceneState` and `Pressure` remain shared/public permitted state.
6. Constitution, Disposition, Circumstance, Observation, Knowledge, Belief, Suspicion, Memory, Goal, and Relationship material are permitted only for the owning subject.
7. Other-owned Character material remains denied.
8. Active `CharacterClaim` is denied for every Character with `CharacterClaimDisclosureDeferred`.
9. No current recall is inferred merely because a CharacterClaim exists.
10. No observation is inferred from co-presence, Candidate visible text, addressed Characters, nomination, Director selection, relationships, SceneState, or causal adjacency.
11. `CharacterAccessProjection.SourceStateHash` is exact Production state association metadata, not fictional knowledge.
12. Fixture-backed Access keeps `SourceStateHash = null` and historical v1 behavior.
13. Context v1 constants and exact frozen Missing Raft digests remain unchanged.
14. Production-bound Context uses `ensemble.e0.context.v2` + `ensemble.e0.context.production-bound.v1` while reusing `ensemble.e0.context.render.v1`.
15. No render-v2 contract exists.
16. Context v2 canonical bytes include `sourceStateHash` immediately after `compositionContract`.
17. `recentPerformances` remains exactly `[]` in canonical structured Context and `RecentPerformanceText` remains empty.
18. `SourceStateHash` never enters Character-facing rendered text.
19. The historical public Context composer rejects Production-backed Access projections; the Production-bound composer remains internal.
20. `E0ProductionContextContinuity.Compose(ProductionStateCheckpoint)` is the only public Continuity composition path.
21. Continuity returns both Access and Context evaluations so Access decisions are not discarded or recomputed merely for audit visibility.
22. `E0TakeStateBinding` does not trust matching `SourceStateHash` alone; exact freshly recomposed content/bytes/identities must also match.
23. Evolved Production states require exact v2 Context.
24. Legacy v1 Take binding is permitted only for an exact genesis Production state and exact freshly Production-derived v1-equivalent Context.
25. CausalCommit uses lower Access + Context directly for source proof and does not depend on Continuity, preserving dependency direction.
26. Production remains independent of Access/Context/Continuity.
27. Opportunity remains unchanged by Patch 0014.
28. Patch 0014 adds no idle/background/network/provider/clock/random/GPU/NPU work.

## Final recursive audit

Audit order:

```text
correctness
-> consistency
-> authority
-> disclosure/privacy
-> dependency direction
-> canonical/version compatibility
-> scope
-> tests
-> simplicity
-> hygiene
-> ARM64 suitability
-> project vision
-> evidence
```

Final post-native result:

- zero material correctness corrections;
- zero consistency corrections;
- zero authority/privacy corrections;
- zero dependency-direction corrections;
- zero canonical/version corrections;
- zero scope corrections;
- zero worthwhile test improvements;
- zero worthwhile simplifications;
- zero material hygiene corrections;
- zero material ARM64-suitability corrections;
- zero project-vision inconsistencies;
- zero evidence corrections.

## Explicit Patch 0014 non-scope

Do not extend Patch 0014 into any of the following without a separately approved next architecture:

- CharacterClaim Context disclosure;
- recent-Performance Context population;
- observation eligibility / CharacterObservation generation;
- full Scene loop;
- provider/model invocation;
- retry/streaming/spend behavior;
- full multi-turn replay from genesis;
- durable persistence/recovery;
- relevance/token/summarization/index layers;
- World Resolver / spatial hearing semantics;
- branch/canon/retcon/rehearsal;
- WinUI;
- Windows AI Foundry / NPU integration;
- MSIX packaging;
- WACK;
- Store certification.

## Open design guard

`docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md` remains a STRONG DIRECTION / OPEN DESIGN GUARD, not a frozen creator-facing schema.

> Keep authority semantics precise; keep creative semantics open.

Completed deterministic-spine authority semantics do not freeze the final creator-facing Production/Studio ontology, future CharacterClaim/Performance disclosure policy, final Observation model, branch/rehearsal UX, or broader creative semantics.

## Prior completed machine authority

H1 Patch 0013 — E0 Effective Opportunity Authority remains historical machine authority:

- Proposal `0.6`;
- exact full-Core-test head `a3fae23dc4df302e834b031ecfc848a3bb2d37fc`;
- `496/496` Core tests PASS;
- exact Core/Harness/fixture head `382e11f9fbe6774806152fad75b6a23cc8733187`;
- PR #27 squash merge `15b85a25fa7969d6db69030fa712eea329471e6b`;
- post-promotion checkpoint before Patch 0014 `e06668a2307433bf99b0501dc38a701db392c633`.

H1 Patch 0012 — E0 Atomic Causal Commit remains historical machine authority at exact tested head `39bc078c130ab1165c6a81c1673dd5cd25da3724` with `473/473` Core tests passed plus native Harness/fixture gates.

H1 Patch 0011 — E0 Take Semantics remains historical machine authority at exact tested head `4250011c167cd9850ad891aaea4ee053216cf135` with `430/430` Core tests passed.

H1 Patches 0003–0010 remain approved/canonical and machine-validated for their exercised gates. Dedicated blueprint/evidence files remain their detailed historical authority.

Do not reload or summarize all historical patches in fresh chats unless required by the current task.

## Fresh-chat bootstrap

For the next Kymaean engineering chat:

1. read this `CURRENT_STATE.md` first;
2. resolve current `main` before changing source;
3. treat PR #28 as merged and Patch 0014 as complete;
4. preserve exact full-Core-test authority `4ac0250005c8d88c3b815c5d53cfca0a982e454c`;
5. preserve exact native Harness/fixture authority `84b3e23db55910f746670cd2e06a67b8a5dea2b3`;
6. do not replace those machine-observed SHAs with the later squash-merge or checkpoint SHA;
7. preserve frozen Context v1 and Patch 0014 v2 reference identities;
8. do not reopen approved Proposal 0.10 or redesign completed Patch 0014;
9. do not silently enter Patch 0014 deferred non-scope;
10. read only the canonical roadmap and source files needed to define the next patch boundary;
11. before substantial new implementation, recursively audit the next blueprint and obtain explicit approval;
12. use the same patch-first, machine-authority, and recursive-audit discipline for the next approved implementation.

## Next action

Patch 0014 architecture, implementation, native validation, recursive audit, evidence, PR review, and promotion are complete.

Next engineering work must begin as a new patch from the canonical Kymaean roadmap rather than by extending Patch 0014 opportunistically.
