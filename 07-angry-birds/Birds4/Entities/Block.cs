using Birds4.Physics;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;

namespace Birds4.Entities;

// A block of wood, stone or glass.
public class Block : Entity
{
    private readonly PhysicsMaterial _material;
    private readonly Vector2 _size;

    public Block(PhysicsMaterial material, Vector2 size, float health, int points, TextureRegion sprite) : base(sprite)
    {
        _material = material;
        _size = size;
        Health = MaxHealth = health;
        Points = points;
    }

    protected override PhysicsBody CreateBody(PhysicsWorld world, Vector2 position, float rotation)
        => world.CreateBox(position, _size, rotation, _material, this);
}
