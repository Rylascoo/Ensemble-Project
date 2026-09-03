# H1 Patch 0009 — E0 State Interpreter Mutation-Proposal Contract

Status: blueprint proposal 0.6 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
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

Patch 0009 defines only the E0 semantic State Interpreter mutation-proposal vocabulary, strict E0 AI JSON proposal parser, exact Candidate/Scene/Integrity structural binding, and bounded structural supporting-record references for later State Authority review.

It does **not** call a model/provider, authenticate semantic-review provenance, allocate a Take, decide provisional-Take timing, approve/reject mutations, mutate Production state, commit history, persist anything, or trigger another Performer.

## 2. Recovered frozen authority

Frozen sequence:

1. Performer candidate output;
2. Director opportunity selection;
3. Integrity Validator;
4. **State Interpreter candidate-mutation schema**;
5. deterministic State Authority;
6. accepted/rejected/alternate Take semantics;
7. atomic causal commit.

Frozen law:

- probabilistic systems propose; deterministic authority decides;
- `Integrity Validator -> State Interpreter -> deterministic State Authority`;
- Interpreter proposes meaning/consequences and never directly mutates authority;
- State Authority later decides under locks, type/transition rules, review policy, and causal evidence;
- accepted Performance + approved consequences later form one atomic causal commit;
- statement != fact; possibility != fact;
- truth, observation, claim, belief, memory, rumor, unresolved proposition remain distinct;
- Constitution read-only in E0;
- Disposition mutation rare/conservative;
- Circumstance comparatively fluid;
- locked canon cannot change;
- technical failure cannot become fiction;
- Observation and World Resolver remain reserved/outside E0;
- E0 preserves proposed Interpreter mutations plus committed/rejected mutation decisions.

Blueprint 0.1 also excludes complete Scene Consolidation / memory-forgetting design from E0 and leaves selective/false memory behavior open in ODR-18.

Patch 0008 freezes Integrity Accept as evaluation only, synthetic Accept testability, later review-provenance authentication, open provisional-Take/Interpreter ordering, and no authority-bearing eligibility token.

## 3. Why schema/parser comes before provider execution

The frozen next requirement is candidate-mutation **schema**, not provider integration. Provider execution would prematurely require Interpreter disclosure composition, provider-attempt provenance, concern-review authentication, Take timing, retry/cost policy, ProductionState/StateHash, and persistence.

## 4. Creator Ontology Extensibility Guard revisit

E0 decision:

> Use an explicitly E0-scoped authority vocabulary containing only distinctions the frozen experiment needs. Do not treat it as final creator-facing or post-E0 storage ontology.

UI labels remain projections; creator dramatic concepts remain open-ended; no speculative meta-ontology is introduced; post-E0 Production/Studio ontology remains open.

## 5. No provisional-Take ordering decision

Patch 0009 freezes only that a structurally matching Integrity Accept evaluation exists before `StateInterpretationSource.Bind` succeeds. It does not decide when a provisional/accepted Take object exists.

## 6. Contracts

```text
StateInterpretationContractVersion = ensemble.e0.state-interpreter.proposal.v1
StateInterpretationJsonSchemaVersion = ensemble.e0.state-interpreter.proposal-json.v1
```

Candidate identity remains `ensemble.e0.integrity.candidate-content.v1`.

## 7. Structurally bound source

```text
StateInterpretationSource.Bind(
    ContextPacket sourceContext,
    CandidatePerformance sourceCandidate,
    IntegrityValidationEvaluation integrityEvaluation)
    -> StateInterpretationSource
```

Only Patch 0009 rich-object binding boundary.

`Bind` enforces only the Patch 0009 cross-boundary facts needed to associate the supplied Context/Candidate with the supplied Patch 0008 evaluation. It does not become a second Integrity Validator.

## 8. Source public shape

```text
StateInterpretationSource
- CandidateContentIdentityContract
- CandidateContentHash
- SourceSceneId
- SourceCharacterId
- SourceContextPacketId
- RosterCharacterIds
```

No public constructor. `SourceSceneId` copies exact `ContextPacket.SceneId`. Roster IDs canonical ordinal.

No Candidate prose, private Context prose, rendering/access audit, Integrity evaluation/evidence, provider/model, Take, State mutation, or authority flag.

