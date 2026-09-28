using System.IO;
using Pacman4.GameStates;
using Pacman4.Views;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman4;

// Game1 loads everything and hands the game over to its states (GameStates): ready, play,
// dying and game over. The drawing that all states share lives here.
public class Game1 : Core
{
    public const int VirtualWidth = 560;
    public const int VirtualHeight = 620;
    private const int MazeTop = 40;

    private readonly StateMachine _states = new();
    private Texture2D _pixel;
    private SpriteFont _font;
    private TextureAtlas _atlas;

    public Game1() : base("Pac-Man", VirtualWidth, VirtualHeight, VirtualWidth, VirtualHeight)
    {
    }

    public World World { get; private set; }
    public MazeView MazeView { get; private set; }
    public PacManView PacManView { get; private set; }
    public GhostView GhostView { get; private set; }

    public ReadyState ReadyState { get; private set; }
    public PlayState PlayState { get; private set; }
    public DyingState DyingState { get; private set; }
    public GameOverState GameOverState { get; private set; }

    public void ChangeState(IState state) => _states.ChangeState(state);

    protected override void LoadContent()
    {
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
        _font = Content.Load<SpriteFont>("fonts/hud");

        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        MazeView = new MazeView(_pixel, _atlas);
        PacManView = new PacManView(_atlas);
        GhostView = new GhostView(_atlas);

        World = new World(ReadText("levels/maze.txt"));

        ReadyState = new ReadyState(this);
        PlayState = new PlayState(this);
        DyingState = new DyingState(this);
        GameOverState = new GameOverState(this);
        ChangeState(ReadyState);
    }

    protected override void Update(GameTime gameTime)
    {
        _states.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _states.Draw(SpriteBatch);
        base.Draw(gameTime);
    }

    // The maze, Pac-Man and the ghosts. States decide what to leave out.
    public void DrawWorld(bool drawPacMan = true, bool drawGhosts = true)
    {
        BeginMaze();
        MazeView.Draw(SpriteBatch, World.Maze);
        if (drawPacMan)
            PacManView.Draw(SpriteBatch, World.PacMan);
        if (drawGhosts)
        {
            foreach (Ghost ghost in World.Ghosts)
                GhostView.Draw(SpriteBatch, ghost);
        }
        SpriteBatch.End();
    }

    public void BeginMaze() => SpriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(0, MazeTop, 0) * ScreenScaleMatrix);

    // The score at the top, and the lives left at the bottom.
    public void DrawHud()
    {
        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        SpriteBatch.DrawString(_font, $"SCORE {World.Score}", new Vector2(16, 8), Color.White);

        TextureRegion life = _atlas.GetRegion("pacman-1");
        for (int i = 0; i < World.Lives - 1; i++)
            life.Draw(SpriteBatch, new Vector2(16 + i * 36, VirtualHeight - 36), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
        SpriteBatch.End();
    }

    // A message in the corridor below the ghost house, like "READY!".
    public void DrawMessage(string text, Color color)
    {
        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        Vector2 size = _font.MeasureString(text);
        float y = MazeTop + 16 * Maze.TileSize + (Maze.TileSize - size.Y) / 2;
        SpriteBatch.DrawString(_font, text, new Vector2((VirtualWidth - size.X) / 2, y), color);
        SpriteBatch.End();
    }

    // Content files are opened through TitleContainer, which works on every platform.
    private string ReadText(string path)
    {
        using Stream stream = TitleContainer.OpenStream(Path.Combine(Content.RootDirectory, path));
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
