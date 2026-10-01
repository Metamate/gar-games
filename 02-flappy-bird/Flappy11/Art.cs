using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy11;

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
}
