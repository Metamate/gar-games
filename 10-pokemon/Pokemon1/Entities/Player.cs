using GARCore.Graphics;
using Pokemon1.Definitions;

namespace Pokemon1.Entities;

// The player-controlled entity. Extends Entity by handling its own
// initialization (start position, size, animations).
public sealed class Player : Entity
{

    public Player(TextureAtlas entityAtlas)
    {
        MapX   = GameSettings.PlayerStartMapX;
        MapY   = GameSettings.PlayerStartMapY;
        Width  = GameSettings.TileSize;
        Height = GameSettings.TileSize;
        X      = MapX * GameSettings.TileSize;
        // A little above its tile, so the figure stands on the ground instead of lying flat on it.
        Y      = MapY * GameSettings.TileSize - GameSettings.EntityLift;

        foreach (var (key, anim) in ContentLoader.CreateEntityAnimations(entityAtlas))
            Animations[key] = anim;
    }
}
