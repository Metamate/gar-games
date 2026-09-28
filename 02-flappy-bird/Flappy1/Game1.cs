using GMDCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy1;

public class Game1 : Core
{
    public const int VirtualWidth = 512;
    public const int VirtualHeight = 288;
    private Texture2D _background;
    private Texture2D _ground;

    public Game1() : base("Flappy1", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _background = Content.Load<Texture2D>("images/background");
        _ground = Content.Load<Texture2D>("images/ground");
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
        SpriteBatch.Draw(_ground, new Vector2(0, VirtualHeight - _ground.Height), Color.White);
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
