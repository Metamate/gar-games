using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3;
using Pokemon3.Definitions;
using Pokemon3.Input;
using Pokemon3.Mons;
using GARCore.States;
using GARCore;
using GARCore.Graphics;

namespace Pokemon3.States.GameStates;

// The title screen: the overworld behind the name, the controls and "Press Enter".
public sealed class StartState : GameStateBase
{
    private readonly StateStack _stack;

    // The game behind the title: a play state that is drawn, but never updated.
    private readonly PlayState _preview;

    public StartState(StateStack stack)
    {
        _stack = stack;
        _preview = new PlayState(stack);
    }

    public override void Enter()
    {
        _preview.Enter();
    }

    public override void Update(GameTime gameTime)
    {
        if (GameController.Confirm)
        {
            _stack.Push(new FadeState(_stack, Color.White, GameSettings.FadeDuration, 0f, 1f,
                () =>
                {
                    _stack.Pop(); // pop StartState
                    _stack.Push(new PlayState(_stack));
                    _stack.Push(new DialogueState(_stack,
                        "Welcome to the world of Pokemon! Walk in the tall grass to fight monsters. " +
                        "Rest at the spring to heal."));
                    _stack.Push(new FadeState(_stack, Color.White, GameSettings.FadeDuration, 1f, 0f, () => { }));
                }));
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _preview.Draw(spriteBatch);

        Core.BeginDraw(spriteBatch);
        TitleScreen.Draw(spriteBatch, Locator.Assets.MediumFont, Locator.Assets.SmallFont, "Pokemon", "Arrows: move   Enter: choose",
            GameSettings.VirtualWidth, GameSettings.VirtualHeight, GameSettings.Paper, GameSettings.Mid, GameSettings.Ink);
        spriteBatch.End();
    }
}
