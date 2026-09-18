# Q-PROD-01 Persistence Version Policy — Native ARM64 Validation

Date: 2026-09-17

Status: **TARGET-DEVICE NATIVE VALIDATION / INTEGRATION PENDING**

## Identity

- Lease: `ENG1-QPROD01-PERSIST-01`.
- Base: Project `main@09f172d8fe50964f1d5c39f1ddf0f0bddb94d8ab`, push-triggered Validation #974 PASS.
- Exact source: `f6a9f0f6b3abc6d8a34debfcfdc9dac0474e0147`.
- Validation tag: `validation/q-prod-01-persistence-version-policy-native-arm64`, annotated and peeled to the exact source.
- Host: SurfSeven, native Windows ARM64.

## Policy earned

The current Product persistence format now has an explicit compatibility boundary without adding migration or broader Product ontology.

- Journal entry/head file-family magic remains stable: `KYMJRN01` / `KYMJHD01`.
- The uint schema field is the authoritative physical journal/head schema version.
- The writer emits journal/head schema **v1**.
- This build reads physical schema **v1 only**.
- `kymaean.production.created.v1` remains the independent Product-event contract version.
- A known future `ProductionCreated` contract such as `.v2` fails with a typed `ProductionPersistenceCompatibilityException`.
- An integrity-valid unsupported journal/head schema fails with that same explicit compatibility category.
- A corrupted version byte remains corruption: entry record integrity / filename or head checksum is validated before schema compatibility is classified.
- Unknown unrelated event contracts remain invalid data rather than being guessed into a version family.
- Open/recovery does not migrate, rewrite or promote unsupported storage implicitly.

The compatibility exception exposes the affected artifact plus found/supported identifiers. It does not leak into Application source and does not create Windows/provider behavior.

## Recursive-audit correction

An earlier candidate checked the schema field before validating record/head integrity. That could have mislabeled a bit-flipped version byte as a future compatible format rather than corruption.

The final source reverses that ordering: stable envelope -> integrity proof -> compatibility check. Dedicated native tests cover both integrity-valid future versions and corrupted version bytes.

## Native validation

At exact source `f6a9f0f6b3abc6d8a34debfcfdc9dac0474e0147`:

- Persistence tests: **30/30 PASS** on `win-arm64`.
- Application regression: **13/13 PASS** on `win-arm64`.
- Kymaean.Windows ARM64 Release build: **PASS, 0 warnings / 0 errors**.
- Repository law: **PASS**.
- Document census: **338 inventory / 217 current / 30 historical / 91 archive / 0 unexplained current**.
- Oracle guard: **288 documented hashes / 17 asserted / 271 document-only**.
- Diff hygiene: **PASS**.
- Exact source scope: five Persistence-owned source/test files; no Application, Windows, provider, E0 or shared-authority mutation.

## Non-authority

This checkpoint does **not** establish:

- a migration engine or implicit migration policy;
- a Production manifest/catalog or persisted `ProductionId` layout;
- snapshots or snapshot rebuild policy;
- portable export/import;
- rename/delete semantics;
- final storage/concurrency policy;
- device-reboot/suspend-resume or arbitrary power-loss certification;
- broader Product event ontology;
- complete P1, Alpha/Beta/release, WACK or Store authority.

No provider traffic occurred and no deferred-E0 namespace was consumed.

## Parallel-lane boundary

Engineer #2 has provisionally published Application checkpoint
`engineer-02/q-prod-01/apparch-01@27f156552c5dc1d8dec75e9d138a36146f0016ce`
with an `IProductionCatalog` interface. Engineer #3 is correctly blocked on a Persistence implementation of that seam.

That catalog adapter is **not part of this validated source**. It is a likely successor Engineer #1 lease only after the Engineer #2 Application checkpoint is reconciled/integrated into exact-main authority.

## Next

Integrate this schema/version-policy checkpoint without widening its source identity. Then reconcile Engineer #2's Application architecture. If its catalog contract survives integration, open a new bounded Engineer #1 Persistence lease for the concrete catalog adapter needed by Engineer #3.
