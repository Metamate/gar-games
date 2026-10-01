using System.Collections.Generic;
using GARCore.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon4;
using Pokemon4.GUI;

namespace UiSamples;

// One screen with each widget: a menu whose options do something you can see, a panel on its
// own, a progress bar that tweens to its new value, and a textbox that pages through a message.
// While the textbox is open it has the input to itself, and the menu waits.
public sealed class SamplesScreen
{
    private const float BarMax = 40;

    private readonly Menu _menu;
    private readonly Panel _panel;
    private readonly ProgressBar _bar;
    private Textbox _textbox;
    private int _colour;

    private static readonly Color[] BarColours = { new(48, 176, 72), new(56, 120, 232), new(224, 168, 32) };

    public SamplesScreen()
    {
        var (menuX, menuY) = Layout.GetPosition(Anchor.TopLeft, 136, 104, 16, 34);
        _menu = new Menu(menuX, menuY, 136, 104, new List<Selection.MenuItem>
        {
            new("Show a message", ShowMessage),
            new("Damage", () => MoveBar(-12)),
            new("Heal", () => MoveBar(+12)),
            new("Change colour", ChangeColour),
        }, Locator.Assets.SmallFont, Locator.Assets.CursorTex);

        _panel = new Panel(160, 34, 152, 44);
        _bar = new ProgressBar(168, 118, 144, 8, BarColours[0], BarMax, BarMax);
    }

    public void Update()
    {
        if (_textbox != null)
        {
            _textbox.Update();
            if (_textbox.IsClosed)
                _textbox = null;
            return;                                   // the menu waits while the textbox is open
        }
        _menu.Update();
    }

    private void ShowMessage()
    {
        var (x, y) = Layout.GetPosition(Anchor.BottomCenter, 304, 64, 0, -6);
        _textbox = new Textbox(x, y, 304, 64,
            "A textbox wraps its text to fit, and shows three lines at a time. Press Enter for the " +
            "next page. When the last page is gone, the textbox closes itself, and the menu gets " +
            "the input back. Until then, the menu waits.",
            Locator.Assets.SmallFont);
    }

    private void MoveBar(float change)
    {
        float target = MathHelper.Clamp(_bar.Value + change, 0, BarMax);
        Locator.Tweens.Tween(0.5f).Add(v => _bar.Value = v, _bar.Value, target);
    }

    private void ChangeColour()
    {
        _colour = (_colour + 1) % BarColours.Length;
        _bar.BarColor = BarColours[_colour];
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var small = Locator.Assets.SmallFont;
        Locator.Assets.MediumFont.Draw(spriteBatch, "UI samples", new Vector2(8, 8), Color.White);

        _menu.Draw(spriteBatch);

        _panel.Draw(spriteBatch);
        small.Draw(spriteBatch, "A panel: the box", new Vector2(168, 44), Color.White);
        small.Draw(spriteBatch, "behind a widget", new Vector2(168, 58), Color.White);

        small.Draw(spriteBatch, $"HP {(int)System.MathF.Round(_bar.Value)} / {BarMax}", new Vector2(168, 102), Color.White);
        _bar.Draw(spriteBatch);

        if (_textbox != null)
            _textbox.Draw(spriteBatch);
        else
            small.Draw(spriteBatch, "Enter: choose   Esc: quit", new Vector2(8, 166), new Color(160, 160, 160));
    }
}
