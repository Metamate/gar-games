using System;
using GARCore.Graphics;

namespace Pokemon1.World;

// The town's layout, and the tiles it is built from.
public static class Town
{
    // The town, one character per tile: T tree, X fence, S sign, * flowers, = path, # tall grass.
    // The digits are the nine tiles of the flat-roofed house; the letters a to l are the twelve of the
    // peaked one, and ^ is the tip of its roof.
    private static readonly string[] Map =
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

    // Fills the two layers from the map.
    public static void Build(Tilemap baseLayer, Tilemap grassLayer)
    {
        for (int y = 0; y < GameSettings.MapRows; y++)
        {
            for (int x = 0; x < GameSettings.MapCols; x++)
            {
                char c = Map[y][x];
                int grass = GameSettings.TileGrass[Random.Shared.Next(GameSettings.TileGrass.Length)];

                // Trees, fences, signs and houses are solid: nothing walks onto them.
                baseLayer.SetTile(x, y, c switch
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
                    grassLayer.SetTile(x, y, new Tile(GameSettings.TileTallGrass));
            }
        }
    }

    // A house is a block of tiles, 3 wide, in the tilesheet, which is 8 tiles wide. Its parts count row by row.
    private static int HouseTile(int first, int part) => first + part / 3 * 8 + part % 3;
}
