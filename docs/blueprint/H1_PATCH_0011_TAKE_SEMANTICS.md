# H1 Patch 0011 — E0 Take Semantics Contract

Status: blueprint proposal 0.9 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
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
        -> freshly verified terminal deterministic State Authority evaluation
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
    -> Take-bindable semantic package

E0Take Accepted
    -> selected for later atomic commit; not yet Production history

E0Take Rejected
    -> fully evaluated package not selected for Production history

E0Take Alternate
    -> fully evaluated noncanonical alternative retained for E0 provenance

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
- TakeId identifies one Take decision/package occurrence within the orchestration scope that supplies it;
- TakeId is not content identity;
- semantically identical Performer outputs may have distinct TakeIds;
- TakeId is not CandidateContentHash, ProposalContentHash, CommitId, RecordId, ContextPacketId, RunId, or future StateHash;
- Core Patch 0011 cannot prove global uniqueness because authoritative Production history/persistence does not exist yet;
- effective E0 orchestration must not reuse a TakeId within one Run;
- exact product-wide allocation/uniqueness/persistence rules remain later authority.

`E0Take.Bind` therefore receives TakeId from trusted orchestration and validates only that it is initialized.

Patch 0011 deliberately does **not** derive TakeId from RunId + ordinal or freeze another string format: RunId composition itself is not yet a frozen identity contract, and premature derivation would create compatibility debt without durable persistence authority. E0 orchestration must provide unique TakeIds and record them; the exact allocator/format remains an explicit later orchestration/persistence dependency rather than an implementation detail the Core coding chat may invent.

No TakeId is derived from Candidate, proposal, authority-decision, clock, random, provider, or model data inside Core Patch 0011.

## 7. E0 Take aggregate and construction authority

Conceptual public shape:

```text
public sealed class E0Take
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
- `AuthorityEvaluation` is the fresh canonical Patch 0010 `StateAuthorityEvaluation` recomputed from the supplied upstream evaluation Trace after exact source/proposal re-binding;
- `Disposition` is Patch 0011 `Accepted`, `Rejected`, or `Alternate`.

The Take therefore preserves the actual Performance rather than reducing accepted history to hashes.

It introduces no rewritten Performance text, corrected control, merged proposal, confidence score, free-form rationale, provider/model field, or hidden reasoning.

`E0Take` has a **private constructor**. `E0Take.Bind` is its sole construction path. Patch 0011 introduces no public or internal alternate constructor/factory that can bypass association replay, terminal-authority checks, disposition validation, or Accepted-specific invariants.

This is an authority property, not cosmetic encapsulation: future Core code must not be able to manufacture an apparently valid Take by directly setting fields or calling a weaker constructor.

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

This is the sole Patch 0011 rich-object reconciliation and construction boundary.

The Take object does not retain `ContextPacket` or `IntegrityValidationEvaluation`; those remain separate E0 provenance records.

## 10. Canonical upstream re-binding and fresh State Authority replay

`E0Take.Bind` verifies association by reusing existing canonical deterministic boundaries rather than reimplementing them.

Conceptually:

1. validate TakeId and current Take disposition;
2. require supplied `authorityEvaluation` to expose the current Patch 0010 contract and structurally initialized Trace/Input/Policy/ReviewSet collections sufficient for safe canonical replay;
3. reconstruct a fresh Patch 0009 source:

```text
freshSource = StateInterpretationSource.Bind(
    sourceContext,
    performance,
    integrityEvaluation)
```

4. reconstruct a fresh Patch 0010 input using the exact Snapshot carried by the supplied evaluation Trace:

```text
freshAuthorityInput = StateAuthorityInput.Bind(
    authorityEvaluation.Trace.Input.Snapshot,
    freshSource,
    interpretationProposal)
```

5. deterministically re-evaluate Patch 0010 using the exact Policy and ReviewSet carried by the supplied evaluation Trace:

```text
freshAuthorityEvaluation = DeterministicStateAuthority.Evaluate(
    freshAuthorityInput,
    authorityEvaluation.Trace.Policy,
    authorityEvaluation.Trace.ReviewSet)
