using Microsoft.Xna.Framework;

namespace Pacman4.Targeting;

// Pinky: four tiles ahead of Pac-Man, to cut him off.
public class AmbushAhead : ITargetStrategy
{
    public const int TilesAhead = 4;

    public Point ChooseTarget(Ghost ghost, World world)
    {
        Point facing = world.PacMan.Facing;
        return world.PacMan.Tile + new Point(facing.X * TilesAhead, facing.Y * TilesAhead);
    }
}
