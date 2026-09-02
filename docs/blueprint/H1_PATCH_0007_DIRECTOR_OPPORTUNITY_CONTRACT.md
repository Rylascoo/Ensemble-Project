# H1 Patch 0007 — E0 Director Opportunity Contract

Status: blueprint proposal 0.4 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0006
Branch: `h1-patch-0007-director-opportunity-blueprint`

## 1. Purpose

Define the next E0-A behavioral boundary after the validated Performer candidate contract:

```text
validated safe ContextPacket
+ structurally valid CandidatePerformance
+ immutable effective opportunity history
    -> deterministic E0 Director proposal/evaluation
        -> later successful atomic causal commit
            -> recompute Director evaluation from accepted source
                -> later effective Current Opportunity
                    -> next Context Composer / Performer
```

Patch 0007 asks:

> Given the Character-safe context associated with the current candidate, its narrow typed social-handoff metadata, and the already-effective opportunity history for the Scene, what next Character opportunity would the E0 reference Director propose while intervening as little as possible and without writing outcomes, inventing world reality, granting knowledge, interpreting hidden prose, or acquiring effective authority before causal commit?

Patch 0007 defines one pure deterministic **E0 reference proposal function**. It does not mutate effective Current Opportunity and does not implement Integrity, State, Take, or atomic-commit authority.

## 2. Authority basis

Frozen Blueprint 0.1 establishes:

- Director manages attention and opportunity;
- its central question is “Whose agency becomes salient next, and why?”;
- it may consider hard eligibility, direct social address, Character nomination, current interaction relevance, relationship relevance, observable pressure, participation balance, and recent repetition;
- no invented numeric weights are frozen;
- equal dialogue is not the goal;
- Director should detect accidental exclusion, repetitive two-character ping-pong, and stalled attention without enforcing a turn quota;
- handoff is opportunity/social pressure, not obligation;
- selected Character may speak, act, evade, redirect, refuse, or remain silent;
- Director may not write a line, force an outcome, grant inaccessible knowledge, create belief/truth, commit state, authorize spending/retries, or manufacture non-Character reality;
- World Resolver is separate and unimplemented in E0;
- deterministic hard eligibility/access/cost rules may not be delegated to an LLM;
- every E0 run preserves Director opportunity inputs and decisions in provenance;
- E0-D requires deterministic round-robin as a separately labeled ablation;
- ODR-12 leaves the **post-E0/final opportunity-selection mechanism** open: structured rules, local semantic judgment, or hybrid;
- E0-A preparation requires Director inputs, responsibilities, prohibitions, and a **least-intervention rule**.

Least intervention for E0 means:

> Preserve Performer/social intention by default. Intervene only for hard structural constraints or a clear degenerate routing pattern. Do not optimize plot twists, emotional outcomes, or equal dialogue.

Scene exhaustion remains outside Patch 0007 because ODR-13/run termination is not frozen. A later run protocol simply stops invoking the selector when the Scene ends.

Validated Patch 0006 freezes:

- `CandidatePerformance` is provisional semantic Performer output;
- Control contains only optional addresses and optional nomination;
- control is Performer intent metadata, not truth/state/observation/Director authority/history;
- provisional/uncommitted control cannot create effective opportunity, trigger another Performer, or survive rollback as routing state;
- discardable precommit Director computation is allowed only with zero authority/effect;
- control associated with a successfully committed accepted Performance may later become one non-binding Director input;
- causally consumed control + resulting Director decision must remain reconstructable.

## 3. Scope: provisional E0 strategy, not final Director

Patch 0007 freezes only:

1. one structured deterministic E0 reference proposal strategy;
2. one semantic proposal contract reusable across Director strategies;
3. one least-intervention trace for reconstructing this strategy;
4. exact E0 anti-degeneracy behavior.

It does **not** resolve ODR-12 after E0.

The reference strategy is model-free so E0-A does not introduce an additional semantic-model variable alongside the same-model Performer reference condition.

## 4. Opening opportunity remains fixture authority

Missing Raft opening opportunity remains exactly `VOSS`.

Patch 0007:

- does not select/reinterpret the opening Character;
- is used only after an effective opportunity has produced a structurally valid CandidatePerformance;
- keeps opening fixture provenance distinct from later Director-produced opportunities.

## 5. Reference input

Preferred API input:

```text
ContextPacket currentContext
CandidatePerformance currentCandidate
ImmutableArray<CharacterId> opportunityHistory
```

`currentContext` contributes only SceneId, SubjectCharacterId, OpportunityCharacterId, ContextPacketId, and roster Character IDs.

