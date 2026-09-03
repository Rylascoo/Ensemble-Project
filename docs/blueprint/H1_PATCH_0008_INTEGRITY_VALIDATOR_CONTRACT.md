# H1 Patch 0008 — E0 Integrity Validator Contract

Status: blueprint proposal 0.7 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0007
Branch: `h1-patch-0008-integrity-validator-blueprint`

## 1. Purpose

Define the next frozen E0-A calculation boundary after Performer Candidate and Director proposal calculation:

```text
ContextPacket + CandidatePerformance
    -> IntegrityCandidateInput.Bind
        -> deterministic Reject codes?
            yes -> Reject without semantic concern assessment
            no  -> completed bounded IntegrityConcernAssessment
                    -> deterministic Integrity Validator
                        concern(s) -> RequestAnotherTake
                        none       -> Accept
```

Patch 0008 defines deterministic candidate-integrity **evaluation** only.

It does not create an accepted Take, an authority-bearing eligibility token, State interpretation, Production mutation, causal commit, provider retry/spend, Current Opportunity, semantic-assessor transport, or assessor-provenance authentication.

## 2. Recovered frozen authority

Blueprint 0.1 orders E0-A preparation as Access Control, Context Composer, Performer candidate output, Director opportunity rules, **Integrity Validator acceptance/rejection rules**, State Interpreter candidate-mutation schema, deterministic State Authority, Take semantics, and atomic causal-commit record.

It freezes:

```text
Integrity Validator -> State Interpreter -> deterministic State Authority
```

Integrity Validator checks candidate Performance before acceptance; deterministic hard invariants have precedence; assisted semantic checks may flag concerns but cannot waive hard rules; dispositions are accept, reject, or request another take; hidden rewriting is forbidden.

Patch 0008 defines the evaluation result needed by that later sequence. It does not pretend that a structurally constructible assessment proves the configured assessor actually ran.

## 3. Integrity evaluation is not acceptance authority

Freeze this distinction:

```text
IntegrityDisposition.Accept != accepted Take
IntegrityValidationEvaluation != Take authority
IntegrityValidationEvaluation != State authority
```

An `Accept` means only:

> Under this Patch 0008 input and supplied structurally bound concern assessment, no deterministic Reject condition or reported semantic concern prevents progression.

For later effective E0 processing, orchestration must additionally establish that the concern assessment came from the configured review path for the relevant attempt/batch and preserve that provenance.

Synthetic concern assessments may therefore produce synthetic Accept evaluations for tests without creating authoritative Production eligibility.

No Patch 0008 object claims assessor authenticity.

## 4. Integrity is not truth policing

A Character may lie, be mistaken, speculate, repeat rumor, contradict another Character, state a false belief, or make a claim unsupported by Production truth.

Those are not automatically integrity failures.

A statement becomes an authority violation only if later authority silently promotes it to truth, or when Performance credibly uses inaccessible information / purports to enact or establish something incompatible with locked authority rather than merely claiming, believing, intending, attempting, or failing.

Patch 0008 therefore has no deterministic secret/canon keyword scanner, no objective-truth contradiction rejection rule, and no automatic rejection of false claims/beliefs.

Patch 0006 leaves non-empty Performance grammar open, so semantic distinctions belong to bounded concern assessment rather than deterministic prose parsing.

## 5. Rich-object binding boundary

Freeze:

```text
IntegrityCandidateInput.Bind(
    ContextPacket sourceContext,
    CandidatePerformance candidate)
    -> IntegrityCandidateInput
```

This is the only Patch 0008 operation that receives full ContextPacket + CandidatePerformance together.

Bind validates trusted prerequisites, computes Candidate content identity and deterministic Reject codes, and returns a least-privilege immutable input.

The deterministic Validator itself receives no ContextPacket or raw CandidatePerformance.

## 6. IntegrityCandidateInput public shape

```text
IntegrityCandidateInput
- CandidateContentIdentityContract
- CandidateContentHash
- SourceContextPacketId
- DeterministicRejectCodes
```

No public constructor.

