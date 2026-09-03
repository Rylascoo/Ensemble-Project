# H1 Patch 0011 — E0 Take Semantics Contract

Status: blueprint proposal 0.14 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
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
    -> fully evaluated package not selected for later atomic commit into Production history

E0Take Alternate
    -> fully evaluated noncanonical alternative retained for E0 provenance

successful atomic commit of Accepted Take
    -> effective accepted historical Performance + approved consequences
```

These terms must not collapse.

## 5. Contract and namespace

Patch 0011 owns the E0-only subsystem namespace:

```text
Ensemble.E0.Core.Take
```

The canonical semantic contract holder is:

```csharp
public static class E0TakeContracts
{
    public const string ContractVersion = "ensemble.e0.take.v1";
}
```

`E0Take.ContractVersion` is exactly `E0TakeContracts.ContractVersion`.

Patch 0011 does not freeze physical `.cs` file grouping. The implementation should use the smallest coherent file layout consistent with the existing Core subsystem pattern rather than creating speculative abstractions.

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

`E0Take.Bind` therefore receives TakeId from higher-level orchestration and validates only that it is initialized.

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

`E0Take` has a **private constructor**. `E0Take.Bind` is its sole construction path. Patch 0011 introduces no public or internal alternate constructor/factory that can bypass association replay, terminal-authority checks, disposition validation, or Take invariants.

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

The Take object does not retain `ContextPacket`, `IntegrityValidationEvaluation`, or `StateInterpretationSource`; those remain separate E0 source/provenance semantics.

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
7. store the exact Performance, exact InterpretationProposal, and fresh canonical StateAuthorityEvaluation in the new E0Take with the explicit validated disposition.

The supplied StateAuthorityEvaluation is the upstream carrier of the Patch 0010 Trace inputs needed for verification. Patch 0011 does not trust or copy its caller-supplied `Status` or `Decisions` as independent authority; the deterministic replay result is canonical for the Take. Under normal canonical Patch 0010 construction, supplied and fresh semantics are identical. If an internally forged/inconsistent evaluation carries a valid replayable Trace but fabricated Status/Decisions, those fabricated fields cannot influence Take semantics because they are discarded in favor of fresh Patch 0010 output.

This replay is verification, not a new authority layer. It prevents a Take from binding a Performance/proposal package to stale/mismatched trace configuration while avoiding a second State Authority implementation or a bespoke evaluation-equality algorithm.

Patch 0011 does not rerun any semantic Integrity assessor, call any provider, reinterpret Performance prose, or infer Take disposition from consequence decisions.

### Source Context / State Authority snapshot identity limit

Patch 0011 does **not** claim that the State Authority snapshot carried by the supplied evaluation is cryptographically or durably proven to be the exact authoritative Production state from which `sourceContext` was composed.

Current boundaries establish different identities:

```text
ContextPacketId
    -> exact structured bounded Character context identity

StateAuthoritySnapshot
    -> structural authority projection used by Patch 0010 evaluation
```

There is no `ProductionState` / `StateHash` identity yet that binds those two artifacts to one evolved authoritative state instance.

For the currently validated Patch 0010 executable scope, State Authority snapshots are fixture-derived initial snapshots. Effective E0 orchestration must source Context composition and State Authority evaluation coherently from the same frozen fixture/current-authority source and preserve the fixture/run provenance that demonstrates that orchestration relationship.

That orchestration provenance is not upgraded into cryptographic Core proof by `E0Take.Bind`. Once evolved multi-turn Production state exists, the later state/commit boundary must introduce the authoritative current-state identity/freshness semantics required to prove the relationship. Patch 0011 must not invent a substitute StateHash, fixture alias, or misleading cross-object hash merely to hide this known boundary.

## 11. Terminal State Authority requirement

A Take may be created only when the fresh Patch 0010 replay is terminal:

```text
StateAuthorityEvaluation.Status == Complete
```

and no fresh decision has disposition:

```text
RequiresReview
```

`RequiresReview` means the consequence decision contract is incomplete. It cannot enter a Take as if consequence authority were already resolved.

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

All of those are terminally evaluated consequence packages. Any may bind under an explicit `Accepted`, `Rejected`, or `Alternate` Take disposition, subject to the same structural association checks.

## 12. Take disposition and consequence disposition are independent authorities

Patch 0011 preserves the frozen authority split:

```text
Integrity Validator
    decides whether Candidate Performance can progress

