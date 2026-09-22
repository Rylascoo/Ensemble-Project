# Q-PROD-04 Production creation Director decision

Date: 2026-09-22  
Status: **DIRECTOR APPROVED / PRODUCT CONTRACT INPUT FROZEN / IMPLEMENTATION READY**

Exact predecessor audit:
`docs/evidence/Q_PROD_04_SUCCESSOR_SELECTION_PRODUCTION_CREATION_2026_09_22.md`

Director disposition: **A / A / A**.

## D1 — ProductionId issuance

**A — catalog-generated opaque identity.**

The creator supplies no Product identity and no filesystem locator. The catalog/creation boundary issues a new stable opaque `ProductionId`. Previously stored Productions retain the full existing `ProductionId` domain; this decision does not reinterpret or narrow historical identity values.

Persistence locator remains technical and non-authoritative. UI/Application must not infer path/layout semantics from the generated identity.

## D2 — successful creation result / Application state

**A — create without implicit open.**

Successful creation returns the authoritative new Production identity and creation-only replay and updates the ProductApplication's known Production catalog projection.

Creation does **not**:

- enter `ApplicationScope.CurrentProduction`;
- choose or change `ProductSpace`;
- replace an already-open Production;
- navigate the UI.

Opening remains a separate explicit Product operation.

## D3 — duplicate creator-facing names

**A — duplicate Production names are allowed.**

`ProductionName` is creator-facing display text, not identity. Stable `ProductionId` disambiguates Productions. No catalog-wide display-name uniqueness or comparison/normalization rule is created.

## Inherited creation law

- creation name uses the existing exact nonblank `ProductionCreatedEvent` domain;
- no silent trim, case-fold, Unicode normalization or punctuation rewrite;
- creation-only replay starts with `WorldCurrentState.Empty`;
- new persistence writes use the existing lossless creation v2 event encoding;
- creation must not expose a discoverable **partial** Production if construction fails;
- technical locator remains separate from authoritative identity;
- environmental I/O/access failures remain exceptional, not relabeled Invalid/Incompatible;
- no automatic retry/recovery/rollback contract is created;
- no Character, Scene, provider, Stage, FIRSTUSE, Home, rename/delete/import/restore semantics are created.

## Q-PROD-04 implementation boundary

Implement exactly:

1. Application-owned creation capability contract and authoritative creation result;
2. ProductApplication creation orchestration that updates its known list while preserving current scope/open Production/ProductSpace;
3. Persistence catalog creation using catalog-issued opaque identity and fully formed entry publication;
4. native Application/Persistence regression coverage.

Stop on any need to define rename/delete/import/restore, creation UI/FIRSTUSE/Home, richer ontology, provider behavior, or new recovery semantics.
