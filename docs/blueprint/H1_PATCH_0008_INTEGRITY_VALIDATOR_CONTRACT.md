# H1 Patch 0008 — E0 Integrity Validator Contract

Status: blueprint proposal 0.1 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0007
Branch: `h1-patch-0008-integrity-validator-blueprint`

## 1. Purpose

Define the next frozen E0-A boundary after Performer Candidate and Director proposal calculation:

```text
source ContextPacket + CandidatePerformance
    -> deterministic Candidate content identity
    -> bounded integrity-concern assessment evidence
    -> deterministic Integrity Validator
        -> Accept | Reject | RequestAnotherTake
            -> only Accept may proceed to later State Interpreter / Take authority
```

Patch 0008 defines candidate-integrity evaluation only.

It does **not** create an accepted Take, mutate Production, interpret consequences, apply State, commit history, authorize retry/spend, establish Current Opportunity, call a provider/model, or implement semantic-assessor transport.

## 2. Recovered frozen authority

Blueprint 0.1 requires the E0-A preparation sequence:

1. Access Control;
2. Context Composer;
3. Performer candidate output;
4. Director opportunity rules;
5. **Integrity Validator acceptance/rejection rules**;
6. State Interpreter candidate-mutation schema;
7. deterministic State Authority;
8. accepted/rejected/alternate Take semantics;
9. atomic causal-commit record.

It also freezes:

```text
Integrity Validator -> State Interpreter -> deterministic State Authority
```

Integrity Validator:

- checks a candidate Performance before acceptance;
- deterministic checks own hard invariants;
- model-assisted checks may flag semantic concerns but cannot waive hard rules;
- asks whether inaccessible information was used, protected information exposed, locked canon/world law violated, required contract broken, or technical failure leaked into fiction;
- returns accept, reject, or request another take;
- must not rewrite the Performance.

E0 hard gates additionally require that inaccessible information/prohibited context, silent truth promotion, creator-locked canon/Constitution mutation, technical failure becoming fiction, unaccepted partial Performance entering history, non-causal history change, and non-atomic Performance/consequence commit invalidate the run.

Patch 0008 owns only the subset that can truthfully be evaluated at the candidate-integrity boundary. It does not steal later State/Take/commit responsibilities merely because those are also E0 hard gates.

## 3. Integrity is not truth policing

A Character may:

- lie;
- be mistaken;
- speculate;
- repeat a rumor;
- contradict another Character;
- state a false belief;
- make a claim that objective Production truth does not support.

Those are not automatically integrity failures.

A model-created statement becomes dangerous only if a later authority silently promotes it into truth, or if the Performance itself credibly uses information the Character could not possess / enacts something forbidden by locked authority.

Therefore Patch 0008 introduces **no deterministic secret/canon keyword scan** and no rule such as “candidate text contradicts objective truth -> Reject.” Such a rule would destroy the frozen distinction between statement, claim, belief, possibility, and fact.

## 4. Input boundary

The deterministic validator consumes exactly:

```text
ContextPacket sourceContext
CandidatePerformance candidate
IntegrityConcernAssessment concernAssessment
```

It does not consume:

- ValidatedFixture / complete Production;
- Access deny-decision text;
- Director proposal/evaluation;
- State Interpreter proposals;
- State Authority;
- Take/Commit identity;
- provider credentials/configuration;
- raw/partial provider output;
- model chain-of-thought/rationale;
- filesystem/network/clock/random state.

`sourceContext` is the Character-bounded ContextPacket under which the Candidate was produced.

## 5. Candidate content identity

Patch 0006 intentionally introduced no CandidateId/TakeId. Integrity concern evidence nevertheless must bind to the exact Candidate semantic content it assessed.

Freeze a deterministic content identity:

```text
CandidateContentHash = SHA-256(canonical candidate semantic JSON)
```

Canonical candidate semantic JSON contains exactly, in this order:

1. `contractVersion`
2. `subjectCharacterId`
3. `contextPacketId`
4. `visibleText`
5. `control`

