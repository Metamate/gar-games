using Microsoft.Xna.Framework;

namespace Survivors1;

// One enemy, as an object.
public sealed class Enemy(EnemyKind kind, Vector2 position)
{
    public EnemyKind Kind { get; } = kind;
    public Vector2 Position { get; set; } = position;
    public float Health { get; set; } = kind.Health;
    public bool IsDead => Health <= 0;
}
