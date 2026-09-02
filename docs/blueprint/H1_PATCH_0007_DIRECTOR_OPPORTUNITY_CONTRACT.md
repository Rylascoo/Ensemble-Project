# H1 Patch 0007 — E0 Director Opportunity Contract

Status: blueprint proposal 0.9 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0006
Branch: `h1-patch-0007-director-opportunity-blueprint`

## 1. Purpose

Define the next E0-A behavioral boundary after the validated Performer candidate contract:

```text
original source ContextPacket + CandidatePerformance + effective opportunity-event history
    -> deterministic Director input binding
        -> least-privilege DirectorOpportunityInput
            -> deterministic E0 Director proposal/evaluation
                -> later successful atomic causal commit
                    -> re-Bind original source ContextPacket + accepted Candidate
                       against then-authoritative opportunity-event history
                        -> recompute Director evaluation
                            -> later effective Current Opportunity
```

Patch 0007 defines one pure deterministic E0 reference Director proposal strategy plus the structural input boundary shared by Director strategies.

It does not mutate Current Opportunity and does not implement Integrity, State, Take, commit, Scene-loop, provider, or World Resolver authority.

## 2. Frozen authority

Blueprint 0.1 establishes:

- Director manages attention/opportunity only;
- central question: whose agency becomes salient next, and why?;
- possible inputs include hard eligibility, direct address, nomination, interaction relevance, relationship relevance, observable pressure, participation balance, and recent repetition;
- no invented numeric weights are frozen;
- equal dialogue is not the goal;
- Director should detect accidental exclusion, repetitive two-character ping-pong, and stalled attention without enforcing a turn quota;
- handoff is opportunity/social pressure, never obligation;
- selected Character may speak, act, evade, redirect, refuse, or remain silent;
- Director cannot write lines/outcomes, grant knowledge, create truth/belief, commit state, spend/retry, or author non-Character reality;
- World Resolver remains separate/unimplemented;
- deterministic hard eligibility/access/cost rules may not be delegated to an LLM;
- E0 provenance preserves Director inputs and decisions;
- E0-D requires deterministic round-robin as a separately labeled ablation;
- ODR-12 leaves the post-E0/final Director mechanism open: structured, local semantic, or hybrid;
- E0-A preparation requires Director inputs, prohibitions, responsibilities, and a least-intervention rule.

Least intervention for this E0 slice means:

> Preserve explicit Performer/social intention. Use deterministic recency only to resolve ambiguity or absence of social intent. Detect degenerate attention patterns for provenance/evaluation, but do not convert those diagnostics into an equal-turn scheduler or silently override explicit Character handoff.

E0 must be able to reveal repetitive ping-pong, exclusion, or mechanical routing rather than having the Director automatically hide those weaknesses.

Scene exhaustion stays outside Patch 0007 because ODR-13/run termination remains open.

Patch 0006 additionally freezes:

- Candidate control = optional addressed Character IDs + optional nomination;
- control is Performer intent metadata, not truth/state/observation/Director authority/history;
- precommit control cannot create effective routing;
- speculative Director work before commit is discardable only;
- associated control may become a later non-binding Director input only after successful causal commit;
- causally used control + Director decision must be reconstructable.

Frozen E0 provenance also preserves complete Character ContextPackets, which is required by the postcommit binding rule below.

## 3. E0-only scope and ODR-12 preservation

Patch 0007 freezes one structured deterministic E0 reference strategy only.

It intentionally does not establish the post-E0 product Director.

A model-free reference avoids adding a second semantic-model variable to the E0-A same-model Performer reference condition.

Future structured/local-semantic/hybrid strategies remain open after E0 evidence.

## 4. Opening opportunity remains fixture authority

Missing Raft opening opportunity remains exactly `VOSS`.

Director does not create or reinterpret opening opportunity.

Patch 0007 begins only after an already-effective opportunity produces a structurally valid CandidatePerformance.

Opening fixture provenance stays distinct from later Director results.

## 5. Validated least-privilege input boundary

Director strategy code must not receive full ContextPacket or CandidatePerformance.

Freeze:

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

