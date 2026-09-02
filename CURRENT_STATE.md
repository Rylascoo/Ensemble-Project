# Ensemble Current State

Updated: 2026-09-01

## Authority
- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law for implementation quality and cleanup unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository for UI/UX architecture, mockups/prototypes, visual identity/artwork, motion/animation, Store/marketing assets, and research/reference material.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

## Active implementation boundary
H1 Patch 0002.1 — E0 Fixture Dialect v1.
Branch: `h1-patch-0002-1-fixture-dialect`
PR: #3 `H1: add E0 Fixture Dialect v1`

Patch 0002.1 includes:
1. typed `E0FixtureDocument` transport schema;
2. distinct fixture family identity, stable `major.minor.patch` fixture version, and derived versioned `FixtureId`;
3. structurally separate Production-authoritative and Character-subjective record categories;
4. Character ownership encoded by nesting rather than redundant owner fields;
5. known access/observation contract identifiers with no fixture-authored ACLs;
6. immutable `ValidatedFixture` promotion boundary;
7. generic deterministic validation for exactly three Characters, Scene roster equality, initial opportunity, fixture-global Record IDs, chronology references, provenance integrity, and relationship targets;
8. E0 JSON preflight bound to 1 MiB with duplicate-property rejection and JSON Number tokens forbidden;
9. Unicode NFC/NUL semantic-text validation without silent normalization;
10. one canonical non-Missing-Raft smoke fixture shared by tests and Harness runtime validation;
11. existing Core test project extended for strong IDs, fixture versions, dialect loading, authority/reference invariants, and parser/preflight failures.

## Clean replacement
The Patch 0001 bootstrap fixture surfaces were removed instead of preserved as compatibility layers:
- `FixtureEnvelopeDocument` deleted;
- `FixtureEnvelopeValidator` deleted;
- fixture-authored `accessPolicies` removed;
- `FixtureLoader` now returns `E0FixtureDocument` directly;
- Harness promotes only through `GenericE0FixtureValidator` to `ValidatedFixture`.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine is test-execution authority for the tests actually exercised.
- Actual target-device execution: runtime authority for the behavior actually exercised.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Patch 0002.1 validation
Tested implementation head: `f0147b09`.

- Static review: PASS.
- Native Windows ARM64 compiler gate: PASS — Core and Harness built successfully; Harness output targeted `net9.0\win-arm64`.
- Core tests: PASS — 30 total, 30 succeeded, 0 failed, 0 skipped.
- Generic fixture runtime smoke: PASS — `ensemble.e0.smoke@0.1.0` validated and process exit code was `0`.
- Final post-evidence hygiene/scope comparison: PASS — branch is a pure fast-forward from `main`; obsolete envelope surfaces are deleted; no compatibility adapter, Missing Raft-specific law, hashing, Access Control, provider, AI, UI, NPU, or Store surface entered the patch.
- Evidence-only/checkpoint commits after tested head `f0147b09` do not alter executable source.
- Detailed evidence: `docs/evidence/H1_PATCH_0002_1_ARM64_VALIDATION.md`.

## Explicitly unvalidated / excluded
No Missing Raft semantic validation, ECJ-1, SHA-256 fixture hashing, deterministic Access Control, Context Composer, Production state construction, persistence, causal commits, provider/AI behavior, Windows AI/NPU execution, WinUI, packaging, WACK, or Store validation exists yet.

## Immediate next action
Promote Patch 0002.1 to `main` as the new validated baseline, then design Patch 0002.2: canonical Missing Raft fixture plus experiment-specific structural validation. Do not add ECJ-1/hash or Access Control until Patch 0002.2 is separately reviewed and machine-validated.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
