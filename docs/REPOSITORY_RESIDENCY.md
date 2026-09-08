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
- compact cross-lane locators/contracts needed by engineering.

### `Rylascoo/Ensemble-Website`

Canonical Git repository for:
- app UI/UX design and interactive design prototypes;
- website design and implementation;
- visual identity, brand-system material, renderer/design methods, and design-specific evidence;
- exported design assets that belong under version control.

### Google Drive `Ensemble Project`

Canonical visual workspace/master-asset surface for design exploration, renders, imagery, source visual assets, motion/animation, Store/marketing assets, and design research. The active Kymaean workspace is identified by stable folder ID in `docs/DESIGN_REPOSITORY.md`; never classify Drive material by a colliding folder name alone.

## Residency classes

Every questionable artifact is resolved to one class:

1. **Engineering-native** — remains in `Ensemble-Project`.
2. **Product/policy central** — remains in `Ensemble-Project`, even when drafted by Design Sol or the evidence lane. Originating-lane qualification remains explicit.
3. **Cross-lane pointer/contract** — remains only where the consuming lane needs a compact durable reference; the referenced artifact keeps one canonical home.
4. **Design-native** — belongs in `Ensemble-Website` and/or the canonical Drive workspace, not `Ensemble-Project`.
5. **Historical/quarantined** — preserved only in the repository/history/archive surface that owns its provenance.
6. **Unclear** — do not move, duplicate, promote, or delete until authority is resolved.

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

## Mechanical boundary

`Ensemble-Project` CI rejects design-native top-level workspaces such as `assets/`, `prototypes/`, `site/`, `intelligence/`, and `updates/`. This is deliberately conservative: semantic document residency still requires human/agent classification because ODRs, design-derived engineering contracts, and product decisions legitimately contain visual/design language.

Fresh chats apply the same rule in reverse when reading `Ensemble-Website`: engineering source/test/provider/runtime/validation implementation belongs in `Ensemble-Project`; design may reference it but must not become its canonical source.
