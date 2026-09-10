---
name: state-distance
description: Measure CURRENT_STATE.md currency at an exact Git ref. Use before relying on volatile state or before closeout when the three-commit freshness limit matters.
---

1. Run from repository root: `& $env:ENSEMBLE_PYTHON -I -B tools/codex-admin-measure.py state-distance --repo . --ref <exact-ref>`.
2. Require the returned exact commit, last `CURRENT_STATE.md` commit, distance, byte count, and maximum distance.
3. Treat `pass=true` only as currency-distance evidence. It does not prove the prose is correct or promote that ref into authority.
4. If the distance exceeds the limit or cannot be measured, fail closed and return the discrepancy to the owning manager.
5. This Skill never edits `CURRENT_STATE.md`; a passing result creates no phase, validation, provider, product, Design, or queue authority.