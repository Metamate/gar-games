namespace Pvz3.Components;

// Hits the defender in front of it, if there is one.
public class Attacker(float damagePerSecond) : Component
{
    public bool IsAttacking { get; private set; }

    public override void Update(float deltaSeconds)
    {
        Entity defender = World.DefenderAt(Owner.Row, Owner.Position.X - 40);
        IsAttacking = defender != null;
        defender?.Get<Health>()?.Damage(damagePerSecond * deltaSeconds);
    }
}
