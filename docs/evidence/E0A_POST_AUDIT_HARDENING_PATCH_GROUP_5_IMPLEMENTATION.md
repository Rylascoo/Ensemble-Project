# E0-A Post-Audit Hardening — Patch Group 5 Implementation

Status: **IMPLEMENTED — RECURSIVE STATIC AUDIT COMPLETE; NATIVE VALIDATION PENDING**

Date: 2026-09-06

## Authority and checkpoint

Repository: `Rylascoo/Ensemble-Project`

Branch: `e0a-phase-b-post-audit-hardening`

Audited `main` baseline / merge base:

`f9cb1a79ad7923b640ff1a97c46d8fd6ab9dac25`

Patch Group 4 executable/test checkpoint:

`2a55319820587b63da13377a775150b90c24b107`

Exact executable/test checkpoint after Patch Group 5:

`f79bc36bf72d3db49b703bf98919534fef9673b5`

Closed-audit authority:

`docs/evidence/E0A_POST_AUDIT_HARDENING_CLOSED_FINDINGS.md`

Proposal 0.15 remains unchanged. Provider execution, credentials, network inference, and spend remain unauthorized.

## Scope

Patch Group 5 implements together:

- **E-04 — Fixture parsing**
- **E-05 — Integrity concern contract consistency**
- **I-05 — Checkout verification**

## E-04 — Strict fixture property-name decoding

`StrictJsonPreflight` continues to enforce the frozen E0 Fixture Dialect strict-JSON boundary unchanged. The only Core source correction is at an already-identified JSON `PropertyName` token: property-name decoding is isolated in a narrow helper and decoding failures are translated into the established sanitized `FixtureValidationException` domain.

The helper narrowly translates `JsonException`, `InvalidOperationException`, and `ArgumentException` from property-name decoding to:

`Fixture contains an invalid JSON property name.`

The outer syntax-error translation remains unchanged. No duplicate-property, number-token, depth, trailing-content, comment, size, or root-object rule was weakened.

## E-05 — Exact Integrity concern names

`E0AIntegrityConcernParser` no longer relies on enum coercion. It uses an ordinal exact-name switch over exactly the five frozen concern names:

- `PotentialInaccessibleInformationUse`
- `PotentialProtectedInformationExposure`
- `PotentialLockedAuthorityViolation`
- `PotentialTechnicalArtifactLeak`
- `IndeterminateSemanticIntegrity`

Numeric strings, padded strings, case variants, undefined names, numeric JSON values, malformed Unicode, and other noncanonical representations are rejected.

Duplicate canonical concern names remain preserved for the deterministic Core duplicate-rejection path established by E-01; this patch does not move duplicate semantics into the Harness.

## I-05 — Repository-root checkout inspection

Checkout verification now discovers the verified repository root with `git rev-parse --show-toplevel`, then performs all authoritative inspection through `git -C <repository-root>`.

Untracked inspection uses `git ls-files --others --exclude-standard --full-name`, making output unambiguously repository-root-relative before material `src/`, `tests/`, and `fixtures/` filtering.

The guard still verifies exact HEAD, tracked cleanliness, staged cleanliness, and material untracked paths. Launching the Harness from a repository subdirectory can no longer change the path basis used by the material-file filter.

## Regression coverage

Core regression coverage proves an escaped unpaired-surrogate property name fails through the sanitized fixture-validation boundary.

Harness regression coverage proves:

- all five exact frozen Integrity concern names are accepted;
- numeric, padded, undefined, case-changed, and numeric-JSON concern representations are rejected;
- checkout commands after root discovery are forced through `git -C <repository-root>`;
- untracked paths are requested with `--full-name`;
- a material root-relative untracked source path is rejected under both simulated root and subdirectory launch contexts.

Existing duplicate-concern coverage remains intact and continues to prove deterministic Core rejection before Interpreter invocation.

## Recursive static audit

The completed Group 5 surface was recursively checked across:

- strict fixture JSON authority;
- property-name decoding exception domains;
- exact concern-name acceptance/rejection;
- malformed Unicode handling;
- duplicate concern preservation;
- repository-root discovery;
- tracked/staged/untracked checkout verification;
- launch-location independence;
- simplicity, ARM64 suitability, and scope.

One worthwhile correction found during recursive review was incorporated before closure: the property-name helper was tightened to translate a `JsonException` thrown specifically by `Utf8JsonReader.GetString()` at the already-verified `PropertyName` token, ensuring the escaped-surrogate case cannot fall through to a less-specific external syntax boundary.

Static closure result: **one complete pass found no remaining material Patch Group 5 correction or worthwhile in-scope simplification.**

## Core / scope statement

Patch Group 5 is the first remaining hardening group to change Core. It changes exactly one Core source file:

`src/Ensemble.E0.Core/Fixture/StrictJsonPreflight.cs`

and its focused Core regression file:

`tests/Ensemble.E0.Core.Tests/Fixture/StrictJsonPreflightTests.cs`

No other `src/Ensemble.E0.Core/**` file is changed by Patch Group 5.

No provider request, credential access, network inference, product persistence, UI, NPU, package/Store work, later phase behavior, or broader fixture-contract redesign was introduced.

## Validation boundary

No compiler, test-runtime, native Windows ARM64, fixture-smoke, or provider validation claim is made for `f79bc36bf72d3db49b703bf98919534fef9673b5`.

Grouped native Windows ARM64 validation remains deferred until all authorized hardening groups complete recursive static audit.

## Gate

- Patch Group 5 implementation: **COMPLETE**
- Patch Group 5 recursive static audit: **COMPLETE**
- Patch Group 5 native validation: **NOT YET PERFORMED**
- Patch Group 6: **NOT STARTED IN THIS RECORD**
- Provider execution: **NOT AUTHORIZED**
- Merge to `main`: **NOT AUTHORIZED / NOT READY**
