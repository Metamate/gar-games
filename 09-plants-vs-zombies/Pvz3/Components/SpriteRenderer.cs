using System;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz3.Components;

// Draws the entity. Standing sprites (plants, zombies) stand on their feet and breathe;
// centred ones (peas, suns) are drawn around their position.
public class SpriteRenderer(TextureRegion sprite, bool centered = false) : Component
{
    private readonly float _phase = Random.Shared.NextSingle() * MathHelper.TwoPi;
    private float _time;

    public float Breathe => centered ? 1 : 1 + 0.025f * MathF.Sin(_time * 3 + _phase);

    public override void Update(float deltaSeconds) => _time += deltaSeconds;

    public override void Draw(SpriteBatch spriteBatch)
    {
        // Red for a moment when hit, if the entity has health.
        Color color = Owner.Get<Health>()?.IsFlashing == true ? new Color(255, 150, 150) : Color.White;
        Vector2 origin = centered ? new Vector2(sprite.Width / 2f, sprite.Height / 2f) : new Vector2(sprite.Width / 2f, sprite.Height);
        sprite.Draw(spriteBatch, Owner.Position, color, 0, origin, new Vector2(1, Breathe), SpriteEffects.None, 0);
    }
}
