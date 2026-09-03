# H1 Patch 0009 — E0 State Interpreter Mutation-Proposal Contract

Status: blueprint proposal 0.7 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
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

It does **not** call a model/provider, authenticate semantic-review/provider provenance, allocate a Take, decide provisional-Take timing, approve/reject mutations, mutate Production state, commit history, persist anything, or trigger another Performer.

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

`Bind` enforces only the Patch 0009 cross-boundary facts needed to associate Context/Candidate with supplied Patch 0008 evaluation. It does not become a second Integrity Validator.

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

No public constructor. SourceSceneId copies `ContextPacket.SceneId`. Roster IDs canonical ordinal.

No Candidate prose, private Context prose, rendering/access audit, Integrity evaluation/evidence, provider/model, Take, State mutation, or authority flag.

Source is a narrow parser/binding input, not eligibility/provenance authority.

## 9. Source Bind invariants

Fail closed unless:

1. sourceContext/sourceCandidate/integrityEvaluation non-null;
2. Context SceneId, subject, opportunity, ContextPacketId, roster IDs initialized;
3. Context subject == Context opportunity;
4. fresh canonical `IntegrityCandidateInput.Bind(sourceContext, sourceCandidate)` succeeds;
5. fresh Integrity input has zero Reject codes;
6. integrityEvaluation Disposition == Accept;
7. evaluation Trace exists and ValidationContract exactly current Patch 0008 contract;
8. Trace Input exists and its CandidateContentIdentityContract, CandidateContentHash, SourceContextPacketId exactly match fresh Integrity input;
9. Context roster exactly three unique initialized E0 Characters containing source once.

Patch 0009 does not inspect/revalidate concern evidence; Patch 0008 remains canonical. Impossible forged upstream objects are defensive/static/reflection review cases without public bypass APIs.

## 10. Structural Accept binding is not authentication

Bound Source means only that normal non-forgeable Patch 0008 output reports Accept for the exact Candidate/source association.

It does not prove configured review/provider attempt, accepted Take, spend permission, or State authority. Synthetic Accept may bind for tests. Effective orchestration later authenticates review/provider provenance.

Source deliberately carries neither Integrity evaluation nor ValidationContract; canonical Integrity provenance stays separate.

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

The parser copies CandidateContentIdentityContract, CandidateContentHash, and SourceSceneId directly from trusted `StateInterpretationSource`; untrusted AI JSON cannot supply or override them.

No Take/Commit ID, authority disposition, provider/model, confidence, free-form rationale, Director/current-opportunity data, or authoritative new RecordId.

## 12. Why proposal identity is not echoed by AI transport

The AI transport does **not** contain CandidateContentIdentityContract, CandidateContentHash, or SourceSceneId.

Reason:

- parser already receives trusted Source;
- asking a model to reproduce opaque machine identity adds failure modes without authenticating the provider attempt;
- source identity should be structural trusted metadata, not model-authored content;
- this matches Patch 0006's pattern where Candidate subject/context semantic identity is supplied by trusted parser input rather than AI JSON;
- actual request/response association belongs later provider-attempt provenance.

Therefore identical raw Interpreter JSON may be parsed against different synthetic Sources in tests and produce differently bound semantic proposals. That is expected and grants no authority.

## 13. Scene identity is explicit in semantic output

SceneState is Scene-scoped and later State Authority must know which Scene a proposal concerns without reverse-mapping a hash.

E0 has one Scene, but semantic `SourceSceneId` prevents ambient-state inference and improves reconstruction. This does not introduce multi-Scene persistence/cross-Scene mutation.

## 14. Empty mutation proposal is valid

`Mutations = []` means no durable projected-state mutation proposed. It is not rejection and does not erase accepted historical texture. Performance-history acceptance remains later Take/commit authority.

## 15. E0 proposal domains

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

## 16. Domain meanings

