using GMDCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake0;

public class Game1 : Core
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 180;
    private Texture2D _atlas;

    public Game1() : base("Snake", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        // All of the game's art lives in a single image: the texture atlas.
        _atlas = Content.Load<Texture2D>("images/atlas");
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);

        // Draw the whole atlas, so we can see what it contains.
        SpriteBatch.Draw(_atlas, new Vector2(10, 10), Color.White);

        // Draw only part of the atlas by passing a source rectangle.
        // The snake is the 20x20 area at (0, 0), the bat is the 20x20 area at (20, 0).
        // Hardcoding these rectangles everywhere quickly becomes unmanageable...
        SpriteBatch.Draw(_atlas, new Vector2(200, 80), new Rectangle(0, 0, 20, 20), Color.White);
        SpriteBatch.Draw(_atlas, new Vector2(240, 80), new Rectangle(20, 0, 20, 20), Color.White);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
