using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake4;

public class Game1 : Core
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 176;
    // The colour of the screen where no dot is lit.
    private static readonly Color ScreenColor = new(20, 24, 20);
    private Tilemap _tilemap;
    private AnimatedSprite _head;
    private AnimatedSprite _food;

    public Game1() : base("Snake", 1280, 704, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        // The room is data too: the tilemap definition lists the tile for every cell.
        _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");

        _head = atlas.CreateAnimatedSprite("head-animation");
        _food = atlas.CreateAnimatedSprite("food-animation");
    }

    protected override void Update(GameTime gameTime)
    {
        _head.Update(gameTime);
        _food.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ScreenColor);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        _tilemap.Draw(SpriteBatch);
        _head.Draw(SpriteBatch, new Vector2(160, 80));
        _food.Draw(SpriteBatch, new Vector2(208, 80));
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
