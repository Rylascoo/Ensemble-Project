# H1 Patch 0011 — E0 Take Semantics Contract

Status: blueprint proposal 0.3 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
Parent baseline: machine-validated H1 Patch 0010
Branch: `h1-patch-0011-take-semantics-blueprint`

## 1. Purpose

Define the next E0 deterministic-spine boundary after machine-validated Patch 0010 State Authority and before atomic causal commit:

```text
ContextPacket
+ CandidatePerformance
+ Integrity Accept evaluation
+ StateInterpretationProposal
+ terminal StateAuthorityEvaluation
+ TakeId
+ explicit E0 Take disposition
    -> E0Take.Bind
        -> exact immutable Performance
        -> exact interpreted consequence proposal
        -> exact terminal deterministic State Authority evaluation
        -> Accepted / Rejected / Alternate Take
            -> later atomic causal commit only for Accepted
```

Patch 0011 defines accepted/rejected/alternate **Take semantics** only.

It does not mutate Production state, apply approved mutations, allocate new RecordIds, create ProductionState or StateHash, allocate CommitId, append causal history, persist anything, establish a new Current Opportunity, trigger another Performer, call any model/provider, or implement branching/rehearsal/retcon UX.

## 2. Recovered frozen authority

Blueprint 0.1 freezes:

- a Performance is one actual generated attempt;
- a generated attempt becomes Production history only if accepted as a Take;
- accepted Performance is preserved without hidden rewriting;
- Integrity Validator checks candidate Performance before acceptance;
- `Integrity Validator -> State Interpreter -> deterministic State Authority`;
- State Interpreter proposes possible consequences and never mutates authority;
- State Authority decides which proposed consequences are Approved / Rejected / RequiresReview;
- accepted Performance plus all approved consequences form one atomic causal commit;
- either the accepted Take and approved mutations commit coherently or neither does;
- partial/cancelled/unaccepted output never becomes Production history;
- accepted Takes are immutable;
- alternate takes, branches, retcons, and corrections cannot silently rewrite accepted history;
- E0 requires accepted/rejected/alternate Take semantics before the atomic causal-commit record;
- E0 excludes final branching, rehearsal, retcon, Another Take UX, long-running persistence, Production UX, and post-E0 canon-promotion design.

Patch 0007 additionally freezes that rejected/request-another-take/alternate attempts do not become effective opportunity-history events while the existing opportunity remains effective.

Patch 0008 freezes Integrity `Accept` as evaluation only, not accepted-Take authority.

Patch 0009 freezes State Interpretation as proposal-only and deliberately leaves provisional-Take ordering open.

Patch 0010 freezes State Authority as deterministic consequence decision only and again leaves provisional-Take ordering to this dedicated contract.

## 3. Ordering decision — no provisional Take before State Interpreter / State Authority

Patch 0011 resolves the previously open ordering as:

```text
CandidatePerformance
    -> Integrity evaluation
        -> State Interpretation
            -> State Authority
                -> E0 Take
                    -> later atomic causal commit
```

There is **no separate provisional Take object before State Interpretation or State Authority** in E0.

Reason:

1. `CandidatePerformance` already is the immutable provisional Performer semantic object;
2. Patch 0008 Candidate content identity intentionally is content identity, not attempt/Take identity;
3. Patch 0009/0010 semantic proposal and authority identities intentionally contain no TakeId;
4. inserting a Take before those validated boundaries would either duplicate Candidate semantics or require retrofitting Take identity into already-approved Interpreter/State Authority contracts;
5. identical Candidate semantics may arise from distinct attempts, so a pre-Interpreter TakeId that is not carried through Interpreter/Authority would not strengthen their semantic association;
6. E0 inefficiency is acceptable; authority ambiguity is not.

Therefore Patch 0011 creates the first Take object only after the Performance has passed Integrity and its Interpreter proposal has reached a terminal deterministic State Authority evaluation.

