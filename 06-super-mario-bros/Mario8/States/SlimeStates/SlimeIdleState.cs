using Microsoft.Xna.Framework;
using Mario8.Entities;

namespace Mario8.States.SlimeStates;

public class SlimeIdleState(Slime slime) : SlimeStateBase(slime)
{
    private float _idleTimer;
    private const float IdleDuration = 2f;

    public override void Enter()
    {
        SetAnimation("slime-idle-animation");
        Slime.Velocity = new Vector2(0, Slime.Velocity.Y);
        _idleTimer = 0f;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _idleTimer += dt;

        if (_idleTimer >= IdleDuration)
        {
            Slime.ChangeState(new SlimeWalkState(Slime));
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
}
