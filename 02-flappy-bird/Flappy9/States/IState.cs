using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy9.States;

public interface IState
{
    void Enter();
    void Exit();
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}