Construction:

```text
DirectorOpportunityInput.Bind(
    ContextPacket sourceContext,
    CandidatePerformance sourceCandidate,
    ImmutableArray<CharacterId> opportunityHistory)
    -> DirectorOpportunityInput
```

Bind validates cross-boundary identity/roster/history invariants, copies only required structural values into immutable storage, and exposes no private Context text or Candidate VisibleText to the strategy.

DirectorOpportunityInput has no public constructor.

The later E0-D round-robin strategy is expected to consume the same input type so the experiment varies strategy rather than input disclosure.

## 6. Canonical input storage

Within validated DirectorOpportunityInput:

- RosterCharacterIds stored ordinally by CharacterId;
- AddressedCharacterIds stored ordinally by CharacterId;
- NominatedCharacterId remains nullable scalar;
- OpportunityHistory preserves exact effective opportunity-event order and is never sorted;
- SceneId, SourceCharacterId, SourceContextPacketId copy exactly from trusted upstream identity.

No trimming, inference, ranking, semantic rewriting, or hidden normalization occurs.

## 7. Binding invariants

Bind fails closed unless:

1. sourceContext non-null/initialized;
2. sourceCandidate non-null/initialized;
3. Context SubjectCharacterId == Context OpportunityCharacterId;
4. Candidate SubjectCharacterId == Context SubjectCharacterId;
5. Candidate ContextPacketId == Context ContextPacketId;
6. Context roster initialized and exactly three E0 Characters;
7. roster Character IDs initialized and unique;
8. source appears exactly once in roster;
9. Candidate address/nomination IDs initialized, exact/case-sensitive roster members, not source, addresses duplicate-free;
10. OpportunityHistory non-default and non-empty;
11. every history Character ID initialized and in roster;
12. history final entry == source Character.

Binder does not recanonicalize Context, recompute hashes, inspect provenance, rerun Access Control, or inspect Candidate VisibleText.

## 8. Exact OpportunityHistory semantics

OpportunityHistory records **effective opportunity establishments/events**, not provider attempts and not merely Character-ID transitions.

It includes:

- fixture-authored opening opportunity exactly once;
- every later newly authorized effective Character opportunity event through the future causal/orchestration boundary.

A newly established effective opportunity appends one history entry even if its CharacterId equals the previous history entry.

It does not append for provider retry/refusal/error, partial/cancelled generation, malformed candidate, rejected CandidatePerformance, request-another-take attempt, alternate candidate attempt while the existing opportunity remains in force, or speculative/precommit Director proposal/evaluation.

Therefore retries/rejections cannot distort recency, while genuinely re-established attention remains causally observable.

Patch 0007 defines this semantic input but does not implement persistence or mutation of it.

## 9. DirectorOpportunityInput is observational, not authority state

It is a validated immutable snapshot of structural signals a Director strategy may consume.

It is not Current Opportunity state, accepted Take authority, Production history, truth, Commit record, State mutation, or permission to trigger a Performer.

Its constructor is Core-internal.

## 10. Rich content is structurally unavailable to the strategy

LeastInterventionDirector receives DirectorOpportunityInput, not rich upstream objects.

It cannot read Candidate VisibleText, Character Constitution/Disposition/Circumstance, observations/knowledge/beliefs/suspicions/memories/goals, relationships, Pressure text, Context rendering, denied Production state, Access decisions, or fixture provenance.

Future approved Director inputs may add specific bounded relevance signals without reopening broad Context/Production access.

## 11. Contracts

Freeze:

```text
DirectorOpportunityContractVersion = ensemble.e0.director.opportunity.v1
DirectorStrategyContract = ensemble.e0.director.least-intervention.v1
```

Semantic proposal version and strategy are distinct.

## 12. Semantic proposal shape

```text
DirectorOpportunityProposal
- ContractVersion
- SceneId
- SourceCharacterId
- SelectedCharacterId
```

Proposal contains no StrategyContract, ContextPacketId, Basis/Reason, score, provider/model data, prose, control, history, or mutation information.

Strategy/context/control/history attribution belongs in trace/provenance.

