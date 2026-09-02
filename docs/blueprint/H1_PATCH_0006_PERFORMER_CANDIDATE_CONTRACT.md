# H1 Patch 0006 — Performer Candidate Output Contract

Status: blueprint proposal 0.9 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0005
Branch: `h1-patch-0006-performer-candidate-blueprint`

## 1. Purpose

Implement the next E0-A boundary after the validated Context Composer:

`ContextPacket -> Performer -> provisional CandidatePerformance -> later Integrity Validator`

Patch 0006 answers one narrow question:

> Can Ensemble represent one provisional Character Performance as a strict, schema-bounded, provider-neutral semantic candidate, deterministically associated with the exact safe ContextPacket supplied for that candidate attempt, while keeping model-supplied control data non-authoritative and preventing malformed output, provider failures, hidden reasoning, mutation proposals, covert routing signals, invisible pseudo-performance, or transport artifacts from entering fiction or authority?

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
- generated work becomes Production history only through later acceptance;
- provider error/refusal/timeout/retry must never become fictional action;
- partial/cancelled/unaccepted output must not enter Production history;
- experimental provenance must preserve Performer outputs, hidden typed control output, errors/refusals/cancellations, and later validation/commit results;
- imported text and fictional dialogue are untrusted creative content rather than instruction authority;
- human Take a Seat uses the same deterministic Access Control -> Context Composer information boundary as an AI Performer.

Validated Patch 0005 freezes the immediate upstream boundary:

- `ContextPacket` is Character-safe;
- `ContextPacketId = CTX:<StructuredContextHash>`;
- `RenderingContract` versions exact provider-neutral rendering;
- `RenderedContextHash` identifies RenderingContract + exact provider-neutral rendered disclosure;
- ContextPacket roster contains safe CharacterId + DisplayName structural metadata;
- provider-neutral creative rendering deliberately omits Character IDs;
- final system/provider request construction remains outside Context Composer.

Patch 0006 preserves all of those separations.

## 3. Why Performer candidate contract comes next

The frozen continuation order explicitly places the Performer candidate-output contract before Director, Integrity Validator, State Interpreter, State Authority, Take semantics, and causal commit.

This ordering is implementable now because the frozen Missing Raft fixture already supplies the first opportunity (`VOSS`). Dynamic Director selection is therefore not required to establish the first candidate boundary.

A stable candidate vocabulary is also prerequisite for later boundaries:

- Director needs a stable accepted/eligible performance-control vocabulary;
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

Patch 0006 does **not** ship a public human-oriented `Create(...)` factory merely for future convenience. That public API is deferred until a real non-JSON construction caller enters scope.

Neither the current JSON path nor any future semantic construction path may accept omniscient Production state.

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
- Subject/context association fields are copied only from the supplied trusted ContextPacket;
- `VisibleText` is the Character-legible provisional Performance representation;
- `Control` is non-visible typed **Performer assertion** output;
- neither candidate nor Control contains an “accepted,” “validated,” “DirectorEligible,” weight, score, or routing-authority flag;
- the candidate is provisional and has no truth, acceptance, persistence, Director, or mutation authority.

Patch 0006 deliberately defines **no PerformanceKind enum**.

## 6. Why there is no PerformanceKind enum

Blueprint 0.1 says a Performance may contain speech, action, silence, refusal, redirection, or another Character-legible response. It separately leaves performance grammar open: dialogue, dramatic, literary, simulation, or other grammars remain unresolved.

A required `speech|action|mixed|silence` enum would prematurely force the Performer to classify portrayal semantics and could distort E0 by making ambiguous or blended human behavior fit an invented engine taxonomy.

Therefore:

- empty `VisibleText` represents silence;
- non-empty `VisibleText` may express speech, action, refusal, redirection, evasion, combinations, or another Character-legible response;
- structural parsing does not classify the dramatic meaning of the text;
- later Integrity/State interpretation may reason about semantic content under their own approved contracts without changing the preserved Performance text.

This preserves ODR performance-grammar freedom and avoids message-centric ontology lock-in.

## 7. Candidate contract version

Patch 0006 freezes:

```text
PerformerCandidateContract = ensemble.e0.performer.candidate.v1
```

The semantic candidate exposes this value as `SchemaVersion`.

For AI JSON input, `schemaVersion` must match this value exactly before candidate construction.

The identifier is structural only. It grants no authority.

## 8. Supported upstream Context contract

