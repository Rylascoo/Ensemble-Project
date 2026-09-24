# Q-PROD-09 — First Product-Native Performance — Native ARM64 Validation

Status: **NATIVE ARM64 PASS / VALIDATED PRODUCT CANDIDATE / MERGE PENDING DIRECTOR**

Date: 2026-09-24

## Authority and scope

Director disposition:
`docs/FIRST_PRODUCT_NATIVE_PERFORMANCE_Q1_Q2_Q5_DIRECTOR_DISPOSITION_2026_09_24.md`.

Integrated base:
`f685a21a699f3c9300a8db15358aa5d5d125115f`.

Implementation branch:
`qprod09/first-product-native-performance-2026-09-23`.

Draft PR:
#266, unmerged.

Exact machine-tested source:
`5038029c79b2177330cc06e81ac338f4b830ad3f`.

Final annotated validation tag:
`validation/q-prod-09-first-product-native-performance-native-arm64-r2`.

Tag object:
`aadeb922b5105a60458cbcdff0dd4cf9dac0173b`.

Verified peel:
`5038029c79b2177330cc06e81ac338f4b830ad3f`.

The earlier tag `validation/q-prod-09-first-product-native-performance-native-arm64` remains preserved at its earlier pre-final source and is not moved or reinterpreted.

## Implemented Product boundary

The executable implements only the Director-adopted first Product-native Performance milestone:

1. explicit established `SceneId + CharacterId` Opportunity;
2. actor membership in that Scene's initial roster;
3. bounded `CharacterPerformanceContext` containing SceneId, CharacterId, CharacterName and only that actor's committed Circumstances;
4. invocation-scoped `IProductPerformer`;
5. separate invocation-scoped `IProductConsequenceInterpreter`;
6. Product-owned Performance text validation with exact empty silence and valid non-empty Character-legible text;
7. provisional deterministic first-slice acceptance;
8. exactly one additive Character Circumstance consequence;
9. one atomic `AcceptedPerformanceCommittedEvent` containing accepted visible Performance plus its consequence;
10. Product `ProductionHistoryRevision` freshness, checked against current authoritative replay inside the locked pre-append journal validation;
11. replay-derived accepted Performance history, persistence codec, reopen/recovery, portable export and snapshot v5 support;
12. later acting-Character context receives that Character's committed Circumstance.

`Kymaean.Application` has no `Ensemble.E0.Core` project reference.

No provider/model identity, provider network traffic, credential, spend, persistent casting, Current/Active Scene or automatic Director selection is present.

## Durable contract and migration boundary

Event:
`kymaean.production.accepted-performance-committed.v1`.

The event contains:

- SceneId;
- acting CharacterId;
- exact accepted visible Performance text;
- one additive Character Circumstance text.

The event is one physical and semantic journal append. Accepted Performance and consequence cannot partially commit.

Future same-family event versions fail closed as `ProductionPersistenceCompatibilityException` and map to Product `Incompatible`.

Journal framing version remains 1.

Portable-export outer version remains 1; it carries individually versioned Product event payloads.

Projection snapshot advances v4 -> v5 because rebuildable cached projection content now includes accepted Performance history. Snapshot identity remains anchored to authoritative journal sequence/hash. Old/invalid snapshots are discarded/rebuilt; snapshot state is never causal authority.

`ProductionHistoryRevision` is Product-owned and derived from authoritative durable event count. It detects semantically idempotent and ABA history advances that projection-only equality cannot detect.

## Native target validation

Host: SurfSeven.

Environment:

- Windows ARM64;
- .NET SDK 10.0.400;
- .NET runtime 10.0.11;
- RID `win-arm64`.

Exact source `5038029c79b2177330cc06e81ac338f4b830ad3f`:

- `Kymaean.Application.Tests`: **92/92 PASS**, native `net10.0|arm64`;
- `Kymaean.Infrastructure.Persistence.Tests`: **153/153 PASS**, native `net10.0|arm64`;
- `Kymaean.Windows` Release `-p:Platform=ARM64 -r win-arm64`: **PASS**, **0 warnings / 0 errors**;
- `git diff --check main...HEAD`: clean;
- detached validation worktree source status: clean.

