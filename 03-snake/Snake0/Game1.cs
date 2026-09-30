using GARCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake0;

public class Game1 : Core
{
    public const int VirtualWidth = 160;
    public const int VirtualHeight = 88;
    // The colour of the screen where no dot is lit.
    private static readonly Color ScreenColor = new(20, 24, 20);
    private Texture2D _atlas;

    public Game1() : base("Snake", 1280, 704, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        // All of the game's art lives in a single image: the texture atlas.
        _atlas = Content.Load<Texture2D>("images/atlas");
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ScreenColor);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);

        // Draw the whole atlas, so we can see what it contains.
        SpriteBatch.Draw(_atlas, new Vector2(8, 8), Color.White);

        // Draw only part of the atlas by passing a source rectangle.
        // The snake's head is the 7x7 area at (8, 0), the food is the 7x7 area at (24, 0).
        // Hardcoding these rectangles everywhere quickly becomes unmanageable...
        SpriteBatch.Draw(_atlas, new Vector2(96, 40), new Rectangle(8, 0, 7, 7), Color.White);
        SpriteBatch.Draw(_atlas, new Vector2(112, 40), new Rectangle(24, 0, 7, 7), Color.White);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
