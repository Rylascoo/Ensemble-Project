# E0-A Phase B — Reference Run Envelope

Status: **PROPOSAL 0.14 — RECURSIVELY AUDITED; DIRECTOR APPROVAL REQUIRED; IMPLEMENTATION FORBIDDEN**
Parent `main`: `2b628c2d97eb39f4d96ce3e81eafefe93ba28b09`
Branch: `e0a-reference-run-envelope-blueprint`

## 1. Question

Can one E0-A envelope drive the closed H1 spine through Same-Model Isolated Cast while provider execution/provenance/spend stays outside Core and semantic results remain bound to configured attempts?

```text
fixture -> Cycle/Context -> Performer
 -> Patch 0017 -> bounded Integrity assessor -> Patch 0008/0018
 -> Interpreter -> State Authority -> Accepted Take/commit
 -> Opportunity -> repeat
```

Blueprint 0.1 remains authority: E0-A precedes E0-B; three Characters share fixed frontier model/settings; E0-C/D/E return to that reference unless varied; provenance is complete; technical failure never becomes fiction; LLMs never own authority.

## 2. Fixed probabilistic roles

```text
Provider              OpenAI official API
Model                 gpt-5.6-sol
Performer reasoning   none
Interpreter reasoning none
Integrity reasoning   high
```

Performers and Interpreter use `none`. Integrity uses the same model/provider at `high` as fixed safety control; it emits concern evidence only, while Patch 0008 owns disposition.

Director, Access, State Authority, Take, commit and Opportunity remain deterministic.

## 3. Reasoning characterization

`CREATIVE-NONE` is normative. First characterization varies the two creative generative roles together while Integrity stays fixed at `high` as a safety control:

```text
CREATIVE-NONE    Performer none    / Integrity high / Interpreter none
CREATIVE-LOW     Performer low     / Integrity high / Interpreter low
CREATIVE-MEDIUM  Performer medium  / Integrity high / Interpreter medium
CREATIVE-HIGH    Performer high    / Integrity high / Interpreter high
```

`xhigh`/`max` deferred. If warranted, later role-isolation may vary Performer or Interpreter alone. CREATIVE-NONE remains E0-C/D/E reference.

Later E0-B records semantic intent `LeastDeliberativeSupported` plus exact provider-native control. Provider `none`, disabled, `minimal`, or low modes are never claimed equivalent.

## 4. Harness boundary

Core stays unchanged. Harness gains logical components:

```text
Run        run policy/driver + fakeable provider-role port
Integrity  bounded assessment packet + concern parser
Evidence   local experimental evidence
OpenAI     HTTP provider adapter
Host       CLI + credential/cancellation edge
```

Harness references Core only; provider/network/filesystem/secrets stay outside Core. Add a Harness test project; no product Application layer.

## 5. OpenAI transport

Use official Responses REST via `HttpClient`; reverify before implementation/batch. No provider conversation/`previous_response_id`, tools or hidden reasoning; `store=false`; `service_tier=default`; `truncation=disabled`; omit temperature/top-p; strict Structured Outputs.

```text
Performer    stream=true   reasoning=none   max_output_tokens=4096
Integrity    stream=false  reasoning=high   max_output_tokens=4096
Interpreter  stream=true   reasoning=none   max_output_tokens=4096
```

Characterization changes Performer and Interpreter reasoning together; Integrity remains `high`.

Use an immutable provider snapshot if exposed/usable; otherwise record requested/returned identifiers without claiming stability. Observable in-batch identity change fails. Schemas stay within verified Structured Outputs; Core parsers retain semantic authority.

## 6. Versioned prompts and bounded disclosure

Freeze exact UTF-8 + SHA-256 for:

```text
PerformerPromptContract
IntegrityAssessorPromptContract
StateInterpreterPromptContract
```

Fixed instructions remain separate from deterministic serialization of dynamic fixture/Context/Candidate/record values as **untrusted, non-instructional data**; dynamic content never enters the instruction channel.

Performer receives exact Character-bounded rendered Context + Candidate schema only.

Interpreter receives that bounded source Context + exact Candidate Performance/control + mutation schema; no full Production, denied/private state, State Authority snapshot/locks or credentials.

Integrity receives a separate `E0AIntegrityAssessmentPacket` containing exactly:

```text
Source Context identity + CharacterId
Candidate visible text + typed control
Character-visible rendered Context
active denied Production records + AccessReason
active SystemImmutable/CreatorLocked records
five Patch 0008 concern definitions
```

Denied records come from exact current Production + Access decisions; inactive excluded; protected deduplicated by RecordId. No unrelated provenance, credentials or creator metadata. Integrity disclosure is never Character knowledge and is recorded separately.

Integrity output is strict root object `{"concerns":[...]}` containing only Patch 0008 concern names; do not rely on provider uniqueness/order constraints. Core binding owns defined/duplicate/canonical semantics. No rationale/confidence/free text.

