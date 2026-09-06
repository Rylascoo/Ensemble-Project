# E0-A Phase B Live-Host Completion — Native Windows ARM64 Validation

Status: **DIRECTOR-MACHINE-SOURCED — PASS**

Date: 2026-09-06

This record covers fake-only/native validation of the E0-A Phase B live-host completion. It does not authorize credentials, provider requests, network inference, or spend.

## Exact machine-tested authority

- Branch: `e0a-phase-b-live-host-completion`
- Exact validated executable/test checkout: `1cfdb3aa22abce62a5bd48e80706407670d1c6a9`
- `origin/main` observed during validation: `13ffde90f78a8bf40a04381b136e846d1cf64d1c`
- initial tracked diff exit: `0`
- initial staged diff exit: `0`
- unrelated untracked file: `patch0012-local-edit.txt`
- no material untracked source/test/fixture files

Any later documentation-only commit must not replace `1cfdb3aa22abce62a5bd48e80706407670d1c6a9` as the machine-tested executable/test authority unless the executable/test surface is rerun at that later checkout.

## Native Windows ARM64 environment

Director-machine observations:

- `PROCESSOR_ARCHITECTURE=ARM64`
- .NET SDK: `9.0.317`
- `dotnet --info` exit: `0`
- `dotnet --info` RID: `win-arm64`
- `dotnet --info` Host Architecture: `arm64`
- Windows version reported by .NET: `10.0.26200`
- Harness build output: `net9.0\win-arm64`

The validation command set was generated from `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`, which records the Director host's PowerShell/architecture-probe behaviors.

## Credential/network/spend state

`OPENAI_API_KEY` was explicitly removed before validation and remained absent.

No provider credential was used. No real provider inference request, provider-network execution, or spend was authorized or performed.

## Results

### Core tests

PASS.

- total: `622`
- succeeded: `622`
- failed: `0`
- skipped: `0`
- exit: `0`

### Harness tests

PASS.

- total: `54`
- succeeded: `54`
- failed: `0`
- skipped: `0`
- exit: `0`

Coverage caveat: the pricing-policy tuple test `PricingPolicy_FreezesSourcePromotionAndConservativeRates` is a **constant-freeze tripwire**, not behavioral pricing validation. Its pass contributes to the suite count but only proves that the compiled frozen constants match the independently maintained expected tuple in the test. It does not establish live provider pricing freshness, provider cache behavior, or runtime spend correctness. Those claims require separate runtime/provider evidence and remain subject to the known pricing-snapshot limitation.

### Harness native ARM64 build

PASS.

- exit: `0`
- output target: `src\Ensemble.E0.Harness\bin\Debug\net9.0\win-arm64\Ensemble.E0.Harness.dll`

### Fixture smokes

PASS.

- Missing Raft: `Fixture validated: ensemble.e0.missing-raft@0.1.0`, exit `0`
- Generic smoke: `Fixture validated: ensemble.e0.smoke@0.1.0`, exit `0`

### Credentialless live-host gate

PASS.

With `OPENAI_API_KEY` absent, the explicit live-host command:

- returned native exit code `1`;
- emitted the expected refusal text `OPENAI_API_KEY is required at the E0-A provider edge.`;
- created no evidence root;
- left `OPENAI_API_KEY` absent.

The PowerShell wrapper followed the recorded host contract: expected native stderr was isolated from `$ErrorActionPreference = 'Stop'`, stdout/stderr were captured, and `$LASTEXITCODE` was asserted independently.

This validates the credentialless host boundary only. It does not authorize or validate a real provider request.

## Post-validation integrity

- post-validation HEAD: `1cfdb3aa22abce62a5bd48e80706407670d1c6a9`
- post tracked diff exit: `0`
- post staged diff exit: `0`
- validation command set reached `VALIDATION_COMMAND_SET_COMPLETE`

## Pricing gate residual

The live host's `RequireNonStaleSnapshot` remains a date-validity guard, not live pricing verification. It fails closed after the published promotional-guarantee date, but it cannot detect provider changes to pricing terms, cache-write multiplier, long-context threshold, or related billing rules that occur earlier within that date window.

Closing that residual still requires the previously identified short-lived trusted pricing attestation or an equivalent stable machine-readable provider pricing source. This native PASS does not erase or narrow that known limit.

## Classification

E0-A Phase B live-host completion is **native Windows ARM64 validated for the fake-only/credentialless scope at exact checkout `1cfdb3aa22abce62a5bd48e80706407670d1c6a9`**.

The credential/network/spend gate remains closed. Real provider execution remains separately Director-authorized work.
