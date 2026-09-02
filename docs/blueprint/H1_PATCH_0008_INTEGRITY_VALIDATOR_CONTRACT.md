# H1 Patch 0008 — E0 Integrity Validator Contract

Status: blueprint proposal 0.5 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0007
Branch: `h1-patch-0008-integrity-validator-blueprint`

## 1. Purpose

Define the next frozen E0-A boundary after Performer Candidate and Director proposal calculation:

```text
ContextPacket + CandidatePerformance
    -> deterministic least-privilege IntegrityCandidateInput.Bind
        -> Candidate content identity + structural Reject codes
            -> bounded IntegrityConcernAssessment
                -> deterministic Integrity Validator
                    -> Accept | Reject | RequestAnotherTake
                        -> Accept alone yields IntegrityEligibleCandidate
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

This is the only Patch 0008 operation that receives full ContextPacket + CandidatePerformance together.

Bind validates trusted Context/Candidate prerequisites, computes Candidate content identity, computes structural Candidate Reject codes, and returns a least-privilege immutable input.

The deterministic Validator itself does **not** receive ContextPacket or raw CandidatePerformance.

## 5. IntegrityCandidateInput public shape

Freeze public read-only fields:

```text
IntegrityCandidateInput
- CandidateContentIdentityContract
- CandidateContentHash
- SourceContextPacketId
- DeterministicRejectCodes
```

No public constructor.

The object exposes no Candidate VisibleText, Context prose, RenderedContext, roster display names, private Character records, denied records, provider data, or semantic assessment rationale.

Internally, Core may retain the exact bound Candidate reference/value solely so an Accept evaluation can construct IntegrityEligibleCandidate without re-supplying/rebinding rich input. That internal retention is not a public disclosure surface.

## 6. Bind trusted prerequisites

Bind uses Integrity exception-domain for malformed trusted states that normal public construction should prevent, including:

- null Context/Candidate;
- uninitialized ContextPacketId/subject/opportunity;
- Context subject != opportunity for E0;
- unsupported Candidate semantic contract;
- malformed Candidate fields that violate the already-frozen public Candidate object invariants.

Bind does not duplicate Patch 0006 text/control semantic validation logic. Defensive checks are limited to what is necessary to safely read/hash a Candidate object.

## 7. Deterministic Candidate Reject codes

For independently valid objects, Bind computes exactly:

```text
SubjectContextMismatch
ContextPacketIdentityMismatch
```

SubjectContextMismatch: Candidate subject != source Context subject.

ContextPacketIdentityMismatch: Candidate ContextPacketId != source ContextPacketId.

Both may be present if both dimensions mismatch.

These make the Candidate unusable for this source Character opportunity but do not mutate/discard anything.

## 8. Candidate content identity contract

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

- identityContract exact v1 value above;
- UTF-8 no BOM;
- reuse existing canonical JSON scalar/string + UTF-8 emission primitive;
- visible text exact;
- addressed IDs preserve Patch 0006 canonical ordinal order;
- nomination string or JSON null;
- lowercase 64-hex SHA-256;
- identity contract participates in preimage.

## 9. CandidateContentHash semantics

CandidateContentHash is content identity only, never provider-attempt identity, CandidateId, TakeId, causal-commit identity, acceptance authority, or provider/model proof.

Distinct attempts with identical Candidate semantics intentionally share it.

`ContextPacketId` inside the preimage is structured semantic identity, not proof of exact rendered/provider disclosure. Exact rendering/provider-attempt facts remain separate provenance.

## 10. Integrity concern assessment evidence

Freeze:

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

Concerns are immutable/distinct/frozen-order values from:

```text
PotentialInaccessibleInformationUse
PotentialProtectedInformationExposure
PotentialLockedAuthorityViolation
PotentialTechnicalArtifactLeak
IndeterminateSemanticIntegrity
```

These are engine integrity-policy categories, not creator-facing dramatic ontology.

Assessment has no prose rationale, chain-of-thought, Character/private/forbidden text, confidence score, accept/reject authority, State mutation, or retry/spend authority.

PotentialLockedAuthorityViolation means concern that Performance purports to enact/establish a locked/world-law conflict, not merely that a Character said something false.

IndeterminateSemanticIntegrity means completed semantic review that cannot responsibly clear content; never assessor technical failure.

## 11. Assessment binding

Freeze:

```text
IntegrityConcernAssessment.Bind(
    IntegrityCandidateInput input,
    ImmutableArray<IntegrityConcernKind> concerns)
    -> IntegrityConcernAssessment
