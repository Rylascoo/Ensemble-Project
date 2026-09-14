# E0-D Ablation Controls — Native Windows ARM64 Validation

Date: 2026-09-14

Status: **NATIVE MACHINE VALIDATION PASS — EXACT CHECKOUT `5ce587b6c9ec83b91f3b5d63a2545cce9b1fe891` — ANNOTATED TAG PUSHED — PROVIDER NETWORK NOT PERFORMED**

## Authority boundary

This record validates the Director-authorized E0-D experiment-only implementation under `docs/blueprint/E0D_ABLATION_CONTROLS_METHOD_AND_ISOLATION_CONTRACT_01.md`, `docs/evidence/E0D_Q_E0D_01_PREREGISTRATION_2026_09_14.json`, `docs/evidence/E0D_IMPLEMENTATION_NATIVE_VALIDATION_DIRECTOR_AUTHORIZATION_2026_09_14.md`, and `docs/evidence/E0D_ABLATION_CONTROLS_IMPLEMENTATION_AUDIT_2026_09_14.md`.

```text
checkout   5ce587b6c9ec83b91f3b5d63a2545cce9b1fe891
source     2afda26825d7ecad11677f858d523300419aa23a
base       0c2ef2c0e8e141a03237ed4eaf00ce8b56c75a7a
branch     e0d-experiment-implementation-2026-09-14
tag        validation/e0d-ablation-controls-native-arm64
tag object 8eae0730664a4dfa21debdae5a2f73e070d5982d
target     5ce587b6c9ec83b91f3b5d63a2545cce9b1fe891
```

The checkpoint adds authority documentation only after source commit `2afda268…`; `src/**` and `tests/**` have zero delta between those commits. The annotated tag was created/pushed only after all gates below passed and was verified to peel exactly to the checkpoint.

## Native host

Validation ran on the Director `SurfSeven` Windows ARM64 host using repository-selected .NET SDK `9.0.317` and target `win-arm64`.

## Native executable results

At exact clean checkout `5ce587b6c9ec83b91f3b5d63a2545cce9b1fe891`:

- Release Harness build for `net9.0/win-arm64`: **PASS**, 0 warnings / 0 errors;
- `Ensemble.E0.Core.Tests`: **626/626 PASS** on `[net9.0|arm64]`;
- `Ensemble.E0.Harness.Tests`: **154/154 PASS** on `[net9.0|arm64]`;
- Missing Raft fixture smoke: exit 0, `Fixture validated: ensemble.e0.missing-raft@0.1.0`;
- generic fixture smoke: exit 0, `Fixture validated: ensemble.e0.smoke@0.1.0`;
- exact executable SHA-256: `498c25f528ab1ba26a592d247737186ac839c61b56154303d6e57a94319d0769`.

Harness coverage includes deterministic 12-turn, zero-network executions for Full reference, relationship omission, omniscient context, and round-robin.

## Credentialless provider-edge validation

With `GEMINI_API_KEY` explicitly absent, the exact-checkout executable invoked `e0d-run E0D-FULL-REFERENCE-01` against the canonical Missing Raft fixture using a non-live synthetic validation identity/root.

The host failed at the existing provider credential edge with `GEMINI_API_KEY is required at the E0-A provider edge.` Exit code was **1** and the proposed evidence root did **not** exist afterward.

Provider network, `countTokens`, generation, inference, and spend were **NOT PERFORMED**.

## Static / repository gates

At the exact validation checkpoint:

- `tools/repository-law-check.py`: **PASS**;
- `tools/document-census.py --summary --check`: **PASS**, 291 inventoried / 170 current / 30 historical / 91 archive / 0 unexplained current;
- `tools/oracle-index.py --check`: **PASS**, 200 documented / 17 asserted / 183 document-only hashes;
- `git diff --check`: **PASS**;
- worktree cleanliness before native execution: **PASS**;
- source identity from implementation source `2afda268…` to checkpoint `5ce587b…`: **zero `src/**` / `tests/**` delta**.

## Conclusion

Exact checkout `5ce587b6c9ec83b91f3b5d63a2545cce9b1fe891` is machine-validated on native Windows ARM64 for the frozen E0-D ablation-controls implementation.

Native runtime authority attaches only to that exact validated checkout and `validation/e0d-ablation-controls-native-arm64`. Later evidence/integration commits do not inherit native runtime authority; they may only reference this validated executable lineage.

This validation authorizes integration of the bounded implementation only. It does not authorize E0-D live RunIds, namespaces, execution windows, credentials, provider traffic, inference, spend, scoring, or activation.
