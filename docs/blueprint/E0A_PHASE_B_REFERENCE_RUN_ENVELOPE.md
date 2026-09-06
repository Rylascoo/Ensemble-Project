# E0-A Phase B — Reference Run Envelope

Status: **PROPOSAL 0.7 — EXPLORATORY; IMPLEMENTATION NOT AUTHORIZED**
Date: 2026-09-05
Parent `main`: `2b628c2d97eb39f4d96ce3e81eafefe93ba28b09`
Branch: `e0a-reference-run-envelope-blueprint`

## 1. Question

Can one immutable E0-A envelope drive the closed H1 Cycle/Turn spine through a real Same-Model Isolated Cast while provider execution, semantic review, spend, cancellation, credentials and evidence remain outside Core and every semantic result is coupled to its exact configured provider attempt?

```text
fixture -> Cycle/Context -> Performer
 -> Patch 0017 -> bounded Integrity assessor -> Patch 0008/0018
 -> Interpreter -> State Authority -> Accepted Take/commit
 -> Opportunity -> repeat
```

Blueprint 0.1 remains authority: E0-A precedes E0-B; all three Characters use the same frontier model; provider/model/settings stay fixed within a batch; E0-C/D/E return to that reference unless explicitly varied; provenance is complete; technical failure never becomes fiction; LLMs never own access/cost/cancel/retry/state/Take/commit authority.

## 2. Fixed probabilistic roles

```text
Provider              OpenAI official API
Model                 gpt-5.6-sol
Performer reasoning   none
Interpreter reasoning none
Integrity reasoning   high
```

All three Performers use the same `none` profile. Interpreter is also `none`. Integrity uses the same model/provider at `high` because it applies a strict safety/integrity rubric rather than portraying a Character. The assessor emits concern evidence only; deterministic Patch 0008 owns disposition and its hard rules cannot be waived.

Director, Access, State Authority, Take, commit and Opportunity remain deterministic Core authority.

## 3. Reasoning characterization

`PERFORMER-NONE` is normative. First characterization varies Performer reasoning only:

```text
PERFORMER-NONE    Performer none    / Integrity high / Interpreter none
PERFORMER-LOW     Performer low     / Integrity high / Interpreter none
PERFORMER-MEDIUM  Performer medium  / Integrity high / Interpreter none
PERFORMER-HIGH    Performer high    / Integrity high / Interpreter none
```

Everything else stays matched. `xhigh`/`max` deferred. A later Interpreter-only characterization may hold Performers at `none`; neither replaces NONE as E0-C/D/E reference.

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

Harness references Core only. Provider/network/filesystem/secrets stay outside Core. Add a Harness test project. No product Application layer or post-E0 compatibility promise.

## 5. OpenAI transport

Use official Responses REST API via `HttpClient`; reverify API/model immediately before implementation and contributing batch. No provider conversation/`previous_response_id`; no tools; `store=false`; `service_tier=default`; `truncation=disabled`; omit temperature/top-p; strict Structured Outputs; request/store no hidden reasoning.

```text
Performer    stream=true   reasoning=none   max_output_tokens=4096
Integrity    stream=false  reasoning=high   max_output_tokens=2048
Interpreter  stream=true   reasoning=none   max_output_tokens=4096
```

Characterization changes only Performer reasoning.

Freeze a provider snapshot if an immutable identifier is actually exposed and usable. Otherwise record requested alias plus every returned model/version identifier available; never claim unobservable snapshot stability. Observable identity change in-batch fails closed.

## 6. Versioned prompts and bounded disclosure

Freeze exact UTF-8 + SHA-256 for:

```text
PerformerPromptContract
IntegrityAssessorPromptContract
StateInterpreterPromptContract
```

No in-batch tuning.

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

Denied records are selected from exact current Production by current Access decisions; inactive records excluded; protected records deduplicated by RecordId. No unrelated provenance graph, commit history, credentials or creator metadata. This trusted Integrity disclosure is never Character knowledge and is recorded separately.

Integrity strict output is only an ordered unique array of Patch 0008 concern enum names; no rationale/confidence/free text.

## 7. Configured-path receipts

Experimental provenance must be a precondition to semantic consumption, not paperwork added afterward.

