using System;
using System.Collections.Generic;
using System.Linq;
using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake9;

// The snake lives on a grid: its segments are cells, head first.
public class Snake
{
    // The snake moves one cell per tick, however often the game updates.
    private static readonly TimeSpan TickDuration = TimeSpan.FromMilliseconds(200);
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
    private int _growth;

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

    // The head as a circle, to test against the food.
    public Circle Bounds => new(
        Head.X * _tileSize + _tileSize / 2,
        Head.Y * _tileSize + _tileSize / 2,
        _tileSize / 2
    );

    // The snake bites itself when its head is on the same cell as one of its other segments.
    public bool IsBitingItself => _segments.IndexOf(Head, 1) >= 0;

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

    // Whether one of the snake's segments is on this cell.
    public bool Contains(Point cell) => _segments.Contains(cell);

    // The snake grows by one segment on its next move.
    public void Grow() => _growth++;

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

        _segments.Insert(0, Head + _direction);

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

    private Vector2 CellPosition(Point cell) => new(cell.X * _tileSize, cell.Y * _tileSize);

    private static Point Opposite(Point direction) => new(-direction.X, -direction.Y);
}
