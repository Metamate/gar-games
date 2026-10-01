using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zelda7.Input;

namespace Zelda7.States.GameStates;

public class StartState(Game1 game) : GameStateBase(game)
{
    // The game behind the title: a play state that is drawn, but never updated.
    private PlayState _preview;

    public override void Enter()
    {
        _preview = new PlayState(Game);
        _preview.Enter();
    }

    public override void Exit() => _preview.Exit();

    public override void Update(GameTime gameTime)
    {
        if (GameController.Confirm)
            Game.SetState(new PlayState(Game));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _preview.Draw(spriteBatch);

        spriteBatch.Begin(transformMatrix: Game.ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        TitleScreen.Draw(spriteBatch, Game1.TitleFont, Game1.DefaultFont, "The Legend of Zelda", "Arrows: move   Space: sword",
            GameSettings.VirtualWidth, GameSettings.VirtualHeight);
        spriteBatch.End();
    }
}