```

6. require the **fresh** evaluation to be terminal Complete;
7. enforce Patch 0011 disposition-specific rules against the fresh ordered Decisions;
8. store the exact Performance, exact InterpretationProposal, and fresh canonical StateAuthorityEvaluation in the new E0Take.

The supplied StateAuthorityEvaluation is the upstream carrier of the Patch 0010 Trace inputs needed for verification. Patch 0011 does not trust or copy its caller-supplied `Status` or `Decisions` as independent authority; the deterministic replay result is canonical for the Take. Under normal canonical Patch 0010 construction, supplied and fresh semantics are identical. If an internally forged/inconsistent evaluation carries a valid replayable Trace but fabricated Status/Decisions, those fabricated fields cannot influence Take semantics because they are discarded in favor of fresh Patch 0010 output.

This replay is verification, not a new authority layer. It prevents a Take from binding a Performance/proposal package to stale/mismatched trace configuration while avoiding a second State Authority implementation or a bespoke evaluation-equality algorithm.

Patch 0011 does not rerun any semantic Integrity assessor, call any provider, or reinterpret Performance prose.

## 11. Terminal State Authority requirement

A Take may be created only when the fresh Patch 0010 replay is terminal:

```text
StateAuthorityEvaluation.Status == Complete
```

and no fresh decision has disposition:

```text
RequiresReview
```

`RequiresReview` means the consequence decision contract is incomplete. It cannot be treated as approved consequence authority and cannot enter a Take.

A complete fresh evaluation may contain any combination of:

```text
Approved
Rejected
```

including:

- zero mutations / zero decisions;
- all Approved;
- mixed Approved and Rejected;
- all Rejected.

All of those are fully evaluated and may bind as `Rejected` or `Alternate` Takes.

For E0 `Accepted`, an additional conservative rule applies:

```text
Accepted Take
    -> zero StateAuthorityDisposition.Rejected decisions
```

Therefore an Accepted Take has either zero proposed mutations or an all-Approved terminal State Authority decision set.

## 12. Rejected consequence != semantic proof that Performance is invalid

Patch 0011 preserves the authority split:

```text
Integrity Validator
    decides whether Candidate Performance can progress

State Authority
    decides whether each Interpreter-proposed consequence is approved

Take disposition
    decides whether the fully evaluated Performance package is selected for Production history, rejected, or retained as an alternate
```

A State Authority `Rejected` mutation does not itself prove that the Performance prose is invalid. It proves only that the proposed consequence cannot become approved consequence authority under the evaluated Patch 0010 inputs.

However, E0 currently has no separate deterministic semantic reconciler that can prove a Performance remains causally coherent after discarding one of its Interpreter-proposed consequences. Blueprint 0.1 requires fail-closed integrity and permits E0 inefficiency. Patch 0011 therefore adopts the conservative E0 rule:

```text
any Rejected State Authority decision
    -> Accepted disposition prohibited
