# Pac-Man

Source code for session **05 Pac-Man** of the Game Architecture (GAR) course: Pac-Man with
the arcade ghosts. The concepts (the State pattern (ghost modes), the Strategy pattern
(targeting), testing each ghost) are explained on the [session
page](https://metamate.github.io/gar/sessions/05-pac-man/). This README is the map of the
code.

## Steps

The game is built up in steps. Each step is a separate project that builds on the previous
one, so you can follow the code's evolution one concept at a time. Compare two neighbouring
steps (e.g. with a diff tool) to see exactly what changed.

| Step | Topic | What's new |
| --- | --- | --- |
| `Pacman0` | The maze | `maze.txt` is read into a `Maze`; Pac-Man moves from tile to tile, turns where the player asked (buffered), and eats dots |
| `Pacman1` | Ghosts, with an enum | Four ghosts with five modes (in the house, scatter, chase, frightened, eaten) as a `GhostMode` enum, and a `switch` in every method |
| `Pacman2` | The State pattern | Each mode becomes a class in `GhostStates`; `Ghost` forwards to its current state |
| `Pacman3` | The Strategy pattern | Each ghost gets its own `ITargetStrategy` for chasing (`Targeting`) |
| `Pacman4` | The whole game | Lives, Pac-Man's death, levels, and game states: ready, play, dying, game over (the finished game) |

## New in GMDCore

Nothing: the core is the same as in [04-sokoban](../04-sokoban/). The maze, the ghosts and
their states belong to Pac-Man, not to every game.

## Code Map

The finished game, `Pacman4`:

| To see | Look at |
| --- | --- |
| The maze, dots and pellets | `Maze.cs`, `Content/Assets/levels/maze.txt` |
| Everything in play, and the rules between them | `World.cs` |
| Scatter and chase, over time | `ModeSchedule.cs` |
| A ghost, and its modes as states | `Ghost.cs`, `GhostStates/` |
| Where each ghost aims, as strategies | `Targeting/` |
| Ready, playing, dying, game over | `GameStates/` |
| Drawing, apart from the rules | `Views/` |
| The tests | `Pacman.Tests/` |

## Tests

`Pacman.Tests` tests the finished game without starting it: each ghost's targeting strategy
on its own (`TargetingTests`), the ghost states (`GhostStateTests`), and Pac-Man's movement
(`PacManTests`). The tests use a small maze of their own (`TestMaze`).

```sh
cd 05-pacman
dotnet test
```

## Content

All steps share the same assets and the same content builder:

```text
Content/Assets/
├── images/sprites.png              # Pac-Man, the ghosts, the dots
├── images/atlas-definition.xml     # Regions and animations, as in Snake
├── levels/maze.txt                 # The maze, as plain text
└── fonts/hud.spritefont            # The score
```

The walls aren't images: `MazeView` draws a line along every side of a wall tile that faces
an open tile. A maze uses these characters:

| Character | Meaning |
| --- | --- |
| `#` | Wall |
| `.` | Dot |
| `o` | Power pellet |
| `-` | Ghost house door |
| `H` | Inside the ghost house |
| `P` | Pac-Man |
| `b` | Blinky (outside the house) |
| `p` `i` `c` | Pinky, Inky and Clyde (inside the house) |

A row that is open at both ends is a tunnel.

## Controls

| Key | Action |
| --- | --- |
| Arrow keys, `W` `A` `S` `D` | Steer |
| `Enter` or `Space` | New game, after game over (`Pacman4`) |
| `Esc` | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 05-pacman
dotnet run --project Pacman4
```

Or open `Pacman.slnx` and choose the step to run.

## Credits

The ghosts' behaviour follows the original arcade game, as described in
[The Pac-Man Dossier](https://pacman.holenet.info/). The maze and the art are our own. The font is
[Press Start 2P](https://fonts.google.com/specimen/Press+Start+2P) by CodeMan38, under the
SIL Open Font License (see `Content/Assets/fonts/retro-OFL.txt`).
