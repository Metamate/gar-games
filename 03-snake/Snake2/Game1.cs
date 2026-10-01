using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake2;

public class Game1 : Core
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 180;
    // The colour of the screen where no dot is lit.
    private static readonly Color ScreenColor = new(20, 24, 20);
    private const float HeadRotationSpeed = 2f;
    private Sprite _body;
    private Sprite _head;

    public Game1() : base("Snake", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        // A Sprite wraps a region together with everything needed to draw it:
        // color, rotation, scale, origin, effects and layer depth.
        _body = atlas.CreateSprite("body");
        _head = atlas.CreateSprite("head-1");

        // Rotate and scale around the center of the head instead of its top-left corner.
        _head.CenterOrigin();
        _head.Scale = new Vector2(4f, 4f);
    }

    protected override void Update(GameTime gameTime)
    {
        _head.Rotation += HeadRotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ScreenColor);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        _body.Draw(SpriteBatch, new Vector2(112, 80));
        _head.Draw(SpriteBatch, new Vector2(192, 88));
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
