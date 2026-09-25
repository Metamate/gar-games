using Microsoft.Xna.Framework;

namespace Pvz4;

// The lawn: 9 columns and 5 rows of cells. Converts between screen positions and cells.
public static class Lawn
{
    public const int Columns = 9;
    public const int Rows = 5;
    public const int CellWidth = 100;
    public const int CellHeight = 110;

    public static readonly Rectangle Bounds = new(260, 150, Columns * CellWidth, Rows * CellHeight);

    // Picking: which cell is this point in? Null when it's not on the lawn.
    public static Point? CellAt(Vector2 position)
    {
        if (!Bounds.Contains(position))
            return null;
        return new Point((int)(position.X - Bounds.X) / CellWidth, (int)(position.Y - Bounds.Y) / CellHeight);
    }

    public static Rectangle CellBounds(Point cell)
        => new(Bounds.X + cell.X * CellWidth, Bounds.Y + cell.Y * CellHeight, CellWidth, CellHeight);

    // Where things stand: the middle of the cell, a little above its bottom edge.
    public static Vector2 CellFeet(Point cell) => new(Bounds.X + (cell.X + 0.5f) * CellWidth, RowFeet(cell.Y));
    public static float RowFeet(int row) => Bounds.Y + (row + 1) * CellHeight - 12;
}
