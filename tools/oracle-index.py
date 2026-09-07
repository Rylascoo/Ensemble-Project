#!/usr/bin/env python3
"""Fail closed if a documented 64-hex oracle loses scanned MSTest assertion coverage."""

from __future__ import annotations

import argparse
import collections
import pathlib
import re
import subprocess
import sys

ROOT = pathlib.Path(__file__).resolve().parents[1]
HASH = re.compile(r'(?<![0-9a-fA-F])[0-9a-fA-F]{64}(?![0-9a-fA-F])')
TOKEN = re.compile(
    r'//[^\n]*|/\*.*?\*/|@"(?:""|[^"])*"|"(?:\\.|[^"\\])*"'
    r"|'(?:\\.|[^'\\])*'|[A-Za-z_][A-Za-z_0-9]*|[^\s]", re.S)


def git(*args: str) -> bytes:
    return subprocess.check_output(['git', *args], cwd=ROOT)


def hashes(text: str) -> set[str]:
    return {match.group().lower() for match in HASH.finditer(text)}


def asserted_hashes(text: str) -> set[str]:
    """Recognize literal/const-string operands of MSTest AreEqual calls.

    This is deliberately lexical inventory, not C# semantic analysis or proof
    that a test executes. Other assertion styles require a scanner extension.
    """
    tokens = [
        match.group() for match in TOKEN.finditer(text)
        if not match.group().startswith(('//', '/*'))
    ]
    constants: dict[str, list[str]] = {}
    for index in range(len(tokens) - 4):
        if tokens[index:index + 2] == ['const', 'string'] and tokens[index + 3] == '=':
            end = index + 4
            while end < len(tokens) and tokens[end] != ';':
                end += 1
            constants[tokens[index + 2]] = tokens[index + 4:end]

    def resolve(operands: list[str], seen: frozenset[str] = frozenset()) -> set[str]:
        found: set[str] = set()
        for token in operands:
            if token.startswith(('"', '@"')):
                found.update(hashes(token))
            elif token in constants and token not in seen:
                found.update(resolve(constants[token], seen | {token}))
        return found

    found: set[str] = set()
    for index in range(len(tokens) - 4):
        if (
            tokens[index] not in ('Assert', 'CollectionAssert')
            or tokens[index + 1:index + 4] != ['.', 'AreEqual', '(']
        ):
            continue
        depth = 0
        argument: list[str] = []
        arguments: list[list[str]] = []
        for token in tokens[index + 4:]:
            if token == ')' and depth == 0:
                arguments.append(argument)
                break
            if token == ',' and depth == 0:
                arguments.append(argument)
                argument = []
                continue
            if token in ('(', '[', '{'):
                depth += 1
            elif token in (')', ']', '}'):
                depth -= 1
            argument.append(token)
        for operand in arguments[:2]:
            found.update(resolve(operand))
    return found


def resolve_commit(ref: str) -> str:
    return git('rev-parse', '--verify', f'{ref}^{{commit}}').decode().strip()


def worktree_paths() -> list[str]:
    raw = git(
        'ls-files', '-z', '--cached', '--others', '--exclude-standard',
        '--', 'docs/', 'tests/'
    )
    return sorted({name for name in raw.decode().split('\0') if name})


def ref_paths(commit: str) -> list[str]:
    raw = git('ls-tree', '-r', '--name-only', '-z', commit, '--', 'docs/', 'tests/')
    return sorted({name for name in raw.decode().split('\0') if name})


def read_text(name: str, commit: str | None) -> str | None:
    try:
        parts = set(pathlib.PurePosixPath(name).parts)
        if {'bin', 'obj'} & parts:
            return None
        if commit is None:
            path = ROOT / name
            if not path.is_file():
                return None
            data = path.read_bytes()
        else:
            data = git('show', f'{commit}:{name}')
        text = data.decode('utf-8-sig')
    except (OSError, UnicodeDecodeError, subprocess.CalledProcessError):
        return None
    return None if '\0' in text else text


def scan(ref: str | None = None) -> dict[str, tuple[set[str], set[str]]]:
    commit = resolve_commit(ref) if ref is not None else None
    names = worktree_paths() if commit is None else ref_paths(commit)
    documents: collections.defaultdict[str, set[str]] = collections.defaultdict(set)
    assertions: collections.defaultdict[str, set[str]] = collections.defaultdict(set)
    for name in names:
        text = read_text(name, commit)
        if text is None:
            continue
        if name.startswith('docs/'):
            for value in hashes(text):
                documents[value].add(name)
        elif name.startswith('tests/') and name.endswith('.cs'):
            for value in asserted_hashes(text):
                assertions[value].add(name)
    values = documents.keys() | assertions.keys()
    return {value: (documents[value], assertions[value]) for value in values}


def assertion_regressions(
    before: dict[str, tuple[set[str], set[str]]],
    after: dict[str, tuple[set[str], set[str]]],
    label: str,
) -> list[str]:
    failures: list[str] = []
    for value, (before_documents, before_tests) in sorted(before.items()):
        if not before_documents or not before_tests:
            continue
        _, after_tests = after.get(value, (set(), set()))
        if after_tests:
            continue
        failures.append(
            f'{label}: documented ASSERTED hash {value} lost all scanned assertion coverage'
        )
    return failures


def parent_ref() -> str | None:
    try:
        return resolve_commit('HEAD^')
    except subprocess.CalledProcessError:
        return None


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument(
        '--check', action='store_true',
        help='reject assertion-coverage loss against the first parent of HEAD'
    )
    parser.add_argument(
        '--baseline',
        help='also reject assertion-coverage loss against this prior Git revision'
    )
    args = parser.parse_args()

    current = scan()
    failures: list[str] = []

    if args.check:
        parent = parent_ref()
        if parent is not None:
            failures.extend(assertion_regressions(scan(parent), current, f'parent {parent}'))

    if args.baseline:
        baseline = resolve_commit(args.baseline)
        if baseline != resolve_commit('HEAD'):
            failures.extend(assertion_regressions(scan(baseline), current, f'baseline {baseline}'))

    documented = {value for value, (documents, _) in current.items() if documents}
    asserted = {
        value for value, (documents, tests) in current.items()
        if documents and tests
    }
    print(f'ORACLE_GUARD_DOCUMENTED_HASHES={len(documented)}')
    print(f'ORACLE_GUARD_ASSERTED={len(asserted)}')
    print(f'ORACLE_GUARD_DOCUMENT_ONLY={len(documented - asserted)}')
    for failure in sorted(set(failures)):
        print(f'ERROR: {failure}', file=sys.stderr)
    return 1 if failures else 0


if __name__ == '__main__':
    try:
        raise SystemExit(main())
    except (OSError, ValueError, subprocess.CalledProcessError) as error:
        print(f'ERROR: {error}', file=sys.stderr)
        raise SystemExit(1)
