using Birds2.Physics;

namespace Birds2;

// The materials of the game. Stone is heavy, glass is light and a little bouncy.
public static class Materials
{
    public static readonly PhysicsMaterial Ground = new(0, 0.8f, 0);
    public static readonly PhysicsMaterial Wood = new(1, 0.6f, 0.1f);
    public static readonly PhysicsMaterial Stone = new(3, 0.7f, 0.05f);
    public static readonly PhysicsMaterial Glass = new(0.8f, 0.4f, 0.2f);
    public static readonly PhysicsMaterial Pig = new(1, 0.6f, 0.2f);
    public static readonly PhysicsMaterial Bird = new(4, 0.8f, 0.3f);
}
