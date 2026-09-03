# H1 Patch 0009 — E0 State Interpreter Mutation-Proposal Contract

Status: blueprint proposal 0.1 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
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

Patch 0009 defines:

1. the E0 semantic State Interpreter mutation-proposal vocabulary;
2. a strict E0 AI JSON transport/parser for that proposal;
3. structural binding to the exact Candidate content identity and an Integrity `Accept` evaluation;
4. evidence/provenance fields sufficient for later deterministic State Authority review.

Patch 0009 does **not** call a model/provider, authenticate semantic-review provenance, allocate a Take, decide whether a provisional Take exists before interpretation, approve/reject mutations, mutate Production state, commit history, persist anything, or trigger the next Performer.

## 2. Recovered frozen authority

Frozen Blueprint 0.1 and the approved H1 sequence place the next boundaries in this order:

1. Performer candidate-output contract;
2. Director opportunity-selection contract;
3. Integrity Validator;
4. **State Interpreter candidate-mutation schema**;
5. deterministic State Authority;
6. accepted/rejected/alternate Take semantics;
7. atomic causal commit.

Frozen constitutional law:

- probabilistic systems may propose; deterministic authority decides;
- Integrity checks before interpretation/acceptance progression;
- State Interpreter proposes what a Performance may mean;
- State Interpreter never directly mutates authority;
- State Authority decides which proposals may commit under creator locks, type/transition rules, review policy, and causal evidence;
- accepted Performance + approved consequences eventually form one atomic causal commit;
- a statement is not automatically a fact;
- possibility is not fact;
- objective truth, observation, claim, belief, memory, rumor, and unresolved proposition remain distinct;
- Constitution is read-only in E0;
- Disposition change is rare/conservative;
- Circumstance may change frequently when supported;
- creator-locked canon cannot be changed;
- a technical failure cannot become fictional action;
- a full observation engine and World Resolver remain reserved/outside E0;
- proposed State Interpreter mutations and committed/rejected mutations with reasons are required E0 provenance.

Patch 0008 additionally freezes:

- `IntegrityDisposition.Accept` is only an Integrity evaluation result, not accepted Take or State authority;
- synthetic concern evidence may produce synthetic Accept for tests;
- later State/Take/orchestration must authenticate configured semantic-review provenance before effective progression;
- immediate provisional-Take-versus-State-Interpreter ordering remains open;
- no Patch 0008 authority-bearing eligibility/attestation token exists.

## 3. Why Patch 0009 is schema/parser only

The canonical source says **State Interpreter candidate-mutation schema** comes next. It does not require provider execution yet.

Defining a provider/model call now would prematurely require unresolved choices about:

- Interpreter input composition and least-privilege Production disclosure;
- semantic-assessor provenance authentication;
- provisional Take timing;
- provider attempt identity;
- retry/cost policy;
- State Authority current-state representation;
- ProductionState/StateHash;
- persistence/commit orchestration.

Therefore Patch 0009 freezes the proposal boundary without inventing those downstream authorities.

## 4. Creator-ontology extensibility guard revisit

`CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md` requires a deliberate revisit before freezing the State Interpreter's broad mutation ontology.

Decision for E0 Patch 0009:

> Keep the mutation vocabulary explicitly E0-scoped and authority-oriented. Preserve the frozen E0 categories needed for deterministic experimentation, but do not claim they are the final creator-facing or post-E0 storage ontology.

The E0 mutation domains below exist because they carry materially different authority/transition semantics in frozen E0—not because every dramatic concept must become an engine enum.

Consequently:

- UI labels are not storage types;
- creator-defined concepts remain open;
- the final post-E0 Production/Studio ontology remains unresolved;
- no generalized meta-ontology is introduced merely for extensibility;
- current E0 categories remain exact for the experiment.

## 5. No provisional-Take ordering decision

Patch 0009 does not decide whether a future orchestration contract allocates a provisional Take object:

- before State Interpreter execution;
- after State Interpreter proposal generation;
- after State Authority review;
- or only as part of the final atomic causal commit.

