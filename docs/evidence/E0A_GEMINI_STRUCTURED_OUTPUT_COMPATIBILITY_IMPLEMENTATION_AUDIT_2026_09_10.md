# E0-A Gemini Structured-Output Compatibility — Implementation Audit

Date: 2026-09-10

Status: **PASS — EXACT SOURCE CANDIDATE `cef3fc15e31192a48aa3bddd99450b65a58bd8f1` — PROVIDER TRAFFIC NOT AUTHORIZED BY THIS RECORD**

## Scope

This audit reviews the correction frozen by `docs/blueprint/E0A_GEMINI_GENERATECONTENT_STRUCTURED_OUTPUT_COMPATIBILITY_AMENDMENT.md` after Q-E0A-03 Run 02 and the bounded compatibility diagnostics.

Base: `6fffffba7275a8612b0d4d43cd2498c0e5513ad4` (`origin/main` at final candidate freeze).

Candidate: `cef3fc15e31192a48aa3bddd99450b65a58bd8f1`.

Executable/test changes versus base are exactly:

- `src/Ensemble.E0.Harness/Run/E0ARequestBuilder.cs`;
- `tests/Ensemble.E0.Harness.Tests/E0ASpendAndRequestTests.cs`;
- `tests/Ensemble.E0.Harness.Tests/GeminiGenerateContentPortWireTests.cs`.

No Core, Fixture, prompt text, schema content/semantics, model catalog, thinking controls, output ceilings, countTokens projection, rate/spend/retry/fallback law, history semantics, or evidence-authority code changed.

## Implementation result

The generation request builder now emits `generationConfig.responseMimeType = "application/json"` and `generationConfig.responseJsonSchema = <the same exact existing JSON schema>` instead of the provider-rejected `generationConfig.responseFormat.text` envelope.
The request-builder tests now positively require `responseMimeType` and `responseJsonSchema` and negatively reject `responseFormat`, preserving the correction as an executable oracle.

The bounded generation-failure diagnostic test continues to prove validated outbound-field retention, but its synthetic field path is now `generation_config.response_mime_type`, which actually resolves against the corrected outbound request. The older countTokens parser fixture containing a synthetic `responseFormat` path remains intentionally unchanged because it exercises parser behavior rather than current generation construction.

## Recursive audit and failed intermediate

The first pre-rebase candidate, `4f5da121f1052550dd98b590b2d365bc5dd70598`, was native-tested before promotion and failed the Harness suite at 133/134. The failing test still supplied `generation_config.response_format.text.mime_type`; the bounded parser correctly discarded that field because the corrected outbound request no longer contained it.

That failure falsified the stale test assumption, not the runtime correction. The test was repaired narrowly to `generation_config.response_mime_type`; targeted regression and the complete Harness suite then passed before final candidate freeze. `4f5da121...` receives no validation authority and no validation tag.

While the repair was pending, `origin/main` advanced through the independent Administrator C3 merge to `6fffffba...`. The Engineering package was rebased onto that exact main, preserving both C3 continuity and the Q-E0A-03 correction. The final candidate is a single commit directly parented on `6fffffba...`.

Final pre-commit checks at the rebased candidate passed:

- targeted stale-oracle regression;
- complete Core suite;
- complete Harness suite;
- byte/UTF-8 hygiene and `git diff --check`;
- repository-law check;
- document census with zero unexplained current documents;
- oracle-index assertion-coverage guard;
- semantic search confirming runtime `responseFormat` construction is absent and corrected legacy fields are present;
- no Core or Fixture diff.
## Conclusion

The smallest provider-evidence-supported correction is implemented without changing the experiment's semantic contract. Implementation audit result: **PASS**.

Native Windows ARM64 validation of the exact candidate is recorded separately in `docs/evidence/E0A_GEMINI_STRUCTURED_OUTPUT_COMPATIBILITY_NATIVE_ARM64_VALIDATION_2026_09_10.md`.

This audit creates no provider authorization. Run 02 remains immutable and consumed; any new full-reference run requires a fresh named Director authorization under the Q-E0A-03 activation/closure contract.