```

Bind copies the input identity contract/hash, rejects default concern arrays, undefined values, duplicates, and count > 5, then stores frozen enum order.

Zero-length initialized array is valid and means the configured assessment path completed with no concerns.

Assessment binding proves structural association only; it does not authenticate assessor provenance.

Synthetic assessments are valid for deterministic tests/E0-F injection.

## 12. Assessor technical failure is not semantic concern

Later integration rule:

```text
completed review but cannot clear content
    -> IndeterminateSemanticIntegrity

assessor transport/provider/refusal/timeout/cancellation failure
    -> no valid IntegrityConcernAssessment
    -> technical/orchestration failure path
```

Technical assessor failure must not automatically become RequestAnotherTake or another paid Character generation.

Later deterministic fallback/cost policy owns recovery.

## 13. Future semantic-assessor privacy

Any later assessor path must:

- avoid complete Production disclosure by default;
- use a separate bounded integrity-assessment packet containing Candidate content + minimum authoritative constraints/references;
- keep disclosure separate from Character Context;
- preserve assessor/provider/model provenance;
- expose no hidden reasoning;
- have no State/commit/retry/spend authority;
- never waive deterministic Reject conditions.

Exact assessor mechanism is later scope.

## 14. E0 experimental isolation

If E0 uses assisted Integrity review, its mechanism/configuration must remain fixed and attributable across E0-A through E0-E comparison batches unless explicitly varied as its own experiment.

Do not tune assessor prompts/rules within a frozen batch.

Assessment results, dispositions, technical assessor failures, and resulting attempt/retry behavior belong in experimental provenance.

## 15. Deterministic Validator API

Freeze:

```text
DeterministicIntegrityValidator.Validate(
    IntegrityCandidateInput input,
    IntegrityConcernAssessment concernAssessment)
    -> IntegrityValidationEvaluation
