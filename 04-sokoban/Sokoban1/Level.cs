using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Sokoban1;

public enum MoveResult { Blocked, Walked, Pushed }

// The state and the rules of one Sokoban level. It knows nothing about drawing or input,
// so the rules can be read, changed and tested on their own.
public class Level
{
    private readonly bool[,] _walls;
    private readonly bool[,] _floor;
    private readonly HashSet<Point> _goals = [];
    private readonly HashSet<Point> _boxes = [];

    private Level(int width, int height)
    {
        Width = width;
        Height = height;
        _walls = new bool[width, height];
        _floor = new bool[width, height];
    }

    public int Width { get; }
    public int Height { get; }
    public Point Player { get; private set; }
    public IReadOnlyCollection<Point> Boxes => _boxes;

    // Solved when every box stands on a goal.
    public bool IsSolved => _boxes.All(_goals.Contains);

    public bool IsWall(Point cell) => IsInside(cell) && _walls[cell.X, cell.Y];
    public bool IsFloor(Point cell) => IsInside(cell) && _floor[cell.X, cell.Y];
    public bool IsGoal(Point cell) => _goals.Contains(cell);
    public bool HasBox(Point cell) => _boxes.Contains(cell);

    // The player can walk into a free cell, or push a box if the cell behind it is free.
    public bool CanMove(Point direction)
    {
        Point next = Player + direction;
        if (IsWall(next))
        {
            return false;
        }

        if (HasBox(next))
        {
            Point behind = next + direction;
            return !IsWall(behind) && !HasBox(behind);
        }

        return true;
    }

    public MoveResult Move(Point direction)
    {
        if (!CanMove(direction))
        {
            return MoveResult.Blocked;
        }

        Player += direction;

        if (HasBox(Player))
        {
            _boxes.Remove(Player);
            _boxes.Add(Player + direction);
            return MoveResult.Pushed;
        }

        return MoveResult.Walked;
    }

    // Reads a level in the classic Sokoban text format, one character per cell:
    //   # wall   $ box   . goal   * box on a goal   @ player   + player on a goal
    public static Level Parse(string text)
    {
        string[] lines = text.Replace("\r", "").Trim('\n').Split('\n');
        var level = new Level(lines.Max(line => line.Length), lines.Length);

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                Point cell = new(x, y);
                switch (lines[y][x])
                {
                    case '#': level._walls[x, y] = true; break;
                    case '$': level._boxes.Add(cell); break;
                    case '.': level._goals.Add(cell); break;
                    case '*': level._boxes.Add(cell); level._goals.Add(cell); break;
                    case '@': level.Player = cell; break;
                    case '+': level.Player = cell; level._goals.Add(cell); break;
                }
            }
        }

        level.FindFloor();
        return level;
    }

    // The floor is every cell the player can reach without walking through a wall.
    // Spaces outside the walls are not floor, so they aren't drawn.
    private void FindFloor()
    {
        var todo = new Stack<Point>([Player]);
        while (todo.Count > 0)
        {
            Point cell = todo.Pop();
            if (!IsInside(cell) || _walls[cell.X, cell.Y] || _floor[cell.X, cell.Y])
            {
                continue;
            }

            _floor[cell.X, cell.Y] = true;
            todo.Push(cell + Direction.Up);
            todo.Push(cell + Direction.Down);
            todo.Push(cell + Direction.Left);
            todo.Push(cell + Direction.Right);
        }
    }

    private bool IsInside(Point cell) => cell.X >= 0 && cell.Y >= 0 && cell.X < Width && cell.Y < Height;
}
