using Microsoft.Xna.Framework;
using Platformer7.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer7.States.SnailStates;

public class SnailWalkState(Snail snail) : SnailStateBase(snail)
{
    private const float WalkSpeed = 15f;
    private int _direction = 1;

    public override void Enter()
    {
        SetAnimation("snail-walk-animation");
        _direction = Snail.Sprite.Effects == SpriteEffects.None ? -1 : 1;
    }

    public override void Update(GameTime gameTime)
    {
        Snail.Velocity = new Vector2(_direction * WalkSpeed, Snail.Velocity.Y);

        base.Update(gameTime);

        if (IsAtEdge() || Snail.Velocity.X == 0)
        {
            _direction *= -1;
            Snail.Sprite.Effects = _direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        if (Snail.Level.Player != null)
        {
            float distance = Vector2.Distance(Snail.Position, Snail.Level.Player.Position);
            if (distance < ChaseDistance)
            {
                Snail.ChangeState(new SnailChaseState(Snail));
            }
        }
    }

    private bool IsAtEdge()
    {
        Rectangle bounds = Snail.Bounds;
        float probeX = _direction > 0 ? bounds.Right : bounds.Left;
        return !Snail.Level.Tilemap.IsSolidAt(probeX, bounds.Bottom + 1);
    }
}