The Persistence suite includes an explicit idempotent-history-advance falsifier: a same-value World-current replacement advances Product history revision while preserving semantic projection, and a Performance generated from the older revision is rejected before any journal mutation.

## Hosted validation

Exact source-head push run:
35960989225.

Exact source-head PR run:
35960992415.

On both runs:

- Product Application tests: PASS;
- Product persistence tests: PASS;
- Core tests: PASS;
- compiler gate: PASS;
- Product WinUI ARM64 cross-compile: PASS;
- document authority census: PASS;
- oracle assertion coverage: PASS.

The sole failed job was repository-law state currency because documentation closeout was intentionally deferred until executable stabilization:

- push run: `CURRENT_STATE.md` stale by 8 commits, maximum 3;
- PR run: `CURRENT_STATE.md` stale by 9 commits, maximum 3.

This evidence/state closeout exists to repair that governance-only failure. It does not change the machine-tested executable source.

## Independent review / correction chain

Independent review was repeatedly run from a detached exact-source worktree.

Material findings and dispositions:

1. **Performer/consequence conflation** — repaired by separating `IProductPerformer` and `IProductConsequenceInterpreter`.
2. **Stale context could append before ProductApplication noticed** — repaired by moving freshness validation into locked pre-append persistence.
3. **Semantic replay equality allowed idempotent/ABA history advance** — repaired with Product-owned `ProductionHistoryRevision`; explicit regression added.
4. **Concurrent accepted-Performance extension could make existing Character/Scene/World commands report Invalid after a valid durable append** — repaired with exact-prefix accepted-history preservation.
5. **New Performance replay path was quadratic in Performance count** — repaired by accumulating replay state and materializing accepted Performance history once.
6. **Malformed/invisible non-silent Performance text could enter history** — repaired with Product-owned semantic validation; empty string remains exact valid silence.
7. **Q-PROD-09 current authority had not yet been durably reconciled** — not a source defect; resolved by the 2026-09-24 Director disposition plus this closeout.

High-effort review also proposed broader E0 semantics such as zero-consequence accepted Performance and recent Performance text in later Character context. Those are not adopted as Product law in this milestone: the Director-selected Q2/Q5 scope intentionally requires one additive Character Circumstance and requires the consequence—not recent transcript text—to become observable in the actor's later bounded Product context.

Final exact-source review after executable repairs produced no remaining executable correctness finding; its sole remaining P1 was the pending durable authorization/state update completed by this documentation closeout.

Reviewer behavior was no-write as verified by clean detached worktree status. Technical write containment is not claimed because the review CLI reported a workspace-write sandbox.

## Deferred-validation exposure

Open deferred gates:

- Q-E0D-01;
- Q-E0E-RUN;
- Q-E0F-01;
- Q-E0G-01;
- Q-E0-CONV;
- Q-POSTE0-01;
- Q-POSTE0-02.

Exposure classification:
**direct durable semantic exposure / provisional Product implementation**.

Falsifier:
if later evidence shows the first Product consequence cannot remain exactly one additive actor Circumstance, or that the bounded context/access rule is insufficient, Product semantics must change under explicit version/compatibility authority.

Replaceable seams:

- `IProductPerformer`;
- `IProductConsequenceInterpreter`;
- `CharacterPerformanceContext`;
- provisional acceptance policy;
- `IProductionPerformanceCommitter`.

Migration/fail-closed:

- accepted-performance-committed.v1 meaning is immutable;
- no silent reinterpretation of accepted history;
- explicit migration/compatibility or fail-closed handling is required if semantics later change;
- projections rebuild from journal causal authority;
- snapshots remain disposable/rebuildable cache.

## Non-authority

This PASS does not authorize:

- PR #266 merge;
- PR #265 or PR #262 merge;
- first live AI Performance;
- provider/model/credentials/network/spend;
- automatic Director selection;
- Current/Active Scene or lifecycle;
- ODR-19 closure;
- deferred-E0 execution;
- final runtime architecture;
- Q-ALPHA-01, Beta/release, WACK or Store advancement.

Next consequential action after hosted closeout validation is explicit Director merge disposition for draft PR #266.