`currentCandidate` contributes only SubjectCharacterId, ContextPacketId, AddressedCharacterIds, and NominatedCharacterId.

`VisibleText` is not inspected.

`opportunityHistory` is the immutable ordered sequence of **effective Character opportunities already established for this Scene**, including the opening fixture opportunity and later effective Director opportunities.

It is attention history, not transcript history, dialogue count, Take storage, world truth, or persistence authority.

## 6. Why VisibleText is excluded

Patch 0007 does not infer interaction relevance from free-form Performance prose because:

- typed social-handoff metadata already exists;
- Performance prose is untrusted creative content;
- semantic parsing would introduce another model or a language ontology;
- that would confound E0 before the behavioral architecture is measured;
- hidden prose interpretation could become a covert routing-authority path;
- ODR-12 preserves later structured/local-semantic/hybrid choice.

Changing VisibleText while preserving Candidate identity/control/history must not change the proposal.

## 7. Why relationship/pressure relevance is not consumed yet

Blueprint 0.1 says Director **may** consider relationship relevance and observable pressure; it does not require every implementation to consume every possible signal.

Patch 0007 intentionally does not inspect private relationships, Production relationship state, Pressure text, WorldState, hidden truth, Access decisions, fixture provenance, or candidate prose.

The deterministic spine does not yet expose a Director-specific authority-safe relevance projection for those concepts. Inventing one here would collapse Director selection with Access/State/semantic interpretation.

This is an E0 reference simplification, not a product judgment.

## 8. Contracts

Freeze:

```text
DirectorOpportunityContractVersion = ensemble.e0.director.opportunity.v1
DirectorStrategyContract = ensemble.e0.director.least-intervention.v1
```

The proposal contract and strategy are separate so E0-D/post-E0 strategies can change selection behavior without changing the semantic proposal shape.

## 9. Semantic proposal shape

```text
DirectorOpportunityProposal
- ContractVersion
- StrategyContract
- SceneId
- SourceCharacterId
- SourceContextPacketId
- SelectedCharacterId
```

The proposal deliberately has **no strategy-specific Basis/Reason field**.

Reasoning belongs to strategy-specific trace/provenance, not the semantic opportunity proposal. This allows a future E0-D round-robin strategy to emit the same proposal contract without adding or misusing least-intervention enum values.

The proposal contains no line, prose direction, emotional instruction, required outcome, world event, state mutation, confidence/salience score, provider/model data, cost/retry authority, or chain-of-thought.

A proposal is not effective Current Opportunity.

## 10. Least-intervention evaluation and trace

```text
DirectorOpportunityEvaluation
- Proposal
- Trace
```

```text
DirectorLeastInterventionTrace
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
- Rule
```

`Rule` is exactly one of:

```text
PingPongBreak
Nomination
DirectAddress
RecencyFallback
```

Trace contains structural IDs/control only and no VisibleText, private Character records, denied IDs, truth records, provider credentials, model reasoning, or free-form rationale.

The intended-vs-final distinction makes an override auditable.

Trace is local/provenance material, never Character-facing context.

A different Director strategy may use the same semantic Proposal contract with a different strategy-specific trace type.

## 11. Binding and structural invariants

Fail closed unless:

1. Context non-null/initialized;
2. Candidate non-null/initialized;
3. Context SubjectCharacterId == OpportunityCharacterId;
4. Candidate SubjectCharacterId == Context SubjectCharacterId;
5. Candidate ContextPacketId == Context ContextPacketId;
6. roster initialized and exactly three E0 Characters with unique initialized IDs;
7. subject appears exactly once in roster;
8. Candidate address/nomination IDs initialized, exact/case-sensitive roster members, not source, duplicate-free;
9. opportunityHistory non-default and non-empty;
10. every history ID initialized and exactly in current roster;
11. history final entry == source Character.

The selector does not recanonicalize Context, recompute hashes, inspect provenance, or rerun Access Control.

## 12. E0 hard eligibility

For the frozen three-Character co-present Scene, Context roster is the eligible set.

Patch 0007 introduces no separate eligibility model, score, or LLM.

Future product eligibility may consider presence, communication channel, incapacity, creator intervention, or other conditions. Those remain later authority design and deterministic when hard.

## 13. Least-intervention strategy

Exact order:

1. derive intended candidate pool from typed social control;
2. derive intended selected Character;
3. if the last four effective opportunities form exact two-Character ping-pong and intended selection would continue it, propose the absent third Character;
4. otherwise preserve intended selection unchanged.

No other fairness override exists.

## 14. Intended selection

### 14.1 Nomination present

