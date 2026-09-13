# E0-C Q-E0C-01 - Exact Two-Slot Preregistration

Date: 2026-09-12

Status: **FROZEN - TWO FRESH SLOTS RESERVED - NO NAMESPACE CLAIM CREATED - NO PROVIDER TRAFFIC**

Authority: `docs/evidence/E0C_Q_E0C_01_METHOD_DIRECTOR_DECISION_2026_09_12.md` approves exactly two fresh repeats of the selected Run 08 condition.

## Frozen condition

- condition: `E0C-IDENTICAL-REFERENCE-01`
- anchor: `E0A-Q03-G35L-20260911-08`
- executable: `bb869fb1c505603612bc718f739b3f1b358e5539`
- validation tag: `validation/e0a-gemini-technical-failure-diagnostic-classification-native-arm64`
- tag object: `c4ff8dc4b9ebd31674208f4ba9c958d50fc975d5`
- fixture: `ensemble.e0.missing-raft@0.1.0`
- fixture SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`
- provider/model/profile: Google Gemini / `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`
- Performer/Interpreter thinking: `minimal`; Integrity: `high`
- accepted turns: `12`; attempts per role: `1`; automatic retries: `0`; timeout: `300s`; visible output ceiling: `4096`
- shadow-spend ceiling: USD `5.00` per slot / USD `10.00` aggregate maximum reservation

## Fresh Repeat 1

- RunId: `E0C-Q01-REF-20260912-01`
- evidence root: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0C-Q01-REF-20260912-01`
- deterministic run claim: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\.ensemble-e0a-claims\run-0fc313de3fcaeab7a55ccb725d5e054ce46474130d1be30e25745197391f0b43.claim`
- deterministic root claim: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\.ensemble-e0a-claims\root-b0ce0c09c35b26b2c8d656304bb8409c03cfbbe12222e336fe572f4bb9f78f73.claim`

At preregistration, repository occurrence count was zero; root, run claim, root claim, and matching external run process were absent. Credentialless exact-host validation reached the missing-key edge with exit 1 and left all three namespace artifacts absent.

## Fresh Repeat 2

- RunId: `E0C-Q01-REF-20260912-02`
- evidence root: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0C-Q01-REF-20260912-02`
- deterministic run claim: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\.ensemble-e0a-claims\run-c1a0ac0f2d87a06c42aa39795aa2c1421e86b32fcd72edb27cc90184a69fc5a9.claim`
- deterministic root claim: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\.ensemble-e0a-claims\root-0b9f9cdd1f1ab82329d831738fe1caf3b9449cbf323a48f88fe92905c4beb945.claim`

At preregistration, repository occurrence count was zero; root, run claim, root claim, and matching external run process were absent. Credentialless exact-host validation reached the missing-key edge with exit 1 and left all three namespace artifacts absent.

## Slot law

Both slots are preregistered before the first fresh transcript. Any actual Harness namespace claim consumes that slot permanently. A technical failure, cancellation, refusal, budget terminal, invalid output, provider availability failure, or hard-gate failure earns no retry/replacement. Repeat 2 remains an independent planned repeat, not a retry of Repeat 1. No third fresh slot exists under this method.

## Frozen blind instrument

`docs/evidence/E0C_Q_E0C_01_BLIND_SCORING_INSTRUMENT_2026_09_12.json` SHA-256: `79ac8017e17c5f97da4687d49b79db576d5841f70783f7ef4767f5c5cc9f2933`.

The instrument freezes all three potential pair memberships before fresh execution and fixes a deterministic runtime-seal-based LEFT/RIGHT mapping law. Actual mappings cannot be chosen by a human and are created only for contribution-eligible pairs before scorer access, then withheld until pair scores/observations are sealed.

This preregistration creates no runtime root, claim, credential access, provider request, inference, or spend.
