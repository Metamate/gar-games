using System;
using Microsoft.Xna.Framework;
using Mario8.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Mario8.States.GoombaStates;

public class GoombaChaseState(Goomba goomba) : GoombaStateBase(goomba)
{
    private const float ChaseSpeed = 30f;

    public override void Enter()
    {
        SetAnimation("goomba-walk-animation");
    }

    public override void Update(GameTime gameTime)
    {
        if (Goomba.Level.Player == null)
        {
            Goomba.ChangeState(new GoombaIdleState(Goomba));
            return;
        }

        float dx = Goomba.Level.Player.Position.X - Goomba.Position.X;
        float distance = Math.Abs(dx);

        if (distance > ChaseDistance * 1.5f)
        {
            Goomba.ChangeState(new GoombaWalkState(Goomba));
            return;
        }

        // Only update direction if outside a small "dead-zone" to prevent flickering
        if (distance > 5f)
        {
            int direction = dx > 0 ? 1 : -1;
            Goomba.Velocity = new Vector2(direction * ChaseSpeed, Goomba.Velocity.Y);
            Goomba.Sprite.Effects = direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        base.Update(gameTime);
    }
}
