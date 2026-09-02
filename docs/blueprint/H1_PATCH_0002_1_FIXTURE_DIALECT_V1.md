# H1 Patch 0002.1 — E0 Fixture Dialect v1

Status: implementation boundary
Parent: H1 Deterministic Spine Blueprint 0.2

## Purpose
Prove that Ensemble can load an E0 fixture into typed, immutable, authority-aware data and deterministically reject structurally invalid experimental state without story-specific reasoning or AI.

## Pipeline
Raw UTF-8 bytes -> StrictJsonPreflight -> E0FixtureDocument (untrusted transport) -> GenericE0FixtureValidator -> ValidatedFixture.

`ValidatedFixture` is the only successful generic-fixture result. Patch 0002.1 does not construct Production state.

## Dialect identity
- schema version: `ensemble.e0.fixture.v1`
- access contract: `ensemble.e0.character-bounded.v1`
- observation contract: `ensemble.e0.copresent-trio.v1`
- fixture family ID and fixture version are separate source fields
- authoritative FixtureId is derived as `<family>@<version>`
- E0 v1 fixture versions use stable `major.minor.patch` semantic-version core syntax only

## Authority structure
Production-authoritative collections are structurally separate:
- HistoricalTruth
- UnresolvedProposition
- WorldState
- SceneState

Character-subjective collections are nested under their owning Character so ownership cannot disagree with a redundant owner field:
- Observation
- Knowledge
- Belief
- Suspicion
- Memory
- Goal
- Relationship
- Constitution
- Disposition
- Circumstance

Relationship source/owner is the containing Character; only the target Character is referenced.

Public E0 Pressure is root-level and available to the Scene roster under the later deterministic access layer.

The fixture cannot author arbitrary ACLs. It selects a known access contract; later Access Control derives access from authority category, structural ownership, roster, and that contract.

## Referential rules
Generic E0 validation requires:
- exactly three Characters
- exactly one Scene with a roster containing exactly those Characters
- initial opportunity references a roster Character
- Character IDs are unique
- Record IDs are unique across all fixture record categories
- relationship targets resolve and may not target the source Character
- chronology references HistoricalTruth records only and contains no duplicate references
- provenance references resolve to existing Record IDs and may not self-reference
- schema version, access contract, and observation contract are known

Patch 0002.1 validates graph integrity, not narrative meaning.

## Collection semantics
Semantic order:
- chronology

ID-keyed / semantically unordered:
- Characters
- all record collections
- Scene roster
- provenance reference sets

Patch 0003 ECJ-1 will canonicalize unordered collections; Patch 0002.1 does not hash.

## Raw JSON boundary
StrictJsonPreflight additionally enforces:
- non-empty input
- maximum 1 MiB fixture size for E0 harness safety
- root object
- maximum depth 64
- no comments
- no trailing commas or trailing content
- no duplicate object properties
- no JSON Number tokens in E0 Fixture Dialect v1

Source whitespace, indentation, line endings, and property order are not semantic.

Semantic text values must be Unicode NFC and contain no NUL character. They are rejected rather than silently normalized.

## Clean replacement rule
Patch 0001 bootstrap surfaces are replaced directly:
- delete `FixtureEnvelopeDocument`
- delete `FixtureEnvelopeValidator`
- remove fixture-authored `accessPolicies`
- update `FixtureLoader` to return `E0FixtureDocument`
- change FixtureId semantics to the derived versioned identity

No legacy adapter or alias is retained.

## Tests
Extend the single existing `Ensemble.E0.Core.Tests` project. Tests cover the valid generic fixture and one-invariant-at-a-time failures for dialect/schema, IDs, roster, references, chronology, provenance, access/observation contracts, JSON numbers, duplicate properties, text canonicality, size bounds, and strong identity behavior.

## Explicit exclusions
- Missing Raft-specific law/content
- ECJ-1
- SHA-256
- Access Control implementation
- Context Composer
- Production state
- persistence / causal commits
- Performer / Director / Interpreter
- providers / AI
- WinUI / Windows AI / NPU / Store work

## Exit gate
On the target Windows ARM64 machine:
1. Harness build succeeds under stable .NET 9.
2. Core tests succeed.
3. Harness successfully loads and validates the generic E0 v1 smoke fixture.
4. Final hygiene review finds no retained bootstrap compatibility layer or duplicate authority representation.
