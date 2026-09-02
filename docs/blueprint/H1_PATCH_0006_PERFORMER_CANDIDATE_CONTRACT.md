# H1 Patch 0006 — Performer Candidate Output Contract

Status: blueprint proposal 1.6 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0005
Branch: `h1-patch-0006-performer-candidate-blueprint`

## 1. Purpose

Implement the next E0-A boundary after the validated Context Composer:

`ContextPacket -> Performer -> provisional CandidatePerformance -> later Integrity / Take / causal commit`

Patch 0006 answers one narrow question:

> Can Ensemble represent one provisional Character Performance as a strict, bounded, Performer-neutral semantic candidate associated with its bounded Character context, while keeping typed control provisional and preventing malformed output, provider failures, hidden reasoning, mutation proposals, invisible pseudo-performance, or uncommitted routing effects from entering fiction or authority?

Patch 0006 defines the semantic candidate contract plus one strict E0 AI JSON transport schema/parser. It does **not** call any provider/model.

The semantic candidate records its source Character and semantic ContextPacket identity. Exact AI disclosure/rendering facts belong to later provider-attempt/provenance records because only orchestration can truthfully record what was actually sent to a provider.

## 2. Authority basis

Frozen Blueprint 0.1 requires the E0-A preparation sequence to define, after Access Control and Context Composer:

1. Performer candidate-output contract, including visible Performance and any typed control data;
2. Director opportunity-selection contract;
3. Integrity Validator acceptance/rejection rules;
4. State Interpreter candidate-mutation schema;
5. deterministic State Authority commit rules;
6. accepted/rejected/alternate Take semantics;
7. atomic causal commit.

Frozen architectural law additionally states:

- Character != Performer;
- Performer receives one bounded Character context and produces a candidate Performance plus any required typed control output;
- Performer portrays agency but owns neither truth nor persistence;
- a Performance may contain speech, action, silence, refusal, redirection, or another Character-legible response;
- Performance grammar remains open rather than a frozen engine taxonomy;
- Director manages attention/opportunity and may later consider direct social address and Character nomination, but offers opportunity rather than obligation;
- Integrity Validator checks candidate Performance before acceptance;
- State Interpreter proposes consequences; deterministic State Authority decides what may commit;
- accepted Performance and approved consequences form one atomic causal commit; either both commit coherently or neither does;
- generated work becomes Production history only through later acceptance/commit;
- provider error/refusal/timeout/retry must never become fictional action;
- partial/cancelled/unaccepted output must not enter Production history;
- experimental provenance must preserve complete Character context packets, Performer outputs including rejected/partial attempts as diagnostics, hidden typed control, Director inputs/decisions, errors/refusals/cancellations, and later validation/commit results;
- imported text and fictional dialogue are untrusted creative content rather than instruction authority;
- human Take a Seat uses the same deterministic Access Control -> Context Composer information-authority boundary as an AI Performer;
- exact Take a Seat guidance/presentation remains open after E0.

Validated Patch 0005 freezes the immediate upstream reference boundary:

- `ContextPacket` is the Character-safe reference Composer artifact;
- `ContextPacketId = CTX:<StructuredContextHash>` identifies structured semantic context;
- `RenderingContract` and `RenderedContextHash` identify provider-neutral rendering separately from semantic ContextPacket identity;
- ContextPacket roster contains safe CharacterId + DisplayName metadata;
- provider-neutral creative rendering omits Character IDs;
- final provider/system request construction remains outside Context Composer;
- relationship-omitted E0-D is a later separately labeled safe composition path;
- omniscient E0-D remains outside safe Access Control/reference Composer.

Patch 0006 preserves those meanings.

## 3. Why Performer candidate contract comes next

The frozen continuation order places Performer candidate-output before Director, Integrity Validator, State Interpreter, State Authority, Take semantics, and causal commit.

Missing Raft already supplies first opportunity `VOSS`, so dynamic Director implementation is unnecessary to establish the first candidate boundary.

A stable candidate vocabulary is prerequisite for later boundaries:

- Director needs a stable vocabulary for committed Performance plus associated Performer control metadata;
- Integrity Validator needs a concrete provisional candidate;
- State Interpreter must interpret an accepted Performance rather than an untyped provider string;
- Take semantics must identify accepted/rejected attempts without redefining candidate content.

Patch 0006 implements none of those later authorities.

## 4. Semantic candidate is distinct from AI transport and provider-attempt provenance

`CandidatePerformance` is semantic Performer output.

E0 JSON is one AI transport representation used to construct it. Exact provider request/disclosure metadata is later provenance.

```text
Semantic Performer candidate
    CandidatePerformance

E0 AI transport
    ensemble.e0.performer.candidate-json.v1

Later provider-attempt provenance
    provider/model/settings
    exact ContextPacket identity
    RenderingContract + RenderedContextHash
    later provider-request identity/framing
    raw/partial/error output and metrics
```

This distinction matters for Take a Seat: a human occupies the same Performer role and bounded Context authority, but Blueprint 0.1 leaves human guidance/presentation open. Semantic Performance must not imply every Performer used an AI JSON transport or saw provider-neutral rendering.

Patch 0006 exposes only the strict AI JSON construction path because AI E0 is the current executable caller. A future approved Take a Seat slice may expose a semantic construction path reusing the same internal candidate invariants without changing CandidatePerformance contract.

Neither AI nor future human construction may bypass Character access.

## 5. Semantic candidate shape

```text
CandidatePerformance
- ContractVersion
- SubjectCharacterId
- ContextPacketId
- VisibleText
- Control
```

```text
CandidatePerformanceControl
- AddressedCharacterIds
- NominatedCharacterId
```

Rules:

- `ContractVersion` identifies the semantic CandidatePerformance contract, not the JSON transport;
- SubjectCharacterId and ContextPacketId copy only from trusted ContextPacket;
- VisibleText is Character-legible provisional Performance;
- Control is non-visible typed Performer intent/assertion metadata;
- candidate/control contain no AI transport schema version, provider/model, RenderingContract, RenderedContextHash, provider-request identity, accepted/committed/validated status, routing weight, score, or Director-decision flag;
- candidate has no truth, acceptance, persistence, mutation, commit, or Director-selection authority.

No PerformanceKind enum is defined.

## 6. E0 textual representation without freezing final Performance ontology

E0 is evaluated through explicit packets, harness runs, transcripts, and blind transcript comparison. Candidate-v1 therefore uses textual VisibleText.

That does **not** freeze the final product ontology as text-only or message-centric.

Future product evidence may justify richer structured, spatial, multimodal, or other Performance representations under a new approved semantic contract.

For candidate-v1:

- empty VisibleText = silence;
- non-empty text may represent speech, action, refusal, redirection, evasion, combinations, or another Character-legible response;
- parsing does not classify dramatic meaning;
- no speech/action/mixed/silence enum;
- later Integrity/State interpretation may reason about semantic content without rewriting preserved Performance.

## 7. Separate semantic and AI transport versions

Patch 0006 freezes two distinct identifiers:

```text
PerformerCandidateContractVersion = ensemble.e0.performer.candidate.v1
PerformerCandidateJsonSchemaVersion = ensemble.e0.performer.candidate-json.v1
```

`CandidatePerformance.ContractVersion` is always `ensemble.e0.performer.candidate.v1`.

The AI JSON root `schemaVersion` must be exactly `ensemble.e0.performer.candidate-json.v1`.

The parser validates the JSON transport version, then the internal semantic builder constructs a candidate with the semantic ContractVersion.

Consequences:

- a future JSON-only syntax/framing change can version the transport without falsely redefining semantic Performer output;
- a future human semantic construction path does not inherit an AI transport version it never used;
- a future semantic CandidatePerformance change requires its own semantic contract version even if a transport could encode both.

Neither identifier grants authority.

## 8. Reference ContextPacket dependency without composition-policy coupling

The public parser accepts the validated safe reference ContextPacket type from Patch 0005.

It does not gate on a particular CompositionContract or RenderingContract string because CandidatePerformance does not interpret those policies. This avoids unnecessarily changing Performer-output semantics for safe composition variants such as relationship omission.

