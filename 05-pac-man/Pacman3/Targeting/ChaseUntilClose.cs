using Microsoft.Xna.Framework;

namespace Pacman3.Targeting;

// Clyde: chases Pac-Man from afar, but loses his nerve within eight tiles, and heads for his
// corner instead.
public class ChaseUntilClose : ITargetStrategy
{
    public const int ShyDistance = 8;

    public Point ChooseTarget(Ghost ghost, World world)
    {
        float distance = Vector2.Distance(ghost.Tile.ToVector2(), world.PacMan.Tile.ToVector2());
        return distance >= ShyDistance ? world.PacMan.Tile : ghost.ScatterTarget;
    }
}
