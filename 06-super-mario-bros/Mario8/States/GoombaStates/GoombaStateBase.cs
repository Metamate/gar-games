using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario8.Entities;

namespace Mario8.States.GoombaStates;

public abstract class GoombaStateBase
{
    protected const float Gravity = 1000f;
    protected const float ChaseDistance = 80f;

    protected Goomba Goomba { get; }

    protected GoombaStateBase(Goomba goomba)
    {
        Goomba = goomba;
    }

    protected void SetAnimation(string name)
    {
        var animation = Goomba.Atlas.GetAnimation(name);
        if (Goomba.Sprite == null)
            Goomba.Sprite = new AnimatedSprite(animation);
        else
            Goomba.Sprite.Play(animation);
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void Update(GameTime gameTime)
    {
        ApplyGravity(gameTime);
        
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Resolve X (Move then Snap)
        Goomba.Position = new Vector2(Goomba.Position.X + Goomba.Velocity.X * dt, Goomba.Position.Y);
        ResolveXCollisions();

        // Resolve Y (Move then Snap)
        Goomba.Position = new Vector2(Goomba.Position.X, Goomba.Position.Y + Goomba.Velocity.Y * dt);
        if (IsOnGround())
        {
            Rectangle bounds = Goomba.Bounds;
            float groundY = Goomba.Level.Tilemap.GetTileTop(bounds.Bottom);
            Goomba.Position = new Vector2(Goomba.Position.X, groundY - Goomba.Sprite.Height);
            Goomba.Velocity = new Vector2(Goomba.Velocity.X, 0);
        }

        Goomba.Sprite?.Update(gameTime);
    }

    private void ResolveXCollisions()
    {
        Rectangle bounds = Goomba.Bounds;
        if (Goomba.Velocity.X > 0)
        {
            if (Goomba.Level.Tilemap.IsSolidAt(bounds.Right, bounds.Center.Y))
            {
                float wallX = Goomba.Level.Tilemap.GetTileLeft(bounds.Right);
                Goomba.Position = new Vector2(wallX - Goomba.Sprite.Width, Goomba.Position.Y);
                Goomba.Velocity = new Vector2(0, Goomba.Velocity.Y);
            }
        }
        else if (Goomba.Velocity.X < 0)
        {
            if (Goomba.Level.Tilemap.IsSolidAt(bounds.Left, bounds.Center.Y))
            {
                float wallX = Goomba.Level.Tilemap.GetTileRight(bounds.Left);
                Goomba.Position = new Vector2(wallX, Goomba.Position.Y);
                Goomba.Velocity = new Vector2(0, Goomba.Velocity.Y);
            }
        }
    }

    protected virtual void ApplyGravity(GameTime gameTime)
    {
        Goomba.Velocity = new Vector2(
            Goomba.Velocity.X,
            Goomba.Velocity.Y + Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds
        );
    }

    protected bool IsOnGround()
    {
        Rectangle bounds = Goomba.Bounds;
        return Goomba.Level.Tilemap.IsSolidAt(bounds.Center.X, bounds.Bottom + 1);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Goomba.Sprite?.Draw(spriteBatch, Goomba.Position);
    }
}
