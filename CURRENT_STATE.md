# Ensemble Current State

Updated: 2026-09-02

## Authority
- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- H1 Patch 0002.2 Missing Raft Fixture Contract Blueprint 0.4 is canonical for the Missing Raft fixture subset.
- H1 Patch 0003 ECJ-1 Canonical Fixture Identity Blueprint 0.2 is APPROVED and canonical.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

H1 Patch 0003 is COMPLETE, machine-validated for its exercised gates, and promoted to `main` through PR #8.

## Validated executable baseline
The validated executable baseline now includes H1 Patch 0002.1, Patch 0002.1a, Patch 0002.2, and Patch 0003.

Latest machine-tested executable implementation head:
`c55eb022983a3954a78c8386a8c83ce8d5f4f2a7`

Patch 0003 promotion head on `main` before this checkpoint update:
`3006f1294b60ff6ec764d0b66097bfd12d7ad8da`

The commits after the tested executable head are documentation-only validation/approval closure. They do not increase the executable validation level.

Validated on the user's native Windows ARM64 machine:
- Native Windows ARM64 compiler gate: PASS — Core and Harness built successfully; Harness output targeted `net9.0\win-arm64`.
- Core tests: PASS — 73 total, 73 succeeded, 0 failed, 0 skipped.
- Canonical Missing Raft runtime with ECJ-1/SHA-256 enforcement: PASS — `ensemble.e0.missing-raft@0.1.0`; exit `0`.
- Generic smoke runtime regression: PASS — `ensemble.e0.smoke@0.1.0`; exit `0`.
- Independent ECJ-1 digest review: PASS advisory.
- Final static/hygiene/scope review: PASS advisory.

Detailed evidence:
`docs/evidence/H1_PATCH_0003_ARM64_VALIDATION.md`

## Patch 0003 implementation result
Canonical specification:
`docs/blueprint/H1_PATCH_0003_ECJ1_FIXTURE_HASH.md`

Implemented architecture:
1. `ValidatedFixture` remains the single successful fixture domain representation;
2. `Ecj1FixtureCanonicalizer` emits explicit deterministic semantic JSON rather than hashing source bytes or serializer/reflection output;
3. ECJ-1 output is UTF-8/no BOM/minified with frozen property order and exact deterministic string escaping;
4. chronology preserves semantic sequence while roster/record/relationship/provenance collections canonicalize ordinally by stable ID;
5. semantic text fails closed on NUL, carriage return, invalid surrogate sequences, or non-NFC input rather than being repaired;
6. `FixtureHash.Compute` defines SHA-256 over ECJ-1 bytes and exposes exactly 64 lowercase hexadecimal characters;
7. Missing Raft 0.1.0 freezes expected digest `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703` inside `MissingRaftContract`;
8. `MissingRaftContract.Validate(ValidatedFixture)` remains the single complete structural/authority/provenance + hash gate;
9. generic unknown fixture families remain generically valid and may be deterministically hashed without acquiring Missing Raft authority;
10. no second fixture representation, canonical source copy, mutable hash sidecar, registry/plugin framework, compatibility mode, or hash/access conflation was introduced.

## ECJ-1 frozen reference
Independent reference serialization paths over the canonical Missing Raft source agreed on:
- ECJ-1 canonical UTF-8 byte length: `9112`;
- SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

The passing Core suite freezes both values so byte-contract drift fails closed.

The canonical Missing Raft source JSON itself was not changed by Patch 0003.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine: test-execution authority for tests actually exercised.
- Actual target-device execution: runtime authority for behavior actually exercised.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Explicitly unvalidated / excluded
Patch 0003 does not establish or implement:
- deterministic Access Control;
- Context Composer or ContextPacketHash;
- StateHash;
- ProductionState construction or persistence;
- accepted-history / causal commits;
- Performer, Director, Integrity Validator, or State Interpreter runtime orchestration;
- provider or AI behavior;
- Windows AI / NPU execution;
- WinUI;
- packaging;
- WACK;
- Microsoft Store certification.

SHA-256 fixture identity is not authorization, secrecy, encryption, signing, or publisher-authenticity evidence.

No NPU, WACK, Store, or broader product-runtime claim may be inferred from Patch 0003 validation.

## Immediate next action
Hold at the validated H1 Patch 0003 boundary.

Do not begin Patch 0004 deterministic Access Control as Patch 0003 cleanup or correction.

Before entering the next H1 slice, read this checkpoint first and then locate/read the relevant canonical H1 roadmap/specification for Patch 0004. If GitHub does not yet contain a complete implementation-level Access Control contract, checkpoint and explicitly approve that blueprint before executable work begins rather than reconstructing authority from chat memory.

No further Patch 0003 implementation work is currently required.

## Continuity
Fresh chats read this file first.

For Patch 0003 history, then read:
- `docs/evidence/H1_PATCH_0003_ARM64_VALIDATION.md`;
- `docs/blueprint/H1_PATCH_0003_ECJ1_FIXTURE_HASH.md`.

For Missing Raft structural/semantic authority, read:
- `docs/blueprint/H1_PATCH_0002_2_MISSING_RAFT_CONTRACT.md`.

`docs/handoff/E0A_H1_PATCH_0002_2_IMPLEMENTATION_HANDOFF.md` is historical and must not be treated as a current implementation instruction.

Do not reconstruct already-approved project state from chat history when GitHub contains it.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
