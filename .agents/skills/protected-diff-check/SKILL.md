---
name: protected-diff-check
description: Detect manager-owned authority surfaces changed between exact Git refs. Use before adopting Worker/executor output or promoting a commissioning candidate.
---

1. Run `& $env:ENSEMBLE_PYTHON -I -B tools/codex-admin-measure.py protected-diff-check --repo . --base <base-sha> --head <head-sha>`.
2. By default, any protected-surface change fails the check. Only the owning manager may rerun with repeated `--allow-protected <exact-path>` for changes it has explicitly reviewed and adopted.
3. Never infer an allowlist from branch name, passing tests, executor intent, or an existing diff.
4. Report all changed paths, protected paths, explicit allowances, and unexplained protected paths.
5. Passing means only that no unexplained protected change was detected. It does not authorize merge, provider traffic, validation promotion, queue closure, or another authority change.