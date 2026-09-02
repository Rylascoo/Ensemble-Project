# H1 Patch 0008 — E0 Integrity Validator Contract

Status: blueprint proposal 0.2 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0007
Branch: `h1-patch-0008-integrity-validator-blueprint`

## 1. Purpose

Define the next frozen E0-A boundary after Performer Candidate and Director proposal calculation:

```text
source ContextPacket + CandidatePerformance
    -> deterministic Candidate content identity
    -> bounded typed integrity-concern assessment evidence
    -> deterministic Integrity Validator
        -> Accept | Reject | RequestAnotherTake
            -> only Accept may proceed to later State Interpreter / Take authority
```

Patch 0008 defines candidate-integrity evaluation only.

It does **not** create an accepted Take, mutate Production, interpret consequences, apply State, commit history, authorize retry/spend, establish Current Opportunity, call a provider/model, or implement semantic-assessor transport.

## 2. Recovered frozen authority

Blueprint 0.1 orders E0-A preparation as:

1. deterministic Access Control;
2. Context Composer;
3. Performer candidate output;
4. Director opportunity rules;
5. **Integrity Validator acceptance/rejection rules**;
6. State Interpreter candidate-mutation schema;
7. deterministic State Authority;
8. accepted/rejected/alternate Take semantics;
9. atomic causal-commit record.

It separately freezes:

```text
Integrity Validator -> State Interpreter -> deterministic State Authority
```

The Integrity Validator:

- checks candidate Performance before acceptance;
- gives deterministic hard invariants precedence;
- may consume model/human-assisted semantic concern evidence, but such evidence cannot waive hard rules;
- concerns include inaccessible-information use, protected-information exposure, locked canon/world-law conflict, contract failure, or technical artifacts leaking into fiction;
- has dispositions accept, reject, or request another take;
- never rewrites Performance.

Patch 0008 owns only candidate-integrity responsibility. Other E0 hard gates remain owned by Access, State, Take, persistence, cost/cancellation, or orchestration boundaries.

## 3. Integrity is not truth policing

A Character may lie, be mistaken, speculate, repeat rumor, contradict another Character, state a false belief, or make a claim unsupported by objective Production truth.

Those are not automatically integrity failures.

A statement becomes an authority violation only when a later authority silently promotes it to truth or when Performance credibly uses inaccessible information / enacts something incompatible with locked authority.

Therefore Patch 0008 has:

- no deterministic secret/canon keyword scanner;
- no `candidate text != objective truth -> Reject` rule;
- no automatic rejection of false claims/beliefs.

This preserves statement != fact and leaves truth promotion to later State Authority.

## 4. Input boundary

The deterministic validator consumes exactly:

```text
ContextPacket sourceContext
CandidatePerformance candidate
IntegrityConcernAssessment concernAssessment
```

It does not consume ValidatedFixture/full Production, Access deny text, Director evaluation, State proposals/authority, Take/Commit identity, provider credentials/configuration, raw/partial provider output, model rationale/chain-of-thought, filesystem/network/clock/random state.

`sourceContext` is the Character-bounded semantic ContextPacket associated with Candidate generation.

## 5. Candidate content identity contract

Patch 0006 intentionally introduced no CandidateId or TakeId. Integrity concern evidence nevertheless must bind to the exact Candidate semantics reviewed.

Freeze:

```text
CandidateContentIdentityContract = ensemble.e0.integrity.candidate-content.v1
CandidateContentHash = SHA-256(canonical candidate-content envelope)
```

Canonical candidate-content envelope root property order:

1. `identityContract`
2. `candidateContractVersion`
3. `subjectCharacterId`
4. `contextPacketId`
5. `visibleText`
6. `control`

`control` property order:

1. `addressedCharacterIds`
2. `nominatedCharacterId`

Rules:

- `identityContract` is exactly `ensemble.e0.integrity.candidate-content.v1`;
- UTF-8 without BOM;
- reuse existing canonical JSON scalar/string + UTF-8 emission discipline;
- visible text emitted exactly as already-validated Candidate text;
- addressed IDs preserve Patch 0006 canonical ordinal order;
- nomination is string or JSON null;
- SHA-256 lowercase 64-hex.

