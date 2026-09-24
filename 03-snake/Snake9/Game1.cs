using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake9;

public class Game1 : Core
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 180;
    private Tilemap _tilemap;
    private Rectangle _room;
    private Snake _snake;
    private Rectangle _roomBounds;
    private Bat _bat;

    public Game1() : base("Snake", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        // The room is data too: the tilemap definition lists the tile for every cell.
        _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");

        // The room is the area inside the walls. The walls are one cell thick, except the top
        // wall, which is two cells tall.
        _room = new Rectangle(1, 2, _tilemap.Columns - 2, _tilemap.Rows - 3);
        _snake = new Snake(atlas.CreateAnimatedSprite("snake-animation"), (int)_tilemap.TileWidth, _room);

        int tileSize = (int)_tilemap.TileWidth;
        _roomBounds = new Rectangle(_room.X * tileSize, _room.Y * tileSize, _room.Width * tileSize, _room.Height * tileSize);
        _bat = new Bat(atlas.CreateAnimatedSprite("bat-animation"), _roomBounds);
        RespawnBat();
    }

    protected override void Update(GameTime gameTime)
    {
        HandleInput();
        _snake.Update(gameTime);
        _bat.Update(gameTime);

        CollisionChecks();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        _tilemap.Draw(SpriteBatch);
        _snake.Draw(SpriteBatch);
        _bat.Draw(SpriteBatch);
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    private void HandleInput()
    {
        if (GameController.Up)
        {
            _snake.Turn(Direction.Up);
        }
        else if (GameController.Down)
        {
            _snake.Turn(Direction.Down);
        }
        else if (GameController.Left)
        {
            _snake.Turn(Direction.Left);
        }
        else if (GameController.Right)
        {
            _snake.Turn(Direction.Right);
        }
    }

    private void CollisionChecks()
    {
        // Hitting a wall or its own body ends the game, and a new one starts.
        if (!_room.Contains(_snake.Head) || _snake.IsBitingItself)
        {
            _snake.Reset(_room.Center);
            RespawnBat();
            return;
        }

        // If the snake catches the bat, it eats the bat and grows, and a new bat appears.
        if (_snake.Bounds.Intersects(_bat.Bounds))
        {
            _snake.Grow();
            RespawnBat();
        }

        // If the bat leaves the room, it has hit a wall and bounces off it.
        // The normal points away from the wall, back into the room.
        if (_bat.Bounds.Top < _roomBounds.Top)
        {
            _bat.Bounce(Vector2.UnitY);
        }
        else if (_bat.Bounds.Bottom > _roomBounds.Bottom)
        {
            _bat.Bounce(-Vector2.UnitY);
        }

        if (_bat.Bounds.Left < _roomBounds.Left)
        {
            _bat.Bounce(Vector2.UnitX);
        }
        else if (_bat.Bounds.Right > _roomBounds.Right)
        {
            _bat.Bounce(-Vector2.UnitX);
        }
    }

    private void RespawnBat()
    {
        _bat.RandomizePosition();
        _bat.RandomizeVelocity();
    }
}
