# H1 Patch 0007 — E0 Director Opportunity Contract

Status: blueprint proposal 0.2 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
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
- the exact E0-A preparation explicitly requires Director inputs, responsibilities, prohibitions, and a **least-intervention rule**.

The recovered E0 reasoning gives least-intervention its operational meaning for this reference slice:

> Preserve Performer/social intention by default. Intervene mainly for hard constraints, continuity, degenerate repetition, accidental exclusion, or Scene exhaustion. Do not optimize plot twists or force outcomes.

Scene exhaustion remains outside Patch 0007 because ODR-13/run termination is not yet frozen. The selector is simply not invoked once a later run protocol has ended the Scene.

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

1. one structured, deterministic E0 reference proposal strategy;
2. one semantic proposal contract;
3. one typed local trace sufficient to reconstruct E0 selection;
4. exact least-intervention/anti-degeneracy behavior needed for the reference experiment.

It does **not** resolve ODR-12 for the product after E0.

The reference strategy is model-free specifically to avoid adding a second semantic-model variable to the E0-A same-model Performer reference condition. A future product Director may be structured, local-semantic, hybrid, or replaced if E0 evidence supports that.

## 4. Opening opportunity remains fixture authority

Missing Raft fixture opening opportunity remains exactly `VOSS`.

Patch 0007:

- does not select the opening Character;
- does not overwrite fixture initial opportunity;
- is invoked only after an effective opportunity has already produced a structurally valid candidate;
- keeps opening fixture provenance distinct from later Director proposals/decisions.

## 5. Public reference input

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

Using `ImmutableArray<CharacterId>` avoids a mutable caller-owned list changing during deterministic evaluation and avoids inventing a speculative Take/Commit persistence DTO.

## 6. Why VisibleText is excluded

The frozen Director may eventually consider current interaction relevance, but Patch 0007 does not infer it from free-form candidate prose because:

- Patch 0006 already provides explicit typed social-handoff metadata;
- Performance prose is untrusted creative content;
- semantic parsing would introduce another model or a new language ontology;
- doing so now would confound E0 before the behavioral architecture is measured;
- hidden semantic parsing could become a covert authority path from prose into routing;
- ODR-12 explicitly preserves post-E0 structured/local-semantic/hybrid choice.

Changing VisibleText while preserving candidate identity/control inputs must not change the Patch 0007 proposal.

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

The deterministic spine does not yet expose a Director-specific, authority-safe relevance projection for those concepts. Inventing one here would collapse selection with Access/State/semantic interpretation.

This is a narrow E0 reference simplification, not a product judgment that those signals are unimportant.

## 8. Contracts

Freeze:

```text
DirectorOpportunityContractVersion = ensemble.e0.director.opportunity.v1
DirectorStrategyContract = ensemble.e0.director.least-intervention.v1
```

