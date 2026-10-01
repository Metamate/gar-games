using System;
using System.Collections.Generic;
using System.Linq;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake7;

// The snake lives on a grid: its segments are cells, head first.
public class Snake
{
    // The snake moves one cell per tick, however often the game updates.
    private static readonly TimeSpan TickDuration = TimeSpan.FromMilliseconds(100);
    private const int StartLength = 3;
    private const int MaxBufferedTurns = 2;

    private readonly Sprite _body;
    private readonly AnimatedSprite _head;
    private readonly int _tileSize;
    private readonly Rectangle _room;
    private readonly List<Point> _segments = [];
    private readonly Queue<Point> _turns = new();
    private Point _direction;
    private TimeSpan _elapsed;

    public Snake(Sprite body, AnimatedSprite head, int tileSize, Rectangle room)
    {
        _body = body;
        _head = head;
        // The head turns around its centre to face where the snake goes.
        _head.CenterOrigin();
        _tileSize = tileSize;
        _room = room;
        Reset(room.Center);
    }

    public Point Head => _segments[0];

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

    public void Update(GameTime gameTime)
    {
        _head.Update(gameTime);

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
        for (int i = 1; i < _segments.Count; i++)
        {
            _body.Draw(spriteBatch, CellPosition(_segments[i]));
        }

        // The head is drawn around its origin, so it is placed at the cell's corner plus the origin.
        _head.Rotation = MathF.Atan2(_direction.Y, _direction.X);
        _head.Draw(spriteBatch, CellPosition(Head) + _head.Origin);
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

        _segments.RemoveAt(_segments.Count - 1);
    }

    private Vector2 CellPosition(Point cell) => new(cell.X * _tileSize, cell.Y * _tileSize);

    private static Point Opposite(Point direction) => new(-direction.X, -direction.Y);

    private static int Wrap(int value, int size) => ((value % size) + size) % size;
}