The only ordering frozen here is:

```text
Integrity Accept evaluation exists before StateInterpretationSource.Bind succeeds.
```

That is structural sequencing, not Take acceptance.

## 6. Contracts

Freeze two independent contract domains:

```text
StateInterpretationContractVersion = ensemble.e0.state-interpreter.proposal.v1
StateInterpretationJsonSchemaVersion = ensemble.e0.state-interpreter.proposal-json.v1
```

Semantic proposal meaning and AI JSON transport syntax are distinct.

Candidate content identity continues to use Patch 0008:

```text
ensemble.e0.integrity.candidate-content.v1
```

Patch 0009 does not redefine Candidate identity.

## 7. Structurally bound source

Preferred construction:

```text
StateInterpretationSource.Bind(
    ContextPacket sourceContext,
    CandidatePerformance sourceCandidate,
    IntegrityValidationEvaluation integrityEvaluation)
    -> StateInterpretationSource
```

This is the only Patch 0009 rich-object binding boundary.

It may inspect the upstream objects only to verify structural source identity and Integrity-evaluation consistency, then copies a narrow immutable structural source.

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

`RosterCharacterIds` is canonical ordinal order.

No Candidate VisibleText, Character private Context prose, rendered context, access audit, Integrity concern prose, provider/model data, Take identity, State mutation, or authority flag is exposed.

## 9. Bind invariants

`StateInterpretationSource.Bind(...)` fails in the State Interpreter exception domain unless:

1. sourceContext is non-null and structurally initialized;
2. sourceCandidate is non-null and structurally initialized through the existing Patch 0006 contract;
3. integrityEvaluation is non-null and structurally initialized;
4. Context subject == Context opportunity Character;
5. a fresh `IntegrityCandidateInput.Bind(sourceContext, sourceCandidate)` succeeds;
6. fresh Integrity input has no deterministic Reject codes;
7. `integrityEvaluation.Disposition == Accept`;
8. Integrity trace ValidationContract is exactly `ensemble.e0.integrity.validation.v1`;
9. Integrity trace Input content identity contract/hash/source ContextPacket identity exactly match the fresh Integrity input;
10. Integrity concern evidence is present and structurally valid for the same content identity;
11. Integrity concern evidence contains zero concerns, consistent with Accept;
12. source Candidate subject/context identities match sourceContext through the fresh Integrity input;
13. source Context roster is initialized, exactly the frozen E0 three Characters, unique, and includes SourceCharacterId exactly once.

Patch 0009 does not reimplement Patch 0006 Candidate text/control validation or Patch 0008 concern-evidence semantics.

## 10. Structural Accept binding is not assessor authentication

A successfully bound `StateInterpretationSource` means only:

> These upstream semantic objects form a structurally consistent Patch 0008 Accept evaluation for this exact Candidate content identity.

It does **not** mean:

- concern review actually came from the configured E0 semantic-review mechanism;
- this was a real provider attempt;
- the Candidate is an accepted Take;
- interpretation is authorized to spend/call a provider;
- any mutation may commit;
- any proposal is Production truth.

Synthetic Patch 0008 Accept evaluations may therefore create synthetic StateInterpretationSource objects for tests without creating Production authority.

Later orchestration still owns assessor/provider provenance authentication before effective use.

## 11. Semantic proposal

```text
StateInterpretationProposal
- ContractVersion
- CandidateContentIdentityContract
- CandidateContentHash
- Mutations
```

No public constructor. The strict parser is the E0 public construction path.

`CandidateContentIdentityContract` and `CandidateContentHash` must exactly match the supplied StateInterpretationSource.

The proposal contains no:

- accepted/rejected Take status;
- State Authority decision;
- CommitId/TakeId;
- provider/model identity;
- confidence score;
- hidden rationale/chain-of-thought;
- Director decision;
- Current Opportunity;
- authoritative RecordId allocation for new records.

## 12. Empty mutation proposal is valid

