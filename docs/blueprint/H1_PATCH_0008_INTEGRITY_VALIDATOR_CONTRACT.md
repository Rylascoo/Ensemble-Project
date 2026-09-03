# H1 Patch 0008 — E0 Integrity Validator Contract

Status: blueprint proposal 1.0 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0007
Branch: `h1-patch-0008-integrity-validator-blueprint`

## 1. Purpose

Define the next frozen E0-A calculation boundary after Performer Candidate and Director proposal calculation:

```text
ContextPacket + CandidatePerformance
    -> IntegrityCandidateInput.Bind
        -> deterministic Reject codes?
            yes -> Reject without semantic concern evidence
            no  -> bounded IntegrityConcernEvidence
                    -> deterministic Integrity Validator
                        concern(s) -> RequestAnotherTake
                        none       -> Accept
```

Patch 0008 defines deterministic candidate-integrity **evaluation** only.

It does not create an accepted Take, authority-bearing eligibility/attestation token, State interpretation, Production mutation, causal commit, provider retry/spend, Current Opportunity, semantic-assessor transport, or assessor-provenance authentication.

## 2. Recovered frozen authority

Blueprint 0.1 orders E0-A preparation as Access Control, Context Composer, Performer candidate output, Director opportunity rules, **Integrity Validator acceptance/rejection rules**, State Interpreter candidate-mutation schema, deterministic State Authority, Take semantics, and atomic causal-commit record.

It freezes:

```text
Integrity Validator -> State Interpreter -> deterministic State Authority
```

Integrity Validator checks candidate Performance before acceptance; deterministic hard invariants have precedence; model/human-assisted checks may flag semantic concerns but cannot waive hard rules; valid outcomes are accept, reject, or request another take; hidden rewriting is forbidden.

The frozen source does not define a broader semantic taxonomy for Reject versus RequestAnotherTake. Patch 0008 therefore adopts the smallest E0 distinction required for the per-Character Candidate path:

- deterministic source/Candidate hard mismatch -> Reject;
- otherwise supplied semantic concern evidence -> RequestAnotherTake;
- otherwise -> Accept.

Reject means the evaluated Candidate/source pairing is invalid for this opportunity. It does not assign fictional blame to the Character.

## 3. Integrity evaluation is not acceptance authority

```text
IntegrityDisposition.Accept != accepted Take
IntegrityValidationEvaluation != Take authority
IntegrityValidationEvaluation != State authority
```

Accept means only:

> Under this Patch 0008 Input and supplied structurally bound concern evidence, no deterministic Reject condition or reported semantic concern prevents progression.

Later effective E0 processing must additionally establish that concern evidence came from the configured review path for the relevant provider attempt/batch and preserve that provenance.

Synthetic concern evidence may produce synthetic evaluation results for tests without creating Production authority.

No Patch 0008 object claims assessor authenticity.

## 4. Integrity is not truth policing

A Character may lie, be mistaken, speculate, repeat rumor, contradict another Character, state a false belief, or make a claim unsupported by Production truth.

Those are not automatically integrity failures.

A statement becomes an authority violation only if later authority silently promotes it to truth, or when Performance credibly uses inaccessible information / purports to enact or establish something incompatible with locked authority rather than merely claiming, believing, intending, attempting, or failing.

Patch 0008 has no deterministic secret/canon keyword scanner, objective-truth contradiction rejection rule, or automatic rejection of false claims/beliefs.

Patch 0006 leaves non-empty Performance grammar open, so semantic distinctions belong to bounded concern review rather than deterministic prose parsing.

## 5. Contracts

Freeze separate version domains:

```text
IntegrityCandidateInputContract = ensemble.e0.integrity.input.v1
CandidateContentIdentityContract = ensemble.e0.integrity.candidate-content.v1
IntegrityConcernEvidenceContract = ensemble.e0.integrity.concerns.v1
IntegrityValidationContract = ensemble.e0.integrity.validation.v1
```

These contracts version different meanings and must not be collapsed:

- InputContract versions the least-privilege Input shape + deterministic Reject-code semantics;
- CandidateContentIdentityContract versions canonical Candidate content identity;
- ConcernEvidenceContract versions the typed concern-evidence vocabulary/shape;
- ValidationContract versions disposition/evaluation semantics.

## 6. Rich-object binding boundary

```text
IntegrityCandidateInput.Bind(
    ContextPacket sourceContext,
    CandidatePerformance candidate)
    -> IntegrityCandidateInput
```

This is the only Patch 0008 operation receiving full ContextPacket + CandidatePerformance together.

Bind validates trusted prerequisites, computes Candidate content identity and deterministic Reject codes, and returns a least-privilege immutable Input.

The deterministic Validator itself receives no ContextPacket or raw CandidatePerformance.

## 7. IntegrityCandidateInput public shape

```text
IntegrityCandidateInput
- InputContract
- CandidateContentIdentityContract
- CandidateContentHash
- SourceContextPacketId
- DeterministicRejectCodes
```

`InputContract` is exactly `ensemble.e0.integrity.input.v1`.

No public constructor.

No Candidate VisibleText, Context prose, rendering, roster display names, private/denied records, provider data, or semantic rationale is exposed.

Patch 0008 retains no hidden Candidate reference because it creates no downstream authority-bearing handoff object.

## 8. Bind trusted prerequisites

Integrity exception-domain for null/malformed trusted objects, uninitialized Context identity/subject/opportunity, Context subject != opportunity, unsupported Candidate semantic contract, or malformed Candidate state impossible through normal public Candidate construction.

Bind does not reimplement Patch 0006 text/control validation; defensive reading checks only protect safe hashing/access.

## 9. Deterministic Reject codes

Exactly:

```text
SubjectContextMismatch
ContextPacketIdentityMismatch
```

Frozen order:

1. SubjectContextMismatch
2. ContextPacketIdentityMismatch

Each appears at most once; both may coexist.

SubjectContextMismatch: Candidate subject != source Context subject.

ContextPacketIdentityMismatch: Candidate ContextPacketId != source ContextPacketId.

These codes reject the Candidate/source pairing for this Integrity evaluation. They do not convert the mismatch into fictional action or Character behavior.

For normal validated upstream objects, a different-subject Candidate will also carry a different ContextPacketId, so both codes are expected together. A subject-only mismatch is a defensive impossible-state case, not a reason to introduce public test construction bypasses.

## 10. Candidate content identity

```text
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

Rules: identityContract exactly `ensemble.e0.integrity.candidate-content.v1` and participates in preimage; UTF-8 without BOM; reuse existing canonical JSON scalar/string + UTF-8 primitive; visible text exact; addressed IDs preserve Patch 0006 canonical ordinal order; nomination string/null; lowercase 64-hex SHA-256.

## 11. CandidateContentHash semantics

Content identity only—not provider-attempt identity, CandidateId, TakeId, causal-commit identity, acceptance authority, or provider/model proof.

Distinct attempts with identical Candidate semantics intentionally share it.

ContextPacketId in the preimage is structured semantic identity, not proof of exact rendered/provider disclosure. Exact disclosure/provider-attempt facts remain separate provenance.

## 12. Integrity concern evidence

```text
IntegrityConcernEvidence
- EvidenceContract
- CandidateContentIdentityContract
- CandidateContentHash
- Concerns
```

`EvidenceContract` is exactly `ensemble.e0.integrity.concerns.v1`.

Frozen concern kinds/order:

1. PotentialInaccessibleInformationUse
2. PotentialProtectedInformationExposure
3. PotentialLockedAuthorityViolation
4. PotentialTechnicalArtifactLeak
5. IndeterminateSemanticIntegrity

`IntegrityConcernEvidence` is structurally bound evidence only. Its name intentionally does not imply that Patch 0008 authenticated who or what performed semantic review.

It has no assessor identity/authenticity flag, prose rationale, chain-of-thought, Character/private/forbidden text, confidence score, disposition authority, State mutation, or retry/spend authority.

Concern semantics for the E0 reference path:

- `PotentialInaccessibleInformationUse`: evidence suggests Performance relies on information unavailable to the Character as knowledge/agency. Mere coincidence with hidden truth, guess, question, suspicion, lie, or unsupported claim is insufficient by itself.
- `PotentialProtectedInformationExposure`: evidence suggests protected creator/system-only material surfaced through Performance in a way requiring review. Generic wording overlap is not a deterministic leak test.
- `PotentialLockedAuthorityViolation`: evidence suggests Performance purports to enact/establish a locked canon/world-law conflict. Mere false speech, belief, intent, or failed attempt is insufficient by itself.
- `PotentialTechnicalArtifactLeak`: evidence suggests provider/system/transport artifacts leaked into fictional Performance. Legitimate technical wording is insufficient by itself.
- `IndeterminateSemanticIntegrity`: a completed semantic review cannot responsibly clear Candidate under its configured review contract.

These are concern categories, not hidden truth-promotion rules.

## 13. Concern evidence binding

```text
IntegrityConcernEvidence.Bind(
    IntegrityCandidateInput input,
    ImmutableArray<IntegrityConcernKind> concerns)
    -> IntegrityConcernEvidence
