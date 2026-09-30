# Plants vs. Zombies

The code for session **09 Plants vs. Zombies** of the Game Architecture (GAR) course: a
lawn-defence game built from components. The [session
page](https://metamate.github.io/gar/sessions/09-plants-vs-zombies/) explains the ideas;
this README shows where to find them in the code.

## Steps

The game is built up in steps. Each step is a separate project that builds on the previous
one, so you can follow the game one concept at a time. Compare two neighbouring
steps (e.g. with a diff tool) to see what changed.

| Step | Topic | What's new |
| --- | --- | --- |
| `Pvz0` | Picking | Choose a seed packet and click a cell to plant: from a mouse position to a packet or a cell |
| `Pvz1` | Inheritance | Plants, zombies, peas and sun, as a class hierarchy (`Plant` → `Peashooter`, `Sunflower`, `WallNut`) |
| `Pvz2` | The Component pattern | Everything is an `Entity` made of components (`Health`, `Shooter`, `Walker`, `Eater`, …). Recipes combine them: a Repeater and a Conehead need no new classes |
| `Pvz3` | Type Object | Plant and zombie types come from `plants.json` and `zombies.json`; each type builds its own entities. The Cherry Bomb: one new component, and data |
| `Pvz4` | The whole game | A level from `level1.json`, sun from the sky, recharging packets, winning and losing (the finished game) |

## New in GARCore

Nothing. The core is the same as in [07-the-legend-of-zelda](../07-the-legend-of-zelda/). The entities and components
here are this game's own. In [11-geometry-wars](../11-geometry-wars/), a component model
moves into GARCore.

## Code Map

The finished game, `Pvz4`:

| To see | Look at |
| --- | --- |
| An entity: a bag of components | `Entity.cs`, `Components/Component.cs` |
| One behaviour each | `Components/` |
| Plant and zombie types (Type Object), from JSON | `Types.cs`, `Content/Assets/data/*.json` |
| Peas, suns and effects | `Recipes.cs` |
| The lawn's grid, and picking a cell | `Lawn.cs` |
| The seed packets | `SeedBar.cs` |
| Waves of zombies | `Level.cs`, `Content/Assets/data/level1.json` |
| Everything in play | `World.cs` |
| The tests | `Pvz.Tests/` |

## Tests

`Pvz.Tests` tests the finished game's components one at a time, namely health, armour that takes
damage first, and a lifetime that runs out. Each test builds an entity with only the
components it needs, without a lawn, textures or a running game. Small components make
that possible.

```sh
cd 09-plants-vs-zombies
dotnet test
```

## Content

The assets all steps share:

```text
Content/Assets/
├── images/background.png           # The lawn
├── images/sprites.png              # Plants, zombies, peas, sun, packets
├── images/atlas-definition.xml     # The regions in sprites.png
├── data/plants.json                # The plant types (Pvz3 on)
├── data/zombies.json               # The zombie types (Pvz3 on)
├── data/level1.json                # The level: starting sun and zombie spawns (Pvz4)
└── fonts/hud.spritefont
```

A plant type gets a component for each kind of data it has:

```json
{ "name": "Repeater", "sprite": "repeater", "cost": 200, "recharge": 7.5, "health": 6,
  "shooter": { "interval": 1.4, "shots": 2, "damage": 1 } }
```

`shooter`, `sunProducer` and `explode` are the plant components that can be set from data;
zombies can have `armour`.

## Controls

| Key | Action |
| --- | --- |
| Mouse | Click a seed packet, then a cell to plant. Click sun to collect it |
| `R` | New game, at any time |
| `Enter` | Start, and a new game at the end |
| `Esc` | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 09-plants-vs-zombies
dotnet run --project Pvz4
```

Or open `PlantsVsZombies.slnx` and choose the step to run.

## Credits

The art is our own. The game is inspired by PopCap's Plants vs. Zombies. The font is
[Press Start 2P](https://fonts.google.com/specimen/Press+Start+2P) by CodeMan38, under the
SIL Open Font License (see `Content/Assets/fonts/retro-OFL.txt`).
