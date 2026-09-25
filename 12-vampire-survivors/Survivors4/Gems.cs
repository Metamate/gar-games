using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Survivors4;

// Experience gems, as arrays: thousands of them lie around by the end, so they're stored like
// the enemies. Gems near the player fly to them.
public sealed class Gems(TextureRegion sprite, int capacity)
{
    private const float Magnet = 110;
    private const float PickUp = 22;
    private const float Speed = 360;

    private readonly Vector2[] _position = new Vector2[capacity];
    private readonly int[] _value = new int[capacity];

    public int Count { get; private set; }

    public void Add(Vector2 position, int value)
    {
        if (Count == _position.Length)
            return;
        _position[Count] = position;
        _value[Count] = value;
        Count++;
    }

    // Returns the experience picked up.
    public int Update(float deltaSeconds, Vector2 player)
    {
        int picked = 0;
        for (int i = Count - 1; i >= 0; i--)
        {
            Vector2 toPlayer = player - _position[i];
            float distance = toPlayer.Length();
            if (distance < PickUp)
            {
                picked += _value[i];
                Count--;
                _position[i] = _position[Count];
                _value[i] = _value[Count];
            }
            else if (distance < Magnet)
            {
                _position[i] += toPlayer / distance * Speed * deltaSeconds;
            }
        }
        return picked;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        for (int i = 0; i < Count; i++)
            sprite.Draw(spriteBatch, _position[i], Color.White, 0, origin, 1, SpriteEffects.None, 0);
    }
}
