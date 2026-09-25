using System;
using System.Collections.Generic;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors2;

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

    public void Update(float deltaSeconds, Swarm swarm, Vector2 origin)
    {
        _cooldown -= deltaSeconds;
        if (_cooldown <= 0 && swarm.Nearest(origin, Range) is Enemy target)
        {
            _cooldown = Interval;
            Fire(origin, target.Position - origin);
        }

        foreach (Bolt bolt in _bolts)
        {
            bolt.Position += bolt.Velocity * deltaSeconds;
            bolt.Age += deltaSeconds;
            foreach (Enemy enemy in swarm.Within(bolt.Position, BoltRadius))
            {
                if (enemy.IsDead)
                    continue;
                enemy.Health -= Damage;
                bolt.Age = Lifetime;
                break;
            }
        }
        _bolts.RemoveAll(bolt => bolt.Age >= Lifetime);
    }

    private void Fire(Vector2 origin, Vector2 direction)
    {
        direction.Normalize();
        _bolts.Add(new Bolt { Position = origin, Velocity = direction * Speed });
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Bolt bolt in _bolts)
            sprite.Draw(spriteBatch, bolt.Position, Color.White, 0, new Vector2(sprite.Width / 2f, sprite.Height / 2f), 1, SpriteEffects.None, 0);
    }
}
