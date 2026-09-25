using Microsoft.Xna.Framework.Graphics;

namespace Pvz2.Components;

// One part of an entity: one thing it has, or one thing it can do.
public abstract class Component
{
    public Entity Owner { get; internal set; }
    protected World World => Owner.World;

    public virtual void Update(float deltaSeconds) { }
    public virtual void Draw(SpriteBatch spriteBatch) { }
}
