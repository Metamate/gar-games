using System;
using Microsoft.Xna.Framework.Content;

namespace Platformer2.LevelMaker;

public class ComplexLevelMaker(ContentManager content) : LevelMakerBase(content)
{
    public override GameLevel Generate(int columns, int rows)
    {
        Tilemap = new(Tilesets[Random.Shared.Next(Tilesets.Count)], columns, rows);
        Toppers = new(Toppersets[Random.Shared.Next(Toppersets.Count)], columns, rows);

        int groundHeight = 3;
        int pillarHeight = 2;
        float pitChance = 0.15f;
        float pillarChance = 0.15f;

        for (int x = 0; x < columns; x++)
        {
            // Always ensure player spawns on a safe platform
            if (x <= 4)
            {
                CreateGroundColumn(x, groundHeight);
                continue;
            }

            // Chance for a pit
            if (Random.Shared.NextDouble() < pitChance)
            {
                continue;
            }

            // If not a pit, determine ground height (with potential pillar)
            int currentHeight = groundHeight;
            if (Random.Shared.NextDouble() < pillarChance)
            {
                currentHeight += pillarHeight;
            }

            CreateGroundColumn(x, currentHeight);
        }

        return new GameLevel(Tilemap, Toppers, GetRandomBackground());
    }
}
