# Ensemble Current State

Updated: 2026-09-10

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; product/lane authority: `docs/PROJECT_AUTHORITY.md`; orchestration: `docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0-A Experimental Harness — Phase B**. Q-E0A-01/02/04 **DONE**. Q-E0A-03 **ACTIVE — RUN 02 AUTHORIZED / UNCONSUMED; CREDENTIAL SECURITY GATE BLOCKED**.

Run 01 `E0A-Q03-G35L-20260909-01` passed corrected `countTokens`, reached generation, then terminated Gemini HTTP 400 with zero accepted turns; its authorization is **CONSUMED**. Run 02 `E0A-Q03-G35L-20260910-02` is now explicitly authorized for exactly one `CREATIVE-MINIMAL / GEMINI-3.5-FLASH-LITE-MINIMAL` full-reference attempt on the current validated executable. No retry/fallback/probe/alternate provider/model is authorized.

## Validation
Current promoted native authority: `7868e5cb12a27260e288d95c248d6f846cf37701`; tag `validation/e0a-gemini-generation-error-diagnostic-native-arm64`, tag object `595fef66a79ac2939f439748fed096094b445cf1`. Native Windows ARM64 Core **622/622**, Harness **134/134**, fresh build/smokes/credentialless gates PASS.

## Evidence boundary
Run 01 analysis: `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`. Run 02 authorization/current activation evidence: `docs/evidence/E0A_Q_E0A_03_G35L_RUN02_DIRECTOR_AUTHORIZATION_2026_09_10.md`. Current Google key policy requires the Run 02 credential gate to fail closed until a Director-local Gemini key is verified as **Key Type: Auth** and supplied only to the launch process.

## Continuity
Q-E0E-PREP remains DONE; Q-E0E-RUN remains blocked through E0-A→D. Design authority remains separate. Administrator C0/C1/C2 are DONE; C3 deterministic Skills remains the next earned Administrator gate and does not change Engineering/provider authority.

## Next
Run 02 is **AUTHORIZED / UNCONSUMED**. All non-secret activation gates pass. Provider traffic remains zero because no `GEMINI_API_KEY` is presently available and Auth-key type cannot be verified from repository state. After the Director supplies a process-local AI Studio **Auth** key on the Windows ARM64 host, execute exactly `E0A-Q03-G35L-20260910-02` once and preserve its terminal evidence. Do not persist or paste the key and do not retry any terminal provider result.