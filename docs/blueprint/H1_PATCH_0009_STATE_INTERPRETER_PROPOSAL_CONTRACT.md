# H1 Patch 0009 — E0 State Interpreter Mutation-Proposal Contract

Status: blueprint proposal 0.3 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
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

Patch 0009 defines only the E0 semantic State Interpreter mutation-proposal vocabulary, strict E0 AI JSON proposal parser, exact Candidate-content/Integrity structural binding, and bounded structural evidence references for later State Authority review.

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

## 8. Source public shape

```text
StateInterpretationSource
- CandidateContentIdentityContract
- CandidateContentHash
- SourceCharacterId
- SourceContextPacketId
- RosterCharacterIds
- IntegrityValidationContract
```

No public constructor. Roster IDs canonical ordinal. No Candidate prose, private Context prose, rendering/access audit, provider/model, Take, State mutation, or authority flag.

## 9. Source Bind invariants

Fail closed unless:

1. upstream objects non-null/structurally initialized;
2. Context subject == Context opportunity;
3. fresh `IntegrityCandidateInput.Bind` succeeds;
4. fresh Integrity input has zero Reject codes;
5. evaluation Disposition == Accept;
6. evaluation ValidationContract exact current Patch 0008 contract;
7. evaluation Input identity contract/hash/source Context identity exactly matches fresh input;
8. concern evidence exists, matches same Candidate content, and contains zero concerns;
9. source Context roster is exactly three unique initialized E0 Characters containing source once.

Do not duplicate Patch 0006 Candidate validation or Patch 0008 concern semantics.

## 10. Structural Accept binding is not authentication

Bound Source means structural consistency only. It does not prove configured concern review/provider attempt, accepted Take, provider-spend permission, or State authority. Synthetic Accept may bind for tests. Effective orchestration later authenticates review/provider provenance.

## 11. Semantic proposal

```text
StateInterpretationProposal
- ContractVersion
- CandidateContentIdentityContract
- CandidateContentHash
- Mutations
```

No public constructor; strict parser is E0 public construction. No Take/Commit ID, authority disposition, provider/model, confidence, free-form rationale, Director/current-opportunity data, or authoritative new RecordId.

## 12. Empty mutation proposal is valid

`Mutations = []` means no durable projected-state mutation proposed. It does not mean rejection or erase accepted historical texture. Performance-history acceptance remains later Take/commit authority.

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

E0 authority targets only, not final ontology.

## 14. Domain meanings

- WorldState — possible Character-caused durable world consequence; not truth merely because proposed.
- SceneState — possible current Scene consequence; not autonomous World Resolver.
- UnresolvedProposition — preserves uncertainty.
- CharacterKnowledge — proposed new knowledge; later authority prevents claim/guess promotion.
- CharacterBelief — proposed belief, potentially false.
- CharacterSuspicion — proposed uncertainty/suspicion.
- CharacterMemory — proposed new recollection; may differ from truth.
- CharacterGoal — proposed current goal change.
- CharacterDisposition — proposed persistent tendency; downstream conservative.
- CharacterCircumstance — proposed fluid immediate state.
- CharacterClaim — structured source-Character claim; claim != fact.
- Relationship — directional semantic relationship state/effect; no numeric psychology.
- Pressure — current/durable pressure without enumerating every dramatic concept.

## 15. Absent domains

No Constitution, HistoricalTruth/accepted Performance history, Observation, direct locked-Canon domain, PresentationPerspective, or Director opportunity mutation.

## 16. Typed change union

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

No public constructors. Parser/internal builder only.

Add introduces a new candidate record. Supersede explicitly replaces current semantic projection while preserving historical trace. Deactivate ends current projection while preserving history. No Delete/silent Replace.

## 17. Typed mutation union

