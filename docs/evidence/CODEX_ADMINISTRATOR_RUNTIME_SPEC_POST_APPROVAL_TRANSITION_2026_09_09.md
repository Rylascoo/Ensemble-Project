# Codex Administrator Runtime Specification - Post-Approval Transition Evidence

Date: 2026-09-09

Status: **ACTIVE ADMINISTRATIVE COMMISSIONING EVIDENCE - NO ENGINEERING PHASE / PROVIDER / VALIDATION AUTHORITY**

## Purpose

This record closes the repository-native transition required by the Director-approved `docs/blueprint/CODEX_ADMINISTRATOR_RUNTIME_SPECIFICATION.md` before C0 machine commissioning begins. It records the approval baseline, continuity retirement, branch/worktree census, and exact administrative boundary. It does not configure Codex, send provider traffic, promote validation, enable Hooks/Automations/browser/Claude, or disposition unique Git history.

## Director approval and baseline

- Director approved the complete Runtime Specification on 2026-09-09.
- Approval/source baseline: `main@46eb164323542cdf2c405964eb914a3ad02d945f`.
- Transition branch: `docs/codex-administrator-runtime-spec-approval-2026-09-09`.
- C0 commissioning baseline will be the exact accepted transition checkpoint preserved by annotated tag `commissioning/codex-administrator-c0-baseline`; commissioning must resolve that tag before C0 action.

## Repository transition

- Canonical Runtime Specification materialized at `docs/blueprint/CODEX_ADMINISTRATOR_RUNTIME_SPECIFICATION.md`.
- Root `AGENTS.md` now directs Administrator work to the approved specification only after normal authority recovery.
- The temporary working-continuity packet was moved to `docs/evidence/archive/CODEX_ADMINISTRATOR_INTEGRATION_WORKING_CONTINUITY_2026_09_09.md`; archive placement preserves provenance and removes it from active bootstrap use.
- `docs/PROJECT_EXECUTION_QUEUE.md` contains Q-ADMIN-02 with C0 as the only currently earned commissioning gate.
- `CURRENT_STATE.md` records the parallel Administrator commissioning boundary without changing the active Engineering Q-E0A-03 checkpoint/provider/validation authority.

## Approval-baseline remote branch census

The census excludes the symbolic `origin/HEAD`, `main`, and the transition branch created after Director approval. Every approval-baseline non-main ref was inspected against exact `main@46eb164323542cdf2c405964eb914a3ad02d945f`.

- Total approval-baseline non-main refs: **58**.
- Ancestral to approval `main`: **36**.
- Ahead of approval `main`: **0**.
- Divergent/side-history refs: **22**.

No branch is deleted, force-moved, merged, or retagged by this transition. Ancestry is not semantic disposition; divergent refs remain preserved until unique content is reconciled.

