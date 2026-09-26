# Vampire Survivors

Source code for session **12 Vampire Survivors** of the Game Architecture (GAR) course: a
survivor game with thousands of enemies. The concepts (profiling, spatial partitioning,
data-oriented design) are explained on the [session
page](https://metamate.github.io/gar/sessions/12-vampire-survivors/). This README is the map
of the code.

## Steps

The game is built up in steps. Each step is a separate project that builds on the previous
one, so you can follow the code's evolution one concept at a time. Compare two neighbouring
steps (e.g. with a diff tool) to see exactly what changed.

| Step | Topic | What's new |
| --- | --- | --- |
| `Survivors0` | Enemies as objects | Swarms of enemies (`List<Enemy>`) that push each other apart, checking every pair. Space adds a thousand more |
| `Survivors1` | Profiling | A `Profiler` times each part of a step and counts allocations (F3). It shows where the time goes |
| `Survivors2` | Spatial partitioning | A `SpatialGrid` (a dictionary of cells) finds nearby enemies without checking every pair |
| `Survivors3` | Data-oriented design | Enemies as a struct of arrays (`Enemies`), and a `FlatGrid` rebuilt each step with a counting sort |
| `Survivors4` | The whole game | Gems and levels, upgrades on the state stack, health, and five minutes to survive (the finished game) |

## New in GMDCore

Nothing: the core is the same as in [11-geometry-wars](../11-geometry-wars/). Its `Core`
runs the game logic in fixed steps (`UpdateGame`), and pauses while the window isn't active.

## Code Map

The finished game, `Survivors4`:

| To see | Look at |
| --- | --- |
| Timing each part of a step | `Profiler.cs` |
| Enemies as a struct of arrays | `Enemies.cs` |
| The flat grid and its counting sort | `FlatGrid.cs` |
| Moving and separating the swarm | `Swarm.cs` |
| One run: player, weapons, gems, spawning | `Run.cs`, `Spawner.cs` |
| Weapons | `BoltWeapon.cs`, `Aura.cs` |
| Level-up choices | `Upgrade.cs`, `States/LevelUpState.cs` |
| The tests | `Survivors.Tests/` |

## Tests

`Survivors.Tests` checks the optimized code against the slow, obvious version: the flat grid
must find exactly the points that checking every point finds, on thousands of random points.
It also tests removing enemies from the arrays.

```sh
cd 12-vampire-survivors
dotnet test
```

## Measuring

Run `Survivors1`, `Survivors2` or `Survivors3` in Release, press F3 for the profiler, and
Space to add a thousand enemies at a time. The session page has one laptop's numbers to
compare with.

```sh
cd 12-vampire-survivors
dotnet run -c Release --project Survivors3
```

## Content

```text
Content/Assets/
├── images/ground.png               # A ground texture that repeats
├── images/sprites.png              # The hero, enemies, bolts, gems and the aura
├── images/atlas-definition.xml     # The regions in sprites.png
└── fonts/hud.spritefont, debug.spritefont
```

## Controls

| Key | Action |
| --- | --- |
| Arrow keys, `W` `A` `S` `D` | Move (you attack by yourself) |
| `Space` | A thousand more enemies (`Survivors0`–`Survivors3`) |
| `F3` | Profiler (`Survivors1` on) |
| `1` `2` `3` | Choose an upgrade (`Survivors4`) |
| `R` | Restart |
| `Enter` | Play again, after the end (`Survivors4`) |
| `Esc` | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 12-vampire-survivors
dotnet run --project Survivors4
```

Run it in Release (`-c Release`) when you measure. Or open `VampireSurvivors.slnx` and
choose the step to run.

## Credits

The art is our own. The game is inspired by poncle's Vampire Survivors. The font is
[Press Start 2P](https://fonts.google.com/specimen/Press+Start+2P) by CodeMan38, under the
SIL Open Font License (see `Content/Assets/fonts/retro-OFL.txt`).
