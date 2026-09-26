# The Legend of Zelda

Source code for session **08 The Legend of Zelda** of the Game Architecture (GAR) course: a
top-down dungeon crawler. The concepts (composition vs. inheritance, events, hitboxes,
tweening, stenciling, data-driven design) are explained on the
[session page](https://metamate.github.io/gar/sessions/08-the-legend-of-zelda/). This README
is the map of the code.

## Steps

The game is built up in steps. Each step is a separate project that builds on the previous
one, so you can follow the code's evolution one concept at a time. Compare two neighbouring
steps (e.g. with a diff tool) to see exactly what changed.

| Step | Topic | What's new |
| --- | --- | --- |
| `Zelda0` | Rooms | A room generated as a tilemap: walls, corners and random floor tiles (Enter: new room) |
| `Zelda1` | Player | Top-down movement with idle and walk states; animations defined in XML |
| `Zelda2` | Enemies | Enemy types defined in XML, and AI states (walk, idle) |
| `Zelda3` | Combat | Sword hitbox, player hurtbox, damage, invulnerability, hearts HUD and game over |
| `Zelda4` | Events | Player death becomes an event; a floor switch opens the doors via `OnCollide` |
| `Zelda5` | Screen scrolling | The dungeon, a camera, and tweened transitions between rooms |
| `Zelda6` | Stencil | The player disappears into the door arches (stencil buffer) |
| `Zelda7` | Audio | Music and sound effects (the finished game) |

## New in GMDCore

Compared with the core in [06-platformer](../06-platformer/):

- `Graphics/Animation`: can play once instead of looping (`Loop`).
- `Graphics/AnimatedSprite`: `Restart()` and `TimesPlayed`, for one-shot animations such
  as a sword swing.
- `Graphics/TextureAtlas`: `FromGrid` splits a sprite sheet into equal frames, and
  `CreateAnimation` builds an animation from frame numbers.
- `Tweening/` (new): tweens, timers and callbacks (`Tween`, `After`, `Every`), used for the
  room transition.

## Code Map

The finished game, `Zelda7`:

```text
Zelda7/
├── Game1.cs              # Owns the current game state and delegates Update/Draw to it
├── GameSettings.cs       # Every constant: sizes, speeds, tile IDs, timings
├── Entities/             # IEntity; Entity (base of Player and Enemy); GameObject
├── States/
│   ├── GameStates/       # StartState, PlayState, GameOverState
│   ├── EntityStates/     # EntityStateBase, EntityWalkState, EntityIdleState (enemy AI)
│   └── PlayerStates/     # PlayerIdleState, PlayerWalkState, PlayerSwingSwordState
├── World/                # Room, Dungeon, Doorway
├── Definitions/          # Loaders for the XML data files
├── Input/GameController.cs  # Keys mapped to game actions
├── Graphics/             # Camera, DebugDraw
└── Audio/SoundManager.cs
```

Where to find things:

| To see | Look at |
| --- | --- |
| How a room is built | `World/Room.cs`: `GenerateWallsAndFloors`, `GenerateEntities`, `GenerateObjects`, `GenerateDoorways` |
| The floor switch and its event | `World/Room.cs`, `GenerateObjects` (the `OnCollide` handler) |
| Player–enemy and player–object collision | `World/Room.cs`, `Update` |
| The sword's hitbox | `States/PlayerStates/PlayerSwingSwordState.cs` |
| The player's hurtbox | `Entities/Player.cs`, `Hurtbox` |
| Enemy AI | `States/EntityStates/EntityWalkState.cs`, `ProcessAI` |
| `OnPlayerDied`, from room to game over | `World/Room.cs` → `World/Dungeon.cs` → `States/GameStates/PlayState.cs` |
| The room transition (camera tween) | `World/Dungeon.cs`: `BeginShift`, `Update`, `FinishShift` |
| The three stencil passes | `World/Dungeon.cs`: `Render`, `DrawArchMasks` |
| Enemy types and animations | `Content/Assets/data/enemy_animations.xml`, loaded by `Definitions/EntityDefinitions.cs` |
| Objects and doorway tiles | `Content/Assets/data/object_definitions.xml` and `door_layouts.xml` |
| The player's animations | `Content/Assets/data/player_animations.xml` |
| The tween system | `GMDCore/Tweening/TweenManager.cs` |

## Tools

The XML files refer to tiles and frames by their number in a sprite sheet.
`Tools/LabelTiles.cs` writes each tile's number onto a copy of a sheet, so you can look
them up. It is a standalone script (it needs the .NET 10 SDK), not part of the game:

```sh
cd 08-zelda/Tools
dotnet run LabelTiles.cs ../Content/Assets/images/entities.png 16 16
```

`tilesheet_labeled.png` and `entities_labeled.png` in `Tools/` are its output for the two
main sheets.

## Content

All steps share the same raw assets, built by the **content builder** (MonoGame 3.8.5+):

```text
Content/
├── Assets/                  # The raw assets: images, font, sounds, XML data files
├── Builder/Builder.cs       # The rules for building the assets, in C#
├── BuildContent.targets     # Runs the builder when the game project builds
└── Content.csproj
```

There is no `.mgcb` file and no MGCB Editor. `Builder.cs` decides how each kind of asset is
processed. Each step project imports `BuildContent.targets`, so building a step also builds
the assets into its output folder, where `Content.Load` finds them.

To add an asset, put it in `Content/Assets` and, if no existing rule matches it, add a rule
in `Builder.cs`.

## Controls

| Key | Action |
| --- | --- |
| Arrow keys, `W` `A` `S` `D` | Walk |
| `Space` | Swing the sword |
| `Enter` | Start, and continue after game over |
| `Esc` | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 08-zelda
dotnet run --project Zelda7
```

Or open `Zelda.slnx` and choose the step to run.

## Credits

The art, sounds and music are our own, made for the course. The font is
[Press Start 2P](https://fonts.google.com/specimen/Press+Start+2P) by CodeMan38, under the
SIL Open Font License (see `Content/Assets/fonts/retro-OFL.txt`).
