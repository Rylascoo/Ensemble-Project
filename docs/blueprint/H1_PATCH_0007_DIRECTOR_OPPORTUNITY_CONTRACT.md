# H1 Patch 0007 — E0 Director Opportunity Contract

Status: blueprint proposal 1.4 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0006
Branch: `h1-patch-0007-director-opportunity-blueprint`

## 1. Purpose

Define the next E0-A behavioral boundary after the validated Performer candidate contract:

```text
source ContextPacket semantics
+ CandidatePerformance
+ supplied current-Scene opportunity-event history ending at source
    -> deterministic structural Director input binding
        -> least-privilege DirectorOpportunityInput
            -> deterministic E0 Director proposal/evaluation

later successful atomic source commit
    -> causal authority supplies authoritative history ending at source
    -> re-Bind source Context semantics + accepted Candidate
    -> recompute Director proposal/evaluation
    -> later effective-opportunity authority attempts application
        -> only successful application establishes/appends next opportunity event
```

Patch 0007 defines one pure deterministic E0 reference Director proposal strategy plus a structurally validated least-privilege input boundary shared by Director strategies.

It does not authenticate opportunity-history provenance, mutate Current Opportunity, append opportunity history, trigger a Performer, or implement Integrity, State, Take, commit, Scene-loop, provider, or World Resolver authority.

## 2. Frozen authority

Blueprint 0.1 establishes:

- Director manages attention/opportunity only;
- its central question is “Whose agency becomes salient next, and why?”;
- possible inputs include hard eligibility, direct social address, Character nomination, current interaction relevance, relationship relevance, observable pressure, participation balance, and recent repetition;
- no invented numeric weights are frozen;
- equal dialogue is not the goal;
- Director should detect accidental exclusion, repetitive two-character ping-pong, and stalled attention without enforcing a turn quota;
- handoff means opportunity/social pressure, not obligation;
- selected Character may speak, act, evade, redirect, refuse, or remain silent;
- Director cannot write required lines/outcomes, grant inaccessible knowledge, create truth/belief, commit state, spend/retry, or author non-Character reality;
- World Resolver remains separate and unimplemented in E0;
- deterministic hard eligibility/access/cost rules may not be delegated to an LLM;
- E0 provenance preserves Director opportunity inputs and decisions;
- E0-D requires deterministic round-robin opportunity order as a separately labeled ablation;
- ODR-12 leaves the post-E0/final Director mechanism open: structured rules, local semantic judgment, or hybrid;
- E0-A preparation explicitly requires Director inputs, responsibilities, prohibitions, and a least-intervention rule.

Least intervention for this E0 reference slice means:

> Preserve explicit Performer/social intention. Use deterministic recency only to resolve ambiguity or absence of social intent. Detect structural attention pathologies for provenance/evaluation, but do not convert those diagnostics into an equal-turn scheduler or silently override explicit Character handoff.

E0 must be able to reveal repetitive ping-pong, exclusion, or mechanical routing rather than having the reference Director automatically hide those weaknesses.

The E0 reference Director can detect structural attention repetition from opportunity-event history. It does not claim to detect semantic Scene stagnation from free-form Performance because Candidate VisibleText is intentionally unavailable to the strategy. Semantic Scene-stall assessment remains experiential evaluation or a future approved Director input.

Scene exhaustion remains outside Patch 0007 because ODR-13/run termination is open.

Validated Patch 0006 additionally freezes:

- Candidate control = optional AddressedCharacterIds + optional NominatedCharacterId;
- control is provisional Performer intent metadata, not truth/state/observation/Director authority/history;
- provisional control cannot establish effective opportunity or trigger another Performer;
- precommit Director computation, if performed, is discardable zero-authority speculation;
- associated control may become a later non-binding Director input only after successful causal commit;
- causally used control and resulting Director decision must remain reconstructable.

Validated Patch 0005 freezes ContextPacketId as structured semantic content identity, not authorization, signing, provider-disclosure proof, or object-instance identity.

Frozen E0 provenance separately preserves complete Character ContextPackets and provider/rendering facts.

## 3. E0-only scope and ODR-12 preservation

Patch 0007 freezes one structured deterministic E0 reference strategy only.

It does not establish the post-E0 product Director.

A model-free reference avoids adding a second semantic-model variable to the E0-A same-model Performer reference condition.

Future structured/local-semantic/hybrid strategies remain open after E0 evidence.

## 4. E0 single-Character attention cardinality only

Blueprint 0.1's general product model permits Current Attention to be one Character or a very small subset.

The validated E0 spine is narrower:

- ContextPacket has one SubjectCharacterId and one OpportunityCharacterId;
- CandidatePerformance has one SubjectCharacterId;
- Missing Raft opening opportunity is one Character: VOSS.

Therefore Patch 0007 proposes exactly one `SelectedCharacterId` for E0.

This is an E0 execution constraint only. It does not freeze subset attention out of the post-E0 product.

## 5. Opening opportunity remains fixture authority

Missing Raft opening opportunity remains exactly `VOSS`.

Patch 0007 does not select, reinterpret, or overwrite the opening opportunity.

Director evaluation begins only after an already-effective opportunity has produced a structurally valid CandidatePerformance.

Opening fixture provenance remains distinguishable from later Director proposals and applied opportunities.

## 6. Structurally validated least-privilege input

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

Public structural construction path:

```text
DirectorOpportunityInput.Bind(
    ContextPacket sourceContext,
    CandidatePerformance sourceCandidate,
    ImmutableArray<CharacterId> opportunityHistory)
    -> DirectorOpportunityInput
```

Bind validates cross-boundary identity, roster, and supplied-history structure, then copies only required structural values into immutable storage.

`DirectorOpportunityInput` has no public constructor.

The later E0-D round-robin strategy is expected to consume the same input type so the ablation changes strategy rather than input disclosure.

## 7. Structural validation is not causal authentication

`Bind(...)` does not prove that caller-supplied OpportunityHistory came from authoritative causal state.

It proves only structural consistency with this E0 contract:

- Context/Candidate identities bind;
- roster is the frozen E0 three-Character set represented by the ContextPacket;
- supplied history is non-empty and roster-bound;
- supplied history ends at the source Character.

Therefore:

- structurally valid synthetic histories are legitimate for tests/speculative analysis;
- DirectorOpportunityInput is not an authenticated history artifact;
- non-public construction protects invariants, not provenance;
- only a later causal/orchestration authority may assert that history supplied for effective postcommit use is authoritative.

## 8. Canonical input storage

Inside DirectorOpportunityInput:

- `RosterCharacterIds` is stored ordinally by CharacterId;
- `AddressedCharacterIds` is stored ordinally by CharacterId;
- `NominatedCharacterId` is a nullable scalar;
- `OpportunityHistory` preserves exact supplied event order and is never sorted;
- SceneId, SourceCharacterId, and SourceContextPacketId copy from trusted upstream identity.

No trimming, semantic inference, ranking, paraphrase, or hidden normalization occurs.

`ImmutableArray<T>` is already a Core convention in Fixture, Access, Context, and Performer boundaries; Patch 0007 introduces no new collection framework.

## 9. Binding invariants

Bind fails closed unless:

1. sourceContext is non-null/initialized;
2. sourceCandidate is non-null/initialized;
3. Context SubjectCharacterId == Context OpportunityCharacterId;
4. Candidate SubjectCharacterId == Context SubjectCharacterId;
5. Candidate ContextPacketId == Context ContextPacketId;
6. Context roster contains exactly the frozen E0 three Characters;
7. roster Character IDs are initialized/unique;
8. source appears exactly once in roster;
9. OpportunityHistory is non-default and non-empty;
10. every history CharacterId is initialized and present in roster;
11. OpportunityHistory final entry == SourceCharacterId.

Patch 0006 CandidatePerformance already owns Candidate control structural validation. Bind does not duplicate or reinterpret that validator; after identity binding it copies the already-canonical control values.

Bind does not recompute Context hashes, rerun Access Control/Context Composer, inspect provenance, or inspect Candidate VisibleText.

## 10. OpportunityHistory semantics

For later **effective** use, the future causal/orchestration authority must supply current-Scene history containing effective opportunity establishments/events only.

The authoritative sequence includes:

- the Scene's fixture-authored opening opportunity exactly once;
- each later newly authorized effective Character opportunity event exactly once.

