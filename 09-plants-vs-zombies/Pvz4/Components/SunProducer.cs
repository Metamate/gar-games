using Microsoft.Xna.Framework;

namespace Pvz4.Components;

// Makes a sun every few seconds.
public class SunProducer(float interval, int amount) : Component
{
    private float _timer = interval / 2;

    public override void Update(float deltaSeconds)
    {
        _timer -= deltaSeconds;
        if (_timer > 0)
            return;

        _timer = interval;
        World.Add(World.Recipes.Sun(World, Owner.Position + new Vector2(0, -50), amount));
    }
}
