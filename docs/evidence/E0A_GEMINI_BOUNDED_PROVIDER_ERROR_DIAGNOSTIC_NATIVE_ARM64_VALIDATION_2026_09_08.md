# E0-A Gemini Bounded Provider-Error Diagnostic — Native Windows ARM64 Validation

Status: **PASS — PROMOTED MACHINE-TESTED CHECKOUT**

Date: **2026-09-08**

## Exact checkout

`e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`

Native attempt 02 validated this exact clean detached checkout after failed attempt 01 at `20f76e7d5f13915471581fb9263d6a0eb1e5343c` exposed two stale malformed-`countTokens` test expectations. The correction between attempts was test-only; production source semantics did not change.

## Hosted prerequisite

Hosted workflow `34309626093` at exact checkout `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a` completed successfully before the native rerun:

- ARM64 cross-compile compiler gate: PASS;
- required x64 Core regression: PASS;
- repository law enforcement: PASS;
- oracle assertion coverage: PASS;
- document authority census: PASS.

Hosted CI did not execute Harness tests and therefore did not itself provide native-runtime authority.

## Host

Director Windows ARM64 machine:

- Windows `10.0.26200`;
- .NET SDK `9.0.317`;
- runtime host `10.0.11`, architecture `arm64`;
- RID `win-arm64`;
- `PROCESSOR_ARCHITECTURE=ARM64`.

The validation worktree Git common directory resolved to the canonical `Ensemble-Project` repository. The historical validated root remained a separate clean detached worktree at `689655eed677b789ab3ee395f1c65b4f2cb72cc8` before and after validation.

## Native tests

```text
Core tests      622/622 PASS
Harness tests   130/130 PASS
```

## Fresh native build and fixture smokes

All stale Core/Harness/test `bin` and `obj` outputs were removed before execution.

- fresh Debug Harness build: `net9.0/win-arm64` PASS;
- build result: 0 warnings, 0 errors;
- frozen Missing Raft fixture smoke: PASS (`ensemble.e0.missing-raft@0.1.0`);
- generic E0 smoke fixture: PASS (`ensemble.e0.smoke@0.1.0`).

## Credentialless provider boundary

Before provider-edge probes, process-scoped `GEMINI_API_KEY` and `OPENAI_API_KEY` were explicitly absent.

Current live profiles each returned the exact expected missing-key refusal:

- `GEMINI-3.5-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL`;
- `GEMINI-3.1-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL`;
- `GEMINI-2.5-FLASH-LITE-NONE` / `CREATIVE-NONE`.

The retired `GEMINI-2.5-FLASH-NONE` route returned the expected pre-credential live-selection rejection. Each probe was required to leave its evidence root absent.

## Post-validation integrity

After all native tests, build, fixture smokes, and credentialless gates:

- validation worktree `HEAD` remained exactly `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`;
- checkout remained detached;
- tracked and staged trees remained clean;
- no material untracked `src/`, `tests/`, or `fixtures/` path remained;
- provider-key environment variables remained absent;
- historical validated root remained clean and detached at `689655eed677b789ab3ee395f1c65b4f2cb72cc8`.

The packet reached `BOUNDED_DIAGNOSTIC_NATIVE_RERUN=PASS`.

## Provider/network scope

```text
Gemini provider network   NOT PERFORMED
Gemini inference          NOT PERFORMED
Gemini spend              0
Provider authorization    NONE
```

This validation establishes native Windows ARM64 execution authority only for the exact bounded-diagnostic checkout. It does not establish live Gemini compatibility, successful provider `countTokens`, account/quota availability, inference, generation quality, live usage accounting, latency, or spend behavior.

## Promotion

Annotated validation tag:

`validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64`

Tag object:

`9b17563474b25920da8615afefdfa3fcfdab884b`

Independent GitHub readback confirmed that annotated tag object dereferences exactly to `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`.

The temporary connector-created closeout probe was separately archived as `archive/tmp-e0a-native-closeout-probe` (tag object `64c1830ff95171ada835c61b8a79a4024a04f2c2`, same exact target) and its remote branch was deleted. It never carried unique project content or authority.

`CURRENT_STATE.md` and `docs/VALIDATION_LEDGER.md` may therefore name `e6e7d6c8...` as the current promoted machine-tested checkpoint. Later documentation-only commits do not inherit native runtime authority. Validation does not authorize provider traffic.
