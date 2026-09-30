using System;
using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake8;

// An apple on one cell of the room. When the snake eats it, it moves to another free cell.
public class Apple(AnimatedSprite sprite, Rectangle room, int tileSize)
{
    private readonly AnimatedSprite _sprite = sprite;
    private readonly Rectangle _room = room;
    private readonly int _tileSize = tileSize;

    public Point Cell { get; private set; }

    // The apple as a circle, to test against the snake's head.
    public Circle Bounds => new(
        Cell.X * _tileSize + _tileSize / 2,
        Cell.Y * _tileSize + _tileSize / 2,
        _tileSize / 2
    );

    public void Update(GameTime gameTime) => _sprite.Update(gameTime);

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, new Vector2(Cell.X * _tileSize, Cell.Y * _tileSize));
    }

    // Put the apple on a random cell of the room that the snake isn't on.
    public void MoveToFreeCell(Snake snake)
    {
        do
        {
            Cell = new Point(Random.Shared.Next(_room.Left, _room.Right), Random.Shared.Next(_room.Top, _room.Bottom));
        }
        while (snake.Contains(Cell));
    }
}
