# Codex Administrator C1 Repository Reconciliation

Date: 2026-09-10

Status: **ACTIVE ADMINISTRATIVE COMMISSIONING EVIDENCE - C1 PASSED; NO ENGINEERING PHASE / PROVIDER / VALIDATION AUTHORITY**

## Scope and baseline

C1 is governed by `docs/blueprint/CODEX_ADMINISTRATOR_RUNTIME_SPECIFICATION.md` and follows accepted C0 closeout on `main@5db11f3b18d808d47c0ffa7eca411d04836662a1`.

C1 materialized a versioned deterministic core only:

- `tools/codex-admin-reconcile.py` SHA-256 `270F7ED519F7F514094AAFB992AD3B67B1C6B6352DE585AA9F3933E0DE0D580B`;
- `tools/codex-admin-reconcile-selftest.py` SHA-256 `4D8E360360FD7AB66154FEE3F8799ABB233B2F6706227CBB691F9EF60809A7BC`.

This is not yet a C3 Skill and is not wired into automation. It requires an approved external `git fetch --prune` before tracking/topology truth is relied upon. The reconciler itself performs no fetch, checkout, switch, reset, clean, pull, push, commit, branch, tag, worktree mutation, or remote mutation.

## Frozen state classifications

Synthetic disposable repositories independently exercised every C1-required state:

`CURRENT_CLEAN`, `BEHIND_FAST_FORWARD_CANDIDATE`, `AHEAD`, `DIVERGED`, `DETACHED`, `DIRTY_OR_UNTRACKED`, `UNEXPLAINED_WORKTREE`, and `REMOTE_IDENTITY_MISMATCH`.

All eight matched their manually constructed fixture truth. A ninth fixture reproduced the real Website condition of a configured upstream removed from the remote; it correctly failed safe as `UNEXPLAINED_WORKTREE` rather than being relabeled current or fast-forwardable.

## Read-only enforcement

Every production Git invocation passes through one guard. Accepted command forms are limited to read-only `remote get-url`, `rev-parse`, `status`, read-form `symbolic-ref`, `worktree list`, `rev-list`, `for-each-ref`, and `merge-base`.

The self-test explicitly proved rejection of `worktree add`, `remote set-url`, write-form `symbolic-ref`, `checkout`, and `reset`. Each synthetic case snapshots HEAD, branch, status, refs and worktrees immediately before and after classification; every snapshot was identical. Result: `C1_CLASSIFIER_MUTATION_CHECK=PASS` and `C1_MUTATING_GIT_GUARD=PASS`.

Fixture setup itself uses ordinary mutating Git only inside temporary synthetic repositories to construct test states. Those setup mutations are not part of the production reconciler and never target Project or Website.

Synthetic remote-ref topology also passed independent relations for `MAIN`, `ANCESTRAL_TO_MAIN`, `AHEAD_OF_MAIN`, and `DIVERGED_SIDE_HISTORY`.

## Real Project result

Manual Git truth and the reconciler agree:

- canonical origin: `https://github.com/Rylascoo/Ensemble-Project`;
- historical root: clean detached `689655eed677b789ab3ee395f1c65b4f2cb72cc8` -> `DETACHED`;
- five pre-existing detached validation/evidence worktrees remain preserved;
- the temporary C1 implementation worktree is an explained commissioning surface only;
- fetched remote topology: **59 heads total = main 1 + ancestral 36 + ahead 0 + divergent 22**.

Therefore all **58 pre-existing non-main Project refs** remain classified/preserved. C1 deletes, merges, retags, force-moves, updates or repurposes none of them.

## Real Website result

Manual Git truth and the reconciler agree:

- canonical origin: `https://github.com/Rylascoo/Ensemble-Website`;
- root branch `repo/pre-handoff-cleanup-2026-09-09` at `e0af363a3cf1401b424f2bee76a9d1c503e09967` is clean but its configured upstream is gone;
- that root commit is eight commits behind current Website `main@b0464e6a63bee3075a9da5acc74864a57ff78e5f` and is **not** treated as a fast-forward candidate because its intended tracked remote branch no longer exists;
- root classification: `UNEXPLAINED_WORKTREE`;
- linked `renovation/w1-txt-corpus-coverage` worktree at `cddbd5e2f955dc894b4e781c43c4f83831d7da93` is clean and exactly equals its live upstream -> `CURRENT_CLEAN`;
- fetched Website topology: **85 heads total = main 1 + ancestral 1 + ahead 0 + divergent 83**.

C1 makes no Design-lane disposition from those facts. The root and linked worktree remain untouched; divergent Design refs remain preserved for owning-lane reconciliation.

## Regression-gate note

The Director host defaulted to .NET SDK `10.0.400`; an initial `dotnet test --no-build` therefore hit Microsoft.Testing.Platform's SDK-10 VSTest compatibility guard before tests ran. This was an environment-selection artifact, not a test failure. A disposable external `global.json` pinned the repository CI SDK `9.0.317` without modifying the repository. Under `9.0.317`, Core passed **622/622**, E0-E deterministic tests passed **10/10**, and required builds completed with zero warnings/errors. These are regression checks only and do not promote native validation authority.

## C1 judgment

C1 **PASSES** the approved gate:

- all eight frozen repository-state classes match synthetic manual truth;
- real Project and Website classifications match independent manual Git measurements;
- remote identity checks fail closed;
- approval-baseline Project ref residue is deterministically classified and preserved;
- detached validation/evidence worktrees are preserved;
- the Website missing-upstream condition is surfaced rather than auto-repaired;
- production Git commands are read-only by guard and observed execution;
- zero automatic overwrite, checkout, update, branch disposal, authority mutation, provider traffic, or validation promotion occurred.

**Only C2 - Fresh Administrator authority recovery - is earned next.** C2 must start a genuinely fresh Administrator session and recover repository/queue/validation/provider/worktree/CI truth without chat-history help or rung inflation.