The identity-contract value is part of the hash preimage for domain/version separation.

## 6. CandidateContentHash is not attempt identity

CandidateContentHash is content identity only.

It is not provider-attempt identity, CandidateId, TakeId, causal-commit identity, acceptance authority, or proof of provider/model origin.

Distinct generation attempts with identical candidate semantics intentionally share CandidateContentHash.

Later attempt provenance must remain separate.

## 7. Integrity concern assessment evidence

Freeze:

```text
IntegrityConcernAssessment
- AssessmentContract
- CandidateContentIdentityContract
- CandidateContentHash
- Concerns
```

Contracts:

```text
AssessmentContract = ensemble.e0.integrity.concerns.v1
CandidateContentIdentityContract = ensemble.e0.integrity.candidate-content.v1
```

`Concerns` is an immutable canonical ordinal/distinct set drawn only from:

```text
PotentialInaccessibleInformationUse
PotentialProtectedInformationExposure
PotentialLockedAuthorityConflict
PotentialTechnicalArtifactLeak
IndeterminateSemanticIntegrity
```

The enum values/order are frozen for this v1 assessment contract.

These are engine integrity-policy categories, not creator-facing dramatic ontology.

Assessment contains no prose rationale, chain-of-thought, Character/private/forbidden text, confidence score, accept/reject authority, State mutation, or retry/spend authority.

## 8. Assessment binding

Preferred binding:

```text
IntegrityConcernAssessment.Bind(
    CandidatePerformance candidate,
    ImmutableArray<IntegrityConcernKind> concerns)
    -> IntegrityConcernAssessment
```

Bind:

- requires non-null/initialized Candidate;
- computes CandidateContentHash under the frozen identity contract;
- rejects default concern arrays;
- rejects undefined concern enum values;
- rejects duplicates rather than silently deduplicating caller mistakes;
- rejects more than the five frozen E0 concern kinds;
- stores concerns in frozen enum/ordinal order.

A zero-length initialized array is valid and means the configured assessor reported no concerns.

Bind proves only structural validity and candidate-content association. It does not authenticate the assessor or assert causal authority.

Synthetic assessments are legitimate for deterministic tests and E0-F failure injection.

## 9. Future semantic-assessor privacy requirement

Patch 0008 does not implement a model/human semantic assessor.

Any later assessor path must obey Context Sovereignty:

- do not disclose complete Production by default;
- define a separate bounded integrity-assessment packet containing Candidate content plus only the minimum authoritative constraints/references needed for review;
- keep that disclosure distinct from Character-facing Context;
- preserve assessor/provider/model provenance when applicable;
- do not expose hidden reasoning;
- do not permit assessor output to mutate State or authorize cost/retry;
- do not permit assessor output to waive deterministic failures.

Exact assessor/provider/model/human mechanism is later scope.

## 10. Trusted input defects versus Candidate rejection

The validator distinguishes infrastructure/evaluation defects from creative Candidate rejection.

### Integrity exception / no disposition

Use the Integrity exception domain for:

- null/uninitialized malformed trusted objects;
- malformed source Context invariants that normal public construction should prevent;
- unsupported Integrity assessment contract;
- unsupported Candidate content identity contract;
- concern-assessment hash that does not equal recomputed CandidateContentHash;
- malformed assessment concern collection.

These mean the evaluation itself is miswired/stale/unsupported. They do **not** mean the Character's Candidate is creatively rejected.

### Deterministic Candidate Reject findings

For independently valid Candidate/source Context objects, closed E0 deterministic reject codes are:

```text
UnsupportedCandidateContract
SubjectContextMismatch
ContextPacketIdentityMismatch
```

`UnsupportedCandidateContract` means the Candidate semantic contract is not the v1 contract this Validator understands.

`SubjectContextMismatch` means Candidate SubjectCharacterId differs from source Context subject.

`ContextPacketIdentityMismatch` means Candidate ContextPacketId differs from source ContextPacketId.

Source Context SubjectCharacterId != OpportunityCharacterId is a malformed trusted-source invariant and therefore exception-domain, not Candidate Reject.

Patch 0008 does not duplicate Patch 0006 VisibleText/control/address/nomination validation.

