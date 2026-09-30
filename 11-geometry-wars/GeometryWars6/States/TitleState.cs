using GARCore.States;
using GeometryWars6.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryWars6.States;

// The title screen: Enter swaps it for a new game, as the game over screen does.
public sealed class TitleState : GameStateBase
{
    private readonly Game1 _game;
    private readonly PlayContext _context;

    public TitleState(Game1 game, PlayContext context)
    {
        _game = game;
        _context = context;
    }

    public override void Update(GameTime gameTime)
    {
        if (_context.Controller.WasConfirmPressed)
        {
            _game.StateStack.Pop();
            _game.StateStack.Push(new PlayState(_game, _context));
        }
    }

    public override void DrawHUD(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        Vector2 center = _context.Frame.ScreenSize / 2;
        DrawCentered(spriteBatch, _context.Assets.TitleFont, "Geometry Wars", center - new Vector2(0, 48), new Color(120, 220, 255));
        DrawCentered(spriteBatch, _context.Assets.Font, "Press Enter", center + new Vector2(0, 40), Color.White);
        spriteBatch.End();
    }

    private static void DrawCentered(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 center, Color color)
    {
        Vector2 size = font.MeasureString(text);
        spriteBatch.DrawString(font, text, Vector2.Floor(center - size / 2), color);
    }
}
