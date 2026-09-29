using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario5.Input;
using Mario5.LevelMaker;
using Mario5.Entities;

namespace Mario5.States.GameStates;

public class PlayState(Game1 game) : GameStateBase(game)
{
    private LevelMakerBase _levelMaker;
    private Player _player;
    private GameLevel _currentLevel;

    public override void Enter()
    {
        _levelMaker = new ComplexLevelMaker(Game.Content);
        _currentLevel = _levelMaker.Generate(50, 9);

        TextureAtlas playerAtlas = TextureAtlas.FromFile(Game.Content, "images/player.xml");
        _player = new Player(playerAtlas, _currentLevel);
        _currentLevel.Player = _player;
    }

    public override void Update(GameTime gameTime)
    {
        if (GameController.Randomize)
        {
            _currentLevel.RandomizeGraphics(_levelMaker);
        }
        if (GameController.Reset)
        {
            Game.SetState(new StartState(Game));
        }

        _currentLevel.Update(gameTime);

        // Falling into a pit ends the game.
        if (_player.Position.Y > _currentLevel.Tilemap.Rows * _currentLevel.Tilemap.TileHeight)
        {
            Game.SetState(new StartState(Game));
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _currentLevel.Draw(spriteBatch, Game.ScreenScaleMatrix);
    }
}
