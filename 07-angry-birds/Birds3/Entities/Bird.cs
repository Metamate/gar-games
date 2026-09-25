using Birds3.Physics;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;

namespace Birds3.Entities;

public class Bird : Entity
{
    public const float Radius = 18;

    public Bird(TextureRegion sprite) : base(sprite)
    {
        Health = MaxHealth = 1;
    }

    protected override PhysicsBody CreateBody(PhysicsWorld world, Vector2 position, float rotation)
        => world.CreateCircle(position, Radius, Materials.Bird, this, isBullet: true);

    // Birds are tough: they don't break.
    public override void TakeDamage(float damage) { }
}
