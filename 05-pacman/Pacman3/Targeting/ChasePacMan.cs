using Microsoft.Xna.Framework;

namespace Pacman3.Targeting;

// Blinky: straight at Pac-Man.
public class ChasePacMan : ITargetStrategy
{
    public Point ChooseTarget(Ghost ghost, World world) => world.PacMan.Tile;
}
