# H1 Patch 0003 — ECJ-1 Canonical Fixture Identity

Status: blueprint proposal 0.1 — APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0002.2
Branch: `h1-patch-0003-ecj1-blueprint`

## 1. Purpose

Replace Patch 0002.2's review-only semantic immutability boundary with deterministic canonical semantic bytes and SHA-256 identity for the frozen Missing Raft fixture.

Patch 0003 answers one narrow question:

> Can any semantically meaningful change to the validated Missing Raft fixture be detected deterministically while irrelevant source formatting and semantically unordered collection order do not change fixture identity?

The patch does not create Production persistence, Access Control, Context Composer, or runtime orchestration.

## 2. Authority and continuity

The following law is already established by the approved H1 deterministic-spine work and frozen E0 requirements:

- E0 records fixture version plus immutable fixture hash/identity.
- Ensemble Canonical JSON v1 (`ECJ-1`) uses UTF-8 with no BOM and no insignificant whitespace.
- canonical object-property order is explicit rather than serializer/reflection dependent;
- semantically unordered collections canonicalize by stable ID using ordinal ordering;
- semantically ordered sequences retain their defined order;
- strings must already satisfy canonical text requirements; canonicalization rejects nonconforming input rather than silently normalizing it;
- ECJ-1 permits JSON strings, integers, booleans, null, arrays, and objects, but not floating-point values;
- duplicate JSON properties remain rejected before deserialization;
- the pipeline remains `raw JSON -> strict preflight -> transport DTO -> generic semantic validation -> immutable ValidatedFixture -> fixture-specific validation -> ECJ-1 bytes -> SHA-256 identity`;
- `FixtureHash = SHA-256(ECJ-1 canonical fixture UTF-8 bytes)`.

GitHub currently contains forward references to this law but not a standalone Patch 0003 implementation specification. This proposal checkpoints and concretizes the missing implementation-level contract before code is written.

## 3. Architecture boundary

Preserve the validated Patch 0002.2 representation:

`ValidatedFixture` remains the only successful fixture domain representation.

Do not add:
- a second canonical-fixture DTO;
- a hash-specific fixture model;
- reflection-based canonical serialization;
- generic canonicalization registries/plugins;
- sidecar copies of the Missing Raft fixture;
- hash-derived access authority;
- filesystem state inside deterministic Core hashing logic.

Canonicalization consumes the already immutable `ValidatedFixture`. It does not canonicalize the raw JSON syntax tree and does not make source whitespace or source object-property order part of semantic identity.

## 4. ECJ-1 byte contract

For Patch 0003 fixture identity, ECJ-1 output is:

- UTF-8;
- no BOM;
- one JSON value only;
- no indentation;
- no spaces or line breaks outside JSON string values;
- explicit property emission order defined below;
- deterministic JSON string escaping;
- no floating-point output.

Canonicalization is pure: identical validated semantic input must produce byte-for-byte identical output without filesystem, clock, culture, randomness, network, or process-global state.

### 4.1 String discipline

All semantic text entering `ValidatedFixture` must already be Unicode NFC. Patch 0003 extends the existing fail-closed text boundary so semantic text containing carriage return (`U+000D`) is rejected rather than normalized to line feed. Line feed (`U+000A`) may remain meaningful text when a later fixture needs it.

ECJ-1 never silently Unicode-normalizes or line-ending-normalizes strings during serialization.

IDs continue to use their existing canonical ID validation.

### 4.2 JSON string encoding

ECJ-1 emits non-control Unicode scalar values directly as UTF-8 rather than optional `\u` escapes. JSON syntax characters and control characters are escaped deterministically. The implementation must use one explicitly configured canonical writer path; serializer defaults, reflection order, dictionary enumeration order, and locale-sensitive formatting are forbidden as authority.

If implementation review finds that a framework JSON writer cannot guarantee the byte contract without relying on unspecified behavior, implement the smallest dedicated ECJ-1 string writer rather than weakening the contract.

## 5. Canonical fixture property order

The root object emits properties exactly in this order:

1. `schemaVersion`
2. `fixture`
3. `accessContract`
4. `observationContract`
5. `chronology`
6. `historicalTruth`
7. `unresolvedPropositions`
8. `worldState`
9. `scene`
10. `sceneState`
11. `characters`
12. `pressures`
13. `initialOpportunity`

`fixture` emits:
1. `id`
2. `version`

`scene` emits:
1. `id`
2. `roster`

A normal fixture record emits:
1. `id`
2. `text`
3. `provenance`

