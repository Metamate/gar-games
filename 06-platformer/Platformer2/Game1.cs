using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Platformer2.Entities;
using Platformer2.Input;
using Platformer2.LevelMaker;

namespace Platformer2;

public class Game1 : Core
{
    private const int Columns = 16;
    private const int Rows = 9;

    private LevelMakerBase _levelMaker;
    private GameLevel _level;
    private Player _player;

    public Game1() : base("Platformer", 1280, 720, GameSettings.VirtualWidth, GameSettings.VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        _levelMaker = new ComplexLevelMaker(Content);
        StartLevel();
    }

    private void StartLevel()
    {
        _level = _levelMaker.Generate(Columns, Rows);

        TextureAtlas alienAtlas = TextureAtlas.FromFile(Content, "images/alien.xml");
        _player = new Player(alienAtlas, _level);
        _level.Player = _player;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GameController.Randomize)
        {
            _level.RandomizeGraphics(_levelMaker);
        }

        _level.Update(gameTime);

        // Falling into a pit starts a new level.
        if (_player.Position.Y > _level.Tilemap.Rows * _level.Tilemap.TileHeight)
        {
            StartLevel();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _level.Draw(SpriteBatch, ScreenScaleMatrix);
        base.Draw(gameTime);
    }
}
