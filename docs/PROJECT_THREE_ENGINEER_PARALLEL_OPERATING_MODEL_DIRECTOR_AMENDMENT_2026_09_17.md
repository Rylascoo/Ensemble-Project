# Project Three-Engineer Parallel Operating Model — Director Amendment — 2026-09-17

Status: **DIRECTOR-APPROVED OPERATING LAW / PREPARED / ENGINEERS #2 AND #3 NOT YET ACTIVATED**

## Purpose

This amendment authorizes three persistent ChatGPT Engineering contexts to advance the Ensemble/Kymaean product in parallel without creating three Engineering authorities, a second backlog, automatic task pickup, or concurrent authority integration.

It is a narrow successor to:

- `docs/PROJECT_PARALLEL_AGENT_OPERATING_MODEL_DIRECTOR_AMENDMENT_2026_09_13.md`;
- `docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md`;
- `docs/KYMAEAN_PRODUCT_BUILD_AHEAD_OF_DEFERRED_E0_VALIDATION_DIRECTOR_AMENDMENT_2026_09_17.md`.

Where this amendment is more specific about persistent Engineering contexts, it governs. All unaffected prior law remains active. The existing Design Sol / Design Relay structure is unchanged.

The governing principle remains:

> **Parallelize implementation and bounded reasoning; serialize authority and integration.**

## Three Engineer identities

There is still exactly one accountable Engineering manager.

### Engineer #1 — Engineering Sol

Engineer #1 is the sole accountable Engineering manager for `Rylascoo/Ensemble-Project`.

Engineer #1 owns:

- Engineering architecture interpretation and adoption;
- final patch-boundary reconciliation;
- shared Engineering authority files;
- central queue transitions;
- validation interpretation/promotion;
- integration order;
- protected merge decisions;
- exact-main validation;
- branch/archive lifecycle;
- cross-engineer conflict reconciliation.

Engineer #1's current production specialization is Product persistence/recovery and systems integration.

### Engineer #2 — Engineering Relay A / Application & Product Architecture

Engineer #2 is a persistent delegated Engineering deputy.

Engineer #2's primary production ownership is:

- `src/Kymaean.Application/**`;
- `tests/Kymaean.Application.Tests/**`.

Engineer #2 owns bounded Application/Product architecture work inside an explicit execution lease, including earned Product identity/lifecycle contracts, commands/queries, deterministic projections, and Presentation-facing Application state.

Engineer #2 does not own Persistence implementation, WinUI/platform implementation, provider code, visual design, shared authority state, queue transitions, validation promotion, or final adoption.

### Engineer #3 — Engineering Relay B / Windows Runtime & Composition

Engineer #3 is a persistent delegated Engineering deputy.

Engineer #3's primary production ownership is:

- `src/Kymaean.Windows/**`;
- Windows/platform-specific tests explicitly assigned by lease.

Engineer #3 owns bounded native Windows composition work inside an explicit execution lease, including composition root, Application/Infrastructure construction, packaged storage/bootstrap, activation/relaunch/recovery plumbing, strict MVVM seams, and Windows lifecycle integration.

Engineer #3 does not own Application semantics, Persistence internals/file formats, provider semantics, visual design, shared authority state, queue transitions, validation promotion, or final adoption.

## Authority model

Engineer #2 and Engineer #3 are full-capability implementation deputies but are not co-equal Engineering authorities.

They may inspect, reason, design within lease scope, implement, test, create isolated worktrees/branches, publish exact interface checkpoints, open PRs when their lease permits, and return evidence.

They may not independently:

- edit `CURRENT_STATE.md`;
- transition `docs/PROJECT_EXECUTION_QUEUE.md`;
- promote `docs/VALIDATION_LEDGER.md`;
- adopt architecture as Project authority;
- merge their own Product changes to `main`;
- authorize provider traffic/spend;
- consume deferred-E0 namespaces;
- redefine Design authority;
- select unrelated next backlog work after lease completion.

Engineer #1 reconciles returned work before any authority-state change.

## One mutation lease per Engineer

Each Engineer may hold at most one active production mutation lease at a time.

Read-only audits/questions may coexist, but an Engineer may not accumulate multiple unfinished implementation branches.

Engineer #1's standing manager duties — reconciliation, protected integration, authority continuity, validation interpretation and branch retirement — do not constitute a second production mutation lease. Engineer #1 must pause/reconcile its own implementation work as needed so those manager actions remain serialized and cannot race its own source mutation.

Every mutation lease must identify:

```text
LEASE ID:
ENGINEER:
QUEUE ITEM:
BASE EXACT MAIN:
BRANCH / WORKTREE:
OBJECTIVE:
OWNED WRITE SURFACE:
READ-ONLY DEPENDENCIES:
PROHIBITED SURFACES:
REQUIRED VALIDATION:
DEPENDENCIES / CONSUMERS:
STOP CONDITIONS:
RETURN TARGET:
```

The branch namespace should normally be:

```text
engineer-01/<lane>/<task>
engineer-02/<lane>/<task>
engineer-03/<lane>/<task>
```

## Repository-visible progress

GitHub Issues are the operational transport for active leases, checkpoints, interface requests, and returned evidence.

They do not become a second backlog and do not replace `docs/PROJECT_EXECUTION_QUEUE.md`.

A lease Issue may carry these operational states:

```text
LEASED
IN_PROGRESS
BLOCKED_ON_INTERFACE
INTERFACE_READY
VALIDATING
RETURNED
CLOSED
```

