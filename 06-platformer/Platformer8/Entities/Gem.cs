using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer8.LevelMaker;
using Platformer8.Audio;

namespace Platformer8.Entities;

public class Gem(TextureRegion region, Vector2 position, Vector2 velocity) : IEntity
{
    public bool Collidable { get; set; } = true;
    public bool IsSolid => false;
    public bool Active { get; set; } = true;
    public Vector2 Position { get; set; } = position;
    public Vector2 Velocity { get; set; } = velocity;
    public TextureRegion Region { get; set; } = region;

    private float _timer = 0;
    private const float Gravity = 800f;

    public Rectangle Bounds => new((int)Position.X, (int)Position.Y, Region.Width, Region.Height);

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Simple physics for the gem animation
        Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * dt);
        Position += Velocity * dt;

        _timer += dt;
        if (_timer > 2.0f) // Despawn after 2 seconds
        {
            Active = false;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Region.Draw(spriteBatch, Position, Color.White);
    }

    public bool Collides(IEntity other)
    {
        if (Active && other is Player player)
        {
            if (Bounds.Intersects(player.Bounds))
            {
                Active = false; // Gem is collected
                player.Score++;
                SoundManager.PlayPickup();
                return true;
            }
        }
        return false;
    }
}
