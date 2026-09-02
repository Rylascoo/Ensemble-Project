# H1 Patch 0007 — E0 Director Opportunity Contract

Status: blueprint proposal 1.1 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0006
Branch: `h1-patch-0007-director-opportunity-blueprint`

## 1. Purpose

Define the next E0-A behavioral boundary after the validated Performer candidate contract:

```text
original source ContextPacket + CandidatePerformance + supplied current-Scene opportunity-event history
    -> deterministic structural Director input binding
        -> least-privilege DirectorOpportunityInput
            -> deterministic E0 Director proposal/evaluation
                -> later successful atomic causal commit
                    -> causal authority supplies authoritative opportunity-event history
                    -> re-Bind original source ContextPacket + accepted Candidate
                    -> recompute Director evaluation
                        -> later effective Current Opportunity
```

Patch 0007 defines one pure deterministic E0 reference Director proposal strategy plus a structurally validated least-privilege input boundary shared by Director strategies.

It does not authenticate opportunity-history provenance, mutate Current Opportunity, or implement Integrity, State, Take, commit, Scene-loop, provider, or World Resolver authority.

## 2. Frozen authority

Blueprint 0.1 establishes:

- Director manages attention/opportunity only;
- central question: whose agency becomes salient next, and why?;
- possible inputs include hard eligibility, direct address, nomination, current interaction relevance, relationship relevance, observable pressure, participation balance, and recent repetition;
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

> Preserve explicit Performer/social intention. Use deterministic recency only to resolve ambiguity or absence of social intent. Detect structural attention pathologies for provenance/evaluation, but do not convert those diagnostics into an equal-turn scheduler or silently override explicit Character handoff.

E0 must be able to reveal repetitive ping-pong, exclusion, or mechanical routing rather than having the Director automatically hide those weaknesses.

The reference Director can detect structural attention repetition from opportunity history. It does not claim to detect semantic Scene stagnation from prose because Candidate VisibleText is intentionally unavailable. Semantic “the Scene is stalled” assessment remains experiential evaluation or future approved Director input, not hidden parsing here.

Scene exhaustion stays outside Patch 0007 because ODR-13/run termination remains open.

Patch 0006 additionally freezes:

- Candidate control = optional addressed Character IDs + optional nomination;
- control is Performer intent metadata, not truth/state/observation/Director authority/history;
- precommit control cannot create effective routing;
- speculative Director work before commit is discardable only;
- associated control may become a later non-binding Director input only after successful causal commit;
- causally used control + Director decision must be reconstructable.

Frozen E0 provenance preserves complete Character ContextPackets, which supports the postcommit source-context binding rule below.

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

## 5. Structurally validated least-privilege input boundary

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

Bind validates cross-boundary identity, roster, and supplied-history **structure**, copies only required values into immutable storage, and exposes no private Context text or Candidate VisibleText to the strategy.

DirectorOpportunityInput has no public constructor.

The later E0-D round-robin strategy is expected to consume the same input type so the experiment varies strategy rather than input disclosure.

## 6. Structural validation is not provenance authentication

`DirectorOpportunityInput.Bind(...)` does **not** prove that caller-supplied OpportunityHistory came from authoritative causal state.

It proves only that the supplied sequence is structurally consistent with this E0 Scene contract:

- IDs are initialized roster members;
- sequence is non-empty;
- current source Character is the tail;
- rich private state is absent from the strategy input.

Therefore:

- synthetic structurally valid histories are legitimate for tests and speculative analysis;
- a DirectorOpportunityInput is not a signed/authorized history artifact;
- non-public construction protects structural invariants, not causal provenance;
- only the later causal/orchestration authority may assert that the history supplied for **effective** postcommit Director use is authoritative.

This prevents a lower-level structural validator from masquerading as the future event-history authority.

## 7. Canonical input storage

Within DirectorOpportunityInput:

- RosterCharacterIds stored ordinally by CharacterId;
- AddressedCharacterIds stored ordinally by CharacterId;
- NominatedCharacterId remains nullable scalar;
- OpportunityHistory preserves exact supplied opportunity-event order and is never sorted;
- SceneId, SourceCharacterId, SourceContextPacketId copy exactly from trusted upstream identity.

