using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Pacman0;

public enum Dot { None, Small, Power }

// The maze: walls, the ghost house, the dots, and where everyone starts. Like Sokoban's Level,
// it's read from text and knows nothing about drawing. One character per tile:
//   # wall   . dot   o power pellet   - ghost house door   H inside the ghost house
//   P Pac-Man   b Blinky (outside the house)   p i c Pinky, Inky, Clyde (inside the house)
// A row that is open at both ends is a tunnel: leaving on one side enters on the other.
public class Maze
{
    public const int TileSize = 20;

    private readonly char[,] _tiles;
    private readonly HashSet<Point> _startDots = [];
    private readonly HashSet<Point> _startPellets = [];
    private readonly HashSet<Point> _dots = [];
    private readonly HashSet<Point> _pellets = [];
    private readonly Dictionary<char, Point> _starts = [];

    private Maze(int width, int height)
    {
        Width = width;
        Height = height;
        _tiles = new char[width, height];
    }

    public int Width { get; }
    public int Height { get; }
    public IReadOnlyCollection<Point> Dots => _dots;
    public IReadOnlyCollection<Point> Pellets => _pellets;
    public int DotsLeft => _dots.Count + _pellets.Count;

    public Point PacManStart => _starts['P'];
    public Point StartOf(char ghost) => _starts[ghost];

    // Where eaten ghosts go: the tile just outside the door, then the middle of the house.
    public Point DoorOutside { get; private set; }
    public Point HouseCenter { get; private set; }

    public bool IsWall(Point cell) => TileAt(cell) == '#';
    public bool IsDoor(Point cell) => TileAt(cell) == '-';
    public bool IsHouse(Point cell) => TileAt(cell) == 'H';
    public bool IsInHouse(Point cell) => IsHouse(cell) || IsDoor(cell);

    // Pac-Man can't enter the ghost house. Ghosts can, but only some pass the door.
    public bool BlocksPacMan(Point cell) => IsWall(cell) || IsInHouse(cell);
    public bool BlocksGhost(Point cell, bool canUseDoor) => IsWall(cell) || (IsDoor(cell) && !canUseDoor);

    // Through the tunnel: a column left of the maze is the rightmost column, and so on.
    public Point Wrap(Point cell) => new(((cell.X % Width) + Width) % Width, cell.Y);

    public static Vector2 Center(Point cell) => new((cell.X + 0.5f) * TileSize, (cell.Y + 0.5f) * TileSize);
    public static Point TileAt(Vector2 position) => new((int)MathF.Floor(position.X / TileSize), (int)MathF.Floor(position.Y / TileSize));

    public Dot Eat(Point cell)
    {
        if (_dots.Remove(cell)) return Dot.Small;
        if (_pellets.Remove(cell)) return Dot.Power;
        return Dot.None;
    }

    public void ResetDots()
    {
        _dots.Clear();
        _dots.UnionWith(_startDots);
        _pellets.Clear();
        _pellets.UnionWith(_startPellets);
    }

    private char TileAt(Point cell)
    {
        if (cell.Y < 0 || cell.Y >= Height)
            return '#';
        Point wrapped = Wrap(cell);
        return _tiles[wrapped.X, wrapped.Y];
    }

    public static Maze Parse(string text)
    {
        string[] lines = text.Replace("\r", "").Trim('\n').Split('\n');
        var maze = new Maze(lines.Max(line => line.Length), lines.Length);

        for (int y = 0; y < maze.Height; y++)
        {
            for (int x = 0; x < maze.Width; x++)
            {
                char c = x < lines[y].Length ? lines[y][x] : ' ';
                Point cell = new(x, y);
                maze._tiles[x, y] = ' ';

                switch (c)
                {
                    case '#': case '-': case 'H': maze._tiles[x, y] = c; break;
                    case '.': maze._startDots.Add(cell); break;
                    case 'o': maze._startPellets.Add(cell); break;
                    case 'P': case 'b': maze._starts[c] = cell; break;
                    case 'p': case 'i': case 'c': maze._starts[c] = cell; maze._tiles[x, y] = 'H'; break;
                }
            }
        }

        maze.FindHouse();
        maze.ResetDots();
        return maze;
    }

    private void FindHouse()
    {
        var door = new List<Point>();
        var house = new List<Point>();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (_tiles[x, y] == '-') door.Add(new Point(x, y));
                if (_tiles[x, y] == 'H') house.Add(new Point(x, y));
            }
        }

        if (door.Count == 0)
            return;

        Point first = door.OrderBy(cell => cell.X).First();
        DoorOutside = first + Direction.Up;
        var column = house.Where(cell => cell.X == first.X).OrderBy(cell => cell.Y).ToList();
        HouseCenter = column.Count > 0 ? column[column.Count / 2] : first + Direction.Down;
    }
}
