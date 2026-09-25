namespace Pvz2.Components;

// Eats the plant in front of it, if there is one.
public class Eater(float damagePerSecond) : Component
{
    public bool IsEating { get; private set; }

    public override void Update(float deltaSeconds)
    {
        Entity plant = World.PlantAt(Owner.Row, Owner.Position.X - 30);
        IsEating = plant != null;
        plant?.Get<Health>()?.Damage(damagePerSecond * deltaSeconds);
    }
}
