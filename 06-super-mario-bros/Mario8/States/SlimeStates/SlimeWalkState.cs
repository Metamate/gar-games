using Microsoft.Xna.Framework;
using Mario8.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Mario8.States.SlimeStates;

public class SlimeWalkState(Slime slime) : SlimeStateBase(slime)
{
    private const float WalkSpeed = 15f;
    private int _direction = 1;

    public override void Enter()
    {
        SetAnimation("slime-walk-animation");
        _direction = Slime.Sprite.Effects == SpriteEffects.None ? -1 : 1;
    }

    public override void Update(GameTime gameTime)
    {
        Slime.Velocity = new Vector2(_direction * WalkSpeed, Slime.Velocity.Y);

        base.Update(gameTime);

        if (IsAtEdge() || Slime.Velocity.X == 0)
        {
            _direction *= -1;
            Slime.Sprite.Effects = _direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        if (Slime.Level.Player != null)
        {
            float distance = Vector2.Distance(Slime.Position, Slime.Level.Player.Position);
            if (distance < ChaseDistance)
            {
                Slime.ChangeState(new SlimeChaseState(Slime));
            }
        }
    }

    private bool IsAtEdge()
    {
        Rectangle bounds = Slime.Bounds;
        float probeX = _direction > 0 ? bounds.Right : bounds.Left;
        return !Slime.Level.Tilemap.IsSolidAt(probeX, bounds.Bottom + 1);
    }
}
