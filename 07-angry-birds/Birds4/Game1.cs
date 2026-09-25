using System.Collections.Generic;
using System.IO;
using System.Linq;
using Birds4.Entities;
using Birds4.GameStates;
using Birds4.Physics;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Birds4;

// Game1 owns the level: the physics world, the entities and the bird. The game states decide
// when the player may shoot, and when a level is won or lost.
public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;
    public const float Gravity = 500;   // pixels per second squared
    public const int LevelCount = 3;
    public const int BirdBonus = 10000;

    private readonly List<Entity> _entities = [];
    private readonly StateMachine _states = new();
    private PhysicsDebugView _debugView;
    private Texture2D _background;
    private Texture2D _pixel;
    private TextureAtlas _atlas;
    private SpriteFont _font;
    private Prefabs _prefabs;

    public Game1() : base("Angry Birds", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    public PhysicsWorld Physics { get; private set; }
    public Slingshot Slingshot { get; private set; }
    public Bird Bird { get; private set; }
    public int LevelIndex { get; private set; }
    public int BirdsLeft { get; set; }
    public int Score { get; set; }
    public int PigsLeft => _entities.OfType<Pig>().Count();

    public AimState AimState { get; private set; }
    public FlyState FlyState { get; private set; }
    public LevelEndState LevelEndState { get; private set; }

    public void ChangeState(IState state) => _states.ChangeState(state);

    protected override void LoadContent()
    {
        _background = Content.Load<Texture2D>("images/background");
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _font = Content.Load<SpriteFont>("fonts/hud");
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
        Slingshot = new Slingshot(_atlas, _pixel);
        _debugView = new PhysicsDebugView(_pixel);
        _prefabs = new Prefabs(_atlas);

        AimState = new AimState(this);
        FlyState = new FlyState(this);
        LevelEndState = new LevelEndState(this);
        StartLevel(0);
    }

    public void StartLevel(int index)
    {
        LevelIndex = index;
        Physics?.Dispose();
        Physics = new PhysicsWorld(Gravity);
        Physics.Hit += OnHit;
        Physics.CreateBox(new Vector2(640, 680), new Vector2(3000, 80), 0, Materials.Ground, isStatic: true);

        Level level = Level.Parse(ReadText($"levels/level{index + 1}.txt"));
        _entities.Clear();
        _entities.AddRange(level.Spawn(_prefabs, Physics));
        Bird = null;
        BirdsLeft = level.Birds;
        Score = 0;
        ChangeState(AimState);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Input.Keyboard.WasKeyJustPressed(Keys.F1))
            DebugDraw.Enabled = !DebugDraw.Enabled;
        if (Input.Keyboard.WasKeyJustPressed(Keys.R))
            StartLevel(LevelIndex);

        _states.Update(gameTime);
        base.Update(gameTime);
    }

    // Every state lets the world run: things keep falling while the player aims.
    public void UpdateWorld(float deltaSeconds)
    {
        Physics.Update(deltaSeconds);
        RemoveDestroyed();
    }

    // One bird at a time: the old one goes when the next is shot.
    public void Launch(Vector2 position, Vector2 velocity)
    {
        if (Bird != null)
            Physics.Destroy(Bird.Body);
        Bird = (Bird)_prefabs.Spawn("bird", Physics, position);
        Bird.Body.Velocity = velocity;
        BirdsLeft--;
    }

    // Don't destroy anything while hits are being reported: only mark, and clean up after.
    private void OnHit(PhysicsBody a, PhysicsBody b, float speed)
    {
        float damage = speed / 100;
        (a.Owner as Entity)?.TakeDamage(damage);
        (b.Owner as Entity)?.TakeDamage(damage);
    }

    private void RemoveDestroyed()
    {
        if (Bird != null && IsOffScreen(Bird))
        {
            Physics.Destroy(Bird.Body);
            Bird = null;
        }

        for (int i = _entities.Count - 1; i >= 0; i--)
        {
            Entity entity = _entities[i];
            if (IsOffScreen(entity))
                entity.TakeDamage(float.MaxValue);
            if (!entity.IsDestroyed)
                continue;

            Score += entity.Points;
            Physics.Destroy(entity.Body);
            _entities.RemoveAt(i);
        }
    }

    private static bool IsOffScreen(Entity entity)
        => entity.Body.Position.X is < -100 or > VirtualWidth + 100 || entity.Body.Position.Y > VirtualHeight + 100;

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        _states.Draw(SpriteBatch);
        SpriteBatch.End();
        base.Draw(gameTime);
    }

    public void DrawWorld()
    {
        SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
        Slingshot.DrawBack(SpriteBatch);
        foreach (Entity entity in _entities)
            entity.Draw(SpriteBatch);
        Bird?.Draw(SpriteBatch);
        Slingshot.DrawFront(SpriteBatch);
        _debugView.Draw(SpriteBatch, Physics);

        DrawText($"Level {LevelIndex + 1}   Score {Score}", new Vector2(24, 16));
        DrawText($"Birds {BirdsLeft}   Pigs {PigsLeft}", new Vector2(24, 48));
    }

    // Where the bird would fly: a few dots along the curve it will follow.
    public void DrawTrajectory(Vector2 start, Vector2 velocity)
    {
        TextureRegion dot = _atlas.GetRegion("dot");
        for (int i = 1; i <= 12; i++)
        {
            float t = i * 0.07f;
            Vector2 position = start + velocity * t + new Vector2(0, Gravity) * t * t / 2;
            dot.Draw(SpriteBatch, position - new Vector2(dot.Width / 2f, dot.Height / 2f), Color.White * (1 - i / 14f));
        }
    }

    public void DrawMessage(string text)
    {
        Vector2 size = _font.MeasureString(text);
        DrawText(text, new Vector2((VirtualWidth - size.X) / 2, 200));
    }

    private void DrawText(string text, Vector2 position)
    {
        SpriteBatch.DrawString(_font, text, position + new Vector2(2, 2), Color.Black * 0.4f);
        SpriteBatch.DrawString(_font, text, position, Color.White);
    }

    // The mouse is in window coordinates; the game draws at its virtual resolution, scaled and
    // centred in the window.
    public Vector2 MousePosition()
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
