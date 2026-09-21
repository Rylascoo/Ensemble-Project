# Q-PROD-01 lossless UTF-16 Persistence contract

Status: Director/Product decision, 2026-09-21. Scope: World-current-truth Persistence consumer and the confirmed creation-event sibling representation defect.

## Decision and provenance

The Director reviewed the mandatory string-domain stop and explicitly retained the current Application domain: exact .NET strings, including isolated UTF-16 surrogate code units accepted by `WorldCurrentTruth` and `ProductionCreatedEvent`. Persistence must adapt; Application validation must not narrow, normalize, trim, case-fold, replace or convert to a scalar-value domain.

The original WORLD-CURRENT-TRUTH PERSISTENCE CONSUMER dispatch and the follow-up DIRECTOR / PRODUCT CONTRACT RESOLUTION in the task on 2026-09-21 authorize this bounded implementation. The original stop remains valid evidence: `Utf8JsonWriter.WriteString` replaced accepted D800/DC00 with FFFD on native .NET 9.0.19; hand-escaped unpaired surrogate JSON could not be retrieved through `JsonElement.GetString`. The sibling creation constructor has the same accepted domain and direct-string mismatch.

The follow-up explicitly authorizes a narrow Persistence-only creation correction, preserving existing v1 decode and using a new version for new lossless writes. Information already replaced by FFFD in an old payload is unrecoverable and must not be guessed.

## Event contracts

`Utf16CodeUnits` manually writes each char as UInt16 big-endian, without a text encoder, then JSON carries those bytes as Base64. Decode requires valid Base64 and even byte length and reconstructs chars directly. No BOM or native-endian dependency exists. Application constructors retain ordering, duplicate detection and value validation.

| Contract | Exact properties | Behavior |
|---|---|---|
| `kymaean.production.created.v1` | `contract`, `productionName` | Existing direct JSON-string decode retained; never rewritten or interpreted as code-unit Base64 |
| `kymaean.production.created.v2` | `contract`, `productionNameUtf16Be` | All new creation writes; name is Base64 of explicit big-endian code units |
| `kymaean.production.creator-replaced-world-current-state.v1` | `contract`, `truthsUtf16Be` | Whole replacement; ordered array of Base64 code-unit strings; empty array allowed |

Missing, duplicate, unexpected or malformed properties/data fail closed. Unrecognized versions within either known family produce compatibility failure; unrelated unknown contracts remain invalid. Old valid creation histories replay with empty World state. Journal schema remains 1 and portable export version remains 1.

## Writer and concurrency

`FileProductionCatalog` implements `IProductionWorldStateWriter`. It resolves an existing identity without creating storage, opens the existing journal, appends exactly one replacement through validated append, and reloads authoritative committed history. Unknown identity is Invalid; there is no automatic recovery.

Compatibility maps to Incompatible; known corruption, malformed data and replay-invalid history map to Invalid. Unrelated environmental I/O/access remains exceptional. Optional cache failure cannot invalidate successful replay.

The policy in `docs/evidence/Q_PROD_01_FINAL_PROVISIONAL_STORAGE_CONCURRENCY_POLICY_NATIVE_ARM64_2026_09_18.md` is unchanged. Append and subsequent replay are separately gated operations, not a new transaction. Another permitted operation may intervene; same-root overlapping access fails fast with environmental sharing I/O. A post-commit read failure may therefore leave an appended event without a successful call result. No retry, catalog-global lock, rollback or cross-Production atomicity is promised. A successful result is derived from committed replay.

## Snapshot and export

Snapshot version 2 retains the existing family header, journal anchor and exact length-prefixed name; it appends a UInt32 big-endian truth count and, for each truth, a UInt32 code-unit count followed by UInt16 big-endian code units. The SHA-256 checksum covers all projection content. A v1/unknown/stale/malformed cache is ignored and rebuilt after authoritative history succeeds. Invalid Application values in a checksummed cache are also cache misses. Cache is never causal authority.

Portable export continues to copy raw committed event payloads. The inspector decodes the recognized event contracts and replays them. No import/restore semantics or local layout, credentials or snapshot internals are added to the package.

## Limits

Application source is unchanged. No UI, provider, richer Product semantics, lifecycle/create/rename/delete, deferred E0, final architecture, Alpha, release, WACK or Store authority is granted. `DESIGN_ARCHITECTURE_READY = NOT READY` until consumer integration and explicit reassessment under `docs/NATIVE_CODEX_APPLICATION_WORKFLOW.md`.

Validation and candidate disposition: `docs/evidence/Q_PROD_01_LOSSLESS_PERSISTENCE_CONSUMER_NATIVE_ARM64_VALIDATION_2026_09_21.md`.
