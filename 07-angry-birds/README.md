# Angry Birds

Source code for session **07 Angry Birds** of the Game Architecture (GAR) course.

The game is built up in steps. Each step is a separate project that builds on the previous
one, so you can follow the code's evolution one concept at a time. Compare two neighbouring
steps (e.g. with a diff tool) to see exactly what changed.

| Step | Topic | What's new |
| --- | --- | --- |
| `Birds0` | Box2D, used directly | A hut, two pigs and a slingshot, with Box2D's types, functions and unit conversions all through `Game1` |
| `Birds1` | Adapter & Facade | `PhysicsWorld` and `PhysicsBody` wrap Box2D; the game works in pixels and never sees a Box2D type. Entities own a body. `F1` shows the physics bodies |
| `Birds2` | Contact events | Hits damage blocks and pigs; destroyed entities are removed after the step, not during it. A score |
| `Birds3` | Prototype | Configured entities (prefabs) are cloned into the world; the level is a text file of prefab names and positions |
| `Birds4` | The whole game | Three levels, a few birds per level, an aiming curve, and game states: aim, fly, level end (the finished game) |

All steps share the **GMDCore** library.

## Box2D

The physics comes from [Box2D.NET](https://github.com/ikpil/Box2D.NET), a C# port of
[Box2D](https://box2d.org/) 3.1 (MIT license). Each step project references it as a NuGet
package:

```xml
<PackageReference Include="Box2D.NET" Version="3.1.*" />
```

From `Birds1` on, only the `Physics` folder uses it. Box2D works in metres with y pointing
up; the game works in pixels with y pointing down. `Physics/Units.cs` converts between the
two, at 50 pixels per metre.

## New in GMDCore

Nothing: the core is the same as in [06-platformer](../06-platformer/). The physics adapter
belongs to this game. Putting it in GMDCore would make every later game depend on Box2D.

## Content

All steps share the same assets and the same content builder:

```text
Content/Assets/
├── images/background.png           # Sky, hills and ground
├── images/sprites.png              # Birds, pigs, blocks and the slingshot
├── images/atlas-definition.xml     # The regions in sprites.png
├── levels/level1.txt …             # The levels (Birds3 on), as plain text
└── fonts/hud.spritefont            # The score
```

A level lists one object per line: the prefab's name, its centre (x and y, in pixels) and
its rotation in degrees. `birds 3` sets how many birds the player gets. The ground is at
y = 640.

```text
birds 3
wood-post    880 590 0
wood-plank   960 530 0
pig          960 618 0
```

The prefabs are `wood-`, `stone-` and `glass-` followed by `plank`, `post` or `box`, and
`pig` and `big-pig`.

## Controls

| Key | Action |
| --- | --- |
| Mouse | Drag back from the slingshot, and let go to shoot |
| `R` | Restart the level |
| `F1` | Show the physics bodies (`Birds1` on) |
| `Enter` | Next level, or try again (`Birds4`) |
| `Esc` | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 07-angry-birds
dotnet run --project Birds4
```

Or open `AngryBirds.slnx` and choose the step to run.

## Credits

The art is our own. Box2D.NET is by ikpil, after Box2D by Erin Catto (both MIT).
