# Codex Administrator C3 Deterministic Skills

Date: 2026-09-10

Status: **ACTIVE ADMINISTRATIVE COMMISSIONING EVIDENCE — C3 PASSED; C4 EARNED NEXT AFTER DURABLE CLOSEOUT**

## Authority and scope

This record is subordinate to `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, and the Director-approved `docs/blueprint/CODEX_ADMINISTRATOR_RUNTIME_SPECIFICATION.md`.

C2 is durably closed on `main@91b419575069b6ad5b2aef60380a1797aef9f856`. C3 was the only earned Administrator gate when this work began. C3 implements and proves only the eight initial repository-native deterministic Skills required by Runtime Specification section 21. It creates no Engineering phase, product, provider, validation, Design, queue, merge, deployment, Hook, Automation, browser/CDP, Claude, or later-gate authority.

The C3 candidate was reconstructed byte-for-byte first onto `main@8bc3551f6d6e7c43b1ae142a4e216e54acabd6cd` after four unrelated accepted Engineering commits advanced `main`, then again onto current authoritative baseline `main@9d0a05e554a97fceb0fcf50e026e0fef112b35f4` after the Run 02/provider-authority closeout advanced `main`; all ten C3 implementation files retained their exact SHA-256 hashes and no stale authority file was replayed.

## Versioned C3 surface

Repository-scoped Skills:

- `.agents/skills/authority-recovery/SKILL.md` — SHA-256 `668147F1133B2F072B8B24760E9BA4A3A3B380A5890A3AE1D17FC1F365226009`
- `.agents/skills/repo-reconciliation/SKILL.md` — SHA-256 `F0242956E2E1BDE1A96C2253858A86AF09458CE86C578E7325A7E98F2427E693`
- `.agents/skills/branch-worktree-census/SKILL.md` — SHA-256 `F0018A908496A41B142BE36A90334F24C2E360FC7BC0EC86C9EC59EFF6C99E83`
- `.agents/skills/state-distance/SKILL.md` — SHA-256 `A27E18651F414B676E0944CBBC1EF03D55F026C969397C425EBE0BBE236CDA85`
- `.agents/skills/protected-diff-check/SKILL.md` — SHA-256 `35A423FAE90AFC3E0E197EF420ACED69113ACD13F03D6CCF0971B865FCB6A501`
- `.agents/skills/ci-status/SKILL.md` — SHA-256 `132B153B39D615FB1BCA0B0202B4F83ECD3BE5EB8B6C489CE8D81E52224EAF7E`
- `.agents/skills/evidence-integrity/SKILL.md` — SHA-256 `D1B48DC1F65A42C982781FB20EDEFB7081F374E7F590450CEDF122081B0DF0ED`
- `.agents/skills/commissioning-closeout/SKILL.md` — SHA-256 `4F9F16EC812EAB17E57129DCDB85AF31032F6B59BA20E47088D949A841F32142`

Deterministic implementation/test tools:

- `tools/codex-admin-measure.py` — SHA-256 `4BA74B7798F8FC1893DCA5CCB3CA093600B08F214CCAF39EF0DA512FFD7E3A0E`
- `tools/codex-admin-skills-selftest.py` — SHA-256 `BD4E15B0119D904CC19B3EBED88BAC0B23C159E1706EFBE8FCA9714E9CCE6E32`

The Skills invoke the pinned private Python through `$env:ENSEMBLE_PYTHON -I -B`; they do not rely on ambient Python. Their deterministic cores perform measurements only. Existing C1 repository reconciliation is wrapped rather than duplicated where it already owns the invariant.

## Deterministic fixture result

The complete C3 suite was rerun after the final CI-contract correction under the dedicated ARM64 Python and returned exit `0` with every required marker:

```text
C3_SKILL_CONTRACTS=PASS
C3_REPO_RECONCILIATION_FIXTURE=PASS
C3_BRANCH_WORKTREE_CENSUS_FIXTURE=PASS
C3_STATE_DISTANCE_FIXTURE=PASS
C3_PROTECTED_DIFF_FIXTURE=PASS
C3_CI_STATUS_FIXTURE=PASS
C3_EVIDENCE_INTEGRITY_FIXTURE=PASS
C3_AUTHORITY_RECOVERY_FIXTURE=PASS
C3_COMMISSIONING_CLOSEOUT_FIXTURE=PASS
C3_MEASUREMENT_READ_ONLY_GUARD=PASS
C3_INITIAL_SKILLS_FIXTURES=PASS
```

The suite recursively exercises repository law, document-authority census, oracle coverage, dirty-worktree/protected-surface negative controls, exact-ref state distance, evidence hashes, authority recovery, and the composite commissioning closeout. Synthetic fixture mutations occur only inside disposable fixture repositories; the production measurement paths remain read-only.

A final reconstructed-tree rerun exposed a latent boundary error in the **test fixture**, not in `state-distance`: the negative control created exactly three commits after `CURRENT_STATE.md` while repository law permits a distance of three. The fixture wrongly expected that legal boundary to fail. C3 corrected the fixture to create four post-state commits, recalculated the self-test hash above, restarted the full suite, and obtained all eleven PASS markers with exit `0`. No production measurement semantics changed.

## Machine-local C3 runtime realization

C3 required deterministic repository Skills to execute inside the commissioned read-only Administrator sandbox. The machine-local runtime therefore admits only a pinned private ARM64 Python interpreter and exact-worktree selection mechanics; these are executable realization, not project authority. Current manifest-backed provenance is:

- `config.toml` SHA-256 `94EFA449D531B45AC4A719DF86CB8A070D9AD8672A7AF20A91E5917260D450CF`;
- `bin/ensemble-admin.ps1` SHA-256 `5B46A520EEB822A8E9CC9D0E435FD3BC61D594AAD5B0BB49DB691F2C8173A1FC`;
- private Python `3.12.10` executable SHA-256 `7F1EBBBB04164CAF8F8073AAFA7F82321FB44695E10FE6A9EBAF2138B63892C4`;
- `state/runtime-manifest.json` SHA-256 `79209C28E42BE503C9EF18360348337D83143865E2E87B20C1A2B198EB64BD7A`, census `2026-09-10T20:12:59.2807988Z`.

The `ensemble-admin-readonly` permission profile extends `:read-only` only to grant read access to `C:/Users/Wiryl/.codex-ensemble/toolchains/python312-arm64`. The launcher verifies the Python path/hash/version, exposes it only as `ENSEMBLE_PYTHON`, and validates any explicit `WorktreePath` by exact worktree root, Git common directory, and expected origin. Administrator approval remains `on-request`; login shells, command-network broadening, browser/CDP/computer-use, Hooks, memories, multi-agent execution, goals, destructive/open-world app tools, and non-`fetch` GitHub tools remain disabled.

Non-secret rollback snapshots exist at `C3-skill-worktree-launcher-20260910T193347Z`, `C3-python-runtime-20260910T194813Z`, `C3-python-permission-profile-20260910T200111Z`, and `C3-private-python-toolchain-20260910T200943Z` under `.codex-ensemble/backups/`.

A manual `evidence-integrity` check first used the obsolete C2 config hash and failed closed. Re-running from the **current runtime manifest** matched the config, launcher, and Python hashes above exactly. This is expected C3 provenance evolution, not unexplained runtime drift.

## Current-main manual cross-checks

After current Engineering advanced `main` to `9d0a05e554a97fceb0fcf50e026e0fef112b35f4`, C3 fetched/pruned tracking truth and reran its real measurements without changing project authority:

- authority recovery: `pass=true`, exact remote main `9d0a05e554a97fceb0fcf50e026e0fef112b35f4`, zero unexpected/missing known worktrees;
- repository/worktree census: 10 known worktrees, zero unexpected/missing; current C3 checkout correctly classified `DIRTY_OR_UNTRACKED` before commit; remote refs = 48 total: 1 `MAIN`, 25 `ANCESTRAL_TO_MAIN`, 0 `AHEAD_OF_MAIN`, 22 `DIVERGED_SIDE_HISTORY`;
- state distance at exact current main: `0` of maximum `3`, `CURRENT_STATE.md` 2,585 UTF-8 bytes;
- evidence integrity: current manifest-backed config, launcher, and private Python all matched exactly;
- exact-SHA CI: GitHub `total_count=2`; both runs supplied; utility selected Validation gate run `#599`, ID `34532437205`, `completed/success`, with no missing workflow and `validation_rung_promoted=false`.

