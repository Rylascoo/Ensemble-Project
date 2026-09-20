# Ensemble Repository Residency Contract

Status: engineering repository law subordinate to `docs/PROJECT_AUTHORITY.md`.

## Purpose

Ensemble uses separate canonical surfaces so valid work does not become misleading merely by living in the wrong place. Classification is by authority and lifecycle, not by whether a filename or topic merely looks visual or technical.

## Canonical homes

### `Rylascoo/Ensemble-Project`

Canonical for:
- engineering source, tests, fixtures, build/CI/tooling, architecture and validation evidence;
- engineering work-package and implementation records;
- product/policy constitution, Director decisions, provider admissibility, and Open Design Register proposals/resolutions, regardless of which lane raised the question;
- current application UX/UI contract/evidence authority under `docs/design/app/`;
- compact cross-lane locators/contracts needed by engineering.

### `Rylascoo/Ensemble-Website`

Canonical Git repository for:
- website design and implementation;
- website-specific interaction/accessibility/motion evidence and prototypes;
- website expressions of shared identity and approved distributable derivatives.

The exact Website app-design sources sealed by `docs/design/app/TRANSFER_RECEIPT.json` remain historical transfer provenance. Website points live app consumers to Project and does not retain a second evolving app-design authority.

### Google Drive `Ensemble Project`

Canonical visual workspace/master-asset surface for design exploration, renders, imagery, source visual assets, motion/animation, Store/marketing assets, and design research. The active Kymaean workspace is identified by stable folder ID in `docs/DESIGN_REPOSITORY.md`; never classify Drive material by a colliding folder name alone.

### `Rylascoo/Ryladmin`

Canonical only for Administrator runtime implementation and Administrator continuity delegated by `Ensemble-Project` law:
- versioned Administrator launchers, role profiles, process supervision, shared-Git locking, runtime schemas/manifests, deterministic runtime tests, install/recovery/rollback tooling;
- Administrator commissioning/continuity records and exact implementation provenance;
- no product, Engineering, Design, provider, validation, experiment, ODR, or cross-project backlog authority.

Q-ADMIN-05 targets administration, orchestration, cross-repository governance, leases/work packages and capability admission to Ryladmin through coordinated owning amendments. Until those amendments execute, existing Project policy/security/orchestration law and the single queue retain their stated authority. A machine-local Administrator installation is generated executable state, not another canonical repository.

## Residency classes

Every questionable artifact is resolved to one class:

1. **Engineering-native** — remains in `Ensemble-Project`.
2. **Product/policy central** — remains in `Ensemble-Project`, even when drafted by Design Sol or the evidence lane. Originating-lane qualification remains explicit.
3. **Cross-lane pointer/contract** — remains only where the consuming lane needs a compact durable reference; the referenced artifact keeps one canonical home.
4. **Application-design-native** — current Design authority lives under Project `docs/design/app/`, distinct from Engineering authority and write surfaces.
5. **Website-design-native** — belongs in `Ensemble-Website`.
6. **Shared creative master/reference** — belongs in the canonical Drive workspace; repository derivatives/pointers preserve exact provenance.
7. **Administrator-runtime-native** — belongs in `Ryladmin` only when it implements already-approved Administrator law.
8. **Historical/quarantined** — preserved only in the repository/history/archive surface that owns its provenance.
9. **Unclear** — do not move, duplicate, promote, or delete until authority is resolved.

## One-canonical-home rule

Do not solve cross-lane continuity by copying the same authoritative document into both repositories. Keep one canonical artifact and use explicit references elsewhere. A derived engineering summary must identify its design source and cannot silently become design authority; the reciprocal rule applies to engineering facts cited by design.

## Migration protocol

When a file is proven to be in the wrong repository:

1. record its original repository/path and exact commit;
2. identify the destination canonical repository/path and authority class;
3. inspect inbound references and update them to the canonical target or a durable locator;
4. preserve provenance in Git history and, when a branch is retired, an annotated archive tag;
5. verify the destination before removing the misplaced active copy;
6. run both repositories' relevant boundary/continuity checks;
7. record the correction in the current state/decision surface only when it changes active continuity.

Moving a file does not change its substantive authority. A design-native artifact moved to the design repository remains design evidence unless the Director separately promotes a product/policy decision.

## Linked-worktree ownership

A linked worktree is owned by the repository named by its Git common directory, not by the folder in which the worktree path happens to sit. For any nested, suspicious, or cross-lane worktree, establish ownership with both:

- `git rev-parse --absolute-git-dir`
- `git rev-parse --git-common-dir`

Before relocating or removing a linked worktree, record cleanliness including untracked files, branch/HEAD, owning common directory, remote-branch state, and whether unique commits exist. Preserve unique work. Use the owning repository's `git worktree move` / `git worktree remove` machinery rather than raw filesystem deletion. If ownership or unique-work disposition is unclear, classify the artifact `Unclear` and stop.

## Mechanical boundary

`Ensemble-Project` CI rejects design-native top-level workspaces such as `assets/`, `prototypes/`, `site/`, `intelligence/`, and `updates/`. This is deliberately conservative: semantic document residency still requires human/agent classification because ODRs, design-derived engineering contracts, and product decisions legitimately contain visual/design language.

Fresh chats apply the same rule in reverse when reading `Ensemble-Website`: engineering source/test/provider/runtime/validation implementation belongs in `Ensemble-Project`; design may reference it but must not become its canonical source.
