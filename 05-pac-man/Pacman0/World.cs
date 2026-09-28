namespace Pacman0;

// Everything in a game of Pac-Man, without the drawing: the maze, Pac-Man and the score.
public class World
{
    public World(string mazeText)
    {
        Maze = Maze.Parse(mazeText);
        PacMan = new PacMan(Maze);
        PacMan.Reset();
    }

    public Maze Maze { get; }
    public PacMan PacMan { get; }
    public int Score { get; private set; }
    public bool IsCleared => Maze.DotsLeft == 0;

    public void Update(float deltaSeconds)
    {
        PacMan.Update(deltaSeconds);

        Score += Maze.Eat(PacMan.Tile) switch
        {
            Dot.Small => 10,
            Dot.Power => 50,
            _ => 0,
        };
    }

    public void NextLevel()
    {
        Maze.ResetDots();
        PacMan.Reset();
    }
}
