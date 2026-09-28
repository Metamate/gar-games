using Microsoft.Xna.Framework;

namespace Pacman2;

// Directions on the grid, as one-tile steps. All lists them in the order ghosts prefer
// when two choices are equally good: up, left, down, right.
public static class Direction
{
    public static readonly Point None = Point.Zero;
    public static readonly Point Up = new(0, -1);
    public static readonly Point Down = new(0, 1);
    public static readonly Point Left = new(-1, 0);
    public static readonly Point Right = new(1, 0);

    public static readonly Point[] All = [Up, Left, Down, Right];

    public static Point Opposite(Point direction) => new(-direction.X, -direction.Y);
}