- WorldState — possible Character-caused durable world consequence; not truth merely because proposed.
- SceneState — possible current Source Scene consequence; not autonomous World Resolver.
- UnresolvedProposition — preserves uncertainty.
- CharacterKnowledge — proposed new knowledge; later authority prevents claim/guess promotion.
- CharacterBelief — proposed belief, potentially false.
- CharacterSuspicion — proposed uncertainty/suspicion.
- CharacterMemory — proposed new recollection; may differ from truth.
- CharacterGoal — proposed current goal change.
- CharacterDisposition — proposed persistent tendency; downstream conservative.
- CharacterCircumstance — proposed fluid immediate state.
- CharacterClaim — interpreted proposition representing what source Character is understood to have claimed; not verbatim quotation/fact.
- Relationship — directional semantic relationship state/effect; no numeric psychology.
- Pressure — current/durable pressure without enumerating every dramatic concept.

## 17. CharacterClaim is interpreted proposition, not quotation

CharacterClaim Text is an interpreted proposition attributable to source Character's Performance, not transcript text.

Exact wording remains Candidate/accepted Performance history. Future UI/provenance distinguishes interpreted proposition from verbatim Performance. Claim proposal cannot rewrite accepted Performance history and never becomes objective truth by parsing.

## 18. Absent domains

No Constitution, HistoricalTruth/accepted Performance history, Observation, direct locked-Canon domain, PresentationPerspective, or Director opportunity mutation.

## 19. Typed change union

```text
StateMutationChange
    AddStateMutationChange(Text)
    SupersedeStateMutationChange(ExistingRecordId, Text)
    DeactivateStateMutationChange(ExistingRecordId)
```

No public constructors. Parser/internal builder only. No Delete/silent Replace.

## 20. Typed mutation union

```text
StateMutationCandidate
    GlobalStateMutationCandidate(Domain, Change, SupportingRecordIds)
    AppendOnlyCharacterStateMutationCandidate(Domain, SubjectCharacterId, AddStateMutationChange, SupportingRecordIds)
    MutableCharacterStateMutationCandidate(Domain, SubjectCharacterId, Change, SupportingRecordIds)
    CharacterClaimMutationCandidate(SubjectCharacterId, Text, SupportingRecordIds)
    RelationshipStateMutationCandidate(SubjectCharacterId, TargetCharacterId, Change, SupportingRecordIds)
```

No public constructors. Flat AI transport converts to immutable semantic variants.

SupportingRecordIds are Interpreter-declared structural support references, not authenticated evidence/authority.

## 21. Domain membership / operation restrictions

### Global
WorldState, SceneState, UnresolvedProposition, Pressure; Add/Supersede/Deactivate candidate allowed for later authority review.

### Append-only Character
CharacterKnowledge, CharacterMemory; roster subject; Add only. This preserves open forgetting/unlearning/memory-rewrite design. Corrective recollection can be another Memory proposal without rewriting earlier memory history.

### Mutable Character
CharacterBelief, CharacterSuspicion, CharacterGoal, CharacterDisposition, CharacterCircumstance; roster subject; Add/Supersede/Deactivate candidate.

### CharacterClaim
Source Character only; Add-only; Text required; no ExistingRecordId.

### Relationship
Distinct Source Scene roster subject+target; any typed Change candidate.

## 22. ExistingRecordId semantics

Only Supersede/Deactivate contain ExistingRecordId. Parser validates syntax only—not existence, domain, ownership, active state, lock status, causal fitness. State Authority later validates.

## 23. Mutation text

Add/Supersede/Claim Text: non-null/non-empty, exact, already NFC, no trim/repair, LF/TAB allowed, other Control scalars rejected, display-bearing scalar required under Patch 0006 category principle. Untrusted creative interpretation, never instruction authority. Deactivate has no Text semantic property.

## 24. SupportingRecordIds

Every mutation has immutable SupportingRecordIds:

- syntactically valid;
- duplicate-free;
- canonical ordinal;
- may be empty because Candidate content is root causal source;
- may refer to state future Interpreter input disclosed;
- proves neither existence, actual disclosure, relevance, sufficiency, nor authority.

Provider/orchestration provenance later establishes actual Interpreter disclosure. State Authority validates references/causal suitability. No rationale/quote/score/confidence/hidden reasoning.

## 25. Candidate content root binding

Semantic Proposal copies Candidate content identity/hash from Source. Candidate hash remains content identity only—not provider attempt, Take, or commit identity.

## 26. Flat AI JSON transport

