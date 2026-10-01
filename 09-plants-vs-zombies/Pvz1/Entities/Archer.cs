using GARCore.Graphics;
using Microsoft.Xna.Framework;

namespace Pvz1.Entities;

// Shoots an arrow when there's a goblin ahead in its row.
public class Archer(Point cell, TextureAtlas atlas) : Defender(cell, atlas.GetRegion("archer"), 6)
{
    private const float Interval = 1.4f;
    private float _cooldown;

    public override void Update(float deltaSeconds, World world)
    {
        base.Update(deltaSeconds, world);
        _cooldown -= deltaSeconds;
        if (_cooldown <= 0 && world.FirstGoblinAhead(Row, Position.X) != null)
        {
            world.Add(new Arrow(atlas, Position + new Vector2(30, -40), Row, 1));
            _cooldown = Interval;
        }
    }
}