## 11. Deterministic validation order

Exact order:

1. validate source Context trusted invariants;
2. validate Candidate object is structurally initialized enough to compute content identity;
3. validate concern assessment structure/contracts;
4. recompute CandidateContentHash;
5. require assessment hash identity match; mismatch -> Integrity exception;
6. compute Candidate Reject codes;
7. apply disposition precedence.

This prevents malformed/stale evaluator evidence from being mislabeled as a Character rejection.

## 12. Integrity disposition

Freeze:

```text
IntegrityDisposition
- Accept
- Reject
- RequestAnotherTake
```

Exact precedence:

### Reject

If one or more deterministic Candidate Reject findings exist:

```text
Disposition = Reject
```

Semantic concerns cannot override or soften Reject.

### RequestAnotherTake

Otherwise, if one or more typed concerns exist:

```text
Disposition = RequestAnotherTake
```

### Accept

Otherwise:

```text
Disposition = Accept
```

`Accept` means **integrity-eligible to proceed to later State Interpreter / review**.

It does not mean accepted Take, Production history, authoritative State, committed consequence, effective next opportunity, or retry/spend authority.

## 13. RequestAnotherTake is not retry authority

RequestAnotherTake is an Integrity disposition only.

It does not call a provider, authorize another paid request, increment retry budget, choose an understudy, modify Current Opportunity, create another Candidate, or spend anything.

Later deterministic orchestration/cost policy decides whether another attempt may occur.

The current Candidate remains unaccepted diagnostic/provenance material only.

## 14. Reject is not fictional action

A rejected Candidate does not enter Production history, recent-performance Context, Character memory/belief/state, observation history, or effective opportunity history.

It remains only experimental/provider-attempt diagnostic material when later provenance exists.

## 15. No hidden rewriting

Validator returns no replacement/corrected text, paraphrase, suggested line, or rewritten control metadata.

Candidate remains exactly what Performer produced.

## 16. Technical provider failure boundary

Provider refusal, timeout, transport error, cancellation, or partial stream is not itself CandidatePerformance and must not be converted to fiction by orchestration.

Validator must not infer transport failure merely from words such as `timeout`, `error`, or `refusal`; Characters may legitimately use those words.

PotentialTechnicalArtifactLeak exists only for typed concern evidence supplied by the configured integrity-review path.

The E0-F provider-failure hard gate remains a later orchestration/provenance integration test.

## 17. Director interaction

Director proposal/evaluation is not Integrity input.

Reject/RequestAnotherTake cannot promote any Director result associated with that Candidate.

Accept also cannot promote precommit Director work: Patch 0007 still requires successful source causal commit followed by postcommit Director re-Bind/recompute before later opportunity establishment.

## 18. State / Take boundary

Only Integrity Accept may proceed to later State Interpreter candidate-mutation work.

Even then:

```text
Integrity Accept != accepted Take
Integrity Accept != authoritative consequence
Integrity Accept != causal commit
```

State Interpreter proposes only; deterministic State Authority and later Take/commit authority remain separate.

## 19. Evaluation and trace

Freeze:

```text
IntegrityValidationEvaluation
- Disposition
- Trace
```

```text
IntegrityValidationTrace
- ValidationContract
- SourceContextPacketId
- ConcernAssessment
- DeterministicRejectCodes
```

```text
ValidationContract = ensemble.e0.integrity.validation.v1
```

Candidate identity is carried once through ConcernAssessment rather than duplicated as another trace field.

Trace contains no Candidate VisibleText, Context text, denied/protected text, provider output, free-form rationale, confidence score, or chain-of-thought.

Trace is local provenance/diagnostics and never Character-facing Context.

## 20. Determinism

Identical source Context semantics + Candidate semantics + concern assessment + contract versions produce identical CandidateContentHash, reject codes, disposition, and trace.

No clock, filesystem, network, random, current culture, provider/model inference, GPU/NPU, or mutable global state.

## 21. Fail-closed exception domain

Use `IntegrityValidationException` or equivalent Integrity-specific domain.

Errors expose structural field names/codes only and never echo Candidate VisibleText, Context/private/protected text, semantic rationale, or raw provider output.

