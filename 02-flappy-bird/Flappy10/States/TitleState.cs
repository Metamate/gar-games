using GARCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy10.States;

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
            game.GameState.ChangeState(game.GameState.PlayState);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        string title = "Flappy Bird";
        Art.DrawCentred(spriteBatch, Art.Font, title, new Vector2(Game1.VirtualWidth / 2, Game1.VirtualHeight / 2 - 40));
        string prompt = "Press Enter";
        Art.DrawCentred(spriteBatch, Art.SmallFont, prompt, new Vector2(Game1.VirtualWidth / 2, Game1.VirtualHeight / 2 + 12));
    }
}