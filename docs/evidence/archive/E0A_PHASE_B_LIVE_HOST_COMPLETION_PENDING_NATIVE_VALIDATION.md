# E0-A Phase B Live-Host Completion — Pending Native Validation

Status: **SUPERSEDED — DIRECTOR-MACHINE NATIVE WINDOWS ARM64 VALIDATION PASSED**

Date: 2026-09-06

This record preserves the pre-validation audit and assumptions for the E0-A Phase B reference-run live-host completion. Native validation subsequently passed at the exact executable/test checkout recorded below. This record does not authorize credentials, provider requests, network inference, or spend.

Canonical native validation evidence:

`docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_ARM64_VALIDATION.md`

## Authority

- Repository: `Rylascoo/Ensemble-Project`
- Current `main` resolved during this audit: `13ffde90f78a8bf40a04381b136e846d1cf64d1c`
- Active branch: `e0a-phase-b-live-host-completion`
- Static implementation checkpoint immediately before this evidence-only record lineage: `fc7fde0c8e6aa652d9d08f657d7150d2601985d6`
- Exact Director-machine-validated executable/test checkout: `1cfdb3aa22abce62a5bd48e80706407670d1c6a9`
- Approved architecture: `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`
- Approved architecture commit: `e1d0b4aea0f7ba29cf85e765bea33d14eab35fde`
- Blueprint law #35 remains intact: E0-A is one provider and one model only — `OpenAI` / `gpt-5.6-sol`.

No `Ensemble.E0.Core` file is changed by this live-host completion branch.

Later documentation-only descendants do not replace `1cfdb3aa22abce62a5bd48e80706407670d1c6a9` as the machine-tested executable/test authority.

## Pricing provenance and stale-snapshot behavior

Provider source consulted:

`https://developers.openai.com/api/docs/models/gpt-5.6-sol`

Verified: **2026-09-06**.

The source stated:

- text input: **$4.00 / 1M tokens**;
- cached input: **$0.40 / 1M tokens**;
- output: **$20.00 / 1M tokens**;
- cache writes: **1.25x the uncached input rate**;
- prompts with **>272K input tokens**: **2x input and 1.5x output** for the full request;
- GPT-5.6 Sol promotional pricing is available **at least through November 21, 2026**.

The live E0-A ceiling therefore uses a conservative standard-tier accounting snapshot of:

- input: **$5.00 / 1M tokens** (`$4.00 x 1.25`);
- cached-input published rate retained as provenance: **$0.40 / 1M tokens**;
- output: **$20.00 / 1M tokens**.

For ceiling enforcement and reconciliation, every reported input token is costed at the conservative $5.00/M rate rather than applying a cache-read discount. This deliberately overestimates when cache reads occur and avoids depending on undocumented overlap behavior among cache-detail fields. Provider billing remains external authority; Harness values are estimates, never claims of actual billed cost.

Any prepared invocation whose provider input-token preflight reports more than **272,000** input tokens is refused before inference, so this E0-A path cannot silently enter the provider's published long-context pricing tier.

### Does the run gate re-verify live pricing immediately before every authorized run?

**No.** The host does not perform a remote pricing-page fetch immediately before each run.

It fails closed on the frozen snapshot's published validity boundary: after **2026-11-21 UTC**, the live host refuses inference until the pricing constants/provenance are deliberately re-verified and updated. Inside that provider-guaranteed promotional window, the run uses the frozen audited snapshot.

**Known limit of this gate:** `RequireNonStaleSnapshot` is a **date-validity guard, not a live pricing-verification guard**. If OpenAI changes a pricing dimension that the ceiling depends on before 2026-11-21 — for example the cache-write multiplier, the 272,000-token long-context threshold, or another relevant rate/tier rule — the current host will not detect that change automatically. The promotional date floor is therefore not evidence that every pricing term remains unchanged throughout the window. Closing this residual requires the trusted short-lived pricing attestation described below, or an equivalent stable machine-readable provider pricing source. Until such a mechanism exists, Director authorization for a real run should treat the frozen snapshot as an audited assumption whose live terms may still require human/provider re-verification.

A stricter immediate-before-run live-pricing gate would require a new trusted pricing-attestation input or a stable machine-readable provider pricing source. No documented machine-readable pricing endpoint was identified in this verification. The safer implementation, if such a gate becomes required, is a short-lived Director/provider pricing attestation containing at minimum model ID, official source URI, observed input/cached/output rates, cache-write multiplier, long-context threshold/multipliers, verification timestamp, and a content/hash identity; the host would reject a missing, stale, mismatched, or differently sourced attestation before credential access or inference. Scraping a human-facing documentation page inside the Harness would be a brittle new network dependency and is not added here.

The run manifest freezes the pricing source URI, verification date, promotional guarantee date, published rates, cache-write multiplier, standard-tier threshold, conservative accounting method, and effective envelope rates.

### Pricing-constant oracle classification

`PricingPolicy_FreezesSourcePromotionAndConservativeRates` is intentionally a **constant-freeze tripwire**, not a behavioral pricing test. Its collection-based expected/actual tuple detects edits to the frozen source URI/date/rates/multiplier without matching updates to the test expectation. It does not establish that OpenAI's live pricing is unchanged, that the date guard is live verification, that cache behavior is correct, or that spend reconciliation behaves correctly at runtime.

Accordingly, the native Harness pass count must not be cited as behavioral pricing validation solely because this tripwire passes. Runtime pricing/spend behavior is covered by separate boundary/reconciliation tests and, for any real run, by the provider/evidence gates and the known stale-snapshot limitation above.