`control` contains exactly:

1. `addressedCharacterIds`
2. `nominatedCharacterId`

Rules:

- UTF-8 without BOM;
- existing canonical JSON scalar/string emission discipline is reused;
- Candidate visible text is emitted exactly as already-validated text;
- addressed Character IDs preserve Patch 0006 canonical ordinal order;
- nomination emits string or JSON `null`;
- SHA-256 is lowercase 64-hex.

`CandidateContentHash` is **content identity only**.

It is not:

- provider-attempt identity;
- CandidateId;
- TakeId;
- causal-commit identity;
- acceptance authority;
- proof of which provider/model generated the content.

Two distinct attempts producing byte-for-byte equivalent candidate semantics intentionally share the same CandidateContentHash. Later attempt provenance remains separate.

## 6. Integrity concern assessment evidence

Freeze:

```text
IntegrityConcernAssessment
- AssessmentContract
- CandidateContentHash
- Concerns
```

```text
AssessmentContract = ensemble.e0.integrity.concerns.v1
```

`Concerns` is a canonical ordinal/distinct immutable set of typed non-authoritative findings.

Initial E0 finding kinds:

```text
PotentialInaccessibleInformationUse
PotentialProtectedInformationExposure
PotentialLockedAuthorityConflict
PotentialTechnicalArtifactLeak
IndeterminateSemanticIntegrity
```

These are integrity-policy categories, not creator-facing dramatic ontology.

The assessment contains:

- no prose rationale;
- no chain-of-thought;
- no Character-private text;
- no forbidden-record text;
- no confidence percentage/score;
- no accept/reject authority;
- no State mutation;
- no retry/spend authority.

## 7. Assessment construction is not semantic authority

Patch 0008 may expose a deterministic binding/factory that associates a Candidate with a typed concern set and computes/stores its CandidateContentHash.

That construction proves only:

- structurally valid concern kinds;
- canonical concern ordering/distinctness;
- exact Candidate content binding.

It does **not** authenticate who/what produced the concern judgment.

For effective E0 orchestration, later provider/reviewer orchestration must supply an assessment from the configured integrity-review path and preserve its provenance.

Synthetic concern assessments remain legitimate for deterministic tests and failure injection.

## 8. Future semantic-assessor privacy requirement

Patch 0008 does not implement a model/human semantic assessor.

Any later semantic-assessor path must obey Context Sovereignty:

- it must not receive complete Production merely because integrity review needs more authority than a Character;
- it should receive a separately defined bounded integrity-assessment packet containing only the Candidate plus the minimum authoritative constraints/references required to evaluate integrity;
- external disclosure must remain attributable and separate from Character-facing Context;
- no semantic assessor may directly return or commit State mutations;
- no semantic assessor can waive deterministic hard failures.

The exact assessor/provider/model/human mechanism is intentionally outside Patch 0008.

## 9. Cross-boundary deterministic validation

The validator fails closed or deterministically Rejects unless the evaluated pair is coherent.

Trusted-object malformed/null/uninitialized states that cannot be expressed through normal public construction use the Integrity exception domain rather than creative disposition.

For independently valid source/candidate objects, deterministic candidate violations include:

1. unsupported Candidate contract version;
2. Candidate SubjectCharacterId != Context SubjectCharacterId;
3. Candidate ContextPacketId != Context ContextPacketId;
4. Context SubjectCharacterId != Context OpportunityCharacterId;
5. concern assessment CandidateContentHash != recomputed CandidateContentHash;
6. unsupported assessment contract.

Patch 0008 does not duplicate Patch 0006 address/nomination/VisibleText parser validation. CandidatePerformance remains the canonical owner of those structural rules.

## 10. Integrity disposition

Freeze:

```text
IntegrityDisposition
- Accept
- Reject
- RequestAnotherTake
```

Exact deterministic precedence:

### Reject

If one or more deterministic candidate hard violations are present:

```text
Disposition = Reject
```

Semantic concerns cannot override this result.

### RequestAnotherTake

Otherwise, if `Concerns` is non-empty:

