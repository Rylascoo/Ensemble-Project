# H1 Patch 0010 — E0 Deterministic State Authority Review/Decision Contract

Status: blueprint proposal 0.2 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; implementation not started
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
            -> deterministic hard-rule evaluation
            -> explicit E0 review policy
            -> optional explicit review choices
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
- objective truth, possibility, observation, claim, belief, memory, rumor, unresolved proposition, and provenance remain distinct;
- accepted Performance + approved consequences later form one atomic causal commit;
- E0 provenance preserves proposed mutations plus committed/rejected mutation decisions with reasons.

Blueprint 0.1 leaves consequence acceptance UX/modes open in ODR-19. Patch 0010 therefore does not freeze product modes such as Autopilot, Review, Strict Creator, or equivalent UI taxonomy.

## 3. Why Patch 0010 is decision logic, not mutation application

Current validated E0 authority has no ProductionState, StateHash, persisted active/inactive record lifecycle, authoritative new-RecordId allocator, TakeId, CommitId, or atomic event writer.

Applying mutations now would collapse three later boundaries:

- State Authority decision;
- Take semantics;
- atomic causal commit.

Patch 0010 therefore produces deterministic decisions over an explicit authority-review snapshot while leaving effective state transition to the later atomic commit boundary.

## 4. State Authority is prose-blind by construction

Patch 0010 must not create a second semantic model or hidden keyword judge.

The rich boundary is:

```text
StateAuthorityInput.Bind(snapshot, source, proposal)
```

Bind may inspect the full Patch 0009 semantic Proposal only to:

- validate exact Source/Proposal association;
- compute exact proposal semantic content identity;
- project prose-free structural mutation metadata.

`DeterministicStateAuthority.Evaluate` never receives the Patch 0009 Proposal object and cannot access mutation Text.

The evaluator does not inspect:

- Candidate VisibleText;
- Character Context prose;
- mutation Text;
- fixture/state record Text;
- free-form rationale;
- provider/model output;
- hidden reasoning;
- numeric model confidence.

This prevents future deterministic authority from drifting into brittle semantic keyword rules.

## 5. Contracts

```text
StateAuthorityContractVersion = ensemble.e0.state-authority.review.v1
StateAuthorityPolicyContractVersion = ensemble.e0.state-authority.policy.v1
StateAuthorityProposalContentIdentityContract = ensemble.e0.state-authority.interpreter-proposal-content.v1
```

Patch 0010 defines no AI JSON transport and no provider schema.

## 6. Exact Interpreter-proposal semantic content identity

`StateAuthorityInput.Bind` computes a lowercase SHA-256 over an explicit deterministic canonical serialization of the complete Patch 0009 semantic `StateInterpretationProposal`.

The identity covers exactly:

- Patch 0009 proposal ContractVersion;
- CandidateContentIdentityContract;
- CandidateContentHash;
- SourceSceneId;
- mutation array in semantic order;
- for every mutation: semantic variant family, domain, operation, subject/target IDs where applicable, ExistingRecordId where applicable, exact Text where applicable, and canonical SupportingRecordIds.

Canonical serialization is implementation-owned and explicit:

- UTF-8 without BOM;
- minified JSON;
- fixed property order;
- fixed exact enum/string tokens;
- explicit nulls where the canonical identity schema specifies them;
- no reflection/property-order-dependent serializer;
- mutation order preserved;
- SupportingRecordIds already canonical ordinal under Patch 0009;
- Text exact and unmodified.

Whitespace/property order from raw AI JSON is irrelevant because identity is over the validated semantic Proposal, not the raw provider response.

This hash is **content identity only**. It is not:

- Interpreter attempt identity;
- provider response identity;
- CandidateId;
- TakeId;
- CommitId;
- RecordId;
- Production StateHash.

Two different Interpreter proposals for the same Candidate must produce different proposal-content hashes whenever any covered semantic content differs, including mutation Text.

