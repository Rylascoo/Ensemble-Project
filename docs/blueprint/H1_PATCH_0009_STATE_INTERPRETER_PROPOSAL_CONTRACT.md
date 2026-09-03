# H1 Patch 0009 — E0 State Interpreter Mutation-Proposal Contract

Status: blueprint proposal 0.2 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: machine-validated H1 Patch 0008
Branch: `h1-patch-0009-state-interpreter-blueprint`

## 1. Purpose

Define the next E0 deterministic-spine boundary after the machine-validated Integrity Validator:

```text
ContextPacket + CandidatePerformance + Integrity Accept evaluation
    -> StateInterpretationSource.Bind
        -> structurally bound, non-authoritative interpretation source
            -> later State Interpreter semantic generation
                -> strict State Interpreter proposal transport
                    -> StateInterpretationProposal
                        -> later deterministic State Authority
```

Patch 0009 defines only:

1. the E0 semantic State Interpreter mutation-proposal vocabulary;
2. a strict E0 AI JSON transport/parser;
3. structural binding to the exact Candidate content identity and an Integrity `Accept` evaluation;
4. bounded structural evidence references for later State Authority review.

It does **not** call a model/provider, authenticate semantic-review provenance, allocate a Take, decide provisional-Take timing, approve/reject mutations, mutate Production state, commit history, persist anything, or trigger another Performer.

## 2. Recovered frozen authority

Frozen Blueprint 0.1 / H1 sequence:

1. Performer candidate output;
2. Director opportunity selection;
3. Integrity Validator;
4. **State Interpreter candidate-mutation schema**;
5. deterministic State Authority;
6. accepted/rejected/alternate Take semantics;
7. atomic causal commit.

Frozen law:

- probabilistic systems may propose; deterministic authority decides;
- `Integrity Validator -> State Interpreter -> deterministic State Authority`;
- State Interpreter proposes what a Performance may mean but never mutates authority;
- State Authority later decides under creator locks, type/transition rules, review policy, and causal evidence;
- accepted Performance + approved consequences later form one atomic causal commit;
- statement != fact; possibility != fact;
- objective truth, observation, claim, belief, memory, rumor, and unresolved proposition remain distinct;
- Constitution is read-only in E0;
- Disposition mutation is rare/conservative;
- Circumstance may change frequently when supported;
- creator-locked canon cannot change;
- technical failure cannot become fiction;
- full Observation and World Resolver remain reserved/outside E0;
- E0 provenance preserves proposed Interpreter mutations plus committed/rejected mutations with reasons.

Patch 0008 additionally freezes:

- Integrity `Accept` is evaluation only, not accepted Take or State authority;
- synthetic concern evidence may produce synthetic Accept for tests;
- later State/Take/orchestration must authenticate configured review provenance before effective progression;
- provisional-Take-versus-Interpreter ordering remains open;
- Patch 0008 creates no authority-bearing eligibility/attestation token.

## 3. Why schema/parser comes before provider execution

The frozen next requirement is a **candidate-mutation schema**, not an Interpreter provider integration.

Provider execution would prematurely require unresolved contracts for Interpreter input disclosure, provider-attempt provenance, concern-review authentication, Take timing, retry/cost policy, current ProductionState, StateHash, and causal persistence.

Patch 0009 therefore freezes the proposal boundary only.

## 4. Creator Ontology Extensibility Guard revisit

The guard requires a deliberate revisit before freezing broad State Interpreter mutation ontology.

E0 decision:

> Use an explicitly E0-scoped authority vocabulary containing only distinctions the frozen experiment already needs. Do not claim these domains are the final creator-facing or post-E0 storage ontology.

Therefore:

- UI labels are not storage types;
- creator-authored dramatic concepts remain open-ended;
- no generalized meta-ontology is introduced prematurely;
- current frozen E0 categories remain precise for E0;
- post-E0 Production/Studio ontology remains open.

## 5. No provisional-Take ordering decision

Patch 0009 does not decide whether a future orchestration contract creates a provisional Take before or after interpretation/State review.

