# H1 Patch 0006 — Performer Candidate Output Contract

Status: blueprint proposal 0.2 — APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0005
Branch: `h1-patch-0006-performer-candidate-blueprint`

## 1. Purpose

Implement the next E0-A boundary after the validated Context Composer:

`ContextPacket -> later provider/Performer execution -> strict candidate-output contract -> CandidatePerformance -> later Integrity Validator`

Patch 0006 answers one narrow question:

> Can Ensemble represent one provisional Character performance as a strict, schema-bounded, provider-neutral candidate object that is deterministically attributed to the exact ContextPacket disclosure that produced it, while keeping model-supplied control data non-authoritative and preventing provider errors, malformed output, hidden reasoning, or mutation proposals from entering fiction?

Patch 0006 defines and implements the candidate-output contract only. It does not call any provider/model.

The ContextPacket hashes provide content identity for attribution. They are not a signature, MAC, provider authentication mechanism, or proof that a particular model produced the candidate.

## 2. Authority basis

Frozen Blueprint 0.1 requires the E0-A preparation sequence to define, after Access Control and Context Composer:

1. Performer candidate-output contract, including visible Performance and typed control data;
2. Director opportunity-selection contract;
3. Integrity Validator acceptance/rejection rules;
4. State Interpreter candidate-mutation schema;
5. deterministic State Authority commit rules;
6. Take semantics;
7. atomic causal commit.

The frozen behavioral architecture additionally states:

- Character != Performer;
- a Performer receives one bounded Character context and produces a candidate Performance plus any required typed control output;
- the Performer portrays agency but owns neither truth nor persistence;
- generated work becomes Production history only through later acceptance;
- provider failure/refusal/timeout/retry must never become fictional action;
- accepted Performance must be preserved rather than silently rewritten;
- Director manages opportunity, not outcomes;
- State Interpreter proposes consequences; State Authority alone decides what may commit;
- experimental provenance records complete Context packets, Performer outputs, hidden typed control output, errors/refusals/cancellations, and later acceptance results.

Validated Patch 0005 freezes the immediate upstream boundary:

- `ContextPacket` is Character-safe;
- `ContextPacketId = CTX:<StructuredContextHash>`;
- `RenderedContextHash` identifies the exact provider-neutral textual disclosure;
- the system/provider request contract remains outside Context Composer.

Patch 0006 must preserve all of those separations.

## 3. Why Performer comes before Director implementation

The frozen E0-A continuation order places the Performer candidate-output contract before the Director contract.

This ordering is also architecturally necessary:

- the opening Missing Raft fixture already supplies the first opportunity (`VOSS`), so the first candidate can be generated without implementing dynamic opportunity selection;
- the Director needs a stable accepted-performance/control vocabulary before its later inputs can be frozen;
- Integrity Validator needs a concrete candidate type before acceptance/rejection rules can be implemented;
- State Interpreter must interpret an accepted Performance, not an untyped provider string.

Therefore Patch 0006 does not implement Director logic merely to obtain a first speaker.

## 4. Architectural boundary

### Trusted input to candidate construction

Patch 0006 candidate construction accepts:

```text
ContextPacket
UTF-8 candidate-output bytes
```

The trusted `ContextPacket` supplies:

- SubjectCharacterId;
- ContextPacketId;
- RenderingContract;
- RenderedContextHash;
- Scene roster for structural target validation.

Those values are copied from the trusted packet. They are **not echoed by the model** and are not trusted from provider output.

### Output

```text
CandidatePerformance
- SchemaVersion
- SubjectCharacterId
- ContextPacketId
- RenderingContract
- RenderedContextHash
- Kind
- VisibleText
- Control
```

with:

```text
CandidatePerformanceControl
- AddressedCharacterIds
- NominatedCharacterId
```

`SchemaVersion` is the exact verified contract value from Section 5. The parser verifies the model-supplied token and stores the canonical contract value; callers do not provide it separately.

