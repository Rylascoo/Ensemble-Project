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
- Patch 0006 Proposal 1.7 completed the required recursive adversarial audit with a full zero-material-change pass before approval.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

H1 Patch 0003 is COMPLETE and machine-validated for its exercised gates.
H1 Patch 0004 is COMPLETE and machine-validated for its exercised gates.
H1 Patch 0005 is COMPLETE and machine-validated for its exercised gates.
H1 Patch 0006 is COMPLETE and machine-validated for its exercised gates.

## Patch 0006 machine validation

Native ARM64 Harness build/runtime authority head:
`fa37653d5c3e492e53d0f927a9c0844e139f12eb`

At that head on the user's native Windows ARM64 machine:
- `Ensemble.E0.Core` build PASS;
- `Ensemble.E0.Harness` build PASS;
- Harness output target `net9.0\win-arm64`;
- Missing Raft runtime PASS; exit `0`;
- generic smoke runtime PASS; exit `0`.

Corrected full Core test authority head:
`93d549b2ca02db81a87596c0936f29f7c89058db`

At that head on the same machine:
- `Ensemble.E0.Core` rebuilt successfully;
- `Ensemble.E0.Core.Tests` built successfully with warnings-as-errors;
- Core tests PASS — `173` total, `173` succeeded, `0` failed, `0` skipped.

The first test attempt at `fa37653...` was blocked before execution by two `MSTEST0032` analyzer errors in new test code only. The assertions were tautological compile-time checks. They were removed without changing production source.

Corrected executable/test content head:
`175f25ff8055943cdd900b08c1b89bea8b887692`

GitHub comparison from `fa37653...` to `175f25...` changes only:
`tests/Ensemble.E0.Core.Tests/Performer/PerformerCandidateContractTests.cs`

No production source changed between the native Harness build/runtime authority and the corrected full test authority. The later tested `93d549b...` head adds validation documentation beyond the corrected test source.

Detailed evidence:
`docs/evidence/H1_PATCH_0006_ARM64_VALIDATION.md`

## Patch 0006 promotion
Approved blueprint PR #13:
- approved proposal head `eea1398cb49ac8ec5275f65083093d7fdaab890b`;
- PR #13 is recorded by GitHub as merged;
- persistent draft state is a connector/UI artifact after the ready-for-review GraphQL mutation failure and is not approval authority.

Approval checkpoint before executable work:
`d637a4c5d476e3d92de78e3d0784f8dcf6258d2c`

Implementation PR #14 closure head / merge SHA:
`76a80171d4011c85d734ddf1ba8806dab546b0f4`

PR #14 is recorded by GitHub as merged. Commits after machine-tested head `93d549b...` on the implementation lineage are documentation-only validation closure and do not increase executable validation authority.

## Patch 0006 implementation result
Canonical specification:
`docs/blueprint/H1_PATCH_0006_PERFORMER_CANDIDATE_CONTRACT.md`

