# H1 Patch 0010 — Chat Recovery Audit

Date: 2026-09-03
Status: COMPLETE — repository authority reconciled; implementation remains unvalidated on target machine

## Purpose

Record the recovery from a fresh-chat bootstrap failure that temporarily reconstructed H1 Patch 0010 from stale `main` state instead of discovering the already-existing audited Patch 0010 branch.

This document is continuity evidence. It does not modify the approved Patch 0010 contract and does not increase executable validation authority.

## Root cause

At the affected fresh-chat bootstrap, `CURRENT_STATE.md` on `main` still ended at machine-validated Patch 0009 and instructed the next chat to recover the next deterministic-spine contract.

The chat read `main` but did not first enumerate relevant branches/PRs/commits. It therefore missed the already-existing branch:

`h1-patch-0010-state-authority-blueprint`

and its exact recursively audited Proposal 0.6 head:

`2188dfe7fe693c7644c0a7a74eb647974bba876d`

The chat then produced a thinner substitute State Authority discussion. That substitute is not project authority.

## Canonical Patch 0010 authority

Canonical blueprint:
`docs/blueprint/H1_PATCH_0010_DETERMINISTIC_STATE_AUTHORITY.md`

Exact recursively audited Proposal 0.6 head:
`2188dfe7fe693c7644c0a7a74eb647974bba876d`

Canonical blueprint promotion commit on `main`:
`fbd3c7004c3f047ebb2b3244e4488f993390231b`

Blueprint approval evidence checkpoint:
`b179d4a1a324349d66dce6158fc89dba5ddf0e2f`

Approval record:
`docs/evidence/H1_PATCH_0010_BLUEPRINT_APPROVAL.md`

The audited blueprint file intentionally preserves its historical Proposal 0.6 audit header. Current status must be read from `CURRENT_STATE.md` plus the approval/evidence records, not inferred from that historical header alone.

## Superseded chat claims

Any Patch 0010 architectural statement from the affected bootstrap chat is superseded when it conflicts with canonical Proposal 0.6.

In particular, the affected chat incorrectly or incompletely suggested that:

- State Authority should collapse to a proposal-level decision rather than ordered per-mutation decisions;
- partial Approved/Rejected/RequiresReview outcomes should be forbidden;
- same-target structural conflicts should generally become review rather than hard rejection;
- mutation order should be normalized away rather than preserved in proposal semantic identity;
- an empty mutation proposal should be invalid rather than valid `Complete` with zero decisions;
- `StateAuthoritySnapshot` should be renamed/reframed despite the approved contract;
- immediate Take ordering was settled, despite Proposal 0.6 explicitly leaving provisional-Take ordering open.

These claims have no authority over the repository contract or implementation.

## Implementation branch recovery

Implementation branch:
`h1-patch-0010-state-authority-implementation`

Static-audit head at recovery:
`401f6bf93aadd8879c1645876b63b729fc6ac962`

Delta from approved implementation checkpoint `b179d4a1...` is six commits and exactly four added executable/test files:

1. `src/Ensemble.E0.Core/StateAuthority/StateAuthorityModels.cs`;
2. `src/Ensemble.E0.Core/StateAuthority/DeterministicStateAuthority.cs`;
3. `tests/Ensemble.E0.Core.Tests/StateAuthority/DeterministicStateAuthorityTests.cs`;
4. `tests/Ensemble.E0.Core.Tests/StateAuthority/StateAuthorityContractAuditTests.cs`.

No pre-existing Access, Context, Performer, Director, Integrity, State Interpreter, fixture, Harness, project configuration, persistence, UI, Windows AI/NPU, packaging, or Store source is changed in that branch delta.

## Recursive static implementation audit

The recovery audit rechecked the implementation against Proposal 0.6 across:

- Interpreter proposal vs deterministic authority separation;
- proposal-content semantic identity and cryptographic domain separation;
- prose-free least-privilege evaluator input/trace;
- initial fixture-derived snapshot mapping and canonical ordering;
- SystemImmutable and exact-record CreatorLocked protection;
- typed mutation domain/transition invariants;
- ReviewSet exact proposal binding;
- hard deterministic rules before explicit review before policy default;
- mandatory-review domains and UnresolvedProposition transition floor;
- policy-eligible domains;
- support/reference/domain/ownership/protection/conflict rules;
- fixed reason ordering;
- Approved/Rejected/RequiresReview and Complete/ReviewRequired semantics;
- empty-proposal behavior;
- claim/belief/knowledge/objective-state separation;
- absence of ProductionState/StateHash/new RecordId allocation/mutation application/Take/Commit/provider/UI/NPU/Store authority;
- deterministic ordering and replay;
- immutable/non-public authority-produced construction surfaces;
- frozen Patch 0008/0009 regression identities represented in tests;
- current initial-fixture-only validation boundary.

Result at `401f6bf...`:

`STATIC / ADVERSARIAL REVIEW: PASS — no material architecture or contract defect found in the audited four-file delta.`

This is advisory only. It is not compiler, test-execution, runtime, ARM64, NPU, WACK, or Store authority.

## Validation boundary

Patch 0010 remains NOT machine-validated.

The next executable authority gate must run on the user's native Windows ARM64 development machine against the exact implementation head being tested.

Required target-machine gates remain:

1. ARM64 Core/Harness build;
2. full Core test suite;
3. Missing Raft Harness regression;
4. generic smoke Harness regression.

Only after those observed gates pass may Patch 0010 be described as machine-validated for the exercised deterministic State Authority scope.

## Fresh-chat prevention rule

When `CURRENT_STATE.md` says to recover a next contract, a fresh engineering chat must not equate "not on main" with "does not exist."

Before inventing a new contract it must inspect, in order:

1. current `main` head and `CURRENT_STATE.md`;
2. relevant active branches;
3. relevant open/merged PRs;
4. recent commits from the validated baseline;
5. exact referenced blueprint/evidence files;
6. only then broader project sources if the next boundary is still unresolved.

Repository branches and PRs are part of durable engineering state during an in-progress patch.

## Authority rule

GitHub canonical project state outranks chat reconstruction.

The affected chat is evidence of a tooling/bootstrap failure, not a source of Patch 0010 architecture.
