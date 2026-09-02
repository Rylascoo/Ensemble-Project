# H1 Patch 0007 — E0 Director Opportunity Contract

Status: blueprint proposal 0.1 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0006
Branch: `h1-patch-0007-director-opportunity-blueprint`

## 1. Purpose

Define the next E0-A behavioral boundary after the validated Performer candidate contract:

```text
validated safe ContextPacket
+ structurally valid CandidatePerformance
+ effective opportunity history
    -> deterministic E0 Director proposal
        -> later causal acceptance/commit boundary
            -> effective Current Opportunity
                -> next Context Composer / Performer
```

Patch 0007 answers one narrow question:

> Given the Character-safe context associated with the current candidate, the candidate's narrow typed handoff metadata, and the already-effective opportunity history for the Scene, what next Character opportunity would the E0 reference Director propose without writing outcomes, inventing world reality, granting knowledge, using hidden prose interpretation, or acquiring authority before the current Performance causally commits?

Patch 0007 defines and, after approval, may implement a deterministic **E0 reference proposal function**. It does not make a proposal effective Current Opportunity and does not implement the later Take/Integrity/State/atomic-commit boundary that authorizes such an effect.

## 2. Authority basis

Frozen Blueprint 0.1 establishes:

- the Director manages attention and opportunity;
- its central question is “Whose agency becomes salient next, and why?”;
- the Director may consider hard eligibility, direct social address, Character nomination, current interaction relevance, relationship relevance, observable pressure, participation balance, and recent repetition;
- Blueprint 0.1 freezes no invented numeric weights;
- equal dialogue is not the goal;
- the Director should detect accidental exclusion, repetitive two-character ping-pong, and stalled attention without enforcing a turn quota;
- a handoff represents opportunity and social pressure, not obligation;
- the selected Character may speak, act, evade, redirect, refuse, or remain silent;
- the Director may not write a required line, force an outcome, grant inaccessible knowledge, create belief/truth, commit state, authorize spending/retries, or manufacture non-Character reality;
- World Resolver remains separate and unimplemented in E0;
- deterministic eligibility/cost/access rules may not be delegated to an LLM;
- every E0 run preserves Director opportunity inputs and decisions in provenance;
- E0-D requires deterministic round-robin opportunity order as a separately labeled ablation;
- ODR-12 intentionally leaves the **post-E0/final opportunity-selection mechanism** open: structured rules, local semantic judgment, or a hybrid.

Therefore Patch 0007 must freeze only the narrow provisional E0 mechanism required to run the experiment. It must not claim to resolve ODR-12 for the product after E0.

Validated Patch 0006 additionally establishes:

- `CandidatePerformance` is provisional semantic Performer output;
- its `Control` contains only optional `AddressedCharacterIds` and optional `NominatedCharacterId`;
- control is Performer intent metadata, not truth/state/observation/Director authority/history;
- provisional/uncommitted control cannot create effective opportunity, trigger another Performer, or survive rollback as routing state;
- discardable precommit Director computation is permissible only with zero effective authority/effect;
- control associated with a successfully committed accepted Performance may later become one non-binding Director input;
- if Director consumes such control for an effective opportunity, the semantic control input and resulting decision must be retained in causal/provenance records.

## 3. Scope decision: provisional E0 selector, not final Director

Blueprint 0.1 explicitly leaves the post-E0 Director mechanism open. Patch 0007 therefore freezes:

- one deterministic, auditable E0 reference proposal strategy;
- one semantic proposal contract;
- one local trace sufficient to reconstruct why the E0 proposal was produced;
- no post-E0 claim that this strategy is the final product Director.

The reference strategy is deliberately structured and model-free so E0 can isolate the Character/context/opportunity architecture without introducing another model/provider variable into the same-model reference condition.

This does **not** establish that a future product Director should remain structured-only. ODR-12 remains open after E0.

## 4. Opening opportunity is fixture authority, not a Director decision

The frozen Missing Raft fixture already establishes opening opportunity `VOSS`.

Patch 0007 does not re-select, reinterpret, or overwrite the opening opportunity.

The E0 Director proposal function is used only **after** a Character has received an effective opportunity and produced a structurally valid candidate.

