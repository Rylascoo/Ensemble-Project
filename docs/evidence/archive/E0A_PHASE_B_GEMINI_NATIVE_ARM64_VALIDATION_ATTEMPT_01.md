# E0-A Phase B — Gemini Normative Reference Native Windows ARM64 Validation Attempt 01

Status: **DIRECTOR-MACHINE-SOURCED — FAIL — HARNESS COMPILER DEFECT FOUND AND REPAIRED; RERUN REQUIRED**

Date: **2026-09-06**

This record preserves the first native Windows ARM64 validation attempt for the Gemini normative-reference amendment. It is failure evidence, not a validation pass, and it does not authorize Gemini credentials, provider requests, network inference, or spend.

## Intended validation authority

- Active work branch: `e0a-gemini-normative-reference-amendment`
- Promoted `main` observed: `f9cb1a79ad7923b640ff1a97c46d8fd6ab9dac25`
- Pre-attempt frozen source/test checkpoint: `39e7984b9ff59228d7d0c424ef7d113582d38bc7`
- Later synchronized amendment tip used for focused reproduction: `a78bae72ec0f259b3123b968dc5135829c6443b9`
- Repair commit after diagnosis: `117635d820792420cf0c30d987f3e0f534afeda0`

The pre-attempt pending-validation record incorrectly described the static audit as closed. Native compilation falsified that closure. `39e7984b9ff59228d7d0c424ef7d113582d38bc7` must not be promoted as a validated executable/test checkpoint.

## Initial Director-machine attempt at `39e7984b...`

### Checkout and working-tree authority

Observed:

- exact HEAD: `39e7984b9ff59228d7d0c424ef7d113582d38bc7`;
- `origin/main`: `f9cb1a79ad7923b640ff1a97c46d8fd6ab9dac25`;
- tracked diff exit: `0`;
- staged diff exit: `0`;
- unrelated untracked root file: `patch0012-local-edit.txt`;
- no material untracked files under `src/`, `tests/`, or `fixtures/`.

### Native Windows ARM64 environment

Observed:

- `PROCESSOR_ARCHITECTURE=ARM64`;
- .NET SDK `9.0.317` selected by repository `global.json`;
- Windows `10.0.26200`;
- `dotnet --info` RID `win-arm64`;
- Host Architecture `arm64`.

These satisfy the established Director-host architecture probes.

### Core tests

PASS at the attempted checkout:

- total: `622`;
- succeeded: `622`;
- failed: `0`;
- skipped: `0`;
- native exit: `0`.

This is valid Director-machine evidence for the unchanged Core surface at that attempted checkout. It does not validate the Harness.

### Harness tests / compilation

FAIL before Harness tests could execute.

Compiler result:

```text
src\Ensemble.E0.Harness\Evidence\E0AEvidenceStore.cs(546,2): error CS1513: } expected
```

Therefore:

- Harness test assertions were not executed;
- no Harness test count may be claimed for this attempt;
- the Harness executable/test surface was not validated.

### Native ARM64 Harness build

FAIL with the same compiler defect:

```text
src\Ensemble.E0.Harness\Evidence\E0AEvidenceStore.cs(546,2): error CS1513: } expected
```

No newly built Gemini Harness binary was produced by this step.

### Fixture smoke output after build failure

The command sequence continued after the expected build gate had failed and found an existing `net9.0\win-arm64\Ensemble.E0.Harness.dll` on disk. Missing-Raft and generic fixture commands printed successful validation messages from that pre-existing binary.

Those outputs are **NON-AUTHORITATIVE STALE-BINARY OUTPUT** for this Gemini amendment attempt. They must not be counted as fixture-smoke validation for `39e7984b...` or any Gemini amendment checkpoint.

### Credentialless output after build failure

The continued command sequence invoked the same pre-existing binary with `GEMINI_API_KEY` absent. The native process exited `1`, but emitted:

```text
OPENAI_API_KEY is required at the E0-A provider edge.
```

rather than the expected Gemini refusal.

Because the current Harness had failed to compile and the invoked DLL was stale historical output, this result is **NON-AUTHORITATIVE STALE-BINARY OUTPUT**, not evidence that the Gemini source path requests `OPENAI_API_KEY`.

The stale invocation created no evidence root and did not use a Gemini credential. No Gemini provider request, network inference, or spend occurred.

### Post-attempt checkout integrity

Observed after the initial attempt:

- HEAD remained `39e7984b9ff59228d7d0c424ef7d113582d38bc7`;
- tracked diff exit `0`;
- staged diff exit `0`.

## Focused reproduction after synchronizing the amendment branch

The Director switched back to `e0a-gemini-normative-reference-amendment` and fast-forwarded the local branch to the then-current remote tip:

`a78bae72ec0f259b3123b968dc5135829c6443b9`

At that exact synchronized tip:

- native ARM64 Harness build again failed with `E0AEvidenceStore.cs(546,2): error CS1513: } expected`;
- Core tests again passed `622/622`;
- Harness tests were again blocked by the same compiler defect.

This reproduction establishes that the defect was present in the current amendment source, not merely an artifact of detached-checkout selection.

## Root cause

Repository audit traced the syntax regression to commit:

`435ca9c1e410ec8014dcd9ccea3c6ccf3412e32d` — `Bind runtime evidence to active Gemini amendment`

That commit accidentally deleted the closing brace of the `foreach` loop in `E0AFileEvidenceStore.IsLowerHex` while adding Gemini evidence-manifest logic. The compiler therefore reached end-of-file still expecting a structural close.

The defect was syntactic. The surrounding Gemini pricing, provider-transport, role-manifest, sealing, and evidence-authority logic did not require redesign.

## Repair

Smallest correction committed on the active Gemini amendment branch:

`117635d820792420cf0c30d987f3e0f534afeda0` — `Fix E0-A evidence hex validator syntax`

Repository comparison from `a78bae72...` to `117635d...` contains exactly:

- one modified path: `src/Ensemble.E0.Harness/Evidence/E0AEvidenceStore.cs`;
- one insertion;
- zero deletions.

The inserted line restores the missing closing brace after the invalid-hex branch of the `foreach`, matching the already-validated historical structure from promoted `main`.

No Core file changed. No evidence semantics, Gemini route policy, provider budget, request schema, or deterministic authority law changed.

## Validation classification

Attempt 01 classification:

```text
Checkout / working-tree authority        PASS
Native ARM64 environment probes          PASS
Core tests                               PASS 622/622
Harness compilation                      FAIL CS1513
Harness tests                            NOT EXECUTED / BLOCKED
Harness native ARM64 build               FAIL CS1513
Post-failure fixture smokes               NON-AUTHORITATIVE STALE-BINARY OUTPUT
Post-failure credentialless probe         NON-AUTHORITATIVE STALE-BINARY OUTPUT
Gemini provider execution                 NOT PERFORMED
Gemini spend                              USD 0 / NOT PERFORMED
Overall Gemini native validation          FAIL — RERUN REQUIRED
```

## Next gate

Native validation must be rerun against a new exact source/test checkpoint containing the repair. The rerun must not reuse pre-existing Harness binaries after a failed build: fixture and credentialless smokes count only if the immediately preceding native ARM64 build succeeds for the exact validated checkout.

The Gemini credential/network/spend gate remains closed. A successful credentialless native rerun still will not authorize a real Gemini request.
