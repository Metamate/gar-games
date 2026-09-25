using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors2;

public sealed class Player(TextureRegion sprite)
{
    public const float Radius = 14;

    public Vector2 Position { get; private set; }
    public float Speed { get; set; } = 180;
    public void Update(float deltaSeconds) => Position += GameController.Move * Speed * deltaSeconds;

    public void Draw(SpriteBatch spriteBatch)
        => sprite.Draw(spriteBatch, Position, Color.White, 0, new Vector2(sprite.Width / 2f, sprite.Height / 2f), 1, SpriteEffects.None, 0);
}
