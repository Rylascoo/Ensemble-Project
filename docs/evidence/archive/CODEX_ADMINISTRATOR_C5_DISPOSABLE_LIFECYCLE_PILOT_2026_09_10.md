# Codex Administrator C5 Disposable Lifecycle Pilot

Status: ARCHIVED COMMISSIONING PILOT FIXTURE — NON-AUTHORITY
Date: 2026-09-10
Gate: C5 — Branch / tag / push / PR lifecycle

## Purpose

This file is the sole repository payload of the disposable C5 lifecycle pilot. It exists only to provide a harmless evidence-bearing commit whose branch lifecycle can be exercised end to end.

The pilot does not change Engineering, provider, validation, Design, ODR, experiment, queue, or product authority. It does not authorize C6 or later Administrator gates.

## Exact starting boundary

Repository: `Rylascoo/Ensemble-Project`
Baseline `main`: `aa2c4a0a32f4f76b5f3d3d186020540b3b590a41`
Pilot branch: `q-admin-02-c5-lifecycle-pilot-2026-09-10`

At baseline, C0–C4 are DONE and C5 is the sole earned Administrator gate. Q-E0A-03 Run 03 is separately ready for one full-reference execution under current Engineering/Director authority; this C5 pilot must not invoke or consume that provider execution.

## Shared-Git serialization prerequisite

Machine-local helper: `C:\Users\Wiryl\.codex-ensemble\bin\ensemble-git-lock.ps1`
Helper SHA-256: `A8550C6234D0C5F8721AB2224DAE36043C7E649077BD73A2A16F765547C55830`
Runtime-manifest SHA-256 after pinning: `135D858C501B7512FB1453F415E919F778E07C0CA3B6DC63376FF017F6FA6FE1`
The helper uses one atomic lock per Git common directory, fails immediately on contention, preserves a pre-existing lock unchanged, does not disclose its contents, and removes only its own byte-identical lock. Its contention negative control returned exit `75` with unchanged lock hash/content.

## Pilot proof required after this commit

The Administrator must independently prove, in order:

1. fresh remote/local `origin/main` race-check before push;
2. serialized push of the exact pilot commit;
3. PR creation from the disposable branch to `main` with no direct `main` push;
4. exact-head CI and PR mechanics without validation-rung inflation;
5. final remote `main`/PR-head race-check before merge;
6. merge through the PR only;
7. annotated `archive/...` tag created and verified to peel exactly to the pilot commit;
8. safe retirement of the disposable worktree/local branch/remote branch only after merge ancestry and archive-tag verification.

The complete observed lifecycle, including post-merge CI and disposal proof, must be written later by the owning manager after those events exist. This fixture must not claim C5 PASS in advance.
