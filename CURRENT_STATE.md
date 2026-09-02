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

## Last fully machine-validated baseline
H1 Patch 0001 + Patch 0001a on `main`:
- native Windows ARM64 Core/Harness build succeeded under .NET SDK 9.0.317;
- Core regression tests passed 2/2;
- native Harness process-start/runtime-guard sanity passed with expected exit code `2`.

## Active implementation boundary
Branch: `h1-patch-0002-1-fixture-dialect`

H1 Patch 0002.1 implements E0 Fixture Dialect v1 only:
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

## Clean replacement completed on this branch
The Patch 0001 bootstrap fixture surfaces are not preserved as compatibility layers:
- `FixtureEnvelopeDocument` deleted;
- `FixtureEnvelopeValidator` deleted;
- fixture-authored `accessPolicies` removed from the transport schema;
- `FixtureLoader` now returns `E0FixtureDocument` directly;
- Harness promotes only through `GenericE0FixtureValidator` to `ValidatedFixture`.

## Explicit exclusions
Patch 0002.1 does not include Missing Raft-specific law/content, ECJ-1, SHA-256, Access Control implementation, Context Composer, Production state construction, persistence, causal commits, Performer/Director/Interpreter, provider/AI work, WinUI, Windows AI, NPU/QNN, packaging, WACK, or Store work.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine is test-execution authority for the tests actually exercised.
- Actual target-device execution: runtime authority for the behavior actually exercised.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Current validation state
- Patch 0001 + 0001a remain the last fully machine-validated baseline.
- Patch 0002.1 static review is complete with no known blocker.
- On 2026-09-01, the target Windows ARM64 machine successfully ran the Patch 0002.1 Harness against `fixtures\smoke\e0-fixture-v1.json`; output was `Fixture validated: ensemble.e0.smoke@0.1.0` and process exit code was `0`.
- This establishes runtime validation only for the generic fixture load/validation path actually exercised.
- Patch 0002.1 compiler and expanded test-suite evidence have not yet been supplied for this gate, so the patch is not yet fully machine-validated and PR #3 remains unmerged.
- No Missing Raft semantic validation, Access Control, Context Composer, persistence, provider behavior, Windows AI/NPU execution, packaging, WACK, or Store validation exists yet.

## Immediate next action
Obtain Patch 0002.1 ARM64 build and Core test output for the current implementation. If both pass, perform the final post-evidence hygiene review and merge PR #3; otherwise patch only evidence-backed failures.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
