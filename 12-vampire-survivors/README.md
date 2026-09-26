# Vampire Survivors

Source code for session **12 Vampire Survivors** of the Game Architecture (GAR) course.

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

All steps share the **GMDCore** library.

## Tests

`Survivors.Tests` checks the optimized code against the slow, obvious version: the flat grid
must find exactly the points that checking every point finds, on thousands of random points.
It also tests removing enemies from the arrays.

```sh
cd 12-vampire-survivors
dotnet test
```

## Measuring

Run `Survivors1`, `Survivors2` or `Survivors3`, press F3 for the profiler, and Space a few
times to add enemies. On one laptop, moving and separating the enemies took (ms per step):

| Enemies | Objects, every pair (`Survivors1`) | Objects + grid (`Survivors2`) | Arrays + flat grid (`Survivors3`) |
| --- | --- | --- | --- |
| 1,000 | 1.2 | 1.3 | 0.24 |
| 5,000 | 27 | 1.9 | 0.64 |
| 10,000 | 107 | 4.1 | 1.3 |
| 20,000 | — | 14.5 | 4.9 |

A step has 16.7 ms at 60 steps per second. Your numbers will differ; the shape shouldn't.

## New in GMDCore

Nothing: the core is the same as in [11-geometry-wars](../11-geometry-wars/). Its `Core`
runs the game logic in fixed steps (`UpdateGame`), and pauses while the window isn't active.

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
