# H1 Patch 0006 — Performer Candidate Output Contract

Status: blueprint proposal 1.2 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0005
Branch: `h1-patch-0006-performer-candidate-blueprint`

## 1. Purpose

Implement the next E0-A boundary after the validated Context Composer:

`ContextPacket -> Performer -> provisional CandidatePerformance -> later Integrity / Take / causal commit`

Patch 0006 answers one narrow question:

> Can Ensemble represent one provisional Character Performance as a strict, schema-bounded, provider-neutral semantic candidate, deterministically associated with the exact reference ContextPacket supplied for that candidate attempt, while keeping model-supplied control data provisional and preventing malformed output, provider failures, hidden reasoning, mutation proposals, invisible pseudo-performance, or uncommitted routing signals from entering fiction or authority?

Patch 0006 defines and implements the candidate-output semantic contract plus its strict E0 JSON transport parser. It does **not** call any provider/model.

ContextPacket hashes provide deterministic local content identity for association. They are not signatures, MACs, provider authentication, authorization, causal proof of what a provider actually saw, or proof that a particular model/person produced a candidate.

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
- Integrity Validator checks candidate Performance before acceptance;
- State Interpreter proposes consequences; deterministic State Authority decides what may commit;
- accepted Performance and approved consequences form one atomic causal commit; either both commit coherently or neither does;
- generated work becomes Production history only through later acceptance/commit;
- provider error/refusal/timeout/retry must never become fictional action;
- partial/cancelled/unaccepted output must not enter Production history;
- experimental provenance must preserve Performer outputs, hidden typed control output, errors/refusals/cancellations, and later validation/commit results;
- imported text and fictional dialogue are untrusted creative content rather than instruction authority;
- human Take a Seat uses the same deterministic Access Control -> Context Composer information boundary as an AI Performer.

Validated Patch 0005 freezes the immediate upstream reference boundary:

- `ContextPacket` is the Character-safe reference Composer artifact;
- `ContextPacketId = CTX:<StructuredContextHash>` identifies structured semantic context;
- `RenderingContract` versions provider-neutral rendering;
- `RenderedContextHash` identifies rendering contract + exact rendered disclosure;
- ContextPacket roster contains safe CharacterId + DisplayName structural metadata;
- provider-neutral creative rendering deliberately omits Character IDs;
- final system/provider request construction remains outside Context Composer;
- relationship-omitted E0-D is a later separately labeled safe composition path;
- omniscient E0-D remains outside safe Access Control and outside the reference Composer.

Patch 0006 preserves those authority meanings.

## 3. Why Performer candidate contract comes next

The frozen continuation order explicitly places the Performer candidate-output contract before Director, Integrity Validator, State Interpreter, State Authority, Take semantics, and causal commit.

This ordering is implementable now because the frozen Missing Raft fixture already supplies the first opportunity (`VOSS`). Dynamic Director selection is therefore not required to establish the first candidate boundary.

A stable candidate vocabulary is prerequisite for later boundaries:

- Director needs a stable committed-Performance/control input vocabulary;
- Integrity Validator needs a concrete provisional candidate object;
- State Interpreter must interpret an accepted Performance rather than an untyped provider string;
- Take semantics must identify accepted/rejected attempts without redefining candidate content.

Patch 0006 does not implement any of those later authorities.

## 4. Semantic candidate is not the raw AI transport

`CandidatePerformance` is the semantic Performer-output object. The E0 JSON shape is a transport representation used to construct it, not the domain object itself.

```text
Semantic Performer candidate
    CandidatePerformance

E0 AI transport representation
    ensemble.e0.performer.candidate.v1 JSON
```

This distinction preserves future human Take a Seat compatibility without adding speculative public API now:

- AI E0 uses the public strict JSON parser;
- CandidatePerformance itself contains no provider/raw-JSON concepts;
- a later explicitly approved Take a Seat slice may expose a validated semantic construction surface that reuses the same internal invariants without changing CandidatePerformance schema or bypassing Context authority.

