using System;
using System.Collections.Generic;
using System.Linq;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake8;

// The snake lives on a grid: its segments are cells, head first.
public class Snake
{
    // The snake moves one cell per tick, however often the game updates.
    private static readonly TimeSpan TickDuration = TimeSpan.FromMilliseconds(200);
    private const int StartLength = 3;
    private const int MaxBufferedTurns = 2;

    private readonly AnimatedSprite _sprite;
    private readonly int _tileSize;
    private readonly Rectangle _room;
    private readonly List<Point> _segments = [];
    private readonly Queue<Point> _turns = new();
    private Point _direction;
    private TimeSpan _elapsed;
    private int _growth;

    public Snake(AnimatedSprite sprite, int tileSize, Rectangle room)
    {
        _sprite = sprite;
        _tileSize = tileSize;
        _room = room;
        Reset(room.Center);
    }

    public Point Head => _segments[0];

    // The head as a circle, to test against the bat.
    public Circle Bounds => new(
        Head.X * _tileSize + _tileSize / 2,
        Head.Y * _tileSize + _tileSize / 2,
        _tileSize / 2
    );

    public void Reset(Point start)
    {
        _segments.Clear();
        for (int i = 0; i < StartLength; i++)
        {
            _segments.Add(new Point(start.X - i, start.Y));
        }

        _direction = Direction.Right;
        _elapsed = TimeSpan.Zero;
        _turns.Clear();
        _growth = 0;
    }

    // Turns are buffered: two quick key presses between ticks are both used, one per tick,
    // instead of the second one overwriting the first. A turn is checked against the last
    // buffered direction, so the snake can never turn back into its own neck.
    public void Turn(Point direction)
    {
        Point current = _turns.Count > 0 ? _turns.Last() : _direction;

        if (_turns.Count < MaxBufferedTurns && direction != current && direction != Opposite(current))
        {
            _turns.Enqueue(direction);
        }
    }

    // The snake grows by one segment on its next move.
    public void Grow() => _growth++;

    public void Update(GameTime gameTime)
    {
        _sprite.Update(gameTime);

        // Collect the time since the last move, and move once for every full tick.
        _elapsed += gameTime.ElapsedGameTime;
        while (_elapsed >= TickDuration)
        {
            _elapsed -= TickDuration;
            Move();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Point segment in _segments)
        {
            _sprite.Draw(spriteBatch, new Vector2(segment.X * _tileSize, segment.Y * _tileSize));
        }
    }

    private void Move()
    {
        if (_turns.Count > 0)
        {
            _direction = _turns.Dequeue();
        }

        // Until the walls are deadly, the snake wraps around to the other side of the room.
        Point next = Head + _direction;
        next.X = _room.X + Wrap(next.X - _room.X, _room.Width);
        next.Y = _room.Y + Wrap(next.Y - _room.Y, _room.Height);
        _segments.Insert(0, next);

        // Growing means keeping the tail for one move.
        if (_growth > 0)
        {
            _growth--;
        }
        else
        {
            _segments.RemoveAt(_segments.Count - 1);
        }
    }

    private static Point Opposite(Point direction) => new(-direction.X, -direction.Y);

    private static int Wrap(int value, int size) => ((value % size) + size) % size;
}
