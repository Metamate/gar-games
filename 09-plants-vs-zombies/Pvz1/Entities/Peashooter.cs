using GMDCore.Graphics;
using Microsoft.Xna.Framework;

namespace Pvz1.Entities;

// Shoots a pea when there's a zombie ahead in its row.
public class Peashooter(Point cell, TextureAtlas atlas) : Plant(cell, atlas.GetRegion("peashooter"), 6)
{
    private const float Interval = 1.4f;
    private float _cooldown;

    public override void Update(float deltaSeconds, World world)
    {
        base.Update(deltaSeconds, world);
        _cooldown -= deltaSeconds;
        if (_cooldown <= 0 && world.FirstZombieAhead(Row, Position.X) != null)
        {
            world.Add(new Pea(atlas, Position + new Vector2(34, -62), Row, 1));
            _cooldown = Interval;
        }
    }
}
