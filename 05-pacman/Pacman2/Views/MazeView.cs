using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman2.Views;

// Draws the maze: walls as blue outlines, the ghost house door, the dots and power pellets.
// The walls aren't images: each wall tile draws a line along every side that faces an open tile.
public class MazeView(Texture2D pixel, TextureAtlas atlas)
{
    public static readonly Color WallColor = new(33, 33, 255);
    private static readonly Color DoorColor = new(255, 184, 255);
    private const int Inset = 6;
    private const int Thickness = 4;
    private const int Size = Maze.TileSize;

    private readonly TextureRegion _dot = atlas.GetRegion("dot");
    private readonly TextureRegion _pellet = atlas.GetRegion("pellet");
    private float _time;

    public void Update(float deltaSeconds) => _time += deltaSeconds;

    public void Draw(SpriteBatch spriteBatch, Maze maze) => Draw(spriteBatch, maze, WallColor);

    public void Draw(SpriteBatch spriteBatch, Maze maze, Color wallColor)
    {
        for (int y = 0; y < maze.Height; y++)
        {
            for (int x = 0; x < maze.Width; x++)
            {
                Point cell = new(x, y);
                if (maze.IsWall(cell))
                    DrawWall(spriteBatch, maze, cell, wallColor);
                else if (maze.IsDoor(cell))
                    spriteBatch.Draw(pixel, new Rectangle(x * Size, y * Size + Size / 2 - 2, Size, 4), DoorColor);
            }
        }

        foreach (Point dot in maze.Dots)
            _dot.Draw(spriteBatch, new Vector2(dot.X * Size, dot.Y * Size), Color.White);

        // Power pellets blink.
        if ((int)(_time * 4) % 2 == 0)
        {
            foreach (Point pellet in maze.Pellets)
                _pellet.Draw(spriteBatch, new Vector2(pellet.X * Size, pellet.Y * Size), Color.White);
        }
    }

    private void DrawWall(SpriteBatch spriteBatch, Maze maze, Point cell, Color color)
    {
        int left = cell.X * Size, top = cell.Y * Size, right = left + Size, bottom = top + Size;
        bool up = IsOpen(maze, cell + Direction.Up), down = IsOpen(maze, cell + Direction.Down);
        bool west = IsOpen(maze, cell + Direction.Left), east = IsOpen(maze, cell + Direction.Right);

        // A line along each side that faces an open tile. It stops where a line on the next
        // side turns the corner, and runs on into the neighbouring wall tile otherwise.
        int x0 = west ? left + Inset : left, x1 = east ? right - Inset : right;
        int y0 = up ? top + Inset : top, y1 = down ? bottom - Inset : bottom;
        if (up) Line(spriteBatch, x0, top + Inset, x1, top + Inset, color);
        if (down) Line(spriteBatch, x0, bottom - Inset, x1, bottom - Inset, color);
        if (west) Line(spriteBatch, left + Inset, y0, left + Inset, y1, color);
        if (east) Line(spriteBatch, right - Inset, y0, right - Inset, y1, color);

        // Inside corners: the open tile is diagonal, so the two lines meet in this tile.
        foreach (Point diagonal in new Point[] { new(-1, -1), new(1, -1), new(-1, 1), new(1, 1) })
        {
            bool sideX = IsOpen(maze, cell + new Point(diagonal.X, 0));
            bool sideY = IsOpen(maze, cell + new Point(0, diagonal.Y));
            if (sideX || sideY || !IsOpen(maze, cell + diagonal))
                continue;

            int cornerX = diagonal.X < 0 ? left + Inset : right - Inset;
            int cornerY = diagonal.Y < 0 ? top + Inset : bottom - Inset;
            Line(spriteBatch, cornerX, cornerY, diagonal.X < 0 ? left : right, cornerY, color);
            Line(spriteBatch, cornerX, cornerY, cornerX, diagonal.Y < 0 ? top : bottom, color);
        }
    }

    private static bool IsOpen(Maze maze, Point cell)
        => cell.X >= 0 && cell.X < maze.Width && cell.Y >= 0 && cell.Y < maze.Height && !maze.IsWall(cell);

    private void Line(SpriteBatch spriteBatch, int x0, int y0, int x1, int y1, Color color)
    {
        int x = System.Math.Min(x0, x1), y = System.Math.Min(y0, y1);
        int width = System.Math.Abs(x1 - x0), height = System.Math.Abs(y1 - y0);
        spriteBatch.Draw(pixel, new Rectangle(x - Thickness / 2, y - Thickness / 2, width + Thickness, height + Thickness), color);
    }
}
