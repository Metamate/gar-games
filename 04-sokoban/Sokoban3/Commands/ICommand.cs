namespace Sokoban3.Commands;

// A request, as an object: it can be stored, counted, queued or logged.
public interface ICommand
{
    void Execute();
}
