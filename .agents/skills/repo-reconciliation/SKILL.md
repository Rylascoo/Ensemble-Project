---
name: repo-reconciliation
description: Classify an Ensemble repository checkout against expected origin and fetched tracking truth without mutation. Use for reconciliation, stale-checkout diagnosis, or pre-dispatch repository state checks.
---

1. Require an already-authorized/fresh local fetch when tracking truth is load-bearing; this Skill never performs network Git operations itself.
2. Run from the repository root: `& $env:ENSEMBLE_PYTHON -I -B tools/codex-admin-reconcile.py --repo . --expected-origin <expected-origin> --pretty`, adding each explained linked worktree with `--known-worktree <path>`.
3. Accept only the frozen classifications emitted by the tool. Preserve `would_modify_repository=false` and inspect `git_commands` if behavior is disputed.
4. Never convert `BEHIND_FAST_FORWARD_CANDIDATE` into permission to update, or another classification into permission to reset, clean, delete, merge, or repurpose a worktree.
5. Return the measurement to the owning Sol/Administrator for interpretation. A passing classification does not create authority.