# Ensemble Current State

Updated: 2026-09-10

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; product/lane authority: `docs/PROJECT_AUTHORITY.md`; orchestration: `docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0-A Experimental Harness - Phase B**. Q-E0A-01/02/04 **DONE**. Q-E0A-03 **ACTIVE - RUN 05 TERMINAL/NONCONTRIBUTING; TECHNICAL-FAILURE DIAGNOSTIC HARDENING IMPLEMENTED; NATIVE VALIDATION PENDING**.

Runs 03/04/05 are immutable/noncontributing and may never be replayed. Run 05 `E0A-Q03-G35L-20260910-05` used exact native-validated executable `29b62a2e778d93c6727b555f58f8d22aa18665a1` after activation merged to `main` `19f6fbb03b890aaa34910a1dc27bd7fad30b6655` and push Validation `34557689303` passed. It terminated `TechnicalFailure` at the first Interpreter generation with accepted turns `0`, final Opportunity `VOSS`, shadow estimate USD `0.16775400`, and unknown final-call usage.

## Run 05 evidence
Performer and Integrity generation succeeded; Performer emitted canonical `MARLOWE`/`WREN`, proving the prior control-ID correction reached its intended boundary. Interpreter `countTokens` succeeded, then generation failed before any evidence-eligible stream event. The preserved `gemini-malformed-or-transport` code cannot distinguish HTTP transport, response I/O, malformed JSON/UTF-8, or defensive overflow. Evidence: `docs/evidence/E0A_Q_E0A_03_G35L_RUN05_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`. All sealed artifact hashes independently match; credential-pattern hits `0`.

## Validation
Current promoted native authority remains exactly `29b62a2e778d93c6727b555f58f8d22aa18665a1`; tag `validation/e0a-role-control-identity-alignment-native-arm64`; tag object `f6080e37df44737b3053fa8237b95bdc2cc0508c`. Core **622/622**, Harness **137/137**, build/smokes/credentialless/repository gates PASS. Merge/document commits do not inherit native authority.

The bounded successor `docs/blueprint/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_AMENDMENT.md` changes only fixed fail-closed exception-class diagnostics; request shape, model/profile, schemas, parser/Core authority, retry/fallback, fixture, rate/spend, and accepted-turn law are unchanged. Working-copy ARM64 regressions currently pass Core **622/622**, Harness **140/140**; this is not yet promoted native validation.

## Continuity
Provider traffic is **zero**. Q-E0E-PREP remains DONE; Q-E0E-RUN remains blocked through E0-A-D. Design authority is separate. Administrator C0-C7 DONE; C8 remains separate and creates no Engineering/provider/validation authority.

## Next
Complete recursive audit and exact-checkout native Windows ARM64 validation of the diagnostic hardening, annotate/tag it if clean, then integrate through hosted CI. Do not preregister or execute a fresh 3.5 run until that integration and post-merge Validation are green. Never retry/replay Run 05 or start 3.1/2.5 first.
