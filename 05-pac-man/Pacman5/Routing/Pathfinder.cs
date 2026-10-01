using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman5.Routing;

// Finds the shortest path between two tiles of the maze, with A*.
public static class Pathfinder
{
    // The tiles to walk through, from the one after start up to and including goal. The list is
    // empty when there is no path. With firstSteps, the path has to begin in one of those
    // directions (a ghost can't turn back).
    public static List<Point> FindPath(Maze maze, Point start, Point goal, bool canUseDoor, IReadOnlyList<Point> firstSteps = null)
    {
        var open = new PriorityQueue<Point, int>();     // tiles to look at, the most promising first
        var steps = new Dictionary<Point, int>();       // the fewest steps found from start to each tile
        var cameFrom = new Dictionary<Point, Point>();  // the tile each tile was reached from

        open.Enqueue(start, 0);
        steps[start] = 0;

        while (open.Count > 0)
        {
            Point tile = open.Dequeue();
            if (tile == goal)
                return Walk(cameFrom, start, goal);

            IReadOnlyList<Point> directions = tile == start && firstSteps != null ? firstSteps : Direction.All;
            foreach (Point direction in directions)
            {
                Point next = maze.Wrap(tile + direction);
                if (maze.BlocksGhost(next, canUseDoor))
                    continue;

                int stepsToNext = steps[tile] + 1;
                if (steps.TryGetValue(next, out int known) && known <= stepsToNext)
                    continue;

                steps[next] = stepsToNext;
                cameFrom[next] = tile;
                open.Enqueue(next, stepsToNext + Estimate(maze, next, goal));
            }
        }

        return [];
    }

    // The steps left to the goal if there were no walls. It is never more than the real
    // distance, which is what A* needs to find the shortest path. Going round through the
    // tunnel can be shorter, so the estimate counts that way too.
    public static int Estimate(Maze maze, Point from, Point goal)
    {
        int across = Math.Abs(from.X - goal.X);
        return Math.Min(across, maze.Width - across) + Math.Abs(from.Y - goal.Y);
    }

    // Follow cameFrom back from the goal to the start, and turn the tiles around.
    private static List<Point> Walk(Dictionary<Point, Point> cameFrom, Point start, Point goal)
    {
        var path = new List<Point>();
        for (Point tile = goal; tile != start; tile = cameFrom[tile])
            path.Add(tile);
        path.Reverse();
        return path;
    }
}
