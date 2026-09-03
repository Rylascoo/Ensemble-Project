# Ensemble Current State

Updated: 2026-09-02

## Authority
- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- H1 Patch 0002.2 Missing Raft Fixture Contract Blueprint 0.4 is canonical for the Missing Raft fixture subset.
- H1 Patch 0003 ECJ-1 Canonical Fixture Identity Blueprint 0.2 is APPROVED and canonical.
- H1 Patch 0004 Deterministic Character-Bounded Access Control Blueprint 0.2 is APPROVED and canonical.
- H1 Patch 0005 Deterministic Context Composer + Dual Context Identity Blueprint 0.3 is APPROVED and canonical.
- H1 Patch 0006 Performer Candidate Output Contract Proposal 1.7 is APPROVED and canonical.
- H1 Patch 0007 E0 Director Opportunity Contract Proposal 1.5 is APPROVED and canonical.
- H1 Patch 0008 E0 Integrity Validator Contract Proposal 1.1 is APPROVED and canonical.
- Patch 0006 Proposal 1.7, Patch 0007 Proposal 1.5, and Patch 0008 Proposal 1.1 each completed recursive adversarial audit with a full zero-material-change pass before approval.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless stronger frozen authority explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Open design guard
`docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md` is a STRONG DIRECTION / OPEN DESIGN GUARD, not a frozen final schema.

> Keep authority semantics precise; keep creative semantics open.

Current E0 categories such as Knowledge, Beliefs, Suspicions, Memories, Goals, Relationships, Pressures, and Circumstance remain authoritative for frozen E0 contracts. They must not be assumed to prove that the final creator-facing dramatic ontology is a closed set of engine enums. Revisit before freezing the broad State Interpreter mutation ontology or durable post-E0 Production/Studio ontology.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

Machine-validated deterministic patches:
- H1 Patch 0003 — COMPLETE;
- H1 Patch 0004 — COMPLETE;
- H1 Patch 0005 — COMPLETE;
- H1 Patch 0006 — COMPLETE;
- H1 Patch 0007 — COMPLETE for its exercised deterministic Director gates;
- H1 Patch 0008 — COMPLETE for its exercised deterministic Integrity gates.

## Patch 0008 canonical specification and approval
Canonical blueprint:
`docs/blueprint/H1_PATCH_0008_INTEGRITY_VALIDATOR_CONTRACT.md`

Exact recursively audited and user-approved proposal head:
`07ccf1516af7c5d5572cac5790650d97d758467b`

Approval PR #17 was squash-merged from that exact audited/approved head.

Canonical blueprint promotion commit on `main`:
`2e62f2b0ce10aaca18d20494844ef5f435b5a097`

Approval record:
`docs/evidence/H1_PATCH_0008_BLUEPRINT_APPROVAL.md`

Implementation approval checkpoint:
`34b3ffdae98df21fc938370e964a39626e7ead32`

## Patch 0008 machine validation
Exact machine-tested executable/test head:
`30a07d0aeee63db927eecd49391fb6271268dc4d`

On the user's native Windows ARM64 development machine at that exact head:

### ARM64 Harness/Core build
Command:
`dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug`

Observed:
- `Ensemble.E0.Core` PASS;
- `Ensemble.E0.Harness` PASS;
- Harness target `net9.0\win-arm64`;
- build PASS.

### Full Core tests
Command:
`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`

Observed:
- total `253`;
- succeeded `253`;
- failed `0`;
- skipped `0`.

Patch 0008 contributes 42 Integrity test executions over the machine-validated 211-test Patch 0007 baseline.

### Harness regressions
Missing Raft:
- `Fixture validated: ensemble.e0.missing-raft@0.1.0`;
- exit `0`.

Generic smoke:
- `Fixture validated: ensemble.e0.smoke@0.1.0`;
- exit `0`.

Detailed evidence:
`docs/evidence/H1_PATCH_0008_ARM64_VALIDATION.md`

Documentation-only closure commits after `30a07d0a...` do not increase executable validation authority.

## Patch 0008 implementation result
Machine-tested executable/test delta from approval checkpoint `34b3ffdae...` to `30a07d0a...` contains exactly three added files:

1. `src/Ensemble.E0.Core/Integrity/IntegrityModels.cs`;
2. `src/Ensemble.E0.Core/Integrity/DeterministicIntegrityValidator.cs`;
3. `tests/Ensemble.E0.Core.Tests/Integrity/DeterministicIntegrityValidatorTests.cs`.

No existing Access, Context, Performer, Director, fixture, Harness, project configuration, persistence, UI, Windows AI/NPU, packaging, or Store source changed in the machine-tested executable/test delta.

