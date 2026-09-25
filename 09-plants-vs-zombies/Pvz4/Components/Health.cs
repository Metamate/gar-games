namespace Pvz4.Components;

public class Health(float max) : Component
{
    private float _flashTime;

    public float Current { get; private set; } = max;
    public bool IsFlashing => _flashTime > 0;

    // Armour takes the damage first, if the entity has any.
    public void Damage(float amount)
    {
        Armour armour = Owner.Get<Armour>();
        if (armour != null)
            amount = armour.Absorb(amount);

        Current -= amount;
        _flashTime = 0.1f;
        if (Current <= 0)
            Owner.Remove();
    }

    public override void Update(float deltaSeconds) => _flashTime -= deltaSeconds;
}
