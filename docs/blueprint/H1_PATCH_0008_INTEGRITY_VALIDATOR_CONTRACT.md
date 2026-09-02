# H1 Patch 0008 — E0 Integrity Validator Contract

Status: blueprint proposal 0.4 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
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
            -> Accept alone yields IntegrityEligibleCandidate
                -> later State / Take design
```

Patch 0008 defines candidate-integrity evaluation and a structurally gated eligibility handoff.

It does **not** create an accepted Take, mutate Production, interpret consequences, apply State, commit history, authorize retry/spend, establish Current Opportunity, call a provider/model, or implement semantic-assessor transport.

## 2. Recovered frozen authority

Blueprint 0.1 orders E0-A preparation as deterministic Access Control, Context Composer, Performer candidate output, Director opportunity rules, **Integrity Validator acceptance/rejection rules**, State Interpreter candidate-mutation schema, deterministic State Authority, accepted/rejected/alternate Take semantics, and atomic causal-commit record.

It separately freezes:

```text
Integrity Validator -> State Interpreter -> deterministic State Authority
```

Integrity Validator checks candidate Performance before acceptance, gives deterministic hard invariants precedence, may consume model/human-assisted semantic concern evidence that cannot waive hard rules, has dispositions accept/reject/request another take, and never rewrites Performance.

Patch 0008 owns only candidate-integrity responsibility. Access, State, Take, persistence, cost/cancellation, and orchestration keep their own hard gates.

## 3. Integrity is not truth policing

A Character may lie, be mistaken, speculate, repeat rumor, contradict another Character, state a false belief, or make a claim unsupported by objective Production truth.

Those are not automatically integrity failures.

A statement becomes an authority violation only when later authority silently promotes it to truth or when Performance credibly uses inaccessible information / itself presents an impossible or locked-authority violation as enacted Performance rather than merely a claim, belief, intention, or failed attempt.

Therefore Patch 0008 has no deterministic secret/canon keyword scanner, no `candidate text != objective truth -> Reject` rule, and no automatic rejection of false claims/beliefs.

Because Patch 0006 leaves non-empty Performance grammar open, any semantic distinction between claim/intention/attempt and enacted Performance belongs to bounded concern assessment rather than brittle deterministic text parsing.

## 4. Input boundary

The deterministic validator consumes exactly:

```text
ContextPacket sourceContext
CandidatePerformance candidate
IntegrityConcernAssessment concernAssessment
```

It does not consume ValidatedFixture/full Production, Access deny text, Director evaluation, State proposals/authority, Take/Commit identity, provider credentials/configuration, raw/partial provider output, model rationale/chain-of-thought, filesystem/network/clock/random state.

`sourceContext` is the Character-bounded semantic ContextPacket associated with Candidate generation.

`ContextPacketId` is structured semantic content identity under Patch 0005, not proof of exact rendered/provider disclosure or attempt identity. Exact rendering/provider-attempt facts remain separate provenance.

## 5. Candidate content identity contract

Patch 0006 intentionally introduced no CandidateId/TakeId. Integrity concern evidence must nevertheless bind to exact Candidate semantics reviewed.

Freeze:

```text
CandidateContentIdentityContract = ensemble.e0.integrity.candidate-content.v1
CandidateContentHash = SHA-256(canonical candidate-content envelope)
```

Canonical root order:

1. `identityContract`
2. `candidateContractVersion`
3. `subjectCharacterId`
4. `contextPacketId`
5. `visibleText`
6. `control`

Control order:

1. `addressedCharacterIds`
2. `nominatedCharacterId`

Rules:

- identityContract exactly `ensemble.e0.integrity.candidate-content.v1`;
- UTF-8 without BOM;
- reuse canonical JSON scalar/string + UTF-8 emission primitive already extracted for ECJ-1/Context;
- visible text emitted exactly;
- addressed IDs preserve Patch 0006 canonical ordinal order;
- nomination string or JSON null;
- SHA-256 lowercase 64-hex;
- identity-contract value participates in preimage.

## 6. CandidateContentHash is not attempt identity

CandidateContentHash is content identity only.

It is not provider-attempt identity, CandidateId, TakeId, causal-commit identity, acceptance authority, or provider/model proof.

Distinct attempts with identical Candidate semantics intentionally share the hash.

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

Concerns are immutable/distinct/frozen-order values from:

```text
PotentialInaccessibleInformationUse
PotentialProtectedInformationExposure
PotentialLockedAuthorityViolation
PotentialTechnicalArtifactLeak
IndeterminateSemanticIntegrity
```

These are engine integrity-policy categories, not creator-facing dramatic ontology.

Assessment contains no prose rationale, chain-of-thought, Character/private/forbidden text, confidence score, accept/reject authority, State mutation, or retry/spend authority.

`PotentialLockedAuthorityViolation` must not mean “the Character said something false.” It represents concern that Performance itself purports to enact/establish something incompatible with locked authority/world law.

`IndeterminateSemanticIntegrity` means a completed semantic assessment that cannot responsibly clear Candidate content. It never encodes assessor transport/refusal/timeout/cancellation failure.

## 8. Assessment binding

Preferred:

```text
IntegrityConcernAssessment.Bind(
    CandidatePerformance candidate,
    ImmutableArray<IntegrityConcernKind> concerns)
    -> IntegrityConcernAssessment