## 7. StateAuthoritySnapshot

Patch 0010 introduces a structural authority-review snapshot, not a ProductionState replacement.

Public E0 construction:

```text
StateAuthoritySnapshot.Bind(
    ValidatedFixture fixture,
    ImmutableArray<RecordId> creatorLockedRecordIds)
    -> StateAuthoritySnapshot
```

The snapshot is a deterministic structural projection of the authoritative E0 fixture plus an explicit creator-lock overlay.

Public shape:

```text
StateAuthoritySnapshot
- OriginFixtureId
- OriginFixtureVersion
- SceneId
- RosterCharacterIds
- Records
```

It contains no creative prose.

The current factory is fixture-based because that is the only validated authoritative state representation today. A later ProductionState/commit patch may add a construction path for evolved state without changing State Authority review semantics.

Patch 0010 does not define a durable StateHash or claim fixture identity is sufficient to identify future evolved state.

## 8. Snapshot record descriptors

Snapshot records are immutable typed structural descriptors:

```text
StateAuthorityRecordDescriptor
    GlobalStateAuthorityRecordDescriptor
    CharacterStateAuthorityRecordDescriptor
    RelationshipStateAuthorityRecordDescriptor
```

Common fields:

```text
- RecordId
- RecordDomain
- Lifecycle
- Protection
```

Character descriptor adds:

```text
- SubjectCharacterId
```

Relationship descriptor adds:

```text
- SubjectCharacterId
- TargetCharacterId
```

No descriptor contains Text, provenance prose, confidence, score, provider identity, or model-authored rationale.

## 9. Structural record domains

State Authority must describe both mutable proposal destinations and protected/support-only authority records.

E0 structural record domains:

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

All records projected from the frozen initial fixture are Active.

Lifecycle exists so later atomic state application can preserve superseded/deactivated records in causal history while preventing stale transition targets. Patch 0010 never changes lifecycle.

Supporting Record IDs may reference known inactive records in a future snapshot because historical causal records remain referenceable. Supersede/Deactivate may target only Active records.

## 11. Record protection

```text
StateAuthorityRecordProtection
- None
- SystemImmutable
- CreatorLocked
```

Fixture projection automatically marks as SystemImmutable:

- HistoricalTruth;
- CharacterConstitution;
- CharacterObservation.

The explicit `creatorLockedRecordIds` overlay marks resolved non-SystemImmutable records as CreatorLocked.

Rules:

- every creator-lock ID must resolve exactly once;
- duplicate creator-lock IDs fail;
- attempting to creator-lock an already SystemImmutable record is allowed but leaves protection SystemImmutable rather than changing meaning;
- SystemImmutable cannot be downgraded;
- creator locks are authority metadata, not mutation Text;
- protected records remain valid supporting references;
- protected records cannot be Supersede/Deactivate targets.

The current fixture dialect does not gain a new lock field in Patch 0010.

Exact-record locking cannot by itself prove whether an unrelated Add semantically contradicts locked canon. Under the current prose-blind evidence contract, objective-reality additions therefore remain MandatoryReview, and E0 canon that must be mechanically immutable should be represented through the already-protected authority distinctions such as HistoricalTruth/Constitution where applicable.

## 12. Snapshot canonicalization

`StateAuthoritySnapshot.Bind` validates and canonicalizes:

- initialized fixture/Scene/roster/record IDs;
- exactly three unique E0 roster Characters;
- globally unique RecordIds across all projected domains;
- creator-lock IDs resolve;
- descriptor family matches record domain;
- relationship subject/target are distinct roster Characters;
- records canonicalized by RecordId ordinal;
- roster canonicalized by CharacterId ordinal;
- creator-lock input order does not affect output.

It does not alter fixture content or rerun semantic interpretation.

## 13. StateAuthorityInput rich bind / least-privilege output

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
8. every proposal mutation is initialized and belongs to the current Patch 0009 semantic union.