It freezes only:

```text
A structurally matching Integrity Accept evaluation must exist before
StateInterpretationSource.Bind succeeds.
```

That does not mean the Candidate is an accepted Take.

## 6. Contract domains

```text
StateInterpretationContractVersion = ensemble.e0.state-interpreter.proposal.v1
StateInterpretationJsonSchemaVersion = ensemble.e0.state-interpreter.proposal-json.v1
```

Candidate content identity remains Patch 0008:

```text
ensemble.e0.integrity.candidate-content.v1
```

Patch 0009 does not redefine Candidate identity.

## 7. Structurally bound source

```text
StateInterpretationSource.Bind(
    ContextPacket sourceContext,
    CandidatePerformance sourceCandidate,
    IntegrityValidationEvaluation integrityEvaluation)
    -> StateInterpretationSource
```

This is the only Patch 0009 rich-object binding boundary.

It verifies upstream structural identity/evaluation consistency and copies only narrow structural data.

## 8. StateInterpretationSource public shape

```text
StateInterpretationSource
- CandidateContentIdentityContract
- CandidateContentHash
- SourceCharacterId
- SourceContextPacketId
- RosterCharacterIds
- IntegrityValidationContract
```

No public constructor.

Roster IDs are canonical ordinal.

No Candidate VisibleText, Character-private Context prose, rendering, access audit, provider/model data, Take identity, State mutation, or authority flag is exposed.

## 9. Source Bind invariants

Fail in the Patch 0009 exception domain unless:

1. source Context/Candidate/evaluation are non-null and structurally initialized;
2. Context subject == Context opportunity Character;
3. fresh `IntegrityCandidateInput.Bind(sourceContext, sourceCandidate)` succeeds;
4. fresh Integrity input has zero deterministic Reject codes;
5. evaluation Disposition == `Accept`;
6. evaluation trace ValidationContract == `ensemble.e0.integrity.validation.v1`;
7. trace Input identity contract/hash/source ContextPacket identity exactly match the fresh Integrity input;
8. concern evidence exists, is structurally bound to the same Candidate content, and contains zero concerns;
9. source Context roster is initialized, exactly three unique E0 Characters, and contains the source Character exactly once.

Do not reimplement Patch 0006 Candidate validation or Patch 0008 concern semantics.

## 10. Structural Accept binding is not authentication

A bound Source means only that the supplied semantic objects form a structurally consistent Patch 0008 Accept evaluation for the Candidate content.

It does not prove:

- configured concern review actually occurred;
- provider/assessor identity;
- real provider attempt;
- accepted Take;
- permission to spend/call a provider;
- permission to commit State.

Synthetic Accept may create synthetic Source objects for tests. Later orchestration must authenticate review/provider provenance before effective use.

## 11. Semantic proposal

```text
StateInterpretationProposal
- ContractVersion
- CandidateContentIdentityContract
- CandidateContentHash
- Mutations
```

No public constructor. Strict ParseJson is the E0 construction path.

Proposal identity must match the supplied Source exactly.

No Take/Commit ID, acceptance flag, State Authority decision, provider/model, confidence, generated rationale, Director decision, Current Opportunity, or authoritative new RecordId exists.

## 12. Empty mutations are valid

`Mutations = []` means only:

> No durable projected-state mutation is proposed for this Performance.

Accepted historical texture may remain true/recoverable later without becoming durable projected state. Empty mutation output is not rejection and does not erase Performance.

## 13. E0 proposal domains

```text
WorldState
SceneState
UnresolvedProposition
CharacterKnowledge
CharacterBelief
CharacterSuspicion
CharacterMemory
CharacterGoal
CharacterDisposition
CharacterCircumstance
CharacterClaim
Relationship
Pressure
```

These are E0 authority-target domains, not final UI/storage ontology.

## 14. Domain meanings