```text
Disposition = RequestAnotherTake
```

This is conservative handling of semantic/integrity concern evidence.

### Accept

Otherwise:

```text
Disposition = Accept
```

`Accept` means **integrity-eligible to proceed to later interpretation/review**.

It does **not** mean:

- accepted Take;
- Production history;
- State approved;
- consequences committed;
- next opportunity established;
- retry/spend authorized.

## 11. RequestAnotherTake is not retry authority

`RequestAnotherTake` records the Integrity Validator's disposition only.

It does not:

- call a provider;
- authorize another paid request;
- increment retry budget;
- choose an understudy;
- modify Current Opportunity;
- discard accepted history;
- create a new Candidate automatically.

Later deterministic orchestration/cost policy decides whether another attempt is permitted.

The current Candidate remains unaccepted diagnostic/provenance material only.

## 12. Reject is not fictional action

A rejected Candidate:

- does not enter Production history;
- does not become recent performance;
- does not alter Character memory/belief/state;
- does not establish a Director opportunity;
- does not become an event that other Characters can observe;
- remains available only as experimental/provider-attempt diagnostics when later provenance exists.

## 13. No hidden rewriting

Integrity Validator returns no replacement text, corrected Performance, paraphrase, suggested line, or rewritten control metadata.

If the Candidate fails or raises concern, the candidate remains exactly what the Performer produced.

This preserves Performer texture and keeps correction/retry explicit.

## 14. Director interaction

Director proposal/evaluation is not an Integrity input.

Any precommit Director calculation associated with a Candidate remains discardable speculation under Patch 0007.

If Integrity disposition is Reject or RequestAnotherTake, no Director result associated with that attempt can become effective routing.

If Integrity disposition is Accept, that still does not promote any precommit Director result. Patch 0007's mandatory postcommit re-Bind/recompute rule remains unchanged.

## 15. State/Take boundary

Only an `Accept` Integrity result may proceed to the later State Interpreter candidate-mutation boundary.

Even then:

```text
Integrity Accept != accepted Take
Integrity Accept != authoritative consequence
Integrity Accept != causal commit
```

The State Interpreter may propose consequences only after Integrity Accept.

Deterministic State Authority and later Take/commit authority remain separate.

## 16. Technical provider failure boundary

A provider refusal, timeout, transport error, cancellation, or partial stream is not itself a CandidatePerformance and must not be converted into fictional content by orchestration.

Patch 0008 cannot infer transport provenance merely by scanning prose for words such as “timeout,” “error,” or “refusal.” Characters may legitimately speak those words.

`PotentialTechnicalArtifactLeak` exists only for bounded concern evidence when a configured assessor/reviewer has reason to believe candidate content contains leaked technical artifacts.

The stronger E0-F provider-failure hard gate remains a later orchestration/provenance integration test.

## 17. Evaluation/trace shape

Freeze:

```text
IntegrityValidationEvaluation
- Disposition
- Trace
```

```text
IntegrityValidationTrace
- ValidationContract
- ContextPacketId
- CandidateContentHash
- DeterministicViolationCodes
- ConcernAssessment
```

```text
ValidationContract = ensemble.e0.integrity.validation.v1
```

Deterministic violation codes are closed E0 structural codes corresponding to Section 9.

Trace contains no Candidate VisibleText, Context text, denied/protected text, provider output, free-form rationale, confidence score, or chain-of-thought.

Trace is diagnostic/provenance material and never Character-facing Context.

## 18. Determinism

Given identical:

- validated source ContextPacket semantics;
- CandidatePerformance semantics;
- IntegrityConcernAssessment;
- contract versions;

the validator produces identical CandidateContentHash, deterministic violations, disposition, and trace.

No clock, filesystem, network, random, current culture, provider/model inference, GPU/NPU, or mutable global state.

## 19. Fail-closed exception domain

Use an Integrity-specific exception domain for malformed trusted objects/programmer misuse that cannot truthfully be represented as a creative Candidate rejection.

