using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GARCore;

namespace GARCore.GUI;

// A bordered panel background: an outer border and an inner fill.
public sealed class Panel
{
    public float X      { get; set; }
    public float Y      { get; set; }
    public float Width  { get; set; }
    public float Height { get; set; }
    public bool  Visible { get; set; } = true;

    // Every panel shares these, so a game sets its look once.
    public static Color BorderColor { get; set; } = Color.White;
    public static Color FillColor   { get; set; } = new(56, 56, 56);

    public Panel(float x, float y, float width, float height)
    {
        X = x; Y = y; Width = width; Height = height;
    }

    public void Hide() => Visible = false;

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!Visible) return;

        // Outer border
        spriteBatch.Draw(Core.Pixel,
            new Rectangle((int)X, (int)Y, (int)Width, (int)Height),
            BorderColor);

        // Inner fill
        spriteBatch.Draw(Core.Pixel,
            new Rectangle((int)X + 2, (int)Y + 2, (int)Width - 4, (int)Height - 4),
            FillColor);
    }
}
