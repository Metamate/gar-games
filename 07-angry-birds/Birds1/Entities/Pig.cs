using Birds1.Physics;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;

namespace Birds1.Entities;

public class Pig : Entity
{
    public Pig(PhysicsWorld world, Vector2 center, float radius, TextureRegion sprite) : base(sprite)
    {
        Body = world.CreateCircle(center, radius, Materials.Pig, this);
    }
}