The Source is a narrow parser/binding input, not an eligibility/provenance token.

## 9. Source Bind invariants

Fail closed unless:

1. sourceContext/sourceCandidate/integrityEvaluation are non-null;
2. Context SceneId, subject, opportunity, ContextPacketId, and roster IDs are initialized;
3. Context subject == Context opportunity;
4. a fresh canonical `IntegrityCandidateInput.Bind(sourceContext, sourceCandidate)` succeeds;
5. fresh Integrity input has zero deterministic Reject codes;
6. `integrityEvaluation.Disposition == Accept`;
7. evaluation Trace is non-null and `ValidationContract` exactly equals the current Patch 0008 ValidationContract;
8. evaluation Trace Input is non-null and its CandidateContentIdentityContract, CandidateContentHash, and SourceContextPacketId exactly match the fresh Integrity input;
9. source Context roster is exactly three unique initialized E0 Characters containing source once.

Patch 0009 deliberately does **not** inspect/revalidate `IntegrityConcernEvidence`, concern ordering, evidence contract, or Accept disposition derivation. Patch 0008 remains the single canonical implementation of those invariants.

Impossible internally forged/malformed Integrity evaluations may be covered by defensive/static/reflection review without adding a second production validation path or public bypass constructors.

## 10. Structural Accept binding is not authentication

Bound Source means only that normal non-forgeable Patch 0008 output reports Accept for the exact Candidate/source content association.

It does not prove configured concern review/provider attempt, accepted Take, provider-spend permission, or State authority. Synthetic Patch 0008 Accept may bind for tests. Effective orchestration later authenticates review/provider provenance.

Because Source does not contain the Integrity evaluation or ValidationContract, it cannot masquerade as proof that Integrity review occurred. Canonical Integrity provenance remains a separate retained object/record.

## 11. Semantic proposal

```text
StateInterpretationProposal
- ContractVersion
- CandidateContentIdentityContract
- CandidateContentHash
- SourceSceneId
- Mutations
```

No public constructor; strict parser is E0 public construction.

CandidateContentIdentityContract, CandidateContentHash, and SourceSceneId must match Source exactly.

No Take/Commit ID, authority disposition, provider/model, confidence, free-form rationale, Director/current-opportunity data, or authoritative new RecordId.

SourceSceneId is structural causal scope only. It does not mean the proposal is accepted into that Scene.

## 12. Why Scene identity is explicit

SceneState is Scene-scoped and later State Authority must know which Scene a proposal concerns without reverse-mapping an opaque Context/Candidate hash.

E0 has one Scene, but explicit SourceSceneId prevents ambient-state inference and improves reconstruction without introducing multi-Scene persistence or cross-Scene mutation.

## 13. Empty mutation proposal is valid

`Mutations = []` means no durable projected-state mutation proposed. It does not mean rejection or erase accepted historical texture. Performance-history acceptance remains later Take/commit authority.

## 14. E0 proposal domains

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

E0 authority targets only, not final ontology.

## 15. Domain meanings

- **WorldState** — possible Character-caused durable world consequence; not truth merely because proposed.
- **SceneState** — possible current Source Scene consequence; not autonomous World Resolver.
- **UnresolvedProposition** — preserves uncertainty.
- **CharacterKnowledge** — proposed new knowledge; later authority prevents claim/guess promotion.
- **CharacterBelief** — proposed belief, potentially false.
- **CharacterSuspicion** — proposed uncertainty/suspicion.
- **CharacterMemory** — proposed new recollection; may differ from truth.
- **CharacterGoal** — proposed current goal change.
- **CharacterDisposition** — proposed persistent tendency; downstream conservative.
- **CharacterCircumstance** — proposed fluid immediate state.
- **CharacterClaim** — interpreted proposition representing what the source Character is understood to have claimed; never a verbatim quotation and never fact merely because proposed.
- **Relationship** — directional semantic relationship state/effect; no numeric psychology.
- **Pressure** — current/durable pressure without enumerating every dramatic concept.

## 16. CharacterClaim is an interpreted proposition, not a quotation

`CharacterClaimMutationCandidate.Text` is an interpreted proposition attributable to the source Character's Performance, not transcript text.

