# H1 Patch 0010 — E0 Deterministic State Authority Review/Decision Contract

Status: blueprint proposal 0.6 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; implementation not started
Parent baseline: machine-validated H1 Patch 0009
Branch: `h1-patch-0010-state-authority-blueprint`

## 1. Purpose

Define the next E0 deterministic-spine boundary after the machine-validated State Interpreter proposal contract:

```text
authoritative E0 state snapshot
+ Patch 0009 StateInterpretationSource
+ Patch 0009 StateInterpretationProposal
    -> StateAuthorityInput.Bind
        -> exact Interpreter-proposal semantic content identity
        -> prose-free structural mutation projection
            -> StateAuthorityReviewSet.Bind
            -> deterministic hard-rule evaluation
            -> explicit E0 review policy
                -> StateAuthorityEvaluation
                    -> ordered Approved / Rejected / RequiresReview decisions
                        -> later Take + atomic causal-commit authority
```

Patch 0010 defines deterministic mutation review/decision rules only.

It does **not** mutate Production state, allocate authoritative new RecordIds, create or accept a Take, allocate a TakeId or CommitId, append causal history, persist anything, establish Current Opportunity, trigger another Performer, or call any model/provider.

`Approved` means only that a proposed consequence passed this deterministic State Authority decision contract for later atomic-commit consideration. It does not mean the consequence is already true, stored, or visible to future Characters.

## 2. Recovered frozen authority

Blueprint 0.1 freezes the sequence:

1. Performer candidate output;
2. Director opportunity selection;
3. Integrity Validator;
4. State Interpreter candidate-mutation schema;
5. **deterministic State Authority commit rules**;
6. accepted/rejected/alternate Take semantics needed by E0;
7. atomic causal-commit record.

Frozen law:

- probabilistic systems propose; deterministic authority decides;
- `Integrity Validator -> State Interpreter -> deterministic State Authority`;
- State Interpreter proposes meaning/consequences and never mutates authority directly;
- State Authority decides under creator locks, type rules, transition rules, confidence/review policy, and causal evidence;
- Constitution is read-only in E0;
- Disposition changes are rare and conservative;
- Circumstance may change frequently when supported;
- creator-locked canon cannot be changed;
- a claim cannot silently become fact;
- technical failure cannot become fictional action;
- truth, possibility, observation, claim, belief, memory, unresolved proposition, and provenance remain distinct;
- accepted Performance + approved consequences later form one atomic causal commit;
- E0 provenance preserves proposed mutations plus committed/rejected mutation decisions with reasons.

Blueprint 0.1 leaves consequence acceptance UX/modes open in ODR-19. Patch 0010 therefore does not freeze product modes such as Autopilot, Review, Strict Creator, or equivalent UI taxonomy.

## 3. Why Patch 0010 is decision logic, not mutation application

Current validated E0 authority has no ProductionState, StateHash, persisted active/inactive record lifecycle, authoritative new-RecordId allocator, TakeId, CommitId, or atomic event writer.

Applying mutations now would collapse three later boundaries: State Authority decision, Take semantics, and atomic causal commit.

Patch 0010 therefore produces deterministic decisions over an explicit authority-review snapshot while leaving effective state transition to the later atomic commit boundary.

## 4. State Authority is prose-blind by construction

The only rich boundary is:

```text
StateAuthorityInput.Bind(snapshot, source, proposal)
```

Bind may inspect the full Patch 0009 semantic Proposal only to validate exact Source/Proposal association, compute exact proposal semantic content identity, and project prose-free structural mutation metadata.

`DeterministicStateAuthority.Evaluate` never receives the Patch 0009 Proposal object and cannot access mutation Text.

The evaluator does not inspect Candidate VisibleText, Character Context prose, mutation Text, record Text, free-form rationale, provider/model output, hidden reasoning, or numeric model confidence.

## 5. Contracts

```text
StateAuthorityContractVersion = ensemble.e0.state-authority.review.v1
StateAuthorityPolicyContractVersion = ensemble.e0.state-authority.policy.v1
StateAuthorityProposalContentIdentityContract = ensemble.e0.state-authority.interpreter-proposal-content.v1
StateAuthorityReviewSetContractVersion = ensemble.e0.state-authority.review-set.v1
```

Patch 0010 defines no AI JSON transport and no provider schema.

## 6. Exact Interpreter-proposal semantic content identity

`StateAuthorityInput.Bind` computes lowercase SHA-256 over an explicit canonical serialization of the complete Patch 0009 semantic `StateInterpretationProposal`.

The identity contract itself is included in the preimage for cryptographic domain separation, matching the Patch 0008 Candidate-content identity pattern.

Canonical semantic JSON shape is exactly:

