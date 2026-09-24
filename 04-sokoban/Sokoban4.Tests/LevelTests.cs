using Microsoft.Xna.Framework;
using Xunit;

namespace Sokoban4.Tests;

// The rules, tested without starting the game: no window, no textures, just a Level.
public class LevelTests
{
    // A corridor: the player, a box to the right, and a goal next to the far wall.
    private const string Corridor = """
        #######
        # @$ .#
        #######
        """;

    [Fact]
    public void Parse_finds_the_player_boxes_and_goals()
    {
        Level level = Level.Parse(Corridor);

        Assert.Equal(new Point(2, 1), level.Player);
        Assert.Equal([new Point(3, 1)], level.Boxes);
        Assert.True(level.IsGoal(new Point(5, 1)));
        Assert.True(level.IsWall(new Point(0, 0)));
    }

    [Fact]
    public void Walking_into_a_free_cell_moves_the_player()
    {
        Level level = Level.Parse(Corridor);

        Assert.Equal(MoveResult.Walked, level.Move(Direction.Left));
        Assert.Equal(new Point(1, 1), level.Player);
    }

    [Fact]
    public void Walking_into_a_wall_is_blocked()
    {
        Level level = Level.Parse(Corridor);

        Assert.Equal(MoveResult.Blocked, level.Move(Direction.Up));
        Assert.Equal(new Point(2, 1), level.Player);
    }

    [Fact]
    public void Walking_into_a_box_pushes_it()
    {
        Level level = Level.Parse(Corridor);

        Assert.Equal(MoveResult.Pushed, level.Move(Direction.Right));
        Assert.Equal(new Point(3, 1), level.Player);
        Assert.Equal([new Point(4, 1)], level.Boxes);
    }

    [Fact]
    public void A_box_against_a_wall_cannot_be_pushed()
    {
        Level level = Level.Parse(Corridor);
        level.Move(Direction.Right);
        level.Move(Direction.Right);

        Assert.Equal(MoveResult.Blocked, level.Move(Direction.Right));
        Assert.Equal([new Point(5, 1)], level.Boxes);
    }

    [Fact]
    public void A_box_against_another_box_cannot_be_pushed()
    {
        Level level = Level.Parse("""
            ######
            #@$$ #
            ######
            """);

        Assert.Equal(MoveResult.Blocked, level.Move(Direction.Right));
    }

    [Fact]
    public void The_level_is_solved_when_every_box_is_on_a_goal()
    {
        Level level = Level.Parse(Corridor);
        level.Move(Direction.Right);
        Assert.False(level.IsSolved);

        level.Move(Direction.Right);
        Assert.True(level.IsSolved);
    }

    [Fact]
    public void Cells_outside_the_walls_are_not_floor()
    {
        Level level = Level.Parse("""
              ####
            ###@ #
            #  $.#
            ######
            """);

        Assert.False(level.IsFloor(new Point(0, 0)));
        Assert.True(level.IsFloor(new Point(1, 2)));
    }
}
