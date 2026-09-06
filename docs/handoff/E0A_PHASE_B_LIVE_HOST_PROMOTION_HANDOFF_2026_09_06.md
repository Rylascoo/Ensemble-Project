# E0-A Phase B Live-Host Completion — Promotion Handoff

Status: **FRESH-CHAT TRANSITION — FINAL AUDIT AND PROMOTION PENDING**

Date: 2026-09-06

This handoff is a transition contract, not a redesign brief. It preserves the completed E0-A Phase B live-host work through fake-only/native validation and hands the next chat the final audit/promotion task.

## Mission

Resume from branch:

`e0a-phase-b-live-host-completion`

Do not redesign Proposal 0.15. Do not widen into E0-B..G or product Application/persistence/UI scope. Do not execute a real provider request.

The next task is:

1. resolve current `main` and the current live-host branch head;
2. fresh-read the authority/evidence/source surfaces listed below;
3. recursively audit the complete branch diff for correctness, consistency, authority, scope, validation wording, pricing residuals, host apparatus continuity, simplicity, and hygiene until one complete pass finds no material correction or worthwhile improvement;
4. verify that no executable/test surface changed after the exact Director-machine-tested checkpoint;
5. if clean, promote the branch to `main` through the normal reviewed GitHub path;
6. preserve the exact machine-tested executable/test SHA separately from any later documentation/merge SHA.

Promotion does **not** authorize credentials, provider-network execution, or spend.

## Read first

Fresh-read in this order from the active branch:

1. `CURRENT_STATE.md`
2. `docs/PROJECT_AUTHORITY.md`
3. `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`
4. `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`
5. `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_APPROVAL.md`
6. `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_ARM64_VALIDATION.md`
7. `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`
8. `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_VALIDATION_ATTEMPT_01.md`
9. `docs/evidence/E0A_PHASE_B_LIVE_HOST_COMPLETION_PENDING_NATIVE_VALIDATION.md` as the historical transition record now superseded by the final native PASS.

Then fresh-read the changed live-host Harness/test surface rather than relying on prior chat summaries:

- `src/Ensemble.E0.Harness/Evidence/E0AEvidenceStore.cs`
- `src/Ensemble.E0.Harness/Evidence/E0AExistingEvidenceEvaluationSealer.cs`
- `src/Ensemble.E0.Harness/Host/E0AReferenceRunHost.cs`
- `src/Ensemble.E0.Harness/Host/E0ARepositoryCheckoutGuard.cs`
- `src/Ensemble.E0.Harness/OpenAI/OpenAIResponsesPort.cs`
- `src/Ensemble.E0.Harness/Program.cs`
- `src/Ensemble.E0.Harness/Run/E0AReferenceRunDriver.cs`
- `src/Ensemble.E0.Harness/Run/E0ARequestBuilder.cs`
- `src/Ensemble.E0.Harness/Run/E0ARunEnvelope.cs`
- `src/Ensemble.E0.Harness/Run/E0ASpendLedger.cs`
- `src/Ensemble.E0.Harness/Run/RoleAttemptModels.cs`
- `tests/Ensemble.E0.Harness.Tests/E0AEvidenceTests.cs`
- `tests/Ensemble.E0.Harness.Tests/E0ALiveHostReadinessTests.cs`
- `tests/Ensemble.E0.Harness.Tests/E0ASpendAndRequestTests.cs`

Use `Rylascoo/Ensemble-Project` only for engineering/product truth. Do not infer current authority from historical chat text when the repository can answer it.

## Authority state

At handoff preparation time, `main` resolved to:

`13ffde90f78a8bf40a04381b136e846d1cf64d1c`

Resolve it again in the fresh chat because parallel work may advance `main`.

The active branch is `e0a-phase-b-live-host-completion`. The last pre-handoff branch checkpoint was documentation-only head:

`4e800e4b646b7ce8ca20ee84e4fe8cc44f1894f5`

The handoff commit itself is a later documentation-only descendant; resolve the exact current branch head rather than treating `4e800e4...` as final authority.

`e0a-phase-b-live-host-completion-2` is a strict stale ancestor with no unique commits. Leave it untouched for Director disposition.

## Product and policy authority

`docs/PROJECT_AUTHORITY.md` now explicitly states:

- the Director owns product constitution;
- the Director owns Open Design Register resolution;
- the Director owns provider admissibility;
- the Director owns decisions about what the product may do;
- engineering and design surfaces supply inputs within their own lanes and do not author product/policy authority;
- ODR proposals and resolutions are filed in `Ensemble-Project/docs` following the ODR-33 precedent regardless of originating lane;
- an input drafted outside its authoring surface's lane must say so in the document.

Do not let engineering convenience silently become product or policy authority during the promotion audit.

## Exact machine-tested authority

The exact Director-machine-tested **live-host executable/test checkpoint** is:

`1cfdb3aa22abce62a5bd48e80706407670d1c6a9`