Patch 0006 does **not** ship a public human-oriented `Create(...)` factory merely for future convenience. That API is deferred until a real non-JSON construction caller enters scope.

Neither the current JSON path nor a future human path may accept omniscient Production state merely because it is constructing a Performer candidate.

## 5. Semantic candidate shape

```text
CandidatePerformance
- SchemaVersion
- SubjectCharacterId
- ContextPacketId
- RenderingContract
- RenderedContextHash
- VisibleText
- Control
```

```text
CandidatePerformanceControl
- AddressedCharacterIds
- NominatedCharacterId
```

Rules:

- `SchemaVersion` is always the canonical Patch 0006 candidate contract identifier;
- Subject/context association fields are copied only from the supplied reference ContextPacket;
- `VisibleText` is the Character-legible provisional Performance representation;
- `Control` is non-visible typed **Performer intent/assertion** output;
- neither candidate nor Control contains an `accepted`, `committed`, `validated`, routing weight, score, or Director-decision flag;
- the candidate is provisional and has no truth, acceptance, persistence, state-mutation, commit, or Director-selection authority.

Patch 0006 deliberately defines **no PerformanceKind enum**.

## 6. Why there is no PerformanceKind enum

Blueprint 0.1 says a Performance may contain speech, action, silence, refusal, redirection, or another Character-legible response. It separately leaves performance grammar open: dialogue, dramatic, literary, simulation, or other grammars remain unresolved.

A required `speech|action|mixed|silence` enum would prematurely force the Performer to classify portrayal semantics and could distort E0 by making ambiguous or blended behavior fit an invented engine taxonomy.

Therefore:

- empty `VisibleText` represents silence;
- non-empty `VisibleText` may express speech, action, refusal, redirection, evasion, combinations, or another Character-legible response;
- structural parsing does not classify dramatic meaning;
- later Integrity/State interpretation may reason about semantic content under their own approved contracts without changing preserved Performance text.

This preserves performance-grammar freedom and avoids message-centric ontology lock-in.

## 7. Candidate contract version

Patch 0006 freezes:

```text
PerformerCandidateContract = ensemble.e0.performer.candidate.v1
```

The semantic candidate exposes this value as `SchemaVersion`.

For AI JSON input, `schemaVersion` must match exactly before candidate construction.

The identifier is structural only. It grants no authority.

## 8. Reference ContextPacket dependency without composition-policy coupling

The Patch 0006 public parser accepts the validated reference `ContextPacket` type produced by the Patch 0005 safe Context boundary.

It does **not** gate on a particular `CompositionContract` or `RenderingContract` string because CandidatePerformance does not interpret either policy. It only copies/associates the fields it consumes. This prevents the candidate schema from becoming unnecessarily coupled to safe composition variants such as a later relationship-omitted E0-D path.

This decoupling does **not** authorize omniscient Production state to masquerade as the reference Character-safe ContextPacket. Patch 0005 explicitly keeps omniscient E0-D outside safe Access Control and outside the reference Composer.

If the later omniscient ablation uses a separately labeled experimental context type/binding path, that path may reuse the same **CandidatePerformance semantic schema and internal invariants** without redefining the Performer output contract. Patch 0006 does not pre-build that experimental adapter.

### Local Context invariants candidate-v1 actually validates

Before parsing untrusted JSON bytes, `ParseJson` validates the supplied trusted ContextPacket sufficiently to establish the local candidate boundary:

- ContextPacket is non-null;
- SubjectCharacterId and OpportunityCharacterId are initialized;
- `OpportunityCharacterId == SubjectCharacterId` for this E0 single-opportunity candidate path;
- ContextPacketId is initialized;
- `Rendered` is non-null;
- `Rendered.RenderingContract` is non-null/non-empty because it is copied as association metadata;
- `RenderedContextHash` is non-null/non-empty because it is copied as association metadata;
- roster Character IDs are initialized and unique;
- subject appears exactly once in roster.

