using System;
using System.Collections.Generic;
using System.Linq;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors1;

// All the enemies, as a list of objects. Every question about enemies near a point looks at
// every enemy.
public sealed class Swarm(TextureAtlas atlas)
{
    private const float FarAway = 1400;
    private readonly List<Enemy> _enemies = [];
    private readonly Dictionary<EnemyKind, TextureRegion> _sprites = EnemyKind.All.ToDictionary(kind => kind, kind => atlas.GetRegion(kind.Sprite));

    public int Count => _enemies.Count;

    public void Spawn(EnemyKind kind, Vector2 position) => _enemies.Add(new Enemy(kind, position));

    // Everyone walks straight at the target. Enemies left far behind reappear on the other side.
    public void Move(float deltaSeconds, Vector2 target)
    {
        foreach (Enemy enemy in _enemies)
        {
            Vector2 toTarget = target - enemy.Position;
            float distance = toTarget.Length();
            if (distance > FarAway)
                enemy.Position = target + toTarget * 0.9f;
            else if (distance > 1)
                enemy.Position += toTarget / distance * enemy.Kind.Speed * deltaSeconds;
        }
    }

    // Enemies push each other apart, so they crowd instead of stacking up. Every enemy is
    // checked against every other: for n enemies, n × n / 2 checks.
    public void Separate()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            for (int j = i + 1; j < _enemies.Count; j++)
                Push(_enemies[i], _enemies[j]);
        }
    }

    // The nearest enemy within range, or null.
    public Enemy Nearest(Vector2 point, float range)
        => _enemies.Where(enemy => Vector2.Distance(enemy.Position, point) <= range)
                   .OrderBy(enemy => Vector2.DistanceSquared(enemy.Position, point))
                   .FirstOrDefault();

    // The enemies touching a circle. A new list, every time.
    public List<Enemy> Within(Vector2 point, float radius)
        => _enemies.Where(enemy => Vector2.Distance(enemy.Position, point) <= radius + enemy.Kind.Radius).ToList();

    public void RemoveDead() => _enemies.RemoveAll(enemy => enemy.IsDead);

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Enemy enemy in _enemies)
        {
            TextureRegion sprite = _sprites[enemy.Kind];
            sprite.Draw(spriteBatch, enemy.Position, Color.White, 0, new Vector2(sprite.Width / 2f, sprite.Height / 2f), 1, SpriteEffects.None, 0);
        }
    }

    private static void Push(Enemy a, Enemy b)
    {
        Vector2 between = a.Position - b.Position;
        float minimum = a.Kind.Radius + b.Kind.Radius;
        float distanceSquared = between.LengthSquared();
        if (distanceSquared >= minimum * minimum || distanceSquared < 0.0001f)
            return;

        float distance = MathF.Sqrt(distanceSquared);
        Vector2 push = between / distance * (minimum - distance) * 0.5f;
        a.Position += push;
        b.Position -= push;
    }
}