A Character emits:
1. `id`
2. `displayName`
3. `constitution`
4. `disposition`
5. `circumstance`
6. `observations`
7. `knowledge`
8. `beliefs`
9. `suspicions`
10. `memories`
11. `goals`
12. `relationships`

A relationship emits:
1. `id`
2. `targetCharacterId`
3. `text`
4. `provenance`

These orders mirror the canonical E0 Fixture Dialect semantic structure and are versioned as ECJ-1 law. Changing them later without changing canonicalization version is forbidden.

## 6. Canonical array order

ECJ-1 distinguishes semantic sequence from semantic set.

Preserve order exactly:
- `chronology` — its order is experiment law.

Sort by canonical ID using `StringComparer.Ordinal` semantics:
- `scene.roster` by Character ID;
- `characters` by Character ID;
- every Production record collection by Record ID;
- every Character record collection by Record ID;
- every Character `relationships` collection by relationship Record ID;
- every record/relationship `provenance` collection by source Record ID.

Rationale:
- Patch 0002.1 already declares Scene roster order semantically unordered;
- Missing Raft's frozen collection contracts are exact sets except chronology;
- provenance expresses an edge set, not a narrative sequence;
- canonical sorting prevents irrelevant source array order from creating a different fixture identity.

No in-memory `ValidatedFixture` collection is mutated to obtain this ordering. Sorting occurs only in canonical emission.

## 7. Fixture hash contract

Define fixture identity as:

`FixtureHash = SHA-256(ECJ1(ValidatedFixture))`

The digest is represented externally as exactly 64 lowercase ASCII hexadecimal characters with no prefix.

The hash covers the complete canonical semantic fixture representation described above, including:
- schema/family/version/contracts;
- Scene identity and roster;
- chronology;
- all Production-authoritative records and text;
- all Character identity/display text and owned records;
- relationship targets/text;
- all provenance edges;
- initial opportunity.

It does not hash:
- source JSON whitespace;
- source object-property order;
- source order of semantically unordered arrays;
- file path or filesystem metadata;
- Git commit identity;
- blueprint/documentation text;
- derived `MissingRaftContract.RelationshipContextRecordIds` metadata that is not itself fixture content.

Therefore a source-only reformat does not create a new fixture identity, while a semantic wording/ID/provenance/ownership/chronology change does.

## 8. Missing Raft binding

`MissingRaftContract` remains the one owner of frozen Missing Raft experiment metadata.

Patch 0003 adds exactly one expected immutable hash value for `ensemble.e0.missing-raft@0.1.0` to that contract. The value is derived from the approved canonical fixture through the implemented ECJ-1 algorithm and then frozen in the same implementation patch.

`MissingRaftContract.Validate(ValidatedFixture)` becomes the complete version-0.1.0 contract gate:
1. existing structural/authority/provenance validation;
2. ECJ-1 canonicalization;
3. SHA-256 computation;
4. constant-time-or-equivalent exact digest comparison against the frozen expected hash;
5. fail closed on mismatch.

No `.sha256` sidecar is authoritative. A mutable sidecar beside a mutable fixture would not independently bind the fixture. The expected Missing Raft digest lives with the contract law in Core.

Unknown generic fixture families remain generically valid. Patch 0003 does not require every generic fixture to have a frozen expected hash.

## 9. Generic canonicalization boundary

The canonical fixture serializer/hash computation should be reusable for any `ValidatedFixture`, because fixture identity is generic E0 infrastructure.

That does not justify a reflection serializer or generalized canonical-object framework.

Preferred shape:
- one small ECJ-1 fixture canonicalizer that emits the existing `ValidatedFixture` explicitly;
- one SHA-256 fixture-hash primitive over the returned canonical bytes;
- Missing Raft owns only its expected digest and fixture-specific enforcement.

Future ContextPacketHash/StateHash work may reuse ECJ-1's byte rules, but Patch 0003 must not pre-implement ContextPacket, ProductionState, causal commit, or persistence canonicalization.

## 10. Source versus semantic identity

Patch 0003 intentionally hashes semantic canonical content rather than the source file's literal bytes.

Consequences:
- reindentation does not change the hash;
- CRLF versus LF outside JSON strings does not change the hash;
- object-property reordering in source does not change the hash;
- reordering a semantically unordered roster/record/relationship/provenance array does not change the hash;
- changing any retained semantic string, ID, relationship target, collection membership, provenance edge, initial opportunity, or chronology sequence changes the hash or fails earlier validation.

The canonical source file remains the human-reviewed source of fixture content. ECJ-1 supplies cryptographic semantic identity, not a second source representation.

## 11. Tests

