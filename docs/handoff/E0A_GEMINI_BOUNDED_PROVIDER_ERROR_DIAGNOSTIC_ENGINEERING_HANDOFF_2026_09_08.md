# E0-A Fresh-Chat Handoff — Bounded Gemini Provider-Error Diagnostic / Request Compatibility

Status: **ACTIVE FRESH-CHAT TRANSITION — ENGINEERING AMENDMENT AUTHORIZED — PROVIDER TRAFFIC NOT AUTHORIZED**

Recorded: 2026-09-08

Authority: this handoff is temporary transition guidance only. It is live only while `CURRENT_STATE.md` names this exact path. It cannot advance phase, validation, product, or provider authority by itself.

## Fresh-chat first action

1. Read `CURRENT_STATE.md` first and resolve the current remote branch head, `main`, and latest Validation result. Repository state overrides this snapshot if anything moved.
2. Then read, in this order:
   - `docs/PROJECT_AUTHORITY.md`;
   - `docs/ENGINEERING_HYGIENE_CONSTITUTION.md`;
   - `docs/VALIDATION_LEDGER.md`;
   - `docs/evidence/E0A_GEMINI_PROVIDER_COMPATIBILITY_DIRECTOR_DECISION_2026_09_08.md`;
   - `docs/evidence/E0A_THIRD_REAL_GEMINI_35_LITE_ATTEMPT_03_ARCHIVE_AUDIT_2026_09_08.md`;
   - `docs/evidence/E0A_GEMINI_AUTH_TRANSPORT_DIFFERENTIAL_RESULT_2026_09_08.md`;
   - `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md`;
   - `docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md`;
   - exact executable source at promoted checkout `689655eed677b789ab3ee395f1c65b4f2cb72cc8`, especially `src/Ensemble.E0.Harness/Gemini/GeminiGenerateContentPort.cs` and the smallest affected Harness-test surface.
3. Treat prior-chat prose as non-authoritative. Reconstruct the current source/test/document dependency surface from GitHub before changing anything.

## Durable checkpoint

At handoff preparation:

- runtime phase: **E0-A Experimental Harness — Phase B**;
- `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`;
- active work branch: `e0a-gemini-rate-discipline-model-comparison`;
- promoted machine-tested executable: `689655eed677b789ab3ee395f1c65b4f2cb72cc8`;
- native validation tag: `validation/e0a-gemini-counttokens-correction-native-arm64`;
- native evidence at that executable: Core **622/622**, Harness **125/125**, fresh native Windows ARM64 build/smokes/credentialless gates PASS;
- existing promoted executable remains valid historical machine evidence, but any new source change creates a new unvalidated executable checkpoint.

Always resolve current branch/head and Validation state fresh rather than assuming the documentation head at handoff creation remains current.

## Provider evidence that must not be lost

Gemini 3.5 Flash-Lite attempts 01-03 are consumed and must not be rerun.

The decisive current evidence is attempt 03:

- run `E0A-REAL-G35L-20260908-03`;
- exact executable `689655...`;
- same fresh unshared Auth key that immediately beforehand succeeded on a simple `gemini-3.5-flash-lite:countTokens` request through `x-goog-api-key`;
- all local preflights passed;
- uploaded evidence ZIP SHA-256 `8120E426AE206439A9E43F8D5C85C277B492B9055A0ED012133793B91EC1D77E` verified;
- all sealed artifact hashes, runtime root, and runtime-seal identity verified;
- first Performer `countTokens` reached Google and returned HTTP **400** after `480.4031 ms`;
- no successful token count, spend reservation, generation/inference, provider receipt, Integrity/Interpreter invocation, accepted performance, or committed turn occurred;
- 0 accepted turns / $0 shadow spend;
- the exact provider 400 field violation is unknown because the current bounded diagnostic records HTTP status but discards the structured provider error body.

The preceding auth differential proved that `x-goog-api-key`, the fresh Auth key, `gemini-3.5-flash-lite`, and the `countTokens` method can work together for a simple body. Do not reopen the already-closed authentication-transport theory without new evidence.

