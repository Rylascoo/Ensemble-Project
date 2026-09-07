# Oracle documentation drift guard

Run `python tools/oracle-index.py` from any directory to regenerate
`docs/evidence/ORACLE_INDEX.md`. Commit the regenerated index with intentional
documentation changes. Run `python tools/oracle-index.py --check` to compare the
working-tree scan with the index committed at HEAD.

The scanner reads UTF-8 documents under `docs/`, excluding its own output, and
C# files under `tests/`. Git-ignored files and build output are excluded; new,
unignored files are included. Hashes are bounded 64-hex sequences, normalized to
lowercase. Rows and paths are sorted for deterministic output.

ASSERTED means a hash occurs in either of the first two operands of an MSTest
`Assert.AreEqual` or `CollectionAssert.AreEqual` call, directly or through a
same-file `const string`. Comments and assertion-message arguments do not count.
This recognizes the existing oracle assertion pattern; it is a lexical inventory,
not C# semantic analysis, reachability analysis, or proof of test execution.
Computed values, cross-file constants, generic assertion calls, and other
assertion styles need an explicit scanner extension before they can be credited.

CI regenerates the index and rejects differences from HEAD. It also compares
against the pre-push revision or pull-request base, so committing a regenerated
index cannot conceal loss of previously ASSERTED coverage. Missing rows with
previous coverage also fail. A prior revision without an index is the bootstrap
case; the committed HEAD index is still required and checked. On a new branch,
the historical baseline is `origin/main`. Errors identify affected full hashes
and changed document/assertion paths; formatting-only drift names the index.

DOCUMENT-ONLY is an inventory status. Disposition remains a Director decision.
This guard supplies no behavioral or runtime validation authority.

In the integrated validation workflow, this remains a separate repository-integrity
job alongside the ARM64 cross-compile compiler gate and advisory x64 Core tests.
Those jobs retain their distinct validation meanings; this guard does not promote
or substitute for either one.
