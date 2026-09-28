using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman4.GameStates;

public class StateMachine
{
    private IState _currentState;

    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Update(GameTime gameTime) => _currentState?.Update(gameTime);

    public void Draw(SpriteBatch spriteBatch) => _currentState?.Draw(spriteBatch);
}
