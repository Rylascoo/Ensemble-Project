# E0-A State Interpreter Semantic-Output Alignment — Implementation Audit

Date: 2026-09-10

Status: **PASS — NARROW PROMPT/CONTRACT ALIGNMENT; DETERMINISTIC ACCEPTANCE LAW UNCHANGED**

## Scope and trigger

Exact source candidate: `e052ef4cdaf8ac7291971f8c42d121e1ee6eecf0`.
Base: `5cd64307471a4095d71e5e4467386b866004e375`.
Architecture amendment: `docs/blueprint/E0A_STATE_INTERPRETER_SEMANTIC_OUTPUT_ALIGNMENT_AMENDMENT.md`.
Triggering evidence: `docs/evidence/E0A_Q_E0A_03_G35L_RUN03_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`.

Run 03 reached six committed Turns and then terminated `InvalidOutput` after Gemini returned a provider-successful Interpreter payload containing `domain=characterGoal`, `subjectCharacterId=VOSS`, and `targetCharacterId=WREN`. The provider JSON Schema allowed that field combination, while `StateInterpretationContract.BuildMutableCharacterMutation` correctly requires `targetCharacterId=null` for mutable Character domains.

The failure therefore exposed a guidance/semantic-contract mismatch: the deterministic parser already enforced cross-field mutation law, but the Interpreter instruction did not disclose that law to the probabilistic model.

## Correction

Only `src/Ensemble.E0.Harness/Run/E0APromptContracts.cs` changes executable behavior. `InterpreterInstructions` now states the already-existing semantic requirements for global, Character, Relationship, append-only, CharacterClaim, add/supersede/deactivate, supporting-record, and duplicate-mutation shapes.

`tests/Ensemble.E0.Harness.Tests/E0ASpendAndRequestTests.cs` adds one regression test requiring those deterministic rules to remain exposed in the Interpreter instruction.

## Recursive semantic audit

The correction was checked against the actual parser branches rather than only the observed Run 03 failure:

- global domains (`worldState`, `sceneState`, `unresolvedProposition`, `pressure`) require null subject/target IDs;
- Character domains require a roster subject and null target;
- Relationship alone requires a non-null, distinct roster target;
- `characterKnowledge`, `characterMemory`, and `characterClaim` remain add-only;
- `characterClaim` remains bound to the source/Candidate Character;
- add requires null `existingRecordId` and non-empty text;
- supersede requires non-null `existingRecordId` and non-empty text;
- deactivate requires non-null `existingRecordId` and null text;
- duplicate supporting Record IDs and exact duplicate semantic mutations remain rejected.

No parser acceptance criterion was relaxed. The provider JSON Schema was deliberately left unchanged because its shared mutation object cannot encode all domain-dependent field relationships through the currently admitted GenerateContent schema surface without redesigning the frozen wire contract.

## Negative-space audit

No change was made to Core, Fixture content/canonicalization, Performer or Integrity prompts, provider/model/profile selection, structured-output transport encoding, response schema, rate/RPD/TPM discipline, token accounting, spend ceiling, retry/fallback law, evidence sealing, state authority, causal commit, accepted-turn semantics, or experiment order.

Run 03 remains immutable/noncontributing and is not repaired retroactively. No provider request was made as part of this correction or its native validation.

## Validation disposition

Pre-commit regression passed Core 622/622 and Harness 135/135 under .NET SDK 9.0.317. Exact-candidate native Windows ARM64 validation is recorded separately in `docs/evidence/E0A_STATE_INTERPRETER_SEMANTIC_OUTPUT_ALIGNMENT_NATIVE_ARM64_VALIDATION_2026_09_10.md`.

Recursive audit result: **PASS**. The change is the minimum deterministic guidance repair justified by Run 03 while preserving all existing acceptance and authority boundaries.
