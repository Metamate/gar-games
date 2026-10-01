using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong9;

// The title screen, the same in every game: the game's name, its controls and "Press Enter",
// on a dark band across the middle of the screen, with the game drawn behind it.
public static class TitleScreen
{
    public const string Prompt = "Press Enter";

    private static Texture2D _pixel;

    // Call between Begin and End, after drawing the game. The width and height are the
    // game's virtual resolution. The controls are drawn dimmer than the name and the prompt.
    public static void Draw(SpriteBatch spriteBatch, SpriteFont titleFont, SpriteFont font, string name, string controls,
        int width, int height, Color? text = null, Color? dim = null, Color? band = null)
    {
        Draw(spriteBatch, titleFont.MeasureString, (s, position, color) => spriteBatch.DrawString(titleFont, s, position, color),
            font.MeasureString, (s, position, color) => spriteBatch.DrawString(font, s, position, color),
            name, controls, width, height, text, dim, band);
    }

    // The layout, whichever kind of font measures and draws the text.
    private static void Draw(SpriteBatch spriteBatch,
        Func<string, Vector2> measureTitle, Action<string, Vector2, Color> drawTitle,
        Func<string, Vector2> measure, Action<string, Vector2, Color> draw,
        string name, string controls, int width, int height, Color? text, Color? dim, Color? band)
    {
        if (_pixel == null)
        {
            _pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _pixel.SetData([Color.White]);
        }

        Color textColor = text ?? Color.White;
        Vector2 nameSize = measureTitle(name);
        Vector2 controlsSize = measure(controls);
        Vector2 promptSize = measure(Prompt);

        // One line of text between the rows, and above and below them.
        int gap = (int)promptSize.Y;
        int total = (int)(nameSize.Y + gap + controlsSize.Y + gap + promptSize.Y);
        int top = (height - total) / 2;

        spriteBatch.Draw(_pixel, new Rectangle(0, top - gap, width, total + 2 * gap), band ?? Color.Black * 0.75f);
        drawTitle(name, Centred(nameSize, width, top), textColor);
        draw(controls, Centred(controlsSize, width, top + (int)nameSize.Y + gap), dim ?? textColor * 0.7f);
        draw(Prompt, Centred(promptSize, width, top + total - (int)promptSize.Y), textColor);
    }

    private static Vector2 Centred(Vector2 size, int width, int y) => new((int)((width - size.X) / 2), y);
}