Director-machine-sourced observations at that checkout:

- Windows `10.0.26200`;
- `PROCESSOR_ARCHITECTURE=ARM64`;
- `.NET SDK 9.0.317`;
- `dotnet --info` RID `win-arm64`;
- Host Architecture `arm64`;
- tracked tree clean;
- staged tree clean;
- no material untracked files under `src/`, `tests/`, or `fixtures/`;
- unrelated root scratch `patch0012-local-edit.txt` present and non-material;
- Core: **622/622 PASS**;
- Harness: **54/54 PASS**;
- Harness native ARM64 build: PASS;
- Missing Raft fixture smoke: PASS;
- generic fixture smoke: PASS;
- credentialless explicit `e0a-run`: PASS with native exit `1`, expected missing-`OPENAI_API_KEY` refusal, no evidence root, credential absent;
- post-validation exact HEAD unchanged and tracked/staged state clean.

The final evidence record is:

`docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_ARM64_VALIDATION.md`

Do not replace `1cfdb3aa...` with a later documentation or merge SHA as the machine-tested executable/test authority unless the executable/test surface actually changes and is rerun natively.

## Post-validation commits

At handoff preparation, every commit after `1cfdb3aa...` was documentation-only. These records added/updated:

- Director Windows ARM64 validation-host behavior;
- final live-host native evidence;
- Attempt 01 classification;
- closure/supersession of the pending-native-validation record;
- project product/policy authority;
- `CURRENT_STATE.md`;
- this handoff.

Fresh-chat audit must verify this statement by commit/file comparison. If any source or test file appears after `1cfdb3aa...`, stop treating the prior native PASS as validation of that changed executable/test surface.

## Attempt 01 — historical apparatus evidence

Attempt 01 at:

`bd04749a20141d1f7d221700ad3f1d3ee9f33eb1`

is deliberately **partial** evidence only.

It established Core 622/622, production Harness ARM64 build PASS, and both fixture smokes PASS, but:

- the Harness test project failed to compile because seven `MSTEST0032` diagnostics correctly identified direct literal-vs-constant assertions as compile-time tautologies;
- therefore all Harness assertions at that checkout were unverified;
- the no-key smoke visibly refused the credential but was not counted because `$ErrorActionPreference='Stop'` broke the scripted native stderr/exit-code oracle.

Do not promote Attempt 01 into a pass retrospectively.

## Director Windows ARM64 host behavior contract

Future native validation commands on the Director host must be generated from:

`docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`

Known facts:

- Windows PowerShell expressions using `RuntimeInformation.OSArchitecture`, `.ProcessArchitecture`, and `.RuntimeIdentifier` have returned blank on this host and are not valid architecture gates.
- Trusted architecture probes are `PROCESSOR_ARCHITECTURE=ARM64` plus parsed `dotnet --info` showing `RID: win-arm64` and Host `Architecture: arm64`.
- Expected native stderr can surface as `NativeCommandError` under `$ErrorActionPreference='Stop'`.
- Expected-failure probes must temporarily isolate that behavior, redirect stdout/stderr, capture `$LASTEXITCODE` immediately, restore the prior preference, and assert exit/message/filesystem side effects independently.

Do not spend another Director-machine cycle rediscovering these apparatus facts.

## Phase-B live-host truths to preserve

Blueprint law #35 remains intact: E0-A is one provider and one model only:

`OpenAI` / `gpt-5.6-sol`

This is the experimental control and must not be relaxed in this handoff/promotion work.

Exactly four Phase-B variants exist:

- `CREATIVE-NONE`: Performer/Interpreter `none`, Integrity `high`; normative reference.
- `CREATIVE-LOW`: Performer/Interpreter `low`, Integrity `high`; characterization.
- `CREATIVE-MEDIUM`: Performer/Interpreter `medium`, Integrity `high`; characterization.
- `CREATIVE-HIGH`: Performer/Interpreter `high`, Integrity `high`; characterization.

Other frozen limits include:

- 12 accepted-Turn cap;
- 1 attempt per probabilistic role invocation;
- 0 automatic retries;
- 300-second attempt timeout including token preflight + inference;
- 4096 max output tokens per role;
- USD 5 estimated model-token spend ceiling per run;
- `store=false`;
- `service_tier=default`;
- truncation disabled;
- no tools/provider conversation/hidden reasoning;
- Performer/Interpreter streaming, Integrity buffered;
- Harness-only provider/network/filesystem/secrets boundary;
- technical/cancelled outputs carry no semantic payload;
- deterministic State Authority/review rejection;
- accepted causal commit followed immediately by next Opportunity establishment;
- immutable local evidence and blind transcript separation.

The live host adds:

- explicit `e0a-run` variant selection;
- exact clean-checkout provenance before secret access;
- evidence-root and fixture validation before secret access;
- credential access confined to the explicit live-run path;
- explicit prompt-cache mode;
- cache-write usage preservation and conservative spend accounting;
- >272K input refusal before inference;
- pricing date-validity guard before credential access;
- independent post-run `e0a-evaluate` sealing with runtime-root verification.

