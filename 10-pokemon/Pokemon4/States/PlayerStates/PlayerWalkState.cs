using Microsoft.Xna.Framework;
using Pokemon4;
using Pokemon4.Entities;
using Pokemon4.Input;
using Pokemon4.States.EntityStates;
using Pokemon4.States.GameStates;
using Pokemon4.World;
using GARCore.States;

namespace Pokemon4.States.PlayerStates;

// Moves the player one tile, then checks whether to continue walking.
// Landing on the spring heals the party's current monster.
// After the move visually finishes, landing on a tall-grass tile has a
// 1-in-10 chance of triggering a random encounter.
public sealed class PlayerWalkState : EntityWalkState
{
    private readonly Player     _player;
    private readonly StateStack _stateStack;

    public PlayerWalkState(Player player, Level level, StateStack stateStack)
        : base(player, level)
    {
        _player     = player;
        _stateStack = stateStack;
    }

    // Returns true 1-in-EncounterChance times.
    private static bool RollEncounter()
        => System.Random.Shared.Next(GameSettings.EncounterChance) == 0;

    private bool TryStartEncounter()
    {
        int tileId = Level.GrassLayer.GetTile(Entity.MapX, Entity.MapY).GraphicId;
        if (tileId != GameSettings.TileTallGrass) return false;
        if (!RollEncounter()) return false;

        // Freeze player in place (PlayerIdleState so input is re-enabled when battle ends)
        Entity.ChangeState(new PlayerIdleState(_player, Level, _stateStack));
        Entity.ChangeAnimation(AnimationKeys.Idle(Entity.Direction));

        Locator.Audio.PauseFieldMusic();
        Locator.Audio.PlayBattleMusic();

        _stateStack.Push(new FadeState(_stateStack, Color.White, GameSettings.FadeDuration, 0f, 1f,
            () =>
            {
                _stateStack.Push(new BattleState(_player, _stateStack));
                _stateStack.Push(new FadeState(_stateStack, Color.White, GameSettings.FadeDuration, 1f, 0f, () => { }));
            }));

        return true;
    }

    // Stepping onto the spring heals the monster in front of the party.
    private bool TryHeal()
    {
        int tileId = Level.BaseLayer.GetTile(Entity.MapX, Entity.MapY).GraphicId;
        if (tileId != GameSettings.TileSpring) return false;

        Entity.ChangeState(new PlayerIdleState(_player, Level, _stateStack));
        Entity.ChangeAnimation(AnimationKeys.Idle(Entity.Direction));

        Locator.Audio.PlayHeal();
        var mon = _player.Party.Current;
        mon.Heal();
        _stateStack.Push(new DialogueState(_stateStack, $"The spring restores {mon.Name} to full health!"));
        return true;
    }

    protected override void OnMovementComplete()
    {
        if (TryHeal() || TryStartEncounter())
            return;

        // Continue walking if a direction key is still held
        Direction? dir = GameController.MovementDirection;

        if (dir.HasValue)
        {
            Entity.Direction = dir.Value;
            Entity.ChangeState(new PlayerWalkState(_player, Level, _stateStack));
        }
        else
        {
            Stop();
        }
    }

    protected override void Stop()
    {
        Entity.ChangeState(new PlayerIdleState(_player, Level, _stateStack));
    }
}
