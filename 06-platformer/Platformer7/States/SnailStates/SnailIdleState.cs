using Microsoft.Xna.Framework;
using Platformer7.Entities;

namespace Platformer7.States.SnailStates;

public class SnailIdleState(Snail snail) : SnailStateBase(snail)
{
    private float _idleTimer;
    private const float IdleDuration = 2f;

    public override void Enter()
    {
        SetAnimation("snail-idle-animation");
        Snail.Velocity = new Vector2(0, Snail.Velocity.Y);
        _idleTimer = 0f;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _idleTimer += dt;

        if (_idleTimer >= IdleDuration)
        {
            Snail.ChangeState(new SnailWalkState(Snail));
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
}
