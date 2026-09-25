using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz1.Entities;

// Click it before it disappears.
public class Sun(TextureAtlas atlas, Vector2 position, int value)
{
    private const float Lifetime = 10;
    private readonly TextureRegion _sprite = atlas.GetRegion("sun");
    private float _age;

    public Vector2 Position { get; } = position;
    public int Value { get; } = value;
    public bool IsGone { get; set; }

    public bool Contains(Vector2 point) => Vector2.Distance(point, Position) < 36;

    public void Update(float deltaSeconds)
    {
        _age += deltaSeconds;
        if (_age > Lifetime)
            IsGone = true;
    }

    public void Draw(SpriteBatch spriteBatch)
        => _sprite.Draw(spriteBatch, Position, Color.White, _age, new Vector2(_sprite.Width / 2f, _sprite.Height / 2f), 1, SpriteEffects.None, 0);
}
