using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy11.States;

public class StateMachine(Game1 game)
{
    private IState _currentState;

    public TitleState TitleState { get; private set; } = new TitleState(game);
    public PlayState PlayState { get; private set; } = new PlayState(game);
    public ScoreState ScoreState { get; private set; } = new ScoreState(game);
    public CountdownState CountdownState { get; private set; } = new CountdownState(game);

    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Update(GameTime gameTime)
    {
        _currentState?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _currentState?.Draw(spriteBatch);
    }
}