Google provider documentation was found internally inconsistent: the model-specific Gemini 3.5 Flash-Lite page says structured outputs are supported, while the general structured-output support table omits 3.5 Flash-Lite. Therefore the route is classified **frozen-request live compatibility unproven**, not generally structured-output unsupported.

## Director decision now in force

`docs/evidence/E0A_GEMINI_PROVIDER_COMPATIBILITY_DIRECTOR_DECISION_2026_09_08.md` authorizes engineering work and decides:

- Gemini 3.5 Flash-Lite live execution is **PAUSED**;
- proceed with the bounded provider-error diagnostic / request-compatibility engineering amendment;
- do **not** advance the Gemini 3.1 Flash-Lite comparator yet;
- provider authorization is **NONE**;
- this engineering authorization does **not** authorize credentials, `countTokens`, generation, probes, inference, spend, fallback, or any other provider traffic.

## Engineering objective

Construct and implement the smallest auditable amendment that makes provider-side request rejection diagnostically useful without persisting arbitrary provider prose or secret-bearing material.

Before source changes, reconcile the current Google structured error envelope against the exact Harness evidence contract. Prefer bounded structured facts only. Candidate facts include validated provider error status/code and validated `google.rpc.BadRequest.fieldViolations[].field` paths. Arbitrary `message`/`description` text, raw response bodies, response headers, credential-bearing URIs, API keys, and uncontrolled provider strings must remain excluded.

Do not assume the candidate schema above is final merely because this handoff names it. Verify current Google contract semantics and recursively audit the privacy/evidence boundary before freezing the amendment.

The implementation should remain Harness-local and minimal unless repository evidence proves a wider change is required. Do not change Core semantics or fixtures from this work package. Do not redesign the Phase-B run architecture, rate discipline, one-attempt/zero-retry law, spend ceiling, deterministic authority, evidence sealing, or comparison strategy beyond what the approved diagnostic amendment requires.

## Required work package

The fresh chat should complete one logically coupled engineering package as far as available tools permit:

1. fresh-read and synthesize current authority/source/tests;
2. reverify the provider error-envelope/request-compatibility facts needed for the amendment;
3. write the smallest explicit amendment/decision specification needed to freeze the diagnostic contract;
4. recursively audit that specification for correctness, privacy, boundedness, provider-text leakage, determinism, evidence compatibility, and scope;
5. implement the smallest Harness/test patch;
6. run available deterministic/unit/compiler/static checks and recursively audit the resulting source/test/document diff;
7. update durable evidence/continuity;
8. if source changed, stop at the required **native Windows ARM64 machine-validation gate** and prepare the exact Director-machine validation packet unless the current environment can genuinely perform that validation.

Do not claim native validation from cloud ARM64 cross-compilation.

## Validation law after source change

Any source modification invalidates machine authority for the modified executable until a new exact clean checkout passes the required validation. Before any future provider request on the amended executable, require at least:

- targeted Harness regressions for safe bounded error extraction, malformed/error-body handling, arbitrary-text suppression, and no credential leakage;
- existing Harness/Core regression coverage appropriate to the changed surface;
- cloud Validation gate;
- native Windows ARM64 Core/Harness tests;
- fresh native Harness build and relevant fixture/credentialless smokes;
- annotated validation tag pointing to the exact native-tested checkout, with honest scope/evidence classification.

Only after that may a later chat prepare a new, separately Director-authorized provider diagnostic/run. This handoff authorizes none.

## Continuity / stop conditions

Do not:

- rerun Gemini 3.5 Flash-Lite;
- advance Gemini 3.1 Flash-Lite provider execution;
- use or request an API key;
- make any real provider request;
- reopen E-R1;
- enter E0-B+, app/UI/persistence, Windows AI/NPU, packaging/WACK/Store, or design-lane work;
- alter Core/fixture semantics without separate authority;
- inflate validation levels.

Before every consequential reply, recursively audit correctness, consistency, authority, scope, tests, simplicity, hygiene, ARM64 suitability, vision, privacy/security, and evidence until one complete pass finds no material correction or worthwhile improvement.

When this handoff has served its transition purpose, remove it from the active tree and fold durable results back into `CURRENT_STATE.md`.
