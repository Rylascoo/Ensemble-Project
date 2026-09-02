# H1 Patch 0008 — E0 Integrity Validator Contract

Status: blueprint proposal 0.6 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0007
Branch: `h1-patch-0008-integrity-validator-blueprint`

## 1. Purpose

Define the next frozen E0-A boundary after Performer Candidate and Director proposal calculation:

```text
ContextPacket + CandidatePerformance
    -> IntegrityCandidateInput.Bind
        -> deterministic Reject codes?
            yes -> Reject without semantic assessment
            no  -> completed bounded IntegrityConcernAssessment
                    -> deterministic Integrity Validator
                        concern(s) -> RequestAnotherTake
                        none       -> Accept + IntegrityEligibleCandidate
```

Patch 0008 defines candidate-integrity evaluation and a structurally gated eligibility handoff.

It does not create an accepted Take, mutate Production, interpret consequences, apply State, commit history, authorize retry/spend, establish Current Opportunity, call a provider/model, or implement semantic-assessor transport.

## 2. Recovered frozen authority

Blueprint 0.1 orders E0-A preparation as Access Control, Context Composer, Performer candidate output, Director opportunity rules, **Integrity Validator acceptance/rejection rules**, State Interpreter candidate-mutation schema, deterministic State Authority, Take semantics, then atomic causal-commit record.

It freezes:

```text
Integrity Validator -> State Interpreter -> deterministic State Authority
```

Integrity Validator checks candidate Performance before acceptance, gives deterministic hard invariants precedence, may consume assisted semantic concern evidence that cannot waive hard rules, returns accept/reject/request another take, and never rewrites Performance.

Patch 0008 owns only candidate-integrity responsibility. Other hard gates remain with Access, State, Take, persistence, cost/cancellation, and orchestration.

## 3. Integrity is not truth policing

A Character may lie, be mistaken, speculate, repeat rumor, contradict another Character, state a false belief, or make a claim unsupported by Production truth.

Those are not automatically integrity failures.

A statement becomes an authority violation only when later authority silently promotes it to truth or when Performance credibly uses inaccessible information / presents an impossible or locked-authority violation as enacted Performance rather than claim, belief, intention, or failed attempt.

Patch 0008 therefore has no deterministic secret/canon keyword scanner, no objective-truth contradiction rejection rule, and no automatic rejection of false claims/beliefs.

Patch 0006 leaves non-empty Performance grammar open, so semantic distinctions belong to bounded concern assessment rather than brittle deterministic text parsing.

## 4. Rich-object binding boundary

Freeze:

```text
IntegrityCandidateInput.Bind(
    ContextPacket sourceContext,
    CandidatePerformance candidate)
    -> IntegrityCandidateInput
```

This is the only Patch 0008 operation receiving full ContextPacket + CandidatePerformance together.

Bind validates trusted prerequisites, computes Candidate content identity and deterministic Reject codes, and returns least-privilege immutable input.

The deterministic Validator does not receive ContextPacket or raw CandidatePerformance.

## 5. IntegrityCandidateInput public shape

```text
IntegrityCandidateInput
- CandidateContentIdentityContract
- CandidateContentHash
- SourceContextPacketId
- DeterministicRejectCodes
```

No public constructor.

No Candidate VisibleText, Context prose, rendering, roster display names, private/denied records, provider data, or semantic rationale is publicly exposed.

Core may internally retain exact Candidate solely so Accept can construct IntegrityEligibleCandidate without re-supplying rich input.

## 6. Bind trusted prerequisites

Integrity exception-domain for null/malformed trusted objects, uninitialized Context identity/subject/opportunity, Context subject != opportunity, unsupported Candidate semantic contract, or malformed Candidate state impossible through normal public Candidate construction.

Bind does not reimplement Patch 0006 text/control validation; defensive reading checks only protect safe hashing/access.

## 7. Deterministic Reject codes

Exactly:

```text
SubjectContextMismatch
ContextPacketIdentityMismatch
```

Frozen order:

1. SubjectContextMismatch
2. ContextPacketIdentityMismatch

Store each at most once, in that order.

SubjectContextMismatch: Candidate subject != source Context subject.

ContextPacketIdentityMismatch: Candidate ContextPacketId != source ContextPacketId.

Both may coexist.

## 8. Candidate content identity

```text
CandidateContentIdentityContract = ensemble.e0.integrity.candidate-content.v1
CandidateContentHash = SHA-256(canonical candidate-content envelope)
```