```json
{
  "identityContract": "ensemble.e0.state-authority.interpreter-proposal-content.v1",
  "contractVersion": "ensemble.e0.state-interpreter.proposal.v1",
  "candidateContentIdentityContract": "ensemble.e0.integrity.candidate-content.v1",
  "candidateContentHash": "...",
  "sourceSceneId": "SCENE-ID",
  "mutations": [
    {
      "domain": "worldState",
      "operation": "add",
      "subjectCharacterId": null,
      "targetCharacterId": null,
      "existingRecordId": null,
      "text": "exact semantic text",
      "supportingRecordIds": []
    }
  ]
}
```

Root property order is exactly: `identityContract`, `contractVersion`, `candidateContentIdentityContract`, `candidateContentHash`, `sourceSceneId`, `mutations`.

Every mutation object has exactly, in order: `domain`, `operation`, `subjectCharacterId`, `targetCharacterId`, `existingRecordId`, `text`, `supportingRecordIds`.

Domain uniquely determines the Patch 0009 semantic variant family, so no redundant `kind` field is serialized. Domain/operation tokens are the exact Patch 0009 transport tokens. Absent scalar fields are canonical JSON `null`.

Serialization law:

- UTF-8 without BOM;
- minified, no insignificant whitespace;
- fixed property order above;
- mutation order preserved;
- SupportingRecordIds emitted in Patch 0009 canonical ordinal order;
- Text exact and unmodified;
- existing Core `CanonicalJson.AppendString` / `CanonicalJson.EncodeUtf8` escaping/UTF-8 law;
- no reflection/property-order-dependent serializer.

Identity is over validated semantic Proposal content, not raw AI JSON. Raw whitespace/property-order differences that parse to the same semantic Proposal do not change identity.

This hash is content identity only, not Interpreter attempt identity, provider response identity, CandidateId, TakeId, CommitId, RecordId, or Production StateHash.

## 7. StateAuthoritySnapshot

```text
StateAuthoritySnapshot.Bind(
    ValidatedFixture fixture,
    ImmutableArray<RecordId> creatorLockedRecordIds)
    -> StateAuthoritySnapshot
```

Public shape:

```text
StateAuthoritySnapshot
- SceneId
- RosterCharacterIds
- Records
```

The snapshot is a deterministic prose-free structural projection of authoritative E0 fixture state plus an exact-record creator-lock overlay. It is not ProductionState and has no durable StateHash.

Fixture identity/version remain run provenance outside the snapshot rather than masquerading as future state identity.

The current public factory is fixture-based because that is the only validated authoritative state representation today. Patch 0010 executable validation therefore proves initial-fixture State Authority review only. Multi-turn evolved-state review remains unvalidated until later ProductionState/atomic-commit work supplies an evolved snapshot construction path.

## 8. Typed snapshot record descriptors

```text
StateAuthorityRecordDescriptor
    GlobalStateAuthorityRecordDescriptor
    CharacterStateAuthorityRecordDescriptor
    RelationshipStateAuthorityRecordDescriptor
```

Common fields: `RecordId`, `RecordDomain`, `Lifecycle`, `Protection`.
Character descriptor adds `SubjectCharacterId`.
Relationship descriptor adds `SubjectCharacterId` + `TargetCharacterId`.

No descriptor contains Text, provenance prose, confidence, score, provider identity, or model-authored rationale.

## 9. Structural record domains

```text
HistoricalTruth
UnresolvedProposition
WorldState
SceneState
CharacterConstitution
CharacterDisposition
CharacterCircumstance
CharacterObservation
CharacterKnowledge
CharacterBelief
CharacterSuspicion
CharacterMemory
CharacterGoal
CharacterClaim
Relationship
Pressure
```

`CharacterClaim` has no initial Missing Raft records but is included because Patch 0009 may propose new claim state later.

This remains E0 authority vocabulary only, not final creator-facing ontology.

## 10. Record lifecycle

```text
StateAuthorityRecordLifecycle
- Active
- Inactive
```

All initial fixture records are Active. Patch 0010 never changes lifecycle.

Future snapshots may retain Inactive records so causal history remains referenceable while stale Supersede/Deactivate targets fail. Supporting IDs may reference known inactive records; transition targets must be Active.

## 11. Record protection

```text
StateAuthorityRecordProtection
- None
- SystemImmutable
- CreatorLocked
```

Initial fixture projection marks HistoricalTruth, CharacterConstitution, and CharacterObservation as SystemImmutable.

Creator-lock overlay marks resolved non-SystemImmutable records CreatorLocked. Overlaying an already SystemImmutable record preserves SystemImmutable.

