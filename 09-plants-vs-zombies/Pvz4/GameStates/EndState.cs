using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz4.GameStates;

// The lawn stands still, with a message. R starts a new game (see Game1).
public class EndState(Game1 game) : IState
{
    public bool Won { get; set; }

    public void Enter() { }
    public void Exit() { }
    public void Update(GameTime gameTime) { }

    public void Draw(SpriteBatch spriteBatch)
    {
        game.DrawGame(showCursor: false);
        game.DrawMessage(Won ? "You survived the zombies!  Press R to play again" : "The zombies ate your brains!  Press R to try again");
    }
}
