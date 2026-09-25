using Birds4.Physics;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Birds4.Entities;

// Something in the game with a physics body and a sprite. The body owns the position: the
// entity reads it from the body every time it's drawn, and never stores its own.
public abstract class Entity(TextureRegion sprite)
{
    public PhysicsBody Body { get; protected set; }
    public TextureRegion Sprite { get; } = sprite;

    public float Health { get; protected set; }
    public float MaxHealth { get; protected set; }
    public int Points { get; protected set; }
    public bool IsDestroyed => Health <= 0;

    // Hits do damage by speed. Only mark the entity destroyed here: its body is removed later,
    // after all hits are handled (see Game1).
    public virtual void TakeDamage(float damage) => Health -= damage;

    // Prototype: a configured entity that isn't in the world yet can be copied, and the copy
    // placed in the world. MemberwiseClone copies every field; that's a shallow copy, so the
    // copy shares the sprite with the prototype (fine: nobody changes it), but it must never
    // share a body. The prototype has none, and the copy gets its own in Spawn.
    public Entity Clone()
    {
        var copy = (Entity)MemberwiseClone();
        copy.Body = null;
        return copy;
    }

    public void Spawn(PhysicsWorld world, Vector2 position, float rotation) => Body = CreateBody(world, position, rotation);

    protected abstract PhysicsBody CreateBody(PhysicsWorld world, Vector2 position, float rotation);

    public void Draw(SpriteBatch spriteBatch)
    {
        var origin = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);
        Sprite.Draw(spriteBatch, Body.Position, Tint, Body.Rotation, origin, 1, SpriteEffects.None, 0);
    }

    // Darker as it takes damage.
    private Color Tint => Color.Lerp(new Color(90, 90, 90), Color.White, MathHelper.Clamp(Health / MaxHealth, 0, 1));
}
