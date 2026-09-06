# E0-A Phase B — Reference Run Envelope

Status: **PROPOSAL 0.1 — EXPLORATORY; IMPLEMENTATION NOT AUTHORIZED**

Date: 2026-09-05
Parent `main`: `2b628c2d97eb39f4d96ce3e81eafefe93ba28b09`
Branch: `e0a-reference-run-envelope-blueprint`

## 1. Purpose

Freeze the smallest E0-A experimental runtime boundary above the closed H1 deterministic spine before any paid provider execution.

The envelope must make one Same-Model Isolated Cast run reproducible and attributable without moving provider, network, spend, retry, credential, diagnostic, or run-record authority into Core.

```text
frozen E0 fixture
 -> synchronized H1 Cycle state
 -> exact Character Context
 -> provider attempt
 -> Patch 0017/0018 semantic gate
 -> Integrity
 -> provider State Interpreter attempt
 -> deterministic State Authority
 -> Accepted Take
 -> atomic causal commit
 -> explicit Opportunity establishment
 -> repeat under one immutable run envelope
```

## 2. Frozen authority retained

Blueprint 0.1 remains unchanged:

- E0-A is Same-Model Isolated Cast and precedes E0-B.
- All three Characters use the same capable frontier model.
- provider/model and supported generation/reasoning settings are fixed within a comparison batch.
- Character definitions, bounded Context, fixture, Director and deterministic authority rules remain fixed.
- E0-C/D/E return to the E0-A same-model reference unless a separately labeled variant deliberately changes another variable.
- E0-E uses the same underlying model and matched supported settings.
- every run preserves complete experimental provenance.
- technical failure/refusal/timeout/retry/cancellation cannot become fictional action.
- deterministic cost, cancellation, eligibility and access rules are never delegated to a model.

H1 Phase A is closed. This proposal must reuse Core Cycle/Turn unchanged.

## 3. Primary reference decision already directed by the Director

Primary E0-A reference inference profile:

```text
provider          OpenAI official API
model             gpt-5.6-sol
reasoning effort  none
reference intent  LEAST_DELIBERATIVE_SUPPORTED
```

The same profile is used for:

- Voss Performer;
- Marlowe Performer;
- Wren Performer;
- State Interpreter.

No probabilistic Director, State Authority, Take authority or commit authority is introduced.

The reference does not use a model-assisted Integrity assessor. Current deterministic Integrity owns the live pre-acceptance gate. The later blind/hard-gate evaluation may invalidate a run for semantic leakage or other Blueprint hard-gate failure, but cannot rewrite that run.

## 4. Reasoning is an explicit experimental variable

`none` is the normative E0-A reference because the architectural question is strongest when the underlying model receives the least supported deliberation assistance.

After the reference envelope is proven, separately labeled characterization variants may run:

```text
E0A-REASONING-NONE    gpt-5.6-sol / none   <- reference
E0A-REASONING-LOW     gpt-5.6-sol / low
E0A-REASONING-MEDIUM  gpt-5.6-sol / medium
E0A-REASONING-HIGH    gpt-5.6-sol / high
```

Only the native reasoning control changes. Model, prompt contracts, fixture, Context, deterministic authority, request limits, service tier and run protocol remain matched.

`xhigh` and `max` are not part of the first characterization sweep. They may be added later only as separately labeled ceiling tests.

The `none` configuration remains the default reference for E0-C/D/E unless a later explicitly labeled experiment intentionally varies reasoning.

## 5. Cross-provider deliberation law

Providers do not expose identical reasoning controls. E0 therefore records two things:

```text
DeliberationIntent = LeastDeliberativeSupported
ProviderNativeSetting = exact provider/model control actually sent
```

For OpenAI GPT-5.6 Sol the current mapping is `reasoning.effort = none`.

Future E0-B adapters map the same intent to the lowest supported native setting and record it exactly. `none`, `minimal`, disabled thinking and provider-specific low modes are never claimed to be computationally identical.