Every lock ID must resolve; duplicates fail. Protected records remain valid support references but cannot be Supersede/Deactivate targets. The fixture dialect gains no lock field.

Exact-record lock cannot prove whether a separate Add semantically contradicts a locked record. State Authority remains prose-blind; objective-reality additions are therefore MandatoryReview, and E0 canon requiring mechanical immutability should use already-protected authority distinctions where applicable.

## 12. Snapshot canonicalization

Bind requires initialized IDs, exactly three unique E0 roster Characters, globally unique RecordIds, valid relationship subject/target roster membership, and resolvable creator locks.

Canonical order: roster CharacterId ordinal; records RecordId ordinal; creator-lock input order irrelevant.

Snapshot binding does not rerun fixture semantic validation or alter fixture content.

## 13. StateAuthorityInput rich bind

```text
StateAuthorityInput.Bind(
    StateAuthoritySnapshot snapshot,
    StateInterpretationSource source,
    StateInterpretationProposal proposal)
    -> StateAuthorityInput
```

No public constructor.

Bind requires:

1. initialized snapshot/source/proposal;
2. current Patch 0009 proposal contract;
3. current Candidate-content identity contract;
4. `snapshot.SceneId == source.SourceSceneId == proposal.SourceSceneId`;
5. snapshot roster exactly equals Source roster;
6. proposal Candidate-content identity contract equals Source;
7. proposal Candidate-content hash equals Source;
8. every proposal mutation belongs to the current Patch 0009 semantic union.

Bind does not reparse Interpreter JSON, rerun Integrity, inspect concern evidence, or upgrade Patch 0009’s structural Integrity-Accept association into authenticated progression.

Patch 0008/0009 synthetic Accept remains synthetic for this boundary. Later effective orchestration still owns configured semantic-review/provider provenance authentication.

## 14. StateAuthorityInput public shape

```text
StateAuthorityInput
- ProposalContentIdentityContract
- ProposalContentHash
- Snapshot
- Mutations
```

No Source object, Proposal object, Candidate hash, SourceContextPacketId, SourceCharacterId, mutation Text, Context prose, record Text, Integrity evidence, provider/model information, Take, or commit authority.

Upstream Source/Proposal/Candidate identities remain separate provenance records. The exact ProposalContentHash binds this decision input back to one semantic proposal without giving the evaluator upstream Character/Performer data.

## 15. Prose-free structural mutation input

```text
StateAuthorityMutationInput
    GlobalStateAuthorityMutationInput
    CharacterStateAuthorityMutationInput
    RelationshipStateAuthorityMutationInput
```

Common: `MutationIndex`, `Domain`, `Transition`, `SupportingRecordIds`.
Character adds `SubjectCharacterId`.
Relationship adds `SubjectCharacterId` + `TargetCharacterId`.

Transition union:

```text
StateAuthorityTransition
    AddStateAuthorityTransition
    SupersedeStateAuthorityTransition(ExistingRecordId)
    DeactivateStateAuthorityTransition(ExistingRecordId)
```

No mutation input contains Text. Internal constructors must enforce approved domain families so invalid authority shapes remain difficult to express outside the binder too.

## 16. Explicit E0 review policy

```text
StateAuthorityPolicy.Create(
    ImmutableArray<StateMutationDomain> autoApproveDomains)
    -> StateAuthorityPolicy
```

Public shape: `ContractVersion`, `AutoApproveDomains`.

Policy is typed deterministic configuration only. It has no UI mode, model/provider field, free-form rationale, or numeric confidence threshold.

Duplicate/undefined enum values fail. AutoApproveDomains canonicalize by contract enum order. Policy cannot waive hard rejection or the mandatory-review floor, including transition-specific mandatory review.

An empty set means all structurally admissible mutations require review unless an explicit ReviewSet choice resolves them.

Policy selection is itself authority configuration. It must not be authored by the Performer/Interpreter or untrusted creative content. Later effective orchestration must retain/authenticate configured policy provenance before relying on policy-default decisions.

Patch 0010 does not choose the E0-A reference policy; that experimental setting remains to be frozen separately if stronger authority has not already done so.

## 17. No invented confidence score

Blueprint 0.1 names confidence/review policy, but Patch 0009 exposes no canonical confidence signal. Patch 0010 therefore invents no percentage, threshold, sentiment score, or model confidence field.

## 18. Mandatory-review floor for current E0 evidence

The following domains are always MandatoryReview regardless of transition:

```text
WorldState
SceneState
CharacterKnowledge
CharacterMemory
CharacterDisposition
Relationship
```

Additionally `UnresolvedProposition + Supersede` and `UnresolvedProposition + Deactivate` are MandatoryReview.

Reason:

