---
name: authority-recovery
description: Recover exact Ensemble repository authority before Administrator status, dispatch, or closeout work. Use when a fresh task begins or live refs may have moved; never use it to create or modify authority.
---

1. Resolve the live authoritative `main` SHA with the admitted GitHub read tool before trusting local tracking refs.
2. Run from the repository root: `& $env:ENSEMBLE_PYTHON -I -B tools/codex-admin-measure.py authority-recovery --repo . --expected-origin https://github.com/Rylascoo/Ensemble-Project --remote-main <exact-40-hex>`, adding each already-known linked worktree with `--known-worktree <path>`.
3. Treat a remote/local mismatch as stale-local evidence. Do not fetch, checkout, reset, clean, switch, merge, push, or delete from this Skill.
4. Read root `AGENTS.md`, `CURRENT_STATE.md`, and every authority path required by root bootstrap through EOF at the exact remote ref. The measurement's blob SHAs are a cross-check, not authority by themselves.
5. Recover exact-SHA CI with `$ci-status`. Preserve validation/provider/Director boundaries from their owning records.
6. Report facts and unresolved anomalies. A passing measurement does not create phase, provider, validation, product, Design, or queue authority.