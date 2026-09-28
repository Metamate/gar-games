using Birds1.Physics;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Birds1.Entities;

// Something in the game with a physics body and a sprite. The body owns the position: the
// entity reads it from the body every time it's drawn, and never stores its own.
public abstract class Entity(TextureRegion sprite)
{
    public PhysicsBody Body { get; protected set; }
    public TextureRegion Sprite { get; } = sprite;

    public void Draw(SpriteBatch spriteBatch)
    {
        var origin = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);
        Sprite.Draw(spriteBatch, Body.Position, Color.White, Body.Rotation, origin, 1, SpriteEffects.None, 0);
    }
}