```

Bind requires supported/initialized InputContract, zero deterministic Reject codes, initialized concern array, only defined concern kinds, no duplicates, and count <= 5.

It copies Candidate content identity contract/hash and stores concerns in frozen contract order.

A zero-length initialized array is valid and means only that this evidence object reports no concerns.

Binding to Reject-coded Input is an Integrity exception and preserves hard-rule short circuit.

Binding proves structural association only; it does not authenticate assessor provenance or assert semantic review really occurred.

Synthetic evidence remains valid for deterministic tests/E0-F injection on otherwise structurally eligible Inputs.

## 14. Deterministic hard-rule short circuit

If `IntegrityCandidateInput.DeterministicRejectCodes` is non-empty:

- semantic concern evidence is not required;
- canonical evaluation is Reject immediately;
- canonical trace has no concern evidence;
- concern evidence cannot be bound through public path.

Reference orchestration should not invoke semantic review because it cannot change result and would add unnecessary disclosure/cost.

## 15. Semantic uncertainty versus assessor technical failure

For Input with zero deterministic Reject codes:

```text
completed semantic review but cannot clear content
    -> authenticated orchestration may supply IndeterminateSemanticIntegrity evidence

assessor transport/provider/refusal/timeout/cancellation failure
    -> no completed concern evidence from configured review path
    -> technical/orchestration failure
    -> no effective Integrity disposition
```

Technical assessor failure must not automatically become RequestAnotherTake or another paid Character-generation call.

Later deterministic fallback/cost policy owns recovery.

## 16. Future semantic-assessor privacy

Any later assessor path must avoid complete Production disclosure by default; use a separate bounded assessment packet containing Candidate content plus minimum authoritative constraints/references; keep disclosure separate from Character Context; preserve assessor/provider/model provenance; expose no hidden reasoning; have no State/commit/retry/spend authority; and never waive deterministic Rejects.

Exact assessor mechanism remains later scope.

## 17. E0 experimental isolation

For E0 variants that use the per-Character Candidate Integrity path, configured concern-review mechanism/settings must remain fixed and attributable within a comparison batch unless the experiment explicitly varies Integrity review.

This applies to E0-A reference and corresponding per-Character E0-B/C/D/F/G runs where Patch 0008 is actually used.

Do not tune concern-review prompts/rules inside a frozen batch.

E0-E single-playwright control may not expose the same per-Character CandidatePerformance boundary. Patch 0008 does not require E0-E to use this Candidate-specific API. E0-E must still satisfy Blueprint 0.1 hard integrity gates under a separately defined control-compatible protocol whose behavior/provenance remains auditable.

Concern evidence, dispositions, assessor technical failures, and resulting attempt/retry behavior belong in experimental provenance wherever Patch 0008 is used.

## 18. Deterministic Validator API

```text
DeterministicIntegrityValidator.Validate(
    IntegrityCandidateInput input,
    IntegrityConcernEvidence? concernEvidence)
    -> IntegrityValidationEvaluation