No Candidate VisibleText, Context prose, rendering, roster display names, private/denied records, provider data, or semantic rationale is publicly exposed.

Patch 0008 needs no hidden retained Candidate reference because it creates no later authority-bearing handoff object.

## 7. Bind trusted prerequisites

Integrity exception-domain for null/malformed trusted objects, uninitialized Context identity/subject/opportunity, Context subject != opportunity, unsupported Candidate semantic contract, or malformed Candidate state impossible through normal public Candidate construction.

Bind does not reimplement Patch 0006 text/control validation; defensive reading checks only protect safe hashing/access.

## 8. Deterministic Reject codes

Exactly:

```text
SubjectContextMismatch
ContextPacketIdentityMismatch
```

Frozen order:

1. SubjectContextMismatch
2. ContextPacketIdentityMismatch

Each appears at most once.

SubjectContextMismatch: Candidate subject != source Context subject.

ContextPacketIdentityMismatch: Candidate ContextPacketId != source ContextPacketId.

Both may coexist.

These codes mean the independently valid Candidate cannot be evaluated as the Performance for this source Character opportunity.

## 9. Candidate content identity

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

Rules: exact identity-contract value participates in preimage; UTF-8 without BOM; reuse existing canonical JSON scalar/string + UTF-8 primitive; visible text exact; addressed IDs preserve Patch 0006 canonical ordinal order; nomination string/null; lowercase 64-hex SHA-256.

## 10. CandidateContentHash semantics

Content identity only—not provider-attempt identity, CandidateId, TakeId, causal-commit identity, acceptance authority, or provider/model proof.

Distinct attempts with identical Candidate semantics intentionally share it.

ContextPacketId in the preimage is structured semantic identity, not proof of exact rendered/provider disclosure. Exact disclosure/attempt facts remain separate provenance.

## 11. Integrity concern assessment evidence

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

Assessment has no prose rationale, chain-of-thought, Character/private/forbidden text, confidence score, disposition authority, State mutation, retry/spend authority, assessor identity, or authenticity flag.

PotentialLockedAuthorityViolation concerns Performance purporting to enact/establish a locked/world-law conflict, not mere false speech.

IndeterminateSemanticIntegrity means a completed semantic review that cannot responsibly clear content, never assessor technical failure.

## 12. Assessment binding

```text
IntegrityConcernAssessment.Bind(
    IntegrityCandidateInput input,
    ImmutableArray<IntegrityConcernKind> concerns)
    -> IntegrityConcernAssessment
```

Bind requires:

- non-null/initialized Input;
- **zero deterministic Reject codes**;
- initialized concern array;
- only defined frozen concern kinds;
- no duplicates;
- count <= 5.

It copies Input identity contract/hash and stores concerns in frozen enum order.

A zero-length initialized array is valid and means only that the supplied assessment evidence reports no concerns.

Binding to an Input that already has Reject codes is an Integrity exception. This structurally preserves hard-rule short circuit and prevents unnecessary semantic-assessment artifacts from becoming part of the canonical Reject path.

Assessment binding proves structural association only; it does not authenticate assessor provenance.

Synthetic assessments remain valid for deterministic tests/E0-F injection on otherwise structurally eligible Inputs.

## 13. Deterministic hard-rule short circuit

If `IntegrityCandidateInput.DeterministicRejectCodes` is non-empty:

- semantic assessment is not required;
- canonical evaluation is Reject immediately;
- canonical trace has no concern assessment;
- concern assessment cannot be bound to that Input through the public binding path.

E0 orchestration should not invoke a semantic assessor because it cannot change the outcome and would add unnecessary disclosure/cost.

## 14. Assessor technical failure is not semantic concern

For an Input with zero deterministic Reject codes:

```text
completed semantic review but cannot clear content
    -> IndeterminateSemanticIntegrity

assessor transport/provider/refusal/timeout/cancellation failure
    -> no valid IntegrityConcernAssessment
    -> technical/orchestration failure
    -> no Integrity disposition
```

Technical assessor failure must not automatically become RequestAnotherTake or another paid Character-generation call.

Later deterministic fallback/cost policy owns recovery.

