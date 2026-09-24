using Microsoft.Xna.Framework;

namespace Sokoban3.Commands;

// Remembers what the move did (walk or push), so that it can be undone exactly.
public class MoveCommand(Level level, Point direction) : ICommand
{
    private MoveResult _result;

    public void Execute() => _result = level.Move(direction);

    public void Undo() => level.UndoMove(direction, _result);
}