## 6. E0-only topology

Do not create a product Application architecture prematurely.

Approved direction for implementation if this blueprint is accepted:

```text
Ensemble.E0.Core
    deterministic fictional/causal authority; unchanged

Ensemble.E0.Experiment
    E0-only provider-neutral run envelope, policy, evidence contracts
    references Core
    no provider SDK, HTTP, filesystem or secrets

Ensemble.E0.Harness
    native Windows ARM64 host
    references Experiment + Core
    CLI, local evidence store, credential source, OpenAI adapter, HTTP/streaming
```

`Ensemble.E0.Experiment` is disposable experimental infrastructure and creates no post-E0 compatibility promise.

## 7. Reference provider transport

The first adapter uses the official OpenAI **Responses REST API** through ordinary `HttpClient` rather than making an OpenAI SDK surface part of the architecture.

Reason:

- official supported provider interface;
- exact request body can be frozen and hashed before disclosure;
- provider-library churn does not become E0 architecture;
- no provider package enters Core or Experiment.

Reference request properties:

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

Do not send `previous_response_id` or a provider Conversation. Every invocation is isolated and receives its complete bounded input from Ensemble.

Do not set `temperature`, `top_p`, tool choice, web/file/computer/code/MCP tools, or hidden provider conversation state. Provider defaults are recorded as omitted settings rather than silently described as Ensemble policy.

Structured Outputs use strict JSON Schema for both Performer Candidate and State Interpreter proposal transport.

No reasoning summary, encrypted reasoning content, chain-of-thought or hidden reasoning is requested or retained.

## 8. Prompt contracts

Implementation must create exactly two versioned E0 prompt contracts:

```text
PerformerPromptContract
StateInterpreterPromptContract
```

Their exact UTF-8 bytes and SHA-256 hashes are frozen in the run manifest before the first contributing run. Prompt wording cannot change inside a comparison batch.

The Performer prompt may disclose only the exact Character-bounded rendered Context plus the output contract needed to portray that Character.

The State Interpreter prompt may disclose only:

- that same bounded source Context;
- the exact Candidate Performance/control being interpreted;
- the E0 mutation-proposal schema and non-authoritative role instructions.

It does not receive the full Production repository, denied information, other Characters' private state, State Authority snapshot/locks, creator-only provenance, or credentials.

## 9. Exact disclosure attribution

Before every provider request, the Harness must have an immutable prepared-attempt record containing at least:

```text
RunId
attempt identity
role
CharacterId when Performer
turn ordinal
provider
requested model
reasoning setting
ContextPacketId
StructuredContextHash
RenderedContextHash
prompt-contract version/hash
response-schema version/hash
exact request-body SHA-256
max_output_tokens
service tier
```

The exact request body is written locally before network execution. Authentication headers/credentials are never part of evidence.

The returned provider response ID, returned model identifier/version when available, service tier, status, usage and terminal metadata are coupled to that exact prepared attempt.

A model/version identity change inside a frozen comparison batch fails closed rather than being silently accepted as the same experimental condition.

## 10. Run identity and deterministic derived IDs

Blueprint 0.1 already requires `RunId`; Core already owns the strong `RunId` type.

The run manifest supplies one initialized RunId. Derived fictional/causal IDs are deterministic and clock/randomness-free:

```text
TakeId   = {RunId}:TAKE:{acceptedTurnOrdinal}
CommitId = {RunId}:COMMIT:{acceptedTurnOrdinal}
RecordId = {RunId}:RECORD:{acceptedTurnOrdinal}:{mutationIndex}
```

Formatting uses fixed zero-padded decimal ordinals and must remain within existing CanonicalId limits.

Provider-attempt identity is E0 experimental metadata, not a new Core strong ID:

```text
{RunId}:ATTEMPT:{role}:{turnOrdinal}:{attemptOrdinal}
```