## Cache-write semantics

Provider-authored/generated source consulted:

`https://github.com/openai/openai-python/blob/main/src/openai/types/completion_usage.py`

This file is generated from OpenAI's OpenAPI specification. It describes prompt-token details as a **breakdown of tokens used in the prompt**, defines `cache_write_tokens` as the **unadjusted number of prompt tokens written to cache**, and defines `cached_tokens` as cached tokens present in the prompt.

Provider API reference also consulted for current cache controls:

`https://developers.openai.com/api/reference/cli/resources/responses/methods/create`

That reference states that `prompt_cache_options.mode = "explicit"` disables the default implicit cache breakpoint. The E0-A prepared request explicitly sets that mode.

Conclusion supported by provider documentation: cache-write tokens are reported as prompt/input-token detail, not as an additional token total on top of `input_tokens`.

The provider documentation consulted does **not** state that `cached_tokens` and `cache_write_tokens` are mutually exclusive categories. The implementation therefore does not require `cached + cacheWrite <= input`. Each detail may independently be at most `input_tokens`.

Spend enforcement does not rely on those detail categories forming a partition. It prices all reported input tokens at the conservative cache-write-capable input rate. A nonzero `CacheWriteTokens` value is still treated as a reservation/configuration mismatch: its usage and estimated spend are recorded first, then the run terminates technical before the provider output can be consumed semantically.

## Phase-B characterization variants

The approved architecture places `CREATIVE-LOW`, `CREATIVE-MEDIUM`, and `CREATIVE-HIGH` in Phase-B characterization rather than deferring them to a later phase.

The live host therefore requires one explicit variant selector:

- `CREATIVE-NONE` — Performer `none`, Interpreter `none`, Integrity `high`;
- `CREATIVE-LOW` — Performer `low`, Interpreter `low`, Integrity `high`;
- `CREATIVE-MEDIUM` — Performer `medium`, Interpreter `medium`, Integrity `high`;
- `CREATIVE-HIGH` — Performer `high`, Interpreter `high`, Integrity `high`.

All four variants retain the same provider, model, fixture, limits, retry policy, spend ceiling, transport controls, State Authority policy, and evidence law. `CREATIVE-NONE` remains the normative reference run; LOW/MEDIUM/HIGH are matched characterization only.

No fifth reasoning variant is accepted by the Phase-B live host.

## Provider/model pin and forward compatibility

The literal `OpenAI` / `gpt-5.6-sol` pin is an **E0-A reference-envelope configuration constraint**:

- `E0ARunEnvelope.Validate()` and its four approved factories enforce those exact literals;
- `E0ARoleProfile` itself only requires nonblank provider/model values;
- `IE0AProviderRolePort` has no OpenAI/model literal;
- `IE0AInputTokenCounter` has no OpenAI/model literal;
- `ConfiguredRoleAttemptBoundary` validates prepared-attempt/receipt identity and receipt shape, not a provider/model literal.

There is intentional E0-A transport coupling outside those neutral interfaces: `E0ARequestBuilder` emits the approved OpenAI Responses request shape and `OpenAIResponsesPort` is an OpenAI adapter. Therefore E0-B's separately labeled multi-provider comparison can add provider-specific adapters and request preparation without relaxing or unwinding the E0-A envelope pin or changing Core, but it should not assume the OpenAI request-body builder is itself provider-neutral.

No multi-provider support is implemented in this branch.

## Stale branch disposition

`e0a-phase-b-live-host-completion-2` was compared against the active branch. It is a strict stale ancestor with no unique commits of its own; the active branch is ahead and contains all of its reachable history plus subsequent corrections. It is **not authoritative** for E0-A live-host work.

The stale branch is intentionally left untouched for Director disposition. No branch ref was moved to make it appear current.

## Director validation host contract

Known Director-machine validation-host behavior is recorded in:

`docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`

Future native Windows ARM64 command sets for this host must be generated from that document. In particular, they must not use the blank-returning PowerShell `RuntimeInformation` property probes as architecture gates and must isolate expected native stderr from `$ErrorActionPreference = 'Stop'` while capturing `$LASTEXITCODE` immediately.

## Native validation result

Attempt 01 at `bd04749a20141d1f7d221700ad3f1d3ee9f33eb1` remains partial evidence only because the Harness test project did not compile and the credentialless failure wrapper was defective.

The corrected executable/test surface `1cfdb3aa22abce62a5bd48e80706407670d1c6a9` was subsequently validated on the Director's native Windows ARM64 machine with `OPENAI_API_KEY` absent:

- Core: **622/622 PASS**;
- Harness: **54/54 PASS**;
- native ARM64 Harness build: **PASS**;
- Missing Raft fixture smoke: **PASS**;
- generic fixture smoke: **PASS**;
- credentialless `e0a-run`: **PASS** — native exit `1`, expected refusal text captured, no evidence root created;
- post-validation HEAD unchanged at `1cfdb3aa22abce62a5bd48e80706407670d1c6a9`;
- post tracked/staged diff exits: `0` / `0`.

Canonical detailed evidence is `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_ARM64_VALIDATION.md`.

## Classification

The E0-A Phase B live-host completion is **native Windows ARM64 validated for the fake-only/credentialless scope at exact checkout `1cfdb3aa22abce62a5bd48e80706407670d1c6a9`**.

Real provider execution remains a separate Director credential/network/spend gate and is **not authorized by this record**.