```

Bind requires initialized Candidate, computes CandidateContentHash, rejects default arrays, undefined enum values, duplicates, and counts above five, and stores concerns in frozen enum order.

A zero-length initialized array is valid and means the configured assessment path completed and reported no concerns.

Bind proves structural validity + Candidate-content association only. It does not authenticate assessor provenance.

Synthetic assessments are valid for deterministic tests/E0-F injection.

## 9. Assessor technical failure is not semantic concern

Later integration rule:

```text
completed semantic review but cannot clear content
    -> IndeterminateSemanticIntegrity

assessor technical/refusal/timeout/cancellation failure
    -> no valid concern assessment
    -> technical/orchestration failure path
```

A technical assessor failure must not automatically become RequestAnotherTake, because that would convert infrastructure failure into Character-facing retry pressure and potentially another paid generation call.

Deterministic fallback/cost policy later decides assessor fallback, human review, cancellation, or any retry.

## 10. Future semantic-assessor privacy requirement

Any later assessor path must obey Context Sovereignty:

- no complete Production disclosure by default;
- separate bounded integrity-assessment packet with Candidate content + minimum authoritative constraints/references;
- disclosure separate from Character Context;
- attributable assessor/provider/model provenance;
- no hidden reasoning exposure;
- no State mutation/commit/retry/spend authority;
- no ability to waive deterministic failure.

Exact assessor mechanism remains later scope.

## 11. E0 experimental isolation for future assessor

If E0 uses assisted Integrity review, assessment mechanism/configuration must remain fixed and attributable across E0-A through E0-E comparison batches unless a separately labeled experiment intentionally varies it.

Do not tune assessor prompts/rules between runs inside a frozen batch.

Assessment results, Integrity dispositions, technical assessor failures, and resulting attempt/retry behavior must be preserved in E0 provenance so Integrity behavior cannot silently confound Character/Director comparisons.

## 12. Trusted input defects versus Candidate rejection

### Integrity exception / no disposition

Use Integrity exception domain for:

- null/uninitialized malformed trusted objects;
- malformed source Context invariants normal public construction should prevent;
- unsupported Candidate semantic contract;
- unsupported Integrity assessment contract;
- unsupported Candidate-content identity contract;
- assessment hash != recomputed CandidateContentHash;
- malformed assessment concern collection.

These mean stale/miswired/unsupported evaluation and do not creatively Reject Candidate.

### Deterministic Candidate Reject codes

Closed E0 Reject codes:

```text
SubjectContextMismatch
ContextPacketIdentityMismatch
```

SubjectContextMismatch means independently valid Candidate subject differs from source Context subject.

ContextPacketIdentityMismatch means independently valid Candidate ContextPacketId differs from source ContextPacketId.

Source Context SubjectCharacterId != OpportunityCharacterId is malformed trusted source and exception-domain.

Patch 0008 does not duplicate Patch 0006 VisibleText/control/address/nomination validation.

## 13. Deterministic validation order

1. validate source Context trusted invariants;
2. validate Candidate supported/initialized enough for identity;
3. validate assessment structure/contracts;
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
Candidate Reject code(s) -> Reject
else concern(s) -> RequestAnotherTake
else -> Accept
```