```text
IntendedCandidatePool = [NominatedCharacterId]
IntendedSelectedCharacterId = NominatedCharacterId
Rule = Nomination unless ping-pong override applies
```

### 14.2 Otherwise direct address present

```text
IntendedCandidatePool = AddressedCharacterIds
IntendedSelectedCharacterId = least-recently-opportunitied member of IntendedCandidatePool
Rule = DirectAddress unless ping-pong override applies
```

### 14.3 Otherwise recency fallback

```text
IntendedCandidatePool = complete roster
IntendedSelectedCharacterId = least-recently-opportunitied member of IntendedCandidatePool
Rule = RecencyFallback unless ping-pong override applies
```

The source Character is not hard-excluded from fallback. It naturally loses ordinary recency comparison because history tail is the source and therefore it is the most recently opportunitied Character whenever another eligible Character exists.

Nomination/address are social-intent cues, not truth/outcome authority. Honoring them grants opportunity only and never obligates a response.

## 15. Least-recently-opportunitied helper

Within a candidate pool:

1. a Character never appearing in history is less recent than any appearing Character;
2. among never-seen Characters choose ordinally smallest CharacterId;
3. otherwise compare each Character's final index in history;
4. choose smallest final index;
5. exact tie uses ordinal CharacterId.

No total turns, dialogue, words, tokens, percentages, probabilities, or randomness are used.

## 16. Exact two-Character ping-pong guard

For the frozen three-Character roster, ping-pong exists only when all are true:

1. history length >= 4;
2. last four IDs are exactly `A,B,A,B` or `B,A,B,A`;
3. `A != B`;
4. exactly one roster Character `C` is absent from those four;
5. intended selection would choose `A` or `B` again rather than `C`.

Then:

```text
FinalCandidatePool = [C]
SelectedCharacterId = C
Rule = PingPongBreak
```

If intended selection already targets `C`, there is no override and original Rule remains.

Four opportunities are the minimum complete two-cycle alternation needed to establish repeated A↔B routing; this is a structural pattern definition, not a salience weight.

## 17. Accidental exclusion without a quota

With exactly three E0 Characters:

- no social control -> recency fallback naturally selects never-seen/least-recent Characters;
- sustained explicit A↔B handoff -> ping-pong guard routes the absent third Character;
- multiple addresses -> recency resolves inside the socially addressed pool.

Patch 0007 therefore needs no arbitrary exclusion timer or ongoing maximum-gap guarantee.

Opportunity history/trace still makes unusual exclusion measurable during E0 evaluation.

## 18. No turn quota

Patch 0007 does not target equal opportunity/dialogue and has no percentages, line/word/token counts, fairness scores, or maximum-gap guarantees.

Explicit social intention may produce unequal opportunity distribution whenever the exact ping-pong guard does not require intervention.

## 19. Silence

Patch 0006 silence is empty VisibleText + empty/null control.

Patch 0007 ignores VisibleText, so silence naturally uses RecencyFallback and the same structural guard as any other Candidate.

Silence is not failure, punishment, or Scene-ending authority.

## 20. Proposal versus effective Current Opportunity

```text
DirectorOpportunityProposal != effective Current Opportunity
```

`ProposeNext(...)` is pure and side-effect free.

It may be computed speculatively before commit only as discardable zero-authority work.

On rejection, cancellation, provider failure, Integrity failure, or failed causal commit:

- no Current Opportunity changes;
- no next Performer triggers;
- speculative proposal/evaluation is discarded as routing state;
- diagnostics/provenance may retain the attempted evaluation where required.

## 21. Mandatory post-commit recomputation before effective use

A precommit `DirectorOpportunityEvaluation` may **never** be promoted directly into effective routing authority.

Reason: Patch 0006 intentionally has no CandidateId/TakeId. Distinct attempts can share the same Context identity and control metadata, so a precommit evaluation cannot by itself prove association with the later accepted Take.

Therefore a later effective-opportunity boundary must:

1. successfully commit the accepted source Performance + all approved consequences atomically;
2. use the accepted source Performance/control and then-authoritative effective opportunity history;
3. recompute Patch 0007 Director evaluation after that successful commit;
4. only then apply the resulting selected Character under that later contract.

Precommit evaluation is preview/speculation only and is always discardable.

Patch 0007 does not define CandidateId, TakeId, CommitId, or application semantics.

## 22. Frozen E0 stale-state scope

E0 uses one fixed co-present three-Character Scene. Patch 0007 consumes no world/relationship/pressure projection that State Authority could mutate between source Performance and postcommit recomputation.

