using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario7.Entities;

namespace Mario7.States.MoleStates;

public abstract class MoleStateBase
{
    protected const float Gravity = 1000f;
    protected const float ChaseDistance = 80f;

    protected Mole Mole { get; }

    protected MoleStateBase(Mole mole)
    {
        Mole = mole;
    }

    protected void SetAnimation(string name)
    {
        var animation = Mole.Atlas.GetAnimation(name);
        if (Mole.Sprite == null)
            Mole.Sprite = new AnimatedSprite(animation);
        else
            Mole.Sprite.Play(animation);
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void Update(GameTime gameTime)
    {
        ApplyGravity(gameTime);
        
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Resolve X (Move then Snap)
        Mole.Position = new Vector2(Mole.Position.X + Mole.Velocity.X * dt, Mole.Position.Y);
        ResolveXCollisions();

        // Resolve Y (Move then Snap)
        Mole.Position = new Vector2(Mole.Position.X, Mole.Position.Y + Mole.Velocity.Y * dt);
        if (IsOnGround())
        {
            Rectangle bounds = Mole.Bounds;
            float groundY = Mole.Level.Tilemap.GetTileTop(bounds.Bottom);
            Mole.Position = new Vector2(Mole.Position.X, groundY - Mole.Sprite.Height);
            Mole.Velocity = new Vector2(Mole.Velocity.X, 0);
        }

        Mole.Sprite?.Update(gameTime);
    }

    private void ResolveXCollisions()
    {
        Rectangle bounds = Mole.Bounds;
        if (Mole.Velocity.X > 0)
        {
            if (Mole.Level.Tilemap.IsSolidAt(bounds.Right, bounds.Center.Y))
            {
                float wallX = Mole.Level.Tilemap.GetTileLeft(bounds.Right);
                Mole.Position = new Vector2(wallX - Mole.Sprite.Width, Mole.Position.Y);
                Mole.Velocity = new Vector2(0, Mole.Velocity.Y);
            }
        }
        else if (Mole.Velocity.X < 0)
        {
            if (Mole.Level.Tilemap.IsSolidAt(bounds.Left, bounds.Center.Y))
            {
                float wallX = Mole.Level.Tilemap.GetTileRight(bounds.Left);
                Mole.Position = new Vector2(wallX, Mole.Position.Y);
                Mole.Velocity = new Vector2(0, Mole.Velocity.Y);
            }
        }
    }

    protected virtual void ApplyGravity(GameTime gameTime)
    {
        Mole.Velocity = new Vector2(
            Mole.Velocity.X,
            Mole.Velocity.Y + Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds
        );
    }

    protected bool IsOnGround()
    {
        Rectangle bounds = Mole.Bounds;
        return Mole.Level.Tilemap.IsSolidAt(bounds.Center.X, bounds.Bottom + 1);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Mole.Sprite?.Draw(spriteBatch, Mole.Position);
    }
}
