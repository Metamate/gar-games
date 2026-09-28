using GMDCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy0;

public class Game1 : Core
{
    public const int VirtualWidth = 512;
    public const int VirtualHeight = 288;

    public Game1() : base("Flappy0", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
