# Ensemble Current State

Updated: 2026-09-02

## Authority
- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- H1 Patch 0002.2 Missing Raft Fixture Contract Blueprint 0.4 is canonical for the Missing Raft fixture subset.
- H1 Patch 0003 ECJ-1 Canonical Fixture Identity Blueprint 0.2 is APPROVED and canonical.
- H1 Patch 0004 Deterministic Character-Bounded Access Control Blueprint 0.2 is APPROVED and canonical.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

H1 Patch 0003 is COMPLETE, machine-validated for its exercised gates, and promoted through PR #8.

H1 Patch 0004 is COMPLETE, machine-validated for its exercised gates, and promoted to `main` through PR #10.

## Validated executable baseline
Latest machine-tested executable implementation head:
`b228cd134f8e3258bb54fcb8e8f1fb01c21b96f8`

Patch 0004 documentation/promotion closure head on `main` before this checkpoint update:
`e0241873a6d2a85aef8a7b782e60495b7768cf61`

The commits after the tested executable head are documentation-only evidence/canonical-status closure. They do not increase executable validation authority.

Validated on the user's native Windows ARM64 machine:
- Native Windows ARM64 compiler gate: PASS — Core and Harness built successfully; Harness targeted `net9.0\win-arm64`.
- Core tests: PASS — 90 total, 90 succeeded, 0 failed, 0 skipped.
- Canonical Missing Raft runtime with structural + ECJ-1/SHA-256 enforcement: PASS; exit `0`.
- Generic smoke runtime regression: PASS; exit `0`.
- Final Patch 0004 static/adversarial/hygiene/scope review: PASS advisory.

Detailed evidence:
`docs/evidence/H1_PATCH_0004_ARM64_VALIDATION.md`

## Patch 0003 frozen identity
Canonical specification:
`docs/blueprint/H1_PATCH_0003_ECJ1_FIXTURE_HASH.md`

Missing Raft 0.1.0 frozen ECJ-1 reference remains:
- canonical UTF-8 byte length: `9112`;
- SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

The canonical Missing Raft source JSON remains unchanged.

## Patch 0004 implementation result
Canonical specification:
`docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`

Implemented architecture:
1. `CharacterBoundedAccessControl` is the single concrete deterministic implementation for `ensemble.e0.character-bounded.v1`;
2. access is derived only from Production authority category, structural Character ownership, Scene roster, and the known contract — never story prose or provenance traversal;
3. Character projections deny Production `HistoricalTruth`, `UnresolvedProposition`, `WorldState`, chronology, and every other Character's private state;
4. Scene identity/roster identities, all `SceneState`, and root `Pressure` are shared with the Scene roster;
5. each Character receives only their own Constitution, Disposition, Circumstance, Observation, Knowledge, Belief, Suspicion, Memory, Goal, and outbound Relationships;
6. inbound Relationships and all other-owner records remain denied;
7. Character-facing projection records contain only safe disclosure fields and no provenance or authoritative `Validated*` object references;
8. safe projection/audit constructors are Core-internal so external assemblies can read Access Control output but cannot forge authoritative safe-projection objects through public constructors;
9. Access Control emits the maximal permitted set; relevance, token budgeting, summarization, and prompt construction remain later Context Composer responsibilities;
10. every fixture record receives exactly one deterministic local permit/deny audit decision, sorted ordinally by Record ID;
11. the audit contains no record text/provenance and remains separate from the future Context Composer/Performer-facing surface;
12. projection collections and roster are deterministically ordered by stable IDs;
13. Access Control does not mutate `ValidatedFixture` or alter the frozen Missing Raft ECJ-1 hash;
14. generic Access Control contains no Missing-Raft-specific policy branch;
15. E0-D omniscient context remains an explicitly labeled experimental ablation outside the safe reference Access Control operation;
16. no ACL framework, policy registry, Context Composer, Director, persistence, provider/AI, Windows AI/NPU, WinUI, packaging, WACK, or Store machinery was introduced.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine: test-execution authority for tests actually exercised.
- Actual target-device execution: runtime authority for behavior actually exercised.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Explicitly unvalidated / excluded
Patch 0004 does not establish or implement:
- Context Composer or ContextPacketHash;
- ProductionState or StateHash;
- Director/opportunity orchestration;
- E0-D omniscient or relationship-omission ablation execution;
- Performer/provider/model behavior;
- Integrity Validator;
- State Interpreter or State Authority;
- causal commit/persistence;
- full observation engine;
- Windows AI or NPU execution/performance;
- WinUI;
- packaging/WACK;
- Microsoft Store certification.

Deterministic Access Control validation proves the exercised information-authority boundary. It does not itself prove context quality, model behavior, runtime orchestration, product UX, or later security boundaries.

## Immediate next action
Hold at the validated H1 Patch 0004 boundary.

Do not enter Context Composer, Director, persistence, or an E0-D bypass as Patch 0004 cleanup.

Before entering the next H1 slice:
1. read this checkpoint first;
2. locate the relevant canonical H1 roadmap/specification for the next deterministic-spine contract, expected to be the Context Composer boundary if that ordering is confirmed by repository authority;
3. if GitHub lacks an implementation-complete contract for that slice, checkpoint and explicitly approve a standalone blueprint before executable work begins;
4. preserve the Patch 0004 `CharacterAccessProjection` as the only Character-safe information input boundary unless a stronger approved specification deliberately changes it.

No further Patch 0004 implementation work is currently required.

## Continuity
Fresh chats read this file first.

For Patch 0004 history and authority, then read:
- `docs/evidence/H1_PATCH_0004_ARM64_VALIDATION.md`;
- `docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`.

For fixture/hash authority, read only as needed:
- `docs/blueprint/H1_PATCH_0003_ECJ1_FIXTURE_HASH.md`;
- `docs/blueprint/H1_PATCH_0002_2_MISSING_RAFT_CONTRACT.md`.

Do not reconstruct already-approved project state from chat history when GitHub contains it.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