Exact wording remains Candidate/accepted Performance history. Future UI/provenance must distinguish interpreted proposition from verbatim Performance. Claim proposals cannot rewrite accepted Performance history and never become objective truth by parsing.

## 17. Absent domains

No Constitution, HistoricalTruth/accepted Performance history, Observation, direct locked-Canon domain, PresentationPerspective, or Director opportunity mutation.

## 18. Typed change union

```text
StateMutationChange
    AddStateMutationChange(Text)
    SupersedeStateMutationChange(ExistingRecordId, Text)
    DeactivateStateMutationChange(ExistingRecordId)
```

No public constructors. Parser/internal builder only. No Delete/silent Replace.

## 19. Typed mutation union

```text
StateMutationCandidate
    GlobalStateMutationCandidate(Domain, Change, SupportingRecordIds)
    AppendOnlyCharacterStateMutationCandidate(Domain, SubjectCharacterId, AddStateMutationChange, SupportingRecordIds)
    MutableCharacterStateMutationCandidate(Domain, SubjectCharacterId, Change, SupportingRecordIds)
    CharacterClaimMutationCandidate(SubjectCharacterId, Text, SupportingRecordIds)
    RelationshipStateMutationCandidate(SubjectCharacterId, TargetCharacterId, Change, SupportingRecordIds)
```

No public constructors. Flat AI transport converts into immutable semantic variants.

SupportingRecordIds are Interpreter-declared structural support references, not authenticated evidence or authority.

## 20. Domain membership and operation restrictions

### Global
WorldState, SceneState, UnresolvedProposition, Pressure; any Add/Supersede/Deactivate candidate.

### Append-only Character
CharacterKnowledge, CharacterMemory; roster subject; **Add only**.

This preserves the E0 exclusion/open design around forgetting, unlearning, memory deletion, and memory rewrite. A corrective recollection can be a new Memory proposal rather than rewriting earlier memory history.

### Mutable Character
CharacterBelief, CharacterSuspicion, CharacterGoal, CharacterDisposition, CharacterCircumstance; roster subject; Add/Supersede/Deactivate candidate.

### CharacterClaim
Source Character only; Add-only; Text required; no ExistingRecordId.

### Relationship
Distinct Source Scene roster subject + target; any typed Change candidate.

## 21. ExistingRecordId semantics

Only Supersede/Deactivate contain ExistingRecordId. Parser validates syntax only—not existence, domain, ownership, active state, lock status, or causal fitness. State Authority later validates those.

## 22. Mutation text

Add/Supersede/Claim Text is non-null/non-empty, exact, already NFC, no trim/repair, LF/TAB allowed, other Control scalars rejected, and includes one display-bearing scalar under Patch 0006 category principle. Text is untrusted creative interpretation, not instruction authority. Deactivate has no Text semantic property.

## 23. SupportingRecordIds

Every semantic mutation carries immutable SupportingRecordIds:

- syntactically valid;
- duplicate-free;
- canonical ordinal order;
- may be empty because Candidate content is root causal source;
- may reference supporting state future Interpreter input disclosed;
- does not prove existence, actual disclosure, relevance, sufficiency, or authority.

Later provider/orchestration provenance establishes actual Interpreter disclosure. State Authority validates references and causal suitability. No free-form rationale, quote, score, confidence, or hidden reasoning.

## 24. Candidate content root binding

Proposal identity contract/hash must match Source. Candidate hash remains content identity only—not provider attempt, Take, or commit identity.

## 25. Flat AI JSON transport

```json
{
  "schemaVersion": "ensemble.e0.state-interpreter.proposal-json.v1",
  "candidateContentIdentityContract": "ensemble.e0.integrity.candidate-content.v1",
  "candidateContentHash": "<64 lowercase hex>",
  "sourceSceneId": "SCENE-MISSING-RAFT",
  "mutations": [
    {
      "domain": "characterBelief",
      "operation": "add",
      "subjectCharacterId": "VOSS",
      "targetCharacterId": null,
      "existingRecordId": null,
      "text": "Voss now believes ...",
      "supportingRecordIds": []
    }
  ]
}
```

All root properties and all seven mutation properties required with explicit nulls where absent. Flat transport; typed Core semantics.

## 26. Transport strings