| Remote ref | Exact HEAD | Relation to approval main | Transition disposition |
|---|---|---|---|
| `origin/design-queue-clr01-method-freeze-2026-09-09` | `ee4478d2a0c3fe566673d4ccb23df63529f2b08e` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/design-queue-clr01-q19-reconcile-2026-09-09` | `05a50652ebe7ba7eed9fe96e20c035cd553b189e` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/design-queue-clr01-q19-reconcile-2026-09-09-v2` | `55541f5ad5e3c47e1a85cc35a3a6e169aa5fda70` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/design-queue-clr01-q19-reconcile-2026-09-09-v3` | `1be8fde5ae5e4f2632918ef9dbfafad657308d40` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/docs/codex-administrator-working-continuity` | `c9df083b0d295bebde854ed66b7ff46bb59044a2` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/docs/codex-fresh-chat-hardening-2026-09-09` | `81e920041f7170b8f44100c5907fc935e51e1a72` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/docs/e0a-attempt04-preflight01-correction-2026-09-09` | `52b8f9006bd196a7e21e7a4551b58ececa33e7a1` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/docs/e0a-continuity-evidence-closeout-2026-09-09` | `f3a7e87207a7822eae9f174ecb66ddd69f11ac92` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/docs/e0a-fresh-chat-continuity-reconciliation-2026-09-09` | `e297a0ca1e00d420df442bf38d41e59dc3e20504` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/docs/engineering-sol-agent-protocol-migration-2026-09-09` | `c52fc5c667ba2da79c20f38444b49a625d646c71` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/docs/engineering-sol-agent-protocol-migration-audit-anchor-2026-09-09` | `bd892837559b4e1a259cb49ca789c9357bf14ffa` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/docs/q-e0a-03-g35l-run01-preflight01-wrapper-fix-2026-09-09` | `4a63d9a9deaec656a657324350570b3a35b5b9b3` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/docs/q-e0a-03-g35l-run01-preflight02-root-drift-2026-09-09` | `40d2e4897ce2fe991683fdda634cc13a517692e7` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/docs/q-e0a-03-g35l-run01-preflight04-rid-parser-2026-09-09` | `69476005558a466c8d2b6c78bef3c2988e9eed77` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/docs/q-e0a-03-g35l-run01-root-forensic-resolution-2026-09-09` | `f4a9ad6346c0284d1c71dcdc3c89f346dd92a0f0` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/e0a-gemini-bounded-provider-error-diagnostic` | `bc97790c3e4415f2253d07e022ed3d6fe317be6a` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/e0a-gemini-counttokens-input-projection-correction` | `8842dac123d31851dee78dd91b760db61045655d` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/e0a-gemini35-attempt04-director-authorization-2026-09-09` | `1724fe7b7d0380a0f1cf1c38ee0bdcff465af962` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/e0e-single-model-playwright-control-preparation` | `fdc0cc736fd55eee776a7c87229c20a496a12688` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/q-e0a-03-g35l-preauthorization-public-facts` | `70aca80676948e03f1487c0d30e64bd4baec16f7` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/q-e0a-03-g35l-run01-director-authorization` | `05a50652ebe7ba7eed9fe96e20c035cd553b189e` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/q-e0a-03-reference-evidence-plan` | `af10e05bf5c327f42e9158ba2b42f42cf5b187bb` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/context-shift-design-differentiation-clean` | `18f50c1a31e52634ff604f2e6a764cb4710f81cd` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/cs2-deployment-envelope-activation` | `fefec8d310cb4af57cc8214144ff470c7d74707f` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/queue/cs2-deployment-envelope-activation-clean` | `6c4a38e3dbd26d8855b5d4e9f5e11aa4d75de7d7` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/cs2-envelope-director-review-clean` | `eb9b8aae805c2125945fe7a7bcea6af0fbba31ac` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/cs2-envelope-director-review-reconcile` | `073dd9515b115a37ffbab60b63f4331694a44800` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/queue/design-post-program-reconciliation-closeout` | `548f1a5813ab7f3051c35c0de4b491cf6f3751b4` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/design-q12-f1-comparison-clean` | `86dfc758d6ca34e60594781a97706c2e46dba57e` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/queue/design-q12-f1-comparison-rebased` | `0ef4216a8351ce634f69473ffb01c719a2c70687` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/design-q13-f1a-deployment-envelope` | `41eafa32823a93c1bad96df9c27e36483188d342` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/queue/design-q13-f1a-deployment-envelope-clean` | `d2b860719b67c8989e7e101b44d2cebef974b413` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/design-q14-f1a-envelope-exemplars` | `ab6e7e24f8068204c139bffa01977e059be57168` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/queue/design-q14-f1a-envelope-exemplars-clean` | `0ec95310182cac9ebbf1cc449a0dfb5873af16d9` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/mot01-blind-review-activation` | `392359d27e7156c3cfd45068d6696d7cbe5b6c7f` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/queue/mot01-blind-review-activation-reconciled` | `b84ccf6500c100620aa939ecfcc679b3ccb4d6be` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/queue/mot01-blind-review-activation-reconciled-v2` | `88e3f7981944796405e52e72e48f5d3dbb3972c7` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/mot01-director-adjudication-f1-refinement` | `769b5893456398cf66796cadbcdc1aeebf640f7f` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/mot01-method-contract-activation` | `f85bcb634774228bb6a8eef02cacc78d1c476712` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/queue/mot01-method-contract-activation-rebased` | `05e394eb1c5d11430557deb10928b740c1666432` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/mot01-preflight-activation` | `60f1fc74151898920fc65dfe510802b39a4a0d76` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/q-design-16-closeout-color-method` | `098824d3b4ac18d843126b781cf936c193dad5bb` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/queue/q-design-18-color-method-clean` | `a35d3954e56658bd820d13515d65ac685a2ac70a` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/q-design-19-clr01-materialization` | `d8626a8e75ea6c45e19a4cd4b758165e9286e717` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/queue/typ-01-method-contract-activation` | `d96a0ecf4d0acabfbec87f3384d781a846c984ef` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/typ01-preflight-termination-closeout` | `4bba35a95871f448a16c158325fcf0ba72c0fe45` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/queue/typ01-preflight-termination-closeout-clean` | `e5ce1e51e06ca8bf572dbf3e40eae3cb206b8cd5` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/queue/typ01-specimen-preflight-activation` | `b6ac10f60a04f087a8b95ca3e0287d258d0da274` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/queue/typ01-specimen-preflight-activation-v2` | `a6c3f627b42ab688c363e125cfe6fe784691c35c` | ANCESTRAL_TO_APPROVAL_MAIN | No active moving-work role established at approval baseline; historical accepted/merged lineage. Preserve until archive-tag/ref-retirement requirements are satisfied. |
| `origin/tmp/context-shift-design-queue-reconcile` | `a0f6ef36f8109a681e12b09aaa64507605e13932` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/tmp/design-q12-queue-materialize` | `0031b04ec9f8f738fca32d863e035262de21a799` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/tmp/design-q12-rebase-176f` | `3e45076dddd53dbf4af9bcd1ec7d860d1dc3bed2` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/tmp/mot01-blind-review-queue` | `ce4b1c3262358bec332d9131fdb47d2a09e643ed` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/tmp/mot01-blind-review-queue-reconcile` | `d2a39eba96ab52a1b9503ab8739a367cf4e64e8c` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/tmp/mot01-preflight-queue-materialize` | `83ccdf3336541a68437fba3581d6fbcba4ba17f7` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/tmp/mot01-queue-activation-materialize` | `fc5ba7969d1553af3453407f85ba16ae3eb81537` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/tmp/mot01-queue-rebase-materialize` | `3bbd020e3cb2413de6bc07b5629ff43c04b9c738` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |
| `origin/tmp/typ01-queue-method-closeout` | `6c2d8ed78603923e764e51f18918e1b48765db6c` | DIVERGENT_SIDE_HISTORY | Unique side history exists. Preserve; semantic disposition against current accepted content is required before archive/delete. |

## Local worktree census at transition

Current Git-common-directory worktrees were re-read after the transition worktree was created. Existing detached validation/evidence worktrees remain untouched.

```text
worktree C:/Users/Wiryl/Sol Dev/Ensemble-Project
HEAD 689655eed677b789ab3ee395f1c65b4f2cb72cc8
detached

