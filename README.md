# gar-games

The games of the Game Architecture (GAR) course: one folder per game, in session order.
The course site explains each session: [metamate.github.io/gar](https://metamate.github.io/gar/).

| Folder | Session | Main topic |
| --- | --- | --- |
| [01-pong](01-pong/) | 01 Pong | The game loop |
| [02-flappy](02-flappy/) | 02 Flappy Bird | Organizing a growing game |
| [03-snake](03-snake/) | 03 Snake | Assets as data |
| [04-sokoban](04-sokoban/) | 04 Sokoban | Command: undo and redo |
| [05-pacman](05-pacman/) | 05 Pac-Man | State |
| [06-platformer](06-platformer/) | 06 Super Mario Bros | Physics & tile collision |
| [07-angry-birds](07-angry-birds/) | 07 Angry Birds | Integrating a third-party library |
| [08-zelda](08-zelda/) | 08 The Legend of Zelda | Composition vs. inheritance |
| [09-plants-vs-zombies](09-plants-vs-zombies/) | 09 Plants vs. Zombies | The Component pattern |
| [10-pokemon](10-pokemon/) | 10 Pokemon | Scenes & UI |
| [11-geometry-wars](11-geometry-wars/) | 11 Geometry Wars | Components vs. systems |
| [12-vampire-survivors](12-vampire-survivors/) | 12 Vampire Survivors | Performance: data-oriented design |

## Each game

Every game folder is set up like a game of its own, and like the
[gar-starter](https://github.com/Metamate/gar-starter) template you start your project from:

```text
03-snake/
├── Content/            # The assets and the content builder
├── GMDCore/            # The course's core library, as it is after this game
├── Snake0 … Snake9/    # The game, built up in steps: one project per concept
├── Snake.slnx
└── README.md           # The steps, the controls, and what's new in GMDCore
```

Compare two neighbouring steps (e.g. with a diff tool) to see exactly what changed. The
last step is the finished game.

## Running a game

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
git clone https://github.com/Metamate/gar-games
cd gar-games/03-snake
dotnet run --project Snake9
```

Or open a game's `.slnx` in Visual Studio or Rider, or its folder in VS Code.

## GMDCore

GMDCore is one library that grows through the course. Each game's `GMDCore` keeps everything
from the previous game and adds to it; its README lists what's new. `python tools/check.py`
shows the differences between games, and checks that the build files are identical in every
game. The build on GitHub runs it too.
