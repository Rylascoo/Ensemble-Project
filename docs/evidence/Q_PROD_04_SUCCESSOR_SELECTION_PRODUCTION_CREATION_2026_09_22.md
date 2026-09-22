# Q-PROD-04 successor-selection audit — Production creation boundary

Date: 2026-09-22  
Status: **READ-ONLY PRODUCT RECONCILIATION / DIRECTOR DECISION REQUIRED / NO IMPLEMENTATION AUTHORITY**  
Exact baseline: `Rylascoo/Ensemble-Project@eda037344747e6873b63031d3b95c7aa82cb84e3`  
Exact-main Validation: #1156 PASS

## Question

After Q-PROD-03 / Q-DESIGN-21 closure, what is the smallest application-development successor that advances the frozen program without inventing unearned Product meaning or consuming provider/deferred-E0 gates?

## Evidence

The active roadmap's runtime-complete Alpha flow begins:

`Create/open Production -> establish Cast/Scene -> ...`

Current integrated Product code already supports:

- opaque stable `ProductionId`;
- complete list / known-ID open;
- explicit recover;
- contextual Product/Application scope;
- creator whole-state World-current replacement;
- lossless creation-event persistence, replay, snapshots and portable **export**.

It does **not** support catalog-level Production creation. `ProductionApplication.Create(name)` is a lower-level event coordinator over an already-created store; `IProductionCatalog` exposes only list/open/recover and the current file catalog has no entry-creation API. The current `ProductApplication` snapshots the catalog list at startup and therefore cannot admit a newly created Production without an explicit successor contract.

Current evidence also explicitly leaves unearned:

- catalog create / rename / delete;
- import / restore;
- Scene / Character / Pressure / Take / Rehearsal product semantics;
- provider / Performer behavior;
- final Studio / Stage / Archive ontology.

FIRSTUSE remains `EVIDENCE_PENDING_ADOPTION` and explicitly authorizes no creation/import command. Home A/B remains unresolved. Neither Design question is necessary to define the Product-side creation contract.

## Successor selection

The strongest non-provider, non-deferred-E0 successor is a **Create Production Product contract only**.

Why:

1. It is directly required by the roadmap's minimum Alpha flow.
2. It closes a real Product/Application gap rather than adding speculative ontology.
3. Existing persistence already has the lossless `ProductionCreatedEvent` payload/replay semantics needed underneath it.
4. It can be isolated from rename/delete/import/restore, FIRSTUSE, Home, Character/Scene ontology and provider work.
5. Once the Product contract is integrated, App Design can lawfully decide where/how creation appears, including whether pending FIRSTUSE evidence should be adopted.

No implementation package should open until the Director resolves the remaining Product choices below.

## Product decisions that remain genuinely open

### D1 — ProductionId issuance

Current `ProductionId` accepts any nonblank exact string and is intentionally opaque to UI. Persistence separates authoritative identity metadata from a bounded filesystem locator.

Choose the creation authority:

- **A — catalog-generated opaque identity:** the catalog/creation port generates a new stable identity; creator UI supplies only creator-facing creation inputs.
- **B — Application-supplied opaque identity:** Application generates/passes the identity to the catalog.

Either choice must preserve the existing full `ProductionId` domain for previously stored Productions and must not expose filesystem locator semantics to Application/UI.

### D2 — successful creation result / Application state

Choose whether successful creation:

- **A — creates and returns authoritative identity/replay, without implicitly entering Current Production**; caller may then explicitly open it.
- **B — creates and atomically makes the new Production the current open Production in the ProductApplication projection.**

This is Product behavior, not merely navigation polish.

### D3 — duplicate creator-facing names

Identity is already distinct from `ProductionName`; no existing contract establishes display-name uniqueness.

Choose:

- **A — duplicate Production names are allowed; stable identity disambiguates them.**
- **B — creator-facing Production names must be unique within a catalog.**

If B, uniqueness comparison rules must also be specified; they cannot be inferred from UI convention.

## Strong inherited constraints — no new decision required

Unless the Director explicitly changes them:

- creation name uses the existing exact nonblank `ProductionCreatedEvent` domain; no trim, case-fold or Unicode normalization;
- creation-only replay starts with `WorldCurrentState.Empty`;
- new writes use the existing lossless creation v2 encoding;
- creation must not leave a discoverable partial Production after a failed operation;
- storage locator remains separate from authoritative identity;
- environmental I/O/access remains environmental rather than Product Invalid/Incompatible;
- no automatic retry/recovery/rollback is inferred;
- no Character, Scene, provider or Stage semantics are created by Production creation.

## Deliberately excluded from Q-PROD-04

- rename;
- delete;
- import / restore;
- portable-export UI;
- FIRSTUSE or Home selection;
- Character/Circumstance/Scene/Pressure/Take/Rehearsal;
- provider/Performer;
- Stage;
- deferred E0;
- final architecture / release.

## Next gate

Director chooses D1, D2 and D3. After that, Q-PROD-04 can be opened as one bounded Application + Persistence creation package with native ARM64 validation. App Design/Windows creation UI remains a later consumer unless the adopted Product result explicitly makes it safe to combine.
