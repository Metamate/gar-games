# Snake

Source code for session **03 Snake** of the Game Architecture (GAR) course: Snake in a room
with bats, with its art and room defined as data. The concepts (texture atlases, sprites and
animation from XML, tilemaps, fixed-tick movement, input as buffered actions) are explained
on the [session page](https://metamate.github.io/gar/sessions/03-snake/). This README is the
map of the code.

## Steps

The game is built up in steps. Each step is a separate project that builds on the previous
one, so you can follow the code's evolution one concept at a time. Compare two neighbouring
steps (e.g. with a diff tool) to see exactly what changed.

| Step | Topic | What's new |
| --- | --- | --- |
| `Snake0` | Starting point | Loads the atlas image and draws parts of it with hardcoded source rectangles |
| `Snake1` | Texture atlas | `TextureAtlas` loads named regions from `atlas-definition.xml` |
| `Snake2` | Sprites | `Sprite` wraps a region with color, rotation, scale and origin |
| `Snake3` | Animation | `AnimatedSprite` plays animations defined in the atlas |
| `Snake4` | The room | The room is drawn from `tilemap-definition.xml` |
| `Snake5` | Fixed-tick movement | A `Snake` made of grid cells moves by itself, one cell per 200 ms tick; the game reads the keys directly |
| `Snake6` | Input as actions | A `GameController` maps W/A/S/D and the arrow keys to actions |
| `Snake7` | Input buffering | Turns are queued and used one per tick, so quick key presses aren't lost |
| `Snake8` | The bat | A bouncing bat with circle collision; eating it makes the snake grow |
| `Snake9` | Game over | Walls and the snake's own body end the game (the finished game) |

All steps share the **GMDCore** library, which contains the final versions of the reusable
classes (`TextureAtlas`, `Sprite`, `AnimatedSprite`, `Tilemap`, `Circle`, input, …).

## New in GMDCore

Compared with the core in [02-flappy](../02-flappy/):

- `Graphics/TextureRegion`, `TextureAtlas`, `Sprite`, `Animation`, `AnimatedSprite`: parts
  of a texture, sprites and frame animation, defined in XML.
- `Graphics/Tileset`, `Tilemap`: a grid of tile IDs drawn from a tileset, defined in XML.
- `Circle`: circle-circle collision.

## Code Map

The finished game, `Snake9`:

| To see | Look at |
| --- | --- |
| The atlas, animations and room, as data | `Content/Assets/images/*.xml` |
| Atlas, sprite, animation and tilemap classes | `GMDCore/Graphics/` |
| The snake: fixed-tick movement and growth | `Snake.cs` |
| Keys to actions, with a buffer | `GameController.cs` |
| The bats | `Bat.cs` |
| The tick accumulator and collisions | `Game1.cs` |

## Content

All steps also share the same assets and the same **content builder** (MonoGame 3.8.5+):

```text
Content/
├── Assets/                  # The raw assets: images and XML definitions
├── Builder/Builder.cs       # The rules for building the assets, in C#
├── BuildContent.targets     # Runs the builder when a game project builds
└── Content.csproj
```

There is no `.mgcb` file and no MGCB Editor. `Builder.cs` decides how each asset is
processed: PNG images are built into textures, and the XML files are copied as they are.
Each step project imports `BuildContent.targets`, so building a step also builds the content
into its output folder, where `Content.Load` finds it.

To add an asset, put it in `Content/Assets` and, if no existing rule matches it, add a rule
in `Builder.cs`.

## Controls

| Key | Action |
| --- | --- |
| `W` `A` `S` `D` | Turn (from `Snake5`) |
| Arrow keys | Turn (from `Snake6`) |
| `Esc` | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 03-snake
dotnet run --project Snake9
```

Or open `Snake.slnx` and choose the step to run.

## Credits

The art is our own, made for the course.
