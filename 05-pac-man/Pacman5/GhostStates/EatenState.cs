using System.Collections.Generic;
using Pacman5.Routing;
using Microsoft.Xna.Framework;

namespace Pacman5.GhostStates;

// Only the eyes are left: they race back to the ghost house, where the ghost comes back to life.
// Whatever routing the ghost has, the eyes take the shortest path.
public class EatenState : GhostState
{
    public const float Speed = 15 * Maze.TileSize;

    private readonly ShortestPath _route = new();

    public override bool CanUseDoor => true;
    public override GhostLook Look => GhostLook.Eyes;

    public override void Update(Ghost ghost, float deltaSeconds)
    {
        ghost.Move(Speed * deltaSeconds);
        if (ghost.Tile == ghost.Maze.HouseCenter)
            ghost.ChangeState(new InHouseState(0));
    }

    public override Point ChooseDirection(Ghost ghost, IReadOnlyList<Point> options)
        => _route.ChooseDirection(ghost, ghost.Maze.HouseCenter, options);
}