```

Nullable evidence exists only because deterministic Reject requires none.

Exact rules:

1. validate InputContract and Input structure;
2. if Reject codes non-empty:
   - concernEvidence must be null;
   - non-null evidence -> Integrity exception/no disposition;
   - null -> Reject;
3. if Reject codes empty:
   - matching structurally valid concernEvidence is required;
   - null -> Integrity exception/no disposition;
   - unsupported EvidenceContract or CandidateContentIdentityContract -> Integrity exception/no disposition;
   - identity-contract/hash mismatch -> Integrity exception/no disposition;
   - malformed concern set -> Integrity exception/no disposition;
   - concerns non-empty -> RequestAnotherTake;
   - concerns empty -> Accept.

Validator receives no rich Context/Candidate prose.

## 19. Why evidence on deterministic Reject is an error

Public evidence binding cannot create concern evidence for Reject-coded Input.

Therefore non-null evidence paired with such Input indicates stale/mismatched/internal misuse rather than useful information.

Validator fails closed instead of ignoring it. Canonical provenance cannot imply semantic review influenced deterministic Reject.

## 20. Integrity disposition

```text
IntegrityDisposition
- Accept
- Reject
- RequestAnotherTake
```

Disposition is deterministic evaluation only.

`Reject` is reserved in Patch 0008 per-Character reference contract for deterministic Candidate/source hard mismatch.

`RequestAnotherTake` is the reference response to one or more structurally supplied semantic concern kinds on an otherwise correctly bound Candidate.

This is narrow provisional E0 semantics and does not freeze final post-E0 creator-facing rejection/retry UX.

## 21. Evaluation shape

```text
IntegrityValidationEvaluation
- Disposition
- Trace
```

No Candidate, Take, eligibility, or attestation token is emitted.

Later E0 State/Take/orchestration authority must bind original Candidate/provider attempt to this evaluation and authenticate concern-review provenance under its own approved contract before progressing.

Patch 0008 does not freeze whether provisional Take selection or State Interpreter is immediate downstream consumer.

## 22. RequestAnotherTake is not retry authority

RequestAnotherTake does not call provider, authorize spend, increment retry budget, choose understudy, modify Current Opportunity, or create another Candidate.

Later deterministic orchestration/cost policy decides recovery.

Candidate remains unaccepted diagnostic/provenance material only.

## 23. Reject is not fictional action

Rejected Candidate does not enter Production history, recent-performance Context, Character state, observation history, or effective opportunity history.

It remains diagnostics only when later provenance exists.

## 24. No hidden rewriting

No replacement/corrected text, paraphrase, suggested line, or rewritten control.

Candidate remains exact Performer output.

## 25. Technical provider failure boundary

Candidate-provider refusal/timeout/transport error/cancellation/partial stream is not CandidatePerformance and must not become fiction.

No technical-word scanning in dialogue.

PotentialTechnicalArtifactLeak exists only as typed concern evidence associated with a completed review path in effective orchestration.

E0-F provider-failure gate remains later integration scope.

## 26. Director interaction

Director is not Integrity input.

Reject/Request cannot promote associated Director work.

Accept cannot promote precommit Director evaluation; Patch 0007 postcommit re-Bind/recompute remains unchanged.

## 27. State / Take boundary

Later contracts preserve:

```text
Integrity Accept != accepted Take
IntegrityValidationEvaluation != accepted Take
IntegrityValidationEvaluation != authoritative consequence
IntegrityValidationEvaluation != causal commit
```

State Interpreter remains proposal-only and State Authority deterministic.

Exact provisional-Take-vs-State-Interpreter immediate ordering remains later authority.

## 28. Trace

```text
IntegrityValidationTrace
- ValidationContract
- Input
- ConcernEvidence
```

`ValidationContract` is exactly `ensemble.e0.integrity.validation.v1`.

ConcernEvidence is null only for canonical deterministic Reject.

Input owns InputContract, source Context identity, Candidate content identity, Reject codes. ConcernEvidence owns typed semantic concern evidence. No duplicated identity fields.

Trace contains no Candidate/Context/protected prose, provider output, rationale, confidence, chain-of-thought, assessor identity, or authenticity claim.

## 29. Evaluation invariants

- Accept => supported InputContract, zero Reject codes, non-null matching evidence, zero concerns;
- RequestAnotherTake => supported InputContract, zero Reject codes, non-null matching evidence, concerns non-empty;
- Reject => supported InputContract, Reject codes non-empty, null concern evidence;
- exception/no disposition => no Evaluation object.

## 30. Determinism

Identical bound Input + matching concern evidence where required + contract versions produce identical disposition/trace.

Bind is deterministic from identical Context/Candidate semantics.

No clock/filesystem/network/random/culture/provider/model/GPU/NPU/global mutable state.

## 31. Fail closed

Integrity-specific exception domain with sanitized structural messages only.

Failure never becomes fallback disposition.

## 32. Testability / impossible upstream states

Do not add public test-only constructors, setters, aliases, or mutation hooks to fabricate impossible ContextPacket/Candidate states.

Executable tests should prefer independently valid objects constructed through canonical upstream paths.

For Patch 0008:

- same-subject, different-context Candidate is a real executable path for ContextPacketIdentityMismatch only;
- different-subject Candidate from another valid Context is a real executable path that naturally produces both SubjectContextMismatch and ContextPacketIdentityMismatch;
- subject-only mismatch with matching ContextPacketId is an impossible validated upstream state under the current Context identity contract and is covered by defensive/static/reflection boundary review only if needed;
- unsupported/malformed Candidate contract states likewise do not justify production bypass APIs.

## 33. Public-surface intent

```text
IntegrityCandidateInput.Bind(ContextPacket, CandidatePerformance)
    -> IntegrityCandidateInput

