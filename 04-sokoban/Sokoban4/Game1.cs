using System.IO;
using Sokoban4.Commands;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban4;

public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;
    private const int LevelCount = 7;

    private LevelView _view;
    private Level _level;
    private readonly CommandHistory _history = new();
    private SpriteFont _font;
    private int _levelIndex;

    public Game1() : base("Sokoban", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        Texture2D texture = Content.Load<Texture2D>("images/tiles");
        _view = new LevelView(new Tileset(new TextureRegion(texture, 0, 0, texture.Width, texture.Height), LevelView.TileSize, LevelView.TileSize));
        _font = Content.Load<SpriteFont>("fonts/arial");
        LoadLevel(0);
    }

    protected override void Update(GameTime gameTime)
    {
        if (_level.IsSolved)
        {
            if (GameController.Continue)
            {
                LoadLevel((_levelIndex + 1) % LevelCount);
            }
        }
        else
        {
            HandleInput();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(60, 64, 72));

        // Centre the level on the screen.
        Matrix centre = Matrix.CreateTranslation(
            (VirtualWidth - _level.Width * LevelView.TileSize) / 2,
            (VirtualHeight - _level.Height * LevelView.TileSize) / 2, 0);
        SpriteBatch.Begin(transformMatrix: centre * ScreenScaleMatrix);
        _view.Draw(SpriteBatch, _level);
        SpriteBatch.End();

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        DrawHud();
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    private void HandleInput()
    {
        if (GameController.Up) Move(Direction.Up);
        else if (GameController.Down) Move(Direction.Down);
        else if (GameController.Left) Move(Direction.Left);
        else if (GameController.Right) Move(Direction.Right);
        else if (GameController.Undo) _history.Undo();
        else if (GameController.Redo) _history.Redo();
        else if (GameController.Restart) LoadLevel(_levelIndex);
    }

    // Only moves that change something become commands, so undo never has to undo nothing.
    private void Move(Point direction)
    {
        _view.Facing = direction;
        if (!_level.CanMove(direction))
        {
            return;
        }

        _history.Execute(new MoveCommand(_level, direction));
    }

    private void LoadLevel(int index)
    {
        _levelIndex = index;
        _level = Level.Parse(ReadText($"levels/level{index + 1}.txt"));
        _view.Facing = Direction.Down;
        _history.Clear();
    }

    private void DrawHud()
    {
        SpriteBatch.DrawString(_font, $"Level {_levelIndex + 1} / {LevelCount}", new Vector2(24, 16), Color.White);
        DrawRightAligned($"Moves: {_history.Count}", 16);

        string help = "Arrows / WASD: move    Z: undo    Y: redo    R: restart";
        Vector2 size = _font.MeasureString(help);
        SpriteBatch.DrawString(_font, help, new Vector2((VirtualWidth - size.X) / 2, VirtualHeight - size.Y - 12), Color.Gray);

        if (_level.IsSolved)
        {
            string message = _levelIndex == LevelCount - 1
                ? "You solved every level! Press Enter to start again."
                : "Level complete! Press Enter for the next level.";
            Vector2 messageSize = _font.MeasureString(message);
            SpriteBatch.DrawString(_font, message, new Vector2((VirtualWidth - messageSize.X) / 2, 16), Color.Gold);
        }
    }

    private void DrawRightAligned(string text, float y)
    {
        Vector2 size = _font.MeasureString(text);
        SpriteBatch.DrawString(_font, text, new Vector2(VirtualWidth - size.X - 24, y), Color.White);
    }

    // Content files are opened through TitleContainer, which works on every platform.
    private string ReadText(string path)
    {
        using Stream stream = TitleContainer.OpenStream(Path.Combine(Content.RootDirectory, path));
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