worktree C:/Users/Wiryl/Sol Dev/Ensemble-Project-Worktrees/codex-admin-runtime-spec-approval
HEAD 87955c86360d362f35cabd4ec72b544b231d77a4
branch refs/heads/docs/codex-administrator-runtime-spec-approval-2026-09-09

worktree C:/Users/Wiryl/Sol Dev/Ensemble-Project-Worktrees/e0a-bounded-provider-error-diagnostic-native-arm64
HEAD e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a
detached

worktree C:/Users/Wiryl/Sol Dev/Ensemble-Project-Worktrees/e0a-counttokens-input-projection-native-arm64
HEAD de38d5d52279c22a1786e11200239c445e04377b
detached

worktree C:/Users/Wiryl/Sol Dev/Ensemble-Project-Worktrees/e0a-counttokens-input-projection-native-arm64-attempt02
HEAD 3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2
detached

worktree C:/Users/Wiryl/Sol Dev/Ensemble-Project-Worktrees/e0a-gemini35-attempt04
HEAD e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a
detached

worktree C:/Users/Wiryl/Sol Dev/Ensemble-Project-Worktrees/e0a-q03-g35l-20260909-01
HEAD 3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2
detached
```

Classification: the root checkout remains a clean detached historical validated checkout rather than current authority; existing detached E0-A validation/evidence worktrees remain preservation surfaces; the `codex-admin-runtime-spec-approval` worktree is the only new active transition worktree. No existing worktree is repurposed or removed.

## Commissioning boundary

After this transition is accepted and tagged, **C0 is the only next Administrator commissioning gate**. C1-C13 remain unavailable until their immediate predecessor closes under the approved Runtime Specification.

C0 must reverify volatile Codex/tool/authentication facts before materializing runtime configuration. No Hooks, Automations, Claude, browser/CDP, provider traffic, danger-full-access, automatic merge, or automatic unique-branch deletion is authorized by this transition.

## Recursive audit

This transition was audited for authority leakage, duplicate backlog/state, archive reachability, branch/worktree loss, validation/provider inflation, cross-lane mutation, and premature commissioning. The generated census initially included the symbolic `origin/HEAD` short ref as `origin`; that mechanical error was corrected before promotion and the audit restarted. Repository-law/CI checks and exact-ref readback remain required before promotion.
