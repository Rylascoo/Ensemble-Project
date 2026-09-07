# E0-A Post-Audit Hardening — Closed Findings Continuity

Status: **AUDIT CLOSED — ZERO-NEW-FINDING PASS ACHIEVED; REMAINING HARDENING AUTHORIZED**

Date: 2026-09-06

## Purpose

This record preserves the complete closed-audit correction set for E0-A Phase B after Patch Group 1. It is continuity evidence, not a new architecture blueprint and not a reinterpretation of Proposal 0.15.

Repository: `Rylascoo/Ensemble-Project`

Hardening branch: `e0a-phase-b-post-audit-hardening`

Audited source HEAD / `main` baseline:

`f9cb1a79ad7923b640ff1a97c46d8fd6ab9dac25`

The comprehensive post-implementation Astra/Codex audit covered 143 relevant source/test/configuration/fixture/state files plus applicable authority documents. The recursive audit reached **AUDIT CLOSED** and then achieved a **zero-new-finding pass**.

## Patch Group 1 disposition

Patch Group 1 is already implemented and recursively static-audited:

- **E-01 — Integrity failure terminal sealing**
- **E-02 — streaming cancellation boundary**

Exact Patch Group 1 executable/test checkpoint:

`171881c1247e1c466fec6abd3e92335a055eb4f2`

Implementation evidence:

`docs/evidence/E0A_POST_AUDIT_HARDENING_PATCH_GROUP_1_IMPLEMENTATION.md`

Patch Group 1 has not yet received the grouped native Windows ARM64 validation planned after all authorized hardening groups are complete.

## Remaining closed-audit findings

### E-03 — P2 — Provider parsing

Location at audited snapshot:

`src/Ensemble.E0.Harness/OpenAI/OpenAIResponsesPort.cs`

Finding: wrong JSON types and malformed Unicode can escape expected failure translation across token preflight, buffered responses, streaming responses, and Integrity concern parsing.

Correction intent:

- validate JSON `ValueKind` before typed access;
- narrowly translate provider-data decoding failures;
- preserve distinction between malformed external input and unexpected programming defects;
- do not add broad `catch (Exception)` relabeling.

### E-04 — P2 — Fixture parsing

Location:

`src/Ensemble.E0.Core/Fixture/StrictJsonPreflight.cs`

Finding: an escaped unpaired-surrogate property name can bypass normal fixture-error handling.

Correction intent: translate property-name decoding failure into the established sanitized fixture-validation failure domain without weakening strict JSON validation.

### E-05 — P3 — Integrity concern contract consistency

Location:

`src/Ensemble.E0.Harness/Run/E0AIntegrityAssessment.cs`

Finding: enum parsing accepts numeric and padded strings beyond the five exact approved Integrity concern names.

Correction intent: use an exact ordinal allowlist for the five frozen concern names. Do not broaden the Integrity contract.

### E-06 — P2 — Spend / provenance

Location:

`src/Ensemble.E0.Harness/Run/E0AReferenceRunDriver.cs`

Finding: refusal/incomplete provider handling can discard available response identity and usage and then release the reservation.

Correction intent:

- preserve validated nonsemantic provider metadata and reported usage where available;
- reconcile usage/spend correctly;
- explicitly represent unavailable/unknown usage rather than fabricating it;
- no refused/incomplete semantic output may enter fiction or authority.

### E-07 — P2 — Timeout configuration

Location:

`src/Ensemble.E0.Harness/Host/E0AReferenceRunHost.cs`

Finding: default `HttpClient` timeout can terminate buffered Integrity/token-preflight work around 100 seconds despite the frozen E0-A attempt envelope being 300 seconds.

Correction intent: make HTTP transport timeout composition consistent with the explicit attempt deadline so the deterministic attempt timeout remains authoritative. Do not silently create a second shorter timeout policy.

### I-02 — P2 — Evaluation ordering

Location:

`src/Ensemble.E0.Harness/Evidence/E0AEvidenceStore.cs`

Finding: the legacy evaluation path writes hard-gate evidence before verifying runtime integrity. Failure can leave a partial write-once evaluation artifact and prevent clean retry/review.

Correction intent:

- consolidate evaluation behavior where practical;
- validate sealed runtime integrity before publishing evaluation artifacts;
- publish evaluation evidence atomically/write-once.

Independent-process reopening itself was **not** established as a missing blueprint requirement and must not be resurrected as one.

### I-03 — P2 — Evidence ownership

Location:

`src/Ensemble.E0.Harness/Evidence/E0AEvidenceStore.cs`

Finding: check-then-write behavior permits concurrent overwrite/race conditions, and separate fresh evidence roots can permit reuse of the same `RunId` without a stronger namespace claim.

Correction intent: provide exclusive evidence/run identity ownership within the applicable evidence namespace and create authoritative artifacts without overwrite races. Do not invent global/cloud persistence or later Production persistence.

### I-04 — P2 — Seal integrity

Locations:

- `src/Ensemble.E0.Harness/Evidence/E0AEvidenceStore.cs`
- `src/Ensemble.E0.Harness/Evidence/E0AExistingEvidenceEvaluationSealer.cs`

Finding: `run.final.json` summary fields can be changed while retaining the same recorded `runtimeRoot`, after which evaluation can endorse the changed summary.

Correction intent: verify original runtime-seal identity and bind all endorsed summary claims to immutable/root-covered runtime evidence. Preserve the two-stage runtime/evaluation authority model.

### I-05 — P2 — Checkout verification

Location:

`src/Ensemble.E0.Harness/Host/E0ARepositoryCheckoutGuard.cs`

Finding: untracked-path filtering assumes repository-root paths, but Git path output depends on invocation context; launches from a subdirectory can miss material code paths.

Correction intent: perform checkout inspection from the verified repository root and use unambiguous root-relative paths.

### P-01 — P3 — Test coupling

Location:

`tests/Ensemble.E0.Core.Tests/CausalCommit/Patch0012StructuralImplementationTests.cs`

Finding: some private-name, direct IL-call, catch-layout, and declaration-order assertions reject behavior-preserving correct refactors.

Correction intent: remove only incidental private implementation coupling.

Preserve:

- behavioral tests;
- reference oracles;
- public-surface architecture assertions;
- reflection used as test plumbing;
- implementation structure explicitly frozen by an approved architecture contract.

Do not perform blanket reflection-test deletion.

### P-02 — P3 — Numeric/spend robustness

Location:

`src/Ensemble.E0.Harness/Run/E0ASpendLedger.cs`

Finding: allowed extreme prices can overflow calculations or round reservations to zero, while zero can also represent “no reservation.”

Correction intent: validate representable numeric bounds and distinguish reservation activity from monetary amount.

### P-03 — P3 — Documentation

Location:

`README.md`

Finding: README phase summary still identifies H1 as active despite recorded H1 closure and current E0-A Phase B work.

Correction intent: align the summary with authoritative current state or defer phase-state detail to `CURRENT_STATE.md`. Do not turn README into another competing state authority.

### P-04 — P3 — Evaluation parsing

Location:

`src/Ensemble.E0.Harness/Evidence/E0AExistingEvidenceEvaluationSealer.cs`

Finding: wrongly typed contract values or malformed Unicode can escape the normal CLI/evaluation failure translation.

Correction intent: validate field types and narrowly translate decoding failures before any evaluation artifact is written.

### P-05 — P3 — Spend robustness / pricing assumptions

Location:

`src/Ensemble.E0.Harness/Run/E0ASpendLedger.cs`

Finding: unexpected reported usage above the long-context threshold terminates the run but may still be estimated using standard-tier pricing assumptions.

Correction intent: either apply the relevant verified pricing tier or explicitly mark the estimate outside its valid pricing assumptions. Do not invent current provider prices. Pricing facts remain subject to the existing provider-pricing verification/staleness discipline.

## Final dependency order

With Patch Group 1 already complete, remaining implementation order is:

1. **Patch Group 2 — Provider decoding and timeout:** E-03, E-07.
2. **Patch Group 3 — Receipt provenance and spend:** E-06, P-02, P-05.
3. **Patch Group 4 — Evidence authority and evaluation:** I-02, I-03, I-04, P-04.
4. **Patch Group 5 — Parser and checkout hardening:** E-04, E-05, I-05.
5. **Patch Group 6 — Test hygiene and continuity cleanup:** P-01, P-03, followed by the final regression-gap review.

Patch Group 4 is the third remaining implementation group after Patch Group 1; the project deletion quota applies there only when the correction naturally makes genuinely superseded/redundant material obsolete. Useful material must not be deleted merely to satisfy a numeric quota.

## Implementation progress

- Patch Group 1 — E-01/E-02: **IMPLEMENTED; RECURSIVE STATIC AUDIT COMPLETE**. Executable/test checkpoint `171881c1247e1c466fec6abd3e92335a055eb4f2`.
- Patch Group 2 — E-03/E-07: **IMPLEMENTED; RECURSIVE STATIC AUDIT COMPLETE**. Executable/test checkpoint `f37cb8ec41e50d50aba027286326a10677db1040`. Evidence: `docs/evidence/E0A_POST_AUDIT_HARDENING_PATCH_GROUP_2_IMPLEMENTATION.md`.
- Patch Group 3 — E-06/P-02/P-05: **NEXT; NOT YET IMPLEMENTED IN THIS PROGRESS ENTRY**.
- Grouped native Windows ARM64 validation: **PENDING AFTER ALL AUTHORIZED GROUPS**.

## Final regression-gap obligations

Before hardening can be declared complete, ensure the test surface covers where still applicable and not already proven by an exact existing test:

- three-role failure matrix;
- failures after previously accepted Turns;
- malformed receipts;
- duplicate Integrity concerns;
- refusal/incomplete responses;
- malformed Unicode;
- cancellation boundaries;
- genuinely stalled-stream cancellation;
- host timeout composition;
- failed-response usage preservation;
- extreme pricing;
- out-of-tier reported usage;
- concurrent run creation;
- `RunId` reuse;
- runtime-summary tampering;
- evaluation failure behavior;
- full causal replay from saved run artifacts including nonempty controls and Add/Supersede/Deactivate;
- checkout verification from subdirectories;
- malformed evaluation-seal inputs.

## Validation boundary

This record makes no compiler, runtime, native Windows ARM64, package, Store, or provider-execution claim.

Real provider execution remains prohibited. Do not access or use `OPENAI_API_KEY`; do not authorize network inference or provider spend.

Do not enter E0-B through E0-G, product Application/persistence/UI, Stage/Scene-ending semantics, Context optimization, Windows AI/NPU, MSIX/WACK, Store/Partner Center, or later ODR scope.

After all hardening groups complete recursive static audit, the exact final executable/test checkout must undergo grouped native Windows ARM64 validation on the Director machine using:

`docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`

Only that later native evidence may replace the prior machine-tested executable authority or permit promotion of this hardening branch toward `main`.