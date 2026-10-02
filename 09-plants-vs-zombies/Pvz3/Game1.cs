using System;
using System.Collections.Generic;
using System.IO;
using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pvz3;

public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;
    private const float FirstGoblin = 15;
    private const float GoblinInterval = 6;

    private readonly Random _random = new();
    private List<GoblinType> _goblinTypes;
    private World _world;
    private CardBar _cardBar;
    private Texture2D _background;
    private Texture2D _pixel;
    private TextureAtlas _atlas;
    private SpriteFont _font;
    private float _goblinTimer;
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
        _cardBar = new CardBar(_atlas, _pixel, _font);
        // One card per defender type in defenders.json.
        foreach (DefenderType type in GameData.Load<DefenderType>(ReadText("data/defenders.json")))
            _cardBar.Add(type);
        _goblinTypes = GameData.Load<GoblinType>(ReadText("data/goblins.json"));
    }

    private void NewGame()
    {
        _world = new World(_atlas);
        _goblinTimer = FirstGoblin;
        _lost = false;
    }

    protected override void Update(GameTime gameTime)
    {
        if (Input.Keyboard.WasKeyJustPressed(Keys.R) || (_lost && Input.Keyboard.WasKeyJustPressed(Keys.Enter)))
            NewGame();

        if (!_lost)
        {
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 mouse = MousePosition();
            if (Input.Mouse.WasLeftButtonJustPressed && !_world.TryCollect(mouse) && !_cardBar.TrySelect(mouse, _world.Gold))
                Place(mouse);

            SpawnGoblins(deltaSeconds);
            _world.Update(deltaSeconds);
            _lost = _world.GoblinReachedCastle;
        }

        base.Update(gameTime);
    }

    private void Place(Vector2 mouse)
    {
        if (_cardBar.Selected == null || Field.CellAt(mouse) is not Point cell || !_world.IsFree(cell))
            return;

        _world.AddDefender(_cardBar.Selected.Type.Create(_world), cell);
        _world.Gold -= _cardBar.Selected.Cost;
        _cardBar.Deselect();
    }

    // For now, a goblin every few seconds, in a random row.
    private void SpawnGoblins(float deltaSeconds)
    {
        _goblinTimer -= deltaSeconds;
        if (_goblinTimer > 0)
            return;

        _goblinTimer = GoblinInterval;
        int row = _random.Next(Field.Rows);
        GoblinType type = _goblinTypes[_random.Next(_goblinTypes.Count)];
        _world.AddGoblin(type.Create(_world), row);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
        _world.Draw(SpriteBatch);
        DrawCursor(MousePosition());
        _cardBar.Draw(SpriteBatch, _world.Gold);
        DrawGold();
        if (_lost)
            DrawMessage("The goblins are over the wall! Press Enter to try again.");
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    // Highlight the cell under the mouse, and show where the chosen defender would go.
    private void DrawCursor(Vector2 mouse)
    {
        if (Field.CellAt(mouse) is not Point cell)
            return;

        SpriteBatch.Draw(_pixel, Field.CellBounds(cell), Color.White * 0.2f);
        if (_cardBar.Selected is { } card && _world.IsFree(cell))
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
        string text = _world.Gold.ToString();
        Vector2 size = _font.MeasureString(text);
        SpriteBatch.DrawString(_font, text, new Vector2(70 - size.X / 2, 103), Color.White);
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
