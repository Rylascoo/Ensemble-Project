# H1 Patch 0006 — Performer Candidate Output Contract

Status: blueprint proposal 1.7 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0005
Branch: `h1-patch-0006-performer-candidate-blueprint`

## 1. Purpose

Implement the next E0-A boundary after the validated Context Composer:

`ContextPacket -> Performer -> provisional CandidatePerformance -> later Integrity / Take / causal commit`

Patch 0006 defines one provisional Character Performance as a strict, bounded, Performer-neutral semantic candidate associated with its bounded Character context, while typed control remains provisional and malformed output, provider failures, hidden reasoning, mutation proposals, invisible pseudo-performance, and uncommitted routing effects remain outside fiction/authority.

Patch 0006 defines the semantic candidate contract plus one strict E0 AI JSON transport schema/parser. It does **not** call any provider/model.

Semantic candidate records source Character + semantic ContextPacket identity. Exact AI disclosure/rendering belongs to later provider-attempt/provenance because only orchestration can truthfully record what was sent.

## 2. Authority basis

Frozen Blueprint 0.1 requires after Access Control/Context Composer:

1. Performer candidate-output contract, including visible Performance and any typed control;
2. Director opportunity-selection contract;
3. Integrity Validator;
4. State Interpreter candidate-mutation schema;
5. deterministic State Authority;
6. accepted/rejected/alternate Take semantics;
7. atomic causal commit.

Frozen law:

- Character != Performer;
- Performer receives bounded Character context and proposes Performance + required typed control;
- Performer owns neither truth nor persistence;
- Performance may be speech, action, silence, refusal, redirection, or another Character-legible response;
- Performance grammar remains open;
- Director manages attention/opportunity, may consider direct social address/Character nomination, and creates opportunity not obligation;
- Integrity checks before acceptance;
- State Interpreter proposes consequences; State Authority decides commit;
- accepted Performance + approved consequences form one atomic causal commit;
- provider failure/refusal/timeout/retry never becomes fiction;
- partial/cancelled/unaccepted output never becomes Production history;
- **every E0 run preserves Performer outputs, rejected/partial attempts as diagnostics, hidden typed control, Director inputs/decisions, validation/commit outcomes, and failures/refusals/cancellations in experimental provenance**;
- fictional dialogue/imported text is untrusted creative content;
- Take a Seat uses the same Access Control -> Context Composer authority boundary as AI Performer, while exact human presentation remains open.

Validated Patch 0005:

- ContextPacket is the Character-safe reference Composer artifact;
- ContextPacketId identifies structured semantic context;
- RenderingContract/RenderedContextHash identify provider-neutral rendering separately;
- safe roster includes CharacterId + DisplayName;
- provider request construction is later;
- relationship-omitted E0-D is a separately labeled safe composition path;
- omniscient E0-D remains outside safe Access Control/reference Composer.

## 3. Why this contract comes next

Frozen preparation ordering places Performer candidate output before Director/Integrity/State/Take/commit. Missing Raft already provides opening opportunity `VOSS`, so dynamic Director is unnecessary to establish this boundary.

Stable candidate vocabulary is needed by later Integrity, State, Take, and Director contracts. Patch 0006 implements none of them.

## 4. Semantic candidate vs AI transport vs attempt provenance

`CandidatePerformance` is semantic Performer output.

```text
Semantic Performer candidate
    CandidatePerformance

E0 AI transport
    ensemble.e0.performer.candidate-json.v1

Later attempt provenance
    provider/model/settings
    complete ContextPacket + identity
    RenderingContract + RenderedContextHash
    provider-request identity/framing when frozen
    raw/partial/error output, hidden control, metrics
```

This separation preserves future Take a Seat: semantic Performance must not imply that every Performer used AI JSON or saw provider-neutral rendering.

Patch 0006 exposes only strict AI JSON construction because AI E0 is the current executable caller. A future approved human path may expose semantic construction through the same internal invariants without changing CandidatePerformance.

## 5. Semantic candidate shape

```text
CandidatePerformance
- ContractVersion
- SubjectCharacterId
- ContextPacketId
- VisibleText
- Control

CandidatePerformanceControl
- AddressedCharacterIds
- NominatedCharacterId
```

Rules:

- ContractVersion identifies semantic CandidatePerformance contract;
- SubjectCharacterId + ContextPacketId copy only from trusted ContextPacket;
- VisibleText is Character-legible provisional Performance;
- Control is non-visible Performer intent/assertion metadata;
- no transport version, provider/model, RenderingContract, RenderedContextHash, request identity, accepted/committed/validated status, routing score/weight, or Director-decision flag;
- no truth, acceptance, persistence, mutation, commit, or Director-selection authority.

No PerformanceKind enum.

## 6. E0 textual representation without freezing final ontology

E0 relies on harness runs/transcripts/blind review, so candidate-v1 uses VisibleText. This does not freeze the final product ontology as text-only/message-centric.

Future approved contracts may introduce richer structured/spatial/multimodal representation.

For candidate-v1:

- empty text = silence;
- non-empty text can represent speech/action/refusal/redirection/evasion/combinations/another Character-legible response;
- parser does not classify dramatic meaning;
- no speech/action/mixed/silence enum;
- later interpretation never silently rewrites preserved Performance.

## 7. Separate semantic and AI transport versions

```text
PerformerCandidateContractVersion = ensemble.e0.performer.candidate.v1
PerformerCandidateJsonSchemaVersion = ensemble.e0.performer.candidate-json.v1
```

CandidatePerformance.ContractVersion is always semantic v1.

AI JSON `schemaVersion` must be exactly candidate-json.v1. Parser validates transport schema then internal semantic builder constructs semantic v1.

A transport-only change can therefore version transport without redefining Performance; future human construction does not inherit an AI schema; semantic change requires semantic version change.

Neither identifier grants authority.

## 8. Reference ContextPacket dependency without policy coupling

ParseJson accepts validated safe reference ContextPacket.

It does not gate on specific CompositionContract/RenderingContract strings because CandidatePerformance does not interpret them. This keeps safe composition variants from changing Performer-output semantics.

It does not authorize omniscient Production state inside safe ContextPacket. Omniscient E0-D remains a separate future experimental binding.

Before inspecting untrusted JSON, ParseJson validates only consumed local Context invariants:

- packet non-null;
- SubjectCharacterId + OpportunityCharacterId initialized;
- opportunity == subject for E0 single-opportunity path;
- ContextPacketId initialized;
- roster CharacterIds initialized + unique;
- subject appears exactly once.

Decoded control targets resolve exactly once in roster.

No Context recanonicalization/hash recomputation/denied-data inspection/record revalidation.

## 9. Exact E0 AI JSON transport

```json
{
  "schemaVersion": "ensemble.e0.performer.candidate-json.v1",
  "performance": { "text": "..." },
  "control": {
    "addressedCharacterIds": ["..."],
    "nominatedCharacterId": null
  }
}
```

Rules:

- nominatedCharacterId = canonical roster ID or null;
- property order + structural whitespace insignificant;
- equivalent escapes -> same semantic candidate;
- Markdown/code fences fail;
- no unknown properties;
- full root required; trailing non-whitespace fails.

Parser safety ceilings:

```text
MaxCandidateJsonBytes = 1,048,576 bytes, inclusive
MaxCandidateJsonDepth = 8
```

Safety ceilings only, not generation/token/artistic limits. Exactly max may succeed if valid; max+1 fails before JSON parsing. Provider execution may impose smaller limits later.

## 10. Visible text contract

### Silence

VisibleText exactly empty. AddressedCharacterIds empty. NominatedCharacterId null. Ensemble invents no narration to represent silence.

### Non-silent

Must contain at least one display-bearing Unicode scalar: not Unicode whitespace and category not Control, Format, NonSpacingMark, SpacingCombiningMark, or EnclosingMark.

This blocks zero-width/format/combining-only pseudo-performance carrying hidden control.

Additional rules:

- leading/trailing whitespace preserved;
- LF/TAB permitted;
- other Control scalars rejected (CR, NUL, DEL/C1, backspace, form feed, etc.);
- Format/combining marks may accompany a display-bearing scalar;
- invalid surrogates rejected;
- already NFC required;
- exact text preserved;
- no trim/normalization/paraphrase/repair/Markdown cleanup/punctuation rewriting.

Character-legibility invariant, not final UI typography policy.

## 11. VisibleText remains untrusted

Parsing does not make prose instruction authority or objective truth.

Instruction-like text has no system authority; Character statements are not truth merely because parsed; text cannot grant knowledge, change state/canon, select next Performer, or commit history.