No trimming, inference, ranking, semantic rewriting, or hidden normalization occurs.

## 8. Binding invariants

Bind fails closed unless:

1. sourceContext non-null/initialized;
2. sourceCandidate non-null/initialized;
3. Context SubjectCharacterId == Context OpportunityCharacterId;
4. Candidate SubjectCharacterId == Context SubjectCharacterId;
5. Candidate ContextPacketId == Context ContextPacketId;
6. Context roster initialized and exactly three E0 Characters;
7. roster Character IDs initialized and unique;
8. source appears exactly once in roster;
9. OpportunityHistory non-default and non-empty;
10. every history Character ID initialized and in roster;
11. history final entry == source Character.

Patch 0006 CandidatePerformance is already a structurally validated non-forgeable Core output. Bind does not become a second implementation of Patch 0006 Candidate control validation; after ContextPacketId/subject binding succeeds, it copies the already-canonical Candidate control.

Binder does not recanonicalize Context, recompute hashes, inspect provenance, rerun Access Control, or inspect Candidate VisibleText.

## 9. Exact authoritative OpportunityHistory semantics for later effective use

When the future causal/orchestration authority supplies OpportunityHistory for **effective** Director use, the sequence is scoped to the current Scene and records effective opportunity establishments/events.

It includes:

- that Scene's fixture-authored opening opportunity exactly once;
- every later newly authorized effective Character opportunity event for that Scene.

A newly established effective opportunity appends one entry even if its CharacterId equals the previous history entry.

It does not append for provider retry/refusal/error, partial/cancelled generation, malformed candidate, rejected CandidatePerformance, request-another-take attempt, alternate candidate attempt while the existing opportunity remains in force, or speculative/precommit Director proposal/evaluation.

Retries/rejections therefore cannot distort recency, while genuinely re-established attention remains causally observable.

Because E0 has exactly one Scene, Bind does not invent a per-entry Scene wrapper. Later multi-Scene persistence must supply Scene-scoped history explicitly.

Patch 0007 defines these semantics but does not authenticate, persist, or mutate the history.

## 10. DirectorOpportunityInput is observational, not authority state

It is an immutable structurally validated snapshot of signals a Director strategy may consume.

It is not Current Opportunity state, accepted Take authority, Production history, truth, Commit record, State mutation, or permission to trigger a Performer.

## 11. Rich content is structurally unavailable to the strategy

LeastInterventionDirector receives DirectorOpportunityInput, not rich upstream objects.

It cannot read Candidate VisibleText, Character Constitution/Disposition/Circumstance, observations/knowledge/beliefs/suspicions/memories/goals, relationships, Pressure text, Context rendering, denied Production state, Access decisions, or fixture provenance.

Future approved Director inputs may add specific bounded relevance signals without reopening broad Context/Production access.

## 12. Contracts

Freeze:

```text
DirectorOpportunityContractVersion = ensemble.e0.director.opportunity.v1
DirectorStrategyContract = ensemble.e0.director.least-intervention.v1
```

Semantic proposal version and strategy are distinct.

## 13. Semantic proposal shape

```text
DirectorOpportunityProposal
- ContractVersion
- SceneId
- SourceCharacterId
- SelectedCharacterId
```

Proposal contains no StrategyContract, ContextPacketId, Basis/Reason, score, provider/model data, prose, control, history, or mutation information.

Strategy/context/control/history attribution belongs in strategy-specific trace/provenance.

A future E0-D round-robin strategy may emit the exact same Proposal shape.

Proposal has no public constructor and is not effective Current Opportunity.

## 14. Strategy-specific evaluation and trace

```text
LeastInterventionDirectorEvaluation
- Proposal
- Trace
```

```text
LeastInterventionDirectorTrace
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

NeverOpportunitiedCharacterIds is the ordinally stored subset of roster IDs absent from the supplied OpportunityHistory. In effective use, its causal meaning depends on the caller supplying authoritative history. It is never a fairness command.

Trace set-like collections are ordinal; OpportunityHistory stays exact order.

Trace contains no Character prose/private state/model reasoning and is never Character-facing context.

A future round-robin strategy may define its own Evaluation/Trace while sharing DirectorOpportunityInput and DirectorOpportunityProposal.

## 15. Hard eligibility

For frozen E0, the exactly three Context roster Characters are the hard eligible set.

No separate eligibility score/model/LLM.

Future product eligibility may include presence/channel/incapacity/creator intervention, but hard eligibility remains deterministic.

## 16. Strategy API

```text
LeastInterventionDirector.Propose(
    DirectorOpportunityInput input)
    -> LeastInterventionDirectorEvaluation
