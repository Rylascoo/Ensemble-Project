# Q-PROD-01 Rebuildable Production Projection Snapshot — Native ARM64 Validation

Date: 2026-09-18

Status: **INTEGRATED REBUILDABLE SNAPSHOT LAW / NATIVE SOURCE AUTHORITY PRESERVED**

## Identity

- Lease: `ENG1-QPROD01-SNAPSHOT-03` / Issue #203.
- Exact validated base: `7918b92dbfca983ffc92967eb3bd6a24016c9493`; push-triggered Validation #1026 PASS.
- Final exact native source: `aaca35069ca68a1a28d0bd189e6a0eb2e7ad8724`.
- Final validation tag: `validation/q-prod-01-persistence-snapshot-native-arm64-r2`.
- Integrated by PR #210 to `main@6df408fe43cb714ad00d63c97f916174beffb0ab`.
- Branch-push Validation #1029 PASS; PR exact-head Validation #1030 PASS; push-triggered exact-main Validation #1031 PASS.
- R1 source `201bb3a204ef97fc93486ad431abeffd9fedbf23` had the same source tree but a two-commit PR shape; its merge-ref repository-law check failed only because CURRENT_STATE currency would become four commits at the synthetic merge. R2 squashed the identical tree to one commit and passed.

## Exact source scope

Exactly five Persistence-owned source/test paths changed:

- `src/Kymaean.Infrastructure.Persistence/FileProductionCatalog.cs`
- `src/Kymaean.Infrastructure.Persistence/FileProductionEventStore.cs`
- `src/Kymaean.Infrastructure.Persistence/ProductionPersistenceVersionPolicy.cs`
- `src/Kymaean.Infrastructure.Persistence/ProductionProjectionSnapshotCache.cs`
- `tests/Kymaean.Infrastructure.Persistence.Tests/ProductionProjectionSnapshotTests.cs`

No Application, Windows, provider, deferred-E0, Design, workflow, tool or shared-authority source changed in the exact native source commit.

## Earned snapshot contract

- Append-only committed journal/event history remains the sole Product source of truth.
- Snapshot format/version/checksum/layout are Persistence-owned and invisible to Application and Windows.
- Snapshot content is limited to the currently earned `ProductionReplayProjection` (`ProductionName`) plus Persistence-owned version and journal-anchor metadata.
- A snapshot is bound to the exact validated final committed journal sequence and SHA-256 record hash.
- Missing, stale-anchor, malformed, checksum-invalid, unsupported-version or wrong-projection snapshot state never changes Product classification; authoritative journal validation/replay wins and the cache is rebuilt best-effort.
- A plausible snapshot cannot mask corrupt journal framing/hash state, an incompatible event contract, or replay-invalid event history.
- Explicit journal recovery refreshes snapshot state only after recovered authoritative history passes decode + replay validation.
- Snapshot writes use a pending file plus flush-to-disk and replace/move; orphan pending snapshot files are discarded before rebuild.
- `IOException` / `UnauthorizedAccessException` during optional cache maintenance are best-effort and cannot retroactively invalidate a durable Product result.
- Unexpected cache invariant/programming failures remain exceptional and are outside the catalog Product-failure translation boundary; they are not relabeled as Product `Invalid`.
- Snapshot state contains no credential material and introduces no Product identity, lifecycle or richer ontology authority.

## Exact-source native Windows ARM64 validation

At exact source `aaca350...` on SurfSeven:

- Kymaean.Infrastructure.Persistence tests: **65/65 PASS**.
- Kymaean.Application regression: **38/38 PASS**.
- Kymaean.Windows Release `win-arm64`: **PASS, 0 warnings / 0 errors**.
- repository law: PASS.
- document census: **343 inventory / 222 current / 30 historical / 91 archive / 0 unexplained**.
- oracle guard: **288 documented / 17 asserted / 271 document-only**.
- diff hygiene, clean worktree and unchanged-main race checks: PASS.

Targeted snapshot evidence includes:

- absent snapshot rebuild;
- malformed snapshot fallback/rebuild;
- valid same-anchor/same-projection no-rewrite recognition;
- stale-anchor rejection;
- corrupt checksum fallback;
- unsupported snapshot-version fallback;
- same-anchor wrong projection cannot override journal truth;
- corrupt journal cannot be masked;
- replay-invalid journal cannot be masked;
- incompatible journal cannot be masked;
- explicit recovery refresh;
- snapshot-write failure does not invalidate durable Product;
- interrupted pending-snapshot cleanup.

## Performance / acceleration non-authority

This checkpoint **does not earn read-path acceleration**.

Current catalog access still:

1. validates the authoritative committed journal chain;
2. decodes every committed event payload;
3. runs deterministic `ProductionReplay.Rebuild`;
4. only then reconciles optional snapshot state.

Therefore the snapshot is presently a rebuildable persisted projection mirror and cache-law proof. It does not reduce authoritative journal I/O, event decode, or replay work. Claiming faster Product reads would be false at this checkpoint.

Actual replay acceleration would require a separately earned Persistence design that can preserve all journal integrity, compatibility and replay-validity obligations without letting optional cache state become authority.

## Integration and branch-shape correction

The first hosted candidate used the same final source tree across two commits. Branch-push repository law passed, but the PR synthetic merge increased `CURRENT_STATE.md` distance from three to four commits and correctly failed the max-three currency law.

Engineer #1 did not weaken repository law or mutate shared authority to bypass that guard. The exact same source tree was rebuilt as final one-commit source `aaca350...`, native validation was rerun, and R2 hosted validation passed.

Protected expected-head merge produced `main@6df408fe43cb714ad00d63c97f916174beffb0ab`; push-triggered Validation #1031 PASS, all eight jobs.

## Milestone and non-authority

`DESIGN_ARCHITECTURE_READY = NOT READY`. Persisted Production-internal content beyond current `ProductionName` remains unearned.

This checkpoint does not establish portable export, final concurrency/power-loss certification, Product create/rename/delete semantics, richer Studio/Stage/Archive ontology, provider behavior, final architecture, WACK/Store authority, or deferred-E0 conclusions.

Per the active three-engineer charter, Engineer #1's next Persistence sequence is **portable credential-independent export**, followed by broader corruption/interruption evidence and final provisional storage/concurrency policy.

No provider traffic occurred and no deferred-E0 namespace was consumed.
