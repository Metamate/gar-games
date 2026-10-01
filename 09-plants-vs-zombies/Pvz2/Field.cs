using Microsoft.Xna.Framework;

namespace Pvz2;

// The field: 14 columns and 5 rows of cells, between the castle wall and the edge of the screen.
// Converts between screen positions and cells.
public static class Field
{
    public const int Columns = 14;
    public const int Rows = 5;
    public const int CellWidth = 80;
    public const int CellHeight = 80;

    public static readonly Rectangle Bounds = new(80, 160, Columns * CellWidth, Rows * CellHeight);

    // Picking: which cell is this point in? Null when it's not on the field.
    public static Point? CellAt(Vector2 position)
    {
        if (!Bounds.Contains(position))
            return null;
        return new Point((int)(position.X - Bounds.X) / CellWidth, (int)(position.Y - Bounds.Y) / CellHeight);
    }

    public static Rectangle CellBounds(Point cell)
        => new(Bounds.X + cell.X * CellWidth, Bounds.Y + cell.Y * CellHeight, CellWidth, CellHeight);

    // Where things stand: the middle of the cell's bottom edge.
    public static Vector2 CellFeet(Point cell) => new(Bounds.X + (cell.X + 0.5f) * CellWidth, RowFeet(cell.Y));
    public static float RowFeet(int row) => Bounds.Y + (row + 1) * CellHeight;
}