```

Validator receives no ContextPacket, raw CandidatePerformance, Production state, Director evaluation, provider data, free-form semantic rationale, filesystem/network/clock/random input.

## 16. Assessment/input binding validation

Before disposition, Validate must require:

- non-null/initialized input;
- supported input identity contract;
- non-null/structurally valid concern assessment;
- supported assessment contract;
- assessment CandidateContentIdentityContract == input contract;
- assessment CandidateContentHash == input hash;
- assessment concerns remain valid canonical set.

Any failure here is Integrity exception-domain/no disposition, not Candidate Reject.

## 17. Integrity disposition

Freeze:

```text
IntegrityDisposition
- Accept
- Reject
- RequestAnotherTake
```

Exact precedence:

```text
input DeterministicRejectCodes non-empty -> Reject
else assessment Concerns non-empty -> RequestAnotherTake
else -> Accept
```

Semantic concerns cannot soften deterministic Reject.

Accept means Integrity eligibility only—not accepted Take, history, authoritative State, committed consequence, effective opportunity, retry/spend authority.

## 18. IntegrityEligibleCandidate

Only Accept creates:

```text
IntegrityEligibleCandidate
- Candidate
- CandidateContentIdentityContract
- CandidateContentHash
```

No public constructor. Read-only.

It preserves the exact Candidate bound by IntegrityCandidateInput and its content identity.

Meaning:

> Candidate passed Patch 0008 Integrity evaluation under supplied concern evidence and is eligible for later E0 State/Take processing.

It is not accepted Take/canon/history/State/causal identity.

Non-public construction protects sequence/object invariants, not assessor provenance authenticity.

## 19. Evaluation shape

Freeze:

```text
IntegrityValidationEvaluation
- Disposition
- EligibleCandidate
- Trace
```

Invariant:

```text
Disposition == Accept <=> EligibleCandidate != null
```

Reject/RequestAnotherTake expose no eligibility handoff.

Patch 0008 does not freeze whether provisional Take selection or State Interpreter is the immediate consumer. Later E0 processing must not proceed from a raw Candidate lacking Integrity eligibility.

## 20. RequestAnotherTake is not retry authority

RequestAnotherTake does not call provider, authorize spend, increment retry budget, choose understudy, change Current Opportunity, or construct another Candidate.

Later deterministic orchestration/cost policy decides recovery.

Candidate remains unaccepted diagnostics/provenance only.

## 21. Reject is not fictional action

Rejected Candidate does not enter Production history, recent-performance Context, Character state, observation history, or effective opportunity history.

Diagnostics only when later provenance exists.

## 22. No hidden rewriting

Validator returns no replacement/corrected text, paraphrase, suggested line, or rewritten control.

Candidate remains exact Performer output.

## 23. Technical provider failure boundary

Candidate-provider refusal/timeout/transport error/cancellation/partial stream is not CandidatePerformance and must not be converted to fiction.

Validator does not infer transport provenance from technical words in dialogue.

PotentialTechnicalArtifactLeak exists only as typed concern evidence from completed assessment.

E0-F provider-failure hard gate remains later integration scope.

## 24. Director interaction

Director is not Integrity input.

Reject/RequestAnotherTake cannot promote associated Director work.

Accept also cannot promote precommit Director evaluation; Patch 0007 postcommit re-Bind/recompute remains unchanged.

## 25. State / Take boundary

Later State Interpreter and Take contracts must preserve:

```text
Integrity Accept != accepted Take
IntegrityEligibleCandidate != accepted Take
IntegrityEligibleCandidate != authoritative consequence
IntegrityEligibleCandidate != causal commit
```

State Interpreter remains proposal-only and State Authority deterministic.

Exact immediate ordering of provisional Take selection versus State interpretation remains later authority.

## 26. Trace

Freeze:

```text
IntegrityValidationTrace
- ValidationContract
- Input
- ConcernAssessment
```

```text
ValidationContract = ensemble.e0.integrity.validation.v1
```

Input owns source Context identity + Candidate content identity + Reject codes; concern assessment owns exact concern evidence. Trace does not duplicate those fields.

Trace contains no Candidate VisibleText, Context/protected/denied text, provider output, rationale, confidence, or chain-of-thought.

Trace is local provenance/diagnostics, never Character-facing Context.

## 27. Determinism

Identical bound input + concern assessment + contracts produce identical disposition, eligibility-handoff presence/identity, and trace.

Bind itself is deterministic from identical Context/Candidate semantics.

No clock/filesystem/network/random/current culture/provider/model/GPU/NPU/global mutable state.

## 28. Fail closed

Use Integrity-specific exception domain.

Errors expose structural field names/codes only; never Candidate VisibleText, Context/private/protected text, semantic rationale, or raw provider output.

Failure never becomes fallback disposition.

## 29. Public-surface intent

Preferred:

```text
IntegrityCandidateInput.Bind(ContextPacket, CandidatePerformance)
    -> IntegrityCandidateInput

IntegrityConcernAssessment.Bind(
    IntegrityCandidateInput,
    ImmutableArray<IntegrityConcernKind>)
    -> IntegrityConcernAssessment

DeterministicIntegrityValidator.Validate(
    IntegrityCandidateInput,
    IntegrityConcernAssessment)
    -> IntegrityValidationEvaluation
