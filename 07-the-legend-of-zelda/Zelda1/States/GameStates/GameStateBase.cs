using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda1.States.GameStates;

public abstract class GameStateBase(Game1 game)
{
    protected Game1 Game { get; } = game;

    public virtual void Enter() { }
    public virtual void Exit() { }
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);

    // Returns the position that centres text horizontally and vertically, with an optional Y
    // offset from dead centre, on whole pixels so the pixel font stays crisp.
    protected static Vector2 ScreenCenter(SpriteFont font, string text, float yOffset = 0f)
    {
        Vector2 size = font.MeasureString(text);
        return Vector2.Floor(new Vector2(
            GameSettings.VirtualWidth  / 2f - size.X / 2f,
            GameSettings.VirtualHeight / 2f - size.Y / 2f + yOffset));
    }

    // Returns positions for a centred title (in the title font) and a subtitle below it.
    protected static (Vector2 TitlePos, Vector2 SubtitlePos) CalculateTitleLayout(string title, string subtitle)
    {
        var titlePos    = ScreenCenter(Game1.TitleFont, title, GameSettings.UiTitleYOffset);
        float titleHeight = Game1.TitleFont.MeasureString(title).Y;
        var subtitlePos = new Vector2(
            ScreenCenter(Game1.DefaultFont, subtitle).X,
            titlePos.Y + titleHeight + GameSettings.UiSubtitleSpacing);
        return (titlePos, subtitlePos);
    }
}