Domains: `worldState | sceneState | unresolvedProposition | characterKnowledge | characterBelief | characterSuspicion | characterMemory | characterGoal | characterDisposition | characterCircumstance | characterClaim | relationship | pressure`

Operations: `add | supersede | deactivate`. Exact case; no aliases.

## 27. Transport-to-semantic rules

- sourceSceneId exactly equals Source.SourceSceneId;
- Global: subject/target null;
- Knowledge/Memory: roster subject, target null, add only, existingRecordId null, valid Text;
- Mutable Character: roster subject, target null, exact change combination;
- Relationship: distinct Source Scene roster subject+target, exact change;
- CharacterClaim: add; subject == SourceCharacter; target/existing null; valid Text;
- Add -> existing null + Text;
- Supersede -> ExistingRecordId + Text;
- Deactivate -> ExistingRecordId + text null.

Invalid combinations fail before semantic construction.

## 28. Strict parser

```text
StateInterpretationContract.ParseJson(StateInterpretationSource source, ReadOnlySpan<byte> utf8Proposal)
    -> StateInterpretationProposal
```

- trusted Source validated first;
- non-empty UTF-8; BOM rejected;
- max 1,048,576 bytes inclusive;
- max depth 8;
- comments/trailing commas/malformed UTF-8/JSON rejected;
- decoded duplicate properties rejected at every object;
- unknown/missing properties rejected;
- exact names/case/enums/types;
- trailing content rejected;
- Candidate identity + Scene identity match Source;
- Character IDs satisfy Source Scene roster/domain rules;
- RecordIds syntax checked;
- SupportingRecordIds duplicates rejected then ordinal canonicalized;
- exact duplicate semantic mutations rejected after support-ID canonicalization;
- sanitized exceptions do not echo mutation Text, arbitrary unknown names/values, raw snippets, provider content.

No provider call.

## 29. Mutation ordering

Array order preserved for reconstruction but confers no State Authority priority or sequential-application semantics. State Authority later defines whole-batch conflict/staleness behavior.

## 30. Duplicates/conflicts

Exact semantic duplicates fail. Non-identical conflicts remain representable; Patch 0009 does not rank/merge/repair. State Authority later decides.

## 31. No hidden rewriting

No text rewrite/summarization, proposal merging, contradiction repair, claim/belief/suspicion->knowledge/fact conversion, WorldState promotion, UnresolvedProposition resolution, Observation inference, or manufactured social effects.

CharacterClaim interpreted proposition is a distinct proposal type, not a rewrite of Performance history.

## 32. Proposal is not authority

`StateInterpretationProposal != approved mutation != committed consequence` and `StateInterpretationProposal != accepted Take`.

Only later State Authority may approve/reject. Only later Take/commit authority makes Performance + approved consequences causal history.

## 33. No direct mutation API

No ProductionState-in/ProductionState-out, authoritative RecordId allocation, fixture/Character/Relationship/Pressure/World mutation, history write, or persistence. Direct Interpreter mutation is an E0 hard-gate violation.

## 34. No truth promotion

Domain names describe proposed destinations only if later approved. Claim does not justify WorldState; guess does not justify Knowledge; explicit physical action may justify a World/Scene proposal but State Authority still checks locks/possibility/transitions/causality.

## 35. Disposition/Relationship conservatism downstream

Interpreter may propose these domains; State Authority later enforces rare/conservative Disposition change, immediate-vs-durable distinction, semantic nonnumeric relationship representation, locks, and transitions. No score/threshold here.

## 36. Observation reserved

No CharacterObservation proposal. Memory/Belief/Suspicion never retroactively prove Observation.

## 37. World Resolver separate

World/Scene proposals are interpreted consequences of Character Performance, not autonomous world evolution. Weather/scheduled/resource/non-Character possibility evolution remains future World Resolver.

## 38. Interpreter input/provider disclosure open

Patch 0009 does not freeze Interpreter input. Future composition minimizes disclosure, preserves attempt provenance, separates trusted state from untrusted Performance, does not treat dialogue as instruction, avoids whole-Production disclosure by default, and preserves credentials/privacy. State Interpreter is not a Character; separate bounded disclosure contract is later.

SupportingRecordIds are optional so output schema does not force internal RecordId disclosure.

## 39. E0 controls

