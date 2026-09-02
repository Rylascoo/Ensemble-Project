# H1 Patch 0007 — E0 Director Opportunity Contract

Status: blueprint proposal 0.3 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0006
Branch: `h1-patch-0007-director-opportunity-blueprint`

## 1. Purpose

Define the next E0-A behavioral boundary after the validated Performer candidate contract:

```text
validated safe ContextPacket
+ structurally valid CandidatePerformance
+ immutable effective opportunity history
    -> deterministic E0 Director proposal/evaluation
        -> later Integrity / State / Take / atomic-commit boundary
            -> effective Current Opportunity
                -> next Context Composer / Performer
```

Patch 0007 asks:

> Given the Character-safe context associated with the current candidate, the candidate's narrow typed social-handoff metadata, and the already-effective opportunity history for the Scene, what next Character opportunity would the E0 reference Director propose while intervening as little as possible and without writing outcomes, inventing world reality, granting knowledge, interpreting hidden prose, or acquiring effective authority before causal commit?

Patch 0007 defines one pure deterministic **E0 reference proposal function**. It does not mutate effective Current Opportunity and does not implement the later Integrity, State, Take, or atomic-commit authority that may eventually apply a proposal.

## 2. Authority basis

Frozen Blueprint 0.1 establishes:

- Director manages attention and opportunity;
- its central question is “Whose agency becomes salient next, and why?”;
- it may consider hard eligibility, direct social address, Character nomination, current interaction relevance, relationship relevance, observable pressure, participation balance, and recent repetition;
- no invented numeric weights are frozen;
- equal dialogue is not the goal;
- Director should detect accidental exclusion, repetitive two-character ping-pong, and stalled attention without enforcing a turn quota;
- handoff represents opportunity/social pressure, not obligation;
- selected Character may speak, act, evade, redirect, refuse, or remain silent;
- Director may not write a line, force an outcome, grant inaccessible knowledge, create belief/truth, commit state, authorize spending/retries, or manufacture non-Character reality;
- World Resolver is separate and unimplemented in E0;
- deterministic hard eligibility/access/cost rules may not be delegated to an LLM;
- every E0 run preserves Director opportunity inputs and decisions in provenance;
- E0-D requires deterministic round-robin opportunity order as a separately labeled ablation;
- ODR-12 intentionally leaves the **post-E0/final opportunity-selection mechanism** open: structured rules, local semantic judgment, or a hybrid;
- E0-A preparation explicitly requires Director inputs, responsibilities, prohibitions, and a **least-intervention rule**.

Least-intervention for the E0 reference means:

> Preserve Performer/social intention by default. Intervene only for hard structural constraints or a clear degenerate routing pattern. Do not optimize plot twists, emotional outcomes, or equal dialogue.

Scene exhaustion remains outside Patch 0007 because ODR-13/run termination is not frozen. A later run protocol simply does not invoke the selector after the Scene ends.

Validated Patch 0006 additionally freezes:

- `CandidatePerformance` is provisional semantic Performer output;
- `Control` contains only optional `AddressedCharacterIds` and optional `NominatedCharacterId`;
- control is Performer intent metadata, not truth/state/observation/Director authority/history;
- provisional/uncommitted control cannot create effective opportunity, trigger another Performer, or survive rollback as routing state;
- discardable precommit Director computation is allowed only with zero authority/effect;
- control associated with a successfully committed accepted Performance may later become one non-binding Director input;
- if Director control is actually consumed for effective attention, the semantic control and decision must remain reconstructable in causal provenance.

## 3. Scope: provisional E0 strategy, not final Director

Patch 0007 freezes only:

1. one structured deterministic E0 reference proposal strategy;
2. one semantic proposal contract;
3. one typed trace sufficient to reconstruct E0 selection;
4. exact least-intervention behavior required for the reference experiment.

It does **not** resolve ODR-12 after E0.

The reference strategy is model-free so E0-A does not introduce a second semantic-model variable alongside the same-model Performer reference condition. A future product Director may be structured, local-semantic, hybrid, or replaced if E0 evidence supports that.

## 4. Opening opportunity remains fixture authority

Missing Raft opening opportunity remains exactly `VOSS`.

Patch 0007:

- does not select or reinterpret the opening Character;
- is used only after an effective opportunity has already produced a structurally valid CandidatePerformance;
- keeps opening fixture provenance distinct from later Director-produced opportunities.

## 5. Reference input

Preferred API input:

```text
ContextPacket currentContext
CandidatePerformance currentCandidate
ImmutableArray<CharacterId> opportunityHistory
```

`currentContext` contributes only:

- SceneId;
- SubjectCharacterId;
- OpportunityCharacterId;
- ContextPacketId;
- roster Character IDs.

`currentCandidate` contributes only:

- SubjectCharacterId;
- ContextPacketId;
- AddressedCharacterIds;
- NominatedCharacterId.

`VisibleText` is not inspected.

`opportunityHistory` is an immutable ordered sequence of **effective Character opportunities already established for this Scene**, including the opening fixture opportunity and later effective Director opportunities.

It is attention history, not transcript history, dialogue counts, accepted-Take storage, world truth, or persistence authority.

Using `ImmutableArray<CharacterId>` prevents caller mutation during deterministic evaluation without inventing a premature Take/Commit history DTO.

## 6. Why VisibleText is excluded

The frozen Director may eventually consider current interaction relevance, but Patch 0007 does not infer it from free-form candidate prose because:

- Patch 0006 already provides explicit typed social-handoff metadata;
- Performance prose is untrusted creative content;
- semantic parsing would introduce another model or a new language ontology;
- doing so now would confound E0 before the behavioral architecture is measured;
- hidden semantic parsing could create a covert authority path from prose into routing;
- ODR-12 explicitly preserves the post-E0 structured/local-semantic/hybrid choice.

Changing VisibleText while preserving Candidate identity/control/history must not change the Patch 0007 proposal.

## 7. Why relationship/pressure relevance is not consumed yet

Blueprint 0.1 says Director **may** consider relationship relevance and observable pressure; it does not require every implementation to consume every signal.

Patch 0007 intentionally does not inspect:

- private relationship records;
- full Production relationship state;
- Pressure text;
- WorldState;
- hidden truth;
- Access decisions;
- fixture provenance;
- candidate prose.

The deterministic spine does not yet expose a Director-specific, authority-safe relevance projection for those concepts. Inventing one here would collapse Director selection with Access/State/semantic interpretation.

This is a narrow E0 reference simplification, not a product judgment that those signals are unimportant.

## 8. Contracts

Freeze:

```text
DirectorOpportunityContractVersion = ensemble.e0.director.opportunity.v1
DirectorStrategyContract = ensemble.e0.director.least-intervention.v1
```

The semantic proposal contract and strategy are separate so E0-D/post-E0 strategy changes do not automatically redefine the proposal object.

Neither identifier grants truth/state/routing authority.

## 9. Proposal shape

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

`Basis` is exactly one of:

```text
PingPongBreak
Nomination
DirectAddress
RecencyFallback
```

The proposal contains no line, prose direction, emotional instruction, required outcome, world event, state mutation, confidence/salience score, provider/model data, cost/retry authority, or chain-of-thought.

A proposal is not effective Current Opportunity.

## 10. Evaluation and trace

