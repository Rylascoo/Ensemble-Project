# Q-PROD-01 Three-Engineer Workstream Charter — 2026-09-17

Status: **PREPARED / NOT YET ACTIVATED**

This charter applies the three-engineer Director amendment to the active Q-PROD-01 Product build.

It does not create a second backlog, a new Product phase, provider authority, deferred-E0 execution authority, or final architecture authority.

## Common starting point

All three Engineers must fresh-resolve live Project authority before work.

At activation they must begin from the then-current exact validated `main`, not from this document's creation-time SHA.

They must read:

1. `AGENTS.md`;
2. exact-ref `CURRENT_STATE.md`;
3. `docs/PROJECT_AUTHORITY.md`;
4. `docs/PROJECT_EXECUTION_QUEUE.md`;
5. `docs/VALIDATION_LEDGER.md`;
6. `docs/PROJECT_THREE_ENGINEER_PARALLEL_OPERATING_MODEL_DIRECTOR_AMENDMENT_2026_09_17.md`;
7. applicable Product architecture/Design authority.

## Engineer #1 — Engineering Sol / Persistence & Integration

Primary production lane: P1 persistence/recovery and Engineering integration.

Owned implementation surface:

- `src/Kymaean.Infrastructure.Persistence/**`;
- `tests/Kymaean.Infrastructure.Persistence.Tests/**`;
- shared Engineering integration/authority surfaces.

Initial technical sequence, subject to fresh audit:

1. schema/version policy;
2. rebuildable snapshot policy/implementation;
3. portable credential-independent export;
4. broader corruption/interruption evidence;
5. final provisional storage/concurrency policy.

Engineer #1 also owns serialized integration of all three lanes.

Engineer #1 must not silently redefine Engineer #2 Application semantics or Engineer #3 Windows behavior during integration.

## Engineer #2 — Engineering Relay A / Application & Product Architecture

Primary goal: unblock real app architecture and Design continuation.

Owned implementation surface:

- `src/Kymaean.Application/**`;
- `tests/Kymaean.Application.Tests/**`.

Initial work should recover existing Product event/replay/lifecycle behavior and define only the smallest earned Application architecture needed by the real app.

Priority questions include:

- stable Production identity/open/select semantics;
- creator-facing commands and queries;
- deterministic Application projections;
- current Production/current situation boundaries;
- model-neutral contracts for Home, Productions, Production shaping, Live Stage and History;
- Presentation-facing state that does not expose Persistence/provider/platform machinery.

Engineer #2 must not implement disk/file mechanics, WinUI/platform APIs, providers, or visual design.

First major milestone: `DESIGN_ARCHITECTURE_READY`.

A returned milestone packet must identify exact contracts Design may safely design against and exact still-provisional areas.

## Engineer #3 — Engineering Relay B / Windows Runtime & Composition

Primary goal: make the native shell consume real Product/Application contracts without inventing Product meaning.

Owned implementation surface:

- `src/Kymaean.Windows/**`;
- explicitly leased Windows/platform tests.

Initial work should recover the existing WinUI shell and implement the smallest real runtime composition boundary.

Priority questions include:

- composition root;
- construction of Application/Persistence services;
- packaged Product storage/bootstrap location;
- activation/relaunch/open/recover plumbing;

- strict MVVM separation;
- Windows lifecycle seams;
- safe consumption of Engineer #2 Application contracts.

Engineer #3 must not invent Application semantics, Persistence formats, provider semantics, or visual design.

If current Product history cannot reconstruct a UI state, Engineer #3 must fail closed and request the missing Application/Persistence contract rather than fabricate it.

## Lease status protocol

Every active Engineer mutation lease is represented by one GitHub Issue used only as transport.

Required state header:

```text
ENGINEER: #1 | #2 | #3
LEASE ID:
QUEUE ITEM: Q-PROD-01
STATUS: LEASED | IN_PROGRESS | BLOCKED_ON_INTERFACE | INTERFACE_READY | VALIDATING | RETURNED | CLOSED
BASE EXACT MAIN:
BRANCH:
HEAD:
OWNED WRITE SURFACE:
CURRENT PHASE:
DEPENDENCIES:
UNBLOCKS:
NEXT ALLOWED ACTION:
STOP CONDITIONS:
```

The opening Issue records `LEASED`. After fresh authority recovery and branch/worktree creation, the Engineer posts an `IN_PROGRESS` comment before the first production mutation. Later status changes are posted as Issue comments when they materially alter what another Engineer may do; the latest valid exact-ref transition comment is the operational status.

Routine internal progress does not require repository chatter.

Before `INTERFACE_READY`, `VALIDATING`, or `RETURNED`, fresh-resolve relevant live refs and race-check the lease baseline. Material movement requires reconciliation before advancing.

`IN_PROGRESS` means implementation has started under the lease.

`INTERFACE_READY` means another named Engineer may consume an exact checkpoint.

`BLOCKED_ON_INTERFACE` means work cannot correctly proceed without another Engineer's owned contract.

`RETURNED` means implementation/testing is complete for reconciliation; it is not authority adoption.

