# E0-A Phase B — Gemini Normative Reference Pending Native Validation

Status: **STATIC AUDIT CLOSED — PENDING DIRECTOR-MACHINE NATIVE WINDOWS ARM64 VALIDATION — NO GEMINI NETWORK EXECUTION AUTHORIZED**

Date: **2026-09-06**

## Exact implementation authority

Frozen source/test checkpoint:

`39e7984b9ff59228d7d0c424ef7d113582d38bc7`

Base promoted `main`:

`f9cb1a79ad7923b640ff1a97c46d8fd6ab9dac25`

Active branch:

`e0a-gemini-normative-reference-amendment`

The pending-validation evidence commit and any continuity-only descendant may sit after the frozen source/test checkpoint. Native validation must prove the exact checkout used and the diff from the frozen source/test checkpoint must remain documentation-only.

## Authority and scope

Approved amendment:

`docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`

Approval evidence:

`docs/evidence/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT_APPROVAL.md`

The amendment changes only the next normative E0-A provider route to Google Gemini API / stable `gemini-2.5-flash` for `CREATIVE-NONE`.

Core/H1 causal authority is unchanged. The source/test comparison from promoted `main` through the frozen checkpoint contains **zero `src/Ensemble.E0.Core` paths**. LOW/MEDIUM/HIGH Gemini characterization, E0-B+, product Application/persistence/UI, NPU, packaging, WACK, Store, and later scope remain excluded.

No Gemini network request, `countTokens` request, inference, credential use, or provider spend is authorized by this checkpoint.

## Static audit closure

The Gemini amendment was recursively audited against the frozen E0-A laws, current GitHub authority, and the current official Gemini API contract until one complete pass found no remaining material correction or worthwhile improvement.

Material corrections made before freeze include:

- preserved true `CREATIVE-NONE` through Gemini 2.5 Flash `thinkingBudget=0` rather than mapping to a nonzero Gemini 3.x reasoning level;
- kept native `generateContent` / `streamGenerateContent` because the normative experiment requires true thinking-off;
- limited the active Gemini live path to `CREATIVE-NONE`; LOW/MEDIUM/HIGH remain deferred;
- retained the 4,096 requested output limit per role and enforce a 4,096 contributing candidate+thought generated-token ceiling;
- retained conservative Integrity pre-spend reservation against the provider model's 65,536 output-token limit because thinking budget is advisory;
- bound `models.countTokens` to the exact prepared `GenerateContentRequest`;
- migrated structured output to the current `generationConfig.responseFormat.text` JSON-schema surface;
- pinned `candidateCount=1`;
- pinned `thinkingConfig.includeThoughts=false` for all normative roles;
- reject streamed thought material before raw diagnostic persistence;
- require complete, internally consistent usage accounting including provider `totalTokenCount` and reject unexpected tool-use prompt tokens;
- allow identity-absent intermediate stream chunks while requiring paired, stable `responseId` / `modelVersion` before success;
- accept canonical SSE `data:` framing with or without the optional space;
- reject semantic bytes after `STOP` while allowing a later metadata-only usage chunk;
- fail closed on wrong-typed and non-object provider responses without adding broad exception handling;
- record successful usage/receipt evidence before terminating on Gemini-specific cache, thinking, or generated-token policy violations;
- bind Gemini runtime manifests to the approved Gemini amendment and shadow-pricing/provider contract rather than the historical OpenAI Proposal 0.15 manifest tuple;
- refreshed `CURRENT_STATE.md` so fresh engineering chats distinguish historical OpenAI native authority from the current unvalidated Gemini amendment.

## Current provider-contract snapshot

Official Google documentation was reverified on 2026-09-06 for the implementation-relevant contract:

- Gemini 2.5 Flash supports `thinkingBudget=0` for thinking-off and the configured elevated Integrity thinking budget is within its documented range;
- `generateContent` / `streamGenerateContent` expose system instructions, generation configuration, `store`, response identity, usage metadata, and SSE streaming;
- structured output supports Gemini 2.5 Flash through JSON Schema and the current `responseFormat.text` surface;
- `candidateCount` is configurable and the Harness pins it to one;
- usage exposes prompt, cached, candidate, thought, tool-use prompt, and total token accounting; provider total is checked against prompt + candidate + thought;
- unspecified service tier resolves to standard and the exact request records the omission while the manifest records the resolved semantic tier;
- Gemini 2.5 implicit caching is provider-managed, so any nonzero cached-token observation is recorded and becomes technical/noncontributing before semantic consumption;
- the dated shadow-pricing/data-use snapshot remains subject to the short fail-closed freshness guard and mandatory pre-network re-verification.

This static provider-contract review is not runtime/provider evidence.

## Validation hierarchy

No compiler, test, native Windows ARM64, or Gemini runtime claim is made for `39e7984b9ff59228d7d0c424ef7d113582d38bc7` by this record.

Native validation must be performed on the Director's Windows ARM64 machine using the established host contract:

`docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`

At minimum the native validation must establish:

1. exact expected Git checkout and clean tracked/staged state;
2. no material untracked files under `src/`, `tests/`, or `fixtures/`;
3. `PROCESSOR_ARCHITECTURE=ARM64`;
4. parsed `dotnet --info` with `RID: win-arm64` and Host `Architecture: arm64`;
5. Core test suite result;
6. Harness test suite result;
7. Harness native ARM64 build result;
8. Missing Raft fixture smoke result;
9. generic fixture smoke result;
10. credentialless explicit `e0a-run CREATIVE-NONE` with `GEMINI_API_KEY` absent, expected native exit `1`, expected missing-key message, and no evidence root;
11. post-validation exact HEAD and clean tracked/staged state.

Expected-failure native stderr must use the established PowerShell host wrapper discipline: temporarily use `$ErrorActionPreference='Continue'`, redirect stdout/stderr, capture `$LASTEXITCODE` immediately, restore the previous preference, and assert exit/message/filesystem effects independently.

The unrelated root-level `patch0012-local-edit.txt` previously observed on the Director machine is nonmaterial under the established checkout-guard rule and must not be added to this work.

## Next gate

If native validation passes, record the exact Director-machine evidence without promoting static claims. Only after that may the Director decide whether to authorize a first real Gemini network execution.

A future authorization for provider execution must be explicit. It is not implied by this amendment, its implementation, this static audit closure, or native credentialless validation.
