using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman2.GhostStates;

// One of a ghost's modes, as an object: how the ghost moves in this mode, and how it reacts to
// what happens in the game. Each state holds its own data, like the frightened timer.
public abstract class GhostState
{
    public virtual void Enter(Ghost ghost) { }
    public virtual void Exit(Ghost ghost) { }

    public abstract void Update(Ghost ghost, float deltaSeconds);

    // At a tile centre: which of the open directions to take.
    public abstract Point ChooseDirection(Ghost ghost, IReadOnlyList<Point> options);

    public virtual bool CanUseDoor => false;
    public virtual GhostLook Look => GhostLook.Normal;

    // What happens in the game. By default, nothing: each state overrides what matters to it.
    public virtual void OnPhaseChanged(Ghost ghost) { }
    public virtual void OnPowerPellet(Ghost ghost) { }
    public virtual TouchResult OnTouch(Ghost ghost) => TouchResult.Nothing;
}