`Mutations = []` is valid.

Meaning:

> The Interpreter proposes no durable projected-state mutation for this Performance.

This is important because accepted historical texture may be true and memorable without deserving a durable state field.

An empty mutation proposal does not mean the Performance did not happen, was rejected, or should be erased. Event-history acceptance remains later Take/commit authority.

## 13. E0 mutation domains

Freeze the following E0-only semantic proposal domains:

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

These are proposal target domains, not creator-facing UI labels and not final post-E0 ontology law.

## 14. Why these domains exist

### WorldState

Allows an accepted Character action to propose a durable world consequence. It does not make the consequence true. State Authority must later enforce creator locks, world rules, causal support, and claim-vs-fact boundaries.

### SceneState

Allows immediate Scene conditions to be proposed as durable/current Scene state without inventing a full World Resolver.

### UnresolvedProposition

Preserves uncertainty as uncertainty. A proposal may add/supersede/deactivate an unresolved proposition but cannot silently confirm objective truth.

### CharacterKnowledge

Allows proposed knowledge changes while preserving the rule that State Authority must not promote a mere claim/guess into knowledge without adequate authority/evidence.

### CharacterBelief / CharacterSuspicion

Preserve distinct epistemic stances. They may be wrong without changing objective truth.

### CharacterMemory

Represents a proposed durable recollection. Memory may differ from objective truth.

### CharacterGoal

Represents a proposed change in what a Character currently seeks.

### CharacterDisposition

Represents a proposed persistent tendency/relationship-adjacent Character change. E0 State Authority must treat this domain conservatively.

### CharacterCircumstance

Represents fluid immediate Character state and may change frequently when causally supported.

### CharacterClaim

Represents a structured statement/claim attributable to a Character while preserving `claim != fact`. It exists as an authority distinction even though the frozen fixture schema does not currently carry an initial Claim collection.

### Relationship

Represents directional semantic relationship state/effect between two Characters, without numeric psychology.

### Pressure

Represents durable/current dramatic pressure state. The text may remain creator-authored/semantic rather than requiring a fixed taxonomy of fear, jealousy, debt, obligation, etc.

## 15. Explicitly absent mutation domains

Patch 0009 provides **no** proposal domain for:

### Constitution

E0 Constitution is read-only. Interpreter cannot even represent a Constitution mutation proposal.

### HistoricalTruth / accepted Performance history

State Interpreter does not author the fact that a Performance occurred. Accepted Performance history belongs later atomic Take/commit authority.

### Observation

Blueprint 0.1 reserves a future observation boundary. State Interpreter must not manufacture authoritative observations without observation-eligibility authority.

### Creator-locked Canon as a special mutable domain

Interpreter may propose World/Scene effects, but creator locks are enforced later by State Authority. No direct “rewrite canon” mutation exists.

### PresentationPerspective

View/disclosure projection is not Production mutation.

### Director opportunity

Attention/routing is Director/effective-opportunity authority, not State mutation.

## 16. Mutation operations

Freeze exactly:

```text
Add
Supersede
Deactivate
```

These names are chosen to preserve append-only causal semantics.

### Add

Propose a new durable semantic record.

- ExistingRecordId must be null.
- Text required.

### Supersede

Propose a new durable semantic record that explicitly supersedes an existing record while preserving historical traceability.

- ExistingRecordId required.
- Text required.

### Deactivate

Propose that an existing record cease to participate in the current projection while remaining historically recoverable.

- ExistingRecordId required.
- Text must be null.

There is no destructive `Delete` or silent in-place `Replace` operation.

## 17. StateMutationCandidate semantic shape

```text
StateMutationCandidate
- Domain
- Operation
- SubjectCharacterId
- TargetCharacterId
- ExistingRecordId
- Text
- EvidenceRecordIds
```

All optional fields are represented explicitly as nullable values in semantic/transport shape rather than omitted polymorphic fields.

This keeps strict validation legible and avoids a large hierarchy of premature mutation subclasses.

## 18. Domain cardinality rules

