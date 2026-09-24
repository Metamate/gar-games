using GMDCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy11.States;

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
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Space) || Core.Input.Mouse.WasLeftButtonJustPressed)
        {
            game.GameState.ChangeState(game.GameState.CountdownState);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        string title = "Flappy Bird";
        spriteBatch.DrawString(Art.Font, title, new Vector2(Game1.VirtualWidth / 2f, Game1.VirtualHeight / 2f - 40), Color.White, 0f, Art.Font.MeasureString(title) * 0.5f, 0.6f, SpriteEffects.None, 0f);
        string prompt = "Press Space or Click to Start!";
        spriteBatch.DrawString(Art.Font, prompt, new Vector2(Game1.VirtualWidth / 2f, Game1.VirtualHeight / 2f + 20), Color.White, 0f, Art.Font.MeasureString(prompt) * 0.5f, 0.2f, SpriteEffects.None, 0f);
    }
}