A newly established effective opportunity appends one event even if it selects the same Character as the previous event.

The sequence does not append for:

- provider retry/refusal/error;
- partial/cancelled generation;
- malformed Candidate output;
- rejected CandidatePerformance;
- request-another-take or alternate Candidate attempt while the existing opportunity remains effective;
- speculative/precommit Director evaluation;
- postcommit Director recomputation by itself;
- an opportunity application attempt that fails.

The history supplied to Bind/Propose for a source Candidate ends at the **source opportunity event**. The newly proposed target is not part of the input history.

Because E0 has exactly one Scene, Patch 0007 does not invent a per-history-entry Scene wrapper. Later multi-Scene persistence must provide Scene-scoped history explicitly.

Patch 0007 defines history semantics but does not authenticate, persist, append, or mutate history.

## 11. DirectorOpportunityInput is observational, not authority state

DirectorOpportunityInput is an immutable structurally validated snapshot of signals a Director strategy may consume.

It is not:

- Current Opportunity state;
- accepted Take authority;
- Production history;
- truth;
- a causal commit record;
- a State mutation;
- permission to apply routing or trigger a Performer.

## 12. Rich content is structurally unavailable to the strategy

`LeastInterventionDirector` receives DirectorOpportunityInput, not ContextPacket or CandidatePerformance.

It therefore cannot inspect:

- Candidate VisibleText;
- Constitution/Disposition/Circumstance;
- observation/knowledge/belief/suspicion/memory/goal text;
- relationship text;
- Pressure text;
- rendered Context;
- denied Production state;
- Access decisions;
- fixture provenance.

Future approved Director input contracts may add specific bounded relevance signals without granting broad Production/Character access.

## 13. Contracts

Freeze:

```text
DirectorOpportunityContractVersion = ensemble.e0.director.opportunity.v1
DirectorStrategyContract = ensemble.e0.director.least-intervention.v1
```

Semantic proposal contract and selection strategy are separate concepts.

Neither identifier grants authority.

## 14. Shared semantic proposal

```text
DirectorOpportunityProposal
- ContractVersion
- SceneId
- SourceCharacterId
- SelectedCharacterId
```

The Proposal deliberately contains no:

- StrategyContract;
- ContextPacketId;
- Rule/Basis/Reason;
- control/history;
- free-form Director prose;
- score/weight/probability;
- provider/model data;
- mutation/application/commit status.

Strategy/context/control/history attribution belongs in strategy-specific evaluation/trace/provenance.

A future E0-D round-robin strategy may emit the exact same Proposal shape.

Proposal has no public constructor.

Proposal is not effective Current Opportunity.

## 15. Least-intervention evaluation and trace

Freeze strategy-specific types rather than a falsely generic evaluation abstraction:

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

Trace set-like collections are ordinal; OpportunityHistory retains exact event order.

`NeverOpportunitiedCharacterIds` and `RecentAttentionPattern` are derived diagnostics that earn their place because frozen Director law explicitly requires detection of exclusion/repetition/stalled attention. They remain diagnostic only and never modify selection.

Trace contains no Character prose/private state/model reasoning and is never Character-facing context.

A future round-robin implementation may define its own Evaluation/Trace while sharing DirectorOpportunityInput and DirectorOpportunityProposal.

## 16. E0 hard eligibility

For frozen E0, the exactly three Context roster Characters form the hard eligible set.

Patch 0007 introduces no separate eligibility score/model/LLM.

Future product eligibility may include presence, channel, incapacity, creator intervention, or other conditions. Hard eligibility remains deterministic.

## 17. Strategy API

```text
LeastInterventionDirector.Propose(
    DirectorOpportunityInput input)
    -> LeastInterventionDirectorEvaluation
```

No strategy overload accepts ContextPacket, CandidatePerformance, ValidatedFixture, Production state, free-form text, provider/model configuration, or arbitrary Director prose.

## 18. Exact least-intervention selection rule

### Nomination present

```text
CandidatePool = [NominatedCharacterId]
SelectedCharacterId = NominatedCharacterId
Rule = Nomination
```

Explicit nomination is preserved. Pattern/fairness diagnostics do not override it.

### Otherwise, direct address present

