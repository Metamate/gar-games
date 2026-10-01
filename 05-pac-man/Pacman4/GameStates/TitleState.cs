using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman4.GameStates;

// The title screen: the empty maze, the game's name, and how to start.
public class TitleState(Game1 game) : IState
{
    public void Enter() { }
    public void Exit() { }

    public void Update(GameTime gameTime)
    {
        if (GameController.Start)
            game.ChangeState(game.ReadyState);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        game.DrawWorld(drawPacMan: false, drawGhosts: false);
        game.DrawTitle();
    }
}
