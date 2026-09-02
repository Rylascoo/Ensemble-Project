# H1 Patch 0006 — Performer Candidate Output Contract

Status: blueprint proposal 1.5 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0005
Branch: `h1-patch-0006-performer-candidate-blueprint`

## 1. Purpose

Implement the next E0-A boundary after the validated Context Composer:

`ContextPacket -> Performer -> provisional CandidatePerformance -> later Integrity / Take / causal commit`

Patch 0006 answers one narrow question:

> Can Ensemble represent one provisional Character Performance as a strict, schema-bounded, Performer-neutral semantic candidate associated with its bounded Character context, while keeping typed control provisional and preventing malformed output, provider failures, hidden reasoning, mutation proposals, invisible pseudo-performance, or uncommitted routing effects from entering fiction or authority?

Patch 0006 defines and implements the candidate-output semantic contract plus its strict E0 AI JSON transport parser. It does **not** call any provider/model.

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
- Performance grammar remains an Open Design Register question rather than a frozen engine taxonomy;
- Director manages attention/opportunity and may later consider direct social address and Character nomination, but offers opportunity rather than obligation;
- Integrity Validator checks a candidate Performance before acceptance;
- State Interpreter proposes consequences; deterministic State Authority decides what may commit;
- accepted Performance and approved consequences form one atomic causal commit; either both commit coherently or neither does;
- generated work becomes Production history only through later acceptance/commit;
- provider error/refusal/timeout/retry must never become fictional action;
- partial/cancelled/unaccepted output must not enter Production history;
- experimental provenance must preserve complete Character context packets, Performer outputs including rejected/partial attempts as diagnostics, hidden typed control output, Director inputs/decisions, errors/refusals/cancellations, and later validation/commit results;
- imported text and fictional dialogue are untrusted creative content rather than instruction authority;
- human Take a Seat uses the same deterministic Access Control -> Context Composer information-authority boundary as an AI Performer;
- the exact amount/presentation of Take a Seat guidance remains open after E0.

Validated Patch 0005 freezes the immediate upstream reference boundary:

- `ContextPacket` is the Character-safe reference Composer artifact;
- `ContextPacketId = CTX:<StructuredContextHash>` identifies structured semantic context;
- `RenderingContract` and `RenderedContextHash` identify the provider-neutral rendering contract/exact rendered disclosure, separately from ContextPacket semantic identity;
- ContextPacket roster contains safe CharacterId + DisplayName structural metadata;
- provider-neutral creative rendering deliberately omits Character IDs;
- final provider/system request construction remains outside Context Composer;
- relationship-omitted E0-D is a later separately labeled safe composition path;
- omniscient E0-D remains outside safe Access Control and outside the reference Composer.

Patch 0006 preserves those meanings rather than folding provider-disclosure provenance into the semantic Performance object.

## 3. Why Performer candidate contract comes next

The frozen continuation order explicitly places the Performer candidate-output contract before Director, Integrity Validator, State Interpreter, State Authority, Take semantics, and causal commit.

The frozen Missing Raft fixture already supplies first opportunity `VOSS`, so dynamic Director implementation is not required to establish the first candidate boundary.

A stable candidate vocabulary is prerequisite for later boundaries:

- Director needs a stable vocabulary for a committed Performance plus associated Performer control metadata;
- Integrity Validator needs a concrete provisional candidate object;
- State Interpreter must interpret an accepted Performance rather than an untyped provider string;
- Take semantics must identify accepted/rejected attempts without redefining candidate content.

Patch 0006 implements none of those later authorities.

## 4. Semantic candidate is not raw AI transport or provider-attempt provenance

`CandidatePerformance` is the semantic Performer-output object.

The E0 JSON shape is one AI transport representation used to construct it. Exact provider request/disclosure metadata is a separate later provenance concern.

```text
Semantic Performer candidate
    CandidatePerformance

E0 AI transport representation
    ensemble.e0.performer.candidate.v1 JSON

Later provider-attempt provenance
    provider/model/settings
    exact ContextPacket identity
    RenderingContract + RenderedContextHash
    later provider-request identity/framing
    raw/partial/error output and metrics
```

This distinction matters for Take a Seat: the human occupies the same Performer role and bounded Context authority, but Blueprint 0.1 deliberately leaves the human guidance/presentation amount open. A semantic Performance therefore must not claim that every Performer saw the AI provider-neutral rendering.