```

Such a fully evaluated package may still be preserved as `Rejected` or `Alternate` E0 provenance. This does **not** claim the Performance itself was necessarily semantically bad; it means the package is not safe for the E0 accepted-history path under the current evidence architecture.

This rule is deliberately E0-scoped. It does not close ODR-19 or require the final product to reject an otherwise valuable Performance whenever one optional Interpreter proposal is declined. A later richer consequence-acceptance/reconciliation contract may earn selective acceptance only if it preserves causal coherence without hidden rewriting.

Historical texture remains fully supported through the empty-proposal case: a valid Performance with `Mutations = []` can be Accepted and later committed as history with no durable projected-state mutation.

The empty-proposal rule is **not** an escape hatch for an omitted required consequence. An empty proposal is safe only when the Performance does not require a durable projected-state change for transcript/current-authority coherence. If an E0 hard-gate audit or failure-injection case demonstrates that the Interpreter omitted a consequence required by the accepted Performance, that run is invalid and must be repaired before it contributes experiential evidence. Patch 0011 does not solve semantic completeness by inventing a hidden second prose reviewer.

Rejected mutation text never becomes authoritative consequence merely because the package is retained.

If a Performance itself violates locked authority or impossible world law, that belongs to the Integrity boundary; Patch 0011 does not create a hidden second prose-reviewer.

## 13. E0 Take disposition

The exact enum is:

```text
E0TakeDisposition
- Unspecified = 0
- Accepted = 1
- Rejected = 2
- Alternate = 3
```

`Unspecified` is not a valid Take disposition and `E0Take.Bind` must reject it. Undefined numeric values also fail closed.

This ordering is deliberate: `default(E0TakeDisposition)` must never mean Accepted, Rejected, or Alternate. Acceptance authority cannot arise from a zero-initialized enum.

Only `Accepted`, `Rejected`, and `Alternate` describe a fully evaluated, Integrity-cleared Take.

They do not replace Patch 0008 dispositions:

```text
IntegrityDisposition.Accept
IntegrityDisposition.Reject
IntegrityDisposition.RequestAnotherTake
```

The Take disposition is explicit typed orchestration input. It is never inferred from Candidate prose, Interpreter text, provider/model output, State Authority reason wording, sentiment, confidence, hidden reasoning, random choice, or an uninitialized/default enum.

Disposition-specific structural rules are deterministic:

- `Accepted` requires fresh terminal Complete authority with zero Rejected decisions;
- `Rejected` may retain any fresh terminal Complete decision set;
- `Alternate` may retain any fresh terminal Complete decision set;
- `Unspecified` or undefined values create no Take.

## 14. Accepted

`Accepted` means:

> This exact fully evaluated Performance package has been selected as the Take that may proceed to the later atomic causal-commit boundary, and every Interpreter-proposed consequence in the package is either absent because the proposal is empty or Approved by fresh terminal State Authority.

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

> This exact Integrity-cleared, interpreted, fully State-Authority-evaluated Performance package is not selected for Production history.

This may occur because the reference E0 package contains one or more rejected consequence decisions, or because an explicitly labeled higher-level E0 test/intervention rejects an otherwise all-Approved package.

It is distinct from Patch 0008 Integrity Reject and does not assert fictional blame or necessarily prove the Performance itself was semantically invalid.

A Rejected Take:

- never enters Production history;
- never applies Approved consequences despite those decisions being preserved as non-effective evaluated provenance;
- never updates recent-performance context;
- never changes effective opportunity history;
- never establishes Current Opportunity;
- never triggers Director progression;
- never becomes Observation;
- remains E0 diagnostic/provenance material.

## 16. Alternate

`Alternate` means:

> Preserve this exact fully evaluated Take package as a noncanonical alternative for E0 comparison or later explicit creative use.

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
- State Authority structural/replay failure;
- fresh State Authority evaluation with RequiresReview;
- malformed/mismatched State Authority Trace inputs;
- Unspecified/undefined Take disposition.

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

Proposal 0.9 removes it.

Reason:

- Patch 0010 already supplies immutable State Authority Trace inputs and deterministic evaluation semantics;
- Patch 0011 can re-bind/replay those inputs and retain the fresh canonical evaluation;
- a second content-identity contract would add an evolution axis before persistence/serialization demonstrates a need for it;
- future Patch 0012 Commit identity may bind exact Take/evaluation semantics under its own causal-commit contract.

No hash should be introduced merely to avoid retaining an already-immutable deterministic semantic object.

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
    -> identity of one Take decision/package occurrence within its supplied orchestration identity scope

CommitId
    -> future atomic causal-commit identity

StateHash
    -> future authoritative Production-state identity
```

TakeId cannot be inferred from any content hash.

Distinct TakeIds may legitimately contain semantically identical CandidatePerformance / Interpretation content.

## 21. State freshness remains Patch 0012

Patch 0011 binds the exact Snapshot/Policy/ReviewSet trace configuration used to freshly reproduce the Take's State Authority evaluation.

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
- Take-disposition human/creator identity;
- global TakeId uniqueness.

This preserves the prior contracts rather than pretending typed objects are credentials.

Before an Accepted Take becomes effective Production history, later E0 orchestration / atomic commit must retain and authenticate whatever configured provider/review/policy provenance the effective run relies upon.

A hash or immutable object association is not authentication.

Rejected/Alternate Takes and synthetic tests may use synthetic provenance without creating Production authority.

### E0 Take provenance obligation

The Core Take deliberately does not absorb Run/provider/request/assessor provenance, but effective E0 run records must make each Take attributable within the frozen experimental record.

For every created E0Take, later Harness/orchestration provenance must retain enough information to associate at minimum:

- RunId;
- TakeId;
- Take disposition;
- Take disposition source: deterministic reference mapping or explicitly labeled intervention/test/control;
- the intervention/exception label when the disposition did not come from the reference mapping;
- source ContextPacketId;
- Candidate content identity/hash;
- Interpreter proposal identity/hash as established by Patch 0010 binding;
- fresh State Authority ordered decisions/reasons and the policy/review provenance required by the run;
- provider/attempt/context-disclosure provenance required by the configured E0 path.

Within one Run, the run-orchestration/provenance gate must reject duplicate TakeIds rather than silently alias two Take occurrences.

A disposition that deviates from the deterministic reference mapping without the required explicit intervention/test/control attribution is an **E0 experimental-provenance failure**. The Core Take may still be structurally non-effective, but that run may not contribute ordinary reference-comparison evidence until the provenance defect is corrected or the run is rerun with the deviation explicitly labeled.

For an Accepted Take that successfully commits, the later causal-commit record must associate its CommitId with that TakeId.

