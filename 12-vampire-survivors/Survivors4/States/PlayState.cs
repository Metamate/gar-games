using GMDCore.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors4.States;

public sealed class PlayState(Game1 game) : GameStateBase
{
    public override void Update(GameTime gameTime)
    {
        Run run = game.CurrentRun;
        run.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

        if (run.Player.IsDead || run.IsWon)
        {
            game.StateStack.Pop();
            game.StateStack.Push(new GameOverState(game, won: !run.Player.IsDead));
        }
        else if (run.CanLevelUp)
        {
            // The level-up screen goes on top: this state stays below it, drawn but paused.
            game.StateStack.Push(new LevelUpState(game));
        }
    }

    public override void Draw(SpriteBatch spriteBatch) => game.CurrentRun.Draw(spriteBatch);
    public override void DrawHUD(SpriteBatch spriteBatch) => game.CurrentRun.DrawHud(spriteBatch, game.Font);
}
