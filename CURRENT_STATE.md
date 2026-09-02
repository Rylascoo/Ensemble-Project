# Ensemble Current State

Updated: 2026-09-02

## Authority
- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- H1 Patch 0002.2 Missing Raft Fixture Contract Blueprint 0.4 is canonical for the Missing Raft fixture subset.
- H1 Patch 0003 ECJ-1 Canonical Fixture Identity Blueprint 0.2 is explicitly approved and canonical for fixture canonicalization/hash implementation.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

H1 Patch 0002.2 is COMPLETE, machine-validated for its exercised gates, and promoted to `main` through PR #6.

H1 Patch 0003 blueprint-only specification is approved and promoted through PR #7. Executable Patch 0003 implementation has not yet been machine-validated.

## Validated executable baseline
Latest machine-tested executable implementation head:
`33a8a9f96d2320260619ac28309f0f40bee8e19e`

Validated on the user's native Windows ARM64 machine:
- Core/Harness build: PASS; Harness targeted `net9.0\win-arm64`.
- Core tests: PASS — 62/62.
- canonical Missing Raft runtime: PASS; exit `0`.
- generic smoke runtime regression: PASS; exit `0`.
- source-content and final hygiene/scope review: PASS.

Detailed evidence:
`docs/evidence/H1_PATCH_0002_2_ARM64_VALIDATION.md`

Documentation/blueprint commits after the tested executable head do not increase the executable validation level.

## Current Patch 0003 specification
Canonical specification:
`docs/blueprint/H1_PATCH_0003_ECJ1_FIXTURE_HASH.md`

Approved law includes:
1. canonicalize semantic `ValidatedFixture` content, not literal source bytes;
2. ECJ-1 emits deterministic UTF-8/no-BOM/minified JSON with explicit property order and exact string escaping;
3. chronology remains ordered while semantically unordered roster/record/relationship/provenance collections sort ordinally by canonical ID;
4. semantic text must be NFC and fail closed on NUL, carriage return, or invalid Unicode surrogate sequences rather than being repaired;
5. `FixtureHash = SHA-256(ECJ1(ValidatedFixture))`, represented as exactly 64 lowercase hexadecimal characters;
6. the Missing Raft 0.1.0 expected digest is frozen in `MissingRaftContract`, not an authoritative sidecar;
7. `MissingRaftContract.Validate(ValidatedFixture)` remains the single complete structural + hash gate;
8. canonical bytes must receive an independent digest-freeze review before the expected digest is frozen;
9. generic unknown fixture families remain generically valid;
10. Patch 0004 Access Control and later H1 surfaces remain excluded.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine: test-execution authority for tests actually exercised.
- Actual target-device execution: runtime authority for behavior actually exercised.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Explicitly unvalidated / excluded
No Patch 0003 executable validation exists yet. No deterministic Access Control, Context Composer, ContextPacketHash, StateHash, Production state/persistence, causal commits, provider/AI behavior, Windows AI/NPU execution, WinUI, packaging, WACK, or Store validation exists yet.

## Immediate next action
Implement H1 Patch 0003 only from current `main` on a dedicated branch, following `docs/blueprint/H1_PATCH_0003_ECJ1_FIXTURE_HASH.md`.

Implementation order:
1. extend canonical text validation for carriage return and invalid surrogate sequences;
2. add the smallest explicit ECJ-1 `ValidatedFixture` canonicalizer;
3. add SHA-256 fixture hash computation;
4. independently review canonical Missing Raft bytes and derive/freeze its expected digest;
5. bind the expected digest into `MissingRaftContract.Validate`;
6. add focused determinism/order/string/hash mutation tests;
7. run static/adversarial/hygiene review;
8. request native Windows ARM64 build, full tests, Missing Raft runtime, generic smoke regression, and semantic-mutation rejection evidence;
9. merge only after all gates pass.

Do not enter Patch 0004 Access Control during Patch 0003 implementation/correction.

## Continuity
Fresh chats read this file first, then `docs/blueprint/H1_PATCH_0003_ECJ1_FIXTURE_HASH.md`, then only source/test files relevant to the immediate Patch 0003 slice.

`docs/handoff/E0A_H1_PATCH_0002_2_IMPLEMENTATION_HANDOFF.md` is historical and must not be treated as a current implementation instruction.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
