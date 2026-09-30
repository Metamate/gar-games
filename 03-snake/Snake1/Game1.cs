using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake1;

public class Game1 : Core
{
    public const int VirtualWidth = 160;
    public const int VirtualHeight = 88;
    // The colour of the screen where no dot is lit.
    private static readonly Color ScreenColor = new(20, 24, 20);
    private TextureRegion _head;
    private TextureRegion _food;

    public Game1() : base("Snake", 1280, 704, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        // The atlas definition names each region of the atlas texture,
        // so the rectangles live in data instead of in code.
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _head = atlas.GetRegion("head-1");
        _food = atlas.GetRegion("food-1");
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ScreenColor);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        _head.Draw(SpriteBatch, new Vector2(72, 40), Color.White);
        _food.Draw(SpriteBatch, new Vector2(88, 40), Color.White);
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