The semantic proposal contract and strategy are named separately so E0-D/post-E0 strategy changes do not automatically redefine the proposal object.

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
InitialExclusionRecovery
PingPongBreak
Nomination
DirectAddress
RecencyFallback
```

The proposal contains no:

- line or prose direction;
- emotional instruction;
- required outcome;
- world event;
- relationship/pressure/state mutation;
- confidence or salience score;
- provider/model data;
- cost/retry authority;
- hidden chain-of-thought.

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

Trace contains structural IDs/control only. It contains no VisibleText, private Character records, denied IDs, truth records, provider credentials, model reasoning, or free-form Director rationale.

The distinction between `IntendedSelectedCharacterId` and final `SelectedCharacterId` makes least-intervention overrides auditable rather than silently replacing Performer social intent.

Trace is local/provenance material and is never Character-facing context.

## 11. Binding and structural invariants

Fail closed unless:

1. Context is non-null and initialized;
2. Candidate is non-null and initialized;
3. Context SubjectCharacterId equals OpportunityCharacterId;
4. Candidate SubjectCharacterId equals Context SubjectCharacterId;
5. Candidate ContextPacketId equals Context ContextPacketId;
6. roster is initialized, contains exactly the frozen E0 three Characters, IDs initialized/unique;
7. subject appears exactly once in roster;
8. Candidate addressed/nomination IDs are initialized, exact/case-sensitive roster members, not source, with no duplicate addresses;
9. opportunityHistory is non-default and non-empty;
10. every history ID is initialized and exactly in current roster;
11. history final element equals the source Character.

The selector does not recanonicalize Context, recompute hashes, inspect provenance, or rerun Access Control.

## 12. E0 hard eligibility

For the frozen three-Character co-present Scene, Context roster is the Director's hard eligible set.

Patch 0007 introduces no separate eligibility model/score/LLM.

The source Character is excluded from ordinary fallback selection because the E0 roster contains two other eligible Characters. Explicit Candidate control already cannot target self under Patch 0006.

Future product eligibility may consider presence, communication channel, incapacity, creator intervention, or other conditions. Those remain later authority design and deterministic when hard.

## 13. Least-intervention strategy overview

The selector first determines the **socially intended result** from typed control. It then asks only whether a narrow structural anti-degeneracy guard must override that result.

Exact order:

1. derive intended social/fallback candidate pool;
2. derive intended selected Character;
3. apply one-time initial-exclusion recovery if required;
4. otherwise apply two-Character ping-pong break if required;
5. otherwise preserve intended selection unchanged.

This order means Director intervention is exceptional and auditable.

It does not optimize drama, plot surprise, pacing, moral outcome, or equal line counts.

## 14. Intended selection from social control

### 14.1 Nomination present

If valid `NominatedCharacterId` is present:

```text
IntendedCandidatePool = [NominatedCharacterId]
IntendedSelectedCharacterId = NominatedCharacterId
IntendedBasis = Nomination
```

### 14.2 Otherwise direct address present

If one or more addressed Characters exist:

```text
IntendedCandidatePool = AddressedCharacterIds
IntendedSelectedCharacterId = least-recently-opportunitied member of IntendedCandidatePool
IntendedBasis = DirectAddress
```

### 14.3 Otherwise fallback

```text
IntendedCandidatePool = roster excluding SourceCharacterId
IntendedSelectedCharacterId = least-recently-opportunitied member of IntendedCandidatePool
IntendedBasis = RecencyFallback
```

Nomination/address are social intent cues, not truth or outcome authority. Honoring them gives the nominated/addressed Character an opportunity only; it does not obligate response.

They may still be overridden by the narrow anti-degeneracy guards below.

## 15. Initial-exclusion recovery guard

The Director must detect a Character who has been accidentally absent from opportunity altogether without enforcing ongoing equality.

For E0's exactly three Characters:

```text
InitialExclusionWindow = 6 effective opportunities
```

When `OpportunityHistory.Length >= 6`, find roster Characters that have **never appeared anywhere in opportunityHistory**.

If one or more never-seen Characters exist and the intended selection is not one of them:

```text
FinalCandidatePool = never-seen Characters excluding SourceCharacterId
SelectedCharacterId = ordinally smallest FinalCandidatePool CharacterId
Basis = InitialExclusionRecovery
```

If the intended selection already targets a never-seen Character, no override occurs and the social basis remains Nomination/DirectAddress/RecencyFallback.

Once every roster Character has appeared at least once, this guard is permanently irrelevant for that history. It does not create a recurring maximum-gap quota.

The six-opportunity threshold is an E0-only structural guard equal to two complete three-Character roster spans; it is not a post-E0 product constant or a numeric salience weight.

## 16. Two-Character ping-pong guard

For E0's three-Character roster, detect a degenerate two-Character exchange using the last four effective opportunities.

Ping-pong exists only when all are true:

1. `OpportunityHistory.Length >= 4`;
2. last four IDs alternate exactly `A,B,A,B` or `B,A,B,A`;
3. `A != B`;
4. exactly one roster Character `C` is absent from those four;
5. intended selection would choose `A` or `B` again rather than `C`.

Then:

```text
FinalCandidatePool = [C]
SelectedCharacterId = C
Basis = PingPongBreak
```

If social intent already selects `C`, no override occurs and the social basis remains intact.

This is a narrow structural anti-degeneracy rule, not a general fairness score.

## 17. Least-recently-opportunitied helper

Within a candidate pool:

1. a Character never appearing in history is less recent than any appearing Character;
2. among never-seen Characters choose ordinally smallest CharacterId;
3. otherwise compare each Character's final index in history;
4. choose smallest final index (oldest most-recent opportunity);
5. exact tie uses ordinal CharacterId.

No total turn count, dialogue count, word count, token count, percentage, probability, or random value is used.

## 18. No turn quota

Patch 0007 deliberately does not:

- target equal opportunities;
- target equal dialogue;
- maintain percentages;
- count words/tokens;
- assign fairness scores;
- route merely because someone is “behind” an ongoing count.

The initial-exclusion guard only guarantees that an eligible Character is not absent forever from the Scene's opportunity grammar; once all three have appeared it ceases to matter. The ping-pong guard only breaks a specific repetitive structural loop.

Explicit social intention otherwise wins even if it gives one Character more opportunities than another.

## 19. Silence

Patch 0006 silence is empty VisibleText + empty/null control.

Patch 0007 does not inspect VisibleText, so a silent candidate naturally produces fallback intent and is subject to the same narrow guards as any other candidate.

Silence is not failure, punishment, or a command to end the Scene. The next selected Character receives opportunity only.

## 20. Proposal versus effective Current Opportunity

Constitutional distinction:

```text
DirectorOpportunityProposal != effective Current Opportunity
```

`ProposeNext(...)` is pure and side-effect free.

It may be computed speculatively before commit only because Patch 0006 explicitly permits discardable zero-authority Director computation.

A proposal may become effective only through a later approved orchestration/causal boundary **after** the source Performance and all approved consequences successfully commit atomically.

On rejection, cancellation, technical failure, or failed causal commit:

- no Current Opportunity changes;
- no next Performer is triggered;
- proposal cannot survive as effective routing state;
- speculative evaluation may remain diagnostics/provenance only where required.

Patch 0007 implements no effective opportunity mutation.

## 21. Causal consistency after commit

Because E0 has one frozen co-present three-Character Scene and Patch 0007 consumes only roster/control/opportunity history, the reference proposal does not depend on mutable world/relationship/pressure projections that might change during State Authority commit.

The later causal layer may therefore either compute the pure proposal after commit or precompute it speculatively and apply it only if the source commit succeeds and the required Context/Candidate/history binding remains valid.

No broader post-E0 stale-state rule is frozen here.

## 22. No hidden Director prose/reasoning

Reference Director output is typed deterministic data only.

No free-form Director reasoning, chain-of-thought, scratchpad, natural-language stage direction, plot instruction, generated rationale, or semantic hidden score is created.

`Basis + typed inputs + strategy contract` are sufficient for E0 reconstruction.

Future Director Glass may render human-readable explanation from provenance outside this patch without exposing private chain-of-thought.

## 23. Information-authority boundary

Director is an orchestration role, not a Character and not truth authority.

Patch 0007 sees only:

- safe Context structural IDs/roster;
- Candidate structural identity/control;
- effective opportunity history.

It does not receive denied Character state or full Production truth merely because it is Director.

Proposal grants no Character knowledge. Stage/Character context receives only later effective opportunity, not the private trace.

## 24. Statement/truth/World Resolver boundary

Nomination/address mean only Performer social intent.

Director cannot:

- treat Character claims as fact;
- promote possibilities;
- infer hidden world events;
- create observations;
- mutate memory/belief/relationship/pressure;
- resolve weather/resources/non-Character reality.

World Resolver remains separate and unimplemented.

## 25. Determinism

Identical:

```text
Context structural identity/roster
+ Candidate identity/control
+ immutable OpportunityHistory
+ Director contract versions
```

must produce identical intended pool, intended selection, guard result, final pool, Basis, Proposal, and Trace.

No filesystem, clock, random source, culture, network, provider/model, AI inference, GPU/NPU, thread-global/process-global mutable state affects selection.

## 26. E0-D round-robin ablation isolation

Required deterministic round-robin remains a **separate strategy/implementation**, never a flag inside `least-intervention.v1`.

Round-robin deliberately ignores social handoff/least-intervention behavior and follows its own fixed cyclic order while other feasible variables remain matched.

Patch 0007 does not implement the E0-D ablation.

The reference fallback may resemble rotation when no social cues occur, but reference and round-robin remain experimentally distinct because the reference honors social intent and only intervenes through the two narrow guards.

## 27. ODR-12 remains open

Patch 0007 is E0 experimental machinery only.

It does not decide whether post-E0 Director should use:

- structured rules;
- local semantic judgment;
- hybrid selection;
- richer relevance projections;
- a different strategy entirely.

E0 evidence must inform that later decision. Deterministic hard eligibility/authority remains required regardless of later semantic assistance.

## 28. Proposed public surface

```text
DirectorOpportunitySelector.ProposeNext(
    ContextPacket currentContext,
    CandidatePerformance currentCandidate,
    ImmutableArray<CharacterId> opportunityHistory)
    -> DirectorOpportunityEvaluation
