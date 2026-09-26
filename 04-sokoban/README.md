# Sokoban

Source code for session **04 Sokoban** of the Game Architecture (GAR) course.

The game is built up in steps. Each step is a separate project that builds on the previous
one, so you can follow the code's evolution one concept at a time. Compare two neighbouring
steps (e.g. with a diff tool) to see exactly what changed.

| Step | Topic | What's new |
| --- | --- | --- |
| `Sokoban0` | Levels as data | `level1.txt` is read as text and drawn character by character |
| `Sokoban1` | Rules apart from drawing | `Level` holds the state and the rules (walk, push); `LevelView` draws it; a `GameController` maps keys to moves |
| `Sokoban2` | Command | Every move is a `MoveCommand` object; the game keeps a list of them to count the moves |
| `Sokoban3` | Undo and redo | Commands can `Undo`; a `CommandHistory` keeps an undo stack and a redo stack (`Z` / `Y`) |
| `Sokoban4` | The whole game | Seven levels, restart, a move counter and a level-complete message (the finished game) |

## Tests

`Sokoban.Tests` holds unit tests for the finished game's rules (`Level`) and for undo and
redo (`CommandHistory`). Each test sets up a small level from a string, does one thing, and
checks the result, without starting the game. The comments in `LevelTests.cs` explain how
a test is built.

```sh
cd 04-sokoban
dotnet test
```

Or run them from your editor's test explorer.

All steps share the **GMDCore** library.

## New in GMDCore

Nothing: the core is the same as in [03-snake](../03-snake/). The new code in this game
(levels, rules and commands) belongs to Sokoban, not to every game.

## Content

All steps share the same assets and the same content builder:

```text
Content/Assets/
├── images/tiles.png         # The tilesheet: 64×64 tiles, 3 columns
├── levels/level1.txt …      # The levels, as plain text
└── fonts/hud.spritefont     # The HUD font (Sokoban4), made from retro.ttf
```

`Builder.cs` builds the tilesheet and the font, and copies the level files as they are,
because the game reads them itself. A level uses the classic Sokoban characters:

| Character | Meaning |
| --- | --- |
| `#` | Wall |
| `$` | Box |
| `.` | Goal |
| `*` | Box on a goal |
| `@` | Player |
| `+` | Player on a goal |

To add a level, add `level8.txt` and raise `LevelCount` in `Sokoban4/Game1.cs`.

## Controls

| Key | Action |
| --- | --- |
| Arrow keys, `W` `A` `S` `D` | Move and push (from `Sokoban1`) |
| `Z` or `Backspace` | Undo (from `Sokoban3`) |
| `Y` | Redo (from `Sokoban3`) |
| `R` | Restart the level (`Sokoban4`) |
| `Enter` | Next level, once solved (`Sokoban4`) |
| `Esc` | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 04-sokoban
dotnet run --project Sokoban4
```

Or open `Sokoban.slnx` and choose the step to run.

## Credits

The art is our own, made for the course. The font is
[Press Start 2P](https://fonts.google.com/specimen/Press+Start+2P) by CodeMan38, under the
SIL Open Font License (see `Content/Assets/fonts/retro-OFL.txt`).
