using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake5;

public class Game1 : Core
{
    public const int VirtualWidth = 160;
    public const int VirtualHeight = 88;
    // The colour of the screen where no dot is lit.
    private static readonly Color ScreenColor = new(20, 24, 20);
    private Tilemap _tilemap;
    private Rectangle _room;
    private Snake _snake;

    public Game1() : base("Snake", 1280, 704, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        // The room is data too: the tilemap definition lists the tile for every cell.
        _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");

        // The room is the area inside the walls, which are one cell thick.
        _room = new Rectangle(1, 1, _tilemap.Columns - 2, _tilemap.Rows - 2);
        _snake = new Snake(atlas.CreateSprite("body"), atlas.CreateAnimatedSprite("head-animation"), (int)_tilemap.TileWidth, _room);
    }

    protected override void Update(GameTime gameTime)
    {
        HandleInput();
        _snake.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ScreenColor);

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
