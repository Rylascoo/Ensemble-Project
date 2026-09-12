# E0-B Mixed-Cast Implementation — Native Windows ARM64 Validation

Date: 2026-09-12

Status: **NATIVE MACHINE VALIDATION PASS — EXACT CHECKOUT `d1073fe2c76e2e05f2daac47465f86b48b456a9a` — ANNOTATED TAG PUSHED — PROVIDER NETWORK NOT PERFORMED**

## Authority boundary

This record validates the Director-approved `E0B-MIXED-CAST-01` implementation defined by `docs/blueprint/E0B_MIXED_MODEL_CAST_METHOD_PROPOSAL_01.md`, approved in `docs/evidence/E0B_Q_E0B_01_MIXED_CAST_DIRECTOR_DECISION_2026_09_12.md`, and recursively audited in `docs/evidence/E0B_MIXED_CAST_IMPLEMENTATION_AUDIT_2026_09_12.md`.

```text
checkout   d1073fe2c76e2e05f2daac47465f86b48b456a9a
base       ba02d1689680d8492a4c52675e7aba91935fd2dd
branch     e0b-mixed-cast-implementation-2026-09-12
tag        validation/e0b-mixed-cast-implementation-native-arm64
tag object f7eadb838c4e01d9b4a01d17ff20b8d938046b03
target     d1073fe2c76e2e05f2daac47465f86b48b456a9a
```

The annotated tag was created and pushed only after the exact implementation commit was clean and its native executable/test/static gates below passed. The remote tag was verified to peel exactly to the candidate.

## Native host and preconditions

Validation ran in `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Worktrees\e0b-mixed-cast-implementation-2026-09-12` on the Director Windows ARM64 host. Repository `global.json` selected SDK `9.0.317`; `PROCESSOR_ARCHITECTURE=ARM64`.

## Native executable results

At exact checkout `d1073fe2c76e2e05f2daac47465f86b48b456a9a` before documentation changes:

- `Ensemble.E0.Core.Tests`: **622/622 PASS** on `[net9.0|arm64]`;
- `Ensemble.E0.Harness.Tests`: **147/147 PASS** on `[net9.0|arm64]`;
- fresh Release Harness build for `net9.0/win-arm64`: **PASS**, 0 warnings / 0 errors;
- Missing Raft fixture smoke: exit 0, `Fixture validated: ensemble.e0.missing-raft@0.1.0`;
- generic fixture smoke: exit 0, `Fixture validated: ensemble.e0.smoke@0.1.0`.

The exact implementation commit contains no `src/Ensemble.E0.Core/**` changes relative to its approved base.

## Credentialless E0-B provider-edge validation

Using the freshly built exact-checkout executable with `GEMINI_API_KEY` absent, the fixed `e0b-run` command was invoked with the canonical Missing Raft fixture, a fresh synthetic RunId/root, and exact executable commit.

The host passed its clean-checkout, single-use-root, fixture, approved-cast, and pricing-snapshot guards, then failed at the provider credential edge with `GEMINI_API_KEY is required at the E0-A provider edge.` Exit code was 1; no evidence root was created.

Provider network, `countTokens`, generation, inference, and spend were **NOT PERFORMED** during validation.

## Static and preservation gates

At the exact implementation commit:

- `tools/repository-law-check.py`: PASS;
- `tools/document-census.py --summary --check`: PASS, 246 inventoried / 129 current / 30 historical / 87 archive / 0 unexplained current;
- `tools/oracle-index.py --check`: PASS, 163 documented / 17 asserted / 146 document-only hashes;
- `git diff --check HEAD^ HEAD`: PASS;
- implementation diff: exactly 9 Harness/Harness-test files; **0 Core/Core-test files**;
- exact implementation worktree was clean before the documentation evidence package began.

The oracle helper completed successfully but required approximately 169 seconds on the host; the longer runtime was not treated as a failed gate.

## Conclusion

Exact checkout `d1073fe2c76e2e05f2daac47465f86b48b456a9a` is machine-validated on native Windows ARM64 for the approved E0-B mixed-model cast Harness/evidence amendment.

Native authority attaches only to that exact executable source candidate and annotated validation tag. The later documentation/integration commit does not inherit runtime authority.

This validation authorizes integration of the bounded implementation only. Provider traffic remains blocked until the validated implementation is integrated through hosted CI and a separate exact E0-B live-run activation durably re-establishes current provider/model/pricing/data-use facts, authenticated project/tier/quota/capacity for both selected routes, exact RunId/evidence root, protected credential readiness, and applicable Director/provider authority.
