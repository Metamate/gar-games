# Sokoban

Source code for session **04 Sokoban** of the Game Architecture (GAR) course.

The game is built up in steps. Each step is a separate project that builds on the previous
one, so you can follow the code's evolution one concept at a time. Compare two neighbouring
steps (e.g. with a diff tool) to see exactly what changed.

| Step | Topic | What's new |
| --- | --- | --- |
| `Sokoban0` | Levels as data | `level1.txt` is read as text and drawn character by character |
| `Sokoban1` | Rules apart from drawing | `Level` holds the state and the rules (walk, push); `LevelView` draws it; a `GameController` maps keys to moves |
| `Sokoban2` | Testable rules | `Sokoban2.Tests` tests the rules with xUnit, without starting the game |
| `Sokoban3` | Command | Every move is a `MoveCommand` object; the game keeps a list of them to count the moves |
| `Sokoban4` | Undo and redo | Commands can `Undo`; a `CommandHistory` keeps an undo stack and a redo stack (`Z` / `Y`) |
| `Sokoban5` | The whole game | Seven levels, restart, a move counter and a level-complete message (the finished game) |

Each test project (`Sokoban2.Tests` … `Sokoban5.Tests`) tests the step of the same name.

All steps share the **GMDCore** library.

## New in GMDCore

Nothing: the core is the same as in [03-snake](../03-snake/). The new code in this game
(levels, rules and commands) belongs to Sokoban, not to every game.

## Content

All steps share the same assets and the same content builder:

```text
Content/Assets/
├── images/tiles.png         # The tilesheet: 64×64 tiles, 13 columns
├── levels/level1.txt …      # The levels, as plain text
└── fonts/arial.spritefont   # The HUD font (Sokoban5)
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

To add a level, add `level8.txt` and raise `LevelCount` in `Sokoban5/Game1.cs`.

## Controls

| Key | Action |
| --- | --- |
| Arrow keys, `W` `A` `S` `D` | Move and push (from `Sokoban1`) |
| `Z` or `Backspace` | Undo (from `Sokoban4`) |
| `Y` | Redo (from `Sokoban4`) |
| `R` | Restart the level (`Sokoban5`) |
| `Enter` | Next level, once solved (`Sokoban5`) |
| `Esc` | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 04-sokoban
dotnet run --project Sokoban5
dotnet test Sokoban5.Tests
```

Or open `Sokoban.slnx` and choose the step to run.

## Credits

The art is [Kenney's Sokoban pack](https://kenney.nl/assets/sokoban) (CC0).
