using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario8.Input;
using Mario8.LevelMaker;
using Mario8.Audio;
using Mario8.Entities;

namespace Mario8.States.GameStates;

public class PlayState(Game1 game) : GameStateBase(game)
{
    private LevelMakerBase _levelMaker;
    private Player _player;
    private GameLevel _currentLevel;

    public override void Enter()
    {
        _levelMaker = new ComplexLevelMaker(Game.Content);
        _currentLevel = _levelMaker.Generate(50, 9);

        TextureAtlas alienAtlas = TextureAtlas.FromFile(Game.Content, "images/alien.xml");
        _player = new Player(alienAtlas, _currentLevel);
        _currentLevel.Player = _player;

        SoundManager.PlayMusic();
    }

    public override void Exit()
    {
        SoundManager.StopMusic();
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

        if (_player.Position.Y > _currentLevel.Tilemap.Rows * _currentLevel.Tilemap.TileHeight)
        {
            _player.Active = false;
        }

        if (!_player.Active)
        {
            SoundManager.PlayDeath();
            Game.SetState(new StartState(Game));
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _currentLevel.Draw(spriteBatch, Game.ScreenScaleMatrix);

        spriteBatch.Begin(transformMatrix: Game.ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        spriteBatch.DrawString(Game1.DefaultFont, $"Score: {_player.Score}", new Vector2(5, 5), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
        spriteBatch.End();
    }
}
