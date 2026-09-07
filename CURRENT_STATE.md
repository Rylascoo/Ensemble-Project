# Ensemble Current State

Updated: 2026-09-07

## Authority

`Rylascoo/Ensemble-Project` is engineering authority. Frozen Blueprint 0.1 plus approved phase/patch blueprints govern architecture. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority.

Product/policy authority: `docs/PROJECT_AUTHORITY.md`. The Director owns product constitution, ODR resolution, provider admissibility, and decisions about what the product may do. Fresh engineering chats must resolve current `main` first and recursively audit material work.

## Current checkpoint

Phase: **E0-A Experimental Harness — Phase B.**

Pre-restructure closure promoted through PR #44; merge `372de76955407762b3f8b83fa2799a93a85e2f5c`.

Exact current machine-tested checkpoint:
`cc395a25162a0a682796bffb44060c799df0db32`

Annotated validation tag:
`validation/e0a-pre-restructure-closure-native-arm64`

Evidence:
`docs/evidence/E0A_PRE_RESTRUCTURE_CLOSURE_NATIVE_ARM64_VALIDATION.md`

Director Windows ARM64: Core 622/622 PASS; Harness 103/103 PASS; fresh `win-arm64` Harness build PASS; both fixture smokes PASS; credentialless Gemini refusal PASS; no evidence root, provider-network execution, inference, or spend. Validation gate run `34164258601` passed at the checkpoint; evidence-only commit `83e2b91f6fd377106a9ed8377c50e19aeb57f753` passed run `34166676374`. Cloud ARM64 results are compiler authority only; x64 Core tests are required regression checks, not native-runtime authority.

## Provider boundary

Google Gemini API is the sole current E0-A provider method. The historical OpenAI executable path is retired; reintroduction of OpenAI or another provider requires a future Director decision.

Only `CREATIVE-NONE` is authorized on the Gemini live path. Real credentials, `countTokens`, provider-network execution, inference, and spend remain **NOT AUTHORIZED**. Provider/account tier, availability, quota, pricing, and data-use terms require fresh verification before any authorized live run.

## Repository state

Closure removed completed E0-A handoffs, archived unambiguously superseded transition evidence, added active/archive document census, retired obsolete OpenAI source/test lineage while preserving provider-neutral invariants, and made deterministic Core x64 regressions CI-blocking without validation inflation.

GitHub plan-dependent `main` protection is deferred. Temporary branch `repo-pre-restructure-closure` remains only for continuity closeout and archive/retirement.

## Boundaries / next

Remain inside E0-A unless separately authorized. Do not enter E0-B..G, application/persistence/UI, Scene endings, Context optimization, Windows AI/NPU, packaging, WACK, or Store work. .NET 10 requires later Director approval. Stage remains design-lane authority.

Next: complete documentation-only continuity, archive and retire `repo-pre-restructure-closure`, then begin approved E-R1 repository/tooling/workflow restructuring from freshly resolved `main`. E-R1 does not authorize a real Gemini run or provider-policy change.
