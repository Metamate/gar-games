using System;
using Microsoft.Xna.Framework.Content;

namespace Platformer8.LevelMaker;

public class PillarLevelMaker(ContentManager content) : LevelMakerBase(content)
{
    public override GameLevel Generate(int columns, int rows)
    {
        Tilemap = new(Tilesets[Random.Shared.Next(Tilesets.Count)], columns, rows);
        Toppers = new(Toppersets[Random.Shared.Next(Toppersets.Count)], columns, rows);

        int groundHeight = 3;
        int pillarHeight = 2;
        float pillarChance = 0.15f;

        for (int x = 0; x < columns; x++)
        {
            int currentHeight = groundHeight;

            // Chance to spawn a pillar on top of it (avoiding the spawn area)
            if (x > 5 && Random.Shared.NextDouble() < pillarChance)
            {
                currentHeight += pillarHeight;
            }

            CreateGroundColumn(x, currentHeight);
        }

        return new GameLevel(Tilemap, Toppers, GetRandomBackground());
    }
}
