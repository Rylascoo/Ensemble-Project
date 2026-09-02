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
- Patch 0006 Proposal 1.7 and Patch 0007 Proposal 1.5 each completed the required recursive adversarial audit with a full zero-material-change pass before approval.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Open design guard
`docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md` is a STRONG DIRECTION / OPEN DESIGN GUARD, not a frozen final product schema.

It preserves the rule:

> Keep authority semantics precise; keep creative semantics open.

Current E0 categories such as Knowledge, Beliefs, Suspicions, Memories, Goals, Relationships, Pressures, and Circumstance remain authoritative for frozen E0 contracts, but they must not be assumed to prove that the final creator-facing dramatic ontology is a closed set of engine enums. Revisit this guard before freezing the broad State Interpreter mutation ontology or durable post-E0 Production/Studio ontology.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

H1 Patch 0003 is COMPLETE and machine-validated for its exercised gates.
H1 Patch 0004 is COMPLETE and machine-validated for its exercised gates.
H1 Patch 0005 is COMPLETE and machine-validated for its exercised gates.
H1 Patch 0006 is COMPLETE and machine-validated for its exercised gates.
H1 Patch 0007 blueprint is APPROVED; executable implementation/validation has not yet begun.

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

## Patch 0007 blueprint promotion
Canonical specification:
`docs/blueprint/H1_PATCH_0007_DIRECTOR_OPPORTUNITY_CONTRACT.md`

Exact recursively audited and user-approved proposal head:
`3562db371f67f07f5896ff2a17f067e0ef02e6e9`

Approval PR #15:
- title `H1: define E0 Director opportunity contract`;
- GitHub records PR #15 as merged at the exact approved head;
- persistent draft state is a connector/UI artifact and is not approval authority;
- cumulative approved PR delta was exactly one blueprint document and zero executable files.

Approval record:
`docs/evidence/H1_PATCH_0007_BLUEPRINT_APPROVAL.md`

Approval-record commit:
`89807fc9e8e07c0ce9a14ee4a0151e76778edf83`

Creator-ontology guard commit:
`116af87022e70bb4cf6a918cb9858ea62f6d63a0`