Failure never becomes fallback Accept or RequestAnotherTake.

## 22. Public-surface intent

Preferred:

```text
CandidateIntegrityIdentity.Compute(CandidatePerformance)
    -> CandidateContentHash

IntegrityConcernAssessment.Bind(
    CandidatePerformance,
    ImmutableArray<IntegrityConcernKind>)
    -> IntegrityConcernAssessment

DeterministicIntegrityValidator.Validate(
    ContextPacket,
    CandidatePerformance,
    IntegrityConcernAssessment)
    -> IntegrityValidationEvaluation
```

`CandidateContentHash` may remain a validated string/value representation rather than introducing a speculative general CandidateId abstraction.

Public models are read-only; construction paths enforce frozen invariants.

No public operation commits/retries/mutates State.

## 23. Required tests / review gates

### Candidate content identity

1. repeated identical Candidate -> identical hash;
2. visible-text change -> hash changes;
3. address-control change -> hash changes;
4. nomination change -> hash changes;
5. ContextPacketId change -> hash changes;
6. Candidate contract participates in preimage;
7. identity contract participates in preimage;
8. canonical addressed order stable;
9. hash lowercase 64-hex;
10. content hash does not expose attempt/Take/causal identity.

### Assessment binding

11. zero-length initialized concerns valid;
12. default concerns invalid;
13. undefined enum invalid;
14. duplicate concern invalid;
15. all five concern kinds canonicalize in frozen order;
16. assessment Candidate hash exactly matches bound Candidate;
17. assessment has no prose/confidence/authority fields.

### Exception versus Reject

18. malformed source Context -> Integrity exception;
19. unsupported assessment contract -> Integrity exception;
20. unsupported candidate-identity contract -> Integrity exception;
21. assessment bound to different Candidate content -> Integrity exception;
22. independently valid wrong-subject Candidate -> Reject/SubjectContextMismatch;
23. independently valid different-Context Candidate -> Reject/ContextPacketIdentityMismatch;
24. unsupported Candidate semantic contract -> Reject/UnsupportedCandidateContract defensive/static path;
25. no duplicate Patch 0006 VisibleText/control validator.

### Disposition

26. zero Reject findings + zero concerns -> Accept;
27. Reject finding + zero concerns -> Reject;
28. Reject finding + concerns -> Reject;
29. one concern -> RequestAnotherTake;
30. multiple concerns -> RequestAnotherTake;
31. each concern kind independently -> RequestAnotherTake;
32. RequestAnotherTake has no retry/spend authority;
33. Accept explicitly not accepted Take/history authority.

### Creative-law guards

34. false Character claim not deterministically rejected solely for objective-truth conflict;
35. no secret/canon keyword scanner;
36. no Performance rewrite surface;
37. no State/Context/Candidate mutation;
38. no Director dependency;
39. no State Interpreter/Authority/Take/Commit dependency;
40. no provider/model/network dependency;
41. no confidence/score/probability fields;
42. trace contains no Candidate/Context/protected prose;
43. technical words in legitimate dialogue do not deterministically create TechnicalArtifactLeak.

### Lifecycle

44. Reject Candidate remains non-history;
45. RequestAnotherTake Candidate remains non-history;
46. Accept only enables later State Interpreter eligibility;
47. precommit Director evaluation not promoted by Accept;
48. no effective opportunity mutation/history append/Performer trigger.

### Regression

49. Missing Raft StructuredContextHash unchanged;
50. Missing Raft RenderedContextHash unchanged;
51. ECJ-1 remains 9112 bytes / frozen SHA-256;
52. all existing 211 Core tests green;
53. Missing Raft Harness PASS/0;
54. smoke Harness PASS/0.

## 24. E0-F compatibility

Patch 0008 prepares but does not complete E0-F.

Later E0-F orchestration must prove inaccessible-secret failures, false-claim truth separation, locked canon/Constitution protection, malformed raw candidate rejection before CandidatePerformance, provider-failure isolation, partial/rejected attempt non-history, deterministic retry/cost, and atomic accepted Performance + authoritative consequence commit.

Patch 0008 must not claim those end-to-end guarantees by itself.

