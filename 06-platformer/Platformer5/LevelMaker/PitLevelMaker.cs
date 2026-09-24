using System;
using Microsoft.Xna.Framework.Content;

namespace Platformer5.LevelMaker;

public class PitLevelMaker(ContentManager content) : LevelMakerBase(content)
{
    public override GameLevel Generate(int columns, int rows)
    {
        Tilemap = new(Tilesets[Random.Shared.Next(Tilesets.Count)], columns, rows);
        Toppers = new(Toppersets[Random.Shared.Next(Toppersets.Count)], columns, rows);

        int groundHeight = 3;
        float pitChance = 0.2f;

        for (int x = 0; x < columns; x++)
        {
            // Skip pit generation for the first few columns to ensure player spawns on ground
            if (x > 3 && Random.Shared.NextDouble() < pitChance)
            {
                continue;
            }

            CreateGroundColumn(x, groundHeight);
        }

        return new GameLevel(Tilemap, Toppers, GetRandomBackground());
    }
}
