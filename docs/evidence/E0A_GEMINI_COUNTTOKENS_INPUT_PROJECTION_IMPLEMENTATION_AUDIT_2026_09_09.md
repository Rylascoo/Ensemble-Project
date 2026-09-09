# E0-A Gemini `countTokens` Input Projection Correction — Implementation Audit

Status: **RECURSIVE IMPLEMENTATION AUDIT COMPLETE — HOSTED GREEN — NATIVE WINDOWS ARM64 VALIDATION PENDING**

Date: **2026-09-09**

## Authority and scope

Frozen contract: `docs/blueprint/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_AMENDMENT.md`.

Evidence trigger: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`.

Implementation branch: `e0a-gemini-counttokens-input-projection-correction`.

Source/test implementation checkpoint: `6db7d8b6ac2cbe84f0ff5c8d99754a938a0f59f7`.

Continuity-merged audited checkpoint: `2f217c6857fdba6f7e309ea8d56d82d5512b7c4d`.

The merge from current `main` into the engineering branch incorporated only the two concurrent Design-owned continuity updates in `CURRENT_STATE.md` and `docs/PROJECT_EXECUTION_QUEUE.md`. It did not alter production source, Harness tests, Core, fixtures, provider policy, or E0 execution semantics.

## Hosted validation

The source/test implementation checkpoint passed hosted Validation gate `34383337117`.

The continuity-merged audited checkpoint passed hosted Validation gate `34383978165`.

Both gates passed:

- ARM64 cross-compilation;
- required x64 Core regression;
- repository law enforcement;
- oracle assertion coverage;
- document authority census.

Hosted CI compiles the Harness test project but does not execute the complete Harness suite. Therefore these hosted results are necessary but not native-runtime authority.

## Exact production change

Only `src/Ensemble.E0.Harness/Gemini/GeminiGenerateContentPort.cs` changes production behavior.

`CountTokensRequestBody` now:

1. requires the prepared request root to be an object and to omit top-level `model`;
2. requires exactly four distinct top-level properties;
3. requires the current properties `systemInstruction`, `contents`, `generationConfig`, and `store`;
4. requires `systemInstruction` to be an object, `contents` an array, `generationConfig` an object, and `store` exactly JSON `false`;
5. rejects any missing, duplicate, additional, or noncanonical current surface before HTTP;
6. emits `generateContentRequest` containing only:
   - `model = models/<exact profile model>`;
   - the exact prepared `systemInstruction` value;
   - the exact prepared `contents` value.

`generationConfig` and `store` are deliberately omitted from the token-count request under the frozen amendment. They remain present in the prepared generation body.

## Generation immutability

No generation execution method changed.

`ExecuteBufferedAsync` continues to send `attempt.RequestBody` directly to `generateContent`.

`ExecuteStreamingAsync` continues to send `attempt.RequestBody` directly to `streamGenerateContent`.

`E0ARequestBuilder` is unchanged. The prepared generation request therefore continues to contain the frozen:

- `candidateCount = 1`;
- role output-token ceiling;
- structured `responseFormat.text.mimeType = application/json`;
- structured response schema;
- thinking configuration;
- `store = false`.

Attempt 04 never reached generation, so this correction does not reinterpret its `countTokens` rejection as evidence against the generation payload.

## Regression-oracle audit

`tests/Ensemble.E0.Harness.Tests/GeminiGenerateContentPortWireTests.cs` replaces the falsified full-projection oracle with an input-projection oracle and adds request-surface drift coverage.

The audit maps the frozen oracle as follows:

1. **Nested model/systemInstruction/contents** — directly asserted by `CountTokens_ProjectsOnlyInputSemanticsAndUsesApiKeyHeader`.
2. **Omit generationConfig/store** — directly asserted by the same test.
3. **Generation request remains frozen** — inherited direct request-shape assertions still prove `generationConfig`, response format/schema, thinking control, output ceiling, and `store=false` remain in the prepared generation request; no generation source changed.
4. **Unknown/additional surface fails before HTTP** — covered with an added `tools` top-level property and zero captured HTTP requests.
5. **Missing/duplicate/noncanonical current surface fails before HTTP** — representative regressions cover missing `store`, duplicate `contents`, and `store=true`; the production guard is structural and applies to every missing/duplicate/additional property class.
6. **Bounded structured diagnostic remains valid against corrected body** — synthetic BadRequest field path was corrected to `generateContentRequest.contents[0].parts[0].text`, which exists in the actual outbound body.
7. **Provider prose/secrets remain excluded** — existing secret message/description assertions remain.
8. **Current model-route nested-model coverage remains** — existing `GeminiModelComparisonTests` are unchanged and continue to exercise the route-specific nested model.
9. **No Core/fixture semantics change** — repository diff contains no Core, Core-test, or fixture change.
10. **Complete regressions** — hosted Core regression and compiler/law/census/oracle gates pass; complete native Core + Harness execution remains intentionally pending.

The expected Harness-suite count after this test addition is **131** if no concurrent Harness-test change occurs before native checkout. The native run, not this static count, is authoritative.

## Bounded diagnostic and privacy audit

The bounded diagnostic implementation itself is unchanged.

Because field-path validation resolves against the actual outbound token-count body, deliberately omitted `generationConfig` paths are no longer evidence-eligible for a corrected request. The synthetic regression now uses an in-body `contents` path.

No provider message, description, raw response body, header, URI, credential, environment value, or arbitrary provider prose becomes evidence-eligible. No new diagnostic channel was introduced.

## Rate, retry, spend, evidence and sequencing audit

No change was made to:

- request-attempt count;
- automatic retry law;
- fallback/model/provider selection;
- rate discipline;
- spend ceiling or output reservation;
- provider timeout/cancellation behavior;
- evidence contracts or runtime seal;
- accepted-history/causal state semantics;
- experiment ordering;
- provider authorization.

Provider authorization remains **NONE**.

## Branch and continuity audit

During implementation, `main` advanced by two Design-only continuity commits. The engineering branch was initially 3 ahead / 2 behind.

A forced or lossy reconciliation was rejected. The two shared files were explicitly merged so that:

- Q-E0A-04 engineering authority and queue state were preserved;
- the latest Design state, Q-DESIGN-13 DONE / Q-DESIGN-14 ACTIVE, was preserved;
- both Design commits became ancestors of the engineering branch;
- no source/test semantics changed in the merge.

After reconciliation, the branch was 4 commits ahead / 0 behind the then-current `main`, with merge base equal to `main`.

## Recursive audit result

The completed recursive pass checked:

- evidence-to-diagnosis fit;
- provider-contract interpretation as frozen by the amendment;
- source minimality;
- generation immutability;
- fail-closed request-surface behavior;
- diagnostic/privacy compatibility;
- token/spend boundary;
- retry/rate/provider sequencing;
- Core/fixture exclusion;
- test-oracle alignment;
- ARM64 suitability;
- branch topology and cross-lane continuity;
- authority reachability and hosted validation.

No material source correction or worthwhile scope expansion remains before native validation.

## Native gate

Q-E0A-04 is **not complete** until the exact documentation-inclusive correction checkout passes the established Director-machine Windows ARM64 package:

- complete Core tests;
- complete Harness tests;
- fresh Debug Harness `net9.0/win-arm64` build after stale build-output cleanup;
- frozen Missing Raft fixture smoke;
- generic E0 smoke fixture;
- credentialless live-profile boundary checks;
- retired-route pre-credential rejection;
- evidence-root absence assertions for credentialless probes;
- checkout/credential/worktree hygiene.

No Gemini/OpenAI credential or provider network request belongs in this validation.

If native validation is clean, create a new annotated validation tag bound to the exact machine-tested checkout, then update native evidence, `docs/VALIDATION_LEDGER.md`, `CURRENT_STATE.md`, and Q-E0A-04 closeout. Provider traffic would still require a separate explicit Director authorization.