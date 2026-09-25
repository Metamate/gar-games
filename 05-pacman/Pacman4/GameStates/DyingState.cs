using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman4.GameStates;

// Pac-Man was caught: everything stops, the ghosts vanish, and Pac-Man shrivels up.
public class DyingState(Game1 game) : IState
{
    private const float FreezeSeconds = 0.8f;
    private const float Seconds = 2.8f;
    private float _elapsed;

    public void Enter() => _elapsed = 0;
    public void Exit() { }

    public void Update(GameTime gameTime)
    {
        _elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_elapsed < Seconds)
            return;

        World world = game.World;
        world.LoseLife();
        if (world.Lives > 0)
        {
            world.ResetPositions();
            game.ChangeState(game.ReadyState);
        }
        else
        {
            game.ChangeState(game.GameOverState);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        bool frozen = _elapsed < FreezeSeconds;
        game.DrawWorld(drawPacMan: frozen, drawGhosts: frozen);
        if (!frozen)
        {
            game.BeginMaze();
            game.PacManView.DrawDying(spriteBatch, game.World.PacMan, _elapsed - FreezeSeconds);
            spriteBatch.End();
        }
        game.DrawHud();
    }
}
