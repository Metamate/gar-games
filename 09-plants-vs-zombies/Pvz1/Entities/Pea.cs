using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz1.Entities;

public class Pea(TextureAtlas atlas, Vector2 position, int row, float damage)
{
    private const float Speed = 300;
    private readonly TextureRegion _sprite = atlas.GetRegion("pea");

    public Vector2 Position { get; private set; } = position;
    public bool IsUsed { get; private set; }

    public void Update(float deltaSeconds, World world)
    {
        Position += new Vector2(Speed * deltaSeconds, 0);
        Zombie zombie = world.FirstZombieAhead(row, Position.X - 20);
        if (zombie != null && zombie.Position.X - Position.X < 20)
        {
            zombie.TakeDamage(damage);
            IsUsed = true;
        }
        else if (Position.X > 1300)
        {
            IsUsed = true;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
        => _sprite.Draw(spriteBatch, Position, Color.White, 0, new Vector2(_sprite.Width / 2f, _sprite.Height / 2f), 1, SpriteEffects.None, 0);
}