This decision does not claim that the final post-E0 product must use the same orchestration shape.

## 4. Distinct lifecycle terms

The E0 pipeline now has distinct states:

```text
provider attempt
    -> may fail technically with no CandidatePerformance

CandidatePerformance
    -> provisional Performer semantic output

Integrity Reject / RequestAnotherTake
    -> Candidate remains non-Take diagnostic/provenance material

Integrity Accept + Interpretation + terminal State Authority
    -> Take-eligible semantic package

E0Take Accepted
    -> selected for later atomic commit; not yet Production history

E0Take Rejected
    -> valid fully evaluated package deliberately not selected for Production history

E0Take Alternate
    -> valid fully evaluated noncanonical alternative retained for E0 provenance

successful atomic commit of Accepted Take
    -> effective accepted historical Performance + approved consequences
```

These terms must not collapse.

## 5. Contract

```text
E0TakeContractVersion = ensemble.e0.take.v1
```

Patch 0011 defines no AI JSON transport, provider schema, persistence serialization, branch schema, or Store/UI contract.

## 6. Existing TakeId is canonical

`TakeId` already exists in `Ensemble.E0.Core.Domain.StrongIds` and remains the canonical strong ID type.

Patch 0011 must not introduce another Take identifier type.

Patch 0011 defines TakeId **semantics**, not a global durable allocator:

- every `E0Take` requires an initialized existing `TakeId`;
- TakeId identifies one Take decision/package occurrence;
- TakeId is not content identity;
- semantically identical Performer outputs may have distinct TakeIds;
- TakeId is not CandidateContentHash, ProposalContentHash, CommitId, RecordId, ContextPacketId, RunId, or future StateHash;
- Core Patch 0011 cannot prove global uniqueness because authoritative Production history/persistence does not exist yet;
- effective E0 orchestration must not reuse a TakeId within one Run;
- exact product-wide allocation/uniqueness/persistence rules remain later authority.

No TakeId is derived from Candidate, proposal, authority-decision, clock, random, provider, or model data inside Core Patch 0011.

## 7. E0 Take aggregate

Conceptual public shape:

```text
E0Take
- ContractVersion
- TakeId
- Disposition
- Performance
- InterpretationProposal
- AuthorityEvaluation
```

Where:

- `Performance` is the exact immutable Patch 0006 `CandidatePerformance`;
- `InterpretationProposal` is the exact immutable Patch 0009 `StateInterpretationProposal`;
- `AuthorityEvaluation` is a freshly verified canonical Patch 0010 `StateAuthorityEvaluation` for that exact proposal and authority trace;
- `Disposition` is Patch 0011 `Accepted`, `Rejected`, or `Alternate`.

The Take therefore preserves the actual Performance rather than reducing accepted history to hashes.

It introduces no rewritten Performance text, corrected control, merged proposal, confidence score, free-form rationale, provider/model field, or hidden reasoning.

## 8. Why the Take retains the Performance

Blueprint 0.1 says accepted Performance is preserved and historical texture remains recoverable even when it produces no durable projected-state mutation.

A Take containing only CandidateContentHash / ProposalContentHash would make later historical reconstruction depend on a separate lookup whose persistence contract does not yet exist.

Patch 0011 therefore retains the exact immutable `CandidatePerformance` object in the semantic Take.

Its `VisibleText` and typed control remain the exact Patch 0006 semantics. No trimming, normalization, paraphrase, rewrite, or regeneration occurs.

## 9. One rich reconciliation boundary

Conceptual construction:

```text
E0Take.Bind(
    TakeId takeId,
    ContextPacket sourceContext,
    CandidatePerformance performance,
    IntegrityValidationEvaluation integrityEvaluation,
    StateInterpretationProposal interpretationProposal,
    StateAuthorityEvaluation authorityEvaluation,
    E0TakeDisposition disposition)
    -> E0Take
```

This is the sole Patch 0011 rich-object reconciliation boundary.

