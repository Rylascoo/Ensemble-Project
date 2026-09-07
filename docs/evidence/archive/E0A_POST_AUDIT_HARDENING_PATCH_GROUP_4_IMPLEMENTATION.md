# E0-A Post-Audit Hardening — Patch Group 4 Implementation

Status: **IMPLEMENTED — RECURSIVE STATIC AUDIT COMPLETE; NATIVE VALIDATION PENDING**

Date: 2026-09-06

## Authority and checkpoint

Repository: `Rylascoo/Ensemble-Project`

Branch: `e0a-phase-b-post-audit-hardening`

Audited `main` baseline / merge base:

`f9cb1a79ad7923b640ff1a97c46d8fd6ab9dac25`

Patch Group 3 executable/test checkpoint:

`c3a6846d267ce3f93a3e80a297057d4a7a14d99d`

Exact executable/test checkpoint after Patch Group 4:

`2a55319820587b63da13377a775150b90c24b107`

Closed-audit authority:

`docs/evidence/E0A_POST_AUDIT_HARDENING_CLOSED_FINDINGS.md`

Proposal 0.15 remains unchanged. Provider execution, credentials, network inference, and spend remain unauthorized.

## Scope

Patch Group 4 implements together:

- **I-02 — Evaluation ordering**
- **I-03 — Evidence ownership**
- **I-04 — Seal integrity**
- **P-04 — Evaluation parsing**

## I-02 — Evaluation ordering and publication

The in-process `E0AFileEvidenceStore.SealEvaluation` path and the reopened `E0AExistingEvidenceEvaluationSealer.Seal` path now delegate to one shared `E0AEvidenceSealAuthority`.

The shared authority validates the complete sealed runtime before creating any authoritative evaluation artifact. After acquiring exclusive evaluation-publication ownership it re-verifies runtime integrity, closing the validation/publication race.

`evaluation/hard-gates.json` is staged before `evaluation.final.json`; only the final seal is authoritative. If final publication fails, the staged hard-gate artifact is removed so a clean retry remains possible. A stale hard-gate file without an evaluation seal is treated as interrupted/unendorsed publication and is replaced only while exclusive publication ownership is held.

## I-03 — Evidence and RunId ownership

`E0AEvidenceNamespaceAuthority` now claims both:

- the `RunId` within the parent evidence namespace; and
- the normalized evidence-root identity.

Claims use create-new file semantics, so same-namespace RunId reuse and same-root concurrent creation fail closed instead of racing through a check-then-create window.

Authoritative JSON artifact creation in `E0AFileEvidenceStore` also uses `FileMode.CreateNew`; a concurrent writer cannot overwrite an already-created request, terminal, manifest, seal, transcript, or other write-once artifact.

This is local evidence-namespace ownership only. No global/cloud persistence or later Production persistence was introduced.

## I-04 — Runtime-seal identity and rooted summary claims

Runtime sealing now creates `run.summary.json` before computing the runtime root. The summary is therefore included in the immutable runtime digest set and carries the endorsed terminal claims:

- terminal status;
- accepted-turn count;
- estimated spend;
- spend-estimate validity status;
- unknown-provider-usage state;
- final Production state hash;
- final Opportunity Character.

`run.final.json` repeats those claims for the final seal, records the exact artifact digest list/root, and adds a recomputable `runtimeSealIdentity` derived from the rooted runtime plus the rooted summary bytes.

Evaluation verifies:

1. the runtime artifact digest list;
2. the recomputed runtime root;
3. the rooted runtime summary;
4. the repeated `run.final.json` summary claims against that rooted summary;
5. the recomputed runtime-seal identity.

`evaluation.final.json` binds both the runtime root and runtime-seal identity and hashes the exact `run.final.json` bytes it endorsed. Changing `run.final.json` summary fields while retaining the old `runtimeRoot` can no longer be endorsed.

Patch Group 3 spend-validity and unknown-usage state is now part of the rooted runtime summary rather than existing only in the event stream/result object.

## P-04 — Evaluation parsing

The shared evaluation authority validates external JSON `ValueKind` before typed access, rejects malformed/ill-formed Unicode strings, requires exact canonical spend-status names, and translates JSON decoding failures into the established `E0AHarnessException` evaluation boundary.

Malformed evaluation identities/findings are rejected before authoritative evaluation publication.

Unexpected programming defects are not relabeled through a broad catch-all.

## Deletion quota

The correction naturally made the standalone implementation inside `E0AExistingEvidenceEvaluationSealer` redundant. That duplicate verification/publication implementation was removed and the class is now a thin compatibility entry point into the shared authority.

No useful behavioral, architectural, or regression test material was deleted merely to satisfy a numeric quota.

## Regression coverage

Added coverage proves:

- concurrent run creation produces one owner and cannot overwrite the manifest;
- same-namespace `RunId` reuse across fresh roots is rejected;
- concurrent creation of one authoritative write-once artifact permits exactly one publisher;
- `run.final.json` summary tampering cannot be endorsed while retaining the original runtime root;
- rooted runtime artifact tampering fails before evaluation publication;
- malformed evaluation Unicode fails before publication;
- wrongly typed runtime-seal contract data fails through the normal evaluation boundary before publication;
- concurrent evaluation publication produces exactly one authoritative evaluation seal;
- reopened and in-process evaluation paths bind the same runtime-seal identity.

Existing coverage continues to prove runtime tamper rejection, exactly-once reopened evaluation, blind evidence isolation, and credential absence from the manifest.

## Recursive static audit

The completed Group 4 surface was recursively checked across:

- manifest -> runtime -> evaluation staging;
- runtime-root recomputation;
- rooted summary binding;
- runtime-seal identity;
- evaluation validation before publication;
- interrupted/failed evaluation publication behavior;
- concurrent run/root ownership;
- concurrent write-once artifact creation;
- reopened versus in-process evaluation equivalence;
- malformed JSON/type/Unicode boundaries;
- secret/credential exclusion;
- simplicity, ARM64 suitability, and scope.

The recursive review incorporated two improvements before closure:

1. Group 3 spend-validity/unknown-usage state was promoted into the rooted runtime summary so evaluation cannot endorse an apparently context-free monetary summary.
2. Runtime-seal identity was made explicit and recomputable from the rooted runtime and rooted summary rather than relying only on the mutable `run.final.json` byte representation.

Static closure result: **one complete pass found no remaining material Patch Group 4 correction or worthwhile in-scope simplification.**

## Core / scope statement

Patch Group 4 changes no `src/Ensemble.E0.Core/**` file.

No provider request, credential access, network inference, product persistence, UI, NPU, package/Store work, later phase behavior, or global evidence service was introduced.

## Validation boundary

No compiler, test-runtime, native Windows ARM64, fixture-smoke, or provider validation claim is made for `2a55319820587b63da13377a775150b90c24b107`.

Grouped native Windows ARM64 validation remains deferred until all authorized hardening groups complete recursive static audit.

## Gate

- Patch Group 4 implementation: **COMPLETE**
- Patch Group 4 recursive static audit: **COMPLETE**
- Patch Group 4 native validation: **NOT YET PERFORMED**
- Patch Group 5: **NOT STARTED IN THIS RECORD**
- Provider execution: **NOT AUTHORIZED**
- Merge to `main`: **NOT AUTHORIZED / NOT READY**
