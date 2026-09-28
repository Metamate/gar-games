using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon4;
using Pokemon4.Audio;

namespace UiSamples;

// The game's UI widgets on one screen, each on its own. It sets up the same services as the
// game (the assets and the audio in the Locator), because the widgets get their fonts, cursor
// and sounds from there.
public sealed class Game1 : Core
{
    private RenderTarget2D _renderTarget;
    private SamplesScreen _screen;

    public Game1()
        : base("UI samples",
               GameSettings.WindowWidth,  GameSettings.WindowHeight,
               GameSettings.VirtualWidth, GameSettings.VirtualHeight)
    { }

    protected override void LoadContent()
    {
        base.LoadContent();

        var tileTex = Content.Load<Texture2D>("images/tiles");
        Locator.Provide(new GameAssets(
            BitmapFont.CreateSmall(Content.Load<Texture2D>("fonts/small_atlas")),
            BitmapFont.CreateMedium(Content.Load<Texture2D>("fonts/medium_atlas")),
            BitmapFont.CreateLarge(Content.Load<Texture2D>("fonts/large_atlas")),
            new Tileset(new TextureRegion(tileTex, 0, 0, tileTex.Width, tileTex.Height), GameSettings.TileSize, GameSettings.TileSize),
            TextureAtlas.FromGrid(Content.Load<Texture2D>("images/entities"), GameSettings.TileSize, GameSettings.TileSize),
            Content.Load<Texture2D>("images/cursor"),
            TextureFactory.CreateEllipse(GraphicsDevice, 72, 24, Color.Transparent)));

        var audio = new SoundManager();
        audio.LoadContent(Content);
        Locator.Provide(audio);

        _renderTarget = new RenderTarget2D(GraphicsDevice, GameSettings.VirtualWidth, GameSettings.VirtualHeight);
        _screen = new SamplesScreen();
    }

    protected override void UpdateGame(GameTime gameTime)
    {
        Locator.Tweens.Update(gameTime);
        _screen.Update();
    }

    protected override void Draw(GameTime gameTime)
    {
        // As in the game: draw at the virtual resolution, then scale that image to the window.
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(new Color(30, 34, 40));
        Core.BeginDraw(SpriteBatch);
        _screen.Draw(SpriteBatch);
        SpriteBatch.End();
        GraphicsDevice.SetRenderTarget(null);

        GraphicsDevice.Viewport = new Viewport(0, 0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight);
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        SpriteBatch.Draw(_renderTarget, DestinationRectangle, Color.White);
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
