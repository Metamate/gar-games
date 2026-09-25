using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pvz3.Components;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pvz3;

public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;
    private const float FirstZombie = 15;
    private const float ZombieInterval = 10;

    private readonly Random _random = new();
    private List<ZombieType> _zombieTypes;
    private World _world;
    private SeedBar _seedBar;
    private Texture2D _background;
    private Texture2D _pixel;
    private TextureAtlas _atlas;
    private SpriteFont _font;
    private float _zombieTimer;
    private bool _lost;

    public Game1() : base("Plants vs. Zombies", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        _background = Content.Load<Texture2D>("images/background");
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _font = Content.Load<SpriteFont>("fonts/hud");
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        NewGame();
        _seedBar = new SeedBar(_atlas, _pixel, _font);
        // One packet per plant type in plants.json.
        foreach (PlantType type in GameData.Load<PlantType>(ReadText("data/plants.json")))
            _seedBar.Add(type);
        _zombieTypes = GameData.Load<ZombieType>(ReadText("data/zombies.json"));
    }

    private void NewGame()
    {
        _world = new World(_atlas);
        _zombieTimer = FirstZombie;
        _lost = false;
    }

    protected override void Update(GameTime gameTime)
    {
        if (Input.Keyboard.WasKeyJustPressed(Keys.R))
            NewGame();

        if (!_lost)
        {
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 mouse = MousePosition();
            if (Input.Mouse.WasLeftButtonJustPressed && !_world.TryCollect(mouse) && !_seedBar.TrySelect(mouse, _world.Sun))
                Plant(mouse);

            SpawnZombies(deltaSeconds);
            _world.Update(deltaSeconds);
            _lost = _world.ZombieReachedHouse;
        }

        base.Update(gameTime);
    }

    private void Plant(Vector2 mouse)
    {
        if (_seedBar.Selected == null || Lawn.CellAt(mouse) is not Point cell || !_world.IsFree(cell))
            return;

        _world.AddPlant(_seedBar.Selected.Type.Create(_world), cell);
        _world.Sun -= _seedBar.Selected.Cost;
        _seedBar.Deselect();
    }

    // For now, a zombie every few seconds, in a random row.
    private void SpawnZombies(float deltaSeconds)
    {
        _zombieTimer -= deltaSeconds;
        if (_zombieTimer > 0)
            return;

        _zombieTimer = ZombieInterval;
        int row = _random.Next(Lawn.Rows);
        ZombieType type = _zombieTypes[_random.Next(_zombieTypes.Count)];
        _world.AddZombie(type.Create(_world), row);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
        _world.Draw(SpriteBatch);
        DrawCursor(MousePosition());
        _seedBar.Draw(SpriteBatch, _world.Sun);
        DrawSun();
        if (_lost)
            DrawMessage("The zombies ate your brains!  Press R to try again");
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    // Highlight the cell under the mouse, and show where the chosen plant would go.
    private void DrawCursor(Vector2 mouse)
    {
        if (Lawn.CellAt(mouse) is not Point cell)
            return;

        SpriteBatch.Draw(_pixel, Lawn.CellBounds(cell), Color.White * 0.2f);
        if (_seedBar.Selected is { } packet && _world.IsFree(cell))
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
        string text = _world.Sun.ToString();
        Vector2 size = _font.MeasureString(text);
        SpriteBatch.DrawString(_font, text, new Vector2(70 - size.X / 2, 88), Color.White);
    }

    private void DrawMessage(string text)
    {
        Vector2 size = _font.MeasureString(text);
        Vector2 position = new((VirtualWidth - size.X) / 2, 380);
        SpriteBatch.Draw(_pixel, new Rectangle((int)position.X - 20, (int)position.Y - 12, (int)size.X + 40, (int)size.Y + 24), Color.Black * 0.6f);
        SpriteBatch.DrawString(_font, text, position, Color.White);
    }

    // The mouse is in window coordinates; the game draws at its virtual resolution, scaled and
    // centred in the window.
    private Vector2 MousePosition()
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
