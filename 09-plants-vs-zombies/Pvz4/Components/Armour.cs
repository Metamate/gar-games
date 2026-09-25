using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz4.Components;

// A cone or a bucket: extra health, worn on the head until it's knocked off.
public class Armour(float health, TextureRegion sprite) : Component
{
    private const float HeadHeight = 104;

    public float Current { get; private set; } = health;
    public bool IsIntact => Current > 0;

    // Takes what it can, and returns the damage that gets through.
    public float Absorb(float amount)
    {
        if (!IsIntact)
            return amount;

        Current -= amount;
        return Current < 0 ? -Current : 0;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsIntact)
            return;

        float breathe = Owner.Get<SpriteRenderer>()?.Breathe ?? 1;
        Vector2 head = Owner.Position + new Vector2(-2, -HeadHeight * breathe);
        sprite.Draw(spriteBatch, head, Color.White, 0, new Vector2(sprite.Width / 2f, sprite.Height), 1, SpriteEffects.None, 0);
    }
}