The Take object does not retain `ContextPacket` or `IntegrityValidationEvaluation`; those remain separate E0 provenance records.

## 10. Canonical upstream re-binding rather than duplicated validation

`E0Take.Bind` verifies association by reusing existing canonical deterministic boundaries rather than reimplementing them.

Conceptually:

1. validate TakeId and current Take disposition;
2. reconstruct a fresh Patch 0009 source:

```text
freshSource = StateInterpretationSource.Bind(
    sourceContext,
    performance,
    integrityEvaluation)
```

3. obtain the supplied authority evaluation Trace and its Patch 0010 Snapshot / Policy / ReviewSet;
4. reconstruct a fresh Patch 0010 input:

```text
freshAuthorityInput = StateAuthorityInput.Bind(
    authorityEvaluation.Trace.Input.Snapshot,
    freshSource,
    interpretationProposal)
```

5. deterministically re-evaluate Patch 0010 using the exact supplied Policy and ReviewSet:

```text
freshAuthorityEvaluation = DeterministicStateAuthority.Evaluate(
    freshAuthorityInput,
    authorityEvaluation.Trace.Policy,
    authorityEvaluation.Trace.ReviewSet)
```

6. require the supplied and fresh Patch 0010 evaluation semantics to match exactly;
7. require terminal complete State Authority result;
8. store the exact Performance, InterpretationProposal, and canonical fresh StateAuthorityEvaluation in the new E0Take.

The deterministic re-evaluation is verification, not a new authority layer. It prevents a Take from binding a Performance/proposal package to a substituted or stale State Authority result while preserving one canonical State Authority implementation.

Patch 0011 does not rerun any semantic Integrity assessor, call any provider, or reinterpret Performance prose.

## 11. Terminal State Authority requirement

A Take may be created only when Patch 0010 State Authority is terminal:

```text
StateAuthorityEvaluation.Status == Complete
```

and no decision has disposition:

```text
RequiresReview
```

`RequiresReview` means the consequence decision contract is incomplete. It cannot be treated as approved consequence authority and cannot enter a Take.

A complete evaluation may contain any combination of:

```text
Approved
Rejected
```

including:

- zero mutations / zero decisions;
- all Approved;
- mixed Approved and Rejected;
- all Rejected.

## 12. Rejected consequence != rejected Performance

Patch 0011 preserves the authority split:

```text
Integrity Validator
    decides whether Candidate Performance can progress

State Authority
    decides whether each Interpreter-proposed consequence is approved

Take disposition
    decides whether the fully evaluated Performance package is selected for Production history, rejected, or retained as an alternate
```

Therefore a State Authority `Rejected` mutation does **not** automatically reject the Performance.

Examples of structurally Take-eligible outcomes include:

```text
accepted Take + zero proposed durable mutations
accepted Take + some approved consequences + some rejected proposals
accepted Take + all Interpreter proposals rejected
```

In these cases the accepted Performance may remain historical texture while only approved consequences are eligible for later projected-state application.

However, **structurally Take-eligible is not a claim that every such package is a valid E0 run**. Frozen Blueprint 0.1 separately requires transcript/current-authority coherence and invalidates a run if a committed consequence required by an accepted Performance is absent, if Performance and approved consequences do not commit atomically, or if an Integrity violation escaped the earlier boundary. Patch 0011 cannot repair such a failure by inventing a second prose reviewer or silently promoting a rejected mutation. Any demonstrated coherence failure remains a hard-gate failure to diagnose at the responsible Integrity / interpretation-completeness / atomic-commit boundary before the run contributes experiential evidence.

Rejected mutation text never becomes authoritative consequence merely because the Performance is accepted.

If a Performance itself violates locked authority or impossible world law, that belongs to the Integrity boundary; Patch 0011 does not create a hidden second prose-reviewer.

## 13. E0 Take disposition

```text
E0TakeDisposition
- Accepted
- Rejected
- Alternate
```

All three values describe a fully evaluated, Integrity-cleared package.

