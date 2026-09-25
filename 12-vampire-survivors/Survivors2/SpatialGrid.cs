using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Survivors2;

// Spatial partitioning with a uniform grid. The world is cut into square cells, and each cell
// lists what's in it. A query only looks at the cells the circle touches, instead of at
// everything. Items are numbers (e.g. indexes into a list), each added at a position.
public sealed class SpatialGrid(float cellSize)
{
    private readonly Dictionary<Point, List<(int Item, Vector2 Position)>> _cells = [];

    public float CellSize => cellSize;

    // Empties every cell, but keeps the lists to fill again: no new lists every frame. Cells
    // the player has left far behind are dropped once in a while.
    public void Clear()
    {
        if (_cells.Count > 4096)
            _cells.Clear();
        foreach (var items in _cells.Values)
            items.Clear();
    }

    public void Add(int item, Vector2 position)
    {
        Point cell = CellOf(position);
        if (!_cells.TryGetValue(cell, out var items))
        {
            items = [];
            _cells[cell] = items;
        }
        items.Add((item, position));
    }

    // Fills results with every item within radius of center.
    public void Query(Vector2 center, float radius, List<int> results)
    {
        results.Clear();
        Point min = CellOf(center - new Vector2(radius));
        Point max = CellOf(center + new Vector2(radius));
        float radiusSquared = radius * radius;

        for (int y = min.Y; y <= max.Y; y++)
        {
            for (int x = min.X; x <= max.X; x++)
            {
                if (!_cells.TryGetValue(new Point(x, y), out var items))
                    continue;
                foreach (var (item, position) in items)
                {
                    if (Vector2.DistanceSquared(position, center) <= radiusSquared)
                        results.Add(item);
                }
            }
        }
    }

    public Point CellOf(Vector2 position) => new((int)MathF.Floor(position.X / cellSize), (int)MathF.Floor(position.Y / cellSize));
}
