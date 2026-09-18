# Q-PROD-01 Final Provisional Persistence Storage / Concurrency Policy — Native Windows ARM64 Evidence

Date: 2026-09-18

Status: **PROVISIONAL POLICY CANDIDATE — RUNTIME SOURCE UNCHANGED**

## Purpose

This record closes the final chartered Engineer #1 Persistence rung for Q-PROD-01 by stating the strongest storage, coordination, concurrency, and durability policy supported by the current source and native Windows ARM64 evidence.

It is intentionally provisional. It does not convert implementation techniques such as exclusive file sharing, write-through streams, flush-to-disk calls, pending files, or same-directory moves into guarantees that have not been demonstrated.

## Authority baseline

- Exact policy activation base: `main@8d47959ddadaa84db82ed5d3ceaeef9c42189f19`.
- Base push-triggered Validation: #1049 PASS.
- Prior durable Persistence checkpoints:
  - catalog/recovery adapter: `1b20d937892c3248a0f84d536870b789d865e851`;
  - rebuildable snapshot: `aaca35069ca68a1a28d0bd189e6a0eb2e7ad8724`;
  - portable export: `ae6a871a9338ae02f63193267a6e596a4ebfefc9`;
  - broader corruption/interruption evidence: `a77170e99daa614c89c7949baab1ad959ffe64bc`.
- Prior evidence closeout exact main: `8d47959ddadaa84db82ed5d3ceaeef9c42189f19`.

No production defect was exposed during this policy audit. Runtime source is therefore intentionally unchanged.

## 1. Storage ownership

### Windows composition boundary

The integrated Windows app supplies:

`Windows.Storage.ApplicationData.Current.LocalFolder.Path`

as one opaque app-private storage root to `FileProductionCatalog`.

That Windows path is composition detail. It is not Product identity, Application semantic authority, or a portable Persistence contract.

### Persistence-owned namespace

Below the supplied root, Persistence owns:

- subordinate `production-catalog`;
- opaque per-Production entry locators;
- exact identity metadata;
- journal entry/head/lock files;
- rebuildable projection snapshot files;
- pending/interruption artifacts.

Application code receives Product identities and projections, not storage locators or filenames.

Portable export is deliberately independent of this live layout.

## 2. Causal authority

The committed append-only journal/event history is the sole causal Product source of truth.

The following are subordinate:

- `.projection.snapshot`: optional rebuildable cache only;
- portable export: validated copy only, never live authority;
- `.journal.lock`: coordination state only;
- pending entry/head/snapshot/export files: non-causal interruption state.

Ordinary open remains non-recovering. Recovery remains explicit.

## 3. Journal coordination scope

`FileProductionJournal.Append`, `ReadAll`, and `Recover` each acquire:

- a `.journal.lock` file inside that Production journal root;
- `FileMode.OpenOrCreate`;
- `FileAccess.ReadWrite`;
- `FileShare.None`.

The gate is therefore **per Production journal root**, not catalog-global.

The current source contains no catalog-global transaction or lock spanning multiple Production roots.

### Consequence

Among callers that use the current journal implementation, only one gated journal operation can own a given Production root at a time.

This includes ordinary reads, appends, and recovery.

This is exclusive coordination, not an in-process queue, cross-process wait protocol, lease service, retry policy, or distributed lock.

## 4. Native Windows same-root contention behavior

A deterministic external two-process probe was built outside the repository against the exact current Persistence project.

Environment:

- host: SurfSeven;
- `dotnet` RID: `win-arm64`;
- `dotnet` host architecture: `arm64`;
- isolated temporary journal roots only;
- no user Product data;
- no provider traffic.

Process A held Production A's `.journal.lock` with the same exclusive sharing mode for 60 seconds.

While the lock remained live, process B observed:

| Operation | Result |
|---|---|
| `ReadAll(A)` | `System.IO.IOException`, HRESULT `0x80070020`, about 3 ms |
| `Append(A)` | `System.IO.IOException`, HRESULT `0x80070020`, about 2 ms |
| `Recover(A)` | `System.IO.IOException`, HRESULT `0x80070020`, about 2 ms |
| `ReadAll(B)` on a different journal root | PASS, one committed entry, about 13 ms |

