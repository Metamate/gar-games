using GMDCore;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Birds2;

// Pull back with the mouse, and let go to shoot. The further you pull, the faster the bird.
public class Slingshot(TextureAtlas atlas, Texture2D pixel)
{
    public static readonly Vector2 Anchor = new(220, 520);
    public const float MaxPull = 90;
    public const float LaunchScale = 10;    // pixels per second of speed, per pixel of pull

    private static readonly Vector2 TopLeft = new(196, 510);
    private static readonly Vector2 BackTip = new(236, 518);
    private static readonly Vector2 FrontTip = new(205, 520);
    private static readonly Color Band = new(60, 30, 15);

    private readonly TextureRegion _back = atlas.GetRegion("slingshot-back");
    private readonly TextureRegion _front = atlas.GetRegion("slingshot-front");
    private readonly TextureRegion _bird = atlas.GetRegion("bird");
    private bool _dragging;
    private Vector2 _pull;

    // Returns true on the frame the player lets go, with where the bird starts and how fast.
    public bool Update(Vector2 mouse, out Vector2 position, out Vector2 velocity)
    {
        position = velocity = Vector2.Zero;
        if (!_dragging && Core.Input.Mouse.WasLeftButtonJustPressed && Vector2.Distance(mouse, Anchor) < 50)
            _dragging = true;

        if (!_dragging)
            return false;

        _pull = mouse - Anchor;
        if (_pull.Length() > MaxPull)
            _pull = Vector2.Normalize(_pull) * MaxPull;

        // MouseInfo has no "just released", but the slingshot knows it was dragging.
        if (Core.Input.Mouse.IsLeftButtonDown)
            return false;

        _dragging = false;
        if (_pull.Length() < 10)
            return false;

        position = Anchor + _pull;
        velocity = -_pull * LaunchScale;
        _pull = Vector2.Zero;
        return true;
    }

    public void DrawBack(SpriteBatch spriteBatch)
    {
        _back.Draw(spriteBatch, TopLeft, Color.White);
    }

    // The bird in the pouch, the bands, and the front of the slingshot on top.
    public void DrawFront(SpriteBatch spriteBatch)
    {
        var bird = _bird;
        Vector2 pouch = _dragging ? Anchor + _pull : Anchor;
        bird.Draw(spriteBatch, pouch, Color.White, 0, new Vector2(bird.Width / 2f, bird.Height / 2f), 1, SpriteEffects.None, 0);
        spriteBatch.DrawLine(pixel, BackTip, pouch, Band, 4);
        spriteBatch.DrawLine(pixel, FrontTip, pouch, Band, 4);
        _front.Draw(spriteBatch, TopLeft, Color.White);
    }
}
