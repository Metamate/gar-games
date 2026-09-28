using Microsoft.Xna.Framework;
using Mario8.Entities;
using Mario8.Audio;

namespace Mario8.States.PlayerStates;

public class PlayerJumpState(Player player) : PlayerStateBase(player)
{
    private const float JumpImpulse = -300f;

    public override void Enter()
    {
        SetAnimation("jump-animation");
        Player.Velocity = new Vector2(Player.Velocity.X, JumpImpulse);
        SoundManager.PlayJump();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Player.Velocity.Y > 0)
        {
            Player.ChangeState(new PlayerFallState(Player));
        }
    }
}