Semantic concerns cannot soften deterministic Reject.

Accept means integrity-eligible for later State/Take work only.

It does not mean accepted Take, Production history, authoritative State, committed consequence, effective next opportunity, or retry/spend authority.

## 15. IntegrityEligibleCandidate structural handoff

Only Accept produces:

```text
IntegrityEligibleCandidate
- Candidate
- CandidateContentIdentityContract
- CandidateContentHash
```

Properties:

- read-only;
- no public constructor;
- created only by DeterministicIntegrityValidator on Accept;
- preserves exact CandidatePerformance semantics;
- carries content identity bound to assessment.

Meaning:

> this Candidate has passed the Patch 0008 Integrity evaluation under the supplied concern evidence and is eligible for later E0 State/Take processing.

It is not accepted Take, canon, history, State authority, retry authority, or causal identity.

Non-public construction protects sequence/object invariants, not assessor-provenance authenticity.

## 16. Evaluation shape

Freeze:

```text
IntegrityValidationEvaluation
- Disposition
- EligibleCandidate
- Trace
```

Invariant:

```text
Disposition == Accept
    <=> EligibleCandidate is non-null
```

Reject/RequestAnotherTake expose no eligibility handoff.

Patch 0008 does **not** freeze whether the future provisional Take boundary or State Interpreter is the immediate consumer of IntegrityEligibleCandidate. It freezes only that later E0 processing must not proceed from a raw Candidate that lacks Integrity clearance.

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

Candidate-provider refusal/timeout/transport error/cancellation/partial stream is not CandidatePerformance and must not be converted to fiction.

Validator does not infer transport provenance from words such as timeout/error/refusal.

PotentialTechnicalArtifactLeak exists only as typed concern evidence from a completed assessment path.

E0-F provider-failure hard gate remains later integration scope.

## 21. Director interaction

Director is not Integrity input.

Reject/RequestAnotherTake cannot promote associated Director work.

Accept also cannot promote precommit Director evaluation. Patch 0007's postcommit re-Bind/recompute rule remains unchanged.

## 22. State / Take boundary

Later State Interpreter and Take contracts must preserve:

```text
Integrity Accept != accepted Take
IntegrityEligibleCandidate != accepted Take
IntegrityEligibleCandidate != authoritative consequence
IntegrityEligibleCandidate != causal commit
```

State Interpreter remains proposal-only and State Authority remains deterministic.

The exact ordering/identity semantics of provisional Take selection versus State interpretation remain for their own approved contracts; Patch 0008 does not invent them.

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

Candidate identity is carried once through ConcernAssessment.

Trace contains no Candidate VisibleText, Context/protected/denied text, provider output, free-form rationale, confidence score, or chain-of-thought.

Trace is local provenance/diagnostics and never Character-facing Context.

## 24. Determinism

Identical source Context semantics + Candidate semantics + concern assessment + contracts produce identical CandidateContentHash, Reject codes, disposition, eligibility-handoff presence/identity, and trace.

No clock, filesystem, network, random, current culture, provider/model inference, GPU/NPU, or mutable global state.

## 25. Fail-closed exception domain

Use Integrity-specific exception type.

Errors expose structural field names/codes only and never Candidate VisibleText, Context/private/protected text, semantic rationale, or raw provider output.

Failure never becomes fallback disposition.

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

No public commit/retry/State mutation.

## 27. Required tests / review gates

### Candidate identity

