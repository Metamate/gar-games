using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon4.Input;
using GARCore.States;
using GARCore;
using GARCore.Graphics;

namespace Pokemon4.States.GameStates;

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
        Locator.Audio.PlayIntroMusic();
    }

    public override void Exit()
    {
        Locator.Audio.StopMusic();
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
        DrawTitle(spriteBatch, "Pokemon", "Arrows: move   Enter: choose");
        spriteBatch.End();
    }

    // The same layout as GARCore's TitleScreen, with this game's bitmap fonts and its shades.
    private static void DrawTitle(SpriteBatch spriteBatch, string name, string controls)
    {
        BitmapFont titleFont = Locator.Assets.MediumFont;
        BitmapFont font = Locator.Assets.SmallFont;
        Vector2 nameSize = titleFont.MeasureString(name);
        Vector2 controlsSize = font.MeasureString(controls);
        Vector2 promptSize = font.MeasureString(TitleScreen.Prompt);

        // One line of text between the rows, and above and below them.
        int width = GameSettings.VirtualWidth;
        int gap = (int)promptSize.Y;
        int total = (int)(nameSize.Y + gap + controlsSize.Y + gap + promptSize.Y);
        int top = (GameSettings.VirtualHeight - total) / 2;
        int controlsTop = top + (int)nameSize.Y + gap;
        int promptTop = top + total - (int)promptSize.Y;

        spriteBatch.Draw(Core.Pixel, new Rectangle(0, top - gap, width, total + 2 * gap), GameSettings.Ink);
        titleFont.Draw(spriteBatch, name, new Vector2((int)((width - nameSize.X) / 2), top), GameSettings.Paper);
        font.Draw(spriteBatch, controls, new Vector2((int)((width - controlsSize.X) / 2), controlsTop), GameSettings.Mid);
        font.Draw(spriteBatch, TitleScreen.Prompt, new Vector2((int)((width - promptSize.X) / 2), promptTop), GameSettings.Paper);
    }
}
