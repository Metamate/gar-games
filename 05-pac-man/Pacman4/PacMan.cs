using Microsoft.Xna.Framework;

namespace Pacman4;

public class PacMan(Maze maze) : Actor(maze)
{
    public const float Speed = 7.5f * Maze.TileSize;

    // The direction the player asked for. It's kept until Pac-Man can turn that way, so a turn
    // pressed just before a corner isn't lost (input buffering, as in Snake).
    public Point Wanted { get; private set; }

    public void Reset()
    {
        Place(Maze.PacManStart, Direction.Left);
        Wanted = Direction.None;
    }

    public void Steer(Point direction)
    {
        Wanted = direction;
        if (direction == Direction.Opposite(Heading))
            Reverse();
    }

    public void Update(float deltaSeconds) => Move(Speed * deltaSeconds);

    // Turn if the player wants to and can; otherwise keep going until a wall stops Pac-Man.
    protected override Point ChooseDirection(Point tile)
    {
        if (Wanted != Direction.None && !Maze.BlocksPacMan(tile + Wanted))
            return Wanted;
        if (!Maze.BlocksPacMan(tile + Heading))
            return Heading;
        return Direction.None;
    }
}
