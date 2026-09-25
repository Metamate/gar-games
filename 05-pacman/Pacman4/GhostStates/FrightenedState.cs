using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman4.GhostStates;

// After a power pellet: slow, blue, wandering at random, and edible.
public class FrightenedState : GhostState
{
    public const float Speed = 4.5f * Maze.TileSize;
    public const float Seconds = 6;
    public const float FlashSeconds = 2;

    private float _elapsed;

    public override GhostLook Look => _elapsed > Seconds - FlashSeconds ? GhostLook.FrightenedEnding : GhostLook.Frightened;

    // Every ghost turns around when it becomes frightened: a sign for the player.
    public override void Enter(Ghost ghost) => ghost.Reverse();

    public override void Update(Ghost ghost, float deltaSeconds)
    {
        _elapsed += deltaSeconds;
        ghost.Move(Speed * deltaSeconds);
        if (_elapsed >= Seconds)
            ghost.ChangeState(ghost.ScheduledState());
    }

    public override Point ChooseDirection(Ghost ghost, IReadOnlyList<Point> options)
        => options[ghost.World.Random.Next(options.Count)];

    // Another power pellet: frightened for longer.
    public override void OnPowerPellet(Ghost ghost) => ghost.ChangeState(new FrightenedState());

    public override TouchResult OnTouch(Ghost ghost)
    {
        ghost.ChangeState(new EatenState());
        return TouchResult.GhostEaten;
    }
}
