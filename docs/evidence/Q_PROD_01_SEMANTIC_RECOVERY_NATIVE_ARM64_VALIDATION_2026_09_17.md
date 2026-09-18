# Q-PROD-01 Semantic-Aware Recovery — Native ARM64 Validation

Date: 2026-09-17

Status: **NATIVE-VALIDATED / INTEGRATION PENDING / PROVISIONAL P1 SLICE**

## Identity

- Project baseline: `77d7ab7a36a27f5c4aa8d86aaf1df9878440f496`.
- Exact native machine-tested source: `97720ffc937853b59110c184392b1f6f97f39420`.
- Validation tag: `validation/q-prod-01-semantic-recovery-native-arm64`.
- Host: SurfSeven, native Windows ARM64, repository .NET 9 baseline.

## Earned behavior

The typed Product event-store boundary now validates semantic history while the existing journal lock is held and before durable authority changes:

- `IProductionEventStore` exposes model-neutral `Recover()`.
- typed append decodes and deterministically replays the complete candidate history before writing a new journal record;
- typed recovery decodes and deterministically replays the available structurally valid history before `.journal.head` may advance;
- replay-invalid typed append leaves the existing journal unchanged;
- undecodable or replay-invalid crash suffixes leave the previous committed head unchanged;
- semantically valid crash suffixes may be promoted by recovery;
- empty storage remains a valid no-Production-yet state.
The raw journal's callback-bearing append/recovery hooks are internal implementation machinery. They do not create a new public persistence abstraction.

This closes the defect identified by the prior recursive audit: raw byte-level recovery no longer has to promote a Product history before typed semantic validation has accepted it.

The same audit also found and closed the related append hole: a duplicate `ProductionCreatedEvent` cannot become durable through the typed store when deterministic replay rejects that candidate history.

## Exact native validation

At exact source `97720ffc937853b59110c184392b1f6f97f39420`:

- `Kymaean.Application.Tests`: **10/10 PASS** on `win-arm64` Release.
- `Kymaean.Infrastructure.Persistence.Tests`: **20/20 PASS** on `win-arm64` Release.
- `Kymaean.Windows` ARM64 Release build: **PASS, 0 warnings / 0 errors**.
- repository law: **PASS**, 10 classified projects.
- document census: **333 / 212 / 30 / 91 / 0 unexplained current**.
- oracle: **284 documented / 17 asserted / 267 document-only**.
- `git diff --check origin/main...HEAD`: PASS.
- exact executable/test scope: four files.

The persistence tests specifically prove empty-store preservation, pre-mutation rejection of replay-invalid typed append, fail-closed load of invalid committed typed history, valid initial-head recovery, and no-head-promotion behavior for both undecodable and replay-invalid crash suffixes.

## Recursive audit
One pre-validation command used `--no-restore` for WinUI in the fresh worktree and failed with NETSDK1004 because `project.assets.json` did not yet exist. No source/compiler defect was reached. The authoritative exact-source validation reran the normal ARM64 build with restore and passed with 0 warnings / 0 errors.

The callback surface was reduced from public to `internal` before the exact source commit because it is implementation machinery for the typed file store, not an earned durable Product API.

No new Product ontology, provider identity, or E0 causal category entered Application.

## Non-authority

This checkpoint does not establish:

- a broader causal event ontology or creator ontology freeze;
- Production identity, Cast/Scene lifecycle, or accepted-performance/consequence schemas;
- snapshots or snapshot rebuild policy;
- portable export, import or migration policy;
- full close/reopen reconstruction across a real Production lifecycle;
- final storage/concurrency policy;
- power-loss certification or complete crash-consistency certification;
- final architecture, Alpha/Beta/release, WACK, Store, or deferred-E0 conclusions.

No Gemini/provider traffic occurred. No deferred-E0 namespace was consumed.

## Next

Integrate this exact candidate through hosted exact-head and exact-main validation. After durable integration, continue the smallest evidence-earned P1 capability without widening Product ontology prematurely.
