using Microsoft.Xna.Framework;
using Mario8.Entities;

namespace Mario8.States.GoombaStates;

public class GoombaIdleState(Goomba goomba) : GoombaStateBase(goomba)
{
    private float _idleTimer;
    private const float IdleDuration = 2f;

    public override void Enter()
    {
        SetAnimation("goomba-idle-animation");
        Goomba.Velocity = new Vector2(0, Goomba.Velocity.Y);
        _idleTimer = 0f;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _idleTimer += dt;

        if (_idleTimer >= IdleDuration)
        {
            Goomba.ChangeState(new GoombaWalkState(Goomba));
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
}