```text
CandidatePool = AddressedCharacterIds
SelectedCharacterId = least-recently-opportunitied member of CandidatePool
Rule = DirectAddress
```

Direct address constrains the social pool. Recency only resolves ambiguity among multiple addressed Characters.

### Otherwise

```text
CandidatePool = complete roster
SelectedCharacterId = least-recently-opportunitied member of CandidatePool
Rule = RecencyFallback
```

The source Character is not hard-excluded. Because structurally valid history ends at the source event, the source is ordinarily the most recent Character and another eligible Character naturally wins recency when alternatives exist.

No attention diagnostic overrides selection.

## 19. Least-recent helper

Within CandidatePool:

1. a never-seen Character is less recent than any seen Character;
2. among never-seen Characters choose ordinally smallest CharacterId;
3. otherwise compare each Character's final index in OpportunityHistory;
4. choose the smallest final index;
5. exact tie uses ordinal CharacterId.

No total-turn count, dialogue count, word/token count, percentage, salience score, probability, random source, or clock is used.

For synthetic history this is deterministic computation only. Causal meaning comes only from later authority supplying authoritative history.

## 20. Structural attention diagnostics

The reference strategy detects structural routing patterns without converting them into routing authority.

### Never-opportunitied roster members

`NeverOpportunitiedCharacterIds` contains roster Character IDs absent from supplied OpportunityHistory, ordinally sorted.

It does not label that absence “accidental,” because intent cannot be established from structural history alone.

### Repeated same-Character attention

`RecentAttentionPattern = RepeatedSameCharacter` when the final two supplied opportunity events have the same CharacterId.

### Two-character alternation

`RecentAttentionPattern = TwoCharacterAlternation` when the final four supplied opportunity events are exact A,B,A,B with A != B.

Otherwise `RecentAttentionPattern = None`.

These diagnostics:

- do not change CandidatePool;
- do not change SelectedCharacterId;
- do not claim semantic Scene stagnation;
- do not force an absent third Character;
- are available for E0 provenance/evaluation of mechanical visibility.

## 21. No turn quota or fairness override

Patch 0007 contains no:

- equal-opportunity/equal-dialogue target;
- exclusion timer;
- maximum-gap guarantee;
- percentages;
- line/token counts;
- fairness score;
- forced third-Character insertion.

Explicit social intention may legitimately create unequal opportunity distribution.

If E0 evidence shows stronger intervention is required, that becomes evidence for a later Director revision rather than something hidden before measurement.

## 22. Silence

Patch 0006 silence has empty control.

Because Director cannot inspect VisibleText, silence naturally reaches RecencyFallback.

Silence is not failure, punishment, automatic handoff semantics, or Scene-ending authority.

## 23. Three distinct Director lifecycle states

Patch 0007 freezes three conceptually distinct states without implementing the later application layer:

### A. Speculative precommit evaluation

May be computed before source commit only as discardable zero-authority preview/diagnostic work.

It cannot mutate Current Opportunity, append history, or trigger a Performer.

### B. Postcommit recomputed proposal/evaluation

After successful atomic source Performance+consequence commit, the later causal authority may supply authoritative history ending at source and recompute the Director result.

This result is **eligible for later application**, but it is still only a proposal/evaluation. Postcommit timing alone does not make it effective routing state.

### C. Successfully applied effective opportunity

Only a later approved effective-opportunity authority may apply SelectedCharacterId.

Only after that application succeeds may the selected Character be appended as a newly established effective opportunity event and become eligible to receive the next Context/Performer execution.

Patch 0007 implements A/B calculation semantics only. It implements no C application/mutation.

## 24. Mandatory postcommit re-Bind + recompute

A precommit DirectorOpportunityInput/Evaluation may never be promoted directly into effective routing authority.

Reasons:

- Patch 0006 intentionally defines no CandidateId/TakeId;
- distinct attempts can share Context/control;
- approved consequences may change future Character context;
- later authority must bind the Director decision to the accepted source, not merely to a speculative attempt.

A future effective-opportunity authority must therefore:

1. successfully commit accepted Performance + approved consequences atomically;
2. retain/recover a validated source ContextPacket whose ContextPacketId matches accepted CandidatePerformance.ContextPacketId;
3. obtain authoritative current-Scene OpportunityHistory **ending at the source opportunity event**;
4. call DirectorOpportunityInput.Bind after source commit;
5. call the selected Director strategy after source commit;
6. attempt application of the recomputed SelectedCharacterId through the later effective-opportunity authority;
7. append SelectedCharacterId to OpportunityHistory only if that application succeeds;
8. trigger/compose for the next Performer only after successful application.

A semantically identical validated ContextPacket reconstruction with the same ContextPacketId is acceptable for Director purposes; object-instance identity is irrelevant. A newly composed postcommit ContextPacket with a different ContextPacketId is not the source context associated with the accepted Candidate.

If opportunity application fails:

- the already-successful source Performance+consequence commit remains valid;
- no selected target is appended to OpportunityHistory;
- no next Performer is triggered;
- no alternate target is invented by Patch 0007;
- the postcommit evaluation may remain diagnostic/provenance but is non-effective.

Patch 0007 does not define the later application's transactional/persistence mechanics.

## 25. Context identity is not provider-disclosure identity

Director source binding uses Candidate.ContextPacketId and ContextPacket semantic content identity only.

It does not claim which exact rendered/request bytes an external Performer received.

RenderingContract, RenderedContextHash, provider/model/settings, provider request framing/identity, raw response, and attempt identity remain separate provider-attempt/provenance facts.

## 26. Provenance phases

If preserved, a precommit evaluation is explicitly **speculative** diagnostic/provenance.

A postcommit recomputed evaluation is explicitly a **postcommit proposal/evaluation**, still non-effective until later opportunity application succeeds.

Only successful application may create an **effective opportunity event** in causal history.

A failed application may retain the postcommit proposal/evaluation for diagnostics, but it must never appear as an effective opportunity event.

Patch 0007 freezes this semantic distinction but does not freeze a storage schema or phase enum.

## 27. E0 stale-state scope

E0 has one fixed co-present three-Character Scene.

DirectorOpportunityInput contains no mutable world/relationship/pressure projection. The postcommit-varying input relevant here is authoritative OpportunityHistory; source Context semantics remain tied to accepted Candidate by ContextPacketId.

If another effective opportunity were somehow established before application, authoritative history would no longer end at SourceCharacterId and Bind would fail closed rather than applying a stale source proposal.

Broader post-E0 stale-state/eligibility semantics remain open.

## 28. Information/truth boundaries

Director input/proposal cannot:

- grant Character knowledge;
- make a Character claim objectively true;
- promote a possibility into fact;
- create observation/memory/belief;
- mutate relationship/pressure/world/Character state;
- resolve non-Character reality.

World Resolver remains separate.

Proposal reveals only structural Scene/source/selected Character. Trace remains non-Character-facing.

## 29. No hidden Director reasoning

Patch 0007 creates no free-form Director rationale, chain-of-thought, scratchpad, stage direction, plot instruction, semantic score, or model reasoning.

Typed Rule + exact structural inputs + deterministic diagnostics are sufficient to reconstruct the reference strategy decision.

## 30. Determinism

Identical structurally validated DirectorOpportunityInput + StrategyContract produces identical CandidatePool, SelectedCharacterId, Rule, diagnostics, Proposal, and Trace.

No filesystem, clock, randomness, culture, network, provider/model, AI inference, GPU/NPU, or mutable global state affects calculation.

Determinism does not authenticate supplied history and does not make a Proposal effective.

## 31. E0-D round-robin isolation

Required E0-D deterministic round-robin remains a separate strategy/implementation, never a flag inside `least-intervention.v1`.

It is expected to consume the same DirectorOpportunityInput and emit the same DirectorOpportunityProposal semantic shape, while ignoring social control according to its separately frozen cyclic rule.

Its Evaluation/Trace should remain strategy-specific rather than forcing least-intervention Rule taxonomy into round-robin.

Patch 0007 does not implement the ablation.

## 32. ODR-12 and post-E0 attention remain open

Patch 0007 does not decide whether the post-E0 Director should use structured rules, local semantic judgment, hybrid selection, richer relevance signals, or another mechanism.

It also does not decide final subset-attention semantics.

E0 evidence must inform those later decisions.

## 33. Public surface

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

