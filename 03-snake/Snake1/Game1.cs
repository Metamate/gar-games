using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake1;

public class Game1 : Core
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 180;
    private TextureRegion _snake;
    private TextureRegion _mouse;

    public Game1() : base("Snake", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        // The atlas definition names each region of the atlas texture,
        // so the rectangles live in data instead of in code.
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _snake = atlas.GetRegion("snake-1");
        _mouse = atlas.GetRegion("mouse-1");
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        _snake.Draw(SpriteBatch, new Vector2(140, 80), Color.White);
        _mouse.Draw(SpriteBatch, new Vector2(180, 80), Color.White);
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
