# H1 Patch 0008 — E0 Integrity Validator Contract

Status: blueprint proposal 0.3 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
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
            -> Accept alone yields IntegrityClearedCandidate
                -> later State Interpreter
```

Patch 0008 defines candidate-integrity evaluation and its structurally gated handoff to later interpretation.

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

Integrity Validator checks candidate Performance before acceptance, gives deterministic hard invariants precedence, may consume model/human-assisted semantic concern evidence that cannot waive hard rules, has dispositions accept/reject/request another take, and never rewrites Performance.

Patch 0008 owns only candidate-integrity responsibility. Access, State, Take, persistence, cost/cancellation, and orchestration keep their own hard gates.

## 3. Integrity is not truth policing

A Character may lie, be mistaken, speculate, repeat rumor, contradict another Character, state a false belief, or make a claim unsupported by objective Production truth.

Those are not automatically integrity failures.

A statement becomes an authority violation only when later authority silently promotes it to truth or when Performance credibly uses inaccessible information / enacts something incompatible with locked authority.

Therefore Patch 0008 has no deterministic secret/canon keyword scanner, no `candidate text != objective truth -> Reject` rule, and no automatic rejection of false claims/beliefs.

## 4. Input boundary

The deterministic validator consumes exactly:

```text
ContextPacket sourceContext
CandidatePerformance candidate
IntegrityConcernAssessment concernAssessment
```

It does not consume ValidatedFixture/full Production, Access deny text, Director evaluation, State proposals/authority, Take/Commit identity, provider credentials/configuration, raw/partial provider output, model rationale/chain-of-thought, filesystem/network/clock/random state.

`sourceContext` is the Character-bounded semantic ContextPacket associated with Candidate generation.

`ContextPacketId` remains structured semantic content identity under Patch 0005. It is not proof of exact rendered/provider disclosure or provider-attempt identity; those remain separate provenance concerns.

## 5. Candidate content identity contract

Patch 0006 intentionally introduced no CandidateId or TakeId. Integrity concern evidence nevertheless must bind to exact Candidate semantics reviewed.

Freeze:

```text
CandidateContentIdentityContract = ensemble.e0.integrity.candidate-content.v1
CandidateContentHash = SHA-256(canonical candidate-content envelope)
```

Canonical root property order:

1. `identityContract`
2. `candidateContractVersion`
3. `subjectCharacterId`
4. `contextPacketId`
5. `visibleText`
6. `control`

`control` order:

1. `addressedCharacterIds`
2. `nominatedCharacterId`

Rules:

- identityContract exactly `ensemble.e0.integrity.candidate-content.v1`;
- UTF-8 without BOM;
- reuse existing canonical JSON scalar/string + UTF-8 emission discipline;
- visible text emitted exactly;
- addressed IDs preserve Patch 0006 canonical ordinal order;
- nomination string or JSON null;
- SHA-256 lowercase 64-hex.

Identity-contract value participates in hash preimage.

## 6. CandidateContentHash is not attempt identity

CandidateContentHash is content identity only.

It is not provider-attempt identity, CandidateId, TakeId, causal-commit identity, acceptance authority, or proof of provider/model origin.

Distinct attempts with identical candidate semantics intentionally share CandidateContentHash.

Later attempt provenance remains separate.

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

`Concerns` is immutable, distinct, and stored in frozen enum order from:

```text
PotentialInaccessibleInformationUse
PotentialProtectedInformationExposure
PotentialLockedAuthorityViolation
PotentialTechnicalArtifactLeak
IndeterminateSemanticIntegrity
```

These are engine integrity-policy categories, not creator-facing dramatic ontology.

Assessment contains no prose rationale, chain-of-thought, Character/private/forbidden text, confidence score, accept/reject authority, State mutation, or retry/spend authority.

`IndeterminateSemanticIntegrity` means a **completed semantic assessment** that cannot responsibly clear the Candidate. It must never be used to encode assessor transport failure, refusal, timeout, cancellation, or unavailable service.

## 8. Assessment binding

Preferred:

```text
IntegrityConcernAssessment.Bind(
    CandidatePerformance candidate,
    ImmutableArray<IntegrityConcernKind> concerns)
    -> IntegrityConcernAssessment