Only after those trusted local invariants pass does the parser inspect untrusted candidate JSON.

Each decoded control target must then resolve exactly once in the established roster.

Patch 0006 does **not** recompute Context hashes, recanonicalize Context, inspect denied information, or duplicate Context record-level validation.

## 9. Exact E0 AI JSON transport shape

Candidate output bytes must decode as one strict JSON object:

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

`nominatedCharacterId` is either a canonical Character ID string or JSON `null`.

Rules:

- property order is semantically insignificant;
- ordinary JSON structural whitespace is insignificant;
- equivalent JSON escapes that decode to identical validated strings produce the same semantic candidate;
- Markdown/code fences fail;
- no additional root, `performance`, or `control` properties are permitted;
- full root object must be present; trailing non-whitespace content fails.

### Deterministic parser safety ceilings

```text
MaxCandidateJsonBytes = 1,048,576 bytes (1 MiB)
MaxCandidateJsonDepth = 8
```

These are parser/denial-of-service safety ceilings, not generation targets, token budgets, artistic limits, or E0 optimization policy.

A later provider-execution contract may freeze a substantially smaller response/generation limit for experimental control, cost, or latency. That later limit does not change these parser safety ceilings unless explicitly versioned.

## 10. Visible Performance text contract

### Silence

Silence is exactly:

```text
VisibleText == ""
```

For silence:

- `AddressedCharacterIds` must be empty;
- `NominatedCharacterId` must be null.

Ensemble does not invent prose such as “Voss says nothing.” An accepted future silent Take can remain a real causal Performance without fabricated narration.

### Non-silent Performance

A non-empty `VisibleText` is valid only if it contains at least one **display-bearing Unicode scalar**.

For this E0 contract, a scalar is display-bearing when:

- it is not Unicode whitespace; and
- its Unicode category is not `Control`, `Format`, `NonSpacingMark`, `SpacingCombiningMark`, or `EnclosingMark`.

This prevents zero-width/format/combining-only strings from masquerading as visible Performance while carrying hidden control metadata.

Additional rules:

- leading/trailing whitespace is preserved rather than trimmed;
- LF (`U+000A`) and TAB (`U+0009`) are permitted;
- all other Unicode `Control`-category scalars are rejected, including CR, NUL, DEL/C1 controls, backspace, and form feed;
- Unicode `Format` and combining-mark scalars may appear alongside at least one display-bearing scalar so ordinary language/emoji/combining behavior is not globally prohibited;
- invalid surrogate sequences are rejected;
- text must already be Unicode NFC;
- text is preserved exactly after validation;
- no paraphrase, repair, normalization, Markdown cleanup, quote insertion, punctuation correction, or hidden rewriting occurs.

The display-bearing rule is a Character-legibility invariant, not a final UI sanitization/typography policy.

## 11. VisibleText remains untrusted creative content

Structural parsing does **not** promote candidate prose to instruction authority or objective truth.

Therefore:

- text resembling system instructions has no system authority;
- a Character statement does not become Production truth because parsing succeeded;
- text cannot grant knowledge, change canon/state, select the next Performer, or commit history;
- accepted future Performance may establish that **the Performance occurred** only through the later successful causal commit; propositions inside remain claims/beliefs/etc. unless separately authorized by later State Authority.

This carries `statement != fact` through the Performer boundary.

## 12. Typed control vocabulary

The non-visible typed control block is deliberately minimal and derives directly from frozen future Director inputs.

### AddressedCharacterIds

Represents the Performer’s machine-readable assertion/intent that one or more other roster Characters are directly socially addressed/turned toward by the Performance.

It does **not** mean generic causal impact, observation eligibility, or world-action target.

Rules:

- zero or more Character IDs;
- exact ordinal/case-sensitive match to ContextPacket roster;
- subject may not appear;
- duplicate IDs fail rather than silently deduplicate;
- semantic candidate stores IDs sorted ordinally;
- valid unique maximum is `ContextPacket.Roster.Length - 1` because self-address is prohibited;
- JSON parsing fails immediately when the array contains more entries than that roster-derived maximum, before accumulating further control entries.