State Authority
    decides whether each Interpreter-proposed consequence may commit

Take disposition
    decides whether the fully evaluated Performance package is selected for later atomic commit, rejected, or retained as an alternate
```

A State Authority `Rejected` mutation does **not** reject the Performance and does not choose the Take disposition. It means only that the proposed mutation is excluded from authoritative consequence commit under the evaluated Patch 0010 inputs.

Therefore structurally valid E0 outcomes include:

```text
Accepted Take + zero proposed durable mutations
Accepted Take + all Approved consequence decisions
Accepted Take + mixed Approved / Rejected consequence decisions
Accepted Take + all Rejected consequence decisions
```

For an Accepted Take, the later atomic causal-commit boundary must commit the exact Performance plus **every Approved consequence and no Rejected consequence**. Rejected proposals remain non-effective provenance with their reasons.

This separation matters experimentally: the State Interpreter is a proposal mechanism. Allowing an over-proposed or optional rejected mutation to veto the Performance would silently promote Interpreter/State Authority consequence review into Performer-acceptance authority, contrary to the frozen non-overlapping roles.

This does not excuse causal incoherence. If an accepted Performance necessarily entails a durable consequence that is omitted, rejected, or otherwise absent such that transcript/current-authority coherence would be false, the E0 run violates the frozen hard gates and cannot contribute experiential evidence until the responsible Integrity / interpretation-completeness / State Authority / atomic-commit defect is diagnosed and repaired. Patch 0011 does not invent a hidden second prose reviewer to guess that condition.

Historical texture remains fully supported through the empty-proposal case: a valid Performance with `Mutations = []` can be Accepted and later committed as history with no durable projected-state mutation.

The empty-proposal rule is **not** an escape hatch for an omitted required consequence. An empty proposal is safe only when the Performance does not require a durable projected-state change for transcript/current-authority coherence.

If a Performance itself violates locked authority or impossible world law, that belongs to the Integrity boundary; Patch 0011 does not convert a rejected mutation into a substitute Integrity verdict.

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

They do not replace Patch 0008 dispositions or Patch 0010 dispositions:

```text
IntegrityDisposition.Accept
IntegrityDisposition.Reject
IntegrityDisposition.RequestAnotherTake

