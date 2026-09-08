# E0-A Gemini Free-Tier RPD Model Selection — Implementation Audit

Status: **SOURCE/TEST IMPLEMENTATION COMPLETE — CLOUD GATES PASS — NATIVE WINDOWS ARM64 PASS — ANNOTATED TAG PROMOTED — REAL GEMINI NETWORK EXECUTION NOT AUTHORIZED**

Updated: **2026-09-08**

## Authority

Director-approved amendment:

`docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md`

Architecture commit:

`825309e4ad0e9e207136aa6b20fd31925e814172`

Corrected source/test checkpoint:

`b4d39cd91d1c23bad1f0354702fb64411e82780d`

Cloud Validation for that correction:

`34185909785` — **PASS**

Exact native Windows ARM64 checkout:

`3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99`

Annotated validation tag:

`validation/e0a-gemini-rpd-model-selection-native-arm64` — remotely verified annotated tag object `ca036b8b44775f77535d04b82ba4ebbbb3d295f0`, dereferencing exactly to the native checkout above.

Native evidence:

`docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`

## Implemented live profile set

```text
GEMINI-3.5-FLASH-LITE-MINIMAL
  model        gemini-3.5-flash-lite
  arm          CREATIVE-MINIMAL
  turn cap     12
  quota        15 RPM / 250K input TPM / 500 RPD

GEMINI-3.1-FLASH-LITE-MINIMAL
  model        gemini-3.1-flash-lite
  arm          CREATIVE-MINIMAL
  turn cap     12
  quota        15 RPM / 250K input TPM / 500 RPD

GEMINI-2.5-FLASH-LITE-NONE
  model        gemini-2.5-flash-lite
  arm          CREATIVE-NONE
  turn cap     3
  quota        10 RPM / 250K input TPM / 20 RPD
```

`GEMINI-2.5-FLASH-NONE` remains an internal historical compatibility anchor and is not live-selectable.

## RPD admission

Each accepted turn has three sequential role invocations. Each role performs one exact `countTokens` request and at most one generation request, so the local conservative upper bound is six Gemini API-bound operations per accepted turn.

```text
3.5 Flash-Lite: 12 turns × 6 = 72 <= 500 RPD
3.1 Flash-Lite: 12 turns × 6 = 72 <= 500 RPD
2.5 Flash-Lite:  3 turns × 6 = 18 <=  20 RPD
```

The code carries `RequestsPerDay`, `AcceptedTurnCap`, and the six-request bound in each profile and rejects a live route whose full local worst-case run cannot fit its recorded RPD. Traffic from other processes remains an external pre-live condition.

## Runtime changes

The amendment is confined to Harness/test/document surfaces.

- `E0AGeminiModelCatalog` carries 3.5 Lite, 3.1 Lite, and 2.5 Lite live profiles, RPD, per-profile turn caps, live-selectability, pricing, and opaque-signature policy.
- `E0ARunEnvelope` preserves the global 12-turn ceiling while exposing a lower selected-profile cap where required.
- `E0AReferenceRunDriver` terminates at the selected profile's accepted-turn cap.
- `E0AEvidenceStore` records amendment identity, RPM/TPM/RPD, selected turn cap, conservative request bound, worst-case request count, and timing evidence.
- `GeminiGenerateContentPort` permits and strips opaque `thoughtSignature` metadata for approved Gemini 3 Flash-Lite profiles while continuing to reject explicit returned thought material.
- `E0AReferenceRunHost` resolves only current live profiles through the explicit two-argument path; historical 2.5 Flash is rejected before credential access/evidence creation.

One-attempt/zero-retry law, RPM/TPM pacing, exact token preflight, spend ceiling, cancellation semantics, 300-second role timeout, deterministic Core authority, fixture identity, and evidence sealing remain unchanged.

## Pricing/thinking snapshot

```text
gemini-3.5-flash-lite   $0.30/M input   $0.03/M cached   $2.50/M output incl thinking
gemini-3.1-flash-lite   $0.25/M input   $0.025/M cached  $1.50/M output incl thinking
gemini-2.5-flash-lite   $0.10/M input   $0.01/M cached   $0.40/M output incl thinking
```

