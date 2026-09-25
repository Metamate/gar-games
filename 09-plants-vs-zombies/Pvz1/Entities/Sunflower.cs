using GMDCore.Graphics;
using Microsoft.Xna.Framework;

namespace Pvz1.Entities;

// Makes a sun every few seconds.
public class Sunflower(Point cell, TextureAtlas atlas) : Plant(cell, atlas.GetRegion("sunflower"), 6)
{
    private const float Interval = 12;
    private float _timer = Interval / 2;

    public override void Update(float deltaSeconds, World world)
    {
        base.Update(deltaSeconds, world);
        _timer -= deltaSeconds;
        if (_timer <= 0)
        {
            world.Add(new Sun(atlas, Position + new Vector2(0, -50), 25));
            _timer = Interval;
        }
    }
}
