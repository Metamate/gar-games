using Microsoft.Xna.Framework;
using Mario7.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Mario7.States.MoleStates;

public class MoleWalkState(Mole mole) : MoleStateBase(mole)
{
    private const float WalkSpeed = 15f;
    private int _direction = 1;

    public override void Enter()
    {
        SetAnimation("mole-walk-animation");
        _direction = Mole.Sprite.Effects == SpriteEffects.None ? -1 : 1;
    }

    public override void Update(GameTime gameTime)
    {
        Mole.Velocity = new Vector2(_direction * WalkSpeed, Mole.Velocity.Y);

        base.Update(gameTime);

        if (IsAtEdge() || Mole.Velocity.X == 0)
        {
            _direction *= -1;
            Mole.Sprite.Effects = _direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        if (Mole.Level.Player != null)
        {
            float distance = Vector2.Distance(Mole.Position, Mole.Level.Player.Position);
            if (distance < ChaseDistance)
            {
                Mole.ChangeState(new MoleChaseState(Mole));
            }
        }
    }

    private bool IsAtEdge()
    {
        Rectangle bounds = Mole.Bounds;
        float probeX = _direction > 0 ? bounds.Right : bounds.Left;
        return !Mole.Level.Tilemap.IsSolidAt(probeX, bounds.Bottom + 1);
    }
}
