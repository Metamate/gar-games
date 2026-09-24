using System;
using GMDCore.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Platformer1.LevelMaker;

public class SimpleLevelMaker(ContentManager content) : LevelMakerBase(content)
{
    public override GameLevel Generate(int columns, int rows)
    {
        Tilemap = new(Tilesets[Random.Shared.Next(Tilesets.Count)], columns, rows);
        Toppers = new(Toppersets[Random.Shared.Next(Toppersets.Count)], columns, rows);

        for (int i = 0; i < Tilemap.Count; i++)
        {
            int x = i % columns;
            int y = i / columns;

            Tilemap.SetTile(x, y, new Tile(0, false));
        }

        return new GameLevel(Tilemap, Toppers, GetRandomBackground());
    }
}
