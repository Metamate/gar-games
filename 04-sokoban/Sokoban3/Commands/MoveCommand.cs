using Microsoft.Xna.Framework;

namespace Sokoban3.Commands;

public class MoveCommand(Level level, Point direction) : ICommand
{
    public void Execute() => level.Move(direction);
}
