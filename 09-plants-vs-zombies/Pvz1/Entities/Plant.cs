using GMDCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz1.Entities;

// The base class of every plant: a cell on the lawn, health, and a sprite. What a plant does
// is up to its subclass.
public abstract class Plant(Point cell, TextureRegion sprite, float health)
{
    private float _time;
    private float _hitTime;

    public Point Cell { get; } = cell;
    public int Row => Cell.Y;
    public Vector2 Position => Lawn.CellFeet(Cell);
    public float Health { get; private set; } = health;
    public bool IsDead => Health <= 0;

    public void TakeDamage(float damage)
    {
        Health -= damage;
        _hitTime = 0.1f;
    }

    public virtual void Update(float deltaSeconds, World world)
    {
        _time += deltaSeconds;
        _hitTime -= deltaSeconds;
    }

    public void Draw(SpriteBatch spriteBatch) => Standing.Draw(spriteBatch, sprite, Position, _time, _hitTime > 0);
}
