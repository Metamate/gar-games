using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Platformer7.Entities;

namespace Platformer7.LevelMaker;

public class ComplexLevelMaker(ContentManager content) : LevelMakerBase(content)
{
    public override GameLevel Generate(int columns, int rows)
    {
        Tilemap = new(Tilesets[Random.Shared.Next(Tilesets.Count)], columns, rows);
        Toppers = new(Toppersets[Random.Shared.Next(Toppersets.Count)], columns, rows);

        GameLevel level = new(Tilemap, Toppers, GetRandomBackground());

        int groundHeight = 3;
        int pillarHeight = 2;
        float pitChance = 0.15f;
        float pillarChance = 0.15f;
        float bushChance = 0.3f;
        float boxChance = 0.1f;
        float snailChance = 0.1f;

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

            // Spawn decorative bushes on solid ground
            if (Random.Shared.NextDouble() < bushChance)
            {
                // Target the tile space directly above the ground column
                Vector2 bushPosition = Tilemap.TileToPoint(x, (rows - currentHeight) - 1);
                level.AddEntity(new Bush(GetRandomBush(), bushPosition));
            }

            // Spawn snails on flat ground (not pillars, to keep it simple)
            if (currentHeight == groundHeight && Random.Shared.NextDouble() < snailChance)
            {
                // Position snail on top of ground
                Vector2 snailPosition = Tilemap.TileToPoint(x, (rows - currentHeight) - 1);
                level.AddEntity(new Snail(CreaturesAtlas, level, snailPosition));
            }

            // Spawn floating mystery boxes
            if (Random.Shared.NextDouble() < boxChance)
            {
                int boxHeight = (currentHeight > groundHeight) ? 3 : 4;
                Vector2 boxPosition = Tilemap.TileToPoint(x, (rows - currentHeight) - boxHeight);
                level.AddEntity(new MysteryBox(level, GetRandomMysteryBox(), boxPosition, Gems));
            }
        }

        return level;
    }
}
