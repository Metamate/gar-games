using System;
using System.Collections.Generic;
using Pacman5;
using Pacman5.Routing;
using Microsoft.Xna.Framework;
using Xunit;

namespace Pacman.Tests;

// The pathfinder, and the two routing strategies that use the maze differently. A path is easy
// to check on a small maze: count the steps on paper, and compare.
public class PathfinderTests
{
    // One corridor that is open at both ends, so it is a tunnel.
    private const string Tunnel = """
        #######
         .....
        #######
        """;

    private readonly World _world = new(TestMaze.Text, new Random(1));
    private Maze Maze => _world.Maze;

    [Fact]
    public void A_path_along_a_corridor_has_one_tile_per_step()
    {
        List<Point> path = Pathfinder.FindPath(Maze, new Point(1, 1), new Point(4, 1), canUseDoor: false);

        Assert.Equal([new Point(2, 1), new Point(3, 1), new Point(4, 1)], path);
    }

    [Fact]
    public void A_path_goes_around_walls()
    {
        // (3, 1) is two tiles above (4, 3) and one to the left, with a wall in between.
        List<Point> path = Pathfinder.FindPath(Maze, new Point(4, 3), new Point(3, 1), canUseDoor: false);

        Assert.Equal(5, path.Count);
        Assert.Equal(new Point(3, 1), path[^1]);
        Assert.All(path, tile => Assert.False(Maze.IsWall(tile)));
    }

    [Fact]
    public void Every_step_of_a_path_is_to_a_neighbouring_tile()
    {
        Point previous = new(1, 1);
        foreach (Point tile in Pathfinder.FindPath(Maze, previous, new Point(11, 3), canUseDoor: false))
        {
            Assert.Equal(1, Math.Abs(tile.X - previous.X) + Math.Abs(tile.Y - previous.Y));
            previous = tile;
        }
    }

    [Fact]
    public void The_house_can_only_be_reached_through_the_door()
    {
        Point start = new(1, 3);

        Assert.Empty(Pathfinder.FindPath(Maze, start, Maze.HouseCenter, canUseDoor: false));
        Assert.Equal(6, Pathfinder.FindPath(Maze, start, Maze.HouseCenter, canUseDoor: true).Count);
    }

    [Fact]
    public void There_is_no_path_to_a_wall()
    {
        Assert.Empty(Pathfinder.FindPath(Maze, new Point(1, 1), new Point(2, 2), canUseDoor: false));
    }

    [Fact]
    public void A_path_can_be_made_to_start_in_a_given_direction()
    {
        // The shortest path from (4, 3) to (3, 1) starts to the right. Starting left is longer.
        List<Point> path = Pathfinder.FindPath(Maze, new Point(4, 3), new Point(3, 1), canUseDoor: false, [Direction.Left]);

        Assert.Equal(new Point(3, 3), path[0]);
        Assert.Equal(7, path.Count);
    }

    [Fact]
    public void A_path_uses_the_tunnel_when_that_is_shorter()
    {
        Maze tunnel = Maze.Parse(Tunnel);

        // Four steps along the corridor, or three through the tunnel.
        List<Point> path = Pathfinder.FindPath(tunnel, new Point(1, 1), new Point(5, 1), canUseDoor: false);

        Assert.Equal([new Point(0, 1), new Point(6, 1), new Point(5, 1)], path);
    }

    [Fact]
    public void The_estimate_is_never_more_than_the_real_distance()
    {
        Maze tunnel = Maze.Parse(Tunnel);

        Assert.Equal(3, Pathfinder.Estimate(tunnel, new Point(1, 1), new Point(5, 1)));
        Assert.Equal(3, Pathfinder.Estimate(Maze, new Point(4, 3), new Point(3, 1)));
    }

    [Fact]
    public void The_two_routing_strategies_can_disagree()
    {
        Ghost blinky = _world.Blinky;
        blinky.Place(new Point(4, 3), Direction.None);
        Point target = new(3, 1);
        Point[] options = [Direction.Left, Direction.Right];

        // Left looks closer in a straight line, but the way up is to the right.
        Assert.Equal(Direction.Left, new NearestTile().ChooseDirection(blinky, target, options));
        Assert.Equal(Direction.Right, new ShortestPath().ChooseDirection(blinky, target, options));
    }

    [Fact]
    public void Without_a_path_the_shortest_path_strategy_gets_as_close_as_it_can()
    {
        Ghost blinky = _world.Blinky;
        blinky.Place(new Point(4, 3), Direction.None);
        Point[] options = [Direction.Left, Direction.Right];

        // Blinky's scatter target is outside the maze, up and to the right.
        Assert.Equal(Direction.Right, new ShortestPath().ChooseDirection(blinky, blinky.ScatterTarget, options));
    }
}
