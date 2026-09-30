using GARCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy11.States;

public class ScoreState(Game1 game) : IState
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
            game.GameState.ChangeState(game.GameState.CountdownState);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var scoreText = $"Score: {game.GameState.PlayState.Score}";
        Art.DrawCentred(spriteBatch, Art.Font, scoreText, new Vector2(Game1.VirtualWidth / 2, Game1.VirtualHeight / 2 - 20));
        string prompt = "Press Enter to play again";
        Art.DrawCentred(spriteBatch, Art.SmallFont, prompt, new Vector2(Game1.VirtualWidth / 2, Game1.VirtualHeight / 2 + 20));
    }
}