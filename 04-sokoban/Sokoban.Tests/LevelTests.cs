using Microsoft.Xna.Framework;
using Sokoban4;
using Xunit;

namespace Sokoban.Tests;

// Unit tests for the rules in Level. Each test is a small method that sets up a level,
// does one thing, and checks the result. No window, no textures, no key presses: Level
// doesn't need them, so the tests don't either.
//
// Run them with `dotnet test` in the 04-sokoban folder, or from your editor's test explorer.
public class LevelTests
{
    // A corridor: the player, a box to the right, and a goal next to the far wall.
    // The cells are (x, y), counted from the top-left corner: the player is at (2, 1).
    private const string Corridor = """
        #######
        # @$ .#
        #######
        """;

    // [Fact] marks a method as a test. The name says what the test checks.
    [Fact]
    public void Parse_finds_the_player_boxes_and_goals()
    {
        Level level = Level.Parse(Corridor);

        // Assert.Equal(expected, actual) fails the test if the two values differ.
        Assert.Equal(new Point(2, 1), level.Player);
        Assert.Equal([new Point(3, 1)], level.Boxes);
        Assert.True(level.IsGoal(new Point(5, 1)));
        Assert.True(level.IsWall(new Point(0, 0)));
    }

    // Most tests have three parts: arrange (set up), act (do one thing), assert (check).
    [Fact]
    public void Walking_into_a_free_cell_moves_the_player()
    {
        // Arrange
        Level level = Level.Parse(Corridor);

        // Act
        MoveResult result = level.Move(Direction.Left);

        // Assert
        Assert.Equal(MoveResult.Walked, result);
        Assert.Equal(new Point(1, 1), level.Player);
    }

    [Fact]
    public void Walking_into_a_wall_is_blocked()
    {
        Level level = Level.Parse(Corridor);

        MoveResult result = level.Move(Direction.Up);

        Assert.Equal(MoveResult.Blocked, result);
        Assert.Equal(new Point(2, 1), level.Player);
    }

    [Fact]
    public void Walking_into_a_box_pushes_it()
    {
        Level level = Level.Parse(Corridor);

        MoveResult result = level.Move(Direction.Right);

        Assert.Equal(MoveResult.Pushed, result);
        Assert.Equal(new Point(3, 1), level.Player);
        Assert.Equal([new Point(4, 1)], level.Boxes);
    }

    [Fact]
    public void A_box_against_a_wall_cannot_be_pushed()
    {
        Level level = Level.Parse(Corridor);
        level.Move(Direction.Right);
        level.Move(Direction.Right);

        MoveResult result = level.Move(Direction.Right);

        Assert.Equal(MoveResult.Blocked, result);
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

        MoveResult result = level.Move(Direction.Right);

        Assert.Equal(MoveResult.Blocked, result);
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
