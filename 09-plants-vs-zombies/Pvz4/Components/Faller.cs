using Microsoft.Xna.Framework;

namespace Pvz4.Components;

// Falls down to a point, then stays there: sun from the sky.
public class Faller(float targetY, float speed) : Component
{
    public override void Update(float deltaSeconds)
    {
        if (Owner.Position.Y < targetY)
            Owner.Position += new Vector2(0, speed * deltaSeconds);
    }
}
