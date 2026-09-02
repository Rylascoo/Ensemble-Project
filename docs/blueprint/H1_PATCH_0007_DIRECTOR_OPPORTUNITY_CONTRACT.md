# H1 Patch 0007 — E0 Director Opportunity Contract

Status: blueprint proposal 0.5 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0006
Branch: `h1-patch-0007-director-opportunity-blueprint`

## 1. Purpose

Define the next E0-A behavioral boundary after the validated Performer candidate contract:

```text
ContextPacket + CandidatePerformance + effective opportunity history
    -> deterministic Director input binding
        -> least-privilege DirectorOpportunityInput
            -> deterministic E0 Director proposal/evaluation
                -> later successful atomic causal commit
                    -> recompute Director evaluation from accepted source
                        -> later effective Current Opportunity
```

Patch 0007 defines one pure deterministic **E0 reference Director proposal strategy** plus the validated structural input boundary shared by Director strategies.

It does not mutate Current Opportunity and does not implement Integrity, State, Take, commit, Scene-loop, provider, or World Resolver authority.

## 2. Frozen authority

Blueprint 0.1 establishes:

- Director manages attention/opportunity only;
- central question: whose agency becomes salient next, and why?;
- possible inputs include hard eligibility, direct address, nomination, interaction relevance, relationship relevance, observable pressure, participation balance, recent repetition;
- no invented numeric weights are frozen;
- equal dialogue is not the goal;
- Director should detect accidental exclusion/repetitive two-character ping-pong/stalled attention without enforcing a turn quota;
- handoff is opportunity/social pressure, never obligation;
- selected Character may speak, act, evade, redirect, refuse, or remain silent;
- Director cannot write lines/outcomes, grant knowledge, create truth/belief, commit state, spend/retry, or author non-Character reality;
- World Resolver remains separate/unimplemented;
- deterministic hard eligibility/access/cost rules may not be delegated to an LLM;
- E0 provenance preserves Director inputs/decisions;
- E0-D requires deterministic round-robin as a separately labeled ablation;
- ODR-12 leaves the post-E0/final Director mechanism open: structured, local semantic, or hybrid;
- E0-A preparation explicitly requires Director inputs, prohibitions, responsibilities, and a least-intervention rule.

Least intervention for this E0 slice means:

> Preserve Performer/social intention by default. Intervene only for hard structural constraints or a clear degenerate routing pattern. Do not optimize plot, emotional outcome, or equal dialogue.

Scene exhaustion stays outside Patch 0007 because ODR-13/run termination remains open.

Patch 0006 additionally freezes:

- Candidate control = optional addressed Character IDs + optional nomination;
- control is Performer intent metadata, not truth/state/observation/Director authority/history;
- precommit control cannot create effective routing;
- speculative Director work before commit is discardable only;
- associated control may become a later non-binding Director input only after successful causal commit;
- causally used control + Director decision must be reconstructable.

## 3. E0-only scope and ODR-12 preservation

Patch 0007 freezes one structured deterministic E0 reference strategy only.

It intentionally does not establish the post-E0 product Director.

Using a model-free E0 reference avoids adding a second semantic-model variable to the E0-A same-model Performer reference condition.

Future structured/local-semantic/hybrid strategies remain open after E0 evidence.

## 4. Opening opportunity remains fixture authority

Missing Raft opening opportunity remains exactly `VOSS`.

Director does not create/reinterpret opening opportunity.

Patch 0007 begins only after an already-effective opportunity produces a structurally valid CandidatePerformance.

Opening fixture provenance stays distinct from later Director results.

## 5. Validated least-privilege input boundary

Director strategy code must **not** receive the full ContextPacket or CandidatePerformance.

Instead freeze:

```text
DirectorOpportunityInput
- SceneId
- SourceCharacterId
- SourceContextPacketId
- RosterCharacterIds
- AddressedCharacterIds
- NominatedCharacterId
- OpportunityHistory
```

Construction authority:

```text
DirectorOpportunityInput.Bind(
    ContextPacket currentContext,
    CandidatePerformance currentCandidate,
    ImmutableArray<CharacterId> opportunityHistory)
    -> DirectorOpportunityInput
```

`Bind(...)` validates cross-boundary identity/roster/history invariants, copies only required structural values into immutable storage, and exposes no private Context record text or Candidate VisibleText to the strategy.

`DirectorOpportunityInput` has no public constructor.

