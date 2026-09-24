using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake2;

public class Game1 : Core
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 180;
    private const float BatRotationSpeed = 2f;
    private Sprite _snake;
    private Sprite _bat;

    public Game1() : base("Snake", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        // A Sprite wraps a region together with everything needed to draw it:
        // color, rotation, scale, origin, effects and layer depth.
        _snake = atlas.CreateSprite("snake-1");
        _bat = atlas.CreateSprite("bat-1");

        // Rotate and scale around the center of the bat instead of its top-left corner.
        _bat.CenterOrigin();
        _bat.Scale = new Vector2(2f, 2f);
    }

    protected override void Update(GameTime gameTime)
    {
        _bat.Rotation += BatRotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        _snake.Draw(SpriteBatch, new Vector2(140, 80));
        _bat.Draw(SpriteBatch, new Vector2(200, 90));
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