```

Public models read-only; no public constructors for Input/Assessment/Evaluation/EligibleCandidate except approved bind/evaluate construction paths.

No commit/retry/State mutation API.

## 30. Required tests / review gates

### Rich-object/least-privilege boundary

1. valid Missing Raft Voss Context+Candidate -> Input;
2. Input public fields exactly identity/hash/source Context ID/Reject codes;
3. Input exposes no Context/Candidate prose or rendering/private records;
4. Validator public API accepts Input+Assessment only;
5. Validator cannot receive ContextPacket/CandidatePerformance directly;
6. Input constructor non-public.

### Candidate identity

7. identical Candidate -> identical hash;
8. visible text/address/nomination/ContextPacketId change -> hash changes;
9. Candidate contract + identity contract in preimage;
10. addressed order stable;
11. lowercase 64-hex;
12. content identity not attempt/Take/causal identity.

### Bind exception vs Reject

13. malformed source Context -> exception;
14. unsupported Candidate contract -> exception;
15. valid wrong-subject Candidate -> Input with SubjectContextMismatch;
16. valid different-Context Candidate -> ContextPacketIdentityMismatch;
17. both mismatch -> both canonical Reject codes;
18. no duplicate Patch 0006 parser validation.

### Assessment

19. zero initialized concerns valid;
20. default/undefined/duplicate/over-count invalid;
21. five kinds canonical frozen order;
22. assessment copies input content identity;
23. no prose/confidence/authority fields;
24. Indeterminate means completed uncertainty, not technical failure.

### Validate exception vs disposition

25. assessment for different input hash -> exception;
26. unsupported assessment/identity contract -> exception;
27. Reject code + no concerns -> Reject;
28. Reject code + concerns -> Reject;
29. no Reject + concern -> RequestAnotherTake;
30. no Reject + no concern -> Accept;
31. each concern independently -> RequestAnotherTake.

### Eligibility

32. Accept -> EligibleCandidate;
33. Reject/Request -> no EligibleCandidate;
34. EligibleCandidate constructor non-public;
35. EligibleCandidate exact Candidate + identity;
36. eligibility explicitly not Take/history/State authority;
37. no frozen immediate State-Interpreter-vs-provisional-Take ordering.

### Creative law

38. false claim not rejected solely for objective-truth conflict;
39. no secret/canon keyword scanner;
40. no Performance rewrite;
41. no State/Context/Candidate mutation;
42. no Director dependency;
43. no State Authority/Take/Commit dependency;
44. no provider/model/network dependency;
45. no confidence/score/probability;
46. trace public surface has no Candidate/Context/protected prose;
47. technical words in dialogue do not deterministically create TechnicalArtifactLeak;
48. PotentialLockedAuthorityViolation is assessment evidence, not deterministic false-speech detector.

### Technical failure / E0 isolation

49. semantic uncertainty -> RequestAnotherTake;
50. assessor technical failure cannot be encoded as Indeterminate or auto-trigger Character retry;
51. RequestAnotherTake has no retry/spend behavior;
52. assessor config/provenance fixed/attributable across E0-A-E unless explicitly varied.

### Lifecycle

53. Reject remains non-history;
54. RequestAnotherTake remains non-history;
55. only EligibleCandidate may advance into later E0 State/Take processing;
56. precommit Director result not promoted by Accept;
57. no opportunity mutation/history append/Performer trigger.

### Regression

58. Missing Raft StructuredContextHash unchanged;
59. Missing Raft RenderedContextHash unchanged;
60. ECJ-1 9112 bytes/frozen hash unchanged;
61. existing 211 Core tests green;
62. Missing Raft Harness PASS/0;
63. smoke Harness PASS/0.

## 31. E0-F compatibility

Patch 0008 prepares but does not complete E0-F.

Later E0-F must prove inaccessible-secret failures, false-claim truth separation, locked canon/Constitution protection, malformed raw candidate rejection before CandidatePerformance, candidate/assessor provider-failure isolation, partial/rejected non-history, deterministic retry/cost, and atomic accepted Performance + authoritative consequence commit.

Patch 0008 claims none of those end-to-end guarantees alone.

## 32. ARM64 / battery

Binding/hashing/disposition are tiny deterministic CPU work with no background activity.

No AI/provider/network/GPU/NPU execution.

## 33. Explicit exclusions

No semantic-assessor implementation; bounded assessor packet implementation; assessor provenance authentication/persistence; provider attempt/provenance; retry/cost/cancellation engine; Take semantics/identity/TakeId; State Interpreter implementation; State Authority; consequence proposal; ProductionState/StateHash; atomic causal commit; persistence/recovery; effective opportunity establishment; Scene loop; World Resolver/observation; E0-D round-robin execution; E0-E playwright; E0-F end-to-end harness; UI/WinUI; Windows AI/NPU; packaging/WACK/Store.

## 34. Recursive adversarial audit dimensions

Restart after every material correction:

1. frozen Blueprint 0.1 Integrity law;
2. H1 sequence correctness;
3. statement/truth/claim/belief law;
4. Performance vs consequence authority;
5. Character vs Performer;
6. Access/Context privacy;
7. Integrity least-privilege input;
8. Integrity vs Parser;
9. Integrity vs Director;
10. Integrity vs State/Take;
11. eligibility vs accepted Take;
12. provisional Take/Interpreter ordering non-preemption;
13. semantic evidence vs deterministic authority;
14. uncertainty vs assessor technical failure;
15. E0 assessor control isolation;
16. assessment privacy;
17. stale assessment/content binding;
18. content identity versioning;
19. content vs attempt/Take/causal identity;
20. technical failure vs fiction;
21. exception vs Reject;
22. no rewriting;
23. RequestAnotherTake vs retry/spend;
24. E0-F compatibility;
25. creator-ontology guard;
26. public API/non-forgeability/minimality;
27. deterministic bounded canonicalization;
28. fail closed/sanitization;
29. tests without bypass;
30. dependency direction;
31. ARM64/battery;
32. scope/hygiene;
33. E0-A/B/C/D/E/F/G isolation;
34. future State/Take/commit compatibility.

Approval only after a complete restart produces zero material corrections or worthwhile improvements.

## 35. Material approval decisions

Approval would freeze only:

1. Patch 0008 as next E0-A boundary after Patch 0007;
2. one least-privilege IntegrityCandidateInput Bind boundary receives ContextPacket+Candidate;
3. deterministic Validator itself receives no Character/Performance prose;
4. versioned CandidateContentHash binds semantic concern evidence to exact Candidate content;
5. content hash is not attempt/Candidate/Take/commit identity;
6. Input carries only source ContextPacketId, content identity, and deterministic Reject codes publicly;
7. Reject codes limited to subject mismatch and ContextPacket mismatch;
8. malformed source/Candidate contracts remain exception-domain;
9. typed five-kind ConcernAssessment is non-authoritative evidence and no assessor provenance authentication;
10. completed semantic uncertainty differs from assessor technical failure;
11. no semantic assessor/provider implementation in Patch 0008;
12. future semantic review uses least-privilege assessment packet, not full Production by default;
13. E0 assessor configuration/provenance fixed/attributable across E0-A-E unless explicitly varied;
14. assessment/input mismatch/unsupported evidence is exception-domain;
15. precedence: Reject code -> Reject; else concern -> RequestAnotherTake; else Accept;
16. only Accept creates non-public IntegrityEligibleCandidate;
17. eligibility means later State/Take processing only, not accepted Take/history/State;
18. immediate provisional-Take-vs-State-Interpreter ordering remains later authority;
19. later E0 processing cannot proceed from raw Candidate lacking Integrity eligibility;
20. RequestAnotherTake does not authorize retry/spend;
21. false claims are not objective-truth-policed; no secret/canon keyword scan;
22. no hidden Performance rewriting;
23. candidate/assessor technical failures remain orchestration concerns;
24. Director is not Integrity input and Accept cannot promote precommit Director work;
25. Trace nests the least-privilege Input + Assessment and contains no Performance/private prose;
26. no State Interpreter/Authority, Take, commit, persistence, opportunity application, Scene loop, provider execution, UI, Windows AI/NPU, or Store machinery enters Patch 0008.

Implementation remains blocked until recursive audit completes and user explicitly approves the final proposal.
