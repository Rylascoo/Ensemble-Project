# Ensemble Current State

Updated: 2026-09-03

## Authority

- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- H1 Patch 0002.2 Missing Raft Fixture Contract Blueprint 0.4 is canonical.
- H1 Patch 0003 ECJ-1 Canonical Fixture Identity Blueprint 0.2 is APPROVED and machine-validated for exercised gates.
- H1 Patch 0004 Deterministic Character-Bounded Access Control Blueprint 0.2 is APPROVED and machine-validated for exercised gates.
- H1 Patch 0005 Deterministic Context Composer + Dual Context Identity Blueprint 0.3 is APPROVED and machine-validated for exercised gates.
- H1 Patch 0006 Performer Candidate Output Contract Proposal 1.7 is APPROVED and machine-validated for exercised gates.
- H1 Patch 0007 E0 Director Opportunity Contract Proposal 1.5 is APPROVED and machine-validated for exercised gates.
- H1 Patch 0008 E0 Integrity Validator Contract Proposal 1.1 is APPROVED and machine-validated for exercised gates.
- H1 Patch 0009 E0 State Interpreter Mutation-Proposal Contract Proposal 0.7 is APPROVED, canonical, implemented, and machine-validated for exercised gates.
- H1 Patch 0010 E0 Deterministic State Authority Review/Decision Contract Proposal 0.6 is APPROVED, canonical, implemented, and machine-validated for exercised deterministic gates.
- H1 Patch 0011 E0 Take Semantics Contract Proposal 0.15 is APPROVED, canonical, implemented, machine-validated for exercised Take gates at exact head `4250011c167cd9850ad891aaea4ee053216cf135`, and promoted to `main` by PR #24 squash merge `82e572d0f56a4e4a9791722a2352e39d62e3d59d`.
- H1 Patch 0012 E0 Atomic Causal Commit Contract Proposal 0.10 is APPROVED and canonical; the first native Windows ARM64 validation attempt at `d8eb91a3bcf077fe8ce5b3bce4f59dbf493ad66d` passed the Core/Harness build but the Core test project failed to compile on four test-source diagnostics. Test-only compiler corrections are applied through exact source/test head `e91b2d30f52210cc65671329e361ae6b7407d2a3`; targeted native test rerun is pending.
- Patches 0006–0012 preserve probabilistic proposal versus deterministic authority separation and completed recursive architecture/static review before approval/closure.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless stronger frozen authority explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is authoritative current engineering state.
- Google Drive `Ensemble Project` is design/research/supporting material, not executable validation authority.

## Open design guard

`docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md` remains a STRONG DIRECTION / OPEN DESIGN GUARD, not a frozen final schema.

> Keep authority semantics precise; keep creative semantics open.

Patch 0009–0012 authority semantics do not freeze the final creator-facing Production/Studio ontology, evolved Character Context disclosure model, or final Take/branch/rehearsal UX.

## Current phase

E0-A Harness Implementation — H1 Deterministic Spine.

Current architecture checkpoint:

`H1 Patch 0012 — E0 Atomic Causal Commit`

Status:

`NATIVE ARM64 CORE/HARNESS BUILD PASSED — TEST COMPILER CORRECTION APPLIED — TARGETED TEST RERUN PENDING`

Exact corrected Patch 0012 source/test head:

`e91b2d30f52210cc65671329e361ae6b7407d2a3`

First native validation evidence:

`docs/evidence/H1_PATCH_0012_NATIVE_VALIDATION_ATTEMPT_01.md`

Historical static implementation audit:

`docs/evidence/H1_PATCH_0012_STATIC_IMPLEMENTATION_AUDIT.md`

Latest fully machine-validated executable checkpoint:

`H1 Patch 0011 — E0 Take Semantics`

Exact machine-tested head:

`4250011c167cd9850ad891aaea4ee053216cf135`

Status:

`COMPLETE FOR EXERCISED E0 TAKE SEMANTICS GATES`