The primary E0-A reference allows exactly one attempt per probabilistic role invocation, so `attemptOrdinal = 1`.

## 11. Reference run length

One primary E0-A run has a hard experimental cap of **12 accepted Turns**.

Rationale:

- bounded enough for deterministic cost authority;
- long enough for multiple causal/opportunity cycles across the three-Character roster;
- does not pretend to resolve ODR-13 Scene-ending semantics.

After the twelfth accepted commit, the Harness still performs explicit Patch 0016 Opportunity establishment so the retained final state is synchronized and opportunity-bearing. It then terminates the experiment as `AcceptedTurnCapReached`.

This is an experimental stop, not a fictional declaration that the Scene is dramatically complete.

## 12. Attempt, timeout and retry policy

Primary E0-A policy:

```text
attempts per role invocation  1
automatic retries             0
attempt timeout               300 seconds
```

Timeout, refusal, provider error, malformed/incomplete output, transport error, cancellation or schema/parser failure cannot trigger an automatic replacement attempt in the reference condition.

They end the run with a typed non-fictional experimental outcome and remain evidence. E0-F may later introduce purpose-built retry/failure variants.

Director/user cancellation is honored through an external cancellation token at the Harness edge. Cancellation never mutates Production or invents a Performance.

## 13. Deterministic spend gate

Reference run maximum **estimated model-token spend**:

```text
USD 5.00
```

The batch manifest freezes the provider token rate card used for deterministic estimation.

Before each model request:

1. prepare the exact request;
2. use the provider's input-token-count endpoint for that exact request when supported;
3. calculate the next-call worst-case token charge from exact input count + `max_output_tokens = 4096` and the frozen rate card;
4. add it to already incurred estimated model-token cost;
5. if the result exceeds USD 5.00, do not issue the inference call.

Budget refusal terminates the run as non-fictional `BudgetExceeded` evidence.

After each completed response, replace the reserved upper-bound contribution with estimated actual token cost from provider usage, including cached/reasoning token classes when applicable. Provider invoice/billing remains external financial authority; Ensemble records its deterministic estimate and the exact rate assumptions.

Token-count/preflight API activity is recorded separately and is never treated as a Performer/Interpreter attempt or fictional event.

## 14. Primary Integrity policy

For E0-A reference portrayal, `DeterministicE0TurnOrchestrator.EvaluateIntegrity` receives an initialized empty semantic-concern set.

This means:

- Patch 0008 deterministic structural/source invariants remain live;
- no hidden second model rewrites or judges a Candidate before acceptance;
- semantic hard-gate failures remain visible to the post-run experimental evaluator and invalidate the run if found;
- E0-F may later introduce explicit semantic-assessor/failure conditions.

If deterministic Integrity returns `RequestAnotherTake`, the no-retry reference policy terminates the run. No Take or commit is created.

## 15. Reference State Authority policy

The autonomous reference run must not pause for hidden human review and must not weaken Patch 0010's mandatory-review floor.

Create one fixed E0-A policy containing every currently policy-eligible domain:

```text
UnresolvedProposition
CharacterBelief
CharacterSuspicion
CharacterGoal
CharacterCircumstance
CharacterClaim
Pressure
```

Patch 0010 still decides transition-specific eligibility and hard rejections.

When State Authority returns `ReviewRequired`, the run driver supplies deterministic **Reject** review choices for every unresolved review-required mutation and calls the existing Patch 0018 review-resolution transition.

Therefore:

- mandatory-review proposals are never auto-approved;
- no creator/human decision is forged;
- the Performance may still become an Accepted Take under Patch 0011 reference policy;
- rejected mutation proposals and reasons remain provenance;
- accepted historical texture can advance even when no durable consequence is approved.

The evidence must label these choices as `E0AReferenceDeterministicReject`, not creator review.

## 16. Provider result mapping

### Performer