- WorldState / SceneState change objective/current reality while no typed action-evidence channel proves enactment;
- CharacterKnowledge cannot be promoted from claim/guess by Interpreter assertion alone;
- CharacterMemory is explicit review while ODR-18 selective/false memory semantics remain open;
- CharacterDisposition is frozen rare/conservative;
- Relationship is durable social state requiring strong reviewable causal support rather than immediate social effect silently becoming durable state;
- adding an UnresolvedProposition preserves uncertainty, but superseding/deactivating one can materially reframe or remove uncertainty and therefore must not happen through automatic policy alone.

This is an E0 floor under current evidence, not a permanent post-E0 automation ban.

`StateAuthorityPolicy.Create` rejects any always-mandatory domain in AutoApproveDomains. `UnresolvedProposition` is allowed in policy because only its Add transition is policy-eligible; non-Add transitions remain MandatoryReview even when the domain appears in AutoApproveDomains.

## 19. Policy-eligible mutations

Subject to hard rules, explicit E0 policy may auto-approve:

```text
UnresolvedProposition + Add
CharacterBelief + any Patch 0009-valid transition
CharacterSuspicion + any Patch 0009-valid transition
CharacterGoal + any Patch 0009-valid transition
CharacterCircumstance + any Patch 0009-valid transition
CharacterClaim + Add
Pressure + any Patch 0009-valid transition
```

Auto-approval remains an explicit recorded E0 authority-policy default. It never changes the destination domain into fact.

## 20. Review choice and exact proposal binding

```text
StateAuthorityReviewChoice
- MutationIndex
- Choice

StateAuthorityReviewChoiceKind
- Approve
- Reject
```

Recommended creation:

```text
StateAuthorityReviewChoice.Approve(index)
StateAuthorityReviewChoice.Reject(index)
```

Choices are bound:

```text
StateAuthorityReviewSet.Bind(
    StateAuthorityInput input,
    ImmutableArray<StateAuthorityReviewChoice> choices)
    -> StateAuthorityReviewSet
```

Public shape:

```text
ContractVersion
ProposalContentIdentityContract
ProposalContentHash
Choices
```

No public constructor.

Bind requires choice count <= mutation count, nonnegative/in-range indices, no duplicate indices, defined choice enum, and canonical MutationIndex ordering.

ReviewSet copies exact ProposalContent identity from Input. `Evaluate` rejects any ReviewSet whose proposal identity differs from Input.

ReviewSet is synthetic-capable and does **not** authenticate that a human/creator/authorized workflow made the choices. Later effective commit must retain/authenticate review provenance before relying on explicitly reviewed approvals/rejections.

## 21. State Authority API and authority hierarchy

```text
DeterministicStateAuthority.Evaluate(
    StateAuthorityInput input,
    StateAuthorityPolicy policy,
    StateAuthorityReviewSet reviewSet)
    -> StateAuthorityEvaluation
```

No overload accepts Patch 0009 Proposal, Candidate VisibleText, Context prose, fixture prose, provider/model settings, or mutation rationale.

Decision authority hierarchy is exactly:

```text
hard deterministic rule
    > explicit review choice
        > policy default
```

Hard rules are unwaivable. For a structurally admissible mutation, an explicit review choice may confirm or override the configured policy default. This preserves creator/authorized-review authority without freezing final ODR-19 UX modes.

## 22. Hard-rule phase

Hard rules execute before explicit review or policy.

Per mutation, collect applicable hard reasons from this ordered subset:

```text
SupportingRecordMissing
ExistingRecordMissing
ExistingRecordInactive
ExistingRecordDomainMismatch
ExistingRecordSubjectMismatch
ExistingRecordTargetMismatch
ExistingRecordProtected
ConflictingExistingRecordTarget
```

Any hard reason makes the mutation Rejected. Policy and review cannot waive it.

If ReviewSet contains a choice for a hard-rejected mutation, the overall Evaluate call fails as miswired/stale review input rather than silently pretending the choice could override deterministic authority.

## 23. Supporting-record rule

Every SupportingRecordId must resolve to a known snapshot record. Missing support is hard rejection.

Known Active or future Inactive records may be support references.

Existence proves only referential integrity. It does not prove disclosure, semantic support, causal sufficiency, or truth authority.

## 24. Existing-record transition rule

For Supersede/Deactivate:

1. ExistingRecordId resolves;
2. target is Active;
3. record domain exactly matches mutation domain;
4. Character target belongs to same SubjectCharacterId;
5. Relationship target matches exact subject->target pair;
6. target Protection is None;
7. no other mutation targets the same resolved ExistingRecordId.

Violations are hard rejection. Patch 0010 never changes lifecycle.

## 25. Add rule

