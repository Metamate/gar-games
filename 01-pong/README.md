# Pong

The code for session **01 Pong** of the Game Architecture (GAR) course: a table tennis game
like Pong, built exercise by exercise. The [session
page](https://metamate.github.io/gar/sessions/01-pong/) explains the ideas; this README
shows where to find them in the code.

## Steps

The game is built up in steps. Each step is a separate project that builds on the previous
one, following the exercises from the session. Compare two neighbouring steps (e.g. with a
diff tool) to see what changed.

| Step | Exercise | What's new |
| --- | --- | --- |
| `Pong0` | An empty game | An empty MonoGame project |
| `Pong1` | Text on screen | Text centred on a 1280×720 window |
| `Pong2` | Virtual resolution | Resolution independent of the window, point filtering |
| `Pong3` | Rectangles from a pixel | Paddles and ball, and a retro font |
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

The sounds are our own, made for the course. The retro font is
[Press Start 2P](https://fonts.google.com/specimen/Press+Start+2P) by CodeMan38, under the
SIL Open Font License (see `Content/Assets/retro-OFL.txt`). The plain font in steps 1 and 2
is [Liberation Sans](https://github.com/liberationfonts/liberation-fonts) by Red Hat, also
under the SIL Open Font License (see `Content/Assets/sans-OFL.txt`).
