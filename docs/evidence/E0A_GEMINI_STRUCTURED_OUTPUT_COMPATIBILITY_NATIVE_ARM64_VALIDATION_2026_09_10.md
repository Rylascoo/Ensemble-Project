# E0-A Gemini Structured-Output Compatibility — Native Windows ARM64 Validation

Date: 2026-09-10

Status: **NATIVE MACHINE VALIDATION PASS — EXACT CHECKOUT `cef3fc15e31192a48aa3bddd99450b65a58bd8f1` — ANNOTATED TAG CREATED — PROVIDER NETWORK NOT PERFORMED**

## Authority boundary

This record validates only the executable correction defined by `docs/blueprint/E0A_GEMINI_GENERATECONTENT_STRUCTURED_OUTPUT_COMPATIBILITY_AMENDMENT.md`. It does not authorize credentials, provider traffic, token counting, generation, inference, retry, rerun, fallback, alternate models, reference contribution, or spend.

Exact source candidate:

```text
checkout   cef3fc15e31192a48aa3bddd99450b65a58bd8f1
base       6fffffba7275a8612b0d4d43cd2498c0e5513ad4
branch     q-e0a-03-g35l-legacy-structured-output-compat-2026-09-10
```

Native execution was performed from a separate clean detached worktree at the exact candidate. A second short-path detached worktree at the same exact candidate was used only for the oracle-index static guard because the long validation path exceeded Windows path handling inside that tool.

## Native host identity

The Director host reported:

```text
PROCESSOR_ARCHITECTURE=ARM64
.NET SDK=9.0.317
RID=win-arm64
Host Architecture=arm64
GEMINI_API_KEY=ABSENT
OPENAI_API_KEY=ABSENT
```
Result: native Windows ARM64 host identity and credentialless precondition **PASS**.

## Complete native validation results

At exact checkout `cef3fc15e31192a48aa3bddd99450b65a58bd8f1`:

- `Ensemble.E0.Core.Tests`: **622/622 PASS** on `[net9.0|arm64]`;
- `Ensemble.E0.Harness.Tests`: **134/134 PASS** on `[net9.0|arm64]`;
- fresh Debug Harness build for `net9.0/win-arm64`: **PASS**, zero warnings/errors;
- Missing Raft fixture smoke: **PASS** (`ensemble.e0.missing-raft@0.1.0`);
- generic fixture smoke: **PASS** (`ensemble.e0.smoke@0.1.0`).

Fresh-output discipline cleared Core/Harness and test `bin/`/`obj/` roots before authoritative execution. The explicit Harness `win-arm64` output roots were cleared again before the build used for executable smokes.

## Credentialless provider-edge validation

With both provider credential environment variables absent:

- `GEMINI-3.5-FLASH-LITE-MINIMAL / CREATIVE-MINIMAL`: exact `GEMINI_API_KEY is required at the E0-A provider edge.` refusal, native exit 1, no evidence root — **PASS**;
- `GEMINI-3.1-FLASH-LITE-MINIMAL / CREATIVE-MINIMAL`: same exact missing-key refusal, native exit 1, no evidence root — **PASS**;
- `GEMINI-2.5-FLASH-LITE-NONE / CREATIVE-NONE`: same exact missing-key refusal, native exit 1, no evidence root — **PASS**;
- retired `GEMINI-2.5-FLASH-NONE / CREATIVE-NONE`: exact `E0-A Gemini provider profile is not approved for live selection.` refusal, native exit 1, no evidence root — **PASS**.

Provider network was **NOT PERFORMED**. Spend during validation was **USD 0**.
## Repository and preservation checks

At the exact candidate:

- tracked/staged state remained clean after tests/build/smokes;
- no material untracked files existed under `src/`, `tests/`, or `fixtures/`;
- `tools/repository-law-check.py` passed;
- `tools/document-census.py --summary --check` passed with zero unexplained current documents;
- `tools/oracle-index.py --check` passed at a short-path detached checkout of the same exact commit, reporting 86 documented hashes, 17 asserted hashes, and 69 document-only hashes;
- provider credential environment variables remained absent;
- every credentialless probe evidence root remained absent.

The first oracle invocation in the long validation worktree failed only at its Git revision/path stat operation with Windows `Filename too long`; it produced no assertion-loss result. The same guard then passed at `C:\Users\Wiryl\Sol Dev\E0V-cef3fc1` on the identical commit. This host/tooling behavior is recorded in `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

## Failed pre-promotion intermediate

An earlier pre-rebase candidate `4f5da121f1052550dd98b590b2d365bc5dd70598` passed Core but failed Harness 133/134 because one bounded-diagnostic test still referenced the removed `generation_config.response_format.text.mime_type` path. The stale test oracle was corrected before the final candidate was frozen. `4f5da121...` is not promoted and has no validation tag.

## Annotated validation tag

```text
tag        validation/e0a-gemini-structured-output-compatibility-native-arm64
tag object 2518fd82273a4c4fff7b891a0d659b79eef602fd
target     cef3fc15e31192a48aa3bddd99450b65a58bd8f1
object     annotated tag
```

The tag was created only after all native executable gates and the short-path oracle guard passed, and it was verified to peel to the exact validated checkout.
## Conclusion

Exact checkout `cef3fc15e31192a48aa3bddd99450b65a58bd8f1` is **machine-validated on native Windows ARM64** for the Gemini GenerateContent structured-output compatibility correction.

Native runtime authority attaches only to that exact source candidate and its annotated validation tag. This evidence file and later documentation/state reconciliation commits do not become machine-tested merely because they record the result.

Run 02 remains immutable and consumed. The corrected route is technically eligible for a fresh named 3.5 Flash-Lite full-reference activation gate, but provider authorization remains **NONE** until the Director separately authorizes that exact run under `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md`.
