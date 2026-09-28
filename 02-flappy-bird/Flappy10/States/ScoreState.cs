using GMDCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy10.States;

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
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Space) || Core.Input.Mouse.WasLeftButtonJustPressed)
        {
            game.GameState.ChangeState(game.GameState.PlayState);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var scoreText = $"Score: {game.GameState.PlayState.Score}";
        var scoreTextOrigin = Art.Font.MeasureString(scoreText) * 0.5f;
        var scoreTextPosition = new Vector2(Game1.VirtualWidth, Game1.VirtualHeight) / 2f;
        spriteBatch.DrawString(Art.Font, scoreText, scoreTextPosition, Color.White, 0f, scoreTextOrigin, 0.6f, SpriteEffects.None, 0f);
    }
}