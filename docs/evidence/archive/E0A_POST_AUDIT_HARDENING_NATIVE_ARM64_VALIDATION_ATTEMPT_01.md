# E0-A Post-Audit Hardening — Native Windows ARM64 Validation Attempt 01

Status: **DIRECTOR-MACHINE-SOURCED — FAIL — STALE TEST ORACLE IDENTIFIED AND REPAIRED; FULL RERUN REQUIRED**

Date: **2026-09-07**

This record preserves the first Director-machine native Windows ARM64 validation attempt of the compiler-repaired post-audit hardening candidate. It is failure evidence, not a validation pass, and it does not authorize provider credentials, provider-network execution, inference, or spend.

## Exact attempted checkout

`dc7ac428f00041f71f3ae78baa9b7bf83c61ce55`

Promoted `origin/main` observed by the command set:

`c882233f589fb8c809a7d3aaa12893172e026d3c`

The attempted checkout contains the one-line compiler repair that removes the redundant literal timeout assertion which previously triggered MSTEST0032. Comparison from prior branch head `8f5e9a0983beafe247a11f62922b8d2bd5974faa` to this candidate is exactly one deleted test assertion and no product-source change.

## Checkout and host authority

Observed before execution:

- exact detached HEAD: `dc7ac428f00041f71f3ae78baa9b7bf83c61ce55`;
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

## Harness compile and test execution

The Harness project and Harness test project both compiled successfully far enough for the native ARM64 test assembly to execute. The earlier MSTEST0032 compiler blocker is therefore closed at this candidate.

Native Harness test result:

- total: `88`;
- succeeded: `87`;
- failed: `1`;
- skipped: `0`;
- test command exit: `1`.

The failing test was later retrieved from the exact native test log:

`BufferedWrongJsonType_FailsClosedAsTechnicalReceipt`

Failure:

```text
Expected: "provider-incomplete"
Actual:   "malformed-provider-response"
```

The failing assertion was at `tests/Ensemble.E0.Harness.Tests/OpenAIResponsesPortWireTests.cs:109` in the attempted checkout.

## Diagnosis

Repository history shows this is a stale test oracle, not a product-behavior defect.

Patch Group 2 / E-03 originally added the wrong-type buffered-response regression and required wrong provider JSON types to fail closed as a technical receipt with no semantic adoption. At that point the implementation classified the wrong-typed `status` case under the coarser `provider-incomplete` diagnostic.

Later Patch Group 3 / E-06 intentionally refined failed-response provenance in commit:

`ce23987c12431295ac78726d36c7cf570ffd02ad` — `Retain failed provider response provenance`

That change separates two cases:

- valid provider refusal/incomplete responses may retain validated nonsemantic identity/model/usage provenance under their corresponding diagnostic;
- malformed provider response structure, including a wrongly typed `status`, is classified `malformed-provider-response` and is not allowed to masquerade as validated incomplete-response provenance.

The E-03 behavioral obligation is therefore still satisfied: wrong-typed provider data terminates as a technical receipt with no semantic output. The later E-06 diagnostic refinement is the stronger/current contract. The test's exact diagnostic string did not follow that later refinement.

No provider source change is justified by this failure.

## Repair

Smallest correction committed on the hardening branch:

`5c70f619d6e951d89bb527a5945b014998573dab` — `Align buffered wrong-type diagnostic oracle`

The repair changes exactly one expected string in `OpenAIResponsesPortWireTests.cs`:

```text
provider-incomplete
```

to:

```text
malformed-provider-response
```

Comparison from the Attempt 01 branch/evidence head to the repair commit is one test-file modification, one addition, one deletion. No `src/**`, Core, fixture, provider implementation, project configuration, or runtime behavior changes.

## Later validation stages in Attempt 01

The validation function threw immediately when the Harness test exit was nonzero. Therefore the following hardening stages were **NOT EXECUTED** in Attempt 01:

- explicit clean-output native ARM64 Harness build;
- Missing Raft fixture smoke;
- generic fixture smoke;
- credentialless `e0a-run` provider-edge gate;
- post-validation cleanliness block inside the validation function.

No result from those stages may be claimed for `dc7ac428...` from this attempt.

No provider credential was intentionally set by the command set and no provider-network action was authorized.

## Classification

```text
Checkout / working-tree authority        PASS
Native Windows ARM64 host probes         PASS
Core tests                               PASS 622/622
Harness compilation                      PASS to native test execution
Harness tests                            FAIL 87/88
Failure cause                            STALE TEST ORACLE
Product-source defect                    NOT FOUND
Fresh explicit Harness build             NOT EXECUTED
Missing Raft smoke                       NOT EXECUTED
Generic fixture smoke                    NOT EXECUTED
Credentialless provider-edge probe       NOT EXECUTED
Provider execution                       NOT AUTHORIZED / NOT PERFORMED
Overall hardening native validation      FAIL — REPAIRED CANDIDATE REQUIRES FULL RERUN
```

## Next gate

Run the complete native Windows ARM64 validation sequence from the beginning against exact repaired source/test checkpoint:

`5c70f619d6e951d89bb527a5945b014998573dab`

A focused rerun of only the previously failing test is insufficient for promotion. Successful grouped validation must again establish exact checkout/cleanliness, trusted native ARM64 host probes, Core tests, all Harness tests, a fresh clean-output native Harness build, both fixture smokes, the credentialless provider-edge refusal with no evidence root, and post-validation cleanliness.

The historical live-host machine-tested authority remains unchanged until that repaired checkpoint passes completely.