- **WorldState** — possible durable world consequence of Character performance; never truth merely because proposed.
- **SceneState** — possible current Scene-state consequence without creating an autonomous World Resolver.
- **UnresolvedProposition** — preserves uncertainty as uncertainty.
- **CharacterKnowledge** — proposed knowledge state; later authority must prevent claim/guess promotion.
- **CharacterBelief** — proposed belief, possibly false.
- **CharacterSuspicion** — proposed suspicion, possibly false/uncertain.
- **CharacterMemory** — proposed durable recollection, which may differ from objective truth.
- **CharacterGoal** — proposed current goal change.
- **CharacterDisposition** — proposed persistent tendency; downstream authority must be conservative.
- **CharacterCircumstance** — proposed fluid immediate Character state.
- **CharacterClaim** — structured claim attributable to the source Character; claim != fact.
- **Relationship** — directional semantic relationship effect/state; no numeric psychology.
- **Pressure** — proposed current/durable pressure without hard-coding every dramatic concept.

## 15. Domains the Interpreter cannot represent

No proposal domain for:

- Constitution — E0 read-only;
- HistoricalTruth / accepted Performance history — later Take/commit authority;
- Observation — reserved observation-eligibility boundary;
- creator-locked Canon as a direct mutable target;
- Presentation Perspective — disclosure projection, not State;
- Director opportunity — routing authority, not State mutation.

## 16. Change semantics: typed semantic union

The semantic Core contract must make invalid operation/text/reference combinations difficult to express.

Freeze one abstract/read-only change concept with exactly three concrete immutable variants:

```text
StateMutationChange
    AddStateMutationChange
        Text

    SupersedeStateMutationChange
        ExistingRecordId
        Text

    DeactivateStateMutationChange
        ExistingRecordId
```

No public constructors; parser/internal invariant builder constructs valid variants.

Semantics:

- **Add** introduces a new durable semantic record candidate.
- **Supersede** proposes a new semantic record explicitly superseding an existing record while preserving history.
- **Deactivate** proposes removal from current projection while preserving historical recoverability.

No destructive Delete or silent in-place Replace exists.

## 17. Typed mutation-candidate union

Freeze one abstract/read-only base concept with exactly four immutable variants:

```text
StateMutationCandidate

    GlobalStateMutationCandidate
        Domain
        Change
        EvidenceRecordIds

    CharacterStateMutationCandidate
        Domain
        SubjectCharacterId
        Change
        EvidenceRecordIds

    CharacterClaimMutationCandidate
        SubjectCharacterId
        Text
        EvidenceRecordIds

    RelationshipStateMutationCandidate
        SubjectCharacterId
        TargetCharacterId
        Change
        EvidenceRecordIds
```

No public constructors. Parser/internal invariant builder constructs valid variants.

This is a semantic type boundary only. The AI JSON transport remains flat and discriminator-based for simplicity.

## 18. Domain membership per semantic variant

### GlobalStateMutationCandidate

Domain must be exactly one of:

```text
WorldState
SceneState
UnresolvedProposition
Pressure
```

### CharacterStateMutationCandidate

Domain must be exactly one of:

```text
CharacterKnowledge
CharacterBelief
CharacterSuspicion
CharacterMemory
CharacterGoal
CharacterDisposition
CharacterCircumstance
```

SubjectCharacterId must be an exact current E0 roster member.

### CharacterClaimMutationCandidate

- implicit Domain = `CharacterClaim`;
- implicit operation = Add;
- SubjectCharacterId must equal SourceCharacterId;
- claim Text required;
- no ExistingRecordId exists in semantic shape.

Later contradictory claims are new claims, never rewrites/deletions of earlier claims.

### RelationshipStateMutationCandidate

- implicit Domain = `Relationship`;
- subject + target must both be exact current roster members;
- subject != target;
- contains a typed Change.

No semantic mutation variant contains nullable Character IDs merely to satisfy another domain's shape.

## 19. ExistingRecordId semantics

ExistingRecordId exists only inside Supersede/Deactivate change variants.

Parser validates syntactic RecordId validity only.

It does **not** prove existence, active status, ownership/domain, mutability, creator-lock status, or causal suitability.

Later deterministic State Authority must validate all of those against authoritative state.

## 20. Mutation text