1. identical Candidate -> identical hash;
2. visible text/address/nomination/ContextPacketId change -> hash changes;
3. Candidate contract + identity contract participate in preimage;
4. addressed order stable;
5. lowercase 64-hex;
6. content identity not attempt/Take/causal identity.

### Assessment

7. zero initialized concerns valid;
8. default concerns invalid;
9. undefined/duplicate/over-count invalid;
10. five kinds canonicalize frozen order;
11. hash matches bound Candidate;
12. no prose/confidence/authority fields;
13. Indeterminate means completed uncertainty, not technical failure.

### Exception vs Reject

14. malformed source Context -> exception;
15. unsupported Candidate/assessment/identity contract -> exception;
16. assessment for different Candidate -> exception;
17. valid wrong-subject Candidate -> Reject/SubjectContextMismatch;
18. valid different-Context Candidate -> Reject/ContextPacketIdentityMismatch;
19. no duplicate Patch 0006 parser validation.

### Disposition / eligibility

20. no Reject/no concerns -> Accept + EligibleCandidate;
21. Reject finding -> Reject + null eligibility;
22. Reject + concerns -> Reject;
23. concern -> RequestAnotherTake + null eligibility;
24. each concern independently -> RequestAnotherTake;
25. EligibleCandidate constructor non-public;
26. EligibleCandidate retains exact Candidate + identity;
27. only Accept can construct eligibility handoff;
28. handoff explicitly not Take/history authority;
29. Patch 0008 does not freeze State-Interpreter-vs-provisional-Take immediate consumer ordering.

### Creative law

30. false Character claim not rejected solely for objective-truth conflict;
31. no secret/canon keyword scanner;
32. no Performance rewrite;
33. no State/Context/Candidate mutation;
34. no Director dependency;
35. no State Authority/Take/Commit dependency;
36. no provider/model/network dependency;
37. no confidence/score/probability;
38. trace contains no Candidate/Context/protected prose;
39. technical words in dialogue do not deterministically create TechnicalArtifactLeak;
40. PotentialLockedAuthorityViolation is not triggered deterministically by mere false speech.

### Technical failure / experimental isolation

41. semantic uncertainty -> RequestAnotherTake;
42. assessor technical failure cannot be encoded as Indeterminate or automatically cause Character retry;
43. RequestAnotherTake has no retry/spend behavior;
44. assessor config/provenance required later and fixed across E0-A-E batches unless explicitly varied.

### Lifecycle

45. Reject remains non-history;
46. RequestAnotherTake remains non-history;
47. only EligibleCandidate may advance into later E0 State/Take processing;
48. precommit Director result not promoted by Accept;
49. no effective opportunity mutation/history append/Performer trigger.

### Regression

50. Missing Raft StructuredContextHash unchanged;
51. Missing Raft RenderedContextHash unchanged;
52. ECJ-1 9112 bytes/frozen hash unchanged;
53. existing 211 Core tests green;
54. Missing Raft Harness PASS/0;
55. smoke Harness PASS/0.

## 28. E0-F compatibility

Patch 0008 prepares but does not complete E0-F.

Later E0-F orchestration must prove inaccessible-secret failures, false-claim truth separation, locked canon/Constitution protection, malformed raw candidate rejection before CandidatePerformance, candidate/assessor provider-failure isolation, partial/rejected attempt non-history, deterministic retry/cost, and atomic accepted Performance + authoritative consequence commit.

Patch 0008 claims none of those end-to-end guarantees by itself.

## 29. ARM64 / battery

Candidate hashing + deterministic disposition are tiny CPU work with no background activity.

No AI/provider/network/GPU/NPU execution in Patch 0008.

Future semantic assessor performance/privacy remains unimplemented/unclaimed.

## 30. Explicit exclusions

No semantic-assessor implementation; bounded integrity-assessment packet implementation; assessor provenance authentication/persistence; provider attempt/provenance; retry/cost/cancellation engine; accepted/rejected/alternate Take semantics/identity; TakeId; State Interpreter implementation; State Authority; consequence proposal; ProductionState/StateHash; atomic causal commit; persistence/recovery; effective opportunity establishment; Scene loop; World Resolver/observation; E0-D round-robin execution; E0-E playwright; E0-F end-to-end injection harness; UI/WinUI; Windows AI/NPU; packaging/WACK/Store.