## Staged candidate static closeout

After the final reconstruction, runtime-provenance correction, and state-distance fixture repair, the exact 15-file staged C3 candidate passed:

- `git diff --cached --check` — PASS;
- `tools/repository-law-check.py` — PASS, including current-state cap/currency, dependency direction, deterministic-warning law, repository residency, live-handoff authority, and remote-action SHA pins;
- `tools/document-census.py --summary --check` — PASS with inventory 194, current 78, historical 30, archive 86, unexplained current 0;
- `tools/oracle-index.py --check --baseline origin/main` — PASS with 84 documented hashes, 17 asserted, 67 document-only.

These are repository/static closeout measurements only. They do not promote the current native validation rung or create provider, product, Design, queue, merge, or deployment authority.

## CI completeness defect discovered and corrected

The first fresh usability probe exposed a real C3 completeness defect even though its narrow command returned success. GitHub returned multiple `Validation gate` runs for the same exact commit, but the Skill allowed the model to preselect one run before deterministic normalization. The model supplied run `#595` while newer exact-SHA run `#596` already existed. The deterministic utility therefore lacked enough information to detect the omission.

That first probe is **not** accepted as C3 usability evidence.

The correction restarted the relevant C3 audit and changed `ci-status` so that:

1. the exact-SHA GitHub query's complete returned run set is supplied to the deterministic utility;
2. GitHub `total_count` is supplied as `--expected-run-count`;
3. incomplete pagination or omitted records fail closed;
4. duplicate run IDs fail closed;
5. every supplied run must match the exact expected SHA; and
6. only the utility selects the highest run-number/ID per workflow.