After later successful atomic commit, the fact the Performance occurred may enter history; propositions inside remain claims/beliefs/etc. unless separately authorized.

## 12. Typed control vocabulary

### AddressedCharacterIds

Performer assertion/intent that other roster Characters are directly socially addressed/turned toward.

Not generic causal impact, observation eligibility, or world-action target.

Rules:

- zero or more;
- exact ordinal/case-sensitive roster IDs;
- no self;
- duplicates fail;
- semantic storage ordinally sorted;
- max = Roster.Length - 1;
- parser fails immediately when array entries exceed max.

### NominatedCharacterId

Optional explicit Character-level handoff/nomination intent:

- null or one exact roster ID;
- no self;
- no next-Performer selection;
- no obligation;
- no truth/knowledge/state authority;
- may overlap addressed set.

**Both control signals are independently optional for a non-silent Performance.** A non-silent candidate with empty addressed set + null nomination is valid; address-only and nomination-only candidates are valid when other invariants hold.

## 13. Provisional control and effective routing

Control is provisional Performer metadata.

Before later causal acceptance/commit succeeds it has no effective Director authority. Rejected/malformed/failed/cancelled/unaccepted/uncommitted control never establishes effective opportunity, triggers another Performer, persists as effective routing state, or survives rollback as if the Performance occurred.

For E0, **all such attempted control/output that exists must still be preserved in required experimental provenance/diagnostics**. Preservation never makes it Scene history or routing authority.

No DirectorEligible projection, semantic-grounding authority, or routing score is introduced.

Associated control for a successfully committed accepted Performance may later be one permissible non-binding Director input.

Even then Performance + authoritative consequences are causal history/state; control remains associated intent metadata, not Production-history prose, truth, or authoritative consequence. Director need not follow it. Control never grants knowledge/mutation/observation eligibility.

Discardable speculative Director computation before commit is permitted only with zero effective authority/effect and mandatory discard on failed commit.

## 14. Causal provenance when Director uses control

If associated control from a committed Performance is actually consumed to establish an effective later opportunity, exact semantic control input + Director decision must be retained in later causal/provenance records sufficiently for audit/reconstruction.

Ordinary diagnostics deletion cannot make a causally relevant attention decision unreconstructable.

This does not promote control to creative Production truth. Exact storage belongs to later Director/Take/commit/provenance contracts.

## 15. Stable IDs and future provider request

Typed control needs stable Character IDs, while Patch 0005 creative rendering omits them.

Future AI provider request must therefore supply a machine-control roster map derived only from safe ContextPacket roster metadata, separate from creative context text, e.g. `VOSS -> Dr. Voss`.

No denied/private/truth/provenance information enters that mapping and Access authority does not change.

Patch 0006 does not construct provider request/system prompt. Future human UI may map human-facing choices to same safe IDs without displaying technical IDs.

## 16. No mutation/private-reasoning/provider channel

Candidate contains no fact/world proposal, state mutation, confidence, consequence list, chain-of-thought/scratchpad/private rationale, CandidateId/TakeId, provider/model/settings/request identity, or AI transport schema version.

## 17. Construction authority

CandidatePerformance + Control are public read-only with no public constructors.

Only public construction:

```text
PerformerCandidateContract.ParseJson(ContextPacket, ReadOnlySpan<byte>) -> CandidatePerformance
```

One non-public semantic builder owns all candidate invariants: Context validation, text, IDs/roster/self/duplicates, address sorting, copied SubjectCharacterId/ContextPacketId, semantic ContractVersion, immutable construction.

Future Take a Seat may expose semantic entry through same invariant path when a real caller exists. No speculative factory now.

Parsing is not acceptance.

## 18. Strict JSON parser

Requirements:

- trusted Context validation first;
- non-empty UTF-8;
- max 1 MiB inclusive;
- BOM rejected;
- root object;
- depth <=8;
- no comments/trailing commas;
- duplicate decoded property names rejected at every object level;
- unknown/missing properties fail;
- property names + transport schema exact/case-sensitive;
- performance only string text;
- control only string-array addressedCharacterIds + string-or-null nomination;
- address array max roster-derived;
- wrong tokens/malformed UTF-8/JSON/trailing content fail.

One candidate-specific exception domain. Incidental CharacterId exceptions converted.

### Diagnostic safety

