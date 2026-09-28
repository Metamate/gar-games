using System.IO;
using Pacman2.Views;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman2;

public class Game1 : Core
{
    public const int VirtualWidth = 560;
    public const int VirtualHeight = 620;
    private const int MazeTop = 40;

    private World _world;
    private Texture2D _pixel;
    private SpriteFont _font;
    private MazeView _mazeView;
    private PacManView _pacManView;
    private GhostView _ghostView;

    public Game1() : base("Pac-Man", VirtualWidth, VirtualHeight, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
        _font = Content.Load<SpriteFont>("fonts/hud");

        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _mazeView = new MazeView(_pixel, atlas);
        _pacManView = new PacManView(atlas);
        _ghostView = new GhostView(atlas);

        _world = new World(ReadText("levels/maze.txt"));
    }

    protected override void Update(GameTime gameTime)
    {
        float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (GameController.Up) _world.PacMan.Steer(Direction.Up);
        else if (GameController.Down) _world.PacMan.Steer(Direction.Down);
        else if (GameController.Left) _world.PacMan.Steer(Direction.Left);
        else if (GameController.Right) _world.PacMan.Steer(Direction.Right);

        _world.Update(deltaSeconds);

        // No lives yet: when a ghost catches Pac-Man, everyone goes back to the start.
        if (_world.PacManCaught)
            _world.ResetPositions();

        if (_world.IsCleared)
            _world.NextLevel();

        _mazeView.Update(deltaSeconds);
        _pacManView.Update(gameTime, _world.PacMan);
        _ghostView.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // The maze starts below the score.
        SpriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(0, MazeTop, 0) * ScreenScaleMatrix);
        _mazeView.Draw(SpriteBatch, _world.Maze);
        _pacManView.Draw(SpriteBatch, _world.PacMan);
        foreach (Ghost ghost in _world.Ghosts)
            _ghostView.Draw(SpriteBatch, ghost);
        SpriteBatch.End();

        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        SpriteBatch.DrawString(_font, $"SCORE {_world.Score}", new Vector2(16, 8), Color.White);
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    // Content files are opened through TitleContainer, which works on every platform.
    private string ReadText(string path)
    {
        using Stream stream = TitleContainer.OpenStream(Path.Combine(Content.RootDirectory, path));
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