The candidate is provisional. It is not an accepted Take, not Production history, and not authority.

### Explicit non-inputs

Patch 0006 candidate parsing/construction does not accept:

- `ValidatedFixture`;
- Production truth/WorldState/chronology;
- Access decisions;
- fixture provenance;
- denied Character state;
- State Interpreter or mutation schema;
- State Authority;
- Director state;
- provider/model credentials;
- generation configuration;
- retry/cost policy;
- diagnostic logs;
- accepted history.

## 5. Output contract version

Patch 0006 freezes:

```text
PerformerCandidateContract = ensemble.e0.performer.candidate.v1
```

The provider/model-facing candidate JSON contains this version so malformed or stale output cannot be silently interpreted under a different schema.

The contract identifier is structural only. It grants no authority.

## 6. Exact model-supplied JSON shape

Candidate output bytes must decode as one strict JSON object of this semantic shape:

```json
{
  "schemaVersion": "ensemble.e0.performer.candidate.v1",
  "performance": {
    "kind": "speech|action|mixed|silence",
    "text": "..."
  },
  "control": {
    "addressedCharacterIds": ["..."],
    "nominatedCharacterId": null
  }
}
```

`nominatedCharacterId` is either a canonical Character ID string or JSON `null`.

Property order in provider output is not semantically significant. Candidate parsing must not depend on property order.

No additional root, `performance`, or `control` properties are permitted.

Patch 0006 freezes schema shape, not a generation-length optimization policy. Deterministic transport/response byte ceilings belong to the later provider-execution contract and must be enforced before unbounded provider data reaches Core parsing.

## 7. Performance kinds

Frozen E0 kinds:

```text
Speech
Action
Mixed
Silence
```

External JSON values are lowercase ASCII:

```text
speech
action
mixed
silence
```

Meaning:

- `Speech`: visible text represents spoken/verbal Character performance.
- `Action`: visible text represents non-verbal Character action.
- `Mixed`: visible text contains both speech and action.
- `Silence`: the Character performs silence/non-response and `text` is exactly the empty string.

Blueprint 0.1 also permits refusal and redirection. Patch 0006 does **not** create `Refusal` or `Redirection` authority categories. Those are semantic forms of Character agency expressible through Speech, Action, Mixed, or Silence plus optional control metadata.

This avoids turning dramatic interpretation into an unnecessary enum taxonomy.

## 8. Visible text contract

For `Speech`, `Action`, and `Mixed`:

- `text` must contain at least one non-whitespace Unicode scalar;
- leading/trailing whitespace is not silently trimmed;
- LF (`U+000A`) may appear;
- CR (`U+000D`) is forbidden;
- NUL is forbidden;
- invalid surrogate sequences are forbidden;
- text must be NFC;
- candidate `VisibleText` is preserved exactly after JSON decoding; no paraphrase, repair, normalization, rewriting, Markdown cleanup, quote insertion, or punctuation correction occurs.

For `Silence`:

- `text` must be exactly `""`;
- `AddressedCharacterIds` must be empty;
- `NominatedCharacterId` must be null.

A silent Character is represented structurally rather than by Ensemble inventing prose such as “Voss says nothing.”

## 9. Typed control data

The hidden typed control block is deliberately narrow.

### AddressedCharacterIds

Represents the Performer’s structured assertion that the visible Performance directly addresses or acts toward one or more other roster Characters.

Rules:

- zero or more Character IDs;
- every ID must resolve exactly once in the ContextPacket roster;
- subject Character may not appear;
- duplicate IDs are invalid;
- candidate object stores the set sorted ordinally by CharacterId;
- in E0 with three co-present Characters, structural maximum follows naturally from roster-minus-subject; no separate numeric product limit is invented.

### NominatedCharacterId

Represents an optional Character-level nomination/handoff signal such as “Wren?” or a turn toward another Character.

Rules:

- null or exactly one Character ID;
- target must resolve exactly once in roster;
- subject may not nominate self;
- nomination does **not** select the next Performer;
- nomination does **not** obligate the nominated Character to act;
- nomination is merely one future Director input after the candidate becomes accepted/eligible under later contracts.

### Authority of control data

Typed control is a **Performer assertion**, not trusted world evidence.

It may not:

- grant knowledge;
- create truth;
- mutate state;
- commit relationship change;
- force Director selection;
- bypass Integrity Validator;
- authorize spending/retry;
- create observation eligibility;
- create historical fact by itself.

A later Integrity Validator may compare control metadata with visible Performance and reject or flag semantic inconsistency. Patch 0006 performs structural validation only.

## 10. Why no state-mutation output exists here

The Performer contract contains no:

- fact proposal;
- belief mutation;
- memory mutation;
- relationship delta;
- pressure change;
- world mutation;
- Constitution/Disposition/Circumstance mutation;
- confidence score;
- causal consequence list.

Those belong to the later State Interpreter proposal boundary.

Allowing the Performer to emit mutation objects here would collapse portrayal and interpretation, violating the frozen rule that models may propose but deterministic authority decides and that State Interpreter is a separate layer.

## 11. Why no private reasoning field exists

The candidate contract contains no chain-of-thought, scratchpad, hidden rationale, private monologue, or “why I chose this” field.

Reasons:

- the E0 experiment evaluates Character-legible Performance, not private model reasoning;
- provider reasoning interfaces differ and must not be faked;
- private chain-of-thought is not required for experimental provenance;
- hidden reasoning must not become a new authority channel;
- provider adapters may record supported provider metadata later without placing private reasoning inside Production state.

The only hidden candidate metadata in Patch 0006 is the narrow typed control block above.

## 12. Strict JSON parsing

Patch 0006 uses one deterministic strict parser.

Requirements:

- UTF-8 input;
- UTF-8 BOM rejected;
- root must be one JSON object;
- duplicate property names rejected at every contract level;
- comments rejected;
- trailing commas rejected;
- unknown properties rejected;
- required properties must appear exactly once;
- `schemaVersion` must match exactly;
- kind values are case-sensitive exact lowercase contract tokens;
- addressed IDs must be JSON strings;
- nominated ID must be string or null;
- non-object/non-array substitutions fail closed;
- malformed JSON fails closed.

A malformed provider response is a technical contract failure. It never becomes fictional dialogue/action.

Patch 0006 may reuse the repository’s existing strict-JSON/preflight techniques when appropriate, but must not couple Performer output parsing to fixture-specific types or fixture schema rules.

## 13. Candidate attribution to exact context disclosure

Trusted candidate construction copies from the supplied `ContextPacket`:

```text
SubjectCharacterId
ContextPacketId
RenderingContract
RenderedContextHash
```

This creates deterministic content attribution from candidate Performance to:

- the structured semantic context identity;
- the exact provider-neutral rendered disclosure contract and content identity.

The model/provider output is not allowed to supply or override those fields.

Consequences:

- a candidate cannot claim it came from a different Character;
- a candidate cannot claim a different ContextPacketId;
- a candidate cannot claim a different RenderedContextHash;
- later experimental provenance can associate a Performance with the exact Character disclosure without reintroducing full fixture/Production state into the candidate.

This is content binding/attribution inside Ensemble’s deterministic object graph. It is **not** provider authentication, signing, non-repudiation, authorization, acceptance, or truth authority.

## 14. No TakeId semantics in Patch 0006

The repository already defines a `TakeId` strong ID, but the frozen continuation places accepted/rejected/alternate Take semantics **after** State Authority contract work.

Patch 0006 therefore does not prematurely define:

- when a TakeId is allocated;
- whether rejected attempts receive TakeIds;
- accepted/alternate Take identity;
- Take numbering;
- RunId/TakeId composition;
- commit identity.

`CandidatePerformance` is deliberately a provisional object without Take acceptance semantics.

