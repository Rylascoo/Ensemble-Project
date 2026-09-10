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
3. `dotnet --info` must semantically report `RID = win-arm64`.
4. `dotnet --info` Host section must semantically report `Architecture = arm64`.
5. Harness build output should resolve under `net9.0\win-arm64` for the current E0 Harness target.

These checks together are the trusted native Windows ARM64 validation probes for this host unless a later Director-machine observation supersedes this document.

### `dotnet --info` parsing rule

`dotnet --info` is human-formatted CLI output and aligns label/value fields with variable horizontal whitespace. Prior native evidence includes display text such as `RID:           win-arm64`. A wrapper must therefore not require the literal contiguous substring `RID: win-arm64` or any fixed count of spaces.

Parse the semantic label/value facts with whitespace-tolerant matching, for example:

```text
(?m)^\s*RID:\s*win-arm64\s*$
(?m)^\s*Architecture:\s*arm64\s*$
```

Equivalent structured parsing is acceptable. Fixed display spacing is not an authority invariant unless explicitly frozen. Attempt 04 of Q-E0A-03 established this rule after a local preflight false gate; no provider invocation occurred.

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

The historical OpenAI credentialless live-host smoke was expected to:

- run with `OPENAI_API_KEY` absent;
- invoke the explicit `e0a-run` command;
- exit with code `1`;
- emit `OPENAI_API_KEY is required at the E0-A provider edge.`;
- create no evidence root;
- perform no provider inference, network call, or spend.

The active Gemini amendment applies the same apparatus rule while requiring both provider credentials absent and expecting:

- `GEMINI_API_KEY` absent;
- exit code `1`;
- `GEMINI_API_KEY is required at the E0-A provider edge.`;
- no evidence root;
- no provider inference, network call, or spend.

The wrapper must use the native-stderr handling rule above. Visible refusal text without captured exit code and filesystem assertions does not count as a complete pass.

## 5. Working-tree authority

Validation command sets must establish before testing:

- exact expected Git HEAD;
- `origin/main` identity for context;
- tracked diff clean;
- staged diff clean;
- no material untracked files under `src/`, `tests/`, or `fixtures/`.

Known unrelated root-level scratch files such as `patch0012-local-edit.txt` may be reported but are not material to the source/test/fixture authority check.

Post-validation, the command set must re-check exact HEAD plus tracked/staged cleanliness and again reject material untracked source/test/fixture files.

## 6. Fresh-output / failed-build rule

Director-machine Gemini validation Attempt 01 established an additional apparatus hazard: a failed `dotnet build` can leave an older `bin\...\Ensemble.E0.Harness.dll` on disk from a prior successful build. `Test-Path` on that DLL can therefore succeed even though the current checkout did not compile.

Consequences:

- fixture smokes or credentialless probes run after a failed build may execute a stale historical binary;
- output from that stale binary is not evidence about the current checkout;
- a stale binary can expose historical behavior, such as an `OPENAI_API_KEY` refusal, even when current source is intended to use Gemini.

Future validation packets must therefore:

1. clear Harness and Harness-test `bin/` and `obj/` output roots before authoritative native test execution when the packet is intended to exclude stale test/build artifacts;
2. treat a failed Harness test build or explicit Harness build as an immediate hard stop for later executable smokes;
3. never infer a successful current build from the existence of an output DLL alone;
4. clear the target `bin\Debug\net9.0\win-arm64` and corresponding target-specific `obj` output before the explicit Harness build used for executable smokes, or otherwise prove the executable was produced by the successful current build;
5. run fixture and credentialless smokes only after the exact-checkout Harness ARM64 build returns native exit `0`;
6. classify any smoke output produced after a failed build as non-authoritative stale-binary output unless independent evidence proves otherwise.

## 7. Interactive PowerShell compound-statement rule

Director-machine Hardening Rerun 02 established that a multi-line PowerShell expression can be accidentally submitted as separate interactive commands. In particular, entering:

```powershell
$value = if (Test-Path $path) {
    Get-Content $path -Raw
}
else {
    ''
}
```

as separate submissions can leave the `if` assignment successfully completed while the later standalone `else` token produces `else : The term 'else' is not recognized`.

Therefore command packets intended for manual interactive entry must keep an `if ... else ...` expression in one submitted statement (for example, one line), or otherwise structure it so the parser receives the `else` as part of the same compound statement. A parser message of this kind must be classified from the surrounding assertions and side effects rather than treated as either product failure or automatic pass.

## 8. Command-generation rule

Future Director-machine Windows ARM64 validation command sets should be generated from this document rather than reintroducing unverified host assumptions.

At minimum they must:

- avoid `RuntimeInformation` PowerShell property gates on this host;
- use `PROCESSOR_ARCHITECTURE` and whitespace-tolerant semantic parsing of `dotnet --info` for architecture;
- never freeze incidental CLI alignment spacing as a semantic oracle;
- capture native exit codes immediately;
- isolate expected-failure stderr from `$ErrorActionPreference = 'Stop'`;
- assert credentialless failure by native exit code, message, and filesystem effects;
- hard-stop executable smokes after any failed current-checkout Harness build;
- use fresh-output discipline when stale artifacts could contaminate evidence;
- keep interactive compound statements parser-safe;
- preserve the credential/network/spend gate as closed unless separately authorized.

If later Director-machine evidence contradicts any item here, update this document first and then regenerate the validation command set from the revised host contract.

## 9. Long-worktree path rule for repository guards

Q-E0A-03 structured-output compatibility validation established that a repository guard may fail on this host when its worktree path is sufficiently long even though the exact checkout is valid. `tools/oracle-index.py --check` attempted a Git `<revision>:<document-path>` stat from the long native-validation worktree and Windows reported `Filename too long` before an oracle-coverage result existed.

This is an apparatus/path failure, not a passing or failing oracle result.

When a deterministic repository guard fails solely on this class of path-length error:

1. keep the native executable evidence bound to its exact validated commit;
2. create a separate clean detached worktree for the **same exact commit** at a materially shorter path;
3. rerun only the affected static repository guard there;
4. require the guard itself to return success before promotion;
5. record both the original apparatus failure and the short-path result in validation evidence;
6. never use a different commit, branch tip, or later documentation commit as a substitute.

For the establishing case, `tools/oracle-index.py --check` passed at `C:\Users\Wiryl\Sol Dev\E0V-cef3fc1` on exact checkout `cef3fc15e31192a48aa3bddd99450b65a58bd8f1`, reporting 86 documented hashes, 17 asserted hashes, and 69 document-only hashes.
