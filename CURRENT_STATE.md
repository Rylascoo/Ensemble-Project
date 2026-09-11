# Ensemble Current State

Updated: 2026-09-10

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0-A Experimental Harness - Phase B**. Q-E0A-01/02/04 **DONE**. Q-E0A-03 **ACTIVE - RUN 05 TERMINAL/NONCONTRIBUTING; TECHNICAL-FAILURE DIAGNOSTIC HARDENING IMPLEMENTED; NATIVE VALIDATION PENDING**.

Runs 03/04/05 are immutable/noncontributing and may never be replayed. Run 05 `E0A-Q03-G35L-20260910-05` used native-validated executable `29b62a2e778d93c6727b555f58f8d22aa18665a1` and terminated `TechnicalFailure` at the first Interpreter generation with `0` accepted turns, final Opportunity `VOSS`, shadow estimate USD `0.16775400`, and unknown final-call usage.

## Run 05 evidence
Performer and Integrity succeeded; Performer emitted canonical `MARLOWE`/`WREN`, proving the prior control-ID correction. Interpreter `countTokens` succeeded, then generation failed before any evidence-eligible stream event. Sealed evidence is clean: `docs/evidence/E0A_Q_E0A_03_G35L_RUN05_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`.

## Validation
Current promoted native authority remains `29b62a2e778d93c6727b555f58f8d22aa18665a1`, tag `validation/e0a-role-control-identity-alignment-native-arm64`; Core **622/622**, Harness **137/137**, build/smokes/credentialless/repository gates PASS. Merge/document commits do not inherit native authority.

The successor `docs/blueprint/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_AMENDMENT.md` changes only fixed fail-closed exception-class diagnostics; provider request semantics, parsers/Core, fixture, retry/fallback, rate/spend, and accepted-turn law are unchanged. Exact successor native validation is still pending.

## Continuity
Provider traffic is **zero**. Q-E0E-PREP remains DONE; Q-E0E-RUN remains blocked through E0-A-D. Design authority is separate. Administrator C0-C8 are DONE; C9 is its separate next gate and creates no Engineering/provider/validation authority.

## Next
Complete recursive audit and exact-checkout native Windows ARM64 validation of the diagnostic hardening, annotate/tag if clean, then integrate through hosted CI. Do not preregister or execute a fresh 3.5 run until integration and post-merge Validation are green. Never replay Run 05 or start 3.1/2.5 first.
