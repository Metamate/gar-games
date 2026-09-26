# Super Mario Bros

Source code for session **06 Super Mario Bros** of the Game Architecture (GAR) course: a 2D
platformer with procedural levels, the State pattern, a camera and platformer physics.

The game is built up in steps. Each step is a separate project that builds on the previous
one, so you can follow the code's evolution one concept at a time. Compare two neighbouring
steps (e.g. with a diff tool) to see exactly what changed.

| Step | Topic | What's new |
| --- | --- | --- |
| `Platformer0` | Tilemaps from code | A level generated in code: sky and solid ground tiles, drawn with a random tileset |
| `Platformer1` | Level makers | Interchangeable level generators (the Strategy pattern), a second tilemap for the toppers, and backgrounds |
| `Platformer2` | Player & physics | Gravity, jumping, tile collision, a smaller hitbox and coyote time; `F1` shows tiles and hitboxes (debug drawing); the player's state is an enum |
| `Platformer3` | State pattern | Each player state (idle, walk, jump, fall, duck) becomes its own class |
| `Platformer4` | Camera | A level wider than the screen, a camera following the player, and a parallax background |
| `Platformer5` | Game states | A title screen and a play state |
| `Platformer6` | Entities | `IEntity`, bushes, mystery boxes that pop out gems, and a score |
| `Platformer7` | Basic AI | Snails with their own states (idle, walk, chase); stomp them or die |
| `Platformer8` | Audio | Music and sound effects (the finished game) |

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

## Content

All steps share the same raw assets, built by the **content builder** (MonoGame 3.8.5+):

```text
Content/
├── Assets/                  # The raw assets: images, fonts, sounds, data files
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
| `A` `D` / arrow keys | Move (from `Platformer2`) |
| `Space` | Jump (from `Platformer2`) |
| `S` / down arrow | Duck (from `Platformer2`) |
| `R` | Randomize the level's graphics (from `Platformer0`) |
| `F1` | Debug drawing: solid tiles, hitboxes and entities (from `Platformer2`) |
| `1`–`5` | Switch level maker (`Platformer1` only) |
| `Enter` | Start the game (from `Platformer5`) |
| `F` | Back to the title screen (from `Platformer5`) |
| `Esc` | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 06-platformer
dotnet run --project Platformer8
```

Or open `Platformer.slnx` and choose the step to run.
