using GARCore.Graphics;
using Pokemon3.Definitions;
using Pokemon3.Mons;

namespace Pokemon3.Entities;

// The player-controlled entity. Extends Entity by adding a Pokemon Party
// and handling its own initialization (start position, size, animations).
public sealed class Player : Entity
{
    public Party Party { get; }

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

        Party = new Party(new[]
        {
            new Mon(PokemonDefinitions.GetRandom(), GameSettings.PlayerStartLevel)
        });
    }
}
