using System;
using Microsoft.Xna.Framework;

namespace Survivors0;

// Spawns enemies on a ring just outside the screen, more and more of them as time goes on.
public sealed class Spawner(Random random)
{
    private const float Distance = 760;
    private float _debt;

    public float Time { get; private set; }

    // How many enemies to spawn this step.
    public int Update(float deltaSeconds)
    {
        Time += deltaSeconds;
        float perSecond = 4 + Time / 6;
        _debt += perSecond * deltaSeconds;
        int count = (int)_debt;
        _debt -= count;
        return count;
    }

    public Vector2 PointAround(Vector2 center)
    {
        float angle = random.NextSingle() * MathHelper.TwoPi;
        float distance = Distance + random.NextSingle() * 120;
        return center + new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * distance;
    }

    // Bats first; skeletons after half a minute, ogres after two.
    public int PickKind()
    {
        float roll = random.NextSingle();
        if (Time > 120 && roll < 0.15f) return 2;
        if (Time > 30 && roll < 0.5f) return 1;
        return 0;
    }
}
