using GMDCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Birds4.GameStates;

// Won or lost: Enter goes to the next level, or tries this one again.
public class LevelEndState(Game1 game) : IState
{
    public bool Won { get; set; }

    public void Enter() { }
    public void Exit() { }

    public void Update(GameTime gameTime)
    {
        game.UpdateWorld((float)gameTime.ElapsedGameTime.TotalSeconds);
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
            game.StartLevel(Won ? (game.LevelIndex + 1) % Game1.LevelCount : game.LevelIndex);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        game.DrawWorld();
        if (!Won)
            game.DrawMessage("Out of birds!  Press Enter to try again");
        else if (game.LevelIndex == Game1.LevelCount - 1)
            game.DrawMessage($"You beat every level!  Score {game.Score}.  Press Enter to start over");
        else
            game.DrawMessage($"Level cleared!  Score {game.Score}.  Press Enter for the next level");
    }
}
