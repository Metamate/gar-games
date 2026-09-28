using System.Collections.Generic;
using GMDCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy6;

public class Game1 : Core
{
    public const int VirtualWidth = 512;
    public const int VirtualHeight = 288;
    private const int BackgroundScrollSpeed = 30;
    private const int GroundScrollSpeed = 60;
    private const int BackgroundLoopingPoint = 413;
    private float _backgroundScroll;
    private float _groundScroll;
    private Bird _bird;
    private List<Pipe> _pipes = [];
    private const float PipeSpawnInterval = 2f;
    private float _pipeSpawnTimer;

    public Game1() : base("Flappy6", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void Initialize()
    {
        base.Initialize();
        _bird = new Bird(new Vector2(100, 100));
        _pipes.Add(new Pipe());
    }

    protected override void LoadContent()
    {
        Art.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        _backgroundScroll += BackgroundScrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _groundScroll += GroundScrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _backgroundScroll %= BackgroundLoopingPoint;
        _groundScroll %= VirtualWidth;

        _bird.Update(gameTime);

        _pipeSpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_pipeSpawnTimer > PipeSpawnInterval)
        {
            _pipes.Add(new Pipe());
            _pipeSpawnTimer -= PipeSpawnInterval;
        }

        foreach (Pipe pipe in _pipes)
        {
            pipe.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        SpriteBatch.Draw(Art.Background, new Vector2(-_backgroundScroll, 0), Color.White);
        SpriteBatch.Draw(Art.Ground, new Vector2(-_groundScroll, VirtualHeight - Art.Ground.Height), Color.White);

        _bird.Draw(SpriteBatch);

        foreach (Pipe pipe in _pipes)
        {
            pipe.Draw(SpriteBatch);
        }

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