They do not replace Patch 0008 dispositions:

```text
IntegrityDisposition.Accept
IntegrityDisposition.Reject
IntegrityDisposition.RequestAnotherTake
```

Undefined Take disposition fails closed.

The Take disposition is explicit typed orchestration input. It is never inferred from Candidate prose, Interpreter text, provider/model output, State Authority reason wording, sentiment, confidence, hidden reasoning, or random choice.

## 14. Accepted

`Accepted` means:

> This exact fully evaluated Performance package has been selected as the Take that may proceed to the later atomic causal-commit boundary.

It does **not** mean:

- Production history has already changed;
- Performance is already visible as authoritative history;
- approved mutations have been applied;
- State has changed;
- Current Opportunity has advanced;
- Director history has advanced;
- another Performer may start;
- a CommitId exists.

Therefore:

```text
E0Take.Disposition == Accepted
    -> commit-eligible Take
    != committed historical Take
```

Only successful Patch 0012 atomic causal commit makes the accepted Performance and all Approved consequences effective together.

If that commit fails, neither Performance history nor consequence state becomes effective. The failed attempt and its Accepted-but-uncommitted Take remain E0 diagnostic/provenance material where the frozen run protocol requires them; they never masquerade as Production history.

## 15. Rejected

`Rejected` means:

> This exact Integrity-cleared, interpreted, fully State-Authority-evaluated Performance package is deliberately not selected for Production history.

It is distinct from Patch 0008 Integrity Reject.

A Rejected Take:

- never enters Production history;
- never applies Approved consequences despite those decisions being preserved as non-effective evaluated provenance;
- never updates recent-performance context;
- never changes effective opportunity history;
- never establishes Current Opportunity;
- never triggers Director progression;
- never becomes Observation;
- remains E0 diagnostic/provenance material.

Rejecting the Take does not assign fictional blame to the Character.

## 16. Alternate

`Alternate` means:

> Preserve this exact valid fully evaluated Take package as a noncanonical alternative for E0 comparison or later explicit creative use.

An Alternate Take:

- is not Production history;
- does not apply Approved consequences;
- does not mutate State;
- does not advance Current Opportunity or effective opportunity history;
- does not enter Character Context as something that happened;
- does not become Observation;
- does not trigger another Performer;
- remains immutable E0 provenance.

`Alternate` is **not** Patch 0008 `RequestAnotherTake`.

`RequestAnotherTake` is an Integrity evaluation outcome and produces no E0Take under this contract.

Patch 0011 does not define branch identity, alternate-history state, retcon, rehearsal, canon promotion, or later Alternate promotion. ODR-20 remains open.

If post-E0 product authority later promotes an alternate into another history, that must occur through explicit new causal authority rather than mutating this E0Take.

## 17. Attempts that never become Takes

The following do not create an E0Take:

- provider refusal;
- provider timeout;
- provider transport error;
- provider cancellation;
- partial stream without valid CandidatePerformance;
- malformed Candidate output;
- Integrity Reject;
- Integrity RequestAnotherTake;
- missing/invalid Integrity evaluation;
- State Interpretation technical/provider failure;
- malformed/invalid StateInterpretationProposal;
- State Authority structural failure;
- State Authority evaluation with RequiresReview;
- malformed/mismatched authority evaluation.

Technical failure remains technical failure.

Candidate attempts that fail before Take remain E0 diagnostics/provenance only where the frozen run protocol requires them.

## 18. Take immutability

Every `E0Take` is immutable regardless of disposition.

No API mutates:

```text
Rejected -> Accepted
Alternate -> Accepted
Accepted -> Rejected
Accepted -> Alternate
```

Accepted historical corrections, retcons, branches, alternate-history promotion, or canon changes must later create explicit new authority/history rather than rewriting a prior Take.

This preserves Blueprint 0.1 accepted-Take immutability without freezing post-E0 ODR-20 UX.

## 19. No new authority-decision content hash in Patch 0011

