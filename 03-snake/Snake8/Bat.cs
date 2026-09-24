using System;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake8;

public class Bat(AnimatedSprite sprite, Rectangle roomBounds)
{
    private const float Speed = 100f;
    private readonly AnimatedSprite _sprite = sprite;
    private readonly Rectangle _roomBounds = roomBounds;
    private Vector2 _velocity;
    public Vector2 Position { get; set; }
    public Circle Bounds => new(
        (int)(Position.X + (_sprite.Width * 0.5f)),
        (int)(Position.Y + (_sprite.Height * 0.5f)),
        (int)(_sprite.Width * 0.5f)
    );

    public void Update(GameTime gameTime)
    {
        Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _sprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, Position);
    }

    public void Bounce(Vector2 normal)
    {
        // Move slightly away from the wall in the direction of the normal,
        // so the bat doesn't stay stuck inside the wall.
        Position += new Vector2(normal.X * _sprite.Width, normal.Y * _sprite.Height) * 0.1f;

        // Reflect the velocity off the wall.
        normal.Normalize();
        _velocity = Vector2.Reflect(_velocity, normal);
    }

    public void RandomizeVelocity()
    {
        // Pick a random angle and convert it to a direction vector.
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);
        Vector2 direction = new((float)Math.Cos(angle), (float)Math.Sin(angle));

        _velocity = direction * Speed;
    }

    public void RandomizePosition()
    {
        // Divide the room into a grid of bat-sized cells and pick a random cell.
        int columns = _roomBounds.Width / (int)_sprite.Width;
        int rows = _roomBounds.Height / (int)_sprite.Height;
        int column = Random.Shared.Next(0, columns);
        int row = Random.Shared.Next(0, rows);

        Position = new Vector2(
            _roomBounds.X + column * _sprite.Width,
            _roomBounds.Y + row * _sprite.Height
        );
    }
}