Root order:

1. `identityContract`
2. `candidateContractVersion`
3. `subjectCharacterId`
4. `contextPacketId`
5. `visibleText`
6. `control`

Control order:

1. `addressedCharacterIds`
2. `nominatedCharacterId`

Rules: exact identity contract in preimage; UTF-8 no BOM; reuse canonical JSON scalar/string + UTF-8 primitive; visible text exact; addressed IDs in Patch 0006 ordinal order; nomination string/null; lowercase 64-hex SHA-256.

## 9. CandidateContentHash semantics

Content identity only—not provider attempt, CandidateId, TakeId, commit ID, acceptance authority, or provider/model proof.

Identical Candidate semantics across distinct attempts intentionally share hash.

ContextPacketId in preimage is structured semantic identity, not proof of exact rendered/provider disclosure. Exact disclosure/attempt facts remain separate provenance.

## 10. Integrity concern assessment evidence

```text
IntegrityConcernAssessment
- AssessmentContract
- CandidateContentIdentityContract
- CandidateContentHash
- Concerns
```

```text
AssessmentContract = ensemble.e0.integrity.concerns.v1
```

Frozen concern kinds/order:

1. PotentialInaccessibleInformationUse
2. PotentialProtectedInformationExposure
3. PotentialLockedAuthorityViolation
4. PotentialTechnicalArtifactLeak
5. IndeterminateSemanticIntegrity

Assessment has no prose rationale, chain-of-thought, Character/private/forbidden text, confidence score, disposition authority, State mutation, or retry/spend authority.

PotentialLockedAuthorityViolation means concern about enacted/established locked conflict, not mere false speech.

IndeterminateSemanticIntegrity means completed semantic review that cannot responsibly clear content, never technical assessor failure.

## 11. Assessment binding

```text
IntegrityConcernAssessment.Bind(
    IntegrityCandidateInput input,
    ImmutableArray<IntegrityConcernKind> concerns)
    -> IntegrityConcernAssessment
```

Bind copies input identity contract/hash; rejects default arrays, undefined kinds, duplicates, count > 5; stores frozen order.

Zero-length initialized array means configured assessment path completed with no concerns.

Structural association only; no assessor provenance authentication.

Synthetic assessments allowed for deterministic tests/E0-F injection.

## 12. Deterministic hard-rule short circuit

If `IntegrityCandidateInput.DeterministicRejectCodes` is non-empty, **no semantic assessment is required for disposition**.

The deterministic path must be able to return Reject immediately.

E0 orchestration should not invoke a semantic assessor for such input because the result cannot change and additional disclosure/cost would be unnecessary.

If an assessment was already computed speculatively, it cannot soften Reject. The canonical reference flow should avoid that work.

## 13. Assessor technical failure is not semantic concern

For an input with zero deterministic Reject codes:

```text
completed review but cannot clear content
    -> IndeterminateSemanticIntegrity

assessor transport/provider/refusal/timeout/cancellation failure
    -> no valid assessment
    -> technical/orchestration failure
    -> no Integrity disposition
```

Technical assessor failure must not automatically become RequestAnotherTake or another paid Character-generation call.

Later deterministic fallback/cost policy owns recovery.

## 14. Future semantic-assessor privacy

Any later assessor path must avoid complete Production disclosure by default, use a separate bounded assessment packet with Candidate + minimum authoritative constraints/references, keep it separate from Character Context, preserve assessor/provider/model provenance, expose no hidden reasoning, have no State/commit/retry/spend authority, and never waive deterministic Rejects.

Exact assessor mechanism remains later scope.

## 15. E0 experimental isolation

If E0 uses assisted review, mechanism/configuration remains fixed and attributable across E0-A through E0-E comparison batches unless explicitly varied.

No assessor prompt/rule tuning inside frozen batch.

Assessment results, dispositions, assessor technical failures, and resulting attempt/retry behavior are experimental provenance.

## 16. Deterministic Validator API

Freeze nullable-assessment shape only to represent hard-rule short circuit:

```text
DeterministicIntegrityValidator.Validate(
    IntegrityCandidateInput input,
    IntegrityConcernAssessment? concernAssessment)
    -> IntegrityValidationEvaluation
```

Rules:

- if Reject codes non-empty: assessment is not required; disposition Reject;
- if Reject codes empty: a valid matching concernAssessment is required;
- missing assessment on otherwise eligible input -> Integrity exception/no disposition;
- supplied assessment on Reject path cannot change Reject.

Validator receives no rich Context/Candidate prose.

## 17. Assessment/input validation

When assessment is required or supplied for a non-Reject input, require supported assessment contract, supported candidate identity contract, matching input/assessment identity contract and hash, and canonical concern set.

Mismatch/unsupported/malformed assessment -> Integrity exception/no disposition.

On deterministic Reject short-circuit, canonical reference behavior does not require or depend on assessment.

## 18. Integrity disposition

```text
IntegrityDisposition
- Accept
- Reject
- RequestAnotherTake
```

Exact precedence:

```text
RejectCodes non-empty -> Reject
else matching assessment missing -> exception / no disposition
else concerns non-empty -> RequestAnotherTake
else -> Accept
```

Accept means Integrity eligibility only, not accepted Take/history/State/commit/opportunity/retry authority.

## 19. IntegrityEligibleCandidate

Only Accept creates:

```text
IntegrityEligibleCandidate
- ValidationContract
- Candidate
- CandidateContentIdentityContract
- CandidateContentHash
```

```text
ValidationContract = ensemble.e0.integrity.validation.v1
```

No public constructor; read-only.

Preserves exact Candidate bound by Input and same content identity.

Meaning: Candidate passed Patch 0008 Integrity evaluation under supplied concern evidence and may enter later E0 State/Take processing.

Not accepted Take/canon/history/State/causal identity.

Non-public construction protects sequence invariants, not assessor-provenance authenticity.

## 20. Evaluation shape

```text
IntegrityValidationEvaluation
- Disposition
- EligibleCandidate
- Trace
```

Invariant:

```text
Accept <=> EligibleCandidate != null
```

Reject/Request expose null eligibility.

Patch 0008 does not freeze whether provisional Take selection or State Interpreter is immediate consumer. Later E0 processing cannot proceed from raw Candidate lacking Integrity eligibility.

## 21. RequestAnotherTake is not retry authority

No provider call, spend, retry count, understudy choice, opportunity change, or new Candidate construction.

Later deterministic orchestration/cost policy owns recovery.

Candidate remains unaccepted diagnostics/provenance only.

## 22. Reject is not fictional action

Rejected Candidate does not enter Production/recent-performance/Character/observation/opportunity history.

Diagnostics only when later provenance exists.

## 23. No hidden rewriting

No replacement/corrected text, paraphrase, suggested line, or rewritten control.

Candidate remains exact Performer output.

## 24. Technical provider failure boundary

Candidate-provider refusal/timeout/transport error/cancellation/partial stream is not CandidatePerformance and must not become fiction.

No technical-word scanning in dialogue.

PotentialTechnicalArtifactLeak exists only as typed concern evidence from completed assessment.

E0-F provider-failure gate remains later integration scope.

## 25. Director interaction

Director is not Integrity input.

Reject/Request cannot promote associated Director work.

Accept cannot promote precommit Director evaluation; Patch 0007 postcommit re-Bind/recompute remains unchanged.

## 26. State / Take boundary

Later contracts preserve:

```text
Integrity Accept != accepted Take
IntegrityEligibleCandidate != accepted Take
IntegrityEligibleCandidate != authoritative consequence
IntegrityEligibleCandidate != causal commit
```

State Interpreter remains proposal-only and State Authority deterministic.

Exact provisional-Take-vs-State-Interpreter immediate ordering remains later authority.

## 27. Trace

```text
IntegrityValidationTrace
- ValidationContract
- Input
- ConcernAssessment
```

ConcernAssessment is nullable only on deterministic Reject short-circuit.

Input owns source Context identity, Candidate content identity, Reject codes. Assessment owns concern evidence. No duplication.

Trace contains no Candidate/Context/protected prose, provider output, rationale, confidence, or chain-of-thought.

## 28. Evaluation invariants

Freeze:

- Accept => assessment non-null, zero concerns, zero Reject codes, EligibleCandidate non-null;
- RequestAnotherTake => assessment non-null, concerns non-empty, zero Reject codes, EligibleCandidate null;
- Reject => Reject codes non-empty, EligibleCandidate null; canonical reference flow assessment null;
- exception/no disposition => no Evaluation object.

## 29. Determinism

Identical bound input + matching assessment (where required) + contracts produce identical disposition/handoff/trace.