A future E0-D round-robin strategy may emit the exact same Proposal shape.

Proposal has no public constructor and is not effective Current Opportunity.

## 13. Least-intervention evaluation and trace

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
- CandidatePoolCharacterIds
- SelectedCharacterId
- Rule
- NeverOpportunitiedCharacterIds
- RecentAttentionPattern
```

Rule:

```text
Nomination
DirectAddress
RecencyFallback
```

RecentAttentionPattern:

```text
None
RepeatedSameCharacter
TwoCharacterAlternation
```

Pattern diagnostics never change SelectedCharacterId.

NeverOpportunitiedCharacterIds is the ordinally stored subset of roster IDs absent from complete OpportunityHistory. It is descriptive evidence, not a fairness command.

Trace set-like collections are ordinal; OpportunityHistory stays exact order.

Trace contains no Character prose/private state/model reasoning and is never Character-facing context.

## 14. Hard eligibility

For frozen E0, the exactly three Context roster Characters are the hard eligible set.

No separate eligibility score/model/LLM.

Future product eligibility may include presence/channel/incapacity/creator intervention, but hard eligibility remains deterministic.

## 15. Strategy API

```text
LeastInterventionDirector.Propose(
    DirectorOpportunityInput input)
    -> DirectorOpportunityEvaluation
