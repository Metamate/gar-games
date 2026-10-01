using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake8;

public class Game1 : Core
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 180;
    // The colour of the screen where no dot is lit.
    private static readonly Color ScreenColor = new(20, 24, 20);
    private Tilemap _tilemap;
    private Rectangle _room;
    private Snake _snake;
    private Food _food;
    private TextureRegion[] _digits;
    private int _score;

    public Game1() : base("Snake", 1280, 720, VirtualWidth, VirtualHeight)
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
        _food = new Food(atlas.CreateAnimatedSprite("food-animation"), _room, (int)_tilemap.TileWidth);
        _food.MoveToFreeCell(_snake);

        // The digits are regions of the atlas too: a font that is only data.
        _digits = new TextureRegion[10];
        for (int i = 0; i < _digits.Length; i++)
        {
            _digits[i] = atlas.GetRegion($"digit-{i}");
        }
    }

    protected override void Update(GameTime gameTime)
    {
        HandleInput();
        _snake.Update(gameTime);
        _food.Update(gameTime);

        CollisionChecks();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ScreenColor);

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        _tilemap.Draw(SpriteBatch);
        _snake.Draw(SpriteBatch);
        _food.Draw(SpriteBatch);
        DrawScore();
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
        // If the snake's head reaches the food, it eats it and grows, and new food appears.
        if (_snake.Bounds.Intersects(_food.Bounds))
        {
            _snake.Grow();
            _score++;
            _food.MoveToFreeCell(_snake);
        }
    }

    // The score in four digits under the room, each drawn from its region in the atlas.
    private void DrawScore()
    {
        string text = _score.ToString("D4");
        for (int i = 0; i < text.Length; i++)
        {
            _digits[text[i] - '0'].Draw(SpriteBatch, new Vector2(6 + i * 4, 172), Color.White);
        }
    }
}
