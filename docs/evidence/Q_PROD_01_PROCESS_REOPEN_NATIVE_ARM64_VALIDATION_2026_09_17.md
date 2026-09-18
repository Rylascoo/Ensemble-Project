# Q-PROD-01 Process Close/Reopen/Recovery - Native ARM64 Validation

Date: 2026-09-17

Status: **TARGET-DEVICE PROCESS EVIDENCE / CURRENT P1 SLICE / INTEGRATION PENDING**

## Identity

- Exact Project checkout under test: `1ab6b5d0ea0e49afd565485e22a4e17e6d758bf3`.
- That checkout is exact-main after lifecycle-reconstruction continuity PR #179; push Validation #962 PASS.
- Product lifecycle native source remains `7694f75479445c1f5ad030e37b1800a53d34dae9`.
- Process-evidence tag: `validation/q-prod-01-process-reopen-native-arm64`, annotated and peeled to exact checkout `1ab6b5d...`.
- Host: SurfSeven, native Windows ARM64.

## Evidence method

No shipping project or repository executable was added for this validation. A preserved external evidence-only console driver was built against the exact checkout's `Kymaean.Application` and `Kymaean.Infrastructure.Persistence` projects with `RuntimeIdentifier=win-arm64`, framework-dependent, warnings as errors.

Preserved local evidence root:

```text
C:\Users\Wiryl\Sol Dev\Ensemble-Project-LocalEvidence\q-prod-01-process-reopen-1ab6b5d
```
External evidence identities:

- driver EXE SHA-256: `fe821698083c5784c7cced8dc060df52cb29ae47f159f3dd279bb50fb165d0ec`;
- driver source SHA-256: `e02edebe7345640df1398ff0a6fdcafb1591fa5f54e2affe61bb66d2038e7f5c`;
- driver project SHA-256: `b459dbf933d8ce4663d2f69e2f7d462f994b162b8646c4fb8e6cbcab937dde97`;
- process log SHA-256: `d4d6c6d37f081d24bcd61c63b6dad509381f0cb2c5a0b42af171f6e3f1e5cace`.

The external driver is validation apparatus only and creates no Product/runtime authority.

## Scenario A - separate-process ordinary reopen

A fresh storage root was created.

1. ARM64 process PID 5748 executed `create` for `ProcessBoundaryOne` and exited 0.
2. The process was confirmed absent after return.
3. A distinct ARM64 process PID 9768 executed `open` against the same storage root.
4. It reconstructed `ProcessBoundaryOne` and exited 0.
5. The second process was confirmed absent after return.

The storage root retained one journal record plus `.journal.head` and `.journal.lock`.

Result: **PASS**.

## Scenario B - separate-process recovery and later reopen

A second fresh storage root was created.
1. ARM64 process PID 22564 executed `create` for `ProcessBoundaryRecovery` and exited 0.
2. The process was confirmed absent.
3. The durable `.journal.head` was removed to simulate interruption before initial head publication.
4. Distinct ARM64 process PID 6584 executed `recover`, reconstructed `ProcessBoundaryRecovery`, republished the durable head, and exited 0.
5. That process was confirmed absent.
6. Distinct ARM64 process PID 22108 then executed ordinary `open`, reconstructed the same Production from committed history, and exited 0.
7. All three PIDs were distinct and no child process survived.

Result: **PASS**.

## Earned scope

For the current minimal Product event slice, target-device persistence now has direct evidence across real OS process boundaries for:

- create, process termination, fresh-process reopen and deterministic reconstruction;
- initial-head-loss recovery in a fresh process;
- subsequent fresh-process ordinary reopen from the recovered committed head;
- native ARM64 execution for every evidence process.

This advances the P1 target-device close/reopen/recovery requirement for the currently implemented Production-name history.

## Non-authority
This evidence does not establish:

- packaged WinUI application close/relaunch wiring to Product persistence;
- device reboot, suspend/resume, forced termination during arbitrary write phases, or power-loss certification;
- complete crash-consistency coverage across every filesystem interruption point;
- a complete Production lifecycle beyond the existing creation event;
- Production identity, Cast/Scene lifecycle, accepted-performance or consequence schemas;
- snapshots or snapshot rebuild policy;
- portable export/import/migration policy;
- final storage/concurrency policy;
- complete P1, final architecture, Alpha/Beta/release, WACK, Store or deferred-E0 conclusions.

No Gemini/provider traffic occurred. No deferred-E0 namespace was consumed.

## Next

Integrate this evidence-only checkpoint through repository authority and hosted validation. Then continue the next smallest unearned P1 capability without widening Product ontology prematurely.