No Core file changed in this live-host completion.

## Pricing residual — do not erase

Pricing source was verified 2026-09-06 against OpenAI's GPT-5.6 Sol promotional pricing. The Harness conservative rates are frozen as $5.00/M input, $0.40/M cached-input provenance, and $20.00/M output, with the 272K standard-tier boundary and 1.25x cache-write multiplier recorded.

`RequireNonStaleSnapshot` is a **date-validity guard, not live pricing verification**.

It fails closed after 2026-11-21 UTC, but cannot detect an OpenAI change to the cache-write multiplier, 272K threshold, rate, or other relevant pricing rule that occurs earlier inside that date window.

The identified closure mechanism is a trusted short-lived pricing attestation or equivalent stable machine-readable provider pricing source. HTML pricing scraping was deliberately not added.

Do not weaken this residual in evidence or promotion language.

## Pricing test classification

`PricingPolicy_FreezesSourcePromotionAndConservativeRates` is a **constant-freeze tripwire**, not behavioral pricing validation.

Its pass contributes to the 54/54 Harness count but only proves that the compiled frozen constants match the separately maintained expected tuple in the test. It does not prove:

- live OpenAI pricing freshness;
- provider cache behavior;
- runtime spend correctness;
- the date guard performs live pricing verification.

Behavioral spend/cache boundaries are separate tests/evidence. Never cite the 54/54 suite count as more pricing coverage than it contains.

## Provider-pin forward compatibility boundary

The `OpenAI` / `gpt-5.6-sol` literals are enforced by the E0-A run envelope/factories. `E0ARoleProfile`, `IE0AProviderRolePort`, `IE0AInputTokenCounter`, and `ConfiguredRoleAttemptBoundary` do not themselves impose those literals.

There is intentional E0-A OpenAI coupling in `E0ARequestBuilder` because it emits the OpenAI Responses request shape, and `OpenAIResponsesPort` is an OpenAI adapter.

This means the E0-A pin is configuration-local at the role/envelope boundary but the current request builder is not provider-neutral. E0-B can later add separately labeled provider-specific preparation/adapters without relaxing E0-A or changing Core. Do not build that now.

## Recursive audit axes before promotion

A clean promotion decision requires one complete pass finding no material correction or worthwhile improvement across at least:

- current `main` ancestry and parallel-work reconciliation;
- Blueprint/Proposal 0.15 authority;
- `docs/PROJECT_AUTHORITY.md` product/policy lane ownership;
- no-Core-change boundary;
- exact machine-tested checkpoint preservation;
- post-checkpoint docs-only claim;
- live-host CLI variant coverage;
- checkout provenance before secret access;
- credential/evidence/fixture gate ordering;
- pricing date guard and known live-pricing residual;
- >272K long-context fail-closed behavior;
- cache-write semantics/accounting and semantic isolation;
- request/receipt association;
- model identity handling;
- cancellation/timeout isolation;
- State Authority and causal commit synchronization;
- evidence write-once/runtime-root/evaluation sealing;
- blind-output separation;
- constant-freeze-tripwire evidence wording;
- Director-host validation apparatus continuity;
- simplicity/hygiene/no accidental scope widening;
- ARM64 suitability;
- five project properties: character continuity, bounded perspective, agency without hidden authorship, causal persistence, creator sovereignty.

If a material executable/test defect is found, correct it narrowly and return to Director-machine validation. If only documentation is corrected, do not manufacture a native-validation requirement unless the executable/test authority actually changes.

## Promotion gate

If the recursive audit is clean:

1. resolve latest `main` again;
2. reconcile any legitimate parallel-main documentation authority without overwriting the validated runtime/test surface;
3. verify final branch vs `main` has no unintended Core or unrelated scope;
4. create/review the promotion PR;
5. promote to `main` through the normal GitHub path;
6. record the resulting promotion/merge SHA in `CURRENT_STATE.md` or an evidence closure record if needed;
7. keep `1cfdb3aa...` identified as the exact machine-tested live-host executable/test checkpoint.

## Closed provider gate

Nothing in this handoff authorizes:

- `OPENAI_API_KEY`;
- any provider credential;
- network inference;
- paid provider execution;
- estimated or actual provider spend;
- the first behavioral/reference run.

After promotion, the first real reference-provider run remains a separate consequential action requiring explicit Director authorization for credentials, network execution, and spend.

Do not substitute a current provider answer, fake run, or readiness statement for that authorization.

## Fresh-chat completion condition

The handoff is complete when the fresh chat has either:

- recursively audited and promoted the validated live-host completion cleanly while preserving exact machine-test authority and the closed provider gate; or
- found a material issue, recorded it precisely, and stopped at the appropriate correction/Director/native-validation gate without widening scope.