```text
DirectorOpportunityEvaluation
- Proposal
- Trace
```

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
- IntendedCandidatePoolCharacterIds
- IntendedSelectedCharacterId
- FinalCandidatePoolCharacterIds
- SelectedCharacterId
- Basis
```

Trace contains structural IDs/control only. It contains no VisibleText, private Character records, denied IDs, truth records, provider credentials, model reasoning, or free-form rationale.

The intended-vs-final distinction makes any least-intervention override auditable.

Trace is local/provenance material and never Character-facing context.

## 11. Binding and structural invariants

Fail closed unless:

1. Context non-null/initialized;
2. Candidate non-null/initialized;
3. Context SubjectCharacterId == OpportunityCharacterId;
4. Candidate SubjectCharacterId == Context SubjectCharacterId;
5. Candidate ContextPacketId == Context ContextPacketId;
6. roster initialized and contains exactly the frozen E0 three Characters with initialized unique IDs;
7. subject appears exactly once in roster;
8. Candidate address/nomination IDs initialized, exact/case-sensitive roster members, not source, duplicate-free;
9. opportunityHistory non-default and non-empty;
10. every history ID initialized and exactly in current roster;
11. history final entry == source Character.

The selector does not recanonicalize Context, recompute hashes, inspect provenance, or rerun Access Control.

## 12. E0 hard eligibility

For the frozen three-Character co-present Scene, the Context roster is the eligible set.

Patch 0007 introduces no separate eligibility model, score, or LLM.

Future product eligibility may consider presence, communication channel, incapacity, creator intervention, or other conditions. Those remain later authority design and deterministic when hard.

## 13. Least-intervention strategy

The selector first computes the **intended selection** from typed Performer control. It then asks only whether the last four effective opportunities show a clear two-Character ping-pong that the intended result would continue.

Exact order:

1. derive intended candidate pool;
2. derive intended selected Character;
3. apply ping-pong break only if the exact structural guard matches;
4. otherwise preserve intended selection unchanged.

No other fairness override exists in Patch 0007.

This keeps the Director from becoming an equal-turn scheduler.

## 14. Intended selection

### 14.1 Nomination present

```text
IntendedCandidatePool = [NominatedCharacterId]
IntendedSelectedCharacterId = NominatedCharacterId
IntendedBasis = Nomination
```

### 14.2 Otherwise direct address present

```text
IntendedCandidatePool = AddressedCharacterIds
IntendedSelectedCharacterId = least-recently-opportunitied member of IntendedCandidatePool
IntendedBasis = DirectAddress
```

### 14.3 Otherwise recency fallback

```text
IntendedCandidatePool = complete roster
IntendedSelectedCharacterId = least-recently-opportunitied member of IntendedCandidatePool
IntendedBasis = RecencyFallback
```

The source Character is **not** hard-excluded from fallback. It naturally loses ordinary recency comparison because history tail is the source and therefore it is the most recently opportunitied Character whenever another eligible Character exists.

Nomination/address remain social-intent cues, not truth or outcome authority. Honoring them grants opportunity only and never obligates a response.

## 15. Least-recently-opportunitied helper

Within a candidate pool:

1. a Character never appearing in history is less recent than any appearing Character;
2. among never-seen Characters choose ordinally smallest CharacterId;
3. otherwise compare each Character's final index in history;
4. choose smallest final index (oldest most-recent opportunity);
5. exact tie uses ordinal CharacterId.

No total turn count, dialogue count, word count, token count, percentage, probability, or random value is used.

This fallback naturally surfaces a never-seen third Character when social control is absent, without an extra exclusion quota.

## 16. Exact two-Character ping-pong guard

For the frozen three-Character roster, ping-pong exists only when all are true:

1. `OpportunityHistory.Length >= 4`;
2. last four IDs are exactly `A,B,A,B` or `B,A,B,A`;
3. `A != B`;
4. exactly one roster Character `C` is absent from those four;
5. intended selection would choose `A` or `B` again rather than `C`.

Then:

```text
FinalCandidatePool = [C]
SelectedCharacterId = C
Basis = PingPongBreak
```

If intended selection already targets `C`, no override occurs and the original Nomination/DirectAddress/RecencyFallback basis remains.

Four opportunities represent the minimum complete two-cycle alternation needed to establish an actual repeated A↔B routing pattern; this is a structural pattern definition, not a salience weight.

## 17. Accidental exclusion without a turn quota

Patch 0007 does not impose a recurring maximum-gap or equal-participation rule.

With exactly three E0 Characters:

- no social control -> recency fallback selects never-seen/least-recent Characters;
- sustained explicit A↔B handoff -> exact ping-pong guard routes the absent third Character;
- multiple addresses -> recency resolves within the socially addressed pool.

This covers the primary structural exclusion modes without an invented “every Character must speak every N turns” policy.

The trace/history still makes unusual exclusion measurable during E0 evaluation even when no hard override applies.

## 18. No turn quota

Patch 0007 does not target equal opportunity or dialogue and does not maintain percentages, line/word/token counts, fairness scores, or ongoing maximum-gap guarantees.

Explicit social intention may produce unequal opportunity distribution whenever the exact ping-pong guard does not require intervention.

## 19. Silence

Patch 0006 silence is empty VisibleText + empty/null control.

Patch 0007 ignores VisibleText, so silence naturally uses RecencyFallback and the same structural guard as any other Candidate.

Silence is not failure, punishment, or a command to end the Scene.

## 20. Proposal versus effective Current Opportunity

Constitutional distinction:

```text
DirectorOpportunityProposal != effective Current Opportunity
```

`ProposeNext(...)` is pure and side-effect free.

It may be computed speculatively before commit only because Patch 0006 permits discardable zero-authority Director computation.

A proposal may become effective only through a later approved orchestration/causal boundary **after** the source Performance and all approved consequences successfully commit atomically.

On rejection, cancellation, provider failure, Integrity failure, or failed causal commit:

- no Current Opportunity changes;
- no next Performer is triggered;
- proposal cannot survive as effective routing state;
- speculative evaluation may remain diagnostics/provenance only where required.

Patch 0007 implements no effective opportunity mutation.

## 21. Stale-evaluation rule for later application

A later causal layer must **not** accept a detached `DirectorOpportunityProposal` as sufficient authority.

It must either:

1. compute the pure evaluation after successful source commit; or
2. if using a precomputed evaluation, verify that the effective Scene/context/candidate/opportunity-history binding represented by the evaluation still matches the authoritative state before applying it.

If binding changed, discard and recompute/fail closed under the later contract.

Patch 0007 does not freeze CommitId/TakeId semantics or implement this application boundary.

## 22. Causal consistency in frozen E0 Scene

E0 has one fixed co-present three-Character Scene. Patch 0007 consumes no world/relationship/pressure projection that State Authority could mutate between candidate production and selection.

That narrow design is intentional. Broader post-E0 stale-state semantics remain open.

## 23. No hidden Director prose/reasoning

Reference Director output is typed deterministic data only.

No free-form reasoning, chain-of-thought, scratchpad, natural-language stage direction, generated rationale, plot instruction, or hidden semantic score is created.

`Basis + exact typed inputs + strategy contract` are sufficient for E0 reconstruction.

Future Director Glass may render a human-readable explanation from provenance outside this patch.

## 24. Information-authority boundary

Director is orchestration, not Character and not truth authority.

Patch 0007 sees only:

- safe Context structural IDs/roster;
- Candidate structural identity/control;
- effective opportunity history.

It does not receive denied Character state or full Production truth merely because it is Director.

Proposal grants no Character knowledge. Character-facing context receives only a later effective opportunity, never the trace.

## 25. Statement/truth/World Resolver boundary

Nomination/address mean Performer social intent only.

Director cannot promote claims/possibilities, infer hidden world events, create observations, mutate Character/world/relationship/pressure state, or resolve non-Character reality.

World Resolver remains separate and unimplemented.

## 26. Determinism

Identical:

```text
Context structural identity/roster
+ Candidate identity/control
+ immutable OpportunityHistory
+ Director contract versions
```

must produce identical intended pool, intended selection, guard result, final pool, Basis, Proposal, and Trace.

No filesystem, clock, random source, culture, network, provider/model, AI inference, GPU/NPU, or mutable global state affects selection.

## 27. E0-D round-robin isolation

Required deterministic round-robin remains a **separate strategy/implementation**, never a flag inside `least-intervention.v1`.

Round-robin deliberately ignores social handoff behavior and follows its own fixed cyclic order while other feasible variables remain matched.

Patch 0007 does not implement the ablation.

The reference fallback may look rotational when social control is absent, but reference and round-robin remain experimentally distinct because the reference honors nomination/address and only breaks exact degenerate A↔B routing.

## 28. ODR-12 remains open

Patch 0007 does not decide whether post-E0 Director should use structured rules, local semantic judgment, hybrid selection, richer relevance projections, or another strategy.

E0 evidence must inform that choice. Deterministic hard eligibility/authority remains required regardless of future semantic assistance.

## 29. Proposed public surface

```text
DirectorOpportunitySelector.ProposeNext(
    ContextPacket currentContext,
    CandidatePerformance currentCandidate,
    ImmutableArray<CharacterId> opportunityHistory)
    -> DirectorOpportunityEvaluation
