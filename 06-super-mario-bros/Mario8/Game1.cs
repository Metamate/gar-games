using GARCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario8.States.GameStates;
using Mario8.Audio;

namespace Mario8;

public class Game1 : Core
{
    private GameStateBase _currentState;
    public new Matrix ScreenScaleMatrix => base.ScreenScaleMatrix;
    
    public static SpriteFont DefaultFont { get; private set; }
    public static SpriteFont TitleFont { get; private set; }

    public Game1() : base("Super Mario Bros", 1152, 648, GameSettings.VirtualWidth, GameSettings.VirtualHeight)
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
        TitleFont = Content.Load<SpriteFont>("fonts/font-big");
        SoundManager.LoadContent(Content);
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
