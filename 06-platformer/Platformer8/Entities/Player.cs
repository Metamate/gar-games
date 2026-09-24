using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer8.States.PlayerStates;
using Platformer8.LevelMaker;

namespace Platformer8.Entities;

public class Player : IEntity
{
    public const int HitboxInset = 2;

    public TextureAtlas Atlas { get; }
    public PlayerStateBase State { get; private set; }
    public GameLevel Level { get; }
    public Tilemap Tilemap => Level.Tilemap;
    public AnimatedSprite Sprite { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float CoyoteTimer { get; set; }
    public int Score { get; set; }

    public Rectangle Bounds => new(
        (int)Position.X + HitboxInset,
        (int)Position.Y,
        (int)Sprite.Width - (HitboxInset * 2),
        (int)Sprite.Height
    );

    public bool Collidable { get; set; } = true;
    public bool IsSolid => true;
    public bool Active { get; set; } = true;

    public Player(TextureAtlas textureAtlas, GameLevel level)
    {
        Atlas = textureAtlas;
        Level = level;
        ChangeState(new PlayerIdleState(this));

        Position = Tilemap.TileToPoint(3, 0);
    }

    public void ChangeState(PlayerStateBase newState)
    {
        State?.Exit();
        State = newState;
        State.Enter();
    }

    public void Update(GameTime gameTime)
    {
        State.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        State.Draw(spriteBatch);
    }

    public bool Collides(IEntity other)
    {
        return Collidable && other.Collidable && Bounds.Intersects(other.Bounds);
    }
}
