# E0-E Single-Model Playwright Control — Preparation Closeout Audit

Status: **CLOSED — PREPARATION VALIDATED; E0-E EXECUTION BLOCKED**

Date: **2026-09-09**

## Authority and scope

This record closes Q-E0E-PREP only. It does not execute E0-E, consume a provider request, handle credentials, spend money, produce a behavioral transcript, score or unblind an arm, alter E0-A through E0-D, or grant product/runtime authority.

Frozen contract: `docs/blueprint/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_CONTRACT.md`.

## Prepared control

The preconstructed control is deliberately throwaway and isolated under `experiments/Ensemble.E0.PlaywrightControl*`. It references public Core Fixture types only; Core semantics, the E0-A Harness, provider adapters, product surfaces, UI, and design repositories are unchanged.

The frozen preparation implements:

- sealed eventual E0-A reference-configuration binding rather than stale provider/model constants;
- exact Fixture id/version/hash binding;
- an iterative single-model playwright with one visible Character performance per invocation;
- playwright-owned next-speaker selection with no deterministic Director Opportunity routing;
- complete relevant Scene/Character disclosure while preserving semantic categories and Character knowledge boundaries;
- explicit exclusion of Fixture `initialOpportunityCharacterId` from model-visible Scene packets so Ensemble routing state cannot cue speaker selection; the identity remains provenance-only for later live evidence;
- exact one-attempt / zero-retry / 300-second timeout / USD 5.00 ceiling discipline and a maximum visible-output ceiling of 4096 tokens;
- strict structured-output parsing, exact three-Character roster enforcement, Unicode/NFC/control-character hygiene, sequential turn/cap enforcement, and bound prepared-request identity;
- E0-A-compatible neutral-label blind transcript shape with identity metadata excluded;
- no direct `System.Net.Http` dependency, provider adapter, key handling, generation path, scoring path, or unblinding path during preparation.

Frozen prompt SHA-256: `aaa94e69e18512ca54de1bf2288546b57a8d8a53075338f046a3ea12690eefa2`.
Frozen schema SHA-256: `34bd76d442f1eeffc07049092c1d03a769f33a8e279ff2dd8765a0449213d219`.

## Recursive audit corrections

Preparation was not accepted on first construction. The recursive pass exposed and corrected:

1. repository-authority reachability lost by an over-compact `CURRENT_STATE.md` refresh;
2. an incorrect MSTest exception-assertion API spelling;
3. missing fail-closed bounds on reference max-output and prepared-turn Scene/reference pairing;
4. a comparison confound in which `initialOpportunityCharacterId` was model-visible and could partially reintroduce Director routing.

After correction, one further standard-gate oracle failure was traced to the deliberate history-normalization force update: the push event carried an unreachable transient `BASE_SHA`, so the oracle checker failed before evaluating assertions. No oracle coverage was lost. Validation was rerun from a fast-forward identical-tree anchor, restoring a reachable baseline and passing the oracle check.

## Hosted validation

Final implementation content tree: `90c887358e973f7c4e3b1cd7ba2f56e0028affe9`.

Substantive finalization commit: `1747cff1d48d24a4077a6d33ffd8eea8946a9499`.
Dedicated E0-E preparation run: GitHub Actions `34314008749` — **SUCCESS**:

- throwaway control `win-arm64` cross-compile: **PASS**, 0 warnings / 0 errors;
- deterministic preparation test build: **PASS**, 0 warnings / 0 errors;
- deterministic preparation tests: **10/10 PASS**, 0 failed, 0 skipped.

Fast-forward validation anchor: `b24522de9c0845644ff2b05c8041a76322e6ac95`, with the exact same tree `90c887358e973f7c4e3b1cd7ba2f56e0028affe9`.
Standard repository validation run: GitHub Actions `34314069624` — **SUCCESS**:

- repository law enforcement: PASS;
- document authority census: PASS;
- oracle assertion coverage: PASS;
- required x64 Core regression: PASS;
- standard ARM64 cross-compile compiler gate: PASS.

These are hosted compiler/static/deterministic preparation results only. They are **not** native Windows ARM64 runtime validation and do not replace or extend `docs/VALIDATION_LEDGER.md` native authority.

## Traffic and evidence boundary

Provider traffic: **NONE**.
Credentials used: **NONE**.
Provider spend: **USD 0**.
E0-E behavioral transcripts: **NONE**.
Scoring/unblinding: **NONE**.

The promoted native E0-A executable authority remains unchanged at `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a` / `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64`.

## Final recursive audit

One complete post-correction pass found no remaining material defect or worthwhile simplification in correctness, experimental isolation, reference binding, complete-Scene semantics, speaker-selection independence, output/failure discipline, provenance, blinding, retry/spend limits, throwaway residency, ARM64 build suitability, authority, or sequencing.

Q-E0E-PREP therefore satisfies its stop condition and may be marked **DONE**. Q-E0E-RUN remains **BLOCKED** until E0-A, E0-B, E0-C, and E0-D are durably closed, an exact contributing E0-A reference configuration exists, execution-time provider/account/pricing/quota facts are current, and any required provider traffic has explicit Director authorization.
