using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman4.GameStates;

public class GameOverState(Game1 game) : IState
{
    public void Enter() { }
    public void Exit() { }

    public void Update(GameTime gameTime)
    {
        if (GameController.Start)
        {
            game.World.NewGame();
            game.ChangeState(game.ReadyState);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        game.DrawWorld(drawPacMan: false, drawGhosts: false);
        game.DrawHud();
        game.DrawMessage("GAME OVER", Color.Red);
    }
}
