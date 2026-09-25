using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz1.Entities;

// Walks left along its row, and eats the plants in its way. Health, damage and drawing are
// the same as a plant's, but a zombie isn't a plant, so it has its own copy of them.
public class Zombie(TextureAtlas atlas, int row)
{
    private const float Speed = 16;
    private const float Bite = 1;   // damage per second
    private readonly TextureRegion _sprite = atlas.GetRegion("zombie");
    private float _time;
    private float _hitTime;

    public int Row { get; } = row;
    public Vector2 Position { get; private set; } = new(Lawn.Bounds.Right + 60, Lawn.RowFeet(row));
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

        Plant plant = world.PlantAt(Row, Position.X - 30);
        if (plant != null)
            plant.TakeDamage(Bite * deltaSeconds);
        else
            Position -= new Vector2(Speed * deltaSeconds, 0);
    }

    public void Draw(SpriteBatch spriteBatch) => Standing.Draw(spriteBatch, _sprite, Position, _time, _hitTime > 0);
}