Proposal 0.1 considered introducing an `AuthorityDecisionContentHash`.

Proposal 0.3 removes it.

Reason:

- Patch 0010 already supplies an immutable `StateAuthorityEvaluation` with exact `Input`, `Policy`, `ReviewSet`, ordered Decisions, dispositions, and ordered reason codes in its Trace;
- Patch 0011 can deterministically re-bind/re-evaluate and retain that canonical evaluation;
- a second content-identity contract would add an evolution axis before persistence/serialization demonstrates a need for it;
- future Patch 0012 Commit identity may bind exact Take/evaluation semantics under its own causal-commit contract.

No hash should be introduced merely to avoid retaining an already-immutable authoritative semantic object.

## 20. Identity distinctions

The following remain distinct:

```text
ContextPacketId
    -> exact structured bounded Character context identity

CandidateContentHash
    -> semantic CandidatePerformance content identity

ProposalContentHash
    -> semantic StateInterpretationProposal content identity

TakeId
    -> identity of one Take decision/package occurrence

CommitId
    -> future atomic causal-commit identity

StateHash
    -> future authoritative Production-state identity
```

TakeId cannot be inferred from any content hash.

Distinct TakeIds may legitimately contain semantically identical CandidatePerformance / Interpretation content.

## 21. State freshness remains Patch 0012

Patch 0011 binds the exact State Authority evaluation that existed for the Take package.

It does not claim that the underlying StateAuthoritySnapshot is still current at commit time.

No StateHash/stale-state commit protocol exists yet.

Therefore `Accepted` means commit-eligible under the evaluated authority package only.

Patch 0012 must establish the exact stale-state/freshness/atomic-application law before an Accepted Take can become effective history.

## 22. Provenance and authentication boundary

Patch 0011 Core binding is structurally trustworthy but synthetic-capable.

It does not authenticate:

- provider identity or exact provider attempt;
- exact rendered Character disclosure;
- Integrity semantic assessor identity;
- concern-review provenance;
- State Authority policy provenance;
- explicit review-choice human/creator identity;
- Take-disposition human/creator identity.

This preserves the prior contracts rather than pretending typed objects are credentials.

Before an Accepted Take becomes effective Production history, later E0 orchestration / atomic commit must retain and authenticate whatever configured provider/review/policy provenance the effective run relies upon.

A hash or immutable object association is not authentication.

Rejected/Alternate Takes and synthetic tests may use synthetic provenance without creating Production authority.

## 23. Current Opportunity and Director interaction

Patch 0007 remains unchanged.

Before successful atomic source commit:

```text
Current Opportunity remains effective
```

Therefore:

- Candidate failure does not advance opportunity;
- Integrity Reject / RequestAnotherTake does not advance opportunity;
- Rejected Take does not advance opportunity;
- Alternate Take does not advance opportunity;
- Accepted-but-uncommitted Take does not advance opportunity;
- failed atomic commit does not advance opportunity.

Only after successful atomic commit may later orchestration obtain authoritative opportunity history ending at the committed source, re-bind source Context semantics, recompute Director proposal/evaluation, and separately establish the next effective opportunity.

Associated Candidate control from Rejected/Alternate/failed-commit Takes never becomes effective routing state.

## 24. Another Take and retry/spend authority

Patch 0011 does not call a Performer or authorize another provider attempt.

```text
Integrity RequestAnotherTake
Rejected Take
Alternate Take
```

may all leave the current opportunity available for later orchestration, but none authorizes spend, retry count, provider selection, understudy substitution, or another call by itself.

Cost/retry/cancellation authority remains deterministic orchestration outside this Core contract.

“Another Take” product UX remains post-E0.

## 25. Take a Seat

Take a Seat remains the human Performer posture under the same deterministic Access Control -> Context Composer boundary as an AI Performer.

A future human-produced CandidatePerformance follows the same Integrity -> Interpretation -> State Authority -> E0 Take semantics when that capability is implemented.

