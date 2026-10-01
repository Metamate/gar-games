using System.Collections.Generic;
using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz3;

// The cards at the top: click one to choose who to place. This is picking too: each
// card is a rectangle on the screen, and a click is either inside it or not.
public class CardBar(TextureAtlas atlas, Texture2D pixel, SpriteFont font)
{
    public class Card
    {
        public string Name { get; init; }
        public string Sprite { get; init; }
        public TextureRegion Icon { get; init; }
        public Rectangle Bounds { get; init; }
        public int Cost { get; init; }
        public DefenderType Type { get; init; }
    }

    private readonly TextureRegion _background = atlas.GetRegion("card");
    private readonly List<Card> _cards = [];

    public Card Selected { get; private set; }

    public Card Add(DefenderType type)
    {
        var card = new Card
        {
            Name = type.Name,
            Sprite = type.Sprite,
            Icon = atlas.GetRegion(type.Sprite),
            Bounds = new Rectangle(140 + _cards.Count * 90, 25, 80, 110),
            Cost = type.Cost,
            Type = type,
        };
        _cards.Add(card);
        return card;
    }

    // Returns true if the click was on a card (whether or not it could be selected).
    public bool TrySelect(Vector2 point, int gold)
    {
        foreach (Card card in _cards)
        {
            if (!card.Bounds.Contains(point))
                continue;
            // Too expensive: the click hits the card, but selects nothing.
            if (card.Cost > gold)
                return true;
            Selected = Selected == card ? null : card;
            return true;
        }
        return false;
    }

    public void Deselect() => Selected = null;

    public void Draw(SpriteBatch spriteBatch, int gold)
    {
        foreach (Card card in _cards)
        {
            bool usable = card.Cost <= gold;
            Color tint = usable ? Color.White : new Color(140, 140, 140);
            _background.Draw(spriteBatch, card.Bounds.Location.ToVector2(), tint);

            TextureRegion icon = card.Icon;
            icon.Draw(spriteBatch, new Vector2(card.Bounds.Center.X, card.Bounds.Y + 40), tint, 0,
                new Vector2(icon.Width / 2f, icon.Height / 2f), 1, SpriteEffects.None, 0);

            string cost = card.Cost.ToString();
            Vector2 size = font.MeasureString(cost);
            spriteBatch.DrawString(font, cost, new Vector2(card.Bounds.Center.X - size.X / 2, card.Bounds.Bottom - 24), new Color(63, 38, 49));

            if (card == Selected)
                Outline(spriteBatch, card.Bounds, Color.Yellow);
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
