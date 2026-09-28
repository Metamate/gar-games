using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Pacman1;

// Everything in a game of Pac-Man, without the drawing: the maze, Pac-Man, the ghosts and the
// score. It can run in a unit test, with a small maze and a seeded Random.
public class World
{
    private int _ghostsEaten;   // since the last power pellet: each is worth twice the last

    public World(string mazeText, Random random = null)
    {
        Maze = Maze.Parse(mazeText);
        Random = random ?? new Random();
        PacMan = new PacMan(Maze);

        // Each ghost has its own corner to scatter to (outside the maze), and waits a little
        // longer in the house.
        Blinky = new Ghost(this, "blinky", Maze.StartOf('b'), new Point(Maze.Width - 3, -3), 0);
        Pinky = new Ghost(this, "pinky", Maze.StartOf('p'), new Point(2, -3), 0);
        Inky = new Ghost(this, "inky", Maze.StartOf('i'), new Point(Maze.Width - 1, Maze.Height + 1), 4);
        Clyde = new Ghost(this, "clyde", Maze.StartOf('c'), new Point(0, Maze.Height + 1), 8);
        Ghosts = [Blinky, Pinky, Inky, Clyde];

        ResetPositions();
    }

    public Maze Maze { get; }
    public PacMan PacMan { get; }
    public Ghost Blinky { get; }
    public Ghost Pinky { get; }
    public Ghost Inky { get; }
    public Ghost Clyde { get; }
    public IReadOnlyList<Ghost> Ghosts { get; }
    public ModeSchedule Schedule { get; } = new();
    public Random Random { get; }
    public int Score { get; private set; }
    public bool PacManCaught { get; private set; }
    public bool IsCleared => Maze.DotsLeft == 0;

    public void Update(float deltaSeconds)
    {
        PacMan.Update(deltaSeconds);
        EatDots();

        // The schedule pauses while ghosts are frightened.
        if (!Ghosts.Any(ghost => ghost.IsFrightened) && Schedule.Update(deltaSeconds))
        {
            foreach (Ghost ghost in Ghosts)
                ghost.OnPhaseChanged();
        }

        foreach (Ghost ghost in Ghosts)
            ghost.Update(deltaSeconds);

        foreach (Ghost ghost in Ghosts)
        {
            if (!Touching(ghost))
                continue;

            switch (ghost.Touch())
            {
                case TouchResult.GhostEaten:
                    Score += 200 << _ghostsEaten++;
                    break;
                case TouchResult.PacManCaught:
                    PacManCaught = true;
                    break;
            }
        }
    }

    // After Pac-Man is caught: everyone back to the start, but the dots stay eaten.
    public void ResetPositions()
    {
        PacMan.Reset();
        Schedule.Reset();
        foreach (Ghost ghost in Ghosts)
            ghost.Reset();
        PacManCaught = false;
    }

    public void NextLevel()
    {
        Maze.ResetDots();
        ResetPositions();
    }

    private void EatDots()
    {
        switch (Maze.Eat(PacMan.Tile))
        {
            case Dot.Small:
                Score += 10;
                break;
            case Dot.Power:
                Score += 50;
                _ghostsEaten = 0;
                foreach (Ghost ghost in Ghosts)
                    ghost.OnPowerPellet();
                break;
        }
    }

    private bool Touching(Ghost ghost) => Vector2.Distance(PacMan.Position, ghost.Position) < Maze.TileSize * 0.75f;
}
