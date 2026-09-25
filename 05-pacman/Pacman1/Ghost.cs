using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman1;

public enum GhostMode { InHouse, Scatter, Chase, Frightened, Eaten }

// What a ghost looks like, for the view.
public enum GhostLook { Normal, Frightened, FrightenedEnding, Eyes }

// What happens when a ghost and Pac-Man touch.
public enum TouchResult { Nothing, GhostEaten, PacManCaught }

// A ghost, with its mode as an enum. It works, but look at how every method switches over the
// mode, and how the fields (timers, flags) each belong to only one mode.
public class Ghost : Actor
{
    public const float Speed = 7 * Maze.TileSize;
    public const float HouseSpeed = 4 * Maze.TileSize;
    public const float FrightenedSpeed = 4.5f * Maze.TileSize;
    public const float EyesSpeed = 15 * Maze.TileSize;
    public const float FrightenedSeconds = 6;
    public const float FlashSeconds = 2;

    private readonly World _world;
    private float _releaseDelay;    // only for InHouse
    private float _houseTimer;      // only for InHouse
    private float _frightenedTimer; // only for Frightened
    private bool _enteringHouse;    // only for Eaten

    public Ghost(World world, string name, Point start, Point scatterTarget, float releaseDelay) : base(world.Maze)
    {
        _world = world;
        Name = name;
        Start = start;
        ScatterTarget = scatterTarget;
        ReleaseDelay = releaseDelay;
    }

    public string Name { get; }
    public Point Start { get; }
    public Point ScatterTarget { get; }
    public float ReleaseDelay { get; }
    public GhostMode Mode { get; private set; }

    public bool IsFrightened => Mode == GhostMode.Frightened;

    public GhostLook Look => Mode switch
    {
        GhostMode.Frightened when _frightenedTimer > FrightenedSeconds - FlashSeconds => GhostLook.FrightenedEnding,
        GhostMode.Frightened => GhostLook.Frightened,
        GhostMode.Eaten => GhostLook.Eyes,
        _ => GhostLook.Normal,
    };

    public void Reset()
    {
        bool inHouse = Maze.IsInHouse(Start);
        Place(Start, inHouse ? Direction.Up : Direction.Left);
        _releaseDelay = ReleaseDelay;
        SetMode(inHouse ? GhostMode.InHouse : ScheduledMode());
    }

    public void Update(float deltaSeconds)
    {
        switch (Mode)
        {
            case GhostMode.InHouse:
                _houseTimer += deltaSeconds;
                if (_houseTimer < _releaseDelay)
                    return;
                Move(HouseSpeed * deltaSeconds);
                if (!Maze.IsInHouse(Tile))
                    SetMode(ScheduledMode());
                break;

            case GhostMode.Scatter:
            case GhostMode.Chase:
                Move(Speed * deltaSeconds);
                break;

            case GhostMode.Frightened:
                _frightenedTimer += deltaSeconds;
                Move(FrightenedSpeed * deltaSeconds);
                if (_frightenedTimer >= FrightenedSeconds)
                    SetMode(ScheduledMode());
                break;

            case GhostMode.Eaten:
                Move(EyesSpeed * deltaSeconds);
                if (Tile == Maze.HouseCenter)
                {
                    _releaseDelay = 0;
                    SetMode(GhostMode.InHouse);
                }
                break;
        }
    }

    // The schedule switched between scatter and chase.
    public void OnPhaseChanged()
    {
        switch (Mode)
        {
            case GhostMode.Scatter:
            case GhostMode.Chase:
                SetMode(ScheduledMode());
                Reverse();
                break;
        }
    }

    // Pac-Man ate a power pellet.
    public void OnPowerPellet()
    {
        switch (Mode)
        {
            case GhostMode.Scatter:
            case GhostMode.Chase:
            case GhostMode.Frightened:
                SetMode(GhostMode.Frightened);
                break;
        }
    }

    // The ghost and Pac-Man touch.
    public TouchResult Touch()
    {
        switch (Mode)
        {
            case GhostMode.Scatter:
            case GhostMode.Chase:
                return TouchResult.PacManCaught;
            case GhostMode.Frightened:
                SetMode(GhostMode.Eaten);
                return TouchResult.GhostEaten;
            default:
                return TouchResult.Nothing;
        }
    }

    protected override Point ChooseDirection(Point tile)
    {
        bool canUseDoor = Mode is GhostMode.InHouse or GhostMode.Eaten;
        List<Point> options = OpenDirections(tile, canUseDoor);
        if (options.Count == 0)
            return Direction.None;

        switch (Mode)
        {
            case GhostMode.InHouse:
                return Closest(Maze.DoorOutside, options);
            case GhostMode.Scatter:
                return Closest(ScatterTarget, options);
            case GhostMode.Chase:
                return Closest(_world.PacMan.Tile, options);
            case GhostMode.Frightened:
                return options[_world.Random.Next(options.Count)];
            case GhostMode.Eaten:
                if (tile == Maze.DoorOutside)
                    _enteringHouse = true;
                return Closest(_enteringHouse ? Maze.HouseCenter : Maze.DoorOutside, options);
            default:
                return Direction.None;
        }
    }

    // What happens when the ghost enters a mode: another switch.
    private void SetMode(GhostMode mode)
    {
        switch (mode)
        {
            case GhostMode.InHouse:
                _houseTimer = 0;
                break;
            case GhostMode.Frightened:
                _frightenedTimer = 0;
                Reverse();
                break;
            case GhostMode.Eaten:
                _enteringHouse = false;
                break;
        }

        Mode = mode;
    }

    private GhostMode ScheduledMode() => _world.Schedule.Current == Phase.Scatter ? GhostMode.Scatter : GhostMode.Chase;

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
    private Point Closest(Point target, IReadOnlyList<Point> options)
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
