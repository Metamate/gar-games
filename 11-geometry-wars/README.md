# Geometry Wars

Source code for session **11 Geometry Wars** of the Game Architecture (GAR) course: a
twin-stick shooter with hundreds of entities built from components. The concepts
(components vs. systems, dependency injection and testing with fakes, object pooling,
flyweights, shaders) are explained on the
[session page](https://metamate.github.io/gar/sessions/11-geometry-wars/). This README is the
map of the code.

## Steps

The game is built up in steps. Each step is a separate project that builds on the previous
one, mostly by adding components to the entity recipes in `EntityFactory` and the systems
they need. Compare two neighbouring steps (e.g. with a diff tool) to see exactly what changed.

| Step | Topic | What's new |
| --- | --- | --- |
| `GeometryWars0` | Entities & components | The player ship, composed from components: sprite, rigidbody, movement input, clamp to screen |
| `GeometryWars1` | Shooting & Object Pool | A weapon component and bullets reused from an object pool |
| `GeometryWars2` | Enemies & collisions | Seeker and wanderer AI, the collision system, score, lives, respawning and game over |
| `GeometryWars3` | Particles | A particle manager for thousands of short-lived particles: exhaust, explosions, bullet sparks |
| `GeometryWars4` | Grid & black holes | The spring grid (data-oriented, flat arrays) and black holes that pull everything in |
| `GeometryWars5` | Bloom | Post-processing shaders for the neon glow |
| `GeometryWars6` | Audio | Music and sound effects (the finished game) |

## New in GMDCore

Compared with the core in [10-pokemon](../10-pokemon/):

- `ECS/` (new): entities made of components.
- `Physics/`, `Collision/CollisionRegistry` (new): colliders, rigidbodies and collision pairs.
- `Particles/` (new): a data-oriented particle system.
- `Collections/ObjectPool` (new).
- `Input/MouseInfo`, `Input/GamePadInfo` (new).
- `Core`: runs game logic in fixed 60 Hz steps with an accumulator, so the game can render
  as fast as it likes while the simulation stays stable.
- `Input/InputManager`, `Input/KeyboardInfo`: input is sampled every frame and handed to
  the game once per logic step, so quick taps are never lost.
- `States/`: a `DrawHUD` pass after post-processing (bloom), and `Draw` is optional.

## Code Map

The finished game, `GeometryWars6`, in layers from the outside in:

| Layer | Class | Owns |
| --- | --- | --- |
| Application | [Game1](GeometryWars6/Game1.cs) | Timing, input, assets, audio, the state stack, drawing |
| Services | [PlayContext](GeometryWars6/Services/PlayContext.cs) | The shared services, passed into gameplay code |
| Screen | [PlayState](GeometryWars6/States/PlayState.cs) | One game in progress: pause, debug, game over, world and HUD drawing |
| Run | [PlaySession](GeometryWars6/Systems/PlaySession.cs) | Everything for one run: score, particles, grid, world, factory, enemy director, player |
| World | [EntityWorld](GeometryWars6/Systems/EntityWorld.cs) | Adding, updating, colliding and removing entities |
| Recipes | [EntityFactory](GeometryWars6/Systems/EntityFactory.cs) | Which components make up each kind of entity |
| Behaviour | `Components/` | One capability each, grouped in `AI`, `Audio`, `Combat`, `Identity`, `Input`, `Lifecycle`, `Physics`, `Visuals` |

A good order to read it in: `Game1`, `PlayState`, `PlaySession`, `EntityFactory`,
[`GMDCore/ECS/Entity.cs`](GMDCore/ECS/Entity.cs), then a few components.

Where to find things:

| To see | Look at |
| --- | --- |
| The component phases, and the order they run in | `GMDCore/ECS/Components/Component.cs`, `GMDCore/ECS/Entity.cs` |
| What the player, bullets, enemies and black holes are made of | `Systems/EntityFactory.cs` |
| The tuning values for each kind of entity | `Definitions/GameplayDefinitions.cs` |
| Collision detection | `Systems/CollisionSystem.cs` |
| When enemies spawn | `Systems/EnemyDirector.cs` |
| The bullet pool | `Systems/BulletSpawner.cs`, `GMDCore/Collections/ObjectPool.cs` |
| What happens when the player dies | `Components/Lifecycle/RespawnState.cs`, and `PlaySession` for the arena |
| Events inside one entity | `Components/Combat/Health.cs`, and the components that subscribe to it |
| Score and multiplier | `Systems/ScoreTracker.cs` (behind `IScoreTracker`) |
| The spring grid | `Systems/Grid.cs` |
| Particles | `GMDCore/Particles/ParticleManager.cs` |
| Shared textures and fonts | `Services/GameAssets.cs` |
| Bloom | `Graphics/` and `Content/Assets/Shaders/` |

## Tests

`GeometryWars.Tests` tests code that gets its services through its constructor, by passing
in its own (the session page explains the idea):

- `AwardScoreOnDestroyedTests`, with `FakeScoreTracker`: destroying an enemy awards its points.
- `ScoreTrackerTests`: the test controls the frame time, so it can check that the
  multiplier expires.

```sh
cd 11-geometry-wars
dotnet test
```

## Content

All steps share the same raw assets, built by the **content builder** (MonoGame 3.8.5+):

```text
Content/
├── Assets/                  # The raw assets: textures, fonts, sounds, shaders
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

| Input | Action |
| --- | --- |
| `W` `A` `S` `D`, or the left stick | Move |
| Mouse, and hold the left button | Aim and fire |
| Arrow keys, or the right stick | Aim and fire, without the mouse |
| `P`, or Start | Pause |
| `F3` | Frame rate and memory |
| `Enter`, or A | Play again, after game over |
| `Esc`, or Back | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 11-geometry-wars
dotnet run --project GeometryWars6
```

Or open `GeometryWars.slnx` and choose the step to run.

## Credits

The art, sounds and music are our own, made for the course. The font is
[Press Start 2P](https://fonts.google.com/specimen/Press+Start+2P) by CodeMan38, under the
SIL Open Font License (see `Content/Assets/retro-OFL.txt`).