Add has no ExistingRecordId and receives no new RecordId in Patch 0010.

Approval means only that the exact semantics identified by ProposalContentHash are commit-eligible later.

No duplicate-text/contradiction/semantic-equivalence logic exists.

## 26. Structural batch conflict

If two or more Supersede/Deactivate mutations target the same resolved ExistingRecordId, all contenders receive `ConflictingExistingRecordTarget` and are Rejected.

No winner, ranking, merge, repair, or semantic comparison.

Multiple Adds are not structurally conflicting merely because they share domain/subject/pair; stronger future state cardinality rules may refine this if earned.

## 27. Review classification and exact reason composition

After hard rules, every surviving mutation follows exactly one default classification path, then an explicit review choice—if present—has precedence over that default.

A mutation is MandatoryReview when either its domain is one of the six always-mandatory domains in Section 18 or it is `UnresolvedProposition` with Supersede/Deactivate.

### Mandatory-review mutation
No choice:

```text
Disposition = RequiresReview
Reasons = [MandatoryReview]
```

Approve choice:

```text
Disposition = Approved
Reasons = [MandatoryReview, ExplicitReviewApproved]
```

Reject choice:

```text
Disposition = Rejected
Reasons = [MandatoryReview, ExplicitReviewRejected]
```

### Policy-eligible mutation whose domain is included in AutoApproveDomains
No choice:

```text
Disposition = Approved
Reasons = [PolicyAutoApproved]
```

Approve choice:

```text
Disposition = Approved
Reasons = [PolicyAutoApproved, ExplicitReviewApproved]
```

Reject choice:

```text
Disposition = Rejected
Reasons = [PolicyAutoApproved, ExplicitReviewRejected]
```

### Policy-eligible mutation whose domain is not auto-approved
No choice:

```text
Disposition = RequiresReview
Reasons = [PolicyReviewRequired]
```

Approve choice:

```text
Disposition = Approved
Reasons = [PolicyReviewRequired, ExplicitReviewApproved]
```

Reject choice:

```text
Disposition = Rejected
Reasons = [PolicyReviewRequired, ExplicitReviewRejected]
```

No explicit review choice can alter a hard-rejected mutation.

## 28. Full deterministic reason vocabulary/order

When applicable, reasons use this fixed order:

```text
1. SupportingRecordMissing
2. ExistingRecordMissing
3. ExistingRecordInactive
4. ExistingRecordDomainMismatch
5. ExistingRecordSubjectMismatch
6. ExistingRecordTargetMismatch
7. ExistingRecordProtected
8. ConflictingExistingRecordTarget
9. MandatoryReview
10. PolicyReviewRequired
11. PolicyAutoApproved
12. ExplicitReviewApproved
13. ExplicitReviewRejected
```

Reasons contain no creative Text or arbitrary snippets.

## 29. Evaluation shape

```text
StateAuthorityEvaluation
- ContractVersion
- Status
- Decisions
- Trace

StateAuthorityEvaluationStatus
- Complete
- ReviewRequired

StateAuthorityDecision
- MutationIndex
- Disposition
- Reasons

StateAuthorityDisposition
- Approved
- Rejected
- RequiresReview

StateAuthorityTrace
- Input
- Policy
- ReviewSet
```

No public constructors for authority-produced Evaluation/Decision/Trace.

Decisions preserve Proposal order through MutationIndex.

Any RequiresReview -> Status ReviewRequired.
All terminal Approved/Rejected -> Status Complete.
Empty mutation input -> Complete + zero decisions.

Neither status commits anything.

## 30. No claim-to-fact promotion

Claim, Belief, Suspicion, Memory, Knowledge, WorldState, and SceneState remain distinct.

Approval in one domain never generates another mutation or promotes another authority type.

WorldState/SceneState/Knowledge proposals are independently MandatoryReview even when related claim/belief mutations are approved.

## 31. Disposition / Relationship / Memory conservatism

Every CharacterDisposition, Relationship, and CharacterMemory mutation requires explicit review under Patch 0010.

No repetition counter, sentiment score, trust meter, arbitrary confidence threshold, or hidden semantic classifier.

Memory approval never changes objective history.
Relationship remains directional semantic state without numeric psychology.

## 32. Unresolved uncertainty conservatism

Adding an UnresolvedProposition may be policy-auto-approved because it preserves an explicit uncertainty boundary.

Superseding or deactivating an existing UnresolvedProposition always requires explicit review. Approval still does not by itself create objective truth; any related WorldState/SceneState/Knowledge proposal remains separately reviewed.

## 33. Circumstance and lower-authority semantics

