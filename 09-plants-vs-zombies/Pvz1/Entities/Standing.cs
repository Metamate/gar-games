using System;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz1.Entities;

// Drawing helpers shared by the plants and the zombies.
public static class Standing
{
    // One pixel of the art is 4 pixels on screen: breathing moves the sprite by whole art pixels.
    public const int ArtPixel = 4;

    // A sprite standing on its feet (its bottom centre), breathing, and red when hit.
    public static void Draw(SpriteBatch spriteBatch, TextureRegion sprite, Vector2 feet, float time, bool hit)
    {
        float rise = MathF.Sin(time * 3) > 0 ? ArtPixel : 0;
        Color color = hit ? new Color(255, 150, 150) : Color.White;
        sprite.Draw(spriteBatch, feet - new Vector2(0, rise), color, 0, new Vector2(sprite.Width / 2f, sprite.Height), 1, SpriteEffects.None, 0);
    }
}