## Patch 0007 approved architecture
Patch 0007 freezes only the E0 deterministic Director proposal boundary:
1. Missing Raft opening opportunity remains fixture-authored `VOSS`;
2. Patch 0007 proposes exactly one selected Character for the singular E0 Context/Performer path without closing post-E0 subset attention;
3. `DirectorOpportunityInput` is a structurally validated least-privilege snapshot containing only Scene/source/context identity, roster IDs, already-validated Candidate control, and supplied current-Scene opportunity-event history;
4. Director strategy code cannot inspect Candidate VisibleText or Character-private Context by type;
5. structural Bind is explicitly not causal-history authentication or routing authority;
6. E0 hard eligible set is the exactly three Context roster Characters;
7. semantic Proposal contract `ensemble.e0.director.opportunity.v1` contains only ContractVersion, SceneId, SourceCharacterId, SelectedCharacterId;
8. least-intervention strategy `ensemble.e0.director.least-intervention.v1` uses nomination, otherwise addressed-pool recency, otherwise complete-roster recency;
9. recency uses never-seen first, then oldest final history index, then ordinal CharacterId tie-break;
10. structural diagnostics detect never-opportunitied roster members, repeated same-Character attention, and last-four A-B-A-B alternation for trace/provenance only;
11. diagnostics never override explicit social selection and never become equal-turn/fairness authority;
12. no exclusion timers, max-gap rules, dialogue/token counts, scores, weights, probabilities, randomness, or LLM Director routing;
13. OpportunityHistory means effective opportunity establishments/events only; retries, rejected/alternate attempts under the same opportunity, speculation, recomputation, and failed opportunity application do not append events;
14. Director evaluation history ends at the source opportunity; the proposed target is not yet history;
15. precommit Director evaluation is discardable zero-authority speculation;
16. after successful atomic source Performance+consequence commit, later authority must obtain authoritative current-Scene history ending at source, re-Bind source Context semantics matching accepted Candidate `ContextPacketId`, and recompute;
17. postcommit recomputation is still only a proposal;
18. later effective-opportunity establishment must atomically establish Current Opportunity and append the corresponding opportunity event, or do neither;
19. failed opportunity establishment leaves the already-committed source Take intact but does not change Current Opportunity/history or trigger another Performer; Patch 0007 defines no fallback application;
20. E0-D round-robin remains a separate future strategy expected to consume the same structural Input and emit the same semantic Proposal shape under its own evaluation/trace;
21. no provider, Integrity, State Interpreter, State Authority, Take semantics, source-commit implementation, persistence, Scene loop, World Resolver, UI, Windows AI/NPU, packaging, WACK, or Store implementation is authorized by Patch 0007.

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
Patch 0007 is approved architecturally but does not yet establish:
- executable Director behavior;
- native ARM64 compiler/test/runtime evidence for Director;
- causal-history authentication/persistence;
- effective Current Opportunity application;
- atomic opportunity-establishment implementation;
- live provider/model behavior or model quality;
- provider request/system-contract framing;
- provider-attempt provenance persistence;
- Integrity Validator acceptance/rejection;
- accepted/rejected/alternate Take semantics or TakeId allocation;
- State Interpreter candidate mutations;
- deterministic State Authority;
- ProductionState / StateHash;
- atomic source causal commit/persistence/recovery replay;
- accepted history / non-empty recent-performance Context;
- observation engine / World Resolver;
- E0-D round-robin implementation/bindings;
- Windows AI or NPU execution/performance;
- WinUI;
- packaging/WACK;
- Microsoft Store certification.

Patch 0006 remains the latest machine-validated executable boundary until Patch 0007 passes its native gates.

## Immediate next action
Implement **H1 Patch 0007 only** from this approval checkpoint.

Implementation constraints:
1. create a dedicated Patch 0007 implementation branch from this exact `main` checkpoint;
2. implement only structurally validated `DirectorOpportunityInput.Bind(...)`, shared semantic `DirectorOpportunityProposal`, `LeastInterventionDirector.Propose(...)`, strategy-specific Evaluation/Trace, recency helper, and trace-only structural diagnostics;
3. do not modify Access Control, Context Composer, Performer Candidate contract, fixture JSON, Harness behavior, or frozen identities unless compiler evidence proves a minimal compatibility correction is required;
4. do not implement effective Current Opportunity mutation, opportunity-history mutation/persistence, source commit, provider execution, Integrity, State Interpreter/Authority, Take semantics, World Resolver, E0-D round-robin, or Scene loop;
5. implementation must receive recursive static/adversarial review before native gate;
6. native gate must include ARM64 Harness build, full Core tests, Missing Raft PASS/0, smoke PASS/0, and frozen identity regressions;
7. if machine evidence finds a defect, patch the smallest affected surface and rerun only the gates invalidated by that change.

## Continuity
Fresh chats read this file first.

For Patch 0007:
- `docs/evidence/H1_PATCH_0007_BLUEPRINT_APPROVAL.md`;
- `docs/blueprint/H1_PATCH_0007_DIRECTOR_OPPORTUNITY_CONTRACT.md`.

For the creator ontology guard:
- `docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md`.

Immediate upstream executable authority:
- `docs/evidence/H1_PATCH_0006_ARM64_VALIDATION.md`;
- `docs/blueprint/H1_PATCH_0006_PERFORMER_CANDIDATE_CONTRACT.md`;
- `docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md`;
- `docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`.

Do not reconstruct approved project state from chat history when GitHub contains it.

## Project rule
Every patch is reviewed against the previous validated baseline:
`correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence`

Git preserves history; the active source tree preserves only the best current architecture.