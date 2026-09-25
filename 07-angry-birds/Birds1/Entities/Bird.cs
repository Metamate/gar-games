using Birds1.Physics;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;

namespace Birds1.Entities;

public class Bird : Entity
{
    public const float Radius = 18;

    public Bird(PhysicsWorld world, Vector2 center, Vector2 velocity, TextureRegion sprite) : base(sprite)
    {
        Body = world.CreateCircle(center, Radius, Materials.Bird, this, isBullet: true);
        Body.Velocity = velocity;
    }
}