The observed Windows error text reported that `.journal.lock` was in use by another process.

### Earned contention contract

Same-Production contention is **fail-fast environmental I/O**.

The Persistence layer does not currently:

- wait for the other owner;
- retry automatically;
- enqueue operations;
- translate the sharing violation into Product `Invalid` or `Incompatible`.

Therefore callers must avoid unsupported overlapping access or provide any desired serialization/retry policy above Persistence.

This checkpoint does not promise a particular millisecond threshold; the important observed behavior is immediate failure rather than wait/retry.

## 5. Supported provisional concurrency posture

The provisional shipping posture is:

> **One active gated Persistence journal operation per Production root at a time.**

This is stricter and more accurate than saying only one mutating writer is supported, because ordinary `ReadAll` also owns the exclusive journal gate.

There is no earned guarantee of transparent concurrent multi-process access to the same Production.

There is no earned guarantee that multiple threads/tasks in one process may overlap same-root journal operations without environmental sharing failures.

Separate Production journal roots use separate lock files and can be accessed independently, as confirmed by the native two-process probe. This independence does **not** create atomicity across Productions.

## 6. Catalog scope and cross-Production consistency

`FileProductionCatalog` has no catalog-global transaction.

Catalog listing:

1. enumerates the Persistence-owned catalog namespace;
2. resolves each Production identity;
3. loads each Production journal separately.

Therefore no single cross-Production serialization point or all-Productions-at-one-instant snapshot is earned.

Current Product lifecycle mutation such as catalog create/delete/rename is not an earned Application contract, so this policy does not define concurrent catalog lifecycle behavior.

For the supported stable-catalog posture, listing remains complete-or-fail as already earned. Environmental contention on an individual Production may escape as environmental I/O rather than being relabeled as Product corruption.

## 7. Read, write, and recovery interaction

The same per-root gate protects the journal portions of:

- committed-history reads;
- append candidate validation + append publication;
- explicit recovery.

A contender cannot enter these journal operations while another gate owner is active.

Recovery is not implicit. A failed ordinary open caused by contention is not permission to recover.

The coordination lock itself may be created by an ordinary read because `AcquireGate` uses `OpenOrCreate`. Therefore a logically non-mutating Product read may create coordination state on disk.

That does not change causal Product history.

## 8. Snapshot concurrency

Snapshot reconciliation occurs **after** authoritative journal load/replay has completed and after the journal gate has been released.

Snapshot maintenance therefore is not serialized by `.journal.lock`.

Snapshot writes use:

- unique pending snapshot filenames;
- `FileShare.None` on each pending file;
- `FileOptions.WriteThrough`;
- `Flush(flushToDisk: true)`;
- final move/replace into `.projection.snapshot`.

Expected snapshot `IOException` / `UnauthorizedAccessException` is swallowed because snapshot state is optional.

A race can therefore cause one process to overwrite another process's snapshot with an older or newer anchor after both have already validated authoritative history. This does not become causal Product authority: future access compares the snapshot anchor to the authoritative committed journal anchor and discards/rebuilds stale cache state.

Thus snapshot concurrency may waste work or transiently preserve stale cache content, but it must not change committed Product truth.

## 9. Portable export concurrency

Portable export first reads and validates authoritative journal history under the normal per-Production journal gate.

After that gated read completes, package encoding and destination-file finalization do not hold the Production journal gate.

Therefore an export represents the committed history observed during its validated read; a later append may occur before destination writing finishes.

The export is still a valid portable copy of the observed committed history, not a transaction that freezes the live Product until file output completes.

Destination finalization uses a unique pending file plus write-through/flush and final move. There is no global export-destination lock or multi-writer transaction. Callers must not assume coordinated concurrent writes to the same destination path.

Export destination I/O/access failures remain environmental and are not Product `Invalid`/`Incompatible`.