Full externally observable exception representation (Message, InnerException messages/data, ToString) may expose only trusted structural diagnostics—known field path, expected token/category, byte/line position, trusted roster ID where relevant.

Never echo raw payload, VisibleText, unknown property name, invalid ID, actual mismatched schemaVersion, arbitrary value, secret, or JSON snippet. Unsafe lower-level exceptions are not retained.

Raw output belongs to provenance. Malformed output never becomes fiction.

## 19. Semantic Context association vs exact disclosure provenance

Candidate copies SubjectCharacterId + ContextPacketId from trusted ContextPacket. AI JSON cannot override them.

This identifies semantic bounded Character context associated with candidate, not what an external model actually received.

Candidate has no RenderingContract/RenderedContextHash.

Later AI attempt provenance associates provider/model/version, settings, complete Context packet/ContextPacketId, RenderingContract/RenderedContextHash, future request framing/identity, transport schema actually used, raw output/control/errors/metrics.

## 20. No CandidateId / TakeId semantics

No CandidateId/hash, TakeId allocation, Take identity/numbering, RunId composition, or CommitId composition. Candidate remains provisional.

## 21. Provider output and E0 provenance

Candidate stores semantic validated output, not raw provider response/JSON spelling.

Every E0 run must preserve, as applicable, complete raw candidate output, partial streaming, raw hidden control, refusal/error payload, provider/model/version, latency/token/cost, settings, retries/cancellations, transport schema, and exact disclosure/request attribution in experimental provenance/diagnostics.

Equivalent JSON spellings may map to one semantic candidate; provenance preserves transport facts.

Rejected/partial/cancelled output and any control present **must be preserved for E0 as diagnostics/provenance only** and never Production history.

## 22. Provider failure vs Character refusal

Provider refusal/error/timeout/cancellation = technical outcome.

Character refusal/redirection/silence = possible Performance.

Later provider layer classifies transport outcome before ParseJson. Technical errors are never automatically wrapped as VisibleText.

## 23. Future Integrity

Patch 0006 validates candidate-contract invariants only, not Take acceptance.

Integrity later owns inaccessible-information use, locked canon/world laws, creator-only disclosure, provider-failure contamination, required output integrity, and semantic concerns between Performance/control. Model-assisted checks cannot waive deterministic gates.

## 24. Future Director

Direct address + nomination are possible Director inputs already frozen by Blueprint 0.1.

Candidate control has no effective pre-commit routing authority. Only associated metadata for later successfully committed accepted Performance may become effective Director input.

Speculative computation before commit cannot trigger Performer/change effective opportunity/survive failed commit as routing state.

Post-commit control remains non-binding metadata, not truth/state. Director may also consider eligibility, interaction/relationship relevance, pressure, participation balance, repetition. Patch 0006 does not score/rank/choose. Silence does not force handoff.

## 25. State Interpreter / State Authority

Candidate has no mutation authority.

Conceptually later:

```text
CandidatePerformance
-> Integrity / State Interpreter / State Authority / Take semantics
-> atomic commit of accepted Performance + approved consequences
-> Performance enters Production history/state
-> associated control may be causal provenance + eligible Director input without becoming Production truth
-> Director establishes later effective opportunity
```

Exact pipeline/IDs/persistence are later contracts.

## 26. Missing Raft examples

Transport-shape examples only:

```json
{
  "schemaVersion": "ensemble.e0.performer.candidate-json.v1",
  "performance": { "text": "Before we decide what happened, what did each of us actually observe?" },
  "control": { "addressedCharacterIds": ["MARLOWE", "WREN"], "nominatedCharacterId": null }
}
```

Silence:

```json
{
  "schemaVersion": "ensemble.e0.performer.candidate-json.v1",
  "performance": { "text": "" },
  "control": { "addressedCharacterIds": [], "nominatedCharacterId": null }
}
```

## 27. Required implementation tests

Use validated Patch 0005 ContextPacket/fixtures; no duplicate fixture JSON.

### Semantic/context/text/control

