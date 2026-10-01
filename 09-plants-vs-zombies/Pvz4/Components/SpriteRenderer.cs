using System;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz4.Components;

// Draws the entity. Standing sprites (defenders, goblins) stand on their feet and breathe;
// centred ones (arrows, coins) are drawn around their position.
public class SpriteRenderer(TextureRegion sprite, bool centered = false) : Component
{
    private readonly float _phase = Random.Shared.NextSingle() * MathHelper.TwoPi;
    private float _time;

    // One pixel of the art is 5 pixels on screen, so breathing rises by whole art pixels.
    private const int ArtPixel = 5;

    public float Rise => !centered && MathF.Sin(_time * 3 + _phase) > 0 ? ArtPixel : 0;

    public override void Update(float deltaSeconds) => _time += deltaSeconds;

    public override void Draw(SpriteBatch spriteBatch)
    {
        // Red for a moment when hit, if the entity has health.
        Color color = Owner.Get<Health>()?.IsFlashing == true ? new Color(255, 150, 150) : Color.White;
        Vector2 origin = centered ? new Vector2(sprite.Width / 2f, sprite.Height / 2f) : new Vector2(sprite.Width / 2f, sprite.Height);
        sprite.Draw(spriteBatch, Owner.Position - new Vector2(0, Rise), color, 0, origin, 1, SpriteEffects.None, 0);
    }
}
