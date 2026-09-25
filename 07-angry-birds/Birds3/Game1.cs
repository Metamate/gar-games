using System.Collections.Generic;
using System.Linq;
using System.IO;
using Birds3.Entities;
using Birds3.Physics;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Birds3;

// No Box2D in here: the game only talks to PhysicsWorld and PhysicsBody.
public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;
    public const float Gravity = 500;   // pixels per second squared

    private readonly List<Entity> _entities = [];
    private PhysicsWorld _physics;
    private Bird _bird;
    private Slingshot _slingshot;
    private PhysicsDebugView _debugView;
    private Texture2D _background;
    private Texture2D _pixel;
    private TextureAtlas _atlas;
    private SpriteFont _font;
    private int _score;
    private Prefabs _prefabs;

    public Game1() : base("Angry Birds", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        _background = Content.Load<Texture2D>("images/background");
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
        _slingshot = new Slingshot(_atlas, _pixel);
        _debugView = new PhysicsDebugView(_pixel);
        _font = Content.Load<SpriteFont>("fonts/hud");
        _prefabs = new Prefabs(_atlas);
        StartLevel();
    }

    private void StartLevel()
    {
        _physics?.Dispose();
        _physics = new PhysicsWorld(Gravity);
        _physics.Hit += OnHit;
        _physics.CreateBox(new Vector2(640, 680), new Vector2(3000, 80), 0, Materials.Ground, isStatic: true);
        _entities.Clear();
        _bird = null;
        _score = 0;

        Level level = Level.Parse(ReadText("levels/level1.txt"));
        _entities.AddRange(level.Spawn(_prefabs, _physics));
    }

    protected override void Update(GameTime gameTime)
    {
        if (Input.Keyboard.WasKeyJustPressed(Keys.F1))
            DebugDraw.Enabled = !DebugDraw.Enabled;
        if (Input.Keyboard.WasKeyJustPressed(Keys.R))
            StartLevel();

        if (_slingshot.Update(MousePosition(), out Vector2 position, out Vector2 velocity))
            Launch(position, velocity);

        _physics.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        RemoveDestroyed();

        base.Update(gameTime);
    }

    // One bird at a time: the old one goes when the next is shot.
    private void Launch(Vector2 position, Vector2 velocity)
    {
        if (_bird != null)
            _physics.Destroy(_bird.Body);
        _bird = (Bird)_prefabs.Spawn("bird", _physics, position);
        _bird.Body.Velocity = velocity;
    }

    // A hit damages both bodies' entities. Don't destroy anything here: the physics world is
    // still reporting hits, and the next one may be about a body we just destroyed. And don't
    // change _entities: we may be in the middle of a loop over it. Only mark, and clean up later.
    private void OnHit(PhysicsBody a, PhysicsBody b, float speed)
    {
        float damage = speed / 100;
        (a.Owner as Entity)?.TakeDamage(damage);
        (b.Owner as Entity)?.TakeDamage(damage);
    }

    // After the step and all its hits: now it's safe to destroy bodies and remove entities.
    private void RemoveDestroyed()
    {
        if (_bird != null && IsOffScreen(_bird))
        {
            _physics.Destroy(_bird.Body);
            _bird = null;
        }

        for (int i = _entities.Count - 1; i >= 0; i--)
        {
            Entity entity = _entities[i];
            if (IsOffScreen(entity))
                entity.TakeDamage(float.MaxValue);
            if (!entity.IsDestroyed)
                continue;

            _score += entity.Points;
            _physics.Destroy(entity.Body);
            _entities.RemoveAt(i);
        }
    }

    private static bool IsOffScreen(Entity entity)
        => entity.Body.Position.X is < -100 or > VirtualWidth + 100 || entity.Body.Position.Y > VirtualHeight + 100;

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
        _slingshot.DrawBack(SpriteBatch);
        foreach (Entity entity in _entities)
            entity.Draw(SpriteBatch);
        _bird?.Draw(SpriteBatch);
        _slingshot.DrawFront(SpriteBatch);
        _debugView.Draw(SpriteBatch, _physics);

        int pigs = _entities.OfType<Pig>().Count();
        DrawText($"Score {_score}", new Vector2(24, 16));
        DrawText(pigs > 0 ? $"Pigs {pigs}" : "All pigs gone!  R to play again", new Vector2(24, 48));
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawText(string text, Vector2 position)
    {
        SpriteBatch.DrawString(_font, text, position + new Vector2(2, 2), Color.Black * 0.4f);
        SpriteBatch.DrawString(_font, text, position, Color.White);
    }

    // The mouse is in window coordinates; the game draws at its virtual resolution, scaled and
    // centred in the window.
    private Vector2 MousePosition()
    {
        Viewport viewport = GraphicsDevice.Viewport;
        Vector2 mouse = Input.Mouse.Position.ToVector2() - new Vector2(viewport.X, viewport.Y);
        return Vector2.Transform(mouse, Matrix.Invert(ScreenScaleMatrix));
    }

    // Content files are opened through TitleContainer, which works on every platform.
    private string ReadText(string path)
    {
        using Stream stream = TitleContainer.OpenStream(Path.Combine(Content.RootDirectory, path));
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
