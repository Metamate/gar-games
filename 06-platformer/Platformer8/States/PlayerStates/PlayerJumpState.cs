using Microsoft.Xna.Framework;
using Platformer8.Entities;
using Platformer8.Audio;

namespace Platformer8.States.PlayerStates;

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
