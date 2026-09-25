using Microsoft.Xna.Framework;

namespace Survivors3;

// Data-oriented: all the enemies as a struct of arrays. There's no Enemy object: an enemy is
// an index, and each of its fields is in its own array, packed from 0 to Count - 1. What the
// hot loops need (speed, radius) is copied from the enemy's kind into arrays of its own, so a
// loop reads arrays from start to end instead of following references.
public sealed class Enemies(int capacity)
{
    public readonly Vector2[] Position = new Vector2[capacity];
    public readonly float[] Health = new float[capacity];
    public readonly float[] Speed = new float[capacity];
    public readonly float[] Radius = new float[capacity];
    public readonly byte[] Kind = new byte[capacity];

    public int Count { get; private set; }
    public bool IsFull => Count == Position.Length;

    public void Add(byte kind, Vector2 position)
    {
        if (IsFull)
            return;
        EnemyKind type = EnemyKind.All[kind];
        Position[Count] = position;
        Health[Count] = type.Health;
        Speed[Count] = type.Speed;
        Radius[Count] = type.Radius;
        Kind[Count] = kind;
        Count++;
    }

    // The last enemy moves into the hole: nothing shifts, and the arrays stay packed.
    public void RemoveAt(int index)
    {
        int last = Count - 1;
        Position[index] = Position[last];
        Health[index] = Health[last];
        Speed[index] = Speed[last];
        Radius[index] = Radius[last];
        Kind[index] = Kind[last];
        Count--;
    }
}