Patch 0006 exposes only the strict AI JSON construction path because AI E0 is the current executable caller. A future approved Take a Seat slice may expose a semantic construction path that reuses the same internal candidate invariants without changing CandidatePerformance schema.

Neither AI nor future human construction may bypass Character access merely because a candidate is being created.

## 5. Semantic candidate shape

```text
CandidatePerformance
- SchemaVersion
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

- `SchemaVersion` is the canonical Patch 0006 candidate-contract identifier;
- `SubjectCharacterId` and `ContextPacketId` are copied only from the supplied trusted ContextPacket;
- `VisibleText` is the Character-legible provisional Performance representation;
- `Control` is non-visible typed Performer intent/assertion metadata;
- candidate/control contain no provider, model, RenderingContract, RenderedContextHash, provider-request identity, accepted/committed/validated status, routing weight, score, or Director-decision flag;
- the candidate has no truth, acceptance, persistence, mutation, commit, or Director-selection authority.

Patch 0006 deliberately defines no PerformanceKind enum.

## 6. E0 textual representation without freezing final Performance ontology

E0 is evaluated largely through explicit packets, harness runs, transcripts, and blind transcript comparison. Patch 0006 therefore uses one textual `VisibleText` representation for the E0 Performer candidate.

That does **not** freeze the final post-E0 product ontology as text-only or message-centric.

Blueprint 0.1 explicitly permits Performance to contain speech, action, silence, refusal, redirection, or another Character-legible response and warns against prematurely making persistent ontology message-centric. Future product evidence may justify richer structured, spatial, multimodal, or other Performance representations under a new approved contract.

For E0 candidate-v1:

- empty `VisibleText` represents silence;
- non-empty `VisibleText` may represent speech, action, refusal, redirection, evasion, combinations, or another Character-legible response;
- structural parsing does not classify dramatic meaning;
- no `speech|action|mixed|silence` engine enum is introduced;
- later Integrity/State interpretation may reason about semantic content without rewriting the preserved Performance.

## 7. Candidate contract version

Patch 0006 freezes:

```text
PerformerCandidateContract = ensemble.e0.performer.candidate.v1
```

The semantic candidate exposes that value as `SchemaVersion`.

AI JSON `schemaVersion` must match exactly before candidate construction.

The identifier is structural only and grants no authority.

## 8. Reference ContextPacket dependency without composition-policy coupling

The Patch 0006 public parser accepts the validated safe reference `ContextPacket` type from Patch 0005.

It does **not** gate on a particular `CompositionContract` or `RenderingContract` string because CandidatePerformance does not interpret either policy. This avoids unnecessarily changing Performer-output semantics for safe context-composition variants such as relationship omission.

This decoupling does **not** authorize omniscient Production state inside the reference Character-safe ContextPacket. Patch 0005 explicitly keeps omniscient E0-D outside safe Access Control/reference Composer.

A later separately labeled omniscient experimental binding may reuse the CandidatePerformance semantic schema/internal validation principles without masquerading as the reference safe packet path. Patch 0006 does not implement that adapter.

### Local Context invariants actually consumed by candidate-v1

Before examining untrusted JSON bytes, `ParseJson` validates:

- ContextPacket is non-null;
- SubjectCharacterId and OpportunityCharacterId are initialized;
- `OpportunityCharacterId == SubjectCharacterId` for the E0 single-opportunity candidate path;
- ContextPacketId is initialized;
- roster Character IDs are initialized and unique;
- subject appears exactly once in roster.

Each decoded control target must then resolve exactly once in that roster.

Patch 0006 does not recompute Context hashes, recanonicalize Context, inspect denied information, validate provider rendering, or duplicate Context record-level validation.

## 9. Exact E0 AI JSON transport shape

Candidate bytes must decode as one strict JSON object:

```json
{
  "schemaVersion": "ensemble.e0.performer.candidate.v1",
  "performance": {
    "text": "..."
  },
  "control": {
    "addressedCharacterIds": ["..."],
    "nominatedCharacterId": null
  }
}
```

`nominatedCharacterId` is a canonical roster Character ID string or JSON `null`.

Rules:

- property order is semantically insignificant;
- ordinary JSON structural whitespace is insignificant;
- equivalent JSON escapes decoding to identical validated strings produce the same semantic candidate;
- Markdown/code fences fail;
- no additional root/performance/control properties;
- full root object required; trailing non-whitespace content fails.

### Deterministic parser safety ceilings

```text
MaxCandidateJsonBytes = 1,048,576 bytes (1 MiB), inclusive
MaxCandidateJsonDepth = 8
```

These are denial-of-service/parser safety ceilings, not generation targets, token budgets, artistic limits, or E0 optimization policy.

Exactly 1 MiB may parse if otherwise valid; 1 MiB + 1 byte fails before JSON parsing.

A later provider-execution contract may impose a much smaller experimental/resource limit without changing this Core safety ceiling unless deliberately versioned.

## 10. Visible Performance text contract

### Silence

Silence is exactly:

```text
VisibleText == ""
```

For silence:

- AddressedCharacterIds must be empty;
- NominatedCharacterId must be null.

Ensemble does not invent narration such as “Voss says nothing.” A future accepted silent Take may remain a real causal Performance without fabricated prose.

### Non-silent Performance

A non-empty VisibleText is valid only if it contains at least one **display-bearing Unicode scalar**.

For this E0 contract a scalar is display-bearing when:

- it is not Unicode whitespace; and
- its category is not `Control`, `Format`, `NonSpacingMark`, `SpacingCombiningMark`, or `EnclosingMark`.

This prevents zero-width/format/combining-only strings from masquerading as visible Performance while carrying hidden control metadata.

Additional rules:

- leading/trailing whitespace is preserved;
- LF (`U+000A`) and TAB (`U+0009`) are permitted;
- other Unicode `Control` scalars are rejected, including CR, NUL, DEL/C1, backspace, and form feed;
- Format and combining marks may accompany at least one display-bearing scalar so ordinary language/emoji/combining behavior is not globally prohibited;
- invalid surrogate sequences are rejected;
- text must already be Unicode NFC;
- text is preserved exactly after validation;
- no trim, normalization, paraphrase, repair, Markdown cleanup, quote insertion, punctuation correction, or hidden rewriting.

This is a Character-legibility invariant, not final UI typography/sanitization policy.

## 11. VisibleText remains untrusted creative content

Structural parsing never promotes candidate prose to system instruction or objective truth.

Therefore:

- text resembling system instructions has no system authority;
- a Character statement does not become Production truth because parsing succeeded;
- text cannot grant knowledge, alter canon/state, select the next Performer, or commit history;
- after a future successful causal commit, the fact that **the Performance occurred** may enter history; propositions inside remain claims/beliefs/etc. unless separately authorized by later State Authority.

This carries `statement != fact` through the Performer boundary.

## 12. Typed control vocabulary

The non-visible control block is deliberately minimal and derives from frozen future Director inputs.

### AddressedCharacterIds

Machine-readable Performer assertion/intent that one or more other roster Characters are directly socially addressed/turned toward.

It is not generic causal impact, observation eligibility, or world-action targeting.

Rules:

- zero or more Character IDs;
- exact ordinal/case-sensitive roster match;
- subject may not appear;
- duplicates fail, never silently deduplicate;
- semantic candidate stores IDs sorted ordinally;
- maximum unique entries = `ContextPacket.Roster.Length - 1`;
- parser fails immediately when array entry count exceeds that roster-derived maximum, before accumulating further control entries.

### NominatedCharacterId

Optional explicit Character-level handoff/nomination intent.

Rules:

- null or one exact roster ID;
- subject may not nominate self;
- nomination does not select the next Performer;
- nomination does not obligate response;
- nomination grants no truth/knowledge/state authority;
- nomination may overlap AddressedCharacterIds.

## 13. Provisional control, causal commit, and effective Director routing

CandidatePerformance.Control is provisional Performer metadata.

Before later causal acceptance/commit succeeds, candidate control has **no effective Director authority**. Rejected, malformed, failed, cancelled, unaccepted, or transactionally uncommitted control must never:

- establish Current Attention/Opportunity;
- trigger another Performer invocation;
- persist as effective Director input;
- survive rollback as if its Performance had occurred.

Failed/rejected control may still be preserved as required experiment provenance/diagnostics. That preservation is not Scene history or routing authority.

Patch 0006 does not invent a DirectorEligible projection, semantic-grounding authority, or routing score.

The later Integrity/Take/State/commit contracts own the transition to successfully committed accepted Performance. Associated control metadata for such a committed Performance/Take may then become one permissible non-binding Director input.

Even then:

- accepted Performance + approved authoritative consequences are the causal content entering Production history/state;
- associated control remains Performer intent/assertion metadata, not Production-history prose, objective truth, or an authoritative consequence;
- Director may consider it but need not follow it;
- nomination remains opportunity pressure, not obligation;
- control never grants truth, knowledge, mutation, or observation eligibility.

A future implementation may compute a discardable speculative Director recommendation before commit, but it cannot trigger another Performer, alter effective Current Opportunity, or survive failed/aborted commit as routing state.

## 14. Causal provenance requirement for control used by Director

Production history and causal/experimental provenance are not the same thing.

If associated control metadata from a successfully committed Performance is actually consumed by the future Director to establish an effective opportunity, the exact semantic control input and resulting Director decision must be retained in the later causal/provenance record sufficiently to audit/reconstruct that attention transition.

Ordinary diagnostic deletion must not make a canonically relevant Director decision unreconstructable merely because the control input was treated as disposable logging.

This requirement does **not** promote control to creative Production truth. It preserves causal attribution for an authority decision that used the metadata.

Exact persistence schema/identity belongs to the later Director/Take/causal-commit/provenance contracts.

## 15. Stable Character IDs and future provider request

Patch 0005 omits Character IDs from provider-neutral creative prose, but typed control needs stable IDs because display names are not identity.

A later AI provider-request contract must therefore supply an allowed machine-control roster mapping derived only from safe ContextPacket roster metadata, conceptually:

```text
MARLOWE -> Marlowe
VOSS    -> Dr. Voss
WREN    -> Wren
```

Requirements:

- mapping derives only from ContextPacket roster;
- no denied record, provenance, Production truth, or private other-Character state leaks;
- mapping is separate from provider-neutral creative state text;
- it exists only to enable stable control IDs;
- Access Control authority does not change.

Patch 0006 does not construct provider requests/system prompts. Future Take a Seat UI may resolve human-facing targets to the same safe IDs without displaying technical identifiers.

## 16. No state-mutation or private-reasoning channel

Candidate schema contains no:

- fact/world proposal;
- knowledge/belief/memory/relationship/pressure/world/identity mutation;
- confidence score;
- consequence list;
- chain-of-thought, scratchpad, private rationale, or private monologue;
- CandidateId/TakeId;
- provider/model/settings/request identity.

Consequences belong to State Interpreter; authority belongs to State Authority. Provider-supported metadata belongs in separately governed attempt/provenance data.

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
- validates Character IDs and roster/self/duplicate rules;
- sorts addressed IDs ordinally;
- copies SubjectCharacterId + ContextPacketId;
- sets canonical SchemaVersion;
- constructs immutable candidate/control objects.

A future Take a Seat implementation may expose a validated semantic entry point through the same invariant path when a real non-JSON caller exists. Patch 0006 does not create that speculative public API now.

Parsing a candidate is not acceptance or authority.

## 18. Strict AI JSON parser

Requirements:

- validate trusted local Context invariants first;
- non-empty UTF-8 input;
- maximum 1 MiB inclusive;
- UTF-8 BOM rejected;
- root one object;
- max depth 8;
- comments/trailing commas rejected;
- duplicate decoded property names rejected at every object level;
- unknown properties rejected;
- required properties exactly once;
- property names and schemaVersion exact/case-sensitive;
- performance object contains only string `text`;
- control object contains only string-array `addressedCharacterIds` and string-or-null `nominatedCharacterId`;
- addressed array cannot exceed roster-derived maximum;
- wrong token types fail;
- malformed UTF-8/JSON fails;
- non-whitespace after root fails.

One small candidate-specific exception represents technical parse/semantic contract failure. Default/uninitialized Character IDs become this exception rather than leaking incidental argument/initialization exceptions.

### Error-message safety

Externally observable exception data may include only safe structural information such as known contract field path, expected token/category, byte/line position, or trusted roster ID when relevant.

It must not echo raw payload, VisibleText, unknown property names, invalid IDs, actual mismatched schemaVersion, arbitrary values, credentials/secrets, or surrounding JSON snippets.

Unknown-property diagnostics name only the containing known object. Invalid-ID diagnostics name only the known field.

The full exception representation—including Message, InnerException messages/data, and `ToString()`—obeys the no-echo rule. Unsafe lower-level exceptions are not retained as inner exceptions.

Exact raw output belongs to separately governed E0 provenance.

Malformed output never becomes fictional Performance.

The parser may reuse generic strict-JSON techniques but not fixture-specific limits/schema/FixtureValidationException.

## 19. Candidate Context association versus exact disclosure provenance

CandidatePerformance copies:

```text
SubjectCharacterId
ContextPacketId
```

from the supplied trusted reference packet. AI JSON cannot supply or override them.

This records which bounded semantic Character context the candidate attempt was associated with inside Ensemble.

It does **not** prove what an external model actually received and deliberately does not carry `RenderingContract` or `RenderedContextHash`.

For an AI external attempt, later orchestration/provenance must associate at least:

- provider/model and relevant version;
- generation/reasoning settings;
- ContextPacketId / complete Context packet;
- RenderingContract + RenderedContextHash for exact provider-neutral disclosure;
- future provider-request framing/identity as required;
- raw Performer output/control/errors/metrics.

That layer can truthfully distinguish semantic candidate identity from exact external disclosure. CandidatePerformance itself remains Performer-neutral.

## 20. No CandidateId / TakeId semantics

Patch 0006 does not define CandidateId/CandidateHash, TakeId allocation, accepted/rejected/alternate Take identity, Take numbering, RunId/TakeId composition, or CommitId composition.

The candidate remains provisional. Exact attempts are preserved later without pretending they are accepted Take identity.

## 21. Raw provider output and E0 provenance

CandidatePerformance stores semantic validated output, not complete raw provider response or original JSON spelling.

Later provider/provenance work must preserve as applicable:

- complete raw candidate output;
- partial streaming;
- raw typed-control transport;
- refusal/error payloads;
- provider/model/version;
- latency/token/cost;
- generation/reasoning settings;
- retries/cancellations;
- exact disclosure/request attribution.

Equivalent JSON spellings may decode to one semantic candidate; provenance preserves transport facts.

Rejected/partial/cancelled output/control remains diagnostics/provenance only and never Production history.

## 22. Provider technical failure versus Character refusal

```text
Provider refusal/error/timeout/cancellation = technical outcome
Character refusal/redirection/silence        = possible Performance
```

Patch 0006 parses only explicit candidate payload bytes. A later provider layer classifies transport outcomes before invoking ParseJson.

Technical transport/authentication/billing/timeout/retry/error messages must never be automatically wrapped as VisibleText.

## 23. Relationship to future Integrity Validator

Patch 0006 validates structural/semantic candidate-contract invariants only. It does not accept or commit a Take.

Future Integrity Validator owns approved checks such as inaccessible-information use, locked canon/impossible-world violation, creator-only disclosure, provider-failure contamination, required output-contract integrity, and semantic concerns between visible Performance and typed control.

Model-assisted checks may flag semantic concerns but cannot waive deterministic hard rules.

## 24. Relationship to future Director

Typed control names two possible Director inputs already frozen in Blueprint 0.1:

- direct social address;
- Character nomination.

Candidate control has no effective pre-commit routing authority. Only associated control metadata for a later successfully committed accepted Performance/Take may become an effective Director input under the future Director contract.

A future implementation may compute a provisional Director recommendation before commit, but it cannot trigger a Performer, alter Current Opportunity, or survive failed commit as effective routing state.

Post-commit associated control remains non-binding metadata rather than Production truth/state. Director may also consider hard eligibility, interaction relevance, relationships, pressure, participation balance, and repetition.

Patch 0006 does not score, weight, rank, enforce quotas, or choose the next opportunity. Silence does not force handoff.

## 25. Relationship to State Interpreter / State Authority

CandidatePerformance has no mutation authority.

Conceptually later:

```text
CandidatePerformance
-> Integrity / State Interpreter / State Authority / Take semantics
-> atomic causal commit of accepted Performance + approved consequences
-> committed Performance enters Production history/state
-> associated control may be retained in causal provenance and may become eligible Director input without becoming Production truth
-> Director establishes effective later opportunity under its own contract
```

Exact runtime pipeline/IDs/persistence remain for later standalone contracts. Patch 0006 freezes only compatibility and authority constraints.

## 26. Missing Raft illustrative examples

Examples demonstrate transport shape only, not canonical screenplay output.

```json
{
  "schemaVersion": "ensemble.e0.performer.candidate.v1",
  "performance": {
    "text": "Before we decide what happened, what did each of us actually observe?"
  },
  "control": {
    "addressedCharacterIds": ["MARLOWE", "WREN"],
    "nominatedCharacterId": null
  }
}
```

Silence:

```json
{
  "schemaVersion": "ensemble.e0.performer.candidate.v1",
  "performance": {
    "text": ""
  },
  "control": {
    "addressedCharacterIds": [],
    "nominatedCharacterId": null
  }
}
```

## 27. Required implementation tests

Use validated Patch 0005 reference ContextPacket path and existing fixtures; do not duplicate fixture JSON.

### Candidate/context/text/control invariants

1. valid Voss JSON produces canonical candidate SchemaVersion;
2. SubjectCharacterId copied from ContextPacket;
3. ContextPacketId copied exactly;
4. CandidatePerformance exposes no RenderingContract/RenderedContextHash/provider/request fields;
5. Context invariants checked before untrusted JSON parsing;
6. opportunity != subject fails closed;
7. malformed/ambiguous roster IDs fail closed if encountered;
8. subject resolves exactly once in roster;
9. null ContextPacket fails candidate-specifically;
10. non-empty visible text preserved exactly;
11. exact empty text valid silence only with empty/null control;
12. whitespace-only non-empty text fails;
13. zero-width/Format-only text fails;
14. combining-mark-only text fails;
15. display-bearing text plus permitted Format/combining content valid/preserved;
16. silence with address fails;
17. silence with nomination fails;
18. LF/TAB preserved;
19. forbidden Control scalars fail;
20. invalid surrogate fails;
21. non-NFC fails rather than normalizes;
22. addressed target exists in roster;
23. uninitialized addressed ID fails candidate-specifically;
24. self-address fails;
25. duplicate addressed IDs fail;
26. addressed IDs stored ordinally;
27. addressed array beyond Roster.Length - 1 fails immediately;
28. nomination target exists;
29. uninitialized nomination fails candidate-specifically;
30. self-nomination fails;
31. nomination may overlap addressed set;
32. parsing does not mutate ContextPacket/Character state;
33. candidate/control constructors non-public;
34. no accepted/committed/validated/routing-decision field exists;
35. no public semantic factory;
36. CandidatePerformance exposes no raw JSON/raw provider output field;
37. CandidatePerformance exposes no fixture, Production truth, provenance, Access decisions, mutations, confidence, private reasoning, accepted history, CandidateId, or TakeId.

### Strict JSON

38. canonical JSON parses;
39. property reordering semantically identical;
40. structural whitespace semantically identical;
41. equivalent escapes semantically identical;
42. Markdown/code fences fail;
43. empty input fails;
44. exactly 1 MiB syntactically/semantically valid candidate parses;
45. 1 MiB + 1 byte fails before JSON parsing;
46. depth >8 fails;
47. schemaVersion exact/case-sensitive;
48. property names case-sensitive;
49. unknown root/performance/control properties fail;
50. missing required property fails;
51. duplicate root/nested/decoded-escaped property names fail;
52. comments fail;
53. trailing comma fails;
54. BOM fails;
55. malformed UTF-8 fails;
56. malformed JSON fails;
57. escaped isolated surrogate fails;
58. wrong token types fail;
59. trailing non-whitespace fails;
60. alternate-case Character ID fails;
61. parser/internal builder share one invariant path;
62. repeated parse semantically identical;
63. parser does not mutate ContextPacket.

### Diagnostic non-leakage

64. invalid VisibleText sentinel absent from complete exception ToString/inner chain;
65. invalid Character-ID sentinel absent from complete exception representation;
66. actual mismatched schemaVersion sentinel absent from complete exception representation;
67. unknown property-name sentinel, including control/newline-bearing name, absent from complete exception representation;
68. malformed-payload sentinel absent from complete exception representation.

### Context-policy / causal-boundary / public-surface / regression

69. candidate parser does not require `full-authorized.v1` specifically;
70. candidate parser does not require `render.v1` specifically;
71. no omniscient bypass/omniscient construction path introduced;
72. public candidate construction ParseJson only;
73. CandidatePerformance.Control exposes no commit/Director authority flag;
74. Patch 0006 contains no path establishing effective Director opportunity from candidate control;
75. CandidatePerformance semantic type contains no AI disclosure provenance fields;
76. frozen Missing Raft StructuredContextHash unchanged;
77. frozen Missing Raft RenderedContextHash unchanged;
78. frozen Missing Raft ECJ-1 exactly 9112 bytes + frozen SHA-256;
79. all existing 115 Core tests green;
80. Missing Raft Harness PASS/0;
81. generic smoke Harness PASS/0.

Where invalid ContextPacket states are impossible through public authority-safe APIs, implementation review may prove defensive branches without creating an invariant-bypass public test hook.

## 28. Harness behavior

Patch 0006 adds no live provider call or normal CLI generation command. Existing Harness validation output remains unchanged.

Core tests exercise local strict candidate parsing. Provider-execution Harness waits for provider assignment, request/system contract, safe roster mapping, exact disclosure/request provenance, secrets, response limits, retries/cancellation, refusal classification, and E0 reference configuration.

## 29. ARM64 and battery suitability

Patch 0006 is deterministic CPU validation over at most 1 MiB/depth-8 JSON, at most roster-minus-subject control IDs, and one small semantic candidate object.

No network, background work, AI inference, GPU, NPU, filesystem I/O, or polling occurs.

Structural validation belongs on CPU because it is deterministic, cheap relative to inference, and authority-sensitive. Parser/roster bounds constrain worst-case local work. No NPU execution/performance claim is made.

## 30. Explicit exclusions

Patch 0006 does not implement:

- provider/model adapter/call;
- provider request/system prompt or machine-control roster rendering;
- exact provider-attempt/disclosure/request provenance persistence;
- provider generation/token/cost limit below parser safety ceiling;
- credentials, casting, understudy execution, generation settings;
- streaming/retry/cancellation/refusal classification;
- raw-response/provenance persistence;
- Human Take a Seat construction API/UI;
- final/multimodal Performance ontology;
- Director selection or effective-opportunity state;
- Integrity acceptance/rejection;
- accepted/rejected/alternate Take semantics or IDs;
- State Interpreter/State Authority/ProductionState/StateHash;
- atomic causal commit or accepted-history persistence;
- observation engine;
- E0-D ablation Composer/omniscient binding implementation;
- playwright control execution;
- final UI Unicode/rendering policy;
- WinUI, Windows AI/NPU, packaging/WACK/Store.

## 31. Recursive adversarial audit dimensions

Before approval, repeat complete passes across:

1. Blueprint 0.1 consistency;
2. Patch 0004 Access authority;
3. Patch 0005 Context authority/identity;
4. Character != Performer + future Take a Seat compatibility;
5. semantic Performance versus AI transport/provenance separation;
6. Director != Performer authority;
7. Integrity separation;
8. State Interpreter != State Authority;
9. statement != fact;
10. Take/history/atomic commit compatibility;
11. causal provenance versus creative Production history;
12. provider-error-to-fiction prevention;
13. JSON ambiguity/malformed behavior;
14. Unicode/invisible-text behavior;
15. stable identity/roster resolution;
16. typed-control sufficiency/boundedness/overreach;
17. performance-grammar/final-ontology openness;
18. public API minimality;
19. safe Context composition decoupling without weakening omniscient boundary;
20. untrusted-input resource/diagnostic leakage;
21. causal ordering/effective pre-commit side effects versus harmless speculation;
22. reconstruction requirements when Director consumes associated control;
23. scope/premature abstraction;
24. ARM64/battery suitability;
25. test completeness and E0-B/C/D/E/F/G compatibility.

Any correction restarts the entire pass. Approval is requested only after one complete pass finds zero remaining material error or worthwhile improvement.

## 32. Exit gate

Before implementation promotion:

1. recursive audit reaches zero-material-change pass + user approval;
2. implementation starts from then-current main dedicated branch;
3. CandidatePerformance semantic/distinct from AI JSON and provider-attempt provenance;
4. no AI RenderingContract/RenderedContextHash in semantic candidate;
5. no speculative human-construction API;
6. E0 text representation does not freeze final Performance ontology;
7. no PerformanceKind taxonomy;
8. parser/internal builder one invariant path;
9. parser uses only reference ContextPacket fields it consumes without hard-coded composition/rendering policy strings;
10. no omniscient bypass;
11. trusted Context invariants before untrusted JSON;
12. public input ContextPacket + JSON bytes;
13. candidate association fields SubjectCharacterId + ContextPacketId copy only from ContextPacket;
14. exact AI disclosure/rendering/request attribution deferred to provider-attempt provenance;
15. JSON/size/depth/control-bound behavior reviewed;
16. diagnostics no untrusted leakage through full exception chain;
17. VisibleText preserved/untrusted/no invisible pseudo-performance;
18. typed control address+nomination/provisional metadata;
19. pre-commit control cannot create effective opportunity or trigger Performer; failed/rejected control may remain provenance only; speculation remains discardable;
20. post-commit associated control may inform Director without becoming Production history/truth/state consequence;
21. control actually used for effective Director routing must be preserved in causal/provenance records sufficient for audit/reconstruction;
22. no mutations/private reasoning/provider runtime data in candidate;
23. roster IDs exact/case-sensitive;
24. constructors cannot bypass validation;
25. malformed/oversized/deep/control-overflow inputs fail technically;
26. exact parser-size boundary proven;
27. existing 115 Core tests green;
28. frozen Context identities unchanged;
29. frozen ECJ-1 unchanged;
30. native ARM64 build passes;
31. full tests pass target machine;
32. Missing Raft/smoke pass/0;
33. hygiene finds no provider/human/Director/Integrity/State/Take/commit/persistence/ablation scope creep;
34. evidence separates machine-tested head from docs closure.

## 33. Material approval decisions

Approval freezes:

1. Patch 0006 is Performer candidate-output contract after Context Composer and before Director/Integrity/State/Take/commit layers;
2. no provider/model call;
3. CandidatePerformance is semantic Performer output, not raw AI JSON or provider-attempt provenance;
4. AI transport schema `ensemble.e0.performer.candidate.v1`;
5. semantic candidate contains SchemaVersion, SubjectCharacterId, ContextPacketId, VisibleText, Control only;
6. RenderingContract/RenderedContextHash/provider/model/request identity remain attempt/provenance data, not CandidatePerformance fields;
7. ParseJson accepts safe reference ContextPacket but does not hard-code composition/rendering policy strings it does not interpret;
8. decoupling does not authorize omniscient state inside safe reference ContextPacket; omniscient E0-D remains separate future experimental binding;
9. future Take a Seat may reuse semantic invariants under same bounded Context authority, but no speculative public human factory now;
10. E0 uses textual VisibleText without freezing final product Performance ontology as text-only/message-centric;
11. no PerformanceKind enum; empty text silence, non-empty grammar-open;
12. non-empty text requires display-bearing scalar;
13. VisibleText preserved and untrusted;
14. invalid Unicode/control/NFC rejected, never repaired;
15. typed control only address IDs + optional nomination;
16. addressed IDs roster-minus-subject bounded with immediate overflow failure;
17. typed control is provisional Performer intent/assertion metadata, not truth/state/observation/Director authority or Production history;
18. no candidate/pre-commit control may create effective Current Opportunity, trigger another Performer, or survive rollback as routing state;
19. associated control for a later successfully committed accepted Performance/Take may become a non-binding Director input without becoming Production truth/history/state consequence;
20. if such control is actually consumed by an effective Director decision, exact semantic control input + decision must be retained in later causal/provenance records sufficient for reconstruction/audit;
21. failed/rejected control may be preserved in experiment provenance only;
22. future discardable/speculative Director computation before commit is allowed only with zero effective authority and rollback discard;
23. nomination may overlap address and never obligates response;
24. provider request later exposes stable IDs only via safe roster-derived machine-control mapping separate from creative text;
25. no mutation/confidence/private reasoning/world-fact proposal/CandidateId/TakeId/provider metadata in candidate;
26. strict JSON fail-closed, semantic case-sensitive, property-order/structural-whitespace insensitive, no repair;
27. parser ceilings 1 MiB inclusive/depth 8, not generation budgets;
28. exceptions expose only trusted structural diagnostics across complete exception chain;
29. public construction ParseJson only; non-public builder owns semantic invariants;
30. candidate/control constructors non-public with no acceptance/commit/routing flags;
31. exact raw/partial/error provider output and exact rendering/request disclosure remain separate provenance and never automatically Production history;
32. Character refusal/redirection/silence distinct from provider refusal/error/cancellation;
33. provider execution, human UI/factory, Director, Integrity, State Interpreter, State Authority, Take semantics, atomic commit, persistence, and E0-D experimental binding remain outside Patch 0006.

Implementation must not begin until recursive audit completes and these decisions are explicitly approved.
