# Ensemble Current State

Updated: 2026-09-10

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; product/lane authority: `docs/PROJECT_AUTHORITY.md`; orchestration: `docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0-A Experimental Harness - Phase B**. Q-E0A-01/02/04 **DONE**. Q-E0A-03 **ACTIVE - RUN 04 TERMINAL/NONCONTRIBUTING; ROLE CONTROL-IDENTITY ALIGNMENT UNDER VALIDATION**.

Run 04 `E0A-Q03-G35L-20260910-04` is immutable/consumed: zero accepted turns, then deterministic Performer `InvalidOutput`. Its only generation was successful/schema-valid but emitted display-style control IDs `Marlowe`/`Wren` instead of exact roster IDs `MARLOWE`/`WREN`. It may never be replayed. Run 03 also remains immutable/noncontributing.

## Validation
Current promoted native authority remains exactly `e052ef4cdaf8ac7291971f8c42d121e1ee6eecf0`; tag `validation/e0a-state-interpreter-semantic-output-alignment-native-arm64`, tag object `a6402b32f4840ee218365dec4b285197bd72548b`; Core **622/622**, Harness **135/135**, build/smokes/credentialless/repository gates PASS, provider network NONE.

Run 04 activation was integrated through PR #66 at `0b715ee67315975ad02d5d3bcccfd998e8cbbda6` with post-merge Validation green. Administrator C6 later advanced `main` to `8e486567e8d3f43f01c065a083f3fa1f9ba724e9` without changing Engineering/provider authority.

## Evidence / correction boundary
Run 04 terminal analysis: `docs/evidence/E0A_Q_E0A_03_G35L_RUN04_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`. Correction: `docs/blueprint/E0A_ROLE_CONTROL_IDENTITY_ALIGNMENT_AMENDMENT.md`. Consumption ledger: `docs/evidence/GEMINI_API_USAGE_LEDGER.md`.

Run 04 consumed exactly **2** Gemini HTTP requests: one 649-token `countTokens` preflight plus one successful Performer generation (649 input / 130 output / 0 reasoning / 0 cached), shadow USD `0.00051970`. Evidence seal independently verifies with zero artifact-hash mismatches.

The narrow repair exposes exact `rosterCharacterIds` in bounded Performer/Interpreter context and requires exact case-sensitive ID reuse. Parsers, Core authority, Fixture, schemas, provider profile, retry/fallback law, and experiment order remain unchanged. Working-copy native regressions: Core **622/622**, Harness **137/137**; these do not yet promote validation authority.

## Continuity
Q-E0E-PREP remains DONE; Q-E0E-RUN remains blocked through E0-A-D. Design authority remains separate. Administrator C0-C6 DONE; C7 remains its separate next gate.

## Next
Use **zero provider traffic**. Recursively audit and commit the role control-identity correction, perform fresh clean native Windows ARM64 validation and annotated tagging, then integrate through hosted CI. Only after that may a fresh 3.5 Flash-Lite RunId be activated. Never replay Run 03/04 or start 3.1/2.5 first.