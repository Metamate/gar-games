using System.Collections.Generic;
using Box2D.NET;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Box2D.NET.B2Bodies;
using static Box2D.NET.B2Geometries;
using static Box2D.NET.B2MathFunction;
using static Box2D.NET.B2Shapes;
using static Box2D.NET.B2Types;
using static Box2D.NET.B2Worlds;

namespace Birds0;

// Box2D, used directly. It works, but look at how much of Game1 is about Box2D: its types
// (B2WorldId, B2BodyId, B2Vec2), its C-style functions and in parameters, and converting
// between its world (metres, y up) and ours (pixels, y down) wherever a position crosses over.
public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;
    private const float PixelsPerMeter = 50;
    private const float TimeStep = 1 / 60f;
    private static readonly Vector2 Anchor = new(220, 520);
    private const float MaxPull = 90;
    private const float LaunchScale = 10;

    private readonly List<(B2BodyId Body, TextureRegion Sprite)> _bodies = [];
    private B2WorldId _world;
    private B2BodyId _bird;
    private bool _hasBird;
    private float _accumulator;
    private bool _dragging;
    private Vector2 _pull;

    private Texture2D _background;
    private TextureAtlas _atlas;

    public Game1() : base("Angry Birds", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        _background = Content.Load<Texture2D>("images/background");
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        CreateWorld();
    }

    private void CreateWorld()
    {
        B2WorldDef worldDef = b2DefaultWorldDef();
        worldDef.gravity = new B2Vec2(0, -10);      // metres per second squared, and down is negative
        _world = b2CreateWorld(in worldDef);

        // The ground: a static box, 3000 x 80 pixels (wider than the screen), with its top at y = 640.
        B2BodyDef groundDef = b2DefaultBodyDef();
        groundDef.position = new B2Vec2(640 / PixelsPerMeter, -680 / PixelsPerMeter);
        B2BodyId ground = b2CreateBody(_world, in groundDef);
        B2Polygon groundBox = b2MakeBox(1500 / PixelsPerMeter, 40 / PixelsPerMeter);
        B2ShapeDef groundShape = b2DefaultShapeDef();
        groundShape.material.friction = 0.8f;
        b2CreatePolygonShape(ground, in groundShape, in groundBox);

        // A hut with a pig inside, and one on the roof.
        AddBox(880, 590, 20, 100, "wood-post");
        AddBox(1040, 590, 20, 100, "wood-post");
        AddBox(960, 530, 180, 20, "wood-plank");
        AddCircle(960, 618, 22, "pig");
        AddBox(960, 495, 50, 50, "glass-box");
        AddCircle(960, 448, 22, "pig");
    }

    private void AddBox(float x, float y, float width, float height, string sprite)
    {
        B2BodyDef bodyDef = b2DefaultBodyDef();
        bodyDef.type = B2BodyType.b2_dynamicBody;
        bodyDef.position = new B2Vec2(x / PixelsPerMeter, -y / PixelsPerMeter);
        B2BodyId body = b2CreateBody(_world, in bodyDef);

        B2Polygon box = b2MakeBox(width / 2 / PixelsPerMeter, height / 2 / PixelsPerMeter);
        B2ShapeDef shapeDef = b2DefaultShapeDef();
        shapeDef.material.friction = 0.6f;
        b2CreatePolygonShape(body, in shapeDef, in box);

        _bodies.Add((body, _atlas.GetRegion(sprite)));
    }

    private B2BodyId AddCircle(float x, float y, float radius, string sprite, float density = 1)
    {
        B2BodyDef bodyDef = b2DefaultBodyDef();
        bodyDef.type = B2BodyType.b2_dynamicBody;
        bodyDef.position = new B2Vec2(x / PixelsPerMeter, -y / PixelsPerMeter);
        B2BodyId body = b2CreateBody(_world, in bodyDef);

        B2Circle circle = new() { center = new B2Vec2(0, 0), radius = radius / PixelsPerMeter };
        B2ShapeDef shapeDef = b2DefaultShapeDef();
        shapeDef.density = density;
        shapeDef.material.friction = 0.6f;
        b2CreateCircleShape(body, in shapeDef, in circle);

        _bodies.Add((body, _atlas.GetRegion(sprite)));
        return body;
    }

    protected override void Update(GameTime gameTime)
    {
        if (Input.Keyboard.WasKeyJustPressed(Keys.R))
        {
            b2DestroyWorld(_world);
            _bodies.Clear();
            _hasBird = false;
            CreateWorld();
        }

        Aim();

        _accumulator += (float)gameTime.ElapsedGameTime.TotalSeconds;
        while (_accumulator >= TimeStep)
        {
            b2World_Step(_world, TimeStep, 4);
            _accumulator -= TimeStep;
        }

        base.Update(gameTime);
    }

    // Pull back with the mouse, and let go to shoot.
    private void Aim()
    {
        Vector2 mouse = MousePosition();
        if (!_dragging && Input.Mouse.WasLeftButtonJustPressed && Vector2.Distance(mouse, Anchor) < 50)
            _dragging = true;
        if (!_dragging)
            return;

        _pull = mouse - Anchor;
        if (_pull.Length() > MaxPull)
            _pull = Vector2.Normalize(_pull) * MaxPull;
        if (Input.Mouse.IsLeftButtonDown)
            return;

        _dragging = false;
        if (_pull.Length() > 10)
            Launch(Anchor + _pull, -_pull * LaunchScale);
        _pull = Vector2.Zero;
    }

    private void Launch(Vector2 position, Vector2 velocity)
    {
        if (_hasBird)
        {
            b2DestroyBody(_bird);
            _bodies.RemoveAll(entry => entry.Body.Equals(_bird));
        }

        _bird = AddCircle(position.X, position.Y, 18, "bird", density: 4);
        b2Body_SetLinearVelocity(_bird, new B2Vec2(velocity.X / PixelsPerMeter, -velocity.Y / PixelsPerMeter));
        _hasBird = true;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        SpriteBatch.Draw(_background, Vector2.Zero, Color.White);

        TextureRegion slingshotBack = _atlas.GetRegion("slingshot-back"), slingshotFront = _atlas.GetRegion("slingshot-front");
        slingshotBack.Draw(SpriteBatch, new Vector2(196, 510), Color.White);

        foreach (var (body, sprite) in _bodies)
        {
            // Back from metres and y up to pixels and y down, for every body, every frame.
            B2Vec2 position = b2Body_GetPosition(body);
            B2Rot rotation = b2Body_GetRotation(body);
            Vector2 pixels = new(position.X * PixelsPerMeter, -position.Y * PixelsPerMeter);
            float angle = -b2Rot_GetAngle(in rotation);
            sprite.Draw(SpriteBatch, pixels, Color.White, angle, new Vector2(sprite.Width / 2f, sprite.Height / 2f), 1, SpriteEffects.None, 0);
        }

        TextureRegion bird = _atlas.GetRegion("bird");
        bird.Draw(SpriteBatch, Anchor + _pull, Color.White, 0, new Vector2(bird.Width / 2f, bird.Height / 2f), 1, SpriteEffects.None, 0);
        slingshotFront.Draw(SpriteBatch, new Vector2(196, 510), Color.White);

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