Bind does not reparse Interpreter JSON and does not re-run Integrity or State Interpreter validation.

After validation it computes proposal semantic content identity and projects only structural mutation data.

## 14. StateAuthorityInput public shape

```text
StateAuthorityInput
- ProposalContentIdentityContract
- ProposalContentHash
- CandidateContentIdentityContract
- CandidateContentHash
- SourceSceneId
- Snapshot
- Mutations
```

It contains no Source object, Proposal object, Candidate text, mutation Text, Context prose, record Text, Integrity evidence, provider/model information, Take, or commit authority.

The input is an audit/reconstruction object, not a permission token.

## 15. Prose-free structural mutation input

```text
StateAuthorityMutationInput
    GlobalStateAuthorityMutationInput
    CharacterStateAuthorityMutationInput
    RelationshipStateAuthorityMutationInput
```

Common fields:

```text
- MutationIndex
- Domain
- Transition
- SupportingRecordIds
```

Character adds:

```text
- SubjectCharacterId
```

Relationship adds:

```text
- SubjectCharacterId
- TargetCharacterId
```

Transition is typed:

```text
StateAuthorityTransition
    AddStateAuthorityTransition
    SupersedeStateAuthorityTransition(ExistingRecordId)
    DeactivateStateAuthorityTransition(ExistingRecordId)
```

No structural mutation input contains Text.

Patch 0009 CharacterClaim, Knowledge, and Memory remain Add-only because Bind can only project the already-valid Patch 0009 semantic shapes.

## 16. Explicit E0 review policy

Patch 0010 needs deterministic review policy without freezing final product acceptance modes.

```text
StateAuthorityPolicy.Create(
    ImmutableArray<StateMutationDomain> autoApproveDomains)
    -> StateAuthorityPolicy
```

Public shape:

```text
StateAuthorityPolicy
- ContractVersion
- AutoApproveDomains
```

Properties:

- typed deterministic configuration only;
- no UI mode name;
- no model/provider identity;
- no numeric confidence threshold;
- duplicate domains fail;
- canonical domain ordering;
- policy must be retained in E0 provenance;
- policy cannot waive hard rejections;
- policy cannot auto-approve mandatory-review domains.

An empty `AutoApproveDomains` set means every structurally admissible mutation requires explicit review.

The authority/provenance of the policy configuration itself must be retained by later effective orchestration. Patch 0010 does not authenticate who selected the policy.

## 17. Why no confidence score

Blueprint 0.1 refers to confidence/review policy, but Patch 0009 carries no canonical confidence signal and Ensemble rejects false numerical precision.

Patch 0010 therefore does not invent a confidence scalar or threshold.

Future authenticated semantic evidence may earn another deterministic policy input. Until then, uncertainty is represented by explicit review requirements, not fake percentages.

## 18. Mandatory-review floor for E0

The following Patch 0009 domains cannot be auto-approved by StateAuthorityPolicy in Patch 0010:

```text
WorldState
SceneState
CharacterKnowledge
CharacterMemory
CharacterDisposition
Relationship
```

Reasoning:

- WorldState / SceneState alter objective/current reality; Patch 0010 has no typed action-evidence channel proving that Performance enacted the proposed fact.
- CharacterKnowledge must not allow a claim/guess to become knowledge merely because an Interpreter proposed it.
- CharacterMemory is held for explicit review in E0 because selective/false memory semantics remain open in ODR-18.
- CharacterDisposition is explicitly rare/conservative in frozen E0 law.
- Relationship mutation may create durable social state; Blueprint 0.1 requires deep change to have strong, reviewable causal support rather than immediate social effect silently becoming durable state.

This is an E0 safety floor under the current evidence contract, not a post-E0 claim that these domains can never become automatically decidable with stronger typed evidence.

## 19. Policy-eligible auto-approval domains

Subject to all hard rules, explicit E0 policy may auto-approve:

```text
UnresolvedProposition
CharacterBelief
CharacterSuspicion
CharacterGoal
CharacterCircumstance
CharacterClaim
Pressure
```

These domains may materially affect future context. Auto-approval is therefore an explicit recorded E0 policy choice, not a default product promise.

Patch 0010 does not choose the E0-A reference policy; that experimental configuration must be frozen separately before runs if not already recovered from stronger authority.

## 20. Explicit review choices

For mutations requiring review, the evaluator may receive bounded typed review choices:

```text
StateAuthorityReviewChoice
- MutationIndex
- Choice

StateAuthorityReviewChoiceKind
- Approve
- Reject
```

Recommended public creation surface:

```text
StateAuthorityReviewChoice.Approve(mutationIndex)
StateAuthorityReviewChoice.Reject(mutationIndex)
```

No public arbitrary constructor is required.

Review choices contain no free-form prose and no model confidence.

They are synthetic-capable deterministic inputs. Patch 0010 does **not** authenticate that a choice came from the creator, a configured consequence-review workflow, or another authorized source.

Later effective commit authority must preserve/authenticate the provenance of any review choice it relies upon, including exact ProposalContentHash and policy configuration.

Review-choice rules:

- mutation index is zero-based into the exact Proposal mutation array represented by Input.Mutations;
- duplicate indices fail;
- out-of-range indices fail;
- a review choice may be supplied only for a mutation that reaches RequiresReview after hard-rule evaluation and policy classification;
- a review choice cannot override a hard rejection;
- a review choice for a policy-auto-approved mutation is invalid/miswired input;
- review-choice input order does not affect evaluation.

## 21. State Authority API

```text
DeterministicStateAuthority.Evaluate(
    StateAuthorityInput input,
    StateAuthorityPolicy policy,
    ImmutableArray<StateAuthorityReviewChoice> reviewChoices)
    -> StateAuthorityEvaluation
```

No overload accepts Patch 0009 Proposal, Candidate VisibleText, Context prose, full fixture prose, provider/model settings, or mutation rationale.

