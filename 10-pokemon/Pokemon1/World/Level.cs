using System;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon1.Entities;

namespace Pokemon1.World;

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

    // The town, one character per tile: T tree, X fence, S sign, * flowers, = path, # tall grass.
    // The digits are the nine tiles of the flat-roofed house; the letters a to l are the twelve of the
    // peaked one, and ^ is the tip of its roof.
    private static readonly string[] Town =
    [
        "TTTTTTTTTTTTTTTTTTTT",
        "T..........^.......T",
        "T.........abc..*...T",
        "T.123.....def......T",
        "T.456.....ghi......T",
        "T.789S....jkl.XXX..T",
        "T..=.......=.......T",
        "T..=========.......T",
        "T.*.....=....######T",
        "T.......=....######T",
        "TTTTTTTT=TTTT######T"
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
                    '^' => new Tile(GameSettings.TileRoofTip, true),
                    >= '1' and <= '9' => new Tile(HouseTile(GameSettings.TileFlatHouse, c - '1'), true),
                    >= 'a' and <= 'l' => new Tile(HouseTile(GameSettings.TilePeakedHouse, c - 'a'), true),
                    _ => new Tile(grass),
                });

                if (c == '#')
                    GrassLayer.SetTile(x, y, new Tile(GameSettings.TileTallGrass));
            }
        }
    }

    // A house is a block of tiles, 3 wide, in the tilesheet, which is 8 tiles wide. Its parts count row by row.
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
