# E0-D Source Snapshot Refresh — Native Windows ARM64 Validation

Date: 2026-09-14

Status: **NATIVE MACHINE VALIDATION PASS — EXACT CHECKOUT `0dacdbf6bd5453c192568cd4718207e145dfcf40` — ANNOTATED TAG PUSHED — PROVIDER NETWORK NOT PERFORMED**

## Authority and scope

Director authorization: `docs/evidence/E0D_SOURCE_SNAPSHOT_REFRESH_AND_NATIVE_REVALIDATION_DIRECTOR_AUTHORIZATION_2026_09_14.md`.

The validated source delta is intentionally narrow: `E0AGeminiPricingPolicy.VerifiedOn` is `2026-09-14`; inclusive `SnapshotValidThrough` is `2026-09-18`; `RequireNonStaleSnapshot` fails closed from UTC `2026-09-19`. No E0-D method, preregistration, fixture, model/profile, provider route, RunId/root, claim identity, pair window, retry law, scoring law, or experiment variant changed.

```text
checkout   0dacdbf6bd5453c192568cd4718207e145dfcf40
base       57ec145c09e701e5d62907ae75046f38b5cbe285
branch     e0d-snapshot-refresh-2026-09-14
tag        validation/e0d-snapshot-refresh-native-arm64
tag object f2df6e8be66e7f1a7106d82c89ae13d284442090
target     0dacdbf6bd5453c192568cd4718207e145dfcf40
```

## Native Windows ARM64 results

Validation ran on the Director `SurfSeven` Windows ARM64 host using repository-selected .NET SDK `9.0.317` and target `win-arm64`.

- Release Harness build: **PASS**, 0 warnings / 0 errors.
- `Ensemble.E0.Core.Tests`: **626/626 PASS** on native ARM64.
- `Ensemble.E0.Harness.Tests`: **154/154 PASS** on native ARM64.
- Missing Raft fixture smoke: **PASS**.
- generic fixture smoke: **PASS**.
- exact executable SHA-256: `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`.

## Credentialless provider-edge gate

With `GEMINI_API_KEY` explicitly absent, an E0-D Full-reference invocation exited **1** with `GEMINI_API_KEY is required at the E0-A provider edge.` The proposed synthetic validation evidence root remained absent and the checkout remained clean.

No provider request, `countTokens`, generation, inference, scoring, or spend occurred.

## Static / repository gates

At exact checkout `0dacdbf6bd5453c192568cd4718207e145dfcf40`:

- repository law: **PASS**;
- document census: **PASS — 300 inventoried / 179 current / 30 historical / 91 archive / 0 unexplained current**;
- oracle guard: **PASS — 215 documented / 17 asserted / 198 document-only hashes**;
- commit diff check: **PASS**;
- frozen six-slot evidence roots/run claims/root claims: **all absent/unconsumed**;
- checkout cleanliness before and after native execution: **PASS**.

## Conclusion

`0dacdbf6bd5453c192568cd4718207e145dfcf40` is the renewed native Windows ARM64 authority for the frozen E0-D executable through the inclusive UTC snapshot date `2026-09-18`. The exact tag is `validation/e0d-snapshot-refresh-native-arm64`.

This validation does **not** authorize namespace claims, evidence-root creation, execution credentials, provider traffic, inference, scoring, spend, or live slot execution. Every slot still requires its own fresh execution-sensitive capacity observation and separate live-execution authority.
