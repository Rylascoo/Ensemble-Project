---
name: branch-worktree-census
description: Measure Ensemble remote-ref topology and linked-worktree ownership without mutation. Use before branch/worktree lifecycle decisions or when repository residue must be explained.
---

1. Require already-fetched tracking refs when current remote topology is load-bearing; the Skill does not fetch or mutate Git.
2. Run `& $env:ENSEMBLE_PYTHON -I -B tools/codex-admin-reconcile.py --repo . --expected-origin <expected-origin> --pretty`, supplying all independently known worktrees with repeated `--known-worktree` arguments.
3. Report `git_common_dir`, `worktrees`, `unexpected_worktrees`, `missing_known_worktrees`, and `remote_ref_census` including `MAIN`, `ANCESTRAL_TO_MAIN`, `AHEAD_OF_MAIN`, and `DIVERGED_SIDE_HISTORY` counts.
4. Treat topology as measurement only. Ancestry, age, or naming never authorizes branch deletion, archive disposition, checkout replacement, or authority promotion.
5. Return anomalies for owning-manager interpretation. A clean census does not create authority.