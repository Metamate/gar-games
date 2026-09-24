namespace Sokoban5.Commands;

// A request, as an object. A command that knows how to reverse itself can be undone.
public interface ICommand
{
    void Execute();
    void Undo();
}