## 25. ARM64 / battery

Candidate hashing + deterministic disposition are tiny CPU work with no background activity.

Patch 0008 introduces no AI/provider/network/GPU/NPU execution.

Future semantic assessor performance/privacy is not implemented or claimed.

## 26. Explicit exclusions

No semantic-assessor provider/model implementation; bounded integrity-assessment packet implementation; assessor provenance authentication/persistence; provider attempt/provenance; retry/cost/cancellation engine; accepted/rejected/alternate Take identity; TakeId; State Interpreter; State Authority; consequence proposal; ProductionState/StateHash; atomic causal commit; persistence/recovery; effective opportunity establishment; Scene loop; World Resolver/observation; E0-D round-robin execution; E0-E playwright; E0-F end-to-end injection harness; UI/WinUI; Windows AI/NPU; packaging/WACK/Store.

## 27. Recursive adversarial audit dimensions

Restart after every material correction:

1. frozen Blueprint 0.1 Integrity law;
2. H1 sequence/next-boundary correctness;
3. statement vs truth/claim/belief law;
4. Character vs Performer authority;
5. Access/Context privacy;
6. Integrity vs Parser ownership;
7. Integrity vs Director ownership;
8. Integrity vs State Interpreter/Authority;
9. Integrity Accept vs accepted Take;
10. semantic concern evidence vs deterministic authority;
11. assessment privacy/minimal disclosure;
12. stale-assessment/candidate-content binding;
13. content identity contract/versioning;
14. content identity vs attempt/Take/causal identity;
15. technical failure vs fictional action;
16. exception/infrastructure fault vs creative Reject;
17. no hidden rewriting;
18. RequestAnotherTake vs retry/spend authority;
19. E0-F hard-gate compatibility;
20. creator-ontology extensibility guard;
21. public API/non-forgeability/minimality;
22. deterministic bounded ordering/canonicalization;
23. fail closed/error sanitization;
24. tests/testability without production bypass APIs;
25. dependency direction;
26. ARM64/battery;
27. scope/hygiene;
28. E0-A/B/C/D/E/F/G experimental isolation;
29. future State/Take/commit compatibility.

Approval only after a complete restart produces zero material corrections or worthwhile improvements.

## 28. Material approval decisions

Approval would freeze only:

1. Patch 0008 as next E0-A boundary after Patch 0007;
2. Integrity evaluation before State Interpreter/Take authority;
3. versioned deterministic CandidateContentHash over exact Candidate semantics for stale-assessment binding;
4. CandidateContentHash is content identity only, never attempt/Candidate/Take/commit identity;
5. typed IntegrityConcernAssessment is non-authoritative evidence and does not authenticate assessor provenance;
6. concern binding uses bounded immutable five-kind E0 vocabulary with no free-form rationale/confidence;
7. no semantic assessor/provider is implemented in Patch 0008;
8. future semantic review uses a separate least-privilege integrity-assessment packet rather than full Production by default;
9. deterministic Candidate Reject codes are only unsupported Candidate contract, source-subject mismatch, and source-ContextPacket identity mismatch;
10. malformed/stale/unsupported evaluator evidence is Integrity exception-domain, not creative Reject;
11. exact disposition precedence: Candidate Reject finding -> Reject; otherwise concern -> RequestAnotherTake; otherwise Accept;
12. Integrity Accept means eligible for later interpretation/review only, not accepted Take/history/state;
13. RequestAnotherTake does not authorize provider retry/spend;
14. false claims/beliefs are not automatically rejected merely because they conflict with objective truth;
15. no deterministic secret/canon keyword scanning;
16. no hidden Performance rewriting;
17. provider failures/partial streams remain outside CandidatePerformance and require later orchestration/provenance enforcement;
18. Director evaluation is not an Integrity input and Accept cannot promote precommit Director work;
19. trace stores only source Context identity + assessment + structural Reject codes, never Performance/private prose;
20. Patch 0008 does not implement State Interpreter, State Authority, Take, commit, persistence, opportunity application, Scene loop, provider execution, UI, Windows AI/NPU, or Store machinery.

Implementation remains blocked until recursive audit completes and user explicitly approves the final proposal.