This narrow design is intentional. Broader post-E0 stale-state semantics remain open.

## 23. No hidden Director prose/reasoning

Reference Director output is typed deterministic data only.

No free-form reasoning, chain-of-thought, scratchpad, natural-language stage direction, generated rationale, plot instruction, or hidden semantic score is created.

`Rule + exact typed inputs + strategy contract` are sufficient for E0 reconstruction.

Future Director Glass may render a human-readable explanation from provenance outside this patch.

## 24. Information-authority boundary

Director is orchestration, not Character and not truth authority.

Patch 0007 sees only safe Context structural IDs/roster, Candidate identity/control, and effective opportunity history.

It does not receive denied Character state or full Production truth merely because it is Director.

Proposal grants no Character knowledge. Character-facing context receives only a later effective opportunity, never the trace.

## 25. Statement/truth/World Resolver boundary

Nomination/address mean Performer social intent only.

Director cannot promote claims/possibilities, infer hidden world events, create observations, mutate Character/world/relationship/pressure state, or resolve non-Character reality.

World Resolver remains separate/unimplemented.

## 26. Determinism

Identical Context structural identity/roster + Candidate identity/control + immutable opportunity history + Director contract versions must produce identical intended pool, intended selection, guard result, final pool, Proposal, and Trace.

No filesystem, clock, randomness, culture, network, provider/model, AI inference, GPU/NPU, or mutable global state affects selection.

## 27. E0-D round-robin isolation

Required deterministic round-robin remains a **separate strategy/implementation**, never a flag inside `least-intervention.v1`.

Round-robin may emit the same `DirectorOpportunityProposal` semantic contract with its own `StrategyContract` and strategy-specific trace.

That lets E0-D vary Director strategy without also varying the semantic opportunity-output shape.

Patch 0007 does not implement the ablation.

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

`DirectorOpportunityProposal`, `DirectorLeastInterventionTrace`, and `DirectorOpportunityEvaluation` are public read-only outputs with Core-internal constructors.

No public method applies Proposal, mutates Current Opportunity, or triggers Performer execution.

## 30. Fail closed

Use one small Director-specific exception domain.

Fail on null/uninitialized Context/Candidate; Context subject/opportunity mismatch; Candidate subject/context mismatch; roster not exactly three; invalid/duplicate roster/source; invalid Candidate control relative to roster; default/empty history; unknown/uninitialized history ID; history tail != source; empty candidate pool; impossible pattern/selection invariant.

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
6. history non-default/non-empty and immutable;
7. history IDs roster-bound/initialized;
8. history tail == source;
9. output constructors non-public;
10. no public apply/mutate/trigger operation;
11. Proposal exact semantic fields only and has no Basis/Reason;
12. Trace contains Rule but no VisibleText/private/provider/model reasoning.

### Least intervention

13. Voss nomination of Wren -> WREN when no ping-pong override;
14. nomination defines ordinary intended pool before address;
15. single direct address selects addressed Character;
16. multiple addresses choose least-recent inside addressed pool;
17. no control -> least-recent complete-roster fallback;
18. fallback naturally avoids source without hard exclusion;
19. never-seen beats seen inside ordinary pool;
20. tie uses ordinal CharacterId;
21. nomination may choose globally more-recent Character when guard does not match;
22. address pool may exclude globally less-recent unaddressed Character when guard does not match;
23. VisibleText mutation with same identity/control/history leaves result unchanged;
24. silence uses fallback without penalty/narration.

### Ping-pong/exclusion

25. exact last-four A,B,A,B continuing to A/B routes third with Trace Rule PingPongBreak;
26. B,A,B,A works symmetrically;
27. A,B,A,C is not ping-pong;
28. A,A,B,A is not ping-pong;
29. fewer than four opportunities cannot trigger;
30. social intent already targeting absent third preserves original Rule;
31. no-control history naturally surfaces never-seen third via recency;
32. no exclusion timer/maximum-gap/equal-turn guard exists;
33. override appears in intended-vs-final trace fields.

### Authority/scope

34. no line/word/token counters;
35. no score/weight/probability fields;
36. social intent can create unequal distribution absent exact guard;
37. Proposal does not mutate Context OpportunityCharacterId;
38. Proposal cannot trigger Performer;
39. precommit Evaluation has no apply/promote operation;
40. semantic Proposal reusable by another strategy without least-intervention Rule enum;
41. Candidate control never becomes truth/state/observation authority;
42. no World Resolver behavior;
43. no relationship/Pressure/private-state input;
44. no Access decisions/full fixture/Production truth input;
45. no TakeId/CommitId allocation;
46. no Integrity/State Interpreter/State Authority behavior;
47. no provider/model/AI dependency.

