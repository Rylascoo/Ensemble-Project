# E0-A Gemini Technical-Failure Diagnostic Classification — Native Windows ARM64 Validation

Date: 2026-09-10

Status: **NATIVE MACHINE VALIDATION PASS — EXACT CHECKOUT `bb869fb1c505603612bc718f739b3f1b358e5539` — ANNOTATED TAG PUSHED — PROVIDER NETWORK NOT PERFORMED**

## Authority boundary

This record validates only `docs/blueprint/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_AMENDMENT.md`, as audited by `docs/evidence/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_IMPLEMENTATION_AUDIT_2026_09_10.md` and triggered by sealed Run 05 evidence.

```text
checkout   bb869fb1c505603612bc718f739b3f1b358e5539
base       e9511cacb2a58e59aa18595248bdf9ea16d7c435
branch     q-e0a-03-run05-technical-diagnostic-hardening-2026-09-10
tag        validation/e0a-gemini-technical-failure-diagnostic-classification-native-arm64
tag object c4ff8dc4b9ebd31674208f4ba9c958d50fc975d5
target     bb869fb1c505603612bc718f739b3f1b358e5539
```

The remote annotated tag was created only after all final exact-checkout executable and static gates below passed and was verified to peel to the exact candidate.

## Native host and preconditions

Validation ran from detached worktree `C:\Users\Wiryl\Sol Dev\E0V-bb869fb` on the Director Windows ARM64 host. Repository `global.json` selected SDK `9.0.317`; `PROCESSOR_ARCHITECTURE=ARM64`. `GEMINI_API_KEY` and `OPENAI_API_KEY` were absent from the validation processes.

## Native executable results

At exact checkout `bb869fb1c505603612bc718f739b3f1b358e5539`:

- `Ensemble.E0.Core.Tests`: **622/622 PASS** on `[net9.0|arm64]`;
- `Ensemble.E0.Harness.Tests`: **140/140 PASS** on `[net9.0|arm64]`;
- fresh Debug Harness build for `net9.0/win-arm64`: **PASS**, 0 warnings / 0 errors;
- Missing Raft fixture smoke: exit 0, `Fixture validated: ensemble.e0.missing-raft@0.1.0`;
- generic fixture smoke: exit 0, `Fixture validated: ensemble.e0.smoke@0.1.0`.

## Credentialless provider-edge validation

Using the freshly built exact-checkout executable with both provider credentials absent and the process working directory set to the exact validator checkout:

- `GEMINI-3.5-FLASH-LITE-MINIMAL / CREATIVE-MINIMAL`: exit 1, exact missing-key refusal, no evidence root — PASS;
- `GEMINI-3.1-FLASH-LITE-MINIMAL / CREATIVE-MINIMAL`: exit 1, exact missing-key refusal, no evidence root — PASS;
- `GEMINI-2.5-FLASH-LITE-NONE / CREATIVE-NONE`: exit 1, exact missing-key refusal, no evidence root — PASS;
- retired `GEMINI-2.5-FLASH-NONE / CREATIVE-NONE`: exit 1, exact non-approved-profile refusal, no evidence root — PASS.

Provider network, `countTokens`, generation, inference, and spend were **NOT PERFORMED** during validation.
## Static and preservation gates

- `tools/repository-law-check.py`: PASS;
- `tools/document-census.py --summary --check`: PASS, 219 inventoried / 102 current / 30 historical / 87 archive / 0 unexplained current;
- `tools/oracle-index.py --check --baseline e9511cacb2a58e59aa18595248bdf9ea16d7c435`: PASS, 116 documented / 17 asserted / 99 document-only hashes;
- `git diff --check`: PASS;
- post-validation HEAD remained exact `bb869fb1c505603612bc718f739b3f1b358e5539`, detached and clean;
- `PROCESSOR_ARCHITECTURE=ARM64` remained confirmed.

A first credentialless wrapper invocation was run from the wrong working directory and therefore failed earlier at the repository checkout guard with no evidence roots and no provider traffic. The corrected validation invoked the same exact executable from the validator checkout and reached the intended missing-key/profile boundaries. This host-wrapper false start is not an Ensemble runtime failure.

## Conclusion

Exact checkout `bb869fb1c505603612bc718f739b3f1b358e5539` is machine-validated on native Windows ARM64 for the bounded Gemini technical-failure diagnostic classification correction.

Native authority attaches only to that exact source candidate and annotated tag. Later documentation/integration commits do not inherit runtime authority. Runs 03/04/05 remain immutable/noncontributing. Provider traffic remains zero until this validated package is integrated through hosted CI and a later fresh 3.5 Flash-Lite RunId is separately preregistered under standing authority.
