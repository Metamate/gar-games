using System;
using System.Collections.Generic;
using GMDCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy7;

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
    private List<PipePair> _pipePairs = [];
    private const float PipeSpawnInterval = 2f;
    private float _pipeSpawnTimer;
    private float lastGapY = VirtualHeight / 2 - PipePair.PipeGap / 2;

    public Game1() : base("Flappy7", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void Initialize()
    {
        base.Initialize();
        _bird = new Bird(new Vector2(100, 100));
        SpawnPipePair();
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
            SpawnPipePair();
            _pipeSpawnTimer -= PipeSpawnInterval;
        }

        foreach (PipePair pipePair in _pipePairs)
        {
            pipePair.Update(gameTime);
        }

        _pipePairs.RemoveAll(p => p.Remove);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        SpriteBatch.Draw(Art.Background, new Vector2(-_backgroundScroll, 0), Color.White);
        SpriteBatch.Draw(Art.Ground, new Vector2(-_groundScroll, VirtualHeight - Art.Ground.Height), Color.White);

        _bird.Draw(SpriteBatch);

        foreach (PipePair pipePair in _pipePairs)
        {
            pipePair.Draw(SpriteBatch);
        }

        SpriteBatch.End();

        base.Draw(gameTime);
    }

    private void SpawnPipePair()
    {
        float gapY = Math.Clamp(lastGapY + Random.Shared.Next(-50, 50), 30, VirtualHeight - PipePair.PipeGap - 30);
        lastGapY = gapY;
        _pipePairs.Add(new(gapY));
    }
}
