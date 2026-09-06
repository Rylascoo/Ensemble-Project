# E0-A Phase B — Reference Run Envelope

Status: **PROPOSAL 0.3 — EXPLORATORY; IMPLEMENTATION NOT AUTHORIZED**
Date: 2026-09-05
Parent `main`: `2b628c2d97eb39f4d96ce3e81eafefe93ba28b09`
Branch: `e0a-reference-run-envelope-blueprint`

## 1. Question

Can one immutable E0-A envelope drive the closed H1 Cycle/Turn spine through a real Same-Model Isolated Cast while keeping provider execution, spend, cancellation, credentials and run evidence outside Core and making every disclosure/result attributable?

```text
frozen fixture -> H1 Cycle/Context -> provider Performer
 -> Patch 0017/0018 -> Integrity -> provider Interpreter
 -> deterministic State Authority -> Accepted Take/commit
 -> explicit Opportunity -> repeat
```

Blueprint 0.1 remains authority: E0-A precedes E0-B; all three Characters use the same frontier model; provider/model and supported generation/reasoning settings are fixed within a comparison batch; E0-C/D/E return to that reference unless separately labeled; every run preserves complete provenance; technical failure never becomes fiction; deterministic access/cost/cancel/retry rules never belong to an LLM.

## 2. Primary reference

Director-directed E0-A reference:

```text
Provider                 OpenAI official API
Model                    gpt-5.6-sol
Deliberation intent      LeastDeliberativeSupported
Provider-native setting  reasoning.effort = none
```

Use the same profile for Voss, Marlowe, Wren and the State Interpreter. Director, Access, Integrity hard checks, State Authority, Take, commit and Opportunity remain deterministic Core authority.

The primary reference has no model-assisted Integrity assessor. Later hard-gate/blind evaluation may invalidate a completed run but cannot rewrite it.

## 3. Reasoning characterization

`none` remains the normative reference. After it works, separately labeled characterization variants may change only reasoning:

```text
NONE    gpt-5.6-sol / none   <- reference
LOW     gpt-5.6-sol / low
MEDIUM  gpt-5.6-sol / medium
HIGH    gpt-5.6-sol / high
```

Fixture, prompts, Context, model, deterministic authority, service tier, timeout, output ceiling, spend ceiling and run protocol stay matched. `xhigh`/`max` are deferred ceiling tests. E0-C/D/E continue to use `none` unless an explicit variant changes reasoning.

Cross-provider law for later E0-B:

```text
DeliberationIntent = LeastDeliberativeSupported
ProviderNativeSetting = exact native control sent
```

Provider `none`, disabled, `minimal`, or low modes are recorded exactly and never claimed computationally equivalent.

## 4. Harness boundary

Do not create a product Application layer or another disposable runtime assembly merely to host E0.

If approved, `Ensemble.E0.Core` stays unchanged. `Ensemble.E0.Harness` gains logically separated namespaces/components for:

```text
Run        provider-neutral run state/policy + deterministic fakeable adapter contract
Evidence   local experimental evidence interfaces/implementation
OpenAI     HTTP/streaming provider adapter
Host       minimal CLI + credential/cancellation edge
```

Harness references Core only. Provider/network/filesystem/secrets never enter Core. A separate Harness test project is earned for fake-adapter, evidence and run-driver tests. No post-E0 compatibility promise is created.

## 5. OpenAI transport

Use the official Responses REST API through `HttpClient`, not a provider SDK dependency. Reverify the API/model surface immediately before implementation and before the contributing batch.

Reference request settings:

```text
model              gpt-5.6-sol
reasoning.effort   none
stream             true
store              false
service_tier       default
tools              []
truncation          disabled
max_output_tokens  4096
```

No provider conversation/`previous_response_id`; no web/file/computer/code/MCP/function tools; omit `temperature` and `top_p` rather than invent a sampling policy. Use strict Structured Outputs for Performer Candidate and Interpreter proposal. Request no reasoning summary/encrypted reasoning/chain-of-thought.

If OpenAI exposes a suitable immutable snapshot identifier, freeze it. Otherwise record requested model alias and every returned model/version identifier available; never claim snapshot immutability the provider does not expose. Any observable returned identity change inside the batch fails closed.

## 6. Bounded disclosures and prompts

Implementation creates exactly two versioned prompt contracts:

```text
PerformerPromptContract
StateInterpreterPromptContract
```

Freeze exact UTF-8 bytes and SHA-256 hashes before a contributing run; no prompt tuning inside the batch.

Performer disclosure: exact current Character-bounded rendered Context + Candidate output contract only.

Interpreter disclosure: same bounded source Context + exact Candidate Performance/control + mutation-proposal contract. It receives no full Production repository, denied/private material, State Authority snapshot/locks, other Characters' private state, creator-only provenance or credentials.

Before network execution create a prepared-attempt record containing:

