"""Checks that keep the games consistent. Run from the repository root:

    python tools/check.py

1. GMDCore lineage. GMDCore is one library that grows through the course: every game keeps
   the previous game's core and adds to it (or deliberately changes it). This lists what each
   game adds, changes and removes, in session order (the folder numbers), and fails if a game
   removes a file.
2. Build files. Every game's Content/Content.csproj and Content/BuildContent.targets must be
   identical, so a fix in one is made in all of them.
"""
import difflib
import hashlib
import pathlib
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
GAMES = sorted(p for p in ROOT.iterdir() if p.is_dir() and p.name[:2].isdigit() and p.name[2] == '-')
SHARED_BUILD_FILES = ['Content/Content.csproj', 'Content/BuildContent.targets']


def lines(path):
    return path.read_text(encoding='utf-8-sig').replace('\r\n', '\n').splitlines()


def core_files(game):
    core = game / 'GMDCore'
    return {p.relative_to(core).as_posix(): p for p in core.rglob('*.cs')
            if not {'bin', 'obj'} & set(p.relative_to(core).parts)}


def check_lineage():
    ok = True
    games = [g for g in GAMES if (g / 'GMDCore').is_dir()]
    for previous, current in zip(games, games[1:]):
        old, new = core_files(previous), core_files(current)
        added = sorted(new.keys() - old.keys())
        removed = sorted(old.keys() - new.keys())
        changed = []
        for name in sorted(old.keys() & new.keys()):
            diff = [l for l in difflib.unified_diff(lines(old[name]), lines(new[name]), lineterm='', n=0)
                    if l[:1] in '+-' and not l.startswith(('+++', '---'))]
            if diff:
                changed.append(f'{name} ({len(diff)} lines)')
        print(f'{previous.name} -> {current.name}')
        for label, items in (('added', added), ('changed', changed), ('REMOVED', removed)):
            if items:
                print(f'  {label}: ' + ', '.join(items))
        if not (added or changed or removed):
            print('  (no changes)')
        ok &= not removed
    if not ok:
        print('A game removed files from GMDCore. Keep them, so the core only grows.')
    return ok


def check_build_files():
    ok = True
    for name in SHARED_BUILD_FILES:
        digests = {}
        for game in GAMES:
            text = '\n'.join(lines(game / name))
            digests.setdefault(hashlib.sha256(text.encode()).hexdigest(), []).append(game.name)
        if len(digests) > 1:
            ok = False
            print(f'{name} differs between games:')
            for games in digests.values():
                print('  ' + ', '.join(games))
    if ok:
        print(f'Build files identical in all {len(GAMES)} games.')
    return ok


if __name__ == '__main__':
    lineage_ok = check_lineage()
    print()
    build_ok = check_build_files()
    sys.exit(0 if lineage_ok and build_ok else 1)
