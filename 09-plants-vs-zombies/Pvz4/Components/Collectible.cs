using Microsoft.Xna.Framework;

namespace Pvz4.Components;

// Click it to collect it (sun), before it disappears.
public class Collectible(int value, float lifetime = 10) : Component
{
    private float _age;

    public int Value { get; } = value;

    public bool Contains(Vector2 point) => Vector2.Distance(point, Owner.Position) < 36;

    public override void Update(float deltaSeconds)
    {
        _age += deltaSeconds;
        if (_age > lifetime)
            Owner.Remove();
    }
}
