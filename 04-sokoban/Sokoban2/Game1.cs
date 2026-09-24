using System.Collections.Generic;
using System.IO;
using Sokoban2.Commands;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban2;

public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;

    private LevelView _view;
    private Level _level;
    private readonly List<ICommand> _moves = [];

    public Game1() : base("Sokoban", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        Texture2D texture = Content.Load<Texture2D>("images/tiles");
        _view = new LevelView(new Tileset(new TextureRegion(texture, 0, 0, texture.Width, texture.Height), LevelView.TileSize, LevelView.TileSize));
        _level = Level.Parse(ReadText("levels/level2.txt"));
        UpdateTitle();
    }

    protected override void Update(GameTime gameTime)
    {
        HandleInput();

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

        base.Draw(gameTime);
    }

    private void HandleInput()
    {
        if (GameController.Up) Move(Direction.Up);
        else if (GameController.Down) Move(Direction.Down);
        else if (GameController.Left) Move(Direction.Left);
        else if (GameController.Right) Move(Direction.Right);
    }

    // Only moves that change something become commands, so there is never a command that did nothing.
    private void Move(Point direction)
    {
        _view.Facing = direction;
        if (!_level.CanMove(direction))
        {
            return;
        }

        // Each move is an object. Here we only keep a list of them, to count the moves.
        ICommand command = new MoveCommand(_level, direction);
        command.Execute();
        _moves.Add(command);
        UpdateTitle();
    }

    // No font yet, so the move count goes in the window title.
    private void UpdateTitle() => Window.Title = $"Sokoban: {_moves.Count} moves";

    // Content files are opened through TitleContainer, which works on every platform.
    private string ReadText(string path)
    {
        using Stream stream = TitleContainer.OpenStream(Path.Combine(Content.RootDirectory, path));
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
