# E0-A State Interpreter Semantic-Output Alignment — Native Windows ARM64 Validation

Date: 2026-09-10

Status: **NATIVE MACHINE VALIDATION PASS — EXACT CHECKOUT `e052ef4cdaf8ac7291971f8c42d121e1ee6eecf0` — ANNOTATED TAG CREATED — PROVIDER NETWORK NOT PERFORMED**

## Authority boundary

This record validates only the prompt/semantic-contract alignment defined by `docs/blueprint/E0A_STATE_INTERPRETER_SEMANTIC_OUTPUT_ALIGNMENT_AMENDMENT.md` and audited in `docs/evidence/E0A_STATE_INTERPRETER_SEMANTIC_OUTPUT_ALIGNMENT_IMPLEMENTATION_AUDIT_2026_09_10.md`.

Exact source candidate:

```text
checkout   e052ef4cdaf8ac7291971f8c42d121e1ee6eecf0
base       5cd64307471a4095d71e5e4467386b866004e375
branch     q-e0a-03-run03-interpreter-semantic-alignment-2026-09-10
tag        validation/e0a-state-interpreter-semantic-output-alignment-native-arm64
tag object a6402b32f4840ee218365dec4b285197bd72548b
target     e052ef4cdaf8ac7291971f8c42d121e1ee6eecf0
```

The annotated tag was created only after every executable and static validation gate below passed and was verified to peel to the exact candidate.

## Native host and preconditions

Validation ran from clean detached short-path worktree `C:\Users\Wiryl\Sol Dev\E0V-e052ef4` with `PROCESSOR_ARCHITECTURE=ARM64`, SDK 9.0.317, RID `win-arm64`, Host Architecture `arm64`, and both `GEMINI_API_KEY` and `OPENAI_API_KEY` absent.

The exact committed Missing Raft Fixture blob was `6c2ed0e1081ee4e1165dbfb162b3028cbd1136fd`. The frozen canonical Fixture identity `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703` was verified through the successful Missing-Raft Harness smoke, which invokes `MissingRaftContract.Validate` and `FixtureHash.Compute`.

Fresh-output discipline cleared source/test `bin/` and `obj/` roots before authoritative tests and cleared Harness outputs again before the executable build/smokes.

## Native executable results

At exact checkout `e052ef4cdaf8ac7291971f8c42d121e1ee6eecf0`:

- `Ensemble.E0.Core.Tests`: **622/622 PASS** on `[net9.0|arm64]`;
- `Ensemble.E0.Harness.Tests`: **135/135 PASS** on `[net9.0|arm64]`;
- fresh Debug Harness build for `net9.0/win-arm64`: **PASS**, 0 warnings / 0 errors;
- Missing Raft fixture smoke: exit 0, `Fixture validated: ensemble.e0.missing-raft@0.1.0`;
- generic fixture smoke: exit 0, `Fixture validated: ensemble.e0.smoke@0.1.0`.

## Credentialless provider-edge validation

Using the freshly built candidate DLL with both provider keys absent:

- `GEMINI-3.5-FLASH-LITE-MINIMAL / CREATIVE-MINIMAL`: exit 1, exact missing-key refusal, no evidence root — PASS;
- `GEMINI-3.1-FLASH-LITE-MINIMAL / CREATIVE-MINIMAL`: exit 1, exact missing-key refusal, no evidence root — PASS;
- `GEMINI-2.5-FLASH-LITE-NONE / CREATIVE-NONE`: exit 1, exact missing-key refusal, no evidence root — PASS;
- retired `GEMINI-2.5-FLASH-NONE / CREATIVE-NONE`: exit 1, exact pre-credential non-approved-profile refusal, no evidence root — PASS.

Provider network, `countTokens`, generation, inference, and spend were **NOT PERFORMED** during validation.

## Static and preservation gates

Post-native exact HEAD remained `e052ef4cdaf8ac7291971f8c42d121e1ee6eecf0`, detached and clean, with no material untracked `src/`, `tests/`, or `fixtures/` files.

- `tools/repository-law-check.py`: PASS;
- `tools/document-census.py --summary --check`: PASS, 204 inventoried / 87 current / 30 historical / 87 archive / 0 unexplained current;
- `tools/oracle-index.py --check`: PASS, 100 documented hashes / 17 asserted / 83 document-only.

## Apparatus observations

Two wrapper-level false gates occurred before authoritative execution and created no provider traffic. First, a raw `Get-FileHash` comparison incorrectly treated canonical Fixture identity `5556...` as a raw working-tree byte digest; existing Run-01 host evidence already defines the correct Git-blob + Harness canonical-validation gate. Second, expected-failure stderr capture encountered Windows PowerShell decoration/null-output behavior until the wrapper used `System.Diagnostics.ProcessStartInfo` with explicitly quoted arguments and raw redirected streams. The final native assertions above use the corrected apparatus.

The Director host now has SDK 10.0.400 as the unpinned `dotnet` default. Microsoft Testing Platform rejects the repository's current test invocation under that SDK before tests execute. Validation therefore pinned the already-installed SDK 9.0.317 through an external temporary `global.json`, leaving the repository clean. This is host/tooling evidence, not Ensemble product behavior.

## Conclusion

Exact checkout `e052ef4cdaf8ac7291971f8c42d121e1ee6eecf0` is machine-validated on native Windows ARM64 for the E0-A State Interpreter semantic-output alignment correction.

Native authority attaches only to that exact source candidate and annotated tag. Later documentation commits do not inherit machine-tested authority. Run 03 remains immutable/noncontributing; a later fresh 3.5 full-reference run must use a new RunId/evidence root and satisfy the standing project-use activation law before provider traffic.