Implemented architecture:
1. `IntegrityCandidateInput.Bind(ContextPacket, CandidatePerformance)` is the sole rich Context+Candidate boundary;
2. Validator receives no Character/Performance prose;
3. versioned Candidate-content SHA-256 identity binds concern evidence to exact Candidate semantics without becoming attempt/Candidate/Take/commit identity;
4. deterministic Reject codes are exactly `SubjectContextMismatch` then `ContextPacketIdentityMismatch`;
5. deterministic Reject short-circuits semantic concern review/disclosure;
6. `IntegrityConcernEvidence` is structurally bound synthetic-capable evidence, not authenticated semantic-assessor provenance;
7. five typed concern kinds preserve claim/belief/guess versus unavailable-knowledge/enacted-authority distinctions without truth policing;
8. exact disposition grammar: Reject-coded Input + null evidence -> Reject; zero Reject + non-empty concerns -> RequestAnotherTake; zero Reject + empty concerns -> Accept; inconsistent/missing evidence -> Integrity exception/no disposition;
9. assessor technical failure remains later technical/orchestration failure and does not automatically become RequestAnotherTake;
10. RequestAnotherTake has no retry/spend/provider authority;
11. no secret/canon keyword scanner, objective-truth contradiction rejection, or hidden Performance rewrite;
12. no authority-bearing eligibility/attestation object;
13. later State/Take/orchestration must authenticate configured concern-review provenance before effective progression;
14. immediate provisional-Take-versus-State-Interpreter ordering remains open for the later authority contract;
15. Director is not Integrity input and Integrity Accept cannot promote precommit Director work;
16. E0-E playwright control is not forced through the per-Character Candidate Integrity API, while frozen hard integrity gates remain required under its later control-compatible protocol;
17. no semantic-assessor transport, provider execution, State Interpreter/Authority, Take, commit, persistence, opportunity application, Scene loop, World Resolver, WinUI, Windows AI/NPU, packaging, WACK, or Store machinery entered Patch 0008.

Reference Candidate-content oracle exercised by Patch 0008 tests:
- SHA-256 `18eb8c9ba9d35f34f84e1fdd983eeb2a19ce015a2e44457c4514576f984af3b2`.

## Patch 0007 retained machine authority
Patch 0007 machine-tested executable/test head:
`aa9cd908194f414801fa0ed1bebea62298f798e2`

Detailed evidence:
`docs/evidence/H1_PATCH_0007_ARM64_VALIDATION.md`

Its full Core test authority was 211/211 before Patch 0008 added 42 Integrity executions. Patch 0008's 253/253 full-suite result supersedes it as the latest exercised Core regression total while preserving Patch 0007's historical validation record.

## Frozen regression identities
Missing Raft / Voss Context:
- StructuredContextHash `bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- ContextPacketId `CTX:bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- RenderedContextHash `ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88`.

Missing Raft ECJ-1:
- `9112` UTF-8 bytes;
- SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

## Validation authority
- Static/adversarial review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine: test-execution authority for exercised tests.
- Actual target-device execution: runtime authority for exercised behavior.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Explicitly unvalidated / excluded
Patch 0008 does not establish or implement:
- authenticated semantic assessor/provider behavior or bounded assessment packet;
- concern-review provenance authentication;
- provider-attempt provenance;
- retry/cost/cancellation execution;
- accepted/rejected/alternate Take semantics or TakeId allocation;
- State Interpreter candidate mutations;
- deterministic State Authority;
- ProductionState / StateHash;
- atomic source Performance+consequence commit/persistence/recovery;
- effective Current Opportunity mutation;
- atomic opportunity establishment/history append;
- next-Performer triggering or Scene loop;
- observation engine / World Resolver;
- E0-D round-robin execution;
- E0-E playwright-control integrity protocol;
- Windows AI or NPU execution/performance;
- WinUI;
- packaging/WACK;
- Microsoft Store certification.

Patch 0008 validation proves only the exercised deterministic Integrity binding/content-identity/evidence/evaluation boundary and its regressions.

## Immediate next action
Hold at the validated H1 Patch 0008 boundary until the next canonical H1 deterministic-spine contract is recovered from existing project authority.

Before new executable work:
1. read this checkpoint first;
2. read frozen Blueprint 0.1 / approved E0-A and H1 authority only as needed to resolve the next boundary;
3. search existing GitHub/project sources for an already-approved next contract before inventing one;
4. preserve the frozen sequence around Integrity, later State Interpreter, deterministic State Authority, Take semantics, atomic causal commit, and effective opportunity establishment;
5. do not infer that Integrity Accept is an accepted Take or authority-bearing eligibility token;
6. do not infer the provisional-Take-versus-State-Interpreter immediate ordering; that remains a later contract decision;
7. preserve the creator-ontology extensibility guard before freezing broad State mutation ontology;
8. if no implementation-complete next contract exists, create a standalone blueprint and recursively adversarial-audit it to a zero-material-change pass before requesting approval;
9. do not begin provider, persistence, Scene-loop, UI, Windows AI/NPU, or Store work merely as Patch 0008 cleanup.

## Continuity
Fresh chats read this file first.

Patch 0008:
- `docs/evidence/H1_PATCH_0008_ARM64_VALIDATION.md`;
- `docs/evidence/H1_PATCH_0008_BLUEPRINT_APPROVAL.md`;
- `docs/blueprint/H1_PATCH_0008_INTEGRITY_VALIDATOR_CONTRACT.md`.

Patch 0007:
- `docs/evidence/H1_PATCH_0007_ARM64_VALIDATION.md`;
- `docs/evidence/H1_PATCH_0007_BLUEPRINT_APPROVAL.md`;
- `docs/blueprint/H1_PATCH_0007_DIRECTOR_OPPORTUNITY_CONTRACT.md`.

Creator ontology guard:
- `docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md`.

Immediate upstream authority:
- `docs/blueprint/H1_PATCH_0006_PERFORMER_CANDIDATE_CONTRACT.md`;
- `docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md`;
- `docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`.

Do not reconstruct approved project state from chat history when GitHub contains it.

## Project rule
Every patch is reviewed against the previous validated baseline:
`correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence`

Git preserves history; the active source tree preserves only the best current architecture.
