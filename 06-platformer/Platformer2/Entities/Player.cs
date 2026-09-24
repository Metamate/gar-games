using System;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer2.Input;
using Platformer2.LevelMaker;

namespace Platformer2.Entities;

public enum PlayerState
{
    Idle,
    Walking,
    Ducking,
    Jumping,
    Falling,
}

// The player's state is an enum, and the behaviour for each state lives in switch statements.
// An enum is better than a set of booleans (isJumping, isDucking, ...) because the player can
// only be in one state at a time. But every new state means another case in every switch,
// and there is nowhere natural to keep data that belongs to a single state.
// In the next step, each state becomes its own class: the State pattern.
public class Player
{
    public const int HitboxInset = 2;
    private const float MoveSpeed = 100f;
    private const float Gravity = 1000f;
    private const float JumpImpulse = -300f;
    private const int CollisionInset = 1;
    private const float CoyoteTime = 0.1f;

    public TextureAtlas Atlas { get; }
    public PlayerState State { get; private set; }
    public GameLevel Level { get; }
    public Tilemap Tilemap => Level.Tilemap;
    public AnimatedSprite Sprite { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float CoyoteTimer { get; set; }

    public Rectangle Bounds => new(
        (int)Position.X + HitboxInset,
        (int)Position.Y,
        (int)Sprite.Width - (HitboxInset * 2),
        (int)Sprite.Height
    );

    public Player(TextureAtlas textureAtlas, GameLevel level)
    {
        Atlas = textureAtlas;
        Level = level;
        ChangeState(PlayerState.Idle);

        Position = Tilemap.TileToPoint(3, 0);
    }

    public void ChangeState(PlayerState newState)
    {
        State = newState;

        switch (State)
        {
            case PlayerState.Idle:
                SetAnimation("idle-animation");
                break;
            case PlayerState.Walking:
                SetAnimation("walk-animation");
                break;
            case PlayerState.Ducking:
                SetAnimation("duck-animation");
                break;
            case PlayerState.Jumping:
                SetAnimation("jump-animation");
                Velocity = new Vector2(Velocity.X, JumpImpulse);
                break;
            case PlayerState.Falling:
                SetAnimation("fall-animation");
                break;
        }
    }

    public void Update(GameTime gameTime)
    {
        // Movement and physics are shared by all states...
        HandleHorizontalMovement();
        ApplyGravity(gameTime);
        MoveAndCollide(gameTime);
        Sprite.Update(gameTime);

        // ...but which state comes next depends on the current state.
        switch (State)
        {
            case PlayerState.Idle:
                if (Velocity.X != 0) ChangeState(PlayerState.Walking);
                if (GameController.Down) ChangeState(PlayerState.Ducking);
                if (GameController.Jump) ChangeState(PlayerState.Jumping);
                if (!IsOnGround()) ChangeState(PlayerState.Falling);
                break;

            case PlayerState.Walking:
                if (Velocity.X == 0) ChangeState(PlayerState.Idle);
                if (GameController.Down) ChangeState(PlayerState.Ducking);
                if (GameController.Jump) ChangeState(PlayerState.Jumping);
                if (!IsOnGround()) ChangeState(PlayerState.Falling);
                break;

            case PlayerState.Ducking:
                if (!GameController.Down) ChangeState(PlayerState.Idle);
                if (!IsOnGround()) ChangeState(PlayerState.Falling);
                break;

            case PlayerState.Jumping:
                if (Velocity.Y > 0) ChangeState(PlayerState.Falling);
                break;

            case PlayerState.Falling:
                if (IsOnGround())
                {
                    ChangeState(Velocity.X == 0 ? PlayerState.Idle : PlayerState.Walking);
                }
                else if (CoyoteTimer > 0 && GameController.Jump)
                {
                    ChangeState(PlayerState.Jumping);
                }
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Sprite.Draw(spriteBatch, Position);
    }

    private void SetAnimation(string name)
    {
        var animation = Atlas.GetAnimation(name);
        if (Sprite == null)
            Sprite = new AnimatedSprite(animation);
        else
            Sprite.Play(animation);
    }

    private void HandleHorizontalMovement()
    {
        float horizontal = 0;
        if (GameController.Left) horizontal -= 1;
        if (GameController.Right) horizontal += 1;

        if (horizontal > 0) Sprite.Effects = SpriteEffects.None;
        else if (horizontal < 0) Sprite.Effects = SpriteEffects.FlipHorizontally;

        // Yet another special case: a ducking player can turn around, but not move.
        if (State == PlayerState.Ducking)
        {
            horizontal = 0;
        }

        Velocity = new Vector2(horizontal * MoveSpeed, Velocity.Y);
    }

    private void ApplyGravity(GameTime gameTime)
    {
        Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    private void MoveAndCollide(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Coyote time: the player can still jump for a moment after walking off a ledge.
        if (IsOnGround())
            CoyoteTimer = CoyoteTime;
        else
            CoyoteTimer = Math.Max(0, CoyoteTimer - dt);

        // Move and resolve one axis at a time: X first, then Y.
        Position = new Vector2(Position.X + Velocity.X * dt, Position.Y);
        ResolveXCollisions();

        Position = new Vector2(Position.X, Position.Y + Velocity.Y * dt);
        ResolveYCollisions();
    }

    private void ResolveXCollisions()
    {
        Rectangle hitbox = Bounds;
        float tilemapWidth = Tilemap.Columns * Tilemap.TileWidth;

        if (Velocity.X > 0) // Moving Right
        {
            if (hitbox.Right > tilemapWidth) { SnapToRight(tilemapWidth); return; }
            if (CheckSideCollision(hitbox.Right)) SnapToRight(Tilemap.GetTileLeft(hitbox.Right));
        }
        else if (Velocity.X < 0) // Moving Left
        {
            if (hitbox.Left < 0) { SnapToLeft(0); return; }
            if (CheckSideCollision(hitbox.Left)) SnapToLeft(Tilemap.GetTileRight(hitbox.Left));
        }
    }

    private void ResolveYCollisions()
    {
        Rectangle hitbox = Bounds;

        if (Velocity.Y > 0) // Falling
        {
            if (CheckVerticalCollision(hitbox.Bottom)) SnapToBottom(Tilemap.GetTileTop(hitbox.Bottom));
        }
        else if (Velocity.Y < 0) // Jumping
        {
            if (CheckVerticalCollision(hitbox.Top)) SnapToTop(Tilemap.GetTileBottom(hitbox.Top));
        }
    }

    private bool CheckSideCollision(float x)
    {
        // Check the left/right edges at the top and bottom (+/- inset)
        return Tilemap.IsSolidAt(x, Bounds.Top + CollisionInset) ||
               Tilemap.IsSolidAt(x, Bounds.Bottom - CollisionInset);
    }

    private bool CheckVerticalCollision(float y)
    {
        // Check the top/bottom edges at the left and right (+/- inset)
        return Tilemap.IsSolidAt(Bounds.Left + CollisionInset, y) ||
               Tilemap.IsSolidAt(Bounds.Right - CollisionInset, y);
    }

    private void SnapToRight(float x)
    {
        Position = new Vector2(x - Sprite.Width + HitboxInset, Position.Y);
        Velocity = new Vector2(0, Velocity.Y);
    }

    private void SnapToLeft(float x)
    {
        Position = new Vector2(x - HitboxInset, Position.Y);
        Velocity = new Vector2(0, Velocity.Y);
    }

    private void SnapToBottom(float y)
    {
        Position = new Vector2(Position.X, y - Sprite.Height);
        Velocity = new Vector2(Velocity.X, 0);
    }

    private void SnapToTop(float y)
    {
        Position = new Vector2(Position.X, y);
        Velocity = new Vector2(Velocity.X, 0);
    }

    private bool IsOnGround()
    {
        Rectangle hitbox = Bounds;
        return Tilemap.IsSolidAt(hitbox.Left + CollisionInset, hitbox.Bottom + 1) ||
               Tilemap.IsSolidAt(hitbox.Right - CollisionInset, hitbox.Bottom + 1);
    }
}
