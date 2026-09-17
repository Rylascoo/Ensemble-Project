# Q-PROD-01 Persistence Journal — Windows ARM64 Validation

Date: 2026-09-17

Status: **PROVISIONAL PERSISTENCE SUBSTRATE IMPLEMENTED / NATIVE ARM64 TEST PASS / NOT COMPLETE P1 OR FINAL ARCHITECTURE**

## Authority and exact identity

Director build-ahead authority remains `docs/KYMAEAN_PRODUCT_BUILD_AHEAD_OF_DEFERRED_E0_VALIDATION_DIRECTOR_AMENDMENT_2026_09_17.md`.

Exact executable source checkpoint: `fff8298a066531cdfbd2bcb7bdacdbc7d024bcca`, based on Project `main@6d2a32e174e2f00d16f91d04d6ad82adab2326fa`.

Annotated validation tag: `validation/q-prod-01-persistence-journal-native-arm64`.

Implementation adds `Kymaean.Infrastructure.Persistence` and deterministic tests without adding a Core/Application dependency on filesystem, provider, UI, Windows AI, or deferred-E0 implementation detail. The existing `IProductionStore` contract is unchanged and this journal is not yet wired as the final Production store.

## Earned implementation behavior

`FileProductionJournal` provides a schema-neutral local append-only record substrate with:

- monotonically ordered records with SHA-256 payload and chained record hashes;
- versioned binary record framing and deterministic filename identity;
- write-through + flush-to-disk pending files before same-directory publish;
- a versioned, checksummed journal-head witness for the latest committed record;
- fail-closed detection of payload corruption, chain gaps, renamed records, corrupt head bytes, and missing committed tail records;
- explicit recovery that removes stale pending files and can promote a fully validated post-crash suffix when entry publication completed before head publication.

## Native validation

Validation ran on SurfSeven, native Windows ARM64, with repository-pinned .NET SDK `9.0.317`.

- `Kymaean.Infrastructure.Persistence.Tests`: **9/9 PASS** on `win-arm64` Release.
- Existing `Kymaean.Application.Tests`: **6/6 PASS** on `win-arm64` Release.
- `Kymaean.Windows` ARM64 Release build: **PASS**, 0 warnings / 0 errors.
- repository law: **PASS**, 10 classified projects.
- document authority census: **330 inventory / 209 current / 30 historical / 91 archive / 0 unexplained current**.
- oracle assertion coverage unchanged: **279 documented / 17 asserted / 262 document-only**.

The recursive audit initially falsified the first prototype because an entry hash chain alone could not detect deletion of the final record. The validated checkpoint adds the checksummed head witness and tests final-tail loss through both normal read and recovery paths. No known correctness defect remained after the final recursive pass.

## Non-authority and remaining P1 work

This checkpoint does **not** establish the final Production event/domain schema, causal-event serialization contract, projection replay/rebuild integration, snapshot policy, portable export format, credential handling, storage-location policy, concurrency policy, full process-crash injection coverage, power-loss certification, WACK/Store behavior, or complete P1 persistence/recovery.

The file publish/head-witness mechanism is provisional infrastructure behind replaceable seams. It must not be described as complete durable Production persistence until Application/domain events, replay/rebuild, recovery semantics, export/version migration, and target-device close/reopen/crash evidence are integrated and validated.

No Gemini/provider traffic occurred. Deferred E0 namespaces, claims, credentials, and provider associations were untouched.

## Exact next action

Integrate this source through hosted exact-head gates and push-triggered exact-main Validation. Continue P1 by defining the smallest Product/Application persistence event boundary and deterministic replay/rebuild contract without freezing unresolved deferred-E0 ontology, while consuming any returned Q-DESIGN-20 implementation guidance independently.