## 15. Future semantic-assessor privacy

Any later assessor path must avoid complete Production disclosure by default; use a separate bounded assessment packet containing Candidate content plus minimum authoritative constraints/references; keep disclosure separate from Character Context; preserve assessor/provider/model provenance; expose no hidden reasoning; have no State/commit/retry/spend authority; and never waive deterministic Rejects.

Exact assessor mechanism remains later scope.

## 16. E0 experimental isolation

If E0 uses assisted review, mechanism/configuration must remain fixed and attributable across E0-A through E0-E comparison batches unless explicitly varied.

No assessor prompt/rule tuning inside a frozen batch.

Assessment results, dispositions, assessor technical failures, and resulting attempt/retry behavior belong in experimental provenance.

## 17. Deterministic Validator API

Freeze:

```text
DeterministicIntegrityValidator.Validate(
    IntegrityCandidateInput input,
    IntegrityConcernAssessment? concernAssessment)
    -> IntegrityValidationEvaluation
```

Nullable assessment exists only because deterministic Reject requires none.

Exact rules:

1. validate Input structural contract;
2. if Reject codes non-empty:
   - concernAssessment **must be null**;
   - non-null assessment -> Integrity exception/no disposition;
   - null -> Reject;
3. if Reject codes empty:
   - matching valid concernAssessment is required;
   - null -> Integrity exception/no disposition;
   - mismatch/unsupported/malformed -> Integrity exception/no disposition;
   - concerns non-empty -> RequestAnotherTake;
   - concerns empty -> Accept.

Validator receives no rich Context/Candidate prose.

## 18. Why non-null assessment on Reject is an error

The public assessment binder cannot create assessment evidence for a Reject-coded Input.

Therefore a non-null assessment paired with such an Input indicates stale/mismatched/internal misuse rather than useful evidence.

The Validator fails closed rather than ignoring it. This prevents accidental provenance suggesting semantic review influenced a deterministic Reject.

## 19. Integrity disposition

```text
IntegrityDisposition
- Accept
- Reject
- RequestAnotherTake
```

Disposition is a deterministic evaluation result only.

Accept is not an acceptance token. Reject is not a fictional event. RequestAnotherTake is not retry authority.

## 20. Evaluation shape

```text
IntegrityValidationEvaluation
- Disposition
- Trace
```

No Candidate or eligibility token is emitted.

Later E0 State/Take/orchestration authority must bind the original Candidate/attempt to this evaluation and authenticated assessment provenance under its own approved contract before progressing.

Patch 0008 deliberately does not freeze whether provisional Take selection or State Interpreter is the immediate downstream consumer.

## 21. RequestAnotherTake is not retry authority

RequestAnotherTake does not call a provider, authorize spend, increment retry budget, choose an understudy, modify Current Opportunity, or create another Candidate.

Later deterministic orchestration/cost policy decides recovery.

Candidate remains unaccepted diagnostics/provenance only.

## 22. Reject is not fictional action

Rejected Candidate does not enter Production history, recent-performance Context, Character state, observation history, or effective opportunity history.

It remains diagnostics only when later provenance exists.

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

Later contracts must preserve:

```text
Integrity Accept != accepted Take
IntegrityValidationEvaluation != accepted Take
IntegrityValidationEvaluation != authoritative consequence
IntegrityValidationEvaluation != causal commit
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

```text
ValidationContract = ensemble.e0.integrity.validation.v1
```

ConcernAssessment is null only for canonical deterministic Reject.

Input owns source Context identity, Candidate content identity, Reject codes. Assessment owns concern evidence. No duplication.

Trace contains no Candidate/Context/protected prose, provider output, rationale, confidence, chain-of-thought, or assessor-authenticity claim.

## 28. Evaluation invariants

- Accept => zero Reject codes, non-null matching assessment, zero concerns;
- RequestAnotherTake => zero Reject codes, non-null matching assessment, concerns non-empty;
- Reject => Reject codes non-empty, null assessment;
- exception/no disposition => no Evaluation object.

## 29. Determinism

Identical bound Input + matching assessment where required + contract versions produce identical disposition/trace.

Bind deterministic from identical Context/Candidate semantics.

No clock/filesystem/network/random/culture/provider/model/GPU/NPU/global mutable state.

## 30. Fail closed

Integrity-specific exception domain with sanitized structural messages only.

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

Public models read-only with non-public constructors except approved Bind/Validate construction paths.

No authority-bearing eligibility token, commit, retry, or State mutation API.

## 32. Required tests / review gates

### Least privilege / Input
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
15. both mismatch -> both frozen order;
16. no duplicates;
17. malformed source/unsupported Candidate contract -> exception;
18. no duplicate Patch 0006 parser validation.

### Assessment binding
19. zero initialized concerns valid on zero-Reject Input;
20. default/undefined/duplicate/over-count invalid;
21. five kinds frozen order;
22. copied Input identity;
23. no prose/confidence/authority/authenticity fields;
24. Indeterminate completed uncertainty only;
25. assessment Bind on Reject-coded Input -> exception.

### Hard-rule short circuit
26. Reject codes + null assessment -> Reject;
27. Reject codes + non-null assessment -> exception/no disposition;
28. canonical Reject trace assessment null;
29. reference flow has no assessor dependency for deterministic Reject.

### Assessment-required path
30. zero Reject + null assessment -> exception/no disposition;
31. assessment for different hash/contract -> exception;
32. zero Reject + concern -> RequestAnotherTake;
33. zero Reject + zero concerns -> Accept.

### Non-authority
34. synthetic empty assessment may yield synthetic Accept evaluation but no authority-bearing eligibility object;
35. Evaluation exposes only Disposition + Trace;
36. no accepted-Take/history/State flag/token;
37. exact provisional-Take-vs-State-Interpreter ordering not frozen;
38. later authority must authenticate assessment provenance separately.

### Creative law
39. false claim not rejected solely for truth conflict;
40. no secret/canon keyword scanner;
41. no Performance rewrite;
42. no State/Context/Candidate mutation;
43. no Director/State Authority/Take/Commit/provider dependency;
44. no score/probability;
45. trace no Character/protected prose;
46. technical words in dialogue do not auto-create concern;
47. PotentialLockedAuthorityViolation is supplied semantic evidence, not false-speech detector.

### Technical failure / E0 isolation
48. semantic uncertainty concern -> RequestAnotherTake;
49. assessor technical failure -> no assessment/no disposition, not Indeterminate;
50. RequestAnotherTake no retry/spend;
51. assessor config fixed/attributable across E0-A-E unless explicit variable.

### Lifecycle
52. Reject non-history;
53. Request non-history;
54. Accept evaluation alone cannot advance Production authority;
55. Accept does not promote Director;
56. no opportunity/history mutation/trigger.

### Regression
57. Missing Raft StructuredContextHash unchanged;
58. Missing Raft RenderedContextHash unchanged;
59. ECJ-1 9112 bytes/frozen hash unchanged;
60. existing 211 Core tests green;
61. Missing Raft Harness PASS/0;
62. smoke Harness PASS/0.

## 33. E0-F compatibility

Patch 0008 prepares but does not complete E0-F.

Later E0-F proves secret failures, false-claim truth separation, locked canon/Constitution protection, malformed raw candidate rejection, candidate/assessor provider-failure isolation, partial/rejected non-history, deterministic retry/cost, and atomic accepted Performance + authoritative consequence commit.

No end-to-end guarantee is claimed here.

## 34. ARM64 / battery

Bind/hash/disposition are tiny deterministic CPU work. No background/AI/provider/network/GPU/NPU execution.

## 35. Explicit exclusions

No authority-bearing Integrity attestation/eligibility token; semantic-assessor implementation; bounded assessor packet; assessor provenance authentication/persistence; provider attempt/provenance; retry/cost/cancellation engine; Take semantics/identity/TakeId; State Interpreter implementation; State Authority; consequence proposal; ProductionState/StateHash; atomic commit; persistence/recovery; effective opportunity establishment; Scene loop; World Resolver/observation; E0-D round-robin; E0-E playwright; E0-F end-to-end harness; UI/WinUI; Windows AI/NPU; packaging/WACK/Store.

## 36. Recursive adversarial audit dimensions

Restart after every correction:

1. frozen Integrity law;
2. H1 sequence;
3. Integrity evaluation vs later acceptance authority;
4. statement/truth distinctions;
5. Performance vs consequence authority;
6. Character vs Performer;
7. Access/Context privacy;
8. least-privilege Input;
9. hard-rule-before-assessment precedence;
10. Integrity vs Parser;
11. Integrity vs Director;
12. Integrity vs State/Take;
13. no fabricated assessor attestation;
14. Take/Interpreter ordering non-preemption;
15. semantic evidence vs deterministic authority;
16. uncertainty vs assessor technical failure;
17. assessor disclosure/cost short circuit;
18. E0 assessor control isolation;
19. assessment privacy;
20. stale assessment/content binding;
21. content identity versioning;
22. content vs attempt/Take/causal identity;
23. technical failure vs fiction;
24. exception vs Reject;
25. no rewriting;
26. RequestAnotherTake vs retry/spend;
27. E0-F compatibility;
28. creator ontology guard;
29. API/non-forgeability/minimality;
30. deterministic bounded canonicalization;
31. fail closed/sanitization;
32. tests without bypass;
33. dependency direction;
34. ARM64/battery;
35. scope/hygiene;
36. E0-A/B/C/D/E/F/G isolation;
37. future State/Take/commit compatibility.

Approval only after complete restart produces zero material corrections or worthwhile improvements.

## 37. Material approval decisions

Approval would freeze only:

1. Patch 0008 as next E0-A Integrity evaluation boundary after Patch 0007;
2. IntegrityValidationEvaluation is deterministic calculation, not accepted-Take/State authority;
3. least-privilege IntegrityCandidateInput is sole rich Context+Candidate binding surface;
4. Input publicly carries only source Context ID, Candidate content identity, Reject codes;
5. hard Rejects short-circuit semantic assessment/disclosure;
6. Validator receives no Character/Performance prose;
7. versioned CandidateContentHash binds assessment to exact Candidate semantics;
8. content hash not attempt/Candidate/Take/commit identity;
9. Reject codes exactly subject mismatch then ContextPacket mismatch;
10. malformed source/unsupported Candidate contract is exception-domain;
11. five-kind ConcernAssessment is structurally bound non-authoritative evidence with no rationale/confidence/authenticity claim;
12. ConcernAssessment cannot be bound to Reject-coded Input;
13. completed semantic uncertainty differs from assessor technical failure;
14. valid assessment required only when zero deterministic Reject codes;
15. assessor failure on otherwise eligible Candidate yields no disposition;
16. no semantic assessor/provider implementation in Patch 0008;
17. future semantic review uses least-privilege packet and E0 assessor configuration/provenance stays fixed/attributable across E0-A-E;
18. exact disposition: RejectCodes+null assessment -> Reject; zero Reject+concerns -> RequestAnotherTake; zero Reject+empty concerns -> Accept; inconsistent/missing evidence -> exception;
19. Patch 0008 emits no authority-bearing eligibility/attestation object;
20. synthetic evidence may produce synthetic evaluation but cannot itself authorize later Production change;
21. later State/Take/orchestration must authenticate assessment provenance under its own contract before progression;
22. immediate provisional-Take-vs-State-Interpreter ordering remains later;
23. RequestAnotherTake does not authorize retry/spend;
24. false claims not truth-policed; no keyword scan or hidden rewrite;
25. candidate/assessor technical failures stay technical/orchestration concerns;
26. Director not Integrity input and Accept cannot promote precommit Director work;
27. Trace nests Input + optional Assessment and contains no Performance/private prose or authenticity claim;
28. no State Interpreter/Authority, Take, commit, persistence, opportunity application, Scene loop, provider execution, UI, Windows AI/NPU, or Store machinery enters Patch 0008.

Implementation remains blocked until recursive audit completes and user explicitly approves final proposal.