This boundary is also the intended input for the later E0-D round-robin strategy, so Full Ensemble versus round-robin varies the strategy rather than the input disclosure surface.

## 6. Binding invariants

`Bind(...)` fails closed unless:

1. Context non-null/initialized;
2. Candidate non-null/initialized;
3. Context SubjectCharacterId == Context OpportunityCharacterId;
4. Candidate SubjectCharacterId == Context SubjectCharacterId;
5. Candidate ContextPacketId == Context ContextPacketId;
6. Context roster initialized and exactly three E0 Characters;
7. roster Character IDs initialized/unique;
8. source appears exactly once in roster;
9. Candidate address/nomination IDs initialized, exact/case-sensitive roster members, not source, addressed IDs duplicate-free;
10. opportunityHistory non-default/non-empty;
11. every history Character ID initialized and exactly in roster;
12. history final entry == source Character.

The binder does not recanonicalize Context, recompute hashes, inspect provenance, rerun Access Control, or inspect Candidate VisibleText.

## 7. DirectorOpportunityInput is observational, not authority state

The input is a validated immutable snapshot of the structural signals the Director strategy may consume.

It is not:

- Current Opportunity state;
- accepted Take authority;
- Production history;
- truth;
- a Commit record;
- a State mutation;
- permission to trigger a Performer.

Its constructor is Core-internal so callers cannot fabricate a validated input through public construction.

## 8. Why VisibleText/private Context are structurally unavailable

The E0 least-intervention strategy receives `DirectorOpportunityInput`, not upstream rich objects.

Therefore it cannot read:

- Candidate VisibleText;
- Constitution/Disposition/Circumstance;
- observations/knowledge/beliefs/suspicions/memories/goals;
- relationships;
- Pressure text;
- Context rendering;
- denied Production state;
- Access decisions;
- fixture provenance.

This enforces by type what earlier drafts only stated by convention.

Future approved Director inputs may add specific bounded relevance signals without reopening full Context/Production access.

## 9. Contracts

Freeze:

```text
DirectorOpportunityContractVersion = ensemble.e0.director.opportunity.v1
DirectorStrategyContract = ensemble.e0.director.least-intervention.v1
```

Semantic proposal version and selection strategy are distinct concepts.

## 10. Semantic proposal shape

```text
DirectorOpportunityProposal
- ContractVersion
- SceneId
- SourceCharacterId
- SelectedCharacterId
```

The semantic Proposal contains no StrategyContract, ContextPacketId, Basis/Reason, score, provider/model data, prose, or mutation information.

Why:

- effective opportunity semantics are only Scene + source handoff + selected Character;
- selection strategy/context/control/history belong provenance/trace;
- E0-D round-robin can emit the exact same Proposal shape while changing only strategy behavior/trace.

Proposal has no public constructor.

Proposal is not effective Current Opportunity.

## 11. Least-intervention evaluation and trace

```text
DirectorOpportunityEvaluation
- Proposal
- Trace
```

```text
DirectorLeastInterventionTrace
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

`Rule`:

```text
PingPongBreak
Nomination
DirectAddress
RecencyFallback
```

Trace contains structural IDs/control only and no Character prose/private state/model reasoning.

Trace + StrategyContract provide the E0 “why.”

Trace is provenance/diagnostic material, never Character-facing context.

A future strategy may use the same Proposal contract with another strategy-specific trace.

## 12. Hard eligibility

For frozen E0, the exactly three Context roster Characters are the hard eligible set.

No separate eligibility score/model/LLM is introduced.

Later product eligibility may include presence/channel/incapacity/creator intervention, but hard eligibility remains deterministic.

## 13. Least-intervention strategy API

```text
LeastInterventionDirector.Propose(
    DirectorOpportunityInput input)
    -> DirectorOpportunityEvaluation
