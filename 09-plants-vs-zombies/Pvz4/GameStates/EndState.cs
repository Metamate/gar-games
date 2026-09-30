using GARCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pvz4.GameStates;

// The lawn stands still, with a message. Enter starts a new game (and R does, anywhere: see Game1).
public class EndState(Game1 game) : IState
{
    public bool Won { get; set; }

    public void Enter() { }
    public void Exit() { }

    public void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
            game.NewGame();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        game.DrawGame(showCursor: false);
        game.DrawMessage(Won ? "You survived the zombies! Press Enter to play again." : "The zombies ate your brains! Press Enter to try again.");
    }
}