The opening lease Issue records `LEASED`. After fresh authority recovery and isolated branch/worktree creation, the Engineer must post an `IN_PROGRESS` Issue comment before the first production mutation. Later material transitions are posted as Issue comments; the latest valid exact-ref transition comment is the operational status. The Issue remains transport, never backlog or authority.

An Engineer may autonomously move through the pre-authorized phases of its current lease when objective prerequisites are satisfied.

Before publishing `INTERFACE_READY`, entering `VALIDATING`, or declaring `RETURNED`, the Engineer fresh-resolves relevant live refs and race-checks the lease baseline. Material baseline movement requires reconciliation/reconstruction before proceeding.

An Engineer may not autonomously select a new unrelated backlog item after `RETURNED`. A new mutation objective requires a new or explicitly amended lease.

## Interface checkpoint law

Parallel work may depend on intermediate contracts without waiting for an entire lane to finish.

When an owned interface becomes stable enough for dependent work, the owning Engineer may publish an `INTERFACE_READY` checkpoint containing:

- exact branch and commit SHA;
- exact files/contracts provided;
- validation already completed;
- known non-authority/limitations;
- named dependent Engineers allowed to consume it.

A consumer may base work on that exact checkpoint only when its own lease permits the dependency.

The consumer must not edit the provider's owned files.

If a consumer discovers a missing foreign-lane contract, it records `BLOCKED_ON_INTERFACE` and a precise interface request. The owning Engineer decides/implements the foreign-lane change.

## Write ownership

No two Engineers write the same file concurrently without an explicit ownership transfer and merge order.

Shared Engineering authority surfaces remain Engineer #1-only during active parallel production, including:

- `CURRENT_STATE.md`;
- `docs/PROJECT_EXECUTION_QUEUE.md`;
- `docs/VALIDATION_LEDGER.md`;
- cross-lane architecture/adoption law;
- final integration/closeout records.

A semantic conflict in another Engineer's owned surface is returned to that Engineer. Engineer #1 may resolve only mechanically trivial integration conflicts that do not change meaning.

## Integration

Development may proceed three-wide. Authority integration remains serialized.

Default dependency order when relevant:

```text
Engineer #2 Application contract
    -> Engineer #1 Persistence implementation and/or Engineer #3 Windows consumption
    -> Engineer #1 reconciliation
    -> hosted exact-head validation
    -> protected merge
    -> exact-main validation
    -> authority reconciliation
    -> branch archive/retirement
```

Independent Persistence work may integrate without waiting for Application/Windows work when no dependency exists.

Dependent branches must reconstruct/rebase against the newly validated `main` before promotion when their baseline becomes materially stale.

## Design-unblocking milestone

Engineer #2's first major program milestone is `DESIGN_ARCHITECTURE_READY`.

That milestone means the Application/Product contracts required for Design Sol to continue app design are stable enough that Design is no longer guessing about runtime Product state.

It does not require complete P1, complete Windows runtime wiring, provider integration, final architecture freeze, or release authority.

Engineer #1 decides whether the returned evidence satisfies the milestone and records the authoritative result.

Design Sol consumes only the integrated, exact-main-validated architecture handoff. A provisional Engineer #2 branch or `INTERFACE_READY` checkpoint may unblock Engineers #1/#3 but does not by itself reopen Design authority.

## Director interaction / engineering handoff reporting

Every substantive Engineer #1/#2/#3 reply must end with a compact routing footer so the Director never has to infer which chat to continue.

The footer reports, at minimum:

- Engineer identity and current work unit;
- user-facing work-unit status: `COMPLETE`, `CONTINUE`, or `BLOCKED`;
- repository lease status using the canonical lease states above;
- `RECOMMENDED NEXT CLICK`: Engineer #1, Engineer #2, Engineer #3, Design Sol, or the current chat;
- one-line dependency reason;
- lanes safe to continue in parallel;
- lanes that should not continue yet;
- exactly what the current Engineer will do if continued;
- Engineering -> UI/Design disposition;
- real/authoritative versus provisional/local-only state.

The routing footer is reporting law, not new authority. It never permits a Relay to acquire a second mutation lease, self-integrate, select an unrelated backlog item after `RETURNED`, or consume a foreign lane.

When validation is still running at the end of a reply, the Engineer must say that no work progresses between Director messages and identify which chat should be continued to inspect the result. Other explicitly independent lanes may still be reported as safe in parallel.

Design coordination is bidirectional. Engineering supplies integrated runtime/Product facts; Design Sol supplies visual, interaction and accessibility requirements. Existing Design work authorized by the Design repository may proceed independently. New runtime-dependent Design meaning may rely only on integrated exact-main Engineering truth, never a provisional Relay branch or `INTERFACE_READY` checkpoint.

## Native-subagent and automation boundary

This amendment governs three persistent ChatGPT Engineering contexts only.

It does not alter:

- Q-ADMIN-03 native-subagent admission;
- the current native-Codex concurrency ceilings;
- Hooks/Automations blocking;
- provider authorization;
- deferred-E0 hold;
- validation hierarchy.

No new autonomous background task pickup is authorized.

## Activation boundary

At adoption of this amendment:

- Engineer #1 is active;
- Engineer #2 is prepared but not yet activated;
- Engineer #3 is prepared but not yet activated.

The Director will start the two additional chats only after this operating law and its workstream charter are durably integrated and exact-main validation passes.
