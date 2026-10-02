# Plants vs. Zombies

The code for session **09 Plants vs. Zombies** of the Game Architecture (GAR) course: a
tower defence game like Plants vs. Zombies, built from components. Ours is set at a castle,
where defenders hold five rows of a field against goblins. The [session
page](https://metamate.github.io/gar/sessions/09-plants-vs-zombies/) explains the ideas;
this README shows where to find them in the code.

## Steps

The game is built up in steps. Each step is a separate project that builds on the previous
one, so you can follow the game one concept at a time. Compare two neighbouring
steps (e.g. with a diff tool) to see what changed.

| Step | Topic | What's new |
| --- | --- | --- |
| `Pvz0` | Picking | Choose a card and click a cell to place a defender: from a mouse position to a card or a cell |
| `Pvz1` | Inheritance | Defenders, goblins, arrows and gold, as a class hierarchy (`Defender` → `Archer`, `Chest`, `Knight`) |
| `Pvz2` | The Component pattern | Everything is an `Entity` made of components (`Health`, `Shooter`, `Walker`, `Attacker`, …). Recipes combine them: a Wizard and a Shieldbearer need no new classes |
| `Pvz3` | Type Object | Defender and goblin types come from `defenders.json` and `goblins.json`; each type builds its own entities. The Bomb: one new component, and data |
| `Pvz4` | The whole game | A level from `level1.json`, gold from the sky, recharging cards, winning and losing (the finished game) |

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
| Defender and goblin types (Type Object), from JSON | `Types.cs`, `Content/Assets/data/*.json` |
| Arrows, coins and effects | `Recipes.cs` |
| The field's grid, and picking a cell | `Field.cs` |
| The cards | `CardBar.cs` |
| Waves of goblins | `Level.cs`, `Content/Assets/data/level1.json` |
| Everything in play | `World.cs` |
| The tests | `Pvz.Tests/` |

## Tests

`Pvz.Tests` tests the finished game's components one at a time: health, armour that takes
damage first, and a lifetime that runs out. Each test builds an entity with only the
components it needs, without a field, textures or a running game. Small components make
that possible.

```sh
cd 09-plants-vs-zombies
dotnet test
```

## Content

The assets all steps share:

```text
Content/Assets/
├── images/background.png           # The field
├── images/sprites.png              # Defenders, goblins, arrows, gold, cards
├── images/atlas-definition.xml     # The regions in sprites.png
├── data/defenders.json             # The defender types (Pvz3 on)
├── data/goblins.json               # The goblin types (Pvz3 on)
├── data/level1.json                # The level: starting gold and goblin spawns (Pvz4)
└── fonts/hud.spritefont
```

A defender type gets a component for each kind of data it has:

```json
{ "name": "Wizard", "sprite": "wizard", "cost": 200, "recharge": 7.5, "health": 6,
  "shooter": { "interval": 1.4, "shots": 2, "damage": 1 } }
```

`shooter`, `goldProducer` and `explode` are the defender components that can be set from data;
goblins can have `armour`.

## Controls

| Key | Action |
| --- | --- |
| Mouse | Click a card, then a cell to place a defender. Click gold to collect it |
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

The game is inspired by PopCap's Plants vs. Zombies. The art is from
[Tiny Town](https://kenney.nl/assets/tiny-town) and [Tiny Dungeon](https://kenney.nl/assets/tiny-dungeon)
by Kenney (CC0), at five times its size. Three things are made from the packs' pieces and
colours: the goblin (the packs' ogre, with green skin), the arrow (the packs' arrow lies
diagonally; ours lies flat), and the card behind each unit. The explosion is our own. The
sheet also holds a Guard, a Frost Wizard, an ice arrow and a pitchfork for the exercises,
which the atlas doesn't describe yet. The font is
[Press Start 2P](https://fonts.google.com/specimen/Press+Start+2P) by CodeMan38, under the
SIL Open Font License (see `Content/Assets/fonts/retro-OFL.txt`).
