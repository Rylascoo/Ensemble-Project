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
- H1 Patch 0006 Performer Candidate Output Contract Blueprint Proposal 1.7 is APPROVED and canonical for executable implementation.
- Patch 0006 Proposal 1.7 completed the required recursive adversarial audit with a full zero-material-change pass before approval.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

H1 Patch 0003 is COMPLETE, machine-validated for its exercised gates, and promoted through PR #8.

H1 Patch 0004 is COMPLETE, machine-validated for its exercised gates, and promoted through PR #10.

H1 Patch 0005 is COMPLETE for its exercised gates and promoted through PR #12.

H1 Patch 0006 blueprint authority is APPROVED through PR #13. Executable Patch 0006 implementation has not yet received compiler/test/runtime authority.

## Patch 0006 approved blueprint checkpoint
Canonical specification:
`docs/blueprint/H1_PATCH_0006_PERFORMER_CANDIDATE_CONTRACT.md`

Approved blueprint head / PR #13 promoted head:
`eea1398cb49ac8ec5275f65083093d7fdaab890b`

PR #13 is recorded by GitHub as merged. The connector's draft-to-ready GraphQL mutation failed on GitHub's schema, so `main` was advanced by a non-force fast-forward to the exact approved head. GitHub then recorded PR #13 as merged at that same head. The lingering draft flag is not authority.

Approved Patch 0006 architecture:
1. `CandidatePerformance` is semantic, provider-neutral Performer output rather than raw AI transport.
2. Semantic candidate contract: `ensemble.e0.performer.candidate.v1`.
3. E0 AI JSON transport schema is independently versioned as `ensemble.e0.performer.candidate-json.v1`.
4. Semantic candidate contains only `ContractVersion`, `SubjectCharacterId`, `ContextPacketId`, `VisibleText`, and `Control`.
5. `CandidatePerformanceControl` contains only `AddressedCharacterIds` and optional `NominatedCharacterId`.
6. No `PerformanceKind` taxonomy is introduced. Empty `VisibleText` means silence; non-empty E0 text remains grammar-open Character-legible Performance.
7. Non-empty text must contain a display-bearing Unicode scalar, must already be NFC, preserves exact text, permits LF/TAB, and rejects unsafe Control/invisible-only pseudo-performance.
8. Candidate prose remains untrusted creative content. Parsing never promotes assertion to truth, knowledge, system instruction, state mutation, routing authority, or history.
9. Control is provisional Performer intent metadata, not Production history/truth/state/observation/Director authority.
10. Address/nomination are independently optional for non-silent Performance; target IDs are exact/case-sensitive roster IDs; self-target and duplicate addresses fail; addresses sort ordinally and are roster-minus-subject bounded.
11. Precommit control cannot create effective opportunity, trigger another Performer, or survive rollback as effective routing state. Only associated control from a later successfully committed accepted Performance may become a non-binding Director input.
12. All E0 attempted output/control that exists, including rejected/partial attempts, remains mandatory experimental provenance/diagnostics and never automatically Production history.
13. `ParseJson(ContextPacket, ReadOnlySpan<byte>)` is the only Patch 0006 public construction path; candidate/control constructors remain non-public; one internal semantic builder owns invariants.
14. Strict parser is fail-closed: trusted Context invariants first; 1 MiB inclusive byte ceiling; max depth 8; BOM/comments/trailing commas/duplicate decoded properties/unknown or missing fields/wrong token types/malformed UTF-8 or JSON/trailing content all fail.
15. Entire externally observable exception representation must not echo raw payload, candidate prose, unknown property names, invalid IDs, actual mismatched schema values, arbitrary untrusted values, secrets, or JSON snippets.
16. Semantic candidate copies only SubjectCharacterId + ContextPacketId from the safe reference ContextPacket. Rendering/provider/request/transport facts belong to later provider-attempt provenance where exact external disclosure can be recorded truthfully.
17. Candidate parsing does not hard-code Context composition/rendering policy strings it does not interpret; this preserves safe composition variation without weakening the separately labeled omniscient E0-D boundary.
18. No provider integration, provider request/system prompt, Director, Integrity Validator, Take semantics, State Interpreter, State Authority, causal commit, persistence, observation engine, E0-D binding, WinUI, Windows AI/NPU, packaging, WACK, or Store work enters Patch 0006.

## Latest machine-validated Patch 0005 implementation state
Corrected full Core test head:
`befb6648c36400546ac4843d575bd64760b23445`

At that head on the user's native Windows ARM64 machine:
- `Ensemble.E0.Core` rebuilt successfully as part of `dotnet test`;
- Core tests: PASS — 115 total, 115 succeeded, 0 failed, 0 skipped.

Native Harness build/runtime authority was established at earlier implementation head:
`44efd6358d8a94e769eee2ff9b2c7bd4715f5587`

At that head on the same native Windows ARM64 machine:
- Core and Harness build: PASS;
- Harness target output: `net9.0\win-arm64`;
- Missing Raft runtime regression: PASS; exit `0`;
- generic smoke runtime regression: PASS; exit `0`.