1. valid Voss JSON -> semantic ContractVersion candidate.v1;
2. transport schema version not stored as semantic contract/property;
3. SubjectCharacterId copied;
4. ContextPacketId copied;
5. no RenderingContract/RenderedContextHash/provider/request fields;
6. Context invariants checked before JSON;
7. opportunity != subject fails;
8. malformed/ambiguous roster IDs fail if encountered;
9. subject resolves exactly once;
10. null packet candidate-specific failure;
11. visible text preserved;
12. empty text valid silence only with empty/null control;
13. whitespace-only fails;
14. Format-only fails;
15. combining-only fails;
16. visible + permitted Format/combining preserved;
17. silence+address fails;
18. silence+nomination fails;
19. non-silent + empty control is valid;
20. non-silent + address-only is valid;
21. non-silent + nomination-only is valid;
22. LF/TAB preserved;
23. forbidden Controls fail;
24. invalid surrogate fails;
25. non-NFC fails without normalization;
26. addressed target must exist;
27. uninitialized address candidate-specific failure;
28. self-address fails;
29. duplicate addresses fail;
30. addresses sorted;
31. address array > roster-minus-subject fails immediately;
32. nomination target must exist;
33. uninitialized nomination candidate-specific failure;
34. self-nomination fails;
35. nomination may overlap address;
36. parser does not mutate Context/state;
37. constructors non-public;
38. no accepted/committed/validated/routing fields;
39. no public semantic factory;
40. no raw output field;
41. no fixture/truth/provenance/Access/mutation/confidence/private-reasoning/accepted-history/CandidateId/TakeId property.

### Strict JSON

42. canonical candidate-json.v1 parses;
43. semantic ContractVersion differs from transport schema;
44. property reorder identical;
45. structural whitespace identical;
46. equivalent escapes identical;
47. code fences fail;
48. empty fails;
49. exactly 1 MiB valid succeeds;
50. max+1 fails before parse;
51. depth >8 fails;
52. exact transport schema required/case-sensitive;
53. semantic contract string used as JSON schema fails;
54. property names case-sensitive;
55. unknown property fails;
56. missing required property fails;
57. duplicate root/nested/escaped-decoded property fails;
58. comments fail;
59. trailing comma fails;
60. BOM fails;
61. malformed UTF-8 fails;
62. malformed JSON fails;
63. escaped isolated surrogate fails;
64. wrong token types fail;
65. trailing content fails;
66. alternate-case Character ID fails;
67. parser/internal builder one invariant path;
68. repeated parse semantically identical;
69. parser does not mutate packet.

### Diagnostic leakage

70. invalid VisibleText sentinel absent from complete exception representation;
71. invalid ID sentinel absent;
72. mismatched transport schema sentinel absent;
73. unknown property sentinel incl. control/newline absent;
74. malformed payload sentinel absent.

### Policy/causal/public/regression

75. parser does not require full-authorized.v1;
76. parser does not require render.v1;
77. no omniscient bypass;
78. ParseJson only public construction;
79. Control no commit/Director flag;
80. no path establishes effective opportunity from candidate control;
81. semantic candidate no AI disclosure/transport/provenance fields;
82. frozen Missing Raft StructuredContextHash unchanged;
83. frozen Missing Raft RenderedContextHash unchanged;
84. frozen ECJ-1 9112 bytes + frozen hash;
85. all existing 115 tests green;
86. Missing Raft Harness PASS/0;
87. smoke Harness PASS/0.

Impossible malformed ContextPacket states may be proven by defensive review without public bypass hooks.

## 28. Harness

No live provider call/CLI generation. Existing Harness output unchanged. Core tests local parser only. Provider execution waits for separately approved provider/request/provenance configuration.

## 29. ARM64/battery

Deterministic CPU validation over <=1 MiB/depth8 JSON, <=roster-minus-subject control entries, one small candidate object. No network/background/AI/GPU/NPU/filesystem/polling. No NPU performance claim.

## 30. Exclusions

No provider call/adapter/request/system prompt; attempt/disclosure persistence; provider resource policy below parser ceiling; credentials/casting/understudy/settings; streaming/retry/cancellation/refusal implementation; Take a Seat API/UI; final Performance ontology; Director selection; Integrity acceptance; Take IDs/semantics; State Interpreter/Authority; ProductionState/StateHash; atomic commit/history persistence; observation; E0-D bindings; playwright execution; final UI text policy; WinUI/Windows AI/NPU/packaging/WACK/Store.

## 31. Recursive audit dimensions

