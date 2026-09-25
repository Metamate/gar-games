using GMDCore;
using GMDCore.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors4.States;

public sealed class GameOverState(Game1 game, bool won) : GameStateBase
{
    public override void Update(GameTime gameTime)
    {
        if (GameController.Confirm)
            game.NewGame();
    }

    public override void Draw(SpriteBatch spriteBatch) => game.CurrentRun.Draw(spriteBatch);

    public override void DrawHUD(SpriteBatch spriteBatch)
    {
        SpriteFont font = game.Font;
        Run run = game.CurrentRun;
        string title = won ? "You survived!" : "You died";
        string stats = $"Level {run.Level}   Kills {run.Kills}   Time {(int)run.Spawner.Time / 60}:{(int)run.Spawner.Time % 60:00}";
        string again = "Press Enter to play again";

        spriteBatch.Begin();
        spriteBatch.Draw(Core.Pixel, new Rectangle(0, 0, Game1.VirtualWidth, Game1.VirtualHeight), Color.Black * 0.5f);
        float y = 260;
        foreach (string line in new[] { title, stats, again })
        {
            spriteBatch.DrawString(font, line, new Vector2((Game1.VirtualWidth - font.MeasureString(line).X) / 2, y), Color.White);
            y += 50;
        }
        spriteBatch.End();
    }
}