### Global domains

```text
WorldState
SceneState
UnresolvedProposition
Pressure
```

Require:

- SubjectCharacterId = null;
- TargetCharacterId = null.

### Character domains

```text
CharacterKnowledge
CharacterBelief
CharacterSuspicion
CharacterMemory
CharacterGoal
CharacterDisposition
CharacterCircumstance
CharacterClaim
```

Require:

- SubjectCharacterId = one exact roster Character;
- TargetCharacterId = null.

### Relationship

Require:

- SubjectCharacterId = one exact roster Character;
- TargetCharacterId = one exact roster Character;
- SubjectCharacterId != TargetCharacterId.

No target ID outside the frozen E0 Scene roster is accepted by the parser.

## 19. CharacterClaim restrictions

`CharacterClaim` is special because a claim is historical speech/agency evidence, not mutable truth.

For Patch 0009:

- Operation must be `Add`;
- SubjectCharacterId must equal StateInterpretationSource.SourceCharacterId;
- ExistingRecordId must be null;
- Text required;
- later contradictory statements are additional claims, not silent replacement/deletion of an old claim.

This prevents Interpreter output from rewriting what another Character supposedly claimed.

## 20. ExistingRecordId semantics

`ExistingRecordId` is a **proposal reference**, not proof that the record exists, is active, belongs to the requested domain/Character, or is mutable.

Patch 0009 validates only:

- syntactic RecordId validity;
- nullability according to Add/Supersede/Deactivate.

Later deterministic State Authority must validate against then-authoritative state:

- record existence;
- ownership/domain;
- active/current status;
- creator locks;
- transition legality;
- stale-state conflicts;
- whether supersession/deactivation is causally justified.

A hallucinated/stale/wrong-domain RecordId cannot become authority merely because the parser accepted its syntax.

## 21. Mutation Text semantics

For Add/Supersede:

- Text must be non-null and non-empty;
- exact decoded text is preserved;
- no trimming;
- no normalization/repair by parser;
- input must already be Unicode NFC;
- LF and TAB are allowed;
- other Unicode Control scalars rejected;
- non-empty text must contain at least one display-bearing Unicode scalar using the same category principle as Patch 0006;
- no hidden prompt/instruction authority is inferred from text.

For Deactivate:

- Text must be null.

Mutation text is untrusted interpreted creative content, never system instruction or authority.

## 22. EvidenceRecordIds

Each mutation includes:

```text
EvidenceRecordIds: ImmutableArray<RecordId>
```

Semantics:

- exact syntactically valid RecordIds only;
- duplicate-free;
- canonical ordinal order in semantic output;
- may be empty because the bound Candidate content itself is always the primary causal source;
- may reference state records the future Interpreter was shown as supporting context;
- never proves the referenced record exists, was actually disclosed, supports the mutation, or authorizes it.

Later provider/orchestration provenance must establish what Interpreter context was actually disclosed. Later State Authority must validate evidence references against authoritative state and transition rules.

No free-form rationale, confidence number, hidden reasoning, or evidence quotation is stored in the mutation schema.

## 23. Candidate content is root causal binding

Every StateInterpretationProposal is bound to:

```text
CandidateContentIdentityContract
CandidateContentHash
```

These come from Patch 0008 and must match StateInterpretationSource exactly.

Therefore mutations cannot be silently detached from the Candidate semantic content they interpret.

CandidateContentHash remains **content identity only**. It does not prove provider attempt, assessor authenticity, accepted Take, or causal commit.

## 24. AI JSON transport

Freeze E0 transport schema:

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

Property order and insignificant structural whitespace have no semantic meaning.

The parser converts transport enum strings into typed semantic enums.

## 25. Exact transport strings

Domain strings:

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

Case-sensitive. No aliases.

## 26. Strict parser

Preferred API:

```text
StateInterpretationContract.ParseJson(
    StateInterpretationSource source,
    ReadOnlySpan<byte> utf8Proposal)
    -> StateInterpretationProposal
```

