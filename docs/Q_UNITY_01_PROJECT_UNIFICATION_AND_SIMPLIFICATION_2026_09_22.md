# Q-UNITY-01 Project unification and simplification

Date: 2026-09-22  
Status: **DIRECTOR APPROVED / ACTIVE-STRUCTURE SIMPLIFIED / CLEAN RECURSIVE AUDIT**

Exact starting main: `07fe8c3728d3c4c0a326e39a3089282e8cad246d`; Validation #1166 PASS.

## Purpose

Reduce organizational/authority debt before additional feature work while preserving load-bearing Product architecture, exact evidence lineage and independent acceptance boundaries.

## Unified operating model

The application has one current authority root: **`Rylascoo/Ensemble-Project`**.

Current work uses roles, not persistent assistant identities: Implementation; Design; Product / Architecture; Independent Review; Governance / Recovery.

Historical Engineering Sol / Design Sol / Engineer #1/#2/#3 / Relay / Administrator labels remain provenance only.

Website is website-only plus historical app-Design provenance. Drive owns shared masters. Ryladmin is exceptional governance/recovery/evidence infrastructure.

Codex is reserved for substantial implementation/debugging. GitHub/files handle authority recovery, source review, PR review, continuity and ordinary repository management. Remote Desktop Commander is last-resort target-machine/local-Git evidence.

## Active-document simplification

- `CURRENT_STATE.md` remains the sole volatile project checkpoint.
- `docs/PROJECT_EXECUTION_QUEUE.md` is reduced to current/deferred/blocking work only.
- The complete pre-unification queue is preserved at `docs/history/PROJECT_EXECUTION_QUEUE_PRE_UNIFICATION_2026_09_22.md`.
- App Design gains `docs/design/app/CURRENT_CONTRACTS.md` as the short active bootstrap.
- `AGENTS.md`, `PROJECT_AUTHORITY.md` and App Design authority now describe roles instead of persona identities.

## Branch simplification

Pre-cleanup remote census: 18 named branches including `main`.

Read-only Git ancestry audit found 16 non-main remote branches were exact ancestors of current `main` with zero unique commits. `engineer-02/application/qprod01-designarch-02@dc1780e46ef999eb01e214671486b83186ca04c4` has one unique historical commit and is the preserved PR #226 producer.

The 16 fully ancestral remote branches were deleted after the audit. Current remote topology is `main` plus the preserved historical PR #226 branch. No unique commit was discarded.

Local historical/deferred-E0 worktree residue remains a separate Q-ADMIN-07 preservation question; ambiguous local evidence was not deleted.

## Create Production simplification audit

A tentative idea to revise duplicate-name Product behavior to exact-name uniqueness was recursively audited and **rejected**.

Correct uniqueness enforcement would introduce a new catalog-wide historical invariant, cross-instance creation-race/locking semantics, migration/compatibility implications, and a truthful duplicate-name failure taxonomy.

The already-integrated A/A/A Product contract is simpler when left intact.

Q-DESIGN-22 therefore adopts conditional presentation-only disambiguation: exact duplicate names remain legal; only duplicate groups show `Production code <fingerprint>`; the fingerprint is SHA-256 over exact UTF-16BE `ProductionId.Value`, shortest unique prefix starting at 8 hex characters; it is not Product state, editable identity, path/locator, or sort authority; created-row selection/focus is by exact returned `ProductionId`.

Contract: `docs/design/app/contracts/CREATE_PRODUCTION_PRESENTATION.md`.

## Preserved boundaries

No change to Q-PROD-04 Product semantics, persistence formats, World-current semantics, Home A/B, FIRSTUSE, rename/delete/import/restore, Character/Scene ontology, provider/Performer, deferred E0, or final architecture/release authority.

## Result

The project resumes from one authority root, explicit task roles, a short current queue, a short current Design index, two live remote branches, and one bounded next implementation package: **Q-PROD-05 Create Production Windows presentation**.

**APPROVED BY DIRECTOR — CLEAN RECURSIVE AUDIT.**
