using GARCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy12.States;

public class TitleState(Game1 game) : IState
{
    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter) || Core.Input.Keyboard.WasKeyJustPressed(Keys.Space) || Core.Input.Mouse.WasLeftButtonJustPressed)
        {
            game.GameState.ChangeState(game.GameState.CountdownState);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // A dark band over the game, with the name, the controls and the prompt.
        int middle = Game1.VirtualWidth / 2;
        spriteBatch.Draw(Art.Pixel, new Rectangle(0, 116, Game1.VirtualWidth, 128), Color.Black * 0.75f);
        Art.DrawCentred(spriteBatch, Art.Font, "Flappy Bird", new Vector2(middle, 148));
        Art.DrawCentred(spriteBatch, Art.SmallFont, "Space or click: flap", new Vector2(middle, 188));
        Art.DrawCentred(spriteBatch, Art.SmallFont, "Press Enter", new Vector2(middle, 220));
    }
}