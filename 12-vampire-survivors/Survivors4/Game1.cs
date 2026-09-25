using System;
using Survivors4.States;
using GMDCore;
using GMDCore.Graphics;
using GMDCore.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors4;

// Game1 loads the content and starts runs. A run is played by the states on the state stack:
// play, level up (on top of play), and game over.
public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;

    private Texture2D _ground;
    private TextureAtlas _atlas;
    private SpriteFont _debugFont;

    public Game1() : base("Vampire Survivors", VirtualWidth, VirtualHeight, VirtualWidth, VirtualHeight)
    {
        Window.AllowUserResizing = false;
        StateStack = new StateStack();
    }

    public Random Random { get; } = new();
    public Profiler Profiler { get; } = new() { IsVisible = false };
    public SpriteFont Font { get; private set; }
    public Run CurrentRun { get; private set; }

    protected override void LoadContent()
    {
        _ground = Content.Load<Texture2D>("images/ground");
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        Font = Content.Load<SpriteFont>("fonts/hud");
        _debugFont = Content.Load<SpriteFont>("fonts/debug");
        NewGame();
    }

    public void NewGame()
    {
        CurrentRun = new Run(_atlas, _ground, Random, Profiler);
        StateStack.Clear();
        StateStack.Push(new PlayState(this));
    }

    // Game logic, in fixed steps of 1/60 second (see Core).
    protected override void UpdateGame(GameTime gameTime)
    {
        if (GameController.Restart)
            NewGame();
        if (GameController.ToggleProfiler)
            Profiler.IsVisible = !Profiler.IsVisible;
        StateStack.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        using (Profiler.Measure("Draw"))
        {
            GraphicsDevice.Clear(Color.Black);
            StateStack.Draw(SpriteBatch);
            StateStack.DrawHUD(SpriteBatch);
        }

        Profiler.EndFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
        SpriteBatch.Begin();
        Profiler.Draw(SpriteBatch, _debugFont, new Vector2(20, 60), CurrentRun.Swarm.Count);
        SpriteBatch.End();
        base.Draw(gameTime);
    }
}
