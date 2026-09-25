using Microsoft.Xna.Framework;

namespace Pvz2.Components;

// Flies along its row, and damages the first zombie it reaches.
public class Projectile(float speed, float damage) : Component
{
    public override void Update(float deltaSeconds)
    {
        Owner.Position += new Vector2(speed * deltaSeconds, 0);

        Entity zombie = World.FirstZombieAhead(Owner.Row, Owner.Position.X - 20);
        if (zombie != null && zombie.Position.X - Owner.Position.X < 20)
        {
            zombie.Get<Health>()?.Damage(damage);
            Owner.Remove();
        }
        else if (Owner.Position.X > 1300)
        {
            Owner.Remove();
        }
    }
}
