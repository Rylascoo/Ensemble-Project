# Ensemble

**Create the cast. Set the scene. See what happens.**

Ensemble is a local-first generative theater and creative simulation for persistent Characters. The creator establishes people, circumstances, knowledge, relationships, possibilities, and pressures; Performers portray Characters; accepted Performances become causal history whose authoritative consequences alter what future Scenes can mean.

## Current development state

Blueprint 0.1 is frozen for E0. The active engineering phase is **E0-A Harness Implementation — H1 Deterministic Spine**.

Read [`CURRENT_STATE.md`](CURRENT_STATE.md) before modifying source.

## Repository roles

- **GitHub:** authoritative engineering source, tests, fixtures, validation evidence, implementation checkpoints.
- **Google Drive — Ensemble Project:** UI/UX architecture, mockups/prototypes, visual identity/artwork, motion/animation, Store/marketing assets, and design research.

See [`docs/PROJECT_AUTHORITY.md`](docs/PROJECT_AUTHORITY.md), [`docs/ENGINEERING_HYGIENE_CONSTITUTION.md`](docs/ENGINEERING_HYGIENE_CONSTITUTION.md), and [`docs/DESIGN_REPOSITORY.md`](docs/DESIGN_REPOSITORY.md).

## Engineering discipline

Every accepted implementation must leave the active codebase at least as coherent as the validated baseline it replaces. Git preserves superseded work; the active tree preserves the best current architecture. New patches are reviewed for correctness, consistency, authority, scope, tests, simplicity, hygiene, ARM64 suitability, vision, and evidence.

## Validation discipline

Static review is advisory. Native ARM64 compiler output is compiler authority; target-device execution is runtime authority; NPU execution, WACK, and Store certification require their own later evidence.

No current repository state should be interpreted as proof of Windows runtime, NPU execution, packaging, or Store certification unless `CURRENT_STATE.md` explicitly records that evidence.
