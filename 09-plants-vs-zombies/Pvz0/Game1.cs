using System.Collections.Generic;
using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pvz0;

// The lawn and picking: choose a seed packet, then click a cell to plant. The plants are only
// pictures, for now.
public class Game1 : Core
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;

    private readonly Dictionary<Point, TextureRegion> _plants = [];
    private Texture2D _background;
    private Texture2D _pixel;
    private TextureAtlas _atlas;
    private SeedBar _seedBar;

    public Game1() : base("Plants vs. Zombies", 1280, 720, VirtualWidth, VirtualHeight)
    {
    }

    protected override void LoadContent()
    {
        _background = Content.Load<Texture2D>("images/background");
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _seedBar = new SeedBar(_atlas, _pixel);
        _seedBar.Add("Sunflower", "sunflower");
        _seedBar.Add("Peashooter", "peashooter");
        _seedBar.Add("Wall-nut", "wall-nut");
    }

    protected override void Update(GameTime gameTime)
    {
        if (Input.Keyboard.WasKeyJustPressed(Keys.R))
            _plants.Clear();

        Vector2 mouse = MousePosition();
        if (Input.Mouse.WasLeftButtonJustPressed && !_seedBar.TrySelect(mouse))
            Plant(mouse);

        base.Update(gameTime);
    }

    private void Plant(Vector2 mouse)
    {
        if (_seedBar.Selected == null || Lawn.CellAt(mouse) is not Point cell || _plants.ContainsKey(cell))
            return;

        _plants[cell] = _atlas.GetRegion(_seedBar.Selected.Sprite);
        _seedBar.Deselect();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin(transformMatrix: ScreenScaleMatrix);
        SpriteBatch.Draw(_background, Vector2.Zero, Color.White);

        foreach (var (cell, sprite) in _plants)
            DrawStanding(sprite, Lawn.CellFeet(cell), Color.White);

        DrawCursor(MousePosition());
        _seedBar.Draw(SpriteBatch);
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    // Highlight the cell under the mouse, and show where the chosen plant would go.
    private void DrawCursor(Vector2 mouse)
    {
        if (Lawn.CellAt(mouse) is not Point cell)
            return;

        SpriteBatch.Draw(_pixel, Lawn.CellBounds(cell), Color.White * 0.2f);
        if (_seedBar.Selected != null && !_plants.ContainsKey(cell))
            DrawStanding(_seedBar.Selected.Icon, Lawn.CellFeet(cell), Color.White * 0.5f);
    }

    // Plants stand on their feet: the sprite's bottom centre.
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