## 7. Configured-path receipts

Provenance is a precondition to semantic consumption.

Every provider invocation starts as a closed `PreparedRoleAttempt` containing:

```text
RunId / attempt / role / turn / CharacterId when applicable
provider / requested model / role reasoning
ContextPacketId / structured + rendered hashes
CandidateContentHash for Integrity/Interpreter
prompt + response-schema hashes
request-body SHA-256 / max output / service tier
Integrity assessment-packet hash/membership when role=Integrity
```

For Integrity, canonical `IntegrityCandidateInput.Bind` may run before disclosure only to obtain Candidate content identity and prove zero deterministic reject codes; it creates no evaluation. Any impossible reject-coded Candidate fails before semantic review.

Write exact request evidence before disclosure. The provider port returns only a closed `RoleAttemptReceipt` bound to that attempt: outcome, response/model identity, usage, raw-output hash and successful structured bytes.

Accept semantic bytes only from a successful receipt matching current RunId/role/turn/source/profile/request hash. Cross-run/turn/Context/profile receipts fail; technical/cancelled receipts carry no semantic payload.

Thus:

- Candidate parsing requires an exact successful Performer receipt;
- Integrity concern binding requires an exact successful Integrity receipt bound to the Candidate content identity and assessment packet;
- Interpreter proposal parsing requires an exact successful Interpreter receipt bound to the current InterpretationSource/Candidate identity.

Configured-path provenance is not Core attestation; secrets never enter evidence.

## 8. Run identity / no overwrite

Manifest supplies one Core `RunId`; run fails if that directory already exists.

```text
TakeId   = {RunId}:TAKE:{turn:D3}
CommitId = {RunId}:COMMIT:{turn:D3}
RecordId = {RunId}:RECORD:{turn:D3}:{mutationIndex:D3}
Attempt  = {RunId}:ATTEMPT:{role}:{turn:D3}:{attempt:D2}
```

Constrain RunId to at most 96 canonical characters so all derived Core IDs remain below the 128-character ceiling. Attempt is Harness metadata.

## 9. Fixed envelope / spend

```text
accepted-Turn cap                     12
attempts per probabilistic invocation  1
automatic retries                       0
attempt timeout                         300 seconds each
estimated model-token spend ceiling     USD 5.00/run
```

After Turn 12 establish the next Patch 0016 Opportunity, then stop `AcceptedTurnCapReached`; this is technical truncation, not Scene ending (ODR-13).

Timeout/refusal/provider/transport/incomplete-invalid output/cancellation of any role terminates without replacement. E0-F may later vary failures/retries. Honor cancellation before/while provider work and before another Turn; after commit, attempt immediate deterministic Opportunity establishment before honoring it, with no external work in that gap.

Manifest freezes assumptions for **estimated model-token cost**; provider billing remains external authority. Before inference reserve worst-case input + role max output using a verified supported count or conservative bound; otherwise block real execution. Refuse if cumulative estimate exceeds USD 5.00 and reconcile reported usage afterward. `BudgetExceeded` is non-fictional.

## 10. Integrity / State Authority behavior

After a successful Integrity receipt, parse `concerns`, verify receipt/packet identity, then call only `DeterministicE0TurnOrchestrator.EvaluateIntegrity(candidateReady, concerns)`. Turn re-binds input/evidence, runs Patch 0008, and creates InterpretationSource on Accept; Harness has no parallel evaluation path.

No concerns -> Accept. Any concern -> `RequestAnotherTake`; zero retries ends before Interpreter/Take/commit. Assessor technical/schema failure creates no Integrity disposition. Synthetic empty evidence is test-only.

Reference `StateAuthorityPolicy` includes all Patch 0010 policy-eligible domains:

```text
UnresolvedProposition, CharacterBelief, CharacterSuspicion,
CharacterGoal, CharacterCircumstance, CharacterClaim, Pressure
```

Patch 0010 owns hard rejection/mandatory review. On `ReviewRequired`, reject every unresolved mutation through Patch 0018 as `E0AReferenceDeterministicReject`, never human/creator review. Performance may still be Accepted; rejected proposals/reasons remain provenance.

## 11. Provider mapping / loop

Successful Performer receipt -> evidence -> `PerformerCandidateContract.ParseJson(exact Context)` -> Patch 0017 -> Turn gate. Provider/refusal/timeout/incomplete/invalid -> TechnicalFailure; cancel -> Cancelled; partial output diagnostic only.

Successful Interpreter receipt -> evidence -> `StateInterpretationContract.ParseJson(exact InterpretationSource)` -> Turn authority. Technical/cancel/refusal/timeout/incomplete/schema failure stops before Take.

Each terminal Approved Add **or Supersede** gets one derived RecordId/materialization; Supersede also names its existing target; Deactivate/rejected mutations get no new RecordId. Provider never chooses IDs.

