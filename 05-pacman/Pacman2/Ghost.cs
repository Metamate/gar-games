using System.Collections.Generic;
using Pacman2.GhostStates;
using Microsoft.Xna.Framework;

namespace Pacman2;

// What a ghost looks like, for the view.
public enum GhostLook { Normal, Frightened, FrightenedEnding, Eyes }

// What happens when a ghost and Pac-Man touch.
public enum TouchResult { Nothing, GhostEaten, PacManCaught }

// A ghost, with its mode as a state object (see GhostStates). Ghost forwards everything that
// depends on the mode to its current state, and doesn't know which states exist.
public class Ghost : Actor
{
    public const float Speed = 7 * Maze.TileSize;

    public Ghost(World world, string name, Point start, Point scatterTarget, float releaseDelay) : base(world.Maze)
    {
        World = world;
        Name = name;
        Start = start;
        ScatterTarget = scatterTarget;
        ReleaseDelay = releaseDelay;
    }

    public World World { get; }
    public string Name { get; }
    public Point Start { get; }
    public Point ScatterTarget { get; }
    public float ReleaseDelay { get; }
    public GhostState State { get; private set; }

    public bool IsFrightened => State is FrightenedState;
    public GhostLook Look => State.Look;

    public void Reset()
    {
        bool inHouse = Maze.IsInHouse(Start);
        Place(Start, inHouse ? Direction.Up : Direction.Left);
        ChangeState(inHouse ? new InHouseState(ReleaseDelay) : ScheduledState());
    }

    public void ChangeState(GhostState state)
    {
        State?.Exit(this);
        State = state;
        State.Enter(this);
    }

    // Scatter or chase, whichever the schedule says now.
    public GhostState ScheduledState() => World.Schedule.Current == Phase.Scatter ? new ScatterState() : new ChaseState();

    public void Update(float deltaSeconds) => State.Update(this, deltaSeconds);
    public void OnPhaseChanged() => State.OnPhaseChanged(this);
    public void OnPowerPellet() => State.OnPowerPellet(this);
    public TouchResult Touch() => State.OnTouch(this);

    protected override Point ChooseDirection(Point tile)
    {
        List<Point> options = OpenDirections(tile, State.CanUseDoor);
        return options.Count == 0 ? Direction.None : State.ChooseDirection(this, options);
    }

    // Ghosts never turn back, unless it's a dead end.
    private List<Point> OpenDirections(Point tile, bool canUseDoor)
    {
        var options = new List<Point>();
        Point back = Direction.Opposite(Heading);
        foreach (Point direction in Direction.All)
        {
            if (direction != back && !Maze.BlocksGhost(tile + direction, canUseDoor))
                options.Add(direction);
        }

        if (options.Count == 0 && !Maze.BlocksGhost(tile + back, canUseDoor))
            options.Add(back);
        return options;
    }

    // The direction whose next tile is closest to the target, in a straight line.
    public Point Closest(Point target, IReadOnlyList<Point> options)
    {
        Point best = options[0];
        float bestDistance = float.MaxValue;
        foreach (Point direction in options)
        {
            Point next = Tile + direction;
            float distance = Vector2.DistanceSquared(next.ToVector2(), target.ToVector2());
            if (distance < bestDistance)
            {
                best = direction;
                bestDistance = distance;
            }
        }
        return best;
    }
}