The opening fixture opportunity must remain distinguishable in provenance from later Director-produced opportunities.

## 5. Public reference input

Preferred semantic input:

```text
ContextPacket currentContext
CandidatePerformance currentCandidate
DirectorOpportunityHistory opportunityHistory
```

`currentContext` supplies only Character-safe structural information needed by the reference selector:

- SceneId;
- SubjectCharacterId;
- OpportunityCharacterId;
- ContextPacketId;
- roster Character IDs.

`currentCandidate` supplies:

- SubjectCharacterId;
- ContextPacketId;
- typed control only.

The selector must not inspect `VisibleText` to derive hidden semantic meaning.

`opportunityHistory` is the ordered sequence of **effective Character opportunity IDs already established for this Scene**, including the opening fixture opportunity and any later effective Director opportunities.

Opportunity history is attention history, not dialogue quota, accepted Performance history, world truth, or transcript text.

## 6. Why VisibleText is not a Director input in E0 reference strategy

The frozen Director may eventually consider current interaction relevance. Patch 0007 does not infer that relevance from free-form candidate prose.

Reasons:

- Patch 0006 already provides explicit machine-legible address/nomination signals;
- free-form Performance is untrusted creative content;
- semantic interpretation would require another model or a new deterministic language ontology;
- introducing that mechanism now would add a confound before the E0 behavioral architecture is tested;
- hidden semantic parsing risks creating a covert authority path from prose into routing;
- ODR-12 intentionally leaves future structured/local-semantic/hybrid Director design open.

The E0 selector therefore uses candidate control plus opportunity history only. Future approved Director contracts may add bounded interaction/relationship/pressure relevance without changing what Patch 0007 proved.

## 7. Why relationship and pressure relevance are not reference inputs yet

Blueprint 0.1 says the Director **may** consider relationship relevance and observable pressure. It does not require every Director implementation to consume every possible signal.

Patch 0007 intentionally does not inspect:

- Character-private relationship records;
- full Production relationship state;
- Pressure text;
- candidate prose;
- WorldState;
- hidden truth;
- Access decisions;
- provenance graph.

The current deterministic spine does not yet provide a Director-specific, authority-safe relevance projection for those concepts, and inventing one inside this patch would conflate Director selection with state interpretation/access design.

This is an E0-reference simplification, not a claim that relationship/pressure relevance is unimportant.

## 8. E0 reference strategy contracts

Freeze:

```text
DirectorOpportunityContractVersion = ensemble.e0.director.opportunity.v1
DirectorStrategyContract = ensemble.e0.director.social-handoff-recency.v1
```

The semantic proposal and the strategy are separately named so later E0-D or post-E0 strategies do not need to redefine the proposal object merely because selection behavior changes.

Neither identifier grants state or truth authority.

## 9. Semantic proposal shape

```text
DirectorOpportunityProposal
- ContractVersion
- StrategyContract
- SceneId
- SourceCharacterId
- SourceContextPacketId
- SelectedCharacterId
- Basis
```

`Basis` is one of:

```text
Nomination
DirectAddress
RecencyFallback
```

The proposal contains no prose direction, line suggestion, emotional instruction, world event, pressure mutation, state mutation, confidence score, numeric salience score, provider/model identity, cost decision, retry authority, or hidden chain-of-thought.

The proposal is **not** itself effective Current Opportunity.

## 10. Local evaluation/trace shape

Preferred result:

```text
DirectorOpportunityEvaluation
- Proposal
- Trace
```

Local trace:

```text
DirectorOpportunityTrace
- ContractVersion
- StrategyContract
- SceneId
- SourceCharacterId
- SourceContextPacketId
- RosterCharacterIds
- OpportunityHistory
- AddressedCharacterIds
- NominatedCharacterId
- CandidatePoolCharacterIds
- SelectedCharacterId
- Basis
```

Trace contains IDs/control only and no `VisibleText`, private Character records, denied IDs, truth records, provider credentials, or model reasoning.

For E0 provenance, the exact input history/control and resulting proposal must be preservable. The trace is not Character-facing context.

## 11. Context/candidate binding invariants

Fail closed unless:

1. `currentContext` is non-null and structurally initialized;
2. `currentCandidate` is non-null and structurally initialized;
3. Context SubjectCharacterId equals Context OpportunityCharacterId;
4. Candidate SubjectCharacterId equals Context SubjectCharacterId;
5. Candidate ContextPacketId equals Context ContextPacketId;
6. Context roster Character IDs are initialized and unique;
7. Context subject appears exactly once in roster;
8. Candidate addressed/nomination IDs are initialized, resolve exactly/case-sensitively in roster, do not equal the source subject, and addressed IDs contain no duplicates;
9. Opportunity history is non-empty;
10. every history Character ID resolves exactly/case-sensitively in current roster;
11. the final history entry equals the current source Character.

The selector does not recompute Context hashes, inspect fixture provenance, or re-run Access Control.

## 12. Eligibility for E0 reference strategy

E0 has exactly three co-present Characters in one Scene.

For Patch 0007 reference selection:

- the current Context roster is the eligible set;
- no separate eligibility model/flag is invented;
- all three roster Characters remain eligible;
- the source Character is excluded from the **fallback pool** when at least one other roster Character exists;
- a candidate cannot nominate/address itself under Patch 0006, so explicit control pools already exclude source.

Future scenes may require richer hard eligibility based on presence, channel, incapacity, creator intervention, or other conditions. Those remain later design and must stay deterministic where they are hard rules.

## 13. Exact deterministic proposal rule

The E0 reference selector performs these steps exactly:

### Step 1 — nomination

If `NominatedCharacterId` is present and valid:

```text
CandidatePool = [NominatedCharacterId]
SelectedCharacterId = NominatedCharacterId
Basis = Nomination
```

Nomination is a strong explicit handoff signal but remains non-binding. It selects whose opportunity would become salient if the later causal boundary applies the proposal; it does not force that Character to speak or act in any particular way.

### Step 2 — direct address

Otherwise, if `AddressedCharacterIds` is non-empty:

```text
CandidatePool = AddressedCharacterIds
SelectedCharacterId = least-recently-opportunitied member of CandidatePool
Basis = DirectAddress
```

### Step 3 — deterministic fallback

Otherwise:

```text
CandidatePool = current roster excluding SourceCharacterId
SelectedCharacterId = least-recently-opportunitied member of CandidatePool
Basis = RecencyFallback
```

For the frozen three-Character E0 Scene, fallback pool therefore contains the other two Characters.

No numeric weight, score, probability, random choice, clock, culture, model, network, or process-global mutable state is used.

## 14. Least-recently-opportunitied rule

For a candidate pool:

1. a Character never appearing in `OpportunityHistory` is considered less recent than any Character that appears;
2. among never-seen Characters, choose the ordinally smallest CharacterId;
3. otherwise compare each Character's last index in `OpportunityHistory`;
4. choose the Character with the smallest last index (oldest most-recent opportunity);
5. exact ties use ordinal CharacterId.

This rule uses recency, not total dialogue count or a turn quota.

Explicit nomination/address always overrides recency across Characters outside its candidate pool. Therefore the strategy does not enforce equal dialogue.

## 15. Why no participation-count quota exists

Blueprint 0.1 says equal dialogue is not the goal and explicitly warns against mechanical scheduling.

Patch 0007 therefore does not:

- track total lines;
- count words/tokens;
- target equal turns;
- assign fairness percentages;
- force a Character because they are “behind” a quota;
- use numeric participation weights.

Recency exists only as a deterministic tie/fallback mechanism to reduce accidental exclusion and repetitive ping-pong when explicit social signals do not uniquely resolve attention.

## 16. Silence behavior

A committed or candidate silence does not itself force a handoff category.

Patch 0006 represents silence as empty VisibleText and empty/null control. Because Patch 0007 ignores VisibleText, silence naturally reaches `RecencyFallback`.

The next selected Character receives only an opportunity. The silent source is not judged as failed, penalized, or rewritten.

## 17. Proposal authority versus effective opportunity

This distinction is constitutional for Patch 0007:

```text
DirectorOpportunityProposal != effective Current Opportunity
```