Implemented architecture:
1. semantic candidate contract version `ensemble.e0.performer.candidate.v1`;
2. E0 AI JSON transport schema separately versioned as `ensemble.e0.performer.candidate-json.v1`;
3. `CandidatePerformance` contains only `ContractVersion`, `SubjectCharacterId`, `ContextPacketId`, `VisibleText`, and `Control`;
4. `CandidatePerformanceControl` contains only `AddressedCharacterIds` and optional `NominatedCharacterId`;
5. no `PerformanceKind` taxonomy; empty text represents silence and non-empty Performance grammar remains open;
6. non-empty text must contain display-bearing Unicode content, already be NFC, preserves exact decoded text, permits LF/TAB, and rejects unsafe Control scalars/invisible-only pseudo-performance;
7. typed control is provisional Performer intent metadata only and has no truth, state, observation, Take, Director-selection, or persistence authority;
8. address/nomination targets resolve exactly and case-sensitively to the safe Context roster, with no self-targets and no duplicate addresses;
9. roster-derived address-array limits are enforced during streaming JSON preflight before DOM allocation;
10. public construction is limited to `PerformerCandidateContract.ParseJson(ContextPacket, ReadOnlySpan<byte>)`;
11. candidate/control constructors are Core-internal;
12. trusted Context invariants are checked before untrusted candidate bytes;
13. parser safety ceilings are exactly `1,048,576` bytes inclusive and JSON depth `8`;
14. BOM, comments, trailing commas, duplicate decoded properties, unknown/missing properties, wrong token types, malformed UTF-8/Unicode/JSON, and trailing content fail closed;
15. candidate-domain exception representation is sanitized so untrusted prose, IDs, schema values, unknown properties, and malformed snippets are not echoed through the exception chain;
16. candidate objects carry no RenderingContract, RenderedContextHash, provider/model/request identity, raw output, mutation proposal, confidence, private reasoning, CandidateId, TakeId, accepted/committed status, or Director authority flag;
17. exact provider disclosure/request identity and raw/partial/error transport output remain later provider-attempt/provenance concerns;
18. rejected/partial/cancelled attempted output/control remains required E0 experimental provenance when that later orchestration layer exists, without becoming Production history;
19. no provider/model call, Director, Integrity Validator, State Interpreter, State Authority, Take semantics, causal commit, persistence, E0-D binding, WinUI, Windows AI/NPU, WACK, or Store machinery was introduced.

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
Patch 0006 does not establish or implement:
- live provider/model behavior or model quality;
- provider request/system-contract framing;
- provider-attempt provenance persistence;
- Director opportunity selection;
- Integrity Validator acceptance/rejection;
- accepted/rejected/alternate Take semantics or TakeId allocation;
- State Interpreter candidate mutations;
- deterministic State Authority;
- ProductionState / StateHash;
- atomic causal commit/persistence/recovery replay;
- accepted history / non-empty recent-performance Context;
- observation engine;
- E0-D execution bindings;
- Windows AI or NPU execution/performance;
- WinUI;
- packaging/WACK;
- Microsoft Store certification.

Patch 0006 validation proves only the exercised deterministic Performer candidate-contract boundary and its regressions.

## Immediate next action
Hold at the validated H1 Patch 0006 boundary.

Frozen E0-A continuation authority places the next contract after Performer candidate output as:

**Director opportunity-selection contract.**

Before executable Director work:
1. read this checkpoint first;
2. read the approved Patch 0006 blueprint and only upstream authority needed for Director inputs;
3. recover any existing canonical Director roadmap/specification from GitHub/project authority;
4. if GitHub lacks an implementation-complete Director contract, create a standalone blueprint and run recursive adversarial audit to a zero-material-change pass before requesting approval;
5. preserve Director as opportunity management only, never outcome/truth/state authority;
6. preserve the rule that provisional/uncommitted Candidate control cannot create an effective opportunity or trigger another Performer;
7. preserve atomic causal history: Performance and authoritative consequences must successfully commit before associated control can become eligible as a non-binding effective Director input;
8. do not implement provider integration merely to define Director authority.

Do not enter provider integration, Integrity, State Interpreter, State Authority, Take semantics, persistence, or E0-D bypasses as Patch 0006 cleanup.

No further Patch 0006 implementation work is required.

## Continuity
Fresh chats read this file first.

For Patch 0006:
- `docs/evidence/H1_PATCH_0006_ARM64_VALIDATION.md`;
- `docs/blueprint/H1_PATCH_0006_PERFORMER_CANDIDATE_CONTRACT.md`.

For immediate upstream authority, read only as needed:
- `docs/evidence/H1_PATCH_0005_ARM64_VALIDATION.md`;
- `docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md`;
- `docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`.

Do not reconstruct approved project state from chat history when GitHub contains it.

## Project rule
Every patch is reviewed against the previous validated baseline:
`correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence`

Git preserves history; the active source tree preserves only the best current architecture.