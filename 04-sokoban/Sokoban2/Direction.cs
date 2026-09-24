using Microsoft.Xna.Framework;

namespace Sokoban2;

// Directions on the grid, as one-cell steps.
public static class Direction
{
    public static readonly Point Up = new(0, -1);
    public static readonly Point Down = new(0, 1);
    public static readonly Point Left = new(-1, 0);
    public static readonly Point Right = new(1, 0);
}
