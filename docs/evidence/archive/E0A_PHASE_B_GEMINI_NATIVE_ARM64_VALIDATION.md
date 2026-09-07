# E0-A Phase B — Gemini Normative Reference Native Windows ARM64 Validation

Status: **DIRECTOR-MACHINE-SOURCED — PASS — CREDENTIALLESS / FAKE-ONLY SCOPE; REAL GEMINI NETWORK EXECUTION NOT AUTHORIZED**

Date: **2026-09-07**

This record preserves the successful Director-machine native Windows ARM64 validation of the compiler-repaired Gemini normative-reference amendment. It establishes native compiler/runtime evidence only for the exact checkout and command scope recorded below. It does not authorize a Gemini credential, `countTokens`, inference, provider-network execution, or spend.

## Exact validated checkout

`9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`

Promoted `origin/main` observed by the command set:

`c882233f589fb8c809a7d3aaa12893172e026d3c`

The validated checkout is the one-commit test-surface repair descendant of prior branch head `00b9052fdd4c65cc200ed01c61f3f2a80c10dc62`. The repair adds only the missing `Ensemble.E0.Core.Integrity` namespace import to three Gemini Harness test files. No product source changed in that repair.

## Checkout and host authority

Observed before execution:

- exact detached HEAD: `9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`;
- tracked tree clean;
- staged index clean;
- only observed nonmaterial untracked file: root-level `patch0012-local-edit.txt`;
- `PROCESSOR_ARCHITECTURE=ARM64`;
- Windows `10.0.26200`;
- .NET SDK `9.0.317` selected by repository `global.json`;
- `dotnet --info` RID `win-arm64`;
- Host Architecture `arm64`.

## Core tests

PASS:

- total: `622`;
- succeeded: `622`;
- failed: `0`;
- skipped: `0`;
- native exit: `0`.

## Harness tests

PASS:

- total: `81`;
- succeeded: `81`;
- failed: `0`;
- skipped: `0`;
- native exit: `0`.

The Harness and Harness test projects compiled successfully and the native ARM64 Harness test assembly executed successfully.

## Fresh native ARM64 Harness build

Before the authoritative build, the validation command removed the prior `src/Ensemble.E0.Harness/bin/Debug/net9.0/win-arm64` and corresponding target-specific `obj` output to prevent stale-binary reuse after a failure.

PASS:

- target: `win-arm64`;
- configuration: Debug;
- build exit: `0`;
- fresh DLL present at `src/Ensemble.E0.Harness/bin/Debug/net9.0/win-arm64/Ensemble.E0.Harness.dll`.

## Fixture smokes

PASS from the freshly built DLL:

- Missing Raft fixture: `Fixture validated: ensemble.e0.missing-raft@0.1.0`; exit `0`.
- Generic smoke fixture: `Fixture validated: ensemble.e0.smoke@0.1.0`; exit `0`.

## Credentialless Gemini provider-edge gate

Both `OPENAI_API_KEY` and `GEMINI_API_KEY` were removed/required absent before validation.

The explicit `e0a-run CREATIVE-NONE` credentialless probe used the freshly built DLL and exact executable checkout identity.

PASS of the expected-failure gate:

- native exit: `1`;
- required Gemini missing-credential refusal observed: `GEMINI_API_KEY is required at the E0-A provider edge.`;
- evidence root remained absent;
- `OPENAI_API_KEY` remained absent;
- `GEMINI_API_KEY` remained absent.

This proves the active compiled Gemini host reaches its provider edge and fails closed before evidence-root creation when the Gemini credential is absent. It is not provider-network or inference evidence.

## Post-validation authority

Observed after validation:

- exact HEAD remained `9bb65a8461c0963d9a9c6e9647633ca5c6d5df24`;
- tracked diff exit `0`;
- staged diff exit `0`.

## Classification

```text
Checkout / working-tree authority        PASS
Native Windows ARM64 host probes         PASS
Core tests                               PASS 622/622
Harness tests                            PASS 81/81
Fresh native ARM64 Harness build         PASS
Missing Raft smoke                       PASS
Generic fixture smoke                    PASS
Credentialless Gemini provider edge      PASS expected refusal
Evidence-root absence                    PASS
Gemini credential use                    NOT PERFORMED
Gemini provider-network execution        NOT PERFORMED
Gemini spend                             NOT PERFORMED
Overall credentialless native validation PASS
```

## Validation boundary

This PASS does not establish live Gemini API compatibility, `countTokens` behavior, live model availability, current quota, provider pricing/data-use terms, inference correctness, provider usage accounting, or spend. Those require separate provider execution after explicit Director authorization and the amendment's required pre-network re-verification.

The prior Attempt 01 remains valid failure evidence and is not erased by this PASS.

## Repository-surface gate

Before this checkout is promoted in `CURRENT_STATE.md` as a machine-validated checkpoint, Repository Surface law requires an annotated validation tag at exactly `9bb65a8461c0963d9a9c6e9647633ca5c6d5df24` recording validation level, scope, and this evidence document.
