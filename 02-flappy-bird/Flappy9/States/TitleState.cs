using GARCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy9.States;

public class TitleState(Game1 game) : IState
{
    public void Enter()
    {
        game.IsScrolling = false;
    }

    public void Exit()
    {
    }

    public void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter) || Core.Input.Keyboard.WasKeyJustPressed(Keys.Space) || Core.Input.Mouse.WasLeftButtonJustPressed)
        {
            game.GameState.ChangeState(game.GameState.PlayState);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Art.DrawTitle(spriteBatch, "Flappy Bird", "Space or click: flap", Game1.VirtualWidth, Game1.VirtualHeight);
    }
}