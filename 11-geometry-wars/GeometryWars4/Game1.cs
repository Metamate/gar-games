using GARCore;
using GARCore.States;
using GeometryWars4.Input;
using GeometryWars4.Services;
using GeometryWars4.Systems;
using GeometryWars4.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryWars4;

public sealed class Game1 : Core
{
    private static readonly Viewport Arena = new(0, 0, GameSettings.Arena.Width, GameSettings.Arena.Height);

    private RenderTarget2D _arena;      // the game is drawn here, then scaled into the window
    private FrameInfo Frame { get; } = new();
    private GameAssets Assets { get; } = new();
    private PerformanceMonitor Performance { get; } = new();
    private GameController Controller { get; }
    public PlayContext PlayContext { get; }

    public Game1() : base("Geometry Wars", GameSettings.Window.Width, GameSettings.Window.Height,
               GameSettings.Arena.Width, GameSettings.Arena.Height)
    {
        Controller = new GameController(Input);
        PlayContext = new PlayContext(Frame, Controller, Assets, Performance);
        Graphics.SynchronizeWithVerticalRetrace = false;
        IsFixedTimeStep = false;
        IsMouseVisible = false;
        Window.AllowUserResizing = false;
        StateStack = new StateStack();
    }

    protected override void Initialize()
    {
        base.Initialize();

        Frame.Update(new GameTime(), Arena);

        StateStack.Push(new PlayState(this, PlayContext));
    }

    protected override void LoadContent()
    {
        Assets.Load(Content);
        _arena = new RenderTarget2D(GraphicsDevice, Arena.Width, Arena.Height);
    }

    protected override void UpdateGame(GameTime gameTime)
    {
        Controller.Update();
        Frame.Update(gameTime, Arena);
        StateStack.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        Performance.Update(gameTime);
        GraphicsDevice.SetRenderTarget(_arena);
        GraphicsDevice.Clear(Color.Black);
        StateStack.Draw(SpriteBatch);

        GraphicsDevice.SetRenderTarget(null);
        SpriteBatch.Begin(samplerState: SamplerState.LinearClamp);
        SpriteBatch.Draw(_arena, DestinationRectangle, Color.White);
        SpriteBatch.End();

        base.Draw(gameTime);
        StateStack.DrawHUD(SpriteBatch);
    }
}
