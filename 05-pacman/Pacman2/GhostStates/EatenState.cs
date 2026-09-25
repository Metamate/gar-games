using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman2.GhostStates;

// Only the eyes are left: they race back to the ghost house, where the ghost comes back to life.
public class EatenState : GhostState
{
    public const float Speed = 15 * Maze.TileSize;

    private bool _enteringHouse;

    public override bool CanUseDoor => true;
    public override GhostLook Look => GhostLook.Eyes;

    public override void Update(Ghost ghost, float deltaSeconds)
    {
        ghost.Move(Speed * deltaSeconds);
        if (ghost.Tile == ghost.Maze.HouseCenter)
            ghost.ChangeState(new InHouseState(0));
    }

    // First to the tile outside the door, then in through the door.
    public override Point ChooseDirection(Ghost ghost, IReadOnlyList<Point> options)
    {
        if (ghost.Tile == ghost.Maze.DoorOutside)
            _enteringHouse = true;
        return ghost.Closest(_enteringHouse ? ghost.Maze.HouseCenter : ghost.Maze.DoorOutside, options);
    }
}
