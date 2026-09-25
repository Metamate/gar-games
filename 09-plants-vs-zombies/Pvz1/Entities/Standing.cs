using System;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz1.Entities;

// Drawing helpers shared by the plants and the zombies.
public static class Standing
{
    // A sprite standing on its feet (its bottom centre), gently breathing, and red when hit.
    public static void Draw(SpriteBatch spriteBatch, TextureRegion sprite, Vector2 feet, float time, bool hit)
    {
        float breathe = 1 + 0.025f * MathF.Sin(time * 3);
        Color color = hit ? new Color(255, 150, 150) : Color.White;
        sprite.Draw(spriteBatch, feet, color, 0, new Vector2(sprite.Width / 2f, sprite.Height), new Vector2(1, breathe), SpriteEffects.None, 0);
    }
}
