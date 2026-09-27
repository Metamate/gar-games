# Flappy Bird

The code for session **02 Flappy Bird** of the Game Architecture (GAR) course: Flappy Bird
on a reusable core library. The [session
page](https://metamate.github.io/gar/sessions/02-flappy-bird/) explains the ideas; this
README shows where to find them in the code.

## Steps

The game is built up in steps. Each step is a separate project that builds on the previous
one, following the exercises from the session. Compare two neighbouring steps (e.g. with a
diff tool) to see exactly what changed.

| Step | Exercise | What's new |
| --- | --- | --- |
| `Flappy0` | A game on the library | `Game1` derives from `Core` in GMDCore |
| `Flappy1` | Drawing images | Background and ground images |
| `Flappy2` | Parallax scrolling | Infinitely scrolling layers at different speeds |
| `Flappy3` | The bird and the Art class | A `Bird` class and a static `Art` class |
| `Flappy4` | Gravity | The bird falls |
| `Flappy5` | An input manager | `InputManager` in GMDCore (keyboard and mouse); flapping with Space or a click |
| `Flappy6` | Spawning on a timer | Pipes spawning on a timer |
| `Flappy7` | A drifting gap | `PipePair` with a gap at a varying height |
| `Flappy8` | Hitboxes | Hitting a pipe, the ground or the ceiling |
| `Flappy9` | A state machine | `IState`, `StateMachine`, title and play states |
| `Flappy10` | Passing data between states | Score while playing, and a score state |
| `Flappy11` | A countdown state | A countdown state before playing |
| `Flappy12` | Audio as a Singleton | Music and sound effects, in an `Audio` Singleton |

All steps share the **GMDCore** library, which contains the final versions of the reusable
classes (`Core`, input, …).

## New in GMDCore

`GMDCore` starts in this session: reusable code that every later game builds on. Each
later repository's `GMDCore` keeps everything from the previous session and adds to it.

- `Core`: a `Game` base class with a window, a virtual resolution and screen scaling.
- `Input/InputManager`, `Input/KeyboardInfo`, `Input/MouseInfo`: keyboard and mouse state
  with "just pressed" and "just released" checks.

## Code Map

The finished game, `Flappy12`:

| To see | Look at |
| --- | --- |
| The shared core: window, scaling, input | `GMDCore/` |
| Loading the textures once | `Art.cs` |
| The bird: gravity and flapping | `Bird.cs` |
| Pipes, and how pairs are generated | `Pipe.cs`, `PipePair.cs`, `States/PlayState.cs` |
| The state machine and its states | `States/StateMachine.cs`, `States/*State.cs` |
| The audio Singleton | `Audio.cs` |

## Content

All steps share one folder of raw assets (fonts, images, sounds), built by the **content
builder** (MonoGame 3.8.5+):

```text
Content/
├── Assets/                  # The raw assets, shared by all steps
├── Builder/Builder.cs       # The rules for building the assets, in C#
├── BuildContent.targets     # Runs the builder when a game project builds
└── Content.csproj
```

There is no `.mgcb` file and no MGCB Editor. `Builder.cs` decides how each kind of asset is
processed. Each step project imports `BuildContent.targets`, so building a step also builds
its assets into its output folder, where `Content.Load` finds them.

To add an asset, put it in `Content/Assets` and, if no existing rule matches it, add a rule
in `Builder.cs`. Compare the `Content.Load` calls in neighbouring steps to see when each
asset comes into use.

If you replace a font's `.ttf`, also save its `.spritefont` (or delete the step's `obj`
folder): the content builder only rebuilds a font when the `.spritefont` itself changes.

## Controls

| Key | Action |
| --- | --- |
| `Space` or a mouse click | Flap, and start |
| `Esc` | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 02-flappy
dotnet run --project Flappy12
```

Or open `Flappy.slnx` and choose the step to run.

## Credits

The art, sounds and music are our own, made for the course. The font is
[Press Start 2P](https://fonts.google.com/specimen/Press+Start+2P) by CodeMan38, under the
SIL Open Font License (see `Content/Assets/fonts/retro-OFL.txt`).
