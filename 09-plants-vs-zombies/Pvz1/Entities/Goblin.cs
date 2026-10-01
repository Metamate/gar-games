using GARCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz1.Entities;

// Walks left along its row, and attacks the defenders in its way. Health, damage and drawing are
// the same as a defender's, but a goblin isn't a defender, so it has its own copy of them.
public class Goblin(TextureAtlas atlas, int row)
{
    private const float Speed = 28;
    private const float Attack = 1;   // damage per second
    private readonly TextureRegion _sprite = atlas.GetRegion("goblin");
    private float _time;
    private float _hitTime;

    public int Row { get; } = row;
    public Vector2 Position { get; private set; } = new(Field.Bounds.Right + 120, Field.RowFeet(row));
    public float Health { get; private set; } = 10;
    public bool IsDead => Health <= 0;

    public void TakeDamage(float damage)
    {
        Health -= damage;
        _hitTime = 0.1f;
    }

    public void Update(float deltaSeconds, World world)
    {
        _time += deltaSeconds;
        _hitTime -= deltaSeconds;

        Defender defender = world.DefenderAt(Row, Position.X - 40);
        if (defender != null)
            defender.TakeDamage(Attack * deltaSeconds);
        else
            Position -= new Vector2(Speed * deltaSeconds, 0);
    }

    public void Draw(SpriteBatch spriteBatch) => Standing.Draw(spriteBatch, _sprite, Position, _time, _hitTime > 0);
}