DirectorOpportunityInput, DirectorOpportunityProposal, LeastInterventionDirectorEvaluation, and LeastInterventionDirectorTrace are public read-only objects with no public constructors.

Only Bind constructs structurally validated input; only the strategy constructs Proposal/Evaluation.

No public Patch 0007 operation applies Current Opportunity, appends history, or triggers Performer execution.

## 34. Fail-closed and diagnostic safety

Use one Director-specific exception domain.

Bind fails on null/uninitialized/mismatched Context/Candidate, invalid E0 roster, default/empty/structurally invalid history, or history-tail mismatch.

Strategy fails defensively on invalid/null input, empty candidate pool, or impossible selection invariant.

Externally observable Director exception text must use stable structural reason/field categories. It must not echo:

- Candidate VisibleText;
- private Context text;
- an arbitrary caller-supplied unknown history CharacterId;
- an arbitrary untrusted payload/snippet.

Trusted canonical Scene/source/roster IDs may be used only where materially useful; prefer stable reason codes over value echoing.

Failure creates no fictional action, alternate Character choice, history append, or implicit routing fallback.

## 35. Required tests and review gates

Use upstream validated Context/Candidate construction and canonical fixtures. Do not add public test-only constructors to fabricate impossible authority states.

### Input/history boundary

1. Missing Raft opening VOSS remains fixture authority;
2. valid Voss Bind succeeds with structurally valid synthetic history `[VOSS]`;
3. Bind is structural, not causal-history authentication;
4. Context/Candidate subject mismatch fails using independently valid upstream objects;
5. ContextPacketId mismatch fails using independently valid upstream objects;
6. history non-default/non-empty/roster-bound/tail==source;
7. Candidate control is copied from Patch 0006 rather than reinterpreted;
8. authoritative-history semantics exclude retries/rejections/alternate attempts/speculation;
9. newly authorized same-Character opportunity events are representable as repeated IDs;
10. input history ends at source and never includes the proposed target pre-application;
11. Input constructor non-public;
12. Input exposes no VisibleText/private Context/Rendering/Access/provenance/authority flag;
13. roster stored ordinally;
14. addresses stored ordinally;
15. history exact supplied order preserved;
16. strategy public API accepts DirectorOpportunityInput only;
17. strategy cannot receive ContextPacket/CandidatePerformance directly.

### Defensive upstream review

18. Bind defensively checks Context subject==opportunity, exactly-three roster, initialized/unique roster, and source once without public invalid-Context escape hatches;
19. no duplicate Candidate-control validator is introduced.

### Proposal/evaluation/trace

20. Proposal exact fields = ContractVersion/SceneId/SourceCharacterId/SelectedCharacterId;
21. Proposal has no StrategyContract/ContextPacketId/Rule/prose/score/control/history/application flag;
22. Proposal selects exactly one Character for E0 and exposes no subset collection;
23. singular E0 Proposal does not claim post-E0 subset attention is forbidden;
24. Proposal constructor non-public;
25. evaluation specifically `LeastInterventionDirectorEvaluation`;
26. trace contains strategy/context/control/history/candidate pool/selection/Rule/diagnostics;
27. trace set ordering canonical and history order exact;
28. trace contains no VisibleText/private/provider/model reasoning.

### Selection

29. nomination selects nominated Character;
30. nomination wins over addresses;
31. one address selects addressed Character;
32. multiple addresses choose least-recent within addressed pool;
33. empty control uses complete-roster least-recent fallback;
34. fallback naturally avoids source when alternatives exist without hard exclusion;
35. never-seen beats seen;
36. ordinal tie-break deterministic;
37. nomination may select globally more-recent Character;
38. direct-address pool may exclude globally less-recent unaddressed Character;
39. VisibleText changes cannot affect Input or result;
40. silence uses fallback.

### Diagnostics/no fairness authority

41. never-seen roster Character recorded diagnostically but does not override selection;
42. final same,same events -> RepeatedSameCharacter;
43. final A,B,A,B -> TwoCharacterAlternation;
44. A,B,A,C -> None;
45. diagnostics never change SelectedCharacterId;
46. diagnostics do not claim semantic Scene stagnation;
47. no exclusion timer/equal-turn/max-gap/forced-third routing guard exists;
48. explicit A↔B nomination may continue despite alternation diagnostic, proving detection != obligation/fairness authority.