Add targeted tests without malformed-fixture copy sprawl.

Required coverage:
1. canonical Missing Raft fixture validates and matches its frozen expected hash;
2. repeated ECJ-1 serialization of the same `ValidatedFixture` is byte-identical;
3. source whitespace/property-order changes that preserve semantics produce the same hash;
4. Character/roster/record/relationship/provenance reordering where the collection is semantically unordered produces the same hash;
5. chronology order remains significant and continues to fail the Missing Raft contract when altered;
6. a semantic text mutation that still passes structural validation produces a different hash and is rejected by the complete Missing Raft contract;
7. a relationship text mutation is likewise rejected by hash binding;
8. a provenance-set source-order change preserves the hash, while adding/removing/changing a provenance edge fails structurally or changes identity as appropriate;
9. semantic text containing carriage return is rejected rather than normalized;
10. generic smoke remains valid through the generic Harness path;
11. explicit Missing Raft validation still rejects generic smoke;
12. existing duplicate-property, NFC, provenance-DAG, and all Patch 0002.2 regressions remain green.

Prefer mutation of the canonical fixture in memory for negative cases. Do not create alternate valid Missing Raft fixture files.

## 12. Harness behavior

Keep the Harness interface unchanged.

For the known Missing Raft family, its existing dispatch invokes `MissingRaftContract.Validate`, which after Patch 0003 also enforces the frozen semantic hash.

Successful output remains:

`Fixture validated: ensemble.e0.missing-raft@0.1.0`

No hash-printing CLI, migration command, or debug compatibility mode is required for the E0 patch.

Unknown generic fixture families remain on the generic path.

## 13. Security and authority properties

- SHA-256 supplies change detection/identity, not authorization or secrecy.
- Hash equality never grants Character access.
- Provenance remains evidence/derivation and never becomes access authority.
- The expected digest is not a signature and does not claim publisher authenticity.
- A hash mismatch is a deterministic fixture-contract failure, not a recoverable story event.
- No model participates in canonicalization, hashing, or hash verification.

## 14. ARM64 and battery suitability

Canonicalization and SHA-256 are deterministic CPU operations over a fixture bounded by the existing 1 MiB fixture limit. Patch 0003 does not route them to AI, GPU, or NPU hardware.

This is preferable for E0 because:
- the work is small and infrequent;
- deterministic CPU hashing avoids model/runtime dependencies;
- no persistent background work is introduced;
- the NPU remains reserved for workloads that actually benefit from inference acceleration later.

No NPU behavior is claimed or tested by this patch.

## 15. Explicit exclusions

Patch 0003 does not implement:
- Patch 0004 deterministic Access Control;
- Context Composer;
- ContextPacketHash;
- StateHash;
- ProductionState or persistence;
- accepted-history/causal-commit serialization;
- Performer/Director/Integrity Validator/State Interpreter orchestration;
- providers or AI;
- WinUI;
- Windows AI/NPU;
- packaging/WACK/Store work;
- signing, HMAC, encryption, or trust-chain infrastructure.

## 16. Exit gate

Before promotion:
1. this blueprint is explicitly approved as the canonical Patch 0003 specification;
2. implementation begins from the then-current `main` on a dedicated branch;
3. canonical Missing Raft source remains semantically unchanged from validated Patch 0002.2 unless an actual defect is separately approved;
4. ECJ-1 static/adversarial review passes;
5. expected Missing Raft fixture hash is frozen from the approved ECJ-1 bytes;
6. native Windows ARM64 Harness/Core build passes with warnings-as-errors;
7. full Core test suite passes;
8. canonical Missing Raft Harness validation passes with hash enforcement;
9. generic smoke Harness regression passes;
10. semantic mutation is demonstrably rejected by hash binding;
11. final hygiene comparison finds no second fixture representation, source-byte identity leak, reflection/serializer-order dependency, generic over-abstraction, access/hash conflation, or later-patch scope leakage.

## 17. Approval decision

Most of Patch 0003 follows already-approved H1 law. This proposal asks explicit approval for the implementation-level compatibility contract that GitHub did not previously preserve, especially:

- the exact ECJ-1 fixture property order in Section 5;
- the exact ordered-versus-unordered array treatment in Section 6;
- lowercase 64-character SHA-256 representation;
- semantic canonical bytes rather than literal source bytes;
- storing the expected Missing Raft digest in `MissingRaftContract` rather than an authoritative sidecar;
- extending canonical semantic text validation to reject carriage return rather than normalize line endings.

Once approved, these become canonical Patch 0003 law and implementation can proceed without reopening them during coding.