For Rejected, Alternate, failed-before-Take, or Accepted-but-failed-commit material, provenance must preserve the explicit **absence of an effective causal commit** rather than allocate or fabricate a CommitId that implies Production history.

This supplements Blueprint 0.1's existing E0 provenance requirements without turning diagnostic provenance into Production history.

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

## 26. E0 reference disposition policy

E0 is an architecture experiment and must not add unrecorded subjective curation that changes transcripts between comparison variants.

Therefore the **reference E0 orchestration policy** is deterministic:

```text
Take-bindable package
+ fresh terminal authority with zero Rejected decisions
    -> Accepted

Take-bindable package
+ fresh terminal authority with one or more Rejected decisions
    -> Rejected
```

`Alternate` is never chosen implicitly. It requires a separately labeled E0 test/control or explicit creator/experiment intervention preserved in provenance.

An explicitly labeled intervention may reject an otherwise Accepted-eligible package or retain it as Alternate. It may not override Patch 0011's E0 prohibition on `Accepted` when any State Authority decision is Rejected.

The Take disposition is never model-authored.

This reference mapping is an E0 orchestration rule, not a reason to introduce a second Core policy class, mode enum, or reference-disposition service in Patch 0011. Core only enforces the disposition invariants at `E0Take.Bind`; later Harness orchestration applies the frozen mapping when it owns a real run loop.

This E0 reference policy is not the final product consequence-acceptance UX and does not close ODR-19 (`Autopilot`, `Review`, `Strict Creator`, or another model).

The purpose is experimental isolation and fail-closed causal integrity: ordinary E0 reference runs expose Performer/architecture behavior without silently selecting only preferred lines and without accepting a package whose own consequence review contains rejected proposals.

## 27. E0 experimental isolation

For ordinary E0-A/C/D/G per-Character reference runs that reach this contract:

- Take semantics remain identical across compared variants unless Take behavior itself is the named variable;
- zero-Rejected terminal packages become Accepted under reference orchestration;
- any-Rejected terminal packages become Rejected under reference orchestration;
- Alternate interventions must be labeled and attributable;
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

Patch 0011 introduces no background execution or polling. Therefore no idle work is attributable to this Core contract by design; this is not target-device power evidence.

## 29. Fail closed and exception-domain normalization

Patch 0011 defines:

```text
E0TakeException
```

as the public expected contract-failure exception domain for `E0Take.Bind`.

Failures include at minimum:

- null/missing inputs;
- uninitialized TakeId;
- Unspecified/undefined Take disposition;
- unsupported supplied State Authority evaluation contract;
- malformed supplied State Authority Trace/Input/Policy/ReviewSet required for safe replay;
- Context/Candidate/Integrity mismatch;
- Integrity disposition other than Accept;
- malformed InterpretationProposal;
- fresh State Authority re-bind/replay failure;
- fresh State Authority status ReviewRequired;
- any fresh RequiresReview decision;
- Accepted disposition paired with any fresh Rejected State Authority decision;
- malformed fresh decision count/order.

`E0Take.Bind` must normalize expected upstream contract failures crossing its public boundary:

- `StateInterpretationException` from canonical source re-binding;
- `StateAuthorityException` from fresh State Authority input binding/evaluation;
- `InvalidOperationException` encountered only while safely reading an uninitialized supplied strong ID/property that Patch 0011 itself is validating.

For `StateInterpretationException` and `StateAuthorityException`, the exact upstream domain exception is retained as `InnerException`; those current upstream contracts contain sanitized structural messages. The public `E0TakeException.Message` is a new sanitized Take-boundary message and does not concatenate upstream message text.

A Take-owned `InvalidOperationException` encountered while checking an uninitialized supplied strong ID/property is normalized to a sanitized `E0TakeException` **without** retaining that runtime exception as an inner exception.

Patch 0011 copies no exception `Data` entries. Raw Candidate text, mutation Text, Context prose, unknown payload snippets, provider content, credentials, or arbitrary user text must not appear anywhere in the public Take exception representation (`Message`, retained upstream inner chain/data, or `ToString()`).

Patch 0011 must **not** catch and relabel arbitrary unexpected programming/runtime failures as ordinary Take rejection. Unexpected failures create no Take and remain technical failures for higher-level diagnostics.

Failure creates no Take and no fallback disposition.

## 30. Required implementation tests

Future implementation should prove at minimum:

