# E0-A Phase B — Native Windows ARM64 Validation

Status: **DIRECTOR-MACHINE-SOURCED — PASS**
Date: 2026-09-06

## Authority

Exact validated branch:

`e0a-reference-run-envelope-implementation`

Exact validated executable/test checkout:

`3749210393282f6aa2ac4ceb0176b6adb5df189e`

This checkout includes the test-only correction required by native validation attempt 01. Any commits created after this validation checkout solely to record or reconcile evidence are not part of the validated executable/test surface.

Tracked and staged trees were clean before and after validation:

```text
TRACKED_DIFF_EXIT=0
STAGED_DIFF_EXIT=0
POST_TRACKED_DIFF_EXIT=0
POST_STAGED_DIFF_EXIT=0
POST_VALIDATION_HEAD=3749210393282f6aa2ac4ceb0176b6adb5df189e
```

One pre-existing untracked non-material file was present:

`patch0012-local-edit.txt`

No untracked file existed under `src/`, `tests/`, or `fixtures/`, so it did not contaminate the validation surface.

## Director-machine environment

```text
PROCESSOR_ARCHITECTURE=ARM64
Windows 10.0.26200
.NET SDK 9.0.317
SDK commit 26570c2743
MSBuild 17.14.51+25f168cee
RID win-arm64
Host 10.0.11
Host Architecture arm64
```

The validation script explicitly removed `OPENAI_API_KEY` before test/build/smoke execution. No provider request, credential use, or spend occurred.

## Results

### Existing Core suite — PASS

```text
total: 622
succeeded: 622
failed: 0
skipped: 0
CORE_TEST_EXIT=0
```

### E0-A Harness suite — PASS

```text
total: 39
succeeded: 39
failed: 0
skipped: 0
HARNESS_TEST_EXIT=0
```

### Native ARM64 Harness build — PASS

```text
Ensemble.E0.Core succeeded
Ensemble.E0.Harness succeeded
HARNESS_BUILD_EXIT=0
```

The Harness output target was `net9.0/win-arm64`.

### Missing Raft fixture smoke — PASS

```text
Fixture validated: ensemble.e0.missing-raft@0.1.0
MISSING_RAFT_EXIT=0
```

### Generic fixture smoke — PASS

```text
Fixture validated: ensemble.e0.smoke@0.1.0
GENERIC_SMOKE_EXIT=0
```

### Validation command set — COMPLETE

```text
VALIDATION_COMMAND_SET_COMPLETE
```

## Attempt-01 closure

Native validation attempt 01 at `a456a74ff6995ecfebc25a6134b2ca25d6260432` exposed six `MSTEST0032` analyzer errors in the constant-oracle test. The correction at the validated checkout changed only the Harness test oracle and evidence documentation; no production source, Core source, fixture, approved constant, or Proposal 0.15 behavior changed.

The rerun at `3749210393282f6aa2ac4ceb0176b6adb5df189e` closed that defect: the Harness test project compiled and all 39 tests passed.

## Native-validation conclusion

The approved E0-A Phase B Reference Run Envelope implementation and its test surface are native-Windows-ARM64 validated at exact checkout `3749210393282f6aa2ac4ceb0176b6adb5df189e` for the authorized fake/local validation scope.

This evidence does **not** authorize or claim real OpenAI provider execution. Provider credentials and estimated-spend authorization remain a separate explicit Director gate.