### NominatedCharacterId

Represents optional explicit Character-level handoff/nomination intent.

Rules:

- null or one exact roster Character ID;
- subject may not nominate self;
- nomination does not select the next Performer;
- nomination does not obligate response;
- nomination grants no knowledge/truth/state authority;
- nominated Character may also appear in AddressedCharacterIds.

## 13. Provisional control, acceptance, commit, and Director eligibility

`CandidatePerformance.Control` is provisional Performer output exactly like `VisibleText` is provisional Performer output.

Before later causal acceptance/commit completes, it has **no Director effect**. Rejected, malformed, failed, cancelled, unaccepted, or transactionally uncommitted candidate control must never advance attention.

Patch 0006 does not invent a separate `DirectorEligible` projection, semantic-grounding authority, or routing score.

The later Integrity/Take/State/commit contracts own the exact transition from candidate to committed accepted Performance. Only control belonging to a Performance/Take that has successfully entered the later atomic causal commit may become available to the Director for the next opportunity.

Even after commit:

- control is committed Performer intent, not a Director command;
- Director may consider it but need not follow it;
- nomination remains opportunity pressure, not obligation;
- control never grants truth, knowledge, state mutation, or observation eligibility.

Semantic mismatch between visible Performance and typed control may be an Integrity concern under the later approved Integrity contract; Patch 0006 does not invent a fuzzy semantic acceptance algorithm now.

This preserves:

`Performer proposes -> Integrity/State/Take path establishes one committed causal Performance -> Director may consider its committed social intent when choosing the next opportunity.`

The exact runtime ordering is frozen later; Patch 0006 freezes only the prohibition on Director consuming pre-commit candidate control.

## 14. Stable Character IDs and future provider request

Patch 0005 omits internal Character IDs from provider-neutral creative prose, but typed control needs stable IDs because display names are not identity.

A later provider-request contract must therefore supply an allowed machine-control roster mapping derived only from safe ContextPacket roster metadata, conceptually:

```text
MARLOWE -> Marlowe
VOSS    -> Dr. Voss
WREN    -> Wren
```

Requirements:

- mapping derives only from ContextPacket roster;
- no denied record, provenance, Production truth, or private other-Character state leaks;
- mapping stays separate from provider-neutral creative state text;
- mapping exists only to enable stable control IDs;
- Access Control authority does not change.

Patch 0006 does not construct provider requests/system prompts. Future Take a Seat UI may resolve human-facing choices to the same safe IDs without visually exposing technical identifiers.

## 15. No state-mutation or private-reasoning channel

Candidate schema contains no:

- fact/world proposal;
- knowledge/belief/memory/relationship/pressure/world/identity mutation;
- confidence score;
- consequence list;
- chain-of-thought, scratchpad, private rationale, or private monologue;
- CandidateId/TakeId.

Consequences belong to State Interpreter; authority belongs to State Authority. Provider-supported metadata may later be stored separately in diagnostics/provenance.

## 16. Construction authority and minimal public surface

`CandidatePerformance` and `CandidatePerformanceControl` are public read-only outputs with no public constructors.

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
- validates initialized Character IDs and roster/self/duplicate rules;
- sorts addressed IDs ordinally;
- copies trusted context-association fields;
- sets canonical SchemaVersion;
- constructs immutable candidate/control objects.

A future Take a Seat implementation may expose an approved semantic entry point using those same invariants when a real caller exists. Patch 0006 does not create that speculative API now.

Parsing a candidate is not acceptance or authority.

## 17. Strict AI JSON parser

Strict requirements:

- validate trusted local Context invariants first;
- non-empty UTF-8 input;
- maximum 1 MiB raw JSON;
- UTF-8 BOM rejected;
- root one object;
- max JSON depth 8;
- comments/trailing commas rejected;
- duplicate decoded property names rejected at every object level;
- unknown properties rejected;
- required properties exactly once;
- property names and schemaVersion exact/case-sensitive;
- `performance` object with string `text` only;
- `control` object with string-array `addressedCharacterIds` and string-or-null `nominatedCharacterId` only;
- addressed array cannot exceed roster-derived maximum;
- wrong token types fail;
- malformed UTF-8/JSON fails;
- non-whitespace after root fails.

One small Performer-candidate-specific exception represents technical parse/semantic contract failure. Default/uninitialized decoded Character IDs become this domain exception instead of leaking incidental argument/initialization exceptions.

### Error-message safety

Exception messages may include only safe structural information such as a known contract field path, expected token/category, byte/line position, or trusted roster Character ID when relevant.

They must not echo untrusted values, including raw payload, VisibleText, unknown property names, invalid IDs, actual mismatched schemaVersion strings, arbitrary values, credentials/secrets, or surrounding JSON snippets.

Unknown-property diagnostics name only the containing known object. Invalid-ID diagnostics name only the known field.

Lower-level parser exceptions may be retained only when their messages/data do not quote untrusted payload values. Exact raw output belongs to separately governed E0 provenance, not ordinary exception text.

Malformed output never becomes fictional Performance.

The parser may reuse generic strict-JSON techniques but not fixture-specific byte limits, fixture schema rules, or `FixtureValidationException`.

## 18. Candidate association with Context disclosure

Validated candidate construction copies from the supplied reference ContextPacket:

```text
SubjectCharacterId
ContextPacketId
Rendered.RenderingContract
RenderedContextHash
```

AI JSON cannot supply or override these.

The candidate records deterministic association with the intended Character context. It does **not** prove an external model actually received those bytes; provider-attempt evidence belongs to later orchestration/provenance.

This association is not authentication, signature, authorization, acceptance, or truth authority.

## 19. No CandidateId / TakeId semantics

Patch 0006 does not define CandidateId/CandidateHash, TakeId allocation, accepted/rejected/alternate Take identity, Take numbering, RunId/TakeId composition, or CommitId composition.

The candidate remains provisional. Exact raw attempts can be preserved later without pretending they are accepted Take identity.

## 20. Raw provider output and E0 provenance

`CandidatePerformance` stores semantic validated output, not complete raw provider response or original JSON spelling.

Later provider/provenance work must separately preserve as applicable complete raw candidate output, partial streaming, refusal/error payloads, latency/token/cost, generation/reasoning settings, retries/cancellations, and model/provider identity.

Equivalent JSON spellings may decode to one semantic candidate; provenance preserves transport facts.

Rejected/partial/cancelled output remains diagnostics/provenance only and never Production history.

## 21. Provider technical failure versus Character refusal

```text
Provider refusal/error/timeout/cancellation = technical outcome
Character refusal/redirection/silence        = possible Performance
```

Patch 0006 parses only explicit candidate payload bytes. A later provider layer must classify transport outcomes before invoking ParseJson.

Transport/authentication/billing/timeout/retry/error messages must never be automatically wrapped as VisibleText.

## 22. Relationship to future Integrity Validator

Patch 0006 validates structural/semantic candidate-contract invariants only. It does not accept or commit a Take.

Future Integrity Validator owns approved checks such as inaccessible-information use, locked canon/impossible-world violations, creator-only disclosure, provider-failure contamination, output-contract integrity, and semantic concerns between visible Performance and typed control.

Model-assisted checks may flag semantic concerns but cannot waive deterministic hard rules.

## 23. Relationship to future Director

Typed control names two possible Director inputs already frozen in Blueprint 0.1:

- direct social address;
- Character nomination.

CandidatePerformance.Control itself is never a pre-commit Director input. Only corresponding control belonging to a later successfully committed accepted Performance/Take may become available under the future Director contract.