```

No overload accepts ContextPacket, CandidatePerformance, ValidatedFixture, Production state, free-form text, model output, or provider configuration.

## 16. Exact selection rule

### Nomination present

```text
CandidatePool = [NominatedCharacterId]
SelectedCharacterId = NominatedCharacterId
Rule = Nomination
```

Nomination is preserved without fairness override.

### Otherwise direct address present

```text
CandidatePool = AddressedCharacterIds
SelectedCharacterId = least-recently-opportunitied member of CandidatePool
Rule = DirectAddress
```

Direct address constrains the social pool; recency resolves ambiguity only among multiple addressed Characters.

### Otherwise

```text
CandidatePool = complete roster
SelectedCharacterId = least-recently-opportunitied member of CandidatePool
Rule = RecencyFallback
```

Source is not hard-excluded. Because history tail is source, recency naturally favors another eligible Character when one exists.

No pattern diagnostic overrides selection.

## 17. Least-recent helper

Within candidate pool:

1. never-seen Character is less recent than any seen Character;
2. among never-seen choose ordinally smallest CharacterId;
3. otherwise compare final history index;
4. choose smallest final index;
5. exact tie -> ordinal CharacterId.

No total-turn/dialogue/word/token counts, percentages, scores, probability, or randomness.

## 18. Structural attention diagnostics

Director detects structural routing patterns without automatically converting them into authority.

### Never-opportunitied roster members

Trace records roster Characters absent from complete OpportunityHistory. It does not label absence “accidental,” because intent cannot be established from history alone.

### Repeated same-Character attention

RecentAttentionPattern = RepeatedSameCharacter when history has at least two entries and final two IDs are equal.

### Two-character alternation

RecentAttentionPattern = TwoCharacterAlternation when history has at least four entries and final four are exact A,B,A,B with A != B.

Otherwise RecentAttentionPattern = None.

These diagnostics do not affect selection. They make the frozen Director concerns observable in E0 provenance without masking the behavior being tested.

## 19. No turn quota or fairness override

Patch 0007 has no equal-opportunity/equal-dialogue target, exclusion timer, maximum-gap guarantee, percentages, line/token counts, fairness score, or forced third-Character insertion.

Explicit social intention may legitimately produce unequal opportunity distribution.

If E0 shows stronger anti-degeneracy intervention is needed, that becomes evidence for a later Director revision.

## 20. Silence

Patch 0006 silence produces empty control.

Director cannot inspect VisibleText, so silence naturally reaches RecencyFallback.

Silence is not failure, punishment, or Scene-ending authority.

## 21. Proposal versus effective Current Opportunity

```text
DirectorOpportunityProposal != effective Current Opportunity
```

Bind and Propose are pure/side-effect free.

Precommit Input/Evaluation may exist only as discardable preview/speculation.

Rejection, cancellation, provider failure, Integrity failure, or failed causal commit cannot change Current Opportunity or trigger another Performer.

## 22. Mandatory postcommit re-Bind + recompute using original source ContextPacket

Precommit DirectorOpportunityInput/Evaluation may never be promoted directly into effective routing authority.

Patch 0006 has no CandidateId/TakeId; distinct attempts can share Context/control and cannot be safely distinguished by precommit evaluation alone.

In addition, approved consequences may change future Character context. A freshly recomposed postcommit ContextPacket is therefore **not** a substitute for the ContextPacket under which the accepted source Performance was actually produced.

A later effective-opportunity boundary must:

1. successfully commit accepted Performance + approved consequences atomically;
2. retain/recover the **exact original validated ContextPacket associated with that accepted source CandidatePerformance**;
3. use that original source ContextPacket + accepted CandidatePerformance/control + then-authoritative effective OpportunityHistory;
4. call DirectorOpportunityInput.Bind after commit;
5. call the selected Director strategy after commit;
6. only then apply SelectedCharacterId through the later effective-opportunity contract.

The original source ContextPacket identity must equal the accepted CandidatePerformance.ContextPacketId through the existing Bind invariant.

Frozen E0 provenance already requires complete Character ContextPackets, so this rule does not invent a new disclosure artifact.

Patch 0007 defines no TakeId/CommitId/application semantics.

## 23. Speculative versus causal provenance

A precommit evaluation, if preserved, is explicitly speculative diagnostic/provenance only.

The postcommit evaluation recomputed from the exact original source ContextPacket + accepted Candidate + authoritative opportunity history is the Director evaluation relevant to causal opportunity provenance.

Failed/rejected attempts may preserve speculative evaluations for E0 diagnostics but never as Scene routing history.

Storage format is later; the semantic distinction is frozen here.

## 24. E0 stale-state scope

E0 has one fixed co-present three-Character Scene.

Director input contains no mutable world/relationship/pressure projection. The only postcommit-varying Director input is the authoritative effective OpportunityHistory; the source ContextPacket remains the original packet tied to the accepted source Performance.

Broader post-E0 stale-state/eligibility rules remain open.

## 25. Information/truth boundaries

Director input/proposal cannot grant Character knowledge, make claims true, promote possibility, create observation/memory/belief, mutate relationship/pressure/world/Character state, or resolve non-Character reality.

World Resolver remains separate.

Proposal reveals only structural Scene/source/selected Character. Trace remains non-Character-facing.

## 26. No hidden Director reasoning

No free-form rationale, chain-of-thought, scratchpad, stage direction, plot instruction, semantic score, or model reasoning.

Trace Rule + structural diagnostics are sufficient for E0 reconstruction.

## 27. Determinism

Identical validated DirectorOpportunityInput + strategy contract produces identical candidate pool, selected Character, Rule, diagnostics, Proposal, and Trace.

No filesystem, clock, random, culture, network, provider/model, AI inference, GPU/NPU, or global mutable state.

## 28. E0-D round-robin isolation

Round-robin remains separate, never a flag in least-intervention.

It should consume the same DirectorOpportunityInput and emit the same semantic DirectorOpportunityProposal contract while ignoring social control under a separately frozen cyclic rule.

Thus E0-D changes strategy only, not input disclosure or semantic output shape.

Patch 0007 does not implement round-robin.

## 29. ODR-12 remains open

Post-E0 structured/local-semantic/hybrid design remains unresolved until evidence.

Nothing in Patch 0007 freezes final Director implementation.

## 30. Public surface

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

All Director input/output/evaluation/trace models are public read-only with no public constructors.

Only Bind constructs validated input; only strategy constructs validated Proposal/Evaluation.

No public operation applies Current Opportunity or triggers Performer.

## 31. Fail closed

Use a Director-specific exception domain.

Bind fails on null/uninitialized/mismatched Context/Candidate, non-three/duplicate/invalid roster, invalid control, default/empty/invalid history, or history tail mismatch.

Strategy fails on invalid/default input defensively, empty candidate pool, or impossible selection invariant.

Errors expose structural diagnostics only and never echo Candidate VisibleText/private Context text.

Failure produces no alternate opportunity.

## 32. Required tests

Use upstream validated Context/Candidate paths and canonical fixtures.

### Input/history boundary

1. opening Missing Raft VOSS remains fixture authority;
2. valid Voss Bind succeeds;
3. Context/Candidate subject mismatch fails;
4. ContextPacketId mismatch fails;
5. Context subject/opportunity mismatch fails;
6. roster exactly three/unique/initialized/source once;
7. control IDs roster-bound/non-self/duplicate-free;
8. history non-default/non-empty/roster-bound/tail==source;
9. history excludes retries/rejections/alternate attempts/speculation but permits a newly effective same-Character opportunity event;
10. Input constructor non-public;
11. Input exact fields contain no VisibleText/private Context/Rendering/Access/provenance;
12. roster stored ordinally;
13. addresses stored ordinally;
14. history exact event order preserved including repeated Character IDs;
15. strategy public API accepts DirectorOpportunityInput only;
16. strategy cannot receive ContextPacket/CandidatePerformance directly.

### Proposal/trace

17. Proposal exact fields ContractVersion/SceneId/SourceCharacterId/SelectedCharacterId only;
18. Proposal has no StrategyContract/Basis/ContextPacketId/prose/score/control/history;
19. Proposal constructor non-public;
20. trace contains strategy/source ContextPacketId/control/history/candidate pool/selection/Rule/diagnostics;
21. trace set ordering canonical and history event ordering exact;
22. trace contains no VisibleText/private/provider/model reasoning.

### Least-intervention selection

23. nomination -> nominated Character;
24. nomination wins over addresses;
25. one address -> addressed Character;
26. multiple addresses -> least-recent in addressed pool;
27. empty control -> least-recent complete roster;
28. fallback naturally avoids source without hard exclusion when alternatives exist;
29. never-seen beats seen;
30. ordinal tie-break deterministic;
31. nomination may choose globally more-recent Character;
32. direct-address pool may exclude globally less-recent unaddressed Character;
33. upstream VisibleText mutation cannot affect Input or result;
34. silence uses fallback.

### Diagnostics/no fairness authority

35. never-seen roster Character recorded diagnostically but does not override nomination/address;
36. final same,same effective events -> RepeatedSameCharacter;
37. final A,B,A,B -> TwoCharacterAlternation;
38. A,B,A,C -> None;
39. pattern diagnostic never changes selected Character;
40. no exclusion timer/equal-turn/max-gap/forced-third guard exists;
41. explicit A↔B nomination may continue despite diagnostic alternation, proving detection != obligation/fairness authority.

### Authority/causal

42. Bind/Propose do not mutate upstream/input;
43. no apply/trigger operation;
44. no precommit promote/apply surface;
45. postcommit effective use requires exact original source ContextPacket, accepted Candidate, authoritative history, re-Bind, and recompute;
46. freshly recomposed postcommit ContextPacket is not accepted as a substitute if its identity differs from Candidate.ContextPacketId;
47. speculative and causal evaluation provenance are distinguished;
48. semantic Proposal reusable by round-robin without least-intervention Rule taxonomy;
49. no truth/state/observation/World Resolver authority;
50. no relationship/Pressure/private-state input;
51. no TakeId/CommitId/Integrity/State behavior;
52. no provider/model/AI dependency;
53. no line/token/score/probability fields.

### Determinism/regression

54. repeated Bind/Propose identical;
55. strategy does not mutate Input;
56. least-intervention distinguishable from round-robin by strategy/trace, not Proposal shape;
57. frozen Missing Raft StructuredContextHash unchanged;
58. frozen Missing Raft RenderedContextHash unchanged;
59. frozen ECJ-1 9112 bytes/hash unchanged;
60. all existing 173 Core tests green;
61. Missing Raft Harness PASS/0;
62. smoke Harness PASS/0.

## 33. Harness

No live provider execution or Scene loop. Existing Harness output unchanged.

Core tests exercise Bind + pure Propose only.

Effective loop waits for downstream causal authority.

## 34. ARM64/battery

Tiny deterministic CPU work over three IDs/control/history. No network/background/AI/GPU/NPU/filesystem/polling.

No NPU claim.

## 35. Explicit exclusions

No final Director; local semantic/hybrid Director; provider Director; prompt reasoning; VisibleText parsing; relationship/pressure relevance; Scene ending; World Resolver/observation; effective opportunity mutation; Scene loop; provider Performer; Integrity; Take semantics/IDs; State Interpreter/Authority; ProductionState/StateHash; atomic commit/persistence; round-robin implementation; playwright execution; Director Glass/Stage UX; WinUI/Windows AI/NPU/packaging/WACK/Store.

## 36. Recursive audit dimensions

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
11. exclusion/ping-pong/stall detection vs authority;
12. no quota/fake precision;
13. untrusted-content isolation;
14. truth/privacy boundaries;
15. exact opportunity-event history semantics;
16. accepted-attempt/original-context identity + postcommit rebind/provenance;
17. E0-D same-input/same-output-shape isolation;
18. E0-A model-confound isolation;
19. public API/non-forgeability/minimality;
20. fail closed;
21. immutable deterministic ordering;
22. tests;
23. ARM64;
24. scope/hygiene;
25. E0-B/C/D/E/F/G compatibility.

Approval only after a full pass finds zero material corrections/worthwhile improvements.

## 37. Material approval decisions

Approval would freeze only:

1. Patch 0007 as E0 Director proposal boundary after Performer candidate output;
2. no effective Current Opportunity mutation in Patch 0007;
3. opening VOSS remains fixture authority;
4. E0 strategy deterministic/model-free/least-intervention/provisional, preserving ODR-12;
5. validated least-privilege DirectorOpportunityInput shared by strategies;
6. Bind only public input construction path, copying Scene/source/context ID/roster/control/history only;
7. strategy cannot access Context private state or Candidate VisibleText by type;
8. exactly three roster Characters form E0 hard eligible set;
9. roster/address sets canonical ordinal; OpportunityHistory exact effective event order;
10. OpportunityHistory records effective opportunity establishments, including newly authorized same-Character opportunities, but never retries/rejections/errors/alternate attempts under an existing opportunity/speculation;
11. semantic Proposal `ensemble.e0.director.opportunity.v1`, fields ContractVersion/SceneId/SourceCharacterId/SelectedCharacterId only;
12. strategy identity/context ID/control/history/reasoning diagnostics remain trace/provenance only;
13. least-intervention strategy `ensemble.e0.director.least-intervention.v1`;
14. exact selection: nomination, else addressed-pool recency, else complete-roster recency;
15. source not hard-excluded; recency naturally deprioritizes it;
16. recency = never-seen first, then oldest final history index, ordinal tie;
17. Director structurally detects never-opportunitied roster members, repeated same-Character effective attention, and last-four two-Character alternation in trace only;
18. diagnostics never override explicit social selection and never create equal-turn/fairness authority;
19. no exclusion timer/max-gap/turn quota/dialogue-token counts/scores/weights/probability/random/LLM routing;
20. silence naturally uses fallback and is never penalized/rewritten;
21. precommit Input/Evaluation always discardable and never promotable;
22. effective Director use must, after successful atomic source commit, re-Bind using the exact original validated source ContextPacket + accepted Candidate + authoritative effective OpportunityHistory and then recompute;
23. a freshly recomposed postcommit ContextPacket is not a substitute for what the accepted Performer actually received;
24. speculative precommit evaluation and causal postcommit evaluation must remain distinguishable in provenance;
25. E0-D round-robin later consumes same Input/emits same Proposal shape under separate strategy/trace;
26. interaction/relationship/pressure relevance and post-E0 structured/local/hybrid Director remain open;
27. Scene ending remains later ODR-13 authority;
28. no provider, World Resolver, Integrity, State, Take, commit, persistence, Scene loop, UI, Windows AI/NPU, or Store scope enters Patch 0007.

Implementation remains blocked until recursive audit completes and user explicitly approves the final proposal.