This does not authorize omniscient Production state inside reference Character-safe ContextPacket. Omniscient E0-D remains outside safe Access Control/reference Composer.

A future separately labeled omniscient binding may reuse CandidatePerformance semantic invariants without masquerading as the reference safe packet path. Patch 0006 does not implement it.

Before examining untrusted JSON, ParseJson validates only local Context invariants it consumes:

- ContextPacket non-null;
- SubjectCharacterId and OpportunityCharacterId initialized;
- OpportunityCharacterId == SubjectCharacterId for this E0 single-opportunity path;
- ContextPacketId initialized;
- roster Character IDs initialized and unique;
- subject appears exactly once.

Decoded control targets must resolve exactly once in roster.

Patch 0006 does not recompute Context hashes, recanonicalize Context, inspect denied information, validate provider rendering, or duplicate Context record-level validation.

## 9. Exact E0 AI JSON transport shape

Candidate bytes must decode as:

```json
{
  "schemaVersion": "ensemble.e0.performer.candidate-json.v1",
  "performance": {
    "text": "..."
  },
  "control": {
    "addressedCharacterIds": ["..."],
    "nominatedCharacterId": null
  }
}
```

Rules:

- nominatedCharacterId is canonical roster ID string or null;
- property order insignificant;
- ordinary structural JSON whitespace insignificant;
- equivalent JSON escapes decoding to identical validated strings yield same semantic candidate;
- Markdown/code fences fail;
- no additional root/performance/control properties;
- full root required; trailing non-whitespace fails.

### Parser safety ceilings

```text
MaxCandidateJsonBytes = 1,048,576 bytes (1 MiB), inclusive
MaxCandidateJsonDepth = 8
```

These are denial-of-service/parser ceilings, not generation targets, token budgets, artistic limits, or E0 optimization policy.

Exactly 1 MiB may parse if otherwise valid; 1 MiB + 1 byte fails before JSON parsing.

Later provider execution may impose a smaller resource/experiment limit without changing this Core ceiling unless deliberately versioned.

## 10. Visible Performance text contract

### Silence

Silence is exactly `VisibleText == ""`.

For silence, AddressedCharacterIds is empty and NominatedCharacterId is null.

Ensemble does not invent narration such as “Voss says nothing.”

### Non-silent Performance

Non-empty VisibleText must contain at least one display-bearing Unicode scalar.

A scalar is display-bearing here when it is not Unicode whitespace and its category is not Control, Format, NonSpacingMark, SpacingCombiningMark, or EnclosingMark.

This prevents zero-width/format/combining-only strings from masquerading as visible Performance while carrying hidden control.

Additional rules:

- leading/trailing whitespace preserved;
- LF and TAB permitted;
- other Unicode Control scalars rejected, including CR, NUL, DEL/C1, backspace, form feed;
- Format/combining marks may accompany at least one display-bearing scalar;
- invalid surrogate sequences rejected;
- text must already be NFC;
- text preserved exactly after validation;
- no trim, normalization, paraphrase, repair, Markdown cleanup, quote insertion, punctuation correction, or hidden rewriting.

This is a Character-legibility invariant, not final UI typography/sanitization policy.

## 11. VisibleText remains untrusted creative content

Parsing never promotes candidate prose to system instruction or objective truth.

Therefore:

- instruction-like text has no system authority;
- a Character statement does not become Production truth because parsing succeeded;
- text cannot grant knowledge, alter canon/state, select next Performer, or commit history;
- after a future successful causal commit, the fact that the Performance occurred may enter history; propositions inside remain claims/beliefs/etc. unless separately authorized by State Authority.

## 12. Typed control vocabulary

Control is deliberately minimal and derived from frozen future Director inputs.

### AddressedCharacterIds

Performer assertion/intent that one or more other roster Characters are directly socially addressed/turned toward.

It is not generic causal impact, observation eligibility, or world-action target.

Rules:

- zero or more IDs;
- exact ordinal/case-sensitive roster match;
- subject prohibited;
- duplicates fail;
- semantic candidate stores sorted ordinally;
- max entries = Roster.Length - 1;
- parser fails immediately when array count exceeds max, before accumulating further entries.

### NominatedCharacterId