```

The strategy has no overload taking ContextPacket, CandidatePerformance, ValidatedFixture, Production state, free-form text, model output, or provider configuration.

## 14. Intended selection

### Nomination present

```text
IntendedPool = [NominatedCharacterId]
IntendedSelected = NominatedCharacterId
Rule = Nomination unless ping-pong override applies
```

### Else direct address present

```text
IntendedPool = AddressedCharacterIds
IntendedSelected = least-recently-opportunitied member of IntendedPool
Rule = DirectAddress unless ping-pong override applies
```

### Else

```text
IntendedPool = complete roster
IntendedSelected = least-recently-opportunitied member of IntendedPool
Rule = RecencyFallback unless ping-pong override applies
```

The source is not hard-excluded. Because history tail is source, ordinary recency naturally favors another eligible Character when one exists.

Nomination/address are social intent cues, not truth/outcome authority. Honoring them grants opportunity only.

## 15. Least-recent helper

Within a candidate pool:

1. never-seen Character is less recent than any seen Character;
2. among never-seen choose ordinally smallest CharacterId;
3. otherwise compare each Character's final history index;
4. choose smallest final index;
5. exact tie -> ordinal CharacterId.

No total-turn/dialogue/word/token counts, percentages, probabilities, or randomness.

## 16. Exact ping-pong guard

For exactly three E0 Characters, override social intention only when all are true:

1. history length >= 4;
2. last four = exact `A,B,A,B` or `B,A,B,A`;
3. A != B;
4. exactly one roster Character C is absent from those four;
5. IntendedSelected is A or B, not C.

Then:

```text
FinalPool = [C]
SelectedCharacterId = C
Rule = PingPongBreak
```

If intended selection already targets C, preserve it and preserve its social/fallback Rule.

Four opportunities are the minimum complete two-cycle repeated A↔B routing pattern, not a salience weight.

## 17. Accidental exclusion without quota

With three Characters:

- no control -> recency naturally surfaces never-seen/least-recent Characters;
- sustained explicit A↔B routing -> ping-pong guard surfaces the absent third;
- multiple direct addresses -> recency operates only within addressed social intent.

No arbitrary exclusion timer or ongoing maximum-gap guarantee is frozen.

History/trace still makes unusual exclusion measurable during E0 evaluation.

## 18. No turn quota

No equal-opportunity/equal-dialogue target, percentages, line counts, token counts, fairness score, or maximum-gap guarantee.

Social intent may create unequal distribution whenever exact ping-pong does not require intervention.

## 19. Silence/stalled attention

Patch 0006 silence produces empty control.

The Director strategy cannot inspect VisibleText, so silence naturally reaches RecencyFallback.

This changes attention without judging silence as failure or manufacturing narration.

Repeated stalled attention can be measured from Performance/provenance later, but Patch 0007 introduces no semantic “stalled” classifier.

## 20. Proposal versus effective Current Opportunity

```text
DirectorOpportunityProposal != effective Current Opportunity
```

Binding and Propose are pure/side-effect free.

A precommit Input/Evaluation may exist only as discardable preview/speculation.

Rejection, cancellation, provider failure, Integrity failure, or failed causal commit cannot change Current Opportunity or trigger another Performer.

## 21. Mandatory postcommit re-bind + recompute

Precommit DirectorOpportunityInput/Evaluation may **never** be promoted directly into effective routing authority.

Patch 0006 has no CandidateId/TakeId, so distinct attempts can share Context/control and cannot be safely distinguished by a precommit evaluation alone.

A later effective-opportunity boundary must:

1. successfully commit accepted Performance + approved consequences atomically;
2. take the accepted source CandidatePerformance/control and then-authoritative effective opportunity history;
3. call `DirectorOpportunityInput.Bind(...)` again after commit;
4. call the selected Director strategy again after commit;
5. only then apply the selected Character through the later effective-opportunity contract.

Precommit evaluation is always disposable.

Patch 0007 defines no TakeId/CommitId/application semantics.

## 22. E0 stale-state scope

E0 has one fixed co-present three-Character Scene.

The Director input contains no world/relationship/pressure projection that State Authority could change between candidate generation and postcommit recomputation.

Broader post-E0 stale-state/eligibility rules remain open.

## 23. Information/truth boundaries

Director input/proposal cannot:

- grant Character knowledge;
- make a claim true;
- promote a possibility;
- create observation/memory/belief;
- mutate relationship/pressure/world/Character state;
- resolve non-Character reality.

World Resolver remains separate.

The Proposal alone reveals only structural source/selected Character within the Scene. Trace remains non-Character-facing.

## 24. No hidden Director reasoning

No free-form Director rationale, chain-of-thought, scratchpad, stage direction, plot instruction, semantic score, or model reasoning.

Trace Rule + exact structural inputs are sufficient for reconstruction.

## 25. Determinism

Identical validated DirectorOpportunityInput + strategy contract must produce identical intended pool, intended selection, ping-pong decision, final pool, Proposal, and Trace.

No filesystem, clock, random, culture, network, provider/model, AI inference, GPU/NPU, or global mutable state.

## 26. E0-D round-robin isolation

Round-robin remains a separate strategy, never a flag in least-intervention.

It should consume the same `DirectorOpportunityInput` and emit the same semantic `DirectorOpportunityProposal` contract, while ignoring social control under its separately frozen cyclic rule.

Thus E0-D changes selection strategy only, not input disclosure or semantic output shape.

Patch 0007 does not implement round-robin yet.

## 27. ODR-12 remains open

Post-E0 structured/local-semantic/hybrid design remains unresolved until evidence.

Nothing in Patch 0007 freezes the final Director implementation.

## 28. Public surface

Preferred:

```text
DirectorOpportunityInput.Bind(
    ContextPacket,
    CandidatePerformance,
    ImmutableArray<CharacterId>)
    -> DirectorOpportunityInput