Exceptions expose structural field names/codes only and never echo Candidate VisibleText, private Context text, protected information, semantic assessor rationale, or raw provider output.

No exception causes fallback acceptance.

## 20. Public-surface intent

Preferred shared surface:

```text
CandidateIntegrityIdentity.Compute(CandidatePerformance)
    -> string CandidateContentHash

IntegrityConcernAssessment.Bind(
    CandidatePerformance,
    IEnumerable<IntegrityConcernKind>)
    -> IntegrityConcernAssessment

DeterministicIntegrityValidator.Validate(
    ContextPacket,
    CandidatePerformance,
    IntegrityConcernAssessment)
    -> IntegrityValidationEvaluation
```

Public models are read-only.

Assessment construction is explicitly evidence binding, not assessor authentication.

No public operation commits or retries.

## 21. Required tests / review gates

### Candidate content identity

1. deterministic repeated CandidateContentHash;
2. visible-text change changes hash;
3. address-control change changes hash;
4. nomination change changes hash;
5. ContextPacketId change changes hash;
6. Candidate contract version participates in hash;
7. canonical addressed order produces stable identity;
8. CandidateContentHash is lowercase 64-hex and not an attempt/Take ID.

### Input binding

9. canonical Missing Raft/Voss Candidate + source Context validates;
10. independently valid Candidate from different ContextPacket -> Reject;
11. independently valid different-subject Candidate -> Reject;
12. concern assessment bound to different Candidate content -> Reject;
13. unsupported assessment contract -> Reject;
14. malformed impossible trusted object -> Integrity exception domain;
15. no duplicate Patch 0006 VisibleText/control validator is introduced.

### Disposition

16. zero deterministic violations + zero concerns -> Accept;
17. deterministic violation + zero concerns -> Reject;
18. deterministic violation + semantic concerns -> Reject;
19. zero deterministic violations + one concern -> RequestAnotherTake;
20. multiple concerns -> RequestAnotherTake;
21. every E0 concern kind independently produces RequestAnotherTake;
22. RequestAnotherTake does not authorize retry/spend;
23. Accept is explicitly not accepted Take/history authority.

### Creative-law guards

24. a false Character claim is not deterministically rejected merely for conflicting with objective truth;
25. no deterministic secret/canon keyword scanner exists;
26. no Performance rewrite output/surface exists;
27. validator cannot mutate State/Context/Candidate;
28. no Director input/dependency;
29. no State Interpreter/State Authority/Take/Commit dependency;
30. no provider/model/network dependency;
31. no confidence/score/probability fields;
32. trace contains no Candidate/Context/protected prose;
33. semantic concern assessment contains typed findings only, no rationale prose;
34. technical words in legitimate Character dialogue do not deterministically create TechnicalArtifactLeak.

### Lifecycle authority

35. Reject candidate remains non-history;
36. RequestAnotherTake candidate remains non-history;
37. Accept only enables later State Interpreter eligibility;
38. precommit Director evaluation is not promoted by Integrity Accept;
39. no effective opportunity mutation/history append/Performer trigger.

### Regression

40. frozen Missing Raft StructuredContextHash unchanged;
41. frozen Missing Raft RenderedContextHash unchanged;
42. frozen ECJ-1 9112-byte/hash identity unchanged;
43. all existing 211 Core tests remain green;
44. Missing Raft Harness PASS/0;
45. smoke Harness PASS/0.

## 22. E0-F compatibility

Patch 0008 prepares but does not complete E0-F failure injection.

Later E0-F orchestration must separately prove:

- inaccessible-secret attempts are caught or invalidate the run;
- tempting false claims remain claims unless State Authority explicitly promotes something through valid authority;
- locked canon/Constitution cannot mutate;
- malformed raw candidate output fails before CandidatePerformance construction;
- provider failure/refusal/cancellation cannot become fictional action;
- rejected/partial attempts stay outside Production history;
- retries/cost remain deterministic;
- accepted Performance + authoritative consequences commit atomically.

Patch 0008 must not falsely claim those end-to-end guarantees by itself.