StateAuthorityDisposition.Approved
StateAuthorityDisposition.Rejected
StateAuthorityDisposition.RequiresReview
```

The Take disposition is explicit typed orchestration input. It is never inferred from Candidate prose, Interpreter text, provider/model output, State Authority dispositions/reason wording, sentiment, confidence, hidden reasoning, random choice, or an uninitialized/default enum.

Disposition-specific structural rules are deterministic:

- `Accepted` may retain any fresh terminal Complete consequence decision set;
- `Rejected` may retain any fresh terminal Complete consequence decision set;
- `Alternate` may retain any fresh terminal Complete consequence decision set;
- `Unspecified` or undefined values create no Take.

## 14. Accepted

`Accepted` means:

> This exact fully evaluated Performance package has been selected as the Take that may proceed to the later atomic causal-commit boundary.

It does **not** mean:

- Production history has already changed;
- Performance is already visible as authoritative history;
- Approved mutations have been applied;
- Rejected mutations have become true;
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

Only successful Patch 0012 atomic causal commit makes the accepted Performance and all Approved consequences effective together. Rejected consequence proposals remain non-effective provenance.

If that commit fails, neither Performance history nor consequence state becomes effective. The failed attempt and its Accepted-but-uncommitted Take remain E0 diagnostic/provenance material where the frozen run protocol requires them; they never masquerade as Production history.

## 15. Rejected

`Rejected` means:

> This exact Integrity-cleared, interpreted, fully State-Authority-evaluated Performance package is not selected for later atomic commit into Production history.

It is distinct from Patch 0008 Integrity Reject and independent of whether Patch 0010 Approved or Rejected individual consequence proposals.

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

Once a Take is created, its retained Performance, InterpretationProposal, AuthorityEvaluation, TakeId, and disposition are one immutable semantic package.

Core Patch 0011 is stateless and therefore cannot detect reuse of the same supplied TakeId across separate `Bind` calls. Effective per-Run orchestration/provenance must reject duplicate TakeIds; later durable identity authority may impose stronger uniqueness.

Accepted historical corrections, retcons, branches, alternate-history promotion, or canon changes must later create explicit new authority/history rather than rewriting a prior Take.

This preserves Blueprint 0.1 accepted-Take immutability without freezing post-E0 ODR-20 UX.

## 19. No new authority-decision content hash in Patch 0011

Proposal 0.1 considered introducing an `AuthorityDecisionContentHash`.

Proposal 0.14 removes it.

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

## 21. State freshness remains Patch 0012; Take semantics cannot be silently replaced

Patch 0011 binds the exact Snapshot/Policy/ReviewSet trace configuration used to freshly reproduce the Take's State Authority evaluation.

It does not claim that the underlying StateAuthoritySnapshot is still current at commit time.

No StateHash/stale-state commit protocol exists yet.

Therefore `Accepted` means commit-eligible under the evaluated authority package only.

Patch 0010 already leaves the later commit boundary free to prove freshness by binding an evaluation to authoritative current-state identity or by deterministic re-evaluation against then-current authority. Patch 0012 owns that mechanism.

However, freshness verification must not silently rewrite this Take:

- `E0Take.AuthorityEvaluation` remains immutable;
- a commit under this Take may apply only consequences whose corresponding retained Take decision is `Approved`;
- a retained `Rejected` consequence may not become effective merely because later state would now permit it;
- an originally `Approved` consequence may not be silently omitted from a successful commit under this Take merely because later state would now reject it;
- if current-state freshness handling would require a different effective Approved/Rejected consequence set, the existing Take cannot be repurposed as though it contained that new package.

In that case the atomic commit must fail closed under the existing Take, or later explicitly approved orchestration must create a new evaluated Take/causal path with its own identity semantics. Patch 0011 does not freeze the post-E0 UX for that later path.

This is not Patch 0012 implementation. It is the minimum consequence of immutable Take semantics plus Blueprint 0.1's law that an accepted Performance and **all approved consequences** enter causal history together or neither does.

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
- global TakeId uniqueness;
- source Context / State Authority snapshot common-state identity beyond the structural associations presently available.

This preserves the prior contracts rather than pretending typed objects are credentials.

Before an Accepted Take becomes effective Production history, later E0 orchestration / atomic commit must retain and authenticate whatever configured provider/review/policy/current-state provenance the effective run relies upon.

A hash or immutable object association is not authentication.

Synthetic canonical objects may be used to construct any disposition, including `Accepted`, in deterministic Core tests. Synthetic provenance never authorizes Production history by itself. Rejected/Alternate Takes likewise remain non-effective regardless of whether their provenance is real or synthetic.

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
- fixture/current-authority provenance sufficient for the configured E0 path to explain why the source Context and State Authority snapshot belonged to the same run-state source;
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

Therefore the **reference E0 orchestration policy** is:

```text
Take-bindable package
+ no separately labeled explicit Take-disposition intervention
    -> Accepted
```

State Authority Approved/Rejected consequence decisions do not choose this disposition. Their role remains consequence authority only.

`Rejected` or `Alternate` Take dispositions may be supplied only by:

- a separately labeled E0 test/failure/control case;
- an explicit creator/experiment intervention whose presence is preserved in provenance;
- a future approved orchestration mode outside the ordinary E0 reference comparison.

`Alternate` is never chosen implicitly.

The Take disposition is never model-authored.

This reference mapping is an E0 orchestration rule, not a reason to introduce a second Core policy class, mode enum, or reference-disposition service in Patch 0011. Core only validates the explicit disposition at `E0Take.Bind`; later Harness orchestration applies the frozen mapping when it owns a real run loop.

This E0 reference policy is not the final product consequence-acceptance UX and does not close ODR-19 (`Autopilot`, `Review`, `Strict Creator`, or another model).

The purpose is experimental isolation: ordinary E0 reference runs expose Performer/architecture behavior rather than silently selecting only the lines a human reviewer happens to prefer or letting Interpreter over-proposal become hidden Performance curation.

## 27. E0 experimental isolation

For ordinary E0-A/C/D/G per-Character reference runs that reach this contract:

- Take semantics remain identical across compared variants unless Take behavior itself is the named variable;
- every Take-bindable package becomes Accepted under reference orchestration unless a separately labeled explicit intervention applies;
- Rejected/Alternate interventions must be labeled and attributable;
- provider/model identity does not alter Take authority;
- State Authority consequence dispositions do not alter Take disposition;
- Take semantics do not inspect provider/model identity.

E0-B may vary Performer assignment without varying Take semantics.

E0-E single-playwright control is not forced through the per-Character H1 Candidate/Take API unless a separately approved control-compatible binding defines equivalent causal acceptance/provenance semantics.

E0-F must prove technical failure, malformed output, Integrity Reject/RequestAnotherTake, unresolved State Authority review, Rejected Take, Alternate Take, and failed commit cannot enter Production history. It must also detect any transcript/current-authority incoherence caused by a missing or rejected consequence required by an accepted Performance.

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

Patch 0011 defines a publicly catchable but subsystem-constructed expected contract-failure domain:

```csharp
public sealed class E0TakeException : Exception
{
    internal E0TakeException(string message)
        : base(message)
    {
    }

