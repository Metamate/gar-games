using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake3;

public class Game1 : Core
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 180;
    private AnimatedSprite _snake;
    private AnimatedSprite _bat;

    public Game1() : base("Snake", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        // The atlas definition also describes animations: a list of frames (regions) and a delay.
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _snake = atlas.CreateAnimatedSprite("snake-animation");
        _bat = atlas.CreateAnimatedSprite("bat-animation");
    }

    protected override void Update(GameTime gameTime)
    {
        // Animated sprites must be updated every frame to advance their animation.
        _snake.Update(gameTime);
        _bat.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        _snake.Draw(SpriteBatch, new Vector2(140, 80));
        _bat.Draw(SpriteBatch, new Vector2(180, 80));
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