Circumstance, Belief, Suspicion, Goal, Claim, UnresolvedProposition Add, and Pressure may be policy-auto-approved after hard rules, but an explicit review choice can still reject or confirm the individual mutation.

This is E0 review configuration, not final product UX or proof of semantic truth.

## 34. Policy/review authentication limits

StateAuthorityPolicy and StateAuthorityReviewSet are typed inputs, not credentials.

Patch 0010 does not prove who configured policy or who made explicit review choices.

No Performer, State Interpreter, provider output, or untrusted creative content may be treated as authority for policy/review selection.

Later effective commit/orchestration must preserve and authenticate the authority provenance it relies on before treating an Approved decision as commit-eligible in live Production.

## 35. Evaluation phase / stale-state boundary

Patch 0010 evaluation is pure and may be speculative or repeated.

A later atomic commit must either recompute State Authority against the then-current authoritative state or bind a completed evaluation to a future authoritative StateHash/current-state identity before application.

Patch 0010 invents no StateHash/stale-state commit protocol.

The full structural Snapshot remains in Input/Trace for reconstruction, but is not promoted into persistent Production-state identity.

## 36. No Take ordering decision

Patch 0010 does not decide when a provisional Take object exists relative to State Interpretation or State Authority.

It freezes only that RequiresReview cannot be treated as approved consequence authority.

Immediate Take ordering remains for the dedicated Take contract unless stronger project authority resolves it.

## 37. Complete is not effective

Complete means every proposed mutation is terminal Approved or Rejected under the supplied Snapshot, Policy, and ReviewSet.

It does not mean accepted Take, committed consequence, state changed, new RecordIds allocated, history appended, or future Context updated.

Only later atomic causal commit can make approved consequences effective together with accepted Performance.

## 38. Failure behavior

Malformed snapshot/input/policy/review-set structures produce a small sanitized State Authority exception domain.

Per-mutation hard-rule violations become typed Rejected decisions when overall input is valid.

Exceptions/reasons never echo mutation Text, record Text, raw JSON, provider content, credentials, or arbitrary snippets.

Failure creates no fiction and no state mutation.

## 39. Determinism

Identical valid Input + Policy + ReviewSet -> identical Evaluation semantics.

No clock/random/culture/filesystem/network/provider/model/GPU/NPU/global mutable state.

Canonical order:

- snapshot roster: CharacterId ordinal;
- snapshot records: RecordId ordinal;
- structural mutations/decisions: Proposal order / MutationIndex;
- policy domains: contract enum order;
- ReviewSet choices: MutationIndex ascending;
- reasons: fixed contract order.

## 40. Creator ontology guard

Patch 0010 evaluates E0 authority domains already frozen by Patch 0009. It does not turn them into final creator-facing UI or post-E0 storage ontology.

## 41. E0 control isolation

Patch 0010 directly applies to paths using Patch 0009 per-Character State Interpreter proposals.

E0-E single-playwright control is not forced through Candidate-specific Source/Interpreter APIs if that contaminates the control, but its eventual consequence authority must still satisfy deterministic authority, locked-canon protection, truth separation, and atomic accepted-Performance/consequence history.

## 42. ARM64 / battery

Tiny deterministic CPU work only: one explicit semantic proposal canonicalization + SHA-256 at Bind, immutable-array canonicalization, dictionary/set RecordId lookup, enum/domain checks, and bounded conflict detection.

No NPU is appropriate. Use average-O(1) lookups rather than pairwise scans.

## 43. Required tests / review gates

Use canonical upstream construction and public production paths. No public test bypass APIs.

### Proposal identity / least privilege
1. canonical Proposal -> lowercase 64-hex ProposalContentHash;
2. canonical hash preimage includes exact StateAuthorityProposalContentIdentityContract;
3. identical semantic Proposal -> identical hash;
4. raw JSON whitespace/property-order changes producing same semantic Proposal -> same hash;
5. mutation Text change -> hash changes while structural StateAuthorityMutationInput remains identical;
6. support ID change -> hash changes;
7. mutation order change -> hash changes;
8. Candidate/Scene identity change -> hash changes;
9. canonical semantic JSON exact fixed-property oracle;
10. no redundant `kind` property in canonical identity;
11. canonical string escaping reuses existing CanonicalJson law;
12. Input exact public fields are ProposalContentIdentityContract, ProposalContentHash, Snapshot, Mutations;
13. Input/evaluator exclude Proposal/Source/Candidate identity/Text/Context/provider/model;
14. evaluator API accepts only StateAuthorityInput + Policy + ReviewSet.

