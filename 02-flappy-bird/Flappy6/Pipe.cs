using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy6;

public class Pipe
{
    private const int PipeSpeed = 60;
    public Vector2 Position { get; set; }

    public Pipe()
    {
        Random random = new();
        Position = new Vector2(Game1.VirtualWidth, random.Next(Game1.VirtualHeight * 3/4, Game1.VirtualHeight - 10));
    }

    public void Update(GameTime gameTime)
    {
        Position -= new Vector2(PipeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Art.Pipe, Position, Color.White);
    }
}