Parser requirements:

- validate trusted StateInterpretationSource before untrusted JSON;
- non-empty UTF-8;
- UTF-8 BOM rejected;
- maximum input size 1,048,576 bytes inclusive;
- JSON maximum depth 8;
- comments rejected;
- trailing commas rejected;
- malformed UTF-8/JSON rejected;
- duplicate decoded property names rejected at every object level;
- unknown properties rejected;
- missing properties rejected;
- exact property names/case;
- exact supported enum strings;
- wrong JSON types rejected;
- trailing content rejected;
- candidate identity contract/hash must match source exactly;
- all Character IDs must be exact current roster members where required;
- all RecordIds must be syntactically valid;
- EvidenceRecordIds canonicalized ordinally;
- exact duplicate semantic mutations rejected;
- exception representation sanitized and must not echo mutation Text, arbitrary unknown property names/values, raw JSON snippets, or provider content.

No provider/model call occurs.

## 27. Mutation ordering

The `mutations` array order is preserved exactly in the semantic proposal for experimental reconstruction.

Array order does **not** grant commit precedence or imply that State Authority must apply mutations sequentially.

Later State Authority must define deterministic conflict/stale-state rules over the complete proposal batch.

Patch 0009 does not resolve conflicting non-identical mutation proposals.

## 28. Duplicate versus conflicting mutation proposals

Exact duplicate semantic mutations are parser errors because they add no information and could create accidental double application.

Non-identical proposals that target the same record/domain are permitted as Interpreter output because probabilistic interpretation may be internally inconsistent.

Such conflict is not silently repaired or ranked by Patch 0009. Later deterministic State Authority must reject/resolve under its own approved rules.

## 29. No hidden rewriting

Patch 0009 preserves Interpreter output exactly after parsing/validation.

It does not:

- rewrite prose;
- summarize mutation text;
- merge two proposals;
- repair contradictory proposals;
- convert belief/suspicion/claim to knowledge/fact;
- auto-promote WorldState;
- auto-resolve UnresolvedProposition;
- infer Observation;
- manufacture Relationship/Disposition changes absent output;
- inject “better” consequences.

## 30. State Interpreter is proposal authority only

```text
StateInterpretationProposal != approved mutation
StateMutationCandidate != committed consequence
StateInterpretationProposal != accepted Take
```

The Interpreter may propose what a Performance may mean.

Only later deterministic State Authority may approve/reject candidate mutations.

Only later Take/commit authority may make accepted Performance + approved consequences causal history.

## 31. No direct State mutation

Patch 0009 exposes no method that:

- accepts a ProductionState and returns a mutated ProductionState;
- applies/supersedes/deactivates records;
- allocates authoritative RecordIds;
- changes fixture state;
- changes Character state;
- changes Relationship/Pressure/World state;
- writes history;
- persists files.

Any State Interpreter implementation that directly mutates authority violates a frozen E0 hard gate.

## 32. No truth promotion

WorldState/SceneState/Knowledge proposals are **not** truth/knowledge merely because their domain names say what they would become if approved.

Examples:

- Character says “the raft broke loose” -> may be a CharacterClaim/Belief proposal; it does not automatically justify WorldState;
- Character guesses Marlowe moved the raft -> may support Suspicion/Belief; it does not justify Knowledge or HistoricalTruth;
- Character performs an explicit physical action -> Interpreter may propose WorldState/SceneState consequence, but State Authority still decides whether action was possible/authorized and transition legal.

Patch 0009 has no objective-truth contradiction or truth-promotion authority.

## 33. Disposition/Relationship conservatism remains downstream authority

Interpreter may propose CharacterDisposition or Relationship changes because Blueprint 0.1 permits possible durable social effects.

But proposal != approval.

The later State Authority contract must enforce:

- Disposition change is rare/conservative in E0;
- immediate social effect is not automatically durable identity change;
- relationship evolution remains semantic/causal rather than fake numeric psychology;
- creator locks/transition rules remain deterministic.

