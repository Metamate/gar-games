using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy9.States;

public class PlayState(Game1 game) : IState
{
    private Bird _bird;
    private List<PipePair> _pipePairs = [];
    private const float PipeSpawnInterval = 2f;
    private float _pipeSpawnTimer;
    private float lastGapY = Game1.VirtualHeight / 2 - PipePair.PipeGap / 2;

    public void Enter()
    {
        game.IsScrolling = true;
        _bird = new Bird(new Vector2(100, 100));
        _pipePairs.Clear();
        _pipeSpawnTimer = 0;
        SpawnPipePair();
    }

    public void Exit()
    {
    }

    public void Update(GameTime gameTime)
    {
        _bird.Update(gameTime);

        if (_bird.Position.Y > Game1.VirtualHeight - Art.Ground.Height - Art.Bird.Height || _bird.Position.Y < 0)
        {
            game.GameState.ChangeState(game.GameState.TitleState);
        }

        _pipeSpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_pipeSpawnTimer > PipeSpawnInterval)
        {
            SpawnPipePair();
            _pipeSpawnTimer -= PipeSpawnInterval;
        }

        foreach (PipePair pipePair in _pipePairs)
        {
            pipePair.Update(gameTime);

            if (_bird.Collides(pipePair.TopPipe) || _bird.Collides(pipePair.BottomPipe))
            {
                game.GameState.ChangeState(game.GameState.TitleState);
                break;
            }
        }

        _pipePairs.RemoveAll(p => p.Remove);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _bird.Draw(spriteBatch);

        foreach (PipePair pipePair in _pipePairs)
        {
            pipePair.Draw(spriteBatch);
        }
    }

    private void SpawnPipePair()
    {
        float gapY = Math.Clamp(lastGapY + Random.Shared.Next(-50, 50), 30, Game1.VirtualHeight - PipePair.PipeGap - 30);
        lastGapY = gapY;
        _pipePairs.Add(new(gapY));
    }
}