Add/Supersede text and CharacterClaim text:

- non-null/non-empty;
- exact decoded content preserved;
- no trim/repair/normalization;
- must already be NFC;
- LF/TAB allowed;
- other Unicode Control scalars rejected;
- must contain a display-bearing Unicode scalar using the Patch 0006 category principle.

Text is untrusted interpreted creative content, never instruction/authority.

Deactivate has no Text property in semantic shape.

## 21. EvidenceRecordIds

Each semantic mutation variant carries immutable EvidenceRecordIds:

- syntactically valid RecordIds;
- duplicate-free;
- canonical ordinal order;
- may be empty because Candidate content itself is always the root causal source;
- may refer to supporting state the future Interpreter was shown;
- never proves record existence, disclosure, relevance, or authority.

Provider/orchestration provenance later establishes actual Interpreter disclosure. State Authority later validates evidence references against authoritative State.

No free-form rationale, quote, numeric confidence, score, or hidden reasoning is part of the mutation contract.

## 22. Candidate content is root binding

Every proposal carries CandidateContentIdentityContract + CandidateContentHash matching StateInterpretationSource.

Candidate hash is content identity only—not provider attempt, accepted Take, or commit identity.

## 23. Flat AI JSON transport

The transport deliberately stays flat even though semantic Core types are stricter:

```json
{
  "schemaVersion": "ensemble.e0.state-interpreter.proposal-json.v1",
  "candidateContentIdentityContract": "ensemble.e0.integrity.candidate-content.v1",
  "candidateContentHash": "<64 lowercase hex>",
  "mutations": [
    {
      "domain": "characterBelief",
      "operation": "add",
      "subjectCharacterId": "VOSS",
      "targetCharacterId": null,
      "existingRecordId": null,
      "text": "Voss now believes ...",
      "evidenceRecordIds": []
    }
  ]
}
```

All seven mutation properties are required in transport, using explicit null where semantically absent. Parser converts a valid flat object into the appropriate immutable semantic variant/change type.

Property order/insignificant whitespace have no meaning.

## 24. Exact transport domain strings

```text
worldState
sceneState
unresolvedProposition
characterKnowledge
characterBelief
characterSuspicion
characterMemory
characterGoal
characterDisposition
characterCircumstance
characterClaim
relationship
pressure
```

Operation strings:

```text
add
supersede
deactivate
```

Exact case only; no aliases.

## 25. Transport-to-semantic rules

### Global domains

Transport requires subjectCharacterId = null and targetCharacterId = null.

### Character state domains

Transport requires roster subjectCharacterId and targetCharacterId = null.

### Relationship

Transport requires distinct roster subject + target.

### CharacterClaim

Transport requires:

- operation = `add`;
- subject = source Character;
- target = null;
- existingRecordId = null;
- text valid.

### Add

existingRecordId = null; text required -> AddStateMutationChange.

### Supersede

existingRecordId required; text required -> SupersedeStateMutationChange.

### Deactivate

existingRecordId required; text = null -> DeactivateStateMutationChange.

Invalid field combinations fail before semantic object construction.

## 26. Strict parser

```text
StateInterpretationContract.ParseJson(
    StateInterpretationSource source,
    ReadOnlySpan<byte> utf8Proposal)
    -> StateInterpretationProposal
```

Requirements:

- validate trusted Source before untrusted JSON;
- non-empty UTF-8; BOM rejected;
- max 1,048,576 bytes inclusive;
- max JSON depth 8;
- comments/trailing commas rejected;
- malformed UTF-8/JSON rejected;
- decoded duplicate property names rejected at every object;
- unknown/missing properties rejected;
- exact property name/case;
- exact enum strings;
- wrong JSON types/trailing content rejected;
- candidate identity contract/hash must match Source exactly;
- Character IDs must satisfy roster/cardinality rules;
- RecordIds syntactically valid;
- EvidenceRecordIds duplicates rejected then canonicalized ordinally;
- exact duplicate semantic mutations rejected after canonical evidence ordering;
- exceptions sanitized: no mutation Text, arbitrary unknown property names/values, raw snippets, or provider content.