Patch 0009 does not invent scores or thresholds.

## 34. Observation boundary remains reserved

State Interpreter cannot emit `CharacterObservation`.

A future observation boundary must determine perception eligibility before authoritative Observation can exist.

Character Memory/Belief/Suspicion proposals do not retroactively prove Observation.

## 35. World Resolver remains separate

Patch 0009 WorldState/SceneState proposals are consequences of the interpreted Character Performance.

They are not autonomous non-Character world evolution.

Future weather/scheduled events/resource depletion/possibility resolution without Character causation remains World Resolver territory and outside Patch 0009.

## 36. Provider/input-composition boundary remains open

Patch 0009 does not freeze what exact Production information a future State Interpreter provider receives.

Future input composition must:

- disclose only information needed for interpretation;
- preserve provider-attempt disclosure provenance;
- separate trusted authority state from untrusted Performance text;
- not treat Character dialogue as instructions;
- avoid assuming the Interpreter needs the whole Production by default;
- respect credential/privacy boundaries.

The Interpreter is not a Character, so Character Access Control is not automatically its information policy; a separate bounded orchestration disclosure contract is required later.

## 37. E0 control isolation

Patch 0009 applies directly to E0 paths using the per-Character Candidate/Integrity spine, including the E0-A reference path and compatible B/C/D/G paths.

E0-F may inject malformed/stale/conflicting Interpreter output to prove fail-closed behavior.

E0-E single-playwright control must **not** be forced through this Candidate-specific parser merely to resemble Ensemble internals. Its consequence/integrity protocol must be defined as part of the playwright-control contract while still satisfying frozen hard integrity/atomic-causality requirements.

## 38. Determinism

For identical:

```text
StateInterpretationSource
+ exact utf8Proposal bytes that parse to equivalent JSON structure/content
+ contract versions
```

semantic output must be deterministic.

No clock, random source, culture, filesystem, network, provider/model, AI inference, GPU/NPU, mutable global state, or hidden state affects parsing/canonicalization.

Property order/insignificant whitespace do not affect semantic output. Mutation array order remains semantic/preserved. EvidenceRecordIds are canonicalized ordinally.

## 39. Exception/failure boundary

Use one small State Interpreter contract exception domain.

Failures include:

- malformed trusted source;
- non-Accept/mismatched Integrity evaluation;
- malformed transport;
- unsupported schema/domain/operation;
- invalid domain cardinality;
- invalid operation field combination;
- invalid Character/Record IDs;
- invalid mutation text;
- stale Candidate content hash binding;
- exact duplicate semantic mutation.

Failure creates no fallback fictional consequence, no State mutation, no retry authorization, and no Take disposition.

## 40. Required tests

Use existing canonical Missing Raft Context/Candidate/Integrity paths. Do not duplicate fixture JSON or add public test bypasses for impossible upstream authority states.

### Source binding

1. canonical Voss Context + Candidate + Patch 0008 Accept binds successfully;
2. source constructor non-public;
3. non-Accept Integrity evaluation fails;
4. fresh Integrity input deterministic Reject fails source Bind;
5. evaluation/input Candidate content hash mismatch fails;
6. evaluation/source ContextPacket identity mismatch fails;
7. unsupported Integrity ValidationContract fails;
8. malformed impossible upstream states covered by defensive/reflection review without public bypass;
9. synthetic Accept may bind for tests but Source exposes no authenticity/authority flag;
10. Source exact public surface contains no Candidate VisibleText/private Context/Take/State/provider fields;
11. roster stored ordinally and exactly three.

### Proposal transport/surface

12. proposal constructor non-public;
13. exact semantic fields ContractVersion/content-identity/hash/Mutations only;
14. valid empty mutations parses;
15. valid representative Add parses;
16. property order/whitespace insignificant;
17. candidate hash mismatch fails;
18. candidate identity contract mismatch fails;
19. unknown/missing/duplicate properties fail;
20. wrong types/trailing content/comments/trailing comma/BOM fail;
21. exactly 1 MiB envelope allowed when otherwise valid; +1 byte rejected;
22. depth >8 rejected;
23. exception messages do not echo mutation text/raw JSON.

