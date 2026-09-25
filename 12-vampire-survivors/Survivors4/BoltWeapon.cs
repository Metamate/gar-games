using System;
using System.Collections.Generic;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors4;

// Fires magic bolts at the nearest enemy. There are only ever a few dozen bolts, so they stay
// simple objects.
public sealed class BoltWeapon(TextureRegion sprite)
{
    private const float Speed = 520;
    private const float Range = 550;
    private const float Lifetime = 1.4f;
    private const float BoltRadius = 8;

    private sealed class Bolt
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Age;
    }

    private readonly List<Bolt> _bolts = [];
    private float _cooldown;

    public float Interval { get; set; } = 0.35f;
    public float Damage { get; set; } = 2;
    public int Count { get; set; } = 1;

    public void Update(float deltaSeconds, Swarm swarm, Vector2 origin)
    {
        _cooldown -= deltaSeconds;
        if (_cooldown <= 0)
        {
            int target = swarm.Nearest(origin, Range);
            if (target >= 0)
            {
                _cooldown = Interval;
                Fire(origin, swarm.Enemies.Position[target] - origin);
            }
        }

        foreach (Bolt bolt in _bolts)
        {
            bolt.Position += bolt.Velocity * deltaSeconds;
            bolt.Age += deltaSeconds;
            foreach (int i in swarm.Within(bolt.Position, BoltRadius))
            {
                swarm.Enemies.Health[i] -= Damage;
                bolt.Age = Lifetime;
                break;
            }
        }
        _bolts.RemoveAll(bolt => bolt.Age >= Lifetime);
    }

    // A volley: one bolt at the target, and the others fanned out around it.
    private void Fire(Vector2 origin, Vector2 direction)
    {
        float angle = MathF.Atan2(direction.Y, direction.X);
        for (int i = 0; i < Count; i++)
        {
            float spread = (i - (Count - 1) / 2f) * 0.18f;
            var velocity = new Vector2(MathF.Cos(angle + spread), MathF.Sin(angle + spread)) * Speed;
            _bolts.Add(new Bolt { Position = origin, Velocity = velocity });
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Bolt bolt in _bolts)
            sprite.Draw(spriteBatch, bolt.Position, Color.White, 0, new Vector2(sprite.Width / 2f, sprite.Height / 2f), 1, SpriteEffects.None, 0);
    }
}