No provider/model call occurs.

## 27. Mutation array ordering

Mutation array order is preserved exactly for experimental reconstruction.

Order grants no State Authority priority and does not define sequential application.

Later State Authority must define deterministic complete-batch conflict/stale-state behavior.

## 28. Duplicates versus conflicts

Exact duplicate semantic mutations fail.

Non-identical conflicting proposals remain representable because probabilistic interpretation may be inconsistent. Patch 0009 does not silently rank, merge, rewrite, or resolve them.

Later deterministic State Authority owns that decision.

## 29. No hidden rewriting

Parser does not rewrite/summarize mutation text; merge proposals; repair contradictions; convert claim/belief/suspicion into knowledge/fact; auto-promote WorldState; auto-resolve UnresolvedProposition; infer Observation; or manufacture social consequences absent output.

## 30. Proposal is not authority

```text
StateInterpretationProposal != approved mutations
StateMutationCandidate != committed consequence
StateInterpretationProposal != accepted Take
```

Only later State Authority may approve/reject mutation candidates. Only later Take/commit authority may make accepted Performance + approved consequences causal history.

## 31. No direct mutation API

Patch 0009 exposes no operation that takes ProductionState and returns mutated State, allocates authoritative RecordIds, modifies fixture/Character/Relationship/Pressure/World state, writes history, or persists files.

Direct State mutation by Interpreter is an E0 hard-gate violation.

## 32. No truth promotion

Domain names describe proposed destination authority only if later approved.

- a claim about the raft does not justify WorldState by itself;
- a guess about Marlowe does not justify Knowledge;
- an explicit physical action may lead to a World/Scene proposal, but State Authority still checks locks, possibility, transition legality, and causality.

Parser has no truth-promotion authority.

## 33. Disposition/Relationship conservatism is downstream

Interpreter may propose Disposition/Relationship changes, but downstream State Authority must enforce rare/conservative durable Disposition change, avoid confusing immediate social effect with durable identity change, preserve semantic rather than numeric psychology, and enforce creator locks/transition rules.

No score/threshold is invented here.

## 34. Observation remains reserved

No CharacterObservation proposal exists. Memory/Belief/Suspicion proposals do not retroactively prove Observation.

## 35. World Resolver remains separate

WorldState/SceneState proposals here are consequences of interpreted Character Performance, not autonomous non-Character world evolution. Weather/scheduled/resource/possibility evolution without Character causation remains future World Resolver scope.

## 36. Interpreter input/provider disclosure remains open

Patch 0009 does not freeze exact Interpreter provider input.

Future input composition must minimize disclosure, preserve provider-attempt provenance, separate trusted State from untrusted Performance text, avoid treating dialogue as instruction, avoid assuming whole-Production disclosure, and preserve credentials/privacy.

State Interpreter is not a Character, so Character Access Control is not automatically its policy; a separate bounded orchestration disclosure contract is later.

## 37. E0 control isolation

Applies directly to per-Character Candidate/Integrity paths (A and compatible B/C/D/G). E0-F may inject malformed/stale/conflicting Interpreter output.

E0-E single-playwright control is not forced through this Candidate-specific parser. Its consequence protocol is later control-specific while still satisfying hard integrity/atomic-causality requirements.

## 38. Determinism

Identical valid Source + semantically equivalent valid JSON structure/content -> identical semantic proposal.

No clock/random/culture/filesystem/network/provider/AI/GPU/NPU/global mutable state affects parsing.

Property order/whitespace do not affect semantics. Mutation array order remains preserved. Evidence IDs canonicalize ordinally.

## 39. Failure domain

One small State Interpreter contract exception type.

Failure includes malformed Source, non-Accept/mismatched Integrity evaluation, malformed transport, unsupported schema/domain/operation, invalid field combination, invalid Character/Record IDs, invalid text, stale Candidate hash, or exact duplicate semantic mutation.

Failure creates no fictional fallback consequence, State mutation, retry permission, or Take disposition.

## 40. Required tests/review gates

