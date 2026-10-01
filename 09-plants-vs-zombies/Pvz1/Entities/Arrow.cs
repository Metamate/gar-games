using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz1.Entities;

public class Arrow(TextureAtlas atlas, Vector2 position, int row, float damage)
{
    private const float Speed = 300;
    private readonly TextureRegion _sprite = atlas.GetRegion("arrow");

    public Vector2 Position { get; private set; } = position;
    public bool IsUsed { get; private set; }

    public void Update(float deltaSeconds, World world)
    {
        Position += new Vector2(Speed * deltaSeconds, 0);
        Goblin goblin = world.FirstGoblinAhead(row, Position.X - 20);
        if (goblin != null && goblin.Position.X - Position.X < 20)
        {
            goblin.TakeDamage(damage);
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
