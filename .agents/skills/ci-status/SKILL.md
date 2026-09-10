---
name: ci-status
description: Normalize exact-SHA GitHub Actions workflow status without promoting CI into native validation authority. Use after retrieving workflow-runs JSON with the admitted GitHub read tool.
---

1. Retrieve workflow-runs JSON for the exact authoritative commit using the admitted GitHub read surface; this Skill itself performs no network action.
2. From the exact-SHA GitHub query, copy **every returned workflow run**, not a preselected latest run, as `id|run_number|name|head_sha|status|conclusion`. Pass every record with repeated `--run-record`, pass GitHub's exact `total_count` as `--expected-run-count <count>`, and add repeated `--require-workflow <exact-name>` when specific gates are required. If `total_count` exceeds the runs returned on the current page, retrieve the remaining pages or fail closed.
3. Run `& $env:ENSEMBLE_PYTHON -I -B tools/codex-admin-measure.py ci-status --run-record '<record-1>' --run-record '<record-2>' --expected-run-count <total_count> --expected-sha <exact-40-hex> ...`. The utility, not the agent, selects the highest run-number/ID per workflow. Missing records, duplicate records, nonmatching SHAs, pending/cancelled/skipped/failed runs, or stale-SHA evidence do not pass.
4. Report workflow name, run ID/number, status, conclusion, and exact SHA. Do not reinterpret compiler/CI success as native Windows ARM64, provider, hardware, WACK, or Store validation.
5. Passing CI evidence creates no phase, provider, validation, product, Design, queue, merge, or deployment authority.