```

Bind requires initialized Candidate, computes CandidateContentHash, rejects default arrays, undefined enum values, duplicates, and counts above five, and stores concerns in frozen order.

A zero-length initialized array is valid and means the configured assessment path completed and reported no concerns.

Bind proves structural validity and Candidate-content association only. It does not authenticate assessor provenance.

Synthetic assessments remain legitimate for tests/E0-F injection.

## 9. Assessor technical failure is not semantic concern

Patch 0008 does not implement a semantic assessor, but freezes this later integration rule:

```text
semantic assessment completed with uncertainty
    -> IndeterminateSemanticIntegrity concern

assessor provider/transport/refusal/timeout/cancellation failure
    -> no valid IntegrityConcernAssessment for that attempt
    -> technical/orchestration failure path
```

A technical assessor failure must not automatically become RequestAnotherTake, because that would convert infrastructure failure into Character-facing retry pressure and potentially spend another provider call.

Later deterministic fallback/cost policy decides whether another assessor, local rule, human review, or cancellation is permitted.

## 10. Future semantic-assessor privacy requirement

Any later assessor path must obey Context Sovereignty:

- do not disclose complete Production by default;
- use a separately defined bounded integrity-assessment packet containing Candidate content plus minimum authoritative constraints/references;
- keep disclosure separate from Character-facing Context;
- preserve assessor/provider/model provenance where applicable;
- no hidden reasoning exposure;
- assessor output cannot mutate State, commit anything, or authorize retry/spend;
- assessor output cannot waive deterministic failures.

Exact assessor/provider/model/human mechanism remains later scope.

## 11. E0 control-isolation requirement for future assessor

If E0 uses model/human-assisted Integrity assessment, its configuration is an experimental variable and must be fixed across an architecture-isolating comparison batch unless the experiment explicitly varies it.

E0 provenance must make assessor identity/configuration attributable when applicable.

Do not tune assessor prompts/rules between E0-A/C/D/E runs within a frozen comparison batch.

Otherwise retry/rejection differences could be falsely attributed to Ensemble's Character/Director architecture.

## 12. Trusted input defects versus Candidate rejection

The validator distinguishes evaluation defects from Candidate rejection.

### Integrity exception / no disposition

Integrity exception domain:

- null/uninitialized malformed trusted objects;
- malformed source Context invariants normal public construction should prevent;
- unsupported Integrity assessment contract;
- unsupported Candidate-content identity contract;
- assessment hash != recomputed CandidateContentHash;
- malformed assessment concern collection.

These mean stale/miswired/unsupported evaluation evidence and do not creatively Reject the Candidate.

### Deterministic Candidate Reject codes

Closed E0 Reject codes:

```text
UnsupportedCandidateContract
SubjectContextMismatch
ContextPacketIdentityMismatch
```

UnsupportedCandidateContract means Candidate semantic contract is not the v1 contract this Validator understands.

SubjectContextMismatch means Candidate SubjectCharacterId != source Context subject.

ContextPacketIdentityMismatch means Candidate ContextPacketId != source ContextPacketId.

Source Context SubjectCharacterId != OpportunityCharacterId is malformed trusted-source state and therefore exception-domain.

Patch 0008 does not duplicate Patch 0006 VisibleText/control/address/nomination validation.

## 13. Deterministic validation order

1. validate source Context trusted invariants;
2. validate Candidate initialized enough for content identity;
3. validate concern-assessment structure/contracts;
4. recompute CandidateContentHash;
5. require assessment hash match; mismatch -> Integrity exception;
6. compute Candidate Reject codes;
7. apply disposition precedence.

Malformed/stale evaluation evidence cannot be mislabeled as Character rejection.

## 14. Integrity disposition

Freeze:

```text
IntegrityDisposition
- Accept
- Reject
- RequestAnotherTake
```

Precedence:

```text
Candidate Reject code(s) present -> Reject
else concern(s) present -> RequestAnotherTake
else -> Accept
```

Semantic concerns cannot soften deterministic Reject.

`Accept` means integrity-eligible for later interpretation/review only.

It does not mean accepted Take, Production history, authoritative State, committed consequence, effective next opportunity, or retry/spend authority.

## 15. IntegrityClearedCandidate structural handoff

To make the frozen `Integrity -> State Interpreter` order structural rather than convention-only, only Accept produces:

```text
IntegrityClearedCandidate
- Candidate
- CandidateContentIdentityContract
- CandidateContentHash
```

Properties:

- read-only;
- no public constructor;
- created only by DeterministicIntegrityValidator on Accept;
- preserves exact CandidatePerformance object/semantics;
- carries the same content identity bound to the concern assessment.

`IntegrityClearedCandidate` means exactly:

> this Candidate is eligible to enter the later State Interpreter under the supplied Integrity evaluation.

It is not accepted Take, canon, history, State authority, retry authority, or causal identity.

Its non-public construction protects sequence/object invariants, not semantic-assessor provenance authentication.

## 16. Evaluation shape

Freeze:

```text
IntegrityValidationEvaluation
- Disposition
- ClearedCandidate
- Trace
```

Invariant:

```text
Disposition == Accept
    <=> ClearedCandidate is non-null
```

Reject/RequestAnotherTake evaluations expose no cleared handoff.

The future State Interpreter should consume IntegrityClearedCandidate rather than raw CandidatePerformance in the E0 reference path.

## 17. RequestAnotherTake is not retry authority

RequestAnotherTake does not call provider, authorize spend, increment retry budget, choose understudy, modify Current Opportunity, or create another Candidate.

Later deterministic orchestration/cost policy decides whether another attempt is permitted.

Current Candidate remains unaccepted diagnostics/provenance only.

## 18. Reject is not fictional action

Rejected Candidate does not enter Production history, recent-performance Context, Character state, observation history, or effective opportunity history.

It remains diagnostics only when later provenance exists.

## 19. No hidden rewriting

Validator returns no replacement text, corrected Performance, paraphrase, suggested line, or rewritten control.

Candidate remains exact Performer output.

## 20. Technical provider failure boundary

Candidate-provider refusal/timeout/transport error/cancellation/partial stream is not itself CandidatePerformance and must not be converted to fiction.

Validator does not infer transport provenance from words such as timeout/error/refusal.

PotentialTechnicalArtifactLeak exists only as typed concern evidence from a completed configured assessment path.

E0-F provider-failure hard gate remains later integration scope.

## 21. Director interaction

Director is not Integrity input.

Reject/RequestAnotherTake cannot promote associated Director work.

Accept also cannot promote precommit Director evaluation. Patch 0007 still requires successful source causal commit followed by postcommit Director re-Bind/recompute before later opportunity establishment.

## 22. State / Take boundary

Only IntegrityClearedCandidate may enter the later E0 State Interpreter path.

Even then:

```text
Integrity Accept != accepted Take
IntegrityClearedCandidate != accepted Take
IntegrityClearedCandidate != authoritative consequence
IntegrityClearedCandidate != causal commit
```

State Interpreter proposes only; deterministic State Authority and later Take/commit authority remain separate.

## 23. Trace

Freeze:

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

Candidate identity is carried once through ConcernAssessment rather than duplicated in trace.

Trace contains no Candidate VisibleText, Context/protected/denied text, provider output, free-form rationale, confidence score, or chain-of-thought.

Trace is local provenance/diagnostics and never Character-facing Context.

## 24. Determinism

Identical source Context semantics + Candidate semantics + concern assessment + contracts produce identical CandidateContentHash, Reject codes, disposition, cleared-handoff presence/content identity, and trace.

No clock, filesystem, network, random, current culture, provider/model inference, GPU/NPU, or mutable global state.

## 25. Fail-closed exception domain

Use Integrity-specific exception type.

Errors expose structural field names/codes only and never echo Candidate VisibleText, Context/private/protected text, semantic rationale, or raw provider output.

Failure never becomes fallback Accept/Reject/RequestAnotherTake.

## 26. Public-surface intent

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

Public models read-only; construction paths enforce structural invariants.

No public operation commits/retries/mutates State.

## 27. Required tests / review gates

### Candidate identity

1. identical Candidate -> identical hash;
2. visible text/address/nomination/ContextPacketId change -> hash changes;
3. Candidate contract + identity contract participate in preimage;
4. canonical addressed order stable;
5. lowercase 64-hex;
6. content identity not attempt/Take/causal identity.

### Assessment

7. zero-length initialized concerns valid;
8. default concerns invalid;
9. undefined enum invalid;
10. duplicate invalid;
11. all five kinds canonicalize frozen order;
12. Candidate hash matches bound Candidate;
13. assessment exposes no prose/confidence/authority fields;
14. Indeterminate means completed semantic uncertainty, not technical assessor failure.

### Exception vs Reject

15. malformed source Context -> Integrity exception;
16. unsupported assessment/identity contract -> Integrity exception;
17. assessment for different Candidate -> Integrity exception;
18. wrong-subject valid Candidate -> Reject/SubjectContextMismatch;
19. different-Context valid Candidate -> Reject/ContextPacketIdentityMismatch;
20. unsupported Candidate contract -> Reject defensive/static path;
21. no duplicate Patch 0006 parser validation.

### Disposition / handoff

22. no Reject/no concerns -> Accept + non-null ClearedCandidate;
23. Reject finding -> Reject + null ClearedCandidate;
24. Reject + concerns -> Reject + null handoff;
25. concern -> RequestAnotherTake + null handoff;
26. each concern independently -> RequestAnotherTake;
27. ClearedCandidate constructor non-public;
28. ClearedCandidate retains exact Candidate + identity;
29. only Accept can construct cleared handoff;
30. Accept/handoff explicitly not Take/history authority.

### Creative-law guards

31. false Character claim not rejected solely for objective-truth conflict;
32. no secret/canon keyword scanner;
33. no Performance rewrite surface;
34. no State/Context/Candidate mutation;
35. no Director dependency;
36. no State Authority/Take/Commit dependency;
37. no provider/model/network dependency;
38. no confidence/score/probability;
39. trace contains no Candidate/Context/protected prose;
40. technical words in dialogue do not deterministically create TechnicalArtifactLeak.

### Technical-failure/control isolation

41. semantic uncertainty concern -> RequestAnotherTake;
42. assessor technical failure has no valid assessment/disposition path and cannot be encoded as Indeterminate;
43. RequestAnotherTake has no retry/spend behavior;
44. assessor configuration/provenance is required later for E0 comparison validity but no provider implementation exists here.

### Lifecycle

45. Reject remains non-history;
46. RequestAnotherTake remains non-history;
47. only ClearedCandidate enters future State Interpreter path;
48. precommit Director result not promoted by Accept;
49. no effective opportunity mutation/history append/Performer trigger.

### Regression

50. Missing Raft StructuredContextHash unchanged;
51. Missing Raft RenderedContextHash unchanged;
52. ECJ-1 9112 bytes/frozen hash unchanged;
53. all existing 211 Core tests green;
54. Missing Raft Harness PASS/0;
55. smoke Harness PASS/0.

## 28. E0-F compatibility

Patch 0008 prepares but does not complete E0-F.

Later E0-F orchestration must prove inaccessible-secret failures, false-claim truth separation, locked canon/Constitution protection, malformed raw candidate rejection before CandidatePerformance, candidate/assessor provider-failure isolation, partial/rejected attempt non-history, deterministic retry/cost, and atomic accepted Performance + authoritative consequence commit.

Patch 0008 must not claim those end-to-end guarantees.

## 29. ARM64 / battery

Candidate hashing + deterministic disposition are tiny CPU work with no background activity.

No AI/provider/network/GPU/NPU execution in Patch 0008.

Future semantic assessor performance/privacy remains unimplemented/unclaimed.

## 30. Explicit exclusions

No semantic-assessor provider/model/human implementation; bounded integrity-assessment packet implementation; assessor provenance authentication/persistence; provider attempt/provenance; retry/cost/cancellation engine; accepted/rejected/alternate Take identity; TakeId; State Interpreter implementation; State Authority; consequence proposal; ProductionState/StateHash; atomic causal commit; persistence/recovery; effective opportunity establishment; Scene loop; World Resolver/observation; E0-D round-robin execution; E0-E playwright; E0-F end-to-end injection harness; UI/WinUI; Windows AI/NPU; packaging/WACK/Store.

## 31. Recursive adversarial audit dimensions

Restart after every material correction:

1. frozen Blueprint 0.1 Integrity law;
2. H1 sequence/next-boundary correctness;
3. statement vs truth/claim/belief law;
4. Character vs Performer authority;
5. Access/Context privacy;
6. Integrity vs Parser ownership;
7. Integrity vs Director ownership;
8. Integrity vs State Interpreter/Authority;
9. structural Integrity handoff vs accepted Take;
10. semantic concern evidence vs deterministic authority;
11. semantic uncertainty vs technical assessor failure;
12. E0 assessor configuration/control isolation;
13. assessment privacy/minimal disclosure;
14. stale-assessment/candidate-content binding;
15. content identity contract/versioning;
16. content vs attempt/Take/causal identity;
17. technical failure vs fictional action;
18. exception/infrastructure fault vs creative Reject;
19. no hidden rewriting;
20. RequestAnotherTake vs retry/spend;
21. E0-F hard-gate compatibility;
22. creator-ontology extensibility guard;
23. public API/non-forgeability/minimality;
24. deterministic bounded canonicalization;
25. fail closed/error sanitization;
26. tests/testability without production bypass;
27. dependency direction;
28. ARM64/battery;
29. scope/hygiene;
30. E0-A/B/C/D/E/F/G isolation;
31. future State/Take/commit compatibility.

Approval only after complete restart produces zero material corrections or worthwhile improvements.

## 32. Material approval decisions

Approval would freeze only:

1. Patch 0008 as next E0-A boundary after Patch 0007;
2. Integrity evaluation before State Interpreter/Take authority;
3. versioned deterministic CandidateContentHash over exact Candidate semantics for stale-assessment binding;
4. CandidateContentHash content identity only, never attempt/Candidate/Take/commit identity;
5. typed IntegrityConcernAssessment non-authoritative evidence, not assessor authentication;
6. five bounded E0 concern kinds with no rationale/confidence;
7. IndeterminateSemanticIntegrity means completed semantic uncertainty only; assessor technical failure is separate infrastructure failure;
8. no semantic assessor/provider implementation in Patch 0008;
9. future semantic review uses least-privilege integrity-assessment packet rather than full Production by default;
10. future E0 assessor configuration/provenance must remain fixed/attributable across architecture-isolating batches;
11. deterministic Reject codes limited to unsupported Candidate contract, source-subject mismatch, source-ContextPacket mismatch;
12. malformed/stale/unsupported evaluator evidence is Integrity exception-domain, not creative Reject;
13. precedence: Reject code -> Reject; otherwise concern -> RequestAnotherTake; otherwise Accept;
14. only Accept creates non-public IntegrityClearedCandidate handoff;
15. IntegrityClearedCandidate means interpretation eligibility only, not Take/history/State authority;
16. future State Interpreter consumes cleared handoff rather than raw Candidate in E0 reference path;
17. RequestAnotherTake does not authorize retry/spend;
18. false claims/beliefs are not automatically rejected for objective-truth conflict;
19. no secret/canon keyword scanning;
20. no hidden Performance rewriting;
21. provider failures/partial streams remain outside CandidatePerformance and later orchestration/provenance enforces them;
22. Director is not Integrity input and Accept cannot promote precommit Director work;
23. trace contains only source Context identity + assessment + structural Reject codes, never Performance/private prose;
24. Patch 0008 does not implement State Interpreter, State Authority, Take, commit, persistence, opportunity application, Scene loop, provider execution, UI, Windows AI/NPU, or Store machinery.

Implementation remains blocked until recursive audit completes and user explicitly approves the final proposal.
