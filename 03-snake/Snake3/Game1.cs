using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake3;

public class Game1 : Core
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 176;
    // The colour of the screen where no dot is lit.
    private static readonly Color ScreenColor = new(20, 24, 20);
    private AnimatedSprite _head;
    private AnimatedSprite _food;

    public Game1() : base("Snake", 1280, 704, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        // The atlas definition also describes animations: a list of frames (regions) and a delay.
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _head = atlas.CreateAnimatedSprite("head-animation");
        _food = atlas.CreateAnimatedSprite("food-animation");
    }

    protected override void Update(GameTime gameTime)
    {
        // Animated sprites must be updated every frame to advance their animation.
        _head.Update(gameTime);
        _food.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ScreenColor);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        _head.Draw(SpriteBatch, new Vector2(144, 80));
        _food.Draw(SpriteBatch, new Vector2(176, 80));
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
