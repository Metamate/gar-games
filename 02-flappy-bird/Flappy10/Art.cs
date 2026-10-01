using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy10;

public static class Art
{
    public static Texture2D Bird { get; set; }
    public static Texture2D Background { get; set; }
    public static Texture2D Ground { get; set; }
    public static Texture2D Pipe { get; set; }
    public static SpriteFont Font { get; set; }
    public static SpriteFont SmallFont { get; set; }
    // One white pixel, stretched into rectangles.
    public static Texture2D Pixel { get; set; }

    public static void LoadContent(ContentManager content)
    {
        Bird = content.Load<Texture2D>("images/bird");
        Background = content.Load<Texture2D>("images/background");
        Ground = content.Load<Texture2D>("images/ground");
        Pipe = content.Load<Texture2D>("images/pipe");
        Font = content.Load<SpriteFont>("fonts/font");
        SmallFont = content.Load<SpriteFont>("fonts/font-small");
    }

    // Text on whole pixels with a dark shadow, so it reads over the clouds and the pipes.
    public static void DrawText(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position)
    {
        position = Vector2.Floor(position);
        spriteBatch.DrawString(font, text, position + new Vector2(2, 2), Color.Black * 0.5f);
        spriteBatch.DrawString(font, text, position, Color.White);
    }

    // The same, centred on a point.
    public static void DrawCentred(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 centre)
        => DrawText(spriteBatch, font, text, centre - font.MeasureString(text) / 2);

    // The title screen: the name, the controls and "Press Enter", on a dark band over the game.
    public static void DrawTitle(SpriteBatch spriteBatch, string name, string controls, int width, int height)
    {
        string prompt = "Press Enter";
        Vector2 nameSize = Font.MeasureString(name);
        Vector2 controlsSize = SmallFont.MeasureString(controls);
        Vector2 promptSize = SmallFont.MeasureString(prompt);

        // One line of text between the rows, and above and below them.
        int gap = (int)promptSize.Y;
        int total = (int)(nameSize.Y + gap + controlsSize.Y + gap + promptSize.Y);
        int top = (height - total) / 2;
        int controlsTop = top + (int)nameSize.Y + gap;
        int promptTop = top + total - (int)promptSize.Y;

        spriteBatch.Draw(Pixel, new Rectangle(0, top - gap, width, total + 2 * gap), Color.Black * 0.75f);
        spriteBatch.DrawString(Font, name, new Vector2((int)((width - nameSize.X) / 2), top), Color.White);
        spriteBatch.DrawString(SmallFont, controls, new Vector2((int)((width - controlsSize.X) / 2), controlsTop), Color.White * 0.7f);
        spriteBatch.DrawString(SmallFont, prompt, new Vector2((int)((width - promptSize.X) / 2), promptTop), Color.White);
    }
}
