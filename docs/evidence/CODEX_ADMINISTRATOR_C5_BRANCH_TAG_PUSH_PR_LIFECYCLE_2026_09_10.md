# Codex Administrator C5 - Branch / tag / push / PR lifecycle

Status: C5 mechanical gate PASS; this manager-adoption record becomes durable Project authority only when its closeout branch is merged. C6 remains blocked until then.

Date: 2026-09-10

## Scope and authority

C5 implements only the Director-approved Runtime Specification gate for shared Git lifecycle mechanics. It creates no Engineering phase, provider/spend, validation, Design/ODR, browser/CDP, Claude, Hooks, Automations, or C6+ authority. Engineering truth remains owned by `CURRENT_STATE.md`; during this work Run 03 remained separately authorized for its one-time provider execution and was never invoked by C5.

The C5 lifecycle proof deliberately used two stages. The disposable pilot branch carried exactly one archived commissioning-evidence fixture. Only after that branch completed merge, archive-tag verification, and disposal did this separate manager-adoption branch record C5 as DONE. This avoids making a branch prove its own post-disposal state.

## Shared-Git lock prerequisite

C5 found that the approved per-Git-common-directory serialization requirement had no executable runtime realization. A machine-local helper was commissioned at `C:\Users\Wiryl\.codex-ensemble\bin\ensemble-git-lock.ps1`. It resolves the Git common directory, derives a deterministic lock file, acquires it atomically with create-new semantics, records only common-dir / operation / process-session / UTC / dispatch fields, executes only Git beneath the lock, and removes only its own unchanged lock on normal completion. It does not wait on or auto-clear an existing lock.

Final helper SHA-256: `A8550C6234D0C5F8721AB2224DAE36043C7E649077BD73A2A16F765547C55830`.

Runtime manifest after C5 commissioning: `D19CD1333E0642960FED60D75BFDCF1BFCAAE1DD3AD2B7BC0C7FB6D6E2D743F9`. Pre-C5 rollback manifest: `C:\Users\Wiryl\.codex-ensemble\backups\C5-shared-git-lock-20260910T235807Z`.

A contention negative control returned exit `75`; the pre-existing synthetic lock's hash and bytes remained unchanged, Git did not run, and the helper did not remove that lock. The Administrator removed only its own synthetic test fixture afterward.

## Disposable evidence-only pilot

Pilot branch/worktree: `q-admin-02-c5-lifecycle-pilot-2026-09-10`. Baseline: `aa2c4a0a32f4f76b5f3d3d186020540b3b590a41`.

The branch changed exactly one file: `docs/evidence/archive/CODEX_ADMINISTRATOR_C5_DISPOSABLE_LIFECYCLE_PILOT_2026_09_10.md`. It changed no current authority, source, test, fixture, provider, validation, or Design surface. Pre-commit diff hygiene, repository law, and document census passed with zero unexplained current documents.

Pilot commit: `e259d6916b9c9b91b96eb2c8922e0c9ff9ced669`. It was clean, directly parented on the live baseline, and pushed only after fresh remote/local `main` race checks matched. Fetch, worktree creation, staging, commit, push, tag creation/push, worktree removal, local-branch deletion, and remote-branch deletion were serialized through the shared-Git lock.

## GitHub lifecycle evidence

Push-triggered Validation gate #610 (`34545613378`) completed successfully on exact pilot `e259d691...`; all five jobs passed. PR #62 contained exactly one commit and one changed file, with head pinned to the pilot commit and base `aa2c4a0...`. PR-triggered Validation gate #611 (`34545741072`) completed successfully with all five jobs passed. No E0-E workflow was triggered for this archive-only pilot; absence is recorded rather than represented as a skipped or passed gate.