## 10. Environmental failure classification

The following remain infrastructure/environmental failures when they arise from the filesystem/platform rather than validated Product content:

- journal-lock sharing violation;
- access denial;
- unavailable storage;
- destination export I/O;
- other ordinary platform I/O not already recognized as a specific journal corruption or compatibility condition.

Persistence must not manufacture Product `Invalid` or `Incompatible` merely to totalize these environmental conditions.

Windows startup already has its own bounded infrastructure-startup failure path for startup-time `IOException` / `UnauthorizedAccessException`.

This policy does not redesign mid-session UI treatment of environmental failures.

## 11. Durability techniques and limits

Current causal journal writes use pending files and durable-write intent:

- unique pending artifact;
- `FileShare.None`;
- `FileOptions.WriteThrough`;
- `Flush(flushToDisk: true)`;
- final same-directory move;
- committed-head validation;
- explicit interruption cleanup/recovery.

Snapshot and portable-export finalization use analogous pending/write-through/flush/final-move techniques.

The broader corruption/interruption checkpoint proves fail-closed/all-or-nothing behavior for the tested torn and crash states.

### What is *not* certified

This evidence does **not** prove:

- that every Windows filesystem/storage device honors identical persistence ordering under sudden power loss;
- that controller or device write caches cannot reorder or lose acknowledged writes;
- directory-entry fsync semantics equivalent to a separately proven POSIX durability model;
- arbitrary crash timing across every instruction boundary;
- general hardware power-loss certification.

Accordingly, Q-PROD-01 earns a robust provisional crash/interruption design and tested fail-closed behavior, **not** universal power-loss durability certification.

## 12. Storage permissions / read-side effects

Because the journal gate uses `OpenOrCreate`, ordinary journal reads require the ability to open/create the coordination file for read/write access.

Successful Product reads may also perform best-effort snapshot cache maintenance after authoritative replay.

Therefore the current supported live store is not a strictly read-only filesystem contract, even when the Product operation is conceptually a read.

This is implementation/private Persistence behavior and must not leak into Product identity semantics.

## 13. Final provisional policy

For the current provisional Q-PROD-01 build:

1. Windows provides one opaque app-private LocalState root.
2. Persistence owns all live storage detail below `production-catalog`.
3. Each Production has an independent journal gate.
4. `ReadAll`, `Append`, and `Recover` serialize only while their per-root gate is held.
5. Same-root contention fails fast as environmental `IOException`; Persistence does not wait/retry.
6. No transparent concurrent same-Production multi-process access is supported.
7. No catalog-global transaction or cross-Production atomicity is supported.
8. Snapshot state is optional, best-effort, and may race outside the journal gate without gaining authority.
9. Portable export is a validated point-in-history copy, not a live transactional freeze.
10. Existing pending/write-through/flush/move techniques and corruption tests support the provisional durability design but do not constitute general power-loss certification.
11. Product `Invalid`/`Incompatible` remains reserved for earned Product-storage corruption/compatibility classes, not ordinary lock/access/platform contention.
12. Product lifecycle/import/restore and richer persisted Studio/Stage/Archive semantics remain outside this policy.

## Milestone effect

This policy does not change `DESIGN_ARCHITECTURE_READY`.

`DESIGN_ARCHITECTURE_READY = NOT READY` remains true because persisted Production-internal content beyond the currently earned `ProductionName` remains unearned.

## Q-PROD-01 Persistence sequence

If this policy candidate is integrated and exact-main validated without exposing a production defect, the chartered Engineer #1 Persistence sequence for Q-PROD-01 is complete:

- schema/version policy;
- rebuildable snapshot policy;
- portable credential-independent export;
- broader corruption/interruption evidence;
- final provisional storage/concurrency policy.

Any successor Engineer #1 work must be selected from fresh repository authority. This document does not auto-authorize a new Persistence implementation rung.

## Provider / deferred-E0 boundary

No provider traffic occurred.

No deferred-E0 namespace was consumed.

Provider/deferred-E0 authority remains unchanged.
