using System;
using Microsoft.Xna.Framework;
using Mario7.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Mario7.States.SlimeStates;

public class SlimeChaseState(Slime slime) : SlimeStateBase(slime)
{
    private const float ChaseSpeed = 30f;

    public override void Enter()
    {
        SetAnimation("slime-walk-animation");
    }

    public override void Update(GameTime gameTime)
    {
        if (Slime.Level.Player == null)
        {
            Slime.ChangeState(new SlimeIdleState(Slime));
            return;
        }

        float dx = Slime.Level.Player.Position.X - Slime.Position.X;
        float distance = Math.Abs(dx);

        if (distance > ChaseDistance * 1.5f)
        {
            Slime.ChangeState(new SlimeWalkState(Slime));
            return;
        }

        // Only update direction if outside a small "dead-zone" to prevent flickering
        if (distance > 5f)
        {
            int direction = dx > 0 ? 1 : -1;
            Slime.Velocity = new Vector2(direction * ChaseSpeed, Slime.Velocity.Y);
            Slime.Sprite.Effects = direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        base.Update(gameTime);
    }
}
