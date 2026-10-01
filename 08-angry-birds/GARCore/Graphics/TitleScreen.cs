using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GARCore.Graphics;

// The title screen, the same in every game: the game's name, its controls and "Press Enter",
// on a dark band across the middle of the screen, with the game drawn behind it.
public static class TitleScreen
{
    public const string Prompt = "Press Enter";

    private static Texture2D _pixel;

    // White text on a dark band.
    public static void Draw(SpriteBatch spriteBatch, SpriteFont titleFont, SpriteFont font, string name, string controls,
        int width, int height)
    {
        Draw(spriteBatch, titleFont, font, name, controls, width, height, Color.White, Color.Black * 0.75f);
    }

    // Call between Begin and End, after drawing the game. The width and height are the
    // game's virtual resolution. The controls are drawn dimmer than the name and the prompt.
    public static void Draw(SpriteBatch spriteBatch, SpriteFont titleFont, SpriteFont font, string name, string controls,
        int width, int height, Color text, Color band)
    {
        if (_pixel == null)
        {
            _pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _pixel.SetData([Color.White]);
        }

        Vector2 nameSize = titleFont.MeasureString(name);
        Vector2 controlsSize = font.MeasureString(controls);
        Vector2 promptSize = font.MeasureString(Prompt);

        // One line of text between the rows, and above and below them.
        int gap = (int)promptSize.Y;
        int total = (int)(nameSize.Y + gap + controlsSize.Y + gap + promptSize.Y);
        int top = (height - total) / 2;
        int controlsTop = top + (int)nameSize.Y + gap;
        int promptTop = top + total - (int)promptSize.Y;

        spriteBatch.Draw(_pixel, new Rectangle(0, top - gap, width, total + 2 * gap), band);
        spriteBatch.DrawString(titleFont, name, new Vector2((int)((width - nameSize.X) / 2), top), text);
        spriteBatch.DrawString(font, controls, new Vector2((int)((width - controlsSize.X) / 2), controlsTop), text * 0.7f);
        spriteBatch.DrawString(font, Prompt, new Vector2((int)((width - promptSize.X) / 2), promptTop), text);
    }
}
