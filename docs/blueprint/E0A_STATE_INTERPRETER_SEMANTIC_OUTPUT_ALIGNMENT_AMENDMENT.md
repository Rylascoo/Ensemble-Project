# E0-A State Interpreter Semantic Output Alignment Amendment

Status: **FROZEN ENGINEERING CORRECTION — RUN 03 FALSIFICATION RESPONSE**

Date: 2026-09-10

Authority: subordinate to `CURRENT_STATE.md`, Blueprint 0.1, the E0-A reference envelope, the Q-E0A-03 activation/closure contract, and the existing deterministic `StateInterpretationContract`.

## 1. Trigger

Q-E0A-03 Run 03 `E0A-Q03-G35L-20260910-03` reached six accepted turns and then terminated `InvalidOutput` after the turn-7 Interpreter provider call returned successful, schema-valid JSON.

The proposal used `domain=characterGoal` with `targetCharacterId=WREN`. Existing deterministic law requires `targetCharacterId=null` for every mutable-character mutation. The current provider schema permits `string|null` because it is shared across all mutation shapes, while the Interpreter instruction never states the cross-field/domain rule.

Evidence: `docs/evidence/E0A_Q_E0A_03_G35L_RUN03_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`.

## 2. Falsified assumption

The assumption that the generic Interpreter instruction plus the shared structural JSON Schema sufficiently communicates the deterministic mutation contract is falsified.

The parser is not defective for rejecting Run 03. Its fail-closed semantic validation is the authority boundary and must remain unchanged.

## 3. Correction

Change only `E0APromptContracts.InterpreterInstructions` so the model receives the already-existing mutation-shape rules before generation. Keep the structural response schema unchanged.

The instruction must preserve the existing anti-injection/proposal/claim-to-fact boundaries and additionally communicate these exact semantic constraints:

- `worldState`, `sceneState`, `unresolvedProposition`, and `pressure`: `subjectCharacterId=null`, `targetCharacterId=null`;
- all Character domains: non-null roster `subjectCharacterId`, `targetCharacterId=null`;
- `relationship`: non-null distinct roster subject and target; this is the only shape permitting non-null `targetCharacterId`;
- `characterKnowledge`, `characterMemory`, and `characterClaim`: `add` only;
- `characterClaim.subjectCharacterId`: must equal the supplied Candidate subject Character;
- `add`: `existingRecordId=null`, non-empty `text`;
- `supersede`: non-null `existingRecordId`, non-empty `text`;
- `deactivate`: non-null `existingRecordId`, `text=null`;
- `supportingRecordIds`: no duplicates;
- no exact duplicate semantic mutations.

Application validation remains authoritative. The prompt does not convert invalid output into accepted state and creates no repair/retry behavior.

## 4. Scope lock

This correction must not change Core, `StateInterpretationContract`, state domains, operation semantics, authority policy, causal commit law, Fixture content/hash, Performer instructions, Integrity instructions, response-schema structure, Gemini model/profile, thinking controls, output ceilings, token accounting, rate/spend law, evidence law, retry/fallback law, or accepted-turn caps.

The Interpreter prompt hash and outbound request bytes will change. That is intentional and makes the prior native executable ineligible for a new reference candidate after this correction.

## 5. Validation

Before any further full-reference provider traffic, the corrected source candidate must pass targeted request/prompt tests, complete Core and Harness suites, repository guards, and fresh native Windows ARM64 validation under the normal validation constitution. An annotated validation tag must bind to the exact machine-tested source commit.
