using System.Collections.Generic;

namespace Sokoban4.Commands;

// Executes commands and remembers them, so they can be undone and redone.
public class CommandHistory
{
    private readonly Stack<ICommand> _done = new();
    private readonly Stack<ICommand> _undone = new();

    public int Count => _done.Count;

    public void Execute(ICommand command)
    {
        command.Execute();
        _done.Push(command);

        // A new command starts a new future: what was undone can no longer be redone.
        _undone.Clear();
    }

    public void Undo()
    {
        if (_done.TryPop(out ICommand command))
        {
            command.Undo();
            _undone.Push(command);
        }
    }

    public void Redo()
    {
        if (_undone.TryPop(out ICommand command))
        {
            command.Execute();
            _done.Push(command);
        }
    }

    public void Clear()
    {
        _done.Clear();
        _undone.Clear();
    }
}