Optional explicit Character-level handoff/nomination intent.

Rules:

- null or one exact roster ID;
- self-nomination prohibited;
- does not select next Performer;
- does not obligate response;
- grants no truth/knowledge/state authority;
- may overlap addressed set.

## 13. Provisional control, causal commit, and effective Director routing

CandidatePerformance.Control is provisional Performer metadata.

Before later causal acceptance/commit succeeds, it has no effective Director authority. Rejected, malformed, failed, cancelled, unaccepted, or uncommitted control must never:

- establish Current Attention/Opportunity;
- trigger another Performer invocation;
- persist as effective Director input;
- survive rollback as if its Performance occurred.

Failed/rejected control may remain experiment provenance/diagnostics without becoming Scene history or routing authority.

Patch 0006 invents no DirectorEligible projection, semantic-grounding authority, or routing score.

Associated control metadata for a successfully committed accepted Performance/Take may later become one permissible non-binding Director input.

Even then:

- accepted Performance + approved authoritative consequences are causal content entering Production history/state;
- control remains associated Performer intent/assertion metadata, not Production-history prose, objective truth, or authoritative consequence;
- Director may consider but need not follow it;
- nomination is opportunity pressure, not obligation;
- control never grants truth, knowledge, mutation, or observation eligibility.

A future discardable speculative Director recommendation before commit is allowed only if it has zero effective authority, cannot trigger another Performer/change effective opportunity, and is discarded on failed commit.

## 14. Causal provenance requirement for control used by Director

If associated control from a successfully committed Performance is actually consumed by the future Director to establish an effective opportunity, the exact semantic control input and resulting Director decision must be retained in later causal/provenance records sufficiently to audit/reconstruct that attention transition.

Ordinary diagnostic deletion must not make a causally relevant Director decision unreconstructable.

This does not promote control to creative Production truth. Exact storage/identity belongs to later Director/Take/causal-commit/provenance contracts.

## 15. Stable Character IDs and future provider request

Patch 0005 omits Character IDs from provider-neutral creative prose, but typed control needs stable IDs because display names are not identity.

A later AI provider-request contract must supply an allowed machine-control roster mapping derived only from safe ContextPacket roster metadata, conceptually:

```text
MARLOWE -> Marlowe
VOSS    -> Dr. Voss
WREN    -> Wren
```

The mapping reveals no denied record/provenance/truth/private state, stays separate from creative context text, exists only to enable stable control IDs, and does not change Access authority.

Patch 0006 does not construct provider requests/system prompts. Future Take a Seat UI may resolve human-facing targets to the same safe IDs without showing technical identifiers.

## 16. No state-mutation or private-reasoning channel

Candidate schema contains no fact/world proposal, state mutation, confidence score, consequence list, chain-of-thought, scratchpad/private rationale, CandidateId/TakeId, provider/model/settings/request identity, or AI transport schema version.

Consequences belong to State Interpreter; authority belongs to State Authority; provider metadata belongs in attempt/provenance data.

## 17. Construction authority and minimal public surface

CandidatePerformance and CandidatePerformanceControl are public read-only outputs with no public constructors.

Patch 0006 exposes exactly one public construction operation:

```text
PerformerCandidateContract.ParseJson(
    ContextPacket contextPacket,
    ReadOnlySpan<byte> utf8CandidateOutput)
    -> CandidatePerformance
```

Inside Core, parsing delegates to one canonical non-public semantic builder that:

- validates local Context invariants before untrusted parsing;
- validates text;
- validates Character IDs/roster/self/duplicates;
- sorts addressed IDs;
- copies SubjectCharacterId + ContextPacketId;
- sets semantic ContractVersion;
- constructs immutable candidate/control.

A future Take a Seat implementation may expose a validated semantic entry point through the same invariant path when a real non-JSON caller exists. No speculative public API now.

Parsing is not acceptance or authority.

## 18. Strict AI JSON parser

Requirements:

- trusted Context invariants first;
- non-empty UTF-8;
- max 1 MiB inclusive;
- BOM rejected;
- root one object;
- depth max 8;
- comments/trailing commas rejected;
- duplicate decoded property names rejected at every object level;
- unknown properties rejected;
- required properties exactly once;
- property names + JSON schemaVersion exact/case-sensitive;
- performance object only string text;
- control object only string-array addressedCharacterIds + string-or-null nominatedCharacterId;
- addressed array max roster-derived;
- wrong token types fail;
- malformed UTF-8/JSON fail;
- non-whitespace after root fails.

One candidate-specific exception represents technical parse/semantic failure. Incidental CharacterId argument/initialization exceptions are converted to this domain exception.

### Error-message safety

Externally visible exception data may include only trusted structural information such as known field path, expected token/category, byte/line position, or trusted roster ID.

It must not echo raw payload, VisibleText, unknown property names, invalid IDs, actual mismatched transport schemaVersion, arbitrary values, credentials/secrets, or JSON snippets.

Unknown-property diagnostics name only the containing known object; invalid-ID diagnostics name only the known field.

The full exception representation—Message, InnerException messages/data, ToString—obeys no-echo. Unsafe lower-level exceptions are not retained.

Exact raw output belongs to E0 provenance. Malformed output never becomes fiction.

The parser may reuse generic strict-JSON techniques but not fixture-specific limits/schema/FixtureValidationException.

## 19. Candidate Context association versus exact disclosure provenance

CandidatePerformance copies SubjectCharacterId + ContextPacketId from trusted ContextPacket. AI JSON cannot supply/override them.

This identifies the semantic bounded Character context associated with the candidate inside Ensemble.

It does not prove what an external model received and carries no RenderingContract/RenderedContextHash.

For an AI attempt, later orchestration/provenance must associate at least:

- provider/model/version;
- generation/reasoning settings;
- complete Context packet + ContextPacketId;
- RenderingContract + RenderedContextHash;
- future provider-request framing/identity where required;
- JSON transport schema version actually requested/parsed;
- raw Performer output/control/errors/metrics.

That layer can distinguish semantic candidate, transport schema, and exact external disclosure. CandidatePerformance remains Performer-neutral.

## 20. No CandidateId / TakeId semantics

Patch 0006 defines no CandidateId/CandidateHash, TakeId allocation, accepted/rejected/alternate Take identity, Take numbering, RunId/TakeId composition, or CommitId composition.

Candidate remains provisional.

## 21. Raw provider output and E0 provenance

CandidatePerformance stores semantic validated output, not complete raw provider response or original JSON spelling.

Later provider/provenance must preserve as applicable complete raw candidate output, partial streaming, raw typed control, refusal/error payloads, provider/model/version, latency/token/cost, settings, retries/cancellations, transport schema, and exact disclosure/request attribution.

Equivalent JSON spellings may decode to one semantic candidate; provenance preserves transport facts.

Rejected/partial/cancelled output/control remains diagnostics/provenance only and never Production history.

## 22. Provider technical failure versus Character refusal

```text
Provider refusal/error/timeout/cancellation = technical outcome
Character refusal/redirection/silence        = possible Performance
```

Patch 0006 parses only explicit candidate payload bytes. A later provider layer classifies transport outcomes before ParseJson.

Technical error messages must never be automatically wrapped as VisibleText.

## 23. Relationship to future Integrity Validator

Patch 0006 validates candidate-contract invariants only. It does not accept/commit a Take.

Future Integrity Validator owns inaccessible-information use, locked canon/impossible-world violation, creator-only disclosure, provider-failure contamination, required output-contract integrity, and semantic concerns between visible Performance/control.

Model-assisted checks may flag semantic concerns but cannot waive deterministic hard rules.

## 24. Relationship to future Director

Typed control names direct social address + Character nomination, two potential Director inputs frozen by Blueprint 0.1.

Candidate control has no effective pre-commit routing authority. Only associated metadata for a later successfully committed accepted Performance/Take may become an effective Director input.

Speculative Director computation before commit may not trigger Performer, alter effective opportunity, or survive failed commit as routing state.

Post-commit control remains non-binding metadata, not Production truth/state. Director may also consider hard eligibility, interaction relevance, relationships, pressure, participation balance, and repetition.

Patch 0006 does not score/rank/choose next opportunity. Silence does not force handoff.

