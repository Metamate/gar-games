using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman5.Routing;

// Take the first step of the shortest path to the target. The path is found again at every
// tile centre, because the target may have moved.
public class ShortestPath : IRouteStrategy
{
    private readonly NearestTile _nearest = new();

    public Point ChooseDirection(Ghost ghost, Point target, IReadOnlyList<Point> options)
    {
        List<Point> path = Pathfinder.FindPath(ghost.Maze, ghost.Tile, target, ghost.State.CanUseDoor, options);

        // No path: the target is inside a wall or outside the maze. Get as close as possible.
        if (path.Count == 0)
            return _nearest.ChooseDirection(ghost, target, options);

        foreach (Point direction in options)
        {
            if (ghost.Maze.Wrap(ghost.Tile + direction) == path[0])
                return direction;
        }
        return options[0];
    }
}