GitHub comparison from `44efd...` through `befb...` contains only:
- `docs/evidence/H1_PATCH_0005_REFERENCE_ORACLE.md`;
- `tests/Ensemble.E0.Core.Tests/Context/DeterministicContextComposerTests.cs`.

No production source changed between those machine-tested heads. Therefore the Harness build/runtime evidence applies to the same production source content that passed the corrected 115-test gate.

Patch 0005 PR #12 closure head:
`7256d5a59776b5c8198b14de4335ebb92eda89c4`

PR #12 merge commit on `main`:
`b0573a6ad8049ee5a03cd5af822476ad013135e3`

Commits after `befb...` on the PR implementation lineage are documentation-only evidence closure and do not increase executable validation authority.

Detailed evidence:
- `docs/evidence/H1_PATCH_0005_ARM64_VALIDATION.md`
- `docs/evidence/H1_PATCH_0005_REFERENCE_ORACLE.md`

## Patch 0005 corrected frozen Context identity
Canonical Missing Raft / Voss reference:
- structured canonical UTF-8 length: `2569` bytes;
- StructuredContextHash: `bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- ContextPacketId: `CTX:bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`;
- TrustedStateText UTF-8 length: `1696` bytes;
- rendered envelope UTF-8 length: `1905` bytes;
- RenderedContextHash: `ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88`.

The original pre-machine oracle accidentally transcribed canonical `REL-VOSS-MARLOWE` text as `competence` instead of fixture-authoritative `perception`. Because both words are ten ASCII bytes, byte-length expectations remained unchanged while both hashes changed. The canonical fixture controlled; production Composer code did not change.

## Patch 0005 implementation result
Canonical specification:
`docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md`

Implemented architecture:
1. `DeterministicContextComposer` is the single safe E0 reference Composer for `ensemble.e0.context.full-authorized.v1`;
2. public Composer input is limited to `CharacterAccessProjection + CurrentOpportunityCharacterId`;
3. current opportunity must equal the projection subject and grants no additional knowledge;
4. every already-permitted Access record is included exactly once in its original authority category;
5. epistemic and authority categories remain structurally distinct;
6. no semantic relevance, ranking, token optimization, summarization, paraphrase, deduplication, or truncation occurs in the E0 reference contract;
7. `recentPerformances` is canonically exact `[]`; no accepted-history DTO or non-empty population path exists yet;
8. provider-neutral rendering uses `ensemble.e0.context.render.v1` with deterministic separate authority layers;
9. rendered context omits internal IDs/provenance/provider/debug data;
10. StructuredContextHash and RenderedContextHash remain distinct deterministic identities;
11. `ContextPacketId = CTX:<StructuredContextHash>` is content identity, not authorization/provenance/signature;
12. local composition trace remains separate from Performer context;
13. public safe output types have Core-internal constructors;
14. ECJ-1 shared canonical JSON primitive preserves frozen Missing Raft identity;
15. E0-D conditions remain separate experimental paths rather than safe-reference bypasses.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine: test-execution authority for tests actually exercised.
- Actual target-device execution: runtime authority for behavior actually exercised.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Explicitly unvalidated / excluded
Patch 0006 approval does not establish or validate:
- any Patch 0006 executable implementation;
- provider/model behavior or provider-request framing;
- exact provider-attempt/disclosure provenance implementation;
- Director/opportunity selection;
- Integrity Validator acceptance/rejection;
- accepted Take/history authority;
- State Interpreter / State Authority;
- ProductionState / StateHash;
- atomic causal commit/persistence/recovery;
- observation engine;
- E0-D execution;
- Windows AI or NPU execution/performance;
- WinUI;
- packaging/WACK;
- Microsoft Store certification.

## Immediate next action
Implement **H1 Patch 0006 only** from this approved checkpoint.

Required workflow:
1. branch from the approved `main` checkpoint after this CURRENT_STATE commit;
2. implement only semantic `CandidatePerformance`/Control plus strict `candidate-json.v1` parser and tests;
3. preserve Patch 0005 Context and ECJ-1 frozen identities exactly;
4. run static/adversarial implementation review against the approved 1.7 exit gate;
5. request native Windows ARM64 build/test/Harness validation before claiming executable validation;
6. do not enter provider integration, Director, Integrity, State, Take, persistence, E0-D, WinUI, Windows AI/NPU, or Store scope as Patch 0006 cleanup.

## Continuity
Fresh chats read this file first.

For current Patch 0006 authority, then read:
- `docs/blueprint/H1_PATCH_0006_PERFORMER_CANDIDATE_CONTRACT.md`;
- PR #13 history only if audit evolution is relevant.

For Patch 0005 executable evidence, read:
- `docs/evidence/H1_PATCH_0005_ARM64_VALIDATION.md`;
- `docs/evidence/H1_PATCH_0005_REFERENCE_ORACLE.md`;
- `docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md`.

For earlier authority, read only as needed:
- `docs/evidence/H1_PATCH_0004_ARM64_VALIDATION.md`;
- `docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`;
- `docs/blueprint/H1_PATCH_0003_ECJ1_FIXTURE_HASH.md`;
- `docs/blueprint/H1_PATCH_0002_2_MISSING_RAFT_CONTRACT.md`.

Do not reconstruct already-approved project state from chat history when GitHub contains it.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
