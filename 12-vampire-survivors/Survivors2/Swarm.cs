using System;
using System.Collections.Generic;
using System.Linq;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors2;

// All the enemies, as a list of objects, and a spatial grid to find the ones near a point.
// The grid is rebuilt after every move.
public sealed class Swarm(TextureAtlas atlas)
{
    private const float FarAway = 1400;
    private readonly List<Enemy> _enemies = [];
    private readonly SpatialGrid _grid = new(64);
    private readonly List<int> _nearby = [];
    private readonly List<Enemy> _found = [];
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

        _grid.Clear();
        for (int i = 0; i < _enemies.Count; i++)
            _grid.Add(i, _enemies[i].Position);
    }

    // Enemies push each other apart. Each enemy only checks the enemies in nearby cells.
    public void Separate()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            Enemy enemy = _enemies[i];
            _grid.Query(enemy.Position, enemy.Kind.Radius + EnemyKind.MaxRadius, _nearby);
            foreach (int j in _nearby)
            {
                if (j > i)
                    Push(enemy, _enemies[j]);
            }
        }
    }

    // The nearest enemy within range, or null.
    public Enemy Nearest(Vector2 point, float range)
    {
        _grid.Query(point, range, _nearby);
        Enemy nearest = null;
        float best = float.MaxValue;
        foreach (int i in _nearby)
        {
            float distance = Vector2.DistanceSquared(_enemies[i].Position, point);
            if (distance < best)
            {
                best = distance;
                nearest = _enemies[i];
            }
        }
        return nearest;
    }

    // The enemies touching a circle. The same list is reused, so it's only valid until the
    // next call: no new list every time.
    public IReadOnlyList<Enemy> Within(Vector2 point, float radius)
    {
        _grid.Query(point, radius + EnemyKind.MaxRadius, _nearby);
        _found.Clear();
        foreach (int i in _nearby)
        {
            Enemy enemy = _enemies[i];
            if (Vector2.Distance(enemy.Position, point) <= radius + enemy.Kind.Radius)
                _found.Add(enemy);
        }
        return _found;
    }

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
