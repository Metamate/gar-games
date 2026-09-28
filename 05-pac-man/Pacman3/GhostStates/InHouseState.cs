using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman3.GhostStates;

// Waiting in the ghost house, then leaving it through the door.
public class InHouseState(float releaseDelay) : GhostState
{
    public const float Speed = 4 * Maze.TileSize;

    private float _waited;

    public override bool CanUseDoor => true;

    public override void Update(Ghost ghost, float deltaSeconds)
    {
        _waited += deltaSeconds;
        if (_waited < releaseDelay)
            return;

        ghost.Move(Speed * deltaSeconds);
        if (!ghost.Maze.IsInHouse(ghost.Tile))
            ghost.ChangeState(ghost.ScheduledState());
    }

    public override Point ChooseDirection(Ghost ghost, IReadOnlyList<Point> options)
        => ghost.Closest(ghost.Maze.DoorOutside, options);
}
