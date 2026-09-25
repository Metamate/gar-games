using System.Linq;
using GMDCore;
using GMDCore.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors4.States;

// Three upgrades to choose from, on top of the paused game (the state stack from Pokemon).
public sealed class LevelUpState(Game1 game) : GameStateBase
{
    private readonly Upgrade[] _choices = Upgrade.All.OrderBy(_ => game.Random.Next()).Take(3).ToArray();

    public override void Update(GameTime gameTime)
    {
        int choice = GameController.Choice;
        if (choice < 0)
            return;

        _choices[choice].Apply(game.CurrentRun);
        game.CurrentRun.LevelUp();
        game.StateStack.Pop();
    }

    public override void DrawHUD(SpriteBatch spriteBatch)
    {
        SpriteFont font = game.Font;
        spriteBatch.Begin();
        spriteBatch.Draw(Core.Pixel, new Rectangle(0, 0, Game1.VirtualWidth, Game1.VirtualHeight), Color.Black * 0.5f);
        var panel = new Rectangle(Game1.VirtualWidth / 2 - 260, 200, 520, 70 + _choices.Length * 50);
        spriteBatch.Draw(Core.Pixel, panel, new Color(30, 30, 50));

        spriteBatch.DrawString(font, $"Level {game.CurrentRun.Level + 1}!  Choose an upgrade:", new Vector2(panel.X + 24, panel.Y + 18), Color.Gold);
        for (int i = 0; i < _choices.Length; i++)
            spriteBatch.DrawString(font, $"{i + 1}   {_choices[i].Name}", new Vector2(panel.X + 40, panel.Y + 70 + i * 50), Color.White);
        spriteBatch.End();
    }
}
