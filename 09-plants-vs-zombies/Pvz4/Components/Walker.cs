using Microsoft.Xna.Framework;

namespace Pvz4.Components;

// Walks left, unless the entity is busy eating.
public class Walker(float speed) : Component
{
    public override void Update(float deltaSeconds)
    {
        if (Owner.Get<Eater>()?.IsEating == true)
            return;
        Owner.Position -= new Vector2(speed * deltaSeconds, 0);
    }
}
