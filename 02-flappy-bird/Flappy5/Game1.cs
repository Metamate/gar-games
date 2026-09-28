using GMDCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy5;

public class Game1 : Core
{
    public const int VirtualWidth = 512;
    public const int VirtualHeight = 288;
    private const int BackgroundScrollSpeed = 30;
    private const int GroundScrollSpeed = 60;
    private const int BackgroundLoopingPoint = 413;
    private float _backgroundScroll;
    private float _groundScroll;
    private Bird _bird;

    public Game1() : base("Flappy5", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void Initialize()
    {
        base.Initialize();
        _bird = new Bird(new Vector2(100, 100));
    }

    protected override void LoadContent()
    {
        Art.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        _backgroundScroll += BackgroundScrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _groundScroll += GroundScrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _backgroundScroll %= BackgroundLoopingPoint;
        _groundScroll %= VirtualWidth;

        _bird.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        SpriteBatch.Draw(Art.Background, new Vector2(-_backgroundScroll, 0), Color.White);
        SpriteBatch.Draw(Art.Ground, new Vector2(-_groundScroll, VirtualHeight - Art.Ground.Height), Color.White);
        _bird.Draw(SpriteBatch);
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