A manual real-data negative control supplied one record while declaring `expected_run_count=2`; it returned `complete_input=false`, `pass=false`, and exit `1`. Supplying the complete two-run set selected `#596` and passed. Targeted Skill-contract and CI fixtures then passed before the complete suite was rerun.

## Accepted fresh-session usability probe

A genuinely fresh read-only Administrator session, thread `01a08d2d-b244-7893-8a6b-61e060aad1e6`, received only the C3 usability task. It discovered and followed the repository Skills, resolved live GitHub `main@8bc3551f6d6e7c43b1ae142a4e216e54acabd6cd`, treated its own four-commit-behind commissioning checkout as stale/dirty evidence, and read current authority from the exact live ref rather than from stale checked-out prose.

The probe measured:

- `CURRENT_STATE.md` state commit = exact live `main`;
- distance `0`, maximum `3`;
- UTF-8 size `2,492` bytes;
- state-distance `pass=true`.

During the accepted probe the exact-SHA GitHub query had grown to **three** `Validation gate` runs: `#595`, `#596`, and `#597`, all at `8bc3551f6d6e7c43b1ae142a4e216e54acabd6cd`. The probe supplied all three records with `expected_run_count=3`. The deterministic utility returned `complete_input=true`, no completeness errors, `pass=true`, and selected latest run `#597`, ID `34529487283`, `completed/success`. It explicitly returned `validation_rung_promoted=false`.

A final GitHub race-check still resolved `main@8bc3551f6d6e7c43b1ae142a4e216e54acabd6cd`. The probe exited `0` and modified no file, Git state, provider state, queue authority, or validation authority.

Restricted-runtime diagnostics observed during the fresh probe — ambient `rg` absent from PATH and PowerShell ConstrainedLanguage rejecting a diagnostic `[pscustomobject]` construction — did not affect the prescribed Skill executions. The probe recovered using admitted read-only/core surfaces. A separate uncommissioned Cloudflare MCP emitted an OAuth-required transport warning; it was not used or enabled and did not affect the admitted GitHub `fetch` path.

## .NET 9 regression

On the C3 code/authority candidate immediately before the evidence-only closeout amendment, a disposable external `global.json` selected repository CI SDK `9.0.317` without modifying project source. The four CI compiler targets built successfully with zero warnings and zero errors, and Core tests passed **622/622** on the Director ARM64 host. These are regression/compiler results only and do not promote native validation authority.

## Authority preservation

C3 does not alter the promoted native Windows ARM64 validation checkpoint `7868e5cb12a27260e288d95c248d6f846cf37701`. CI success is not native validation. C3 sent no provider request, `countTokens`, generation, inference, or spend. Parallel Engineering consumed Run 02 and established the bounded standing Gemini Free-tier synthetic diagnostic authority now recorded by current `main`; C3 preserves that provider authority without expanding, consuming, or interpreting it.

Parallel Q-E0A-03 worktrees/refs and all detached validation/evidence worktrees were observed and preserved; C3 made no disposition decision about them.

## C3 judgment

C3 **PASSES** the approved gate:

- all eight initial deterministic Skills exist as repository-native wrappers;
- deterministic fixtures and negative controls pass;
- the complete read-only guard passes;
- the discovered CI-selection ambiguity was corrected and the audit restarted;
- the accepted fresh session independently used the repaired full-result contract and selected the actual latest exact-SHA run;
- no passing result invented authority or inflated validation;
- current Engineering/provider/Design authority remained unchanged.

**Only C4 — Isolated Worker mutation — is earned next after this closeout is durably promoted. C4 and later gates have not started.**
