using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy12;

public class Pipe(Vector2 position, bool flipped)
{
    public Vector2 Position { get; set; } = position;

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Art.Pipe, Position, null, Color.White, 0f, Vector2.Zero, 1f, flipped ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
    }
}