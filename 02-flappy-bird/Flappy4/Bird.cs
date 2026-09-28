using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy4;

public class Bird(Vector2 position)
{
    private const float Gravity = 980f;
    public Vector2 Position { get; set; } = position;
    public Vector2 Velocity { get; set; } = Vector2.Zero;

    public void Update(GameTime gameTime)
    {
        Velocity += new Vector2(0, Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Art.Bird, Position, Color.White);
    }
}