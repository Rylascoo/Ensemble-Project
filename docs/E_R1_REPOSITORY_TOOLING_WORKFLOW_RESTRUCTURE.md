# E-R1 Repository / Tooling / Workflow Restructure

Status: COMPLETE — PROMOTED 2026-09-07
Entered: 2026-09-07
Baseline: `db4bb1c5d5cb6f6de230da88f1c0352fb75aac23`
Historical work branch: `repo-restructure-e-r1`
Promotion: PR #46 merge `21a10aff823734418f36284744a1fd26aef3bcf6`
Closure evidence: `docs/evidence/E_R1_REPOSITORY_TOOLING_WORKFLOW_RESTRUCTURE_EVIDENCE.md`

## Completion

E-R1 completed Phases 0–5 without changing any file under `src/`, `tests/`, or `fixtures/` relative to its entry baseline. The exact native Windows ARM64 machine-tested checkpoint therefore remains `cc395a25162a0a682796bffb44060c799df0db32` under `validation/e0a-pre-restructure-closure-native-arm64`.

Audited implementation/tooling head `fd655bae81064378a0c58fc627e43dbdd88fef58` passed gate `34169804442`; documentation-inclusive head `d0141d5cf51c3fb88fd400362a70c5ce1f9ff501` passed branch gate `34170023136` and PR gate `34170086319`. PR #46 then promoted E-R1 to `main` at merge `21a10aff823734418f36284744a1fd26aef3bcf6`.

Post-merge continuity and branch retirement are repository-surface administration only; they do not reopen E-R1 or authorize runtime/provider work. The completed branch must be preserved by annotated `archive/repo-restructure-e-r1` at its final head before deletion.

## Authority and purpose

`CURRENT_STATE.md` identified E-R1 as the approved next engineering action. A repository search at E-R1 entry found no earlier E-R1 work-package artifact on `main`; this document made the corrected work-package boundary durable so future engineering chats do not depend on chat history.

E-R1 improves repository structure, authority clarity, documentation lifecycle, mechanical hygiene enforcement, and CI/tooling coherence. It does **not** redesign Ensemble runtime architecture or provider policy.

Higher authority remains unchanged: frozen Blueprint/approved phase specifications -> `CURRENT_STATE.md` -> source/tests/fixtures/commits/PR evidence -> decision/evidence records -> design evidence -> historical material.

## Entry facts

At the exact E-R1 baseline:

- pre-restructure closure and continuity were promoted;
- the exact native Windows ARM64 machine-tested executable/test checkpoint was `cc395a25162a0a682796bffb44060c799df0db32` under `validation/e0a-pre-restructure-closure-native-arm64`;
- completed branch `repo-pre-restructure-closure` was archived and retired;
- prior to creating this work branch, the remote branch surface contained only `main`;
- `CURRENT_STATE.md` was compact and remained the sole phase/checkpoint/validation/next-action authority;
- the solution had four projects: Core, Harness, Core.Tests, Harness.Tests;
- CI already contained ARM64-target compiler checks, required x64 Core regression tests, oracle-documentation drift protection, and a document-reference census;
- `docs/evidence/archive/` existed and contained explicitly historical E0-A evidence;
- historical OpenAI executable/test support was retired; Gemini was the sole current E0-A provider method;
- real Gemini credentials/network execution/inference/spend remained unauthorized;
- GitHub plan-dependent `main` protection was deferred and did not block E-R1.

## Corrections to the originally discussed restructure shape

Current repository facts superseded stale assumptions. E-R1 therefore followed these corrections:

1. There was no two-live-branch cleanup problem at entry. Branch state was resolved dynamically, not encoded as a supposedly stable committed branch inventory.
2. `CURRENT_STATE.md` remains compact: target <= 3 KiB and <= 80 lines. Historical prose belongs in durable evidence/history, not in current state.
3. Validation ledgers contain validation facts only; they are not a dumping ground for removed current-state prose.
4. E-R1 did not add a fifth .NET test project merely to enforce repository laws. Existing tests/checks were mapped first; small static/source guards were used where they were the simpler canonical enforcement mechanism.
5. Dependency, forbidden-surface, repository-layout, and documentation checks use deterministic source/static checks where unit tests would be brittle or semantically misplaced.
6. The established patch artifact discipline was preserved: compact blueprint/contract + one evidence record + machine-readable oracle when an oracle is warranted. E-R1 did not impose a mandatory PATCH/VALIDATION/CLOSE document trio.
7. Historical documents were not mass-rewritten to add status headers or generic disclaimer sections. Historical artifacts remain historically accurate unless a current ambiguity requires a narrow correction.
8. Generated document navigation is excluded from authority traversal. Archive references do not falsely establish current authority for active evidence.
9. Historical evidence is archived by role and current authority, not to hit an arbitrary document-count target. Git history and archive paths preserve provenance.
10. README remains explanatory only and never carries phase authority.
11. Drive remains design authority, not engineering phase-status authority.
12. The hypothesis ledger cleanly separates unverified assumptions from decisions and validation facts.
13. GitHub Actions dependency/runtime modernization used reviewed official versions/SHAs; no version was adopted by name plausibility.

