using GMDCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer6.States.GameStates;

namespace Platformer6;

public class Game1 : Core
{
    private GameStateBase _currentState;
    public new Matrix ScreenScaleMatrix => base.ScreenScaleMatrix;
    
    public static SpriteFont DefaultFont { get; private set; }

    public Game1() : base("Platformer", 1280, 720, GameSettings.VirtualWidth, GameSettings.VirtualHeight)
    {
    }

    protected override void Initialize()
    {
        base.Initialize();
        SetState(new StartState(this));
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        DefaultFont = Content.Load<SpriteFont>("fonts/font");
    }

    public void SetState(GameStateBase newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    protected override void Update(GameTime gameTime)
    {
        _currentState?.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _currentState?.Draw(SpriteBatch);
        base.Draw(gameTime);
    }
}