Use canonical upstream Missing Raft Context/Candidate/Integrity construction. Do not add public bypass constructors for impossible authority states.

### Source binding
1. canonical Voss Context + Candidate + Patch 0008 Accept binds;
2. Source constructor non-public;
3. non-Accept fails;
4. fresh deterministic Reject fails;
5. evaluation/content hash mismatch fails;
6. evaluation/source Context identity mismatch fails;
7. unsupported Integrity ValidationContract fails;
8. synthetic Accept may bind but Source exposes no auth/authority flag;
9. Source public surface contains no Candidate text/private Context/Take/State/provider data;
10. roster canonical ordinal/exactly three;
11. impossible malformed upstream states handled by defensive/reflection review only.

### Proposal/transport
12. Proposal constructor non-public;
13. exact proposal fields only;
14. empty mutations valid;
15. representative valid mutation parses;
16. property order/whitespace insignificant;
17. candidate hash/identity mismatch fails;
18. unknown/missing/decoded-duplicate properties fail;
19. wrong types/trailing content/comments/trailing comma/BOM fail;
20. exact 1 MiB otherwise-valid envelope accepted; +1 rejected;
21. depth >8 rejected;
22. exception output does not echo mutation text/raw JSON.

### Semantic type safety
23. semantic base/variants/change variants constructors non-public;
24. Global semantic variant cannot carry Character IDs by shape;
25. Character semantic variant has one required SubjectCharacterId and no target by shape;
26. Relationship semantic variant has required subject+target by shape;
27. Claim semantic variant has no ExistingRecordId/Deactivate/Supersede shape;
28. Deactivate change has no Text property;
29. Add change has no ExistingRecordId property;
30. Supersede has both ExistingRecordId + Text;
31. parser cannot construct invalid variant/change combination.

### Domains/operations
32. every frozen E0 domain parses under valid rules;
33. no Constitution/HistoricalTruth/Observation/PresentationPerspective/Director domain exists;
34. non-roster Character target fails;
35. Relationship self-target fails;
36. Claim must be source Character/Add-only;
37. Add/Supersede/Deactivate transport combinations exact;
38. Delete/Replace operation strings rejected;
39. RecordId syntax validated but existence/authority not claimed;
40. Evidence ID duplicate fails and order canonicalizes;
41. exact duplicate semantic mutation fails;
42. conflicting non-identical mutations remain representable.

### Text/truth/authority
43. text exact/no trim/rewrite;
44. non-NFC/control/invisible-only text fails;
45. empty mutation list means no durable mutation only;
46. Claim remains distinct from WorldState/Knowledge;
47. Belief/Suspicion/Memory may differ from truth without parser rejection;
48. WorldState/Knowledge/Disposition proposal creates no authority;
49. no confidence/score/rationale;
50. no State apply/RecordId allocation;
51. no Take/Commit API;
52. no provider/retry/spend API;
53. no Director/opportunity mutation.

### Regression
54. Patch 0008 Candidate content hash oracle unchanged;
55. Missing Raft StructuredContextHash unchanged;
56. Missing Raft RenderedContextHash unchanged;
57. Missing Raft ECJ-1 9112 bytes/hash unchanged;
58. all existing 253 Core tests green;
59. Missing Raft Harness PASS/0;
60. smoke Harness PASS/0.

## 41. ARM64/battery suitability

Tiny deterministic CPU work only: bounded UTF-8 JSON parsing, enum/ID validation, Unicode checks, immutable-array canonicalization.

No network/provider/background/AI/GPU/NPU/filesystem/polling. No NPU claim.

## 42. Explicit exclusions

No Interpreter provider call/input composer; assessor/provider auth; provider-attempt persistence; retry/spend/cancellation execution; provisional/accepted/rejected/alternate Take semantics; State Authority; ProductionState/StateHash; authoritative RecordId allocation; mutation application; atomic Performance+consequence commit; causal persistence/recovery; effective opportunity mutation/history append; Scene loop; full Observation; World Resolver; E0-E control protocol; post-E0 ontology; final Studio/Archive mutation UX; WinUI; Windows AI/NPU; packaging/WACK/Store.

