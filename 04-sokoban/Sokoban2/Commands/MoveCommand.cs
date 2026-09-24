using Microsoft.Xna.Framework;

namespace Sokoban2.Commands;

public class MoveCommand(Level level, Point direction) : ICommand
{
    public void Execute() => level.Move(direction);
}
