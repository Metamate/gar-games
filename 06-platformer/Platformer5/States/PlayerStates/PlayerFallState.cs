using Microsoft.Xna.Framework;
using Platformer5.Entities;
using Platformer5.Input;

namespace Platformer5.States.PlayerStates;

public class PlayerFallState(Player player) : PlayerStateBase(player)
{
    public override void Enter()
    {
        SetAnimation("fall-animation");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsOnGround())
        {
            if (Player.Velocity.X == 0)
                Player.ChangeState(new PlayerIdleState(Player));
            else
                Player.ChangeState(new PlayerWalkState(Player));
        }
        else if (Player.CoyoteTimer > 0 && GameController.Jump)
        {
            Player.ChangeState(new PlayerJumpState(Player));
        }
    }
}