Every provider invocation starts as a closed `PreparedRoleAttempt` containing:

```text
RunId / attempt / role / turn / CharacterId when applicable
provider / requested model / role reasoning
ContextPacketId / structured + rendered hashes
prompt + response-schema hashes
request-body SHA-256 / max output / service tier
Integrity assessment-packet hash/membership when role=Integrity
```

The exact request body/evidence is written before network disclosure. The configured provider port may return only a closed `RoleAttemptReceipt` bound to that exact prepared attempt. Receipt records terminal outcome, response ID, observable model identity, usage, raw-output hash and exact raw structured output bytes when successful.

The run driver accepts semantic bytes only from a successful receipt whose RunId/role/turn/source identities/profile/request hash equal the current prepared attempt. A receipt from another run/turn/Context/profile fails closed. Technical/cancelled receipts carry no semantic payload.

Thus:

- Candidate parsing requires an exact successful Performer receipt;
- Integrity concern binding requires an exact successful Integrity receipt bound to the Candidate content identity and assessment packet;
- Interpreter proposal parsing requires an exact successful Interpreter receipt bound to the current InterpretationSource/Candidate identity.

This is configured-path provenance coupling, not a claim of cryptographic attestation by Core. Auth headers/secrets never enter receipts/evidence.

## 8. Run identity / no overwrite

Manifest supplies one Core `RunId`; run fails if that directory already exists.

```text
TakeId   = {RunId}:TAKE:{turn:D3}
CommitId = {RunId}:COMMIT:{turn:D3}
RecordId = {RunId}:RECORD:{turn:D3}:{mutationIndex:D3}
Attempt  = {RunId}:ATTEMPT:{role}:{turn:D3}:{attempt:D2}
```

Constrain RunId for Core CanonicalId limits. Attempt identity is Harness metadata.

## 9. Fixed envelope / spend

```text
accepted-Turn cap                     12
attempts per probabilistic invocation  1
automatic retries                       0
attempt timeout                         300 seconds each
estimated model-token spend ceiling     USD 5.00/run
```

After Turn 12 establish next Patch 0016 Opportunity, then stop `AcceptedTurnCapReached`; experimental stop only, not Scene ending (ODR-13 open).

Timeout/refusal/provider/transport/incomplete-invalid output/cancellation of any role terminates without replacement. E0-F may later vary failures/retries.

Manifest freezes rate assumptions for **estimated model-token cost**; provider billing remains external authority. Before each inference: prepare exact request; obtain supported Responses input-token count; reserve worst-case uncached input + that role's max output; refuse before inference if cumulative estimate exceeds USD 5.00. After response replace reservation with estimated actual usage charge, distinguishing cached/reasoning classes where supported. Token-count calls are preflight evidence, not role attempts. `BudgetExceeded` is non-fictional terminal evidence.

## 10. Integrity / State Authority behavior

For a correctly bound Candidate:

```text
successful configured Integrity receipt
 -> strict concern array
 -> IntegrityConcernEvidence.Bind
 -> Patch 0008 deterministic Validate / Patch 0018 EvaluateIntegrity
```

No concerns -> Accept. Any concern -> `RequestAnotherTake`; with zero retries, run terminates before Interpreter/Take/commit. Deterministic Reject short-circuits before semantic review. Assessor technical failure gives no concern evidence/no Integrity disposition and terminates. This satisfies Patch 0008's configured concern-review provenance requirement; synthetic empty evidence is test-only, never contributing-run authority.

Reference `StateAuthorityPolicy` includes all Patch 0010 policy-eligible domains:

```text
UnresolvedProposition, CharacterBelief, CharacterSuspicion,
CharacterGoal, CharacterCircumstance, CharacterClaim, Pressure
```

Patch 0010 still owns hard rejection/mandatory review. On `ReviewRequired`, supply explicit Reject choices for every unresolved review mutation through Patch 0018, recorded as `E0AReferenceDeterministicReject`, never human/creator review. Mandatory-review mutations never auto-approve; Performance may still be Accepted under Patch 0011; rejected proposals/reasons remain provenance.

## 11. Provider mapping / loop