For accepted turn 1..12:

```text
Cycle.ComposeContext
 -> Performer spend gate/receipt -> Patch 0017/Turn.GateAttempt
 -> deterministic Integrity hard check
 -> bounded Integrity spend gate/receipt -> Patch 0008/Turn.EvaluateIntegrity
 -> Interpreter spend gate/receipt -> strict parse
 -> Turn.EvaluateAuthority -> reject unresolved mandatory-review items
 -> Turn.BindAcceptedTake(derived TakeId)
 -> materializations -> Turn.CommitAccepted(derived CommitId)
 -> VALID POSTCOMMIT -> Patch 0016 EstablishOpportunity
 -> next synchronized state
```

No provider call occurs between accepted commit and Opportunity establishment.

## 12. Immutable evidence

Evidence is local experimental provenance, **not Production persistence**:

```text
manifest.json
attempts/<id>/request.json
attempts/<id>/terminal.json
attempts/<streaming-id>/stream.ndjson
events.ndjson
run.final.json
transcript.json
blind/transcript.json
blind/mapping.json
evaluation/hard-gates.json
evaluation.final.json
```

Manifest precedes inference/immutable; attempts write-once; events append-only. `run.final.json` seals the runtime artifact set with a canonical digest list/root that excludes itself. Independent post-run hard-gate review under a versioned checklist fixed before the batch writes `evaluation/hard-gates.json`; `evaluation.final.json` seals it to the runtime root. In-run probabilistic roles cannot perform it; reviewer/method identity is recorded. Review may invalidate contribution but cannot rewrite runtime evidence. Credentials never enter evidence. Partial/rejected/technical output never enters accepted history. Hidden reasoning is neither requested nor stored.

Record Blueprint/fixture/hash; executable commit + host; variant; prompt/schema hashes; Access/Context; role profiles/disclosures/receipts; Integrity packet/concerns; Performer/control; Director/Interpreter/Authority; mutations; causal IDs/Opportunity; latency/usage/cost/failures; blind mapping.

Fixture + accepted Take/commit/Opportunity evidence must replay the H1 transition chain; `run.final.json` is checkpoint/projection only.

Blind transcript hides variant/provider/reasoning/RunId/hashes/implementation metadata and uses stable neutral speaker labels; mapping remains separate.

## 13. Contribution / reasoning sweep

Reference contributes only if manifest preceded execution; receipts are complete; observable model identity stayed fixed; all 12 Turns reached commit + next Opportunity; no technical/cancel/budget/Integrity terminal; runtime/evaluation seals complete; hard-gate review passes. Review failure invalidates contribution but never rewrites history.

Failed/noncontributing runs remain immutable; RunId never reused. Their frequency/reasons are condition evidence and must be reported; clean-run selection cannot hide instability.

CREATIVE-LOW/MEDIUM/HIGH reuse the exact envelope except matched Performer+Interpreter reasoning; Integrity stays high. Output and USD 5.00 ceilings remain fixed. Extra reasoning cost/truncation/timeout is observable behavior, not compensated.

## 14. Credentials / pre-spend validation

Adapter reads `OPENAI_API_KEY` only as process-injected secret. Missing secret fails before provider execution. Never persist/echo/hash/pass below Harness provider edge. E0 only, not product secret storage.

Before real spend, fake-adapter tests prove Core unchanged; role/reasoning isolation; Context/request/receipt coupling; stale receipt rejection; Integrity packet/provenance with no parallel evaluation; deterministic hard-rule supremacy; one attempt/zero retry; cost preflight; reject-only review; IDs/materializations; 12-turn synchronization; partial-output exclusion; sealed secret-free replay evidence; blind identity removal.

Native Windows ARM64 compile/tests/Harness smoke must pass before first real provider request.

## 15. Exclusions / approval

No Core redesign; product Application/persistence; provider conversation; retry/backoff; understudy; E0-B mixed model; E0-E playwright implementation; E0-F failure implementation; Context optimization; Scene ending; ODR-12/13/30/32; WinUI; Windows AI/NPU; MSIX/WACK/Store.

Approval freezes: GPT-5.6 Sol; CREATIVE-NONE (Performer+Interpreter none) + fixed Integrity HIGH; matched CREATIVE-LOW/MEDIUM/HIGH characterization; bounded role prompts + Integrity packet; closed configured-path attempt receipts; REST/stateless/no-tools/strict output; 12 accepted Turns; one attempt/zero retry/300s per role; 4096 max output each role; USD 5.00 estimated run-token ceiling; deterministic authority/review-reject policy and IDs; immutable replay-capable evidence/blind package; Harness-only provider edge, Core unchanged.

Approval **does not authorize provider requests, credential use, or spend**. Implementation remains fake-first and returns to native ARM64 validation before first real provider call.
