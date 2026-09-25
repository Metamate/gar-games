using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Birds2;

public static class LineDrawing
{
    // A line is a 1x1 white pixel, stretched to the length of the line and turned to its angle.
    public static void DrawLine(this SpriteBatch spriteBatch, Texture2D pixel, Vector2 from, Vector2 to, Color color, float thickness = 1)
    {
        Vector2 line = to - from;
        float angle = MathF.Atan2(line.Y, line.X);
        spriteBatch.Draw(pixel, from, null, color, angle, new Vector2(0, 0.5f), new Vector2(line.Length(), thickness), SpriteEffects.None, 0);
    }
}
