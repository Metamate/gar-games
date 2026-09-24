using System;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer4.Entities;
using Platformer4.Input;

namespace Platformer4.States.PlayerStates;

public abstract class PlayerStateBase
{
    protected const float MoveSpeed = 100f;
    protected const float Gravity = 1000f;
    protected const int CollisionInset = 1;
    protected const float CoyoteTime = 0.1f;

    protected Player Player { get; }

    protected PlayerStateBase(Player player)
    {
        Player = player;
    }

    protected void SetAnimation(string name)
    {
        var animation = Player.Atlas.GetAnimation(name);
        if (Player.Sprite == null)
            Player.Sprite = new AnimatedSprite(animation);
        else
            Player.Sprite.Play(animation);
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    protected virtual void HandleHorizontalMovement(GameTime gameTime)
    {
        float horizontal = 0;
        if (GameController.Left) horizontal -= 1;
        if (GameController.Right) horizontal += 1;

        Player.Velocity = new Vector2(horizontal * MoveSpeed, Player.Velocity.Y);

        if (horizontal > 0) Player.Sprite.Effects = SpriteEffects.None;
        else if (horizontal < 0) Player.Sprite.Effects = SpriteEffects.FlipHorizontally;
    }

    protected virtual void ApplyGravity(GameTime gameTime)
    {
        Player.Velocity = new Vector2(
            Player.Velocity.X,
            Player.Velocity.Y + Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds
        );
    }

    public virtual void Update(GameTime gameTime)
    {
        HandleHorizontalMovement(gameTime);
        ApplyGravity(gameTime);

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Manage Coyote Time
        if (IsOnGround())
            Player.CoyoteTimer = CoyoteTime;
        else
            Player.CoyoteTimer = Math.Max(0, Player.CoyoteTimer - dt);

        // Resolve X (Move then Snap)
        Player.Position = new Vector2(Player.Position.X + Player.Velocity.X * dt, Player.Position.Y);
        ResolveXCollisions();

        // Resolve Y (Move then Snap)
        Player.Position = new Vector2(Player.Position.X, Player.Position.Y + Player.Velocity.Y * dt);
        ResolveYCollisions();

        Player.Sprite?.Update(gameTime);
    }

    private void ResolveXCollisions()
    {
        Rectangle hitbox = Player.Bounds;
        float tilemapWidth = Player.Tilemap.Columns * Player.Tilemap.TileWidth;

        if (Player.Velocity.X > 0) // Moving Right
        {
            if (hitbox.Right > tilemapWidth) { SnapToRight(tilemapWidth); return; }
            if (CheckSideCollision(hitbox.Right)) SnapToRight(Player.Tilemap.GetTileLeft(hitbox.Right));
        }
        else if (Player.Velocity.X < 0) // Moving Left
        {
            if (hitbox.Left < 0) { SnapToLeft(0); return; }
            if (CheckSideCollision(hitbox.Left)) SnapToLeft(Player.Tilemap.GetTileRight(hitbox.Left));
        }
    }

    private void ResolveYCollisions()
    {
        Rectangle hitbox = Player.Bounds;

        if (Player.Velocity.Y > 0) // Falling
        {
            if (CheckVerticalCollision(hitbox.Bottom))
            {
                SnapToBottom(Player.Tilemap.GetTileTop(hitbox.Bottom));
            }
        }
        else if (Player.Velocity.Y < 0) // Jumping
        {
            if (CheckVerticalCollision(hitbox.Top))
            {
                SnapToTop(Player.Tilemap.GetTileBottom(hitbox.Top));
            }
        }
    }

    private bool CheckSideCollision(float x)
    {
        // Check the left/right edges at the top and bottom (+/- inset)
        return Player.Tilemap.IsSolidAt(x, Player.Bounds.Top + CollisionInset) ||
               Player.Tilemap.IsSolidAt(x, Player.Bounds.Bottom - CollisionInset);
    }

    private bool CheckVerticalCollision(float y)
    {
        // Check the top/bottom edges at the left and right (+/- inset)
        return Player.Tilemap.IsSolidAt(Player.Bounds.Left + CollisionInset, y) ||
               Player.Tilemap.IsSolidAt(Player.Bounds.Right - CollisionInset, y);
    }

    private void SnapToRight(float x)
    {
        Player.Position = new Vector2(x - Player.Sprite.Width + Player.HitboxInset, Player.Position.Y);
        Player.Velocity = new Vector2(0, Player.Velocity.Y);
    }

    private void SnapToLeft(float x)
    {
        Player.Position = new Vector2(x - Player.HitboxInset, Player.Position.Y);
        Player.Velocity = new Vector2(0, Player.Velocity.Y);
    }

    private void SnapToBottom(float y)
    {
        Player.Position = new Vector2(Player.Position.X, y - Player.Sprite.Height);
        Player.Velocity = new Vector2(Player.Velocity.X, 0);
    }

    private void SnapToTop(float y)
    {
        Player.Position = new Vector2(Player.Position.X, y);
        Player.Velocity = new Vector2(Player.Velocity.X, 0);
    }

    protected bool IsOnGround()
    {
        Rectangle hitbox = Player.Bounds;

        return Player.Tilemap.IsSolidAt(hitbox.Left + CollisionInset, hitbox.Bottom + 1) ||
               Player.Tilemap.IsSolidAt(hitbox.Right - CollisionInset, hitbox.Bottom + 1);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Player.Sprite.Draw(spriteBatch, Player.Position);
    }
}
