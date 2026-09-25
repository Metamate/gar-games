using System.Collections.Generic;
using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz0;

// The seed packets at the top: click one to choose what to plant. This is picking too: each
// packet is a rectangle on the screen, and a click is either inside it or not.
public class SeedBar(TextureAtlas atlas, Texture2D pixel)
{
    public class Packet
    {
        public string Name { get; init; }
        public string Sprite { get; init; }
        public TextureRegion Icon { get; init; }
        public Rectangle Bounds { get; init; }
    }

    private readonly TextureRegion _background = atlas.GetRegion("packet");
    private readonly List<Packet> _packets = [];

    public Packet Selected { get; private set; }

    public Packet Add(string name, string sprite)
    {
        var packet = new Packet
        {
            Name = name,
            Sprite = sprite,
            Icon = atlas.GetRegion(sprite),
            Bounds = new Rectangle(140 + _packets.Count * 92, 16, 84, 110),
        };
        _packets.Add(packet);
        return packet;
    }

    // Returns true if the click was on a packet (whether or not it could be selected).
    public bool TrySelect(Vector2 point)
    {
        foreach (Packet packet in _packets)
        {
            if (!packet.Bounds.Contains(point))
                continue;
            Selected = Selected == packet ? null : packet;
            return true;
        }
        return false;
    }

    public void Deselect() => Selected = null;

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Packet packet in _packets)
        {
            bool usable = true;
            Color tint = usable ? Color.White : new Color(140, 140, 140);
            _background.Draw(spriteBatch, packet.Bounds.Location.ToVector2(), tint);

            TextureRegion icon = packet.Icon;
            icon.Draw(spriteBatch, new Vector2(packet.Bounds.Center.X, packet.Bounds.Y + 42), tint, 0,
                new Vector2(icon.Width / 2f, icon.Height / 2f), 0.72f, SpriteEffects.None, 0);

            if (packet == Selected)
                Outline(spriteBatch, packet.Bounds, Color.Yellow);
        }
    }

    private void Outline(SpriteBatch spriteBatch, Rectangle r, Color color)
    {
        spriteBatch.Draw(pixel, new Rectangle(r.X, r.Y, r.Width, 3), color);
        spriteBatch.Draw(pixel, new Rectangle(r.X, r.Bottom - 3, r.Width, 3), color);
        spriteBatch.Draw(pixel, new Rectangle(r.X, r.Y, 3, r.Height), color);
        spriteBatch.Draw(pixel, new Rectangle(r.Right - 3, r.Y, 3, r.Height), color);
    }
}
