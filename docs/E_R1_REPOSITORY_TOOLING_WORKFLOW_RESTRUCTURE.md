# E-R1 Repository / Tooling / Workflow Restructure

Status: ACTIVE ENGINEERING WORK PACKAGE
Entered: 2026-09-07
Baseline: `db4bb1c5d5cb6f6de230da88f1c0352fb75aac23`
Branch: `repo-restructure-e-r1`

## Authority and purpose

`CURRENT_STATE.md` identifies E-R1 as the approved next engineering action. A repository search at E-R1 entry found no earlier E-R1 work-package artifact on `main`; this document makes the corrected work-package boundary durable so future engineering chats do not depend on chat history.

E-R1 improves repository structure, authority clarity, documentation lifecycle, mechanical hygiene enforcement, and CI/tooling coherence. It does **not** redesign Ensemble runtime architecture or provider policy.

Higher authority remains unchanged: frozen Blueprint/approved phase specifications -> `CURRENT_STATE.md` -> source/tests/fixtures/commits/PR evidence -> decision/evidence records -> design evidence -> historical material.

## Entry facts

At the exact E-R1 baseline:

- pre-restructure closure and continuity are promoted;
- the exact native Windows ARM64 machine-tested executable/test checkpoint is `cc395a25162a0a682796bffb44060c799df0db32` under `validation/e0a-pre-restructure-closure-native-arm64`;
- completed branch `repo-pre-restructure-closure` is archived and retired;
- prior to creating this work branch, the remote branch surface contained only `main`;
- `CURRENT_STATE.md` is compact and remains the sole phase/checkpoint/validation/next-action authority;
- the solution has four projects: Core, Harness, Core.Tests, Harness.Tests;
- CI already contains ARM64-target compiler checks, required x64 Core regression tests, oracle-documentation drift protection, and a document-reference census;
- `docs/evidence/archive/` exists and contains explicitly historical E0-A evidence;
- historical OpenAI executable/test support is retired; Gemini is the sole current E0-A provider method;
- real Gemini credentials/network execution/inference/spend remain unauthorized;
- GitHub plan-dependent `main` protection is deferred and does not block E-R1.

## Corrections to the originally discussed restructure shape

Current repository facts supersede stale assumptions. E-R1 therefore follows these corrections:

1. There is no two-live-branch cleanup problem at entry. Branch state is resolved dynamically, not encoded as a supposedly stable committed branch inventory.
2. `CURRENT_STATE.md` remains compact: target <= 3 KiB and <= 80 lines. Historical prose belongs in durable evidence/history, not in current state.
3. Validation ledgers, if introduced, contain validation facts only; they are not a dumping ground for removed current-state prose.
4. Do not add a fifth .NET test project merely to enforce repository laws. Map existing tests/checks first; use small static/source guards where they are the simpler canonical enforcement mechanism.
5. Dependency, forbidden-surface, repository-layout, and documentation checks should be deterministic source/static checks when unit tests would be brittle or semantically misplaced.
6. Preserve the established patch artifact discipline: compact blueprint/contract + one evidence record + machine-readable oracle when an oracle is warranted. Do not impose a new mandatory PATCH/VALIDATION/CLOSE document trio.
7. Do not mass-rewrite historical documents to add status headers or generic disclaimer sections. Historical artifacts remain historically accurate unless a current ambiguity requires a narrow correction.
8. A generated document index/census must exclude its own generated references from inbound-reference calculations. Archive references must not falsely establish current authority for active evidence.
9. Historical evidence is archived by role and current inbound authority, not to hit an arbitrary document-count target. Git history and archive paths preserve provenance.
10. README is explanatory only and should change only when it materially improves navigation or accurately reflects current work; it never carries phase authority.
11. Drive remains design authority, not engineering phase-status authority. Phase truth is not expanded into an artificial fixed count of “active sources.”
12. A hypothesis/assumption ledger is useful if it cleanly separates unverified assumptions from decisions and validation facts.
13. GitHub Actions dependency/runtime modernization is in E-R1 scope, but reviewed versions/SHAs must be verified before adoption; no version is adopted by name plausibility.

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

Authorized work includes:

- keep `CURRENT_STATE.md` inside its compact cap;
- correct active-vs-archive inbound-reference semantics in the document census;
- add a concise document index only if it improves navigation without becoming a second phase authority;
- add a validation ledger and/or hypothesis ledger only with sharply separated roles;
- archive evidence that is demonstrably historical under Repository Surface law, after checking current inbound role;
- keep active handoff surface empty unless `CURRENT_STATE.md` names a real live transition artifact.

## Phase 2 — mechanical repository-law enforcement

Goal: convert high-value objective hygiene laws into the smallest reliable checks.

Process:

1. census existing unit/static/CI enforcement;
2. identify objective gaps;
3. add deterministic source/repository checks rather than a speculative architecture framework;
4. integrate checks into CI;
5. ensure failures are actionable and do not claim runtime authority.

Candidate objective guards include repository/project dependency direction, forbidden obsolete provider source, source TODO/dead-scaffolding markers, and other laws that can be checked without encoding domain judgment as brittle string dictionaries.

## Phase 3 — workflow and artifact normalization

Goal: make future patches easier to author, review, validate, and retire.

Authorized work includes concise documentation conventions, generated navigation where useful, evidence/archive lifecycle rules, and explicit active/historical roles. Do not impose one generic template on JSON, frozen historical artifacts, or every document type.

Future patch documentation should normally remain the smallest set that carries architecture/decision, evidence, and oracle truth. Handoffs remain temporary transition artifacts.

## Phase 4 — CI/toolchain hygiene

Goal: remove avoidable workflow debt without changing Ensemble runtime policy.

Authorized work includes reviewing GitHub Actions versions/runtime deprecations, pinning reviewed action revisions where appropriate, keeping required semantic regressions blocking, and preserving the distinction between Linux x64 regression/compiler evidence and native Windows ARM64 runtime evidence.

.NET 10 remains outside E-R1 without Director approval.

## Phase 5 — closure

Perform a recursive repository audit across correctness, consistency, authority, scope, tests/checks, simplicity, hygiene, ARM64 suitability, vision, evidence, documentation lifecycle, and branch/tag hygiene.

If E-R1 changes only docs/scripts/CI/repository structure and not executable/test/fixture semantics, native Windows ARM64 rerun is not automatically required. Any executable/test/fixture change must be classified against validation law and may require a fresh native gate before promotion.

Promote through reviewable PR(s) while preserving one E-R1 work branch. Before deleting the completed branch, create an annotated `archive/...` tag at its final head per Repository Surface law.

## Non-goals / closed gates

E-R1 does not authorize:

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

Any newly discovered product/policy ambiguity returns to the Director. Engineering may resolve repository/tooling implementation choices inside this contract after recursive audit.