```

No overload accepts ContextPacket, CandidatePerformance, ValidatedFixture, Production state, free-form text, model output, or provider configuration.

## 17. Exact selection rule

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

Source is not hard-excluded. Because structurally valid history tail is source, recency naturally favors another eligible Character when one exists.

No pattern diagnostic overrides selection.

## 18. Least-recent helper

Within candidate pool:

1. never-seen Character is less recent than any seen Character;
2. among never-seen choose ordinally smallest CharacterId;
3. otherwise compare final history index;
4. choose smallest final index;
5. exact tie -> ordinal CharacterId.

No total-turn/dialogue/word/token counts, percentages, scores, probability, or randomness.

For synthetic/non-authoritative test histories this is simply deterministic computation. For effective use, causal meaning comes from the future authority supplying authoritative history.

## 19. Structural attention diagnostics

The strategy detects structural routing patterns without converting them into authority.

### Never-opportunitied roster members

Trace records roster Characters absent from the supplied history. It does not label absence “accidental.”

### Repeated same-Character attention

RecentAttentionPattern = RepeatedSameCharacter when history has at least two entries and final two IDs are equal.

### Two-character alternation

RecentAttentionPattern = TwoCharacterAlternation when history has at least four entries and final four are exact A,B,A,B with A != B.

Otherwise RecentAttentionPattern = None.

These diagnostics do not affect selection and do not claim to identify semantic Scene stagnation.

## 20. No turn quota or fairness override

Patch 0007 has no equal-opportunity/equal-dialogue target, exclusion timer, maximum-gap guarantee, percentages, line/token counts, fairness score, or forced third-Character insertion.

Explicit social intention may legitimately produce unequal opportunity distribution.

If E0 shows stronger anti-degeneracy intervention is needed, that becomes evidence for later Director revision.

## 21. Silence

Patch 0006 silence produces empty control.

Director cannot inspect VisibleText, so silence naturally reaches RecencyFallback.

Silence is not failure, punishment, or Scene-ending authority.

## 22. Proposal versus effective Current Opportunity

```text
DirectorOpportunityProposal != effective Current Opportunity
```

Bind and Propose are pure/side-effect free.

Precommit Input/Evaluation may exist only as discardable preview/speculation.

A structurally valid Proposal/Evaluation is not proof that its supplied history was authoritative and is never sufficient to trigger a Performer.

## 23. Mandatory postcommit re-Bind + recompute using original source ContextPacket

Precommit DirectorOpportunityInput/Evaluation may never be promoted directly into effective routing authority.

Patch 0006 has no CandidateId/TakeId; distinct attempts can share Context/control and cannot be safely distinguished by precommit evaluation alone.

Approved consequences may also change future Character context. A freshly recomposed postcommit ContextPacket is therefore not a substitute for the ContextPacket under which the accepted source Performance was actually produced.

A later effective-opportunity authority must:

1. successfully commit accepted Performance + approved consequences atomically;
2. retain/recover the exact original validated ContextPacket associated with that accepted source CandidatePerformance;
3. obtain the then-authoritative current-Scene OpportunityHistory from causal authority;
4. call DirectorOpportunityInput.Bind using the original source ContextPacket + accepted CandidatePerformance + authoritative history;
5. call the selected Director strategy after commit;
6. apply SelectedCharacterId only through the later effective-opportunity authority.

The original source ContextPacket identity must equal accepted CandidatePerformance.ContextPacketId through Bind.

Frozen E0 provenance already requires complete Character ContextPackets.

Patch 0007 defines no TakeId/CommitId/application semantics.

## 24. Speculative versus causal provenance

A precommit evaluation, if preserved, is explicitly speculative diagnostic/provenance only and does not authenticate its supplied history.

The postcommit evaluation recomputed from exact original source ContextPacket + accepted Candidate + causally authoritative current-Scene OpportunityHistory is the Director evaluation relevant to effective opportunity provenance.

Failed/rejected attempts may preserve speculative evaluations for E0 diagnostics but never as Scene routing history.

Storage format is later; the semantic distinction is frozen here.

## 25. E0 stale-state scope

E0 has one fixed co-present three-Character Scene.

Director input contains no mutable world/relationship/pressure projection. The postcommit authority supplies current authoritative OpportunityHistory; source ContextPacket remains the original packet tied to the accepted source Performance.

Broader post-E0 stale-state/eligibility rules remain open.

## 26. Information/truth boundaries

Director input/proposal cannot grant Character knowledge, make claims true, promote possibility, create observation/memory/belief, mutate relationship/pressure/world/Character state, or resolve non-Character reality.

World Resolver remains separate.

Proposal reveals only structural Scene/source/selected Character. Trace remains non-Character-facing.

## 27. No hidden Director reasoning

No free-form rationale, chain-of-thought, scratchpad, stage direction, plot instruction, semantic score, or model reasoning.

Trace Rule + structural diagnostics are sufficient for E0 reconstruction.

## 28. Determinism

Identical structurally validated DirectorOpportunityInput + strategy contract produces identical candidate pool, selected Character, Rule, diagnostics, Proposal, and Trace.

No filesystem, clock, random, culture, network, provider/model, AI inference, GPU/NPU, or global mutable state.

Determinism does not imply the input history is causally authoritative; provenance authority is a separate concern.

## 29. E0-D round-robin isolation

Round-robin remains separate, never a flag in least-intervention.

It should consume the same DirectorOpportunityInput and emit the same semantic DirectorOpportunityProposal contract while ignoring social control under a separately frozen cyclic rule.

Thus E0-D changes strategy only, not input disclosure or semantic output shape.

Patch 0007 does not implement round-robin.

## 30. ODR-12 remains open

Post-E0 structured/local-semantic/hybrid design remains unresolved until evidence.

Nothing in Patch 0007 freezes final Director implementation.

## 31. Public surface

Preferred:

```text
DirectorOpportunityInput.Bind(
    ContextPacket,
    CandidatePerformance,
    ImmutableArray<CharacterId>)
    -> DirectorOpportunityInput