```text
completed structured response
 -> exact raw output evidence
 -> PerformerCandidateContract.ParseJson(current Context, bytes)
 -> DeterministicE0PerformerAttemptBoundary.BindCandidate
 -> Turn GateAttempt
```

Refusal/timeout/transport/provider failure/incomplete response/invalid structured output maps to Patch 0017 `TechnicalFailure`; explicit cancellation maps to `Cancelled`. Raw or partial output remains diagnostics only.

### State Interpreter

```text
ReadyForInterpretation
 -> bounded Interpreter request
 -> completed structured response
 -> StateInterpretationContract.ParseJson(exact InterpretationSource, bytes)
 -> Turn EvaluateAuthority
```

Any Interpreter provider/transport/refusal/cancel/timeout/incomplete/schema failure terminates the run before Take binding. No partial Interpreter result reaches State Authority.

## 17. Commit materialization

For every terminal Approved `Add` mutation, the run driver creates exactly one deterministic `E0RecordMaterialization` using the derived RecordId for that turn/mutation index.

Rejected mutations receive no materialization. Supersede/Deactivate use their existing target RecordIds and create no new RecordId.

The set is then bound through existing `E0RecordMaterializationSet` and passed to Patch 0018 `CommitAccepted`.

No provider/model chooses IDs.

## 18. Canonical primary run loop

For each accepted-turn ordinal 1..12:

```text
current opportunity-bearing Cycle state
 -> ComposeContext
 -> prepare + record Performer disclosure
 -> deterministic cost gate
 -> execute one streamed Performer attempt
 -> map through Patch 0017
 -> Turn.GateAttempt

technical/cancelled
 -> terminate run, no fiction

CandidateReady
 -> Turn.EvaluateIntegrity(empty semantic concerns)

RequestAnotherTake
 -> terminate run, no retry

ReadyForInterpretation
 -> prepare + record Interpreter disclosure
 -> deterministic cost gate
 -> execute one streamed Interpreter attempt
 -> strict ParseJson
 -> Turn.EvaluateAuthority(reference policy)
 -> if ReviewRequired: deterministically Reject every unresolved review item
 -> Turn.BindAcceptedTake(derived TakeId)
 -> derive approved-Add materializations
 -> Turn.CommitAccepted(derived CommitId)
 -> VALID POSTCOMMIT STATE
 -> Patch 0016 EstablishOpportunity
 -> adopt next synchronized opportunity-bearing state
```

No provider request occurs between accepted commit adoption and deterministic Opportunity establishment.

## 19. Experimental evidence package

Run evidence is local experimental provenance, not Production persistence.

Minimum run directory:

```text
manifest.json
attempts/<attempt-id>/request.json
attempts/<attempt-id>/stream.ndjson
attempts/<attempt-id>/terminal.json
events.ndjson
run.final.json
transcript.json
blind/transcript.json
blind/mapping.json
```

Rules:

- manifest is written before the first inference call and then immutable;
- exact provider request/stream/terminal artifacts are write-once;
- event log is append-only;
- finalization records SHA-256 for every artifact;
- credentials/auth headers are never written;
- partial/rejected/technical provider output remains diagnostics and never enters accepted Performance history;
- no provider hidden reasoning/chain-of-thought is requested or stored;
- deleting diagnostics must not be described as reconstructing Production authority.

The evidence records all Blueprint 0.1 provenance fields including fixture identity/hash, reference configuration/variant, access/context, provider/model/settings, outputs, typed control, Director/Integrity/Interpreter/Authority, Take/commit, latency, usage/cost estimate, failures/cancellations and blind mapping.

## 20. Blind transcript

The blind package hides:

- variant label;
- provider/model;
- reasoning setting;
- run IDs and hashes;
- implementation metadata.

Speaker identity is replaced by stable neutral labels sufficient to follow recurring speakers. The private mapping retains Character identity and variant/provider information.

No evaluator may change Production/run history. Hard-gate and experiential judgments are evidence layered after the immutable run.

## 21. Reference completion / contribution rule

