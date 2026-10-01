using Microsoft.Xna.Framework;

namespace Pvz3.Components;

// Flies along its row, and damages the first goblin it reaches.
public class Projectile(float speed, float damage) : Component
{
    public override void Update(float deltaSeconds)
    {
        Owner.Position += new Vector2(speed * deltaSeconds, 0);

        Entity goblin = World.FirstGoblinAhead(Owner.Row, Owner.Position.X - 20);
        if (goblin != null && goblin.Position.X - Owner.Position.X < 20)
        {
            goblin.Get<Health>()?.Damage(damage);
            Owner.Remove();
        }
        else if (Owner.Position.X > 1300)
        {
            Owner.Remove();
        }
    }
}
