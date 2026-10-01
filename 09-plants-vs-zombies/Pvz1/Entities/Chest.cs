using GARCore.Graphics;
using Microsoft.Xna.Framework;

namespace Pvz1.Entities;

// Makes a coin every few seconds.
public class Chest(Point cell, TextureAtlas atlas) : Defender(cell, atlas.GetRegion("chest"), 6)
{
    private const float Interval = 9;
    private float _timer = Interval / 2;

    public override void Update(float deltaSeconds, World world)
    {
        base.Update(deltaSeconds, world);
        _timer -= deltaSeconds;
        if (_timer <= 0)
        {
            world.Add(new Coin(atlas, Position + new Vector2(0, -70), 25));
            _timer = Interval;
        }
    }
}
