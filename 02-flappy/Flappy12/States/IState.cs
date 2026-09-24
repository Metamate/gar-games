using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy12.States;

public interface IState
{
    void Enter();
    void Exit();
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}