`CLOSED` is recorded only after disposition/integration or explicit abandonment is reconciled.

## Interface request packet

A cross-engineer request must be precise:

```text
REQUEST FROM:
REQUEST TO:
CONSUMER TASK:
REQUIRED CONTRACT / BEHAVIOR:
WHY CURRENT CONTRACT IS INSUFFICIENT:
EXACT CONSUMER BASE/HEAD:
MUST PRESERVE:
DO NOT ASSUME:
BLOCKING: yes | no
```

The owning Engineer may satisfy, reject, narrow or defer the request under current Product authority.

## Interface-ready packet

An `INTERFACE_READY` checkpoint must record:

```text
PROVIDER ENGINEER:
EXACT BRANCH/HEAD:
CONTRACTS PROVIDED:
FILES:
VALIDATION COMPLETED:
KNOWN LIMITATIONS:
AUTHORIZED CONSUMERS:
BASELINE STALENESS RULE:
```

Consumers must treat the checkpoint as provisional until Engineer #1 integrates it to validated `main`.

## Merge and dependency order

No worker merge is self-authorizing.

Engineer #1 chooses merge order based on dependency truth.

Typical order:

1. Application contract producer;
2. Persistence and/or Windows consumers;
3. dependent-branch reconstruction against validated main;
4. remaining consumer;
5. shared authority reconciliation.

Independent work may integrate earlier when it has no dependency edge.

Every merge requires expected-head protection and exact-main validation before it becomes a baseline for new authoritative work.

## Shared-file rule

Engineer #2 and Engineer #3 must not edit:

- `CURRENT_STATE.md`;
- `docs/PROJECT_EXECUTION_QUEUE.md`;
- `docs/VALIDATION_LEDGER.md`;
- this charter or its governing Director amendment;
- shared final integration/closeout authority.

They return facts to Engineer #1 instead.

Engineer #1 should also avoid modifying worker-owned implementation files except for explicitly transferred ownership or semantics-neutral conflict resolution.

## Design coordination

Design Sol remains Design authority.

Engineer #2's `DESIGN_ARCHITECTURE_READY` packet should be the primary architecture handoff that reopens app-design work after Engineer #1 integrates and exact-main validates it.

Design Sol must not treat a provisional worker branch or `INTERFACE_READY` checkpoint as adopted Product architecture.

Engineer #3 may provide Windows feasibility/runtime facts to Design without selecting visual outcomes.

Engineer #1 may provide persistence/runtime truth inputs without becoming Design authority.

## Activation plan

Activation is intentionally deferred until this charter and its governing amendment are merged and exact-main Validation passes.

Then:

- Engineer #1 continues in the current chat;
- a fresh chat is started as Engineer #2 from live repository authority;
- a second fresh chat is started as Engineer #3 from live repository authority;
- each receives one initial mutation lease;
- all three lanes may then progress concurrently under this charter.

No Engineer #2/#3 branch or lease should be created before that activation point.


## Initial activation leases

The exact baseline SHA is resolved at activation; these objectives are pre-authorized.

### ENG1-QPROD01-PERSIST-01

Engineer: #1.

Objective: determine and implement the smallest fresh Product persistence schema/version policy that safely extends the current append-only journal/event boundary without inventing broader Product ontology.

Primary owned surface: Persistence implementation/tests plus Engineer #1 authority/integration surfaces.

Expected return: exact source/evidence, native validation, compatibility/failure semantics, and the next earned Persistence boundary.

### ENG2-QPROD01-APPARCH-01

Engineer: #2.

Objective: recover current Application/Product behavior and implement the smallest stable Application architecture needed to unblock the real app and Design.

Primary owned surface: `src/Kymaean.Application/**` and Application tests.

Required output includes a precise `DESIGN_ARCHITECTURE_READY` assessment even if the answer is not-ready.

Stop rather than invent when Product ontology, Persistence mechanics, Windows implementation, provider semantics or visual judgment are required.

### ENG3-QPROD01-WINCOMP-01

Engineer: #3.

Objective: recover the current WinUI shell and implement the smallest real Windows runtime-composition/bootstrap boundary that can correctly consume existing Application/Persistence contracts.

Primary owned surface: `src/Kymaean.Windows/**` and explicitly leased Windows/platform tests.

Begin with composition root, Product service construction, packaged storage/bootstrap and activation/relaunch/recover seams that are already supported by current contracts.

If a required Application or Persistence contract is missing, publish `BLOCKED_ON_INTERFACE` with an exact request rather than editing the foreign lane.

Expected return: exact source/evidence, native ARM64 validation, interface requests, and a clear statement of which design/runtime states are now real versus still placeholders.

## Initial coordination intent

Engineer #2 is expected to create the first likely cross-lane checkpoint because Application contracts are upstream of much Windows/Product-state work.

Engineer #1 and Engineer #3 may proceed independently where current contracts already suffice.

No Engineer waits merely because another lease is active; it waits only on an actual dependency edge.

When an interface checkpoint unblocks a dependent lane, that lane may consume the exact checkpoint immediately if its lease allows it, while final adoption remains serialized through Engineer #1.
