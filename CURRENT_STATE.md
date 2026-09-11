# Ensemble Current State

Updated: 2026-09-10

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; product/lane authority: `docs/PROJECT_AUTHORITY.md`; orchestration: `docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0-A Experimental Harness - Phase B**. Q-E0A-01/02/04 **DONE**. Q-E0A-03 **ACTIVE - RUN 05 PREEXECUTION GATES PASS; ONE EXECUTION ELIGIBLE ONLY AFTER ACTIVATION PACKAGE INTEGRATION + POST-MERGE VALIDATION**.

Runs 03/04 are immutable/noncontributing and may never be replayed. Run 05 is preregistered as `E0A-Q03-G35L-20260910-05` against exact native-validated executable `29b62a2e778d93c6727b555f58f8d22aa18665a1`, profile `GEMINI-3.5-FLASH-LITE-MINIMAL / CREATIVE-MINIMAL`, canonical Missing Raft fixture, 12-turn cap, zero retries, and no fallback/substitution.

## Validation
Current promoted native authority is exactly `29b62a2e778d93c6727b555f58f8d22aa18665a1`; tag `validation/e0a-role-control-identity-alignment-native-arm64`; tag object `f6080e37df44737b3053fa8237b95bdc2cc0508c`. Native Windows ARM64 Core **622/622**, Harness **137/137**, build/smokes/credentialless/repository gates PASS; provider network during validation NONE.

The repair was integrated through PR #69 to `main` `89e5200052e8379caf308833568eb3f832925f37`; post-merge Validation run `34556439871` passed. Merge/document commits do not inherit native authority.

## Run 05 activation
Activation record: `docs/evidence/E0A_Q_E0A_03_G35L_RUN05_PREEXECUTION_ACTIVATION_2026_09_10.md`. Evidence root `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0A-Q03-G35L-20260910-05` did not exist at activation. Public provider facts were rechecked with no contradiction; the reusable 3.5 quota snapshot remains 15 RPM / 250,000 input TPM / 500 RPD; executable freshness remains valid through 2026-09-14. Protected credential decryptability, exact detached checkout, fresh Release build, and fixture smoke pass. Preexecution consumption: **0**.

## Continuity
Q-E0E-PREP remains DONE; Q-E0E-RUN remains blocked through E0-A-D. Design authority is separate. Administrator C0-C8 DONE; C9 is its separate next gate and creates no Engineering/provider/validation authority.

## Next
Use **zero provider traffic** until this Run 05 activation package is integrated and post-merge main Validation is green. Then execute Run 05 exactly once under standing authority; its own `countTokens` is the first provider operation. Preserve any terminal result, update usage, and never retry/replay/fallback or start 3.1/2.5 first.