### Determinism/regression

48. repeated evaluation identical;
49. inputs not mutated;
50. Candidate address order deterministic;
51. strategy distinct from future E0-D round-robin;
52. frozen Missing Raft StructuredContextHash unchanged;
53. frozen Missing Raft RenderedContextHash unchanged;
54. frozen Missing Raft ECJ-1 9112 bytes/hash unchanged;
55. all existing 173 Core tests green;
56. Missing Raft Harness PASS/0;
57. generic smoke Harness PASS/0.

Impossible malformed immutable authority objects may be covered by defensive review/reflection without public bypass constructors.

## 32. Harness behavior

No live provider execution or self-running Scene loop.

Existing Harness output remains unchanged.

Core tests exercise the pure proposal/evaluation using upstream validated Context/Candidate objects.

Effective Scene-loop integration waits for downstream Integrity/State/Take/atomic-commit contracts.

## 33. ARM64/battery suitability

Tiny deterministic CPU work only: validate three IDs/roster, inspect narrow typed control, scan E0 opportunity history, inspect last four for ping-pong, perform ordinal comparisons.

No network/background task/AI/GPU/NPU/filesystem/polling/embedding/semantic model. No NPU claim.

## 34. Explicit exclusions

No final/post-E0 Director mechanism; local semantic/hybrid Director; provider/model Director role; prompt-based reasoning; VisibleText semantic parsing; relationship/pressure relevance projection; Scene ending policy; World Resolver/observation; effective opportunity mutation; Scene loop; provider Performer execution; Integrity Validator; Take semantics/IDs; State Interpreter/Authority; ProductionState/StateHash; atomic commit/persistence; E0-D round-robin implementation; E0-E playwright execution; final Stage/Director Glass UX; WinUI/Windows AI/NPU/packaging/WACK/Store.

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
18. causal provenance/attempt identity/postcommit recomputation;
19. E0-D semantic-output isolation;
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

1. Patch 0007 follows validated Performer candidate output and precedes downstream effective-opportunity application;
2. Director output here is pure Proposal/Evaluation, not Current Opportunity mutation;
3. Missing Raft opening VOSS remains fixture authority;
4. E0 reference Director is deterministic/model-free/least-intervention and provisional, preserving ODR-12;
5. semantic proposal contract `ensemble.e0.director.opportunity.v1`;
6. strategy `ensemble.e0.director.least-intervention.v1`;
7. Proposal semantic fields are ContractVersion, StrategyContract, SceneId, SourceCharacterId, SourceContextPacketId, SelectedCharacterId only;
8. strategy-specific Rule exists only in local trace, so E0-D can reuse Proposal shape with a different trace/strategy;
9. inputs limited to safe Context structural IDs/roster + Candidate identity/control + immutable effective opportunity history;
10. exactly three Context roster Characters form E0 hard eligible set;
11. VisibleText/private Character state/Production truth/relationship text/Pressure text/Access decisions/provenance are not selection inputs;
12. Context/Candidate identity must bind and history tail must equal source;
13. ordinary least intervention honors nomination, otherwise direct address, otherwise complete-roster recency fallback;
14. multiple addresses use least-recent within addressed pool;
15. source is not hard-excluded; recency naturally prevents immediate repeat when alternatives exist;
16. least-recent uses only final history index + ordinal tie-break;
17. only automatic social-intent override is exact last-four A↔B continuation, proposing absent third Character;
18. social intent already targeting absent third prevents override;
19. no exclusion timer, maximum-gap guarantee, turn quota, dialogue/token count, numeric salience weight, probability, randomness, or LLM routing;
20. silence receives no penalty/obligation and naturally uses fallback when control empty;
21. Trace records intended/final structural selection and Rule; it is not Character-facing;
22. Proposal/control have zero effective routing authority before successful later causal commit;
23. precommit Evaluation is always discardable and may never be promoted directly;
24. any effective Director decision must be recomputed after successful atomic commit from the accepted source Performance/control and authoritative effective opportunity history;
25. E0-D round-robin remains separate strategy, not flag/bypass, while sharing semantic Proposal shape;
26. interaction/relationship/pressure semantic relevance and final structured/local/hybrid choice remain future design;
27. Scene ending/exhaustion remains later ODR-13/run-protocol authority;
28. no provider, World Resolver, Integrity, State, Take, commit, persistence, Scene loop, UI, Windows AI/NPU, or Store scope enters Patch 0007.

Implementation remains blocked until recursive audit completes and user explicitly approves the final proposal.