A primary E0-A run is a **contributing completed run** only if:

1. the manifest was frozen before the first provider request;
2. the same requested/returned provider-model identity remained valid for the batch;
3. all 12 Turns reached Accepted Take + atomic commit + explicit next Opportunity;
4. no technical/cancel/budget/integrity terminal occurred;
5. evidence finalization succeeded;
6. subsequent hard-integrity evaluation finds no Blueprint hard-gate failure.

A failed/noncontributing run is never deleted or silently rerun under the same RunId. A newly authorized run receives a new RunId and remains separately attributable.

## 22. Reasoning characterization rule

After one reference envelope is operational, low/medium/high characterization runs reuse the exact same frozen run plan except `ProviderNativeReasoningSetting`.

The same `4096` max-output ceiling and USD 5.00 estimated model-token ceiling remain fixed. If higher reasoning consumes enough reasoning tokens to increase cost, truncate, or time out, that is part of the resource/behavior difference under the matched envelope rather than something the Harness silently compensates for.

These characterization variants do not replace the `none` reference configuration for later architecture-isolating controls.

## 23. Credentials

The E0-A Harness must never persist a provider secret in source, fixture, Production, manifest, diagnostics or run evidence.

The first OpenAI adapter may consume `OPENAI_API_KEY` only as process-injected secret material. Absence fails before the first provider request. The value is never echoed, hashed into evidence, or passed below the Harness provider edge.

This is an experimental credential mechanism, not the post-E0 product secret-store architecture.

## 24. Tests before real provider spend

Implementation must prove with deterministic fake adapters before a real call:

- Core source remains unchanged;
- exact same-model profile is applied to all Performer/Interpreter roles;
- reasoning `none` reference + separate low/medium/high mapping;
- exact Context/request association and request hashes;
- stale result rejection;
- no retries in reference policy;
- timeout/cancel/refusal/invalid output never enters fiction;
- cost gate refuses before network execution;
- State Authority mandatory-review items are rejected, never auto-approved;
- deterministic Take/Commit/RecordId derivation;
- 12 accepted Turns preserve synchronized Cycle/Turn progression;
- partial streams remain diagnostic only;
- evidence is append-only/write-once and secret-free;
- blind package omits experimental labels/provider identity.

Native Windows ARM64 compile/tests/Harness smoke remain required before any claim of implementation readiness.

A real provider smoke/run is a separate external-runtime/spend authority step after implementation is approved and machine validation passes.

## 25. Explicit exclusions

No Core redesign; no product Application architecture; no durable Production persistence; no provider conversation memory; no automatic retry/backoff; no understudy; no mixed-model E0-B; no E0-E playwright implementation; no E0-F injection implementation; no semantic Integrity model; no Context summarization/retrieval; no Scene-ending semantics; no ODR-12/13/30/32 resolution; no WinUI; no Windows AI/NPU; no MSIX/WACK/Store.

No provider/model/response can authorize spend, retries, state mutation, Take disposition, commit or Opportunity.

## 26. Approval boundary

Approval of this proposal would freeze the E0-A reference run envelope, including:

- OpenAI `gpt-5.6-sol` with `reasoning.effort = none` as the normative reference;
- same model/profile for all three Performers and State Interpreter;
- reasoning low/medium/high as separately labeled characterization variants;
- 12 accepted-Turn cap;
- one attempt, zero retries, 300-second attempt timeout;
- 4096 max output tokens;
- USD 5.00 deterministic estimated model-token ceiling;
- stateless/no-tools/strict-structured Responses API use;
- deterministic reference State Authority policy and reject-only resolution of mandatory review;
- deterministic run-local Take/Commit/Record IDs;
- local immutable experimental evidence and blind package;
- E0-only Experiment assembly + Harness provider edge; Core unchanged.

Approval would **not** authorize sending a provider request, using credentials, or spending money. Implementation would proceed fake-first and return to native ARM64 validation before the first real provider call.