```json
{
  "schemaVersion": "ensemble.e0.state-interpreter.proposal-json.v1",
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

Root has exactly `schemaVersion` and `mutations`.

Every mutation has all seven properties with explicit nulls where absent. Flat transport; typed Core semantics.

## 27. Transport strings

Domains: `worldState | sceneState | unresolvedProposition | characterKnowledge | characterBelief | characterSuspicion | characterMemory | characterGoal | characterDisposition | characterCircumstance | characterClaim | relationship | pressure`

Operations: `add | supersede | deactivate`. Exact case; no aliases.

## 28. Transport-to-semantic rules

- Global: subject/target null;
- Knowledge/Memory: roster subject, target null, add only, existingRecordId null, valid Text;
- Mutable Character: roster subject, target null, exact change combination;
- Relationship: distinct Source Scene roster subject+target, exact change;
- CharacterClaim: add; subject == SourceCharacter; target/existing null; valid Text;
- Add -> existing null + Text;
- Supersede -> ExistingRecordId + Text;
- Deactivate -> ExistingRecordId + text null.

Invalid combinations fail before semantic construction.

## 29. Strict parser

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
- Character IDs satisfy Source Scene roster/domain rules;
- RecordIds syntax checked;
- SupportingRecordIds duplicates rejected then ordinal canonicalized;
- exact duplicate semantic mutations rejected after support-ID canonicalization;
- semantic Proposal copies Candidate identity + Scene identity from trusted Source;
- sanitized exceptions do not echo mutation Text, arbitrary unknown names/values, raw snippets, provider content.

No provider call.

## 30. Mutation ordering

Array order preserved for reconstruction but confers no State Authority priority/sequential semantics. State Authority later defines whole-batch conflict/staleness behavior.

## 31. Duplicates/conflicts

Exact semantic duplicates fail. Non-identical conflicts remain representable; no ranking/merge/repair. State Authority later decides.

## 32. No hidden rewriting

No text rewrite/summarization, proposal merging, contradiction repair, claim/belief/suspicion->knowledge/fact conversion, WorldState promotion, UnresolvedProposition resolution, Observation inference, or manufactured social effects.

CharacterClaim interpreted proposition is a distinct proposal type, not Performance-history rewrite.

## 33. Proposal is not authority

`StateInterpretationProposal != approved mutation != committed consequence` and `StateInterpretationProposal != accepted Take`.

Only later State Authority approves/rejects. Later Take/commit authority makes Performance + approved consequences causal history.

## 34. No direct mutation API

No ProductionState-in/ProductionState-out, authoritative RecordId allocation, fixture/Character/Relationship/Pressure/World mutation, history write, or persistence. Direct Interpreter mutation is hard-gate violation.

## 35. No truth promotion

Domain names describe proposed destination only if later approved. Claim does not justify WorldState; guess does not justify Knowledge; physical action may support World/Scene proposal but State Authority still checks locks/possibility/transitions/causality.

## 36. Disposition/Relationship conservatism downstream

Interpreter may propose; State Authority later enforces rare/conservative Disposition change, immediate-vs-durable distinction, semantic nonnumeric relationship representation, locks/transitions. No score/threshold here.

## 37. Observation reserved

No CharacterObservation proposal. Memory/Belief/Suspicion never retroactively prove Observation.

## 38. World Resolver separate

World/Scene proposals are interpreted consequences of Character Performance, not autonomous world evolution. Weather/scheduled/resource/non-Character possibility evolution remains future World Resolver.

## 39. Interpreter input/provider disclosure open

Patch 0009 does not freeze Interpreter input. Future composition minimizes disclosure, preserves attempt provenance, separates trusted state from untrusted Performance, avoids treating dialogue as instruction/whole-Production disclosure, preserves credentials/privacy. Interpreter is not Character; separate bounded disclosure contract later.

SupportingRecordIds optional so schema does not force internal RecordId disclosure.

## 40. Provider response association remains later

Because AI JSON carries no Candidate/Scene hash echo, Patch 0009 alone cannot prove which provider request produced a raw response.

That is intentional: a model-authored echo would not prove it either.

Future provider-attempt/orchestration provenance must bind:

```text
Interpreter request
+ exact disclosed input
+ StateInterpretationSource identity
+ raw/parsed response
+ provider/model/settings/failures
```

before any effective progression. Patch 0009 semantic parsing grants no such authenticity.

## 41. E0 controls

Directly applies to per-Character Candidate/Integrity paths (A and compatible B/C/D/G). E0-F may inject failures. E0-E playwright is not forced through Candidate-specific parser; its consequence protocol is later control-specific while satisfying hard integrity/causality.

## 42. Determinism

Identical valid Source + semantically equivalent JSON structure/content -> identical semantic proposal. No clock/random/culture/filesystem/network/provider/AI/GPU/NPU/global mutable state. Property order/whitespace insignificant; mutation order preserved; support IDs canonicalized.

## 43. Failure domain

One small State Interpreter exception domain. Malformed Source, non-Accept/mismatch, bad transport/schema/domain/operation/field combination/ID/text/duplicate -> exception. Failure creates no fictional consequence, State mutation, retry authorization, or Take disposition.

## 44. Required tests/review gates

Use canonical upstream construction; no public test bypasses.

### Source
1. canonical Voss Context/Candidate/Patch 0008 Accept binds;
2. Source constructor non-public;
3. non-Accept/reject/mismatch/unsupported validation contract fail during Bind;
4. Source Bind does not duplicate concern-evidence validation;
5. synthetic Accept may bind but no auth/authority flag;
6. Source public surface contains SceneId but excludes Integrity eval/contract, prose/Take/State/provider;
7. SourceSceneId equals ContextPacket.SceneId;
8. roster canonical exactly three;
9. impossible upstream malformed states defensive/reflection review.

### Proposal/transport
10. Proposal constructor non-public/exact fields including trusted-copy Candidate identity + SourceSceneId;
11. empty mutations valid;
12. representative mutation parses;
13. root transport exact fields only schemaVersion/mutations;
14. Candidate/Scene identity fields in AI JSON are unknown properties and rejected;
15. property order/whitespace insignificant;
16. unknown/missing/decoded-duplicate/wrong type/trailing/comments/comma/BOM fail;
17. 1 MiB inclusive valid envelope; +1 fails;
18. depth >8 fails;
19. errors do not echo creative text/raw JSON;
20. same raw JSON parsed against two valid synthetic Sources copies each respective trusted identity without claiming provider authenticity.

### Semantic type safety
21. all semantic constructors non-public;
22. Global cannot carry Character IDs by shape;
23. append-only Character exposes Add only;
24. mutable Character has subject/no target;
25. Relationship has required subject+target;
26. Claim has no existing/change-operation shape;
27. Deactivate has no Text; Add no ExistingRecordId; Supersede has both;
28. parser cannot construct invalid variant/change combination.

### Domains/operations
29. all E0 domains valid under exact rules;
30. Knowledge/Memory reject supersede/deactivate;
31. no Constitution/HistoricalTruth/Observation/PresentationPerspective/Director domain;
32. non-roster target/self-relationship fail;
33. Claim source-only/Add-only;
34. Delete/Replace rejected;
35. RecordId syntax does not claim existence/authority;
36. Supporting ID duplicate fails/order canonicalizes;
37. exact duplicate mutation fails; non-identical conflict representable.

### Claim/text/truth/authority
38. CharacterClaim Text interpreted proposition, not verbatim quote;
39. Performance exact words remain outside proposal;
40. exact mutation text/no rewrite; non-NFC/control/invisible-only fail;
41. empty list means no durable mutation only;
42. Claim distinct from WorldState/Knowledge;
43. Belief/Suspicion/Memory may diverge from truth;
44. WorldState/Knowledge/Disposition proposal creates no authority;
45. no memory forgetting/unlearning proposal shape;
46. no confidence/score/rationale;
47. no State apply/RecordId allocation;
48. no Take/Commit/provider/retry/spend/Director mutation APIs.

### Regression
49. Patch 0008 Candidate hash oracle unchanged;
50. Missing Raft Structured/Rendered Context hashes unchanged;
51. Missing Raft ECJ-1 9112 bytes/hash unchanged;
52. existing 253 Core tests green;
53. Missing Raft Harness PASS/0;
54. smoke Harness PASS/0.

## 45. ARM64/battery

Tiny deterministic bounded parsing/ID/Unicode/immutable-array work. No network/provider/background/AI/GPU/NPU/filesystem/polling. No NPU claim.

## 46. Explicit exclusions

No Interpreter provider/input composer; review/provider authentication; provider-attempt persistence; retries/spend/cancellation; provisional/accepted/rejected/alternate Take semantics; State Authority; ProductionState/StateHash; authoritative RecordId allocation; mutation application; atomic causal commit; persistence/recovery; effective opportunity/history append; Scene loop; memory-forgetting design; full Observation; World Resolver; E0-E control protocol; multi-Scene persistence; post-E0 ontology; final mutation UX; WinUI/Windows AI/NPU/packaging/WACK/Store.

## 47. Recursive audit dimensions

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
14. trusted semantic binding vs model-authored identity echo;
15. provider-attempt authenticity deferred;
16. historical texture vs durable consequence;
17. append-only/supersession semantics;
18. invalid semantic state representability;
19. proposal vs authority;
20. locks/conservative change downstream;
21. supporting-record/provenance sufficiency;
22. RecordId non-authority;
23. least privilege;
24. untrusted text/no rewrite;
25. strict transport/resource bounds;
26. deterministic ordering/conflicts;
27. E0 control isolation;
28. experimental reconstruction;
29. API minimality/non-forgeability;
30. testability;
31. Hygiene Constitution;
32. ARM64;
33. scope/validation claims.

Approval only after a complete restart finds zero material correction/worthwhile improvement.

## 48. Material approval decisions

Approval would freeze only:

1. Patch 0009 schema/parser only;
2. Source Bind requires exact Patch 0008 Accept association but does not reimplement Patch 0008 concern validation/authenticate assessor/Take;
3. Source omits Integrity evaluation/ValidationContract; canonical Integrity provenance stays separate;
4. Take ordering remains open;
5. semantic `ensemble.e0.state-interpreter.proposal.v1` and JSON `ensemble.e0.state-interpreter.proposal-json.v1`;
6. Source carries Candidate content identity, SourceSceneId, source Character/Context IDs, canonical roster only;
7. semantic Proposal copies Candidate identity + SourceSceneId from trusted Source; AI JSON contains only schemaVersion + mutations and cannot author/override those identities;
8. provider request/response authenticity remains later orchestration provenance;
9. empty mutations valid = no durable projected mutation proposed;
10. E0 domains exactly WorldState, SceneState, UnresolvedProposition, CharacterKnowledge, CharacterBelief, CharacterSuspicion, CharacterMemory, CharacterGoal, CharacterDisposition, CharacterCircumstance, CharacterClaim, Relationship, Pressure;
11. E0 domains do not freeze final ontology;
12. Constitution/HistoricalTruth/Observation/PresentationPerspective/Director absent;
13. typed variants prevent invalid global/append-only-character/mutable-character/claim/relationship states;
14. typed Add/Supersede/Deactivate; no Delete/Replace;
15. Knowledge/Memory Add-only, preserving unresolved forgetting/unlearning design;
16. CharacterClaim source-only Add interpreted proposition, not verbatim quotation/history replacement;
17. ExistingRecordId/SupportingRecordIds syntactic proposal references only;
18. Candidate hash content binding is not attempt/Take/commit identity;
19. text exact/NFC/display-bearing; no parser rewrite;
20. SupportingRecordIds canonical/duplicate-free/may-empty and imply no evidence/auth/disclosure authority;
21. mutation order preserved for reconstruction, no commit priority;
22. exact duplicates fail; non-identical conflicts remain State Authority problem;
23. parser 1 MiB inclusive/depth 8/strict structure;
24. no scores/rationale/provider/Take/State/commit/Director authority fields;
25. domain names do not create truth/knowledge/durable change;
26. Observation/autonomous World evolution separate;
27. E0-E not forced through Candidate-specific schema;
28. later State Authority owns existence/domain/locks/transitions/stale/conflict/evidence/conservative change;
29. post-E0 ontology remains open;
30. no provider, Take, State Authority, commit, persistence, Scene loop, UI, Windows AI/NPU, Store scope.

Implementation remains blocked until recursive audit completes and user explicitly approves the final proposal.
