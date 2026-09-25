using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pvz4.GameStates;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pvz4;

// Game1 loads the data and owns the round: the world, the seed bar and the level's clock. The
// game states decide whether the round is being played, or is over.
public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;

    private readonly Random _random = new();
    private readonly StateMachine _states = new();
    private Dictionary<string, ZombieType> _zombieTypes;
    private Level _level;
    private Texture2D _background;
    private Texture2D _pixel;
    private TextureAtlas _atlas;
    private SpriteFont _font;
    private float _time;
    private float _skySunTimer;
    private int _nextSpawn;

    public Game1() : base("Plants vs. Zombies", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    public World World { get; private set; }
    public SeedBar SeedBar { get; private set; }
    public PlayState PlayState { get; private set; }
    public EndState EndState { get; private set; }

    public bool AllZombiesSpawned => _nextSpawn >= _level.Spawns.Count;
    public int ZombiesToCome => _level.Spawns.Count - _nextSpawn + World.ZombieCount;

    public void ChangeState(IState state) => _states.ChangeState(state);

    protected override void LoadContent()
    {
        _background = Content.Load<Texture2D>("images/background");
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _font = Content.Load<SpriteFont>("fonts/hud");
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        SeedBar = new SeedBar(_atlas, _pixel, _font);
        foreach (PlantType type in GameData.Load<PlantType>(ReadText("data/plants.json")))
            SeedBar.Add(type);
        _zombieTypes = GameData.Load<ZombieType>(ReadText("data/zombies.json")).ToDictionary(type => type.Name);
        _level = GameData.LoadOne<Level>(ReadText("data/level1.json"));

        PlayState = new PlayState(this);
        EndState = new EndState(this);
        NewGame();
    }

    public void NewGame()
    {
        World = new World(_atlas) { Sun = _level.StartingSun };
        _time = 0;
        _skySunTimer = _level.SkySunInterval / 2;
        _nextSpawn = 0;
        SeedBar.Deselect();
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
        if (Input.Mouse.WasLeftButtonJustPressed && !World.TryCollect(mouse) && !SeedBar.TrySelect(mouse, World.Sun))
            Plant(mouse);

        _time += deltaSeconds;
        SeedBar.Update(deltaSeconds);
        DropSkySun(deltaSeconds);
        SpawnZombies();
        World.Update(deltaSeconds);
    }

    private void Plant(Vector2 mouse)
    {
        if (SeedBar.Selected is not { } packet || Lawn.CellAt(mouse) is not Point cell || !World.IsFree(cell))
            return;

        World.AddPlant(packet.Type.Create(World), cell);
        World.Sun -= packet.Cost;
        SeedBar.StartRecharge(packet);
        SeedBar.Deselect();
    }

    private void DropSkySun(float deltaSeconds)
    {
        _skySunTimer -= deltaSeconds;
        if (_skySunTimer > 0)
            return;

        _skySunTimer = _level.SkySunInterval;
        float x = Lawn.Bounds.X + 40 + _random.NextSingle() * (Lawn.Bounds.Width - 80);
        float y = Lawn.Bounds.Y + 40 + _random.NextSingle() * (Lawn.Bounds.Height - 80);
        World.Add(World.Recipes.FallingSun(World, x, y));
    }

    // The level's spawns, in order, each at its time.
    private void SpawnZombies()
    {
        while (!AllZombiesSpawned && _level.Spawns[_nextSpawn].Time <= _time)
        {
            ZombieType type = _zombieTypes[_level.Spawns[_nextSpawn].Zombie];
            World.AddZombie(type.Create(World), _random.Next(Lawn.Rows));
            _nextSpawn++;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
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
        SeedBar.Draw(SpriteBatch, World.Sun);
        DrawSun();

        string zombies = $"Zombies left {ZombiesToCome}";
        SpriteBatch.DrawString(_font, zombies, new Vector2(VirtualWidth - _font.MeasureString(zombies).X - 24, 52), Color.White);
    }

    // Highlight the cell under the mouse, and show where the chosen plant would go.
    private void DrawCursor(Vector2 mouse)
    {
        if (Lawn.CellAt(mouse) is not Point cell)
            return;

        SpriteBatch.Draw(_pixel, Lawn.CellBounds(cell), Color.White * 0.2f);
        if (SeedBar.Selected is { } packet && World.IsFree(cell))
        {
            TextureRegion icon = packet.Icon;
            icon.Draw(SpriteBatch, Lawn.CellFeet(cell), Color.White * 0.5f, 0, new Vector2(icon.Width / 2f, icon.Height), 1, SpriteEffects.None, 0);
        }
    }

    private void DrawSun()
    {
        SpriteBatch.Draw(_pixel, new Rectangle(16, 16, 108, 110), new Color(80, 50, 25));
        TextureRegion sun = _atlas.GetRegion("sun");
        sun.Draw(SpriteBatch, new Vector2(70, 52), Color.White, 0, new Vector2(sun.Width / 2f, sun.Height / 2f), 1, SpriteEffects.None, 0);
        string text = World.Sun.ToString();
        Vector2 size = _font.MeasureString(text);
        SpriteBatch.DrawString(_font, text, new Vector2(70 - size.X / 2, 88), Color.White);
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