```text
StateMutationCandidate

    GlobalStateMutationCandidate
        Domain
        Change
        EvidenceRecordIds

    AppendOnlyCharacterStateMutationCandidate
        Domain
        SubjectCharacterId
        AddStateMutationChange
        EvidenceRecordIds

    MutableCharacterStateMutationCandidate
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

No public constructors. Flat AI transport converts into these immutable semantic variants.

## 18. Domain membership and operation restrictions

### GlobalStateMutationCandidate

Domains:

```text
WorldState
SceneState
UnresolvedProposition
Pressure
```

Any Add/Supersede/Deactivate change may be proposed; later authority decides legality.

### AppendOnlyCharacterStateMutationCandidate

Domains exactly:

```text
CharacterKnowledge
CharacterMemory
```

Requires roster SubjectCharacterId and **Add only**.

Reason: E0 may model learning/new recollection, but Patch 0009 must not silently freeze forgetting, unlearning, memory deletion, or memory-rewrite semantics that Blueprint 0.1 explicitly leaves outside E0/ODR-18.

A later contradictory/corrective recollection may be a new Memory proposal; it does not overwrite the historical existence of the earlier memory record in Patch 0009.

### MutableCharacterStateMutationCandidate

Domains:

```text
CharacterBelief
CharacterSuspicion
CharacterGoal
CharacterDisposition
CharacterCircumstance
```

Requires roster SubjectCharacterId; any Add/Supersede/Deactivate change may be proposed. State Authority later controls legality and conservatism.

### CharacterClaimMutationCandidate

- implicit CharacterClaim domain;
- source Character only;
- Add-only;
- Text required;
- no ExistingRecordId semantic field.

Claims are historical assertions. Later contradictory statements are additional claims, never rewritten prior claims.

### RelationshipStateMutationCandidate

- implicit Relationship domain;
- distinct roster subject + target;
- any typed Change candidate;
- downstream authority controls durability.

## 19. ExistingRecordId semantics

Only Supersede/Deactivate contain ExistingRecordId. Parser validates syntax only—not existence, domain, ownership, active state, lock status, or causal fitness. State Authority later validates those.

## 20. Mutation text

Add/Supersede/Claim Text is non-null/non-empty, exact, already NFC, no trim/repair, LF/TAB allowed, other Control scalars rejected, and must include one display-bearing scalar under Patch 0006 category principle. Text is untrusted creative interpretation, not instruction authority. Deactivate semantic type has no Text property.

## 21. EvidenceRecordIds

Every semantic mutation carries immutable EvidenceRecordIds:

- syntactically valid;
- duplicate-free;
- canonical ordinal order;
- may be empty because Candidate content is root causal source;
- may reference supporting state future Interpreter input disclosed;
- does not prove existence/disclosure/relevance/authority.

Later provider provenance establishes actual disclosure. State Authority validates references. No free-form rationale, quote, score, confidence, or hidden reasoning.

## 22. Candidate content root binding

Proposal identity contract/hash must exactly match Source. Candidate hash remains content identity only—not provider attempt, Take, or commit identity.

## 23. Flat AI JSON transport

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

All seven mutation properties required with explicit nulls where absent. Transport stays flat for AI simplicity; semantic Core representation is typed.

## 24. Transport strings

Domains:

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

Operations: `add | supersede | deactivate`. Exact case; no aliases.

## 25. Transport-to-semantic rules

- Global domains: subject/target null.
- Knowledge/Memory: roster subject, target null, operation must add, existingRecordId null, Text valid.
- Mutable Character domains: roster subject, target null, change combination exact.
- Relationship: distinct roster subject+target, change exact.
- CharacterClaim: operation add; subject == source; target/existingRecordId null; Text valid.
- Add -> existingRecordId null + Text -> AddStateMutationChange.
- Supersede -> ExistingRecordId + Text -> SupersedeStateMutationChange.
- Deactivate -> ExistingRecordId + text null -> DeactivateStateMutationChange.

Invalid combinations fail before semantic construction.

## 26. Strict parser

```text
StateInterpretationContract.ParseJson(
    StateInterpretationSource source,
    ReadOnlySpan<byte> utf8Proposal)
    -> StateInterpretationProposal
