using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer4.Entities;
using Platformer4.Input;

namespace Platformer4.States.PlayerStates;

public class PlayerDuckState(Player player) : PlayerStateBase(player)
{
    public override void Enter()
    {
        SetAnimation("duck-animation");
    }

    protected override void HandleHorizontalMovement(GameTime gameTime)
    {
        // Shuts off horizontal movement by not calling base
        // But we still allow flipping
        if (GameController.Right) Player.Sprite.Effects = SpriteEffects.None;
        else if (GameController.Left) Player.Sprite.Effects = SpriteEffects.FlipHorizontally;

        Player.Velocity = new Vector2(0, Player.Velocity.Y);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!GameController.Down)
        {
            Player.ChangeState(new PlayerIdleState(Player));
        }

        if (!IsOnGround())
        {
            Player.ChangeState(new PlayerFallState(Player));
        }
    }
}