A later approved Take/acceptance patch may wrap or identify a candidate without changing the candidate’s content contract.

## 15. Candidate immutability and construction authority

Public candidate model types are read-only.

Their constructors are Core-internal so external assemblies cannot fabricate “validated candidate” objects directly.

Only the strict candidate parser/factory may construct them from:

- a trusted validated `ContextPacket`;
- successfully parsed/validated candidate-output bytes.

This follows the same authority-hardening principle used by Patch 0004 and Patch 0005 safe outputs.

## 16. Candidate parser result

Preferred public surface:

```text
PerformerCandidateContract.Parse(
    ContextPacket contextPacket,
    ReadOnlySpan<byte> utf8CandidateOutput)
    -> CandidatePerformance
```

A small contract-specific exception type is used for failures.

No provider abstraction, retry loop, logging side effect, file I/O, network I/O, clock, random source, or model call occurs inside parsing.

## 17. Determinism

For identical `ContextPacket + candidate-output UTF-8 bytes`:

- parsing result is semantically identical;
- `SchemaVersion` is identical;
- AddressedCharacterIds ordering is identical;
- all copied context-attribution fields are identical;
- no culture, filesystem, clock, randomness, network, provider state, process-global mutable state, or machine architecture affects the result.

The parser never mutates the ContextPacket.

## 18. Candidate text versus raw provider response

`CandidatePerformance.VisibleText` contains the decoded contract text only.

Patch 0006 does not store the entire raw provider response inside the candidate.

Later diagnostics/provenance may separately preserve:

- raw provider response;
- streamed partial output;
- refusal/error payload;
- latency/token metadata;
- generation settings.

Those diagnostic artifacts do not become Character state or accepted Production history merely because they exist.

This preserves Blueprint 0.1’s distinction between creative history and diagnostics.

## 19. Provider refusal/error boundary

Patch 0006 parses only a provider response that the later provider execution layer classifies as a candidate-output payload.

The following must never be passed into fiction as candidate text:

- transport error message;
- HTTP/service error;
- provider refusal object;
- timeout message;
- retry notice;
- cancellation exception;
- authentication/billing error;
- unsupported-model message.

The later provider adapter owns classification of those technical outcomes before calling the candidate parser.

Patch 0006 does not implement that adapter, but its API must make accidental error-to-fiction conversion difficult: it accepts only explicit candidate bytes plus trusted ContextPacket.

## 20. Relationship to future Integrity Validator

Patch 0006 performs structural contract validation only.

It does not decide whether a well-formed candidate should be accepted.

The next Integrity Validator contract is expected to own hard candidate acceptance checks such as:

- inaccessible-information leakage/use;
- locked canon or impossible-world violation;
- creator-only disclosure;
- semantic mismatch between visible Performance and hidden control metadata where required;
- required output-contract integrity beyond parser shape;
- provider technical failure contamination.

Model-assisted semantic checks may later flag concerns but cannot waive deterministic hard gates.

Patch 0006 must not pre-implement those acceptance decisions inside the parser.

## 21. Relationship to future Director

The candidate’s typed control metadata provides two possible future Director inputs:

- direct address/acted-toward Characters;
- optional Character nomination.

Neither is a Director decision.

The future Director may also consider:

- hard eligibility;
- current interaction relevance;
- relationships;
- observable pressure;
- participation balance;
- recent repetition.

Patch 0006 does not score, weight, rank, or choose the next opportunity.

## 22. Relationship to State Interpreter / State Authority

Only a later accepted Performance may be interpreted for consequences.

Patch 0006 candidate objects have no mutation authority and cannot directly enter Production state.

Required downstream shape remains:

```text
CandidatePerformance
-> Integrity Validator
-> later accepted Performance/Take boundary
-> State Interpreter proposals
-> deterministic State Authority
-> atomic causal commit
```

Exact acceptance/Take ordering will be frozen in later approved patches; Patch 0006 must not collapse those layers.

## 23. E0 Missing Raft reference examples