## Phase 0 — repository inventory and falsification

Goal: prove the restructure target against the actual baseline before structural edits.

Required work:

- resolve `main`, E-R1 branch, merge-base, validation checkpoint, and branch/tag state;
- inventory repository/document/tooling surfaces;
- classify what the pre-restructure closure already solved;
- run the document census and inspect false-positive/false-authority modes;
- map proposed mechanical laws against existing tests/checks before adding enforcement;
- identify current tooling deprecations/drift without changing runtime semantics;
- record corrections in this work package and `CURRENT_STATE.md`.

Exit criterion: one recursive pass finds no material Phase-0 factual error, stale premise, or unaccounted dependency.

## Phase 1 — authority and documentation lifecycle

Goal: make active authority small, explicit, navigable, and mechanically auditable.

Authorized work included:

- keep `CURRENT_STATE.md` inside its compact cap;
- correct active-vs-archive inbound-reference semantics in the document census;
- add a concise document index only if it improves navigation without becoming a second phase authority;
- add validation/hypothesis ledgers only with sharply separated roles;
- archive evidence demonstrably historical under Repository Surface law after checking current authority role;
- keep active handoff surface empty unless `CURRENT_STATE.md` names a real live transition artifact.

## Phase 2 — mechanical repository-law enforcement

Goal: convert high-value objective hygiene laws into the smallest reliable checks.

Process:

1. census existing unit/static/CI enforcement;
2. identify objective gaps;
3. add deterministic source/repository checks rather than a speculative architecture framework;
4. integrate checks into CI;
5. ensure failures are actionable and do not claim runtime authority.

Implemented guards include project dependency direction, ARM64 Harness identity, warnings/determinism, current-state compactness, retired provider source/test surface, dead scaffolding, live handoff authority, exact-SHA action pins, oracle assertion coverage, and active-evidence authority reachability.

## Phase 3 — workflow and artifact normalization

Goal: make future patches easier to author, review, validate, and retire.

E-R1 normalized concise documentation conventions, navigation, evidence/archive lifecycle rules, and explicit current/historical roles without imposing a generic template on JSON, frozen historical artifacts, or every document type.

Future patch documentation remains the smallest set that carries architecture/decision, evidence, and oracle truth. Handoffs remain temporary transition artifacts.

## Phase 4 — CI/toolchain hygiene

Goal: remove avoidable workflow debt without changing Ensemble runtime policy.

E-R1 reviewed GitHub Actions releases/runtime metadata, migrated from mutable Node-20 major tags to exact reviewed Node-24 commit pins, made full-SHA remote action references a repository law, kept required semantic regressions blocking, and preserved the distinction between Linux x64 regression/compiler evidence and native Windows ARM64 runtime evidence.

.NET 10 remained outside E-R1; the SDK remains `9.0.317`.

## Phase 5 — closure

The recursive repository audit covered correctness, consistency, authority, scope, tests/checks, simplicity, hygiene, ARM64 suitability, vision, evidence, documentation lifecycle, and branch/tag hygiene.

Because E-R1 changed only docs/scripts/CI/repository structure and not executable/test/fixture semantics, no fresh native Windows ARM64 rerun was required. Promotion used reviewable PR #46 while preserving the E-R1 work branch for continuity.

## Non-goals / closed gates

E-R1 did not authorize:

- real Gemini credentials, `countTokens`, inference, provider-network execution, or spend;
- provider admission or provider-policy changes;
- reintroduction of OpenAI executable support;
- E0-B through E0-G runtime work;
- application/UI/persistence work;
- Scene endings or Context optimization;
- Windows AI/NPU implementation;
- .NET 10;
- packaging, WACK, or Store work;
- design-lane decisions.

Any newly discovered product/policy ambiguity returns to the Director. Engineering may resolve repository/tooling implementation choices inside this completed contract only as historical interpretation; new work requires current authority.
