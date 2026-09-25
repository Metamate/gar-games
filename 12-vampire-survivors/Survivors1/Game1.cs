using System;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors1;

public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;

    private readonly Random _random = new();
    private Texture2D _ground;
    private TextureAtlas _atlas;
    private SpriteFont _font;
    private Player _player;
    private Swarm _swarm;
    private Spawner _spawner;
    private BoltWeapon _bolts;
    private Aura _aura;
    private readonly Profiler _profiler = new();
    private SpriteFont _debugFont;

    public Game1() : base("Vampire Survivors", VirtualWidth, VirtualHeight, VirtualWidth, VirtualHeight)
    {
        Window.AllowUserResizing = false;
    }

    protected override void LoadContent()
    {
        _ground = Content.Load<Texture2D>("images/ground");
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _font = Content.Load<SpriteFont>("fonts/hud");
        _debugFont = Content.Load<SpriteFont>("fonts/debug");
        NewGame();
    }

    private void NewGame()
    {
        _player = new Player(_atlas.GetRegion("hero"));
        _swarm = new Swarm(_atlas);
        _spawner = new Spawner(_random);
        _bolts = new BoltWeapon(_atlas.GetRegion("bolt"));
        _aura = new Aura(_atlas.GetRegion("aura"));
    }

    // Game logic, in fixed steps of 1/60 second (see Core).
    protected override void UpdateGame(GameTime gameTime)
    {
        float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (GameController.Restart)
            NewGame();
        if (GameController.ToggleProfiler)
            _profiler.IsVisible = !_profiler.IsVisible;

        // A stress test: a thousand more enemies at once.
        if (GameController.Stress)
        {
            for (int i = 0; i < 1000; i++)
                _swarm.Spawn(EnemyKind.All[_random.Next(2)], _spawner.PointAround(_player.Position));
        }

        using (_profiler.Measure("Player"))
            _player.Update(deltaSeconds);
        using (_profiler.Measure("Spawn"))
        {
            int count = _spawner.Update(deltaSeconds);
            for (int i = 0; i < count; i++)
                _swarm.Spawn(EnemyKind.All[_spawner.PickKind()], _spawner.PointAround(_player.Position));
        }
        using (_profiler.Measure("Move"))
            _swarm.Move(deltaSeconds, _player.Position);
        using (_profiler.Measure("Separate"))
            _swarm.Separate();
        using (_profiler.Measure("Weapons"))
        {
            _bolts.Update(deltaSeconds, _swarm, _player.Position);
            _aura.Update(deltaSeconds, _swarm, _player.Position);
            _swarm.RemoveDead();
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        using (_profiler.Measure("Draw"))
        {
            GraphicsDevice.Clear(Color.Black);

            // The ground: one texture, repeated, and scrolled with the camera.
            SpriteBatch.Begin(samplerState: SamplerState.LinearWrap);
            Rectangle view = new((int)_player.Position.X - VirtualWidth / 2, (int)_player.Position.Y - VirtualHeight / 2, VirtualWidth, VirtualHeight);
            SpriteBatch.Draw(_ground, new Rectangle(0, 0, VirtualWidth, VirtualHeight), view, Color.White);
            SpriteBatch.End();

            // The world, with the camera centred on the player.
            Matrix camera = Matrix.CreateTranslation(VirtualWidth / 2 - _player.Position.X, VirtualHeight / 2 - _player.Position.Y, 0);
            SpriteBatch.Begin(transformMatrix: camera);
            _aura.Draw(SpriteBatch, _player.Position);
            _swarm.Draw(SpriteBatch);
            _bolts.Draw(SpriteBatch);
            _player.Draw(SpriteBatch);
            SpriteBatch.End();

            SpriteBatch.Begin();
            SpriteBatch.DrawString(_font, $"Enemies {_swarm.Count}   Time {(int)_spawner.Time} s", new Vector2(20, 16), Color.White);
            SpriteBatch.DrawString(_font, "Space: +1000 enemies   R: restart   F3: profiler", new Vector2(20, VirtualHeight - 40), Color.White * 0.7f);
            SpriteBatch.End();
        }

        _profiler.EndFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
        SpriteBatch.Begin();
        _profiler.Draw(SpriteBatch, _debugFont, new Vector2(20, 60), _swarm.Count);
        SpriteBatch.End();
        base.Draw(gameTime);
    }
}
