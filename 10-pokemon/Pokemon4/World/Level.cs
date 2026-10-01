using System;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon4.Entities;

namespace Pokemon4.World;

// The overworld level: a small town on two tilemap layers (the ground, and tall grass over it) and the player entity.
public sealed class Level
{
    public Tilemap BaseLayer  { get; }
    public Tilemap GrassLayer { get; }

    public Player Player { get; }

    public Level(Player player, Tileset tileset)
    {
        Player     = player;
        BaseLayer  = new Tilemap(tileset, GameSettings.MapCols, GameSettings.MapRows);
        GrassLayer = new Tilemap(tileset, GameSettings.MapCols, GameSettings.MapRows);
        GenerateMaps();
    }

    // The town, one character per tile: T tree, X fence, S sign, * flowers, = path, # tall grass, ~ the healing spring.
    // The digits are the nine tiles of the flat-roofed house, the letters a to i those of the peaked one.
    private static readonly string[] Town =
    [
        "TTTTTTTTTTTTTTTTTTTT",
        "T.123.....abc..*...T",
        "T.456.....def......T",
        "T.789S....ghi.XXXX.T",
        "T..=.......=.......T",
        "T..=========....~..T",
        "T.*.....=..........T",
        "T.......=......*...T",
        "####################",
        "####################",
        "####################"
    ];

    private void GenerateMaps()
    {
        for (int y = 0; y < GameSettings.MapRows; y++)
        {
            for (int x = 0; x < GameSettings.MapCols; x++)
            {
                char c = Town[y][x];
                int grass = GameSettings.TileGrass[Random.Shared.Next(GameSettings.TileGrass.Length)];

                // Trees, fences, signs and houses are solid: nothing walks onto them.
                BaseLayer.SetTile(x, y, c switch
                {
                    'T' => new Tile(GameSettings.TileTree, true),
                    'X' => new Tile(GameSettings.TileFence, true),
                    'S' => new Tile(GameSettings.TileSign, true),
                    '*' => new Tile(GameSettings.TileFlowers),
                    '=' => new Tile(GameSettings.TilePath),
                    '~' => new Tile(GameSettings.TileSpring),
                    >= '1' and <= '9' => new Tile(HouseTile(GameSettings.TileFlatHouse, c - '1'), true),
                    >= 'a' and <= 'i' => new Tile(HouseTile(GameSettings.TilePeakedHouse, c - 'a'), true),
                    _ => new Tile(grass),
                });

                if (c == '#')
                    GrassLayer.SetTile(x, y, new Tile(GameSettings.TileTallGrass));
            }
        }
    }

    // A house is 3 x 3 tiles in the tilesheet, which is 8 tiles wide: part 0 to 8, row by row.
    private static int HouseTile(int first, int part) => first + part / 3 * 8 + part % 3;

    // Whether an entity can't step onto this tile.
    public bool IsSolid(Point tile) => BaseLayer.GetTile(tile.X, tile.Y).IsSolid;

    public void Update(GameTime gameTime)
    {
        Player.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        BaseLayer.Draw(spriteBatch);
        GrassLayer.Draw(spriteBatch);
        Player.Draw(spriteBatch);
    }
}
