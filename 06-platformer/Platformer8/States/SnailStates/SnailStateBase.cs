using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer8.Entities;

namespace Platformer8.States.SnailStates;

public abstract class SnailStateBase
{
    protected const float Gravity = 1000f;
    protected const float ChaseDistance = 80f;

    protected Snail Snail { get; }

    protected SnailStateBase(Snail snail)
    {
        Snail = snail;
    }

    protected void SetAnimation(string name)
    {
        var animation = Snail.Atlas.GetAnimation(name);
        if (Snail.Sprite == null)
            Snail.Sprite = new AnimatedSprite(animation);
        else
            Snail.Sprite.Play(animation);
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void Update(GameTime gameTime)
    {
        ApplyGravity(gameTime);
        
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Resolve X (Move then Snap)
        Snail.Position = new Vector2(Snail.Position.X + Snail.Velocity.X * dt, Snail.Position.Y);
        ResolveXCollisions();

        // Resolve Y (Move then Snap)
        Snail.Position = new Vector2(Snail.Position.X, Snail.Position.Y + Snail.Velocity.Y * dt);
        if (IsOnGround())
        {
            Rectangle bounds = Snail.Bounds;
            float groundY = Snail.Level.Tilemap.GetTileTop(bounds.Bottom);
            Snail.Position = new Vector2(Snail.Position.X, groundY - Snail.Sprite.Height);
            Snail.Velocity = new Vector2(Snail.Velocity.X, 0);
        }

        Snail.Sprite?.Update(gameTime);
    }

    private void ResolveXCollisions()
    {
        Rectangle bounds = Snail.Bounds;
        if (Snail.Velocity.X > 0)
        {
            if (Snail.Level.Tilemap.IsSolidAt(bounds.Right, bounds.Center.Y))
            {
                float wallX = Snail.Level.Tilemap.GetTileLeft(bounds.Right);
                Snail.Position = new Vector2(wallX - Snail.Sprite.Width, Snail.Position.Y);
                Snail.Velocity = new Vector2(0, Snail.Velocity.Y);
            }
        }
        else if (Snail.Velocity.X < 0)
        {
            if (Snail.Level.Tilemap.IsSolidAt(bounds.Left, bounds.Center.Y))
            {
                float wallX = Snail.Level.Tilemap.GetTileRight(bounds.Left);
                Snail.Position = new Vector2(wallX, Snail.Position.Y);
                Snail.Velocity = new Vector2(0, Snail.Velocity.Y);
            }
        }
    }

    protected virtual void ApplyGravity(GameTime gameTime)
    {
        Snail.Velocity = new Vector2(
            Snail.Velocity.X,
            Snail.Velocity.Y + Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds
        );
    }

    protected bool IsOnGround()
    {
        Rectangle bounds = Snail.Bounds;
        return Snail.Level.Tilemap.IsSolidAt(bounds.Center.X, bounds.Bottom + 1);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Snail.Sprite?.Draw(spriteBatch, Snail.Position);
    }
}
