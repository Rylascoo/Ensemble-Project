# E0-A Phase B — Reference Run Envelope Implementation Handoff

Status: **IMPLEMENTATION IN PROGRESS — STATIC CONVERGENCE NEAR COMPLETE; NATIVE VALIDATION NOT YET RUN**
Date: 2026-09-05

This is a transition artifact for a fresh engineering chat. It does not supersede approved architecture or advance `CURRENT_STATE.md` before validation/promotion.

## 1. Resume authority

Repository:

`Rylascoo/Ensemble-Project`

Read `CURRENT_STATE.md` first and resolve current `main` before doing substantive work.

Approved parent `main` for this implementation:

`2b628c2d97eb39f4d96ce3e81eafefe93ba28b09`

Implementation branch:

`e0a-reference-run-envelope-implementation`

Latest implementation/audit checkpoint before this handoff document:

`74305ce3c501d5da0762bd88598d6bd83f627d14`

At that checkpoint the branch was **83 commits ahead / 0 behind** `main`. The diff contained only:

- approved E0-A Phase-B blueprint and approval evidence;
- new Harness-only run/evidence/OpenAI implementation;
- new Harness test project/tests.

No `Ensemble.E0.Core` file was changed. No product Application/persistence/UI scope was entered.

Authoritative architecture:

`docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`

Approval evidence:

`docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_APPROVAL.md`

Exact approved architecture commit:

`e1d0b4aea0f7ba29cf85e765bea33d14eab35fde`

Do not redesign Proposal 0.15 during implementation audit.

## 2. Frozen implementation envelope

Reference configuration:

```text
Provider              OpenAI official API
Model                 gpt-5.6-sol
Performer reasoning   none
Interpreter reasoning none
Integrity reasoning   high
accepted-Turn cap     12
attempts/invocation   1
automatic retries     0
attempt timeout       300 seconds
max output/role       4096
estimated run ceiling USD 5.00
```

CREATIVE-NONE is normative. LOW/MEDIUM/HIGH later vary Performer + Interpreter together; Integrity remains high.

No tools, provider conversation state, hidden reasoning, product persistence, Scene-ending semantics, E0-B..G implementation, Context optimization, WinUI, Windows AI/NPU, MSIX/WACK/Store, or Core redesign.

Provider execution, credentials and spend remain **NOT AUTHORIZED**. Approval is fake-first only.

## 3. Implemented surface

Harness implementation added under:

```text
src/Ensemble.E0.Harness/Run/
src/Ensemble.E0.Harness/Evidence/
src/Ensemble.E0.Harness/OpenAI/
```

Key responsibilities now present:

- deterministic run envelope/profiles and role identities;
- deterministic attempt/Take/Commit/Record IDs;
- bounded Performer, Integrity and Interpreter request construction;
- configured-path prepared attempts and closed receipts;
- strict request/receipt association before semantic consumption;
- one-attempt/zero-retry spend reservation and reconciliation;
- 300-second role boundary covering token preflight + inference;
- model-identity continuity checking;
- Integrity concern binding through the existing Core Turn path;
- reference State Authority policy plus deterministic reject of unresolved mandatory-review mutations;
- Add + Supersede materialization with Harness-derived IDs;
- accepted commit followed immediately by synchronized Opportunity establishment;
- local immutable run evidence, blind transcript/mapping and two-stage runtime/evaluation seals;
- official OpenAI Responses REST adapter with strict structured output and streaming provisional-output evidence.

New Harness test project:

`tests/Ensemble.E0.Harness.Tests`

## 4. Material corrections already made during recursive audit

Do not regress these fixes:

1. **Usage-overrun accounting** — if provider-reported usage exceeds the preflight reservation, preserve the receipt/observed cost, consume no semantic output, and terminate/seal as technical failure rather than throwing before provenance is written.
2. **Failed-run sealing** — truthful observed spend above USD 5 may be recorded in a failed run; sealing rejects negative spend, not evidence merely because observed provider usage exceeded the deterministic preauthorization ceiling.
3. **Timeout ownership** — the 300-second role limit covers token preflight and inference. Provider timeout releases spend reservation exactly once.
4. **Cancellation boundary** — cancellation is checked before authority-bearing work/commit. Once accepted commit begins, commit -> Opportunity establishment remains uninterrupted by external work.
5. **Manifest reproducibility** — manifest freezes approved blueprint identity/commit, reference configuration, fixture version, pricing, role profiles, host and executable commit before inference.
6. **Hard-gate anti-post-hoc rule** — manifest freezes not just a checklist version but the exact frozen E0 hard-gate checklist contents; post-run evaluation must match that pre-run contract.
7. **State Authority provenance** — manifest/evidence records the deterministic reference authority policy; authority decisions/rejection reasons are explicit evidence.
8. **Semantic provenance** — Candidate, Integrity disposition, Interpreter proposal identity, State Authority decisions, spend reservation/reconciliation and provider latency are explicit checkpoints rather than reconstructed only from raw bytes.
9. **Executable identity** — executable commit provenance is validated as a real lowercase 40-hex Git object identity in the test surface.
10. **Blind/evidence separation** — partial/rejected/technical/provider diagnostics never become accepted Character-visible transcript material.

Latest two commits after the prior cancellation checkpoint touched only `E0AEvidenceStore.cs` and `E0AEvidenceTests.cs` to strengthen the exact frozen hard-gate checklist contract. No scope expansion occurred.

## 5. Current audit status

A recursive correctness/consistency/authority/privacy/dependency/scope/test/simplicity/hygiene/ARM64/evidence/improvement audit was underway.

The last completed scope check at the preceding checkpoint found:

- branch ahead of `main`, not behind;
- no Core modifications;
- diff limited to approved blueprint/approval + Harness implementation/tests;
- approved blueprint remains under the 15KB cap (`14969` bytes).

The conversation ended before one final complete **zero-material-correction pass** and before native compile/runtime validation. Therefore:

- do **not** claim implementation audit complete yet;
- do **not** claim compiler/test/runtime validation yet;
- do **not** open a real provider request;
- do **not** use credentials or incur spend.

## 6. Fresh-chat first actions

1. Read `CURRENT_STATE.md` first and resolve current `main`.
2. Read this handoff, the approved blueprint and approval evidence.
3. Resolve current `e0a-reference-run-envelope-implementation` head. Confirm every descendant after `74305ce3c501d5da0762bd88598d6bd83f627d14` is handoff/docs-only unless explicitly audited.
4. Compare branch to `main`; confirm Core remains untouched and scope remains Proposal 0.15 only.
5. Resume the interrupted recursive zero-change audit over the exact current branch. Search actively for correctness, privacy/disclosure, provenance, cancellation, spend, stale-association, evidence-sealing, structured-output/wire, test-semantic and simplification defects.
6. Reverify only volatile OpenAI wire assumptions against current official documentation before declaring wire audit complete. Do not redesign the approved envelope because a newer model exists.
7. If the audit produces no material correction, prepare compact pending-validation evidence/oracle only if consistent with current project evidence caps.
8. Then ask the Director to run native Windows ARM64 validation against one exact resolved commit. Validation must include existing Core tests, new Harness tests, Harness ARM64 build/smoke and fixtures. Record results explicitly as **Director-machine-sourced**.
9. Only after successful native validation may the implementation be considered for PR/promotion. Real provider execution remains a separate explicit Director credential/spend gate.

## 7. Validation authority

The previous H1 executable baseline was Director-machine validated at 622/622 Core tests. This E0-A implementation adds a separate Harness test project, so the new authoritative test total must be obtained from the Director's actual Windows ARM64 run; do not predict or invent it.

Do not substitute Linux/container or assistant-environment results for the Director-machine validation gate.

## 8. Project discipline

Preserve the five engineering properties:

1. Character continuity independent of Performer/model.
2. Deterministic bounded perspective: Access Control before Context Composition.
3. Agency without hidden authorship: Director offers opportunity; Performer proposes; deterministic authority commits.
4. Causal persistence: accepted Performance + authoritative consequence = attributable causal unit.
5. Creator sovereignty: probabilistic systems suggest/interpret; deterministic authority/creator owns Production truth.

Continue patch-first. Recursively audit until one complete pass finds no material correction or worthwhile improvement. Do not ask the Director to restate state available from GitHub/Drive.

Technical reply close convention:

`Next submission: GPT-5.6 · High`
