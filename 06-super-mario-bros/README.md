# Super Mario Bros

The code for session **06 Super Mario Bros** of the Game Architecture (GAR) course: a 2D
platformer with generated levels. The [session
page](https://metamate.github.io/gar/sessions/06-super-mario-bros/) explains the ideas; this
README shows where to find them in the code.

## Steps

The game is built up in steps. Each step is a separate project that builds on the previous
one, so you can follow the game one concept at a time. Compare two neighbouring
steps (e.g. with a diff tool) to see exactly what changed.

| Step | Topic | What's new |
| --- | --- | --- |
| `Mario0` | Tilemaps from code | A level generated in code: sky and solid ground tiles, drawn with a random tileset |
| `Mario1` | Level makers | Interchangeable level generators (the Strategy pattern), a second tilemap for the toppers, and backgrounds |
| `Mario2` | Player & physics | Gravity, jumping, tile collision, a smaller hitbox and coyote time; `F1` shows tiles and hitboxes (debug drawing); the player's state is an enum |
| `Mario3` | State pattern | Each player state (idle, walk, jump, fall, duck) becomes its own class |
| `Mario4` | Camera | A level wider than the screen, a camera following the player, and a parallax background |
| `Mario5` | Game states | A title screen and a play state |
| `Mario6` | Entities | `IEntity`, bushes, mystery boxes that pop out gems, and a score |
| `Mario7` | Basic AI | Snails with their own states (idle, walk, chase); stomp them or die |
| `Mario8` | Audio | Music and sound effects (the finished game) |

All steps share the **GMDCore** library, which contains the final versions of the reusable
classes (`Tilemap`, `Tile`, `Tileset`, `TextureAtlas`, `AnimatedSprite`, input, …).

## New in GMDCore

Compared with the core in [03-snake](../03-snake/):

- `Graphics/Tile` (new): a tile knows whether it is solid, not just its graphic.
- `Graphics/DebugDraw` (new): outlines for hitboxes and solid tiles, drawn only when enabled.
- `Graphics/Tilemap`: stores `Tile` values, has a `Position`, and adds collision helpers
  (`IsSolidAt`, `GetTileLeft`/`Right`/`Top`/`Bottom`, `TileToPoint`).
- `Graphics/AnimatedSprite`: `Play(animation)` switches to an animation from its first frame
  (and does nothing if it is already playing).

## Code Map

The finished game, `Mario8`:

| To see | Look at |
| --- | --- |
| The level makers (Strategy) | `LevelMaker/` |
| A level: tiles, toppers, entities | `LevelMaker/GameLevel.cs` |
| Tile collision helpers | `GMDCore/Graphics/Tilemap.cs` |
| The player, and its states | `Entities/Player.cs`, `States/PlayerStates/` |
| The snail's AI, as states | `Entities/Snail.cs`, `States/SnailStates/` |
| Boxes and gems | `Entities/MysteryBox.cs`, `Entities/Gem.cs` |
| The camera | `Graphics/Camera.cs` |
| Every constant | `GameSettings.cs` |

## Content

`Content/Assets/images/extras.png` holds art the game doesn't use yet: power-ups (a star, a
mushroom, a heart, a coin, a key and a potion, 16 × 16 each) for the exercises.

## Controls

| Key | Action |
| --- | --- |
| `A` `D` / arrow keys | Move (from `Mario2`) |
| `Space` | Jump (from `Mario2`) |
| `S` / down arrow | Duck (from `Mario2`) |
| `R` | Randomize the level's graphics (from `Mario0`) |
| `F1` | Debug drawing: solid tiles, hitboxes and entities (from `Mario2`) |
| `1`–`5` | Switch level maker (`Mario1` only) |
| `Enter` | Start the game (from `Mario5`) |
| `F` | Back to the title screen (from `Mario5`) |
| `Esc` | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 06-super-mario-bros
dotnet run --project Mario8
```

Or open `SuperMarioBros.slnx` and choose the step to run.

## Credits

The art, sounds and music are our own, made for the course. The font is
[Press Start 2P](https://fonts.google.com/specimen/Press+Start+2P) by CodeMan38, under the
SIL Open Font License (see `Content/Assets/fonts/retro-OFL.txt`).