### Snapshot
15. Missing Raft fixture binds empty lock overlay;
16. snapshot constructor non-public;
17. exact public fields SceneId/RosterCharacterIds/Records only;
18. exact canonical trio roster;
19. every frozen fixture RecordId represented exactly once;
20. exact descriptor domain/subject/pair mapping;
21. HistoricalTruth/Constitution/Observation SystemImmutable;
22. other initial records unprotected;
23. creator lock marks mutable record CreatorLocked;
24. duplicate/unknown lock IDs fail;
25. SystemImmutable overlay stays SystemImmutable;
26. snapshot excludes fixture identity/version/Text/prose/provider/model/apply APIs;
27. records RecordId ordinal;
28. all initial records Active.

### Source/proposal binding
29. canonical Patch 0009 Source+Proposal binds;
30. Scene mismatch fails;
31. roster mismatch fails;
32. Candidate identity/hash mismatch fails during Bind even though those fields are not retained in Input;
33. unsupported Patch 0009 contract fails;
34. Bind does not rerun Integrity/Interpreter parsing;
35. synthetic Patch 0008 Accept association remains non-authenticated;
36. structural mutation projection preserves index/domain/transition/IDs/supports and drops Text.

### ReviewSet / policy binding
37. empty Policy valid;
38. duplicate/undefined policy domains fail;
39. always-mandatory domains cannot auto-approve;
40. UnresolvedProposition is valid in Policy because only Add is policy-eligible;
41. Policy public surface has no confidence/UI mode/provider;
42. ReviewChoice factory rejects negative index;
43. ReviewSet constructor non-public;
44. duplicate/out-of-range/undefined choices fail;
45. ReviewSet canonicalizes index order;
46. ReviewSet copies ProposalContentHash;
47. ReviewSet for different Input ProposalContentHash fails Evaluate;
48. choice count cannot exceed mutation count.

### Hard transition rules
49. valid Add survives hard rules;
50. valid Supersede resolves exact active record;
51. valid Deactivate resolves exact active record;
52. missing ExistingRecord -> Reject;
53. inactive target -> Reject when later internal evolved snapshot construction exists; current patch static/internal invariant gate;
54. wrong domain -> Reject;
55. wrong Character subject -> Reject;
56. wrong relationship pair -> Reject;
57. SystemImmutable target -> Reject;
58. CreatorLocked target -> Reject;
59. missing support -> Reject;
60. known inactive support remains referenceable in future snapshot;
61. same resolved ExistingRecord targeted twice -> all contenders Reject;
62. no conflict winner/merge;
63. explicit review choice for hard-rejected mutation fails Evaluate.

### Review algorithm
64. each always-mandatory domain -> RequiresReview without choice;
65. UnresolvedProposition Add follows policy classification;
66. UnresolvedProposition Supersede -> MandatoryReview;
67. UnresolvedProposition Deactivate -> MandatoryReview;
68. mandatory + Approve -> Approved `[MandatoryReview, ExplicitReviewApproved]`;
69. mandatory + Reject -> Rejected `[MandatoryReview, ExplicitReviewRejected]`;
70. eligible absent policy + no choice -> RequiresReview `[PolicyReviewRequired]`;
71. eligible absent policy + Approve -> Approved `[PolicyReviewRequired, ExplicitReviewApproved]`;
72. eligible absent policy + Reject -> Rejected `[PolicyReviewRequired, ExplicitReviewRejected]`;
73. eligible in policy + no choice -> Approved `[PolicyAutoApproved]`;
74. eligible in policy + Approve -> Approved `[PolicyAutoApproved, ExplicitReviewApproved]`;
75. eligible in policy + Reject -> Rejected `[PolicyAutoApproved, ExplicitReviewRejected]`;
76. UnresolvedProposition domain in policy does not auto-approve Supersede/Deactivate;
77. explicit review takes precedence over policy but never hard rule;
78. ReviewSet choice order does not affect result.

### Evaluation
79. decision order exact MutationIndex/Proposal order;
80. reason order exact contract order;
81. any pending -> ReviewRequired status;
82. all terminal -> Complete;
83. empty mutations -> Complete / zero decisions;
84. Evaluation/Decision/Trace constructors non-public;
85. Trace contains exact prose-free Input/Policy/ReviewSet;
86. Approved exposes no state-applied/Take/Commit authority;
87. no creative Text in Input/Trace/reason messages.

### Truth / scope
88. approved Claim remains Claim only;
89. Belief/Suspicion/Memory approval cannot create WorldState/Knowledge;
90. objective domains cannot policy-auto-approve;
91. Disposition/Relationship/Memory cannot policy-auto-approve;
92. unresolved uncertainty cannot be auto-removed/replaced;
93. creator lock cannot be overridden by review;
94. explicit review may override policy auto-approval;
95. no State apply/NewRecordId/Take/Commit/provider API;
96. no numeric psychology/confidence.

