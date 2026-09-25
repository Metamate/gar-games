using Microsoft.Xna.Framework;

namespace Pacman2;

// Something that moves through the maze: Pac-Man and the ghosts. An actor moves from the centre
// of one tile to the centre of the next. At every tile centre, it chooses where to go next.
public abstract class Actor(Maze maze)
{
    private Point _heading;
    private Point _target;      // the tile the actor is moving to

    public Maze Maze => maze;
    public Vector2 Position { get; private set; }
    public Point Tile => maze.Wrap(Maze.TileAt(Position));

    // Where the actor is going. None when it stands still.
    public Point Heading
    {
        get => _heading;
        private set
        {
            _heading = value;
            if (value != Direction.None)
                Facing = value;
        }
    }

    // Where the actor looks: the last direction it moved in. Only for drawing.
    public Point Facing { get; private set; } = Direction.Left;

    public void Place(Point tile, Point heading)
    {
        Position = Maze.Center(tile);
        _target = tile;
        Heading = heading;
    }

    // Turn around at once, even between two tiles.
    public void Reverse()
    {
        if (Heading == Direction.None)
            return;

        _target -= Heading;
        Heading = Direction.Opposite(Heading);
    }

    public void Move(float distance)
    {
        while (true)
        {
            Vector2 goal = Maze.Center(_target);
            float left = Vector2.Distance(Position, goal);
            if (left > distance)
            {
                Position += (goal - Position) / left * distance;
                return;
            }

            // Arrived at a tile centre: wrap through the tunnel, and choose the next direction.
            distance -= left;
            _target = maze.Wrap(_target);
            Position = Maze.Center(_target);
            Heading = ChooseDirection(_target);
            if (Heading == Direction.None)
                return;
            _target += Heading;
        }
    }

    protected abstract Point ChooseDirection(Point tile);
}