### Causal/application boundary

49. Bind/Propose do not mutate Context/Candidate/Input/history;
50. no Patch 0007 apply/append/trigger operation exists;
51. no precommit promote/apply surface exists;
52. structurally valid synthetic history cannot become authenticated authority through DirectorOpportunityInput;
53. postcommit effective use requires source Context semantics matching Candidate.ContextPacketId + authoritative current-Scene history ending at source + re-Bind/recompute;
54. same ContextPacketId semantic reconstruction is acceptable; different postcommit ContextPacketId is not source Context;
55. Director Context identity does not claim rendered/provider disclosure identity;
56. postcommit recomputation alone remains non-effective and does not append target;
57. specification requires append/next Performer only after later opportunity-application success;
58. failed later application leaves target unappended/untriggered and source commit intact;
59. stale authoritative history whose tail no longer equals source fails Bind;
60. speculative, postcommit-proposed, and successfully applied effective opportunity phases remain semantically distinct without adding a Patch 0007 phase flag.

### Scope/regression

61. semantic Proposal reusable by round-robin with different strategy-specific Evaluation/Trace;
62. no truth/state/observation/World Resolver authority;
63. no relationship/Pressure/private-state strategy input;
64. no TakeId/CommitId/Integrity/State behavior;
65. no provider/model/AI dependency;
66. no line/token/score/probability fields;
67. repeated Bind/Propose identical;
68. strategy does not mutate Input;
69. frozen Missing Raft StructuredContextHash unchanged;
70. frozen Missing Raft RenderedContextHash unchanged;
71. frozen Missing Raft ECJ-1 remains 9112 bytes and existing SHA-256;
72. all existing 173 Core tests remain green;
73. Missing Raft Harness remains PASS/0;
74. generic smoke Harness remains PASS/0.

## 36. Harness behavior

Patch 0007 adds no provider execution or self-running Scene loop.

Existing Harness CLI behavior remains unchanged.

Core tests exercise structural Bind + pure Propose only.

Effective opportunity application waits for later causal authority.

## 37. ARM64 and memory suitability

Patch 0007 is tiny deterministic CPU work over three roster IDs, narrow control, and opportunity history.

Implementation should preserve immutable inputs and may reuse immutable arrays by reference where safe rather than deep-copying history merely for appearance of isolation. Only small derived arrays such as CandidatePool or NeverOpportunitiedCharacterIds need allocation.

This is routine deterministic CPU work; no NPU/GPU/model offload is appropriate.

No network, background polling, filesystem I/O, AI inference, or NPU claim.

E0 still excludes memory/token/cost optimization; no arbitrary history cap is invented here.

## 38. Explicit exclusions

Patch 0007 does not implement:

- causal-history authentication/persistence;
- effective-opportunity application or history append;
- Scene loop/orchestration;
- final/post-E0 Director mechanism;
- post-E0 subset-attention contract;
- local semantic/hybrid Director;
- provider/model Director;
- VisibleText semantic parsing;
- relationship/pressure relevance projection;
- semantic Scene-stall classifier;
- Scene ending policy;
- World Resolver/observation;
- provider Performer execution;
- Integrity Validator;
- Take semantics/IDs;
- State Interpreter/State Authority;
- ProductionState/StateHash;
- atomic commit/persistence/recovery;
- E0-D round-robin implementation;
- E0-E playwright execution;
- Director Glass/Stage UX;
- WinUI/Windows AI/NPU/packaging/WACK/Store.

## 39. Recursive audit dimensions

After every correction restart from the top and test:

1. frozen Director law;
2. least-intervention law;
3. ODR-12 preservation;
4. E0 singular vs post-E0 subset attention;
5. Patch 0005 Context identity law;
6. Patch 0006 control/commit law;
7. least-privilege disclosure;
8. structural validation vs causal authority;
9. Director vs Performer/World Resolver/Integrity/State/Take;
10. Proposal vs effective opportunity;
11. opening fixture authority;
12. hard eligibility;
13. social intent/non-obligation;
14. exclusion/repetition/stall diagnostics vs routing authority;
15. no quota/fake precision;
16. untrusted-content isolation;
17. truth/privacy boundaries;
18. exact current-Scene opportunity-event semantics;
19. source Context content identity;
20. precommit vs postcommit vs applied opportunity lifecycle;
21. history append ordering and failed-application behavior;
22. E0-D same-input/same-Proposal isolation;
23. E0-A model-confound isolation;
24. shared vs strategy-specific abstraction hygiene;
25. public API/non-forgeability/minimality;
26. fail-closed/diagnostic safety;
27. immutable deterministic ordering;
28. executable-vs-defensive testability;
29. ARM64/memory suitability;
30. E0-B/C/D/E/F/G compatibility;
31. total scope/hygiene.