### Regression
97. existing 330 Core tests green;
98. Patch 0008 Candidate hash oracle unchanged;
99. Missing Raft Context hashes unchanged;
100. Missing Raft ECJ-1 9112 bytes/hash unchanged;
101. Missing Raft Harness PASS/0;
102. smoke Harness PASS/0.

## 44. Explicit validation limit

Patch 0010 machine validation, when implemented, can prove fixture-derived initial-snapshot State Authority review, exact proposal binding/prose-free evaluation, creator-lock overlay, deterministic policy/review decisions and overrides, operation-sensitive uncertainty protection, and regressions.

It cannot yet prove multi-turn evolved-state review because no ProductionState/evolved snapshot constructor exists. That higher validation level belongs to later state-application/atomic-commit work.

## 45. Explicit exclusions

No Interpreter provider/input composer; provider-attempt authentication; final consequence UX modes; authenticated creator-review UI; confidence model; ProductionState; StateHash; new RecordId allocation; mutation application; Take semantics/TakeId; CommitId; atomic causal commit; persistence/recovery; opportunity application; Scene loop; Observation engine; World Resolver; memory-forgetting design; final ontology; E0-E protocol; WinUI; Windows AI/NPU execution; packaging/WACK; Store certification.

## 46. Recursive audit dimensions

Restart after every material correction:

1. Interpreter vs State Authority;
2. State Authority vs commit;
3. State Authority vs Take;
4. proposal non-authority;
5. proposal-content identity/domain separation;
6. prose-blind least privilege;
7. Integrity Accept non-authentication;
8. locks/SystemImmutable;
9. truth/claim/knowledge separation;
10. possibility/unresolved-proposition transition separation;
11. memory ODR-18;
12. Disposition/Relationship conservatism;
13. Circumstance fluidity;
14. ODR-19 policy openness;
15. explicit review vs policy authority hierarchy;
16. no fake confidence;
17. causal evidence limits;
18. support existence vs sufficiency;
19. transition/domain/ownership rules;
20. lifecycle;
21. batch conflict;
22. no semantic keyword judge;
23. creator ontology guard;
24. exact Source/Proposal/Snapshot binding;
25. review choice exact proposal binding;
26. stale-state/future StateHash;
27. RequiresReview nonterminal semantics;
28. policy/review provenance authentication limits;
29. reconstruction/provenance;
30. E0 control isolation;
31. deterministic ordering;
32. invalid-state representability;
33. API minimality;
34. exception safety;
35. testability;
36. current-vs-multiturn validation level;
37. Hygiene Constitution;
38. ARM64/battery;
39. total scope/claims.

## 47. Material approval decisions

Approval would freeze only:

1. Patch 0010 as deterministic review/decision logic, not state application;
2. sole rich `StateAuthorityInput.Bind(snapshot, source, proposal)` boundary;
3. exact semantic ProposalContentHash with its identity contract inside the canonical preimage and existing CanonicalJson law;
4. evaluator prose-blind structural input containing only proposal identity, Snapshot, and mutation structure;
5. Snapshot containing only SceneId/roster/structural records plus exact-record lock overlay at construction;
6. typed record descriptors/lifecycle/protection;
7. SystemImmutable HistoricalTruth/Constitution/Observation;
8. typed structural mutation projection;
9. explicit E0 auto-approve-domain Policy, not final product mode;
10. no confidence scalar;
11. hard deterministic rules > explicit review choice > policy default;
12. always-mandatory review for WorldState, SceneState, Knowledge, Memory, Disposition, Relationship;
13. operation-sensitive mandatory review for UnresolvedProposition Supersede/Deactivate;
14. policy-eligible UnresolvedProposition Add, Belief, Suspicion, Goal, Circumstance, Claim, Pressure;
15. ReviewSet bound to exact ProposalContentHash;
16. policy/review inputs are non-authenticating and cannot come from untrusted creative/model output;
17. hard structural rules are unwaivable;
18. support existence != semantic sufficiency;
19. exact ExistingRecord active/domain/owner/pair/protection rules;
20. same ExistingRecord batch conflict rejects all contenders;
21. exact reason-composition algorithm including explicit review override of policy;
22. Approved/Rejected/RequiresReview + Complete/ReviewRequired semantics;
23. Approved is later commit-eligible only;
24. no StateHash yet; later commit must recompute or bind current state;
25. executable validation limited to fixture-derived initial snapshots until evolved state exists;
26. Take ordering remains open;
27. no ProductionState/StateHash/RecordId allocation/application/Take/commit/persistence/provider/UI/NPU/Store scope.

Implementation remains blocked until recursive audit completes and the user explicitly approves the final proposal.
