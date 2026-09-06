# Director Windows ARM64 Validation Host — Known Behaviors

Status: **AUTHORITATIVE VALIDATION-HOST NOTES**

Date established: 2026-09-06

This document records validated behaviors of the Director's native Windows ARM64 validation host so future command sets do not rediscover the same apparatus quirks. These are host/tooling facts, not Ensemble product behavior.

## 1. Architecture probes

On this Director host, Windows PowerShell expressions using:

```powershell
[System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture
[System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture
[System.Runtime.InteropServices.RuntimeInformation]::RuntimeIdentifier
```

have returned blank values in validation sessions. They must not be used as authoritative architecture gates in this host.

Use these probes instead:

1. `PROCESSOR_ARCHITECTURE` must equal `ARM64`.
2. `dotnet --info` must succeed.
3. `dotnet --info` must report `RID: win-arm64`.
4. `dotnet --info` Host section must report `Architecture: arm64`.
5. Harness build output should resolve under `net9.0\win-arm64` for the current E0 Harness target.

These checks together are the trusted native Windows ARM64 validation probes for this host unless a later Director-machine observation supersedes this document.

## 2. Native stderr under `$ErrorActionPreference = 'Stop'`

In this Windows PowerShell host, a native process that writes to stderr can surface as a PowerShell `NativeCommandError` when `$ErrorActionPreference = 'Stop'`.

For expected-failure native probes, this can terminate or disrupt the wrapper before the intended native exit code/output assertions are captured. Visible console text alone is not a sufficient scripted oracle.

Therefore, when a native command is expected to fail as part of validation:

1. Save the current `$ErrorActionPreference`.
2. Temporarily set `$ErrorActionPreference = 'Continue'` for that native invocation.
3. Redirect stdout and stderr to explicit temporary files.
4. Capture `$LASTEXITCODE` immediately after the native command.
5. Restore the original `$ErrorActionPreference` in `finally`.
6. Read the captured files after the process exits.
7. Assert the native exit code, expected message, and any filesystem side effects independently.
8. Delete the temporary capture files after assertions.

Do not infer pass/fail from PowerShell's wrapper exception category.

## 3. Exit-code capture rule

`$LASTEXITCODE` must be copied to a dedicated variable immediately after every native command whose exit status is part of the oracle.

Do not run another native command before capturing it. Do not rely on a later `$LASTEXITCODE` value after formatting, cleanup, or secondary probes.

For expected-failure probes, stdout/stderr capture and exit-code capture must be separated from assertion logic so wrapper behavior cannot overwrite or obscure the native result.

## 4. Credentialless E0-A host probe

The credentialless live-host smoke is expected to:

- run with `OPENAI_API_KEY` absent;
- invoke the explicit `e0a-run` command;
- exit with code `1`;
- emit `OPENAI_API_KEY is required at the E0-A provider edge.`;
- create no evidence root;
- perform no provider inference, network call, or spend.

The wrapper must use the native-stderr handling rule above. Visible refusal text without captured exit code and filesystem assertions does not count as a complete pass.

## 5. Working-tree authority

Validation command sets must establish before testing:

- exact expected Git HEAD;
- `origin/main` identity for context;
- tracked diff clean;
- staged diff clean;
- no material untracked files under `src/`, `tests/`, or `fixtures/`.

Known unrelated root-level scratch files such as `patch0012-local-edit.txt` may be reported but are not material to the source/test/fixture authority check.

Post-validation, the command set must re-check exact HEAD plus tracked/staged cleanliness.

## 6. Command-generation rule

Future Director-machine Windows ARM64 validation command sets should be generated from this document rather than reintroducing unverified host assumptions.

At minimum they must:

- avoid `RuntimeInformation` PowerShell property gates on this host;
- use `PROCESSOR_ARCHITECTURE` and parsed `dotnet --info` for architecture;
- capture native exit codes immediately;
- isolate expected-failure stderr from `$ErrorActionPreference = 'Stop'`;
- assert credentialless failure by native exit code, message, and filesystem effects;
- preserve the credential/network/spend gate as closed unless separately authorized.

If later Director-machine evidence contradicts any item here, update this document first and then regenerate the validation command set from the revised host contract.
