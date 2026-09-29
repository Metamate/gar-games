# Pokemon

The code for session **10 Pokemon** of the Game Architecture (GAR) course: a turn-based RPG.
The [session page](https://metamate.github.io/gar/sessions/10-pokemon/) explains the ideas;
this README shows where to find them in the code.

## Steps

The game is built up in steps. Each step is a separate project that builds on the previous
one, so you can follow the game one concept at a time. Compare two neighbouring
steps (e.g. with a diff tool) to see exactly what changed.

| Step | Topic | What's new |
| --- | --- | --- |
| `Pokemon0` | Overworld | Two tile layers, and tile-based movement tweened between tiles |
| `Pokemon1` | State stack | Title screen, fade transitions and dialogue boxes layered on the stack; species data from JSON |
| `Pokemon2` | Battles | Random encounters in tall grass, the battle scene, messages and a menu; for now you can only run |
| `Pokemon3` | Turns & RPG mechanics | Fight: turn order, damage, experience, level-up, fainting; healing at the spring |
| `Pokemon4` | Audio | A real audio service replaces the silent `NullAudio` in the locator (the finished game) |

Next to the steps, `UiSamples` puts the game's widgets on one screen, each on its own: a
menu whose options do something you can see, a panel, a progress bar that tweens, and a
textbox that pages through a message. It uses the widgets from `Pokemon4` itself.

## New in GARCore

Compared with the core in [07-the-legend-of-zelda](../07-the-legend-of-zelda/):

- `States/StateStack`, `States/GameStateBase` (new): layered game states.
- `GUI/Panel`, `GUI/ProgressBar`, `Graphics/BitmapFont`, `Graphics/TextureFactory` (new).
- `Core`: owns the `StateStack`, always reads input before game logic (games override
  `UpdateGame`), draws the game into a letterboxed rectangle at its virtual resolution
  (`DestinationRectangle`), and provides a 1×1 `Pixel` texture.

## Code Map

The finished game, `Pokemon4`:

```text
Pokemon4/
├── Game1.cs              # Loads content, registers services, updates tweens then the state stack
├── GameSettings.cs       # Every constant: sizes, tile IDs, timings, encounter chance
├── GameAssets.cs         # Fonts and textures shared through the locator
├── Locator.cs            # The Service Locator: tweens, audio, assets
├── Audio/                # IAudio, NullAudio, SoundManager
├── Definitions/          # ContentLoader: JSON into animations and textures
├── Entities/             # Entity, Player, Direction, animation keys
├── GUI/                  # Textbox, Menu, Selection
├── Input/GameController.cs  # Keys mapped to game actions
├── Mons/                 # PokemonSpecies, PokemonDefinitions, Mon, Move, Party
├── Battle/               # BattleSprite, Opponent
├── States/
│   ├── GameStates/       # StartState, PlayState, BattleState, BattleMenuState, TakeTurnState,
│   │                     # BattleMessageState, FadeState, DialogueState
│   ├── EntityStates/     # EntityStateBase, EntityWalkState, EntityIdleState
│   └── PlayerStates/     # PlayerIdleState, PlayerWalkState
└── World/Level.cs        # The two tile layers and the player
```

A good order to read it in:

1. `Game1.cs`: the big picture.
2. `GARCore/States/StateStack.cs` and `GameStateBase.cs`: how the game's screens are layered.
3. `States/GameStates/PlayState.cs`: the overworld, as one state.
4. `Entities/Entity.cs`, `States/EntityStates/EntityWalkState.cs` and
   `States/PlayerStates/PlayerWalkState.cs`: tile movement and encounters.
5. `States/GameStates/BattleState.cs`, `BattleMenuState.cs` and `TakeTurnState.cs`: the
   battle, as states that work together.
6. `Mons/Mon.cs`: stats, damage, experience and levelling.

On a first read, skip the rest of `GARCore/Graphics`, the bitmap font, the audio and the
tween internals: they support the game, but the architecture is in the files above.

Where to find things:

| To see | Look at |
| --- | --- |
| The order of updates each frame | `Game1.cs`, `UpdateGame` |
| Pushing, popping and drawing states | `GARCore/States/StateStack.cs` |
| A fade | `States/GameStates/FadeState.cs` |
| Moving from tile to tile | `States/EntityStates/EntityWalkState.cs`, `AttemptMove` |
| Random encounters | `States/PlayerStates/PlayerWalkState.cs`, `TryStartEncounter` |
| The healing spring | `States/PlayerStates/PlayerWalkState.cs`, `TryHeal` |
| The battle scene and its bars | `States/GameStates/BattleState.cs` |
| One turn, as a chain of tweens | `States/GameStates/TakeTurnState.cs` |
| Stats, damage, EXP and level-ups | `Mons/Mon.cs` |
| The species | `Content/Assets/data/pokemon_definitions.json`, loaded by `Mons/PokemonDefinitions.cs` |
| The player's animations | `Content/Assets/data/entity_animations.json`, loaded by `Definitions/ContentLoader.cs` |
| The services and the null object | `Locator.cs`, `Audio/NullAudio.cs` |
| The widgets | `GARCore/GUI/`, and `GUI/` for the game's own |
| The tween system | `GARCore/Tweening/TweenManager.cs` |

## Tools

The bitmap fonts in `Content/Assets/fonts/` are atlases made from `retro.ttf` by
`Tools/GenerateFontAtlas.py` (it needs Python and Pillow). To use another font, point the
script at it, run it from this folder, and copy the cell sizes and advance widths it prints
into `GARCore/Graphics/BitmapFont.cs`:

```sh
cd 10-pokemon
python Tools/GenerateFontAtlas.py
```

## Controls

| Key | Action |
| --- | --- |
| Arrow keys, `W` `A` `S` `D` | Walk, and move through menus |
| `Enter` or `Space` | Confirm |
| `Esc` | Quit |

## Running a step

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
cd 10-pokemon
dotnet run --project Pokemon4
```

The UI samples run the same way: `dotnet run --project UiSamples`.

Or open `Pokemon.slnx` and choose the step to run.

## Credits

The art, sounds and music are our own, made for the course; the monsters are original designs.
The font is [Press Start 2P](https://fonts.google.com/specimen/Press+Start+2P) by CodeMan38,
under the SIL Open Font License (see `Content/Assets/fonts/retro-OFL.txt`).
