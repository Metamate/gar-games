using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Survivors4;

// The spatial grid again, laid out for the machine. Instead of a dictionary of lists, a fixed
// area around the player is cut into Columns x Rows cells, and the grid is rebuilt every step
// with a counting sort: count the items in each cell, turn the counts into start positions,
// then copy each item into its cell's range. A cell's items end up next to each other in
// memory, and nothing is allocated. Items outside the area go into the nearest edge cell.
public sealed class FlatGrid(float cellSize, int columns, int rows)
{
    private readonly int[] _cellStart = new int[columns * rows + 1];
    private readonly int[] _next = new int[columns * rows];
    private int[] _cellOf = [];
    private Vector2 _origin;

    public int Columns => columns;
    public int Rows => rows;
    public int Count { get; private set; }

    // Cell c holds Items[CellStart[c]] up to Items[CellStart[c + 1] - 1], and Positions holds
    // their positions in the same places.
    public int[] CellStart => _cellStart;
    public int[] Items { get; private set; } = [];
    public Vector2[] Positions { get; private set; } = [];

    public void Build(Vector2 center, Vector2[] positions, int count)
    {
        if (Items.Length < count)
        {
            int size = Math.Max(count, Items.Length * 2);
            Items = new int[size];
            Positions = new Vector2[size];
            _cellOf = new int[size];
        }

        Count = count;
        _origin = center - new Vector2(columns, rows) * cellSize / 2;
        Array.Clear(_cellStart);

        // 1. Count the items in each cell.
        for (int i = 0; i < count; i++)
        {
            int cell = CellIndex(positions[i]);
            _cellOf[i] = cell;
            _cellStart[cell + 1]++;
        }

        // 2. Each cell starts where the one before it ends.
        for (int c = 0; c < columns * rows; c++)
            _cellStart[c + 1] += _cellStart[c];

        // 3. Copy each item into the next free place in its cell.
        Array.Copy(_cellStart, _next, columns * rows);
        for (int i = 0; i < count; i++)
        {
            int slot = _next[_cellOf[i]]++;
            Items[slot] = i;
            Positions[slot] = positions[i];
        }
    }

    // Fills results with every item within radius of center.
    public void Query(Vector2 center, float radius, List<int> results)
    {
        results.Clear();
        var (x0, y0) = CellOf(center - new Vector2(radius));
        var (x1, y1) = CellOf(center + new Vector2(radius));
        float radiusSquared = radius * radius;

        for (int y = y0; y <= y1; y++)
        {
            for (int x = x0; x <= x1; x++)
            {
                int cell = y * columns + x;
                for (int k = _cellStart[cell]; k < _cellStart[cell + 1]; k++)
                {
                    if (Vector2.DistanceSquared(Positions[k], center) <= radiusSquared)
                        results.Add(Items[k]);
                }
            }
        }
    }

    public (int X, int Y) CellOf(Vector2 position)
        => (Math.Clamp((int)MathF.Floor((position.X - _origin.X) / cellSize), 0, columns - 1),
            Math.Clamp((int)MathF.Floor((position.Y - _origin.Y) / cellSize), 0, rows - 1));

    private int CellIndex(Vector2 position)
    {
        var (x, y) = CellOf(position);
        return y * columns + x;
    }
}
