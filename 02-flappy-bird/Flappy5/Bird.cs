using GMDCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy5;

public class Bird(Vector2 position)
{
    private const float Gravity = 980f;
    private const float JumpForce = 300f;
    public Vector2 Position { get; set; } = position;
    public Vector2 Velocity { get; set; } = Vector2.Zero;

    public void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Space) || Core.Input.Mouse.WasLeftButtonJustPressed)
        {
            Velocity = new Vector2(Velocity.X, -JumpForce);
        }

        Velocity += new Vector2(0, Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Art.Bird, Position, Color.White);
    }
}