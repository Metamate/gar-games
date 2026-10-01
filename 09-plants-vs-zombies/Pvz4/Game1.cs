using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pvz4.GameStates;
using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pvz4;

// Game1 loads the data and owns the round: the world, the card bar and the level's clock. The
// game states decide whether the round is being played, or is over.
public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;

    private readonly Random _random = new();
    private readonly StateMachine _states = new();
    private Dictionary<string, GoblinType> _goblinTypes;
    private Level _level;
    private Texture2D _background;
    private Texture2D _pixel;
    private TextureAtlas _atlas;
    private SpriteFont _font;
    private SpriteFont _titleFont;
    private SpriteFont _nameFont;
    private float _time;
    private float _skyGoldTimer;
    private int _nextSpawn;

    public Game1() : base("Plants vs. Zombies", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    public World World { get; private set; }
    public CardBar CardBar { get; private set; }
    public TitleState TitleState { get; private set; }
    public PlayState PlayState { get; private set; }
    public EndState EndState { get; private set; }

    public bool AllGoblinsSpawned => _nextSpawn >= _level.Spawns.Count;
    public int GoblinsToCome => _level.Spawns.Count - _nextSpawn + World.GoblinCount;

    public void ChangeState(IState state) => _states.ChangeState(state);

    protected override void LoadContent()
    {
        _background = Content.Load<Texture2D>("images/background");
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _font = Content.Load<SpriteFont>("fonts/hud");
        _titleFont = Content.Load<SpriteFont>("fonts/title");
        _nameFont = Content.Load<SpriteFont>("fonts/name");
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        CardBar = new CardBar(_atlas, _pixel, _font);
        foreach (DefenderType type in GameData.Load<DefenderType>(ReadText("data/defenders.json")))
            CardBar.Add(type);
        _goblinTypes = GameData.Load<GoblinType>(ReadText("data/goblins.json")).ToDictionary(type => type.Name);
        _level = GameData.LoadOne<Level>(ReadText("data/level1.json"));

        TitleState = new TitleState(this);
        PlayState = new PlayState(this);
        EndState = new EndState(this);
        NewGame();
        ChangeState(TitleState);
    }

    public void NewGame()
    {
        World = new World(_atlas) { Gold = _level.StartingGold };
        _time = 0;
        _skyGoldTimer = _level.SkyGoldInterval / 2;
        _nextSpawn = 0;
        CardBar.Deselect();
        ChangeState(PlayState);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Input.Keyboard.WasKeyJustPressed(Keys.R))
            NewGame();
        _states.Update(gameTime);
        base.Update(gameTime);
    }

    // One step of play: the player's click, the level's clock, and the world.
    public void UpdatePlay(float deltaSeconds)
    {
        Vector2 mouse = MousePosition();
        if (Input.Mouse.WasLeftButtonJustPressed && !World.TryCollect(mouse) && !CardBar.TrySelect(mouse, World.Gold))
            Place(mouse);

        _time += deltaSeconds;
        CardBar.Update(deltaSeconds);
        DropSkyGold(deltaSeconds);
        SpawnGoblins();
        World.Update(deltaSeconds);
    }

    private void Place(Vector2 mouse)
    {
        if (CardBar.Selected is not { } card || Field.CellAt(mouse) is not Point cell || !World.IsFree(cell))
            return;

        World.AddDefender(card.Type.Create(World), cell);
        World.Gold -= card.Cost;
        CardBar.StartRecharge(card);
        CardBar.Deselect();
    }

    private void DropSkyGold(float deltaSeconds)
    {
        _skyGoldTimer -= deltaSeconds;
        if (_skyGoldTimer > 0)
            return;

        _skyGoldTimer = _level.SkyGoldInterval;
        float x = Field.Bounds.X + 40 + _random.NextSingle() * (Field.Bounds.Width - 80);
        float y = Field.Bounds.Y + 40 + _random.NextSingle() * (Field.Bounds.Height - 80);
        World.Add(World.Recipes.FallingCoin(World, x, y));
    }

    // The level's spawns, in order, each at its time.
    private void SpawnGoblins()
    {
        while (!AllGoblinsSpawned && _level.Spawns[_nextSpawn].Time <= _time)
        {
            GoblinType type = _goblinTypes[_level.Spawns[_nextSpawn].Goblin];
            World.AddGoblin(type.Create(World), _random.Next(Field.Rows));
            _nextSpawn++;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        _states.Draw(SpriteBatch);
        SpriteBatch.End();
        base.Draw(gameTime);
    }

    public void DrawGame(bool showCursor)
    {
        SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
        World.Draw(SpriteBatch);
        if (showCursor)
            DrawCursor(MousePosition());
        CardBar.Draw(SpriteBatch, World.Gold);
        DrawGold();

        string goblins = $"Goblins left {GoblinsToCome}";
        SpriteBatch.DrawString(_font, goblins, new Vector2(VirtualWidth - _font.MeasureString(goblins).X - 32, 70), new Color(63, 38, 49));
    }

    // Highlight the cell under the mouse, and show where the chosen defender would go.
    private void DrawCursor(Vector2 mouse)
    {
        if (Field.CellAt(mouse) is not Point cell)
            return;

        SpriteBatch.Draw(_pixel, Field.CellBounds(cell), Color.White * 0.2f);
        if (CardBar.Selected is { } card && World.IsFree(cell))
        {
            TextureRegion icon = card.Icon;
            icon.Draw(SpriteBatch, Field.CellFeet(cell), Color.White * 0.5f, 0, new Vector2(icon.Width / 2f, icon.Height), 1, SpriteEffects.None, 0);
        }
    }

    private void DrawGold()
    {
        SpriteBatch.Draw(_pixel, new Rectangle(16, 25, 108, 110), new Color(80, 50, 25));
        TextureRegion coin = _atlas.GetRegion("coin");
        coin.Draw(SpriteBatch, new Vector2(70, 65), Color.White, 0, new Vector2(coin.Width / 2f, coin.Height / 2f), 1, SpriteEffects.None, 0);
        string text = World.Gold.ToString();
        Vector2 size = _font.MeasureString(text);
        SpriteBatch.DrawString(_font, text, new Vector2(70 - size.X / 2, 103), Color.White);
    }

    // The title screen, over the field.
    public void DrawTitle()
    {
        TitleScreen.Draw(SpriteBatch, _nameFont, _titleFont, "Plants vs. Zombies", "Click a card, then a cell", VirtualWidth, VirtualHeight);
    }

    public void DrawMessage(string text)
    {
        Vector2 size = _font.MeasureString(text);
        Vector2 position = new((VirtualWidth - size.X) / 2, 380);
        SpriteBatch.Draw(_pixel, new Rectangle((int)position.X - 20, (int)position.Y - 12, (int)size.X + 40, (int)size.Y + 24), Color.Black * 0.6f);
        SpriteBatch.DrawString(_font, text, position, Color.White);
    }

    // The mouse is in window coordinates; the game draws at its virtual resolution, scaled and
    // centred in the window.
    public Vector2 MousePosition()
    {
        Viewport viewport = GraphicsDevice.Viewport;
        Vector2 mouse = Input.Mouse.Position.ToVector2() - new Vector2(viewport.X, viewport.Y);
        return Vector2.Transform(mouse, Matrix.Invert(ScreenScaleMatrix));
    }

    // Content files are opened through TitleContainer, which works on every platform.
    private string ReadText(string path)
    {
        using Stream stream = TitleContainer.OpenStream(Path.Combine(Content.RootDirectory, path));
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
