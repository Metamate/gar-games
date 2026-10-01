using Flappy9.States;
using GARCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy9;

public class Game1 : Core
{
    public const int VirtualWidth = 640;
    public const int VirtualHeight = 360;
    private const int BackgroundScrollSpeed = 30;
    private const int GroundScrollSpeed = 60;
    private const int BackgroundLoopingPoint = 412;
    private const int GroundLoopingPoint = 512;
    private float _backgroundScroll;
    private float _groundScroll;
    public bool IsScrolling { get; set; }
    public StateMachine GameState { get; set; }

    public Game1() : base("Flappy Bird", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void Initialize()
    {
        base.Initialize();
        GameState = new StateMachine(this);
        GameState.ChangeState(GameState.TitleState);
    }

    protected override void LoadContent()
    {
        Art.LoadContent(Content);
        Art.Pixel = new Texture2D(GraphicsDevice, 1, 1);
        Art.Pixel.SetData([Color.White]);
    }

    protected override void Update(GameTime gameTime)
    {
        if (IsScrolling)
        {
            _backgroundScroll += BackgroundScrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _groundScroll += GroundScrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _backgroundScroll %= BackgroundLoopingPoint;
            _groundScroll %= GroundLoopingPoint;
        }

        GameState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        SpriteBatch.Draw(Art.Background, new Vector2(-_backgroundScroll, 0), Color.White);
        SpriteBatch.Draw(Art.Ground, new Vector2(-_groundScroll, VirtualHeight - Art.Ground.Height), Color.White);

        GameState.Draw(SpriteBatch);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