### Domain rules

24. every frozen E0 domain parses under valid cardinality;
25. global domain rejects subject/target IDs;
26. Character domain requires roster subject and null target;
27. Relationship requires distinct roster subject+target;
28. non-roster Character ID fails;
29. CharacterClaim requires source subject;
30. CharacterClaim rejects Supersede/Deactivate;
31. no Constitution domain exists;
32. no HistoricalTruth domain exists;
33. no Observation domain exists;
34. no PresentationPerspective/Director opportunity domain exists.

### Operation rules

35. Add requires null ExistingRecordId + non-empty Text;
36. Supersede requires ExistingRecordId + non-empty Text;
37. Deactivate requires ExistingRecordId + null Text;
38. destructive Delete/Replace operation strings rejected;
39. RecordId syntax checked but existence/domain not claimed;
40. invalid/stale evidence RecordId syntax fails;
41. EvidenceRecordIds deduplicated? No — duplicates fail before canonical order;
42. EvidenceRecordIds canonical ordinal output;
43. exact duplicate semantic mutations fail;
44. conflicting non-identical mutations remain representable for later State Authority decision.

### Text/truth/authority

45. text preserved exactly with no trim/rewrite;
46. non-NFC text fails;
47. prohibited Control scalar fails;
48. invisible-only text fails;
49. empty mutation list means no durable mutation, not rejection;
50. Claim proposal remains distinct from WorldState/Knowledge;
51. Suspicion/Belief/Memory can differ from truth without parser rejection;
52. WorldState proposal creates no truth/State mutation;
53. Disposition/Relationship proposal creates no durable change;
54. no confidence/score/rationale fields;
55. no State apply/commit/RecordId-allocation API;
56. no TakeId/CommitId/accepted-Take API;
57. no provider/model/retry/spend API;
58. no Director/opportunity mutation.

### Regression

59. Patch 0008 canonical Candidate content hash unchanged;
60. Missing Raft StructuredContextHash unchanged;
61. Missing Raft RenderedContextHash unchanged;
62. Missing Raft ECJ-1 9112 bytes/hash unchanged;
63. all existing 253 Core tests remain green;
64. Missing Raft Harness PASS/0;
65. smoke Harness PASS/0.

## 41. ARM64/battery suitability

Patch 0009 implementation should be tiny deterministic CPU work:

- strict bounded UTF-8 JSON parsing;
- small enum/ID validation;
- Unicode validation;
- small immutable-array canonicalization.

No provider/network/background work, embeddings, AI inference, GPU/NPU, filesystem, or polling.

No NPU claim.

## 42. Explicit exclusions

No:

- State Interpreter provider/model call;
- Interpreter prompt/input composer;
- assessor/provider provenance authentication;
- provider-attempt identity/persistence;
- retry/spend/cancellation execution;
- provisional/accepted/rejected/alternate Take object/ID semantics;
- deterministic State Authority decisions;
- ProductionState/StateHash;
- authoritative RecordId allocation;
- mutation application;
- atomic Performance+consequence commit;
- causal history persistence/recovery;
- effective Current Opportunity mutation;
- opportunity-history append;
- Scene loop;
- full observation engine;
- World Resolver;
- E0-E playwright consequence protocol;
- post-E0 ontology;
- final Studio/Archive mutation UI;
- WinUI;
- Windows AI/NPU;
- packaging/WACK/Store.

## 43. Recursive audit dimensions

After every material correction restart from the top and test:

