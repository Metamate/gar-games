using Birds1.Physics;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;

namespace Birds1.Entities;

// A block of wood, stone or glass.
public class Block : Entity
{
    public Block(PhysicsWorld world, Vector2 center, Vector2 size, float rotation, PhysicsMaterial material, TextureRegion sprite) : base(sprite)
    {
        Body = world.CreateBox(center, size, rotation, material, this);
    }
}
