using Microsoft.Xna.Framework;
using Mario7.Entities;

namespace Mario7.States.MoleStates;

public class MoleIdleState(Mole mole) : MoleStateBase(mole)
{
    private float _idleTimer;
    private const float IdleDuration = 2f;

    public override void Enter()
    {
        SetAnimation("mole-idle-animation");
        Mole.Velocity = new Vector2(0, Mole.Velocity.Y);
        _idleTimer = 0f;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _idleTimer += dt;

        if (_idleTimer >= IdleDuration)
        {
            Mole.ChangeState(new MoleWalkState(Mole));
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
}
