# Ensemble Current State

Updated: 2026-09-10

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0-A Experimental Harness - Phase B**. Q-E0A-01/02/04 **DONE**. Q-E0A-03 **ACTIVE - RUN 05 TERMINAL/NONCONTRIBUTING; TECHNICAL-FAILURE DIAGNOSTIC HARDENING NATIVE-VALIDATED; INTEGRATION PENDING**.

Runs 03/04/05 are immutable/noncontributing and may never be replayed. Run 05 `E0A-Q03-G35L-20260910-05` terminated `TechnicalFailure` at the first Interpreter generation with `0` accepted turns and unknown final-call usage. Sealed analysis: `docs/evidence/E0A_Q_E0A_03_G35L_RUN05_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`.

## Validation
Current promoted native authority is exact checkout `bb869fb1c505603612bc718f739b3f1b358e5539`; tag `validation/e0a-gemini-technical-failure-diagnostic-classification-native-arm64`; tag object `c4ff8dc4b9ebd31674208f4ba9c958d50fc975d5`. Native Windows ARM64 Core **622/622**, Harness **140/140**, fresh build/smokes/credentialless/repository gates PASS; provider network during validation NONE. Evidence: `docs/evidence/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_NATIVE_ARM64_VALIDATION_2026_09_10.md`.

The bounded correction changes only fixed fail-closed exception-class diagnostics; provider request semantics, parsers/Core, fixture, retry/fallback, rate/spend, and accepted-turn law are unchanged. Documentation/integration commits do not inherit native runtime authority.

## Continuity
Provider traffic is **zero**. Q-E0E-PREP remains DONE; Q-E0E-RUN remains blocked through E0-A-D. Design authority is separate. Administrator C0-C8 are DONE; C9 is separate and creates no Engineering/provider/validation authority.

## Next
Integrate the native-validated diagnostic package through hosted CI and require green post-merge Validation. Only then preregister a fresh 3.5 Flash-Lite run under standing authority. Never retry/replay Runs 03/04/05 or start 3.1/2.5 first.