LeastInterventionDirector.Propose(
    DirectorOpportunityInput)
    -> LeastInterventionDirectorEvaluation
```

DirectorOpportunityInput, DirectorOpportunityProposal, LeastInterventionDirectorEvaluation, and LeastInterventionDirectorTrace are public read-only with no public constructors.

Only Bind structurally constructs input; only the strategy constructs its Proposal/Evaluation.

This non-forgeability protects object invariants, not causal-history authenticity.

No public operation applies Current Opportunity or triggers Performer.

## 32. Fail closed

Use a Director-specific exception domain.

Bind fails on null/uninitialized/mismatched Context/Candidate, non-three/duplicate/invalid roster, default/empty/structurally invalid history, or history tail mismatch.

Strategy fails on null/invalid input defensively, empty candidate pool, or impossible selection invariant.

Errors expose structural diagnostics only and never echo Candidate VisibleText/private Context text.

Failure produces no alternate opportunity.

## 33. Required tests

Use upstream validated Context/Candidate paths and canonical fixtures.

### Input/history boundary

1. opening Missing Raft VOSS remains fixture authority;
2. valid Voss Bind succeeds with synthetic structurally valid history;
3. Bind is explicitly structural, not a causal-history authentication API;
4. Context/Candidate subject mismatch fails;
5. ContextPacketId mismatch fails;
6. Context subject/opportunity mismatch fails;
7. roster exactly three/unique/initialized/source once;
8. history non-default/non-empty/roster-bound/tail==source;
9. Patch 0006 Candidate control is copied rather than reimplemented/reinterpreted;
10. authoritative-history semantics exclude retries/rejections/alternate attempts/speculation but permit newly effective same-Character opportunity events;
11. Input constructor non-public;
12. Input exact fields contain no VisibleText/private Context/Rendering/Access/provenance/authority flag;
13. roster stored ordinally;
14. addresses stored ordinally;
15. history exact supplied event order preserved including repeated Character IDs;
16. strategy public API accepts DirectorOpportunityInput only;
17. strategy cannot receive ContextPacket/CandidatePerformance directly.

### Proposal/evaluation/trace

18. Proposal exact fields ContractVersion/SceneId/SourceCharacterId/SelectedCharacterId only;
19. Proposal has no StrategyContract/Basis/ContextPacketId/prose/score/control/history/authority flag;
20. Proposal constructor non-public;
21. evaluation is specifically LeastInterventionDirectorEvaluation;
22. trace contains strategy/source ContextPacketId/control/history/candidate pool/selection/Rule/diagnostics;
23. trace set ordering canonical and history event ordering exact;
24. trace contains no VisibleText/private/provider/model reasoning.

### Least-intervention selection

25. nomination -> nominated Character;
26. nomination wins over addresses;
27. one address -> addressed Character;
28. multiple addresses -> least-recent in addressed pool;
29. empty control -> least-recent complete roster;
30. fallback naturally avoids source without hard exclusion when alternatives exist;
31. never-seen beats seen;
32. ordinal tie-break deterministic;
33. nomination may choose globally more-recent Character;
34. direct-address pool may exclude globally less-recent unaddressed Character;
35. upstream VisibleText mutation cannot affect Input or result;
36. silence uses fallback.

### Diagnostics/no fairness authority

37. never-seen roster Character recorded diagnostically but does not override nomination/address;
38. final same,same supplied events -> RepeatedSameCharacter;
39. final A,B,A,B -> TwoCharacterAlternation;
40. A,B,A,C -> None;
41. pattern diagnostic never changes selected Character;
42. diagnostics do not claim semantic Scene stagnation;
43. no exclusion timer/equal-turn/max-gap/forced-third guard exists;
44. explicit A↔B nomination may continue despite diagnostic alternation, proving detection != obligation/fairness authority.

### Authority/causal

45. Bind/Propose do not mutate upstream/input;
46. no apply/trigger operation;
47. no precommit promote/apply surface;
48. structurally valid synthetic history cannot be represented as authenticated causal authority by DirectorOpportunityInput;
49. postcommit effective use requires exact original source ContextPacket, accepted Candidate, authoritative current-Scene history supplied by later causal authority, re-Bind, and recompute;
50. freshly recomposed postcommit ContextPacket is not a substitute if identity differs from Candidate.ContextPacketId;
51. speculative and causal evaluation provenance are distinguished;
52. semantic Proposal reusable by round-robin with a different strategy-specific evaluation/trace;
53. no truth/state/observation/World Resolver authority;
54. no relationship/Pressure/private-state input;
55. no TakeId/CommitId/Integrity/State behavior;
56. no provider/model/AI dependency;
57. no line/token/score/probability fields.

### Determinism/regression

58. repeated Bind/Propose identical;
59. strategy does not mutate Input;
60. least-intervention distinguishable from round-robin by strategy/evaluation/trace, not Proposal shape;
61. frozen Missing Raft StructuredContextHash unchanged;
62. frozen Missing Raft RenderedContextHash unchanged;
63. frozen ECJ-1 9112 bytes/hash unchanged;
64. all existing 173 Core tests green;
65. Missing Raft Harness PASS/0;
66. smoke Harness PASS/0.

## 34. Harness

No live provider execution or Scene loop. Existing Harness output unchanged.

Core tests exercise structural Bind + pure Propose only.

Effective loop waits for downstream causal authority.

## 35. ARM64/battery

Tiny deterministic CPU work over three IDs/control/history. No network/background/AI/GPU/NPU/filesystem/polling.

No NPU claim.

## 36. Explicit exclusions

No causal-history authentication/persistence; final Director; local semantic/hybrid Director; provider Director; prompt reasoning; VisibleText parsing; relationship/pressure relevance; semantic Scene-stall classifier; Scene ending; World Resolver/observation; effective opportunity mutation; Scene loop; provider Performer; Integrity; Take semantics/IDs; State Interpreter/Authority; ProductionState/StateHash; atomic commit/persistence; round-robin implementation; playwright execution; Director Glass/Stage UX; WinUI/Windows AI/NPU/packaging/WACK/Store.

## 37. Recursive audit dimensions

Restart after every correction and test:

1. frozen Director law;
2. least intervention;
3. ODR-12;
4. Patch 0006 control/commit law;
5. least-privilege input disclosure;
6. structural validation vs causal authority;
7. Director vs Performer/World Resolver/Integrity/State/Take;
8. proposal vs effective opportunity;
9. opening fixture authority;
10. hard eligibility;
11. social intent/non-obligation;
12. exclusion/ping-pong/structural stall detection vs authority;
13. no quota/fake precision;
14. untrusted-content isolation;
15. truth/privacy boundaries;
16. exact current-Scene opportunity-event semantics;
17. accepted-attempt/original-context identity + postcommit rebind/provenance;
18. E0-D same-input/same-Proposal-shape isolation;
19. E0-A model-confound isolation;
20. public API/non-forgeability/minimality;
21. strategy-specific vs shared abstractions;
22. fail closed;
23. immutable deterministic ordering;
24. tests;
25. ARM64;
26. scope/hygiene;
27. E0-B/C/D/E/F/G compatibility.

Approval only after a full pass finds zero material corrections/worthwhile improvements.

## 38. Material approval decisions

Approval would freeze only:

1. Patch 0007 as E0 Director proposal boundary after Performer candidate output;
2. no effective Current Opportunity mutation in Patch 0007;
3. opening VOSS remains fixture authority;
4. E0 strategy deterministic/model-free/least-intervention/provisional, preserving ODR-12;
5. structurally validated least-privilege DirectorOpportunityInput shared by strategies;
6. Bind only public input construction path, copying Scene/source/context ID/roster/control/supplied history only;
7. DirectorOpportunityInput structural validity is not causal-history authentication or routing authority;
8. strategy cannot access Context private state or Candidate VisibleText by type;
9. exactly three roster Characters form E0 hard eligible set;
10. roster/address sets canonical ordinal; OpportunityHistory preserves exact supplied event order;
11. later effective use requires current-Scene authoritative OpportunityHistory containing effective opportunity establishments only, including newly authorized same-Character events and excluding retries/rejections/errors/alternate attempts under the same opportunity/speculation;
12. semantic Proposal `ensemble.e0.director.opportunity.v1`, fields ContractVersion/SceneId/SourceCharacterId/SelectedCharacterId only;
13. strategy identity/context ID/control/history/reasoning diagnostics remain strategy-specific trace/provenance only;
14. least-intervention strategy `ensemble.e0.director.least-intervention.v1` returns LeastInterventionDirectorEvaluation/Trace;
15. exact selection: nomination, else addressed-pool recency, else complete-roster recency;
16. source not hard-excluded; recency naturally deprioritizes it;
17. recency = never-seen first, then oldest final history index, ordinal tie;
18. Director structurally detects never-opportunitied roster members, repeated same-Character effective attention, and last-four two-Character alternation in trace only;
19. diagnostics never override explicit social selection, never claim semantic stagnation, and never create equal-turn/fairness authority;
20. no exclusion timer/max-gap/turn quota/dialogue-token counts/scores/weights/probability/random/LLM routing;
21. silence naturally uses fallback and is never penalized/rewritten;
22. precommit Input/Evaluation always discardable and never promotable;
23. effective Director use must, after successful atomic source commit, obtain authoritative history from later causal authority, re-Bind using exact original validated source ContextPacket + accepted Candidate, then recompute;
24. freshly recomposed postcommit Context is not a substitute for what accepted Performer actually received;
25. speculative precommit evaluation and causal postcommit evaluation remain distinguishable in provenance;
26. E0-D round-robin later consumes same Input/emits same Proposal shape under its own strategy-specific evaluation/trace;
27. interaction/relationship/pressure relevance and post-E0 structured/local/hybrid Director remain open;
28. Scene ending remains later ODR-13 authority;
29. no provider, World Resolver, Integrity, State, Take, commit, persistence, Scene loop, UI, Windows AI/NPU, or Store scope enters Patch 0007.

Implementation remains blocked until recursive audit completes and user explicitly approves the final proposal.
