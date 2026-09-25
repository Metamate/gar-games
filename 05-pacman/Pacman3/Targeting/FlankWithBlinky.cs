using Microsoft.Xna.Framework;

namespace Pacman3.Targeting;

// Inky: take the tile two ahead of Pac-Man, and double the line from Blinky to it. Inky closes
// in from the other side of Blinky, so the two work together.
public class FlankWithBlinky : ITargetStrategy
{
    public Point ChooseTarget(Ghost ghost, World world)
    {
        Point facing = world.PacMan.Facing;
        Point pivot = world.PacMan.Tile + new Point(facing.X * 2, facing.Y * 2);
        Point fromBlinky = pivot - world.Blinky.Tile;
        return pivot + fromBlinky;
    }
}