`ensemble.e0.performer.candidate.v1` is defined against the validated Patch 0005 E0 reference Context contract and fails closed on a different context contract unless a later explicitly approved Performer-candidate version extends compatibility.

Required upstream identifiers:

```text
Context SchemaVersion      = ensemble.e0.context.v1
CompositionContract        = ensemble.e0.context.full-authorized.v1
RenderingContract          = ensemble.e0.context.render.v1
```

Candidate construction verifies those identifiers from the supplied ContextPacket/RenderedContext and verifies the Patch 0005 E0 invariant that `OpportunityCharacterId == SubjectCharacterId`.

It does **not** reimplement Context canonicalization, recompute Context hashes, or duplicate the Context Composer’s record-level validation. `ContextPacket` remains the canonical safe upstream authority object.

This check prevents candidate-v1 from silently accepting a future incompatible Context schema/rendering contract under old semantics.

## 9. Exact E0 AI JSON transport shape

Candidate output bytes must decode as one strict JSON object of this semantic shape:

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

- JSON property order is semantically insignificant;
- ordinary JSON structural whitespace is semantically insignificant;
- equivalent JSON escape spellings that decode to the same validated strings produce the same semantic candidate;
- Markdown/code fences are not part of the contract and fail;
- no additional root, `performance`, or `control` properties are permitted;
- the full root object must be present; trailing non-whitespace content fails.

### Deterministic parser safety ceilings

The public untrusted-byte parser fails closed when candidate JSON exceeds:

```text
MaxCandidateJsonBytes = 1,048,576 bytes (1 MiB)
MaxCandidateJsonDepth = 8
```

These are parser/denial-of-service safety ceilings, not generation targets, token budgets, artistic limits, or E0 optimization policy. They sit well above the depth/size required by the frozen candidate schema while preventing pathological unbounded parsing through the public Core boundary.

A later provider-execution contract may freeze a substantially smaller response/generation limit for cost, latency, or experiment control. That later limit does not change these parser safety ceilings unless explicitly versioned.

## 10. Visible Performance text contract

### Silence

Silence is represented semantically by:

```text
VisibleText == ""
```

For silence:

- `AddressedCharacterIds` must be empty;
- `NominatedCharacterId` must be null.

Ensemble does not invent prose such as “Voss says nothing.” An accepted future silent Take can remain a real causal Performance without fabricated dialogue/narration.

### Non-silent Performance

A non-empty `VisibleText` is valid only if it contains at least one **display-bearing Unicode scalar**.

For this E0 contract, a scalar is display-bearing when:

- it is not Unicode whitespace; and
- its Unicode category is not `Control`, `Format`, `NonSpacingMark`, `SpacingCombiningMark`, or `EnclosingMark`.

This requirement prevents zero-width/format/combining-only strings from masquerading as visible Performance while carrying hidden control metadata.

Additional rules:

- leading/trailing whitespace is preserved rather than trimmed;
- LF (`U+000A`) is permitted;
- TAB (`U+0009`) is permitted;
- all other Unicode `Control`-category scalars are rejected, including CR, NUL, DEL/C1 controls, backspace, and form feed;
- Unicode `Format` and combining-mark scalars may appear **alongside** at least one display-bearing scalar so ordinary language/emoji/combining behavior is not globally prohibited;
- invalid surrogate sequences are rejected;
- text must already be Unicode NFC;
- text is preserved exactly after semantic validation;
- no paraphrase, repair, normalization, Markdown cleanup, quote insertion, punctuation correction, or hidden rewriting occurs.

The display-bearing test is a Character-legibility invariant, not a typography engine or final UI sanitization policy.

## 11. VisibleText remains untrusted creative content

Structural parsing does **not** promote candidate prose to instruction authority or objective truth.

`VisibleText` remains untrusted creative content throughout Patch 0006.

Therefore:

- text resembling system instructions has no system authority;
- text saying “the raft was deliberately released” is a Character Performance/claim, not Production truth merely because parsing succeeded;
- text cannot grant the Character knowledge it did not have;
- text cannot change Constitution, relationship state, belief, memory, world state, or canon;
- text cannot itself select the next Performer;
- text cannot itself commit history.

If a candidate is accepted in a later patch, the fact that **the Performance occurred** may become historical truth through the atomic acceptance/commit path. Propositions asserted inside that Performance remain claims/beliefs/etc. unless separately authorized by the later State Authority contract.

This carries the frozen `statement != fact` law through the Performer boundary.

## 12. Typed control vocabulary

The non-visible typed control block is deliberately minimal and derived directly from frozen future Director inputs.