Approval only after one complete pass finds zero material corrections or worthwhile improvements.

## 40. Material approval decisions

Approval would freeze only these E0 Patch 0007 decisions:

1. Patch 0007 is the E0 Director proposal boundary after Performer candidate output;
2. Patch 0007 does not mutate/apply Current Opportunity or append history;
3. opening VOSS remains fixture authority;
4. E0 Director strategy is deterministic/model-free/least-intervention/provisional, preserving ODR-12;
5. E0 selection is singular while post-E0 subset attention remains open;
6. shared structurally validated least-privilege DirectorOpportunityInput is the strategy input boundary;
7. Bind copies only Scene/source/context ID/roster/control/supplied current-Scene history;
8. Input structural validity is not history authentication or routing authority;
9. strategy cannot inspect Character-private Context or Candidate VisibleText by type;
10. exactly three roster Characters form E0 hard eligible set;
11. roster/address sets canonical ordinal; OpportunityHistory preserves exact event order;
12. effective-use OpportunityHistory contains opportunity establishments only, including newly authorized same-Character events, and excludes retries/rejections/errors/alternate attempts/speculation/recomputation/failed application;
13. Director input history always ends at source; proposed target is not pre-appended;
14. shared semantic Proposal `ensemble.e0.director.opportunity.v1` contains ContractVersion/SceneId/SourceCharacterId/SelectedCharacterId only;
15. strategy/context/control/history/reasoning stay in strategy-specific trace/provenance;
16. least-intervention strategy `ensemble.e0.director.least-intervention.v1` returns LeastInterventionDirectorEvaluation/Trace;
17. exact selection = nomination, else addressed-pool recency, else complete-roster recency;
18. source is not hard-excluded; recency naturally deprioritizes it;
19. recency = never-seen first, then oldest final history index, ordinal tie;
20. structural diagnostics detect never-opportunitied members, repeated same-Character events, and last-four A↔B alternation;
21. diagnostics never override social selection, never claim semantic stagnation, and never create fairness authority;
22. no exclusion timer/max-gap/turn quota/dialogue-token count/score/weight/probability/random/LLM routing;
23. silence naturally uses fallback without penalty or rewriting;
24. precommit evaluation is discardable speculation and never promotable;
25. after successful atomic source commit, effective-use authority must obtain authoritative history ending at source, re-Bind using source Context semantics matching accepted Candidate.ContextPacketId, and recompute;
26. ContextPacketId is semantic content identity; same-ID semantic reconstruction is acceptable, different postcommit ID is not source Context, and provider/render disclosure remains separate provenance;
27. postcommit recomputation is still only an eligible proposal/evaluation, not effective routing;
28. selected target may be appended and next Performer triggered only after later opportunity application succeeds;
29. failed opportunity application leaves source commit intact, appends no target, triggers nobody, and creates no alternate fallback in Patch 0007;
30. speculative, postcommit-proposed, and successfully applied effective opportunity states remain distinct in provenance without adding a Patch 0007 phase field;
31. E0-D round-robin later consumes same Input/emits same Proposal shape under its own strategy-specific Evaluation/Trace;
32. relationship/pressure/interaction semantic relevance and post-E0 structured/local/hybrid Director remain open;
33. Scene ending remains later ODR-13 authority;
34. impossible malformed upstream authority states are reviewed defensively/reflection-tested without production bypass APIs;
35. no provider, World Resolver, Integrity, State, Take, commit, persistence, Scene loop, UI, Windows AI/NPU, or Store scope enters Patch 0007.

Implementation remains blocked until recursive audit completes and the user explicitly approves the final proposal.