Even then it remains one non-binding input. Director may also consider hard eligibility, interaction relevance, relationships, pressure, participation balance, and repetition.

Patch 0006 does not score, weight, rank, enforce quotas, or choose the next opportunity. Silence does not force a handoff.

## 24. Relationship to State Interpreter / State Authority

CandidatePerformance has no mutation authority.

Conceptually later:

```text
CandidatePerformance
-> Integrity / State Interpreter / State Authority / Take semantics
-> atomic causal commit of accepted Performance + approved consequences
-> committed Performance/control becomes eligible historical input
-> Director chooses a later opportunity under its own contract
```

The exact runtime pipeline and identifiers remain for the later standalone contracts; Patch 0006 freezes only compatibility constraints and pre-commit prohibitions.

## 25. Missing Raft illustrative examples

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

## 26. Required implementation tests

Use validated Patch 0005 reference ContextPacket path and existing fixtures; do not duplicate fixture JSON.

### Candidate/context/text/control invariants

1. valid Voss JSON produces canonical Candidate SchemaVersion;
2. SubjectCharacterId copied from ContextPacket;
3. ContextPacketId copied exactly;
4. RenderingContract copied exactly;
5. RenderedContextHash copied exactly;
6. Context invariants are checked before untrusted JSON parsing;
7. opportunity != subject fails closed;
8. malformed/ambiguous roster IDs fail closed if encountered;
9. subject resolves exactly once in roster;
10. null ContextPacket fails candidate-specifically;
11. non-empty visible text preserved exactly;
12. exact empty text is valid silence only with empty/null control;
13. whitespace-only non-empty text fails;
14. zero-width/Format-only text fails;
15. combining-mark-only text fails;
16. display-bearing text plus permitted Format/combining content remains valid/preserved;
17. silence with address fails;
18. silence with nomination fails;
19. LF/TAB preserved;
20. forbidden Control scalars fail;
21. invalid surrogate fails;
22. non-NFC fails rather than normalizes;
23. addressed target exists in roster;
24. uninitialized addressed ID fails candidate-specifically;
25. self-address fails;
26. duplicate addressed IDs fail;
27. addressed IDs stored ordinally;
28. addressed array beyond Roster.Length - 1 fails immediately;
29. nomination target exists;
30. uninitialized nomination fails candidate-specifically;
31. self-nomination fails;
32. nomination may overlap addressed set;
33. parsing does not mutate ContextPacket/Character state;
34. candidate/control constructors non-public;
35. no accepted/committed/validated/routing-decision field exists;
36. no public semantic factory;
37. exceptions do not echo invalid VisibleText sentinel;
38. invalid-ID exceptions do not echo ID sentinel;
39. CandidatePerformance exposes no fixture, Production truth, provenance, Access decisions, mutations, confidence, private reasoning, provider config/credentials, accepted history, CandidateId, or TakeId.

### Strict JSON

40. canonical JSON parses;
41. property reordering semantically identical;
42. structural whitespace semantically identical;
43. equivalent escapes semantically identical;
44. Markdown/code fences fail;
45. empty input fails;
46. >1 MiB fails;
47. depth >8 fails;
48. schemaVersion exact/case-sensitive;
49. mismatch diagnostic does not echo actual version;
50. property names case-sensitive;
51. unknown root/performance/control properties fail;
52. unknown-name diagnostic does not echo sentinel;
53. missing required property fails;
54. duplicate root/nested/decoded-escaped property names fail;
55. comments fail;
56. trailing comma fails;
57. BOM fails;
58. malformed UTF-8 fails;
59. malformed JSON fails;
60. escaped isolated surrogate fails;
61. wrong token types fail;
62. trailing non-whitespace fails;
63. alternate-case Character ID fails;
64. malformed-payload diagnostic does not echo sentinel;
65. parser/internal builder share one invariant path;
66. repeated parse semantically identical;
67. parser does not mutate ContextPacket.

### Context-policy / causal-boundary / public-surface / regression

