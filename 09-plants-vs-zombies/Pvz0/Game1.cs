using System.Collections.Generic;
using GARCore;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pvz0;

// The field and picking: choose a card, then click a cell to place a defender. The defenders are only
// pictures, for now.
public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;

    private readonly Dictionary<Point, TextureRegion> _defenders = [];
    private Texture2D _background;
    private Texture2D _pixel;
    private TextureAtlas _atlas;
    private CardBar _cardBar;

    public Game1() : base("Plants vs. Zombies", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        _background = Content.Load<Texture2D>("images/background");
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _cardBar = new CardBar(_atlas, _pixel);
        _cardBar.Add("Chest", "chest");
        _cardBar.Add("Archer", "archer");
        _cardBar.Add("Knight", "knight");
    }

    protected override void Update(GameTime gameTime)
    {
        if (Input.Keyboard.WasKeyJustPressed(Keys.R))
            _defenders.Clear();

        Vector2 mouse = MousePosition();
        if (Input.Mouse.WasLeftButtonJustPressed && !_cardBar.TrySelect(mouse))
            Place(mouse);

        base.Update(gameTime);
    }

    private void Place(Vector2 mouse)
    {
        if (_cardBar.Selected == null || Field.CellAt(mouse) is not Point cell || _defenders.ContainsKey(cell))
            return;

        _defenders[cell] = _atlas.GetRegion(_cardBar.Selected.Sprite);
        _cardBar.Deselect();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix, samplerState: SamplerState.PointClamp);
        SpriteBatch.Draw(_background, Vector2.Zero, Color.White);

        foreach (var (cell, sprite) in _defenders)
            DrawStanding(sprite, Field.CellFeet(cell), Color.White);

        DrawCursor(MousePosition());
        _cardBar.Draw(SpriteBatch);
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    // Highlight the cell under the mouse, and show where the chosen defender would go.
    private void DrawCursor(Vector2 mouse)
    {
        if (Field.CellAt(mouse) is not Point cell)
            return;

        SpriteBatch.Draw(_pixel, Field.CellBounds(cell), Color.White * 0.2f);
        if (_cardBar.Selected != null && !_defenders.ContainsKey(cell))
            DrawStanding(_cardBar.Selected.Icon, Field.CellFeet(cell), Color.White * 0.5f);
    }

    // Defenders stand on their feet: the sprite's bottom centre.
    private void DrawStanding(TextureRegion sprite, Vector2 feet, Color color)
    {
        sprite.Draw(SpriteBatch, feet, color, 0, new Vector2(sprite.Width / 2f, sprite.Height), 1, SpriteEffects.None, 0);
    }

    // The mouse is in window coordinates; the game draws at its virtual resolution, scaled and
    // centred in the window.
    private Vector2 MousePosition()
    {
        Viewport viewport = GraphicsDevice.Viewport;
        Vector2 mouse = Input.Mouse.Position.ToVector2() - new Vector2(viewport.X, viewport.Y);
        return Vector2.Transform(mouse, Matrix.Invert(ScreenScaleMatrix));
    }
}
