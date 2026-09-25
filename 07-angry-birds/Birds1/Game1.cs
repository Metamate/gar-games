using System.Collections.Generic;
using Birds1.Entities;
using Birds1.Physics;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Birds1;

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
        StartLevel();
    }

    private void StartLevel()
    {
        _physics?.Dispose();
        _physics = new PhysicsWorld(Gravity);
        _physics.CreateBox(new Vector2(640, 680), new Vector2(3000, 80), 0, Materials.Ground, isStatic: true);
        _entities.Clear();
        _bird = null;

        // A hut with a pig inside, and one on the roof.
        var post = new Vector2(20, 100);
        _entities.Add(new Block(_physics, new Vector2(880, 590), post, 0, Materials.Wood, _atlas.GetRegion("wood-post")));
        _entities.Add(new Block(_physics, new Vector2(1040, 590), post, 0, Materials.Wood, _atlas.GetRegion("wood-post")));
        _entities.Add(new Block(_physics, new Vector2(960, 530), new Vector2(180, 20), 0, Materials.Wood, _atlas.GetRegion("wood-plank")));
        _entities.Add(new Pig(_physics, new Vector2(960, 618), 22, _atlas.GetRegion("pig")));
        _entities.Add(new Block(_physics, new Vector2(960, 495), new Vector2(50, 50), 0, Materials.Glass, _atlas.GetRegion("glass-box")));
        _entities.Add(new Pig(_physics, new Vector2(960, 448), 22, _atlas.GetRegion("pig")));
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

        base.Update(gameTime);
    }

    // One bird at a time: the old one goes when the next is shot.
    private void Launch(Vector2 position, Vector2 velocity)
    {
        if (_bird != null)
            _physics.Destroy(_bird.Body);
        _bird = new Bird(_physics, position, velocity, _atlas.GetRegion("bird"));
    }

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
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    // The mouse is in window coordinates; the game draws at its virtual resolution, scaled and
    // centred in the window.
    private Vector2 MousePosition()
    {
        Viewport viewport = GraphicsDevice.Viewport;
        Vector2 mouse = Input.Mouse.Position.ToVector2() - new Vector2(viewport.X, viewport.Y);
        return Vector2.Transform(mouse, Matrix.Invert(ScreenScaleMatrix));
    }
}
