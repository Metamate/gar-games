using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman4.GameStates;

public class PlayState(Game1 game) : IState
{
    public void Enter() { }
    public void Exit() { }

    public void Update(GameTime gameTime)
    {
        float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        World world = game.World;

        if (GameController.Up) world.PacMan.Steer(Direction.Up);
        else if (GameController.Down) world.PacMan.Steer(Direction.Down);
        else if (GameController.Left) world.PacMan.Steer(Direction.Left);
        else if (GameController.Right) world.PacMan.Steer(Direction.Right);

        world.Update(deltaSeconds);
        game.MazeView.Update(deltaSeconds);
        game.PacManView.Update(gameTime, world.PacMan);
        game.GhostView.Update(gameTime);

        if (world.PacManCaught)
        {
            game.ChangeState(game.DyingState);
        }
        else if (world.IsCleared)
        {
            world.NextLevel();
            game.ChangeState(game.ReadyState);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        game.DrawWorld();
        game.DrawHud();
    }
}
