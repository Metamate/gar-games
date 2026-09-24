using System;
using Microsoft.Xna.Framework;
using Platformer7.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer7.States.SnailStates;

public class SnailChaseState(Snail snail) : SnailStateBase(snail)
{
    private const float ChaseSpeed = 30f;

    public override void Enter()
    {
        SetAnimation("snail-walk-animation");
    }

    public override void Update(GameTime gameTime)
    {
        if (Snail.Level.Player == null)
        {
            Snail.ChangeState(new SnailIdleState(Snail));
            return;
        }

        float dx = Snail.Level.Player.Position.X - Snail.Position.X;
        float distance = Math.Abs(dx);

        if (distance > ChaseDistance * 1.5f)
        {
            Snail.ChangeState(new SnailWalkState(Snail));
            return;
        }

        // Only update direction if outside a small "dead-zone" to prevent flickering
        if (distance > 5f)
        {
            int direction = dx > 0 ? 1 : -1;
            Snail.Velocity = new Vector2(direction * ChaseSpeed, Snail.Velocity.Y);
            Snail.Sprite.Effects = direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        base.Update(gameTime);
    }
}
