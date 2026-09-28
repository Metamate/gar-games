using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman3.GhostStates;

// Hunting Pac-Man. Where exactly the ghost aims is up to its targeting strategy.
public class ChaseState : GhostState
{
    public override void Update(Ghost ghost, float deltaSeconds) => ghost.Move(Ghost.Speed * deltaSeconds);

    public override Point ChooseDirection(Ghost ghost, IReadOnlyList<Point> options)
        => ghost.Closest(ghost.Targeting.ChooseTarget(ghost, ghost.World), options);

    public override void OnPhaseChanged(Ghost ghost)
    {
        ghost.ChangeState(new ScatterState());
        ghost.Reverse();
    }

    public override void OnPowerPellet(Ghost ghost) => ghost.ChangeState(new FrightenedState());

    public override TouchResult OnTouch(Ghost ghost) => TouchResult.PacManCaught;
}
