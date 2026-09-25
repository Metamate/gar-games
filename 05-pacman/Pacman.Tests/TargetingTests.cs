using System;
using Pacman4;
using Microsoft.Xna.Framework;
using Xunit;

namespace Pacman.Tests;

// Each ghost's targeting strategy, tested on its own: put Pac-Man (and Blinky) somewhere, and
// check which tile the ghost aims for. A strategy is a small class with one method, so it's
// easy to test.
public class TargetingTests
{
    private readonly World _world = new(TestMaze.Text, new Random(1));

    private Point TargetOf(Ghost ghost) => ghost.Targeting.ChooseTarget(ghost, _world);

    [Fact]
    public void Blinky_aims_at_Pac_Man()
    {
        _world.PacMan.Place(new Point(5, 1), Direction.Right);

        Assert.Equal(new Point(5, 1), TargetOf(_world.Blinky));
    }

    [Fact]
    public void Pinky_aims_four_tiles_ahead_of_Pac_Man()
    {
        _world.PacMan.Place(new Point(5, 1), Direction.Right);

        Assert.Equal(new Point(9, 1), TargetOf(_world.Pinky));
    }

    [Fact]
    public void Pinky_aims_ahead_even_when_that_is_outside_the_maze()
    {
        _world.PacMan.Place(new Point(5, 1), Direction.Up);

        Assert.Equal(new Point(5, -3), TargetOf(_world.Pinky));
    }

    [Fact]
    public void Inky_doubles_the_line_from_Blinky_to_two_tiles_ahead_of_Pac_Man()
    {
        _world.PacMan.Place(new Point(5, 1), Direction.Right);
        _world.Blinky.Place(new Point(3, 3), Direction.Left);

        // Two tiles ahead of Pac-Man is (7, 1). From Blinky that's (+4, -2), doubled: (11, -1).
        Assert.Equal(new Point(11, -1), TargetOf(_world.Inky));
    }

    [Fact]
    public void Clyde_chases_Pac_Man_from_far_away()
    {
        _world.PacMan.Place(new Point(11, 1), Direction.Left);
        _world.Clyde.Place(new Point(1, 3), Direction.Right);

        Assert.Equal(new Point(11, 1), TargetOf(_world.Clyde));
    }

    [Fact]
    public void Clyde_heads_for_his_corner_when_Pac_Man_is_close()
    {
        _world.PacMan.Place(new Point(5, 1), Direction.Left);
        _world.Clyde.Place(new Point(3, 3), Direction.Right);

        Assert.Equal(_world.Clyde.ScatterTarget, TargetOf(_world.Clyde));
    }
}
