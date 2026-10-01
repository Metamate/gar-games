using GARCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Birds4.GameStates;

// The title screen, in front of the first level: Enter hands the slingshot to the player.
public class TitleState(Game1 game) : IState
{
    public void Enter() { }
    public void Exit() { }

    public void Update(GameTime gameTime)
    {
        game.UpdateWorld((float)gameTime.ElapsedGameTime.TotalSeconds);
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
            game.ChangeState(game.AimState);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        game.DrawWorld();
        game.DrawTitle();
    }
}