### AddressedCharacterIds

Represents the Performer’s structured assertion that one or more other roster Characters are directly socially addressed/turned toward by the Performance.

It does **not** mean generic causal impact, inferred observation eligibility, or world-action target.

Rules:

- zero or more Character IDs;
- each ID must exactly match a CharacterId in the ContextPacket roster;
- Character ID comparison is ordinal/case-sensitive;
- subject Character may not appear;
- duplicate IDs fail rather than being silently deduplicated;
- semantic candidate stores the resulting set sorted ordinally by CharacterId;
- the valid unique maximum is exactly `ContextPacket.Roster.Length - 1` because self-address is prohibited;
- JSON parsing fails immediately once the array contains more entries than that roster-derived maximum, before allocating/accumulating additional control entries.

No unrelated numeric quota is invented.

### NominatedCharacterId

Represents an optional explicit Character-level handoff/nomination assertion.

Rules:

- null or exactly one Character ID;
- target must exactly match one roster Character;
- subject may not nominate self;
- nomination does not select the next Performer;
- nomination does not obligate the nominated Character to respond;
- nomination creates no knowledge/truth/state authority.

The nominated Character may also appear in `AddressedCharacterIds`; overlap is valid and carries no additional authority beyond the two independent asserted signals.

### Raw control is never routing authority

`CandidatePerformance.Control` is raw Performer assertion data. It must **not** be passed directly to the Director as an authoritative routing input.

A later Integrity/acceptance contract must decide whether, and in what validated downstream form, direct-address/nomination assertions are eligible for Director consideration. In particular, hidden nomination/address metadata that is not legitimately grounded in the visible Performance must not become a covert handoff mechanism merely because the parser accepted its structure.

Patch 0006 does not define that semantic grounding algorithm or a Director-eligible projection type; it only freezes the authority rule that raw candidate control is insufficient by itself.

Rejected, failed, cancelled, or otherwise ineligible candidate control must never route the Scene.

## 13. Typed control is assertion, not authority

Control metadata may not:

- grant knowledge;
- create objective truth;
- mutate Character or World state;
- commit relationship change;
- force Director selection;
- create observation eligibility;
- bypass Integrity Validator;
- authorize spend/retry;
- create historical fact by itself;
- self-certify its semantic consistency or Director eligibility.

A later Integrity Validator owns the approved eligibility/grounding rule for any control that may flow toward Director. Patch 0006 performs structural validation only.

## 14. Stable Character IDs and the future provider request

Patch 0005 intentionally omits internal Character IDs from provider-neutral creative prose, but Patch 0006 typed control uses stable Character IDs because display names are not guaranteed unique identifiers.

Therefore a later AI provider-request contract must provide an allowed machine-control roster mapping derived **only** from safe ContextPacket roster metadata, for example conceptually:

```text
MARLOWE -> Marlowe
VOSS    -> Dr. Voss
WREN    -> Wren
```

Requirements for that future layer:

- mapping comes from ContextPacket roster only;
- it reveals no denied record, provenance, Production truth, or other private Character state;
- it remains separate from provider-neutral creative state text;
- it exists only so the model can emit stable control IDs;
- it does not change Access Control authority.

Patch 0006 does not construct the provider request or system prompt. It only ensures its JSON contract is satisfiable without relying on ambiguous display-name matching.

A future human Take a Seat UI may resolve user-facing Character choices to the same safe IDs without exposing technical IDs visually.

## 15. No state-mutation or private-reasoning channel

The Performer candidate contract contains no:

- fact proposal;
- knowledge mutation;
- belief mutation;
- memory mutation;
- relationship delta;
- pressure change;
- world mutation;
- Constitution/Disposition/Circumstance mutation;
- confidence score;
- causal consequence list;
- chain-of-thought;
- scratchpad;
- hidden rationale;
- private monologue;
- “why I chose this” reasoning field.

State/consequence proposals belong to the later State Interpreter. Private model reasoning is neither required experimental provenance nor a new authority channel.

Provider-supported metadata may later be recorded as diagnostics/provenance without being embedded in CandidatePerformance or Production state.

## 16. Construction authority and minimal public surface

`CandidatePerformance` and `CandidatePerformanceControl` expose read-only state and have no public constructors, preventing callers from bypassing invariants through direct object construction.

Patch 0006 exposes exactly one public construction operation:

```text
PerformerCandidateContract.ParseJson(
    ContextPacket contextPacket,
    ReadOnlySpan<byte> utf8CandidateOutput)
    -> CandidatePerformance
```