Directly applies to per-Character Candidate/Integrity paths (A and compatible B/C/D/G). E0-F may inject failures. E0-E playwright is not forced through this Candidate-specific parser; its consequence protocol is later control-specific while satisfying hard integrity/causality.

## 40. Determinism

Identical valid Source + semantically equivalent JSON structure/content -> identical semantic proposal. No clock/random/culture/filesystem/network/provider/AI/GPU/NPU/global mutable state. Property order/whitespace insignificant; mutation order preserved; supporting IDs canonicalized.

## 41. Failure domain

One small State Interpreter exception domain. Malformed Source, non-Accept/mismatch, bad transport/schema/domain/operation/field combination/ID/text/hash/Scene mismatch/duplicate -> exception. Failure creates no fictional consequence, State mutation, retry authorization, or Take disposition.

## 42. Required tests/review gates

Use canonical upstream construction; no public test bypasses.

### Source
1. canonical Voss Context/Candidate/Patch 0008 Accept binds;
2. Source constructor non-public;
3. non-Accept/reject/mismatch/unsupported validation contract fail during Bind;
4. Source Bind does not duplicate concern-evidence validation;
5. synthetic Accept may bind but no auth/authority flag;
6. Source public surface contains SceneId but excludes Integrity evaluation/contract, prose/Take/State/provider;
7. SourceSceneId exactly equals ContextPacket.SceneId;
8. roster canonical exactly three;
9. impossible upstream malformed states defensive/reflection review.

### Proposal/transport
10. Proposal constructor non-public/exact fields including SourceSceneId;
11. empty mutations valid;
12. representative mutation parses;
13. property order/whitespace insignificant;
14. candidate identity mismatch fails;
15. sourceSceneId mismatch fails;
16. unknown/missing/decoded-duplicate/wrong type/trailing/comments/comma/BOM fail;
17. 1 MiB inclusive valid envelope; +1 fails;
18. depth >8 fails;
19. errors do not echo creative text/raw JSON.

### Semantic type safety
20. all semantic constructors non-public;
21. Global cannot carry Character IDs by shape;
22. append-only Character exposes Add only;
23. mutable Character has subject/no target;
24. Relationship has required subject+target;
25. Claim has no existing/change-operation shape;
26. Deactivate has no Text; Add no ExistingRecordId; Supersede has both;
27. parser cannot construct invalid variant/change combination.

### Domains/operations
28. all E0 domains valid under exact rules;
29. Knowledge/Memory reject supersede/deactivate;
30. no Constitution/HistoricalTruth/Observation/PresentationPerspective/Director domain;
31. non-roster target/self-relationship fail;
32. Claim source-only/Add-only;
33. Delete/Replace rejected;
34. RecordId syntax does not claim existence/authority;
35. Supporting ID duplicate fails/order canonicalizes;
36. exact duplicate mutation fails; non-identical conflict representable.

### Claim/text/truth/authority
37. CharacterClaim Text interpreted proposition, not verbatim quote;
38. Performance exact words remain outside proposal;
39. exact mutation text/no rewrite; non-NFC/control/invisible-only fail;
40. empty list means no durable mutation only;
41. Claim distinct from WorldState/Knowledge;
42. Belief/Suspicion/Memory may diverge from truth;
43. WorldState/Knowledge/Disposition proposal creates no authority;
44. no memory forgetting/unlearning proposal shape;
45. no confidence/score/rationale;
46. no State apply/RecordId allocation;
47. no Take/Commit/provider/retry/spend/Director mutation APIs.

### Regression
48. Patch 0008 Candidate hash oracle unchanged;
49. Missing Raft Structured/Rendered Context hashes unchanged;
50. Missing Raft ECJ-1 9112 bytes/hash unchanged;
51. existing 253 Core tests green;
52. Missing Raft Harness PASS/0;
53. smoke Harness PASS/0.

## 43. ARM64/battery

Tiny deterministic bounded parsing/ID/Unicode/immutable-array work. No network/provider/background/AI/GPU/NPU/filesystem/polling. No NPU claim.

## 44. Explicit exclusions