Take a Seat does not bypass this Take contract and does not grant the occupied Character omniscient Production access.

Patch 0011 implements no Take-a-Seat UI.

## 26. E0 reference acceptance policy

E0 is an architecture experiment and must not add unrecorded subjective curation that changes transcripts between comparison variants.

Therefore the **reference E0 orchestration policy** is:

```text
Take-eligible package
+ no separately labeled explicit Take-disposition intervention
    -> Accepted
```

Rejected or Alternate dispositions may be supplied only by:

- a separately labeled E0 test/failure/control case;
- an explicit creator/experiment intervention whose presence is preserved in provenance;
- a future approved orchestration mode outside the ordinary E0 reference comparison.

The Take disposition itself is never model-authored.

This E0 reference policy is not the final product consequence-acceptance UX and does not close ODR-19 (`Autopilot`, `Review`, `Strict Creator`, or another model).

The purpose is experimental isolation: ordinary E0 reference runs should expose Performer/architecture behavior rather than silently selecting only the lines a human reviewer happens to prefer.

## 27. E0 experimental isolation

For ordinary E0-A/C/D/G per-Character reference runs that reach this contract:

- Take semantics remain identical across compared variants unless Take behavior itself is the named variable;
- the default Take-eligible disposition is Accepted;
- Rejected/Alternate interventions must be labeled and attributable;
- provider/model identity does not alter Take authority;
- Take semantics do not inspect provider/model identity.

E0-B may vary Performer assignment without varying Take semantics.

E0-E single-playwright control is not forced through the per-Character H1 Candidate/Take API unless a separately approved control-compatible binding defines equivalent causal acceptance/provenance semantics.

E0-F must prove technical failure, malformed output, Integrity Reject/RequestAnotherTake, unresolved State Authority review, Rejected Take, Alternate Take, and failed commit cannot enter Production history.

## 28. Determinism, memory, and ARM64 suitability

Patch 0011 Core logic is deterministic and model-free.

No:

- network;
- filesystem;
- clock-dependent decision;
- randomness;
- provider API;
- GPU;
- NPU;
- global mutable state.

Association verification reuses existing deterministic binds/evaluation and is linear in the bounded E0 Context/Proposal/State Authority decision surface.

The Take aggregate retains references to existing immutable semantic objects rather than copying Character Context or duplicating proposal/evaluation structures.

No background execution exists; idle battery impact from this Core contract is effectively zero by architecture. This is not target-device power evidence.

## 29. Fail closed

Patch 0011 receives its own exception domain with sanitized structural messages only.

Failures include at minimum:

- null/missing inputs;
- uninitialized TakeId;
- undefined Take disposition;
- unsupported upstream contract versions;
- Context/Candidate/Integrity mismatch;
- Integrity disposition other than Accept;
- malformed InterpretationProposal;
- missing/malformed State Authority Trace/Input/Policy/ReviewSet;
- fresh State Authority re-bind mismatch;
- fresh deterministic re-evaluation mismatch;
- State Authority status ReviewRequired;
- any RequiresReview decision;
- malformed decision count/order.

Failure creates no Take and no fallback disposition.

## 30. Required implementation tests

Future implementation should prove at minimum:

1. existing `TakeId` strong type is reused; no new Take ID type exists;
2. TakeId required/initialized;
3. Take contract/disposition exact and defined;
4. exact CandidatePerformance object is retained;
5. exact InterpretationProposal object is retained;
6. State Authority evaluation is freshly and deterministically verified through canonical Patch 0010 logic;
7. Integrity Reject cannot bind a Take;
8. Integrity RequestAnotherTake cannot bind a Take;
9. technical/provider failure has no Take construction path;
10. mismatched Context/Candidate fails;
11. mismatched Integrity evaluation fails;
12. proposal Candidate identity mismatch fails;
13. proposal Scene mismatch fails;
14. State Authority proposal identity mismatch fails;
15. State Authority evaluation substitution fails;
16. State Authority ReviewRequired cannot bind;
17. any RequiresReview decision cannot bind;
18. empty mutation proposal + Complete authority can bind;
19. all-Approved decision set can bind;
20. mixed Approved/Rejected decision set can bind;
21. all-Rejected decision set can bind structurally but carries no blanket E0 hard-gate-validity claim;
22. Accepted Take applies no State and writes no history;
23. Rejected Take applies no State/history/Director routing;
24. Alternate Take applies no State/history/Director routing;
25. Rejected/Alternate Take cannot make Candidate control effective;
26. Accepted-but-uncommitted Take cannot make Candidate control effective;
27. failed commit leaves Accepted Take outside Production history and preserves only required E0 provenance;
28. identical semantic Candidate/Proposal packages may bind under distinct TakeIds;
29. TakeId is not equal/derived by contract from CandidateContentHash or ProposalContentHash;
30. Take aggregate exposes no provider/model/confidence/rationale/chain-of-thought field;
31. Take is immutable;
32. no disposition mutation API exists;
33. repeated Bind over identical semantic inputs and TakeId/disposition produces equivalent Take semantics;
34. fixed Missing Raft Context/Candidate/State Authority regression identities remain unchanged;
35. full Core regression suite remains green;
36. Missing Raft Harness regression remains green;
37. generic smoke Harness regression remains green.

Patch 0011 implementation must add no tests that falsely claim ProductionState, persistence, provider authentication, atomic commit, or runtime/hardware behavior.

## 31. Explicit non-goals

Patch 0011 does not implement or freeze:

- new TakeId type;
- global/durable TakeId allocator;
- CandidateId;
- provider-attempt ID schema;
- provider/model execution;
- State Interpreter provider input/request composition;
- authenticated provider/review/policy provenance machinery;
- ProductionState;
- StateHash;
- authoritative new RecordId allocation;
- mutation application;
- CommitId allocation/composition;
- atomic causal commit;
- persistence/recovery;
- branch DAG;
- rehearsal state;
- retcon;
- Alternate promotion;
- final Another Take UX;
- final Take a Seat UX;
- final ODR-19 consequence-acceptance modes;
- final ODR-20 rehearsal/branching/canon-promotion model;
- Director effective-opportunity application;
- Scene loop;
- World Resolver;
- Observation engine;
- Windows AI Foundry;
- NPU execution;
- WinUI;
- MSIX;
- WACK;
- Partner Center / Store certification.

## 32. Patch boundary summary

Completed E0 deterministic spine after Patch 0011 becomes:

```text
Access Control
    -> Context Composer
        -> Performer Candidate
            -> Director proposal calculation
            -> Integrity evaluation
                -> State Interpretation proposal
                    -> deterministic State Authority evaluation
                        -> E0 TAKE SEMANTICS
                            -> future atomic causal commit
```

Patch 0011's principal law is:

> A CandidatePerformance is the provisional generated Performance. An E0 Take exists only after that Performance has passed Integrity and its interpreted consequences have reached a terminal deterministic State Authority evaluation. The Take preserves the exact Performance, exact consequence proposal, exact authority evaluation, and an explicit Accepted / Rejected / Alternate disposition under a distinct TakeId. Accepted means selected for atomic commit, not already historical. Only later successful atomic commit makes the accepted Performance and all Approved consequences effective together.

## 33. Approval / implementation gate

This blueprint is architecture only.

Before implementation:

1. recursively adversarial-audit Proposal 0.3 against frozen Blueprint 0.1, approved Patches 0006–0010, current source/tests, engineering hygiene, E0 experiment isolation, provenance boundaries, and future Patch 0012 separation;
2. restart the audit after every material correction;
3. require one complete final pass with zero material corrections and zero worthwhile architectural improvements;
4. obtain explicit user approval;
5. create a fresh-chat implementation handoff;
6. do not write Patch 0011 executable code in the architecture chat.