```text
RunId / attempt identity / role / turn ordinal
CharacterId when Performer
provider / requested model / reasoning setting
ContextPacketId / StructuredContextHash / RenderedContextHash
prompt + response-schema version/hash
exact request-body SHA-256
max output / service tier
```

Write exact request body locally before disclosure. Never record auth headers/secrets. Couple returned response ID, observable model identity, service tier, status, usage and terminal metadata to that attempt.

## 7. Identity and no-overwrite law

The manifest supplies one initialized Core `RunId`. A run cannot start if that RunId/run directory already exists; evidence is never overwritten.

Derive fictional/causal IDs without clock/randomness:

```text
TakeId   = {RunId}:TAKE:{turn:D3}
CommitId = {RunId}:COMMIT:{turn:D3}
RecordId = {RunId}:RECORD:{turn:D3}:{mutationIndex:D3}
```

Constrain RunId length so derived values stay within Core CanonicalId limits.

Provider-attempt identity is experimental metadata, not a Core strong ID:

```text
{RunId}:ATTEMPT:{role}:{turn:D3}:{attempt:D2}
```

## 8. Fixed reference envelope

```text
accepted-Turn cap        12
attempts/role invocation 1
automatic retries        0
attempt timeout          300 seconds
max_output_tokens        4096
estimated model-token spend ceiling  USD 5.00
```

Twelve accepted Turns bound cost while allowing multiple three-Character causal cycles. After Turn 12, establish the next Patch 0016 Opportunity so final state is synchronized, then stop as `AcceptedTurnCapReached`. This is an experimental stop, not a dramatic Scene-ending decision; ODR-13 remains open.

Timeout/refusal/provider error/incomplete or invalid output/transport failure/cancellation terminates the reference run without replacement. E0-F may later introduce retry/failure variants.

## 9. Deterministic spend gate

The manifest freezes rate assumptions used for **estimated model-token cost**. Provider billing/invoice remains external financial authority.

Before each inference request:

1. prepare exact request;
2. obtain exact input token count through the supported Responses input-token-count endpoint;
3. calculate worst-case next inference token charge using uncached input rate + 4096 output tokens;
4. add it to already incurred estimated inference cost;
5. if the sum would exceed USD 5.00, do not send inference.

After a response, replace reservation with estimated actual charge from returned usage, distinguishing cached/reasoning classes where supported. Preflight/token-count activity is separately recorded and is not a Performer/Interpreter attempt or fictional event. No claim is made that USD 5.00 equals final account billing exactly.

`BudgetExceeded` is terminal non-fictional evidence.

## 10. Integrity and authority policy

Live reference Integrity calls Patch 0018 `EvaluateIntegrity` with an initialized empty semantic-concern set. Current deterministic source/structural checks remain authoritative; no hidden second model judges/rephrases the Candidate. If Integrity requests another Take, zero-retry policy terminates before Take/commit.

Reference `StateAuthorityPolicy` contains every Patch 0010 policy-eligible domain:

```text
UnresolvedProposition
CharacterBelief
CharacterSuspicion
CharacterGoal
CharacterCircumstance
CharacterClaim
Pressure
```

Patch 0010 still owns hard rejection and transition-specific mandatory review. If evaluation is `ReviewRequired`, the run driver supplies explicit **Reject** choices for every unresolved review-required mutation and resolves through Patch 0018.

Record these choices as `E0AReferenceDeterministicReject`, never creator/human review. Mandatory-review mutations are never auto-approved. Patch 0011 may still accept the Performance; rejected proposals/reasons remain provenance.

## 11. Provider result mapping

Performer:

```text
completed structured output
 -> save raw/stream diagnostics
 -> PerformerCandidateContract.ParseJson(exact Context)
 -> Patch 0017 BindCandidate -> Turn.GateAttempt
```

Refusal/timeout/transport/provider/incomplete/invalid output -> Patch 0017 `TechnicalFailure`; explicit cancel -> `Cancelled`. Partial output remains diagnostics only.

Interpreter:

```text
ReadyForInterpretation
 -> bounded provider request
 -> save raw/stream diagnostics
 -> StateInterpretationContract.ParseJson(exact InterpretationSource)
 -> Turn.EvaluateAuthority
```

Any Interpreter technical/cancel/refusal/timeout/incomplete/schema failure terminates before Take. No partial proposal reaches State Authority.

## 12. Materialization and run loop

For each terminal Approved `Add`, derive one RecordId and create one `E0RecordMaterialization`. Rejected mutations get none; Supersede/Deactivate use existing targets. Provider output never chooses IDs.

For accepted-turn ordinal 1..12:

```text
opportunity-bearing Cycle
 -> ComposeContext
 -> record Performer request -> spend gate -> one streamed attempt
 -> Patch 0017 -> Turn.GateAttempt
 -> Turn.EvaluateIntegrity(empty concerns)
 -> record Interpreter request -> spend gate -> one streamed attempt
 -> strict Interpreter parse
 -> Turn.EvaluateAuthority(reference policy)
 -> reject unresolved mandatory-review mutations if required
 -> Turn.BindAcceptedTake(derived TakeId)
 -> deterministic materializations
 -> Turn.CommitAccepted(derived CommitId)
 -> VALID POSTCOMMIT STATE
 -> Patch 0016 EstablishOpportunity
 -> adopt next synchronized opportunity-bearing state
```

Technical/cancel/budget/Integrity/Interpreter terminal outcomes stop immediately. No provider call occurs between accepted commit and Opportunity establishment.

## 13. Immutable experimental evidence

Run evidence is local experimental provenance, **not Production persistence**.

Minimum run directory:

```text
manifest.json
attempts/<id>/request.json
attempts/<id>/stream.ndjson
attempts/<id>/terminal.json
events.ndjson
run.final.json
transcript.json
blind/transcript.json
blind/mapping.json
```

Manifest is immutable before first inference; attempt artifacts write-once; event log append-only; finalization records SHA-256 for every artifact. Credentials never enter evidence. Partial/rejected/technical output remains diagnostic and never accepted history. Hidden reasoning is neither requested nor stored.

Manifest/evidence records Blueprint/fixture/hash; repository executable commit + host OS/architecture/.NET runtime; variant/reference identity; prompt/schema hashes; access/context; provider/model/native settings; Performer output/control; Director/Integrity/Interpreter/Authority; accepted/rejected mutations/reasons; Take/commit/Opportunity transitions; latency; token usage/cost estimate; technical outcomes; blind mapping.

The initial fixture plus exact accepted Take/commit/Opportunity evidence must be sufficient to replay the supported H1 transition chain; `run.final.json` is a projection/checkpoint, not a competing causal authority.

Blind transcript hides variant/provider/reasoning/RunId/hashes/implementation metadata and uses stable neutral speaker labels. Mapping stays separate/private to evaluation coordination.

## 14. Contribution and reasoning-sweep rules

A reference run contributes experiential evidence only if:

- manifest preceded provider execution;
- observable provider/model identity stayed within the frozen condition;
- all 12 Turns reached Accepted Take + atomic commit + next Opportunity;
- no technical/cancel/budget/Integrity terminal occurred;
- evidence finalization succeeded;
- hard-integrity review finds no Blueprint gate violation.

Failed/noncontributing runs remain immutable evidence and are never silently rerun under the same RunId.

LOW/MEDIUM/HIGH characterization reuses the exact envelope except provider-native reasoning setting. Keep 4096 output and USD 5.00 estimated token ceiling matched. Extra reasoning cost/truncation/timeout is observable behavior, not silently compensated. These variants never replace NONE as the E0 reference.

## 15. Credentials and pre-spend validation

The adapter may consume `OPENAI_API_KEY` only as process-injected secret material. Missing credential fails before provider execution. Never persist, echo, hash or pass the secret below the Harness provider edge. This is E0 infrastructure, not post-E0 secret-store design.

Before real spend, fake-adapter tests must prove:

- Core unchanged; provider/network/filesystem dependency stays in Harness;
- same model/profile for all three Performers + Interpreter;
- NONE reference and separate LOW/MEDIUM/HIGH mapping;
- exact Context/request coupling and stale-result rejection;
- one attempt/zero retry; timeout/cancel/refusal/invalid output never fiction;
- cost gate refuses before inference;
- mandatory-review mutations reject, never auto-approve;
- deterministic IDs/materializations;
- 12 accepted Turns preserve Cycle/Turn synchronization;
- partial streams remain diagnostic;
- evidence is no-overwrite/write-once/append-only, replay-capable and secret-free;
- blind package omits experimental identity.

Native Windows ARM64 compile/tests/Harness smoke must pass before the first real provider call.

## 16. Exclusions / approval boundary

No Core redesign; product Application/persistence; provider conversation state; automatic retry/backoff; understudy; mixed-model E0-B; E0-E playwright implementation; E0-F failure implementation; semantic Integrity model; Context optimization; Scene-ending resolution; ODR-12/13/30/32; WinUI; Windows AI/NPU; MSIX/WACK/Store.

Approval freezes: GPT-5.6 Sol/NONE reference; LOW/MEDIUM/HIGH characterization; same profile for Performers + Interpreter; REST/stateless/no-tools/strict-output transport; 12 accepted Turns; one attempt/zero retry/300s timeout; 4096 output ceiling; USD 5.00 estimated model-token ceiling; deterministic authority/review-reject policy; deterministic run-local IDs; immutable replay-capable evidence/blind package; Harness-only provider edge with Core unchanged.

Approval **does not authorize a provider request, credential use, or spend**. Implementation remains fake-first and returns to native ARM64 validation before the first real call.
