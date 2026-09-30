using GARCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pvz4.GameStates;

// The title screen, over the empty lawn: Enter starts the game.
public class TitleState(Game1 game) : IState
{
    public void Enter() { }
    public void Exit() { }

    public void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
            game.ChangeState(game.PlayState);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        game.DrawGame(showCursor: false);
        game.DrawTitle("Plants vs. Zombies");
    }
}
