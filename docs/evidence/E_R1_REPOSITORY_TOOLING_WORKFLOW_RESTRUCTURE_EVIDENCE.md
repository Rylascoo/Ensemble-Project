# E-R1 Repository / Tooling / Workflow Restructure Evidence

Status: PRE-PROMOTION CLOSURE EVIDENCE
Recorded: 2026-09-07
Work package: `docs/E_R1_REPOSITORY_TOOLING_WORKFLOW_RESTRUCTURE.md`
Branch: `repo-restructure-e-r1`
Baseline: `db4bb1c5d5cb6f6de230da88f1c0352fb75aac23`
Audited implementation/tooling head: `fd655bae81064378a0c58fc627e43dbdd88fef58`

## Scope and validation boundary

E-R1 changed repository documentation, evidence placement, repository/static tooling, and CI workflow configuration. The baseline-to-audited-head compare is 30 commits ahead / 0 behind with merge base exactly the E-R1 baseline and contains no changed file under `src/`, `tests/`, or `fixtures/`. Historical evidence moves are content-preserving renames.

Therefore E-R1 does not supersede the exact native Windows ARM64 machine-tested executable/test checkpoint `cc395a25162a0a682796bffb44060c799df0db32`, preserved by annotated tag `validation/e0a-pre-restructure-closure-native-arm64` and `docs/evidence/E0A_PRE_RESTRUCTURE_CLOSURE_NATIVE_ARM64_VALIDATION.md`.

Cloud results below establish compiler, required x64 regression, and repository-integrity authority only. They do not establish a new native Windows ARM64 runtime rung.

No real Gemini credential, `countTokens`, provider request, inference, or spend was performed by E-R1.

## Phase results

### Phase 0 — inventory and falsification

Repository, branch/tag, validation, project, workflow, document, and tooling assumptions were reconciled against the actual baseline. Stale restructure assumptions were corrected before structural changes.

### Phase 1 — authority and documentation lifecycle

Checkpoint: `0b267f9eadec603e934b55283c6d2801953a87ae`
Validation gate: `34168768724` PASS.

Results:
- `CURRENT_STATE.md` remains the sole phase/checkpoint/validation/next-action authority and is kept under its compact cap.
- navigation, validation-fact, hypothesis, active-evidence, archive, and handoff roles are separated;
- document authority is modeled from durable roots `CURRENT_STATE.md` and `docs/PROJECT_AUTHORITY.md`;
- navigation and archive references cannot manufacture current authority;
- historical E0-A/H1 evidence chains were moved unchanged to `docs/evidence/archive/`;
- the active-evidence census reached zero unreachable evidence;
- before this closure record was added, the active evidence surface contained only the current native validation, its Director-host validation contract, and H1 convergence evidence reachable through the program roadmap.

### Phase 2 — mechanical repository-law enforcement

Checkpoint: `5723d567291860e9c9192976a82a4f53a70ece4e`
Validation gate: `34169337847` PASS.

`tools/repository-law-check.py` and CI now fail closed on objective repository laws including:
- the classified four-project dependency graph;
- Harness `win-arm64` / `ARM64` project identity;
- warnings-as-errors and deterministic build settings;
- `CURRENT_STATE.md` size/line cap;
- reintroduction of retired OpenAI executable/test paths and obsolete transport/provider artifacts while retaining legitimate negative retirement assertions;
- `NotImplementedException` source scaffolding;
- live handoff files without exact `CURRENT_STATE.md` authority.

The document census also gained `--check`, making unreachable active evidence CI-blocking. The checker itself falsified two mistakes during construction: an over-cap `CURRENT_STATE.md` and an over-broad OpenAI token rule. Both were corrected rather than bypassed.

### Phase 3 — workflow and artifact normalization

Checkpoint: `e5df6381513b1730cfc7db895e8cb23462567ba0`
Validation gate: `34169478970` PASS.

Results:
- future patch documentation defaults to the smallest sufficient architecture/decision + evidence + warranted-oracle set;
- handoffs are temporary and live only when current authority names the exact path;
- historical blueprints may remain as provenance without becoming current;
- historical artifacts are not mass-rewritten for cosmetic normalization;
- navigation is explicitly non-authoritative;
- current/support/historical document roles are distinguishable without creating a second phase authority.

### Phase 4 — CI/toolchain hygiene

Audited checkpoint: `fd655bae81064378a0c58fc627e43dbdd88fef58`
Validation gate: `34169804442` PASS, all five jobs.

Official GitHub release/tag metadata and action manifests were reviewed before adoption. The workflow now uses exact immutable Node-24 action revisions:
- `actions/checkout` v7.0.1 -> `3d3c42e5aac5ba805825da76410c181273ba90b1`;
- `actions/setup-dotnet` v6.0.0 -> `a98b56852c35b8e3190ac28c8c2271da59106c68`;
- `actions/setup-python` v7.0.0 -> `5fda3b95a4ea91299a34e894583c3862153e4b97`.

The prior Node-20 deprecation warning is absent on the reviewed revisions. `.NET` remains explicitly `9.0.317`. Repository law now requires every remote GitHub Action reference in workflow YAML to use a full 40-hex commit SHA, preventing silent regression to mutable action tags.

### Oracle guard normalization

The committed path-sensitive `docs/evidence/ORACLE_INDEX.md` was removed. `tools/oracle-index.py` now enforces the actual invariant across revisions: previously documented-and-asserted 64-hex oracle coverage cannot silently disappear. Evidence relocation therefore no longer creates meaningless generated path churn while assertion loss remains fail-closed.

## Phase 5 recursive closure audit

The pre-promotion audit covered correctness, consistency, authority, scope, tests/checks, simplicity, hygiene, ARM64 suitability, vision, evidence, documentation lifecycle, and branch/tag hygiene.

Findings at the audited implementation/tooling head:
- E-R1 baseline remains the merge base; branch is ahead only, not behind;
- no `src/`, `tests/`, or `fixtures/` delta exists;
- no native-validation reset is introduced;
- all five CI jobs pass at the Phase-4 audited head;
- active evidence reachability is fail-closed and clean;
- oracle assertion coverage is fail-closed and clean;
- remote action references are exact-SHA pinned;
- the active handoff surface is empty;
- remote branch surface consists of `main` plus the active E-R1 work branch;
- GitHub plan-dependent `main` protection remains Director-deferred rather than silently treated as completed;
- no product/provider-policy boundary was changed and no provider execution occurred.

One closure-audit wording defect was corrected: repository authority now requires root reachability for documents intended to carry continuing current authority, while clearly historical/support documents may remain outside that graph. Active evidence retains the stricter fail-closed rule.

## Promotion classification

E-R1 is ready for reviewable PR promotion subject to final documentation-inclusive CI. Because the post-audit commits that add this evidence and promotion state are documentation-only descendants of the audited implementation/tooling head, they do not create a new native-runtime validation checkpoint.

After promotion, repository continuity must record the merge checkpoint before the E-R1 branch is archive-tagged and retired under Repository Surface law.