Successful Performer receipt -> raw/stream evidence -> `PerformerCandidateContract.ParseJson(exact Context)` -> Patch 0017 BindCandidate -> Turn GateAttempt. Provider/refusal/timeout/incomplete/invalid -> TechnicalFailure; explicit cancel -> Cancelled; partial output diagnostics only.

Successful Interpreter receipt -> raw/stream evidence -> `StateInterpretationContract.ParseJson(exact InterpretationSource)` -> Turn EvaluateAuthority. Technical/cancel/refusal/timeout/incomplete/schema failure stops before Take; no partial proposal reaches authority.

Each terminal Approved Add gets one derived RecordId/materialization; rejected mutations get none; Supersede/Deactivate use existing targets. Provider output never chooses IDs.

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
```

Manifest precedes inference/immutable; attempts write-once; events append-only; finalization SHA-256 hashes all artifacts. Credentials never enter evidence. Partial/rejected/technical output never enters accepted history. Hidden reasoning neither requested nor stored.

Record Blueprint/fixture/hash; executable Git commit + host OS/arch/.NET; variant; prompt/schema hashes; Access/Context; every role's provider/model/reasoning/disclosure/receipt; Integrity packet membership/concerns; Performer/control; Director/Integrity/Interpreter/Authority; mutation decisions; Take/commit/Opportunity; latency; usage/cost; failures; blind mapping.

Initial fixture + accepted Take/commit/Opportunity evidence must replay the supported H1 transition chain; `run.final.json` is only a checkpoint/projection.

Blind transcript hides variant/provider/reasoning/RunId/hashes/implementation metadata and uses stable neutral speaker labels; mapping remains separate.

## 13. Contribution / reasoning sweep

Reference contributes only if manifest preceded execution; configured-path receipts are complete; observable model identity stayed frozen; all 12 Turns reached accepted commit + next Opportunity; no technical/cancel/budget/Integrity terminal; evidence finalized; independent post-run hard-gate review finds no Blueprint violation. Post-run failure invalidates contribution but never rewrites history.

Failed/noncontributing runs remain immutable; RunId never reused.

PERFORMER LOW/MEDIUM/HIGH reuse exact envelope except Performer reasoning; Integrity high and Interpreter none stay fixed. Same role output ceilings and USD 5.00 run ceiling remain. Extra reasoning cost/truncation/timeout is observable behavior, not compensated.

## 14. Credentials / pre-spend validation

Adapter reads `OPENAI_API_KEY` only as process-injected secret. Missing secret fails before provider execution. Never persist/echo/hash/pass below Harness provider edge. E0 only, not product secret storage.

Before real spend, fake-adapter tests prove: Core unchanged/dependencies clean; role profiles/reasoning isolation; exact Context/request/receipt coupling; stale/cross-run receipt rejection; exact Integrity-packet derivation; configured concern provenance required; assessor cannot waive deterministic Reject; one attempt/zero retry; technical outcomes never fiction; cost refusal precedes inference; mandatory-review reject-only behavior; deterministic IDs/materializations; 12-turn synchronization; partial streams diagnostic only; no-overwrite/replay-capable/secret-free evidence; blind identity removal.

Native Windows ARM64 compile/tests/Harness smoke must pass before first real provider request.

## 15. Exclusions / approval

No Core redesign; product Application/persistence; provider conversation; retry/backoff; understudy; E0-B mixed model; E0-E playwright implementation; E0-F failure implementation; Context optimization; Scene ending; ODR-12/13/30/32; WinUI; Windows AI/NPU; MSIX/WACK/Store.

Approval freezes: GPT-5.6 Sol; Performers NONE + Interpreter NONE + fixed Integrity HIGH; isolated Performer LOW/MEDIUM/HIGH characterization; bounded role prompts + Integrity packet; closed configured-path attempt receipts; REST/stateless/no-tools/strict output; 12 accepted Turns; one attempt/zero retry/300s per role; Performer/Interpreter 4096 and Integrity 2048 output ceilings; USD 5.00 estimated run-token ceiling; deterministic authority/review-reject policy and IDs; immutable replay-capable evidence/blind package; Harness-only provider edge, Core unchanged.

Approval **does not authorize provider requests, credential use, or spend**. Implementation remains fake-first and returns to native ARM64 validation before first real provider call.
