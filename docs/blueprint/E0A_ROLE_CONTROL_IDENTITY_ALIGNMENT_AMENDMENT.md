# E0-A Role Control Identity Alignment Amendment

Status: **FROZEN ENGINEERING CORRECTION — RUN 04 FALSIFICATION RESPONSE**

Date: 2026-09-10

Authority: subordinate to `CURRENT_STATE.md`, Blueprint 0.1, the E0-A reference envelope, the Q-E0A-03 activation/closure contract, and the existing deterministic Performer/Interpreter contracts.

## 1. Trigger

Q-E0A-03 Run 04 `E0A-Q03-G35L-20260910-04` terminated `InvalidOutput` before Integrity after the first Performer provider call returned successful, schema-valid JSON.

Typed control used display-style `Marlowe` and `Wren`. Existing deterministic roster identity is exact and case-sensitive: `MARLOWE`, `VOSS`, `WREN`. The parser correctly rejected the candidate as outside the roster.

Evidence: `docs/evidence/E0A_Q_E0A_03_G35L_RUN04_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`.

## 2. Falsified assumption

The assumption that rendered story names plus the subject canonical ID sufficiently communicate the exact Character-ID vocabulary required by typed provider output is falsified.

The deterministic parser is not defective. Normalizing or guessing a provider-emitted identity would weaken provenance and authority.

## 3. Correction

Add `rosterCharacterIds` to `E0APromptContracts.ContextData`, derived only from `ContextPacket.Roster` canonical `CharacterId` values. This bounded structured field is supplied to Performer and Interpreter requests.

Performer instructions must require every non-null `addressedCharacterIds` entry and `nominatedCharacterId` to be copied exactly, case-sensitively, from `context.rosterCharacterIds`; display names or changed capitalization are invalid. Existing no-self-address/no-self-nomination and duplicate-ID laws remain unchanged.

Interpreter instructions must likewise require every non-null `subjectCharacterId` and `targetCharacterId` to use an exact case-sensitive ID from `context.rosterCharacterIds`, in addition to the already-frozen mutation-shape law.

Application validation remains authoritative. The added context does not authorize the model to see any additional Character-owned state; it exposes only roster identities already present in the bounded ContextPacket roster.

## 4. Scope lock

This correction must not change Core, `PerformerCandidateContract`, `StateInterpretationContract`, roster identity semantics, state domains, authority policy, causal commit law, Fixture content/hash, Integrity instructions/input, response-schema structure, Gemini model/profile, thinking controls, output ceilings, token accounting, rate/spend law, evidence law, retry/fallback law, or accepted-turn caps.

No display-name mapping, case normalization, fuzzy matching, inferred identity repair, automatic retry, or fallback is permitted.

Performer and Interpreter prompt hashes and request bytes will change. That is intentional; the prior native executable `e052ef4cdaf8ac7291971f8c42d121e1ee6eecf0` is not eligible for another reference candidate after this correction.

Implementation audit: `docs/evidence/E0A_ROLE_CONTROL_IDENTITY_ALIGNMENT_IMPLEMENTATION_AUDIT_2026_09_10.md`.

## 5. Validation

Before any further full-reference provider traffic, the corrected source candidate must pass targeted request/prompt tests, complete Core and Harness suites, repository guards, fixture/smoke/credentialless checks, and fresh native Windows ARM64 validation under the validation constitution.

An annotated validation tag must bind to the exact machine-tested source commit. Only after integration and fresh activation may a new immutable 3.5 Flash-Lite RunId consume provider traffic.