    internal E0TakeException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
```

`E0TakeException` has **no public constructor**. Callers can catch/inspect the typed failure, but only the Take subsystem can create canonical Take-boundary exceptions. This prevents arbitrary external code from manufacturing unsanitized exceptions that falsely present themselves as output from the Take contract.

`E0TakeException` is the public expected contract-failure exception emitted by `E0Take.Bind`.

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
- malformed fresh decision count/order.

`E0Take.Bind` must normalize expected upstream contract failures crossing its public boundary:

- `StateInterpretationException` from canonical source re-binding;
- `StateAuthorityException` from fresh State Authority input binding/evaluation;
- `InvalidOperationException` encountered only while safely reading an uninitialized supplied strong ID/property that Patch 0011 itself is validating.

For `StateInterpretationException` and `StateAuthorityException`, the exact upstream domain exception is retained as `InnerException`; those current upstream contracts contain sanitized structural messages. The public `E0TakeException.Message` is a new sanitized Take-boundary message and does not concatenate upstream message text.

A Take-owned `InvalidOperationException` encountered while checking an uninitialized supplied strong ID/property is normalized to a sanitized `E0TakeException` **without** retaining that runtime exception as an inner exception.

Patch 0011 copies no exception `Data` entries. For exceptions emitted by `E0Take.Bind`, raw Candidate text, mutation Text, Context prose, unknown payload snippets, provider content, credentials, or arbitrary user text must not appear anywhere in the public Take exception representation (`Message`, retained upstream inner chain/data, or `ToString()`).

Patch 0011 must **not** catch and relabel arbitrary unexpected programming/runtime failures as ordinary Take rejection. Unexpected failures create no Take and remain technical failures for higher-level diagnostics.

Failure creates no Take and no fallback disposition.

## 30. Required implementation tests

Future implementation should prove at minimum:

1. Patch 0011 public types live under `Ensemble.E0.Core.Take`;
2. `E0TakeContracts.ContractVersion` exists and equals exactly `ensemble.e0.take.v1`;
3. existing `TakeId` strong type is reused; no new Take ID type exists;
4. TakeId required/initialized;
5. `E0TakeDisposition.Unspecified == 0`, Accepted/Rejected/Alternate are explicit nonzero values, and default/undefined disposition fails closed;
6. `E0Take` is sealed, its constructor is private, and `Bind` is its sole construction path;
7. `E0TakeException` is sealed, publicly catchable, has no public constructors, and is constructed only through non-public Take-subsystem constructors;
8. `E0Take.ContractVersion == E0TakeContracts.ContractVersion`;
9. exact CandidatePerformance object is retained;
10. exact InterpretationProposal object is retained;
11. supplied StateAuthorityEvaluation Status/Decisions are not independently trusted; fresh canonical Patch 0010 replay is retained;
12. unsupported/malformed supplied State Authority trace configuration fails safely;
13. Integrity Reject cannot bind a Take;
14. Integrity RequestAnotherTake cannot bind a Take;
15. technical/provider failure has no Take construction path;
16. mismatched Context/Candidate fails;
17. mismatched Integrity evaluation fails;
18. proposal Candidate identity mismatch fails;
19. proposal Scene mismatch fails;
20. mismatched State Authority ReviewSet/proposal identity fails during fresh replay;
21. fresh State Authority ReviewRequired cannot bind;
22. any fresh RequiresReview decision cannot bind;
23. empty mutation proposal + Complete authority can bind Accepted;
24. all-Approved Complete decision set can bind Accepted;
25. mixed Approved/Rejected Complete decision set can bind Accepted;
26. all-Rejected Complete decision set can bind Accepted;
27. all-Approved Complete decision set can bind Rejected;
28. all-Approved Complete decision set can bind Alternate;
29. mixed/all-Rejected Complete decision sets can bind Rejected;
30. mixed/all-Rejected Complete decision sets can bind Alternate;
31. State Authority Approved/Rejected decisions never infer or constrain a terminal Take disposition;
32. Accepted Take applies no State and writes no history;
33. Rejected Take applies no State/history/Director routing;
34. Alternate Take applies no State/history/Director routing;
35. Rejected/Alternate Take cannot make Candidate control effective;
36. Accepted-but-uncommitted Take cannot make Candidate control effective;
37. Take public surface exposes no CommitId, committed/history flag, ProductionState, StateHash, mutation-application API, persistence API, or effective-opportunity authority;
38. Take public surface does not retain ContextPacket, IntegrityValidationEvaluation, StateInterpretationSource, RunId, provider/model/confidence/rationale, or chain-of-thought fields;
39. identical semantic Candidate/Proposal packages may bind under distinct TakeIds;
40. TakeId is not equal/derived by contract from CandidateContentHash or ProposalContentHash;
41. Core defines no TakeId allocator/format beyond existing canonical strong-ID validation;
42. Take is immutable and exposes no disposition or retained-package mutation API;
43. repeated Bind over identical semantic inputs and TakeId/disposition produces equivalent Take semantics;
44. expected upstream StateInterpretation/StateAuthority contract failures normalize to E0TakeException with the expected sanitized upstream domain exception retained as InnerException;
45. Take-owned uninitialized-strong-ID InvalidOperationException is normalized without retaining that runtime exception;
46. Take exception messages/inner chains/data/ToString from `E0Take.Bind` remain sanitized and do not expose Candidate/Context/mutation prose or arbitrary user/provider content;
47. unexpected runtime/programming failures are not converted into a Take disposition;
48. no Patch 0011 type claims a ProductionState/StateHash/common-state identity proof between ContextPacket and StateAuthoritySnapshot;
49. fixed Missing Raft Context/Candidate/State Authority regression identities remain unchanged;
50. full Core regression suite remains green;
51. Missing Raft Harness regression remains green;
52. generic smoke Harness regression remains green.

The reference disposition mapping, per-Run TakeId uniqueness, disposition-source attribution, failed-commit behavior, source-Context/snapshot common-state provenance, stale-state/freshness implementation, causal-coherence hard-gate behavior, and E0 Take provenance obligations are later Harness/atomic-commit/orchestration invariants; Patch 0011 Core need not add implementation surfaces or tests that claim those later capabilities already exist.

Patch 0011 implementation must add no tests that falsely claim ProductionState, persistence, provider authentication, global TakeId allocation, atomic commit, current-state identity proof, stale-state runtime handling, or runtime/hardware behavior.

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
- authoritative ContextPacket / StateAuthoritySnapshot common-state identity;
- authoritative new RecordId allocation;
- mutation application;
- CommitId allocation/composition;
- atomic causal commit;
- stale-state/freshness implementation;
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

> A CandidatePerformance is the provisional generated Performance. An E0 Take exists only after that Performance has passed Integrity and its interpreted consequences have reached a terminal deterministic State Authority replay. The Take preserves the exact Performance, exact consequence proposal, fresh canonical authority evaluation, and an explicit non-default Accepted / Rejected / Alternate disposition under a distinct supplied TakeId. Take disposition and consequence disposition remain independent authorities. Accepted means selected for atomic commit, not already historical. Only later successful atomic commit makes the accepted Performance and every Approved consequence effective together; Rejected consequence proposals remain non-effective provenance. Later freshness handling may validate the Take but may not silently substitute a different consequence package under the same immutable Take. Patch 0011 does not claim a Production-state identity proof that does not yet exist.

## 33. Approval / implementation gate

This blueprint is architecture only.

Before implementation:

1. recursively adversarial-audit Proposal 0.14 against frozen Blueprint 0.1, approved Patches 0006–0010, current source/tests, engineering hygiene, E0 experiment isolation, provenance boundaries, exception boundaries, invalid-state construction, source-state identity limits, causal-coherence limitations, immutable freshness boundaries, and future Patch 0012 separation;
2. restart the audit after every material correction;
3. require one complete final pass with zero material corrections and zero worthwhile architectural improvements;
4. obtain explicit user approval;
5. create a fresh-chat implementation handoff;
6. do not write Patch 0011 executable code in the architecture chat.