## 23. ARM64 / battery

Candidate hashing and deterministic disposition are small CPU operations with no background work.

Patch 0008 introduces no AI/provider/network/GPU/NPU execution.

A future semantic assessor may have performance/privacy implications but is not implemented or claimed here.

## 24. Explicit exclusions

No semantic-assessor provider/model implementation; full integrity-assessment packet; provider attempt/provenance; retry/cost/cancellation engine; accepted/rejected/alternate Take identity; TakeId; State Interpreter; State Authority; consequence proposal; ProductionState/StateHash; atomic causal commit; persistence/recovery; effective opportunity establishment; Scene loop; World Resolver/observation; E0-D round-robin execution; E0-E playwright; E0-F end-to-end injection harness; UI; WinUI; Windows AI/NPU; packaging/WACK/Store.

## 25. Recursive adversarial audit dimensions

Restart after every material correction:

1. frozen Blueprint 0.1 Integrity law;
2. H1 sequence/next-boundary correctness;
3. statement vs truth/claim/belief law;
4. Character vs Performer authority;
5. Access/Context privacy;
6. Integrity vs parser ownership;
7. Integrity vs Director ownership;
8. Integrity vs State Interpreter/Authority;
9. Integrity Accept vs accepted Take;
10. semantic concern evidence vs deterministic authority;
11. assessment privacy/minimal disclosure;
12. stale-assessment/candidate-content binding;
13. content identity vs attempt/Take/causal identity;
14. technical failure vs fictional action;
15. no hidden rewriting;
16. RequestAnotherTake vs retry/spend authority;
17. E0-F hard-gate compatibility;
18. creator-ontology extensibility guard;
19. public API/non-forgeability/minimality;
20. deterministic ordering/canonicalization;
21. fail closed/error sanitization;
22. tests/testability without production bypass APIs;
23. dependency direction;
24. ARM64/battery;
25. scope/hygiene;
26. E0-A/B/C/D/E/F/G experimental isolation;
27. future State/Take/commit compatibility.

Approval only after a complete restart produces zero material corrections or worthwhile improvements.

## 26. Material approval decisions

Approval would freeze only:

1. Patch 0008 as the next E0-A boundary after Patch 0007;
2. candidate Integrity evaluation before State Interpreter/Take authority;
3. deterministic CandidateContentHash over exact candidate semantic content for stale-assessment binding;
4. CandidateContentHash is content identity only, never attempt/Candidate/Take/commit identity;
5. bounded typed IntegrityConcernAssessment is non-authoritative evidence, not assessor authentication;
6. no semantic assessor/provider is implemented in Patch 0008;
7. future semantic review must use a separate least-privilege integrity-assessment packet rather than full Production by default;
8. E0 concern kinds: inaccessible-information use, protected-information exposure, locked-authority conflict, technical-artifact leak, indeterminate semantic integrity;
9. semantic concerns contain no prose rationale/confidence/chain-of-thought;
10. deterministic hard cross-boundary violations dominate semantic concerns;
11. exact disposition precedence: hard violation -> Reject; otherwise concern -> RequestAnotherTake; otherwise Accept;
12. Integrity Accept means eligible for later interpretation/review only, not accepted Take/history/state;
13. RequestAnotherTake does not authorize provider retry/spend;
14. false claims/beliefs are not automatically rejected merely because they conflict with objective truth;
15. no deterministic secret/canon keyword scanning;
16. no hidden Performance rewriting;
17. provider failures/partial streams remain outside CandidatePerformance and require later orchestration/provenance enforcement;
18. Director evaluation is not an Integrity input and Integrity Accept cannot promote precommit Director work;
19. trace stores only structural IDs/hashes/codes/typed assessment, never Performance/private prose;
20. Patch 0008 does not implement State Interpreter, State Authority, Take, commit, persistence, opportunity application, Scene loop, provider execution, UI, Windows AI/NPU, or Store machinery.

Implementation remains blocked until recursive audit completes and the user explicitly approves the final proposal.
