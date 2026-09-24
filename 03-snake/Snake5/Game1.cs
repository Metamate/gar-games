using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake5;

public class Game1 : Core
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 180;
    private Tilemap _tilemap;
    private Rectangle _room;
    private Snake _snake;

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
    }

    protected override void Update(GameTime gameTime)
    {
        HandleInput();
        _snake.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        _tilemap.Draw(SpriteBatch);
        _snake.Draw(SpriteBatch);
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    // For now, the game reads the keys directly.
    private void HandleInput()
    {
        if (Input.Keyboard.WasKeyJustPressed(Keys.W))
        {
            _snake.Turn(Direction.Up);
        }
        else if (Input.Keyboard.WasKeyJustPressed(Keys.S))
        {
            _snake.Turn(Direction.Down);
        }
        else if (Input.Keyboard.WasKeyJustPressed(Keys.A))
        {
            _snake.Turn(Direction.Left);
        }
        else if (Input.Keyboard.WasKeyJustPressed(Keys.D))
        {
            _snake.Turn(Direction.Right);
        }
    }
}
