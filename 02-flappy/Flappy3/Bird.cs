using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy3;

public class Bird(Vector2 position)
{
    public Vector2 Position { get; set; } = position;

    public void Update(GameTime gameTime)
    {
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Art.Bird, Position, Color.White);
    }
}