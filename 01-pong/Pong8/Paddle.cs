using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong8;

public class Paddle
{
    public const int SPEED = 150;

    public required float X { get; set; }
    public required float Y { get; set; }
    public required int PaddleIndex { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }

    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (PaddleIndex == 1)
        {
            if (keyboardState.IsKeyDown(Keys.W))
                Y = Math.Clamp(Y - SPEED * deltaTime, 0, Game1.VIRTUAL_HEIGHT - Height);
            if (keyboardState.IsKeyDown(Keys.S))
                Y = Math.Clamp(Y + SPEED * deltaTime, 0, Game1.VIRTUAL_HEIGHT - Height);
        }
        else if (PaddleIndex == 2)
        {
            if (keyboardState.IsKeyDown(Keys.Up))
                Y = Math.Clamp(Y - SPEED * deltaTime, 0, Game1.VIRTUAL_HEIGHT - Height);
            if (keyboardState.IsKeyDown(Keys.Down))
                Y = Math.Clamp(Y + SPEED * deltaTime, 0, Game1.VIRTUAL_HEIGHT - Height);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Game1.Texture, new Rectangle((int)X, (int)Y, Width, Height), Color.White);
    }
}