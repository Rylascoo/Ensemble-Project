# Q-PROD-01 Persistence Catalog / Recovery Adapter — Native ARM64 Validation

Date: 2026-09-18

Status: **INTEGRATED PERSISTENCE CATALOG/RECOVERY BOUNDARY / NATIVE SOURCE AUTHORITY PRESERVED**

## Identity

- Lease: `ENG1-QPROD01-PERSISTCAT-02` / Issue #197.
- Windows consumer request: `ENG3-REQ-PERSIST-01` / Issue #195.
- Exact validated base: `257fbc52946dad32b0376e825c8137262ae3c849`; push-triggered Validation #1005 PASS.
- Exact native source: `1b20d937892c3248a0f84d536870b789d865e851`.
- Validation tag: `validation/q-prod-01-persistence-catalog-recovery-adapter-native-arm64`.
- Archive tag: `archive/engineer-01/q-prod-01/persistcat-02`.
- Both peel to exact source `1b20d937...`.
- Integrated by PR #199 to `main@b14b579408bec23865efede9a92a66d4f3bd1cd8`.
- Hosted exact-head Validation #1007 PASS; push-triggered exact-main Validation #1008 PASS.

## Exact source scope

Exactly six Persistence-owned source/test files changed:

- `src/Kymaean.Infrastructure.Persistence/FileProductionCatalog.cs`
- `src/Kymaean.Infrastructure.Persistence/FileProductionEventStore.cs`
- `src/Kymaean.Infrastructure.Persistence/FileProductionJournal.cs`
- `src/Kymaean.Infrastructure.Persistence/ProductionIdentityMetadata.cs`
- `src/Kymaean.Infrastructure.Persistence/ProductionPersistenceVersionPolicy.cs`
- `tests/Kymaean.Infrastructure.Persistence.Tests/FileProductionCatalogTests.cs`

No Application, Windows, provider, Design, deferred-E0, workflow, tool, or shared-authority source changed in the native source commit.

## Earned Persistence contract

- Public `FileProductionCatalog` implements the integrated Application-owned `IProductionCatalog`.
- Caller supplies only an opaque app-private root; Persistence owns a subordinate `production-catalog` namespace.
- Production entry locators are bounded opaque technical names and are not authoritative Product identity.
- Exact `ProductionId.Value` is stored in versioned, checksummed binary metadata as explicit UTF-16 code units, preserving the current .NET string domain including long values, lone surrogates, path-hostile/NUL characters and case-distinct values.
- Identity metadata version incompatibility maps to Application `Incompatible`; malformed, missing, duplicate or inconsistent identity/persisted Product state maps to `Invalid`.
- Catalog listing is deterministic and complete-or-fail; malformed entries are never silently skipped.
- Summaries derive only from committed `FileProductionEventStore.LoadAll()` + deterministic `ProductionReplay`.
- Selected open never recovers implicitly.
- Selected recover is explicit and uses the existing validated `Recover()` path.
- Unknown/stale IDs are resolved before non-creating store/journal construction and do not create Product storage.
- Unexpected environmental/platform I/O remains exceptional rather than being coerced into the two Product failure kinds.

## Native Windows ARM64 validation

At exact source `1b20d937...` on SurfSeven:

- Kymaean.Infrastructure.Persistence tests: **52/52 PASS**.
- Kymaean.Application regression: **38/38 PASS**.
- Kymaean.Windows Release `win-arm64`: **PASS, 0 warnings / 0 errors**.
- repository law: PASS.
- document census: **341 inventory / 220 current / 30 historical / 91 archive / 0 unexplained**.
- oracle guard: **288 documented / 17 asserted / 271 document-only**.
- diff hygiene, clean worktree and unchanged-main race checks: PASS.

Engineer #3 independently revalidated the exact source with the same 52/52 Persistence, 38/38 Application and 0-warning/0-error Windows ARM64 results and accepted the public consumer seam.

## Blockers resolved during implementation

The initial single-component strict-UTF8/hex identity draft was rejected before commit because it could not represent the full integrated `ProductionId` domain: long strings exceeded Windows component limits and lone UTF-16 surrogates were not UTF-8 encodable. The accepted source separates bounded locator from authoritative identity metadata and therefore does not narrow Application identity semantics.

The initial draft also conflated the caller-supplied app-private root with the Production-entry namespace. The accepted source moves entries under a Persistence-owned subordinate catalog directory, so unrelated app-root siblings do not invalidate the Product catalog.

The exact Application bootstrap contract remains unchanged: complete-list bootstrap may fail `Invalid`/`Incompatible` without exposing a recovery target. Explicit recovery is truthful only for an already-known `ProductionId`; no partial catalog or `RECOVERY_REQUIRED` state was invented.

## Hosted integration and branch lifecycle

- PR #199 exact head `1b20d937...`: Validation #1007 PASS, all eight jobs.
- Protected merge produced `main@b14b579408bec23865efede9a92a66d4f3bd1cd8`.
- Push-triggered exact-main Validation #1008 PASS, all eight jobs.
- Lease #197 and interface request #195 are CLOSED.
- Former Engineer #1 implementation branch/worktree were retired only after durable integration.
- Archive tag preserves exact source.
- Engineer #3 lease `ENG3-QPROD01-WINCOMP-01` subsequently refreshed to clean exact validated `main@b14b5794...` and posted `IN_PROGRESS`.

## Milestone and non-authority

`DESIGN_ARCHITECTURE_READY = NOT READY`. The project now has a truthful typed Application access seam plus concrete Persistence catalog/recovery adapter, but authoritative persisted Production-internal Studio/Stage/Archive content beyond current ProductionName replay remains unearned.

This checkpoint does not establish Product create/rename/delete semantics, catalog-entry creation policy, migration, snapshots/export, final concurrency/power-loss certification, richer Product ontology, Windows composition/runtime proof, provider behavior, final architecture, Alpha/Beta/release authority, WACK/Store authority, or deferred-E0 conclusions.

No provider traffic occurred and no deferred-E0 namespace was consumed.