```

- validate trusted Source first;
- non-empty UTF-8; BOM rejected;
- max 1,048,576 bytes inclusive;
- max depth 8;
- comments/trailing commas/malformed UTF-8/JSON rejected;
- decoded duplicate properties rejected at every object;
- unknown/missing properties rejected;
- exact names/case/enums/types;
- trailing content rejected;
- Candidate identity must match Source;
- Character IDs must satisfy roster/domain rules;
- RecordIds syntax checked;
- Evidence duplicates rejected then ordinal canonicalization;
- exact duplicate semantic mutations rejected after evidence canonicalization;
- sanitized exceptions do not echo mutation Text, unknown names/values, raw snippets, provider content.

No provider call.

## 27. Mutation array ordering

Array order preserved for reconstruction but confers no State Authority priority/sequential application. State Authority later defines whole-batch conflict/staleness behavior.

## 28. Duplicates/conflicts

Exact duplicate semantic mutations fail. Non-identical conflicts remain representable; Patch 0009 does not rank/merge/repair. State Authority later decides.

## 29. No hidden rewriting

No text rewrite/summarization, proposal merging, contradiction repair, claim/belief/suspicion->knowledge/fact conversion, WorldState promotion, UnresolvedProposition resolution, Observation inference, or manufactured social effects.

## 30. Proposal is not authority

`StateInterpretationProposal != approved mutation != committed consequence` and `StateInterpretationProposal != accepted Take`.

Only later State Authority may approve/reject. Only later Take/commit authority makes Performance + approved consequences causal history.

## 31. No direct mutation API

No ProductionState-in/ProductionState-out, authoritative RecordId allocation, fixture/Character/Relationship/Pressure/World mutation, history write, or persistence. Direct Interpreter mutation is an E0 hard-gate violation.

## 32. No truth promotion

Domain names describe proposed destinations only if later approved. Claim does not justify WorldState; guess does not justify Knowledge; explicit physical action may justify a World/Scene proposal but State Authority still checks locks/possibility/transitions/causality.

## 33. Disposition/Relationship conservatism downstream

Interpreter may propose these domains; State Authority later enforces rare/conservative Disposition change, immediate-vs-durable distinction, semantic nonnumeric relationship representation, locks, and transitions. No score/threshold here.

## 34. Observation reserved

No CharacterObservation proposal. Memory/Belief/Suspicion never retroactively prove Observation.

## 35. World Resolver separate

World/Scene proposals are interpreted consequences of Character Performance, not autonomous world evolution. Weather/scheduled/resource/non-Character possibility evolution remains future World Resolver.

## 36. Interpreter input/provider disclosure open

Patch 0009 does not freeze Interpreter input. Future composition minimizes disclosure, preserves attempt provenance, separates trusted state from untrusted Performance, does not treat dialogue as instruction, avoids whole-Production disclosure by default, and preserves credentials/privacy. State Interpreter is not a Character; a separate bounded disclosure contract is later.

## 37. E0 controls

Directly applies to per-Character Candidate/Integrity paths (A and compatible B/C/D/G). E0-F may inject failures. E0-E playwright is not forced through this Candidate-specific parser; its consequence protocol is later control-specific while satisfying hard integrity/causality.

## 38. Determinism

Identical valid Source + semantically equivalent JSON structure/content -> identical semantic proposal. No clock/random/culture/filesystem/network/provider/AI/GPU/NPU/global mutable state. Property order/whitespace insignificant; mutation order preserved; evidence IDs canonicalized.

## 39. Failure domain

One small State Interpreter exception domain. Malformed Source, non-Accept/mismatch, bad transport/schema/domain/operation/field combination/ID/text/hash/duplicate -> exception. Failure creates no fictional consequence, State mutation, retry authorization, or Take disposition.

## 40. Required tests/review gates

Use canonical upstream construction; no public test bypasses.

### Source
1. canonical Voss Context/Candidate/Patch 0008 Accept binds;
2. Source constructor non-public;
3. non-Accept/reject/mismatch/unsupported validation contract fail;
4. synthetic Accept may bind but no auth/authority flag;
5. Source public surface excludes prose/Take/State/provider;
6. roster canonical exactly three;
7. impossible upstream malformed states defensive/reflection review.

### Proposal/transport
8. Proposal constructor non-public/exact fields;
9. empty mutations valid;
10. representative mutation parses;
11. property order/whitespace insignificant;
12. candidate identity mismatch fails;
13. unknown/missing/decoded-duplicate/wrong type/trailing/comments/comma/BOM fail;
14. 1 MiB inclusive valid envelope; +1 fails;
15. depth >8 fails;
16. errors do not echo creative text/raw JSON.

### Semantic type safety
17. all semantic constructors non-public;
18. Global variant cannot carry Character IDs by shape;
19. append-only Character variant exposes Add change only;
20. mutable Character variant has subject/no target;
21. Relationship has required subject+target;
22. Claim has no existing-record/change operation shape;
23. Deactivate has no Text; Add no ExistingRecordId; Supersede has both;
24. parser cannot construct invalid variant/change combination.

### Domains/operations
25. all E0 domains valid under exact variant rules;
26. Knowledge/Memory reject supersede/deactivate;
27. no Constitution/HistoricalTruth/Observation/PresentationPerspective/Director domain;
28. non-roster target/self-relationship fail;
29. Claim source-only/Add-only;
30. Delete/Replace strings rejected;
31. RecordId syntax does not claim existence/authority;
32. Evidence duplicate fails/order canonicalizes;
33. exact duplicate mutation fails; non-identical conflict representable.

### Text/truth/authority
34. exact text/no rewrite; non-NFC/control/invisible-only fail;
35. empty list means no durable mutation only;
36. Claim distinct from WorldState/Knowledge;
37. Belief/Suspicion/Memory may diverge from truth;
38. WorldState/Knowledge/Disposition proposal creates no authority;
39. no memory forgetting/unlearning proposal shape in E0;
40. no confidence/score/rationale;
41. no State apply/RecordId allocation;
42. no Take/Commit/provider/retry/spend/Director mutation APIs.

### Regression
43. Patch 0008 Candidate hash oracle unchanged;
44. Missing Raft Structured/Rendered Context hashes unchanged;
45. Missing Raft ECJ-1 9112 bytes/hash unchanged;
46. existing 253 Core tests green;
47. Missing Raft Harness PASS/0;
48. smoke Harness PASS/0.

## 41. ARM64/battery

Tiny deterministic bounded parsing/ID/Unicode/immutable-array work. No network/provider/background/AI/GPU/NPU/filesystem/polling. No NPU claim.

## 42. Explicit exclusions

No Interpreter provider/input composer; review/provider authentication; provider-attempt persistence; retries/spend/cancellation; provisional/accepted/rejected/alternate Take semantics; State Authority; ProductionState/StateHash; authoritative RecordId allocation; mutation application; atomic causal commit; persistence/recovery; effective opportunity/history append; Scene loop; memory-forgetting design; full Observation; World Resolver; E0-E control protocol; post-E0 ontology; final mutation UX; WinUI/Windows AI/NPU/packaging/WACK/Store.

## 43. Recursive audit dimensions

Restart after each material correction:

1. Interpreter vs State Authority;
2. Integrity sequencing/Accept non-authority;
3. Take ordering open;
4. creator-ontology guard;
5. E0 category fidelity;
6. Constitution read-only;
7. truth/claim/knowledge/belief/suspicion/memory separation;
8. memory-forgetting ODR preservation;
9. Observation boundary;
10. World Resolver;
11. historical texture vs durable consequence;
12. append-only/supersession semantics;
13. invalid semantic state representability;
14. proposal vs authority;
15. locks/conservative change downstream;
16. Candidate content/Integrity structural binding vs auth;
17. evidence/provenance sufficiency;
18. RecordId non-authority;
19. least privilege;
20. untrusted text/no rewrite;
21. strict transport/resource bounds;
22. deterministic ordering/conflicts;
23. E0 control isolation;
24. experimental reconstruction;
25. API minimality/non-forgeability;
26. testability;
27. Hygiene Constitution;
28. ARM64;
29. scope/validation claims.

Approval only after a complete restart finds zero material correction/worthwhile improvement.

## 44. Material approval decisions

Approval would freeze only:

1. Patch 0009 schema/parser only;
2. structural Source Bind requires exact Patch 0008 Accept but authenticates neither assessor nor Take;
3. Take ordering remains open;
4. semantic `ensemble.e0.state-interpreter.proposal.v1` and JSON `ensemble.e0.state-interpreter.proposal-json.v1`;
5. Source is narrow Candidate identity/source Character/Context/roster/Integrity-contract data only;
6. empty mutations valid = no durable projected mutation proposed;
7. E0 domains exactly WorldState, SceneState, UnresolvedProposition, CharacterKnowledge, CharacterBelief, CharacterSuspicion, CharacterMemory, CharacterGoal, CharacterDisposition, CharacterCircumstance, CharacterClaim, Relationship, Pressure;
8. E0 domains do not freeze final ontology;
9. Constitution/HistoricalTruth/Observation/PresentationPerspective/Director absent;
10. semantic typed variants make global/append-only-character/mutable-character/claim/relationship invalid states difficult to express;
11. typed changes Add/Supersede/Deactivate; no Delete/Replace;
12. CharacterKnowledge and CharacterMemory are Add-only in E0, preserving unresolved forgetting/unlearning design;
13. Claim source-Character Add-only;
14. Existing/Evidence RecordIds are syntactic proposal references only;
15. Candidate hash root binding is not attempt/Take/commit identity;
16. text exact/NFC/display-bearing; no rewrite;
17. evidence IDs canonical/duplicate-free/may-empty and imply no auth/disclosure;
18. mutation order preserved for reconstruction, no commit priority;
19. exact duplicates fail; non-identical conflicts remain State Authority problem;
20. parser 1 MiB inclusive/depth 8/strict structure;
21. no scores/rationale/provider/Take/State/commit/Director authority fields;
22. domain names do not create truth/knowledge/durable change;
23. Observation/autonomous World evolution separate;
24. E0-E not forced through Candidate-specific schema;
25. later State Authority owns existence/domain/locks/transitions/stale/conflict/evidence/conservative change;
26. post-E0 ontology remains open;
27. no provider, Take, State Authority, commit, persistence, Scene loop, UI, Windows AI/NPU, Store scope.

Implementation remains blocked until recursive audit completes and user explicitly approves the final proposal.
