# E0-A Role Control Identity Alignment — Native Windows ARM64 Validation

Date: 2026-09-10

Status: **NATIVE MACHINE VALIDATION PASS — EXACT CHECKOUT `29b62a2e778d93c6727b555f58f8d22aa18665a1` — ANNOTATED TAG PUSHED — PROVIDER NETWORK NOT PERFORMED**

## Authority boundary

This record validates only the correction frozen by `docs/blueprint/E0A_ROLE_CONTROL_IDENTITY_ALIGNMENT_AMENDMENT.md` and audited in `docs/evidence/E0A_ROLE_CONTROL_IDENTITY_ALIGNMENT_IMPLEMENTATION_AUDIT_2026_09_10.md`.

Exact source candidate:

```text
checkout   29b62a2e778d93c6727b555f58f8d22aa18665a1
base       8e486567e8d3f43f01c065a083f3fa1f9ba724e9
branch     q-e0a-03-run04-control-id-alignment-2026-09-10
tag        validation/e0a-role-control-identity-alignment-native-arm64
tag object f6080e37df44737b3053fa8237b95bdc2cc0508c
target     29b62a2e778d93c6727b555f58f8d22aa18665a1
```

The remote annotated tag was created only after all executable and static validation gates below passed and was verified to peel to the exact candidate.

## Native host and preconditions

Validation ran from clean detached worktree `C:\Users\Wiryl\Sol Dev\E0V-29b62a2` on the Director Windows ARM64 host. From that checkout, repository `global.json` selected SDK `9.0.317`; `PROCESSOR_ARCHITECTURE=ARM64`. Both `GEMINI_API_KEY` and `OPENAI_API_KEY` were absent.

The validator started detached and clean at the exact source candidate. Source/test `bin` and `obj` trees were cleared before authoritative tests.
## Native executable results

At exact checkout `29b62a2e778d93c6727b555f58f8d22aa18665a1`:

- `Ensemble.E0.Core.Tests`: **622/622 PASS** on `[net9.0|arm64]`;
- `Ensemble.E0.Harness.Tests`: **137/137 PASS** on `[net9.0|arm64]`;
- fresh Debug Harness build for `net9.0/win-arm64`: **PASS**, 0 warnings / 0 errors;
- Missing Raft fixture smoke: exit 0, `Fixture validated: ensemble.e0.missing-raft@0.1.0`;
- generic fixture smoke: exit 0, `Fixture validated: ensemble.e0.smoke@0.1.0`.

## Credentialless provider-edge validation

Using the freshly built exact-checkout executable with both provider keys absent:

- `GEMINI-3.5-FLASH-LITE-MINIMAL / CREATIVE-MINIMAL`: exit 1, exact missing-key refusal, no evidence root — PASS;
- `GEMINI-3.1-FLASH-LITE-MINIMAL / CREATIVE-MINIMAL`: exit 1, exact missing-key refusal, no evidence root — PASS;
- `GEMINI-2.5-FLASH-LITE-NONE / CREATIVE-NONE`: exit 1, exact missing-key refusal, no evidence root — PASS;
- retired `GEMINI-2.5-FLASH-NONE / CREATIVE-NONE`: exit 1, exact pre-credential non-approved-profile refusal, no evidence root — PASS.

Provider network, `countTokens`, generation, inference, and spend were **NOT PERFORMED** during validation.

## Static and preservation gates

- `tools/repository-law-check.py`: PASS;
- `tools/document-census.py --summary --check`: PASS, 212 inventoried / 95 current / 30 historical / 87 archive / 0 unexplained current;
- `tools/oracle-index.py --check`: PASS, 104 documented hashes / 17 asserted / 87 document-only.

Post-validation exact HEAD remained `29b62a2e778d93c6727b555f58f8d22aa18665a1`, detached and clean.
## Apparatus observations

Two wrapper-only false starts occurred and created no provider traffic or evidence roots. First, a Windows PowerShell 5.1 expression used an unsupported inline `if` form before cleanup/tests began. Second, direct native stderr capture converted the expected first missing-key refusal into a PowerShell error record and stopped that wrapper before the remaining profiles. Raw `System.Diagnostics.ProcessStartInfo` redirection then completed all credentialless assertions.

These are host-wrapper observations, not Ensemble runtime failures.

## Conclusion

Exact checkout `29b62a2e778d93c6727b555f58f8d22aa18665a1` is machine-validated on native Windows ARM64 for the E0-A role control identity alignment correction.

Native authority attaches only to that exact source candidate and annotated tag. Documentation or integration commits do not inherit it. Runs 03 and 04 remain immutable/noncontributing. Provider traffic remains zero until this validated package is integrated through hosted CI and a later fresh 3.5 Flash-Lite RunId satisfies the standing activation law.