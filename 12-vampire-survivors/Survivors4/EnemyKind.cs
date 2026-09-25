namespace Survivors4;

// The kinds of enemy, and what every enemy of a kind shares (the Type Object pattern, from
// Plants vs. Zombies). An enemy only needs to know which kind it is.
public sealed record EnemyKind(string Sprite, float Speed, float Radius, float Health, float Damage, int Experience)
{
    public static readonly EnemyKind Bat = new("bat", 95, 12, 1, 5, 1);
    public static readonly EnemyKind Skeleton = new("skeleton", 60, 14, 3, 8, 2);
    public static readonly EnemyKind Ogre = new("ogre", 40, 24, 14, 15, 5);

    public static readonly EnemyKind[] All = [Bat, Skeleton, Ogre];
    public const float MaxRadius = 24;
}
