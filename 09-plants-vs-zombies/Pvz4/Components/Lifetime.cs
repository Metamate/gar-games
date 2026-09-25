namespace Pvz4.Components;

// Removes the entity after a while: for effects like the explosion.
public class Lifetime(float seconds) : Component
{
    private float _age;

    public override void Update(float deltaSeconds)
    {
        _age += deltaSeconds;
        if (_age > seconds)
            Owner.Remove();
    }
}