## 31. Recursive adversarial audit dimensions

Restart after every material correction:

1. frozen Blueprint 0.1 Integrity law;
2. H1 sequence/next-boundary correctness;
3. statement vs truth/claim/belief law;
4. Performance semantics vs consequence authority;
5. Character vs Performer authority;
6. Access/Context privacy;
7. Integrity vs Parser ownership;
8. Integrity vs Director ownership;
9. Integrity vs State Interpreter/Authority;
10. Integrity eligibility vs accepted Take;
11. provisional Take/Interpreter ordering non-preemption;
12. semantic concern evidence vs deterministic authority;
13. semantic uncertainty vs assessor technical failure;
14. E0 assessor control isolation;
15. assessment privacy/minimal disclosure;
16. stale-assessment/Candidate-content binding;
17. content identity contract/versioning;
18. content vs attempt/Take/causal identity;
19. technical failure vs fictional action;
20. exception/infrastructure fault vs Reject;
21. no hidden rewriting;
22. RequestAnotherTake vs retry/spend;
23. E0-F compatibility;
24. creator-ontology extensibility guard;
25. public API/non-forgeability/minimality;
26. deterministic bounded canonicalization;
27. fail closed/error sanitization;
28. tests without production bypass;
29. dependency direction;
30. ARM64/battery;
31. scope/hygiene;
32. E0-A/B/C/D/E/F/G isolation;
33. future State/Take/commit compatibility.

Approval only after complete restart produces zero material corrections or worthwhile improvements.

## 32. Material approval decisions

Approval would freeze only:

1. Patch 0008 as next E0-A boundary after Patch 0007;
2. Integrity evaluation before later State/Take authority;
3. versioned deterministic CandidateContentHash over exact Candidate semantics for stale-assessment binding;
4. CandidateContentHash content identity only, never attempt/Candidate/Take/commit identity;
5. typed IntegrityConcernAssessment non-authoritative evidence, not assessor authentication;
6. five bounded E0 concern kinds with no rationale/confidence;
7. PotentialLockedAuthorityViolation concerns enacted Performance conflict, not mere false claim/belief;
8. IndeterminateSemanticIntegrity means completed semantic uncertainty only; assessor technical failure is separate infrastructure failure;
9. no semantic assessor/provider implementation in Patch 0008;
10. future semantic review uses least-privilege assessment packet rather than full Production by default;
11. future E0 assessor configuration/provenance fixed/attributable across E0-A-E unless explicitly varied;
12. deterministic Reject codes limited to subject mismatch and source-ContextPacket mismatch;
13. malformed/stale/unsupported evaluator/contract evidence is Integrity exception-domain, not creative Reject;
14. precedence: Reject code -> Reject; otherwise concern -> RequestAnotherTake; otherwise Accept;
15. only Accept creates non-public IntegrityEligibleCandidate;
16. IntegrityEligibleCandidate means later processing eligibility only, not Take/history/State authority;
17. exact immediate ordering between provisional Take selection and State Interpreter remains for later contracts;
18. later E0 State/Take processing must not advance from raw Candidate lacking Integrity clearance;
19. RequestAnotherTake does not authorize retry/spend;
20. no objective-truth contradiction scanner and no hidden rewriting;
21. candidate/assessor provider failures remain separate technical/orchestration concerns;
22. Director is not Integrity input and Accept cannot promote precommit Director work;
23. trace contains only source Context identity + assessment + structural Reject codes, never Performance/private prose;
24. Patch 0008 does not implement State Interpreter, State Authority, Take, commit, persistence, opportunity application, Scene loop, provider execution, UI, Windows AI/NPU, or Store machinery.

Implementation remains blocked until recursive audit completes and user explicitly approves the final proposal.
