using Microsoft.Xna.Framework;
using Zelda2.Entities;
using Zelda2.Input;
using Zelda2.States.EntityStates;
using Zelda2.World;

namespace Zelda2.States.PlayerStates;

// Extends EntityWalkState: adds player input handling.
public class PlayerWalkState(Player player, Room room) : EntityWalkState(player)
{
    private readonly Player _player = player;
    private readonly Room _room = room;

    public override void Enter()
    {
        _player.SpriteOffset = new Vector2(0, GameSettings.PlayerSpriteOffsetY);
    }

    public override void Update(GameTime gameTime)
    {
        // Map input to direction; transition to idle if no key is held
        Direction? dir = GameController.WalkDirection(_player.Direction);

        if (dir is null)
        {
            _player.ChangeState(new PlayerIdleState(_player, _room));
            return;
        }

        _player.Direction = dir.Value;
        _player.ChangeAnimation(AnimationKeys.Walk(dir.Value));

        // Apply movement and wall collision (from EntityWalkState)
        base.Update(gameTime);
    }
}