A final remote `main`/PR-head race check showed `main` still equal to the PR base and head still exact. PR #62 was merged with expected-head protection; there was no direct push to `main`. Merge commit: `5cd64307471a4095d71e5e4467386b866004e375`. Candidate tree and merge tree matched exactly at `161a86b5eed81d2dbb7e062e372ac2c56e508149`, and the pilot commit was verified ancestral to the merge.

Post-merge Validation gate #612 (`34545849938`) completed successfully on exact merge commit with all five jobs passed.

## Archive and disposal

Annotated archive tag: `archive/q-admin-02-c5-lifecycle-pilot-2026-09-10`. Tag object: `a240016bec3456d7e7c3fa9237d1aed10c6543a9`. Local and remote tag peel were independently verified equal to exact pilot commit `e259d6916b9c9b91b96eb2c8922e0c9ff9ced669`.

After a locked fetch, `origin/main` was `5cd64307471a4095d71e5e4467386b866004e375`; the pilot commit remained ancestral; the disposable worktree was clean. Only then were the pilot worktree, local branch, and remote branch retired. Final verification: worktree absent, local branch absent, remote branch absent, archive tag intact, ancestry PASS, shared-Git lock residue zero.

One attempted local branch-delete invocation was rejected by PowerShell parameter binding because Git `-D` was interpreted as a helper parameter abbreviation. It executed no deletion. The retry passed Git arguments explicitly via `-GitArgs`, after which deletion completed under the lock. This is preserved as commissioning evidence rather than hidden.

## Recursive audit findings

The first lock-helper draft contained two joined PowerShell statements from chunked creation, producing stray output, and its contention path used `Write-Error` under `ErrorActionPreference=Stop`. Both were corrected before the helper was pinned or used for C5 mutations. The corrected contention path writes a bounded stderr message and exits 75 without disclosing existing lock contents or removing the lock.

A verifier initially reported `TAG_VERIFY=False` despite visibly equal local/remote peel values because PowerShell collapsed a one-element array to a scalar and `[0]` indexed its first character. The verifier was corrected; the independent rerun returned one remote peel and `TAG_VERIFY=True`. No tag mutation was repeated.

One full recursive pass after these corrections found no remaining C5 authority leak, direct-main mutation, unlocked shared-Git mutation, unexplained disposal, validation inflation, provider use, or worthwhile in-scope simplification.

## C5 disposition

C5 PASS criteria are satisfied: fresh origin race checks were used; shared Git operations were serialized with an atomic per-common-directory lock; push and PR mechanics were exercised on a disposable evidence-only branch; `main` changed only through the pinned PR merge; exact-head CI was observed before and after merge; archive-tag discipline was proven; and safe branch/worktree disposal was completed only after tag/ancestry/cleanliness checks.

This record does not itself authorize C6 execution until the manager-adoption closeout is merged into authoritative `main`. Once merged, C6 Cross-lane transport pilot is the sole next Administrator gate.

## Manager-adoption verification

The separate manager-closeout worktree was created from exact merged pilot `main@5cd64307471a4095d71e5e4467386b866004e375` after a locked fetch. The candidate changes only this C5 evidence record plus `CURRENT_STATE.md`, `docs/PROJECT_EXECUTION_QUEUE.md`, `docs/AGENT_TOOLING_CAPABILITY_SNAPSHOT.md`, and `docs/DOCUMENT_INDEX.md`. Engineering Run 03/provider/native-validation text remains unchanged outside the Administrator continuity sentence.

Pre-commit recursive checks on the staged five-file adoption candidate passed: `git diff --cached --check` exit 0; repository law PASS; document census 203 inventory / 86 current / 30 historical / 87 archive / 0 unexplained current; oracle coverage 96 documented hashes / 17 asserted / 79 document-only, exit 0. `CURRENT_STATE.md` is 2,683 bytes, below the 3 KiB cap.

A successor-state scan found no stale current `C5 next` claim. Historical C3/C4 evidence retains its then-correct successor wording because rewriting historical evidence would damage provenance. The navigation index's stale C3 successor phrase was removed as a current navigation correction.