## 25. Relationship to State Interpreter / State Authority

CandidatePerformance has no mutation authority.

Conceptually later:

```text
CandidatePerformance
-> Integrity / State Interpreter / State Authority / Take semantics
-> atomic causal commit of accepted Performance + approved consequences
-> committed Performance enters Production history/state
-> associated control may be causal provenance + eligible Director input without becoming Production truth
-> Director establishes later effective opportunity
```

Exact runtime pipeline/IDs/persistence remain for later standalone contracts.

## 26. Missing Raft illustrative examples

Transport examples only, not canonical screenplay output:

```json
{
  "schemaVersion": "ensemble.e0.performer.candidate-json.v1",
  "performance": { "text": "Before we decide what happened, what did each of us actually observe?" },
  "control": {
    "addressedCharacterIds": ["MARLOWE", "WREN"],
    "nominatedCharacterId": null
  }
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

Use validated Patch 0005 ContextPacket/fixtures; do not duplicate fixture JSON.

### Semantic/context/text/control

1. valid Voss JSON yields semantic ContractVersion `ensemble.e0.performer.candidate.v1`;
2. transport schema version is not stored as CandidatePerformance contract version/property;
3. SubjectCharacterId copied from ContextPacket;
4. ContextPacketId copied exactly;
5. candidate exposes no RenderingContract/RenderedContextHash/provider/request fields;
6. Context invariants checked before untrusted JSON;
7. opportunity != subject fails;
8. malformed/ambiguous roster IDs fail if encountered;
9. subject resolves exactly once;
10. null ContextPacket fails candidate-specifically;
11. non-empty visible text preserved;
12. empty text valid silence only with empty/null control;
13. whitespace-only fails;
14. zero-width/Format-only fails;
15. combining-mark-only fails;
16. display-bearing + permitted Format/combining valid/preserved;
17. silence with address fails;
18. silence with nomination fails;
19. LF/TAB preserved;
20. forbidden Control scalars fail;
21. invalid surrogate fails;
22. non-NFC fails without normalization;
23. addressed target must exist;
24. uninitialized addressed ID candidate-specific failure;
25. self-address fails;
26. duplicate addresses fail;
27. addressed IDs sorted;
28. addressed array beyond Roster.Length - 1 fails immediately;
29. nomination target must exist;
30. uninitialized nomination candidate-specific failure;
31. self-nomination fails;
32. nomination may overlap address;
33. parsing does not mutate ContextPacket/state;
34. candidate/control constructors non-public;
35. no accepted/committed/validated/routing field;
36. no public semantic factory;
37. no raw JSON/raw provider-output property;
38. no fixture/Production truth/provenance/Access decisions/mutations/confidence/private reasoning/accepted history/CandidateId/TakeId property.

### Strict JSON transport

39. canonical candidate-json.v1 parses;
40. semantic candidate ContractVersion differs from transport schema version;
41. property reordering identical;
42. structural whitespace identical;
43. equivalent escapes identical;
44. Markdown/code fence fails;
45. empty input fails;
46. exactly 1 MiB valid input succeeds;
47. 1 MiB + 1 fails before parse;
48. depth >8 fails;
49. exact transport schemaVersion required/case-sensitive;
50. semantic contract-version string supplied as JSON schemaVersion fails;
51. property names case-sensitive;
52. unknown root/performance/control property fails;
53. missing required property fails;
54. duplicate root/nested/decoded-escaped property fails;
55. comments fail;
56. trailing comma fails;
57. BOM fails;
58. malformed UTF-8 fails;
59. malformed JSON fails;
60. escaped isolated surrogate fails;
61. wrong token types fail;
62. trailing non-whitespace fails;
63. alternate-case Character ID fails;
64. parser/internal builder share invariant path;
65. repeated parse semantically identical;
66. parser does not mutate ContextPacket.

### Diagnostic non-leakage

67. invalid VisibleText sentinel absent from complete exception representation;
68. invalid Character-ID sentinel absent;
69. mismatched JSON schemaVersion sentinel absent;
70. unknown property-name sentinel including control/newline name absent;
71. malformed-payload sentinel absent.

### Policy/causal/public-surface/regression

72. parser does not require `full-authorized.v1`;
73. parser does not require `render.v1`;
74. no omniscient bypass/construction path;
75. public candidate construction ParseJson only;
76. Control exposes no commit/Director authority flag;
77. no path establishes effective Director opportunity from candidate control;
78. semantic candidate contains no AI disclosure/provenance/transport fields;
79. frozen Missing Raft StructuredContextHash unchanged;
80. frozen Missing Raft RenderedContextHash unchanged;
81. frozen Missing Raft ECJ-1 exactly 9112 bytes + frozen SHA-256;
82. all existing 115 Core tests green;
83. Missing Raft Harness PASS/0;
84. generic smoke Harness PASS/0.

Impossible malformed ContextPacket states may be proven by review/defensive branches without adding public invariant-bypass hooks.

## 28. Harness behavior

No live provider call or CLI generation command. Existing Harness output unchanged.

Core tests exercise strict local candidate transport parsing. Provider-execution Harness waits for provider assignment, request/system contract, safe roster mapping, disclosure/request provenance, secrets, response limits, retries/cancellation, refusal classification, and E0 reference configuration.

## 29. ARM64 and battery suitability

Deterministic CPU validation over at most 1 MiB/depth-8 JSON, at most roster-minus-subject control IDs, and one small candidate object.

No network/background/AI/GPU/NPU/filesystem/polling.

Authority validation belongs on CPU: deterministic, cheap relative to inference, and security-sensitive. No NPU execution/performance claim.

## 30. Explicit exclusions

No provider/model call/adapter; provider request/system prompt/roster rendering; exact attempt/disclosure/request provenance persistence; provider generation/token/cost limit below parser ceiling; credentials/casting/understudy/settings; streaming/retry/cancellation/refusal classification; raw provenance persistence; Take a Seat construction API/UI; final/multimodal Performance ontology; Director selection; Integrity acceptance; Take semantics/IDs; State Interpreter/Authority/ProductionState/StateHash; atomic commit/history persistence; observation engine; E0-D Composer/omniscient binding; playwright execution; final UI Unicode policy; WinUI/Windows AI/NPU/packaging/WACK/Store.

## 31. Recursive adversarial audit dimensions

Repeat full passes across:

1. Blueprint 0.1 consistency;
2. Patch 0004 Access authority;
3. Patch 0005 Context authority/identity;
4. Character != Performer + future Take a Seat compatibility;
5. semantic Performance vs AI transport vs provider-attempt provenance separation;
6. semantic-contract vs transport-schema version separation;
7. Director != Performer;
8. Integrity separation;
9. State Interpreter != State Authority;
10. statement != fact;
11. Take/history/atomic commit compatibility;
12. causal provenance vs creative Production history;
13. provider-error-to-fiction prevention;
14. JSON ambiguity/malformed behavior;
15. Unicode/invisible-text behavior;
16. stable identity/roster resolution;
17. typed-control sufficiency/boundedness/overreach;
18. performance-grammar/final-ontology openness;
19. public API minimality;
20. safe Context composition decoupling without weakening omniscient boundary;
21. untrusted-input resource/diagnostic leakage;
22. causal ordering/effective pre-commit effects vs harmless speculation;
23. reconstruction when Director consumes control;
24. scope/premature abstraction;
25. ARM64/battery suitability;
26. test completeness + E0-B/C/D/E/F/G compatibility.

Any correction restarts the entire pass. Approval only after one full pass finds zero remaining material error or worthwhile improvement.

## 32. Exit gate

Before implementation promotion:

1. zero-material-change recursive pass + explicit user approval;
2. implementation from then-current main dedicated branch;
3. CandidatePerformance semantic/distinct from AI JSON/provider provenance;
4. semantic ContractVersion separate from AI JSON schemaVersion;
5. no AI RenderingContract/RenderedContextHash in semantic candidate;
6. no speculative human public factory;
7. E0 text representation does not freeze final ontology;
8. no PerformanceKind taxonomy;
9. parser/internal builder one invariant path;
10. parser uses only ContextPacket fields consumed, no composition/rendering string gate;
11. no omniscient bypass;
12. trusted Context invariants before JSON;
13. candidate association SubjectCharacterId + ContextPacketId only from packet;
14. exact disclosure/provider/transport attribution deferred to attempt provenance;
15. JSON/size/depth/control bounds reviewed;
16. full exception chain has no untrusted leakage;
17. VisibleText preserved/untrusted/no invisible pseudo-performance;
18. typed control address+nomination provisional;
19. pre-commit control cannot create effective opportunity/trigger Performer; failed control may remain provenance only; speculation discardable;
20. post-commit associated control may inform Director without becoming Production history/truth/state consequence;
21. control actually used by Director retained in causal/provenance records sufficient for reconstruction;
22. no mutations/private reasoning/provider runtime data in candidate;
23. roster IDs exact/case-sensitive;
24. constructors cannot bypass validation;
25. malformed/oversized/deep/control-overflow fail technically;
26. exact parser-size boundary proven;
27. existing 115 tests green;
28. frozen Context identities unchanged;
29. frozen ECJ-1 unchanged;
30. native ARM64 build/test/harness gates pass;
31. hygiene finds no later-scope creep;
32. evidence separates machine-tested executable head from docs closure.

## 33. Material approval decisions

Approval freezes:

1. Patch 0006 is Performer candidate-output contract after Context Composer and before Director/Integrity/State/Take/commit;
2. no provider/model call;
3. CandidatePerformance is semantic Performer output, not AI JSON or provider-attempt provenance;
4. semantic contract `ensemble.e0.performer.candidate.v1` is separate from AI JSON transport schema `ensemble.e0.performer.candidate-json.v1`;
5. candidate contains ContractVersion, SubjectCharacterId, ContextPacketId, VisibleText, Control only;
6. RenderingContract/RenderedContextHash/provider/model/request/transport identity remain attempt provenance, not candidate fields;
7. ParseJson accepts safe ContextPacket without hard-coding composition/rendering policy strings;
8. omniscient E0-D remains separate binding, never masquerades as safe ContextPacket;
9. future Take a Seat may reuse semantic invariants; no speculative public human factory now;
10. E0 text representation does not freeze final product ontology;
11. no PerformanceKind enum; empty text silence, non-empty grammar-open;
12. non-empty text requires display-bearing scalar;
13. VisibleText preserved + untrusted; invalid Unicode/control/NFC rejected, never repaired;
14. typed control only address IDs + optional nomination;
15. address IDs roster-minus-subject bounded/immediate overflow failure;
16. control provisional intent metadata, not truth/state/observation/Director authority or Production history;
17. pre-commit control cannot create effective opportunity/trigger Performer/survive rollback as routing state;
18. associated control for successfully committed accepted Performance may become non-binding Director input without becoming Production truth/history/state consequence;
19. if consumed by effective Director decision, exact semantic control input + decision retained in causal/provenance record sufficient for reconstruction;
20. failed/rejected control may remain experiment provenance only;
21. discardable speculative Director computation before commit allowed only with zero effective authority and rollback discard;
22. nomination may overlap address and never obligates response;
23. provider request later exposes stable IDs only via safe roster-derived machine-control mapping separate from creative text;
24. no mutation/confidence/private reasoning/world-fact proposal/CandidateId/TakeId/provider metadata in candidate;
25. strict JSON fail-closed, semantic case-sensitive, property-order/structural-whitespace insensitive, no repair;
26. parser ceilings 1 MiB inclusive/depth 8, not generation budgets;
27. exceptions expose only trusted structural diagnostics across full chain;
28. public construction ParseJson only; non-public builder owns semantic invariants;
29. candidate/control constructors non-public with no acceptance/commit/routing flags;
30. exact raw/partial/error provider output + exact transport/disclosure/request data remain separate provenance and never automatically Production history;
31. Character refusal/redirection/silence distinct from provider refusal/error/cancellation;
32. provider execution, human UI/factory, Director, Integrity, State Interpreter, State Authority, Take semantics, atomic commit, persistence, and E0-D binding remain outside Patch 0006.

Implementation must not begin until recursive audit completes and these decisions are explicitly approved.