## 22. Evaluation shape

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
- ReviewChoices
```

No public constructors for authority-produced Evaluation/Decision/Trace.

Decisions remain in exact Proposal mutation order through MutationIndex.

`Status = ReviewRequired` if one or more decisions remain RequiresReview.

`Status = Complete` only when every mutation is terminal Approved or Rejected. An empty mutation proposal yields Complete with an empty decision list.

Neither status commits anything.

## 23. Deterministic reason vocabulary and order

Reasons appear only in this fixed contract order when applicable:

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

Global Source/Snapshot/Proposal binding defects fail with a State Authority exception rather than fabricating per-mutation decisions.

Reasons contain no mutation Text, record Text, IDs from untrusted unknown properties, or arbitrary snippets.

## 24. Supporting-record rule

Every `SupportingRecordId` must resolve to a known snapshot record.

Missing support is a deterministic hard rejection for that mutation.

A supporting reference may point to an Active or Inactive record in a future snapshot because historical causal records remain referenceable.

Existence does **not** prove:

- that the record was disclosed to the Interpreter;
- that it semantically supports the proposal;
- that it is sufficient causal evidence;
- that it authorizes truth promotion.

Patch 0010 verifies structural referential integrity only.

## 25. Existing-record transition rule

For Supersede or Deactivate:

1. `ExistingRecordId` must resolve;
2. target must be Active;
3. target record domain must exactly match the proposed mutation domain;
4. character mutation target must belong to the same SubjectCharacterId;
5. relationship mutation target must match the exact subject->target pair;
6. target must have `Protection == None`;
7. no other mutation in the same Input may target the same ExistingRecordId.

Any violation is a hard rejection.

Patch 0010 does not change target lifecycle. Later atomic application does.

## 26. Add rule

Add mutations have no ExistingRecordId.

State Authority does not allocate a new RecordId in Patch 0010.

Add may require MandatoryReview or policy review. Approval merely authorizes the proposed consequence semantics identified by ProposalContentHash for later atomic commit.

No semantic duplicate-text detection occurs because the evaluator is prose-blind.

## 27. Batch conflict rule

Patch 0009 deliberately allows non-identical conflicts to reach State Authority.

Patch 0010 resolves only conflicts that are deterministically structural.

If two or more mutations Supersede/Deactivate the same ExistingRecordId, all mutations targeting that record are Rejected with `ConflictingExistingRecordTarget`.

State Authority does not choose a winner, rank mutations, merge prose, or infer semantic equivalence.

Multiple Add mutations are not structurally conflicting merely because they share a domain/subject/relationship pair; later state semantics may refine cardinality if stronger authority earns such a rule.

## 28. System-immutable and creator-locked protection

Protected records cannot be Supersede/Deactivate targets.

HistoricalTruth, Constitution, and Observation are structurally unavailable as Patch 0009 mutation domains already, but SystemImmutable snapshot descriptors provide defense in depth and keep supporting references distinct from mutation authority.

Creator lock is an explicit exact-record overlay. Patch 0010 does not parse prose to detect semantic contradiction with a locked record.

Therefore objective-reality additions remain MandatoryReview under the current evidence contract.

## 29. No claim-to-fact promotion

CharacterClaim, CharacterBelief, CharacterSuspicion, and CharacterMemory remain distinct semantic destinations.

Approval of one cannot automatically create WorldState, SceneState, HistoricalTruth, or CharacterKnowledge.

A State Interpreter WorldState/SceneState/Knowledge proposal is independently reviewed under its own mutation decision; related claim/belief approval does not satisfy that review.

## 30. Disposition conservatism

Every CharacterDisposition mutation requires explicit review in Patch 0010 regardless of policy.

No repetition counter, confidence score, arbitrary threshold, or sentiment classifier is invented.

## 31. Relationship conservatism

Every Relationship mutation requires explicit review under the current evidence contract.

State Authority verifies exact directional ownership for Supersede/Deactivate but does not measure trust, affinity, sentiment, or numerical relationship score.

No immediate social cue becomes durable relationship state without an explicit review choice.

## 32. Circumstance and lower-authority semantics

Circumstance may be policy-auto-approved because frozen E0 law permits frequent causally supported Circumstance change.

Belief, Suspicion, Goal, Claim, UnresolvedProposition, and Pressure may also be configured for E0 auto-approval after hard rules.

This does not make them fact. Their authority remains their own typed domain.

## 33. Memory remains review-only in E0

CharacterMemory is Add-only from Patch 0009 and MandatoryReview in Patch 0010.

This avoids silently choosing how selective, false, reconstructed, or corrective memory should enter durable Character state while ODR-18 remains open.

Approved Memory still does not alter objective history.

## 34. Evaluation phase and staleness

Patch 0010 has one pure deterministic evaluation phase.

It may be run speculatively and recomputed later.

Evaluation creates no authority mutation, spend/retry permission, Take, or opportunity transition.

A later atomic commit boundary must either:

- recompute State Authority against the then-current authoritative state representation; or
- bind a completed evaluation to a future authoritative StateHash/current-state identity before application.

Patch 0010 does not invent that StateHash or stale-state commit protocol.

The full structural Snapshot remains in StateAuthorityInput/Trace so an evaluation is reconstructable as a decision over a specific supplied snapshot, but this is not promoted into durable ProductionState identity.

## 35. No provisional-Take ordering decision

Patch 0010 does not resolve when a provisional Take object is allocated relative to State Interpretation or State Authority.

It freezes only that no effective accepted Take/commit may treat unreviewed RequiresReview mutations as approved consequences.

Immediate Take ordering remains for the next dedicated Take contract unless stronger authority resolves it.

## 36. Complete versus effective

A Complete StateAuthorityEvaluation means only:

- every proposed mutation has a terminal Approved or Rejected decision under the supplied snapshot, policy, and review choices.

It does **not** mean:

- accepted Take;
- committed consequence;
- current state changed;
- new RecordIds allocated;
- causal history appended;
- future Context may see the mutation.

Only the later atomic causal-commit boundary can make approved consequences effective together with accepted Performance.

## 37. Failure behavior

State Authority fails closed.

Malformed snapshot/input/policy/review-choice structures produce a small sanitized State Authority exception domain.

Per-mutation hard-rule failures become typed Rejected decisions when the overall input itself is structurally valid.

Exceptions/reasons never echo mutation Text, record Text, raw JSON, provider content, credentials, or arbitrary snippets.

Failure creates no fictional action and no state mutation.

## 38. Determinism

Identical valid:

```text
StateAuthorityInput
+ StateAuthorityPolicy
+ review choices
```

produce identical Evaluation semantics.

No clock, random source, culture-dependent sorting, filesystem, network, provider, model, GPU, NPU, or global mutable state.

Canonical ordering:

- snapshot roster: CharacterId ordinal;
- snapshot records: RecordId ordinal;
- Input mutations: Proposal mutation order;
- policy auto-approve domains: enum-contract order;
- review choices in Trace: MutationIndex ascending;
- decisions: MutationIndex ascending / Proposal order;
- reasons: fixed contract order.

## 39. Creator ontology guard

Patch 0010 evaluates E0 authority domains already frozen by Patch 0009.

It does not convert those domains into final creator-facing UI taxonomy or post-E0 Production storage ontology.

## 40. E0 control isolation

Patch 0010 directly applies to variants using the Patch 0009 per-Character State Interpreter proposal path.

E0-E single-playwright control is not forced through Candidate-specific Interpreter/Source binding if doing so contaminates the control. Its eventual consequence-authority protocol must still satisfy frozen hard gates: no silent truth promotion, no locked-canon mutation, deterministic authority, and atomic accepted-Performance/consequence history.

## 41. ARM64 / battery implications

Patch 0010 is tiny deterministic CPU work:

- explicit proposal canonicalization + SHA-256 once at Bind;
- immutable-array canonicalization;
- RecordId dictionary/set lookup;
- enum/domain checks;
- bounded batch conflict detection.

No NPU is appropriate. Offloading authority logic would add latency, nondeterminism, power cost, and an unnecessary dependency on AI readiness.

Implementation should use average-O(1) lookup structures for record resolution/conflict detection rather than pairwise scans over proposal batches.

## 42. Required tests / review gates

Use canonical upstream construction and public production paths. No public test bypass APIs.

### Proposal content identity / least privilege
1. canonical Patch 0009 Proposal binds to deterministic lowercase 64-hex ProposalContentHash;
2. identical semantic Proposal -> identical hash;
3. raw JSON whitespace/property-order differences that parse to same Proposal -> same hash;
4. mutation Text change -> different hash even when structural shape identical;
5. supporting RecordId change -> different hash;
6. mutation order change -> different hash;
7. Source Scene/Candidate-content identity change -> different hash;
8. Input public surface contains hash/structural metadata but no Proposal, Source, mutation Text, Candidate VisibleText, Context prose, provider/model;
9. evaluator public API accepts StateAuthorityInput, not Patch 0009 Proposal;
10. canonical identity serializer is explicit/fixed-order and not reflection-order dependent.

### Snapshot
11. canonical Missing Raft fixture binds with empty creator-lock overlay;
12. snapshot constructor non-public;
13. exactly three canonical roster IDs;
14. all frozen fixture RecordIds represented exactly once;
15. descriptor domain/subject/relationship mapping exact;
16. HistoricalTruth/Constitution/Observation SystemImmutable;
17. all other initial records unprotected;
18. creator-lock overlay marks a mutable record CreatorLocked;
19. duplicate/unknown creator-lock IDs fail;
20. overlay of SystemImmutable record preserves SystemImmutable;
21. snapshot public surface contains no Text/prose/provider/model/state-apply API;
22. records canonical RecordId ordinal;
23. all initial fixture records Active.

### Source / proposal binding
24. canonical Patch 0009 Source+Proposal binds;
25. Scene mismatch fails;
26. roster mismatch fails;
27. Candidate-content identity mismatch fails;
28. Candidate-content hash mismatch fails;
29. unsupported Patch 0009 contract fails;
30. Bind does not rerun Integrity or Interpreter parsing;
31. input constructor non-public;
32. structural mutation projection preserves MutationIndex/domain/transition/IDs/supports exactly while dropping Text.

### Policy
33. policy constructor non-public;
34. empty auto-approve set valid;
35. policy domain order canonical;
36. duplicates fail;
37. mandatory-review domains cannot be configured for auto-approval;
38. policy has no confidence/score/UI-mode/provider field.

### Hard transition rules
39. valid Add survives hard rules;
40. valid Supersede resolves exact active record;
41. valid Deactivate resolves exact active record;
42. missing ExistingRecordId -> Reject;
43. inactive target -> Reject when future-capable internal snapshot construction exists; until then constructor/static invariant gate;
44. wrong domain -> Reject;
45. wrong Character subject -> Reject;
46. wrong relationship subject/target -> Reject;
47. SystemImmutable target -> Reject defense-in-depth;
48. CreatorLocked target -> Reject;
49. missing supporting RecordId -> Reject;
50. known inactive supporting record remains referenceable in future-capable snapshot;
51. two mutations targeting same ExistingRecordId -> both Reject;
52. no winner chosen for structural batch conflict.

### Review floor / policy
53. WorldState always RequiresReview absent explicit choice;
54. SceneState always RequiresReview;
55. CharacterKnowledge always RequiresReview;
56. CharacterMemory always RequiresReview;
57. CharacterDisposition always RequiresReview;
58. Relationship always RequiresReview;
59. mandatory-review auto-approval cannot be configured;
60. eligible domain not in policy -> RequiresReview;
61. eligible domain in policy -> Approved / PolicyAutoApproved;
62. explicit Approve resolves RequiresReview to Approved;
63. explicit Reject resolves RequiresReview to Rejected;
64. duplicate/out-of-range review choice fails;
65. review choice for hard-rejected mutation fails as miswired input;
66. review choice for policy-auto-approved mutation fails;
67. review-choice order does not affect result.

### Evaluation semantics
68. decisions preserve Proposal order through MutationIndex;
69. reasons preserve fixed contract order;
70. any pending review -> Status ReviewRequired;
71. all terminal -> Status Complete;
72. empty Proposal -> Complete + zero decisions;
73. Approved exposes no committed/state-applied/Take/Commit fields;
74. Trace contains exact prose-free Input/Policy/canonical review choices;
75. Evaluation/Decision/Trace constructors non-public;
76. no mutation/record Text copied into Input/Trace/reasons.

### Truth / authority
77. approved CharacterClaim remains CharacterClaim only;
78. approved Belief/Suspicion/Memory does not create WorldState/Knowledge;
79. objective add cannot auto-approve under Patch 0010 policy;
80. Disposition/Relationship cannot auto-approve;
81. creator lock cannot be overridden by review choice;
82. no direct State apply/NewRecordId/Take/Commit APIs;
83. no model/provider call;
84. no numeric relationship/identity score.

### Regression
85. existing 330 Core tests green;
86. Patch 0008 Candidate hash oracle unchanged;
87. Missing Raft Structured/Rendered Context hashes unchanged;
88. Missing Raft ECJ-1 9112 bytes/hash unchanged;
89. Missing Raft Harness PASS/0;
90. smoke Harness PASS/0.

## 43. Explicit exclusions

Patch 0010 does not implement:

- State Interpreter provider/input composer;
- authenticated provider-attempt provenance;
- final consequence acceptance UX/modes;
- authenticated creator-review workflow;
- model confidence scoring;
- ProductionState;
- StateHash;
- authoritative new RecordId allocation;
- mutation application;
- accepted/rejected/alternate Take semantics;
- TakeId;
- CommitId;
- atomic causal commit;
- append-only persistence/recovery;
- effective opportunity establishment/history append;
- Scene loop;
- Observation engine;
- World Resolver;
- memory-forgetting design;
- final creator-facing ontology;
- E0-E control protocol;
- WinUI;
- Windows AI/NPU execution;
- packaging/WACK;
- Store certification.

## 44. Recursive audit dimensions

Restart from frozen authority after every material correction:

1. State Interpreter vs State Authority separation;
2. State Authority vs atomic commit separation;
3. State Authority vs Take semantics separation;
4. proposal non-authority;
5. exact proposal-content identity;
6. prose-blind least privilege;
7. Integrity Accept non-authority;
8. creator locks;
9. SystemImmutable records;
10. truth/claim/knowledge separation;
11. possibility/unresolved proposition separation;
12. memory ODR-18 preservation;
13. Disposition conservatism;
14. Relationship conservatism;
15. Circumstance fluidity;
16. review policy vs final ODR-19 UX;
17. no fake confidence precision;
18. causal evidence limits;
19. support-reference existence vs semantic sufficiency;
20. record domain/ownership transition rules;
21. active/inactive lifecycle;
22. batch conflicts;
23. no semantic keyword judge;
24. creator-ontology guard;
25. source/proposal/snapshot binding;
26. stale-state limitation / future StateHash;
27. nonterminal RequiresReview semantics;
28. review-choice/policy authentication limits;
29. provenance/reconstruction;
30. E0 control isolation;
31. deterministic ordering;
32. invalid-state representability;
33. API minimality;
34. exception/diagnostic safety;
35. testability;
36. Hygiene Constitution;
37. ARM64/battery;
38. scope/validation claims.

## 45. Material approval decisions

Approval would freeze only:

1. Patch 0010 as deterministic review/decision logic, not state application;
2. `StateAuthorityInput.Bind` as sole rich Snapshot+Source+Proposal boundary;
3. versioned deterministic semantic ProposalContentHash covering exact mutation Text while evaluator remains prose-blind;
4. structural `StateAuthoritySnapshot` over current fixture + exact-record creator-lock overlay;
5. prose-free typed record descriptors with Active/Inactive + protection metadata;
6. SystemImmutable HistoricalTruth/Constitution/Observation;
7. prose-free typed State Authority mutation projection with exact mutation indices;
8. explicit E0 policy as auto-approve-domain set, not final product mode taxonomy;
9. no confidence scalar;
10. mandatory E0 review for WorldState, SceneState, CharacterKnowledge, CharacterMemory, CharacterDisposition, Relationship;
11. policy-eligible auto-approval only for UnresolvedProposition, Belief, Suspicion, Goal, Circumstance, Claim, Pressure;
12. synthetic-capable typed explicit review choices with later provenance authentication required;
13. policy selection provenance also required before effective use;
14. hard structural rules cannot be waived by policy/review;
15. SupportingRecord existence checked but semantic causal sufficiency not inferred;
16. exact ExistingRecord domain/ownership/active/protection checks;
17. same-existing-record batch conflicts reject all contenders rather than rank them;
18. Approved / Rejected / RequiresReview per mutation with typed reasons;
19. Complete vs ReviewRequired evaluation status;
20. Approved means later commit-eligible consequence only, not current state;
21. evaluation may be speculative/recomputed and has no persistent StateHash yet;
22. later atomic commit must re-evaluate against current state or bind to future StateHash;
23. Take ordering remains open;
24. no ProductionState/StateHash/RecordId allocation/application/Take/commit/persistence/provider/UI/NPU/Store scope.

Implementation remains blocked until recursive audit completes and the user explicitly approves the final proposal.
