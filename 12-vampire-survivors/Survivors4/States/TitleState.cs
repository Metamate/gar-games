using GARCore;
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
        const string title = "Vampire Survivors";
        const string prompt = "Press Enter";
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(Core.Pixel, new Rectangle(0, 0, Game1.VirtualWidth, Game1.VirtualHeight), Color.Black * 0.5f);
        spriteBatch.DrawString(game.TitleFont, title, new Vector2((int)((Game1.VirtualWidth - game.TitleFont.MeasureString(title).X) / 2), 200), Color.White);
        spriteBatch.DrawString(game.Font, prompt, new Vector2((int)((Game1.VirtualWidth - game.Font.MeasureString(prompt).X) / 2), 264), Color.White);
        spriteBatch.End();
    }
}
