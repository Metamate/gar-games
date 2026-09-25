using Microsoft.Xna.Framework;

namespace Pacman4.Targeting;

// How a ghost picks its target tile while chasing. Each ghost gets its own strategy when it's
// created, and keeps it: the strategies are interchangeable, but never change during the game.
public interface ITargetStrategy
{
    Point ChooseTarget(Ghost ghost, World world);
}
