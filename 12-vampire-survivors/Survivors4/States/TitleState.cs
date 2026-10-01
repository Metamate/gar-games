using GARCore;
using GARCore.Graphics;
using GARCore.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors4.States;

// The title screen, over a run that hasn't started: Enter starts the game.
public sealed class TitleState(Game1 game) : GameStateBase
{
    public override void Update(GameTime gameTime)
    {
        if (GameController.Confirm)
            game.NewGame();
    }

    public override void Draw(SpriteBatch spriteBatch) => game.CurrentRun.Draw(spriteBatch);

    public override void DrawHUD(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        TitleScreen.Draw(spriteBatch, game.NameFont, game.TitleFont, "Vampire Survivors", "Arrows: move", Game1.VirtualWidth, Game1.VirtualHeight);
        spriteBatch.End();
    }
}
