using GMDCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Platformer1.LevelMaker;

namespace Platformer1;

public class Game1 : Core
{
    private const int Columns = 16;
    private const int Rows = 9;

    private LevelMakerBase _levelMaker;
    private GameLevel _level;

    public Game1() : base("Platformer", 1280, 720, GameSettings.VirtualWidth, GameSettings.VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        UseLevelMaker(new FlatLevelMaker(Content));
    }

    // The game doesn't know how a level is made. It just asks the current level maker.
    // Swapping the level maker swaps the algorithm: the Strategy pattern.
    private void UseLevelMaker(LevelMakerBase levelMaker)
    {
        _levelMaker = levelMaker;
        _level = _levelMaker.Generate(Columns, Rows);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Input.Keyboard.WasKeyJustPressed(Keys.D1)) UseLevelMaker(new SimpleLevelMaker(Content));
        if (Input.Keyboard.WasKeyJustPressed(Keys.D2)) UseLevelMaker(new FlatLevelMaker(Content));
        if (Input.Keyboard.WasKeyJustPressed(Keys.D3)) UseLevelMaker(new PillarLevelMaker(Content));
        if (Input.Keyboard.WasKeyJustPressed(Keys.D4)) UseLevelMaker(new PitLevelMaker(Content));
        if (Input.Keyboard.WasKeyJustPressed(Keys.D5)) UseLevelMaker(new ComplexLevelMaker(Content));

        // Press R to keep the level but draw it with random graphics.
        if (Input.Keyboard.WasKeyJustPressed(Keys.R))
        {
            _level.RandomizeGraphics(_levelMaker);
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
