using System.IO;
using System.Linq;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban0;

public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;
    private const int TileSize = 64;

    // Tile numbers in the tilesheet (3 columns of 64×64 tiles).
    private const int FloorTile = 0;
    private const int WallTile = 1;
    private const int GoalTile = 2;
    private const int BoxTile = 3;
    private const int BoxOnGoalTile = 4;
    private const int PlayerDownTile = 5;
    private const int PlayerUpTile = 6;
    private const int PlayerRightTile = 8;
    private const int PlayerLeftTile = 7;

    private Tileset _tiles;
    private string[] _level;

    public Game1() : base("Sokoban", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        Texture2D texture = Content.Load<Texture2D>("images/tiles");
        _tiles = new Tileset(new TextureRegion(texture, 0, 0, texture.Width, texture.Height), TileSize, TileSize);

        // The level is plain text, one character per cell.
        _level = ReadText("levels/level1.txt").Replace("\r", "").Trim('\n').Split('\n');
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(60, 64, 72));

        int width = _level.Max(line => line.Length);
        Matrix centre = Matrix.CreateTranslation((VirtualWidth - width * TileSize) / 2, (VirtualHeight - _level.Length * TileSize) / 2, 0);
        SpriteBatch.Begin(transformMatrix: centre * ScreenScaleMatrix);

        // Each character decides what is drawn in its cell.
        for (int y = 0; y < _level.Length; y++)
        {
            for (int x = 0; x < _level[y].Length; x++)
            {
                char c = _level[y][x];
                Vector2 position = new(x * TileSize, y * TileSize);

                if (c == '#')
                {
                    DrawTile(WallTile, position);
                    continue;
                }

                DrawTile(FloorTile, position);
                if (c is '.' or '*' or '+') DrawTile(GoalTile, position);
                if (c == '$') DrawTile(BoxTile, position);
                if (c == '*') DrawTile(BoxOnGoalTile, position);
                if (c is '@' or '+') DrawTile(PlayerDownTile, position);
            }
        }

        SpriteBatch.End();
        base.Draw(gameTime);
    }

    private void DrawTile(int tile, Vector2 position)
    {
        _tiles.GetTile(tile).Draw(SpriteBatch, position, Color.White);
    }

    // Content files are opened through TitleContainer, which works on every platform.
    private string ReadText(string path)
    {
        using Stream stream = TitleContainer.OpenStream(Path.Combine(Content.RootDirectory, path));
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
