using GARCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mario7.Input;
using Mario7.LevelMaker;

namespace Mario7.States.GameStates;

public class StartState(Game1 game) : GameStateBase(game)
{
    private readonly string[] _title = ["Super Mario", "Bros"];
    private const string Subtitle = "Press Enter";
    private GameLevel _backgroundLevel;

    public override void Enter()
    {
        var levelMaker = new ComplexLevelMaker(Game.Content);
        _backgroundLevel = levelMaker.Generate(30, 9);
    }

    public override void Update(GameTime gameTime)
    {
        if (GameController.Start)
        {
            Game.SetState(new PlayState(Game));
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _backgroundLevel.Draw(spriteBatch, Game.ScreenScaleMatrix);

        spriteBatch.Begin(transformMatrix: Game.ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        float y = 36;
        foreach (string line in _title)
        {
            DrawCentred(spriteBatch, Game1.TitleFont, line, y);
            y += Game1.TitleFont.LineSpacing;
        }
        DrawCentred(spriteBatch, Game1.DefaultFont, Subtitle, y + 8);
        spriteBatch.End();
    }

    // Centred, on whole pixels, with a dark shadow so it reads over the clouds.
    private static void DrawCentred(SpriteBatch spriteBatch, SpriteFont font, string text, float y)
    {
        var position = new Vector2((int)((GameSettings.VirtualWidth - font.MeasureString(text).X) / 2), (int)y);
        spriteBatch.DrawString(font, text, position + Vector2.One, Color.Black * 0.6f);
        spriteBatch.DrawString(font, text, position, Color.White);
    }
}