Bind deterministic from identical Context/Candidate semantics.

No clock/filesystem/network/random/culture/provider/model/GPU/NPU/global mutable state.

## 30. Fail closed

Integrity-specific exception domain; sanitized structural messages only.

Failure never becomes fallback disposition.

## 31. Public-surface intent

```text
IntegrityCandidateInput.Bind(ContextPacket, CandidatePerformance)
    -> IntegrityCandidateInput

IntegrityConcernAssessment.Bind(
    IntegrityCandidateInput,
    ImmutableArray<IntegrityConcernKind>)
    -> IntegrityConcernAssessment

DeterministicIntegrityValidator.Validate(
    IntegrityCandidateInput,
    IntegrityConcernAssessment?)
    -> IntegrityValidationEvaluation
```

Public models read-only; no public constructors except bind/evaluate paths.

No commit/retry/State mutation API.

## 32. Required tests / review gates

### Least privilege / Bind

1. valid Missing Raft Voss Context+Candidate -> Input;
2. Input exact public structural fields only;
3. no Context/Candidate prose/render/private record exposure;
4. Validator API accepts Input + nullable Assessment only;
5. no ContextPacket/CandidatePerformance Validator overload;
6. Input constructor non-public.

### Candidate identity

7. identical Candidate -> identical hash;
8. visible text/address/nomination/ContextPacketId change -> hash changes;
9. Candidate + identity contracts in preimage;
10. addressed order stable;
11. lowercase 64-hex;
12. not attempt/Take/causal identity.

### Reject codes

13. wrong subject -> SubjectContextMismatch;
14. wrong Context -> ContextPacketIdentityMismatch;
15. both mismatch -> both in frozen order;
16. no duplicates;
17. malformed source/unsupported Candidate contract -> exception;
18. no duplicate Patch 0006 parser validation.

### Assessment

19. zero initialized concerns valid;
20. default/undefined/duplicate/over-count invalid;
21. five kinds frozen order;
22. copied input identity;
23. no prose/confidence/authority fields;
24. Indeterminate completed uncertainty only.

### Hard-rule short circuit

25. Reject codes + null assessment -> Reject;
26. Reject codes do not require assessment;
27. concerns cannot soften Reject;
28. reference flow does not invoke assessor for deterministic Reject;
29. Reject trace assessment null in canonical flow.

### Assessment-required path

30. zero Reject + null assessment -> Integrity exception/no disposition;
31. assessment for different hash -> exception;
32. unsupported assessment/identity contract -> exception;
33. zero Reject + concern -> RequestAnotherTake;
34. zero Reject + zero concerns -> Accept.

### Eligibility

35. Accept -> EligibleCandidate with validation contract + exact Candidate identity;
36. Reject/Request -> no handoff;
37. constructor non-public;
38. not Take/history/State authority;
39. no immediate State-vs-provisional-Take ordering frozen.

### Creative law

40. false claim not rejected solely for objective-truth conflict;
41. no secret/canon keyword scanner;
42. no Performance rewrite;
43. no State/Context/Candidate mutation;
44. no Director/State Authority/Take/Commit/provider dependency;
45. no score/probability;
46. trace public surface no prose;
47. technical words in dialogue do not auto-create concern;
48. PotentialLockedAuthorityViolation is supplied semantic evidence, not deterministic false-speech detector.

### Technical failure / E0 isolation

49. semantic uncertainty concern -> RequestAnotherTake;
50. assessor technical failure -> no assessment/no disposition, not Indeterminate;
51. RequestAnotherTake no retry/spend;
52. assessor config fixed/attributable across E0-A-E unless explicit variable.

### Lifecycle

53. Reject non-history;
54. Request non-history;
55. only EligibleCandidate may enter later E0 State/Take processing;
56. Accept does not promote Director;
57. no opportunity/history mutation/trigger.

### Regression

58. Missing Raft StructuredContextHash unchanged;
59. Missing Raft RenderedContextHash unchanged;
60. ECJ-1 9112 bytes/frozen hash unchanged;
61. existing 211 Core tests green;
62. Missing Raft Harness PASS/0;
63. smoke Harness PASS/0.

## 33. E0-F compatibility

Patch 0008 prepares but does not complete E0-F.

Later E0-F proves secret failures, false-claim truth separation, locked canon/Constitution protection, malformed raw candidate rejection, candidate/assessor provider-failure isolation, partial/rejected non-history, deterministic retry/cost, and atomic accepted Performance + authoritative consequence commit.

No end-to-end guarantee is claimed here.

## 34. ARM64 / battery

Bind/hash/disposition are tiny deterministic CPU work. No background/AI/provider/network/GPU/NPU execution.

## 35. Explicit exclusions

No semantic-assessor implementation; bounded assessor packet; assessor provenance authentication/persistence; provider attempt/provenance; retry/cost/cancellation engine; Take semantics/identity/TakeId; State Interpreter implementation; State Authority; consequence proposal; ProductionState/StateHash; atomic commit; persistence/recovery; effective opportunity establishment; Scene loop; World Resolver/observation; E0-D round-robin; E0-E playwright; E0-F end-to-end harness; UI/WinUI; Windows AI/NPU; packaging/WACK/Store.

## 36. Recursive adversarial audit dimensions

Restart after every correction:

1. frozen Integrity law;
2. H1 sequence;
3. statement/truth distinctions;
4. Performance vs consequence authority;
5. Character vs Performer;
6. Access/Context privacy;
7. least-privilege Input;
8. hard-rule-before-semantic-assessment precedence;
9. Integrity vs Parser;
10. Integrity vs Director;
11. Integrity vs State/Take;
12. eligibility vs accepted Take;
13. Take/Interpreter ordering non-preemption;
14. semantic evidence vs deterministic authority;
15. uncertainty vs assessor technical failure;
16. assessor disclosure/cost short circuit;
17. E0 assessor control isolation;
18. assessment privacy;
19. stale assessment/content binding;
20. content identity versioning;
21. content vs attempt/Take/causal identity;
22. technical failure vs fiction;
23. exception vs Reject;
24. no rewriting;
25. RequestAnotherTake vs retry/spend;
26. E0-F compatibility;
27. creator ontology guard;
28. API/non-forgeability/minimality;
29. deterministic bounded canonicalization;
30. fail closed/sanitization;
31. tests without bypass;
32. dependency direction;
33. ARM64/battery;
34. scope/hygiene;
35. E0-A/B/C/D/E/F/G isolation;
36. future State/Take/commit compatibility.

Approval only after complete restart produces zero material corrections or worthwhile improvements.

## 37. Material approval decisions

Approval would freeze only:

1. Patch 0008 as next E0-A boundary after Patch 0007;
2. least-privilege IntegrityCandidateInput is sole rich Context+Candidate binding surface;
3. Input publicly carries only source Context ID, Candidate content identity, Reject codes;
4. deterministic hard Rejects short-circuit semantic assessment/disclosure;
5. Validator itself receives no Character/Performance prose;
6. versioned CandidateContentHash binds assessment to exact Candidate semantics;
7. content hash not attempt/Candidate/Take/commit identity;
8. Reject codes exactly subject mismatch then ContextPacket mismatch;
9. malformed source/unsupported Candidate contract is exception-domain;
10. five-kind ConcernAssessment non-authoritative, no rationale/confidence;
11. completed semantic uncertainty differs from assessor technical failure;
12. valid assessment required only when no deterministic Reject exists;
13. assessor failure on otherwise eligible Candidate yields no Integrity disposition;
14. no semantic assessor/provider implementation in Patch 0008;
15. future semantic review uses least-privilege packet and E0 assessor config/provenance stays fixed/attributable across E0-A-E;
16. precedence: RejectCodes -> Reject; else missing assessment -> exception; else concerns -> RequestAnotherTake; else Accept;
17. only Accept creates IntegrityEligibleCandidate carrying validation contract + exact Candidate content identity;
18. eligibility not Take/history/State authority;
19. immediate provisional-Take-vs-State-Interpreter ordering remains later;
20. later State/Take processing cannot proceed from raw Candidate lacking Integrity eligibility;
21. RequestAnotherTake does not authorize retry/spend;
22. false claims not objective-truth-policed; no keyword scan or hidden rewriting;
23. candidate/assessor technical failures stay technical/orchestration concerns;
24. Director not Integrity input; Accept cannot promote precommit Director work;
25. trace nests Input + optional Assessment and contains no Performance/private prose;
26. no State Interpreter/Authority, Take, commit, persistence, opportunity application, Scene loop, provider execution, UI, Windows AI/NPU, or Store machinery.

Implementation remains blocked until recursive audit completes and user explicitly approves final proposal.