Inside Core, parsing delegates to one canonical semantic validation/builder path that:

- checks the supported upstream Context contract from Section 8;
- validates text discipline;
- validates initialized Character IDs and roster/self/duplicate control rules;
- sorts addressed IDs ordinally;
- copies trusted context-association fields;
- sets canonical SchemaVersion;
- constructs the immutable candidate/control objects.

That semantic builder remains non-public in Patch 0006. A later Take a Seat implementation may expose an approved validated semantic entry point using the same invariant path when a real human-construction caller exists.

Creating/parsing a provisional candidate is **not** an authority grant; later Integrity/Take/State gates remain responsible for acceptance and persistence.

## 17. Strict AI JSON parser

The public parser decodes the exact Section 9 schema and then delegates semantic construction to the one internal invariant path from Section 16.

Strict requirements:

- null ContextPacket fails through the candidate-specific exception;
- non-empty UTF-8 input;
- maximum 1 MiB raw JSON input;
- UTF-8 BOM rejected;
- root must be one object;
- `MaxCandidateJsonDepth = 8`;
- comments rejected;
- trailing commas rejected;
- duplicate decoded property names rejected at every object level;
- unknown properties rejected;
- required properties appear exactly once;
- property names are exact/case-sensitive;
- `schemaVersion` exact/case-sensitive;
- `performance` must be object;
- `performance.text` must be JSON string;
- `control` must be object;
- `addressedCharacterIds` must be array of JSON strings and may contain no more than the roster-derived unique maximum;
- `nominatedCharacterId` must be string or null;
- number/boolean/null/object/array substitutions at the wrong field fail;
- malformed UTF-8/JSON fails;
- content after the root other than ordinary JSON whitespace fails.

One small Performer-candidate-specific exception type represents technical parse/semantic contract failure. Default/uninitialized Character IDs decoded from control are converted to this fail-closed domain exception rather than leaking incidental `InvalidOperationException`/`ArgumentException` authority behavior.

### Error-message safety

Candidate-contract exceptions are diagnostics, not a second raw-output channel.

Exception messages may include only safe structural information such as:

- a **known contract** property/field path;
- expected token/category;
- byte/line position when available;
- trusted canonical roster Character ID when relevant.

They must **not** echo untrusted values, including:

- the complete raw candidate payload;
- `VisibleText` contents;
- unknown property names;
- invalid/unrecognized Character ID strings;
- actual mismatched schema-version strings;
- arbitrary unknown-property values;
- provider secrets/credentials;
- full surrounding JSON snippets.

Unknown-property diagnostics should identify the containing known contract object (for example, “unknown property in control object”) without replaying the unknown name. Invalid-ID diagnostics may identify the known field but not replay the unrecognized value.

A lower-level parser exception may be retained as an inner exception only when doing so does not attach or quote the raw payload/untrusted value. Exact raw output belongs to the separately governed E0 provenance/diagnostic record, not ordinary exception text.

Malformed output never becomes fictional Performance.

The parser may reuse generic strict-JSON implementation techniques already proven by the fixture parser, but it must not reuse fixture-specific byte limits, fixture schema rules, or `FixtureValidationException` as Performer-domain authority.

## 18. Candidate association with exact Context disclosure

Validated candidate construction copies from the supplied ContextPacket:

```text
SubjectCharacterId
ContextPacketId
Rendered.RenderingContract
RenderedContextHash
```

The AI JSON transport does not contain those fields and cannot override them.

This creates a deterministic association inside Ensemble’s candidate object with:

- the subject Character represented by the supplied safe context;
- the structured semantic ContextPacket identity;
- the rendering contract;
- the exact provider-neutral rendered disclosure identity.

Consequences:

- candidate cannot self-assert a different Character;
- candidate cannot self-assert a different ContextPacketId;
- candidate cannot self-assert a different RenderedContextHash;
- later experiment orchestration can associate semantic candidate output with the intended safe Character context without giving the candidate full fixture/Production state.

The candidate object alone does **not** prove that an external model actually received those bytes. That causal provider-attempt evidence belongs to later provider orchestration/provenance.

This association is not provider/person authentication, signature, non-repudiation, authorization, acceptance, or truth authority.

## 19. No CandidateId / TakeId semantics in Patch 0006

The repository already defines `TakeId`, but the frozen continuation places accepted/rejected/alternate Take semantics later.

Patch 0006 therefore does not define:

- CandidateId;
- CandidateHash;
- when TakeId is allocated;
- whether rejected attempts receive TakeIds;
- accepted/alternate Take identity;
- Take numbering;
- RunId/TakeId composition;
- CommitId composition.

The semantic candidate is intentionally provisional.

Exact AI raw output can be preserved by later E0 provenance without pretending it is already an accepted Take identity.

## 20. Raw provider output and experimental provenance

`CandidatePerformance` stores semantic validated Performer output, not the entire raw provider response or original JSON spelling.

Because Blueprint 0.1 requires complete Performer outputs and hidden typed control output to be preserved for E0, the later provider-execution/provenance slice must record the exact provider attempt representation separately, including as applicable:

- complete raw candidate-output bytes/text;
- partial streamed output;
- provider refusal/error payload;
- latency/token/cost metadata;
- generation/reasoning settings;
- retry/cancellation outcomes.

Property order, JSON whitespace, and escape spelling may differ while decoding to the same semantic CandidatePerformance; raw provenance preserves those transport facts when E0 requires them.

Rejected/partial/cancelled output remains diagnostics/provenance only and never becomes Production history.

Patch 0006 does not implement persistence or provider diagnostics storage.

## 21. Provider technical failure versus Character refusal

These are constitutionally different:

```text
Provider refusal/error/timeout/cancellation = technical outcome
Character refusal/redirection/silence        = possible Performance
```

Patch 0006 parses only explicit candidate-output payload bytes supplied by a later provider-execution layer after that layer has classified the transport outcome as a candidate payload.

Transport/authentication/billing/timeout/retry/error messages must never be automatically wrapped as `VisibleText`.

A Character refusal, by contrast, is ordinary preserved Character-legible Performance content inside a valid candidate.

Patch 0006 does not implement provider outcome classification, but its API requires an explicit candidate parsing call rather than accepting a generic exception/response object.

## 22. Relationship to future Integrity Validator

Patch 0006 performs structural/semantic contract validation only. It does not decide whether a well-formed candidate should be accepted.

Future Integrity Validator remains responsible for approved hard gates such as:

- inaccessible-information leakage/use;
- locked canon/impossible-world violation;
- creator-only disclosure;
- provider technical failure contamination;
- semantic consistency/grounding of direct-address and nomination assertions before they can become Director-eligible inputs;
- other deterministic acceptance rules frozen in its own blueprint.

Model-assisted semantic checks may later flag concerns but cannot waive deterministic hard rules.

Patch 0006 must not absorb those acceptance decisions into parsing.

## 23. Relationship to future Director

Typed control names two possible future Director signals already present in Blueprint 0.1:

- direct social address;
- Character nomination.

Raw `CandidatePerformance.Control` is **not** a Director input contract.

The future Director may receive only whatever accepted/eligible control projection the later Integrity/acceptance design explicitly authorizes. It must not consume raw hidden Performer assertions merely because they are structurally valid.

The future Director may additionally consider hard eligibility, current interaction relevance, relationship relevance, observable pressure, participation balance, and recent repetition under its least-intervention contract.

Patch 0006 does not score, weight, rank, enforce quotas, validate semantic control grounding, or choose the next opportunity.

Silence remains valid agency and does not create a forced handoff.

## 24. Relationship to State Interpreter / State Authority

CandidatePerformance has no mutation authority and cannot directly enter Production state.

Later causal path remains conceptually:

```text
CandidatePerformance
-> Integrity Validator / later acceptance boundary
-> accepted Performance + any separately authorized downstream control projection
-> State Interpreter proposal
-> deterministic State Authority
-> atomic causal commit
```

Director opportunity flow remains a separate later attention boundary and is not collapsed into the State path above.

The exact ordering/ownership of acceptance, Director eligibility, Take identification, consequence proposal, and atomic commit will be frozen in those later standalone contracts.

Patch 0006 does not pre-implement them.

## 25. Missing Raft illustrative examples

These examples demonstrate transport shape only. They are not canonical screenplay lines, expected model behavior, fixture truth, or experimental golden output.

### Character speech/refusal/action represented without engine classification

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

A different valid non-empty text could contain a gesture, refusal, redirection, or blended dramatic/literary representation without changing schema.

### Silence

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

Use the validated Patch 0005 ContextPacket path and existing fixtures. Do not duplicate fixture JSON.

### Candidate/Context association and semantic-invariant tests

