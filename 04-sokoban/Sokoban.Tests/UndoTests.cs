using Microsoft.Xna.Framework;
using Sokoban4;
using Sokoban4.Commands;
using Xunit;

namespace Sokoban.Tests;

// Tests for undo and redo: after undoing, the level must be exactly as it was.
public class UndoTests
{
    private const string Corridor = """
        #######
        # @$ .#
        #######
        """;

    [Fact]
    public void Undoing_a_walk_puts_the_player_back()
    {
        Level level = Level.Parse(Corridor);
        var history = new CommandHistory();

        history.Execute(new MoveCommand(level, Direction.Left));
        history.Undo();

        Assert.Equal(new Point(2, 1), level.Player);
    }

    [Fact]
    public void Undoing_a_push_pulls_the_box_back()
    {
        Level level = Level.Parse(Corridor);
        var history = new CommandHistory();

        history.Execute(new MoveCommand(level, Direction.Right));
        history.Undo();

        Assert.Equal(new Point(2, 1), level.Player);
        Assert.Equal([new Point(3, 1)], level.Boxes);
    }

    [Fact]
    public void Redo_repeats_the_undone_move()
    {
        Level level = Level.Parse(Corridor);
        var history = new CommandHistory();

        history.Execute(new MoveCommand(level, Direction.Right));
        history.Undo();
        history.Redo();

        Assert.Equal(new Point(3, 1), level.Player);
        Assert.Equal([new Point(4, 1)], level.Boxes);
    }

    [Fact]
    public void A_new_move_clears_what_could_be_redone()
    {
        Level level = Level.Parse(Corridor);
        var history = new CommandHistory();

        history.Execute(new MoveCommand(level, Direction.Right));
        history.Undo();
        history.Execute(new MoveCommand(level, Direction.Left));
        history.Redo();

        Assert.Equal(new Point(1, 1), level.Player);
        Assert.Equal(1, history.Count);
    }
}
