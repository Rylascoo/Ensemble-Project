# E0-D Implementation / Native Validation — Director Authorization

Date: 2026-09-14

Status: **AUTHORIZED — EXPERIMENT-ONLY IMPLEMENTATION + DETERMINISTIC ISOLATION TESTS + NATIVE WINDOWS ARM64 VALIDATION; LIVE ACTIVATION NOT AUTHORIZED**

## Director disposition

After E0-D method/preregistration preparation closed, Engineering Sol presented the exact next authorization boundary:

> Authorize experiment-only E0-D implementation under the frozen method/preregistration, including deterministic isolation tests and applicable compiler/native Windows ARM64 validation. Do not authorize RunIds, provider traffic, credentials, inference, spend, or live activation.

The Director replied: **approved**.

## Authorized scope

This disposition authorizes only:

- experiment-only implementation of the three frozen E0-D ablations plus their Full controls;
- deterministic tests proving single-variable isolation and Full-reference preservation;
- compiler/build/static validation;
- native Windows ARM64 validation on the Director validation host;
- credentialless provider-edge validation that must stop before any network request.

## Still prohibited

This authorization does **not** authorize:

- allocation or consumption of any E0-D live RunId/evidence namespace;
- execution-window creation or live slot activation;
- production credential use;
- provider probes, `countTokens`, generation, inference, or spend;
- retry/replacement slots or changes to the frozen six-slot preregistration;
- implementation changes after observing future transcript/provider outcomes;
- experiential scoring, mapping reveal, or E0-D closure.

The frozen method remains `docs/blueprint/E0D_ABLATION_CONTROLS_METHOD_AND_ISOLATION_CONTRACT_01.md`; the machine-readable preregistration remains `docs/evidence/E0D_Q_E0D_01_PREREGISTRATION_2026_09_14.json`.

## Candidate continuity

The implementation-only source candidate was first frozen at `56a441096538d526c678981f7ba0cc865e0e4616`; after source-neutral concurrent Administrator integration it was rebased unchanged onto current main as `2afda26825d7ecad11677f858d523300419aa23a`. Deterministic native ARM64 test coverage reached Core 626/626 and Harness 154/154 with zero provider traffic. Native validation authority must attach only to a later exact clean checkout that contains this source unchanged and a current repository-state checkpoint; a validation tag may be created only after all applicable native/static gates pass.