1. existing `TakeId` strong type is reused; no new Take ID type exists;
2. TakeId required/initialized;
3. `E0TakeDisposition.Unspecified == 0`, Accepted/Rejected/Alternate are explicit nonzero values, and default/undefined disposition fails closed;
4. `E0Take` is sealed, its constructor is private, and `Bind` is its sole construction path;
5. Take contract/disposition exact and defined;
6. exact CandidatePerformance object is retained;
7. exact InterpretationProposal object is retained;
8. supplied StateAuthorityEvaluation Status/Decisions are not independently trusted; fresh canonical Patch 0010 replay is retained;
9. unsupported/malformed supplied State Authority trace configuration fails safely;
10. Integrity Reject cannot bind a Take;
11. Integrity RequestAnotherTake cannot bind a Take;
12. technical/provider failure has no Take construction path;
13. mismatched Context/Candidate fails;
14. mismatched Integrity evaluation fails;
15. proposal Candidate identity mismatch fails;
16. proposal Scene mismatch fails;
17. mismatched State Authority ReviewSet/proposal identity fails during fresh replay;
18. fresh State Authority ReviewRequired cannot bind;
19. any fresh RequiresReview decision cannot bind;
20. empty mutation proposal + Complete authority can bind Accepted;
21. all-Approved decision set can bind Accepted;
22. mixed Approved/Rejected decision set cannot bind Accepted;
23. all-Rejected decision set cannot bind Accepted;
24. mixed/all-Rejected Complete decision sets can bind Rejected;
25. mixed/all-Rejected Complete decision sets can bind Alternate;
26. Alternate is never inferred by Core from authority decisions;
27. Accepted Take applies no State and writes no history;
28. Rejected Take applies no State/history/Director routing;
29. Alternate Take applies no State/history/Director routing;
30. Rejected/Alternate Take cannot make Candidate control effective;
31. Accepted-but-uncommitted Take cannot make Candidate control effective;
32. failed commit leaves Accepted Take outside Production history and preserves only required E0 provenance;
33. identical semantic Candidate/Proposal packages may bind under distinct TakeIds;
34. TakeId is not equal/derived by contract from CandidateContentHash or ProposalContentHash;
35. Core defines no TakeId allocator/format beyond existing canonical strong-ID validation;
36. Take aggregate exposes no RunId/provider/model/confidence/rationale/chain-of-thought field;
37. Take is immutable and exposes no disposition mutation API;
38. repeated Bind over identical semantic inputs and TakeId/disposition produces equivalent Take semantics;
39. expected upstream StateInterpretation/StateAuthority contract failures normalize to E0TakeException with the expected sanitized upstream domain exception retained as InnerException;
40. Take-owned uninitialized-strong-ID InvalidOperationException is normalized without retaining that runtime exception;
41. Take exception messages/inner chains/data/ToString remain sanitized and do not expose Candidate/Context/mutation prose or arbitrary user/provider content;
42. unexpected runtime/programming failures are not converted into a Take disposition;
43. fixed Missing Raft Context/Candidate/State Authority regression identities remain unchanged;
44. full Core regression suite remains green;
45. Missing Raft Harness regression remains green;
46. generic smoke Harness regression remains green.

The reference disposition mapping, per-Run TakeId uniqueness, disposition-source attribution, and E0 Take provenance obligations are later Harness/orchestration invariants; Patch 0011 Core need not add implementation surfaces merely to restate them.

Patch 0011 implementation must add no tests that falsely claim ProductionState, persistence, provider authentication, global TakeId allocation, atomic commit, or runtime/hardware behavior.

## 31. Explicit non-goals

Patch 0011 does not implement or freeze:

- new TakeId type;
- global/durable TakeId allocator or RunId-derived TakeId format;
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

> A CandidatePerformance is the provisional generated Performance. An E0 Take exists only after that Performance has passed Integrity and its interpreted consequences have reached a terminal deterministic State Authority replay. The Take preserves the exact Performance, exact consequence proposal, fresh canonical authority evaluation, and an explicit non-default Accepted / Rejected / Alternate disposition under a distinct supplied TakeId. E0 Accepted requires zero rejected consequence decisions and means selected for atomic commit, not already historical. Only later successful atomic commit makes the accepted Performance and all Approved consequences effective together.

## 33. Approval / implementation gate

This blueprint is architecture only.

Before implementation:

1. recursively adversarial-audit Proposal 0.9 against frozen Blueprint 0.1, approved Patches 0006–0010, current source/tests, engineering hygiene, E0 experiment isolation, provenance boundaries, exception boundaries, invalid-state construction, causal-coherence limitations, and future Patch 0012 separation;
2. restart the audit after every material correction;
3. require one complete final pass with zero material corrections and zero worthwhile architectural improvements;
4. obtain explicit user approval;
5. create a fresh-chat implementation handoff;
6. do not write Patch 0011 executable code in the architecture chat.
