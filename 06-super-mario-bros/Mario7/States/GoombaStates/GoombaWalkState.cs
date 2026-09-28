using Microsoft.Xna.Framework;
using Mario7.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Mario7.States.GoombaStates;

public class GoombaWalkState(Goomba goomba) : GoombaStateBase(goomba)
{
    private const float WalkSpeed = 15f;
    private int _direction = 1;

    public override void Enter()
    {
        SetAnimation("goomba-walk-animation");
        _direction = Goomba.Sprite.Effects == SpriteEffects.None ? -1 : 1;
    }

    public override void Update(GameTime gameTime)
    {
        Goomba.Velocity = new Vector2(_direction * WalkSpeed, Goomba.Velocity.Y);

        base.Update(gameTime);

        if (IsAtEdge() || Goomba.Velocity.X == 0)
        {
            _direction *= -1;
            Goomba.Sprite.Effects = _direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        if (Goomba.Level.Player != null)
        {
            float distance = Vector2.Distance(Goomba.Position, Goomba.Level.Player.Position);
            if (distance < ChaseDistance)
            {
                Goomba.ChangeState(new GoombaChaseState(Goomba));
            }
        }
    }

    private bool IsAtEdge()
    {
        Rectangle bounds = Goomba.Bounds;
        float probeX = _direction > 0 ? bounds.Right : bounds.Left;
        return !Goomba.Level.Tilemap.IsSolidAt(probeX, bounds.Bottom + 1);
    }
}