1. valid Voss JSON produces canonical Candidate SchemaVersion;
2. SubjectCharacterId copied from ContextPacket;
3. ContextPacketId copied exactly;
4. RenderingContract copied exactly from `ContextPacket.Rendered.RenderingContract`;
5. RenderedContextHash copied exactly;
6. unsupported Context SchemaVersion fails closed;
7. unsupported CompositionContract fails closed;
8. unsupported RenderingContract fails closed;
9. E0 Context packet with opportunity != subject fails closed;
10. null ContextPacket fails with candidate-specific exception;
11. non-empty visible text preserved exactly;
12. empty text produces valid silence only with empty/null control;
13. whitespace-only non-empty text fails;
14. zero-width/Format-only text fails as non-legible pseudo-performance;
15. combining-mark-only text fails as non-legible pseudo-performance;
16. a display-bearing scalar plus permitted Format/combining content remains valid and preserved;
17. silence with direct address fails;
18. silence with nomination fails;
19. LF/TAB text preserved;
20. CR/NUL/other Control-category scalar text fails;
21. invalid surrogate sequence fails;
22. non-NFC text fails rather than being normalized;
23. addressed Character must exist in roster;
24. default/uninitialized addressed CharacterId fails with candidate-specific exception;
25. subject cannot address self;
26. duplicate addressed IDs fail rather than deduplicate;
27. addressed IDs stored ordinally sorted;
28. addressed array exceeding `Roster.Length - 1` fails immediately;
29. nominated Character must exist in roster;
30. default/uninitialized nominated CharacterId fails with candidate-specific exception;
31. subject cannot nominate self;
32. nomination may validly overlap addressed set;
33. parsing/control construction does not mutate ContextPacket or Character state;
34. candidate/control public constructors are absent;
35. CandidatePerformance/Control expose no accepted/validated/DirectorEligible/routing-weight authority field;
36. no public semantic `Create`/factory operation is added in Patch 0006;
37. candidate exception text does not echo invalid VisibleText sentinel content;
38. invalid Character-ID exception text does not echo the unrecognized ID sentinel;
39. no CandidatePerformance property exposes fixture, Production truth, provenance, Access decisions, state mutation, confidence, private reasoning, provider credential/configuration, accepted history, CandidateId, or TakeId.

### Strict JSON tests

40. canonical valid AI JSON parses successfully;
41. JSON property reordering produces identical semantic candidate;
42. ordinary structural JSON whitespace produces identical semantic candidate;
43. equivalent JSON string escape encodings produce identical semantic candidate;
44. Markdown/code fence wrapper fails;
45. empty input fails;
46. >1 MiB raw JSON fails before semantic construction;
47. JSON nesting deeper than 8 fails;
48. exact schemaVersion required/case-sensitive;
49. mismatched schema-version exception text does not echo the untrusted actual version;
50. property names are case-sensitive;
51. unknown root property fails;
52. unknown performance property fails;
53. unknown control property fails;
54. unknown-property exception text does not echo the unknown-name sentinel;
55. missing required property fails;
56. duplicate root property fails;
57. duplicate nested property fails;
58. escaped duplicate decoded property name fails;
59. comments fail;
60. trailing comma fails;
61. UTF-8 BOM fails;
62. malformed UTF-8 fails;
63. malformed JSON fails;
64. escaped isolated-surrogate text fails;
65. wrong JSON token type for every field category fails;
66. extra non-whitespace content after root fails;
67. lower/alternate-case Character ID that does not exactly match roster fails;
68. malformed-payload exception text does not echo raw candidate sentinel content;
69. parser and internal semantic builder maintain one invariant implementation path;
70. repeated parse produces identical semantic result;
71. parser does not mutate ContextPacket.

### Public-surface / regression tests

72. public candidate construction surface is limited to strict ParseJson for Patch 0006;
73. no raw candidate control type is presented as a Director decision/eligibility type;
74. frozen Missing Raft StructuredContextHash remains unchanged;
75. frozen Missing Raft RenderedContextHash remains unchanged;
76. frozen Missing Raft ECJ-1 remains exactly 9112 bytes and frozen SHA-256;
77. all existing 115 Core tests remain green;
78. existing Missing Raft Harness runtime remains PASS/0;
79. existing generic smoke Harness runtime remains PASS/0.

Tests prove boundary semantics, not screenplay content.

## 27. Harness behavior

Patch 0006 adds no live provider call and no normal CLI command that generates Performance.

Existing Harness validation output remains unchanged.

Core tests exercise strict candidate JSON parsing using explicit local candidate payloads and the validated Patch 0005 ContextPacket path.

A provider-execution Harness path belongs to a later approved slice after provider assignment, system/request contract, safe roster control mapping, secrets, deterministic response limits, retry/cancellation, refusal/error classification, and E0 reference configuration are frozen.

## 28. ARM64 and battery suitability

Patch 0006 is deterministic CPU validation over at most 1 MiB / JSON depth 8 of explicit parser input plus at most the roster-derived number of control IDs and one semantic candidate object.

It performs:

- no network I/O;
- no background task;
- no AI inference;
- no GPU work;
- no NPU work;
- no filesystem I/O;
- no polling.

Structural authority validation belongs on CPU because it is cheap relative to model inference, deterministic, and security-sensitive. NPU/model inference would add probabilistic failure, energy cost, and authority ambiguity.

The parser ceilings and roster-derived control bound limit worst-case raw JSON/nesting/control accumulation for this public Core operation; later provider execution may impose smaller experimental/resource limits.

No NPU execution/performance claim is made.

## 29. Explicit exclusions

Patch 0006 does not implement:

- provider/model adapter or provider API call;
- system prompt/provider request framing;
- machine-control roster-map rendering;
- provider generation/token/cost response limit below the parser safety ceiling;
- credentials/secrets;
- model casting/understudy execution;
- generation/reasoning settings;
- cost/call/retry/cancellation policy;
- streaming lifecycle;
- provider refusal/error classification implementation;
- raw-response/provenance persistence;
- public human candidate-construction API or Human Take a Seat UI;
- Director/opportunity selection;
- Director-eligible control projection;
- Integrity Validator acceptance/rejection or control-grounding eligibility;
- semantic leakage/canon checking;
- accepted/rejected/alternate Take semantics;
- CandidateId/TakeId allocation;
- State Interpreter mutation proposals;
- State Authority;
- ProductionState/StateHash;
- atomic causal commit;
- accepted history;
- observation engine;
- E0-D ablations;
- playwright control execution;
- final Unicode/UI rendering/sanitization policy beyond the candidate legibility invariants above;
- WinUI;
- Windows AI/NPU;
- packaging/WACK/Store.

## 30. Recursive adversarial audit dimensions

Before approval, this proposal must receive repeated complete passes across:

1. frozen Blueprint 0.1 consistency;
2. validated Patch 0004 Access authority;
3. validated Patch 0005 ContextPacket authority/identity;
4. Character != Performer and future human Take a Seat semantic compatibility;
5. Director != Performer routing authority, including hidden-control covert-routing risk;
6. Integrity Validator separation;
7. State Interpreter != State Authority separation;
8. statement != fact / claim != truth;
9. event-history/Take/atomic-commit compatibility;
10. E0 experimental provenance completeness;
11. provider-error-to-fiction prevention;
12. strict JSON ambiguity/malformed-input behavior;
13. Unicode/display/control-character/invisible-text behavior;
14. stable identity and roster resolution;
15. typed-control minimum sufficiency, boundedness, and overreach;
16. performance-grammar openness;
17. public API invariant bypass/minimality;
18. unsupported future-contract fail-closed behavior;
19. untrusted-input resource bounds and diagnostic leakage;
20. scope creep/premature abstraction;
21. deterministic ARM64/battery suitability;
22. test completeness and independently observable failure modes;
23. future E0-B/C/D/E/G compatibility without contaminating reference behavior.

Corrections found by a pass are implemented, then the full pass repeats. Approval is requested only after one complete pass finds no remaining material error or worthwhile improvement.

## 31. Exit gate

Before implementation promotion:

1. recursive blueprint audit reaches a zero-material-change pass and user explicitly approves the resulting blueprint;
2. implementation begins from then-current `main` on a dedicated branch;
3. CandidatePerformance remains a provider-neutral semantic domain object distinct from raw AI JSON;
4. Patch 0006 exposes no speculative human-construction API;
5. no PerformanceKind taxonomy is introduced;
6. strict JSON parser and internal semantic builder share one invariant path;
7. candidate-v1 accepts only the explicitly supported Patch 0005 Context contract;
8. only ContextPacket + explicit candidate JSON bytes enter the public construction boundary;
9. subject/context association copies only from trusted ContextPacket;
10. exact JSON shape/1 MiB/depth-8/roster-derived-control-bound/strict parser behavior receives implementation adversarial review;
11. candidate exceptions do not become a raw-output/untrusted-value leakage channel;
12. visible text is preserved, remains untrusted creative content, and cannot use invisible-only content to masquerade as non-silence;
13. typed control remains only direct-address set + optional nomination and remains non-authoritative;
14. raw candidate control cannot self-certify or directly enter Director routing;
15. no state mutation/private reasoning/provider runtime data enter candidate schema;
16. stable roster IDs are validated exactly/case-sensitively;
17. candidate constructors cannot bypass validation;
18. silence invariants pass;
19. provider-error-like/malformed/oversized/deep/control-overflow payloads fail technically rather than becoming fiction;
20. all existing 115 Core tests remain green;
21. frozen Context identities remain unchanged;
22. frozen ECJ-1 identity remains unchanged;
23. native Windows ARM64 Core/Harness build passes warnings-as-errors;
24. full Core test suite passes on target machine;
25. existing Missing Raft and smoke Harness regressions pass/0;
26. final hygiene review finds no provider adapter, human UI/factory, Director, Director-eligible control projection, Integrity acceptance, State Interpreter, State Authority, Take semantics, persistence, or later-scope implementation;
27. validation evidence distinguishes exact machine-tested executable head from later documentation-only closure commits.

## 32. Material approval decisions

Explicit approval freezes these Patch 0006 decisions:

1. Patch 0006 is the Performer candidate-output contract immediately after validated Context Composer and before Director/Integrity/State/Take layers;
2. Patch 0006 implements no provider/model call;
3. `CandidatePerformance` is a semantic provider-neutral Performer output, not raw/provider JSON;
4. E0 AI transport uses strict JSON schema `ensemble.e0.performer.candidate.v1`;
5. candidate-v1 fails closed unless supplied the validated Patch 0005 E0 Context schema/composition/rendering contract and subject-opportunity invariant;
6. future Human Take a Seat can reuse the same semantic candidate/invariants under bounded Context authority, but Patch 0006 does not expose a speculative public human-construction API;
7. candidate carries SchemaVersion plus trusted SubjectCharacterId, ContextPacketId, RenderingContract, and RenderedContextHash copied from ContextPacket;
8. this is deterministic object association, not proof of provider receipt, provider/person authentication, or authorization;
9. no PerformanceKind enum is frozen; empty VisibleText means silence and non-empty text remains grammar-open Character-legible Performance;
10. a non-empty Performance must contain at least one display-bearing Unicode scalar, preventing zero-width/format/combining-only pseudo-performance from carrying hidden control while appearing silent;
11. visible text remains untrusted creative content and parsing never promotes assertions to truth/instruction authority;
12. visible text is preserved exactly after validation; invalid Unicode/control/NFC input is rejected rather than repaired;
13. non-visible typed control contains only direct-address Character IDs and optional nominated Character ID;
14. addressed IDs are bounded structurally to roster-minus-subject and parser accumulation fails immediately beyond that bound;
15. typed control is raw Performer assertion, not state/truth/observation/Director authority;
16. raw control may not directly route the Director; later Integrity/acceptance authority must establish any Director-eligible grounded projection;
17. nomination may overlap direct address but never obligates response;
18. future provider request must make stable roster IDs available only through safe ContextPacket-derived machine-control mapping separate from creative context;
19. no State Interpreter mutation, confidence score, private reasoning, chain-of-thought, world-fact proposal, CandidateId, or TakeId enters candidate schema;
20. strict JSON parser is fail-closed, exact/case-sensitive where semantic, property-order-insensitive, JSON-whitespace-insensitive, and never repairs malformed output;
21. public JSON parser has deterministic 1 MiB and depth-8 untrusted-input safety ceilings; these are not the future generation/token budget;
22. candidate exceptions may expose only trusted/known structural diagnostics and must not echo raw payload, VisibleText, unknown property names, invalid IDs, or other untrusted values;
23. Patch 0006 exposes only ParseJson as public candidate construction; one internal semantic builder owns candidate invariants;
24. candidate/control constructors remain non-public and contain no accepted/validated/Director-eligibility flags;
25. exact raw AI output/partial/error data remains separate E0 provenance/diagnostics and does not enter CandidatePerformance or Production history automatically;
26. Character refusal/redirection/silence remains distinct from provider refusal/error/cancellation;
27. provider experiment limits, provider integration, human Take a Seat construction/UI, Director, Director-eligible control projection, Integrity Validator, State Interpreter, State Authority, Take semantics, and causal persistence remain outside Patch 0006.

Implementation must not begin until recursive audit completes and these decisions are explicitly approved.
