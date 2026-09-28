using Birds4;
using Birds4.Physics;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PhysicsSamples;

// Physics ideas, one scene at a time, through the game's own facade and drawn by its debug view.
// 1-6: choose a scene. R: start it again. Click: drop a box. Space: drop a ball.
public sealed class Game1 : Core
{
    private readonly Scene[] _scenes =
        [new BodyTypesScene(), new BounceScene(), new FrictionScene(), new DensityScene(), new SleepScene(), new JointsScene()];

    private PhysicsWorld _physics;
    private PhysicsDebugView _debugView;
    private SpriteFont _font;
    private Texture2D _pixel;
    private Scene _scene;

    public Game1() : base("Physics samples", 1280, 720, Birds4.Game1.VirtualWidth, Birds4.Game1.VirtualHeight) { }

    protected override void LoadContent()
    {
        base.LoadContent();
        _font = Content.Load<SpriteFont>("fonts/hud");
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
        _debugView = new PhysicsDebugView(_pixel);
        DebugDraw.Enabled = true;                     // the samples are all debug view
        Start(_scenes[0]);
    }

    private void Start(Scene scene)
    {
        _physics?.Dispose();
        _physics = new PhysicsWorld(Birds4.Game1.Gravity);
        _scene = scene;
        _scene.Build(_physics);
    }

    protected override void Update(GameTime gameTime)
    {
        for (int i = 0; i < _scenes.Length; i++)
            if (Input.Keyboard.WasKeyJustPressed(Keys.D1 + i))
                Start(_scenes[i]);
        if (Input.Keyboard.WasKeyJustPressed(Keys.R))
            Start(_scene);
        if (Input.Mouse.WasLeftButtonJustPressed)
            _physics.CreateBox(MousePosition(), new Vector2(40, 40), 0, Materials.Wood);
        if (Input.Keyboard.WasKeyJustPressed(Keys.Space))
            _physics.CreateCircle(MousePosition(), 20, Materials.Wood);

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _scene.Update(_physics, dt);
        _physics.Update(dt);
        base.Update(gameTime);
    }

    // The mouse is in window pixels; the scene is at the virtual resolution.
    private Vector2 MousePosition()
    {
        Viewport viewport = GraphicsDevice.Viewport;
        Vector2 mouse = Input.Mouse.Position.ToVector2() - new Vector2(viewport.X, viewport.Y);
        return Vector2.Transform(mouse, Matrix.Invert(ScreenScaleMatrix));
    }

    // The retro font is large at the game's size: the samples draw their text smaller.
    private void Text(string text, Vector2 position, Color color, float scale)
        => SpriteBatch.DrawString(_font, text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(24, 26, 30));
        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        _debugView.Draw(SpriteBatch, _physics);

        var dim = new Color(150, 150, 150);
        Text("1 Body types  2 Bounce  3 Friction  4 Density  5 Sleep  6 Joints    R: again  Click: box  Space: ball",
            new Vector2(20, 14), dim, 0.5f);
        Text(_scene.Title, new Vector2(20, 40), Color.White, 0.8f);
        for (int i = 0; i < _scene.Notes.Length; i++)
            Text(_scene.Notes[i], new Vector2(20, 76 + i * 20), dim, 0.5f);
        foreach (var (at, text) in _scene.Labels)
            Text(text, at, Color.White, 0.55f);
        SpriteBatch.End();
        base.Draw(gameTime);
    }
}