IntegrityConcernEvidence.Bind(
    IntegrityCandidateInput,
    ImmutableArray<IntegrityConcernKind>)
    -> IntegrityConcernEvidence

DeterministicIntegrityValidator.Validate(
    IntegrityCandidateInput,
    IntegrityConcernEvidence?)
    -> IntegrityValidationEvaluation
```

Public models read-only with non-public constructors except approved Bind/Validate paths.

No authenticated assessment/eligibility token, commit, retry, or State-mutation API.

## 34. Required tests / review gates

### Least privilege / Input
1. valid Missing Raft Voss Context+Candidate -> Input;
2. Input exact public fields include InputContract + content identity + source Context ID + Reject codes only;
3. no Context/Candidate prose/render/private record exposure;
4. Validator API accepts Input + nullable Evidence only;
5. no ContextPacket/CandidatePerformance Validator overload;
6. Input constructor non-public;
7. InputContract exact v1 value and Validator rejects unsupported contract defensively.

### Candidate identity
8. identical Candidate -> identical hash;
9. visible text/address/nomination/ContextPacketId change -> hash changes;
10. Candidate + identity contracts in preimage;
11. addressed order stable;
12. lowercase 64-hex;
13. not attempt/Take/causal identity.

### Reject codes through valid upstream states
14. same-subject/different-context Candidate -> ContextPacketIdentityMismatch only;
15. different-subject valid Candidate -> SubjectContextMismatch + ContextPacketIdentityMismatch in frozen order;
16. no duplicate Reject codes;
17. subject-only mismatch remains defensive impossible-state review, not public test-construction requirement;
18. malformed source/unsupported Candidate contract -> exception/static defensive review without bypass API;
19. no duplicate Patch 0006 parser validation.

### Concern evidence binding
20. zero initialized concerns valid on zero-Reject Input;
21. default/undefined/duplicate/over-count invalid;
22. five kinds frozen order;
23. copied Input Candidate content identity;
24. no prose/confidence/disposition/assessor/authenticity fields;
25. evidence Bind on Reject-coded Input -> exception;
26. synthetic evidence explicitly does not claim completed-review authenticity.

### Hard-rule short circuit
27. Reject codes + null evidence -> Reject;
28. Reject codes + non-null evidence -> exception/no disposition;
29. canonical Reject trace evidence null;
30. reference flow has no assessor dependency for deterministic Reject.

### Evidence-required path
31. zero Reject + null evidence -> exception/no disposition;
32. evidence for different hash/contract -> exception;
33. zero Reject + concern -> RequestAnotherTake;
34. zero Reject + zero concerns -> Accept.

### Non-authority
35. synthetic empty evidence may yield synthetic Accept evaluation but no authority-bearing token;
36. Evaluation exposes only Disposition + Trace;
37. no accepted-Take/history/State flag/token;
38. exact provisional-Take-vs-State-Interpreter ordering not frozen;
39. later authority must authenticate concern-review provenance separately.

### Creative law
40. false claim not rejected solely for truth conflict;
41. hidden-truth coincidence alone does not deterministically produce InaccessibleInformationUse;
42. no secret/canon keyword scanner;
43. no Performance rewrite;
44. no State/Context/Candidate mutation;
45. no Director/State Authority/Take/Commit/provider dependency;
46. no score/probability;
47. trace no Character/protected prose;
48. technical words in dialogue do not auto-create concern;
49. PotentialLockedAuthorityViolation is supplied semantic evidence, not false-speech detector.

### Technical failure / experimental isolation
50. completed authenticated semantic uncertainty evidence -> RequestAnotherTake;
51. assessor technical failure -> no authenticated evidence/no effective disposition, not Indeterminate;
52. RequestAnotherTake no retry/spend;
53. Patch 0008 review configuration fixed/attributable for per-Character Candidate-path comparison batches unless explicitly varied;
54. E0-E not forced through Patch 0008 Candidate API but remains subject to separately auditable hard integrity gates.

### Lifecycle
55. Reject non-history;
56. Request non-history;
57. Accept evaluation alone cannot advance Production authority;
58. Accept does not promote Director;
59. no opportunity/history mutation/trigger.

### Regression
60. Missing Raft StructuredContextHash unchanged;
61. Missing Raft RenderedContextHash unchanged;
62. ECJ-1 9112 bytes/frozen hash unchanged;
63. existing 211 Core tests green;
64. Missing Raft Harness PASS/0;
65. smoke Harness PASS/0.

## 35. E0-F compatibility

Patch 0008 prepares but does not complete E0-F.

Later E0-F proves secret failures, false-claim truth separation, locked canon/Constitution protection, malformed raw candidate rejection, candidate/assessor provider-failure isolation, partial/rejected non-history, deterministic retry/cost, and atomic accepted Performance + authoritative consequence commit.

No end-to-end guarantee is claimed here.

## 36. ARM64 / battery

Bind/hash/disposition are tiny deterministic CPU work. No background/AI/provider/network/GPU/NPU execution.

## 37. Explicit exclusions

No authenticated semantic-assessment artifact/attestation; semantic-assessor implementation; bounded assessor packet; assessor provenance authentication/persistence; provider attempt/provenance; retry/cost/cancellation engine; Take semantics/identity/TakeId; State Interpreter implementation; State Authority; consequence proposal; ProductionState/StateHash; atomic commit; persistence/recovery; effective opportunity establishment; Scene loop; World Resolver/observation; E0-D round-robin; E0-E playwright implementation/control protocol; E0-F end-to-end harness; UI/WinUI; Windows AI/NPU; packaging/WACK/Store.

## 38. Recursive adversarial audit dimensions

Restart after every correction:

1. frozen Integrity law;
2. H1 sequence;
3. Integrity evaluation vs later acceptance authority;
4. version-domain separation;
5. Reject vs RequestAnotherTake minimality;
6. statement/truth distinctions;
7. concern-category semantics;
8. Performance vs consequence authority;
9. Character vs Performer;
10. Access/Context privacy;
11. least-privilege Input;
12. hard-rule-before-semantic-review precedence;
13. Integrity vs Parser;
14. Integrity vs Director;
15. Integrity vs State/Take;
16. no fabricated assessor authentication;
17. Take/Interpreter ordering non-preemption;
18. semantic evidence vs deterministic authority;
19. uncertainty vs assessor technical failure;
20. assessor disclosure/cost short circuit;
21. E0 control isolation, especially E0-E;
22. concern-evidence privacy;
23. stale evidence/content binding;
24. content identity versioning;
25. content vs attempt/Take/causal identity;
26. technical failure vs fiction;
27. exception vs Reject;
28. no rewriting;
29. RequestAnotherTake vs retry/spend;
30. E0-F compatibility;
31. creator ontology guard;
32. API/non-forgeability/minimality;
33. deterministic bounded canonicalization;
34. fail closed/sanitization;
35. executable vs defensive testability;
36. dependency direction;
37. ARM64/battery;
38. scope/hygiene;
39. E0-A/B/C/D/E/F/G isolation;
40. future State/Take/commit compatibility.

Approval only after complete restart produces zero material corrections or worthwhile improvements.

## 39. Material approval decisions

Approval would freeze only:

1. Patch 0008 as next E0-A Integrity evaluation boundary after Patch 0007;
2. IntegrityValidationEvaluation is deterministic calculation, not accepted-Take/State authority;
3. four separate v1 contract/version domains for Input, Candidate-content identity, ConcernEvidence, and Validation;
4. least-privilege IntegrityCandidateInput is sole rich Context+Candidate binding surface;
5. Input publicly carries only InputContract, source Context ID, Candidate content identity, Reject codes;
6. hard Rejects short-circuit semantic review/disclosure;
7. Validator receives no Character/Performance prose;
8. versioned CandidateContentHash binds concern evidence to exact Candidate semantics;
9. content hash not attempt/Candidate/Take/commit identity;
10. Reject codes exactly subject mismatch then ContextPacket mismatch;
11. under valid upstream paths, different-subject naturally also means different Context identity; impossible subject-only mismatch does not justify bypass API;
12. malformed source/unsupported Candidate contract is exception-domain;
13. E0 Reject means deterministic Candidate/source hard mismatch; RequestAnotherTake means typed semantic concern on otherwise correctly bound Candidate;
14. concern kinds preserve guess/claim/belief/intention versus unavailable knowledge/enacted authority distinctions;
15. `IntegrityConcernEvidence` is structurally bound synthetic-capable evidence, not authenticated assessment;
16. five concern kinds carry no rationale/confidence/assessor/authenticity claim;
17. concern evidence cannot bind to Reject-coded Input;
18. completed semantic uncertainty differs from assessor technical failure;
19. valid concern evidence required only when zero deterministic Reject codes;
20. assessor failure on otherwise eligible Candidate yields no effective Integrity disposition until later deterministic fallback/review resolves it;
21. no semantic assessor/provider implementation in Patch 0008;
22. future semantic review uses least-privilege packet;
23. Patch 0008 review mechanism/configuration stays fixed/attributable for per-Character E0 comparison paths where used, but E0-E is not forced through Candidate-specific API;
24. exact disposition: RejectCodes+null evidence -> Reject; zero Reject+concerns -> RequestAnotherTake; zero Reject+empty concerns -> Accept; inconsistent/missing evidence -> exception;
25. Patch 0008 emits no authority-bearing eligibility/attestation object;
26. synthetic evidence may produce synthetic evaluation but cannot authorize Production change;
27. later State/Take/orchestration must authenticate semantic-review provenance under own contract before progression;
28. immediate provisional-Take-vs-State-Interpreter ordering remains later;
29. RequestAnotherTake does not authorize retry/spend;
30. false claims not truth-policed; no keyword scan or hidden rewrite;
31. candidate/assessor technical failures stay technical/orchestration concerns;
32. Director not Integrity input and Accept cannot promote precommit Director work;
33. Trace nests Input + optional ConcernEvidence and contains no Performance/private prose or assessor-authenticity claim;
34. no State Interpreter/Authority, Take, commit, persistence, opportunity application, Scene loop, provider execution, UI, Windows AI/NPU, or Store machinery enters Patch 0008.

Implementation remains blocked until recursive audit completes and user explicitly approves final proposal.
