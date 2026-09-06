# E0-A Phase B Live-Host Completion — Promotion Handoff

Status: **DIVERGENCE RECONCILED — BOUNDED PROMOTION AUDIT CLEAN; REVIEWED PROMOTION PATH AUTHORIZED; PROVIDER GATE CLOSED**

Date: 2026-09-06

This handoff was corrected in place after `main` diverged from the live-host work branch. It is a transition contract, not a redesign brief.

## Authority and reconciled history

Former heads before reconciliation:

- `main`: `a3b5126dad440d2f75bef655fdc6e4ac7098b839`
- `e0a-phase-b-live-host-completion`: `9228c77a5b789c4c6a1e0417f889851c6b92730e`
- merge base: `13ffde90f78a8bf40a04381b136e846d1cf64d1c`
- divergence: branch 54 ahead / 4 behind.

Both pre-merge versions of `CURRENT_STATE.md` were read and reconciled as complementary authority. The union preserves:

- branch-only `docs/PROJECT_AUTHORITY.md` product-and-policy authority;
- main-only `docs/ODR_26_COST_GOVERNANCE_AND_PROVIDER_ADMISSION_WORKING_CONTRACT.md` unchanged;
- main-only ODR-26 entry in `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md`;
- main Authority pointers to `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md` and `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`;
- branch live-host checkpoint, validation-host continuity, pricing residual, and lane-ownership content.

The ODR-26 working contract was drafted outside its authoring surface's lane and does not carry the disclosure required by `docs/PROJECT_AUTHORITY.md`. Engineering does not adjudicate or edit that Director-owned product/policy input. It is preserved unchanged and reported to the Director for disposition.

Do not redesign Proposal 0.15. Blueprint law #35 remains intact: E0-A is one provider and one model only — `OpenAI` / `gpt-5.6-sol`.

## Exact machine-tested authority

The exact Director-machine-tested live-host executable/test checkpoint remains:

`1cfdb3aa22abce62a5bd48e80706407670d1c6a9`

Director-machine-sourced observations at that checkout:

- Windows `10.0.26200`;
- `PROCESSOR_ARCHITECTURE=ARM64`;
- .NET SDK `9.0.317`;
- `dotnet --info` RID `win-arm64` and Host Architecture `arm64`;
- tracked/staged tree clean and no material untracked source/test/fixture files;
- Core **622/622 PASS**;
- Harness **54/54 PASS**;
- Harness native ARM64 build PASS;
- Missing Raft fixture smoke PASS;
- generic fixture smoke PASS;
- credentialless explicit `e0a-run` PASS with native exit `1`, expected missing-`OPENAI_API_KEY` refusal, no evidence root, credential absent;
- post-validation exact HEAD unchanged and tracked/staged state clean.

The canonical record is `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_ARM64_VALIDATION.md`. No later documentation or merge SHA replaces `1cfdb3aa...` as machine-tested executable/test authority.

Attempt 01 at `bd04749a20141d1f7d221700ad3f1d3ee9f33eb1` remains partial evidence only: the Harness test project did not compile there and the credentialless wrapper did not establish the scripted exit/message/filesystem oracle. Do not promote it retrospectively.

## Post-checkpoint and Core verification

A direct compare from `1cfdb3aa22abce62a5bd48e80706407670d1c6a9` to former branch head `9228c77a5b789c4c6a1e0417f889851c6b92730e` shows exactly eight commits and only these documentation paths:

- `CURRENT_STATE.md`;
- `docs/PROJECT_AUTHORITY.md`;
- `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`;
- `docs/evidence/E0A_PHASE_B_LIVE_HOST_COMPLETION_PENDING_NATIVE_VALIDATION.md`;
- `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_ARM64_VALIDATION.md`;
- `docs/evidence/E0A_PHASE_B_LIVE_HOST_NATIVE_VALIDATION_ATTEMPT_01.md`;
- this handoff.

No source, test, or fixture file changed after `1cfdb3aa...`.

A direct compare from original Phase-B native checkpoint `3749210393282f6aa2ac4ceb0176b6adb5df189e` through `1cfdb3aa...` changes Harness/tests and documentation only; no `src/Ensemble.E0.Core` file appears. The four parallel-main commits after merge base are documentation-only. Core therefore remains unchanged by the live-host completion and by this reconciliation.

## Director Windows ARM64 host behavior contract

Preserve `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`. Future Director-machine validation commands must not rediscover the documented host quirks: the PowerShell `RuntimeInformation` probes have returned blank; trusted architecture checks are `PROCESSOR_ARCHITECTURE=ARM64` plus parsed `dotnet --info`; expected native stderr under `$ErrorActionPreference='Stop'` must be isolated and `$LASTEXITCODE` captured immediately.

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

## Bounded promotion audit

The promotion audit is intentionally limited to five checks:

1. no executable, test, or fixture surface changed after `1cfdb3aa...` on the branch — **PASS**;
2. the reconciliation preserves all required main-only and branch-only authority/state — **PASS**;
3. validation wording does not overstate what the Director-machine run proved — **PASS**;
4. pricing residual and constant-freeze-tripwire classifications survive verbatim — **PASS**;
5. Core is unchanged — **PASS**.

Do not reopen a general improvement pass. No further documentation commit is required or authorized merely to restate promotion after this reconciliation merge.

## Promotion path

The reconciled head may be promoted to `main` through the normal reviewed GitHub path. Review must confirm the PR contains the union and no unintended executable/test/Core changes. The reviewed promotion/merge SHA is repository history; it does not replace `1cfdb3aa...` as the exact machine-tested live-host executable/test checkpoint.

## Closed provider gate

Nothing in this handoff authorizes:

- `OPENAI_API_KEY`;
- any provider credential;
- network inference;
- paid provider execution;
- estimated or actual provider spend;
- the first behavioral/reference run.

After promotion, the first real reference-provider run remains a separate consequential action requiring explicit Director authorization for credentials, network execution, and spend.

Do not enter E0-B..G, multi-provider implementation, Application/persistence/UI, Windows AI/NPU, MSIX/WACK/Store, or later product scope.