LeastInterventionDirector.Propose(
    DirectorOpportunityInput)
    -> DirectorOpportunityEvaluation
```

All Director input/output/evaluation/trace models are public read-only and have no public constructors.

Only Bind constructs validated input; only strategy constructs validated proposal/evaluation.

No public operation applies Current Opportunity or triggers Performer.

## 29. Fail closed

Use a Director-specific exception domain.

Bind fails on null/uninitialized/mismatched Context/Candidate, non-three/duplicate/invalid roster, invalid control, default/empty/invalid history, history tail mismatch.

Strategy fails on invalid/default input defensively, empty pool, impossible pattern/selection invariant.

Errors expose structural diagnostics only and never echo Candidate VisibleText/private Context text.

Failure produces no alternate opportunity.

## 30. Required tests

Use upstream validated Context/Candidate paths and canonical fixtures.

### Input boundary

1. opening Missing Raft VOSS remains fixture authority;
2. Bind subject/context/history invariants pass for valid Voss input;
3. Context/Candidate subject mismatch fails;
4. ContextPacketId mismatch fails;
5. Context subject/opportunity mismatch fails;
6. roster exactly three/unique/initialized/source once;
7. control IDs roster-bound/non-self/duplicate-free;
8. history non-default/non-empty/roster-bound/tail==source;
9. Input constructor non-public;
10. Input exact public fields contain no VisibleText/private Context/Rendering/Access/provenance;
11. strategy public API accepts DirectorOpportunityInput only;
12. strategy cannot receive ContextPacket/CandidatePerformance directly.

### Semantic proposal/trace

13. Proposal exact fields ContractVersion/SceneId/SourceCharacterId/SelectedCharacterId only;
14. Proposal has no StrategyContract/Basis/ContextPacketId/prose/score;
15. Proposal constructor non-public;
16. trace contains strategy/source ContextPacketId/control/history/intended/final IDs/Rule;
17. trace contains no VisibleText/private records/provider/model reasoning.

### Least intervention

18. nomination -> nominated Character absent ping-pong override;
19. nomination defines ordinary intent before addresses;
20. one address -> addressed Character;
21. multiple addresses -> least-recent in addressed pool;
22. empty control -> least-recent complete roster;
23. fallback naturally avoids source without hard exclusion;
24. never-seen beats seen;
25. ordinal tie-break deterministic;
26. social intent may choose globally more-recent Character;
27. unaddressed less-recent Character does not override social pool absent ping-pong;
28. upstream VisibleText mutation cannot affect DirectorOpportunityInput or result;
29. silence uses fallback.

### Ping-pong/exclusion

30. A,B,A,B continuing A/B -> absent third/PingPongBreak;
31. B,A,B,A symmetric;
32. A,B,A,C not ping-pong;
33. A,A,B,A not ping-pong;
34. fewer than four never triggers;
35. intended third Character preserves original Rule;
36. no-control history surfaces unseen third through recency;
37. no exclusion timer/equal-turn/max-gap guard exists;
38. trace preserves intended vs final override.

### Authority/causal

39. Binding/Proposal do not mutate Context/Candidate/history;
40. no apply/trigger operation;
41. no precommit promote/apply surface;
42. semantic Proposal reusable by round-robin strategy without least-intervention enum;
43. no truth/state/observation/World Resolver authority;
44. no relationship/Pressure/private-state input;
45. no TakeId/CommitId/Integrity/State behavior;
46. no provider/model/AI dependency;
47. no line/token/score/probability fields.

### Determinism/regression

48. repeated bind/propose identical;
49. strategy does not mutate Input;
50. least-intervention strategy distinguishable from round-robin by StrategyContract/trace, not Proposal shape;
51. frozen Missing Raft StructuredContextHash unchanged;
52. frozen Missing Raft RenderedContextHash unchanged;
53. frozen ECJ-1 9112 bytes/hash unchanged;
54. all existing 173 Core tests green;
55. Missing Raft Harness PASS/0;
56. smoke Harness PASS/0.

## 31. Harness

No live provider execution or Scene loop. Existing Harness output unchanged.

Core tests exercise Bind + pure least-intervention Propose only.

Effective loop waits for downstream causal authority.

## 32. ARM64/battery

Tiny deterministic CPU work over three IDs/control/history. No network/background/AI/GPU/NPU/filesystem/polling.

No NPU claim.

## 33. Explicit exclusions

No final Director; local semantic/hybrid Director; provider Director; prompt reasoning; VisibleText parsing; relationship/pressure relevance; Scene ending; World Resolver/observation; effective opportunity mutation; Scene loop; provider Performer; Integrity; Take semantics/IDs; State Interpreter/Authority; ProductionState/StateHash; atomic commit/persistence; round-robin implementation; playwright execution; Director Glass/Stage UX; WinUI/Windows AI/NPU/packaging/WACK/Store.

## 34. Recursive audit dimensions

Restart after every correction and test:

1. frozen Director law;
2. least intervention;
3. ODR-12;
4. Patch 0006 control/commit law;
5. least-privilege input disclosure;
6. Director vs Performer/World Resolver/Integrity/State/Take;
7. proposal vs effective opportunity;
8. opening fixture authority;
9. hard eligibility;
10. social intent/non-obligation;
11. accidental exclusion/ping-pong/stall;
12. no quota/fake precision;
13. untrusted-content isolation;
14. truth/privacy boundaries;
15. causal attempt identity/postcommit rebind;
16. E0-D same-input/same-output-shape isolation;
17. E0-A model-confound isolation;
18. public API/non-forgeability/minimality;
19. fail closed;
20. immutable determinism;
21. tests;
22. ARM64;
23. scope/hygiene;
24. E0-B/C/D/E/F/G compatibility.

Approval only after a full pass finds zero material corrections/worthwhile improvements.

## 35. Material approval decisions

Approval would freeze only:

1. Patch 0007 as E0 Director proposal boundary after Performer candidate output;
2. no effective Current Opportunity mutation in Patch 0007;
3. opening VOSS remains fixture authority;
4. E0 strategy deterministic/model-free/least-intervention/provisional, preserving ODR-12;
5. validated least-privilege `DirectorOpportunityInput` shared by Director strategies;
6. Bind is the only public input construction path and copies only Scene/source/context ID/roster/control/history;
7. strategy cannot access Context private state or Candidate VisibleText by type;
8. exactly three roster Characters form E0 hard eligible set;
9. semantic Proposal version `ensemble.e0.director.opportunity.v1` with fields ContractVersion/SceneId/SourceCharacterId/SelectedCharacterId only;
10. strategy identity/reasoning/context ID/control/history stay in trace, not semantic Proposal;
11. least-intervention strategy `ensemble.e0.director.least-intervention.v1`;
12. ordinary intent: nomination, else addressed-pool recency, else complete-roster recency fallback;
13. source not hard-excluded; recency naturally deprioritizes it;
14. recency = never-seen first, then oldest last history index, ordinal tie;
15. only automatic social-intent override = exact last-four A↔B continuation to absent third;
16. no exclusion timer/max-gap/turn quota/dialogue/token counts/scores/weights/probability/random/LLM routing;
17. silence naturally uses fallback and is never penalized/rewritten;
18. Trace stores intended/final selection and Rule, never Character-facing;
19. precommit Input/Evaluation always discardable and never promotable;
20. later effective Director use must re-Bind and recompute **after** successful atomic source commit;
21. E0-D round-robin later consumes same Input/emits same Proposal shape under separate strategy/trace;
22. interaction/relationship/pressure relevance and structured/local/hybrid post-E0 Director remain open;
23. Scene ending remains later ODR-13 authority;
24. no provider, World Resolver, Integrity, State, Take, commit, persistence, Scene loop, UI, Windows AI/NPU, or Store scope enters Patch 0007.

Implementation remains blocked until recursive audit completes and user explicitly approves the final proposal.
