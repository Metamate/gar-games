using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban2;

// Draws a level with the tilesheet. It reads the level, but never changes it.
public class LevelView(Tileset tiles)
{
    public const int TileSize = 64;

    // Tile numbers in the tilesheet (13 columns of 64×64 tiles).
    private const int FloorTile = 89;
    private const int WallTile = 84;
    private const int GoalTile = 24;
    private const int BoxTile = 6;
    private const int BoxOnGoalTile = 19;
    private const int PlayerDownTile = 52;
    private const int PlayerUpTile = 55;
    private const int PlayerRightTile = 78;
    private const int PlayerLeftTile = 81;

    // Which way the player faces. This is only about drawing, so it lives here, not in Level.
    public Point Facing { get; set; } = Direction.Down;

    public void Draw(SpriteBatch spriteBatch, Level level)
    {
        for (int y = 0; y < level.Height; y++)
        {
            for (int x = 0; x < level.Width; x++)
            {
                Point cell = new(x, y);
                if (level.IsWall(cell))
                {
                    DrawTile(spriteBatch, WallTile, cell);
                }
                else if (level.IsFloor(cell))
                {
                    DrawTile(spriteBatch, FloorTile, cell);
                    if (level.IsGoal(cell))
                    {
                        DrawTile(spriteBatch, GoalTile, cell);
                    }
                }
            }
        }

        foreach (Point box in level.Boxes)
        {
            DrawTile(spriteBatch, level.IsGoal(box) ? BoxOnGoalTile : BoxTile, box);
        }

        DrawTile(spriteBatch, PlayerTile(), level.Player);
    }

    private int PlayerTile()
    {
        if (Facing == Direction.Up) return PlayerUpTile;
        if (Facing == Direction.Left) return PlayerLeftTile;
        if (Facing == Direction.Right) return PlayerRightTile;
        return PlayerDownTile;
    }

    private void DrawTile(SpriteBatch spriteBatch, int tile, Point cell)
    {
        tiles.GetTile(tile).Draw(spriteBatch, new Vector2(cell.X * TileSize, cell.Y * TileSize), Color.White);
    }
}