No Interpreter provider/input composer; review/provider authentication; provider-attempt persistence; retries/spend/cancellation; provisional/accepted/rejected/alternate Take semantics; State Authority; ProductionState/StateHash; authoritative RecordId allocation; mutation application; atomic causal commit; persistence/recovery; effective opportunity/history append; Scene loop; memory-forgetting design; full Observation; World Resolver; E0-E control protocol; multi-Scene persistence; post-E0 ontology; final mutation UX; WinUI/Windows AI/NPU/packaging/WACK/Store.

## 45. Recursive audit dimensions

Restart after each material correction:

1. Interpreter vs State Authority;
2. Integrity sequencing/Accept non-authority;
3. one canonical Patch 0008 Integrity implementation;
4. Take ordering open;
5. creator-ontology guard;
6. E0 category fidelity;
7. Constitution read-only;
8. truth/claim/knowledge/belief/suspicion/memory separation;
9. claim proposition vs verbatim Performance provenance;
10. memory-forgetting ODR preservation;
11. Observation boundary;
12. World Resolver;
13. Scene scoping;
14. historical texture vs durable consequence;
15. append-only/supersession semantics;
16. invalid semantic state representability;
17. proposal vs authority;
18. locks/conservative change downstream;
19. Candidate content/Integrity structural binding vs auth;
20. supporting-record/provenance sufficiency;
21. RecordId non-authority;
22. least privilege;
23. untrusted text/no rewrite;
24. strict transport/resource bounds;
25. deterministic ordering/conflicts;
26. E0 control isolation;
27. experimental reconstruction;
28. API minimality/non-forgeability;
29. testability;
30. Hygiene Constitution;
31. ARM64;
32. scope/validation claims.

Approval only after a complete restart finds zero material correction/worthwhile improvement.

## 46. Material approval decisions

Approval would freeze only:

1. Patch 0009 schema/parser only;
2. Source Bind requires exact Patch 0008 Accept association but does not reimplement Patch 0008 concern validation or authenticate assessor/Take;
3. Source deliberately does not retain Integrity evaluation/ValidationContract; canonical Integrity provenance stays separate;
4. Take ordering remains open;
5. semantic `ensemble.e0.state-interpreter.proposal.v1` and JSON `ensemble.e0.state-interpreter.proposal-json.v1`;
6. Source carries Candidate content identity, SourceSceneId, source Character/Context IDs, canonical roster only;
7. Proposal carries SourceSceneId so Scene-scoped consequences are explicit without hash reverse lookup;
8. empty mutations valid = no durable projected mutation proposed;
9. E0 domains exactly WorldState, SceneState, UnresolvedProposition, CharacterKnowledge, CharacterBelief, CharacterSuspicion, CharacterMemory, CharacterGoal, CharacterDisposition, CharacterCircumstance, CharacterClaim, Relationship, Pressure;
10. E0 domains do not freeze final ontology;
11. Constitution/HistoricalTruth/Observation/PresentationPerspective/Director absent;
12. typed variants prevent invalid global/append-only-character/mutable-character/claim/relationship states;
13. typed Add/Supersede/Deactivate; no Delete/Replace;
14. Knowledge/Memory Add-only, preserving unresolved forgetting/unlearning design;
15. CharacterClaim source-only Add interpreted proposition, not verbatim quotation/history replacement;
16. ExistingRecordId/SupportingRecordIds syntactic proposal references only;
17. Candidate hash content binding is not attempt/Take/commit identity;
18. text exact/NFC/display-bearing; no parser rewrite;
19. SupportingRecordIds canonical/duplicate-free/may-empty and imply no evidence/auth/disclosure authority;
20. mutation order preserved for reconstruction, no commit priority;
21. exact duplicates fail; non-identical conflicts remain State Authority problem;
22. parser 1 MiB inclusive/depth 8/strict structure;
23. no scores/rationale/provider/Take/State/commit/Director authority fields;
24. domain names do not create truth/knowledge/durable change;
25. Observation/autonomous World evolution separate;
26. E0-E not forced through Candidate-specific schema;
27. later State Authority owns existence/domain/locks/transitions/stale/conflict/evidence/conservative change;
28. post-E0 ontology remains open;
29. no provider, Take, State Authority, commit, persistence, Scene loop, UI, Windows AI/NPU, Store scope.

Implementation remains blocked until recursive audit completes and user explicitly approves the final proposal.
