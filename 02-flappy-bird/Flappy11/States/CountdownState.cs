using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy11.States;

public class CountdownState(Game1 game) : IState
{
    private const int Count = 3;
    private const float Interval = 0.5f;
    private int _currentCount;
    private float _timer;

    public void Enter()
    {
        _currentCount = Count;
        _timer = 0f;
    }

    public void Exit()
    {
    }

    public void Update(GameTime gameTime)
    {
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_timer >= Interval)
        {
            _timer -= Interval;
            _currentCount--;

            if (_currentCount <= 0)
            {
                game.GameState.ChangeState(game.GameState.PlayState);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(Art.Font, _currentCount.ToString(), new Vector2(Game1.VirtualWidth, Game1.VirtualHeight) / 2f, Color.White, 0f, Art.Font.MeasureString(_currentCount.ToString()) * 0.5f, 0.8f, SpriteEffects.None, 0f);
    }
}