using Birds3.Physics;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;

namespace Birds3.Entities;

public class Pig : Entity
{
    private readonly float _radius;

    public Pig(float radius, float health, int points, TextureRegion sprite) : base(sprite)
    {
        _radius = radius;
        Health = MaxHealth = health;
        Points = points;
    }

    protected override PhysicsBody CreateBody(PhysicsWorld world, Vector2 position, float rotation)
        => world.CreateCircle(position, _radius, Materials.Pig, this);
}