Illustrative structurally valid candidate for Voss:

```json
{
  "schemaVersion": "ensemble.e0.performer.candidate.v1",
  "performance": {
    "kind": "speech",
    "text": "Before we decide what happened, what did each of us actually observe?"
  },
  "control": {
    "addressedCharacterIds": ["MARLOWE", "WREN"],
    "nominatedCharacterId": null
  }
}
```

This example is **not** canonical story output and is not a required E0 line. It demonstrates contract shape only.

Illustrative silence:

```json
{
  "schemaVersion": "ensemble.e0.performer.candidate.v1",
  "performance": {
    "kind": "silence",
    "text": ""
  },
  "control": {
    "addressedCharacterIds": [],
    "nominatedCharacterId": null
  }
}
```

No example candidate may be promoted into fixture truth, expected model behavior, or screenplay canon.

## 24. Required tests

Implementation tests must use the validated Patch 0005 ContextPacket path and existing fixtures; do not duplicate fixture JSON.

Required coverage:

1. valid Speech candidate parses and binds to Voss ContextPacket metadata;
2. valid Action candidate parses;
3. valid Mixed candidate parses;
4. valid Silence candidate requires empty text and empty/null control;
5. non-silence empty/whitespace-only text fails;
6. Silence with text fails;
7. Silence with address or nomination fails;
8. exact schema version required and exposed on parsed candidate;
9. unknown root property fails;
10. unknown performance property fails;
11. unknown control property fails;
12. missing required property fails;
13. duplicate property names fail;
14. comments fail;
15. trailing commas fail;
16. UTF-8 BOM fails;
17. malformed UTF-8/JSON fails;
18. CR/NUL/non-NFC candidate text fails without rewriting;
19. multiline LF text is preserved exactly;
20. addressed Character must be in roster;
21. subject cannot address self;
22. duplicate addressed IDs fail;
23. addressed ID storage is ordinally sorted;
24. nominated Character must be in roster;
25. subject cannot nominate self;
26. nomination remains metadata and does not alter ContextPacket/Character state;
27. candidate SubjectCharacterId is copied from ContextPacket, not provider payload;
28. ContextPacketId copied exactly;
29. RenderingContract copied exactly;
30. RenderedContextHash copied exactly;
31. no candidate property exposes Production fixture, provenance, Access decisions, world truth, mutations, confidence, private reasoning, provider credentials, retry policy, or accepted history;
32. candidate/control constructors have no public constructors;
33. repeated parse produces identical semantic result;
34. source JSON property reordering does not change semantic result;
35. parser does not mutate ContextPacket;
36. existing frozen Missing Raft Context identities remain unchanged;
37. existing frozen Missing Raft ECJ-1 identity remains unchanged;
38. all existing 115 Core tests remain green;
39. existing Missing Raft Harness runtime remains PASS/0;
40. existing generic smoke Harness runtime remains PASS/0.

Tests must prove strict boundary behavior rather than screenplay semantics.

## 25. Harness behavior

Patch 0006 does not add a live provider call or normal CLI command that generates a Performance.

Existing Harness validation output remains unchanged.

Core tests exercise candidate contract parsing using explicit candidate JSON bytes.

A provider-execution Harness mode belongs to a later explicitly approved slice once provider configuration, secrets, retry/cancellation, reference-model assignment, and system-contract authority are frozen.

## 26. ARM64 and battery suitability

Patch 0006 is tiny deterministic CPU work over one already transport-bounded JSON candidate.

It performs:

- no network I/O;
- no background task;
- no AI inference;
- no GPU work;
- no NPU work;
- no polling;
- no filesystem I/O.

This authority parsing belongs on CPU because it is deterministic, cheap, and security-sensitive. Offloading structural authority checks to an NPU/model would add latency, power use, and probabilistic failure while violating the frozen rule that deterministic authority remains deterministic.

No NPU execution/performance claim is made.

## 27. Explicit exclusions

