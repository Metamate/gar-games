using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Birds4.GameStates;

// A state of the game, as in Flappy Bird and Pac-Man.
public interface IState
{
    void Enter();
    void Exit();
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