Patch 0012 has partial machine evidence only: at `d8eb91a3bcf077fe8ce5b3bce4f59dbf493ad66d`, the native Windows ARM64 Core/Harness Debug build passed and produced the Harness under `net9.0\win-arm64`, while the Core test project failed during compilation. The two Harness fixture runs also succeeded, but Patch 0012 is not yet a complete compiler/test/Harness milestone because the test assembly did not compile. Patch 0011 therefore remains the latest fully machine-validated executable/test authority.

## Patch 0012 canonical authority

Canonical blueprint:
`docs/blueprint/H1_PATCH_0012_ATOMIC_CAUSAL_COMMIT.md`

Approved proposal:
`0.10`

Exact recursively audited Proposal 0.10 head:
`ad1e84121653457454f6bf90e831e2064a79f224`

Blueprint approval record:
`docs/evidence/H1_PATCH_0012_BLUEPRINT_APPROVAL.md`

Fresh-chat implementation handoff:
`docs/handoff/E0A_H1_PATCH_0012_IMPLEMENTATION_HANDOFF.md`

Historical static implementation audit:
`docs/evidence/H1_PATCH_0012_STATIC_IMPLEMENTATION_AUDIT.md`

First native validation attempt:
`docs/evidence/H1_PATCH_0012_NATIVE_VALIDATION_ATTEMPT_01.md`

Implementation branch:
`h1-patch-0012-atomic-causal-commit-implementation`

Approved implementation baseline:
`163696f4a89aa3a3b1ea4975167c15b827328d1d`

Historical statically audited source/test head:
`48e7932c8ba8753709f2d37464ad366eebbddea7`

First native-tested branch head:
`d8eb91a3bcf077fe8ce5b3bce4f59dbf493ad66d`

Corrected source/test head after native compiler feedback:
`e91b2d30f52210cc65671329e361ae6b7407d2a3`

Blueprint branch:
`h1-patch-0012-atomic-causal-commit-blueprint`

Approval/promotion PR:
`#25 — H1: approve Atomic Causal Commit contract`

Canonical blueprint promotion squash commit on `main`:
`570061f0f035b06b752137bf091e70fedde331cc`

Important status rule:

The canonical Patch 0012 blueprint preserves its historical Proposal 0.10 audit-stage header at the exact approved bytes. Current approval status is established by this file plus `docs/evidence/H1_PATCH_0012_BLUEPRINT_APPROVAL.md`; do not edit the audited blueprint merely to turn its historical header into a live status field.

The Patch 0012 static implementation audit is historical advisory evidence. The user's native compiler feedback supersedes any implication that static convergence proved test-project compilability.

PR #25 is documentation-only. It introduced no Patch 0012 executable/test code and establishes no compiler, runtime, ARM64, NPU, WACK, packaging, or Store validation.

The current implementation branch contains Patch 0012 source/tests and the test-only compiler correction. Complete machine validation still requires a successful test compile/execution and final grouped gate on the corrected branch.

## Frozen Patch 0012 architecture boundary

Proposal 0.10 establishes the E0 atomic causal-commit contract only.

Key frozen laws include:

1. the exact Accepted Performance and every retained Approved consequence become effective together, or neither does;
2. retained Rejected consequences never become effective;
3. only Accepted Takes cross the commit-binding boundary; Rejected/Alternate Takes remain non-effective Patch 0011 provenance;
4. `ProductionState` is an immutable current projection; causal events remain the conceptual creative-history source of truth;
5. `ValidatedFixture` remains immutable genesis input and is never mutated into evolved state;
6. `ProductionStateCheckpoint.Capture(...)` occurs before Access/Context/Performance, retains the exact immutable source-state reference, and is O(1) metadata/reference work without re-hashing or full-state revalidation;
7. Patch 0012 introduces history-sensitive `StateHash` for genesis and causal-commit transitions only;
8. binding performs one exact Production-derived StateAuthority snapshot association proof; Commit then uses exact StateHash equality as freshness authority and does not repeat that O(record-ledger) snapshot projection;
9. Replay has no binding and therefore independently verifies its parent Production-derived StateAuthority snapshot against the retained event Take;
10. `E0TakeStateBinding` is Accepted-only and retains the exact immutable E0Take reference it validated plus exact SourceStateHash;
11. `DeterministicCausalCommit.Commit(...)` consumes the binding and accepts no separately substitutable Take parameter;
12. stale/current-state mismatch fails closed and may not rewrite/substitute the immutable Take consequence package;
13. existing `CommitId`, `RecordId`, and `TakeId` remain canonical; no second ID type or Core allocator/format is introduced;
14. caller-supplied RecordId materializations exist exactly for retained Approved Add/Supersede mutations, never for Approved Deactivate or retained Rejected mutations;
15. Add/Supersede/Deactivate application follows exact retained proposal/decision semantics and makes no second approval decision;
16. Supersede/Deactivate ExistingRecordId remains transition lineage in the Take proposal and is not silently inserted into support Provenance;
17. RecordIds are globally unique across active and inactive Production records and are never reused;
18. Fixture and Production share one internal neutral RecordId provenance-DAG validation primitive rather than duplicate cycle algorithms;
19. Production domain/lifecycle/protection enums have explicit `Unspecified = 0` values and map semantically to existing Patch 0010 enums; numeric enum casts are forbidden because their numeric layouts intentionally differ;
20. existing fixture-derived StateAuthority snapshot behavior remains compatible while a new Production-derived snapshot overload maps evolved state without Text/provenance leakage;
21. one internal exact StateAuthority snapshot semantic comparator owns Scene/roster/descriptor equality; no SnapshotHash is introduced;
22. the causal event is minimal: CommitId, ParentStateHash, ResultStateHash, exact Accepted Take, and exact RecordMaterializations only;
23. there is no AppliedEffects hierarchy or duplicated committed-opportunity/domain/kind/existing-target authority;
24. successful commit consumes Current Opportunity and sets result CurrentOpportunityCharacterId to null; Patch 0012 does not choose/apply the next Director opportunity;
25. zero-mutation and all-Rejected-consequence Accepted Takes remain valid commits and advance causal history/StateHash because the Performance became historical;
26. canonical Production projection JSON, causal Take payload, materialization payload, invariant integer encoding, and StateHash envelopes have frozen byte-level rules;
27. Patch 0012 reuses existing internal `CanonicalJson` rules and one shared Patch 0009 mutation-domain token mapping rather than competing serializers/token tables;
28. derived effective CommitId/TakeId indexes are immutable duplicate-rejection caches and are intentionally excluded from canonical Production projection hashing;
29. Commit and Replay share one deterministic transition/canonicalization engine;
30. Replay guarantees exactly one commit-owned transition from an authoritative parent state; full multi-turn replay from genesis is explicitly not claimed;
31. current Access remains fixture-based; evolved Production -> Access/Context integration is deferred because CharacterClaim/recent-Performance disclosure semantics and direct Context StateHash identity are not yet frozen;
32. Patch 0012 does not define next-opportunity StateHash transitions, durable persistence/recovery, branch/canon, retcon/rehearsal, provider execution, Scene loop, Observation, World Resolver, UI, Windows AI/NPU, packaging, WACK, or Store behavior;
33. `ProductionStateException` and `E0CausalCommitException` are public sealed typed domains with no public constructors and sanitized expected upstream normalization;
34. unexpected programming/runtime failures are not catch-all relabeled as ordinary commit failure;
35. Patch 0012 Core architecture introduces no network/filesystem/clock/random/provider/GPU/NPU/polling/background/global-mutable-state work and no idle work.

## Patch 0012 implementation checkpoint

Implementation began from the approved promoted `main` checkpoint `163696f4a89aa3a3b1ea4975167c15b827328d1d` on the approved implementation branch.

