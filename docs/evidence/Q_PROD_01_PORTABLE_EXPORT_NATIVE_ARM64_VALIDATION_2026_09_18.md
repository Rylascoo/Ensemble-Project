# Q-PROD-01 Portable Credential-Independent Production Export — Native ARM64 Validation

Date: 2026-09-18

Status: **INTEGRATED PORTABLE EXPORT LAW / NATIVE SOURCE AUTHORITY PRESERVED**

## Identity

- Lease: `ENG1-QPROD01-EXPORT-04` / Issue #213. Duplicate transport Issue #212 is closed as duplicate.
- Exact validated base: `040f6b66410fd1d5076fd99188b624564682a09a`; push-triggered Validation #1034 PASS.
- Final exact native source: `ae6a871a9338ae02f63193267a6e596a4ebfefc9`.
- Validation tag: `validation/q-prod-01-portable-export-native-arm64`.
- Integrated by PR #214 to `main@adaeb47c366e60e3b00b92ab517a5068f8b1a426`.
- Exact-SHA hosted Validation #1038 PASS.
- PR exact-head Validation #1039 PASS.
- Push-triggered exact-main Validation #1040 PASS.

## Exact source scope

Exactly six Persistence-owned source/test paths changed:

- `src/Kymaean.Infrastructure.Persistence/FileProductionCatalog.cs`
- `src/Kymaean.Infrastructure.Persistence/FileProductionEventStore.cs`
- `src/Kymaean.Infrastructure.Persistence/FileProductionExporter.cs`
- `src/Kymaean.Infrastructure.Persistence/ProductionPersistenceVersionPolicy.cs`
- `src/Kymaean.Infrastructure.Persistence/ProductionPortableExport.cs`
- `tests/Kymaean.Infrastructure.Persistence.Tests/ProductionPortableExportTests.cs`

No Application, Windows, provider, deferred-E0, Design, workflow, tool or shared-authority source changed in the exact native source commit.

## Earned portable-export contract

- Export is Persistence-owned; `IProductionCatalog` and Application Product semantics remain unchanged.
- Export begins only after exact Product identity metadata and committed journal history validate successfully.
- Committed journal/event history remains the sole causal Product authority.
- Export preserves the exact validated raw journal event payload bytes in original order; event payloads are not semantically re-encoded for portability.
- The full `ProductionId.Value` string domain is preserved as UTF-16 code units, including path-hostile values and lone surrogate code units already supported by the Application identity contract.
- The portable package has its own format family, explicit version field and SHA-256 checksum.
- The package is independent of the live catalog locator, LocalState path, journal/head filenames, lock files, snapshots, pending files or installation-local coordinates.
- Rebuildable projection snapshot state is not causal package authority and is not required for export correctness.
- Provider credentials, API keys, tokens, provider-local files and secret-store references are not part of current Persistence Product state and are not exported.
- Causal Product state is not mutated, recovered or repaired implicitly during export: identity metadata, committed journal history/head and snapshot state remain authoritative and unchanged. `FileProductionJournal.ReadAll()` may create a missing `.journal.lock` coordination artifact via `FileMode.OpenOrCreate`; this is not causal Product state and must not be overstated as blanket source-filesystem immutability.
- Unknown Product, corrupt journal or replay-invalid history returns Product `Invalid`; unsupported identity/event contract returns `Incompatible`.
- Destination filesystem `IOException` / `UnauthorizedAccessException` remain environmental failures and are not coerced into Product `Invalid` or `Incompatible`.
- Durable file export uses a pending artifact, flush-to-disk and final move; failed destination finalization does not intentionally leave the requested final artifact as a successful package.
- Durable export rejects a destination inside the live application root.
- A read-only inspector validates package checksum, version and event/replay semantics without creating or replacing Product state.
- Import/restore semantics are explicitly unearned.

## Exact-source native Windows ARM64 validation

At exact source `ae6a871...` on SurfSeven:

- Kymaean.Infrastructure.Persistence tests: **80/80 PASS**.
- Kymaean.Application regression: **38/38 PASS**.
- Kymaean.Windows Release `win-arm64`: **PASS, 0 warnings / 0 errors**.
- repository law: PASS.
- oracle guard: **288 documented / 17 asserted / 271 document-only**.
- document tree unchanged from the pre-freeze census: **344 inventory / 223 current / 30 historical / 91 archive / 0 unexplained**.
- hosted exact-SHA and integrated-main census checks: PASS.
- diff hygiene, clean worktree, owned-scope check and unchanged-main race: PASS.

Targeted export evidence includes:

- deterministic identical export for the same validated source;
- exact identity preservation including lone-surrogate coverage;
- exact raw event-payload preservation;
- catalog locator and local-only artifact exclusion;
- snapshot/provider-local marker exclusion;
- causal source-state immutability (identity metadata, committed history/head and snapshot state), with the explicit `.journal.lock` coordination-file caveat above;
- no implicit recovery when committed head is missing;
- unknown Product -> Invalid without source creation;
- corrupt committed journal -> Invalid;
- future event contract -> Incompatible;
- future identity metadata -> Incompatible;
- replay-invalid history -> Invalid;
- inspector accepts a valid package;
- inspector rejects checksum corruption;
- inspector rejects unsupported package version;
- inspector rejects truncated package;
- durable file export finalizes outside the source root;
- blocked destination leaves no pending export artifact;
- destination inside the source root is rejected.

## Hosted force-push baseline reconciliation

The first final-source push run, Validation #1037, failed only because its push-event `BASE_SHA` was the superseded pre-force-push draft `5f5cce4816239a88aac1420987e9bdc43bfe7178`. That rewritten-away commit was unavailable in the runner checkout, so Oracle baseline resolution failed with `fatal: Needed a single revision`.

No source or repository-law weakening followed. The exact same final source `ae6a871...` was pushed to a fresh temporary validation branch, where current main was the valid baseline. Validation #1038 PASS, including Oracle, repository law, census, Persistence, Application, Core and compiler/WinUI gates. The temporary branch was deleted before PR promotion.

Validation #1037 is therefore diagnostic evidence only, not promotion authority.

## Milestone and non-authority

`DESIGN_ARCHITECTURE_READY = NOT READY`. Persisted Production-internal content beyond current `ProductionName` remains unearned.

This checkpoint does not establish Product import/restore, create/rename/delete lifecycle, richer Studio/Stage/Archive ontology, final corruption/power-loss/concurrency certification, provider behavior, final architecture, WACK/Store authority, or deferred-E0 conclusions.

Per the active Q-PROD-01 charter, Engineer #1's next Persistence rung is **broader corruption/interruption evidence**, followed by **final provisional storage/concurrency policy**.

No provider traffic occurred and no deferred-E0 namespace was consumed.
