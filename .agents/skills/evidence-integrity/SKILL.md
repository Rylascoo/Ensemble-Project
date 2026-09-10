---
name: evidence-integrity
description: Verify exact evidence-file existence, size, and SHA-256 without interpreting what the evidence proves. Use when preserved local or repository evidence must be integrity-checked before analysis.
---

1. Obtain expected SHA-256 values from the governing evidence record or Director-supplied integrity contract; never invent an expected hash from the file being checked.
2. Run `& $env:ENSEMBLE_PYTHON -I -B tools/codex-admin-measure.py evidence-integrity --root <root> --expect <path=sha256>`, repeating `--expect` for each required file.
3. Require every expected file to exist and match exactly. A missing or mismatched file fails closed.
4. Report path, size, expected hash, actual hash, and match status. Do not inspect secrets unless the governing task explicitly authorizes their content; hashing does not require content disclosure.
5. Integrity success proves only byte identity against the supplied expectation. It does not prove causal interpretation, validation, provider success, authorization, or project authority.