Patch 0006 does not implement:

- provider/model adapter;
- provider API call;
- Performer system prompt/request framing;
- provider-response byte ceiling;
- credentials/secrets;
- model casting;
- generation/reasoning settings;
- cost/call/retry/cancellation policy;
- streaming lifecycle;
- refusal/error classification implementation;
- raw-response diagnostics persistence;
- Director/opportunity selection;
- Integrity Validator acceptance/rejection;
- semantic leakage/canon checking;
- accepted/rejected/alternate Take semantics;
- TakeId allocation;
- State Interpreter mutations;
- State Authority;
- ProductionState/StateHash;
- atomic causal commit;
- accepted history;
- observation engine;
- E0-D ablations;
- playwright control;
- WinUI;
- Windows AI/NPU;
- packaging/WACK/Store.

## 28. Exit gate

Before promotion:

1. this blueprint is explicitly approved;
2. implementation begins from then-current `main` on a dedicated branch;
3. only ContextPacket + explicit candidate bytes enter parser;
4. exact JSON shape and strict parser behavior receive adversarial review;
5. candidate copies subject/context/render attribution only from trusted ContextPacket;
6. verified SchemaVersion is carried on parsed candidate;
7. visible text remains preserved and not silently rewritten;
8. typed control remains narrow and non-authoritative;
9. no mutation/private-reasoning/provider/runtime fields enter candidate schema;
10. candidate authority objects cannot be publicly forged;
11. Silence invariants pass;
12. roster-target/self-target/duplicate checks pass;
13. malformed/provider-error-like payloads fail technically rather than becoming fiction;
14. all existing 115 Core tests remain green;
15. frozen Context hashes remain unchanged;
16. frozen ECJ-1 hash/byte length remain unchanged;
17. native Windows ARM64 Core/Harness build passes warnings-as-errors;
18. full Core test suite passes on target machine;
19. existing Missing Raft and smoke Harness regressions pass/0;
20. final hygiene review finds no provider adapter, Director, Integrity decision, State Interpreter, State Authority, Take semantics, persistence, or later-scope implementation;
21. validation evidence distinguishes exact machine-tested executable head from documentation-only closure commits.

## 29. Material approval decisions

Explicit approval freezes these Patch 0006 decisions:

1. Patch 0006 is the Performer candidate-output contract immediately after validated Context Composer and before Director/Integrity/State layers;
2. Patch 0006 implements no provider call;
3. model output contract is strict JSON version `ensemble.e0.performer.candidate.v1`;
4. parsed CandidatePerformance exposes the verified SchemaVersion;
5. trusted candidate attribution fields are copied from ContextPacket and cannot be model-supplied;
6. candidate is attributed to SubjectCharacterId, ContextPacketId, RenderingContract, and RenderedContextHash, without claiming signature/authentication semantics;
7. performance kinds are Speech, Action, Mixed, Silence;
8. refusal/redirection remain semantic behavior rather than extra authority enums;
9. Silence uses exact empty visible text and cannot carry address/nomination control;
10. hidden typed control contains only addressed Character IDs and optional nominated Character ID;
11. typed control is Performer assertion, not state/truth/Director authority;
12. no State Interpreter mutation, confidence score, private reasoning, chain-of-thought, or world-fact proposal enters the Performer candidate schema;
13. parser is strict/fail-closed and never repairs malformed model output;
14. visible candidate text is preserved exactly after decoding; invalid canonical text is rejected rather than rewritten;
15. public validated candidate/control constructors are Core-internal;
16. raw provider response and technical errors remain separate diagnostics, not candidate fiction;
17. TakeId/accepted/rejected/alternate Take semantics remain deferred to their later frozen boundary;
18. provider response size ceilings are deferred to the later provider-execution contract rather than invented as E0 behavior here;
19. Director, Integrity Validator, State Interpreter, State Authority, provider integration, and causal persistence remain outside Patch 0006.

Implementation must not begin until these decisions are approved.