68. candidate parser does not require `full-authorized.v1` specifically;
69. candidate parser does not require `render.v1` specifically;
70. no omniscient bypass/omniscient construction path is introduced;
71. public candidate construction is ParseJson only;
72. CandidatePerformance.Control exposes no commit/Director authority flag;
73. Patch 0006 contains no path that routes candidate control to Director;
74. frozen Missing Raft StructuredContextHash unchanged;
75. frozen Missing Raft RenderedContextHash unchanged;
76. frozen Missing Raft ECJ-1 remains exactly 9112 bytes + frozen SHA-256;
77. all existing 115 Core tests remain green;
78. Missing Raft Harness remains PASS/0;
79. generic smoke Harness remains PASS/0.

Where intentionally invalid ContextPacket states are impossible through public authority-safe APIs, implementation review may prove defensive branches without creating an invariant-bypass public test hook.

## 27. Harness behavior

Patch 0006 adds no live provider call or normal CLI generation command. Existing Harness validation output remains unchanged.

Core tests exercise local strict candidate parsing. Provider-execution Harness waits for provider assignment, request/system contract, safe roster mapping, secrets, response limits, retries/cancellation, refusal classification, and E0 reference configuration.

## 28. ARM64 and battery suitability

Patch 0006 is deterministic CPU validation over at most 1 MiB/depth-8 JSON, at most roster-minus-subject control IDs, and one small semantic candidate object.

No network, background work, AI inference, GPU, NPU, filesystem I/O, or polling occurs.

Structural validation belongs on CPU because it is deterministic, cheap relative to inference, and authority-sensitive. Parser/roster bounds constrain worst-case local work. No NPU execution/performance claim is made.

## 29. Explicit exclusions

Patch 0006 does not implement:

- provider/model adapter/call;
- provider request/system prompt or machine-control roster rendering;
- provider generation/token/cost limit below parser safety ceiling;
- credentials, casting, understudy execution, generation settings;
- streaming/retry/cancellation/refusal classification;
- raw-response/provenance persistence;
- Human Take a Seat construction API/UI;
- Director selection;
- Integrity acceptance/rejection;
- accepted/rejected/alternate Take semantics or IDs;
- State Interpreter/State Authority/ProductionState/StateHash;
- atomic causal commit or accepted-history persistence;
- observation engine;
- E0-D ablation Composer/omniscient binding implementation;
- playwright control execution;
- final UI Unicode/rendering policy;
- WinUI, Windows AI/NPU, packaging/WACK/Store.

## 30. Recursive adversarial audit dimensions

Before approval, repeat complete passes across:

1. Blueprint 0.1 consistency;
2. Patch 0004 Access authority;
3. Patch 0005 Context authority/identity;
4. Character != Performer + future Take a Seat compatibility;
5. Director != Performer authority;
6. Integrity separation;
7. State Interpreter != State Authority;
8. statement != fact;
9. Take/history/atomic commit compatibility;
10. provenance completeness;
11. provider-error-to-fiction prevention;
12. JSON ambiguity/malformed behavior;
13. Unicode/invisible-text behavior;
14. stable identity/roster resolution;
15. typed-control sufficiency/boundedness/overreach;
16. performance-grammar openness;
17. public API minimality;
18. safe Context composition decoupling without weakening omniscient boundary;
19. untrusted-input resource/diagnostic leakage;
20. causal ordering/pre-commit side effects;
21. scope/premature abstraction;
22. ARM64/battery suitability;
23. test completeness and E0-B/C/D/E/F/G compatibility.

Any correction restarts the entire pass. Approval is requested only after one complete pass finds zero remaining material errors or worthwhile improvements.

## 31. Exit gate

Before implementation promotion:

1. recursive blueprint audit reaches a zero-material-change pass and user explicitly approves;
2. implementation starts from then-current `main` on dedicated branch;
3. CandidatePerformance provider-neutral/distinct from raw JSON;
4. no speculative human-construction API;
5. no PerformanceKind taxonomy;
6. parser/internal builder share one invariant path;
7. candidate parser uses reference ContextPacket fields it consumes without hard-coded composition/rendering strings;
8. decoupling does not introduce omniscient bypass;
9. trusted Context invariants validate before untrusted JSON;
10. public input remains ContextPacket + explicit JSON bytes;
11. association fields copy only from ContextPacket;
12. JSON/size/depth/control-bound behavior passes adversarial review;
13. diagnostics do not leak untrusted content;
14. VisibleText preserved/untrusted and cannot be invisible pseudo-performance;
15. typed control remains address + nomination and provisional;
16. candidate/pre-commit control cannot advance Director;
17. no mutations/private reasoning/provider runtime data in candidate;
18. roster IDs exact/case-sensitive;
19. constructors cannot bypass validation;
20. malformed/oversized/deep/control-overflow inputs fail technically;
21. existing 115 Core tests green;
22. frozen Context identities unchanged;
23. frozen ECJ-1 unchanged;
24. native ARM64 Core/Harness build passes warnings-as-errors;
25. full Core tests pass target machine;
26. Missing Raft/smoke Harness pass/0;
27. hygiene finds no provider adapter, human factory/UI, Director, Integrity, State, Take, commit, persistence, ablation bypass, or later scope;
28. evidence distinguishes machine-tested executable head from docs-only closure.

## 32. Material approval decisions

Approval freezes:

1. Patch 0006 is Performer candidate-output contract after Context Composer and before Director/Integrity/State/Take/commit layers;
2. no provider/model call;
3. CandidatePerformance semantic/provider-neutral, not raw JSON;
4. AI transport schema `ensemble.e0.performer.candidate.v1`;
5. ParseJson accepts safe reference ContextPacket but does not hard-code composition/rendering policy strings it does not interpret;
6. policy decoupling does not authorize omniscient state inside safe reference ContextPacket; omniscient E0-D remains separate future experimental binding;
7. future Take a Seat reuses semantic invariants, but no speculative public human factory now;
8. candidate carries SchemaVersion + SubjectCharacterId + ContextPacketId + RenderingContract + RenderedContextHash copied from ContextPacket;
9. association is not provider receipt/authentication/authorization proof;
10. no PerformanceKind enum; empty text silence, non-empty grammar-open;
11. non-empty text requires display-bearing scalar, preventing invisible-only pseudo-performance;
12. VisibleText preserved after validation and remains untrusted creative content;
13. invalid Unicode/control/NFC rejected, never repaired;
14. typed control only direct-address IDs + optional nomination;
15. addressed IDs roster-minus-subject bounded with immediate overflow failure;
16. typed control is provisional Performer intent, not truth/state/observation/Director authority;
17. pre-commit candidate control cannot affect Director; only control belonging to a later successfully atomically committed accepted Performance/Take may become eligible as a non-binding Director input;
18. nomination may overlap address and never obligates response;
19. provider request later exposes stable IDs only via safe roster-derived machine-control mapping separate from creative text;
20. no mutation/confidence/private reasoning/world-fact proposal/CandidateId/TakeId in candidate schema;
21. strict JSON fail-closed, semantic case-sensitive, order/structural-whitespace insensitive, no repair;
22. parser ceilings 1 MiB/depth 8, not generation budgets;
23. exceptions expose only trusted structural diagnostics, never raw/untrusted candidate values;
24. public construction ParseJson only; non-public builder owns invariants;
25. candidate/control constructors non-public with no acceptance/commit/routing flags;
26. exact raw/partial/error provider data separate provenance/diagnostics and never automatically Production history;
27. Character refusal/redirection/silence distinct from provider refusal/error/cancellation;
28. provider execution, human UI/factory, Director, Integrity, State Interpreter, State Authority, Take semantics, atomic commit, persistence, and E0-D experimental binding remain outside Patch 0006.

Implementation must not begin until recursive audit completes and these decisions are explicitly approved.