Required authority was read before executable changes:

1. `CURRENT_STATE.md`;
2. `docs/handoff/E0A_H1_PATCH_0012_IMPLEMENTATION_HANDOFF.md`;
3. `docs/blueprint/H1_PATCH_0012_ATOMIC_CAUSAL_COMMIT.md`;
4. `docs/evidence/H1_PATCH_0012_BLUEPRINT_APPROVAL.md`;
5. `docs/ENGINEERING_HYGIENE_CONSTITUTION.md`;
6. only the exact upstream source/tests needed for the smallest implementation surface.

Patch-first implementation remained inside the approved Production/CausalCommit surface plus narrow Fixture/StateAuthority/StateInterpreter refactors required by Proposal 0.10.

Recursive static audit order:

`correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence`

At historical source/test head `48e7932c8ba8753709f2d37464ad366eebbddea7`, static review found no further material correction. Native test-project compilation then exposed four test-source diagnostics at branch head `d8eb91a3bcf077fe8ce5b3bce4f59dbf493ad66d`. Patch-first correction changed only the two affected test files and produced corrected source/test head `e91b2d30f52210cc65671329e361ae6b7407d2a3`.

The correction preserves the frozen contract: one ambiguous BCL overload call was made explicit, and three compile-time-constant assertions were converted to reflection-based public constant-surface checks. No production semantics or architecture changed.

Do not claim complete Patch 0012 compiler/test/runtime validation until the corrected branch passes the required native Windows ARM64 test and final grouped gates.

## Patch 0011 executable and machine authority

Canonical Patch 0011 blueprint:
`docs/blueprint/H1_PATCH_0011_TAKE_SEMANTICS.md`

Approval evidence:
`docs/evidence/H1_PATCH_0011_BLUEPRINT_APPROVAL.md`

Machine evidence:
`docs/evidence/H1_PATCH_0011_ARM64_VALIDATION.md`

Implementation/promotion PR:
`#24 — H1: implement and validate E0 Take semantics`

Exact native Windows ARM64 machine-tested executable/test head:
`4250011c167cd9850ad891aaea4ee053216cf135`

Squash-merge commit on `main`:
`82e572d0f56a4e4a9791722a2352e39d62e3d59d`

At exact machine-tested head `4250011c167cd9850ad891aaea4ee053216cf135`:

- Core/Harness Debug native ARM64 build: PASS;
- Harness output target: `net9.0\win-arm64`;
- full Core tests: `430/430` PASS, `0` failed, `0` skipped;
- Missing Raft Harness: PASS;
- generic smoke Harness: PASS.

This remains the current fully machine-validated compiler/test/Harness authority until Patch 0012 independently closes its corrected validation gate.

## Prior patch evidence

Detailed prior architecture, compiler-correction, and machine-validation histories remain authoritative in their dedicated files under `docs/evidence/` and approved blueprints/handoffs under `docs/blueprint/` and `docs/handoff/`.

In particular:

- Patch 0010 blueprint: `docs/blueprint/H1_PATCH_0010_DETERMINISTIC_STATE_AUTHORITY.md`;
- Patch 0010 machine evidence: `docs/evidence/H1_PATCH_0010_ARM64_VALIDATION.md`;
- Patch 0011 blueprint: `docs/blueprint/H1_PATCH_0011_TAKE_SEMANTICS.md`;
- Patch 0011 machine evidence: `docs/evidence/H1_PATCH_0011_ARM64_VALIDATION.md`;
- Patch 0012 first native validation attempt: `docs/evidence/H1_PATCH_0012_NATIVE_VALIDATION_ATTEMPT_01.md`.

`CURRENT_STATE.md` is the live checkpoint, not a duplicate transcript of every historical compiler iteration.

## Immediate next action

Pull the corrected Patch 0012 implementation branch and rerun the Core test project first:

```powershell
git pull --ff-only
git status --short
git rev-parse HEAD
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

The exact corrected source/test content head is `e91b2d30f52210cc65671329e361ae6b7407d2a3`; later documentation-only commits do not alter that executable/test content.

If the targeted test rerun passes, run the complete Patch 0012 native Windows ARM64 grouped validation gate on the same branch before recording Patch 0012 as fully machine-validated. If another compiler/test failure appears, patch only the smallest affected surface and rerun the relevant gate.

Implementation status:

`TEST COMPILER CORRECTION APPLIED — TARGETED NATIVE TEST RERUN PENDING`

Do not enter later evolved Access/Context or next-opportunity integration scope during Patch 0012.

## Explicit validation limits

Current machine evidence establishes for Patch 0012 branch head `d8eb91a3bcf077fe8ce5b3bce4f59dbf493ad66d`:

- native Windows ARM64 Core/Harness Debug build: PASS;
- Harness output target: `net9.0\win-arm64`;
- Missing Raft Harness execution after that build: PASS;
- smoke Harness execution after that build: PASS.

It does **not** establish successful Core test-project compilation or test execution. Those remain pending on the corrected source/test content through `e91b2d30f52210cc65671329e361ae6b7407d2a3`.

Current repository state does not establish any of the following later or higher validation claims:

- authoritative evolved ContextPacket / ProductionState common-state identity;
- authoritative new RecordId allocation policy;
- full multi-turn state-transition replay;
- next-opportunity ProductionState transition;
- persistence/recovery;
- authenticated provider/policy/review/Take-disposition provenance machinery;
- provider/model State Interpreter execution;
- evolved Production -> Access/Context execution;
- Scene-loop execution;
- World Resolver / Observation engine;
- final branching/rehearsal/retcon/alternate-promotion UX;
- Windows AI / NPU execution or performance;
- WinUI behavior;
- packaging / WACK;
- Microsoft Store certification.

## Frozen regression identities

Missing Raft / Voss Context:

- StructuredContextHash `bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- ContextPacketId `CTX:bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- RenderedContextHash `ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88`.

Patch 0008 Candidate-content oracle:
`18eb8c9ba9d35f34f84e1fdd983eeb2a19ce015a2e44457c4514576f984af3b2`

Missing Raft ECJ-1:

- `9112` UTF-8 bytes;
- SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

Patch 0012 test oracles pending successful native test execution:

- genesis StateHash `6e5bb762614a6885721a5d160b99b8c67dc6d096a560e3f04a15fc10a7451376`;
- oracle post-commit StateHash `59ae86a03a3aad544ca828406349c1b5cc148871a4b479ad8fff6101d2be71bf`.

These Patch 0012 values remain test expectations until the corrected native test gate executes successfully.

## Validation authority

- Static/adversarial review: advisory only.
- Visual Studio / `dotnet` ARM64 build output on target machine: compiler authority.
- `dotnet test` on target machine: test-execution authority for exercised tests.
- Actual target-device execution: runtime authority for exercised behavior.
- NPU execution requires explicit hardware evidence.
- WACK is package-validation authority.
- Partner Center is Store-certification authority.

Never promote a lower validation level into a higher one.

## Fresh-chat bootstrap law

Fresh engineering chats must:

1. read `CURRENT_STATE.md` first;
2. resolve current `main` branch/commit;
3. inspect relevant active branches, PRs, and recent commits before concluding a contract or implementation does not exist;
4. read only the exact blueprint/evidence/source files needed for the current task;
5. treat machine evidence supplied by the user as authoritative for that machine;
6. patch the smallest affected surface;
7. update this checkpoint only when a meaningful project state changes.

When this file says to recover a contract, `not on main` does not mean `does not exist`.

Do not reconstruct approved project state from chat history when GitHub contains it.

## Project rule

Every patch is reviewed against the previous validated baseline:

`correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence`

Git preserves history; the active source tree preserves only the best current architecture.
