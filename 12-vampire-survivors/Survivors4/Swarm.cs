using System;
using System.Collections.Generic;
using System.Linq;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors4;

// All the enemies, as arrays (see Enemies), and a flat grid over the area around the player
// (see FlatGrid). Every method is a loop over arrays; there are no enemy objects to follow.
public sealed class Swarm(TextureAtlas atlas, int capacity)
{
    private const float FarAway = 1400;
    private readonly Enemies _enemies = new(capacity);

    // Cells as big as the biggest overlap (two ogres), so overlapping enemies are always in
    // the same cell or neighbouring cells. 64 x 64 cells cover 3072 pixels around the player.
    private readonly FlatGrid _grid = new(2 * EnemyKind.MaxRadius, 64, 64);
    private readonly List<int> _nearby = [];
    private readonly List<int> _found = [];
    private readonly TextureRegion[] _sprites = EnemyKind.All.Select(kind => atlas.GetRegion(kind.Sprite)).ToArray();

    public int Count => _enemies.Count;
    public Enemies Enemies => _enemies;

    public void Spawn(int kind, Vector2 position) => _enemies.Add((byte)kind, position);

    // Everyone walks straight at the target. Enemies left far behind reappear on the other side.
    public void Move(float deltaSeconds, Vector2 target)
    {
        Vector2[] positions = _enemies.Position;
        float[] speeds = _enemies.Speed;
        for (int i = 0; i < _enemies.Count; i++)
        {
            Vector2 toTarget = target - positions[i];
            float distance = toTarget.Length();
            if (distance > FarAway)
                positions[i] = target + toTarget * 0.9f;
            else if (distance > 1)
                positions[i] += toTarget / distance * speeds[i] * deltaSeconds;
        }

        _grid.Build(target, positions, _enemies.Count);
    }

    // Enemies push each other apart. The grid holds the positions sorted by cell, so this works
    // on that copy, cell by cell: each enemy against the rest of its cell and the neighbouring
    // cells to the right and below (so each pair is checked once). No queries, no lists. At the
    // end, the new positions go back to the enemies.
    public void Separate()
    {
        int[] start = _grid.CellStart;
        for (int y = 0; y < _grid.Rows; y++)
        {
            for (int x = 0; x < _grid.Columns; x++)
            {
                int cell = y * _grid.Columns + x;
                for (int a = start[cell]; a < start[cell + 1]; a++)
                {
                    for (int b = a + 1; b < start[cell + 1]; b++)
                        Push(a, b);
                    PushAll(a, x + 1, y);
                    PushAll(a, x - 1, y + 1);
                    PushAll(a, x, y + 1);
                    PushAll(a, x + 1, y + 1);
                }
            }
        }

        int[] items = _grid.Items;
        Vector2[] sorted = _grid.Positions;
        for (int k = 0; k < _grid.Count; k++)
            _enemies.Position[items[k]] = sorted[k];
    }

    private void PushAll(int a, int x, int y)
    {
        if (x < 0 || x >= _grid.Columns || y >= _grid.Rows)
            return;
        int cell = y * _grid.Columns + x;
        for (int b = _grid.CellStart[cell]; b < _grid.CellStart[cell + 1]; b++)
            Push(a, b);
    }

    private void Push(int a, int b)
    {
        Vector2[] sorted = _grid.Positions;
        Vector2 between = sorted[a] - sorted[b];
        float minimum = _enemies.Radius[_grid.Items[a]] + _enemies.Radius[_grid.Items[b]];
        float distanceSquared = between.LengthSquared();
        if (distanceSquared >= minimum * minimum || distanceSquared < 0.0001f)
            return;

        float distance = MathF.Sqrt(distanceSquared);
        Vector2 push = between / distance * (minimum - distance) * 0.5f;
        sorted[a] += push;
        sorted[b] -= push;
    }

    // The index of the nearest enemy within range, or -1.
    public int Nearest(Vector2 point, float range)
    {
        _grid.Query(point, range, _nearby);
        int nearest = -1;
        float best = float.MaxValue;
        foreach (int i in _nearby)
        {
            float distance = Vector2.DistanceSquared(_enemies.Position[i], point);
            if (distance < best)
            {
                best = distance;
                nearest = i;
            }
        }
        return nearest;
    }

    // The indexes of the enemies touching a circle. The same list is reused, so it's only
    // valid until the next call.
    public List<int> Within(Vector2 point, float radius)
    {
        _grid.Query(point, radius + EnemyKind.MaxRadius, _nearby);
        _found.Clear();
        foreach (int i in _nearby)
        {
            if (_enemies.Health[i] > 0 && Vector2.Distance(_enemies.Position[i], point) <= radius + _enemies.Radius[i])
                _found.Add(i);
        }
        return _found;
    }

    // Backwards, so that the enemy moved into a hole has already been checked. Each dead
    // enemy leaves a gem. Returns how many died.
    public int RemoveDead(Gems gems)
    {
        int killed = 0;
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            if (_enemies.Health[i] > 0)
                continue;
            gems.Add(_enemies.Position[i], EnemyKind.All[_enemies.Kind[i]].Experience);
            _enemies.RemoveAt(i);
            killed++;
        }
        return killed;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            TextureRegion sprite = _sprites[_enemies.Kind[i]];
            sprite.Draw(spriteBatch, _enemies.Position[i], Color.White, 0, new Vector2(sprite.Width / 2f, sprite.Height / 2f), 1, SpriteEffects.None, 0);
        }
    }
}