1. frozen State Interpreter/State Authority separation;
2. Integrity -> Interpreter sequencing;
3. Integrity Accept != accepted Take;
4. provisional-Take ordering remains open;
5. creator-ontology extensibility guard;
6. E0 category fidelity;
7. Constitution read-only;
8. claim/belief/suspicion/memory/knowledge/truth separation;
9. Observation boundary;
10. World Resolver separation;
11. historical texture vs durable consequence;
12. append-only/supersession semantics;
13. proposal vs authority;
14. creator locks deferred to State Authority;
15. Disposition/Relationship conservatism;
16. source Candidate content binding;
17. Integrity-evaluation structural binding vs assessor authentication;
18. evidence/provenance sufficiency;
19. stale/hallucinated RecordId non-authority;
20. least-privilege public surface;
21. untrusted-text isolation;
22. hidden rewrite prohibition;
23. transport strictness/resource bounds;
24. deterministic immutable ordering;
25. conflicting proposal handling;
26. E0-A/B/C/D/F/G compatibility;
27. E0-E control isolation;
28. experimental provenance reconstruction;
29. public API/non-forgeability/minimality;
30. executable-vs-defensive testability;
31. Engineering Hygiene Constitution;
32. ARM64 suitability;
33. scope exclusions and validation-claim discipline.

Approval only after one complete restart returns zero material corrections or worthwhile architectural improvements.

## 44. Material approval decisions

Approval would freeze only these Patch 0009 E0 decisions:

1. Patch 0009 defines State Interpreter mutation proposal schema/parser only, not provider execution or State mutation;
2. structural source Bind requires a Patch 0008 Accept evaluation for exact Candidate content identity but does not authenticate assessor provenance or accepted-Take status;
3. provisional-Take-versus-Interpreter ordering remains open;
4. semantic contract `ensemble.e0.state-interpreter.proposal.v1`;
5. JSON schema `ensemble.e0.state-interpreter.proposal-json.v1`;
6. StateInterpretationSource exposes only Candidate content identity, source Character/Context IDs, canonical three-Character roster, and Integrity ValidationContract;
7. empty mutation list is valid and means no durable projected-state proposal;
8. E0-only mutation domains are WorldState, SceneState, UnresolvedProposition, CharacterKnowledge, CharacterBelief, CharacterSuspicion, CharacterMemory, CharacterGoal, CharacterDisposition, CharacterCircumstance, CharacterClaim, Relationship, Pressure;
9. those domains are E0 authority vocabulary, not final creator-facing/post-E0 ontology;
10. Constitution, HistoricalTruth, Observation, PresentationPerspective, and Director opportunity cannot be proposed by State Interpreter;
11. operations are Add/Supersede/Deactivate only; no destructive Delete/silent Replace;
12. global/Character/Relationship target cardinality rules are exact;
13. CharacterClaim is Add-only and attributable only to source Character;
14. ExistingRecordId/EvidenceRecordIds are syntactic proposal references only and gain no existence/authority from parsing;
15. Candidate content hash is root source binding; no attempt/Take/commit identity is invented;
16. mutation text exact/NFC/nonempty where required; no hidden rewriting;
17. EvidenceRecordIds canonical ordinal, duplicate-free, may be empty, and carry no assessor/disclosure/authenticity claim;
18. mutation array order is preserved for reconstruction but grants no commit precedence;
19. exact duplicate mutations fail; conflicting non-identical proposals remain for later deterministic State Authority;
20. parser strictness uses 1 MiB inclusive envelope, depth 8, strict decoded duplicate/unknown/missing/type/trailing rules;
21. proposal has no confidence/rationale/score/provider/Take/State/commit/Director authority fields;
22. no WorldState/Knowledge/Disposition/etc proposal becomes truth/state merely by parsing;
23. observation and autonomous world evolution remain separate future authority paths;
24. E0-E playwright control is not forced through this Candidate-specific schema;
25. later deterministic State Authority must validate record existence/domain, locks, transition legality, stale/conflicting proposals, causal evidence, and conservative durable change;
26. post-E0 ontology remains open under Creator Ontology Extensibility Guard;
27. no provider, Take, State Authority, commit, persistence, Scene-loop, UI, Windows AI/NPU, or Store scope enters Patch 0009.

Implementation remains blocked until recursive audit completes and the user explicitly approves the final proposal.
