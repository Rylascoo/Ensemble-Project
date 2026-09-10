---
name: commissioning-closeout
description: Run deterministic repository closeout checks for an Administrator commissioning candidate before owning-manager promotion. Use after implementation evidence exists and before push/merge/archive mechanics.
---

1. Run `& $env:ENSEMBLE_PYTHON -I -B tools/codex-admin-measure.py commissioning-closeout --repo . --baseline <exact-base> --head <exact-head>`.
2. By default the closeout rejects every changed protected authority surface. The owning manager may add repeated `--allow-protected <exact-path>` only after explicitly reviewing/adopting those exact changes.
3. The composite check runs repository law, document authority census, oracle-coverage guard, `git diff --check`, and protected-diff measurement. Preserve each component result; do not collapse failures into a generic pass/fail explanation.
4. This Skill performs no fetch, push, merge, tag, checkout, reset, clean, branch deletion, provider call, or authority-file write.
5. A passing closeout means only that these deterministic checks found no blocking condition. Promotion, validation interpretation, queue/state adoption, and external actions remain with their owning authority.