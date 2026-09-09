# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` product/policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` backlog order.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 bounded provider-error diagnostic is **CLOSED — MACHINE-VALIDATED**. Contract: `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`. Audit: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_08.md`. Gemini 3 signature: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
Native executable: `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`; tag `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64`; tag object `9b17563474b25920da8615afefdfa3fcfdab884b`. Windows ARM64: Core **622/622**, Harness **130/130**, build/smokes/credentialless PASS. Later commits do not inherit native authority.

## Provider
Attempt 03: first Performer full-request `countTokens` HTTP **400**, 0 turns, $0, no generation; same-key/header/model/method simple `countTokens` succeeded.

Attempt 04 at exact `e6e7d6c8...` reached provider and terminated **TechnicalFailure**, acceptedTurns **0**, estimated shadow spend **$0.000000**, native exit **3**. Authorization is **CONSUMED**; provider authorization is **NONE**; **DO NOT RERUN**. Terminal record: `docs/evidence/E0A_GEMINI35_ATTEMPT04_TERMINAL_RESULT_2026_09_09.md`. Packet-reported evidence ZIP SHA-256: `042038F0CE16F07248ECE653A2E6545E67174CD3552CF7326A4A830AE90F76B1`. Exact failure stage/field remains unproven until ZIP audit.

Quota baseline remains **15 RPM / 250,000 input TPM / 500 RPD** until a material contrary signal. Pricing/data-use freshness and route-change/error triggers remain binding.

## Parallel E0-E
Q-E0E-PREP **CLOSED — HOSTED NON-NETWORK PREPARATION VALIDATED**. Q-E0E-RUN remains BLOCKED until E0-A -> E0-B -> E0-C -> E0-D close.

## Cross-lane
Q-DESIGN-10 DONE; Q-DESIGN-11 ACTIVE. Design workflow does not alter E0 blind scoring or engineering order.

## Next
Q-E0A-02 remains **ACTIVE only for non-network Attempt-04 evidence ingestion/audit**. Provider authorization is **NONE**. Ingest the local ZIP, independently match its SHA-256, verify evidence/provenance/privacy, identify the exact terminal stage and approved bounded diagnostic if present, then recursively audit before any source/provider decision. Handoff: `docs/handoff/E0A_GEMINI35_ATTEMPT04_POSTRUN_EVIDENCE_AUDIT_HANDOFF_2026_09_09.md`. Q-E0A-03, E0-B+, and E0-E execution remain BLOCKED.
