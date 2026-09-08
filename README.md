# Ensemble

**Create the cast. Set the scene. See what happens.**

Ensemble is a local-first generative theater and creative simulation for persistent Characters. The creator establishes people, circumstances, knowledge, relationships, possibilities, and pressures; Performers portray Characters; accepted Performances become causal history whose authoritative consequences alter what future Scenes can mean.

## Current development state

Blueprint 0.1 is frozen for E0. The runtime phase is **E0-A Phase B**. E-R1 repository/tooling/workflow restructuring is **closed and archived**. The Gemini Free-tier RPD/model-selection work and first-real-attempt `countTokens` correction are native-validated and promoted at tagged Windows ARM64 checkout `689655eed677b789ab3ee395f1c65b4f2cb72cc8`. Real Gemini 3.5 Flash-Lite attempts 01 and 02 are consumed technical failures at zero accepted turns; attempt 02 reached first Performer `countTokens` and received HTTP 401 before generation. A later Director-only diagnostic proved a new `AQ.` auth key can reach current native Gemini 3.6 Flash generation through `?key=`, leaving auth transport/method/model/key as unresolved axes. No source patch is authorized. Exactly two `gemini-3.5-flash-lite:countTokens` authentication-differential requests are now authorized with one fresh unshared key—one via `?key=` and one via `x-goog-api-key`; no generation or retries. H1 Phase A is **CLOSED**.

`README.md` never carries phase authority; `CURRENT_STATE.md` is the only phase source.

Read [`CURRENT_STATE.md`](CURRENT_STATE.md) before modifying source, then use [`docs/DOCUMENT_INDEX.md`](docs/DOCUMENT_INDEX.md) for navigation. Current provider-comparison architecture is recorded in [`docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md`](docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md) and its approved RPD/model-selection successor [`docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md`](docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md). E-R1 remains historical authority only at [`docs/E_R1_REPOSITORY_TOOLING_WORKFLOW_RESTRUCTURE.md`](docs/E_R1_REPOSITORY_TOOLING_WORKFLOW_RESTRUCTURE.md).

Then read [`docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md`](docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md). Whenever applicable, every fresh or continuing project chat must maximize useful value per reply by deliberately optimizing substantive task scope for the capabilities of GPT-5.6 Sol High reasoning; this means more useful reasoning, synthesis, verification, and dependency closure per reply, not artificially longer replies or greater token consumption.

## Repository roles

- **GitHub:** authoritative engineering source, tests, fixtures, validation evidence, implementation checkpoints.
- **Google Drive — Ensemble Project:** UI/UX architecture, mockups/prototypes, visual identity/artwork, motion/animation, Store/marketing assets, and design research.

See [`docs/PROJECT_AUTHORITY.md`](docs/PROJECT_AUTHORITY.md), [`docs/ENGINEERING_HYGIENE_CONSTITUTION.md`](docs/ENGINEERING_HYGIENE_CONSTITUTION.md), and [`docs/DESIGN_REPOSITORY.md`](docs/DESIGN_REPOSITORY.md).

## Engineering discipline

Every accepted implementation must leave the active codebase at least as coherent as the validated baseline it replaces. New patches are reviewed for correctness, consistency, authority, scope, tests, simplicity, hygiene, ARM64 suitability, vision, and evidence.

For each substantive Director turn, choose the largest logically coupled, falsifiable work package that belongs to one objective and can be completed with current evidence/tools. Spend Sol High reasoning on source reconciliation, architecture/dependency analysis, root-cause diagnosis, smallest-surface patch planning, edge cases, regression strategy, privacy/accessibility/ARM64 implications, contradiction detection, and recursive audit—not on artificially larger replies, code blocks, file counts, or unrelated scope. Simple tasks remain simple. Consequential architecture, implementation, native runtime, WACK, Store, security, and other external-validation gates remain explicit.

Use the patch-first flow for compiler/runtime feedback: