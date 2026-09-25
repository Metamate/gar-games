using System;
using Pacman4;
using Microsoft.Xna.Framework;
using Xunit;

namespace Pacman.Tests;

public class PacManTests
{
    private readonly World _world = new(TestMaze.Text, new Random(1));

    // How long Pac-Man takes to move a number of tiles.
    private static float TimeFor(float tiles) => tiles * Maze.TileSize / PacMan.Speed;

    [Fact]
    public void A_turn_pressed_early_is_taken_at_the_next_opening()
    {
        PacMan pacMan = _world.PacMan;
        pacMan.Place(new Point(2, 3), Direction.Right);

        // Up is a wall at (3, 3) and (4, 3), but open at (5, 3).
        pacMan.Steer(Direction.Up);
        pacMan.Update(TimeFor(3.6f));

        Assert.Equal(Direction.Up, pacMan.Heading);
        Assert.Equal(new Point(5, 2), pacMan.Tile);
    }

    [Fact]
    public void Pac_Man_stops_at_a_wall()
    {
        PacMan pacMan = _world.PacMan;
        pacMan.Place(new Point(9, 1), Direction.Right);

        pacMan.Update(TimeFor(5));

        Assert.Equal(Direction.None, pacMan.Heading);
        Assert.Equal(new Point(11, 1), pacMan.Tile);
    }

    [Fact]
    public void Pac_Man_can_turn_around_between_two_tiles()
    {
        PacMan pacMan = _world.PacMan;
        pacMan.Place(new Point(5, 1), Direction.Right);
        pacMan.Update(TimeFor(0.3f));

        pacMan.Steer(Direction.Left);

        Assert.Equal(Direction.Left, pacMan.Heading);
    }

    [Fact]
    public void Eating_a_dot_scores_ten_points()
    {
        _world.PacMan.Place(new Point(3, 1), Direction.Left);

        _world.Update(TimeFor(1));

        Assert.Equal(10, _world.Score);
    }
}
