using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman2.GhostStates;

// Heading for the ghost's own corner of the maze (its scatter target is outside the maze, so
// it never arrives, and circles the walls near the corner instead).
public class ScatterState : GhostState
{
    public override void Update(Ghost ghost, float deltaSeconds) => ghost.Move(Ghost.Speed * deltaSeconds);

    public override Point ChooseDirection(Ghost ghost, IReadOnlyList<Point> options)
        => ghost.Closest(ghost.ScatterTarget, options);

    public override void OnPhaseChanged(Ghost ghost)
    {
        ghost.ChangeState(new ChaseState());
        ghost.Reverse();
    }

    public override void OnPowerPellet(Ghost ghost) => ghost.ChangeState(new FrightenedState());

    public override TouchResult OnTouch(Ghost ghost) => TouchResult.PacManCaught;
}