`ProposeNext(...)` is a pure deterministic calculation with no side effect.

It may be computed speculatively before commit because Patch 0006 permits discardable zero-authority Director speculation.

A proposal may become an effective opportunity only through a later approved causal/orchestration boundary **after** the source Performance and its approved consequences successfully commit atomically.

If the Performance is rejected, cancelled, fails, or its causal commit fails:

- the proposal must not alter Current Opportunity;
- it must not trigger another Performer;
- it must not survive as effective routing state;
- speculative proposal data may remain diagnostics/provenance only as appropriate.

Patch 0007 does not implement effective opportunity mutation.

## 18. No hidden Director prose or chain-of-thought

The reference selector produces only typed deterministic artifacts.

No:

- free-form “Director reasoning”;
- model chain-of-thought;
- hidden scratchpad;
- natural-language stage direction;
- generated rationale;
- prompt-derived salience explanation.

`Basis + exact typed inputs + deterministic strategy contract` are sufficient to reconstruct the E0 proposal.

Future Director Glass may render a human-readable explanation from provenance, but that presentation is outside this patch and must not expose private chain-of-thought.

## 19. Information-authority boundary

The E0 Director is an orchestration role, not a Character and not a truth authority.

Patch 0007 may inspect only:

- safe Context structural IDs/roster;
- Candidate structural identity/control;
- effective opportunity history.

It does not receive denied Character information or full Production truth merely because it is “the Director.”

The proposal itself reveals only the selected Character plus structural basis. It does not grant any Character new knowledge.

## 20. Statement/truth/world boundary

Director control signals cannot make Character prose true.

Nomination/address mean only Performer intent about social attention.

The Director cannot:

- treat a claim as historical fact;
- promote a possibility;
- infer hidden world events;
- create an observation;
- alter memory/belief/relationship/pressure;
- resolve non-Character reality.

World Resolver remains separate and unimplemented.

## 21. Determinism

For identical:

```text
ContextPacket structural identity/roster
+ CandidatePerformance identity/control
+ OpportunityHistory
+ Director contract versions
```

Patch 0007 produces identical:

- candidate pool;
- selected Character;
- Basis;
- Proposal;
- Trace.

No filesystem, clock, random source, culture, network, provider/model, AI inference, GPU/NPU, or global mutable state affects selection.

## 22. E0-D round-robin ablation boundary

Required E0-D deterministic round-robin is **not** a flag inside `social-handoff-recency.v1`.

It must later be a separately labeled experimental strategy/implementation, with the same fixture, same-model Performer reference configuration, and other feasible variables held fixed.

The round-robin ablation ignores nomination/address and follows its own frozen cyclic order.

Patch 0007 does not implement or silently invoke that ablation.

This keeps the experimental variable explicit.

## 23. ODR-12 remains open after E0

Patch 0007 does not answer the final product question:

> structured rules, local semantic judgment, or hybrid?

The E0 reference strategy is provisional experimental machinery.

E0 evidence may later show that:

- explicit typed social handoff is sufficient;
- richer deterministic relevance is needed;
- a local semantic proposal layer helps;
- a hybrid works better;
- the entire strategy should change.

Any post-E0 mechanism requires separate approval/evidence and must preserve deterministic hard eligibility/authority gates.

## 24. Proposed public surface

Preferred reference API:

```text
DirectorOpportunitySelector.ProposeNext(
    ContextPacket currentContext,
    CandidatePerformance currentCandidate,
    IReadOnlyList<CharacterId> opportunityHistory)
    -> DirectorOpportunityEvaluation
```

`opportunityHistory` is caller-supplied observed history, not mutated by the selector.

`DirectorOpportunityProposal`, `DirectorOpportunityTrace`, and `DirectorOpportunityEvaluation` are public read-only outputs with Core-internal constructors, preserving the non-forgeable output pattern used by Access/Context/Performer authority artifacts.

The input history type is intentionally a standard read-only sequence rather than a speculative persistence/Take/Commit DTO.

## 25. Fail-closed errors

Use one small Director-specific exception domain.

Fail on:

- null/uninitialized Context;
- null/uninitialized Candidate;
- Context subject/opportunity mismatch;
- Candidate subject/context mismatch;
- invalid/duplicate roster IDs;
- source missing/duplicated in roster;
- invalid Candidate control relative to current roster;
- empty opportunity history;
- unknown/uninitialized Character in history;
- history tail not equal to current source Character;
- empty candidate pool;
- impossible selection invariant.

Errors contain structural diagnostics only. They must not echo Candidate VisibleText or private Context record text.

Technical failure never becomes fictional action or an implicit fallback opportunity.

## 26. Required tests

Use validated Patch 0005 ContextPacket and Patch 0006 CandidatePerformance paths; no duplicated fixture JSON.

Required coverage:

1. Missing Raft opening opportunity remains fixture `VOSS`; selector does not create opening state;
2. valid Voss nomination of Wren proposes WREN with `Nomination` basis;
3. nomination overrides addressed set;
4. single direct address proposes addressed Character;
5. multiple direct addresses choose least-recently-opportunitied addressed Character;
6. empty control chooses least-recently-opportunitied non-source roster Character;
7. never-seen Character beats previously seen Character in recency selection;
8. never-seen tie resolves ordinally;
9. last-index comparison is deterministic;
10. explicit nomination may select a recently selected Character, proving no quota overrides handoff;
11. explicit direct address pool may exclude a less-recent unaddressed Character, proving no global quota;
12. candidate VisibleText changes with identical control/history do not change proposal;
13. silence reaches fallback without invented narration/penalty;
14. selected Character receives opportunity only; proposal contains no line/outcome/state mutation;
15. Proposal carries semantic contract + strategy contract;
16. Proposal carries SceneId/source Character/source ContextPacketId/selected Character/basis only;
17. Trace contains exact roster/history/control/candidate pool/selection IDs and no VisibleText;
18. output constructors are non-public;
19. Context/Candidate/history are not mutated;
20. repeated evaluation is identical;
21. source-order-independent addressed control remains deterministic because Patch 0006 stores it ordinally;
22. null Context fails Director-specific;
23. null Candidate fails Director-specific;
24. Context subject != opportunity fails;
25. Candidate subject != Context subject fails;
26. Candidate ContextPacketId mismatch fails;
27. invalid/duplicate roster fails where defensively constructible;
28. empty history fails;
29. history unknown Character fails;
30. uninitialized history Character fails;
31. history tail != source fails;
32. Director failure does not synthesize an alternate proposal;
33. no method mutates effective opportunity or triggers Performer;
34. no provider/model/AI dependency;
35. no full fixture/Production truth/Access decision input;
36. no Candidate VisibleText inspection in selection implementation;
37. no relationship/private-record/Pressure text inspection;
38. no numeric score/weight/probability field;
39. no dialogue/word/token count quota;
40. no World Resolver behavior;
41. no TakeId/CommitId allocation or commit semantics;
42. no Integrity/State Interpreter/State Authority behavior;
43. social-handoff strategy distinct from future round-robin ablation contract;
44. frozen Missing Raft Context hashes unchanged;
45. frozen Missing Raft ECJ-1 identity unchanged;
46. all existing 173 Core tests remain green;
47. Missing Raft Harness remains PASS/0;
48. generic smoke Harness remains PASS/0.

## 27. Harness behavior

Patch 0007 does not add live provider execution or a self-running Scene loop.

Existing Harness validation output remains unchanged.

Core tests exercise the pure Director proposal function using ContextPackets and CandidatePerformances produced through validated upstream boundaries.

The first effective Scene-loop integration must wait until the downstream Integrity/State/Take/atomic-commit contracts can guarantee that a proposal is applied only after successful causal commit.

## 28. ARM64/battery suitability

Patch 0007 reference selection is tiny deterministic CPU work:

- validate a three-Character roster;
- inspect narrow typed control;
- scan bounded E0 opportunity history;
- choose by ordinal deterministic recency.

No network, background task, AI inference, GPU, NPU, filesystem I/O, polling, embeddings, or semantic model.

This authority logic belongs on CPU because it is trivial and deterministic. No NPU execution/performance claim is made.

## 29. Explicit exclusions

Patch 0007 does not implement:

- final/post-E0 Director mechanism;
- local semantic Director model;
- hybrid semantic + deterministic Director;
- provider/model Director role;
- prompt-based Director reasoning;
- relationship/pressure relevance projection;
- VisibleText semantic parsing;
- World Resolver;
- observation engine;
- effective opportunity state mutation;
- Scene loop/orchestration;
- provider Performer execution;
- Integrity Validator;
- accepted/rejected/alternate Take semantics;
- State Interpreter;
- State Authority;
- ProductionState/StateHash;
- atomic causal commit;
- persistence/recovery replay;
- E0-D round-robin implementation;
- E0-E playwright execution;
- final Stage/Director Glass UX;
- Windows AI/NPU;
- WinUI;
- packaging/WACK/Store.

## 30. Recursive audit dimensions

Before approval, recursively audit from the top after every correction for:

1. Blueprint 0.1 Director/attention law;
2. ODR-12 preservation;
3. Patch 0006 provisional-control/atomic-commit law;
4. Director vs Performer;
5. Director vs World Resolver;
6. Director vs Integrity/State/Take authority;
7. proposal vs effective opportunity distinction;
8. opening fixture opportunity vs Director decision;
9. deterministic hard eligibility;
10. social cue authority and nomination non-obligation;
11. equal-dialogue/quota avoidance;
12. repetitive ping-pong/exclusion fallback behavior;
13. VisibleText/untrusted-content isolation;
14. privacy/access leakage;
15. statement-vs-truth boundary;
16. causal provenance/reconstruction;
17. E0-D round-robin experimental isolation;
18. same-model reference-confound avoidance;
19. public API/forgery/minimality;
20. fail-closed behavior;
21. determinism;
22. test completeness;
23. ARM64/battery suitability;
24. scope/hygiene/premature abstraction;
25. E0-B/C/D/E/F/G compatibility.

Approval is requested only after one complete pass returns zero remaining material corrections or worthwhile improvements.

## 31. Material approval decisions

Approval would freeze only these E0 Patch 0007 decisions:

1. Patch 0007 is the Director opportunity boundary after Performer candidate output;
2. Patch 0007 defines a pure proposal/evaluation, not effective Current Opportunity mutation;
3. opening Missing Raft opportunity remains fixture authority, not a Director decision;
4. E0 reference strategy is deterministic/model-free and provisional, preserving post-E0 ODR-12;
5. semantic proposal contract is `ensemble.e0.director.opportunity.v1`;
6. reference strategy is `ensemble.e0.director.social-handoff-recency.v1`;
7. input is safe Context structural identity/roster + Candidate identity/control + effective opportunity history;
8. Candidate VisibleText, private Character records, Production truth, relationship text, Pressure text, Access decisions, and provenance are not selection inputs;
9. Context/Candidate identity must match and history tail must equal source Character;
10. E0 roster is the eligible set; no separate eligibility scoring model is invented;
11. nomination selects first when present;
12. otherwise direct-address pool selects by least-recent opportunity;
13. otherwise fallback excludes current source and chooses least-recent opportunity among remaining roster;
14. least-recent rule uses history last index + ordinal tie-break only;
15. no dialogue-count quota, numeric weight, probability, random choice, or LLM selection exists;
16. explicit social signals may override global recency, so equal dialogue is not enforced;
17. silence has no special penalty/obligation and naturally reaches fallback when control is empty;
18. proposal Basis is typed `Nomination | DirectAddress | RecencyFallback` and contains no generated rationale;
19. trace preserves exact structural inputs/selection for E0 provenance and is not Character-facing;
20. proposal/control have zero effective routing authority before successful later causal commit;
21. failed/rejected/cancelled/uncommitted source Performance cannot establish effective next opportunity;
22. E0-D round-robin remains a separate later experimental strategy, never a flag/bypass in the reference strategy;
23. relationship/pressure/current-interaction semantic relevance remains permitted future Director design, not silently frozen out of the product;
24. no provider, World Resolver, Integrity, State, Take, commit, persistence, Scene-loop, UI, Windows AI/NPU, or Store scope enters Patch 0007.

Implementation must not begin until the recursively audited proposal is explicitly approved.