## 43. Recursive audit dimensions

Restart after every material correction and test:

1. State Interpreter vs State Authority;
2. Integrity -> Interpreter sequencing;
3. Integrity Accept != accepted Take;
4. provisional-Take ordering remains open;
5. creator-ontology guard;
6. E0 category fidelity;
7. Constitution read-only;
8. truth/claim/knowledge/belief/suspicion/memory separation;
9. Observation boundary;
10. World Resolver separation;
11. historical texture vs durable consequence;
12. append-only/supersession semantics;
13. semantic invalid-state representability;
14. proposal vs authority;
15. creator locks deferred to State Authority;
16. Disposition/Relationship conservatism;
17. Candidate content binding;
18. Integrity structural binding vs assessor auth;
19. evidence/provenance sufficiency;
20. stale/hallucinated RecordId non-authority;
21. least-privilege public surface;
22. untrusted text/hidden rewrite;
23. transport strictness/resource bounds;
24. deterministic ordering;
25. conflicting proposal handling;
26. E0 control compatibility/isolation;
27. experimental provenance reconstruction;
28. API non-forgeability/minimality;
29. executable-vs-defensive testability;
30. Hygiene Constitution;
31. ARM64 suitability;
32. scope/validation claims.

Approval only after a complete restart finds zero material correction or worthwhile improvement.

## 44. Material approval decisions

Approval would freeze only:

1. Patch 0009 is State Interpreter proposal schema/parser only;
2. Source Bind structurally requires exact Patch 0008 Accept evaluation but authenticates neither assessor provenance nor Take status;
3. provisional-Take ordering remains open;
4. semantic contract `ensemble.e0.state-interpreter.proposal.v1`;
5. JSON schema `ensemble.e0.state-interpreter.proposal-json.v1`;
6. Source exposes Candidate content identity, source Character/Context IDs, canonical three-Character roster, Integrity ValidationContract only;
7. empty mutation list valid = no durable projected mutation proposed;
8. E0 domains: WorldState, SceneState, UnresolvedProposition, CharacterKnowledge, CharacterBelief, CharacterSuspicion, CharacterMemory, CharacterGoal, CharacterDisposition, CharacterCircumstance, CharacterClaim, Relationship, Pressure;
9. domains are E0 authority vocabulary, not final creator-facing/post-E0 ontology;
10. Constitution/HistoricalTruth/Observation/PresentationPerspective/Director opportunity absent;
11. semantic mutations use typed Global/Character/Claim/Relationship variants and typed Add/Supersede/Deactivate changes, preventing invalid nullable discriminator states;
12. no destructive Delete/silent Replace;
13. Claim is source-Character Add-only and cannot be rewritten;
14. ExistingRecordId/EvidenceRecordIds are syntactic proposal references only;
15. Candidate hash is root content binding and not attempt/Take/commit identity;
16. mutation text exact/NFC/display-bearing where required; no rewrite;
17. Evidence IDs duplicate-free/canonical/may-empty and imply no auth/disclosure claim;
18. mutation array order preserved for reconstruction but confers no commit priority;
19. exact duplicate semantic mutations fail; conflicting non-identical proposals remain for State Authority;
20. strict parser uses 1 MiB inclusive, depth 8, strict unknown/missing/duplicate/type/trailing rules;
21. no confidence/rationale/score/provider/Take/State/commit/Director authority fields;
22. proposal domain names never themselves create truth/knowledge/durable change;
23. Observation and autonomous World evolution remain separate future authority paths;
24. E0-E not forced through Candidate-specific schema;
25. later State Authority owns existence/domain/locks/transitions/staleness/conflict/causal-evidence/conservative-change review;
26. post-E0 ontology remains open under Creator Ontology Extensibility Guard;
27. no provider, Take, State Authority, commit, persistence, Scene-loop, UI, Windows AI/NPU, or Store scope enters Patch 0009.

Implementation remains blocked until recursive audit completes and the user explicitly approves the final proposal.