```

`DirectorOpportunityProposal`, `DirectorOpportunityTrace`, and `DirectorOpportunityEvaluation` are public read-only outputs with Core-internal constructors.

No public method applies the proposal, mutates Current Opportunity, or triggers Performer execution.

## 30. Fail closed

Use one small Director-specific exception domain.

Fail on:

- null/uninitialized Context;
- null/uninitialized Candidate;
- Context subject/opportunity mismatch;
- Candidate subject/context mismatch;
- roster not exactly three for this E0 strategy;
- invalid/duplicate roster/source;
- invalid Candidate control relative to roster;
- default/empty history;
- unknown/uninitialized history ID;
- history tail != source;
- empty candidate pool;
- impossible pattern/selection invariant.

Errors contain structural diagnostics only and never echo Candidate VisibleText or private Context text.

Failure produces no alternate fictional action and no implicit fallback opportunity.

## 31. Required tests

Use validated Patch 0005 ContextPacket and Patch 0006 CandidatePerformance paths; no duplicate fixture JSON.

### Binding/opening/surface

1. Missing Raft opening opportunity remains fixture VOSS; selector creates no opening state;
2. Context/Candidate subject mismatch fails;
3. Candidate ContextPacketId mismatch fails;
4. Context subject/opportunity mismatch fails;
5. roster exactly three/unique/initialized/source once;
6. history immutable/non-default/non-empty;
7. history IDs roster-bound/initialized;
8. history tail == source;
9. output constructors non-public;
10. no public apply/mutate/trigger operation;
11. Proposal exact fields only;
12. Trace contains no VisibleText/private/provider/model reasoning.

### Least-intervention/social intent

13. Voss nomination of Wren -> WREN/Nomination when ping-pong guard does not override;
14. nomination defines ordinary intended pool before address;
15. single direct address -> addressed Character/DirectAddress;
16. multiple direct addresses choose least-recent inside addressed pool;
17. no control -> least-recent complete-roster fallback;
18. fallback naturally avoids source because source is history tail, without a hard source-exclusion rule;
19. never-seen beats seen inside ordinary pool;
20. tie uses ordinal CharacterId;
21. explicit nomination may select globally more-recent Character when no guard matches;
22. direct-address pool may exclude globally less-recent unaddressed Character when no guard matches;
23. VisibleText mutation with same Candidate identity/control/history leaves proposal unchanged;
24. silence uses fallback without penalty/narration.

### Ping-pong/exclusion

25. exact last-four A,B,A,B continuing to A/B routes third with PingPongBreak;
26. B,A,B,A works symmetrically;
27. A,B,A,C is not ping-pong;
28. A,A,B,A is not ping-pong;
29. fewer than four opportunities cannot trigger ping-pong;
30. social intent already targeting absent third preserves original social basis;
31. no-control history naturally surfaces never-seen third via recency;
32. no recurring maximum-gap/equal-turn guard exists;
33. guard override appears in intended-vs-final trace fields.

### Authority/scope

34. no line/word/token counters;
35. no score/weight/probability fields;
36. social intent can create unequal opportunity distribution absent exact guard;
37. Proposal does not mutate Context OpportunityCharacterId;
38. Proposal cannot trigger Performer;
39. Candidate control never becomes truth/state/observation authority;
40. no World Resolver behavior;
41. no relationship/Pressure/private-state input;
42. no Access decisions/full fixture/Production truth input;
43. no TakeId/CommitId allocation;
44. no Integrity/State Interpreter/State Authority behavior;
45. no provider/model/AI dependency.

### Determinism/regression

46. repeated evaluation identical;
47. inputs not mutated;
48. Candidate address order remains deterministic;
49. strategy distinct from future E0-D round-robin;
50. frozen Missing Raft StructuredContextHash unchanged;
51. frozen Missing Raft RenderedContextHash unchanged;
52. frozen Missing Raft ECJ-1 9112 bytes/hash unchanged;
53. all existing 173 Core tests green;
54. Missing Raft Harness PASS/0;
55. generic smoke Harness PASS/0.

Impossible malformed immutable authority objects may be covered by defensive review/reflection without public bypass constructors.

## 32. Harness behavior

No live provider execution or self-running Scene loop.

Existing Harness output remains unchanged.

Core tests exercise the pure proposal/evaluation using upstream validated Context/Candidate objects.

Effective Scene-loop integration waits for downstream Integrity/State/Take/atomic-commit contracts.

## 33. ARM64/battery suitability

Tiny deterministic CPU work only: validate three IDs/roster, inspect narrow typed control, scan bounded E0 history, inspect last four for ping-pong, perform ordinal comparisons.

No network/background task/AI/GPU/NPU/filesystem/polling/embedding/semantic model.

No NPU claim.

## 34. Explicit exclusions

No final/post-E0 Director mechanism; local semantic/hybrid Director; provider/model Director role; prompt-based Director reasoning; VisibleText semantic parsing; relationship/pressure relevance projection; Scene ending policy; World Resolver/observation; effective opportunity mutation; Scene loop; provider Performer execution; Integrity Validator; Take semantics/IDs; State Interpreter/Authority; ProductionState/StateHash; atomic commit/persistence; E0-D round-robin implementation; E0-E playwright execution; final Stage/Director Glass UX; WinUI/Windows AI/NPU/packaging/WACK/Store.

## 35. Recursive audit dimensions

After every correction restart from the top and test:

1. frozen Director/attention law;
2. least-intervention law;
3. ODR-12 preservation;
4. Patch 0006 provisional-control/commit law;
5. Director vs Performer;
6. Director vs World Resolver;
7. Director vs Integrity/State/Take;
8. proposal vs effective opportunity;
9. opening fixture vs Director;
10. hard eligibility;
11. social intent/non-obligation;
12. accidental exclusion;
13. ping-pong/stalled attention;
14. no turn quota/fake precision;
15. VisibleText/untrusted-content isolation;
16. privacy/access leakage;
17. statement-vs-truth;
18. causal provenance/stale evaluation;
19. E0-D isolation;
20. E0-A same-model confound isolation;
21. public API/non-forgeability/minimality;
22. fail-closed behavior;
23. deterministic immutable inputs;
24. test completeness;
25. ARM64 suitability;
26. scope/hygiene/premature abstraction;
27. E0-B/C/D/E/F/G compatibility.

Approval only after one complete pass returns zero material corrections or worthwhile improvements.

## 36. Material approval decisions

Approval would freeze only these E0 Patch 0007 decisions:

1. Patch 0007 follows validated Performer candidate output and precedes downstream effective opportunity application;
2. Director output here is pure proposal/evaluation, not Current Opportunity mutation;
3. Missing Raft opening VOSS remains fixture authority;
4. E0 reference Director is deterministic/model-free/least-intervention and provisional, preserving ODR-12;
5. proposal contract `ensemble.e0.director.opportunity.v1`;
6. strategy `ensemble.e0.director.least-intervention.v1`;
7. inputs limited to safe Context structural IDs/roster + Candidate identity/control + immutable effective opportunity history;
8. exactly three Context roster Characters form the E0 hard eligible set;
9. VisibleText/private Character state/Production truth/relationship text/Pressure text/Access decisions/provenance are not selection inputs;
10. Context/Candidate identity must bind and history tail must equal source;
11. ordinary least-intervention honors nomination, otherwise direct address, otherwise complete-roster recency fallback;
12. multiple addresses use least-recent within addressed pool;
13. source is not hard-excluded from fallback; recency naturally prevents immediate repeat when alternatives exist;
14. least-recent uses only final history index + ordinal tie-break;
15. the only automatic social-intent override is exact E0 three-Character last-four A↔B ping-pong continuation, which proposes the absent third Character;
16. if social intent already targets the absent third Character, no override/basis replacement occurs;
17. no exclusion timer, maximum-gap guarantee, turn quota, dialogue/token count, numeric salience weight, probability, randomness, or LLM routing;
18. silence receives no penalty/obligation and naturally uses fallback when control empty;
19. Basis is typed `PingPongBreak | Nomination | DirectAddress | RecencyFallback`, with no generated rationale;
20. Trace records intended and final structural selection so intervention is auditable and is not Character-facing;
21. proposal/control have zero effective routing authority before successful later causal commit;
22. rejection/cancellation/failure/rollback cannot establish next effective opportunity;
23. later application must recompute or validate the entire evaluation binding; detached Proposal alone is insufficient authority;
24. E0-D round-robin remains separate strategy, not flag/bypass;
25. interaction/relationship/pressure semantic relevance and final structured/local/hybrid choice remain future design;
26. Scene ending/exhaustion remains later ODR-13/run-protocol authority;
27. no provider, World Resolver, Integrity, State, Take, commit, persistence, Scene loop, UI, Windows AI/NPU, or Store scope enters Patch 0007.

Implementation remains blocked until recursive audit completes and user explicitly approves the final proposal.