```

`DirectorOpportunityProposal`, `DirectorOpportunityTrace`, and `DirectorOpportunityEvaluation` are public read-only outputs with Core-internal constructors.

Input `ImmutableArray<CharacterId>` is caller-provided observational attention history, not an authority object and not a speculative persistence abstraction.

No public method applies the proposal or mutates Current Opportunity.

## 29. Fail closed

Use one small Director-specific exception domain.

Fail on:

- null/uninitialized Context;
- null/uninitialized Candidate;
- Context subject/opportunity mismatch;
- Candidate subject/context mismatch;
- roster not exactly three for this E0 strategy;
- invalid/duplicate roster;
- source missing/duplicated;
- invalid Candidate control relative to roster;
- default/empty history;
- unknown/uninitialized history ID;
- history tail != source;
- empty intended/final candidate pool;
- impossible guard/selection invariant.

Errors expose structural diagnostics only and never echo Candidate VisibleText/private Context record text.

Failure produces no alternate fictional action and no implicit fallback opportunity.

## 30. Required tests

Use validated Patch 0005 ContextPacket and Patch 0006 CandidatePerformance paths; no duplicated fixture JSON.

### Binding/opening/surface

1. Missing Raft opening opportunity remains fixture VOSS and selector does not create opening state;
2. Context/Candidate SubjectCharacterId must match;
3. Candidate ContextPacketId must match Context;
4. Context SubjectCharacterId must equal OpportunityCharacterId;
5. roster exactly three/unique/initialized/source once;
6. history immutable/non-default/non-empty;
7. history IDs must be roster members/initialized;
8. history tail must equal source;
9. output constructors non-public;
10. no public apply/mutate/trigger operation;
11. Proposal exact fields only;
12. Trace contains no VisibleText/private records/provider/model reasoning.

### Least-intervention/social intent

13. Voss nomination of Wren -> WREN/Nomination when no guard overrides;
14. nomination overrides ordinary address intent;
15. single address -> addressed Character/DirectAddress;
16. multiple addresses choose least-recent within addressed pool;
17. no control -> least-recent non-source fallback;
18. never-seen beats seen inside ordinary pool;
19. ordinary ties use ordinal ID;
20. explicit nomination may choose globally more-recent Character when no guard requires intervention;
21. direct-address pool may exclude globally less-recent unaddressed Character when no guard requires intervention;
22. VisibleText mutation with same identity/control/history leaves selection unchanged;
23. silence uses fallback without penalty/narration.

### Anti-degeneracy

24. before six opportunities, never-seen Character does not by itself override social intent;
25. at six+ opportunities, globally never-seen third Character overrides intended A/B target with InitialExclusionRecovery;
26. if intended selection already targets never-seen Character, no exclusion override/basis replacement;
27. once every Character appeared, initial-exclusion guard never reactivates merely because one later becomes infrequent;
28. exact A,B,A,B last-four pattern continuing to A/B routes third Character with PingPongBreak;
29. A,B,A,C is not ping-pong;
30. A,A,B,A is not ping-pong;
31. if social intent already targets third Character, ping-pong guard does not replace social basis;
32. ping-pong detection uses opportunity IDs only, never prose;
33. guard override is visible in Trace intended-vs-final fields.

### No quota/authority leakage

34. no line/word/token counts;
35. no numeric score/weight/probability field;
36. social intent can produce unequal opportunity distribution absent narrow guards;
37. Proposal alone does not mutate Context OpportunityCharacterId;
38. Proposal alone cannot trigger Performer;
39. no Candidate control becomes truth/state/observation authority;
40. no World Resolver behavior;
41. no relationship/Pressure/private-state input;
42. no Access decisions/full fixture/Production truth input;
43. no TakeId/CommitId allocation;
44. no Integrity/State Interpreter/State Authority behavior;
45. no provider/model/AI dependency.

### Determinism/regression

46. repeated evaluation identical;
47. inputs not mutated;
48. Candidate address order already canonical and selection deterministic;
49. `least-intervention.v1` distinct from future E0-D round-robin strategy;
50. frozen Missing Raft StructuredContextHash unchanged;
51. frozen Missing Raft RenderedContextHash unchanged;
52. frozen Missing Raft ECJ-1 9112 bytes/hash unchanged;
53. all existing 173 Core tests green;
54. Missing Raft Harness PASS/0;
55. generic smoke Harness PASS/0.

Impossible malformed immutable authority objects may be covered by defensive review/reflection without adding public bypass constructors.

## 31. Harness behavior

No live provider execution or self-running Scene loop is introduced.

Existing Harness output remains unchanged.

Core tests exercise the pure proposal/evaluation using upstream validated Context/Candidate objects.

Effective Scene-loop integration waits for downstream Integrity/State/Take/atomic-commit contracts so proposal application cannot outrun causal authority.

## 32. ARM64/battery suitability

Tiny deterministic CPU work only:

- validate three IDs/roster;
- inspect narrow typed control;
- scan E0 opportunity history;
- inspect at most last four for ping-pong;
- deterministic ordinal comparisons.

No network/background task/AI/GPU/NPU/filesystem/polling/embedding/semantic model.

No NPU claim.

## 33. Explicit exclusions

No:

- final/post-E0 Director mechanism;
- local semantic/hybrid Director;
- provider/model Director role;
- prompt-based Director reasoning;
- VisibleText semantic parsing;
- relationship/pressure relevance projection;
- Scene exhaustion/ending policy;
- World Resolver/observation;
- effective opportunity state mutation;
- Scene loop/orchestration;
- provider Performer execution;
- Integrity Validator;
- Take semantics/IDs;
- State Interpreter/Authority;
- ProductionState/StateHash;
- atomic causal commit/persistence/recovery;
- E0-D round-robin implementation;
- E0-E playwright execution;
- final Stage/Director Glass UX;
- WinUI/Windows AI/NPU/packaging/WACK/Store.

## 34. Recursive audit dimensions

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
14. no ongoing turn quota;
15. VisibleText/untrusted-content isolation;
16. privacy/access leakage;
17. statement-vs-truth;
18. causal provenance/reconstruction;
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

## 35. Material approval decisions

Approval would freeze only these E0 Patch 0007 decisions:

1. Patch 0007 follows validated Performer candidate output and precedes downstream Integrity/State/Take/commit integration;
2. Director output here is pure proposal/evaluation, not effective Current Opportunity;
3. Missing Raft opening VOSS remains fixture authority;
4. E0 reference Director is deterministic/model-free/least-intervention and provisional, preserving ODR-12;
5. semantic proposal contract `ensemble.e0.director.opportunity.v1`;
6. strategy `ensemble.e0.director.least-intervention.v1`;
7. inputs limited to safe Context structural IDs/roster + Candidate identity/control + immutable effective opportunity history;
8. exactly three roster Characters are hard eligible for this E0 strategy;
9. VisibleText/private Character state/Production truth/relationship text/Pressure text/Access decisions/provenance are not selection inputs;
10. Context/Candidate identity must bind and history tail must equal source;
11. ordinary least-intervention honors nomination first, otherwise direct address, otherwise non-source recency fallback;
12. multiple addresses use least-recent within addressed pool;
13. first narrow override is E0-only initial-exclusion recovery after six effective opportunities if a roster Character has never appeared;
14. initial-exclusion guard disables once all three have ever appeared and is not an ongoing quota;
15. second narrow override breaks exact last-four A,B,A,B two-Character ping-pong by proposing the absent third Character;
16. guards do not replace social basis when social intent already targets the recovery Character;
17. least-recent helper uses only final history index + ordinal tie-break;
18. no total-turn/dialogue/token counts, ongoing fairness quotas, numeric salience weights, probabilities, randomness, or LLM routing;
19. silence receives no penalty/obligation and naturally uses fallback when control empty;
20. Basis is typed `InitialExclusionRecovery | PingPongBreak | Nomination | DirectAddress | RecencyFallback`, no generated rationale;
21. Trace records intended and final structural inputs/selection so intervention is auditable and is not Character-facing;
22. proposal/control have zero effective routing authority before successful later causal commit;
23. rejection/cancellation/failure/rollback cannot establish next effective opportunity;
24. E0-D round-robin remains separate strategy, not flag/bypass;
25. interaction/relationship/pressure semantic relevance and final structured/local/hybrid choice remain permitted future design;
26. Scene ending/exhaustion remains later ODR-13/run-protocol authority;
27. no provider, World Resolver, Integrity, State, Take, commit, persistence, Scene loop, UI, Windows AI/NPU, or Store scope enters Patch 0007.

Implementation remains blocked until recursive audit completes and user explicitly approves the final proposal.