The Free-tier live path remains synthetic-fixture-only. 3.5 and 3.1 use `thinkingLevel=minimal` for Performer/Interpreter and `high` for Integrity; minimal is not represented as literal thinking-off. 2.5 Flash-Lite remains the exact-no-thinking control with `thinkingBudget=0` for creative roles.

Gemini 3.1 Flash-Lite has an announced earliest shutdown date of 2027-05-07 and is treated as an experimental comparator rather than a presumed durable production dependency.

## Regression coverage

The Harness-test surface covers:

- live profile membership and approved arm/profile pairings;
- rejection of historical 2.5 Flash from live selection;
- static RPD/turn-cap admissibility;
- 3.1 Flash-Lite thinking controls and route pricing;
- fake 2.5 Flash-Lite execution terminating at exactly three accepted turns with nine role calls and nine token preflights;
- manifest binding of amendment commit, RPD, accepted-turn cap, request bound, worst-case request count, and pricing;
- 3.1 and 3.5 streaming `thoughtSignature` acceptance/stripping;
- explicit-thought rejection;
- existing RPM/TPM pacing and request/usage/spend behavior.

## Native falsification attempt 01

Director-machine validation at `31436ee52238c2de96e9bcbe9d61ece173f81dd3` established native ARM64 and Core **622/622 PASS**, then stopped at Harness **121/122**. The failure was:

`E0ALiveHostReadinessTests.LiveHost_ExposesOnlyApprovedArmProfilePairings`

The predecessor readiness test still attempted to construct `GEMINI-2.5-FLASH-NONE` as live. The amended catalog correctly rejected it. Correction `b4d39cd91d1c23bad1f0354702fb64411e82780d` changed only `E0ALiveHostReadinessTests.cs`: it positively exercises the three current live pairings and expects historical 2.5 Flash to fail closed. Cloud Validation `34185909785` passed.

Attempt 01 did not reach fresh Harness build, fixture smokes, credentialless profile probes, or retired-route host probe and receives no partial promotion.

## Native validation attempt 02

A fresh complete Director-machine run at exact checkout `3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99` passed:

```text
Windows ARM64 host                         PASS
Core tests                         622/622 PASS
Harness tests                      122/122 PASS
Fresh win-arm64 Harness build              PASS
Missing Raft fixture smoke                 PASS
Generic fixture smoke                      PASS
3.5 Lite missing-key/no-evidence gate      PASS
3.1 Lite missing-key/no-evidence gate      PASS
2.5 Lite missing-key/no-evidence gate      PASS
Retired 2.5 Flash pre-credential reject    PASS
Post-run exact HEAD/cleanliness            PASS
```

Both provider-key environment variables were absent before and after the run. No real Gemini provider network, `countTokens`, inference, credential use, or spend occurred.

Native runtime authority applies only to exact checkout `3d6d8a7f...`. Later evidence/continuity commits do not inherit it.

## Scope audit

The amendment has no change under:

- `src/Ensemble.E0.Core/`;
- `tests/Ensemble.E0.Core.Tests/`;
- `fixtures/`.

It therefore does not modify deterministic Core semantics or frozen fixture content.

## Provider boundary

```text
Gemini credential use             NOT PERFORMED
Gemini provider network           NOT PERFORMED
Gemini countTokens network call   NOT PERFORMED
Gemini inference                  NOT PERFORMED
Gemini spend                      NOT PERFORMED
```

## Recursive audit disposition

The implementation, both native attempts, evidence, and promotion were recursively checked for architecture consistency, profile identity, RPD arithmetic, run-cap enforcement, thinking controls, response metadata handling, pricing/accounting, evidence provenance, retired-route isolation, Core/fixture scope, ARM64 suitability, validation classification, tag identity, and continuity.

Corrections discovered were applied before attempt 02: the test API mismatch, stale predecessor wording, RPD/run-cap evidence omissions, and stale live-readiness expectation. Attempt 02 passed the full native packet; the annotated tag then independently dereferenced to that exact machine-tested checkout. No further material correction was identified inside the approved amendment boundary.

## Next gate

The RPD/model-selection amendment is **promoted**. The next consequential gate is separate provider authorization: reverify current project/account/key/quota/pricing/data-use conditions and remaining daily capacity, then obtain explicit Director approval for exactly one real synthetic Missing Raft `GEMINI-3.5-FLASH-LITE-MINIMAL` run at promoted checkout `3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99`. No real Gemini execution is authorized by this promotion.
