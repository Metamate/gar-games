using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario8.Entities;

namespace Mario8.States.SlimeStates;

public abstract class SlimeStateBase
{
    protected const float Gravity = 1000f;
    protected const float ChaseDistance = 80f;

    protected Slime Slime { get; }

    protected SlimeStateBase(Slime slime)
    {
        Slime = slime;
    }

    protected void SetAnimation(string name)
    {
        var animation = Slime.Atlas.GetAnimation(name);
        if (Slime.Sprite == null)
            Slime.Sprite = new AnimatedSprite(animation);
        else
            Slime.Sprite.Play(animation);
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void Update(GameTime gameTime)
    {
        ApplyGravity(gameTime);
        
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Resolve X (Move then Snap)
        Slime.Position = new Vector2(Slime.Position.X + Slime.Velocity.X * dt, Slime.Position.Y);
        ResolveXCollisions();

        // Resolve Y (Move then Snap)
        Slime.Position = new Vector2(Slime.Position.X, Slime.Position.Y + Slime.Velocity.Y * dt);
        if (IsOnGround())
        {
            Rectangle bounds = Slime.Bounds;
            float groundY = Slime.Level.Tilemap.GetTileTop(bounds.Bottom);
            Slime.Position = new Vector2(Slime.Position.X, groundY - Slime.Sprite.Height);
            Slime.Velocity = new Vector2(Slime.Velocity.X, 0);
        }

        Slime.Sprite?.Update(gameTime);
    }

    private void ResolveXCollisions()
    {
        Rectangle bounds = Slime.Bounds;
        if (Slime.Velocity.X > 0)
        {
            if (Slime.Level.Tilemap.IsSolidAt(bounds.Right, bounds.Center.Y))
            {
                float wallX = Slime.Level.Tilemap.GetTileLeft(bounds.Right);
                Slime.Position = new Vector2(wallX - Slime.Sprite.Width, Slime.Position.Y);
                Slime.Velocity = new Vector2(0, Slime.Velocity.Y);
            }
        }
        else if (Slime.Velocity.X < 0)
        {
            if (Slime.Level.Tilemap.IsSolidAt(bounds.Left, bounds.Center.Y))
            {
                float wallX = Slime.Level.Tilemap.GetTileRight(bounds.Left);
                Slime.Position = new Vector2(wallX, Slime.Position.Y);
                Slime.Velocity = new Vector2(0, Slime.Velocity.Y);
            }
        }
    }

    protected virtual void ApplyGravity(GameTime gameTime)
    {
        Slime.Velocity = new Vector2(
            Slime.Velocity.X,
            Slime.Velocity.Y + Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds
        );
    }

    protected bool IsOnGround()
    {
        Rectangle bounds = Slime.Bounds;
        return Slime.Level.Tilemap.IsSolidAt(bounds.Center.X, bounds.Bottom + 1);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Slime.Sprite?.Draw(spriteBatch, Slime.Position);
    }
}
