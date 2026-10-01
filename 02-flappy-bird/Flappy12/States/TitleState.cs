using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy12.States;

public class TitleState(Game1 game) : IState
{
    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter) || Core.Input.Keyboard.WasKeyJustPressed(Keys.Space) || Core.Input.Mouse.WasLeftButtonJustPressed)
        {
            game.GameState.ChangeState(game.GameState.CountdownState);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        TitleScreen.Draw(spriteBatch, Art.Font, Art.SmallFont, "Flappy Bird", "Space or click: flap", Game1.VirtualWidth, Game1.VirtualHeight);
    }
}