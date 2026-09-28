using Microsoft.Xna.Framework;
using Mario4.Entities;
using Mario4.Input;

namespace Mario4.States.PlayerStates;

public class PlayerIdleState(Player player) : PlayerStateBase(player)
{
    public override void Enter()
    {
        SetAnimation("idle-animation");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Player.Velocity.X != 0)
        {
            Player.ChangeState(new PlayerWalkState(Player));
        }

        if (GameController.Down)
        {
            Player.ChangeState(new PlayerDuckState(Player));
        }

        if (GameController.Jump)
        {
            Player.ChangeState(new PlayerJumpState(Player));
        }

        if (!IsOnGround())
        {
            Player.ChangeState(new PlayerFallState(Player));
        }
    }
}
