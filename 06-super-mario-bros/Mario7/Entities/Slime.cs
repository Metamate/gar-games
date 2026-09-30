using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario7.States.SlimeStates;
using Mario7.States.PlayerStates;
using Mario7.LevelMaker;

namespace Mario7.Entities;

public class Slime : IEntity
{
    public TextureAtlas Atlas { get; }
    public GameLevel Level { get; }
    public AnimatedSprite Sprite { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public SlimeStateBase State { get; private set; }

    public Rectangle Bounds => new(
        (int)Position.X,
        (int)Position.Y,
        (int)Sprite.Width,
        (int)Sprite.Height
    );

    public bool Collidable { get; set; } = true;
    public bool IsSolid => false;
    public bool Active { get; set; } = true;

    public Slime(TextureAtlas atlas, GameLevel level, Vector2 position)
    {
        Atlas = atlas;
        Level = level;
        Position = position;
        ChangeState(new SlimeIdleState(this));
    }

    public void ChangeState(SlimeStateBase newState)
    {
        State?.Exit();
        State = newState;
        State.Enter();
    }

    public void Update(GameTime gameTime)
    {
        State?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        State?.Draw(spriteBatch);
    }

    public bool Collides(IEntity other)
    {
        if (!Active || !Collidable || !other.Collidable) return false;

        bool intersects = Bounds.Intersects(other.Bounds);
        
        if (intersects && other is Player player)
        {
            // STOMP CHECK: If player is falling and hits the top half of the slime
            if (player.State is PlayerFallState && player.Bounds.Bottom <= Bounds.Top + 8)
            {
                Active = false;
                player.Score++;
                return false; // No physical collision if it's a kill
            }
            else
            {
                player.Active = false;
            }
        }

        return intersects;
    }
}
