# E0-B Mixed-Cast Implementation Audit

Date: 2026-09-12

Status: **PASS — APPROVED E0B-MIXED-CAST-01 IMPLEMENTED OUTSIDE CORE — PROVIDER TRAFFIC BLOCKED**

## Authority and scope

Director-approved method: `docs/blueprint/E0B_MIXED_MODEL_CAST_METHOD_PROPOSAL_01.md`.

Director decision: `docs/evidence/E0B_Q_E0B_01_MIXED_CAST_DIRECTOR_DECISION_2026_09_12.md`.

Implementation base: `ba02d1689680d8492a4c52675e7aba91935fd2dd`.

Exact implementation commit: `d1073fe2c76e2e05f2daac47465f86b48b456a9a`.

The implementation changes only Harness and Harness-test surfaces. `src/Ensemble.E0.Core/**` is unchanged.

## Implemented condition

`E0BMixedCastConfiguration` hard-pins `E0B-MIXED-CAST-01` rather than exposing arbitrary mixed-model selection:

- `VOSS` Performer -> Gemini 3.1 Flash-Lite Minimal;
- `MARLOWE` Performer -> Run 08 Gemini 3.5 Flash-Lite Minimal;
- `WREN` Performer -> Run 08 Gemini 3.5 Flash-Lite Minimal;
- Integrity -> Run 08 Gemini 3.5 Flash-Lite High;
- Interpreter -> Run 08 Gemini 3.5 Flash-Lite Minimal.

## Harness / evidence amendment

The bounded implementation adds a dedicated `e0b-run` host and does not turn the E0-A CLI into an arbitrary cast selector. The live E0-B host reconstructs the approved cast from code and accepts only fixture, RunId, fresh evidence root, and executable commit.

Per-Turn Performer selection resolves from the current Context subject Character. Integrity and Interpreter stay on the Run 08 reference route. The driver validates each successful provider receipt against the exact model expected by the selected route before the Turn can contribute.

Quota discipline is two-layered: each selected model route retains its own RPM/TPM/RPD accounting, while one conservative shared-project request/input-token discipline prevents mixed routing from manufacturing additional Gemini-project throughput.

Shadow spend is reserved and reconciled against the actual selected route pricing and input limit, but all routes share the unchanged deterministic USD 5.00 run ceiling.

The E0-B manifest is `ensemble.e0b.run-manifest.v1`. It records `conditionId`, the complete `performerCast`, per-route pricing/quota provenance, and shared-project pacing. E0-A-only single-route fields `providerProfileId` and `roles` are null under E0-B; Run 08 baseline provenance is instead explicit under `referenceProviderProfileId` and `referenceRoles`.

## Isolation and fake-first proof

The native Harness suite contains direct E0-B coverage for:

- exact MARLOWE/WREN 3.5 and VOSS 3.1 Performer routing;
- Run 08 3.5 Integrity/Interpreter routing across the full 12-Turn fake condition;
- same-Turn cross-route receipt rejection even when deterministic attempt IDs coincide;
- cross-role receipt rejection and inherited cross-Turn prepared-attempt identity rejection;
- exact returned-model mismatch termination before any accepted Turn;
- mixed-cast manifest provenance and shared-project pacing;
- route-specific shadow pricing under one global spend ceiling.

The full fake condition reaches 12/12 accepted Turns with 36/36 role invocations. The initial VOSS Opportunity guarantees that the alternate 3.1 route is exercised immediately.

Preserved provider evidence supports the exact identity invariant rather than leaving it fake-only: Run 08 receipts returned `gemini-3.5-flash-lite`, and preserved Run 07 successful receipts returned `gemini-3.1-flash-lite`.

## Recursive audit findings

Two defects were caught before freeze and corrected in place:

1. the first E0-B manifest shape retained E0-A's single-route `providerProfileId`/`roles` fields, which could falsely imply one model governed the whole run; E0-B now nulls those fields and records reference-versus-cast provenance separately;
2. inherited attempt-identity coverage proved cross-Turn reuse rejection, but the approved E0-B method called for explicit Character/role/route boundary proof; dedicated same-Turn cross-route and cross-role rejection assertions were added.

No Core dependency was required. No retry, fallback, accepted-Turn, deterministic authority, fixture, prompt/schema, or State/Take semantics changed.

## Provider boundary

The clean exact implementation commit was exercised credentiallessly. `e0b-run` passed repository cleanliness, fixture and pricing guards, reached the provider credential edge, rejected the absent `GEMINI_API_KEY`, returned exit 1, and created no evidence root. No provider request, `countTokens`, generation, inference, or spend occurred.

## Conclusion

The approved `E0B-MIXED-CAST-01` Harness/evidence amendment is implemented within scope and is suitable for exact native validation and integration. Live E0-B execution is not authorized by this audit. A separate exact live-run activation remains mandatory after integration and must re-establish current provider/model/pricing/data-use and authenticated project/tier/quota/capacity gates before any credential use or provider traffic.
