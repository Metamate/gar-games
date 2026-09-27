# Pong

Source code for session **01 Pong** of the Game Architecture (GAR) course: a two-player
Pong, built exercise by exercise. The concepts (the game loop, drawing, input, delta time,
the Update Method pattern, AABB collision, game state) are explained on the [session
page](https://metamate.github.io/gar/sessions/01-pong/). This README is the map of the code.

## Steps

The game is built up in steps. Each step is a separate project that builds on the previous
one, following the exercises from the session. Compare two neighbouring steps (e.g. with a
diff tool) to see exactly what changed.

| Step | Exercise | What's new |
| --- | --- | --- |
| `Pong0` | An empty game | An empty MonoGame project |
| `Pong1` | Text on screen | Text centred on a 1280×720 window |
| `Pong2` | Virtual resolution | Resolution independent of the window, point filtering |
| `Pong3` | Rectangles from a pixel | Paddles and ball, and a custom retro font (`sans` → `font`) |
| `Pong4` | Input and delta time | Keyboard input, frame-rate independent movement |
| `Pong5` | A moving ball | Launch the ball, keep the paddles on screen; a first game mode |
| `Pong6` | The Update Method | `Paddle` and `Ball` classes (Update Method pattern) |
| `Pong7` | Collisions and the bounce | AABB bounces off paddles and walls; where the ball hits the paddle sets the angle |
| `Pong8` | Scoring | Scores in a bigger font |
| `Pong9` | A serve mode | The player who was scored on serves |
| `Pong10` | A winner | A done mode when a player reaches 10 points |
| `Pong11` | Sound effects | Sound effects for hits and scoring |

## Code Map

The finished game, `Pong11`:

| To see | Look at |
| --- | --- |
| The game loop, states, scoring and drawing | `Game1.cs` |
| A paddle: input, movement, drawing | `Paddle.cs` |
| The ball: movement and AABB collision | `Ball.cs` |

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
| `W` `S` | Left paddle |
| Up and down arrows | Right paddle |
| `Enter` | Start, serve, and restart after a win |
| `Esc` | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 01-pong
dotnet run --project Pong11
```

Or open `Pong.slnx` and choose the step to run.

## Credits

The sounds are our own, made for the course. The font is
[Press Start 2P](https://fonts.google.com/specimen/Press+Start+2P) by CodeMan38, under the
SIL Open Font License (see `Content/Assets/retro-OFL.txt`).
