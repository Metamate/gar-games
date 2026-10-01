using Microsoft.Xna.Framework;

namespace Pvz2.Components;

// Walks left, unless the entity is busy fighting.
public class Walker(float speed) : Component
{
    public override void Update(float deltaSeconds)
    {
        if (Owner.Get<Attacker>()?.IsAttacking == true)
            return;
        Owner.Position -= new Vector2(speed * deltaSeconds, 0);
    }
}
