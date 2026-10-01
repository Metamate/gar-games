using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz2.Components;

// A shield: extra health, held in front until it breaks.
public class Armour(float health, TextureRegion sprite) : Component
{
    // Where the shield is held: in front of the goblin, at its middle.
    private static readonly Vector2 Hold = new(-25, -15);

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

        float rise = Owner.Get<SpriteRenderer>()?.Rise ?? 0;
        Vector2 hold = Owner.Position + Hold - new Vector2(0, rise);
        sprite.Draw(spriteBatch, hold, Color.White, 0, new Vector2(sprite.Width / 2f, sprite.Height), 1, SpriteEffects.None, 0);
    }
}
