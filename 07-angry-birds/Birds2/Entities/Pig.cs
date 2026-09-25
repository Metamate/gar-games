using Birds2.Physics;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;

namespace Birds2.Entities;

public class Pig : Entity
{
    public Pig(PhysicsWorld world, Vector2 center, float radius, float health, int points, TextureRegion sprite) : base(sprite)
    {
        Body = world.CreateCircle(center, radius, Materials.Pig, this);
        Health = MaxHealth = health;
        Points = points;
    }
}