Repeat complete passes across Blueprint consistency; Patch 0004/0005 authority; Character/Performer/Take a Seat; semantic-vs-transport-vs-provenance separation; semantic-vs-transport versioning; Director/Integrity/State separations; statement-vs-fact; Take/atomic commit; causal provenance vs Production history; E0 mandatory provenance; provider failure; JSON/Unicode/identity/control bounds; Performance ontology openness; API minimality; safe composition/omniscient separation; resource/diagnostic leakage; precommit side effects/speculation; Director reconstruction; scope/hygiene; ARM64 suitability; tests and E0-B/C/D/E/F/G isolation.

Any correction restarts the full pass. Approval only after one full zero-material-change pass.

## 32. Exit gate

Before implementation promotion:

1. zero-change recursive pass + user approval;
2. dedicated implementation branch from then-current main;
3. semantic candidate distinct from AI transport/provenance;
4. semantic/transport versions separate;
5. no AI rendering/provider fields in candidate;
6. no speculative human factory;
7. E0 text does not freeze final ontology;
8. no PerformanceKind;
9. one internal invariant builder;
10. no composition/rendering policy gate;
11. no omniscient bypass;
12. trusted Context checks before JSON;
13. candidate copies only subject + packet identity;
14. exact disclosure/provider/transport in attempt provenance;
15. strict JSON/size/depth/control bounds;
16. no diagnostic leakage;
17. preserved untrusted visible text/no invisible pseudo-performance;
18. optional address+nomination control;
19. precommit control no effective opportunity/trigger; rejected control still mandatory E0 provenance; speculation discardable;
20. postcommit control may inform Director without becoming history/truth/state;
21. control used by Director preserved causally for reconstruction;
22. no mutation/private reasoning/provider runtime candidate data;
23. exact IDs;
24. no public constructor bypass;
25. malformed/resource overflow fails technically;
26. parser edge boundaries tested;
27. existing tests/hash regressions green;
28. native ARM64 build/tests/harness pass;
29. no later-scope creep;
30. evidence separates tested head from docs closure.

## 33. Material approval decisions

Approval freezes:

1. Patch 0006 position after Context Composer, before Director/Integrity/State/Take/commit;
2. no provider/model call;
3. CandidatePerformance semantic Performer output;
4. semantic contract `ensemble.e0.performer.candidate.v1` separate from AI JSON `ensemble.e0.performer.candidate-json.v1`;
5. candidate fields ContractVersion, SubjectCharacterId, ContextPacketId, VisibleText, Control only;
6. rendering/provider/request/transport facts stay attempt provenance;
7. safe ContextPacket accepted without irrelevant composition/render policy gates;
8. omniscient E0-D separate;
9. Take a Seat may later reuse invariants; no public factory now;
10. text E0 representation does not freeze final ontology;
11. no PerformanceKind; empty=silence; non-empty grammar-open;
12. non-empty requires display-bearing scalar; exact text preserved/untrusted; invalid Unicode/control/NFC rejected, not repaired;
13. control = optional address IDs + optional nomination only;
14. addresses roster-minus-subject bounded/sorted/duplicates fail;
15. control provisional intent metadata, not truth/state/observation/Director authority/history;
16. all E0 attempted output/control, including rejected/partial when present, is mandatory experimental provenance/diagnostics and never automatically history;
17. precommit control cannot create effective opportunity/trigger Performer/survive rollback as routing;
18. control associated with successfully committed Performance may become non-binding Director input without becoming truth/history/state;
19. if Director consumes it effectively, exact semantic control + decision preserved in causal provenance for audit/reconstruction;
20. discardable precommit Director speculation allowed only with zero authority/effect and rollback discard;
21. nomination may overlap address and never obligates response;
22. provider request later supplies stable IDs only through safe roster-derived machine-control map separate from creative text;
23. no mutation/confidence/private reasoning/world proposal/CandidateId/TakeId/provider metadata in candidate;
24. strict JSON fail-closed/case-sensitive, order+structural-whitespace insensitive, no repair;
25. parser 1MiB inclusive/depth8 safety ceilings, not generation budgets;
26. exception chain only trusted structural diagnostics;
27. public construction ParseJson only; internal builder owns invariants;
28. candidate/control constructors non-public/no authority flags;
29. exact raw/partial/error provider output + exact transport/disclosure/request remain separate provenance;
30. Character refusal/redirection/silence distinct from provider refusal/error/cancel;
31. provider execution, human UI/factory, Director, Integrity, State Interpreter, State Authority, Take semantics, atomic commit, persistence, E0-D bindings all outside Patch 0006.

Implementation must not begin until recursive audit completes and these decisions are explicitly approved.
